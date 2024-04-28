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


        DialogResult err_message01_01;

        void Program_CCD01_01()
        {
            //sub_Program_PLC_CCD01_Main_全檢測一次();
            //sub_Program_PLC_CCD01_Main_全Snap一次();
            this.CCD01_01_儲存圖片();
            this.sub_Program_CCD01_01_SNAP();
            this.sub_Program_CCD01_01_Main_取像並檢驗();
            this.sub_Program_CCD01_01_Tech_取像並檢驗();
            this.sub_Program_CCD01_01_Tech_檢驗一次();
            this.sub_Program_CCD01_01_計算一次();
            this.sub_Program_CCD01_01_基準線量測();
            this.sub_Program_CCD01_01基準圓量測框調整();
            this.sub_Program_CCD01_01圓柱相似度量測();
            this.sub_Program_CCD01_01比對樣板範圍();
            this.sub_Program_CCD01_01_讀取樣板();
            this.sub_Program_CCD01_01圓直徑量測();
            this.sub_Program_CCD01_01_Main_檢驗一次();
            if (PLC_Device_CCD01_01圓柱相似度量測_樣板相似度門檻_MinScore.Value / 100 < 0) PLC_Device_CCD01_01圓柱相似度量測_樣板相似度門檻_MinScore.Value = 0;
            if (PLC_Device_CCD01_01圓柱相似度量測_樣板相似度門檻_MinScore.Value >= 100) PLC_Device_CCD01_01圓柱相似度量測_樣板相似度門檻_MinScore.Value = 100;
        }

        #region PLC_CCD01_01_SNAP
        
        PLC_Device PLC_Device_CCD01_01_SNAP_按鈕 = new PLC_Device("M15010");
        PLC_Device PLC_Device_CCD01_01_SNAP = new PLC_Device("M15005");
        PLC_Device PLC_Device_CCD01_01_SNAP_LIVE = new PLC_Device("M15006");
        PLC_Device PLC_Device_CCD01_01_SNAP_電子快門 = new PLC_Device("F9000");
        PLC_Device PLC_Device_CCD01_01_SNAP_視訊增益 = new PLC_Device("F9001");
        PLC_Device PLC_Device_CCD01_01_SNAP_銳利度 = new PLC_Device("F9002");
        PLC_Device PLC_Device_CCD01_01_SNAP_光源亮度_紅正照 = new PLC_Device("F25000");
        PLC_Device PLC_Device_CCD01_01_SNAP_光源亮度_白正照 = new PLC_Device("F25001");
        MyTimer CCD01_01_Snap_Timer = new MyTimer();
        int cnt_Program_CCD01_01_SNAP = 65534;
        void sub_Program_CCD01_01_SNAP()
        {
            if (cnt_Program_CCD01_01_SNAP == 65534)
            {
                PLC_Device_CCD01_01_SNAP.SetComment("PLC_CCD01_01_SNAP");
                PLC_Device_CCD01_01_SNAP.Bool = false;
                PLC_Device_CCD01_01_SNAP_按鈕.Bool = false;
                CCD01_Init();
                CCD02_Init();
                cnt_Program_CCD01_01_SNAP = 65535;
            }
            if (cnt_Program_CCD01_01_SNAP == 65535) cnt_Program_CCD01_01_SNAP = 1;
            if (cnt_Program_CCD01_01_SNAP == 1) cnt_Program_CCD01_01_SNAP_檢查按下(ref cnt_Program_CCD01_01_SNAP);
            if (cnt_Program_CCD01_01_SNAP == 2) cnt_Program_CCD01_01_SNAP_初始化(ref cnt_Program_CCD01_01_SNAP);
            if (cnt_Program_CCD01_01_SNAP == 3) cnt_Program_CCD01_01_SNAP_開始取像(ref cnt_Program_CCD01_01_SNAP);
            if (cnt_Program_CCD01_01_SNAP == 4) cnt_Program_CCD01_01_SNAP_取像結束(ref cnt_Program_CCD01_01_SNAP);
            if (cnt_Program_CCD01_01_SNAP == 5) cnt_Program_CCD01_01_SNAP_繪製影像(ref cnt_Program_CCD01_01_SNAP);
            if (cnt_Program_CCD01_01_SNAP == 6) cnt_Program_CCD01_01_SNAP = 65500;
            if (cnt_Program_CCD01_01_SNAP > 1) cnt_Program_CCD01_01_SNAP_檢查放開(ref cnt_Program_CCD01_01_SNAP);

            if (cnt_Program_CCD01_01_SNAP == 65500)
            {
                PLC_Device_CCD01_01_SNAP_按鈕.Bool = false;
                PLC_Device_CCD01_01_SNAP.Bool = false;
                cnt_Program_CCD01_01_SNAP = 65535;
            }
        }
        void cnt_Program_CCD01_01_SNAP_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_01_SNAP_按鈕.Bool)
            {
                PLC_Device_CCD01_01_SNAP.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_01_SNAP_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_01_SNAP.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_01_SNAP_初始化(ref int cnt)
        {
            PLC_Device_CCD01_SNAP_電子快門.Value = PLC_Device_CCD01_01_SNAP_電子快門.Value;
            PLC_Device_CCD01_SNAP_視訊增益.Value = PLC_Device_CCD01_01_SNAP_視訊增益.Value;
            PLC_Device_CCD01_SNAP_銳利度.Value = PLC_Device_CCD01_01_SNAP_銳利度.Value;
            if (PLC_Device_CCD01_01_SNAP_光源亮度_紅正照.Value != 0)
            {
                this.光源控制(enum_光源.CCD01_紅正照, (byte)this.PLC_Device_CCD01_01_SNAP_光源亮度_紅正照.Value);
                this.光源控制(enum_光源.CCD01_紅正照, true);
            }
            else if (this.PLC_Device_CCD01_01_SNAP_光源亮度_紅正照.Value == 0)
            {
                this.光源控制(enum_光源.CCD01_紅正照, (byte)0);
                this.光源控制(enum_光源.CCD01_紅正照, false);
            }
            if (PLC_Device_CCD01_01_SNAP_光源亮度_白正照.Value != 0)
            {
                this.光源控制(enum_光源.CCD01_白正照, (byte)this.PLC_Device_CCD01_01_SNAP_光源亮度_白正照.Value);
                this.光源控制(enum_光源.CCD01_白正照, true);
            }
            else if (this.PLC_Device_CCD01_01_SNAP_光源亮度_白正照.Value == 0)
            {
                this.光源控制(enum_光源.CCD01_白正照, (byte)0);
                this.光源控制(enum_光源.CCD01_白正照, false);
            }
            cnt++;
        }
        void cnt_Program_CCD01_01_SNAP_開始取像(ref int cnt)
        {
            if (!PLC_Device_CCD01_SNAP.Bool)
            {
                PLC_Device_CCD01_SNAP.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_01_SNAP_取像結束(ref int cnt)
        {
            if (!PLC_Device_CCD01_SNAP.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_01_SNAP_繪製影像(ref int cnt)
        {
            this.CCD01_01_SrcImageHandle = this.h_Canvas_Main_CCD01_01_檢測畫面.VegaHandle;
            this.h_Canvas_Main_CCD01_01_檢測畫面.ImageCopy(this.CCD01_AxImageBW8.VegaHandle);

            this.CCD01_01_SrcImageHandle = this.h_Canvas_Tech_CCD01_01.VegaHandle;
            this.h_Canvas_Tech_CCD01_01.ImageCopy(this.CCD01_AxImageBW8.VegaHandle);
            this.h_Canvas_Tech_CCD01_01.SetImageSize(this.h_Canvas_Tech_CCD01_01.ImageWidth, this.h_Canvas_Tech_CCD01_01.ImageHeight);

            if (!PLC_Device_CCD01_01_Tech_取像並檢驗.Bool && !PLC_Device_CCD01_01_Main_取像並檢驗.Bool)
            {
                if (this.PLC_Device_CCD01_01_SNAP.Bool) this.h_Canvas_Tech_CCD01_01.RefreshCanvas();


                if (PLC_Device_CCD01_01_SNAP_LIVE.Bool)
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
        #region PLC_CCD01_01_Main_取像並檢驗
        PLC_Device PLC_Device_CCD01_01_Main_取像並檢驗按鈕 = new PLC_Device("S39900");
        PLC_Device PLC_Device_CCD01_01_Main_取像並檢驗 = new PLC_Device("S39901");
        PLC_Device PLC_Device_CCD01_01_Main_取像並檢驗_OK = new PLC_Device("S39902");
        PLC_Device PLC_Device_CCD01_01_PLC觸發檢測 = new PLC_Device("S39700");
        PLC_Device PLC_Device_CCD01_01_PLC觸發檢測完成 = new PLC_Device("S39701");
        PLC_Device PLC_Device_CCD01_01_Main_取像完成 = new PLC_Device("S39702");
        PLC_Device PLC_Device_CCD01_01_Main_BUSY = new PLC_Device("S39703");
        MyTimer CCD01_01_Init_Timer = new MyTimer();
        bool flag_CCD01_01_開始存檔 = false;
        String CCD01_01_原圖位置, CCD01_01_量測圖位置;
        PLC_Device PLC_NumBox_CCD01_01_OK最大儲存張數 = new PLC_Device("F13003");
        PLC_Device PLC_NumBox_CCD01_01_NG最大儲存張數 = new PLC_Device("F13004");
        int cnt_Program_CCD01_01_Main_取像並檢驗 = 65534;
        void sub_Program_CCD01_01_Main_取像並檢驗()
        {
            if (cnt_Program_CCD01_01_Main_取像並檢驗 == 65534)
            {
                PLC_Device_CCD01_01_Main_取像並檢驗.SetComment("PLC_CCD01_01_Main_取像並檢驗");
                PLC_Device_CCD01_01_Main_BUSY.Bool = false;
                PLC_Device_CCD01_01_Main_取像完成.Bool = false;
                PLC_Device_CCD01_01_Main_取像並檢驗.Bool = false;
                PLC_Device_CCD01_01_PLC觸發檢測.Bool = false;
                PLC_Device_CCD01_01_PLC觸發檢測完成.Bool = false;
                PLC_Device_CCD01_01_Main_取像並檢驗_OK.Bool = false;
                PLC_Device_CCD01_01_Main_取像並檢驗按鈕.Bool = false;
                cnt_Program_CCD01_01_Main_取像並檢驗 = 65535;

            }
            if (cnt_Program_CCD01_01_Main_取像並檢驗 == 65535) cnt_Program_CCD01_01_Main_取像並檢驗 = 1;
            if (cnt_Program_CCD01_01_Main_取像並檢驗 == 1) cnt_Program_CCD01_01_Main_取像並檢驗_檢查按下(ref cnt_Program_CCD01_01_Main_取像並檢驗);
            if (cnt_Program_CCD01_01_Main_取像並檢驗 == 2) cnt_Program_CCD01_01_Main_取像並檢驗_初始化(ref cnt_Program_CCD01_01_Main_取像並檢驗);
            if (cnt_Program_CCD01_01_Main_取像並檢驗 == 3) cnt_Program_CCD01_01_Main_取像並檢驗_開始SNAP(ref cnt_Program_CCD01_01_Main_取像並檢驗);
            if (cnt_Program_CCD01_01_Main_取像並檢驗 == 4) cnt_Program_CCD01_01_Main_取像並檢驗_結束SNAP(ref cnt_Program_CCD01_01_Main_取像並檢驗);
            if (cnt_Program_CCD01_01_Main_取像並檢驗 == 5) cnt_Program_CCD01_01_Main_取像並檢驗_開始計算一次(ref cnt_Program_CCD01_01_Main_取像並檢驗);
            if (cnt_Program_CCD01_01_Main_取像並檢驗 == 6) cnt_Program_CCD01_01_Main_取像並檢驗_結束計算一次(ref cnt_Program_CCD01_01_Main_取像並檢驗);
            if (cnt_Program_CCD01_01_Main_取像並檢驗 == 7) cnt_Program_CCD01_01_Main_取像並檢驗_繪製畫布(ref cnt_Program_CCD01_01_Main_取像並檢驗);
            if (cnt_Program_CCD01_01_Main_取像並檢驗 == 8) cnt_Program_CCD01_01_Main_取像並檢驗_檢查重測次數(ref cnt_Program_CCD01_01_Main_取像並檢驗);
            if (cnt_Program_CCD01_01_Main_取像並檢驗 == 9) cnt_Program_CCD01_01_Main_取像並檢驗 = 65500;
            if (cnt_Program_CCD01_01_Main_取像並檢驗 > 1) cnt_Program_CCD01_01_Main_取像並檢驗_檢查放開(ref cnt_Program_CCD01_01_Main_取像並檢驗);

            if (cnt_Program_CCD01_01_Main_取像並檢驗 == 65500)
            {
                PLC_Device_CCD01_01_Main_BUSY.Bool = false;
                PLC_Device_CCD01_01_Main_取像完成.Bool = false;
                PLC_Device_CCD01_01_Main_取像並檢驗.Bool = false;
                PLC_Device_CCD01_01_PLC觸發檢測.Bool = false;
                PLC_Device_CCD01_01_Main_取像並檢驗按鈕.Bool = false;
                cnt_Program_CCD01_01_Main_取像並檢驗 = 65535;
            }
        }
        void cnt_Program_CCD01_01_Main_取像並檢驗_檢查按下(ref int cnt)
        {

            if (PLC_Device_CCD01_01_Main_取像並檢驗按鈕.Bool || PLC_Device_CCD01_01_PLC觸發檢測.Bool)
            {
                CCD01_01_Init_Timer.TickStop();
                CCD01_01_Init_Timer.StartTickTime(100000);
                PLC_Device_CCD01_01_Main_取像並檢驗.Bool = true;
                cnt++;
            }



        }
        void cnt_Program_CCD01_01_Main_取像並檢驗_檢查放開(ref int cnt)
        {
            //if (!PLC_Device_CCD01_01_Main_取像並檢驗.Bool && !PLC_Device_CCD01_01_PLC觸發檢測.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_01_Main_取像並檢驗_初始化(ref int cnt)
        {
            PLC_Device_CCD01_01_Main_BUSY.Bool = true;
            PLC_Device_CCD01_01_PLC觸發檢測完成.Bool = false;
            cnt++;
        }
        void cnt_Program_CCD01_01_Main_取像並檢驗_開始SNAP(ref int cnt)
        {
            if (!PLC_Device_CCD01_01_SNAP.Bool)
            {
                PLC_Device_CCD01_01_SNAP_按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_01_Main_取像並檢驗_結束SNAP(ref int cnt)
        {
            if (!PLC_Device_CCD01_01_SNAP_按鈕.Bool)
            {
                光源控制(enum_光源.CCD01_紅正照, (byte)0);
                光源控制(enum_光源.CCD01_紅正照, false);
                光源控制(enum_光源.CCD01_白正照, (byte)0);
                光源控制(enum_光源.CCD01_白正照, false);
                PLC_Device_CCD01_01_Main_取像完成.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_01_Main_取像並檢驗_開始計算一次(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_01_計算一次.Bool)
            {

                this.PLC_Device_CCD01_01_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_01_Main_取像並檢驗_結束計算一次(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_01_計算一次.Bool)
            {

                Console.WriteLine($"CCD01_01檢測,耗時 {CCD01_01_Init_Timer.ToString()}");
                cnt++;
            }
        }
        void cnt_Program_CCD01_01_Main_取像並檢驗_繪製畫布(ref int cnt)
        {
            if (CCD01_01_SrcImageHandle != 0)
            {
                this.h_Canvas_Main_CCD01_01_檢測畫面.RefreshCanvas();
                PLC_Device_CCD01_01_PLC觸發檢測完成.Bool = true;
                flag_CCD01_01_開始存檔 = true;
            }
            cnt++;
        }
        void cnt_Program_CCD01_01_Main_取像並檢驗_檢查重測次數(ref int cnt)
        {
            cnt++;
        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            flag_CCD01_01_開始存檔 = true;
            flag_CCD02_01_開始存檔 = true;
        }
        private void CCD01_01_儲存圖片()
        {
            if (flag_CCD01_01_開始存檔)
            {
                String FilePlaceOK = plC_WordBox_CCD01_01_OK存圖路徑.Text;
                String FileNameOK = "CCD01_01_OK";
                String FilePlaceNG = plC_WordBox_CCD01_01_NG存圖路徑.Text;
                String FileNameNG = "CCD01_01_NG";
                儲存檔案_往後移位(FilePlaceOK, FileNameOK, PLC_NumBox_CCD01_01_OK最大儲存張數.Value);
                儲存檔案_往後移位(FilePlaceNG, FileNameNG, PLC_NumBox_CCD01_01_NG最大儲存張數.Value);
                if (PLC_Device_CCD01_01_Main_取像並檢驗_OK.Bool)
                {
                    整理檔案(FilePlaceOK, FileNameOK, PLC_NumBox_CCD01_01_OK最大儲存張數.Value);
                    FileNameOK = FileNameOK + "_OK";
                    CCD01_01_原圖位置 = CCD01_01_OK儲存檔案檢查(FilePlaceOK, FileNameOK + "_A", PLC_NumBox_CCD01_01_OK最大儲存張數.Value);
                    CCD01_01_量測圖位置 = CCD01_01_原圖位置.Replace("_A", "_B");
                    this.Invoke(new Action(delegate
                    {
                        if (plC_ComboBox_CCD01_01_OK是否存圖.SelectedIndex == 0)
                        {
                            this.h_Canvas_Main_CCD01_01_檢測畫面.SaveImage(CCD01_01_原圖位置);
                        }
                    }));
                }
                else if (!PLC_Device_CCD01_01_Main_取像並檢驗_OK.Bool)
                {
                    整理檔案(FilePlaceNG, FileNameNG, PLC_NumBox_CCD01_01_NG最大儲存張數.Value);
                    FileNameNG = FileNameNG + "_NG";
                    CCD01_01_原圖位置 = CCD01_01_NG儲存檔案檢查(FilePlaceNG, FileNameNG + "_A", PLC_NumBox_CCD01_01_NG最大儲存張數.Value);
                    CCD01_01_量測圖位置 = CCD01_01_原圖位置.Replace("_A", "_B");
                    this.Invoke(new Action(delegate
                    {
                        if (plC_ComboBox_CCD01_01_NG是否存圖.SelectedIndex == 0)
                        {
                            this.h_Canvas_Main_CCD01_01_檢測畫面.SaveImage(CCD01_01_原圖位置);
                        }
                    }));
                }
                flag_CCD01_01_開始存檔 = false;
            }
        }
        #endregion
        #region PLC_CCD01_01_Tech_取像並檢驗
        PLC_Device PLC_Device_CCD01_01_Tech_取像並檢驗按鈕 = new PLC_Device("M15610");
        PLC_Device PLC_Device_CCD01_01_Tech_取像並檢驗 = new PLC_Device("M15605");
        MyTimer CCD01_01_Tech_Timer = new MyTimer();
        int cnt_Program_CCD01_01_Tech_取像並檢驗 = 65534;
        void sub_Program_CCD01_01_Tech_取像並檢驗()
        {
            if (cnt_Program_CCD01_01_Tech_取像並檢驗 == 65534)
            {
                PLC_Device_CCD01_01_Tech_取像並檢驗.SetComment("PLC_CCD01_01_Tech_取像並檢驗");
                PLC_Device_CCD01_01_Tech_取像並檢驗按鈕.Bool = false;
                PLC_Device_CCD01_01_Tech_取像並檢驗.Bool = false;
                cnt_Program_CCD01_01_Tech_取像並檢驗 = 65535;
            }
            if (cnt_Program_CCD01_01_Tech_取像並檢驗 == 65535) cnt_Program_CCD01_01_Tech_取像並檢驗 = 1;
            if (cnt_Program_CCD01_01_Tech_取像並檢驗 == 1) cnt_Program_CCD01_01_Tech_取像並檢驗_檢查按下(ref cnt_Program_CCD01_01_Tech_取像並檢驗);
            if (cnt_Program_CCD01_01_Tech_取像並檢驗 == 2) cnt_Program_CCD01_01_Tech_取像並檢驗_初始化(ref cnt_Program_CCD01_01_Tech_取像並檢驗);
            if (cnt_Program_CCD01_01_Tech_取像並檢驗 == 3) cnt_Program_CCD01_01_Tech_取像並檢驗_SNAP一次開始(ref cnt_Program_CCD01_01_Tech_取像並檢驗);
            if (cnt_Program_CCD01_01_Tech_取像並檢驗 == 4) cnt_Program_CCD01_01_Tech_取像並檢驗_SNAP一次結束(ref cnt_Program_CCD01_01_Tech_取像並檢驗);
            if (cnt_Program_CCD01_01_Tech_取像並檢驗 == 5) cnt_Program_CCD01_01_Tech_取像並檢驗_計算一次開始(ref cnt_Program_CCD01_01_Tech_取像並檢驗);
            if (cnt_Program_CCD01_01_Tech_取像並檢驗 == 6) cnt_Program_CCD01_01_Tech_取像並檢驗_計算一次結束(ref cnt_Program_CCD01_01_Tech_取像並檢驗);
            if (cnt_Program_CCD01_01_Tech_取像並檢驗 == 7) cnt_Program_CCD01_01_Tech_取像並檢驗_繪製畫布(ref cnt_Program_CCD01_01_Tech_取像並檢驗);
            if (cnt_Program_CCD01_01_Tech_取像並檢驗 == 8) cnt_Program_CCD01_01_Tech_取像並檢驗 = 65500;
            if (cnt_Program_CCD01_01_Tech_取像並檢驗 > 1) cnt_Program_CCD01_01_Tech_取像並檢驗_檢查放開(ref cnt_Program_CCD01_01_Tech_取像並檢驗);

            if (cnt_Program_CCD01_01_Tech_取像並檢驗 == 65500)
            {
                PLC_Device_CCD01_01_Tech_取像並檢驗按鈕.Bool = false;
                PLC_Device_CCD01_01_Tech_取像並檢驗.Bool = false;
                cnt_Program_CCD01_01_Tech_取像並檢驗 = 65535;
            }
        }
        void cnt_Program_CCD01_01_Tech_取像並檢驗_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_01_Tech_取像並檢驗按鈕.Bool)
            {
                PLC_Device_CCD01_01_Tech_取像並檢驗.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_01_Tech_取像並檢驗_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_01_Tech_取像並檢驗.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_01_Tech_取像並檢驗_初始化(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_01_Tech_取像並檢驗_SNAP一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_01_SNAP.Bool)
            {
                this.PLC_Device_CCD01_01_SNAP_按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_01_Tech_取像並檢驗_SNAP一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_01_SNAP_按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_01_Tech_取像並檢驗_計算一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_01_計算一次.Bool)
            {
                this.PLC_Device_CCD01_01_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_01_Tech_取像並檢驗_計算一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_01_計算一次.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_01_Tech_取像並檢驗_繪製畫布(ref int cnt)
        {
            if (CCD01_01_SrcImageHandle != 0)
            {
                this.h_Canvas_Tech_CCD01_01.RefreshCanvas();
            }
            if (PLC_Device_CCD01_01_SNAP_LIVE.Bool)
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

            Console.WriteLine($"CCD0101檢測,耗時 {CCD01_01_Tech_Timer.ToString()}");
        }

        #endregion
        #region PLC_CCD01_01_Tech_檢驗一次
        PLC_Device PLC_Device_CCD01_01_Tech_檢驗一次按鈕 = new PLC_Device("M15310");
        PLC_Device PLC_Device_CCD01_01_Tech_檢驗一次 = new PLC_Device("M15305");
        MyTimer CCD01_01_檢驗_Timer = new MyTimer();
        int cnt_Program_CCD01_01_Tech_檢驗一次 = 65534;
        void sub_Program_CCD01_01_Tech_檢驗一次()
        {
            if (cnt_Program_CCD01_01_Tech_檢驗一次 == 65534)
            {
                PLC_Device_CCD01_01_Tech_檢驗一次.SetComment("PLC_CCD01_01_Tech_檢驗一次");
                PLC_Device_CCD01_01_Tech_檢驗一次.Bool = false;
                PLC_Device_CCD01_01_Tech_檢驗一次按鈕.Bool = false;
                cnt_Program_CCD01_01_Tech_檢驗一次 = 65535;
            }
            if (cnt_Program_CCD01_01_Tech_檢驗一次 == 65535) cnt_Program_CCD01_01_Tech_檢驗一次 = 1;
            if (cnt_Program_CCD01_01_Tech_檢驗一次 == 1) cnt_Program_CCD01_01_Tech_檢驗一次_檢查按下(ref cnt_Program_CCD01_01_Tech_檢驗一次);
            if (cnt_Program_CCD01_01_Tech_檢驗一次 == 2) cnt_Program_CCD01_01_Tech_檢驗一次_初始化(ref cnt_Program_CCD01_01_Tech_檢驗一次);
            if (cnt_Program_CCD01_01_Tech_檢驗一次 == 3) cnt_Program_CCD01_01_Tech_檢驗一次_計算一次開始(ref cnt_Program_CCD01_01_Tech_檢驗一次);
            if (cnt_Program_CCD01_01_Tech_檢驗一次 == 4) cnt_Program_CCD01_01_Tech_檢驗一次_計算一次結束(ref cnt_Program_CCD01_01_Tech_檢驗一次);
            if (cnt_Program_CCD01_01_Tech_檢驗一次 == 5) cnt_Program_CCD01_01_Tech_檢驗一次_繪製畫布(ref cnt_Program_CCD01_01_Tech_檢驗一次);
            if (cnt_Program_CCD01_01_Tech_檢驗一次 == 6) cnt_Program_CCD01_01_Tech_檢驗一次 = 65500;
            if (cnt_Program_CCD01_01_Tech_檢驗一次 > 1) cnt_Program_CCD01_01_Tech_檢驗一次_檢查放開(ref cnt_Program_CCD01_01_Tech_檢驗一次);

            if (cnt_Program_CCD01_01_Tech_檢驗一次 == 65500)
            {
                PLC_Device_CCD01_01_Tech_檢驗一次按鈕.Bool = false;
                PLC_Device_CCD01_01_Tech_檢驗一次.Bool = false;
                cnt_Program_CCD01_01_Tech_檢驗一次 = 65535;
            }
        }
        void cnt_Program_CCD01_01_Tech_檢驗一次_檢查按下(ref int cnt)
        {
            CCD01_01_檢驗_Timer.TickStop();
            CCD01_01_檢驗_Timer.StartTickTime(1000000);
            if (PLC_Device_CCD01_01_Tech_檢驗一次按鈕.Bool)
            {
                PLC_Device_CCD01_01_Tech_檢驗一次.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_01_Tech_檢驗一次_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_01_Tech_檢驗一次.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_01_Tech_檢驗一次_初始化(ref int cnt)
        {
            CCD01_01_Tech_Timer.TickStop();
            CCD01_01_Tech_Timer.StartTickTime(1000000);
            cnt++;
        }
        void cnt_Program_CCD01_01_Tech_檢驗一次_計算一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_01_計算一次.Bool)
            {
                this.PLC_Device_CCD01_01_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_01_Tech_檢驗一次_計算一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_01_計算一次.Bool)
            {
                
                cnt++;
            }
        }
        void cnt_Program_CCD01_01_Tech_檢驗一次_繪製畫布(ref int cnt)
        {
            if (CCD01_01_SrcImageHandle != 0)
            {
                Console.WriteLine($"CCD0101檢驗一次,耗時 {CCD01_01_Tech_Timer.ToString()}");
                this.h_Canvas_Tech_CCD01_01.RefreshCanvas();
            }
            
            cnt++;
        }
        #endregion
        #region PLC_CCD01_01_Main_檢驗一次
        PLC_Device PLC_Device_CCD01_01_Main_檢驗一次按鈕 = new PLC_Device("M15800");
        PLC_Device PLC_Device_CCD01_01_Main_檢驗一次 = new PLC_Device("M15801");
        int cnt_Program_CCD01_01_Main_檢驗一次 = 65534;
        void sub_Program_CCD01_01_Main_檢驗一次()
        {
            if (cnt_Program_CCD01_01_Main_檢驗一次 == 65534)
            {
                PLC_Device_CCD01_01_Main_檢驗一次.SetComment("PLC_CCD01_01_Main_檢驗一次");
                PLC_Device_CCD01_01_Main_檢驗一次.Bool = false;
                PLC_Device_CCD01_01_Main_檢驗一次按鈕.Bool = false;
                cnt_Program_CCD01_01_Main_檢驗一次 = 65535;
            }
            if (cnt_Program_CCD01_01_Main_檢驗一次 == 65535) cnt_Program_CCD01_01_Main_檢驗一次 = 1;
            if (cnt_Program_CCD01_01_Main_檢驗一次 == 1) cnt_Program_CCD01_01_Main_檢驗一次_檢查按下(ref cnt_Program_CCD01_01_Main_檢驗一次);
            if (cnt_Program_CCD01_01_Main_檢驗一次 == 2) cnt_Program_CCD01_01_Main_檢驗一次_初始化(ref cnt_Program_CCD01_01_Main_檢驗一次);
            if (cnt_Program_CCD01_01_Main_檢驗一次 == 3) cnt_Program_CCD01_01_Main_檢驗一次_計算一次開始(ref cnt_Program_CCD01_01_Main_檢驗一次);
            if (cnt_Program_CCD01_01_Main_檢驗一次 == 4) cnt_Program_CCD01_01_Main_檢驗一次_計算一次結束(ref cnt_Program_CCD01_01_Main_檢驗一次);
            if (cnt_Program_CCD01_01_Main_檢驗一次 == 5) cnt_Program_CCD01_01_Main_檢驗一次_繪製畫布(ref cnt_Program_CCD01_01_Main_檢驗一次);
            if (cnt_Program_CCD01_01_Main_檢驗一次 == 6) cnt_Program_CCD01_01_Main_檢驗一次 = 65500;
            if (cnt_Program_CCD01_01_Main_檢驗一次 > 1) cnt_Program_CCD01_01_Main_檢驗一次_檢查放開(ref cnt_Program_CCD01_01_Main_檢驗一次);

            if (cnt_Program_CCD01_01_Main_檢驗一次 == 65500)
            {
                PLC_Device_CCD01_01_Main_檢驗一次按鈕.Bool = false;
                PLC_Device_CCD01_01_Main_檢驗一次.Bool = false;
                cnt_Program_CCD01_01_Main_檢驗一次 = 65535;
            }
        }
        void cnt_Program_CCD01_01_Main_檢驗一次_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_01_Main_檢驗一次按鈕.Bool)
            {
                PLC_Device_CCD01_01_Main_檢驗一次.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_01_Main_檢驗一次_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_01_Main_檢驗一次.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_01_Main_檢驗一次_初始化(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_01_Main_檢驗一次_計算一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_01_計算一次.Bool)
            {
                this.PLC_Device_CCD01_01_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_01_Main_檢驗一次_計算一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_01_計算一次.Bool)
            {

                cnt++;
            }
        }
        void cnt_Program_CCD01_01_Main_檢驗一次_繪製畫布(ref int cnt)
        {
            if (CCD01_01_SrcImageHandle != 0)
            {
                this.h_Canvas_Main_CCD01_01_檢測畫面.RefreshCanvas();
            }

            cnt++;
        }
        #endregion
        #region PLC_CCD01_01_計算一次
        PLC_Device PLC_Device_CCD01_01_計算一次 = new PLC_Device("S5005");
        PLC_Device PLC_Device_CCD01_01_計算一次_OK = new PLC_Device("S5006");
        PLC_Device PLC_Device_CCD01_01_計算一次_READY = new PLC_Device("S5007");

        MyTimer MyTimer_CCD01_01_計算一次 = new MyTimer();
        int cnt_Program_CCD01_01_計算一次 = 65534;
        void sub_Program_CCD01_01_計算一次()
        {
            this.PLC_Device_CCD01_01_計算一次_READY.Bool = !this.PLC_Device_CCD01_01_計算一次.Bool;
            if (cnt_Program_CCD01_01_計算一次 == 65534)
            {
                PLC_Device_CCD01_01_計算一次.SetComment("PLC_CCD01_01_計算一次");
                PLC_Device_CCD01_01_計算一次.Bool = false;
                PLC_Device_CCD01_01_Main_取像並檢驗_OK.Bool = false;
                cnt_Program_CCD01_01_計算一次 = 65535;
            }
            if (cnt_Program_CCD01_01_計算一次 == 65535) cnt_Program_CCD01_01_計算一次 = 1;
            if (cnt_Program_CCD01_01_計算一次 == 1) cnt_Program_CCD01_01_計算一次_檢查按下(ref cnt_Program_CCD01_01_計算一次);
            if (cnt_Program_CCD01_01_計算一次 == 2) cnt_Program_CCD01_01_計算一次_初始化(ref cnt_Program_CCD01_01_計算一次);
            if (cnt_Program_CCD01_01_計算一次 == 3) cnt_Program_CCD01_01_計算一次_步驟01開始(ref cnt_Program_CCD01_01_計算一次);
            if (cnt_Program_CCD01_01_計算一次 == 4) cnt_Program_CCD01_01_計算一次_步驟01結束(ref cnt_Program_CCD01_01_計算一次);
            if (cnt_Program_CCD01_01_計算一次 == 5) cnt_Program_CCD01_01_計算一次_步驟02開始(ref cnt_Program_CCD01_01_計算一次);
            if (cnt_Program_CCD01_01_計算一次 == 6) cnt_Program_CCD01_01_計算一次_步驟02結束(ref cnt_Program_CCD01_01_計算一次);
            if (cnt_Program_CCD01_01_計算一次 == 7) cnt_Program_CCD01_01_計算一次_步驟03開始(ref cnt_Program_CCD01_01_計算一次);
            if (cnt_Program_CCD01_01_計算一次 == 8) cnt_Program_CCD01_01_計算一次_步驟03結束(ref cnt_Program_CCD01_01_計算一次);
            if (cnt_Program_CCD01_01_計算一次 == 9) cnt_Program_CCD01_01_計算一次_步驟04開始(ref cnt_Program_CCD01_01_計算一次);
            if (cnt_Program_CCD01_01_計算一次 == 10) cnt_Program_CCD01_01_計算一次_步驟04結束(ref cnt_Program_CCD01_01_計算一次);
            if (cnt_Program_CCD01_01_計算一次 == 11) cnt_Program_CCD01_01_計算一次_步驟05開始(ref cnt_Program_CCD01_01_計算一次);
            if (cnt_Program_CCD01_01_計算一次 == 12) cnt_Program_CCD01_01_計算一次_步驟05結束(ref cnt_Program_CCD01_01_計算一次);
            if (cnt_Program_CCD01_01_計算一次 == 13) cnt_Program_CCD01_01_計算一次_步驟06開始(ref cnt_Program_CCD01_01_計算一次);
            if (cnt_Program_CCD01_01_計算一次 == 14) cnt_Program_CCD01_01_計算一次_步驟06結束(ref cnt_Program_CCD01_01_計算一次);
            if (cnt_Program_CCD01_01_計算一次 == 15) cnt_Program_CCD01_01_計算一次_計算結果(ref cnt_Program_CCD01_01_計算一次);
            if (cnt_Program_CCD01_01_計算一次 == 16) cnt_Program_CCD01_01_計算一次 = 65500;
            if (cnt_Program_CCD01_01_計算一次 > 1) cnt_Program_CCD01_01_計算一次_檢查放開(ref cnt_Program_CCD01_01_計算一次);

            if (cnt_Program_CCD01_01_計算一次 == 65500)
            {
                PLC_Device_CCD01_01_計算一次.Bool = false;
                cnt_Program_CCD01_01_計算一次 = 65535;
            }
        }
        void cnt_Program_CCD01_01_計算一次_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_01_計算一次.Bool) cnt++;
        }
        void cnt_Program_CCD01_01_計算一次_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_01_計算一次.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_01_計算一次_初始化(ref int cnt)
        {
            PLC_Device_CCD01_01_基準線量測.Bool = false;
            PLC_Device_CCD01_01基準圓量測框調整.Bool = false;
            PLC_Device_CCD01_01圓柱相似度量測.Bool = false;
            PLC_Device_CCD01_01圓直徑量測.Bool = false;
            PLC_Device_CCD01_01_Main_取像並檢驗_OK.Bool = false;
            cnt++;
        }
        void cnt_Program_CCD01_01_計算一次_步驟01開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_01_基準線量測按鈕.Bool)
            {
                this.PLC_Device_CCD01_01_基準線量測按鈕.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_01_計算一次_步驟01結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_01_基準線量測按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_01_計算一次_步驟02開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_01基準圓量測框調整按鈕.Bool)
            {
                this.PLC_Device_CCD01_01基準圓量測框調整按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_01_計算一次_步驟02結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_01基準圓量測框調整按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_01_計算一次_步驟03開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_01圓柱相似度量測按鈕.Bool)
            {
                this.PLC_Device_CCD01_01圓柱相似度量測按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_01_計算一次_步驟03結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_01圓柱相似度量測按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_01_計算一次_步驟04開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_01圓直徑量測按鈕.Bool)
            {
                this.PLC_Device_CCD01_01圓直徑量測按鈕.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_01_計算一次_步驟04結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_01圓直徑量測按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_01_計算一次_步驟05開始(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_01_計算一次_步驟05結束(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_01_計算一次_步驟06開始(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_01_計算一次_步驟06結束(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_01_計算一次_計算結果(ref int cnt)
        {
            bool flag = true;
            if (!this.PLC_Device_CCD01_01_基準線量測_OK.Bool) flag = false;
            if (!this.PLC_Device_CCD01_01基準圓量測框調整_OK.Bool) flag = false;           
            if (!this.PLC_Device_CCD01_01圓柱相似度量測_OK.Bool) flag = false;
            if (!this.PLC_Device_CCD01_01圓直徑量測_OK.Bool) flag = false;
            this.PLC_Device_CCD01_01_計算一次_OK.Bool = flag;
            this.PLC_Device_CCD01_01_Main_取像並檢驗_OK.Bool = flag;
            //flag_CCD01_01_上端水平度寫入列表資料 = true;
            //flag_CCD01_01_上端間距寫入列表資料 = true;
            //flag_CCD01_01_上端水平度差值寫入列表資料 = true;

            cnt++;
        }
        #endregion
        #region PLC_CCD01_01_基準線量測
        AxOvkMsr.AxLineMsr CCD01_01_水平基準線量測_AxLineMsr;
        AxOvkMsr.AxLineRegression CCD01_01_水平基準線量測_AxLineRegression;
        AxOvkMsr.AxLineMsr CCD01_01_垂直基準線量測_AxLineMsr;
        AxOvkMsr.AxLineRegression CCD01_01_垂直基準線量測_AxLineRegression;
        AxOvkMsr.AxIntersectionMsr CCD01_01_基準線量測_AxIntersectionMsr;
        private PointF Point_CCD01_01_中心基準座標_量測點 = new PointF();
        PLC_Device PLC_Device_CCD01_01_基準線量測按鈕 = new PLC_Device("S6230");
        PLC_Device PLC_Device_CCD01_01_基準線量測 = new PLC_Device("S6225");
        PLC_Device PLC_Device_CCD01_01_基準線量測_OK = new PLC_Device("S6226");
        PLC_Device PLC_Device_CCD01_01_基準線量測_測試完成 = new PLC_Device("S6227");
        PLC_Device PLC_Device_CCD01_01_基準線量測_RefreshCanvas = new PLC_Device("S6228");

        PLC_Device PLC_Device_CCD01_01_基準線量測_變化銳利度 = new PLC_Device("F18000");
        PLC_Device PLC_Device_CCD01_01_基準線量測_延伸變化強度 = new PLC_Device("F18001");
        PLC_Device PLC_Device_CCD01_01_基準線量測_灰階變化面積 = new PLC_Device("F18002");
        PLC_Device PLC_Device_CCD01_01_基準線量測_雜訊抑制 = new PLC_Device("F18003");
        PLC_Device PLC_Device_CCD01_01_基準線量測_最佳回歸線計算次數 = new PLC_Device("F18004");
        PLC_Device PLC_Device_CCD01_01_基準線量測_最佳回歸線濾波 = new PLC_Device("F18005");
        PLC_Device PLC_Device_CCD01_01_基準線量測_量測顏色變化 = new PLC_Device("F18010");
        PLC_Device PLC_Device_CCD01_01_基準線量測_基準線偏移 = new PLC_Device("F18011");
        PLC_Device PLC_Device_CCD01_01_基準線量測_基準線偏移_上排 = new PLC_Device("F18018");
        PLC_Device PLC_Device_CCD01_01_基準線量測_基準線偏移_下排 = new PLC_Device("F18019");

        PLC_Device PLC_Device_CCD01_01_水平基準線量測_量測框起點X座標 = new PLC_Device("F18006");
        PLC_Device PLC_Device_CCD01_01_水平基準線量測_量測框起點Y座標 = new PLC_Device("F18007");
        PLC_Device PLC_Device_CCD01_01_水平基準線量測_量測框終點X座標 = new PLC_Device("F18008");
        PLC_Device PLC_Device_CCD01_01_水平基準線量測_量測框終點Y座標 = new PLC_Device("F18009");
        PLC_Device PLC_Device_CCD01_01_水平基準線量測_量測高度 = new PLC_Device("F18012");
        PLC_Device PLC_Device_CCD01_01_水平基準線量測_量測中心_X = new PLC_Device("F18020");
        PLC_Device PLC_Device_CCD01_01_水平基準線量測_量測中心_Y = new PLC_Device("F18021");

        PLC_Device PLC_Device_CCD01_01_垂直基準線量測_量測框起點X座標 = new PLC_Device("F18013");
        PLC_Device PLC_Device_CCD01_01_垂直基準線量測_量測框起點Y座標 = new PLC_Device("F18014");
        PLC_Device PLC_Device_CCD01_01_垂直基準線量測_量測框終點X座標 = new PLC_Device("F18015");
        PLC_Device PLC_Device_CCD01_01_垂直基準線量測_量測框終點Y座標 = new PLC_Device("F18016");
        PLC_Device PLC_Device_CCD01_01_垂直基準線量測_量測高度 = new PLC_Device("F18017");

        private void H_Canvas_Tech_CCD01_01_基準線量測_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {
            try
            {
                PointF 水平量測中心 = new PointF(Point_CCD01_01_中心基準座標_量測點.X, Point_CCD01_01_中心基準座標_量測點.Y);

                if (PLC_Device_CCD01_01_Main_取像並檢驗.Bool || PLC_Device_CCD01_01_PLC觸發檢測.Bool || PLC_Device_CCD01_01_Main_檢驗一次.Bool)
                {
                    if (this.PLC_Device_CCD01_01_基準線量測_RefreshCanvas.Bool)
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);

                        DrawingClass.Draw.水平線段繪製(0, 10000, CCD01_01_水平基準線量測_AxLineMsr.MeasuredSlope, CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotX, CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD01_01_基準線量測_基準線偏移.Value, Color.Lime, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.垂直線段繪製(0, 10000, CCD01_01_垂直基準線量測_AxLineMsr.MeasuredSlope, CCD01_01_垂直基準線量測_AxLineMsr.MeasuredPivotX, CCD01_01_垂直基準線量測_AxLineMsr.MeasuredPivotY, Color.Lime, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.十字中心(水平量測中心, 100, Color.Red, 2, g, ZoomX, ZoomY);

                        if (PLC_Device_CCD01_01_基準線量測_OK.Bool)
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
                else if(PLC_Device_CCD01_01_Tech_檢驗一次.Bool)
                {
                    if (this.PLC_Device_CCD01_01_基準線量測_RefreshCanvas.Bool)
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);

                        DrawingClass.Draw.水平線段繪製(0, 10000, CCD01_01_水平基準線量測_AxLineMsr.MeasuredSlope, CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotX, CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD01_01_基準線量測_基準線偏移.Value, Color.Lime, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.垂直線段繪製(0, 10000, CCD01_01_垂直基準線量測_AxLineMsr.MeasuredSlope, CCD01_01_垂直基準線量測_AxLineMsr.MeasuredPivotX, CCD01_01_垂直基準線量測_AxLineMsr.MeasuredPivotY, Color.Lime, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.十字中心(水平量測中心, 100, Color.Red, 2, g, ZoomX, ZoomY);
                        if(PLC_Device_CCD01_01_基準線量測_OK.Bool)
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
                    if (this.PLC_Device_CCD01_01_基準線量測_RefreshCanvas.Bool)
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);


                        if (this.plC_CheckBox_CCD01_01_基準線量測_繪製量測框.Checked)
                        {
                            this.CCD01_01_水平基準線量測_AxLineMsr.Title = ("水平基準線");
                            this.CCD01_01_水平基準線量測_AxLineMsr.DrawFrame(HDC, ZoomX, ZoomY, 0, 0);
                            this.CCD01_01_垂直基準線量測_AxLineMsr.Title = ("垂直基準線");
                            this.CCD01_01_垂直基準線量測_AxLineMsr.DrawFrame(HDC, ZoomX, ZoomY, 0, 0);
                        }
                        if (this.plC_CheckBox_CCD01_01_基準線量測_繪製量測線段.Checked)
                        {
                            this.CCD01_01_水平基準線量測_AxLineMsr.DrawFittedPrimitives(HDC, ZoomX, ZoomY, 0, 0);
                            this.CCD01_01_垂直基準線量測_AxLineMsr.DrawFittedPrimitives(HDC, ZoomX, ZoomY, 0, 0);
                            //DrawingClass.Draw.水平線段繪製(0, 10000, CCD01_01_水平基準線量測_AxLineMsr.MeasuredSlope, CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotX, CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD01_01_基準線量測_基準線偏移.Value, Color.Yellow, 2, g, ZoomX, ZoomY);
                            //DrawingClass.Draw.垂直線段繪製(0, 10000, CCD01_01_垂直基準線量測_AxLineMsr.MeasuredSlope, CCD01_01_垂直基準線量測_AxLineMsr.MeasuredPivotX, CCD01_01_垂直基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD01_01_基準線量測_基準線偏移.Value, Color.Yellow, 2, g, ZoomX, ZoomY);
                        }
                        if (this.plC_CheckBox_CCD01_01_基準線量測_繪製量測點.Checked)
                        {
                            this.CCD01_01_水平基準線量測_AxLineMsr.DrawPoints(HDC, ZoomX, ZoomY, 0, 0);
                            this.CCD01_01_垂直基準線量測_AxLineMsr.DrawPoints(HDC, ZoomX, ZoomY, 0, 0);
                        }
                        DrawingClass.Draw.十字中心(水平量測中心, 100, Color.Red, 2, g, ZoomX, ZoomY);


                        if (PLC_Device_CCD01_01_基準線量測_OK.Bool)
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

            this.PLC_Device_CCD01_01_基準線量測_RefreshCanvas.Bool = false;
        }
        private AxOvkMsr.TxAxLineMsrDragHandle CCD01_01_AxOvkMsr_水平基準線量測_TxAxLineMsrDragHandle = new AxOvkMsr.TxAxLineMsrDragHandle();
        private bool flag_CCD01_01_AxOvkMsr_水平基準線量測_MouseDown = false;
        private AxOvkMsr.TxAxLineMsrDragHandle CCD01_01_AxOvkMsr_垂直基準線量測_TxAxLineMsrDragHandle = new AxOvkMsr.TxAxLineMsrDragHandle();
        private bool flag_CCD01_01_AxOvkMsr_垂直基準線量測_MouseDown = false;

        private void H_Canvas_Tech_CCD01_01_基準線量測_OnCanvasMouseDownEvent(int x, int y, float ZoomX, float ZoomY, ref int InUsedEventNum, int InUsedCanvasHandle)
        {

            if (this.PLC_Device_CCD01_01_基準線量測.Bool)
            {
                this.CCD01_01_AxOvkMsr_水平基準線量測_TxAxLineMsrDragHandle = this.CCD01_01_水平基準線量測_AxLineMsr.HitTest(x, y, ZoomX, ZoomY, 0, 0);
                if (this.CCD01_01_AxOvkMsr_水平基準線量測_TxAxLineMsrDragHandle != AxOvkMsr.TxAxLineMsrDragHandle.AX_LINEMSR_NONE)
                {
                    this.flag_CCD01_01_AxOvkMsr_水平基準線量測_MouseDown = true;
                    InUsedEventNum = 10;
                }

                this.CCD01_01_AxOvkMsr_垂直基準線量測_TxAxLineMsrDragHandle = this.CCD01_01_垂直基準線量測_AxLineMsr.HitTest(x, y, ZoomX, ZoomY, 0, 0);
                if (this.CCD01_01_AxOvkMsr_垂直基準線量測_TxAxLineMsrDragHandle != AxOvkMsr.TxAxLineMsrDragHandle.AX_LINEMSR_NONE)
                {
                    this.flag_CCD01_01_AxOvkMsr_垂直基準線量測_MouseDown = true;
                    InUsedEventNum = 10;
                }
            }

        }
        private void H_Canvas_Tech_CCD01_01_基準線量測_OnCanvasMouseMoveEvent(int x, int y, float ZoomX, float ZoomY)
        {
            if (this.flag_CCD01_01_AxOvkMsr_水平基準線量測_MouseDown)
            {
                this.CCD01_01_水平基準線量測_AxLineMsr.DragFrame(this.CCD01_01_AxOvkMsr_水平基準線量測_TxAxLineMsrDragHandle, x, y, ZoomX, ZoomY, 0, 0);
                this.PLC_Device_CCD01_01_水平基準線量測_量測框起點X座標.Value = CCD01_01_水平基準線量測_AxLineMsr.NLineStartX;
                this.PLC_Device_CCD01_01_水平基準線量測_量測框起點Y座標.Value = CCD01_01_水平基準線量測_AxLineMsr.NLineStartY;
                this.PLC_Device_CCD01_01_水平基準線量測_量測框終點X座標.Value = CCD01_01_水平基準線量測_AxLineMsr.NLineEndX;
                this.PLC_Device_CCD01_01_水平基準線量測_量測框終點Y座標.Value = CCD01_01_水平基準線量測_AxLineMsr.NLineEndY;
                this.PLC_Device_CCD01_01_水平基準線量測_量測高度.Value = CCD01_01_水平基準線量測_AxLineMsr.HalfHeight;
            }

            if (this.flag_CCD01_01_AxOvkMsr_垂直基準線量測_MouseDown)
            {
                this.CCD01_01_垂直基準線量測_AxLineMsr.DragFrame(this.CCD01_01_AxOvkMsr_垂直基準線量測_TxAxLineMsrDragHandle, x, y, ZoomX, ZoomY, 0, 0);
                this.PLC_Device_CCD01_01_垂直基準線量測_量測框起點X座標.Value = CCD01_01_垂直基準線量測_AxLineMsr.NLineStartX;
                this.PLC_Device_CCD01_01_垂直基準線量測_量測框起點Y座標.Value = CCD01_01_垂直基準線量測_AxLineMsr.NLineStartY;
                this.PLC_Device_CCD01_01_垂直基準線量測_量測框終點X座標.Value = CCD01_01_垂直基準線量測_AxLineMsr.NLineEndX;
                this.PLC_Device_CCD01_01_垂直基準線量測_量測框終點Y座標.Value = CCD01_01_垂直基準線量測_AxLineMsr.NLineEndY;
                this.PLC_Device_CCD01_01_垂直基準線量測_量測高度.Value = CCD01_01_垂直基準線量測_AxLineMsr.HalfHeight;
            }


        }
        private void H_Canvas_Tech_CCD01_01_基準線量測_OnCanvasMouseUpEvent(int x, int y, float ZoomX, float ZoomY)
        {
            this.flag_CCD01_01_AxOvkMsr_水平基準線量測_MouseDown = false;
            this.flag_CCD01_01_AxOvkMsr_垂直基準線量測_MouseDown = false;
        }

        int cnt_Program_CCD01_01_基準線量測 = 65534;
        void sub_Program_CCD01_01_基準線量測()
        {
            if (cnt_Program_CCD01_01_基準線量測 == 65534)
            {
                this.h_Canvas_Tech_CCD01_01.OnCanvasMouseDownEvent += H_Canvas_Tech_CCD01_01_基準線量測_OnCanvasMouseDownEvent;
                this.h_Canvas_Tech_CCD01_01.OnCanvasMouseMoveEvent += H_Canvas_Tech_CCD01_01_基準線量測_OnCanvasMouseMoveEvent;
                this.h_Canvas_Tech_CCD01_01.OnCanvasMouseUpEvent += H_Canvas_Tech_CCD01_01_基準線量測_OnCanvasMouseUpEvent;
                this.h_Canvas_Tech_CCD01_01.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_01_基準線量測_OnCanvasDrawEvent;

                this.h_Canvas_Main_CCD01_01_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_01_基準線量測_OnCanvasDrawEvent;

                PLC_Device_CCD01_01_基準線量測.SetComment("PLC_CCD01_01_基準線量測");
                PLC_Device_CCD01_01_基準線量測.Bool = false;
                PLC_Device_CCD01_01_基準線量測按鈕.Bool = false;
                cnt_Program_CCD01_01_基準線量測 = 65535;
            }
            if (cnt_Program_CCD01_01_基準線量測 == 65535) cnt_Program_CCD01_01_基準線量測 = 1;
            if (cnt_Program_CCD01_01_基準線量測 == 1) cnt_Program_CCD01_01_基準線量測_檢查按下(ref cnt_Program_CCD01_01_基準線量測);
            if (cnt_Program_CCD01_01_基準線量測 == 2) cnt_Program_CCD01_01_基準線量測_初始化(ref cnt_Program_CCD01_01_基準線量測);
            if (cnt_Program_CCD01_01_基準線量測 == 3) cnt_Program_CCD01_01_基準線量測_開始量測(ref cnt_Program_CCD01_01_基準線量測);
            if (cnt_Program_CCD01_01_基準線量測 == 4) cnt_Program_CCD01_01_基準線量測_兩線交點(ref cnt_Program_CCD01_01_基準線量測);
            if (cnt_Program_CCD01_01_基準線量測 == 5) cnt_Program_CCD01_01_基準線量測_兩線交點量測(ref cnt_Program_CCD01_01_基準線量測);
            if (cnt_Program_CCD01_01_基準線量測 == 6) cnt_Program_CCD01_01_基準線量測_開始繪製(ref cnt_Program_CCD01_01_基準線量測);
            if (cnt_Program_CCD01_01_基準線量測 == 7) cnt_Program_CCD01_01_基準線量測 = 65500;
            if (cnt_Program_CCD01_01_基準線量測 > 1) cnt_Program_CCD01_01_基準線量測_檢查放開(ref cnt_Program_CCD01_01_基準線量測);

            if (cnt_Program_CCD01_01_基準線量測 == 65500)
            {
                if (PLC_Device_CCD01_01_計算一次.Bool)
                {
                    PLC_Device_CCD01_01_基準線量測按鈕.Bool = false;
                }
                PLC_Device_CCD01_01_基準線量測.Bool = false;
                cnt_Program_CCD01_01_基準線量測 = 65535;
            }
        }
        void cnt_Program_CCD01_01_基準線量測_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_01_基準線量測按鈕.Bool)
            {
                PLC_Device_CCD01_01_基準線量測.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_01_基準線量測_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_01_基準線量測.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_01_基準線量測_初始化(ref int cnt)
        {
            this.PLC_Device_CCD01_01_基準線量測_OK.Bool = false;
            this.CCD01_01_水平基準線量測_AxLineMsr.SrcImageHandle = this.CCD01_01_SrcImageHandle;
            this.CCD01_01_水平基準線量測_AxLineMsr.Hysteresis = PLC_Device_CCD01_01_基準線量測_延伸變化強度.Value;
            this.CCD01_01_水平基準線量測_AxLineMsr.DeriThreshold = PLC_Device_CCD01_01_基準線量測_變化銳利度.Value;
            this.CCD01_01_水平基準線量測_AxLineMsr.MinGreyStep = PLC_Device_CCD01_01_基準線量測_灰階變化面積.Value;
            this.CCD01_01_水平基準線量測_AxLineMsr.SmoothFactor = PLC_Device_CCD01_01_基準線量測_雜訊抑制.Value;
            this.CCD01_01_水平基準線量測_AxLineMsr.HalfProfileThickness = 5;
            this.CCD01_01_水平基準線量測_AxLineMsr.SampleStep = 1;
            this.CCD01_01_水平基準線量測_AxLineMsr.FilterCount = PLC_Device_CCD01_01_基準線量測_最佳回歸線計算次數.Value;
            this.CCD01_01_水平基準線量測_AxLineMsr.FilterThreshold = PLC_Device_CCD01_01_基準線量測_最佳回歸線濾波.Value / 10;

            if (this.PLC_Device_CCD01_01_水平基準線量測_量測框起點X座標.Value == 0 && this.PLC_Device_CCD01_01_水平基準線量測_量測框終點X座標.Value == 0)
            {
                this.PLC_Device_CCD01_01_水平基準線量測_量測框起點X座標.Value = 100;
                this.PLC_Device_CCD01_01_水平基準線量測_量測框終點X座標.Value = 100;
            }
            if (this.PLC_Device_CCD01_01_水平基準線量測_量測框起點Y座標.Value == 0 && this.PLC_Device_CCD01_01_水平基準線量測_量測框終點Y座標.Value == 0)
            {
                this.PLC_Device_CCD01_01_水平基準線量測_量測框起點Y座標.Value = 200;
                this.PLC_Device_CCD01_01_水平基準線量測_量測框終點Y座標.Value = 200;
            }
            if (this.PLC_Device_CCD01_01_水平基準線量測_量測高度.Value == 0)
            {
                this.PLC_Device_CCD01_01_水平基準線量測_量測高度.Value = 100;
            }

            this.CCD01_01_水平基準線量測_AxLineMsr.NLineStartX = PLC_Device_CCD01_01_水平基準線量測_量測框起點X座標.Value;
            this.CCD01_01_水平基準線量測_AxLineMsr.NLineStartY = PLC_Device_CCD01_01_水平基準線量測_量測框起點Y座標.Value;
            this.CCD01_01_水平基準線量測_AxLineMsr.NLineEndX = PLC_Device_CCD01_01_水平基準線量測_量測框終點X座標.Value;
            this.CCD01_01_水平基準線量測_AxLineMsr.NLineEndY = PLC_Device_CCD01_01_水平基準線量測_量測框終點Y座標.Value;
            this.CCD01_01_水平基準線量測_AxLineMsr.HalfHeight = PLC_Device_CCD01_01_水平基準線量測_量測高度.Value;

            this.CCD01_01_水平基準線量測_AxLineMsr.EdgeType = (AxOvkMsr.TxAxTransitionType)PLC_Device_CCD01_01_基準線量測_量測顏色變化.Value;
            this.CCD01_01_水平基準線量測_AxLineMsr.LockedMsrDirection = AxOvkMsr.TxAxLineMsrLockedMsrDirection.AX_LINEMSR_LOCKED_CLOCKWISE;


            this.CCD01_01_垂直基準線量測_AxLineMsr.SrcImageHandle = this.CCD01_01_SrcImageHandle;
            this.CCD01_01_垂直基準線量測_AxLineMsr.Hysteresis = PLC_Device_CCD01_01_基準線量測_延伸變化強度.Value;
            this.CCD01_01_垂直基準線量測_AxLineMsr.DeriThreshold = PLC_Device_CCD01_01_基準線量測_變化銳利度.Value;
            this.CCD01_01_垂直基準線量測_AxLineMsr.MinGreyStep = PLC_Device_CCD01_01_基準線量測_灰階變化面積.Value;
            this.CCD01_01_垂直基準線量測_AxLineMsr.SmoothFactor = PLC_Device_CCD01_01_基準線量測_雜訊抑制.Value;
            this.CCD01_01_垂直基準線量測_AxLineMsr.HalfProfileThickness = 5;
            this.CCD01_01_垂直基準線量測_AxLineMsr.SampleStep = 1;
            this.CCD01_01_垂直基準線量測_AxLineMsr.FilterCount = PLC_Device_CCD01_01_基準線量測_最佳回歸線計算次數.Value;
            this.CCD01_01_垂直基準線量測_AxLineMsr.FilterThreshold = PLC_Device_CCD01_01_基準線量測_最佳回歸線濾波.Value / 10;

            if (this.PLC_Device_CCD01_01_垂直基準線量測_量測框起點X座標.Value == 0 && this.PLC_Device_CCD01_01_垂直基準線量測_量測框終點X座標.Value == 0)
            {
                this.PLC_Device_CCD01_01_垂直基準線量測_量測框起點X座標.Value = 100;
                this.PLC_Device_CCD01_01_垂直基準線量測_量測框終點X座標.Value = 100;
            }
            if (this.PLC_Device_CCD01_01_垂直基準線量測_量測框起點Y座標.Value == 0 && this.PLC_Device_CCD01_01_垂直基準線量測_量測框終點Y座標.Value == 0)
            {
                this.PLC_Device_CCD01_01_垂直基準線量測_量測框起點Y座標.Value = 200;
                this.PLC_Device_CCD01_01_垂直基準線量測_量測框終點Y座標.Value = 200;
            }
            if (this.PLC_Device_CCD01_01_垂直基準線量測_量測高度.Value == 0)
            {
                this.PLC_Device_CCD01_01_垂直基準線量測_量測高度.Value = 100;
            }

            this.CCD01_01_垂直基準線量測_AxLineMsr.NLineStartX = PLC_Device_CCD01_01_垂直基準線量測_量測框起點X座標.Value;
            this.CCD01_01_垂直基準線量測_AxLineMsr.NLineStartY = PLC_Device_CCD01_01_垂直基準線量測_量測框起點Y座標.Value;
            this.CCD01_01_垂直基準線量測_AxLineMsr.NLineEndX = PLC_Device_CCD01_01_垂直基準線量測_量測框終點X座標.Value;
            this.CCD01_01_垂直基準線量測_AxLineMsr.NLineEndY = PLC_Device_CCD01_01_垂直基準線量測_量測框終點Y座標.Value;
            this.CCD01_01_垂直基準線量測_AxLineMsr.HalfHeight = PLC_Device_CCD01_01_垂直基準線量測_量測高度.Value;

            this.CCD01_01_垂直基準線量測_AxLineMsr.EdgeType = (AxOvkMsr.TxAxTransitionType)PLC_Device_CCD01_01_基準線量測_量測顏色變化.Value;
            this.CCD01_01_垂直基準線量測_AxLineMsr.LockedMsrDirection = AxOvkMsr.TxAxLineMsrLockedMsrDirection.AX_LINEMSR_LOCKED_CLOCKWISE;
            cnt++;

        }
        void cnt_Program_CCD01_01_基準線量測_開始量測(ref int cnt)
        {
            if (CCD01_01_SrcImageHandle != 0)
            {
                this.CCD01_01_水平基準線量測_AxLineMsr.DetectPrimitives();
                this.CCD01_01_垂直基準線量測_AxLineMsr.DetectPrimitives();
            }

            if (this.CCD01_01_水平基準線量測_AxLineMsr.LineIsFitted && this.CCD01_01_垂直基準線量測_AxLineMsr.LineIsFitted)
            {

                PointF 水平量測點p1 = new PointF();
                PointF 水平量測點p2 = new PointF();

                CCD01_01_水平基準線量測_AxLineMsr.ValidPointIndex = 0;
                水平量測點p1.X = (int)CCD01_01_水平基準線量測_AxLineMsr.ValidPointX;
                水平量測點p1.Y = (int)CCD01_01_水平基準線量測_AxLineMsr.ValidPointY;
                CCD01_01_水平基準線量測_AxLineMsr.ValidPointIndex = CCD01_01_水平基準線量測_AxLineMsr.ValidPointCount;
                水平量測點p2.X = (int)CCD01_01_水平基準線量測_AxLineMsr.ValidPointX;
                水平量測點p2.Y = (int)CCD01_01_水平基準線量測_AxLineMsr.ValidPointY;
                //Point_CCD01_01_中心基準座標_量測點.X = (int)((水平量測點p1.X + 水平量測點p2.X) / 2);
                //Point_CCD01_01_中心基準座標_量測點.Y = (int)((水平量測點p1.Y + 水平量測點p2.Y) / 2);

                PointF 水平p1 = new PointF();
                PointF 水平p2 = new PointF();
                double 水平confB;
                double 水平Slope = this.CCD01_01_水平基準線量測_AxLineMsr.MeasuredSlope;
                double 水平PivotX = this.CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotX;
                double 水平PivotY = this.CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotY;
                水平confB = Conf0Msr(水平Slope, 水平PivotX, 水平PivotY);
                水平p1.X = 1;
                水平p1.Y = (float)FunctionMsr_Y(水平confB, 水平Slope, 水平p1.X);
                水平p2.X = 10000;
                水平p2.Y = (float)FunctionMsr_Y(水平confB, 水平Slope, 水平p2.X);
                水平p1 = new PointF((水平p1.X), (水平p1.Y));
                水平p2 = new PointF((水平p2.X), (水平p2.Y));

                this.CCD01_01_水平基準線量測_AxLineRegression.RegressionOrientation = AxOvkMsr.TxAxLineRegressionOrientation.AX_QUASI_HORIZONTAL_REGRESSION;
                this.CCD01_01_水平基準線量測_AxLineRegression.PointIndex = 0;
                this.CCD01_01_水平基準線量測_AxLineRegression.PointX = 水平p1.X;
                this.CCD01_01_水平基準線量測_AxLineRegression.PointY = 水平p1.Y;
                this.CCD01_01_水平基準線量測_AxLineRegression.PointIndex = 1;
                this.CCD01_01_水平基準線量測_AxLineRegression.PointX = 水平p2.X;
                this.CCD01_01_水平基準線量測_AxLineRegression.PointY = 水平p2.Y;
                this.CCD01_01_水平基準線量測_AxLineRegression.DetectPrimitives();

                PointF 垂直p1 = new PointF();
                PointF 垂直p2 = new PointF();
                double 垂直confB;
                double 垂直Slope = this.CCD01_01_垂直基準線量測_AxLineMsr.MeasuredSlope;
                double 垂直PivotX = this.CCD01_01_垂直基準線量測_AxLineMsr.MeasuredPivotX;
                double 垂直PivotY = this.CCD01_01_垂直基準線量測_AxLineMsr.MeasuredPivotY;
                垂直confB = Conf0Msr(垂直Slope, 垂直PivotX, 垂直PivotY);
                垂直p1.X = (float)FunctionMsr_Y(垂直confB, 垂直Slope, 垂直p1.X);
                垂直p1.Y = 1;
                垂直p2.X = (float)FunctionMsr_Y(垂直confB, 垂直Slope, 垂直p2.X);
                垂直p2.Y = 10000;
                垂直p1 = new PointF((垂直p1.X), (垂直p1.Y));
                垂直p2 = new PointF((垂直p2.X), (垂直p2.Y));

                this.CCD01_01_垂直基準線量測_AxLineRegression.RegressionOrientation = AxOvkMsr.TxAxLineRegressionOrientation.AX_QUASI_VERTICAL_REGRESSION;
                this.CCD01_01_垂直基準線量測_AxLineRegression.PointIndex = 0;
                this.CCD01_01_垂直基準線量測_AxLineRegression.PointX = 垂直p1.X;
                this.CCD01_01_垂直基準線量測_AxLineRegression.PointY = 垂直p1.Y;
                this.CCD01_01_垂直基準線量測_AxLineRegression.PointIndex = 1;
                this.CCD01_01_垂直基準線量測_AxLineRegression.PointX = 垂直p2.X;
                this.CCD01_01_垂直基準線量測_AxLineRegression.PointY = 垂直p2.Y;
                this.CCD01_01_垂直基準線量測_AxLineRegression.DetectPrimitives();

                this.PLC_Device_CCD01_01_基準線量測_OK.Bool = true;
            }

            cnt++;
        }
        void cnt_Program_CCD01_01_基準線量測_兩線交點(ref int cnt)
        {
            CCD01_01_基準線量測_AxIntersectionMsr.Line1HorzVert = AxOvkMsr.TxAxLineHorzVert.AX_LINE_QUASI_HORIZONTAL;
            CCD01_01_基準線量測_AxIntersectionMsr.Line1PivotX = CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotX;
            CCD01_01_基準線量測_AxIntersectionMsr.Line1PivotY = CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotY;
            CCD01_01_基準線量測_AxIntersectionMsr.Line1Slope = CCD01_01_水平基準線量測_AxLineMsr.MeasuredSlope;

            CCD01_01_基準線量測_AxIntersectionMsr.Line2HorzVert = AxOvkMsr.TxAxLineHorzVert.AX_LINE_QUASI_VERTICAL;
            CCD01_01_基準線量測_AxIntersectionMsr.Line2PivotX = CCD01_01_垂直基準線量測_AxLineMsr.MeasuredPivotX;
            CCD01_01_基準線量測_AxIntersectionMsr.Line2PivotY = CCD01_01_垂直基準線量測_AxLineMsr.MeasuredPivotY;
            CCD01_01_基準線量測_AxIntersectionMsr.Line2Slope = CCD01_01_垂直基準線量測_AxLineMsr.MeasuredSlope;

            CCD01_01_基準線量測_AxIntersectionMsr.FindIntersection();

            cnt++;
        }
        void cnt_Program_CCD01_01_基準線量測_兩線交點量測(ref int cnt)
        {
            Point_CCD01_01_中心基準座標_量測點.X = (float)CCD01_01_基準線量測_AxIntersectionMsr.IntersectionX;
            Point_CCD01_01_中心基準座標_量測點.Y = (float)CCD01_01_基準線量測_AxIntersectionMsr.IntersectionY;

            if (!PLC_Device_CCD01_01_計算一次.Bool)
            {
                PLC_Device_CCD01_01_水平基準線量測_量測中心_X.Value = (int)CCD01_01_基準線量測_AxIntersectionMsr.IntersectionX;
                PLC_Device_CCD01_01_水平基準線量測_量測中心_Y.Value = (int)CCD01_01_基準線量測_AxIntersectionMsr.IntersectionY;
                //PLC_Device_CCD01_01_水平基準線量測_量測中心_X.Value = 2199;
                //PLC_Device_CCD01_01_水平基準線量測_量測中心_Y.Value = 1175;
            }

            cnt++;
        }
        void cnt_Program_CCD01_01_基準線量測_開始繪製(ref int cnt)
        {

            this.PLC_Device_CCD01_01_基準線量測_RefreshCanvas.Bool = true;
            if (this.PLC_Device_CCD01_01_基準線量測按鈕.Bool && !PLC_Device_CCD01_01_計算一次.Bool)
            {
                this.h_Canvas_Tech_CCD01_01.RefreshCanvas();
            }
            cnt++;
        }




        #endregion
        #region PLC_CCD01_01基準圓量測框調整

        private List<AxOvkBase.AxROIBW8> List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整 = new List<AxOvkBase.AxROIBW8>();
        private List<AxOvkBlob.AxObject> List_CCD01_01_基準圓量測_AxObject_區塊分析 = new List<AxOvkBlob.AxObject>();

        private AxOvkPat.AxVisionInspectionFrame CCD01_01基準圓AxVisionInspectionFrame_量測框調整;

        private PLC_Device PLC_Device_CCD01_01基準圓量測框調整按鈕 = new PLC_Device("S6370");
        private PLC_Device PLC_Device_CCD01_01基準圓量測框調整 = new PLC_Device("S6365");
        private PLC_Device PLC_Device_CCD01_01基準圓量測框調整_OK = new PLC_Device("S6366");
        private PLC_Device PLC_Device_CCD01_01基準圓量測框調整_測試完成 = new PLC_Device("S6367");
        private PLC_Device PLC_Device_CCD01_01基準圓量測框調整_RefreshCanvas = new PLC_Device("S6368");
        private List<PLC_Device> List_PLC_Device_CCD01_01_基準圓量測_灰階門檻值 = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_01_基準圓量測_CenterX = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_01_基準圓量測_CenterY = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_01_基準圓量測_Width = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_01_基準圓量測_Height = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_01_基準圓量測_面積上限 = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_01_基準圓量測_面積下限 = new List<PLC_Device>();

        private PLC_Device PLC_Device_CCD01_01_左基準圓量測_灰階門檻值 = new PLC_Device("F7000");
        private PLC_Device PLC_Device_CCD01_01_左基準圓量測_CenterX = new PLC_Device("F7001");
        private PLC_Device PLC_Device_CCD01_01_左基準圓量測_CenterY = new PLC_Device("F7002");
        private PLC_Device PLC_Device_CCD01_01_左基準圓量測_Width = new PLC_Device("F7003");
        private PLC_Device PLC_Device_CCD01_01_左基準圓量測_Height = new PLC_Device("F7004");

        private PLC_Device PLC_Device_CCD01_01_左基準圓量測_面積上限 = new PLC_Device("F7005");
        private PLC_Device PLC_Device_CCD01_01_左基準圓量測_面積下限 = new PLC_Device("F7006");

        private float[] List_CCD01_01_基準圓_CenterX = new float[1];
        private float[] List_CCD01_01_基準圓_CenterY = new float[1];
        private float[] List_CCD01_01_基準圓_Radius = new float[1];
        private PointF[] List_CCD01_01_基準圓_量測點 = new PointF[1];
        private PointF[] List_CCD01_01_基準圓_量測點_結果 = new PointF[1];
        private Point[] List_CCD01_01_基準圓_量測點_轉換後座標 = new Point[1];
        private bool[] List_CCD01_01_基準圓_量測點_有無 = new bool[1];
        private double 圓柱相距長度 = new double();



        private void H_Canvas_Tech_CCD01_01基準圓量測框調整_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {

            if (PLC_Device_CCD01_01_Main_取像並檢驗.Bool || PLC_Device_CCD01_01_PLC觸發檢測.Bool|| PLC_Device_CCD01_01_Main_檢驗一次.Bool)
            {
                if (this.PLC_Device_CCD01_01基準圓量測框調整_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);
                        for (int i = 0; i < 1; i++)
                        {
                            this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].ShowTitle = true;
                            this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[0].Title = "圓1";
                            this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].DrawFrame(HDC, ZoomX, ZoomY, 0, 0, 0x0000FF);
                        }
                        for (int i = 0; i < this.List_CCD01_01_基準圓_量測點.Length; i++)
                        {
                            DrawingClass.Draw.十字中心(this.List_CCD01_01_基準圓_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
                        }
                        if (PLC_Device_CCD01_01基準圓量測框調整_OK.Bool) DrawingClass.Draw.文字左上繪製("圓有無量測OK!", new PointF(0, 200), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        else DrawingClass.Draw.文字左上繪製("圓有無量測NG!", new PointF(0, 200), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }


            }
            else if (PLC_Device_CCD01_01_Tech_檢驗一次.Bool)
            {
                if (this.PLC_Device_CCD01_01基準圓量測框調整_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);
                        for (int i = 0; i < 1; i++)
                        {
                            this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].ShowTitle = true;
                            this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[0].Title = "圓1";
                            this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].DrawFrame(HDC, ZoomX, ZoomY, 0, 0, 0x0000FF);
                        }
                        for (int i = 0; i < this.List_CCD01_01_基準圓_量測點.Length; i++)
                        {
                            DrawingClass.Draw.十字中心(this.List_CCD01_01_基準圓_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
                        }
                        if (PLC_Device_CCD01_01基準圓量測框調整_OK.Bool) DrawingClass.Draw.文字左上繪製("圓有無量測OK!", new PointF(0, 200), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
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
                if (this.PLC_Device_CCD01_01基準圓量測框調整_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);


                        if (this.plC_CheckBox_CCD01_01基準圓繪製量測框.Checked)
                        {
                            for (int i = 0; i < 1; i++)
                            {
                                this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].ShowTitle = true;
                                this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[0].Title = "圓1";
                                this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].DrawFrame(HDC, ZoomX, ZoomY, 0, 0, 0x0000FF);
                            }
                        }
                        if (this.plC_CheckBox_CCD01_01基準圓繪製量測區塊.Checked)
                        {
                            for (int i = 0; i < this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整.Count; i++)
                            {
                                this.List_CCD01_01_基準圓量測_AxObject_區塊分析[i].DrawBlobs(HDC, -1, ZoomX, ZoomY, 0, 0, true, -1);
                            }

                        }
                        for (int i = 0; i < this.List_CCD01_01_基準圓_量測點.Length; i++)
                        {
                            DrawingClass.Draw.十字中心(this.List_CCD01_01_基準圓_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
                        }
                        if (PLC_Device_CCD01_01基準圓量測框調整_OK.Bool) DrawingClass.Draw.文字左上繪製("圓有無量測OK!", new PointF(0, 200), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        else DrawingClass.Draw.文字左上繪製("圓有無量測NG!", new PointF(0, 200), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }
            }

            this.PLC_Device_CCD01_01基準圓量測框調整_RefreshCanvas.Bool = false;
        }

        AxOvkBase.TxAxHitHandle[] CCD01_01基準圓AxCircleROIBW8_TxAxCircleRoiHitHandle = new AxOvkBase.TxAxHitHandle[1];
        bool[] flag_CCD01_01基準圓AxCircleROIBW8_MouseDown = new bool[1];

        private void H_Canvas_Tech_CCD01_01基準圓量測框調整_OnCanvasMouseDownEvent(int x, int y, float ZoomX, float ZoomY, ref int InUsedEventNum, int InUsedCanvasHandle)
        {
            if (this.PLC_Device_CCD01_01基準圓量測框調整.Bool)
            {
                for (int i = 0; i < 1; i++)
                {
                    this.CCD01_01基準圓AxCircleROIBW8_TxAxCircleRoiHitHandle[i] = this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].HitTest(x, y, ZoomX, ZoomY, 0, 0);
                    if (this.CCD01_01基準圓AxCircleROIBW8_TxAxCircleRoiHitHandle[i] != AxOvkBase.TxAxHitHandle.AX_HANDLE_NONE)
                    {
                        this.flag_CCD01_01基準圓AxCircleROIBW8_MouseDown[i] = true;
                        InUsedEventNum = 10;
                    }
                    
                }


            }

        }
        private void H_Canvas_Tech_CCD01_01基準圓量測框調整_OnCanvasMouseMoveEvent(int x, int y, float ZoomX, float ZoomY)
        {
            for (int i = 0; i < 1; i++)
            {
                if (this.flag_CCD01_01基準圓AxCircleROIBW8_MouseDown[i])
                {
                    this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].DragROI(this.CCD01_01基準圓AxCircleROIBW8_TxAxCircleRoiHitHandle[i], x, y, ZoomX, ZoomY, 0, 0);
                    List_PLC_Device_CCD01_01_基準圓量測_CenterX[i].Value = (int)this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].OrgX;
                    List_PLC_Device_CCD01_01_基準圓量測_CenterY[i].Value = (int)this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].OrgY;
                    List_PLC_Device_CCD01_01_基準圓量測_Height[i].Value = (int)this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].ROIHeight;
                    List_PLC_Device_CCD01_01_基準圓量測_Width[i].Value = (int)this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].ROIWidth;

                }
            }

        }
        private void H_Canvas_Tech_CCD01_01基準圓量測框調整_OnCanvasMouseUpEvent(int x, int y, float ZoomX, float ZoomY)
        {
            for (int i = 0; i < 1; i++)
            {
                this.flag_CCD01_01基準圓AxCircleROIBW8_MouseDown[i] = false;
            }
        }

        int cnt_Program_CCD01_01基準圓量測框調整 = 65534;
        void sub_Program_CCD01_01基準圓量測框調整()
        {
            if (cnt_Program_CCD01_01基準圓量測框調整 == 65534)
            {
                this.h_Canvas_Tech_CCD01_01.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_01基準圓量測框調整_OnCanvasDrawEvent;
                this.h_Canvas_Tech_CCD01_01.OnCanvasMouseDownEvent += H_Canvas_Tech_CCD01_01基準圓量測框調整_OnCanvasMouseDownEvent;
                this.h_Canvas_Tech_CCD01_01.OnCanvasMouseMoveEvent += H_Canvas_Tech_CCD01_01基準圓量測框調整_OnCanvasMouseMoveEvent;
                this.h_Canvas_Tech_CCD01_01.OnCanvasMouseUpEvent += H_Canvas_Tech_CCD01_01基準圓量測框調整_OnCanvasMouseUpEvent;

                this.h_Canvas_Main_CCD01_01_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_01基準圓量測框調整_OnCanvasDrawEvent;

                #region list add
                this.List_PLC_Device_CCD01_01_基準圓量測_灰階門檻值.Add(this.PLC_Device_CCD01_01_左基準圓量測_灰階門檻值);
                this.List_PLC_Device_CCD01_01_基準圓量測_CenterX.Add(this.PLC_Device_CCD01_01_左基準圓量測_CenterX);
                this.List_PLC_Device_CCD01_01_基準圓量測_CenterY.Add(this.PLC_Device_CCD01_01_左基準圓量測_CenterY);
                this.List_PLC_Device_CCD01_01_基準圓量測_Width.Add(this.PLC_Device_CCD01_01_左基準圓量測_Width);
                this.List_PLC_Device_CCD01_01_基準圓量測_Height.Add(this.PLC_Device_CCD01_01_左基準圓量測_Height);
                this.List_PLC_Device_CCD01_01_基準圓量測_面積上限.Add(this.PLC_Device_CCD01_01_左基準圓量測_面積上限);
                this.List_PLC_Device_CCD01_01_基準圓量測_面積下限.Add(this.PLC_Device_CCD01_01_左基準圓量測_面積下限);
                #endregion
                for (int i = 0; i < 1; i++)
                {
                    if (this.List_PLC_Device_CCD01_01_基準圓量測_灰階門檻值[i].Value == 0) this.List_PLC_Device_CCD01_01_基準圓量測_灰階門檻值[i].Value = 200;
                    if (this.List_PLC_Device_CCD01_01_基準圓量測_Height[i].Value == 0) this.List_PLC_Device_CCD01_01_基準圓量測_Height[i].Value = 150;
                    if (this.List_PLC_Device_CCD01_01_基準圓量測_Width[i].Value == 0) this.List_PLC_Device_CCD01_01_基準圓量測_Width[i].Value = 150;

                }
                
                PLC_Device_CCD01_01基準圓量測框調整.SetComment("PLC_CCD01_01基準圓量測框調整");
                PLC_Device_CCD01_01基準圓量測框調整.Bool = false;
                PLC_Device_CCD01_01基準圓量測框調整按鈕.Bool = false;
                cnt_Program_CCD01_01基準圓量測框調整 = 65535;
            }
            if (cnt_Program_CCD01_01基準圓量測框調整 == 65535) cnt_Program_CCD01_01基準圓量測框調整 = 1;
            if (cnt_Program_CCD01_01基準圓量測框調整 == 1) cnt_Program_CCD01_01基準圓量測框調整_檢查按下(ref cnt_Program_CCD01_01基準圓量測框調整);
            if (cnt_Program_CCD01_01基準圓量測框調整 == 2) cnt_Program_CCD01_01基準圓量測框調整_初始化(ref cnt_Program_CCD01_01基準圓量測框調整);
            if (cnt_Program_CCD01_01基準圓量測框調整 == 3) cnt_Program_CCD01_01基準圓量測框調整_座標轉換(ref cnt_Program_CCD01_01基準圓量測框調整);
            if (cnt_Program_CCD01_01基準圓量測框調整 == 4) cnt_Program_CCD01_01基準圓量測框調整_讀取參數(ref cnt_Program_CCD01_01基準圓量測框調整);
            if (cnt_Program_CCD01_01基準圓量測框調整 == 5) cnt_Program_CCD01_01基準圓量測框調整_開始區塊分析(ref cnt_Program_CCD01_01基準圓量測框調整);
            if (cnt_Program_CCD01_01基準圓量測框調整 == 6) cnt_Program_CCD01_01基準圓量測框調整_圓柱間距量測(ref cnt_Program_CCD01_01基準圓量測框調整);
            if (cnt_Program_CCD01_01基準圓量測框調整 == 7) cnt_Program_CCD01_01基準圓量測框調整_繪製畫布(ref cnt_Program_CCD01_01基準圓量測框調整);
            if (cnt_Program_CCD01_01基準圓量測框調整 == 8) cnt_Program_CCD01_01基準圓量測框調整 = 65500;
            if (cnt_Program_CCD01_01基準圓量測框調整 > 1) cnt_Program_CCD01_01基準圓量測框調整_檢查放開(ref cnt_Program_CCD01_01基準圓量測框調整);

            if (cnt_Program_CCD01_01基準圓量測框調整 == 65500)
            {
                if (PLC_Device_CCD01_01_計算一次.Bool)
                {
                    PLC_Device_CCD01_01基準圓量測框調整按鈕.Bool = false;
                }
                PLC_Device_CCD01_01基準圓量測框調整.Bool = false;
                cnt_Program_CCD01_01基準圓量測框調整 = 65535;
            }
        }
        void cnt_Program_CCD01_01基準圓量測框調整_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_01基準圓量測框調整按鈕.Bool)
            {
                PLC_Device_CCD01_01基準圓量測框調整.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_01基準圓量測框調整_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_01基準圓量測框調整.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_01基準圓量測框調整_初始化(ref int cnt)
        {
            this.List_CCD01_01_基準圓_量測點 = new PointF[1];
            this.List_CCD01_01_基準圓_量測點_結果 = new PointF[1];
            this.List_CCD01_01_基準圓_量測點_轉換後座標 = new Point[1];
            this.List_CCD01_01_基準圓_量測點_有無 = new bool[1];
            this.圓柱相距長度 = new double();
            this.PLC_Device_CCD01_01基準圓量測框調整_OK.Bool = false;
            cnt++;
        }
        void cnt_Program_CCD01_01基準圓量測框調整_座標轉換(ref int cnt)
        {
            if (PLC_Device_CCD01_01_計算一次.Bool)
            {
                CCD01_01基準圓AxVisionInspectionFrame_量測框調整.RefPointX = PLC_Device_CCD01_01_水平基準線量測_量測中心_X.Value;
                CCD01_01基準圓AxVisionInspectionFrame_量測框調整.RefPointY = PLC_Device_CCD01_01_水平基準線量測_量測中心_Y.Value;
                CCD01_01基準圓AxVisionInspectionFrame_量測框調整.RefAngle = 0;
                CCD01_01基準圓AxVisionInspectionFrame_量測框調整.CurrentRefPointX = Point_CCD01_01_中心基準座標_量測點.X;
                CCD01_01基準圓AxVisionInspectionFrame_量測框調整.CurrentRefPointY = Point_CCD01_01_中心基準座標_量測點.Y;
                CCD01_01基準圓AxVisionInspectionFrame_量測框調整.CurrentRefAngle = 0;
                CCD01_01基準圓AxVisionInspectionFrame_量測框調整.NumOfVisionPoints = 1;

                for (int j = 0; j < 1; j++)
                {
                    if (this.List_PLC_Device_CCD01_01_基準圓量測_CenterX[j].Value == 0) this.List_PLC_Device_CCD01_01_基準圓量測_CenterX[j].Value = 100;
                    if (this.List_PLC_Device_CCD01_01_基準圓量測_CenterY[j].Value == 0) this.List_PLC_Device_CCD01_01_基準圓量測_CenterY[j].Value = 100;
                    if (this.List_PLC_Device_CCD01_01_基準圓量測_Width[j].Value == 0) this.List_PLC_Device_CCD01_01_基準圓量測_Width[j].Value = 100;
                    if (this.List_PLC_Device_CCD01_01_基準圓量測_Height[j].Value == 0) this.List_PLC_Device_CCD01_01_基準圓量測_Height[j].Value = 100;

                    CCD01_01基準圓AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
                    CCD01_01基準圓AxVisionInspectionFrame_量測框調整.VisionPointX = this.List_PLC_Device_CCD01_01_基準圓量測_CenterX[j].Value;
                    CCD01_01基準圓AxVisionInspectionFrame_量測框調整.VisionPointY = this.List_PLC_Device_CCD01_01_基準圓量測_CenterY[j].Value;
                }
                CCD01_01基準圓AxVisionInspectionFrame_量測框調整.EstimateCurrentVisionPoints();
                for (int j = 0; j < 1; j++)
                {
                    CCD01_01基準圓AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
                    List_CCD01_01_基準圓_量測點_轉換後座標[j].X = (int)CCD01_01基準圓AxVisionInspectionFrame_量測框調整.CurrentVisionPointX;
                    List_CCD01_01_基準圓_量測點_轉換後座標[j].Y = (int)CCD01_01基準圓AxVisionInspectionFrame_量測框調整.CurrentVisionPointY;
                }
            }
            cnt++;

        }
        void cnt_Program_CCD01_01基準圓量測框調整_讀取參數(ref int cnt)
        {
            for (int i = 0; i < this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整.Count; i++)
            {
                if (this.List_PLC_Device_CCD01_01_基準圓量測_CenterX[i].Value > 2596) this.List_PLC_Device_CCD01_01_基準圓量測_CenterX[i].Value = 0;
                if (this.List_PLC_Device_CCD01_01_基準圓量測_CenterY[i].Value > 1922) this.List_PLC_Device_CCD01_01_基準圓量測_CenterY[i].Value = 0;
                if (this.List_PLC_Device_CCD01_01_基準圓量測_CenterX[i].Value < 0) this.List_PLC_Device_CCD01_01_基準圓量測_CenterX[i].Value = 0;
                if (this.List_PLC_Device_CCD01_01_基準圓量測_CenterY[i].Value < 0) this.List_PLC_Device_CCD01_01_基準圓量測_CenterY[i].Value = 0;

                if (this.List_CCD01_01_基準圓_量測點_轉換後座標[i].X > 2596) this.List_CCD01_01_基準圓_量測點_轉換後座標[i].X = 0;
                if (this.List_CCD01_01_基準圓_量測點_轉換後座標[i].Y > 1922) this.List_CCD01_01_基準圓_量測點_轉換後座標[i].Y = 0;
                if (this.List_CCD01_01_基準圓_量測點_轉換後座標[i].X < 0) this.List_CCD01_01_基準圓_量測點_轉換後座標[i].X = 0;
                if (this.List_CCD01_01_基準圓_量測點_轉換後座標[i].Y < 0) this.List_CCD01_01_基準圓_量測點_轉換後座標[i].Y = 0;

                this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].ParentHandle = this.CCD01_01_SrcImageHandle;

                if (PLC_Device_CCD01_01_計算一次.Bool)
                {
                    this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].OrgX = List_CCD01_01_基準圓_量測點_轉換後座標[i].X;
                    this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].OrgY = List_CCD01_01_基準圓_量測點_轉換後座標[i].Y;
                    this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].ROIWidth = List_PLC_Device_CCD01_01_基準圓量測_Width[i].Value;
                    this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].ROIHeight = List_PLC_Device_CCD01_01_基準圓量測_Height[i].Value;
                }
                else
                {
                    this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].OrgX = List_PLC_Device_CCD01_01_基準圓量測_CenterX[i].Value;
                    this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].OrgY = List_PLC_Device_CCD01_01_基準圓量測_CenterY[i].Value;
                    this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].ROIWidth = List_PLC_Device_CCD01_01_基準圓量測_Width[i].Value;
                    this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].ROIHeight = List_PLC_Device_CCD01_01_基準圓量測_Height[i].Value;

                }

                
            }
            cnt++;
        }
        void cnt_Program_CCD01_01基準圓量測框調整_開始區塊分析(ref int cnt)
        {
            uint object_value = 4294963615;

            for (int i = 0; i < 1; i++)
            {

                this.List_CCD01_01_基準圓量測_AxObject_區塊分析[i].SrcImageHandle = this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].VegaHandle;
                this.AxMatch_CCD01_01_圓柱相似度測試.SrcImageHandle = this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].VegaHandle;
                this.List_CCD01_01_基準圓量測_AxObject_區塊分析[i].ObjectClass = AxOvkBlob.TxAxObjClass.AX_OBJECT_DETECT_LIGHTER_CLASS;
                this.List_CCD01_01_基準圓量測_AxObject_區塊分析[i].HighThreshold = List_PLC_Device_CCD01_01_基準圓量測_灰階門檻值[i].Value;
                if (this.CCD01_01_SrcImageHandle != 0)
                {

                    this.List_CCD01_01_基準圓量測_AxObject_區塊分析[i].BlobAnalyze(false);

                }
                this.List_CCD01_01_基準圓量測_AxObject_區塊分析[i].CalculateFeatures((int)object_value, -1);
                this.List_CCD01_01_基準圓量測_AxObject_區塊分析[i].SortObjects(AxOvkBlob.TxAxObjFeatureSortOrder.AX_OBJECT_SORT_ORDER_LARGE_TO_SMALL, AxOvkBlob.TxAxObjFeature.AX_OBJECT_FEATURE_AREA, 0, -1);
                this.List_CCD01_01_基準圓量測_AxObject_區塊分析[i].SelectObjects(AxOvkBlob.TxAxObjFeature.AX_OBJECT_FEATURE_AREA, AxOvkBlob.TxAxObjFeatureOperation.AX_OBJECT_REMOVE_LESS_THAN, this.List_PLC_Device_CCD01_01_基準圓量測_面積下限[i].Value);
                this.List_CCD01_01_基準圓量測_AxObject_區塊分析[i].SelectObjects(AxOvkBlob.TxAxObjFeature.AX_OBJECT_FEATURE_AREA, AxOvkBlob.TxAxObjFeatureOperation.AX_OBJECT_REMOVE_GREAT_THAN, this.List_PLC_Device_CCD01_01_基準圓量測_面積上限[i].Value);
                if (this.List_CCD01_01_基準圓量測_AxObject_區塊分析[i].DetectedNumObjs > 0)
                {
                    this.List_CCD01_01_基準圓_量測點_有無[i] = true;
                    this.List_CCD01_01_基準圓量測_AxObject_區塊分析[i].BlobIndex = 0;
                    this.List_CCD01_01_基準圓_量測點[i].X = (float)this.List_CCD01_01_基準圓量測_AxObject_區塊分析[i].BlobCentroidX;
                    this.List_CCD01_01_基準圓_量測點[i].X += this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].OrgX;
                    this.List_CCD01_01_基準圓_量測點[i].Y = (float)this.List_CCD01_01_基準圓量測_AxObject_區塊分析[i].BlobCentroidY;
                    this.List_CCD01_01_基準圓_量測點[i].Y += this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整[i].OrgY;
                    this.PLC_Device_CCD01_01基準圓量測框調整_OK.Bool = true;
                }


            }

            cnt++;
        }
        void cnt_Program_CCD01_01基準圓量測框調整_圓柱間距量測(ref int cnt)
        {
            //double x = this.List_CCD01_01_基準圓_量測點[1].X - this.List_CCD01_01_基準圓_量測點[0].X;
            //double y = this.List_CCD01_01_基準圓_量測點[1].Y - this.List_CCD01_01_基準圓_量測點[0].Y;
            //double temp1 = Math.Pow(x, 2);
            //double temp2 = Math.Pow(y, 2);
            //double reslut = temp1 + temp2;

            //圓柱相距長度 = Math.Sqrt(reslut) * CCD01_比例尺_pixcel_To_mm;



            cnt++;
        }
        void cnt_Program_CCD01_01基準圓量測框調整_繪製畫布(ref int cnt)
        {
            this.PLC_Device_CCD01_01基準圓量測框調整_RefreshCanvas.Bool = true;
            if (this.PLC_Device_CCD01_01基準圓量測框調整按鈕.Bool && !PLC_Device_CCD01_01_計算一次.Bool)
            {
                this.h_Canvas_Tech_CCD01_01.RefreshCanvas();
            }

            cnt++;
        }





        #endregion
        #region PLC_CCD01_01圓直徑量測

        private AxOvkMsr.AxCircleMsr CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整 = new AxOvkMsr.AxCircleMsr();
        private AxOvkPat.AxVisionInspectionFrame CCD01_01基準圓直徑AxVisionInspectionFrame_量測框調整;

        private PLC_Device PLC_Device_CCD01_01圓直徑量測按鈕 = new PLC_Device("S6540");
        private PLC_Device PLC_Device_CCD01_01圓直徑量測 = new PLC_Device("S6541");
        private PLC_Device PLC_Device_CCD01_01圓直徑量測_OK = new PLC_Device("S6542");
        private PLC_Device PLC_Device_CCD01_01圓直徑量測_測試完成 = new PLC_Device("S6543");
        private PLC_Device PLC_Device_CCD01_01圓直徑量測_RefreshCanvas = new PLC_Device("S6544");

        private PLC_Device PLC_Device_CCD01_01圓直徑量測_變化銳利度門檻 = new PLC_Device("F7050");
        private PLC_Device PLC_Device_CCD01_01圓直徑量測_延伸變化強度門檻 = new PLC_Device("F7051");
        private PLC_Device PLC_Device_CCD01_01圓直徑量測_灰階面化面積門檻= new PLC_Device("F7052");
        private PLC_Device PLC_Device_CCD01_01圓直徑量測_垂直量測寬度 = new PLC_Device("F7053");
        private PLC_Device PLC_Device_CCD01_01圓直徑量測_量測密度間隔 = new PLC_Device("F7054");
        private PLC_Device PLC_Device_CCD01_01圓直徑量測_雜訊抑制 = new PLC_Device("F7055");
        private PLC_Device PLC_Device_CCD01_01圓直徑量測_最佳回歸線計算次數 = new PLC_Device("F7056");
        private PLC_Device PLC_Device_CCD01_01圓直徑量測_最佳回歸線過濾門檻 = new PLC_Device("F7057");
        private PLC_Device PLC_Device_CCD01_01圓直徑量測_外圓半徑 = new PLC_Device("F7058");
        private PLC_Device PLC_Device_CCD01_01圓直徑量測_內圓半徑 = new PLC_Device("F7059");
        private PLC_Device PLC_Device_CCD01_01圓直徑量測_CenterX = new PLC_Device("F7060");
        private PLC_Device PLC_Device_CCD01_01圓直徑量測_CenterY = new PLC_Device("F7061");
        private PLC_Device PLC_Device_CCD01_01圓直徑量測_量測顏色變化 = new PLC_Device("F7062");
        private PLC_Device PLC_Device_CCD01_01圓直徑量測_量測方向 = new PLC_Device("F7063");

        private PLC_Device PLC_Device_CCD01_01_基準圓直徑_量測上限值 = new PLC_Device("F7065");
        private PLC_Device PLC_Device_CCD01_01_基準圓直徑_量測標準值 = new PLC_Device("F7066");
        private PLC_Device PLC_Device_CCD01_01_基準圓直徑_量測下限值 = new PLC_Device("F7067");

        private float CCD01_01_基準圓直徑_CenterX = new float();
        private float CCD01_01_基準圓直徑_CenterY = new float();
        private double CCD01_01_基準圓直徑_Radius = new float();
        private Point CCD01_01_基準圓直徑_轉換後座標 = new Point();
        private bool CCD01_01_基準圓直徑_量測OK = new bool();

        private void H_Canvas_Tech_CCD01_01圓直徑量測_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {

            if (PLC_Device_CCD01_01_Main_取像並檢驗.Bool || PLC_Device_CCD01_01_PLC觸發檢測.Bool || PLC_Device_CCD01_01_Main_檢驗一次.Bool)
            {
                if (this.PLC_Device_CCD01_01圓直徑量測_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);
                        if (PLC_Device_CCD01_01圓直徑量測_OK.Bool)
                        {

                            DrawingClass.Draw.文字左上繪製(CCD01_01_基準圓直徑_Radius.ToString(),
                                new PointF(CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.CenterX, CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.CenterY + 50), new Font("標楷體", 12), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            DrawingClass.Draw.文字左上繪製("圓直徑量測OK!", new PointF(0, 400), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                        }

                        else
                        {
                            DrawingClass.Draw.文字左上繪製(CCD01_01_基準圓直徑_Radius.ToString(),
                                new PointF(CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.CenterX, CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.CenterY + 50), new Font("標楷體", 12), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            DrawingClass.Draw.文字左上繪製("圓直徑量測NG!", new PointF(0, 400), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        }
                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }


            }
            else if (PLC_Device_CCD01_01_Tech_檢驗一次.Bool)
            {
                if (this.PLC_Device_CCD01_01圓直徑量測_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);

                        if (PLC_Device_CCD01_01圓直徑量測_OK.Bool)
                        {

                            DrawingClass.Draw.文字左上繪製(CCD01_01_基準圓直徑_Radius.ToString(),
                                new PointF(CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.CenterX, CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.CenterY + 50), new Font("標楷體", 12), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            DrawingClass.Draw.文字左上繪製("圓直徑量測OK!", new PointF(0, 300), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                        }

                        else
                        {
                            DrawingClass.Draw.文字左上繪製(CCD01_01_基準圓直徑_Radius.ToString(),
                                new PointF(CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.CenterX, CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.CenterY + 50), new Font("標楷體", 12), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            DrawingClass.Draw.文字左上繪製("圓直徑量測NG!", new PointF(0, 300), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
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
                if (this.PLC_Device_CCD01_01圓直徑量測_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);


                        if (this.plC_CheckBox_CCD01_01基準圓直徑繪製量測框.Checked)
                        {
                            this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.DrawFrame(HDC, ZoomX, ZoomY, 0, 0);
                        }
                        if (this.plC_CheckBox_CCD01_01基準圓直徑繪製量測線段.Checked)
                        {
                            this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.DrawFittedPrimitives(HDC, ZoomX, ZoomY, 0, 0);

                        }
                        if (this.plC_CheckBox_CCD01_01基準圓直徑繪製量測點.Checked)
                        {
                            this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.DrawPoints(HDC, ZoomX, ZoomY, 0, 0);

                        }
                        //for (int i = 0; i < this.List_CCD01_01_基準圓_量測點.Length; i++)
                        //{
                        //    DrawingClass.Draw.十字中心(this.List_CCD01_01_基準圓_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
                        //}

                        if (PLC_Device_CCD01_01圓直徑量測_OK.Bool)
                        {

                            DrawingClass.Draw.文字左上繪製(CCD01_01_基準圓直徑_Radius.ToString(),
                                new PointF(CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.CenterX, CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.CenterY + 50), new Font("標楷體", 12), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            DrawingClass.Draw.文字左上繪製("圓直徑量測OK!", new PointF(0, 300), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                        }

                        else
                        {
                            DrawingClass.Draw.文字左上繪製(CCD01_01_基準圓直徑_Radius.ToString(),
                                new PointF(CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.CenterX, CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.CenterY + 50), new Font("標楷體", 12), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            DrawingClass.Draw.文字左上繪製("圓直徑量測NG!", new PointF(0, 300), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        }

                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }
            }

            this.PLC_Device_CCD01_01圓直徑量測_RefreshCanvas.Bool = false;
        }

        private AxOvkMsr.TxAxCircleMsrDragHandle CCD01_01基準圓直徑AxOvkMsr_TxAxCircleMsrDragHandle = new AxOvkMsr.TxAxCircleMsrDragHandle();
        bool flag_CCD01_01基準圓直徑AxCircleMsr_MouseDown = new bool();

        private void H_Canvas_Tech_CCD01_01圓直徑量測_OnCanvasMouseDownEvent(int x, int y, float ZoomX, float ZoomY, ref int InUsedEventNum, int InUsedCanvasHandle)
        {
            if (this.PLC_Device_CCD01_01圓直徑量測.Bool)
            {
                this.CCD01_01基準圓直徑AxOvkMsr_TxAxCircleMsrDragHandle = this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.HitTest(x, y, ZoomX, ZoomY, 0, 0);
                if (this.CCD01_01基準圓直徑AxOvkMsr_TxAxCircleMsrDragHandle != AxOvkMsr.TxAxCircleMsrDragHandle.AX_CIRCLEMSR_NONE)
                {
                    this.flag_CCD01_01基準圓直徑AxCircleMsr_MouseDown = true;
                    InUsedEventNum = 10;
                }
            }

        }
        private void H_Canvas_Tech_CCD01_01圓直徑量測_OnCanvasMouseMoveEvent(int x, int y, float ZoomX, float ZoomY)
        {

            if (this.flag_CCD01_01基準圓直徑AxCircleMsr_MouseDown)
            {
                this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.DragFrame(this.CCD01_01基準圓直徑AxOvkMsr_TxAxCircleMsrDragHandle, x, y, ZoomX, ZoomY, 0, 0);
                this.PLC_Device_CCD01_01圓直徑量測_CenterX.Value = this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.CenterX;
                this.PLC_Device_CCD01_01圓直徑量測_CenterY.Value = this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.CenterY;
                this.PLC_Device_CCD01_01圓直徑量測_內圓半徑.Value = this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.InnerRadius;
                this.PLC_Device_CCD01_01圓直徑量測_外圓半徑.Value = this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.OuterRadius;

            }


        }
        private void H_Canvas_Tech_CCD01_01圓直徑量測_OnCanvasMouseUpEvent(int x, int y, float ZoomX, float ZoomY)
        {
            this.flag_CCD01_01基準圓直徑AxCircleMsr_MouseDown = false;
        }

        int cnt_Program_CCD01_01圓直徑量測 = 65534;
        void sub_Program_CCD01_01圓直徑量測()
        {
            if (cnt_Program_CCD01_01圓直徑量測 == 65534)
            {
                this.h_Canvas_Tech_CCD01_01.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_01圓直徑量測_OnCanvasDrawEvent;
                this.h_Canvas_Tech_CCD01_01.OnCanvasMouseDownEvent += H_Canvas_Tech_CCD01_01圓直徑量測_OnCanvasMouseDownEvent;
                this.h_Canvas_Tech_CCD01_01.OnCanvasMouseMoveEvent += H_Canvas_Tech_CCD01_01圓直徑量測_OnCanvasMouseMoveEvent;
                this.h_Canvas_Tech_CCD01_01.OnCanvasMouseUpEvent += H_Canvas_Tech_CCD01_01圓直徑量測_OnCanvasMouseUpEvent;

                this.h_Canvas_Main_CCD01_01_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_01圓直徑量測_OnCanvasDrawEvent;


                PLC_Device_CCD01_01圓直徑量測.SetComment("PLC_CCD01_01圓直徑量測");
                PLC_Device_CCD01_01圓直徑量測.Bool = false;
                PLC_Device_CCD01_01圓直徑量測按鈕.Bool = false;
                cnt_Program_CCD01_01圓直徑量測 = 65535;
            }
            if (cnt_Program_CCD01_01圓直徑量測 == 65535) cnt_Program_CCD01_01圓直徑量測 = 1;
            if (cnt_Program_CCD01_01圓直徑量測 == 1) cnt_Program_CCD01_01圓直徑量測_檢查按下(ref cnt_Program_CCD01_01圓直徑量測);
            if (cnt_Program_CCD01_01圓直徑量測 == 2) cnt_Program_CCD01_01圓直徑量測_初始化(ref cnt_Program_CCD01_01圓直徑量測);
            if (cnt_Program_CCD01_01圓直徑量測 == 3) cnt_Program_CCD01_01圓直徑量測_座標轉換(ref cnt_Program_CCD01_01圓直徑量測);
            if (cnt_Program_CCD01_01圓直徑量測 == 4) cnt_Program_CCD01_01圓直徑量測_讀取參數(ref cnt_Program_CCD01_01圓直徑量測);
            if (cnt_Program_CCD01_01圓直徑量測 == 5) cnt_Program_CCD01_01圓直徑量測_開始量測(ref cnt_Program_CCD01_01圓直徑量測);
            if (cnt_Program_CCD01_01圓直徑量測 == 6) cnt_Program_CCD01_01圓直徑量測_量測結果(ref cnt_Program_CCD01_01圓直徑量測);
            if (cnt_Program_CCD01_01圓直徑量測 == 7) cnt_Program_CCD01_01圓直徑量測_繪製畫布(ref cnt_Program_CCD01_01圓直徑量測);
            if (cnt_Program_CCD01_01圓直徑量測 == 8) cnt_Program_CCD01_01圓直徑量測 = 65500;
            if (cnt_Program_CCD01_01圓直徑量測 > 1) cnt_Program_CCD01_01圓直徑量測_檢查放開(ref cnt_Program_CCD01_01圓直徑量測);

            if (cnt_Program_CCD01_01圓直徑量測 == 65500)
            {
                if (PLC_Device_CCD01_01_計算一次.Bool)
                {
                    PLC_Device_CCD01_01圓直徑量測按鈕.Bool = false;
                }
                PLC_Device_CCD01_01圓直徑量測.Bool = false;
                cnt_Program_CCD01_01圓直徑量測 = 65535;
            }
        }
        void cnt_Program_CCD01_01圓直徑量測_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_01圓直徑量測按鈕.Bool)
            {
                PLC_Device_CCD01_01圓直徑量測.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_01圓直徑量測_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_01圓直徑量測.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_01圓直徑量測_初始化(ref int cnt)
        {
            this.CCD01_01_基準圓直徑_轉換後座標 = new Point();
            this.PLC_Device_CCD01_01圓直徑量測_OK.Bool = false;
            cnt++;
        }
        void cnt_Program_CCD01_01圓直徑量測_座標轉換(ref int cnt)
        {
            if (PLC_Device_CCD01_01_計算一次.Bool)
            {
                CCD01_01基準圓直徑AxVisionInspectionFrame_量測框調整.RefPointX = PLC_Device_CCD01_01_水平基準線量測_量測中心_X.Value;
                CCD01_01基準圓直徑AxVisionInspectionFrame_量測框調整.RefPointY = PLC_Device_CCD01_01_水平基準線量測_量測中心_Y.Value;
                CCD01_01基準圓直徑AxVisionInspectionFrame_量測框調整.RefAngle = 0;
                CCD01_01基準圓直徑AxVisionInspectionFrame_量測框調整.CurrentRefPointX = Point_CCD01_01_中心基準座標_量測點.X;
                CCD01_01基準圓直徑AxVisionInspectionFrame_量測框調整.CurrentRefPointY = Point_CCD01_01_中心基準座標_量測點.Y;
                CCD01_01基準圓直徑AxVisionInspectionFrame_量測框調整.CurrentRefAngle = 0;
                CCD01_01基準圓直徑AxVisionInspectionFrame_量測框調整.NumOfVisionPoints = 1;


                if (this.PLC_Device_CCD01_01圓直徑量測_CenterX.Value == 0) this.PLC_Device_CCD01_01圓直徑量測_CenterX.Value = 100;
                if (this.PLC_Device_CCD01_01圓直徑量測_CenterY.Value == 0) this.PLC_Device_CCD01_01圓直徑量測_CenterY.Value = 100;

                CCD01_01基準圓直徑AxVisionInspectionFrame_量測框調整.VisionPointIndex = 0;
                CCD01_01基準圓直徑AxVisionInspectionFrame_量測框調整.VisionPointX = this.PLC_Device_CCD01_01圓直徑量測_CenterX.Value;
                CCD01_01基準圓直徑AxVisionInspectionFrame_量測框調整.VisionPointY = this.PLC_Device_CCD01_01圓直徑量測_CenterY.Value;

                CCD01_01基準圓直徑AxVisionInspectionFrame_量測框調整.EstimateCurrentVisionPoints();

                CCD01_01基準圓直徑AxVisionInspectionFrame_量測框調整.VisionPointIndex = 0;
                CCD01_01_基準圓直徑_轉換後座標.X = (int)CCD01_01基準圓直徑AxVisionInspectionFrame_量測框調整.CurrentVisionPointX;
                CCD01_01_基準圓直徑_轉換後座標.Y = (int)CCD01_01基準圓直徑AxVisionInspectionFrame_量測框調整.CurrentVisionPointY;

            }
            cnt++;

        }
        void cnt_Program_CCD01_01圓直徑量測_讀取參數(ref int cnt)
        {
            if (this.PLC_Device_CCD01_01圓直徑量測_CenterX.Value > 2596) this.PLC_Device_CCD01_01圓直徑量測_CenterX.Value = 0;
            if (this.PLC_Device_CCD01_01圓直徑量測_CenterY.Value > 1922) this.PLC_Device_CCD01_01圓直徑量測_CenterY.Value = 0;
            if (this.PLC_Device_CCD01_01圓直徑量測_CenterX.Value < 0) this.PLC_Device_CCD01_01圓直徑量測_CenterX.Value = 0;
            if (this.PLC_Device_CCD01_01圓直徑量測_CenterY.Value < 0) this.PLC_Device_CCD01_01圓直徑量測_CenterY.Value = 0;

            if (this.CCD01_01_基準圓直徑_轉換後座標.X > 2596) this.CCD01_01_基準圓直徑_轉換後座標.X = 0;
            if (this.CCD01_01_基準圓直徑_轉換後座標.Y > 1922) this.CCD01_01_基準圓直徑_轉換後座標.Y = 0;
            if (this.CCD01_01_基準圓直徑_轉換後座標.X < 0) this.CCD01_01_基準圓直徑_轉換後座標.X = 0;
            if (this.CCD01_01_基準圓直徑_轉換後座標.Y < 0) this.CCD01_01_基準圓直徑_轉換後座標.Y = 0;

            this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.SrcImageHandle = this.CCD01_01_SrcImageHandle;

            if (PLC_Device_CCD01_01_計算一次.Bool)
            {
                this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.CenterX = CCD01_01_基準圓直徑_轉換後座標.X;
                this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.CenterY = CCD01_01_基準圓直徑_轉換後座標.Y;
            }
            else
            {
                this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.CenterX = PLC_Device_CCD01_01圓直徑量測_CenterX.Value;
                this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.CenterY = PLC_Device_CCD01_01圓直徑量測_CenterY.Value;
            }
            this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.DeriThreshold = PLC_Device_CCD01_01圓直徑量測_變化銳利度門檻.Value;
            this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.Hysteresis = PLC_Device_CCD01_01圓直徑量測_延伸變化強度門檻.Value;
            this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.MinGreyStep = PLC_Device_CCD01_01圓直徑量測_灰階面化面積門檻.Value;
            this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.HalfProfileThickness = PLC_Device_CCD01_01圓直徑量測_垂直量測寬度.Value;
            this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.SmoothFactor = PLC_Device_CCD01_01圓直徑量測_雜訊抑制.Value;
            this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.SampleStep = PLC_Device_CCD01_01圓直徑量測_量測密度間隔.Value;
            this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.FilterCount = PLC_Device_CCD01_01圓直徑量測_最佳回歸線計算次數.Value;
            this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.FilterThreshold = PLC_Device_CCD01_01圓直徑量測_最佳回歸線過濾門檻.Value / 10;
            this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.StartAngle = 180;
            this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.SweepAngle = 360;
            this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.HalfProfileThickness = 8;
            this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.InnerRadius = PLC_Device_CCD01_01圓直徑量測_內圓半徑.Value;
            this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.OuterRadius = PLC_Device_CCD01_01圓直徑量測_外圓半徑.Value;
            this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.EdgeType = (AxOvkMsr.TxAxTransitionType)PLC_Device_CCD01_01圓直徑量測_量測顏色變化.Value;
            this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.Direction = (AxOvkMsr.TxAxCircleMsrDirection)PLC_Device_CCD01_01圓直徑量測_量測方向.Value;

            this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.DetectPrimitives();
            cnt++;
        }
        void cnt_Program_CCD01_01圓直徑量測_開始量測(ref int cnt)
        {
            this.PLC_Device_CCD01_01圓直徑量測_OK.Bool = true;
            if (CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.CircleIsFitted == true)
            {
                this.CCD01_01_基準圓直徑_Radius = this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整.MeasuredRadius * 2 * this.CCD01_比例尺_pixcel_To_mm;
                this.CCD01_01_基準圓直徑_Radius = Math.Round(this.CCD01_01_基準圓直徑_Radius, 3);
            }
            else this.PLC_Device_CCD01_01圓直徑量測_OK.Bool = false;
            cnt++;
        }
        void cnt_Program_CCD01_01圓直徑量測_量測結果(ref int cnt)
        {
            int 標準值 = this.PLC_Device_CCD01_01_基準圓直徑_量測標準值.Value;
            int 標準值上限 = this.PLC_Device_CCD01_01_基準圓直徑_量測上限值.Value;
            int 標準值下限 = this.PLC_Device_CCD01_01_基準圓直徑_量測下限值.Value;
            double 量測距離 = this.CCD01_01_基準圓直徑_Radius;

            量測距離 = 量測距離 * 1000 - 標準值;
            量測距離 /= 1000;
            if (!PLC_Device_CCD01_02_PIN量測_間距量測不測試.Bool)
            {
                if (量測距離 >= 0)
                {
                    if (標準值上限 <= Math.Abs(量測距離) * 1000 || 標準值下限 > Math.Abs(量測距離) * 1000)
                    {

                        PLC_Device_CCD01_01圓直徑量測_OK.Bool = false;
                    }
                    else
                    {
                        this.PLC_Device_CCD01_01圓直徑量測_OK.Bool = true;
                    }
                }
            }
            else
            {
                this.PLC_Device_CCD01_01圓直徑量測_OK.Bool = true;
            }

            cnt++;
        }
        void cnt_Program_CCD01_01圓直徑量測_繪製畫布(ref int cnt)
        {
            this.PLC_Device_CCD01_01圓直徑量測_RefreshCanvas.Bool = true;
            if (this.PLC_Device_CCD01_01圓直徑量測按鈕.Bool && !PLC_Device_CCD01_01_計算一次.Bool)
            {
                this.h_Canvas_Tech_CCD01_01.RefreshCanvas();
            }
            cnt++;
        }





        #endregion
        #region PLC_CCD01_01圓柱相似度量測

        private AxOvkPat.AxMatch AxMatch_CCD01_01_圓柱相似度測試 = new AxOvkPat.AxMatch();
        private AxOvkImage.AxImageCopier AxImageCopier_CCD01_01_圓柱相似度測試_GetPattern = new AxOvkImage.AxImageCopier();
        private AxOvkBase.AxImageBW8 CCD01_01_圓柱相似度測試_GetPattern_AxImageBW8 = new AxOvkBase.AxImageBW8();

        private PLC_Device PLC_Device_CCD01_01圓柱相似度量測按鈕 = new PLC_Device("S6500");
        private PLC_Device PLC_Device_CCD01_01圓柱相似度量測 = new PLC_Device("S6505");
        private PLC_Device PLC_Device_CCD01_01圓柱相似度量測_OK = new PLC_Device("S6506");
        private PLC_Device PLC_Device_CCD01_01圓柱相似度量測_測試完成 = new PLC_Device("S6507");
        private PLC_Device PLC_Device_CCD01_01圓柱相似度量測_RefreshCanvas = new PLC_Device("S6508");

        private PLC_Device PLC_Device_CCD01_01圓柱相似度量測_樣板取樣細緻度_MinReducedArea = new PLC_Device("F7030");
        private PLC_Device PLC_Device_CCD01_01圓柱相似度量測_樣板相似度門檻_MinScore = new PLC_Device("F7031");
        private PLC_Device PLC_Device_CCD01_01_圓柱相似度量測_樣本圖片辨識代碼 = new PLC_Device("F7032");
        string CCD01_01_樣板儲存名稱 = "CCD1-1_PAT";
        float Match_CCD0101_Score;

        private void H_Canvas_Tech_CCD01_01圓柱相似度量測_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {
             PointF 相似度數值顯示 = new PointF(List_CCD01_01_基準圓_量測點[0].X + 100, List_CCD01_01_基準圓_量測點[0].Y - 150);


            if (PLC_Device_CCD01_01_Main_取像並檢驗.Bool || PLC_Device_CCD01_01_PLC觸發檢測.Bool || PLC_Device_CCD01_01_Main_檢驗一次.Bool)
            {
                if (this.PLC_Device_CCD01_01圓柱相似度量測_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        this.AxMatch_CCD01_01_圓柱相似度測試.DrawMatchedPattern(HDC, -1, ZoomX, ZoomY, 0, 0);
                        if (PLC_Device_CCD01_01圓柱相似度量測_OK.Bool) DrawingClass.Draw.文字左上繪製("圓量測OK!", new PointF(0, 600), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        else DrawingClass.Draw.文字左上繪製("圓量測NG!", new PointF(0, 600), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);

                        if (PLC_Device_CCD01_01圓柱相似度量測_OK.Bool) DrawingClass.Draw.文字左上繪製((this.Match_CCD0101_Score * 100).ToString("0.0") + "%", 相似度數值顯示, new Font("標楷體", 12), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        else DrawingClass.Draw.文字左上繪製(this.Match_CCD0101_Score.ToString() + "%", 相似度數值顯示, new Font("標楷體", 12), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }


            }
            else if (PLC_Device_CCD01_01_Tech_檢驗一次.Bool)
            {
                if (this.PLC_Device_CCD01_01圓柱相似度量測_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        this.AxMatch_CCD01_01_圓柱相似度測試.DrawMatchedPattern(HDC, -1, ZoomX, ZoomY, 0, 0);
                        if (PLC_Device_CCD01_01圓柱相似度量測_OK.Bool) DrawingClass.Draw.文字左上繪製("圓量測OK!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        else DrawingClass.Draw.文字左上繪製("圓量測NG!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);

                        if (PLC_Device_CCD01_01圓柱相似度量測_OK.Bool) DrawingClass.Draw.文字左上繪製((this.Match_CCD0101_Score * 100).ToString("0.0") + "%", 相似度數值顯示, new Font("標楷體", 12), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        else DrawingClass.Draw.文字左上繪製(this.Match_CCD0101_Score.ToString() + "%", 相似度數值顯示, new Font("標楷體", 12), Color.Black, Color.Red, g, ZoomX, ZoomY);
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
                if (this.PLC_Device_CCD01_01圓柱相似度量測_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        this.AxMatch_CCD01_01_圓柱相似度測試.DrawMatchedPattern(HDC, -1, ZoomX, ZoomY, 0, 0);
                        if (PLC_Device_CCD01_01圓柱相似度量測_OK.Bool) DrawingClass.Draw.文字左上繪製("圓量測OK!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        else DrawingClass.Draw.文字左上繪製("圓量測NG!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);

                        if (PLC_Device_CCD01_01圓柱相似度量測_OK.Bool) DrawingClass.Draw.文字左上繪製((this.Match_CCD0101_Score * 100).ToString("0.0") + "%", 相似度數值顯示, new Font("標楷體", 12), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        else DrawingClass.Draw.文字左上繪製(this.Match_CCD0101_Score.ToString() + "%", 相似度數值顯示, new Font("標楷體", 12), Color.Black, Color.Red, g, ZoomX, ZoomY);

                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }
            }

            this.PLC_Device_CCD01_01圓柱相似度量測_RefreshCanvas.Bool = false;
        }
        int cnt_Program_CCD01_01圓柱相似度量測 = 65534;
        void sub_Program_CCD01_01圓柱相似度量測()
        {
            if (cnt_Program_CCD01_01圓柱相似度量測 == 65534)
            {
                this.h_Canvas_Tech_CCD01_01.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_01圓柱相似度量測_OnCanvasDrawEvent;
                this.h_Canvas_Main_CCD01_01_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_01圓柱相似度量測_OnCanvasDrawEvent;
                this.

                PLC_Device_CCD01_01圓柱相似度量測.SetComment("PLC_CCD01_01圓柱相似度量測");
                PLC_Device_CCD01_01圓柱相似度量測.Bool = false;
                PLC_Device_CCD01_01圓柱相似度量測按鈕.Bool = false;
                cnt_Program_CCD01_01圓柱相似度量測 = 65535;
            }
            if (cnt_Program_CCD01_01圓柱相似度量測 == 65535) cnt_Program_CCD01_01圓柱相似度量測 = 1;
            if (cnt_Program_CCD01_01圓柱相似度量測 == 1) cnt_Program_CCD01_01圓柱相似度量測_檢查按下(ref cnt_Program_CCD01_01圓柱相似度量測);
            if (cnt_Program_CCD01_01圓柱相似度量測 == 2) cnt_Program_CCD01_01圓柱相似度量測_比對範圍設定開始(ref cnt_Program_CCD01_01圓柱相似度量測);
            if (cnt_Program_CCD01_01圓柱相似度量測 == 3) cnt_Program_CCD01_01圓柱相似度量測_比對範圍設定結束(ref cnt_Program_CCD01_01圓柱相似度量測);
            if (cnt_Program_CCD01_01圓柱相似度量測 == 4) cnt_Program_CCD01_01圓柱相似度量測_初始化(ref cnt_Program_CCD01_01圓柱相似度量測);
            if (cnt_Program_CCD01_01圓柱相似度量測 == 5) cnt_Program_CCD01_01圓柱相似度量測_搜尋樣板(ref cnt_Program_CCD01_01圓柱相似度量測);
            if (cnt_Program_CCD01_01圓柱相似度量測 == 6) cnt_Program_CCD01_01圓柱相似度量測_繪製畫布(ref cnt_Program_CCD01_01圓柱相似度量測);
            if (cnt_Program_CCD01_01圓柱相似度量測 == 7) cnt_Program_CCD01_01圓柱相似度量測 = 65500;
            if (cnt_Program_CCD01_01圓柱相似度量測 > 1) cnt_Program_CCD01_01圓柱相似度量測_檢查放開(ref cnt_Program_CCD01_01圓柱相似度量測);

            if (cnt_Program_CCD01_01圓柱相似度量測 == 65500)
            {
                PLC_Device_CCD01_01圓柱相似度量測.Bool = false;
                PLC_Device_CCD01_01圓柱相似度量測按鈕.Bool = false;
                cnt_Program_CCD01_01圓柱相似度量測 = 65535;
            }
        }
        void cnt_Program_CCD01_01圓柱相似度量測_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_01圓柱相似度量測按鈕.Bool)
            {
                PLC_Device_CCD01_01圓柱相似度量測.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_01圓柱相似度量測_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_01圓柱相似度量測.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_01圓柱相似度量測_比對範圍設定開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_01比對樣板範圍.Bool)
            {
                this.PLC_Device_CCD01_01比對樣板範圍.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_01圓柱相似度量測_比對範圍設定結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_01比對樣板範圍.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_01圓柱相似度量測_初始化(ref int cnt)
        {
            this.Match_CCD0101_Score = new float();
            this.PLC_Device_CCD01_01圓柱相似度量測_OK.Bool = false;
            this.AxMatch_CCD01_01_圓柱相似度測試.DstImageHandle = AxROIBW8_CCD01_01_比對樣板範圍.VegaHandle;
            this.AxMatch_CCD01_01_圓柱相似度測試.PositionType = AxOvkPat.TxAxMatchPositionType.AX_MATCH_POSITION_TYPE_CENTER;
            this.AxMatch_CCD01_01_圓柱相似度測試.MaxPositions = 1;
            this.AxMatch_CCD01_01_圓柱相似度測試.MinScore = (float)PLC_Device_CCD01_01圓柱相似度量測_樣板相似度門檻_MinScore.Value / 100;
            this.AxMatch_CCD01_01_圓柱相似度測試.Match();
            cnt++;
        }
        void cnt_Program_CCD01_01圓柱相似度量測_搜尋樣板(ref int cnt)
        {
            bool effect = this.AxMatch_CCD01_01_圓柱相似度測試.EffectMatch;
            int num = this.AxMatch_CCD01_01_圓柱相似度測試.NumMatchedPos;
            this.AxMatch_CCD01_01_圓柱相似度測試.PatternIndex = 0;
            this.Match_CCD0101_Score = this.AxMatch_CCD01_01_圓柱相似度測試.MatchedScore;
            cnt++;

        }
        void cnt_Program_CCD01_01圓柱相似度量測_繪製畫布(ref int cnt)
        {
            if(CCD01_01_SrcImageHandle != 0)
            {
                AxMatch_CCD01_01_圓柱相似度測試.PatternIndex = 0;
                if (AxMatch_CCD01_01_圓柱相似度測試.EffectMatch)
                {
                    PLC_Device_CCD01_01圓柱相似度量測_OK.Bool = true;
                }

                this.PLC_Device_CCD01_01圓柱相似度量測_RefreshCanvas.Bool = true;
                if (this.PLC_Device_CCD01_01圓柱相似度量測按鈕.Bool && !PLC_Device_CCD01_01_計算一次.Bool)
                {
                    this.h_Canvas_Tech_CCD01_01.RefreshCanvas();
                }
            }

            cnt++;
        }

        #endregion
        #region PLC_CCD01_01比對樣板範圍
        private AxOvkBase.AxROIBW8 AxROIBW8_CCD01_01_比對樣板範圍 = new AxOvkBase.AxROIBW8();
        private PLC_Device PLC_Device_CCD01_01比對樣板範圍按鈕 = new PLC_Device("S6510");
        private PLC_Device PLC_Device_CCD01_01比對樣板範圍 = new PLC_Device("S6515");
        private PLC_Device PLC_Device_CCD01_01比對樣板範圍_OK = new PLC_Device("S6516");
        private PLC_Device PLC_Device_CCD01_01比對樣板範圍_測試完成 = new PLC_Device("S6517");
        private PLC_Device PLC_Device_CCD01_01比對樣板範圍_RefreshCanvas = new PLC_Device("S6518");

        private PLC_Device PLC_Device_CCD01_01比對樣板範圍_CenterX = new PLC_Device("F7035");
        private PLC_Device PLC_Device_CCD01_01比對樣板範圍_CenterY = new PLC_Device("F7036");
        private PLC_Device PLC_Device_CCD01_01比對樣板範圍_Width = new PLC_Device("F7037");
        private PLC_Device PLC_Device_CCD01_01比對樣板範圍_Height = new PLC_Device("F7038");

        private void H_Canvas_Tech_CCD01_01比對樣板範圍_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {

            if (PLC_Device_CCD01_01_Main_取像並檢驗.Bool || PLC_Device_CCD01_01_PLC觸發檢測.Bool)
            {
                if (this.PLC_Device_CCD01_01比對樣板範圍_RefreshCanvas.Bool)
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
            else if (PLC_Device_CCD01_01_Tech_檢驗一次.Bool)
            {
                if (this.PLC_Device_CCD01_01比對樣板範圍_RefreshCanvas.Bool)
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
                if (this.PLC_Device_CCD01_01比對樣板範圍_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);
                        this.AxROIBW8_CCD01_01_比對樣板範圍.ShowTitle = true;
                        this.AxROIBW8_CCD01_01_比對樣板範圍.Title = "樣板量測範圍";
                        this.AxROIBW8_CCD01_01_比對樣板範圍.DrawFrame(HDC, ZoomX, ZoomY, 0, 0, 0X0000FF);
                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }
            }

            this.PLC_Device_CCD01_01比對樣板範圍_RefreshCanvas.Bool = false;
        }

        AxOvkBase.TxAxHitHandle CCD01_01樣板比對範圍AxROIBW8_TxAxRoiHitHandle = new AxOvkBase.TxAxHitHandle();
        bool flag_CCD01_01_樣板比對範圍AxROIBW8_MouseDown = new bool();
        private void H_Canvas_Tech_CCD01_01比對樣板範圍_OnCanvasMouseDownEvent(int x, int y, float ZoomX, float ZoomY, ref int InUsedEventNum, int InUsedCanvasHandle)
        {
            if (this.PLC_Device_CCD01_01比對樣板範圍.Bool)
            {
                this.CCD01_01樣板比對範圍AxROIBW8_TxAxRoiHitHandle = this.AxROIBW8_CCD01_01_比對樣板範圍.HitTest(x, y, ZoomX, ZoomY, 0, 0);
                if(this.CCD01_01樣板比對範圍AxROIBW8_TxAxRoiHitHandle != AxOvkBase.TxAxHitHandle.AX_HANDLE_NONE)
                {
                    this.flag_CCD01_01_樣板比對範圍AxROIBW8_MouseDown = true;
                    InUsedEventNum = 10;
                }
            }

        }
        private void H_Canvas_Tech_CCD01_01比對樣板範圍_OnCanvasMouseMoveEvent(int x, int y, float ZoomX, float ZoomY)
        {
            if(this.flag_CCD01_01_樣板比對範圍AxROIBW8_MouseDown)
            {
                this.AxROIBW8_CCD01_01_比對樣板範圍.DragROI(CCD01_01樣板比對範圍AxROIBW8_TxAxRoiHitHandle, x, y, ZoomX, ZoomY, 0, 0);
                this.PLC_Device_CCD01_01比對樣板範圍_CenterX.Value = this.AxROIBW8_CCD01_01_比對樣板範圍.OrgX;
                this.PLC_Device_CCD01_01比對樣板範圍_CenterY.Value = this.AxROIBW8_CCD01_01_比對樣板範圍.OrgY;
                this.PLC_Device_CCD01_01比對樣板範圍_Width.Value = this.AxROIBW8_CCD01_01_比對樣板範圍.ROIWidth;
                this.PLC_Device_CCD01_01比對樣板範圍_Height.Value = this.AxROIBW8_CCD01_01_比對樣板範圍.ROIHeight;

            }

        }
        private void H_Canvas_Tech_CCD01_01比對樣板範圍_OnCanvasMouseUpEvent(int x, int y, float ZoomX, float ZoomY)
        {

            this.flag_CCD01_01_樣板比對範圍AxROIBW8_MouseDown = false;
            
        }
        int cnt_Program_CCD01_01比對樣板範圍 = 65534;
        void sub_Program_CCD01_01比對樣板範圍()
        {
            if (cnt_Program_CCD01_01比對樣板範圍 == 65534)
            {
                this.h_Canvas_Tech_CCD01_01.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_01比對樣板範圍_OnCanvasDrawEvent;
                this.h_Canvas_Tech_CCD01_01.OnCanvasMouseDownEvent += H_Canvas_Tech_CCD01_01比對樣板範圍_OnCanvasMouseDownEvent;
                this.h_Canvas_Tech_CCD01_01.OnCanvasMouseMoveEvent += H_Canvas_Tech_CCD01_01比對樣板範圍_OnCanvasMouseMoveEvent;
                this.h_Canvas_Tech_CCD01_01.OnCanvasMouseUpEvent += H_Canvas_Tech_CCD01_01比對樣板範圍_OnCanvasMouseUpEvent;

                this.h_Canvas_Main_CCD01_01_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_01比對樣板範圍_OnCanvasDrawEvent;


                    if (this.PLC_Device_CCD01_01比對樣板範圍_Height.Value == 0) this.PLC_Device_CCD01_01比對樣板範圍_Height.Value = 150;
                    if (this.PLC_Device_CCD01_01比對樣板範圍_Width.Value == 0) this.PLC_Device_CCD01_01比對樣板範圍_Width.Value = 150;

                

                PLC_Device_CCD01_01比對樣板範圍.SetComment("PLC_CCD01_01比對樣板範圍");
                PLC_Device_CCD01_01比對樣板範圍.Bool = false;
                PLC_Device_CCD01_01比對樣板範圍按鈕.Bool = false;
                cnt_Program_CCD01_01比對樣板範圍 = 65535;
            }
            if (cnt_Program_CCD01_01比對樣板範圍 == 65535) cnt_Program_CCD01_01比對樣板範圍 = 1;
            if (cnt_Program_CCD01_01比對樣板範圍 == 1) cnt_Program_CCD01_01比對樣板範圍_檢查按下(ref cnt_Program_CCD01_01比對樣板範圍);
            if (cnt_Program_CCD01_01比對樣板範圍 == 2) cnt_Program_CCD01_01比對樣板範圍_初始化(ref cnt_Program_CCD01_01比對樣板範圍);
            if (cnt_Program_CCD01_01比對樣板範圍 == 3) cnt_Program_CCD01_01比對樣板範圍_生成量測框(ref cnt_Program_CCD01_01比對樣板範圍);
            if (cnt_Program_CCD01_01比對樣板範圍 == 4) cnt_Program_CCD01_01比對樣板範圍_繪製畫布(ref cnt_Program_CCD01_01比對樣板範圍);
            if (cnt_Program_CCD01_01比對樣板範圍 == 5) cnt_Program_CCD01_01比對樣板範圍 = 65500;
            if (cnt_Program_CCD01_01比對樣板範圍 > 1) cnt_Program_CCD01_01比對樣板範圍_檢查放開(ref cnt_Program_CCD01_01比對樣板範圍);

            if (cnt_Program_CCD01_01比對樣板範圍 == 65500)
            {
                PLC_Device_CCD01_01比對樣板範圍.Bool = false;
                cnt_Program_CCD01_01比對樣板範圍 = 65535;
            }
        }
        void cnt_Program_CCD01_01比對樣板範圍_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_01比對樣板範圍按鈕.Bool || PLC_Device_CCD01_01圓柱相似度量測按鈕.Bool)
            {
                PLC_Device_CCD01_01比對樣板範圍.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_01比對樣板範圍_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_01比對樣板範圍.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_01比對樣板範圍_初始化(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_01比對樣板範圍_生成量測框(ref int cnt)
        {

            if (this.PLC_Device_CCD01_01比對樣板範圍_CenterX.Value > 2596) this.PLC_Device_CCD01_01比對樣板範圍_CenterX.Value = 0;
            if (this.PLC_Device_CCD01_01比對樣板範圍_CenterY.Value > 1922) this.PLC_Device_CCD01_01比對樣板範圍_CenterY.Value = 0;
            if (this.PLC_Device_CCD01_01比對樣板範圍_Width.Value < 0) this.PLC_Device_CCD01_01比對樣板範圍_Width.Value = 0;
            if (this.PLC_Device_CCD01_01比對樣板範圍_Height.Value < 0) this.PLC_Device_CCD01_01比對樣板範圍_Height.Value = 0;          

            this.AxROIBW8_CCD01_01_比對樣板範圍.ParentHandle = this.CCD01_01_SrcImageHandle;
            this.AxROIBW8_CCD01_01_比對樣板範圍.OrgX = PLC_Device_CCD01_01比對樣板範圍_CenterX.Value;
            this.AxROIBW8_CCD01_01_比對樣板範圍.OrgY = PLC_Device_CCD01_01比對樣板範圍_CenterY.Value;
            this.AxROIBW8_CCD01_01_比對樣板範圍.ROIWidth = PLC_Device_CCD01_01比對樣板範圍_Width.Value;
            this.AxROIBW8_CCD01_01_比對樣板範圍.ROIHeight = PLC_Device_CCD01_01比對樣板範圍_Height.Value;

            cnt++;
        }
        void cnt_Program_CCD01_01比對樣板範圍_繪製畫布(ref int cnt)
        {
            if(CCD01_01_SrcImageHandle != 0)
            {
                this.PLC_Device_CCD01_01比對樣板範圍_RefreshCanvas.Bool = true;
                if (this.PLC_Device_CCD01_01比對樣板範圍按鈕.Bool && !PLC_Device_CCD01_01_計算一次.Bool)
                {
                    this.h_Canvas_Tech_CCD01_01.RefreshCanvas();
                }
            }
            cnt++;
        }

        #region 讀取樣板
        int cnt_Program_CCD01_01_讀取樣板 = 65534;
        void sub_Program_CCD01_01_讀取樣板()
        {
            if (cnt_Program_CCD01_01_讀取樣板 == 65534)
            {
                cnt_Program_CCD01_01_讀取樣板 = 65535;
            }
            if (cnt_Program_CCD01_01_讀取樣板 == 65535) cnt_Program_CCD01_01_讀取樣板 = 1;
            if (cnt_Program_CCD01_01_讀取樣板 == 1) cnt_CCD01_01_讀取樣板_開始讀取(ref cnt_Program_CCD01_01_讀取樣板);
            if (cnt_Program_CCD01_01_讀取樣板 == 2) cnt_Program_CCD01_01_讀取樣板 = 255;
        }
        void cnt_CCD01_01_讀取樣板_開始讀取(ref int cnt)
        {
            AxMatch_CCD01_01_圓柱相似度測試.LoadFile(".//" + CCD01_01_樣板儲存名稱);
            if (AxMatch_CCD01_01_圓柱相似度測試.IsLearnPattern)
            {
                this.AxImageCopier_CCD01_01_圓柱相似度測試_GetPattern.SrcImageHandle = AxMatch_CCD01_01_圓柱相似度測試.PatternVegaHandle;
                this.AxImageCopier_CCD01_01_圓柱相似度測試_GetPattern.DstImageHandle = this.CCD01_01_圓柱相似度測試_GetPattern_AxImageBW8.VegaHandle;
                this.AxImageCopier_CCD01_01_圓柱相似度測試_GetPattern.Copy();
                this.CCD01_01_PatternCanvasRefresh(this.CCD01_01_圓柱相似度測試_GetPattern_AxImageBW8.VegaHandle, this.CCD01_01_PatternCanvas_ZoomX, this.CCD01_01_PatternCanvas_ZoomY);
            }
            cnt++;
        }
        #endregion
        #endregion
        #region Event

        private void PlC_RJ_Button_CCD01_01_儲存圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (saveImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.h_Canvas_Tech_CCD01_01.SaveImage(saveImageDialog.FileName);
                }
            }));

        }
        private void PlC_RJ_Button_CCD01_01_讀取圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (openImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.CCD01_AxImageBW8.LoadFile(openImageDialog.FileName);
                    try
                    {
                        this.h_Canvas_Tech_CCD01_01.ImageCopy(CCD01_AxImageBW8.VegaHandle);
                        this.CCD01_01_SrcImageHandle = h_Canvas_Tech_CCD01_01.VegaHandle;
                        this.h_Canvas_Tech_CCD01_01.RefreshCanvas();
                    }
                    catch
                    {
                        err_message01_01 = MessageBox.Show("讀取圖片空白", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        if (err_message01_01 == DialogResult.OK)
                        {

                        }
                    }
                }
            }));


        }
        private void PlC_RJ_Button_Main_CCD01_01儲存圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (saveImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.h_Canvas_Main_CCD01_01_檢測畫面.SaveImage(saveImageDialog.FileName);
                }
            }));
        }
        private void PlC_RJ_Button_Main_CCD01_01讀取圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (openImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.CCD01_AxImageBW8.LoadFile(openImageDialog.FileName);
                    try
                    {
                        this.h_Canvas_Main_CCD01_01_檢測畫面.ImageCopy(CCD01_AxImageBW8.VegaHandle);
                        this.CCD01_01_SrcImageHandle = h_Canvas_Main_CCD01_01_檢測畫面.VegaHandle;
                        this.h_Canvas_Main_CCD01_01_檢測畫面.RefreshCanvas();
                    }
                    catch
                    {
                        err_message01_01 = MessageBox.Show("讀取圖片空白", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        if (err_message01_01 == DialogResult.OK)
                        {

                        }
                    }
                }
            }));
        }
        private void PlC_Button_Main_CCD01_01_ZOOM更新_btnClick(object sender, EventArgs e)
        {
            if (CCD01_01_SrcImageHandle != 0)
            {
                PLC_Device_Main_CCD01_01_ZOOM更新.Bool = true;
                h_Canvas_Main_CCD01_01_檢測畫面.RefreshCanvas();
            }
        }
        private void plC_Button_CCD01_01_學習樣板按鈕_btnClick(object sender, EventArgs e)
        {

            AxMatch_CCD01_01_圓柱相似度測試.MinReducedArea = 144;
            AxMatch_CCD01_01_圓柱相似度測試.LearnPattern();
            if(AxMatch_CCD01_01_圓柱相似度測試.IsLearnPattern)
            {
                MessageBox.Show("學習樣板成功", "訊息", MessageBoxButtons.OKCancel);
                this.PLC_Device_CCD01_01_圓柱相似度量測_樣本圖片辨識代碼.Value = (int)AxMatch_CCD01_01_圓柱相似度測試.PatternVegaHandle;
                this.AxImageCopier_CCD01_01_圓柱相似度測試_GetPattern.SrcImageHandle = AxMatch_CCD01_01_圓柱相似度測試.PatternVegaHandle;
                this.AxImageCopier_CCD01_01_圓柱相似度測試_GetPattern.DstImageHandle = this.CCD01_01_圓柱相似度測試_GetPattern_AxImageBW8.VegaHandle;
                this.AxImageCopier_CCD01_01_圓柱相似度測試_GetPattern.Copy();
                this.CCD01_01_PatternCanvasRefresh(this.CCD01_01_圓柱相似度測試_GetPattern_AxImageBW8.VegaHandle, this.CCD01_01_PatternCanvas_ZoomX, this.CCD01_01_PatternCanvas_ZoomY);
                this.AxMatch_CCD01_01_圓柱相似度測試.SaveFile(".//" + CCD01_01_樣板儲存名稱);
            }
            else MessageBox.Show("學習樣板失敗!!", "訊息", MessageBoxButtons.OKCancel);

        }
        public delegate void CCD01_01_PatternCanvas_Refresh_EventHandler(long SurfaceHadle, float ZoomX, float ZoomY);
        public event CCD01_01_PatternCanvas_Refresh_EventHandler CCD01_01_PatternCanvas_Refresh_Event;
        void CCD01_01_PatternCanvasRefresh(long SurfaceHadle, float ZoomX, float ZoomY)
        {
            this.CCD01_01_PatternCanvas.ClearCanvas();
            this.CCD01_01_PatternCanvas.DrawSurface(SurfaceHadle, ZoomX, ZoomY, 0, 0);
            //
            if (this.CCD01_01_PatternCanvas_Refresh_Event != null) this.CCD01_01_PatternCanvas_Refresh_Event(SurfaceHadle, ZoomX, ZoomY);
            //
            this.CCD01_01_PatternCanvas.RefreshCanvas();

        }
        #endregion
    }
}
