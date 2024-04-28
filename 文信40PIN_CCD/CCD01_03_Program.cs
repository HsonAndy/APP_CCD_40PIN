using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyUI;
using Basic;
using AxAxAltairUDrv;
using AxAxOvkImage;

namespace 文信40PIN_CCD
{
    public partial class Form1 : Form
    {


        DialogResult err_message01_03;

        void Program_CCD01_03()
        {
            CCD01_03_儲存圖片();
            this.sub_Program_CCD01_03_SNAP();
            this.sub_Program_CCD01_03_Main_取像並檢驗();
            this.sub_Program_CCD01_03_Tech_檢驗一次();
            this.sub_Program_CCD01_03_計算一次();
            this.sub_Program_CCD01_03_基準線量測();
            this.sub_Program_CCD01_03基準圓量測框調整();
            this.sub_Program_CCD01_03_Tech_取像並檢驗();
            this.sub_Program_CCD01_03圓柱相似度量測();
            this.sub_Program_CCD01_03比對樣板範圍();
            this.sub_Program_CCD01_03_讀取樣板();
            this.sub_Program_CCD01_03圓直徑量測();
            this.sub_Program_CCD01_03_Main_檢驗一次();
            if (PLC_Device_CCD01_03圓柱相似度量測_樣板相似度門檻_MinScore.Value / 100 < 0) PLC_Device_CCD01_03圓柱相似度量測_樣板相似度門檻_MinScore.Value = 0;
            if (PLC_Device_CCD01_03圓柱相似度量測_樣板相似度門檻_MinScore.Value >= 100) PLC_Device_CCD01_03圓柱相似度量測_樣板相似度門檻_MinScore.Value = 100;
        }

        #region PLC_CCD01_03_SNAP
        PLC_Device PLC_Device_CCD01_03_SNAP_按鈕 = new PLC_Device("M15090");
        PLC_Device PLC_Device_CCD01_03_SNAP = new PLC_Device("M15085");
        PLC_Device PLC_Device_CCD01_03_SNAP_LIVE = new PLC_Device("M15086");
        PLC_Device PLC_Device_CCD01_03_SNAP_電子快門 = new PLC_Device("F9040");
        PLC_Device PLC_Device_CCD01_03_SNAP_視訊增益 = new PLC_Device("F9041");
        PLC_Device PLC_Device_CCD01_03_SNAP_銳利度 = new PLC_Device("F9042");
        PLC_Device PLC_Device_CCD01_03_SNAP_光源亮度_紅正照 = new PLC_Device("F25040");
        PLC_Device PLC_Device_CCD01_03_SNAP_光源亮度_白正照 = new PLC_Device("F25041");
        int cnt_Program_CCD01_03_SNAP = 65534;
        void sub_Program_CCD01_03_SNAP()
        {
            if (cnt_Program_CCD01_03_SNAP == 65534)
            {
                PLC_Device_CCD01_03_SNAP.SetComment("PLC_CCD01_03_SNAP");
                PLC_Device_CCD01_03_SNAP.Bool = false;
                PLC_Device_CCD01_03_SNAP_按鈕.Bool = false;
                cnt_Program_CCD01_03_SNAP = 65535;
            }
            if (cnt_Program_CCD01_03_SNAP == 65535) cnt_Program_CCD01_03_SNAP = 1;
            if (cnt_Program_CCD01_03_SNAP == 1) cnt_Program_CCD01_03_SNAP_檢查按下(ref cnt_Program_CCD01_03_SNAP);
            if (cnt_Program_CCD01_03_SNAP == 2) cnt_Program_CCD01_03_SNAP_初始化(ref cnt_Program_CCD01_03_SNAP);
            if (cnt_Program_CCD01_03_SNAP == 3) cnt_Program_CCD01_03_SNAP_開始取像(ref cnt_Program_CCD01_03_SNAP);
            if (cnt_Program_CCD01_03_SNAP == 4) cnt_Program_CCD01_03_SNAP_取像結束(ref cnt_Program_CCD01_03_SNAP);
            if (cnt_Program_CCD01_03_SNAP == 5) cnt_Program_CCD01_03_SNAP_繪製影像(ref cnt_Program_CCD01_03_SNAP);
            if (cnt_Program_CCD01_03_SNAP == 6) cnt_Program_CCD01_03_SNAP = 65500;
            if (cnt_Program_CCD01_03_SNAP > 1) cnt_Program_CCD01_03_SNAP_檢查放開(ref cnt_Program_CCD01_03_SNAP);

            if (cnt_Program_CCD01_03_SNAP == 65500)
            {
                PLC_Device_CCD01_03_SNAP_按鈕.Bool = false;
                PLC_Device_CCD01_03_SNAP.Bool = false;
                cnt_Program_CCD01_03_SNAP = 65535;
            }
        }
        void cnt_Program_CCD01_03_SNAP_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_03_SNAP_按鈕.Bool)
            {
                PLC_Device_CCD01_03_SNAP.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_03_SNAP_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_03_SNAP.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_03_SNAP_初始化(ref int cnt)
        {
            PLC_Device_CCD01_SNAP_電子快門.Value = PLC_Device_CCD01_03_SNAP_電子快門.Value;
            PLC_Device_CCD01_SNAP_視訊增益.Value = PLC_Device_CCD01_03_SNAP_視訊增益.Value;
            PLC_Device_CCD01_SNAP_銳利度.Value = PLC_Device_CCD01_03_SNAP_銳利度.Value;
            if (PLC_Device_CCD01_03_SNAP_光源亮度_紅正照.Value != 0)
            {
                this.光源控制(enum_光源.CCD01_紅正照, (byte)this.PLC_Device_CCD01_03_SNAP_光源亮度_紅正照.Value);
                this.光源控制(enum_光源.CCD01_紅正照, true);
            }
            else if (this.PLC_Device_CCD01_03_SNAP_光源亮度_紅正照.Value == 0)
            {
                this.光源控制(enum_光源.CCD01_紅正照, (byte)0);
                this.光源控制(enum_光源.CCD01_紅正照, false);
            }
            if (PLC_Device_CCD01_03_SNAP_光源亮度_白正照.Value != 0)
            {
                this.光源控制(enum_光源.CCD01_白正照, (byte)this.PLC_Device_CCD01_03_SNAP_光源亮度_白正照.Value);
                this.光源控制(enum_光源.CCD01_白正照, true);
            }
            else if (this.PLC_Device_CCD01_03_SNAP_光源亮度_白正照.Value == 0)
            {
                this.光源控制(enum_光源.CCD01_白正照, (byte)0);
                this.光源控制(enum_光源.CCD01_白正照, false);
            }
            cnt++;
        }
        void cnt_Program_CCD01_03_SNAP_開始取像(ref int cnt)
        {
            if (!PLC_Device_CCD01_SNAP.Bool)
            {
                PLC_Device_CCD01_SNAP.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_03_SNAP_取像結束(ref int cnt)
        {
            if (!PLC_Device_CCD01_SNAP.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_03_SNAP_繪製影像(ref int cnt)
        {
            this.CCD01_03_SrcImageHandle = this.h_Canvas_Main_CCD01_03_檢測畫面.VegaHandle;
            this.h_Canvas_Main_CCD01_03_檢測畫面.ImageCopy(this.CCD01_AxImageBW8.VegaHandle);

            this.CCD01_03_SrcImageHandle = this.h_Canvas_Tech_CCD01_03.VegaHandle;
            this.h_Canvas_Tech_CCD01_03.ImageCopy(this.CCD01_AxImageBW8.VegaHandle);
            this.h_Canvas_Tech_CCD01_03.SetImageSize(this.h_Canvas_Tech_CCD01_03.ImageWidth, this.h_Canvas_Tech_CCD01_03.ImageHeight);

            if (!PLC_Device_CCD01_03_Tech_取像並檢驗.Bool && !PLC_Device_CCD01_03_Main_取像並檢驗.Bool)
            {
                if (this.PLC_Device_CCD01_03_SNAP.Bool) this.h_Canvas_Tech_CCD01_03.RefreshCanvas();


                if (PLC_Device_CCD01_03_SNAP_LIVE.Bool)
                {
                    cnt = 2;
                    return;
                }
                else
                {
                    光源控制(enum_光源.CCD01_紅正照, (byte)0);
                    光源控制(enum_光源.CCD01_紅正照, false);
                    光源控制(enum_光源.CCD01_白正照, (byte)0);
                    光源控制(enum_光源.CCD01_白正照, false);
                    cnt++;
                }
            }
            else cnt++;

        }





        #endregion
        #region PLC_CCD01_03_Main_取像並檢驗
        PLC_Device PLC_Device_CCD01_03_Main_取像並檢驗按鈕 = new PLC_Device("S39920");
        PLC_Device PLC_Device_CCD01_03_Main_取像並檢驗 = new PLC_Device("S39921");
        PLC_Device PLC_Device_CCD01_03_Main_取像並檢驗_OK = new PLC_Device("S39922");
        PLC_Device PLC_Device_CCD01_03_PLC觸發檢測 = new PLC_Device("S39720");
        PLC_Device PLC_Device_CCD01_03_PLC觸發檢測完成 = new PLC_Device("S39721");
        PLC_Device PLC_Device_CCD01_03_Main_取像完成 = new PLC_Device("S39722");
        PLC_Device PLC_Device_CCD01_03_Main_BUSY = new PLC_Device("S39723");
        bool flag_CCD01_03_開始存檔 = false;
        String CCD01_03_原圖位置, CCD01_03_量測圖位置;
        PLC_Device PLC_NumBox_CCD01_03_OK最大儲存張數 = new PLC_Device("F12403");
        PLC_Device PLC_NumBox_CCD01_03_NG最大儲存張數 = new PLC_Device("F12404");
        MyTimer CCD01_03_Init_Timer = new MyTimer();
        int cnt_Program_CCD01_03_Main_取像並檢驗 = 65534;
        void sub_Program_CCD01_03_Main_取像並檢驗()
        {
            if (cnt_Program_CCD01_03_Main_取像並檢驗 == 65534)
            {
                PLC_Device_CCD01_03_Main_取像並檢驗.SetComment("PLC_CCD01_03_Main_取像並檢驗");
                PLC_Device_CCD01_03_Main_BUSY.Bool = false;
                PLC_Device_CCD01_03_Main_取像完成.Bool = false;
                PLC_Device_CCD01_03_Main_取像並檢驗.Bool = false;
                PLC_Device_CCD01_03_PLC觸發檢測.Bool = false;
                PLC_Device_CCD01_03_PLC觸發檢測完成.Bool = false;
                PLC_Device_CCD01_03_Main_取像並檢驗_OK.Bool = false;
                PLC_Device_CCD01_03_Main_取像並檢驗按鈕.Bool = false;
                cnt_Program_CCD01_03_Main_取像並檢驗 = 65535;

            }
            if (cnt_Program_CCD01_03_Main_取像並檢驗 == 65535) cnt_Program_CCD01_03_Main_取像並檢驗 = 1;
            if (cnt_Program_CCD01_03_Main_取像並檢驗 == 1) cnt_Program_CCD01_03_Main_取像並檢驗_檢查按下(ref cnt_Program_CCD01_03_Main_取像並檢驗);
            if (cnt_Program_CCD01_03_Main_取像並檢驗 == 2) cnt_Program_CCD01_03_Main_取像並檢驗_初始化(ref cnt_Program_CCD01_03_Main_取像並檢驗);
            if (cnt_Program_CCD01_03_Main_取像並檢驗 == 3) cnt_Program_CCD01_03_Main_取像並檢驗_開始SNAP(ref cnt_Program_CCD01_03_Main_取像並檢驗);
            if (cnt_Program_CCD01_03_Main_取像並檢驗 == 4) cnt_Program_CCD01_03_Main_取像並檢驗_結束SNAP(ref cnt_Program_CCD01_03_Main_取像並檢驗);
            if (cnt_Program_CCD01_03_Main_取像並檢驗 == 5) cnt_Program_CCD01_03_Main_取像並檢驗_開始計算一次(ref cnt_Program_CCD01_03_Main_取像並檢驗);
            if (cnt_Program_CCD01_03_Main_取像並檢驗 == 6) cnt_Program_CCD01_03_Main_取像並檢驗_結束計算一次(ref cnt_Program_CCD01_03_Main_取像並檢驗);
            if (cnt_Program_CCD01_03_Main_取像並檢驗 == 7) cnt_Program_CCD01_03_Main_取像並檢驗_繪製畫布(ref cnt_Program_CCD01_03_Main_取像並檢驗);
            if (cnt_Program_CCD01_03_Main_取像並檢驗 == 8) cnt_Program_CCD01_03_Main_取像並檢驗_檢查重測次數(ref cnt_Program_CCD01_03_Main_取像並檢驗);
            if (cnt_Program_CCD01_03_Main_取像並檢驗 == 9) cnt_Program_CCD01_03_Main_取像並檢驗 = 65500;
            if (cnt_Program_CCD01_03_Main_取像並檢驗 > 1) cnt_Program_CCD01_03_Main_取像並檢驗_檢查放開(ref cnt_Program_CCD01_03_Main_取像並檢驗);

            if (cnt_Program_CCD01_03_Main_取像並檢驗 == 65500)
            {
                PLC_Device_CCD01_03_Main_BUSY.Bool = false;
                PLC_Device_CCD01_03_Main_取像完成.Bool = false;
                PLC_Device_CCD01_03_Main_取像並檢驗.Bool = false;
                PLC_Device_CCD01_03_PLC觸發檢測.Bool = false;
                PLC_Device_CCD01_03_Main_取像並檢驗按鈕.Bool = false;
                cnt_Program_CCD01_03_Main_取像並檢驗 = 65535;
            }
        }
        void cnt_Program_CCD01_03_Main_取像並檢驗_檢查按下(ref int cnt)
        {

            if (PLC_Device_CCD01_03_Main_取像並檢驗按鈕.Bool || PLC_Device_CCD01_03_PLC觸發檢測.Bool)
            {
                CCD01_03_Init_Timer.TickStop();
                CCD01_03_Init_Timer.StartTickTime(100000);
                PLC_Device_CCD01_03_Main_取像並檢驗.Bool = true;
                cnt++;
            }



        }
        void cnt_Program_CCD01_03_Main_取像並檢驗_檢查放開(ref int cnt)
        {
            //if (!PLC_Device_CCD01_03_Main_取像並檢驗.Bool && !PLC_Device_CCD01_03_PLC觸發檢測.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_03_Main_取像並檢驗_初始化(ref int cnt)
        {
            PLC_Device_CCD01_03_Main_BUSY.Bool = true;
            PLC_Device_CCD01_03_PLC觸發檢測完成.Bool = false;
            cnt++;
        }
        void cnt_Program_CCD01_03_Main_取像並檢驗_開始SNAP(ref int cnt)
        {
            if (!PLC_Device_CCD01_03_SNAP.Bool)
            {
                PLC_Device_CCD01_03_SNAP_按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_03_Main_取像並檢驗_結束SNAP(ref int cnt)
        {
            if (!PLC_Device_CCD01_03_SNAP_按鈕.Bool)
            {
                光源控制(enum_光源.CCD01_紅正照, (byte)0);
                光源控制(enum_光源.CCD01_紅正照, false);
                光源控制(enum_光源.CCD01_白正照, (byte)0);
                光源控制(enum_光源.CCD01_白正照, false);
                PLC_Device_CCD01_03_Main_取像完成.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_03_Main_取像並檢驗_開始計算一次(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_03_計算一次.Bool)
            {

                this.PLC_Device_CCD01_03_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_03_Main_取像並檢驗_結束計算一次(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_03_計算一次.Bool)
            {

                Console.WriteLine($"CCD01_03檢測,耗時 {CCD01_03_Init_Timer.ToString()}");
                cnt++;
            }
        }
        void cnt_Program_CCD01_03_Main_取像並檢驗_繪製畫布(ref int cnt)
        {
            if (CCD01_03_SrcImageHandle != 0)
            {
                this.h_Canvas_Main_CCD01_03_檢測畫面.RefreshCanvas();
                PLC_Device_CCD01_03_PLC觸發檢測完成.Bool = true;
                flag_CCD01_03_開始存檔 = true;
            }
            cnt++;
        }
        void cnt_Program_CCD01_03_Main_取像並檢驗_檢查重測次數(ref int cnt)
        {
            cnt++;
        }
        private void button7_Click(object sender, EventArgs e)
        {
            flag_CCD01_03_開始存檔 = true;
            flag_CCD02_03_開始存檔 = true;
        }
        private void CCD01_03_儲存圖片()
        {
            if (flag_CCD01_03_開始存檔)
            {
                String FilePlaceOK = plC_WordBox_CCD01_03_OK存圖路徑.Text;
                String FileNameOK = "CCD01_03_OK";
                String FilePlaceNG = plC_WordBox_CCD01_03_NG存圖路徑.Text;
                String FileNameNG = "CCD01_03_NG";
                儲存檔案_往後移位(FilePlaceOK, FileNameOK, PLC_NumBox_CCD01_03_OK最大儲存張數.Value);
                儲存檔案_往後移位(FilePlaceNG, FileNameNG, PLC_NumBox_CCD01_03_NG最大儲存張數.Value);
                if (PLC_Device_CCD01_03_Main_取像並檢驗_OK.Bool)
                {
                    整理檔案(FilePlaceOK, FileNameOK, PLC_NumBox_CCD01_03_OK最大儲存張數.Value);
                    FileNameOK = FileNameOK + "_OK";
                    CCD01_03_原圖位置 = CCD01_03_OK儲存檔案檢查(FilePlaceOK, FileNameOK + "_A", PLC_NumBox_CCD01_03_OK最大儲存張數.Value);
                    CCD01_03_量測圖位置 = CCD01_03_原圖位置.Replace("_A", "_B");
                    this.Invoke(new Action(delegate
                    {
                        if (plC_ComboBox_CCD01_03_OK是否存圖.SelectedIndex == 0)
                        {
                            this.h_Canvas_Main_CCD01_03_檢測畫面.SaveImage(CCD01_03_原圖位置);
                        }
                    }));
                }
                else if (!PLC_Device_CCD01_03_Main_取像並檢驗_OK.Bool)
                {
                    整理檔案(FilePlaceNG, FileNameNG, PLC_NumBox_CCD01_03_NG最大儲存張數.Value);
                    FileNameNG = FileNameNG + "_NG";
                    CCD01_03_原圖位置 = CCD01_03_NG儲存檔案檢查(FilePlaceNG, FileNameNG + "_A", PLC_NumBox_CCD01_03_NG最大儲存張數.Value);
                    CCD01_03_量測圖位置 = CCD01_03_原圖位置.Replace("_A", "_B");
                    this.Invoke(new Action(delegate
                    {
                        if (plC_ComboBox_CCD01_03_NG是否存圖.SelectedIndex == 0)
                        {
                            this.h_Canvas_Main_CCD01_03_檢測畫面.SaveImage(CCD01_03_原圖位置);
                        }
                    }));
                }
                flag_CCD01_03_開始存檔 = false;
            }
        }
        #endregion
        #region PLC_CCD01_03_Tech_取像並檢驗
        PLC_Device PLC_Device_CCD01_03_Tech_取像並檢驗按鈕 = new PLC_Device("M15690");
        PLC_Device PLC_Device_CCD01_03_Tech_取像並檢驗 = new PLC_Device("M15685");
        int cnt_Program_CCD01_03_Tech_取像並檢驗 = 65534;
        void sub_Program_CCD01_03_Tech_取像並檢驗()
        {
            if (cnt_Program_CCD01_03_Tech_取像並檢驗 == 65534)
            {
                PLC_Device_CCD01_03_Tech_取像並檢驗.SetComment("PLC_CCD01_03_Tech_取像並檢驗");
                PLC_Device_CCD01_03_Tech_取像並檢驗.Bool = false;
                PLC_Device_CCD01_03_Tech_取像並檢驗按鈕.Bool = false;
                cnt_Program_CCD01_03_Tech_取像並檢驗 = 65535;
            }
            if (cnt_Program_CCD01_03_Tech_取像並檢驗 == 65535) cnt_Program_CCD01_03_Tech_取像並檢驗 = 1;
            if (cnt_Program_CCD01_03_Tech_取像並檢驗 == 1) cnt_Program_CCD01_03_Tech_取像並檢驗_檢查按下(ref cnt_Program_CCD01_03_Tech_取像並檢驗);
            if (cnt_Program_CCD01_03_Tech_取像並檢驗 == 2) cnt_Program_CCD01_03_Tech_取像並檢驗_初始化(ref cnt_Program_CCD01_03_Tech_取像並檢驗);
            if (cnt_Program_CCD01_03_Tech_取像並檢驗 == 3) cnt_Program_CCD01_03_Tech_取像並檢驗_SNAP一次開始(ref cnt_Program_CCD01_03_Tech_取像並檢驗);
            if (cnt_Program_CCD01_03_Tech_取像並檢驗 == 4) cnt_Program_CCD01_03_Tech_取像並檢驗_SNAP一次結束(ref cnt_Program_CCD01_03_Tech_取像並檢驗);
            if (cnt_Program_CCD01_03_Tech_取像並檢驗 == 5) cnt_Program_CCD01_03_Tech_取像並檢驗_計算一次開始(ref cnt_Program_CCD01_03_Tech_取像並檢驗);
            if (cnt_Program_CCD01_03_Tech_取像並檢驗 == 6) cnt_Program_CCD01_03_Tech_取像並檢驗_計算一次結束(ref cnt_Program_CCD01_03_Tech_取像並檢驗);
            if (cnt_Program_CCD01_03_Tech_取像並檢驗 == 7) cnt_Program_CCD01_03_Tech_取像並檢驗_繪製畫布(ref cnt_Program_CCD01_03_Tech_取像並檢驗);
            if (cnt_Program_CCD01_03_Tech_取像並檢驗 == 8) cnt_Program_CCD01_03_Tech_取像並檢驗 = 65500;
            if (cnt_Program_CCD01_03_Tech_取像並檢驗 > 1) cnt_Program_CCD01_03_Tech_取像並檢驗_檢查放開(ref cnt_Program_CCD01_03_Tech_取像並檢驗);

            if (cnt_Program_CCD01_03_Tech_取像並檢驗 == 65500)
            {
                PLC_Device_CCD01_03_Tech_取像並檢驗按鈕.Bool = false;
                PLC_Device_CCD01_03_Tech_取像並檢驗.Bool = false;
                cnt_Program_CCD01_03_Tech_取像並檢驗 = 65535;
            }
        }
        void cnt_Program_CCD01_03_Tech_取像並檢驗_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_03_Tech_取像並檢驗按鈕.Bool)
            {
                PLC_Device_CCD01_03_Tech_取像並檢驗.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_03_Tech_取像並檢驗_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_03_Tech_取像並檢驗.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_03_Tech_取像並檢驗_初始化(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_03_Tech_取像並檢驗_SNAP一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_03_SNAP.Bool)
            {
                this.PLC_Device_CCD01_03_SNAP_按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_03_Tech_取像並檢驗_SNAP一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_03_SNAP_按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_03_Tech_取像並檢驗_計算一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_03_計算一次.Bool)
            {
                this.PLC_Device_CCD01_03_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_03_Tech_取像並檢驗_計算一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_03_計算一次.Bool)
            {

                cnt++;
            }
        }
        void cnt_Program_CCD01_03_Tech_取像並檢驗_繪製畫布(ref int cnt)
        {
            if (CCD01_03_SrcImageHandle != 0)
            {
                this.h_Canvas_Tech_CCD01_03.RefreshCanvas();
            }
            if (PLC_Device_CCD01_03_SNAP_LIVE.Bool)
            {

                cnt = 2;
                return;
            }
            else
            {
                光源控制(enum_光源.CCD01_紅正照, (byte)0);
                光源控制(enum_光源.CCD01_紅正照, false);
                光源控制(enum_光源.CCD01_白正照, (byte)0);
                光源控制(enum_光源.CCD01_白正照, false);
                cnt++;
            }

        }



        #endregion
        #region PLC_CCD01_03_Tech_檢驗一次
        PLC_Device PLC_Device_CCD01_03_Tech_檢驗一次按鈕 = new PLC_Device("M15390");
        PLC_Device PLC_Device_CCD01_03_Tech_檢驗一次 = new PLC_Device("M15385");
        int cnt_Program_CCD01_03_Tech_檢驗一次 = 65534;
        void sub_Program_CCD01_03_Tech_檢驗一次()
        {
            if (cnt_Program_CCD01_03_Tech_檢驗一次 == 65534)
            {
                PLC_Device_CCD01_03_Tech_檢驗一次.SetComment("PLC_CCD01_03_Tech_檢驗一次");
                PLC_Device_CCD01_03_Tech_檢驗一次.Bool = false;
                PLC_Device_CCD01_03_Tech_檢驗一次按鈕.Bool = false;
                cnt_Program_CCD01_03_Tech_檢驗一次 = 65535;
            }
            if (cnt_Program_CCD01_03_Tech_檢驗一次 == 65535) cnt_Program_CCD01_03_Tech_檢驗一次 = 1;
            if (cnt_Program_CCD01_03_Tech_檢驗一次 == 1) cnt_Program_CCD01_03_Tech_檢驗一次_檢查按下(ref cnt_Program_CCD01_03_Tech_檢驗一次);
            if (cnt_Program_CCD01_03_Tech_檢驗一次 == 2) cnt_Program_CCD01_03_Tech_檢驗一次_初始化(ref cnt_Program_CCD01_03_Tech_檢驗一次);
            if (cnt_Program_CCD01_03_Tech_檢驗一次 == 3) cnt_Program_CCD01_03_Tech_檢驗一次_計算一次開始(ref cnt_Program_CCD01_03_Tech_檢驗一次);
            if (cnt_Program_CCD01_03_Tech_檢驗一次 == 4) cnt_Program_CCD01_03_Tech_檢驗一次_計算一次結束(ref cnt_Program_CCD01_03_Tech_檢驗一次);
            if (cnt_Program_CCD01_03_Tech_檢驗一次 == 5) cnt_Program_CCD01_03_Tech_檢驗一次_繪製畫布(ref cnt_Program_CCD01_03_Tech_檢驗一次);
            if (cnt_Program_CCD01_03_Tech_檢驗一次 == 6) cnt_Program_CCD01_03_Tech_檢驗一次 = 65500;
            if (cnt_Program_CCD01_03_Tech_檢驗一次 > 1) cnt_Program_CCD01_03_Tech_檢驗一次_檢查放開(ref cnt_Program_CCD01_03_Tech_檢驗一次);

            if (cnt_Program_CCD01_03_Tech_檢驗一次 == 65500)
            {
                PLC_Device_CCD01_03_Tech_檢驗一次按鈕.Bool = false;
                PLC_Device_CCD01_03_Tech_檢驗一次.Bool = false;
                cnt_Program_CCD01_03_Tech_檢驗一次 = 65535;
            }
        }
        void cnt_Program_CCD01_03_Tech_檢驗一次_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_03_Tech_檢驗一次按鈕.Bool)
            {
                PLC_Device_CCD01_03_Tech_檢驗一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_03_Tech_檢驗一次_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_03_Tech_檢驗一次.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_03_Tech_檢驗一次_初始化(ref int cnt)
        {

            cnt++;
        }
        void cnt_Program_CCD01_03_Tech_檢驗一次_計算一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_03_計算一次.Bool)
            {
                this.PLC_Device_CCD01_03_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_03_Tech_檢驗一次_計算一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_03_計算一次.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_03_Tech_檢驗一次_繪製畫布(ref int cnt)
        {
            if (CCD01_03_SrcImageHandle != 0)
            {
                this.h_Canvas_Tech_CCD01_03.RefreshCanvas();
            }
            cnt++;
        }

































        #endregion
        #region PLC_CCD01_03_Main_檢驗一次
        PLC_Device PLC_Device_CCD01_03_Main_檢驗一次按鈕 = new PLC_Device("M15804");
        PLC_Device PLC_Device_CCD01_03_Main_檢驗一次 = new PLC_Device("M15805");
        int cnt_Program_CCD01_03_Main_檢驗一次 = 65534;
        void sub_Program_CCD01_03_Main_檢驗一次()
        {
            if (cnt_Program_CCD01_03_Main_檢驗一次 == 65534)
            {
                PLC_Device_CCD01_03_Main_檢驗一次.SetComment("PLC_CCD01_03_Main_檢驗一次");
                PLC_Device_CCD01_03_Main_檢驗一次.Bool = false;
                PLC_Device_CCD01_03_Main_檢驗一次按鈕.Bool = false;
                cnt_Program_CCD01_03_Main_檢驗一次 = 65535;
            }
            if (cnt_Program_CCD01_03_Main_檢驗一次 == 65535) cnt_Program_CCD01_03_Main_檢驗一次 = 1;
            if (cnt_Program_CCD01_03_Main_檢驗一次 == 1) cnt_Program_CCD01_03_Main_檢驗一次_檢查按下(ref cnt_Program_CCD01_03_Main_檢驗一次);
            if (cnt_Program_CCD01_03_Main_檢驗一次 == 2) cnt_Program_CCD01_03_Main_檢驗一次_初始化(ref cnt_Program_CCD01_03_Main_檢驗一次);
            if (cnt_Program_CCD01_03_Main_檢驗一次 == 3) cnt_Program_CCD01_03_Main_檢驗一次_計算一次開始(ref cnt_Program_CCD01_03_Main_檢驗一次);
            if (cnt_Program_CCD01_03_Main_檢驗一次 == 4) cnt_Program_CCD01_03_Main_檢驗一次_計算一次結束(ref cnt_Program_CCD01_03_Main_檢驗一次);
            if (cnt_Program_CCD01_03_Main_檢驗一次 == 5) cnt_Program_CCD01_03_Main_檢驗一次_繪製畫布(ref cnt_Program_CCD01_03_Main_檢驗一次);
            if (cnt_Program_CCD01_03_Main_檢驗一次 == 6) cnt_Program_CCD01_03_Main_檢驗一次 = 65500;
            if (cnt_Program_CCD01_03_Main_檢驗一次 > 1) cnt_Program_CCD01_03_Main_檢驗一次_檢查放開(ref cnt_Program_CCD01_03_Main_檢驗一次);

            if (cnt_Program_CCD01_03_Main_檢驗一次 == 65500)
            {
                PLC_Device_CCD01_03_Main_檢驗一次按鈕.Bool = false;
                PLC_Device_CCD01_03_Main_檢驗一次.Bool = false;
                cnt_Program_CCD01_03_Main_檢驗一次 = 65535;
            }
        }
        void cnt_Program_CCD01_03_Main_檢驗一次_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_03_Main_檢驗一次按鈕.Bool)
            {
                PLC_Device_CCD01_03_Main_檢驗一次.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_03_Main_檢驗一次_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_03_Main_檢驗一次.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_03_Main_檢驗一次_初始化(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_03_Main_檢驗一次_計算一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_03_計算一次.Bool)
            {
                this.PLC_Device_CCD01_03_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_03_Main_檢驗一次_計算一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_03_計算一次.Bool)
            {

                cnt++;
            }
        }
        void cnt_Program_CCD01_03_Main_檢驗一次_繪製畫布(ref int cnt)
        {
            if (CCD01_03_SrcImageHandle != 0)
            {
                this.h_Canvas_Main_CCD01_03_檢測畫面.RefreshCanvas();
            }

            cnt++;
        }
        #endregion
        #region PLC_CCD01_03_計算一次
        PLC_Device PLC_Device_CCD01_03_計算一次 = new PLC_Device("S5045");
        PLC_Device PLC_Device_CCD01_03_計算一次_OK = new PLC_Device("S5046");
        PLC_Device PLC_Device_CCD01_03_計算一次_READY = new PLC_Device("S5047");
        MyTimer MyTimer_CCD01_03_計算一次 = new MyTimer();
        int cnt_Program_CCD01_03_計算一次 = 65534;
        void sub_Program_CCD01_03_計算一次()
        {
            this.PLC_Device_CCD01_03_計算一次_READY.Bool = !this.PLC_Device_CCD01_03_計算一次.Bool;
            if (cnt_Program_CCD01_03_計算一次 == 65534)
            {
                PLC_Device_CCD01_03_計算一次.SetComment("PLC_CCD01_03_計算一次");
                PLC_Device_CCD01_03_計算一次.Bool = false;

                cnt_Program_CCD01_03_計算一次 = 65535;
            }
            if (cnt_Program_CCD01_03_計算一次 == 65535) cnt_Program_CCD01_03_計算一次 = 1;
            if (cnt_Program_CCD01_03_計算一次 == 1) cnt_Program_CCD01_03_計算一次_檢查按下(ref cnt_Program_CCD01_03_計算一次);
            if (cnt_Program_CCD01_03_計算一次 == 2) cnt_Program_CCD01_03_計算一次_初始化(ref cnt_Program_CCD01_03_計算一次);
            if (cnt_Program_CCD01_03_計算一次 == 3) cnt_Program_CCD01_03_計算一次_步驟01開始(ref cnt_Program_CCD01_03_計算一次);
            if (cnt_Program_CCD01_03_計算一次 == 4) cnt_Program_CCD01_03_計算一次_步驟01結束(ref cnt_Program_CCD01_03_計算一次);
            if (cnt_Program_CCD01_03_計算一次 == 5) cnt_Program_CCD01_03_計算一次_步驟02開始(ref cnt_Program_CCD01_03_計算一次);
            if (cnt_Program_CCD01_03_計算一次 == 6) cnt_Program_CCD01_03_計算一次_步驟02結束(ref cnt_Program_CCD01_03_計算一次);
            if (cnt_Program_CCD01_03_計算一次 == 7) cnt_Program_CCD01_03_計算一次_步驟03開始(ref cnt_Program_CCD01_03_計算一次);
            if (cnt_Program_CCD01_03_計算一次 == 8) cnt_Program_CCD01_03_計算一次_步驟03結束(ref cnt_Program_CCD01_03_計算一次);
            if (cnt_Program_CCD01_03_計算一次 == 9) cnt_Program_CCD01_03_計算一次_步驟04開始(ref cnt_Program_CCD01_03_計算一次);
            if (cnt_Program_CCD01_03_計算一次 == 10) cnt_Program_CCD01_03_計算一次_步驟04結束(ref cnt_Program_CCD01_03_計算一次);
            if (cnt_Program_CCD01_03_計算一次 == 11) cnt_Program_CCD01_03_計算一次_步驟05開始(ref cnt_Program_CCD01_03_計算一次);
            if (cnt_Program_CCD01_03_計算一次 == 12) cnt_Program_CCD01_03_計算一次_步驟05結束(ref cnt_Program_CCD01_03_計算一次);
            if (cnt_Program_CCD01_03_計算一次 == 13) cnt_Program_CCD01_03_計算一次_步驟06開始(ref cnt_Program_CCD01_03_計算一次);
            if (cnt_Program_CCD01_03_計算一次 == 14) cnt_Program_CCD01_03_計算一次_步驟06結束(ref cnt_Program_CCD01_03_計算一次);
            if (cnt_Program_CCD01_03_計算一次 == 15) cnt_Program_CCD01_03_計算一次_計算結果(ref cnt_Program_CCD01_03_計算一次);
            if (cnt_Program_CCD01_03_計算一次 == 16) cnt_Program_CCD01_03_計算一次 = 65500;
            if (cnt_Program_CCD01_03_計算一次 > 1) cnt_Program_CCD01_03_計算一次_檢查放開(ref cnt_Program_CCD01_03_計算一次);

            if (cnt_Program_CCD01_03_計算一次 == 65500)
            {
                PLC_Device_CCD01_03_計算一次.Bool = false;
                cnt_Program_CCD01_03_計算一次 = 65535;
            }
        }
        void cnt_Program_CCD01_03_計算一次_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_03_計算一次.Bool) cnt++;
        }
        void cnt_Program_CCD01_03_計算一次_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_03_計算一次.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_03_計算一次_初始化(ref int cnt)
        {
            PLC_Device_CCD01_03_基準線量測.Bool = false;
            PLC_Device_CCD01_03基準圓量測框調整.Bool = false;
            PLC_Device_CCD01_03圓柱相似度量測.Bool = false;
            PLC_Device_CCD01_03圓直徑量測.Bool = false;
            PLC_Device_CCD01_03_Main_取像並檢驗_OK.Bool = false;
            cnt++;
        }
        void cnt_Program_CCD01_03_計算一次_步驟01開始(ref int cnt)
        {
            this.MyTimer_CCD01_03_計算一次.TickStop();
            this.MyTimer_CCD01_03_計算一次.StartTickTime(99999);
            if(!PLC_Device_CCD01_03_基準線量測按鈕.Bool)
            {
                PLC_Device_CCD01_03_基準線量測按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_03_計算一次_步驟01結束(ref int cnt)
        {
            if (!PLC_Device_CCD01_03_基準線量測按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_03_計算一次_步驟02開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_03基準圓量測框調整按鈕.Bool)
            {
                this.PLC_Device_CCD01_03基準圓量測框調整按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_03_計算一次_步驟02結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_03基準圓量測框調整按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_03_計算一次_步驟03開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_03圓柱相似度量測按鈕.Bool)
            {
                this.PLC_Device_CCD01_03圓柱相似度量測按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_03_計算一次_步驟03結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_03圓柱相似度量測按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_03_計算一次_步驟04開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_03圓直徑量測按鈕.Bool)
            {
                this.PLC_Device_CCD01_03圓直徑量測按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_03_計算一次_步驟04結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_03圓直徑量測按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_03_計算一次_步驟05開始(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_03_計算一次_步驟05結束(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_03_計算一次_步驟06開始(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_03_計算一次_步驟06結束(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_03_計算一次_計算結果(ref int cnt)
        {
            bool flag = true;
            if (!this.PLC_Device_CCD01_03_基準線量測_OK.Bool) flag = false;
            if (!this.PLC_Device_CCD01_03圓柱相似度量測_OK.Bool) flag = false;
            if (!this.PLC_Device_CCD01_03基準圓量測框調整_OK.Bool) flag = false;
            if (!this.PLC_Device_CCD01_03圓直徑量測_OK.Bool) flag = false;

            this.PLC_Device_CCD01_03_計算一次_OK.Bool = flag;
            this.PLC_Device_CCD01_03_Main_取像並檢驗_OK.Bool = flag;
            //flag_CCD01_03_上端水平度寫入列表資料 = true;
            //flag_CCD01_03_上端間距寫入列表資料 = true;
            //flag_CCD01_03_上端水平度差值寫入列表資料 = true;

            cnt++;
        }





        #endregion
        #region PLC_CCD01_03_基準線量測
        AxOvkMsr.AxLineMsr CCD01_03_水平基準線量測_AxLineMsr;
        AxOvkMsr.AxLineRegression CCD01_03_水平基準線量測_AxLineRegression;
        AxOvkMsr.AxLineMsr CCD01_03_垂直基準線量測_AxLineMsr;
        AxOvkMsr.AxLineRegression CCD01_03_垂直基準線量測_AxLineRegression;
        AxOvkMsr.AxIntersectionMsr CCD01_03_基準線量測_AxIntersectionMsr;
        private PointF Point_CCD01_03_中心基準座標_量測點 = new PointF();
        PLC_Device PLC_Device_CCD01_03_基準線量測按鈕 = new PLC_Device("S6640");
        PLC_Device PLC_Device_CCD01_03_基準線量測 = new PLC_Device("S6635");
        PLC_Device PLC_Device_CCD01_03_基準線量測_OK = new PLC_Device("S6636");
        PLC_Device PLC_Device_CCD01_03_基準線量測_測試完成 = new PLC_Device("S6637");
        PLC_Device PLC_Device_CCD01_03_基準線量測_RefreshCanvas = new PLC_Device("S6638");

        PLC_Device PLC_Device_CCD01_03_基準線量測_變化銳利度 = new PLC_Device("F18100");
        PLC_Device PLC_Device_CCD01_03_基準線量測_延伸變化強度 = new PLC_Device("F18101");
        PLC_Device PLC_Device_CCD01_03_基準線量測_灰階變化面積 = new PLC_Device("F18102");
        PLC_Device PLC_Device_CCD01_03_基準線量測_雜訊抑制 = new PLC_Device("F18103");
        PLC_Device PLC_Device_CCD01_03_基準線量測_最佳回歸線計算次數 = new PLC_Device("F18104");
        PLC_Device PLC_Device_CCD01_03_基準線量測_最佳回歸線濾波 = new PLC_Device("F18105");
        PLC_Device PLC_Device_CCD01_03_基準線量測_量測顏色變化 = new PLC_Device("F18110");
        PLC_Device PLC_Device_CCD01_03_基準線量測_基準線偏移 = new PLC_Device("F18111");
        PLC_Device PLC_Device_CCD01_03_基準線量測_基準線偏移_上排 = new PLC_Device("F18118");
        PLC_Device PLC_Device_CCD01_03_基準線量測_基準線偏移_下排 = new PLC_Device("F18119");

        PLC_Device PLC_Device_CCD01_03_水平基準線量測_量測框起點X座標 = new PLC_Device("F18106");
        PLC_Device PLC_Device_CCD01_03_水平基準線量測_量測框起點Y座標 = new PLC_Device("F18107");
        PLC_Device PLC_Device_CCD01_03_水平基準線量測_量測框終點X座標 = new PLC_Device("F18108");
        PLC_Device PLC_Device_CCD01_03_水平基準線量測_量測框終點Y座標 = new PLC_Device("F18109");
        PLC_Device PLC_Device_CCD01_03_水平基準線量測_量測高度 = new PLC_Device("F18112");
        PLC_Device PLC_Device_CCD01_03_水平基準線量測_量測中心_X = new PLC_Device("F18120");
        PLC_Device PLC_Device_CCD01_03_水平基準線量測_量測中心_Y = new PLC_Device("F18121");

        PLC_Device PLC_Device_CCD01_03_垂直基準線量測_量測框起點X座標 = new PLC_Device("F18113");
        PLC_Device PLC_Device_CCD01_03_垂直基準線量測_量測框起點Y座標 = new PLC_Device("F18114");
        PLC_Device PLC_Device_CCD01_03_垂直基準線量測_量測框終點X座標 = new PLC_Device("F18115");
        PLC_Device PLC_Device_CCD01_03_垂直基準線量測_量測框終點Y座標 = new PLC_Device("F18116");
        PLC_Device PLC_Device_CCD01_03_垂直基準線量測_量測高度 = new PLC_Device("F18117");




        private void H_Canvas_Tech_CCD01_03_基準線量測_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {
            try
            {
                PointF 水平量測中心 = new PointF(Point_CCD01_03_中心基準座標_量測點.X, Point_CCD01_03_中心基準座標_量測點.Y);

                if (PLC_Device_CCD01_03_Main_取像並檢驗.Bool || PLC_Device_CCD01_03_PLC觸發檢測.Bool || PLC_Device_CCD01_03_Main_檢驗一次.Bool)
                {
                    if (this.PLC_Device_CCD01_03_基準線量測_RefreshCanvas.Bool)
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);

                        DrawingClass.Draw.水平線段繪製(0, 10000, CCD01_03_水平基準線量測_AxLineMsr.MeasuredSlope, CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotX, CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD01_03_基準線量測_基準線偏移.Value, Color.Lime, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.垂直線段繪製(0, 10000, CCD01_03_垂直基準線量測_AxLineMsr.MeasuredSlope, CCD01_03_垂直基準線量測_AxLineMsr.MeasuredPivotX, CCD01_03_垂直基準線量測_AxLineMsr.MeasuredPivotY, Color.Lime, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.十字中心(水平量測中心, 100, Color.Red, 2, g, ZoomX, ZoomY);
                        if (PLC_Device_CCD01_03_基準線量測_OK.Bool)
                        {
                            DrawingClass.Draw.文字左上繪製("基準線OK!", new PointF(0, 0), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                        }
                        else
                        {
                            DrawingClass.Draw.文字左上繪製("基準線NG!", new PointF(0, 0), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        }
                        g.Dispose();
                        g = null;
                    }
                }
                else if (PLC_Device_CCD01_03_Tech_檢驗一次.Bool)
                {
                    if (this.PLC_Device_CCD01_03_基準線量測_RefreshCanvas.Bool)
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);

                        DrawingClass.Draw.水平線段繪製(0, 10000, CCD01_03_水平基準線量測_AxLineMsr.MeasuredSlope, CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotX, CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD01_03_基準線量測_基準線偏移.Value, Color.Lime, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.垂直線段繪製(0, 10000, CCD01_03_垂直基準線量測_AxLineMsr.MeasuredSlope, CCD01_03_垂直基準線量測_AxLineMsr.MeasuredPivotX, CCD01_03_垂直基準線量測_AxLineMsr.MeasuredPivotY, Color.Lime, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.十字中心(水平量測中心, 100, Color.Red, 2, g, ZoomX, ZoomY);
                        if (PLC_Device_CCD01_03_基準線量測_OK.Bool)
                        {
                            DrawingClass.Draw.文字左上繪製("基準線OK!", new PointF(0, 0), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                        }
                        else
                        {
                            DrawingClass.Draw.文字左上繪製("基準線NG!", new PointF(0, 0), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        }
                        g.Dispose();
                        g = null;
                    }
                }

                else
                {
                    if (this.PLC_Device_CCD01_03_基準線量測_RefreshCanvas.Bool)
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);


                        if (this.plC_CheckBox_CCD01_03_基準線量測_繪製量測框.Checked)
                        {
                            this.CCD01_03_水平基準線量測_AxLineMsr.Title = ("水平基準線");
                            this.CCD01_03_水平基準線量測_AxLineMsr.DrawFrame(HDC, ZoomX, ZoomY, 0, 0);
                            this.CCD01_03_垂直基準線量測_AxLineMsr.Title = ("垂直基準線");
                            this.CCD01_03_垂直基準線量測_AxLineMsr.DrawFrame(HDC, ZoomX, ZoomY, 0, 0);
                        }
                        if (this.plC_CheckBox_CCD01_03_基準線量測_繪製量測線段.Checked)
                        {
                            this.CCD01_03_水平基準線量測_AxLineMsr.DrawFittedPrimitives(HDC, ZoomX, ZoomY, 0, 0);
                            this.CCD01_03_垂直基準線量測_AxLineMsr.DrawFittedPrimitives(HDC, ZoomX, ZoomY, 0, 0);
                            //DrawingClass.Draw.水平線段繪製(0, 10000, CCD01_03_水平基準線量測_AxLineMsr.MeasuredSlope, CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotX, CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD01_03_基準線量測_基準線偏移.Value, Color.Yellow, 2, g, ZoomX, ZoomY);
                            //DrawingClass.Draw.垂直線段繪製(0, 10000, CCD01_03_垂直基準線量測_AxLineMsr.MeasuredSlope, CCD01_03_垂直基準線量測_AxLineMsr.MeasuredPivotX, CCD01_03_垂直基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD01_03_基準線量測_基準線偏移.Value, Color.Yellow, 2, g, ZoomX, ZoomY);
                        }
                        if (this.plC_CheckBox_CCD01_03_基準線量測_繪製量測點.Checked)
                        {
                            this.CCD01_03_水平基準線量測_AxLineMsr.DrawPoints(HDC, ZoomX, ZoomY, 0, 0);
                            this.CCD01_03_垂直基準線量測_AxLineMsr.DrawPoints(HDC, ZoomX, ZoomY, 0, 0);
                        }
                        DrawingClass.Draw.十字中心(水平量測中心, 100, Color.Red, 2, g, ZoomX, ZoomY);


                        if (PLC_Device_CCD01_03_基準線量測_OK.Bool)
                        {
                            DrawingClass.Draw.文字左上繪製("基準線OK!", new PointF(0, 0), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                        }
                        else
                        {
                            DrawingClass.Draw.文字左上繪製("基準線NG!", new PointF(0, 0), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        }
                        g.Dispose();
                        g = null;
                    }
                }

            }

            catch
            {

            }

            this.PLC_Device_CCD01_03_基準線量測_RefreshCanvas.Bool = false;
        }
        private AxOvkMsr.TxAxLineMsrDragHandle CCD01_03_AxOvkMsr_水平基準線量測_TxAxLineMsrDragHandle = new AxOvkMsr.TxAxLineMsrDragHandle();
        private bool flag_CCD01_03_AxOvkMsr_水平基準線量測_MouseDown = false;
        private AxOvkMsr.TxAxLineMsrDragHandle CCD01_03_AxOvkMsr_垂直基準線量測_TxAxLineMsrDragHandle = new AxOvkMsr.TxAxLineMsrDragHandle();
        private bool flag_CCD01_03_AxOvkMsr_垂直基準線量測_MouseDown = false;

        private void H_Canvas_Tech_CCD01_03_基準線量測_OnCanvasMouseDownEvent(int x, int y, float ZoomX, float ZoomY, ref int InUsedEventNum, int InUsedCanvasHandle)
        {

            if (this.PLC_Device_CCD01_03_基準線量測.Bool)
            {
                this.CCD01_03_AxOvkMsr_水平基準線量測_TxAxLineMsrDragHandle = this.CCD01_03_水平基準線量測_AxLineMsr.HitTest(x, y, ZoomX, ZoomY, 0, 0);
                if (this.CCD01_03_AxOvkMsr_水平基準線量測_TxAxLineMsrDragHandle != AxOvkMsr.TxAxLineMsrDragHandle.AX_LINEMSR_NONE)
                {
                    this.flag_CCD01_03_AxOvkMsr_水平基準線量測_MouseDown = true;
                    InUsedEventNum = 10;
                }

                this.CCD01_03_AxOvkMsr_垂直基準線量測_TxAxLineMsrDragHandle = this.CCD01_03_垂直基準線量測_AxLineMsr.HitTest(x, y, ZoomX, ZoomY, 0, 0);
                if (this.CCD01_03_AxOvkMsr_垂直基準線量測_TxAxLineMsrDragHandle != AxOvkMsr.TxAxLineMsrDragHandle.AX_LINEMSR_NONE)
                {
                    this.flag_CCD01_03_AxOvkMsr_垂直基準線量測_MouseDown = true;
                    InUsedEventNum = 10;
                }
            }

        }
        private void H_Canvas_Tech_CCD01_03_基準線量測_OnCanvasMouseMoveEvent(int x, int y, float ZoomX, float ZoomY)
        {
            if (this.flag_CCD01_03_AxOvkMsr_水平基準線量測_MouseDown)
            {
                this.CCD01_03_水平基準線量測_AxLineMsr.DragFrame(this.CCD01_03_AxOvkMsr_水平基準線量測_TxAxLineMsrDragHandle, x, y, ZoomX, ZoomY, 0, 0);
                this.PLC_Device_CCD01_03_水平基準線量測_量測框起點X座標.Value = CCD01_03_水平基準線量測_AxLineMsr.NLineStartX;
                this.PLC_Device_CCD01_03_水平基準線量測_量測框起點Y座標.Value = CCD01_03_水平基準線量測_AxLineMsr.NLineStartY;
                this.PLC_Device_CCD01_03_水平基準線量測_量測框終點X座標.Value = CCD01_03_水平基準線量測_AxLineMsr.NLineEndX;
                this.PLC_Device_CCD01_03_水平基準線量測_量測框終點Y座標.Value = CCD01_03_水平基準線量測_AxLineMsr.NLineEndY;
                this.PLC_Device_CCD01_03_水平基準線量測_量測高度.Value = CCD01_03_水平基準線量測_AxLineMsr.HalfHeight;
            }

            if (this.flag_CCD01_03_AxOvkMsr_垂直基準線量測_MouseDown)
            {
                this.CCD01_03_垂直基準線量測_AxLineMsr.DragFrame(this.CCD01_03_AxOvkMsr_垂直基準線量測_TxAxLineMsrDragHandle, x, y, ZoomX, ZoomY, 0, 0);
                this.PLC_Device_CCD01_03_垂直基準線量測_量測框起點X座標.Value = CCD01_03_垂直基準線量測_AxLineMsr.NLineStartX;
                this.PLC_Device_CCD01_03_垂直基準線量測_量測框起點Y座標.Value = CCD01_03_垂直基準線量測_AxLineMsr.NLineStartY;
                this.PLC_Device_CCD01_03_垂直基準線量測_量測框終點X座標.Value = CCD01_03_垂直基準線量測_AxLineMsr.NLineEndX;
                this.PLC_Device_CCD01_03_垂直基準線量測_量測框終點Y座標.Value = CCD01_03_垂直基準線量測_AxLineMsr.NLineEndY;
                this.PLC_Device_CCD01_03_垂直基準線量測_量測高度.Value = CCD01_03_垂直基準線量測_AxLineMsr.HalfHeight;
            }


        }
        private void H_Canvas_Tech_CCD01_03_基準線量測_OnCanvasMouseUpEvent(int x, int y, float ZoomX, float ZoomY)
        {
            this.flag_CCD01_03_AxOvkMsr_水平基準線量測_MouseDown = false;
            this.flag_CCD01_03_AxOvkMsr_垂直基準線量測_MouseDown = false;
        }
        int cnt_Program_CCD01_03_基準線量測 = 65534;
        void sub_Program_CCD01_03_基準線量測()
        {
            if (cnt_Program_CCD01_03_基準線量測 == 65534)
            {
                this.h_Canvas_Tech_CCD01_03.OnCanvasMouseDownEvent += H_Canvas_Tech_CCD01_03_基準線量測_OnCanvasMouseDownEvent;
                this.h_Canvas_Tech_CCD01_03.OnCanvasMouseMoveEvent += H_Canvas_Tech_CCD01_03_基準線量測_OnCanvasMouseMoveEvent;
                this.h_Canvas_Tech_CCD01_03.OnCanvasMouseUpEvent += H_Canvas_Tech_CCD01_03_基準線量測_OnCanvasMouseUpEvent;
                this.h_Canvas_Tech_CCD01_03.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_03_基準線量測_OnCanvasDrawEvent;

                this.h_Canvas_Main_CCD01_03_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_03_基準線量測_OnCanvasDrawEvent;
                PLC_Device_CCD01_03_基準線量測.SetComment("PLC_CCD01_03_基準線量測");
                PLC_Device_CCD01_03_基準線量測.Bool = false;
                PLC_Device_CCD01_03_基準線量測按鈕.Bool = false;
                cnt_Program_CCD01_03_基準線量測 = 65535;
            }
            if (cnt_Program_CCD01_03_基準線量測 == 65535) cnt_Program_CCD01_03_基準線量測 = 1;
            if (cnt_Program_CCD01_03_基準線量測 == 1) cnt_Program_CCD01_03_基準線量測_檢查按下(ref cnt_Program_CCD01_03_基準線量測);
            if (cnt_Program_CCD01_03_基準線量測 == 2) cnt_Program_CCD01_03_基準線量測_初始化(ref cnt_Program_CCD01_03_基準線量測);
            if (cnt_Program_CCD01_03_基準線量測 == 3) cnt_Program_CCD01_03_基準線量測_開始量測(ref cnt_Program_CCD01_03_基準線量測);
            if (cnt_Program_CCD01_03_基準線量測 == 4) cnt_Program_CCD01_03_基準線量測_兩線交點(ref cnt_Program_CCD01_03_基準線量測);
            if (cnt_Program_CCD01_03_基準線量測 == 5) cnt_Program_CCD01_03_基準線量測_兩線交點量測(ref cnt_Program_CCD01_03_基準線量測);
            if (cnt_Program_CCD01_03_基準線量測 == 6) cnt_Program_CCD01_03_基準線量測_開始繪製(ref cnt_Program_CCD01_03_基準線量測);
            if (cnt_Program_CCD01_03_基準線量測 == 7) cnt_Program_CCD01_03_基準線量測 = 65500;
            if (cnt_Program_CCD01_03_基準線量測 > 1) cnt_Program_CCD01_03_基準線量測_檢查放開(ref cnt_Program_CCD01_03_基準線量測);

            if (cnt_Program_CCD01_03_基準線量測 == 65500)
            {
                if (PLC_Device_CCD01_03_計算一次.Bool)
                {
                    PLC_Device_CCD01_03_基準線量測按鈕.Bool = false;
                }
                PLC_Device_CCD01_03_基準線量測.Bool = false;
                cnt_Program_CCD01_03_基準線量測 = 65535;
            }
        }
        void cnt_Program_CCD01_03_基準線量測_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_03_基準線量測按鈕.Bool)
            {
                PLC_Device_CCD01_03_基準線量測.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_03_基準線量測_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_03_基準線量測.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_03_基準線量測_初始化(ref int cnt)
        {
            this.PLC_Device_CCD01_03_基準線量測_OK.Bool = false;

            this.CCD01_03_水平基準線量測_AxLineMsr.SrcImageHandle = this.CCD01_03_SrcImageHandle;
            this.CCD01_03_水平基準線量測_AxLineMsr.Hysteresis = PLC_Device_CCD01_03_基準線量測_延伸變化強度.Value;
            this.CCD01_03_水平基準線量測_AxLineMsr.DeriThreshold = PLC_Device_CCD01_03_基準線量測_變化銳利度.Value;
            this.CCD01_03_水平基準線量測_AxLineMsr.MinGreyStep = PLC_Device_CCD01_03_基準線量測_灰階變化面積.Value;
            this.CCD01_03_水平基準線量測_AxLineMsr.SmoothFactor = PLC_Device_CCD01_03_基準線量測_雜訊抑制.Value;
            this.CCD01_03_水平基準線量測_AxLineMsr.HalfProfileThickness = 5;
            this.CCD01_03_水平基準線量測_AxLineMsr.SampleStep = 1;
            this.CCD01_03_水平基準線量測_AxLineMsr.FilterCount = PLC_Device_CCD01_03_基準線量測_最佳回歸線計算次數.Value;
            this.CCD01_03_水平基準線量測_AxLineMsr.FilterThreshold = PLC_Device_CCD01_03_基準線量測_最佳回歸線濾波.Value / 10;

            if (this.PLC_Device_CCD01_03_水平基準線量測_量測框起點X座標.Value == 0 && this.PLC_Device_CCD01_03_水平基準線量測_量測框終點X座標.Value == 0)
            {
                this.PLC_Device_CCD01_03_水平基準線量測_量測框起點X座標.Value = 100;
                this.PLC_Device_CCD01_03_水平基準線量測_量測框終點X座標.Value = 100;
            }
            if (this.PLC_Device_CCD01_03_水平基準線量測_量測框起點Y座標.Value == 0 && this.PLC_Device_CCD01_03_水平基準線量測_量測框終點Y座標.Value == 0)
            {
                this.PLC_Device_CCD01_03_水平基準線量測_量測框起點Y座標.Value = 200;
                this.PLC_Device_CCD01_03_水平基準線量測_量測框終點Y座標.Value = 200;
            }
            if (this.PLC_Device_CCD01_03_水平基準線量測_量測高度.Value == 0)
            {
                this.PLC_Device_CCD01_03_水平基準線量測_量測高度.Value = 100;
            }

            this.CCD01_03_水平基準線量測_AxLineMsr.NLineStartX = PLC_Device_CCD01_03_水平基準線量測_量測框起點X座標.Value;
            this.CCD01_03_水平基準線量測_AxLineMsr.NLineStartY = PLC_Device_CCD01_03_水平基準線量測_量測框起點Y座標.Value;
            this.CCD01_03_水平基準線量測_AxLineMsr.NLineEndX = PLC_Device_CCD01_03_水平基準線量測_量測框終點X座標.Value;
            this.CCD01_03_水平基準線量測_AxLineMsr.NLineEndY = PLC_Device_CCD01_03_水平基準線量測_量測框終點Y座標.Value;
            this.CCD01_03_水平基準線量測_AxLineMsr.HalfHeight = PLC_Device_CCD01_03_水平基準線量測_量測高度.Value;

            this.CCD01_03_水平基準線量測_AxLineMsr.EdgeType = (AxOvkMsr.TxAxTransitionType)PLC_Device_CCD01_03_基準線量測_量測顏色變化.Value;
            this.CCD01_03_水平基準線量測_AxLineMsr.LockedMsrDirection = AxOvkMsr.TxAxLineMsrLockedMsrDirection.AX_LINEMSR_LOCKED_CLOCKWISE;


            this.CCD01_03_垂直基準線量測_AxLineMsr.SrcImageHandle = this.CCD01_03_SrcImageHandle;
            this.CCD01_03_垂直基準線量測_AxLineMsr.Hysteresis = PLC_Device_CCD01_03_基準線量測_延伸變化強度.Value;
            this.CCD01_03_垂直基準線量測_AxLineMsr.DeriThreshold = PLC_Device_CCD01_03_基準線量測_變化銳利度.Value;
            this.CCD01_03_垂直基準線量測_AxLineMsr.MinGreyStep = PLC_Device_CCD01_03_基準線量測_灰階變化面積.Value;
            this.CCD01_03_垂直基準線量測_AxLineMsr.SmoothFactor = PLC_Device_CCD01_03_基準線量測_雜訊抑制.Value;
            this.CCD01_03_垂直基準線量測_AxLineMsr.HalfProfileThickness = 5;
            this.CCD01_03_垂直基準線量測_AxLineMsr.SampleStep = 1;
            this.CCD01_03_垂直基準線量測_AxLineMsr.FilterCount = PLC_Device_CCD01_03_基準線量測_最佳回歸線計算次數.Value;
            this.CCD01_03_垂直基準線量測_AxLineMsr.FilterThreshold = PLC_Device_CCD01_03_基準線量測_最佳回歸線濾波.Value / 10;

            if (this.PLC_Device_CCD01_03_垂直基準線量測_量測框起點X座標.Value == 0 && this.PLC_Device_CCD01_03_垂直基準線量測_量測框終點X座標.Value == 0)
            {
                this.PLC_Device_CCD01_03_垂直基準線量測_量測框起點X座標.Value = 100;
                this.PLC_Device_CCD01_03_垂直基準線量測_量測框終點X座標.Value = 100;
            }
            if (this.PLC_Device_CCD01_03_垂直基準線量測_量測框起點Y座標.Value == 0 && this.PLC_Device_CCD01_03_垂直基準線量測_量測框終點Y座標.Value == 0)
            {
                this.PLC_Device_CCD01_03_垂直基準線量測_量測框起點Y座標.Value = 200;
                this.PLC_Device_CCD01_03_垂直基準線量測_量測框終點Y座標.Value = 200;
            }
            if (this.PLC_Device_CCD01_03_垂直基準線量測_量測高度.Value == 0)
            {
                this.PLC_Device_CCD01_03_垂直基準線量測_量測高度.Value = 100;
            }

            this.CCD01_03_垂直基準線量測_AxLineMsr.NLineStartX = PLC_Device_CCD01_03_垂直基準線量測_量測框起點X座標.Value;
            this.CCD01_03_垂直基準線量測_AxLineMsr.NLineStartY = PLC_Device_CCD01_03_垂直基準線量測_量測框起點Y座標.Value;
            this.CCD01_03_垂直基準線量測_AxLineMsr.NLineEndX = PLC_Device_CCD01_03_垂直基準線量測_量測框終點X座標.Value;
            this.CCD01_03_垂直基準線量測_AxLineMsr.NLineEndY = PLC_Device_CCD01_03_垂直基準線量測_量測框終點Y座標.Value;
            this.CCD01_03_垂直基準線量測_AxLineMsr.HalfHeight = PLC_Device_CCD01_03_垂直基準線量測_量測高度.Value;

            this.CCD01_03_垂直基準線量測_AxLineMsr.EdgeType = (AxOvkMsr.TxAxTransitionType)PLC_Device_CCD01_03_基準線量測_量測顏色變化.Value;
            this.CCD01_03_垂直基準線量測_AxLineMsr.LockedMsrDirection = AxOvkMsr.TxAxLineMsrLockedMsrDirection.AX_LINEMSR_LOCKED_CLOCKWISE;
            cnt++;

        }
        void cnt_Program_CCD01_03_基準線量測_開始量測(ref int cnt)
        {
            if (CCD01_03_SrcImageHandle != 0)
            {
                this.CCD01_03_水平基準線量測_AxLineMsr.DetectPrimitives();
                this.CCD01_03_垂直基準線量測_AxLineMsr.DetectPrimitives();
            }

            if (this.CCD01_03_水平基準線量測_AxLineMsr.LineIsFitted && this.CCD01_03_垂直基準線量測_AxLineMsr.LineIsFitted)
            {

                PointF 水平量測點p1 = new PointF();
                PointF 水平量測點p2 = new PointF();

                CCD01_03_水平基準線量測_AxLineMsr.ValidPointIndex = 0;
                水平量測點p1.X = (int)CCD01_03_水平基準線量測_AxLineMsr.ValidPointX;
                水平量測點p1.Y = (int)CCD01_03_水平基準線量測_AxLineMsr.ValidPointY;
                CCD01_03_水平基準線量測_AxLineMsr.ValidPointIndex = CCD01_03_水平基準線量測_AxLineMsr.ValidPointCount;
                水平量測點p2.X = (int)CCD01_03_水平基準線量測_AxLineMsr.ValidPointX;
                水平量測點p2.Y = (int)CCD01_03_水平基準線量測_AxLineMsr.ValidPointY;
                //Point_CCD01_03_中心基準座標_量測點.X = (int)((水平量測點p1.X + 水平量測點p2.X) / 2);
                //Point_CCD01_03_中心基準座標_量測點.Y = (int)((水平量測點p1.Y + 水平量測點p2.Y) / 2);

                PointF 水平p1 = new PointF();
                PointF 水平p2 = new PointF();
                double 水平confB;
                double 水平Slope = this.CCD01_03_水平基準線量測_AxLineMsr.MeasuredSlope;
                double 水平PivotX = this.CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotX;
                double 水平PivotY = this.CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotY;
                水平confB = Conf0Msr(水平Slope, 水平PivotX, 水平PivotY);
                水平p1.X = 1;
                水平p1.Y = (float)FunctionMsr_Y(水平confB, 水平Slope, 水平p1.X);
                水平p2.X = 10000;
                水平p2.Y = (float)FunctionMsr_Y(水平confB, 水平Slope, 水平p2.X);
                水平p1 = new PointF((水平p1.X), (水平p1.Y));
                水平p2 = new PointF((水平p2.X), (水平p2.Y));

                this.CCD01_03_水平基準線量測_AxLineRegression.RegressionOrientation = AxOvkMsr.TxAxLineRegressionOrientation.AX_QUASI_HORIZONTAL_REGRESSION;
                this.CCD01_03_水平基準線量測_AxLineRegression.PointIndex = 0;
                this.CCD01_03_水平基準線量測_AxLineRegression.PointX = 水平p1.X;
                this.CCD01_03_水平基準線量測_AxLineRegression.PointY = 水平p1.Y + (this.PLC_Device_CCD01_03_基準線量測_基準線偏移.Value / 1000) * CCD01_比例尺_mm_To_pixcel;
                this.CCD01_03_水平基準線量測_AxLineRegression.PointIndex = 1;
                this.CCD01_03_水平基準線量測_AxLineRegression.PointX = 水平p2.X;
                this.CCD01_03_水平基準線量測_AxLineRegression.PointY = 水平p2.Y + (this.PLC_Device_CCD01_03_基準線量測_基準線偏移.Value / 1000) * CCD01_比例尺_mm_To_pixcel;
                this.CCD01_03_水平基準線量測_AxLineRegression.DetectPrimitives();

                PointF 垂直p1 = new PointF();
                PointF 垂直p2 = new PointF();
                double 垂直confB;
                double 垂直Slope = this.CCD01_03_垂直基準線量測_AxLineMsr.MeasuredSlope;
                double 垂直PivotX = this.CCD01_03_垂直基準線量測_AxLineMsr.MeasuredPivotX;
                double 垂直PivotY = this.CCD01_03_垂直基準線量測_AxLineMsr.MeasuredPivotY;
                垂直confB = Conf0Msr(垂直Slope, 垂直PivotX, 垂直PivotY);
                垂直p1.X = (float)FunctionMsr_Y(垂直confB, 垂直Slope, 垂直p1.X);
                垂直p1.Y = 1;
                垂直p2.X = (float)FunctionMsr_Y(垂直confB, 垂直Slope, 垂直p2.X);
                垂直p2.Y = 10000;
                垂直p1 = new PointF((垂直p1.X), (垂直p1.Y));
                垂直p2 = new PointF((垂直p2.X), (垂直p2.Y));

                this.CCD01_03_垂直基準線量測_AxLineRegression.RegressionOrientation = AxOvkMsr.TxAxLineRegressionOrientation.AX_QUASI_VERTICAL_REGRESSION;
                this.CCD01_03_垂直基準線量測_AxLineRegression.PointIndex = 0;
                this.CCD01_03_垂直基準線量測_AxLineRegression.PointX = 垂直p1.X;
                this.CCD01_03_垂直基準線量測_AxLineRegression.PointY = 垂直p1.Y;
                this.CCD01_03_垂直基準線量測_AxLineRegression.PointIndex = 1;
                this.CCD01_03_垂直基準線量測_AxLineRegression.PointX = 垂直p2.X;
                this.CCD01_03_垂直基準線量測_AxLineRegression.PointY = 垂直p2.Y;
                this.CCD01_03_垂直基準線量測_AxLineRegression.DetectPrimitives();

                this.PLC_Device_CCD01_03_基準線量測_OK.Bool = true;
            }

            cnt++;
        }
        void cnt_Program_CCD01_03_基準線量測_兩線交點(ref int cnt)
        {
            CCD01_03_基準線量測_AxIntersectionMsr.Line1HorzVert = AxOvkMsr.TxAxLineHorzVert.AX_LINE_QUASI_HORIZONTAL;
            CCD01_03_基準線量測_AxIntersectionMsr.Line1PivotX = CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotX;
            CCD01_03_基準線量測_AxIntersectionMsr.Line1PivotY = CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotY;
            CCD01_03_基準線量測_AxIntersectionMsr.Line1Slope = CCD01_03_水平基準線量測_AxLineMsr.MeasuredSlope;

            CCD01_03_基準線量測_AxIntersectionMsr.Line2HorzVert = AxOvkMsr.TxAxLineHorzVert.AX_LINE_QUASI_VERTICAL;
            CCD01_03_基準線量測_AxIntersectionMsr.Line2PivotX = CCD01_03_垂直基準線量測_AxLineMsr.MeasuredPivotX;
            CCD01_03_基準線量測_AxIntersectionMsr.Line2PivotY = CCD01_03_垂直基準線量測_AxLineMsr.MeasuredPivotY;
            CCD01_03_基準線量測_AxIntersectionMsr.Line2Slope = CCD01_03_垂直基準線量測_AxLineMsr.MeasuredSlope;

            CCD01_03_基準線量測_AxIntersectionMsr.FindIntersection();

            cnt++;
        }
        void cnt_Program_CCD01_03_基準線量測_兩線交點量測(ref int cnt)
        {
            Point_CCD01_03_中心基準座標_量測點.X = (float)CCD01_03_基準線量測_AxIntersectionMsr.IntersectionX;
            Point_CCD01_03_中心基準座標_量測點.Y = (float)CCD01_03_基準線量測_AxIntersectionMsr.IntersectionY;

            if (!PLC_Device_CCD01_03_計算一次.Bool)
            {
                PLC_Device_CCD01_03_水平基準線量測_量測中心_X.Value = (int)CCD01_03_基準線量測_AxIntersectionMsr.IntersectionX;
                PLC_Device_CCD01_03_水平基準線量測_量測中心_Y.Value = (int)CCD01_03_基準線量測_AxIntersectionMsr.IntersectionY;
                //PLC_Device_CCD01_03_水平基準線量測_量測中心_X.Value = 2199;
                //PLC_Device_CCD01_03_水平基準線量測_量測中心_Y.Value = 1175;
            }

            cnt++;
        }
        void cnt_Program_CCD01_03_基準線量測_開始繪製(ref int cnt)
        {

            this.PLC_Device_CCD01_03_基準線量測_RefreshCanvas.Bool = true;
            if (this.PLC_Device_CCD01_03_基準線量測按鈕.Bool && !PLC_Device_CCD01_03_計算一次.Bool)
            {
                this.h_Canvas_Tech_CCD01_03.RefreshCanvas();
            }
            cnt++;
        }




        #endregion
        #region PLC_CCD01_03基準圓量測框調整

        private List<AxOvkBase.AxROIBW8> List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整 = new List<AxOvkBase.AxROIBW8>();
        private List<AxOvkBlob.AxObject> List_CCD01_03_基準圓量測_AxObject_區塊分析 = new List<AxOvkBlob.AxObject>();
        private AxOvkPat.AxVisionInspectionFrame CCD01_03基準圓AxVisionInspectionFrame_量測框調整;

        private PLC_Device PLC_Device_CCD01_03基準圓量測框調整按鈕 = new PLC_Device("S6610");
        private PLC_Device PLC_Device_CCD01_03基準圓量測框調整 = new PLC_Device("S6605");
        private PLC_Device PLC_Device_CCD01_03基準圓量測框調整_OK = new PLC_Device("S6606");
        private PLC_Device PLC_Device_CCD01_03基準圓量測框調整_測試完成 = new PLC_Device("S6607");
        private PLC_Device PLC_Device_CCD01_03基準圓量測框調整_RefreshCanvas = new PLC_Device("S6608");
        private List<PLC_Device> List_PLC_Device_CCD01_03_基準圓量測_灰階門檻值 = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_03_基準圓量測_CenterX = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_03_基準圓量測_CenterY = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_03_基準圓量測_Width = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_03_基準圓量測_Height = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_03_基準圓量測_面積上限 = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_03_基準圓量測_面積下限 = new List<PLC_Device>();


        private PLC_Device PLC_Device_CCD01_03_右基準圓量測_灰階門檻值 = new PLC_Device("F7010");
        private PLC_Device PLC_Device_CCD01_03_右基準圓量測_CenterX = new PLC_Device("F7011");
        private PLC_Device PLC_Device_CCD01_03_右基準圓量測_CenterY = new PLC_Device("F7012");
        private PLC_Device PLC_Device_CCD01_03_右基準圓量測_Width = new PLC_Device("F7013");
        private PLC_Device PLC_Device_CCD01_03_右基準圓量測_Height = new PLC_Device("F7014");

        private PLC_Device PLC_Device_CCD01_03_右基準圓量測_面積上限 = new PLC_Device("F7015");
        private PLC_Device PLC_Device_CCD01_03_右基準圓量測_面積下限 = new PLC_Device("F7016");

        private PLC_Device PLC_Device_CCD01_03_右基準圓量測_實際圓座標換算 = new PLC_Device("F7020");
        private float[] List_CCD01_03_基準圓_CenterX = new float[1];
        private float[] List_CCD01_03_基準圓_CenterY = new float[1];
        private float[] List_CCD01_03_基準圓_Radius = new float[1];
        private PointF[] List_CCD01_03_基準圓_量測點 = new PointF[1];
        private PointF[] List_CCD01_03_基準圓_量測點_結果 = new PointF[1];
        private Point[] List_CCD01_03_基準圓_量測點_轉換後座標 = new Point[1];
        private bool[] List_CCD01_03_基準圓_量測點_有無 = new bool[1];



        private void H_Canvas_Tech_CCD01_03基準圓量測框調整_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {

            if (PLC_Device_CCD01_03_Main_取像並檢驗.Bool || PLC_Device_CCD01_03_PLC觸發檢測.Bool || PLC_Device_CCD01_03_Main_檢驗一次.Bool || PLC_Device_CCD01_03_Main_檢驗一次.Bool)
            {
                if (this.PLC_Device_CCD01_03基準圓量測框調整_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);
                        for (int i = 0; i < 1; i++)
                        {
                            this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].ShowTitle = true;
                            this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[0].Title = "圓2";
                            this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].DrawFrame(HDC, ZoomX, ZoomY, 0, 0, 0x0000FF);
                        }
                        for (int i = 0; i < this.List_CCD01_03_基準圓_量測點.Length; i++)
                        {
                            DrawingClass.Draw.十字中心(this.List_CCD01_03_基準圓_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
                        }
                        //DrawingClass.Draw.線段繪製(List_CCD01_03_基準圓_量測點[0], List_CCD01_03_基準圓_量測點[1], Color.Lime, 1, g, ZoomX, ZoomY);
                        DrawingClass.Draw.文字中心繪製(圓柱相距長度.ToString("0.000"), new PointF(List_CCD01_03_基準圓_量測點[0].X , List_CCD01_03_基準圓_量測點[0].Y + 200),
                            font, Color.Black, Color.Lime, g, ZoomX, ZoomY);

                        if (PLC_Device_CCD01_03基準圓量測框調整_OK.Bool) DrawingClass.Draw.文字左上繪製("圓有無量測OK!", new PointF(0, 200), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        else DrawingClass.Draw.文字左上繪製("圓有無量測NG!", new PointF(0, 200), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);

                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }


            }
            else if (PLC_Device_CCD01_03_Tech_檢驗一次.Bool)
            {
                if (this.PLC_Device_CCD01_03基準圓量測框調整_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);
                        for (int i = 0; i < 1; i++)
                        {
                            this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].ShowTitle = true;
                            this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[0].Title = "圓2";
                            this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].DrawFrame(HDC, ZoomX, ZoomY, 0, 0, 0x0000FF);
                        }
                        for (int i = 0; i < this.List_CCD01_03_基準圓_量測點.Length; i++)
                        {
                            DrawingClass.Draw.十字中心(this.List_CCD01_03_基準圓_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
                        }
                        //DrawingClass.Draw.線段繪製(List_CCD01_03_基準圓_量測點[0], List_CCD01_03_基準圓_量測點[1], Color.Lime, 1, g, ZoomX, ZoomY);
                        DrawingClass.Draw.文字中心繪製(圓柱相距長度.ToString("0.000"), new PointF(List_CCD01_03_基準圓_量測點[0].X, List_CCD01_03_基準圓_量測點[0].Y + 200),
    font, Color.Black, Color.Lime, g, ZoomX, ZoomY);

                        if (PLC_Device_CCD01_03基準圓量測框調整_OK.Bool) DrawingClass.Draw.文字左上繪製("圓有無量測OK!", new PointF(0, 200), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        else DrawingClass.Draw.文字左上繪製("圓有無量測NG!", new PointF(0, 200), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);

                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }


            }
            else
            {
                if (this.PLC_Device_CCD01_03基準圓量測框調整_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);


                        if (this.plC_CheckBox_CCD01_03_基準圓繪製量測框.Checked)
                        {
                            for (int i = 0; i < 1; i++)
                            {
                                this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].ShowTitle = true;
                                this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[0].Title = "圓2";
                                this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].DrawFrame(HDC, ZoomX, ZoomY, 0, 0, 0x0000FF);
                            }
                        }
                        if (this.plC_CheckBox_CCD01_03_基準圓繪製量測區塊.Checked)
                        {
                            for (int i = 0; i < this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整.Count; i++)
                            {
                                this.List_CCD01_03_基準圓量測_AxObject_區塊分析[i].DrawBlobs(HDC, -1, ZoomX, ZoomY, 0, 0, true, -1);
                            }

                        }
                        for (int i = 0; i < this.List_CCD01_03_基準圓_量測點.Length; i++)
                        {
                            DrawingClass.Draw.十字中心(this.List_CCD01_03_基準圓_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
                        }
                        //DrawingClass.Draw.線段繪製(List_CCD01_03_基準圓_量測點[0], List_CCD01_03_基準圓_量測點[1], Color.Lime, 1, g, ZoomX, ZoomY);
                        DrawingClass.Draw.文字中心繪製(圓柱相距長度.ToString("0.000"), new PointF(List_CCD01_03_基準圓_量測點[0].X, List_CCD01_03_基準圓_量測點[0].Y + 200),
    font, Color.Black, Color.Lime, g, ZoomX, ZoomY);

                       if(PLC_Device_CCD01_03基準圓量測框調整_OK.Bool) DrawingClass.Draw.文字左上繪製("圓有無量測OK!", new PointF(0, 200), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                       else DrawingClass.Draw.文字左上繪製("圓有無量測NG!", new PointF(0, 200), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);

                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }
            }
            this.PLC_Device_CCD01_03基準圓量測框調整_RefreshCanvas.Bool = false;
        }

        AxOvkBase.TxAxHitHandle[] CCD01_03基準圓AxCircleROIBW8_TxAxCircleRoiHitHandle = new AxOvkBase.TxAxHitHandle[1];
        bool[] flag_CCD01_03基準圓AxCircleROIBW8_MouseDown = new bool[1];

        private void H_Canvas_Tech_CCD01_03基準圓量測框調整_OnCanvasMouseDownEvent(int x, int y, float ZoomX, float ZoomY, ref int InUsedEventNum, int InUsedCanvasHandle)
        {
            if (this.PLC_Device_CCD01_03基準圓量測框調整.Bool)
            {
                for (int i = 0; i < 1; i++)
                {
                    this.CCD01_03基準圓AxCircleROIBW8_TxAxCircleRoiHitHandle[i] = this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].HitTest(x, y, ZoomX, ZoomY, 0, 0);
                    if (this.CCD01_03基準圓AxCircleROIBW8_TxAxCircleRoiHitHandle[i] != AxOvkBase.TxAxHitHandle.AX_HANDLE_NONE)
                    {
                        this.flag_CCD01_03基準圓AxCircleROIBW8_MouseDown[i] = true;
                        InUsedEventNum = 10;
                    }

                }


            }

        }
        private void H_Canvas_Tech_CCD01_03基準圓量測框調整_OnCanvasMouseMoveEvent(int x, int y, float ZoomX, float ZoomY)
        {
            for (int i = 0; i < 1; i++)
            {
                if (this.flag_CCD01_03基準圓AxCircleROIBW8_MouseDown[i])
                {
                    this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].DragROI(this.CCD01_03基準圓AxCircleROIBW8_TxAxCircleRoiHitHandle[i], x, y, ZoomX, ZoomY, 0, 0);
                    List_PLC_Device_CCD01_03_基準圓量測_CenterX[i].Value = (int)this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].OrgX;
                    List_PLC_Device_CCD01_03_基準圓量測_CenterY[i].Value = (int)this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].OrgY;
                    List_PLC_Device_CCD01_03_基準圓量測_Height[i].Value = (int)this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].ROIHeight;
                    List_PLC_Device_CCD01_03_基準圓量測_Width[i].Value = (int)this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].ROIWidth;

                }
            }

        }
        private void H_Canvas_Tech_CCD01_03基準圓量測框調整_OnCanvasMouseUpEvent(int x, int y, float ZoomX, float ZoomY)
        {
            for (int i = 0; i < 1; i++)
            {
                this.flag_CCD01_03基準圓AxCircleROIBW8_MouseDown[i] = false;
            }
        }

        int cnt_Program_CCD01_03基準圓量測框調整 = 65534;
        void sub_Program_CCD01_03基準圓量測框調整()
        {
            if (cnt_Program_CCD01_03基準圓量測框調整 == 65534)
            {
                this.h_Canvas_Tech_CCD01_03.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_03基準圓量測框調整_OnCanvasDrawEvent;
                this.h_Canvas_Tech_CCD01_03.OnCanvasMouseDownEvent += H_Canvas_Tech_CCD01_03基準圓量測框調整_OnCanvasMouseDownEvent;
                this.h_Canvas_Tech_CCD01_03.OnCanvasMouseMoveEvent += H_Canvas_Tech_CCD01_03基準圓量測框調整_OnCanvasMouseMoveEvent;
                this.h_Canvas_Tech_CCD01_03.OnCanvasMouseUpEvent += H_Canvas_Tech_CCD01_03基準圓量測框調整_OnCanvasMouseUpEvent;

                this.h_Canvas_Main_CCD01_03_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_03基準圓量測框調整_OnCanvasDrawEvent;

                #region list add

                this.List_PLC_Device_CCD01_03_基準圓量測_灰階門檻值.Add(this.PLC_Device_CCD01_03_右基準圓量測_灰階門檻值);
                this.List_PLC_Device_CCD01_03_基準圓量測_CenterX.Add(this.PLC_Device_CCD01_03_右基準圓量測_CenterX);
                this.List_PLC_Device_CCD01_03_基準圓量測_CenterY.Add(this.PLC_Device_CCD01_03_右基準圓量測_CenterY);
                this.List_PLC_Device_CCD01_03_基準圓量測_Width.Add(this.PLC_Device_CCD01_03_右基準圓量測_Width);
                this.List_PLC_Device_CCD01_03_基準圓量測_Height.Add(this.PLC_Device_CCD01_03_右基準圓量測_Height);
                this.List_PLC_Device_CCD01_03_基準圓量測_面積上限.Add(this.PLC_Device_CCD01_03_右基準圓量測_面積上限);
                this.List_PLC_Device_CCD01_03_基準圓量測_面積下限.Add(this.PLC_Device_CCD01_03_右基準圓量測_面積下限);
                #endregion
                for (int i = 0; i < 1; i++)
                {
                    if (this.List_PLC_Device_CCD01_03_基準圓量測_灰階門檻值[i].Value == 0) this.List_PLC_Device_CCD01_03_基準圓量測_灰階門檻值[i].Value = 200;
                    if (this.List_PLC_Device_CCD01_03_基準圓量測_Height[i].Value == 0) this.List_PLC_Device_CCD01_03_基準圓量測_Height[i].Value = 150;
                    if (this.List_PLC_Device_CCD01_03_基準圓量測_Width[i].Value == 0) this.List_PLC_Device_CCD01_03_基準圓量測_Width[i].Value = 150;

                }

                PLC_Device_CCD01_03基準圓量測框調整.SetComment("PLC_CCD01_03基準圓量測框調整");
                PLC_Device_CCD01_03基準圓量測框調整.Bool = false;
                PLC_Device_CCD01_03基準圓量測框調整按鈕.Bool = false;
                cnt_Program_CCD01_03基準圓量測框調整 = 65535;
            }
            if (cnt_Program_CCD01_03基準圓量測框調整 == 65535) cnt_Program_CCD01_03基準圓量測框調整 = 1;
            if (cnt_Program_CCD01_03基準圓量測框調整 == 1) cnt_Program_CCD01_03基準圓量測框調整_觸發按下(ref cnt_Program_CCD01_03基準圓量測框調整);
            if (cnt_Program_CCD01_03基準圓量測框調整 == 2) cnt_Program_CCD01_03基準圓量測框調整_檢查按下(ref cnt_Program_CCD01_03基準圓量測框調整);
            if (cnt_Program_CCD01_03基準圓量測框調整 == 3) cnt_Program_CCD01_03基準圓量測框調整_初始化(ref cnt_Program_CCD01_03基準圓量測框調整);
            if (cnt_Program_CCD01_03基準圓量測框調整 == 4) cnt_Program_CCD01_03基準圓量測框調整_座標轉換(ref cnt_Program_CCD01_03基準圓量測框調整);
            if (cnt_Program_CCD01_03基準圓量測框調整 == 5) cnt_Program_CCD01_03基準圓量測框調整_讀取參數(ref cnt_Program_CCD01_03基準圓量測框調整);
            if (cnt_Program_CCD01_03基準圓量測框調整 == 6) cnt_Program_CCD01_03基準圓量測框調整_開始區塊分析(ref cnt_Program_CCD01_03基準圓量測框調整);
            if (cnt_Program_CCD01_03基準圓量測框調整 == 7) cnt_Program_CCD01_03基準圓量測框調整_圓柱間距量測(ref cnt_Program_CCD01_03基準圓量測框調整);
            if (cnt_Program_CCD01_03基準圓量測框調整 == 8) cnt_Program_CCD01_03基準圓量測框調整_繪製畫布(ref cnt_Program_CCD01_03基準圓量測框調整);
            if (cnt_Program_CCD01_03基準圓量測框調整 == 9) cnt_Program_CCD01_03基準圓量測框調整 = 65500;
            if (cnt_Program_CCD01_03基準圓量測框調整 > 1) cnt_Program_CCD01_03基準圓量測框調整_檢查放開(ref cnt_Program_CCD01_03基準圓量測框調整);

            if (cnt_Program_CCD01_03基準圓量測框調整 == 65500)
            {
                if (PLC_Device_CCD01_03_計算一次.Bool)
                {
                    PLC_Device_CCD01_03基準圓量測框調整按鈕.Bool = false;
                }
                PLC_Device_CCD01_03基準圓量測框調整.Bool = false;
                cnt_Program_CCD01_03基準圓量測框調整 = 65535;
            }
        }
        void cnt_Program_CCD01_03基準圓量測框調整_觸發按下(ref int cnt)
        {
            if (PLC_Device_CCD01_03基準圓量測框調整按鈕.Bool)
            {
                PLC_Device_CCD01_03基準圓量測框調整.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_03基準圓量測框調整_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_03基準圓量測框調整.Bool)
            {
                cnt++;
            }

        }
        void cnt_Program_CCD01_03基準圓量測框調整_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_03基準圓量測框調整按鈕.Bool)
            {
                PLC_Device_CCD01_03基準圓量測框調整.Bool = false;
                cnt = 65500;
            }
        }
        void cnt_Program_CCD01_03基準圓量測框調整_初始化(ref int cnt)
        {
            this.List_CCD01_03_基準圓_量測點 = new PointF[1];
            this.List_CCD01_03_基準圓_量測點_結果 = new PointF[1];
            this.List_CCD01_03_基準圓_量測點_轉換後座標 = new Point[1];
            this.List_CCD01_03_基準圓_量測點_有無 = new bool[1];
            this.CCD01_03_SrcImageHandle = h_Canvas_Tech_CCD01_03.VegaHandle;
            this.PLC_Device_CCD01_03基準圓量測框調整_OK.Bool = false;
            cnt++;
        }
        void cnt_Program_CCD01_03基準圓量測框調整_座標轉換(ref int cnt)
        {
            if (PLC_Device_CCD01_03_計算一次.Bool)
            {
                CCD01_03基準圓AxVisionInspectionFrame_量測框調整.RefPointX = PLC_Device_CCD01_03_水平基準線量測_量測中心_X.Value;
                CCD01_03基準圓AxVisionInspectionFrame_量測框調整.RefPointY = PLC_Device_CCD01_03_水平基準線量測_量測中心_Y.Value;
                CCD01_03基準圓AxVisionInspectionFrame_量測框調整.RefAngle = 0;
                CCD01_03基準圓AxVisionInspectionFrame_量測框調整.CurrentRefPointX = Point_CCD01_03_中心基準座標_量測點.X;
                CCD01_03基準圓AxVisionInspectionFrame_量測框調整.CurrentRefPointY = Point_CCD01_03_中心基準座標_量測點.Y;
                CCD01_03基準圓AxVisionInspectionFrame_量測框調整.CurrentRefAngle = 0;
                CCD01_03基準圓AxVisionInspectionFrame_量測框調整.NumOfVisionPoints = 1;

                for (int j = 0; j < 1; j++)
                {
                    if (this.List_PLC_Device_CCD01_03_基準圓量測_CenterX[j].Value == 0) this.List_PLC_Device_CCD01_03_基準圓量測_CenterX[j].Value = 100;
                    if (this.List_PLC_Device_CCD01_03_基準圓量測_CenterY[j].Value == 0) this.List_PLC_Device_CCD01_03_基準圓量測_CenterY[j].Value = 100;
                    if (this.List_PLC_Device_CCD01_03_基準圓量測_Width[j].Value == 0) this.List_PLC_Device_CCD01_03_基準圓量測_Width[j].Value = 100;
                    if (this.List_PLC_Device_CCD01_03_基準圓量測_Height[j].Value == 0) this.List_PLC_Device_CCD01_03_基準圓量測_Height[j].Value = 100;

                    CCD01_03基準圓AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
                    CCD01_03基準圓AxVisionInspectionFrame_量測框調整.VisionPointX = this.List_PLC_Device_CCD01_03_基準圓量測_CenterX[j].Value;
                    CCD01_03基準圓AxVisionInspectionFrame_量測框調整.VisionPointY = this.List_PLC_Device_CCD01_03_基準圓量測_CenterY[j].Value;
                }
                CCD01_03基準圓AxVisionInspectionFrame_量測框調整.EstimateCurrentVisionPoints();
                for (int j = 0; j < 1; j++)
                {
                    CCD01_03基準圓AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
                    List_CCD01_03_基準圓_量測點_轉換後座標[j].X = (int)CCD01_03基準圓AxVisionInspectionFrame_量測框調整.CurrentVisionPointX;
                    List_CCD01_03_基準圓_量測點_轉換後座標[j].Y = (int)CCD01_03基準圓AxVisionInspectionFrame_量測框調整.CurrentVisionPointY;
                }
            }
            cnt++;

        }
        void cnt_Program_CCD01_03基準圓量測框調整_讀取參數(ref int cnt)
        {
            for (int i = 0; i < this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整.Count; i++)
            {
                if (this.List_PLC_Device_CCD01_03_基準圓量測_CenterX[i].Value > 2596) this.List_PLC_Device_CCD01_03_基準圓量測_CenterX[i].Value = 0;
                if (this.List_PLC_Device_CCD01_03_基準圓量測_CenterY[i].Value > 1922) this.List_PLC_Device_CCD01_03_基準圓量測_CenterY[i].Value = 0;
                if (this.List_PLC_Device_CCD01_03_基準圓量測_CenterX[i].Value < 0) this.List_PLC_Device_CCD01_03_基準圓量測_CenterX[i].Value = 0;
                if (this.List_PLC_Device_CCD01_03_基準圓量測_CenterY[i].Value < 0) this.List_PLC_Device_CCD01_03_基準圓量測_CenterY[i].Value = 0;

                if (this.List_CCD01_03_基準圓_量測點_轉換後座標[i].X > 2596) this.List_CCD01_03_基準圓_量測點_轉換後座標[i].X = 0;
                if (this.List_CCD01_03_基準圓_量測點_轉換後座標[i].Y > 1922) this.List_CCD01_03_基準圓_量測點_轉換後座標[i].Y = 0;
                if (this.List_CCD01_03_基準圓_量測點_轉換後座標[i].X < 0) this.List_CCD01_03_基準圓_量測點_轉換後座標[i].X = 0;
                if (this.List_CCD01_03_基準圓_量測點_轉換後座標[i].Y < 0) this.List_CCD01_03_基準圓_量測點_轉換後座標[i].Y = 0;

                this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].ParentHandle = this.CCD01_03_SrcImageHandle;

                if (PLC_Device_CCD01_03_計算一次.Bool)
                {
                    this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].OrgX = List_CCD01_03_基準圓_量測點_轉換後座標[i].X;
                    this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].OrgY = List_CCD01_03_基準圓_量測點_轉換後座標[i].Y;
                    this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].ROIWidth = List_PLC_Device_CCD01_03_基準圓量測_Width[i].Value;
                    this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].ROIHeight = List_PLC_Device_CCD01_03_基準圓量測_Height[i].Value;
                }
                else
                {
                    this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].OrgX = List_PLC_Device_CCD01_03_基準圓量測_CenterX[i].Value;
                    this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].OrgY = List_PLC_Device_CCD01_03_基準圓量測_CenterY[i].Value;
                    this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].ROIWidth = List_PLC_Device_CCD01_03_基準圓量測_Width[i].Value;
                    this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].ROIHeight = List_PLC_Device_CCD01_03_基準圓量測_Height[i].Value;

                }


            }
            cnt++;
        }
        void cnt_Program_CCD01_03基準圓量測框調整_開始區塊分析(ref int cnt)
        {
            uint object_value = 4294963615;

            for (int i = 0; i < 1; i++)
            {

                this.List_CCD01_03_基準圓量測_AxObject_區塊分析[i].SrcImageHandle = this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].VegaHandle;
                this.AxMatch_CCD01_03_圓柱相似度測試.SrcImageHandle = this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].VegaHandle;
                this.List_CCD01_03_基準圓量測_AxObject_區塊分析[i].ObjectClass = AxOvkBlob.TxAxObjClass.AX_OBJECT_DETECT_LIGHTER_CLASS;
                this.List_CCD01_03_基準圓量測_AxObject_區塊分析[i].HighThreshold = List_PLC_Device_CCD01_03_基準圓量測_灰階門檻值[i].Value;

                if (this.CCD01_03_SrcImageHandle != 0)
                {
                    this.List_CCD01_03_基準圓量測_AxObject_區塊分析[i].BlobAnalyze(false);

                }


                this.List_CCD01_03_基準圓量測_AxObject_區塊分析[i].CalculateFeatures((int)object_value, -1);
                this.List_CCD01_03_基準圓量測_AxObject_區塊分析[i].SortObjects(AxOvkBlob.TxAxObjFeatureSortOrder.AX_OBJECT_SORT_ORDER_LARGE_TO_SMALL, AxOvkBlob.TxAxObjFeature.AX_OBJECT_FEATURE_AREA, 0, -1);
                this.List_CCD01_03_基準圓量測_AxObject_區塊分析[i].SelectObjects(AxOvkBlob.TxAxObjFeature.AX_OBJECT_FEATURE_AREA, AxOvkBlob.TxAxObjFeatureOperation.AX_OBJECT_REMOVE_LESS_THAN, this.List_PLC_Device_CCD01_03_基準圓量測_面積下限[i].Value);
                this.List_CCD01_03_基準圓量測_AxObject_區塊分析[i].SelectObjects(AxOvkBlob.TxAxObjFeature.AX_OBJECT_FEATURE_AREA, AxOvkBlob.TxAxObjFeatureOperation.AX_OBJECT_REMOVE_GREAT_THAN, this.List_PLC_Device_CCD01_03_基準圓量測_面積上限[i].Value);
                if (this.List_CCD01_03_基準圓量測_AxObject_區塊分析[i].DetectedNumObjs > 0)
                {
                    this.List_CCD01_03_基準圓_量測點_有無[i] = true;
                    this.PLC_Device_CCD01_03基準圓量測框調整_OK.Bool = true;
                    this.List_CCD01_03_基準圓量測_AxObject_區塊分析[i].BlobIndex = 0;
                    this.List_CCD01_03_基準圓_量測點[i].X = (float)this.List_CCD01_03_基準圓量測_AxObject_區塊分析[i].BlobCentroidX;
                    this.List_CCD01_03_基準圓_量測點[i].X += this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].OrgX;
                    this.List_CCD01_03_基準圓_量測點[i].Y = (float)this.List_CCD01_03_基準圓量測_AxObject_區塊分析[i].BlobCentroidY;
                    //this.List_CCD01_03_基準圓量測_量測點[i].Y = (float)this.List_CCD01_03基準圓AxObject_區塊分析[i].BlobCentroidY - (float)this.List_CCD01_03基準圓AxObject_區塊分析[i].BlobLimBoxHeight / 2;
                    this.List_CCD01_03_基準圓_量測點[i].Y += this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整[i].OrgY;
                }


            }



            cnt++;
        }
        void cnt_Program_CCD01_03基準圓量測框調整_圓柱間距量測(ref int cnt)
        {
            double x = this.List_CCD01_01_基準圓_量測點[0].X - this.List_CCD01_03_基準圓_量測點[0].X;
            double y = this.List_CCD01_01_基準圓_量測點[0].Y - this.List_CCD01_03_基準圓_量測點[0].Y;
            double temp1 = Math.Pow(x, 2);
            double temp2 = Math.Pow(y, 2);
            double reslut = temp1 + temp2;

            圓柱相距長度 = (Math.Sqrt(reslut) * CCD01_比例尺_pixcel_To_mm) + PLC_Device_CCD01_03_右基準圓量測_實際圓座標換算.Value;


            cnt++;
        }
        void cnt_Program_CCD01_03基準圓量測框調整_繪製畫布(ref int cnt)
        {
            this.PLC_Device_CCD01_03基準圓量測框調整_RefreshCanvas.Bool = true;
            if (this.PLC_Device_CCD01_03基準圓量測框調整按鈕.Bool && !PLC_Device_CCD01_03_計算一次.Bool)
            {
                this.h_Canvas_Tech_CCD01_03.RefreshCanvas();
            }

            cnt++;
        }





        #endregion
        #region PLC_CCD01_03圓直徑量測

        private AxOvkMsr.AxGapMsr CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整 = new AxOvkMsr.AxGapMsr();
        private AxOvkPat.AxVisionInspectionFrame CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整;

        private PLC_Device PLC_Device_CCD01_03圓直徑量測按鈕 = new PLC_Device("S6550");
        private PLC_Device PLC_Device_CCD01_03圓直徑量測 = new PLC_Device("S6551");
        private PLC_Device PLC_Device_CCD01_03圓直徑量測_OK = new PLC_Device("S6552");
        private PLC_Device PLC_Device_CCD01_03圓直徑量測_測試完成 = new PLC_Device("S6553");
        private PLC_Device PLC_Device_CCD01_03圓直徑量測_RefreshCanvas = new PLC_Device("S6554");

        private PLC_Device PLC_Device_CCD01_03圓直徑量測_起點變化銳利度門檻 = new PLC_Device("F7100");
        private PLC_Device PLC_Device_CCD01_03圓直徑量測_起點延伸變化強度門檻 = new PLC_Device("F7101");
        private PLC_Device PLC_Device_CCD01_03圓直徑量測_起點灰階面化面積門檻 = new PLC_Device("F7102");
        private PLC_Device PLC_Device_CCD01_03圓直徑量測_起點垂直量測寬度 = new PLC_Device("F7103");
        private PLC_Device PLC_Device_CCD01_03圓直徑量測_起點雜訊抑制 = new PLC_Device("F7104");
        private PLC_Device PLC_Device_CCD01_03圓直徑量測_起點量測顏色變化 = new PLC_Device("F7105");
        private PLC_Device PLC_Device_CCD01_03圓直徑量測_起點量測方向 = new PLC_Device("F7106");


        private PLC_Device PLC_Device_CCD01_03圓直徑量測_終點變化銳利度門檻 = new PLC_Device("F7110");
        private PLC_Device PLC_Device_CCD01_03圓直徑量測_終點延伸變化強度門檻 = new PLC_Device("F7111");
        private PLC_Device PLC_Device_CCD01_03圓直徑量測_終點灰階面化面積門檻 = new PLC_Device("F7112");
        private PLC_Device PLC_Device_CCD01_03圓直徑量測_終點垂直量測寬度 = new PLC_Device("F7113");
        private PLC_Device PLC_Device_CCD01_03圓直徑量測_終點雜訊抑制 = new PLC_Device("F7114");
        private PLC_Device PLC_Device_CCD01_03圓直徑量測_終點量測顏色變化 = new PLC_Device("F7115");
        private PLC_Device PLC_Device_CCD01_03圓直徑量測_終點量測方向 = new PLC_Device("F7116");

        private PLC_Device PLC_Device_CCD01_03圓直徑量測_ProbeCenterX = new PLC_Device("F7120");
        private PLC_Device PLC_Device_CCD01_03圓直徑量測_ProbeCenterY = new PLC_Device("F7121");
        private PLC_Device PLC_Device_CCD01_03圓直徑量測_StartTipX = new PLC_Device("F7122");
        private PLC_Device PLC_Device_CCD01_03圓直徑量測_StartTipY = new PLC_Device("F7123");
        private PLC_Device PLC_Device_CCD01_03圓直徑量測_EndTipX = new PLC_Device("F7124");
        private PLC_Device PLC_Device_CCD01_03圓直徑量測_EndTipY = new PLC_Device("F7125");



        private PLC_Device PLC_Device_CCD01_03_基準圓直徑_量測上限值 = new PLC_Device("F7130");
        private PLC_Device PLC_Device_CCD01_03_基準圓直徑_量測標準值 = new PLC_Device("F7131");
        private PLC_Device PLC_Device_CCD01_03_基準圓直徑_量測下限值 = new PLC_Device("F7132");

        private float CCD01_03_基準圓直徑_CenterX = new float();
        private float CCD01_03_基準圓直徑_CenterY = new float();
        private double CCD01_03_基準圓直徑_Radius = new float();
        private Point CCD01_03_基準圓直徑_轉換後座標_ProbeCenter = new Point();
        private Point CCD01_03_基準圓直徑_轉換後座標_StartTip = new Point();
        private Point CCD01_03_基準圓直徑_轉換後座標_EndTip = new Point();


        private bool CCD01_03_基準圓直徑_量測OK = new bool();


        private void H_Canvas_Tech_CCD01_03圓直徑量測_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {

            if (PLC_Device_CCD01_03_Main_取像並檢驗.Bool || PLC_Device_CCD01_03_PLC觸發檢測.Bool || PLC_Device_CCD01_03_Main_檢驗一次.Bool)
            {
                if (this.PLC_Device_CCD01_03圓直徑量測_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);
                        if (PLC_Device_CCD01_03圓直徑量測_OK.Bool)
                        {

                            DrawingClass.Draw.文字左上繪製(CCD01_03_基準圓直徑_Radius.ToString(),
                                new PointF(CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.ProbeCenterX, CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.ProbeCenterY + 50), new Font("標楷體", 12), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            DrawingClass.Draw.文字左上繪製("圓直徑量測OK!", new PointF(0, 300), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            if (CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.GapIsFitted == true)
                            {
                                DrawingClass.Draw.十字中心(new PointF((float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredStartTipX, (float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredStartTipY), 50, Color.Lime, 2, g, ZoomX, ZoomY);
                                DrawingClass.Draw.十字中心(new PointF((float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredEndTipX, (float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredEndTipY), 50, Color.Lime, 2, g, ZoomX, ZoomY);
                            }
                        }
                        else
                        {
                            DrawingClass.Draw.文字左上繪製(CCD01_03_基準圓直徑_Radius.ToString(),
                                new PointF(CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.ProbeCenterX, CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.ProbeCenterY + 50), new Font("標楷體", 12), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            DrawingClass.Draw.文字左上繪製("圓直徑量測NG!", new PointF(0, 300), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            if (CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.GapIsFitted == true)
                            {
                                DrawingClass.Draw.十字中心(new PointF((float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredStartTipX, (float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredStartTipY), 50, Color.Lime, 2, g, ZoomX, ZoomY);
                                DrawingClass.Draw.十字中心(new PointF((float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredEndTipX, (float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredEndTipY), 50, Color.Lime, 2, g, ZoomX, ZoomY);

                            }
                        }
                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }


            }
            else if (PLC_Device_CCD01_03_Tech_檢驗一次.Bool)
            {
                if (this.PLC_Device_CCD01_03圓直徑量測_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);

                        if (PLC_Device_CCD01_03圓直徑量測_OK.Bool)
                        {

                            DrawingClass.Draw.文字左上繪製(CCD01_03_基準圓直徑_Radius.ToString(),
                                new PointF(CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.ProbeCenterX, CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.ProbeCenterY + 50), new Font("標楷體", 12), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            DrawingClass.Draw.文字左上繪製("圓直徑量測OK!", new PointF(0, 300), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            if (CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.GapIsFitted == true)
                            {
                                DrawingClass.Draw.十字中心(new PointF((float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredStartTipX, (float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredStartTipY), 50, Color.Lime, 2, g, ZoomX, ZoomY);
                                DrawingClass.Draw.十字中心(new PointF((float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredEndTipX, (float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredEndTipY), 50, Color.Lime, 2, g, ZoomX, ZoomY);
                            }
                        }
                        else
                        {
                            DrawingClass.Draw.文字左上繪製(CCD01_03_基準圓直徑_Radius.ToString(),
                                new PointF(CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.ProbeCenterX, CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.ProbeCenterY + 50), new Font("標楷體", 12), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            DrawingClass.Draw.文字左上繪製("圓直徑量測NG!", new PointF(0, 300), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            if (CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.GapIsFitted == true)
                            {
                                DrawingClass.Draw.十字中心(new PointF((float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredStartTipX, (float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredStartTipY), 50, Color.Lime, 2, g, ZoomX, ZoomY);
                                DrawingClass.Draw.十字中心(new PointF((float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredEndTipX, (float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredEndTipY), 50, Color.Lime, 2, g, ZoomX, ZoomY);

                            }
                        }
                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }


            }
            else
            {
                if (this.PLC_Device_CCD01_03圓直徑量測_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);


                        //for (int i = 0; i < this.List_CCD01_03_基準圓_量測點.Length; i++)
                        //{
                        //    DrawingClass.Draw.十字中心(this.List_CCD01_03_基準圓_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
                        //}

                        if (plC_CheckBox_CCD01_03基準圓直徑繪製量測框.Checked)
                        {
                            this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.DrawFrame(HDC, ZoomX, ZoomY, 0, 0);
                        }
                        if (plC_CheckBox_CCD01_03基準圓直徑繪製量測點.Checked)
                        {
                            this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.DrawFittedPrimitives(HDC, ZoomX, ZoomY, 0, 0);
                        }

                        if (PLC_Device_CCD01_03圓直徑量測_OK.Bool)
                        {

                            DrawingClass.Draw.文字左上繪製(CCD01_03_基準圓直徑_Radius.ToString(),
                                new PointF(CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.ProbeCenterX, CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.ProbeCenterY + 50), new Font("標楷體", 12), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            DrawingClass.Draw.文字左上繪製("圓直徑量測OK!", new PointF(0, 300), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            if (CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.GapIsFitted == true)
                            {
                                DrawingClass.Draw.十字中心(new PointF((float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredStartTipX, (float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredStartTipY), 50, Color.Lime, 2, g, ZoomX, ZoomY);
                                DrawingClass.Draw.十字中心(new PointF((float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredEndTipX, (float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredEndTipY), 50, Color.Lime, 2, g, ZoomX, ZoomY);
                            }
                        }
                        else
                        {
                            DrawingClass.Draw.文字左上繪製(CCD01_03_基準圓直徑_Radius.ToString(),
                                new PointF(CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.ProbeCenterX, CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.ProbeCenterY + 50), new Font("標楷體", 12), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            DrawingClass.Draw.文字左上繪製("圓直徑量測NG!", new PointF(0, 300), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            if (CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.GapIsFitted == true)
                            {
                                DrawingClass.Draw.十字中心(new PointF((float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredStartTipX, (float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredStartTipY), 50, Color.Lime, 2, g, ZoomX, ZoomY);
                                DrawingClass.Draw.十字中心(new PointF((float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredEndTipX, (float)this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredEndTipY), 50, Color.Lime, 2, g, ZoomX, ZoomY);

                            }
                        }

                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }
            }

            this.PLC_Device_CCD01_03圓直徑量測_RefreshCanvas.Bool = false;
        }

        private AxOvkMsr.TxAxGapMsrDragHandle CCD01_03基準圓直徑AxOvkMsr_TxAxGapMsrDragHandle = new AxOvkMsr.TxAxGapMsrDragHandle();
        bool flag_CCD01_03基準圓直徑AxGapMsr_MouseDown = new bool();

        private void H_Canvas_Tech_CCD01_03圓直徑量測_OnCanvasMouseDownEvent(int x, int y, float ZoomX, float ZoomY, ref int InUsedEventNum, int InUsedCanvasHandle)
        {
            if (this.PLC_Device_CCD01_03圓直徑量測.Bool)
            {
                this.CCD01_03基準圓直徑AxOvkMsr_TxAxGapMsrDragHandle = this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.HitTest(x, y, ZoomX, ZoomY, 0, 0);
                if (this.CCD01_03基準圓直徑AxOvkMsr_TxAxGapMsrDragHandle != AxOvkMsr.TxAxGapMsrDragHandle.AX_GAP_MSR_NONE)
                {
                    this.flag_CCD01_03基準圓直徑AxGapMsr_MouseDown = true;
                    InUsedEventNum = 10;
                }
            }

        }
        private void H_Canvas_Tech_CCD01_03圓直徑量測_OnCanvasMouseMoveEvent(int x, int y, float ZoomX, float ZoomY)
        {

            if (this.flag_CCD01_03基準圓直徑AxGapMsr_MouseDown)
            {
                this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.DragFrame(this.CCD01_03基準圓直徑AxOvkMsr_TxAxGapMsrDragHandle, x, y, ZoomX, ZoomY, 0, 0);
                this.PLC_Device_CCD01_03圓直徑量測_ProbeCenterX.Value = this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.ProbeCenterX;
                this.PLC_Device_CCD01_03圓直徑量測_ProbeCenterY.Value = this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.ProbeCenterY;
                this.PLC_Device_CCD01_03圓直徑量測_StartTipX.Value = this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.StartTipX;
                this.PLC_Device_CCD01_03圓直徑量測_StartTipY.Value = this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.StartTipY;
                this.PLC_Device_CCD01_03圓直徑量測_EndTipX.Value = this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.EndTipX;
                this.PLC_Device_CCD01_03圓直徑量測_EndTipY.Value = this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.EndTipY;

            }


        }
        private void H_Canvas_Tech_CCD01_03圓直徑量測_OnCanvasMouseUpEvent(int x, int y, float ZoomX, float ZoomY)
        {
            this.flag_CCD01_03基準圓直徑AxGapMsr_MouseDown = false;
        }

        int cnt_Program_CCD01_03圓直徑量測 = 65534;
        void sub_Program_CCD01_03圓直徑量測()
        {
            if (cnt_Program_CCD01_03圓直徑量測 == 65534)
            {
                this.h_Canvas_Tech_CCD01_03.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_03圓直徑量測_OnCanvasDrawEvent;
                this.h_Canvas_Tech_CCD01_03.OnCanvasMouseDownEvent += H_Canvas_Tech_CCD01_03圓直徑量測_OnCanvasMouseDownEvent;
                this.h_Canvas_Tech_CCD01_03.OnCanvasMouseMoveEvent += H_Canvas_Tech_CCD01_03圓直徑量測_OnCanvasMouseMoveEvent;
                this.h_Canvas_Tech_CCD01_03.OnCanvasMouseUpEvent += H_Canvas_Tech_CCD01_03圓直徑量測_OnCanvasMouseUpEvent;

                this.h_Canvas_Main_CCD01_03_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_03圓直徑量測_OnCanvasDrawEvent;
                if (PLC_Device_CCD01_03圓直徑量測_ProbeCenterX.Value == 0) PLC_Device_CCD01_03圓直徑量測_ProbeCenterX.Value = 100;
                if (PLC_Device_CCD01_03圓直徑量測_ProbeCenterY.Value == 0) PLC_Device_CCD01_03圓直徑量測_ProbeCenterY.Value = 100;
                if (PLC_Device_CCD01_03圓直徑量測_StartTipX.Value == 0) PLC_Device_CCD01_03圓直徑量測_StartTipX.Value = 200;
                if (PLC_Device_CCD01_03圓直徑量測_StartTipY.Value == 0) PLC_Device_CCD01_03圓直徑量測_StartTipY.Value = 100;
                if (PLC_Device_CCD01_03圓直徑量測_EndTipX.Value == 0) PLC_Device_CCD01_03圓直徑量測_EndTipX.Value = 150;
                if (PLC_Device_CCD01_03圓直徑量測_EndTipY.Value == 0) PLC_Device_CCD01_03圓直徑量測_EndTipY.Value = 100;

                PLC_Device_CCD01_03圓直徑量測.SetComment("PLC_CCD01_03圓直徑量測");
                PLC_Device_CCD01_03圓直徑量測.Bool = false;
                PLC_Device_CCD01_03圓直徑量測按鈕.Bool = false;
                cnt_Program_CCD01_03圓直徑量測 = 65535;
            }
            if (cnt_Program_CCD01_03圓直徑量測 == 65535) cnt_Program_CCD01_03圓直徑量測 = 1;
            if (cnt_Program_CCD01_03圓直徑量測 == 1) cnt_Program_CCD01_03圓直徑量測_檢查按下(ref cnt_Program_CCD01_03圓直徑量測);
            if (cnt_Program_CCD01_03圓直徑量測 == 2) cnt_Program_CCD01_03圓直徑量測_初始化(ref cnt_Program_CCD01_03圓直徑量測);
            if (cnt_Program_CCD01_03圓直徑量測 == 3) cnt_Program_CCD01_03圓直徑量測_座標轉換(ref cnt_Program_CCD01_03圓直徑量測);
            if (cnt_Program_CCD01_03圓直徑量測 == 4) cnt_Program_CCD01_03圓直徑量測_讀取參數(ref cnt_Program_CCD01_03圓直徑量測);
            if (cnt_Program_CCD01_03圓直徑量測 == 5) cnt_Program_CCD01_03圓直徑量測_開始量測(ref cnt_Program_CCD01_03圓直徑量測);
            if (cnt_Program_CCD01_03圓直徑量測 == 6) cnt_Program_CCD01_03圓直徑量測_量測結果(ref cnt_Program_CCD01_03圓直徑量測);
            if (cnt_Program_CCD01_03圓直徑量測 == 7) cnt_Program_CCD01_03圓直徑量測_繪製畫布(ref cnt_Program_CCD01_03圓直徑量測);
            if (cnt_Program_CCD01_03圓直徑量測 == 8) cnt_Program_CCD01_03圓直徑量測 = 65500;
            if (cnt_Program_CCD01_03圓直徑量測 > 1) cnt_Program_CCD01_03圓直徑量測_檢查放開(ref cnt_Program_CCD01_03圓直徑量測);

            if (cnt_Program_CCD01_03圓直徑量測 == 65500)
            {
                if (PLC_Device_CCD01_03_計算一次.Bool)
                {
                    PLC_Device_CCD01_03圓直徑量測按鈕.Bool = false;
                }
                PLC_Device_CCD01_03圓直徑量測.Bool = false;
                cnt_Program_CCD01_03圓直徑量測 = 65535;
            }
        }
        void cnt_Program_CCD01_03圓直徑量測_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_03圓直徑量測按鈕.Bool)
            {
                PLC_Device_CCD01_03圓直徑量測.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_03圓直徑量測_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_03圓直徑量測.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_03圓直徑量測_初始化(ref int cnt)
        {
            this.CCD01_03_基準圓直徑_轉換後座標_ProbeCenter = new Point();
            this.CCD01_03_基準圓直徑_轉換後座標_StartTip = new Point();
            this.CCD01_03_基準圓直徑_轉換後座標_EndTip = new Point();
            this.PLC_Device_CCD01_03圓直徑量測_OK.Bool = false;
            cnt++;
        }
        void cnt_Program_CCD01_03圓直徑量測_座標轉換(ref int cnt)
        {
            if (PLC_Device_CCD01_03_計算一次.Bool)
            {
                CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.RefPointX = PLC_Device_CCD01_03_水平基準線量測_量測中心_X.Value;
                CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.RefPointY = PLC_Device_CCD01_03_水平基準線量測_量測中心_Y.Value;
                CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.RefAngle = 0;
                CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.CurrentRefPointX = Point_CCD01_03_中心基準座標_量測點.X;
                CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.CurrentRefPointY = Point_CCD01_03_中心基準座標_量測點.Y;
                CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.CurrentRefAngle = 0;
                CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.NumOfVisionPoints = 3;


                if (this.PLC_Device_CCD01_03圓直徑量測_ProbeCenterX.Value == 0) this.PLC_Device_CCD01_03圓直徑量測_ProbeCenterX.Value = 100;
                if (this.PLC_Device_CCD01_03圓直徑量測_ProbeCenterY.Value == 0) this.PLC_Device_CCD01_03圓直徑量測_ProbeCenterY.Value = 100;
                if (this.PLC_Device_CCD01_03圓直徑量測_StartTipX.Value == 0) this.PLC_Device_CCD01_03圓直徑量測_StartTipX.Value = 100;
                if (this.PLC_Device_CCD01_03圓直徑量測_StartTipY.Value == 0) this.PLC_Device_CCD01_03圓直徑量測_StartTipY.Value = 100;
                if (this.PLC_Device_CCD01_03圓直徑量測_EndTipX.Value == 0) this.PLC_Device_CCD01_03圓直徑量測_EndTipX.Value = 100;
                if (this.PLC_Device_CCD01_03圓直徑量測_EndTipY.Value == 0) this.PLC_Device_CCD01_03圓直徑量測_EndTipY.Value = 100;

                CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.VisionPointIndex = 0;
                CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.VisionPointX = this.PLC_Device_CCD01_03圓直徑量測_ProbeCenterX.Value;
                CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.VisionPointY = this.PLC_Device_CCD01_03圓直徑量測_ProbeCenterY.Value;
                CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.VisionPointIndex = 1;
                CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.VisionPointX = this.PLC_Device_CCD01_03圓直徑量測_StartTipX.Value;
                CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.VisionPointY = this.PLC_Device_CCD01_03圓直徑量測_StartTipY.Value;
                CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.VisionPointIndex = 2;
                CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.VisionPointX = this.PLC_Device_CCD01_03圓直徑量測_EndTipX.Value;
                CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.VisionPointY = this.PLC_Device_CCD01_03圓直徑量測_EndTipY.Value;
                CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.EstimateCurrentVisionPoints();

                CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.VisionPointIndex = 0;
                CCD01_03_基準圓直徑_轉換後座標_ProbeCenter.X = (int)CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.CurrentVisionPointX;
                CCD01_03_基準圓直徑_轉換後座標_ProbeCenter.Y = (int)CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.CurrentVisionPointY;
                CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.VisionPointIndex = 1;
                CCD01_03_基準圓直徑_轉換後座標_StartTip.X = (int)CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.CurrentVisionPointX;
                CCD01_03_基準圓直徑_轉換後座標_StartTip.Y = (int)CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.CurrentVisionPointY;
                CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.VisionPointIndex = 2;
                CCD01_03_基準圓直徑_轉換後座標_EndTip.X = (int)CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.CurrentVisionPointX;
                CCD01_03_基準圓直徑_轉換後座標_EndTip.Y = (int)CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整.CurrentVisionPointY;

            }
            cnt++;

        }
        void cnt_Program_CCD01_03圓直徑量測_讀取參數(ref int cnt)
        {
            #region ProbeCenter
            if (this.PLC_Device_CCD01_03圓直徑量測_ProbeCenterX.Value > 2596) this.PLC_Device_CCD01_03圓直徑量測_ProbeCenterX.Value = 0;
            if (this.PLC_Device_CCD01_03圓直徑量測_ProbeCenterY.Value > 1922) this.PLC_Device_CCD01_03圓直徑量測_ProbeCenterY.Value = 0;
            if (this.PLC_Device_CCD01_03圓直徑量測_ProbeCenterX.Value < 0) this.PLC_Device_CCD01_03圓直徑量測_ProbeCenterX.Value = 0;
            if (this.PLC_Device_CCD01_03圓直徑量測_ProbeCenterY.Value < 0) this.PLC_Device_CCD01_03圓直徑量測_ProbeCenterY.Value = 0;

            if (this.CCD01_03_基準圓直徑_轉換後座標_ProbeCenter.X > 2596) this.CCD01_03_基準圓直徑_轉換後座標_ProbeCenter.X = 0;
            if (this.CCD01_03_基準圓直徑_轉換後座標_ProbeCenter.Y > 1922) this.CCD01_03_基準圓直徑_轉換後座標_ProbeCenter.Y = 0;
            if (this.CCD01_03_基準圓直徑_轉換後座標_ProbeCenter.X < 0) this.CCD01_03_基準圓直徑_轉換後座標_ProbeCenter.X = 0;
            if (this.CCD01_03_基準圓直徑_轉換後座標_ProbeCenter.Y < 0) this.CCD01_03_基準圓直徑_轉換後座標_ProbeCenter.Y = 0;
            #endregion
            #region StartTip
            if (this.PLC_Device_CCD01_03圓直徑量測_StartTipX.Value > 2596) this.PLC_Device_CCD01_03圓直徑量測_StartTipX.Value = 0;
            if (this.PLC_Device_CCD01_03圓直徑量測_StartTipY.Value > 1922) this.PLC_Device_CCD01_03圓直徑量測_StartTipY.Value = 0;
            if (this.PLC_Device_CCD01_03圓直徑量測_StartTipX.Value < 0) this.PLC_Device_CCD01_03圓直徑量測_StartTipX.Value = 0;
            if (this.PLC_Device_CCD01_03圓直徑量測_StartTipY.Value < 0) this.PLC_Device_CCD01_03圓直徑量測_StartTipY.Value = 0;

            if (this.CCD01_03_基準圓直徑_轉換後座標_StartTip.X > 2596) this.CCD01_03_基準圓直徑_轉換後座標_StartTip.X = 0;
            if (this.CCD01_03_基準圓直徑_轉換後座標_StartTip.Y > 1922) this.CCD01_03_基準圓直徑_轉換後座標_StartTip.Y = 0;
            if (this.CCD01_03_基準圓直徑_轉換後座標_StartTip.X < 0) this.CCD01_03_基準圓直徑_轉換後座標_StartTip.X = 0;
            if (this.CCD01_03_基準圓直徑_轉換後座標_StartTip.Y < 0) this.CCD01_03_基準圓直徑_轉換後座標_StartTip.Y = 0;
            #endregion
            #region EndTip
            if (this.PLC_Device_CCD01_03圓直徑量測_EndTipX.Value > 2596) this.PLC_Device_CCD01_03圓直徑量測_EndTipX.Value = 0;
            if (this.PLC_Device_CCD01_03圓直徑量測_EndTipY.Value > 1922) this.PLC_Device_CCD01_03圓直徑量測_EndTipY.Value = 0;
            if (this.PLC_Device_CCD01_03圓直徑量測_EndTipX.Value < 0) this.PLC_Device_CCD01_03圓直徑量測_EndTipX.Value = 0;
            if (this.PLC_Device_CCD01_03圓直徑量測_EndTipY.Value < 0) this.PLC_Device_CCD01_03圓直徑量測_EndTipY.Value = 0;

            if (this.CCD01_03_基準圓直徑_轉換後座標_EndTip.X > 2596) this.CCD01_03_基準圓直徑_轉換後座標_EndTip.X = 0;
            if (this.CCD01_03_基準圓直徑_轉換後座標_EndTip.Y > 1922) this.CCD01_03_基準圓直徑_轉換後座標_EndTip.Y = 0;
            if (this.CCD01_03_基準圓直徑_轉換後座標_EndTip.X < 0) this.CCD01_03_基準圓直徑_轉換後座標_EndTip.X = 0;
            if (this.CCD01_03_基準圓直徑_轉換後座標_EndTip.Y < 0) this.CCD01_03_基準圓直徑_轉換後座標_EndTip.Y = 0;
            #endregion
            this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.SrcImageHandle = this.CCD01_03_SrcImageHandle;

            if (PLC_Device_CCD01_03_計算一次.Bool)
            {
                this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.ProbeCenterX = CCD01_03_基準圓直徑_轉換後座標_ProbeCenter.X;
                this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.ProbeCenterY = CCD01_03_基準圓直徑_轉換後座標_ProbeCenter.Y;
                this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.StartTipX = CCD01_03_基準圓直徑_轉換後座標_StartTip.X;
                this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.StartTipY = CCD01_03_基準圓直徑_轉換後座標_StartTip.Y;
                this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.EndTipX = CCD01_03_基準圓直徑_轉換後座標_EndTip.X;
                this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.EndTipY = CCD01_03_基準圓直徑_轉換後座標_EndTip.Y;
            }
            else
            {
                this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.ProbeCenterX = PLC_Device_CCD01_03圓直徑量測_ProbeCenterX.Value;
                this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.ProbeCenterY = PLC_Device_CCD01_03圓直徑量測_ProbeCenterY.Value;
                this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.StartTipX = PLC_Device_CCD01_03圓直徑量測_StartTipX.Value;
                this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.StartTipY = PLC_Device_CCD01_03圓直徑量測_StartTipY.Value;
                this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.EndTipX = PLC_Device_CCD01_03圓直徑量測_EndTipX.Value;
                this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.EndTipY = PLC_Device_CCD01_03圓直徑量測_EndTipY.Value;
            }
            this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.StartTipDeriThreshold = PLC_Device_CCD01_03圓直徑量測_起點變化銳利度門檻.Value;
            this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.StartTipHysteresis = PLC_Device_CCD01_03圓直徑量測_起點延伸變化強度門檻.Value;
            this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.StartTipMinGreyStep = PLC_Device_CCD01_03圓直徑量測_起點灰階面化面積門檻.Value;
            this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.StartTipHalfProfileThickness = PLC_Device_CCD01_03圓直徑量測_起點垂直量測寬度.Value;
            this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.StartTipSmoothFactor = PLC_Device_CCD01_03圓直徑量測_起點雜訊抑制.Value;
            this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.StartTipEdgeType = (AxOvkMsr.TxAxTransitionType)PLC_Device_CCD01_03圓直徑量測_起點量測顏色變化.Value;
            this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.StartTipMsrDirection = (AxOvkMsr.TxAxGapMsrDirection)PLC_Device_CCD01_03圓直徑量測_起點量測方向.Value;

            this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.EndTipDeriThreshold = PLC_Device_CCD01_03圓直徑量測_終點變化銳利度門檻.Value;
            this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.EndTipHysteresis = PLC_Device_CCD01_03圓直徑量測_終點延伸變化強度門檻.Value;
            this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.EndTipMinGreyStep = PLC_Device_CCD01_03圓直徑量測_終點灰階面化面積門檻.Value;
            this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.EndTipHalfProfileThickness = PLC_Device_CCD01_03圓直徑量測_終點垂直量測寬度.Value;
            this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.EndTipSmoothFactor = PLC_Device_CCD01_03圓直徑量測_終點雜訊抑制.Value;
            this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.EndTipEdgeType = (AxOvkMsr.TxAxTransitionType)PLC_Device_CCD01_03圓直徑量測_終點量測顏色變化.Value;
            this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.EndTipMsrDirection = (AxOvkMsr.TxAxGapMsrDirection)PLC_Device_CCD01_03圓直徑量測_終點量測方向.Value;
            this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.SetProbeHorizontal();
            this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.DetectPrimitives();
            cnt++;
        }
        void cnt_Program_CCD01_03圓直徑量測_開始量測(ref int cnt)
        {
            this.PLC_Device_CCD01_03圓直徑量測_OK.Bool = true;
            if (CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.GapIsFitted == true)
            {
                this.CCD01_03_基準圓直徑_Radius = (this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredEndTipX - this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整.MeasuredStartTipX) * this.CCD01_比例尺_pixcel_To_mm;
                this.CCD01_03_基準圓直徑_Radius = Math.Round(this.CCD01_03_基準圓直徑_Radius, 3);
            }
            else this.PLC_Device_CCD01_03圓直徑量測_OK.Bool = false;

            cnt++;
        }
        void cnt_Program_CCD01_03圓直徑量測_量測結果(ref int cnt)
        {
            int 標準值 = this.PLC_Device_CCD01_03_基準圓直徑_量測標準值.Value;
            int 標準值上限 = this.PLC_Device_CCD01_03_基準圓直徑_量測上限值.Value;
            int 標準值下限 = this.PLC_Device_CCD01_03_基準圓直徑_量測下限值.Value;
            double 量測距離 = this.CCD01_03_基準圓直徑_Radius;

            量測距離 = 量測距離 * 1000 - 標準值;
            量測距離 /= 1000;
            if (!PLC_Device_CCD01_02_PIN量測_間距量測不測試.Bool)
            {
                if (量測距離 >= 0)
                {
                    if (標準值上限 <= Math.Abs(量測距離) * 1000 || 標準值下限 > Math.Abs(量測距離) * 1000)
                    {

                        PLC_Device_CCD01_03圓直徑量測_OK.Bool = false;
                    }
                    else
                    {
                        this.PLC_Device_CCD01_03圓直徑量測_OK.Bool = true;
                    }
                }
            }
            else
            {
                this.PLC_Device_CCD01_03圓直徑量測_OK.Bool = true;
            }

            cnt++;
        }
        void cnt_Program_CCD01_03圓直徑量測_繪製畫布(ref int cnt)
        {
            this.PLC_Device_CCD01_03圓直徑量測_RefreshCanvas.Bool = true;
            if (this.PLC_Device_CCD01_03圓直徑量測按鈕.Bool && !PLC_Device_CCD01_03_計算一次.Bool)
            {
                this.h_Canvas_Tech_CCD01_03.RefreshCanvas();
            }
            cnt++;
        }





        #endregion
        #region PLC_CCD01_03圓柱相似度量測

        private AxOvkPat.AxMatch AxMatch_CCD01_03_圓柱相似度測試 = new AxOvkPat.AxMatch();
        private AxOvkImage.AxImageCopier AxImageCopier_CCD01_03_圓柱相似度測試_GetPattern = new AxOvkImage.AxImageCopier();
        private AxOvkBase.AxImageBW8 CCD01_03_圓柱相似度測試_GetPattern_AxImageBW8 = new AxOvkBase.AxImageBW8();

        private PLC_Device PLC_Device_CCD01_03圓柱相似度量測按鈕 = new PLC_Device("S6520");
        private PLC_Device PLC_Device_CCD01_03圓柱相似度量測 = new PLC_Device("S6525");
        private PLC_Device PLC_Device_CCD01_03圓柱相似度量測_OK = new PLC_Device("S6526");
        private PLC_Device PLC_Device_CCD01_03圓柱相似度量測_測試完成 = new PLC_Device("S6527");
        private PLC_Device PLC_Device_CCD01_03圓柱相似度量測_RefreshCanvas = new PLC_Device("S6528");

        private PLC_Device PLC_Device_CCD01_03圓柱相似度量測_樣板取樣細緻度_MinReducedArea = new PLC_Device("F7040");
        private PLC_Device PLC_Device_CCD01_03圓柱相似度量測_樣板相似度門檻_MinScore = new PLC_Device("F7041");
        private PLC_Device PLC_Device_CCD01_03_圓柱相似度量測_樣本圖片辨識代碼 = new PLC_Device("F7042");
        string CCD01_03_樣板儲存名稱 = "CCD1-3_PAT";
        float Match_CCD0103_Score;

        private void H_Canvas_Tech_CCD01_03圓柱相似度量測_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {
            PointF 相似度數值顯示 = new PointF(List_CCD01_03_基準圓_量測點[0].X + 100, List_CCD01_03_基準圓_量測點[0].Y - 150);
            if (PLC_Device_CCD01_03_Main_取像並檢驗.Bool || PLC_Device_CCD01_03_PLC觸發檢測.Bool || PLC_Device_CCD01_03_Main_檢驗一次.Bool)
            {
                if (this.PLC_Device_CCD01_03圓柱相似度量測_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        this.AxMatch_CCD01_03_圓柱相似度測試.DrawMatchedPattern(HDC, -1, ZoomX, ZoomY, 0, 0);
                        if (PLC_Device_CCD01_03圓柱相似度量測_OK.Bool) DrawingClass.Draw.文字左上繪製("圓量測OK!", new PointF(0, 400), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        else DrawingClass.Draw.文字左上繪製("圓量測NG!", new PointF(0, 400), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);

                        if (PLC_Device_CCD01_03圓柱相似度量測_OK.Bool) DrawingClass.Draw.文字左上繪製((this.Match_CCD0103_Score * 100).ToString("0.0") + "%", 相似度數值顯示, new Font("標楷體", 12), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        else DrawingClass.Draw.文字左上繪製(this.Match_CCD0103_Score.ToString() + "%", 相似度數值顯示, new Font("標楷體", 12), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }


            }
            else if (PLC_Device_CCD01_03_Tech_檢驗一次.Bool)
            {
                if (this.PLC_Device_CCD01_03圓柱相似度量測_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        this.AxMatch_CCD01_03_圓柱相似度測試.DrawMatchedPattern(HDC, -1, ZoomX, ZoomY, 0, 0);
                        if (PLC_Device_CCD01_03圓柱相似度量測_OK.Bool) DrawingClass.Draw.文字左上繪製("圓量測OK!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        else DrawingClass.Draw.文字左上繪製("圓量測NG!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);

                        if (PLC_Device_CCD01_03圓柱相似度量測_OK.Bool) DrawingClass.Draw.文字左上繪製((this.Match_CCD0103_Score * 100).ToString("0.0") + "%", 相似度數值顯示, new Font("標楷體", 12), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        else DrawingClass.Draw.文字左上繪製(this.Match_CCD0103_Score.ToString() + "%", 相似度數值顯示, new Font("標楷體", 12), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }


            }
            else
            {
                if (this.PLC_Device_CCD01_03圓柱相似度量測_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        this.AxMatch_CCD01_03_圓柱相似度測試.DrawMatchedPattern(HDC, -1, ZoomX, ZoomY, 0, 0);
                        if (PLC_Device_CCD01_03圓柱相似度量測_OK.Bool) DrawingClass.Draw.文字左上繪製("圓量測OK!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        else DrawingClass.Draw.文字左上繪製("圓量測NG!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);

                        if (PLC_Device_CCD01_03圓柱相似度量測_OK.Bool) DrawingClass.Draw.文字左上繪製((this.Match_CCD0103_Score * 100).ToString("0.0") + "%", 相似度數值顯示, new Font("標楷體", 12), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        else DrawingClass.Draw.文字左上繪製(this.Match_CCD0103_Score.ToString() + "%", 相似度數值顯示, new Font("標楷體", 12), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }
            }

            this.PLC_Device_CCD01_03圓柱相似度量測_RefreshCanvas.Bool = false;
        }
        int cnt_Program_CCD01_03圓柱相似度量測 = 65534;
        void sub_Program_CCD01_03圓柱相似度量測()
        {
            if (cnt_Program_CCD01_03圓柱相似度量測 == 65534)
            {
                this.h_Canvas_Tech_CCD01_03.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_03圓柱相似度量測_OnCanvasDrawEvent;
                this.h_Canvas_Main_CCD01_03_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_03圓柱相似度量測_OnCanvasDrawEvent;
                this.

                PLC_Device_CCD01_03圓柱相似度量測.SetComment("PLC_CCD01_03圓柱相似度量測");
                PLC_Device_CCD01_03圓柱相似度量測.Bool = false;
                PLC_Device_CCD01_03圓柱相似度量測按鈕.Bool = false;
                cnt_Program_CCD01_03圓柱相似度量測 = 65535;
            }
            if (cnt_Program_CCD01_03圓柱相似度量測 == 65535) cnt_Program_CCD01_03圓柱相似度量測 = 1;
            if (cnt_Program_CCD01_03圓柱相似度量測 == 1) cnt_Program_CCD01_03圓柱相似度量測_檢查按下(ref cnt_Program_CCD01_03圓柱相似度量測);
            if (cnt_Program_CCD01_03圓柱相似度量測 == 2) cnt_Program_CCD01_03圓柱相似度量測_比對範圍設定開始(ref cnt_Program_CCD01_03圓柱相似度量測);
            if (cnt_Program_CCD01_03圓柱相似度量測 == 3) cnt_Program_CCD01_03圓柱相似度量測_比對範圍設定結束(ref cnt_Program_CCD01_03圓柱相似度量測);
            if (cnt_Program_CCD01_03圓柱相似度量測 == 4) cnt_Program_CCD01_03圓柱相似度量測_初始化(ref cnt_Program_CCD01_03圓柱相似度量測);
            if (cnt_Program_CCD01_03圓柱相似度量測 == 5) cnt_Program_CCD01_03圓柱相似度量測_搜尋樣板(ref cnt_Program_CCD01_03圓柱相似度量測);
            if (cnt_Program_CCD01_03圓柱相似度量測 == 6) cnt_Program_CCD01_03圓柱相似度量測_繪製畫布(ref cnt_Program_CCD01_03圓柱相似度量測);
            if (cnt_Program_CCD01_03圓柱相似度量測 == 7) cnt_Program_CCD01_03圓柱相似度量測 = 65500;
            if (cnt_Program_CCD01_03圓柱相似度量測 > 1) cnt_Program_CCD01_03圓柱相似度量測_檢查放開(ref cnt_Program_CCD01_03圓柱相似度量測);

            if (cnt_Program_CCD01_03圓柱相似度量測 == 65500)
            {
                PLC_Device_CCD01_03圓柱相似度量測.Bool = false;
                PLC_Device_CCD01_03圓柱相似度量測按鈕.Bool = false;
                cnt_Program_CCD01_03圓柱相似度量測 = 65535;
            }
        }
        void cnt_Program_CCD01_03圓柱相似度量測_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_03圓柱相似度量測按鈕.Bool)
            {
                PLC_Device_CCD01_03圓柱相似度量測.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_03圓柱相似度量測_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_03圓柱相似度量測.Bool) cnt = 65500;
        }

        void cnt_Program_CCD01_03圓柱相似度量測_比對範圍設定開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_03比對樣板範圍.Bool)
            {
                this.PLC_Device_CCD01_03比對樣板範圍.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_03圓柱相似度量測_比對範圍設定結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_03比對樣板範圍.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_03圓柱相似度量測_初始化(ref int cnt)
        {
            this.Match_CCD0103_Score = new float();
            this.PLC_Device_CCD01_03圓柱相似度量測_OK.Bool = false;
            this.AxMatch_CCD01_03_圓柱相似度測試.DstImageHandle = AxROIBW8_CCD01_03_比對樣板範圍.VegaHandle;
            this.AxMatch_CCD01_03_圓柱相似度測試.PositionType = AxOvkPat.TxAxMatchPositionType.AX_MATCH_POSITION_TYPE_CENTER;
            this.AxMatch_CCD01_03_圓柱相似度測試.MaxPositions = 1;
            this.AxMatch_CCD01_03_圓柱相似度測試.MinScore = (float)PLC_Device_CCD01_03圓柱相似度量測_樣板相似度門檻_MinScore.Value / 100;
            this.AxMatch_CCD01_03_圓柱相似度測試.Match();
            cnt++;
        }
        void cnt_Program_CCD01_03圓柱相似度量測_搜尋樣板(ref int cnt)
        {
            bool effect = this.AxMatch_CCD01_03_圓柱相似度測試.EffectMatch;
            int num = this.AxMatch_CCD01_03_圓柱相似度測試.NumMatchedPos;
            this.AxMatch_CCD01_03_圓柱相似度測試.PatternIndex = 0;
            this.Match_CCD0103_Score = this.AxMatch_CCD01_03_圓柱相似度測試.MatchedScore;
            cnt++;

        }
        void cnt_Program_CCD01_03圓柱相似度量測_繪製畫布(ref int cnt)
        {
            if (CCD01_03_SrcImageHandle != 0)
            {
                AxMatch_CCD01_03_圓柱相似度測試.PatternIndex = 0;
                if (AxMatch_CCD01_03_圓柱相似度測試.EffectMatch)
                {
                    PLC_Device_CCD01_03圓柱相似度量測_OK.Bool = true;
                }

                this.PLC_Device_CCD01_03圓柱相似度量測_RefreshCanvas.Bool = true;
                if (this.PLC_Device_CCD01_03圓柱相似度量測按鈕.Bool && !PLC_Device_CCD01_03_計算一次.Bool)
                {
                    this.h_Canvas_Tech_CCD01_03.RefreshCanvas();
                }
            }

            cnt++;
        }

        #endregion
        #region PLC_CCD01_03比對樣板範圍
        private AxOvkBase.AxROIBW8 AxROIBW8_CCD01_03_比對樣板範圍 = new AxOvkBase.AxROIBW8();
        private PLC_Device PLC_Device_CCD01_03比對樣板範圍按鈕 = new PLC_Device("S6530");
        private PLC_Device PLC_Device_CCD01_03比對樣板範圍 = new PLC_Device("S6535");
        private PLC_Device PLC_Device_CCD01_03比對樣板範圍_OK = new PLC_Device("S6536");
        private PLC_Device PLC_Device_CCD01_03比對樣板範圍_測試完成 = new PLC_Device("S6537");
        private PLC_Device PLC_Device_CCD01_03比對樣板範圍_RefreshCanvas = new PLC_Device("S6538");

        private PLC_Device PLC_Device_CCD01_03比對樣板範圍_CenterX = new PLC_Device("F7045");
        private PLC_Device PLC_Device_CCD01_03比對樣板範圍_CenterY = new PLC_Device("F7046");
        private PLC_Device PLC_Device_CCD01_03比對樣板範圍_Width = new PLC_Device("F7047");
        private PLC_Device PLC_Device_CCD01_03比對樣板範圍_Height = new PLC_Device("F7048");

        private void H_Canvas_Tech_CCD01_03比對樣板範圍_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {

            if (PLC_Device_CCD01_03_Main_取像並檢驗.Bool || PLC_Device_CCD01_03_PLC觸發檢測.Bool || PLC_Device_CCD01_03_Main_檢驗一次.Bool)
            {
                if (this.PLC_Device_CCD01_03比對樣板範圍_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);

                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }


            }
            else if (PLC_Device_CCD01_03_Tech_檢驗一次.Bool)
            {
                if (this.PLC_Device_CCD01_03比對樣板範圍_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);

                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }


            }
            else
            {
                if (this.PLC_Device_CCD01_03比對樣板範圍_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);
                        this.AxROIBW8_CCD01_03_比對樣板範圍.ShowTitle = true;
                        this.AxROIBW8_CCD01_03_比對樣板範圍.Title = "樣板量測範圍";
                        this.AxROIBW8_CCD01_03_比對樣板範圍.DrawFrame(HDC, ZoomX, ZoomY, 0, 0, 0X0000FF);
                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }
            }

            this.PLC_Device_CCD01_03比對樣板範圍_RefreshCanvas.Bool = false;
        }

        AxOvkBase.TxAxHitHandle CCD01_03樣板比對範圍AxROIBW8_TxAxRoiHitHandle = new AxOvkBase.TxAxHitHandle();
        bool flag_CCD01_03_樣板比對範圍AxROIBW8_MouseDown = new bool();
        private void H_Canvas_Tech_CCD01_03比對樣板範圍_OnCanvasMouseDownEvent(int x, int y, float ZoomX, float ZoomY, ref int InUsedEventNum, int InUsedCanvasHandle)
        {
            if (this.PLC_Device_CCD01_03比對樣板範圍.Bool)
            {
                this.CCD01_03樣板比對範圍AxROIBW8_TxAxRoiHitHandle = this.AxROIBW8_CCD01_03_比對樣板範圍.HitTest(x, y, ZoomX, ZoomY, 0, 0);
                if (this.CCD01_03樣板比對範圍AxROIBW8_TxAxRoiHitHandle != AxOvkBase.TxAxHitHandle.AX_HANDLE_NONE)
                {
                    this.flag_CCD01_03_樣板比對範圍AxROIBW8_MouseDown = true;
                    InUsedEventNum = 10;
                }
            }

        }
        private void H_Canvas_Tech_CCD01_03比對樣板範圍_OnCanvasMouseMoveEvent(int x, int y, float ZoomX, float ZoomY)
        {
            if (this.flag_CCD01_03_樣板比對範圍AxROIBW8_MouseDown)
            {
                this.AxROIBW8_CCD01_03_比對樣板範圍.DragROI(CCD01_03樣板比對範圍AxROIBW8_TxAxRoiHitHandle, x, y, ZoomX, ZoomY, 0, 0);
                this.PLC_Device_CCD01_03比對樣板範圍_CenterX.Value = this.AxROIBW8_CCD01_03_比對樣板範圍.OrgX;
                this.PLC_Device_CCD01_03比對樣板範圍_CenterY.Value = this.AxROIBW8_CCD01_03_比對樣板範圍.OrgY;
                this.PLC_Device_CCD01_03比對樣板範圍_Width.Value = this.AxROIBW8_CCD01_03_比對樣板範圍.ROIWidth;
                this.PLC_Device_CCD01_03比對樣板範圍_Height.Value = this.AxROIBW8_CCD01_03_比對樣板範圍.ROIHeight;

            }

        }
        private void H_Canvas_Tech_CCD01_03比對樣板範圍_OnCanvasMouseUpEvent(int x, int y, float ZoomX, float ZoomY)
        {

            this.flag_CCD01_03_樣板比對範圍AxROIBW8_MouseDown = false;

        }
        int cnt_Program_CCD01_03比對樣板範圍 = 65534;
        void sub_Program_CCD01_03比對樣板範圍()
        {
            if (cnt_Program_CCD01_03比對樣板範圍 == 65534)
            {
                this.h_Canvas_Tech_CCD01_03.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_03比對樣板範圍_OnCanvasDrawEvent;
                this.h_Canvas_Tech_CCD01_03.OnCanvasMouseDownEvent += H_Canvas_Tech_CCD01_03比對樣板範圍_OnCanvasMouseDownEvent;
                this.h_Canvas_Tech_CCD01_03.OnCanvasMouseMoveEvent += H_Canvas_Tech_CCD01_03比對樣板範圍_OnCanvasMouseMoveEvent;
                this.h_Canvas_Tech_CCD01_03.OnCanvasMouseUpEvent += H_Canvas_Tech_CCD01_03比對樣板範圍_OnCanvasMouseUpEvent;

                this.h_Canvas_Main_CCD01_03_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_03比對樣板範圍_OnCanvasDrawEvent;


                if (this.PLC_Device_CCD01_03比對樣板範圍_Height.Value == 0) this.PLC_Device_CCD01_03比對樣板範圍_Height.Value = 150;
                if (this.PLC_Device_CCD01_03比對樣板範圍_Width.Value == 0) this.PLC_Device_CCD01_03比對樣板範圍_Width.Value = 150;



                PLC_Device_CCD01_03比對樣板範圍.SetComment("PLC_CCD01_03比對樣板範圍");
                PLC_Device_CCD01_03比對樣板範圍.Bool = false;
                PLC_Device_CCD01_03比對樣板範圍按鈕.Bool = false;
                cnt_Program_CCD01_03比對樣板範圍 = 65535;
            }
            if (cnt_Program_CCD01_03比對樣板範圍 == 65535) cnt_Program_CCD01_03比對樣板範圍 = 1;
            if (cnt_Program_CCD01_03比對樣板範圍 == 1) cnt_Program_CCD01_03比對樣板範圍_檢查按下(ref cnt_Program_CCD01_03比對樣板範圍);
            if (cnt_Program_CCD01_03比對樣板範圍 == 2) cnt_Program_CCD01_03比對樣板範圍_初始化(ref cnt_Program_CCD01_03比對樣板範圍);
            if (cnt_Program_CCD01_03比對樣板範圍 == 3) cnt_Program_CCD01_03比對樣板範圍_生成量測框(ref cnt_Program_CCD01_03比對樣板範圍);
            if (cnt_Program_CCD01_03比對樣板範圍 == 4) cnt_Program_CCD01_03比對樣板範圍_繪製畫布(ref cnt_Program_CCD01_03比對樣板範圍);
            if (cnt_Program_CCD01_03比對樣板範圍 == 5) cnt_Program_CCD01_03比對樣板範圍 = 65500;
            if (cnt_Program_CCD01_03比對樣板範圍 > 1) cnt_Program_CCD01_03比對樣板範圍_檢查放開(ref cnt_Program_CCD01_03比對樣板範圍);

            if (cnt_Program_CCD01_03比對樣板範圍 == 65500)
            {
                PLC_Device_CCD01_03比對樣板範圍.Bool = false;
                cnt_Program_CCD01_03比對樣板範圍 = 65535;
            }
        }
        void cnt_Program_CCD01_03比對樣板範圍_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_03比對樣板範圍按鈕.Bool || PLC_Device_CCD01_03圓柱相似度量測按鈕.Bool)
            {
                PLC_Device_CCD01_03比對樣板範圍.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_03比對樣板範圍_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_03比對樣板範圍.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_03比對樣板範圍_初始化(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_03比對樣板範圍_生成量測框(ref int cnt)
        {

            if (this.PLC_Device_CCD01_03比對樣板範圍_CenterX.Value > 2596) this.PLC_Device_CCD01_03比對樣板範圍_CenterX.Value = 0;
            if (this.PLC_Device_CCD01_03比對樣板範圍_CenterY.Value > 1922) this.PLC_Device_CCD01_03比對樣板範圍_CenterY.Value = 0;
            if (this.PLC_Device_CCD01_03比對樣板範圍_Width.Value < 0) this.PLC_Device_CCD01_03比對樣板範圍_Width.Value = 0;
            if (this.PLC_Device_CCD01_03比對樣板範圍_Height.Value < 0) this.PLC_Device_CCD01_03比對樣板範圍_Height.Value = 0;

            this.AxROIBW8_CCD01_03_比對樣板範圍.ParentHandle = this.CCD01_03_SrcImageHandle;
            this.AxROIBW8_CCD01_03_比對樣板範圍.OrgX = PLC_Device_CCD01_03比對樣板範圍_CenterX.Value;
            this.AxROIBW8_CCD01_03_比對樣板範圍.OrgY = PLC_Device_CCD01_03比對樣板範圍_CenterY.Value;
            this.AxROIBW8_CCD01_03_比對樣板範圍.ROIWidth = PLC_Device_CCD01_03比對樣板範圍_Width.Value;
            this.AxROIBW8_CCD01_03_比對樣板範圍.ROIHeight = PLC_Device_CCD01_03比對樣板範圍_Height.Value;

            cnt++;
        }
        void cnt_Program_CCD01_03比對樣板範圍_繪製畫布(ref int cnt)
        {
            if (CCD01_03_SrcImageHandle != 0)
            {
                this.PLC_Device_CCD01_03比對樣板範圍_RefreshCanvas.Bool = true;
                if (this.PLC_Device_CCD01_03比對樣板範圍按鈕.Bool && !PLC_Device_CCD01_03_計算一次.Bool)
                {
                    this.h_Canvas_Tech_CCD01_03.RefreshCanvas();
                }
            }
            cnt++;
        }

        #region 讀取樣板
        int cnt_Program_CCD01_03_讀取樣板 = 65534;
        void sub_Program_CCD01_03_讀取樣板()
        {
            if (cnt_Program_CCD01_03_讀取樣板 == 65534)
            {
                cnt_Program_CCD01_03_讀取樣板 = 65535;
            }
            if (cnt_Program_CCD01_03_讀取樣板 == 65535) cnt_Program_CCD01_03_讀取樣板 = 1;
            if (cnt_Program_CCD01_03_讀取樣板 == 1) cnt_CCD01_03_讀取樣板_開始讀取(ref cnt_Program_CCD01_03_讀取樣板);
            if (cnt_Program_CCD01_03_讀取樣板 == 2) cnt_Program_CCD01_03_讀取樣板 = 255;
        }
        void cnt_CCD01_03_讀取樣板_開始讀取(ref int cnt)
        {
            AxMatch_CCD01_03_圓柱相似度測試.LoadFile(".//" + CCD01_03_樣板儲存名稱);
            if (AxMatch_CCD01_03_圓柱相似度測試.IsLearnPattern)
            {
                this.AxImageCopier_CCD01_03_圓柱相似度測試_GetPattern.SrcImageHandle = AxMatch_CCD01_03_圓柱相似度測試.PatternVegaHandle;
                this.AxImageCopier_CCD01_03_圓柱相似度測試_GetPattern.DstImageHandle = this.CCD01_03_圓柱相似度測試_GetPattern_AxImageBW8.VegaHandle;
                this.AxImageCopier_CCD01_03_圓柱相似度測試_GetPattern.Copy();
                this.CCD01_03_PatternCanvasRefresh(this.CCD01_03_圓柱相似度測試_GetPattern_AxImageBW8.VegaHandle, this.CCD01_03_PatternCanvas_ZoomX, this.CCD01_03_PatternCanvas_ZoomY);
            }
            cnt++;
        }
        #endregion
        #endregion

        #region Event
        private void plC_RJ_Button_CCD01_03_儲存圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (saveImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.h_Canvas_Tech_CCD01_03.SaveImage(saveImageDialog.FileName);
                }
            }));
        }
        private void plC_RJ_Button_CCD01_03_讀取圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (openImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.CCD01_AxImageBW8.LoadFile(openImageDialog.FileName);
                    try
                    {
                        this.h_Canvas_Tech_CCD01_03.ImageCopy(CCD01_AxImageBW8.VegaHandle);
                        this.CCD01_03_SrcImageHandle = h_Canvas_Tech_CCD01_03.VegaHandle;
                        this.h_Canvas_Tech_CCD01_03.RefreshCanvas();
                    }
                    catch
                    {
                        err_message01_03 = MessageBox.Show("讀取圖片空白", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        if (err_message01_03 == DialogResult.OK)
                        {

                        }
                    }
                }
            }));
        }
        private void PlC_RJ_Button_Main_CCD01_03儲存圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (saveImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.h_Canvas_Main_CCD01_03_檢測畫面.SaveImage(saveImageDialog.FileName);
                }
            }));
        }
        private void PlC_RJ_Button_Main_CCD01_03讀取圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (openImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.CCD01_AxImageBW8.LoadFile(openImageDialog.FileName);
                    try
                    {
                        this.h_Canvas_Main_CCD01_03_檢測畫面.ImageCopy(CCD01_AxImageBW8.VegaHandle);
                        this.CCD01_03_SrcImageHandle = h_Canvas_Main_CCD01_03_檢測畫面.VegaHandle;
                        this.h_Canvas_Main_CCD01_03_檢測畫面.RefreshCanvas();
                    }
                    catch
                    {
                        err_message01_03 = MessageBox.Show("讀取圖片空白", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        if (err_message01_03 == DialogResult.OK)
                        {

                        }
                    }
                }
            }));
        }
        private void PlC_Button_Main_CCD01_03_ZOOM更新_btnClick(object sender, EventArgs e)
        {
            if (CCD01_03_SrcImageHandle != 0)
            {
                PLC_Device_Main_CCD01_03_ZOOM更新.Bool = true;
                h_Canvas_Main_CCD01_03_檢測畫面.RefreshCanvas();
            }
        }

        private void plC_Button_CCD01_03_學習樣板按鈕_btnClick(object sender, EventArgs e)
        {

            AxMatch_CCD01_03_圓柱相似度測試.MinReducedArea = 144;
            AxMatch_CCD01_03_圓柱相似度測試.LearnPattern();
            if (AxMatch_CCD01_03_圓柱相似度測試.IsLearnPattern)
            {
                MessageBox.Show("學習樣板成功", "訊息", MessageBoxButtons.OKCancel);
                this.PLC_Device_CCD01_03_圓柱相似度量測_樣本圖片辨識代碼.Value = (int)AxMatch_CCD01_03_圓柱相似度測試.PatternVegaHandle;
                this.AxImageCopier_CCD01_03_圓柱相似度測試_GetPattern.SrcImageHandle = AxMatch_CCD01_03_圓柱相似度測試.PatternVegaHandle;
                this.AxImageCopier_CCD01_03_圓柱相似度測試_GetPattern.DstImageHandle = this.CCD01_03_圓柱相似度測試_GetPattern_AxImageBW8.VegaHandle;
                this.AxImageCopier_CCD01_03_圓柱相似度測試_GetPattern.Copy();
                this.CCD01_03_PatternCanvasRefresh(this.CCD01_03_圓柱相似度測試_GetPattern_AxImageBW8.VegaHandle, this.CCD01_03_PatternCanvas_ZoomX, this.CCD01_03_PatternCanvas_ZoomY);
                this.AxMatch_CCD01_03_圓柱相似度測試.SaveFile(".//" + CCD01_03_樣板儲存名稱);
            }
            else MessageBox.Show("學習樣板失敗!!", "訊息", MessageBoxButtons.OKCancel);

        }
        public delegate void CCD01_03_PatternCanvas_Refresh_EventHandler(long SurfaceHadle, float ZoomX, float ZoomY);
        public event CCD01_03_PatternCanvas_Refresh_EventHandler CCD01_03_PatternCanvas_Refresh_Event;
        void CCD01_03_PatternCanvasRefresh(long SurfaceHadle, float ZoomX, float ZoomY)
        {
            this.CCD01_03_PatternCanvas.ClearCanvas();
            this.CCD01_03_PatternCanvas.DrawSurface(SurfaceHadle, ZoomX, ZoomY, 0, 0);
            //
            if (this.CCD01_03_PatternCanvas_Refresh_Event != null) this.CCD01_03_PatternCanvas_Refresh_Event(SurfaceHadle, ZoomX, ZoomY);
            //
            this.CCD01_03_PatternCanvas.RefreshCanvas();

        }
        #endregion
    }
}
