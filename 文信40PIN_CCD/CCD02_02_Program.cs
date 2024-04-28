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


        DialogResult err_message02_02;

        void Program_CCD02_02()
        {
            this.CCD02_02_儲存圖片();
            this.sub_Program_CCD02_02_SNAP();
            this.sub_Program_CCD02_02_Tech_檢驗一次();
            this.sub_Program_CCD02_02_計算一次();
            this.sub_Program_CCD02_02_Tech_取像並檢驗();
            this.sub_Program_CCD02_02_Main_取像並檢驗();
            this.sub_Program_CCD02_02_PIN量測_量測框調整();
            this.sub_Program_CCD02_02_PIN量測_檢測距離計算();
            this.sub_Program_CCD02_02_PIN正位度量測_設定規範位置();
            this.sub_Program_CCD02_02_PIN量測_檢測正位度計算();
            this.sub_Program_CCD02_02比對樣板範圍();
            this.sub_Program_CCD02_02PIN相似度量測();
            this.sub_Program_CCD02_02_讀取樣板();
            this.sub_Program_CCD02_02_Main_檢驗一次();
            if (PLC_Device_CCD02_02PIN相似度量測_樣板相似度門檻_MinScore.Value / 100 < 0) PLC_Device_CCD02_02PIN相似度量測_樣板相似度門檻_MinScore.Value = 0;
            if (PLC_Device_CCD02_02PIN相似度量測_樣板相似度門檻_MinScore.Value >= 100) PLC_Device_CCD02_02PIN相似度量測_樣板相似度門檻_MinScore.Value = 100;
        }

        #region PLC_CCD02_02_SNAP
        PLC_Device PLC_Device_CCD02_02_SNAP_按鈕 = new PLC_Device("M15130");
        PLC_Device PLC_Device_CCD02_02_SNAP = new PLC_Device("M15125");
        PLC_Device PLC_Device_CCD02_02_SNAP_LIVE = new PLC_Device("M15126");
        PLC_Device PLC_Device_CCD02_02_SNAP_電子快門 = new PLC_Device("F9110");
        PLC_Device PLC_Device_CCD02_02_SNAP_視訊增益 = new PLC_Device("F9111");
        PLC_Device PLC_Device_CCD02_02_SNAP_銳利度 = new PLC_Device("F9112");
        PLC_Device PLC_Device_CCD02_02_SNAP_光源亮度_紅正照 = new PLC_Device("F25142");

        int cnt_Program_CCD02_02_SNAP = 65534;
        void sub_Program_CCD02_02_SNAP()
        {
            if (cnt_Program_CCD02_02_SNAP == 65534)
            {
                PLC_Device_CCD02_02_SNAP.SetComment("PLC_CCD02_02_SNAP");
                PLC_Device_CCD02_02_SNAP.Bool = false;
                PLC_Device_CCD02_02_SNAP_按鈕.Bool = false;
                cnt_Program_CCD02_02_SNAP = 65535;
            }
            if (cnt_Program_CCD02_02_SNAP == 65535) cnt_Program_CCD02_02_SNAP = 1;
            if (cnt_Program_CCD02_02_SNAP == 1) cnt_Program_CCD02_02_SNAP_檢查按下(ref cnt_Program_CCD02_02_SNAP);
            if (cnt_Program_CCD02_02_SNAP == 2) cnt_Program_CCD02_02_SNAP_初始化(ref cnt_Program_CCD02_02_SNAP);
            if (cnt_Program_CCD02_02_SNAP == 3) cnt_Program_CCD02_02_SNAP_開始取像(ref cnt_Program_CCD02_02_SNAP);
            if (cnt_Program_CCD02_02_SNAP == 4) cnt_Program_CCD02_02_SNAP_取像結束(ref cnt_Program_CCD02_02_SNAP);
            if (cnt_Program_CCD02_02_SNAP == 5) cnt_Program_CCD02_02_SNAP_繪製影像(ref cnt_Program_CCD02_02_SNAP);
            if (cnt_Program_CCD02_02_SNAP == 6) cnt_Program_CCD02_02_SNAP = 65500;
            if (cnt_Program_CCD02_02_SNAP > 1) cnt_Program_CCD02_02_SNAP_檢查放開(ref cnt_Program_CCD02_02_SNAP);

            if (cnt_Program_CCD02_02_SNAP == 65500)
            {
                PLC_Device_CCD02_02_SNAP_按鈕.Bool = false;
                PLC_Device_CCD02_02_SNAP.Bool = false;
                cnt_Program_CCD02_02_SNAP = 65535;
            }
        }
        void cnt_Program_CCD02_02_SNAP_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD02_02_SNAP_按鈕.Bool)
            {
                PLC_Device_CCD02_02_SNAP.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_02_SNAP_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD02_02_SNAP.Bool) cnt = 65500;
        }
        void cnt_Program_CCD02_02_SNAP_初始化(ref int cnt)
        {
            PLC_Device_CCD02_SNAP_電子快門.Value = PLC_Device_CCD02_02_SNAP_電子快門.Value;
            PLC_Device_CCD02_SNAP_視訊增益.Value = PLC_Device_CCD02_02_SNAP_視訊增益.Value;
            PLC_Device_CCD02_SNAP_銳利度.Value = PLC_Device_CCD02_02_SNAP_銳利度.Value;

            if (PLC_Device_CCD02_02_SNAP_光源亮度_紅正照.Value != 0)
            {
                this.光源控制(enum_光源.CCD02_紅正照, (byte)this.PLC_Device_CCD02_02_SNAP_光源亮度_紅正照.Value);
                this.光源控制(enum_光源.CCD02_紅正照, true);
            }
            else if (this.PLC_Device_CCD02_02_SNAP_光源亮度_紅正照.Value == 0)
            {
                this.光源控制(enum_光源.CCD02_紅正照, (byte)0);
                this.光源控制(enum_光源.CCD02_紅正照, false);
            }
            cnt++;
        }
        void cnt_Program_CCD02_02_SNAP_開始取像(ref int cnt)
        {
            if (!PLC_Device_CCD02_SNAP.Bool)
            {
                PLC_Device_CCD02_SNAP.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_02_SNAP_取像結束(ref int cnt)
        {
            if (!PLC_Device_CCD02_SNAP.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD02_02_SNAP_繪製影像(ref int cnt)
        {
            this.CCD02_02_SrcImageHandle = this.h_Canvas_Main_CCD02_02_檢測畫面.VegaHandle;
            this.h_Canvas_Main_CCD02_02_檢測畫面.ImageCopy(this.CCD02_AxImageBW8.VegaHandle);

            this.CCD02_02_SrcImageHandle = this.h_Canvas_Tech_CCD02_02.VegaHandle;
            this.h_Canvas_Tech_CCD02_02.ImageCopy(this.CCD02_AxImageBW8.VegaHandle);
            this.h_Canvas_Tech_CCD02_02.SetImageSize(this.h_Canvas_Tech_CCD02_02.ImageWidth, this.h_Canvas_Tech_CCD02_02.ImageHeight);

            if (!PLC_Device_CCD02_02_Tech_取像並檢驗.Bool && !PLC_Device_CCD02_02_Main_取像並檢驗.Bool)
            {
                if (this.PLC_Device_CCD02_02_SNAP.Bool) this.h_Canvas_Tech_CCD02_02.RefreshCanvas();


                if (PLC_Device_CCD02_02_SNAP_LIVE.Bool)
                {
                    cnt = 2;
                    return;
                }
                else
                {
                    光源控制(enum_光源.CCD02_紅正照, (byte)0);
                    光源控制(enum_光源.CCD02_紅正照, false);
                    cnt++;
                }
            }
            else cnt++;

        }





        #endregion
        #region PLC_CCD02_02_Main_取像並檢驗
        PLC_Device PLC_Device_CCD02_02_Main_取像並檢驗按鈕 = new PLC_Device("S39960");
        PLC_Device PLC_Device_CCD02_02_Main_取像並檢驗 = new PLC_Device("S39961");
        PLC_Device PLC_Device_CCD02_02_Main_取像並檢驗_OK = new PLC_Device("S39962");
        PLC_Device PLC_Device_CCD02_02_PLC觸發檢測 = new PLC_Device("S39760");
        PLC_Device PLC_Device_CCD02_02_PLC觸發檢測完成 = new PLC_Device("S39761");
        PLC_Device PLC_Device_CCD02_02_Main_取像完成 = new PLC_Device("S39762");
        PLC_Device PLC_Device_CCD02_02_Main_BUSY = new PLC_Device("S39763");
        bool flag_CCD02_02_開始存檔 = false;
        String CCD02_02_原圖位置, CCD02_02_量測圖位置;
        PLC_Device PLC_NumBox_CCD02_02_OK最大儲存張數 = new PLC_Device("F14203");
        PLC_Device PLC_NumBox_CCD02_02_NG最大儲存張數 = new PLC_Device("F14204");
        MyTimer CCD02_02_Init_Timer = new MyTimer();
        int cnt_Program_CCD02_02_Main_取像並檢驗 = 65534;
        void sub_Program_CCD02_02_Main_取像並檢驗()
        {
            if (cnt_Program_CCD02_02_Main_取像並檢驗 == 65534)
            {
                PLC_Device_CCD02_02_Main_取像並檢驗.SetComment("PLC_CCD02_02_Main_取像並檢驗");
                PLC_Device_CCD02_02_Main_BUSY.Bool = false;
                PLC_Device_CCD02_02_Main_取像完成.Bool = false;
                PLC_Device_CCD02_02_Main_取像並檢驗.Bool = false;
                PLC_Device_CCD02_02_PLC觸發檢測.Bool = false;
                PLC_Device_CCD02_02_PLC觸發檢測完成.Bool = false;
                PLC_Device_CCD02_02_Main_取像並檢驗_OK.Bool = false;
                PLC_Device_CCD02_02_Main_取像並檢驗按鈕.Bool = false;
                cnt_Program_CCD02_02_Main_取像並檢驗 = 65535;


            }
            if (cnt_Program_CCD02_02_Main_取像並檢驗 == 65535) cnt_Program_CCD02_02_Main_取像並檢驗 = 1;
            if (cnt_Program_CCD02_02_Main_取像並檢驗 == 1) cnt_Program_CCD02_02_Main_取像並檢驗_檢查按下(ref cnt_Program_CCD02_02_Main_取像並檢驗);
            if (cnt_Program_CCD02_02_Main_取像並檢驗 == 2) cnt_Program_CCD02_02_Main_取像並檢驗_初始化(ref cnt_Program_CCD02_02_Main_取像並檢驗);
            if (cnt_Program_CCD02_02_Main_取像並檢驗 == 3) cnt_Program_CCD02_02_Main_取像並檢驗_開始SNAP(ref cnt_Program_CCD02_02_Main_取像並檢驗);
            if (cnt_Program_CCD02_02_Main_取像並檢驗 == 4) cnt_Program_CCD02_02_Main_取像並檢驗_結束SNAP(ref cnt_Program_CCD02_02_Main_取像並檢驗);
            if (cnt_Program_CCD02_02_Main_取像並檢驗 == 5) cnt_Program_CCD02_02_Main_取像並檢驗_開始計算一次(ref cnt_Program_CCD02_02_Main_取像並檢驗);
            if (cnt_Program_CCD02_02_Main_取像並檢驗 == 6) cnt_Program_CCD02_02_Main_取像並檢驗_結束計算一次(ref cnt_Program_CCD02_02_Main_取像並檢驗);
            if (cnt_Program_CCD02_02_Main_取像並檢驗 == 7) cnt_Program_CCD02_02_Main_取像並檢驗_繪製畫布(ref cnt_Program_CCD02_02_Main_取像並檢驗);
            if (cnt_Program_CCD02_02_Main_取像並檢驗 == 8) cnt_Program_CCD02_02_Main_取像並檢驗_檢查重測次數(ref cnt_Program_CCD02_02_Main_取像並檢驗);
            if (cnt_Program_CCD02_02_Main_取像並檢驗 == 9) cnt_Program_CCD02_02_Main_取像並檢驗 = 65500;
            if (cnt_Program_CCD02_02_Main_取像並檢驗 > 1) cnt_Program_CCD02_02_Main_取像並檢驗_檢查放開(ref cnt_Program_CCD02_02_Main_取像並檢驗);

            if (cnt_Program_CCD02_02_Main_取像並檢驗 == 65500)
            {
                PLC_Device_CCD02_02_Main_BUSY.Bool = false;
                PLC_Device_CCD02_02_Main_取像完成.Bool = false;
                PLC_Device_CCD02_02_Main_取像並檢驗.Bool = false;
                PLC_Device_CCD02_02_PLC觸發檢測.Bool = false;
                PLC_Device_CCD02_02_Main_取像並檢驗按鈕.Bool = false;
                cnt_Program_CCD02_02_Main_取像並檢驗 = 65535;
            }
        }
        void cnt_Program_CCD02_02_Main_取像並檢驗_檢查按下(ref int cnt)
        {

            if (PLC_Device_CCD02_02_Main_取像並檢驗按鈕.Bool || PLC_Device_CCD02_02_PLC觸發檢測.Bool)
            {
                CCD02_02_Init_Timer.TickStop();
                CCD02_02_Init_Timer.StartTickTime(100000);
                PLC_Device_CCD02_02_Main_取像並檢驗.Bool = true;
                cnt++;
            }



        }
        void cnt_Program_CCD02_02_Main_取像並檢驗_檢查放開(ref int cnt)
        {
            //if (!PLC_Device_CCD02_02_Main_取像並檢驗.Bool && !PLC_Device_CCD02_02_PLC觸發檢測.Bool) cnt = 65500;
        }
        void cnt_Program_CCD02_02_Main_取像並檢驗_初始化(ref int cnt)
        {
            PLC_Device_CCD02_02_PLC觸發檢測完成.Bool = false;
            PLC_Device_CCD02_02_Main_BUSY.Bool = true;
            cnt++;
        }
        void cnt_Program_CCD02_02_Main_取像並檢驗_開始SNAP(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_02_SNAP.Bool)
            {
                this.PLC_Device_CCD02_02_SNAP_按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_02_Main_取像並檢驗_結束SNAP(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_02_SNAP_按鈕.Bool)
            {
                光源控制(enum_光源.CCD02_紅正照, (byte)0);
                光源控制(enum_光源.CCD02_紅正照, false);
                cnt++;
            }
        }
        void cnt_Program_CCD02_02_Main_取像並檢驗_開始計算一次(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_02_計算一次.Bool)
            {
                this.PLC_Device_CCD02_02_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_02_Main_取像並檢驗_結束計算一次(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_02_計算一次.Bool)
            {
                Console.WriteLine($"CCD02_02檢測,耗時 {CCD02_02_Init_Timer.ToString()}");
                cnt++;
            }
        }
        void cnt_Program_CCD02_02_Main_取像並檢驗_繪製畫布(ref int cnt)
        {
            //this.CCD02_02_SrcImageHandle = this.h_Canvas_Main_CCD02_02_檢測畫面.VegaHandle;
            //this.h_Canvas_Main_CCD02_02_檢測畫面.ImageCopy(this.CCD02_AxImageBW8.VegaHandle);

            if (CCD02_02_SrcImageHandle != 0)
            {
                PLC_Device_CCD02_02_PLC觸發檢測完成.Bool = true;
                flag_CCD02_02_開始存檔 = true;
                this.h_Canvas_Main_CCD02_02_檢測畫面.RefreshCanvas();
            }
            cnt++;
        }
        void cnt_Program_CCD02_02_Main_取像並檢驗_檢查重測次數(ref int cnt)
        {
            cnt++;
        }
        private void CCD02_02_儲存圖片()
        {
            if (flag_CCD02_02_開始存檔)
            {
                String FilePlaceOK = plC_WordBox_CCD02_02_OK存圖路徑.Text;
                String FileNameOK = "CCD02_02_OK";
                String FilePlaceNG = plC_WordBox_CCD02_02_NG存圖路徑.Text;
                String FileNameNG = "CCD02_02_NG";
                儲存檔案_往後移位(FilePlaceOK, FileNameOK, PLC_NumBox_CCD02_02_OK最大儲存張數.Value);
                儲存檔案_往後移位(FilePlaceNG, FileNameNG, PLC_NumBox_CCD02_02_NG最大儲存張數.Value);
                if (PLC_Device_CCD02_02_Main_取像並檢驗_OK.Bool)
                {
                    整理檔案(FilePlaceOK, FileNameOK, PLC_NumBox_CCD02_02_OK最大儲存張數.Value);
                    FileNameOK = FileNameOK + "_OK";
                    CCD02_02_原圖位置 = CCD02_02_OK儲存檔案檢查(FilePlaceOK, FileNameOK + "_A", PLC_NumBox_CCD02_02_OK最大儲存張數.Value);
                    CCD02_02_量測圖位置 = CCD02_02_原圖位置.Replace("_A", "_B");
                    this.Invoke(new Action(delegate
                    {
                        if (plC_ComboBox_CCD02_02_OK是否存圖.SelectedIndex == 0)
                        {
                            this.h_Canvas_Main_CCD02_02_檢測畫面.SaveImage(CCD02_02_原圖位置);
                        }
                    }));
                }
                else if (!PLC_Device_CCD02_02_Main_取像並檢驗_OK.Bool)
                {
                    整理檔案(FilePlaceNG, FileNameNG, PLC_NumBox_CCD02_02_NG最大儲存張數.Value);
                    FileNameNG = FileNameNG + "_NG";
                    CCD02_02_原圖位置 = CCD02_02_NG儲存檔案檢查(FilePlaceNG, FileNameNG + "_A", PLC_NumBox_CCD02_02_NG最大儲存張數.Value);
                    CCD02_02_量測圖位置 = CCD02_02_原圖位置.Replace("_A", "_B");
                    this.Invoke(new Action(delegate
                    {
                        if (plC_ComboBox_CCD02_02_NG是否存圖.SelectedIndex == 0)
                        {
                            this.h_Canvas_Main_CCD02_02_檢測畫面.SaveImage(CCD02_02_原圖位置);
                        }
                    }));
                }
                flag_CCD02_02_開始存檔 = false;
            }
        }




        #endregion
        #region PLC_CCD02_02_Tech_取像並檢驗
        PLC_Device PLC_Device_CCD02_02_Tech_取像並檢驗按鈕 = new PLC_Device("M15730");
        PLC_Device PLC_Device_CCD02_02_Tech_取像並檢驗 = new PLC_Device("M15725");
        MyTimer CCD02_02_Tech_Timer = new MyTimer();
        int cnt_Program_CCD02_02_Tech_取像並檢驗 = 65534;
        void sub_Program_CCD02_02_Tech_取像並檢驗()
        {
            if (cnt_Program_CCD02_02_Tech_取像並檢驗 == 65534)
            {
                PLC_Device_CCD02_02_Tech_取像並檢驗.SetComment("PLC_CCD02_02_Tech_取像並檢驗");
                PLC_Device_CCD02_02_Tech_取像並檢驗.Bool = false;
                cnt_Program_CCD02_02_Tech_取像並檢驗 = 65535;
            }
            if (cnt_Program_CCD02_02_Tech_取像並檢驗 == 65535) cnt_Program_CCD02_02_Tech_取像並檢驗 = 1;
            if (cnt_Program_CCD02_02_Tech_取像並檢驗 == 1) cnt_Program_CCD02_02_Tech_取像並檢驗_檢查按下(ref cnt_Program_CCD02_02_Tech_取像並檢驗);
            if (cnt_Program_CCD02_02_Tech_取像並檢驗 == 2) cnt_Program_CCD02_02_Tech_取像並檢驗_初始化(ref cnt_Program_CCD02_02_Tech_取像並檢驗);
            if (cnt_Program_CCD02_02_Tech_取像並檢驗 == 3) cnt_Program_CCD02_02_Tech_取像並檢驗_SNAP一次開始(ref cnt_Program_CCD02_02_Tech_取像並檢驗);
            if (cnt_Program_CCD02_02_Tech_取像並檢驗 == 4) cnt_Program_CCD02_02_Tech_取像並檢驗_SNAP一次結束(ref cnt_Program_CCD02_02_Tech_取像並檢驗);
            if (cnt_Program_CCD02_02_Tech_取像並檢驗 == 5) cnt_Program_CCD02_02_Tech_取像並檢驗_計算一次開始(ref cnt_Program_CCD02_02_Tech_取像並檢驗);
            if (cnt_Program_CCD02_02_Tech_取像並檢驗 == 6) cnt_Program_CCD02_02_Tech_取像並檢驗_計算一次結束(ref cnt_Program_CCD02_02_Tech_取像並檢驗);
            if (cnt_Program_CCD02_02_Tech_取像並檢驗 == 7) cnt_Program_CCD02_02_Tech_取像並檢驗_繪製畫布(ref cnt_Program_CCD02_02_Tech_取像並檢驗);
            if (cnt_Program_CCD02_02_Tech_取像並檢驗 == 8) cnt_Program_CCD02_02_Tech_取像並檢驗 = 65500;
            if (cnt_Program_CCD02_02_Tech_取像並檢驗 > 1) cnt_Program_CCD02_02_Tech_取像並檢驗_檢查放開(ref cnt_Program_CCD02_02_Tech_取像並檢驗);

            if (cnt_Program_CCD02_02_Tech_取像並檢驗 == 65500)
            {
                PLC_Device_CCD02_02_Tech_取像並檢驗按鈕.Bool = false;
                PLC_Device_CCD02_02_Tech_取像並檢驗.Bool = false;
                cnt_Program_CCD02_02_Tech_取像並檢驗 = 65535;
            }
        }
        void cnt_Program_CCD02_02_Tech_取像並檢驗_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD02_02_Tech_取像並檢驗按鈕.Bool)
            {
                PLC_Device_CCD02_02_Tech_取像並檢驗.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD02_02_Tech_取像並檢驗_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD02_02_Tech_取像並檢驗.Bool) cnt = 65500;
        }
        void cnt_Program_CCD02_02_Tech_取像並檢驗_初始化(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD02_02_Tech_取像並檢驗_SNAP一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_02_SNAP.Bool)
            {
                this.PLC_Device_CCD02_02_SNAP_按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_02_Tech_取像並檢驗_SNAP一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_02_SNAP_按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD02_02_Tech_取像並檢驗_計算一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_02_計算一次.Bool)
            {
                this.PLC_Device_CCD02_02_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_02_Tech_取像並檢驗_計算一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_02_計算一次.Bool)
            {

                cnt++;
            }
        }
        void cnt_Program_CCD02_02_Tech_取像並檢驗_繪製畫布(ref int cnt)
        {
            if (CCD02_02_SrcImageHandle != 0)
            {
                this.h_Canvas_Tech_CCD02_02.RefreshCanvas();
            }
            if (PLC_Device_CCD02_02_SNAP_LIVE.Bool)
            {

                cnt = 2;
                return;
            }
            else
            {
                光源控制(enum_光源.CCD02_紅正照, (byte)0);
                光源控制(enum_光源.CCD02_紅正照, false);
                cnt++;
            }

            Console.WriteLine($"CCD0201檢測,耗時 {CCD02_02_Tech_Timer.ToString()}");
        }

































        #endregion
        #region PLC_CCD02_02_Tech_檢驗一次
        PLC_Device PLC_Device_CCD02_02_Tech_檢驗一次按鈕 = new PLC_Device("M15430");
        PLC_Device PLC_Device_CCD02_02_Tech_檢驗一次 = new PLC_Device("M15425");
        int cnt_Program_CCD02_02_Tech_檢驗一次 = 65534;
        void sub_Program_CCD02_02_Tech_檢驗一次()
        {
            if (cnt_Program_CCD02_02_Tech_檢驗一次 == 65534)
            {
                PLC_Device_CCD02_02_Tech_檢驗一次.SetComment("PLC_CCD02_02_Tech_檢驗一次");
                PLC_Device_CCD02_02_Tech_檢驗一次.Bool = false;
                cnt_Program_CCD02_02_Tech_檢驗一次 = 65535;
            }
            if (cnt_Program_CCD02_02_Tech_檢驗一次 == 65535) cnt_Program_CCD02_02_Tech_檢驗一次 = 1;
            if (cnt_Program_CCD02_02_Tech_檢驗一次 == 1) cnt_Program_CCD02_02_Tech_檢驗一次_檢查按下(ref cnt_Program_CCD02_02_Tech_檢驗一次);
            if (cnt_Program_CCD02_02_Tech_檢驗一次 == 2) cnt_Program_CCD02_02_Tech_檢驗一次_初始化(ref cnt_Program_CCD02_02_Tech_檢驗一次);
            if (cnt_Program_CCD02_02_Tech_檢驗一次 == 3) cnt_Program_CCD02_02_Tech_檢驗一次_計算一次開始(ref cnt_Program_CCD02_02_Tech_檢驗一次);
            if (cnt_Program_CCD02_02_Tech_檢驗一次 == 4) cnt_Program_CCD02_02_Tech_檢驗一次_計算一次結束(ref cnt_Program_CCD02_02_Tech_檢驗一次);
            if (cnt_Program_CCD02_02_Tech_檢驗一次 == 5) cnt_Program_CCD02_02_Tech_檢驗一次_繪製畫布(ref cnt_Program_CCD02_02_Tech_檢驗一次);
            if (cnt_Program_CCD02_02_Tech_檢驗一次 == 6) cnt_Program_CCD02_02_Tech_檢驗一次 = 65500;
            if (cnt_Program_CCD02_02_Tech_檢驗一次 > 1) cnt_Program_CCD02_02_Tech_檢驗一次_檢查放開(ref cnt_Program_CCD02_02_Tech_檢驗一次);

            if (cnt_Program_CCD02_02_Tech_檢驗一次 == 65500)
            {
                PLC_Device_CCD02_02_Tech_檢驗一次按鈕.Bool = false;
                PLC_Device_CCD02_02_Tech_檢驗一次.Bool = false;
                cnt_Program_CCD02_02_Tech_檢驗一次 = 65535;
            }
        }
        void cnt_Program_CCD02_02_Tech_檢驗一次_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD02_02_Tech_檢驗一次按鈕.Bool)
            {
                PLC_Device_CCD02_02_Tech_檢驗一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_02_Tech_檢驗一次_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD02_02_Tech_檢驗一次.Bool) cnt = 65500;
        }
        void cnt_Program_CCD02_02_Tech_檢驗一次_初始化(ref int cnt)
        {

            cnt++;
        }
        void cnt_Program_CCD02_02_Tech_檢驗一次_計算一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_02_計算一次.Bool)
            {
                this.PLC_Device_CCD02_02_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_02_Tech_檢驗一次_計算一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_02_計算一次.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD02_02_Tech_檢驗一次_繪製畫布(ref int cnt)
        {

            if (CCD02_02_SrcImageHandle != 0)
            {
                this.h_Canvas_Tech_CCD02_02.RefreshCanvas();
            }
            cnt++;
        }

































        #endregion
        #region PLC_CCD02_02_Main_檢驗一次
        PLC_Device PLC_Device_CCD02_02_Main_檢驗一次按鈕 = new PLC_Device("M15812");
        PLC_Device PLC_Device_CCD02_02_Main_檢驗一次 = new PLC_Device("M15813");
        int cnt_Program_CCD02_02_Main_檢驗一次 = 65534;
        void sub_Program_CCD02_02_Main_檢驗一次()
        {
            if (cnt_Program_CCD02_02_Main_檢驗一次 == 65534)
            {
                PLC_Device_CCD02_02_Main_檢驗一次.SetComment("PLC_CCD02_02_Main_檢驗一次");
                PLC_Device_CCD02_02_Main_檢驗一次.Bool = false;
                PLC_Device_CCD02_02_Main_檢驗一次按鈕.Bool = false;
                cnt_Program_CCD02_02_Main_檢驗一次 = 65535;
            }
            if (cnt_Program_CCD02_02_Main_檢驗一次 == 65535) cnt_Program_CCD02_02_Main_檢驗一次 = 1;
            if (cnt_Program_CCD02_02_Main_檢驗一次 == 1) cnt_Program_CCD02_02_Main_檢驗一次_檢查按下(ref cnt_Program_CCD02_02_Main_檢驗一次);
            if (cnt_Program_CCD02_02_Main_檢驗一次 == 2) cnt_Program_CCD02_02_Main_檢驗一次_初始化(ref cnt_Program_CCD02_02_Main_檢驗一次);
            if (cnt_Program_CCD02_02_Main_檢驗一次 == 3) cnt_Program_CCD02_02_Main_檢驗一次_計算一次開始(ref cnt_Program_CCD02_02_Main_檢驗一次);
            if (cnt_Program_CCD02_02_Main_檢驗一次 == 4) cnt_Program_CCD02_02_Main_檢驗一次_計算一次結束(ref cnt_Program_CCD02_02_Main_檢驗一次);
            if (cnt_Program_CCD02_02_Main_檢驗一次 == 5) cnt_Program_CCD02_02_Main_檢驗一次_繪製畫布(ref cnt_Program_CCD02_02_Main_檢驗一次);
            if (cnt_Program_CCD02_02_Main_檢驗一次 == 6) cnt_Program_CCD02_02_Main_檢驗一次 = 65500;
            if (cnt_Program_CCD02_02_Main_檢驗一次 > 1) cnt_Program_CCD02_02_Main_檢驗一次_檢查放開(ref cnt_Program_CCD02_02_Main_檢驗一次);

            if (cnt_Program_CCD02_02_Main_檢驗一次 == 65500)
            {
                PLC_Device_CCD02_02_Main_檢驗一次按鈕.Bool = false;
                PLC_Device_CCD02_02_Main_檢驗一次.Bool = false;
                cnt_Program_CCD02_02_Main_檢驗一次 = 65535;
            }
        }
        void cnt_Program_CCD02_02_Main_檢驗一次_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD02_02_Main_檢驗一次按鈕.Bool)
            {
                PLC_Device_CCD02_02_Main_檢驗一次.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD02_02_Main_檢驗一次_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD02_02_Main_檢驗一次.Bool) cnt = 65500;
        }
        void cnt_Program_CCD02_02_Main_檢驗一次_初始化(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD02_02_Main_檢驗一次_計算一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_02_計算一次.Bool)
            {
                this.PLC_Device_CCD02_02_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_02_Main_檢驗一次_計算一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_02_計算一次.Bool)
            {

                cnt++;
            }
        }
        void cnt_Program_CCD02_02_Main_檢驗一次_繪製畫布(ref int cnt)
        {
            if (CCD02_02_SrcImageHandle != 0)
            {
                this.h_Canvas_Main_CCD02_02_檢測畫面.RefreshCanvas();
            }

            cnt++;
        }
        #endregion

        #region PLC_CCD02_02_計算一次
        PLC_Device PLC_Device_CCD02_02_計算一次 = new PLC_Device("S5055");
        PLC_Device PLC_Device_CCD02_02_計算一次_OK = new PLC_Device("S5056");
        PLC_Device PLC_Device_CCD02_02_計算一次_READY = new PLC_Device("S5057");
        MyTimer MyTimer_CCD02_02_計算一次 = new MyTimer();

        int cnt_Program_CCD02_02_計算一次 = 65534;
        void sub_Program_CCD02_02_計算一次()
        {
            this.PLC_Device_CCD02_02_計算一次_READY.Bool = !this.PLC_Device_CCD02_02_計算一次.Bool;
            if (cnt_Program_CCD02_02_計算一次 == 65534)
            {
                PLC_Device_CCD02_02_計算一次.SetComment("PLC_CCD02_02_計算一次");
                PLC_Device_CCD02_02_計算一次.Bool = false;

                cnt_Program_CCD02_02_計算一次 = 65535;
            }
            if (cnt_Program_CCD02_02_計算一次 == 65535) cnt_Program_CCD02_02_計算一次 = 1;
            if (cnt_Program_CCD02_02_計算一次 == 1) cnt_Program_CCD02_02_計算一次_檢查按下(ref cnt_Program_CCD02_02_計算一次);
            if (cnt_Program_CCD02_02_計算一次 == 2) cnt_Program_CCD02_02_計算一次_初始化(ref cnt_Program_CCD02_02_計算一次);
            if (cnt_Program_CCD02_02_計算一次 == 3) cnt_Program_CCD02_02_計算一次_步驟01開始(ref cnt_Program_CCD02_02_計算一次);
            if (cnt_Program_CCD02_02_計算一次 == 4) cnt_Program_CCD02_02_計算一次_步驟01結束(ref cnt_Program_CCD02_02_計算一次);
            if (cnt_Program_CCD02_02_計算一次 == 5) cnt_Program_CCD02_02_計算一次_步驟02開始(ref cnt_Program_CCD02_02_計算一次);
            if (cnt_Program_CCD02_02_計算一次 == 6) cnt_Program_CCD02_02_計算一次_步驟02結束(ref cnt_Program_CCD02_02_計算一次);
            if (cnt_Program_CCD02_02_計算一次 == 7) cnt_Program_CCD02_02_計算一次_步驟03開始(ref cnt_Program_CCD02_02_計算一次);
            if (cnt_Program_CCD02_02_計算一次 == 8) cnt_Program_CCD02_02_計算一次_步驟03結束(ref cnt_Program_CCD02_02_計算一次);
            if (cnt_Program_CCD02_02_計算一次 == 9) cnt_Program_CCD02_02_計算一次_步驟04開始(ref cnt_Program_CCD02_02_計算一次);
            if (cnt_Program_CCD02_02_計算一次 == 10) cnt_Program_CCD02_02_計算一次_步驟04結束(ref cnt_Program_CCD02_02_計算一次);
            if (cnt_Program_CCD02_02_計算一次 == 11) cnt_Program_CCD02_02_計算一次_步驟05開始(ref cnt_Program_CCD02_02_計算一次);
            if (cnt_Program_CCD02_02_計算一次 == 12) cnt_Program_CCD02_02_計算一次_步驟05結束(ref cnt_Program_CCD02_02_計算一次);
            if (cnt_Program_CCD02_02_計算一次 == 13) cnt_Program_CCD02_02_計算一次_步驟06開始(ref cnt_Program_CCD02_02_計算一次);
            if (cnt_Program_CCD02_02_計算一次 == 14) cnt_Program_CCD02_02_計算一次_步驟06結束(ref cnt_Program_CCD02_02_計算一次);
            if (cnt_Program_CCD02_02_計算一次 == 15) cnt_Program_CCD02_02_計算一次_計算結果(ref cnt_Program_CCD02_02_計算一次);
            if (cnt_Program_CCD02_02_計算一次 == 16) cnt_Program_CCD02_02_計算一次 = 65500;
            if (cnt_Program_CCD02_02_計算一次 > 1) cnt_Program_CCD02_02_計算一次_檢查放開(ref cnt_Program_CCD02_02_計算一次);

            if (cnt_Program_CCD02_02_計算一次 == 65500)
            {
                PLC_Device_CCD02_02_計算一次.Bool = false;
                cnt_Program_CCD02_02_計算一次 = 65535;
            }
        }
        void cnt_Program_CCD02_02_計算一次_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD02_02_計算一次.Bool) cnt++;
        }
        void cnt_Program_CCD02_02_計算一次_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD02_02_計算一次.Bool) cnt = 65500;
        }
        void cnt_Program_CCD02_02_計算一次_初始化(ref int cnt)
        {
            PLC_Device_CCD02_02_PIN量測_量測框調整.Bool = false;
            PLC_Device_CCD02_02_PIN量測_檢測距離計算.Bool = false;
            PLC_Device_CCD02_02_PIN正位度量測_設定規範位置.Bool = false;
            PLC_Device_CCD02_02_PIN量測_檢測距離計算.Bool = false;
            cnt++;
        }
        void cnt_Program_CCD02_02_計算一次_步驟01開始(ref int cnt)
        {

            cnt++;

        }
        void cnt_Program_CCD02_02_計算一次_步驟01結束(ref int cnt)
        {

            cnt++;

        }
        void cnt_Program_CCD02_02_計算一次_步驟02開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_02_PIN量測_量測框調整按鈕.Bool)
            {
                this.PLC_Device_CCD02_02_PIN量測_量測框調整按鈕.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD02_02_計算一次_步驟02結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_02_PIN量測_量測框調整按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD02_02_計算一次_步驟03開始(ref int cnt)
        {
            if (!PLC_Device_CCD02_02_PIN正位度量測_設定規範按鈕.Bool)
            {
                PLC_Device_CCD02_02_PIN正位度量測_設定規範按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_02_計算一次_步驟03結束(ref int cnt)
        {
            if (!PLC_Device_CCD02_02_PIN正位度量測_設定規範按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD02_02_計算一次_步驟04開始(ref int cnt)
        {
            if (!PLC_Device_CCD02_02_PIN量測_檢測距離計算按鈕.Bool && !PLC_Device_CCD02_02_PIN量測_檢測正位度計算按鈕.Bool && !PLC_Device_CCD02_02PIN相似度量測按鈕.Bool)
            {
                PLC_Device_CCD02_02PIN相似度量測按鈕.Bool = true;
                PLC_Device_CCD02_02_PIN量測_檢測距離計算按鈕.Bool = true;
                PLC_Device_CCD02_02_PIN量測_檢測正位度計算按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_02_計算一次_步驟04結束(ref int cnt)
        {
            if (!PLC_Device_CCD02_02_PIN量測_檢測距離計算按鈕.Bool && !PLC_Device_CCD02_02_PIN量測_檢測正位度計算按鈕.Bool && !PLC_Device_CCD02_02PIN相似度量測按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD02_02_計算一次_步驟05開始(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD02_02_計算一次_步驟05結束(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD02_02_計算一次_步驟06開始(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD02_02_計算一次_步驟06結束(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD02_02_計算一次_計算結果(ref int cnt)
        {
            bool flag = true;
            if (!this.PLC_Device_CCD02_02_PIN量測_檢測距離計算_OK.Bool) flag = false;
            if (!this.PLC_Device_CCD02_02_PIN量測_檢測正位度計算_OK.Bool) flag = false;
            this.PLC_Device_CCD02_02_Main_取像並檢驗_OK.Bool = flag;
            this.PLC_Device_CCD02_02_計算一次_OK.Bool = flag;
            //flag_CCD02_02_上端水平度寫入列表資料 = true;
            //flag_CCD02_02_上端間距寫入列表資料 = true;
            //flag_CCD02_02_上端水平度差值寫入列表資料 = true;

            cnt++;
        }





        #endregion
        #region PLC_CCD02_02_PIN量測_量測框調整
        MyTimer MyTimer_CCD02_02_PIN量測_量測框調整 = new MyTimer();
        PLC_Device PLC_Device_CCD02_02_PIN量測_量測框調整按鈕 = new PLC_Device("S6270");
        PLC_Device PLC_Device_CCD02_02_PIN量測_量測框調整 = new PLC_Device("S6265");
        PLC_Device PLC_Device_CCD02_02_PIN量測_量測框調整_OK = new PLC_Device("S6266");
        PLC_Device PLC_Device_CCD02_02_PIN量測_量測框調整_測試完成 = new PLC_Device("S6267");
        PLC_Device PLC_Device_CCD02_02_PIN量測_量測框調整_RefreshCanvas = new PLC_Device("S6568");
        PLC_Device PLC_Device_CCD02_02_PIN量測_有無量測不測試 = new PLC_Device("S6122");
        private List<AxOvkBase.AxROIBW8> List_CCD02_02_PIN量測_AxROIBW8_量測框調整 = new List<AxOvkBase.AxROIBW8>();
        private List<AxOvkBlob.AxObject> List_CCD02_02_PIN量測_AxObject_區塊分析 = new List<AxOvkBlob.AxObject>();
        private AxOvkPat.AxVisionInspectionFrame CCD02_02_PIN量測_AxVisionInspectionFrame_量測框調整;

        private List<PLC_Device> List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值 = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD02_02_PIN量測參數_OrgX = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD02_02_PIN量測參數_OrgY = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD02_02_PIN量測參數_Width = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD02_02_PIN量測參數_Height = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD02_02_PIN量測參數_面積上限 = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD02_02_PIN量測參數_面積下限 = new List<PLC_Device>();
        private PointF[] List_CCD02_02_PIN量測參數_量測點 = new PointF[22];
        private PointF[] List_CCD02_02_PIN量測參數_量測點_結果 = new PointF[22];
        private Point[] List_CCD02_02_PIN量測參數_量測點_轉換後座標 = new Point[22];
        private bool[] List_CCD02_02_PIN量測參數_量測點_有無 = new bool[22];
        #region 灰階門檻值
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN01 = new PLC_Device("F1400");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN02 = new PLC_Device("F1401");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN03 = new PLC_Device("F1402");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN04 = new PLC_Device("F1403");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN05 = new PLC_Device("F1404");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN06 = new PLC_Device("F1405");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN07 = new PLC_Device("F1406");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN08 = new PLC_Device("F1407");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN09 = new PLC_Device("F1408");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN10 = new PLC_Device("F1409");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_上排PIN11 = new PLC_Device("F2130");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN11 = new PLC_Device("F1410");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN12 = new PLC_Device("F1411");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN13 = new PLC_Device("F1412");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN14 = new PLC_Device("F1413");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN15 = new PLC_Device("F1414");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN16 = new PLC_Device("F1415");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN17 = new PLC_Device("F1416");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN18 = new PLC_Device("F1417");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN19 = new PLC_Device("F1418");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN20 = new PLC_Device("F1419");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_下排PIN11 = new PLC_Device("F2131");
        #endregion
        #region OrgX
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN01 = new PLC_Device("F1500");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN02 = new PLC_Device("F1501");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN03 = new PLC_Device("F1502");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN04 = new PLC_Device("F1503");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN05 = new PLC_Device("F1504");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN06 = new PLC_Device("F1505");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN07 = new PLC_Device("F1506");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN08 = new PLC_Device("F1507");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN09 = new PLC_Device("F1508");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN10 = new PLC_Device("F1509");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgX_上排PIN11 = new PLC_Device("F2132");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN11 = new PLC_Device("F1510");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN12 = new PLC_Device("F1511");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN13 = new PLC_Device("F1512");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN14 = new PLC_Device("F1513");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN15 = new PLC_Device("F1514");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN16 = new PLC_Device("F1515");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN17 = new PLC_Device("F1516");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN18 = new PLC_Device("F1517");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN19 = new PLC_Device("F1518");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN20 = new PLC_Device("F1519");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgX_下排PIN11 = new PLC_Device("F2133");
        #endregion
        #region OrgY
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN01 = new PLC_Device("F1600");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN02 = new PLC_Device("F1601");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN03 = new PLC_Device("F1602");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN04 = new PLC_Device("F1603");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN05 = new PLC_Device("F1604");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN06 = new PLC_Device("F1605");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN07 = new PLC_Device("F1606");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN08 = new PLC_Device("F1607");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN09 = new PLC_Device("F1608");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN10 = new PLC_Device("F1609");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgY_上排PIN11 = new PLC_Device("F2134");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN11 = new PLC_Device("F1610");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN12 = new PLC_Device("F1611");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN13 = new PLC_Device("F1612");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN14 = new PLC_Device("F1613");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN15 = new PLC_Device("F1614");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN16 = new PLC_Device("F1615");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN17 = new PLC_Device("F1616");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN18 = new PLC_Device("F1617");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN19 = new PLC_Device("F1618");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN20 = new PLC_Device("F1619");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_OrgY_下排PIN11 = new PLC_Device("F2135");
        #endregion
        #region Width
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Width_PIN01 = new PLC_Device("F1700");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Width_PIN02 = new PLC_Device("F1701");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Width_PIN03 = new PLC_Device("F1702");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Width_PIN04 = new PLC_Device("F1703");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Width_PIN05 = new PLC_Device("F1704");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Width_PIN06 = new PLC_Device("F1705");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Width_PIN07 = new PLC_Device("F1706");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Width_PIN08 = new PLC_Device("F1707");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Width_PIN09 = new PLC_Device("F1708");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Width_PIN10 = new PLC_Device("F1709");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Width_上排PIN11 = new PLC_Device("F2136");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Width_PIN11 = new PLC_Device("F1710");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Width_PIN12 = new PLC_Device("F1711");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Width_PIN13 = new PLC_Device("F1712");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Width_PIN14 = new PLC_Device("F1713");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Width_PIN15 = new PLC_Device("F1714");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Width_PIN16 = new PLC_Device("F1715");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Width_PIN17 = new PLC_Device("F1716");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Width_PIN18 = new PLC_Device("F1717");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Width_PIN19 = new PLC_Device("F1718");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Width_PIN20 = new PLC_Device("F1719");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Width_下排PIN11 = new PLC_Device("F2137");
        #endregion
        #region Height
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Height_PIN01 = new PLC_Device("F1800");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Height_PIN02 = new PLC_Device("F1801");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Height_PIN03 = new PLC_Device("F1802");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Height_PIN04 = new PLC_Device("F1803");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Height_PIN05 = new PLC_Device("F1804");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Height_PIN06 = new PLC_Device("F1805");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Height_PIN07 = new PLC_Device("F1806");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Height_PIN08 = new PLC_Device("F1807");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Height_PIN09 = new PLC_Device("F1808");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Height_PIN10 = new PLC_Device("F1809");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Height_上排PIN11 = new PLC_Device("F2138");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Height_PIN11 = new PLC_Device("F1810");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Height_PIN12 = new PLC_Device("F1811");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Height_PIN13 = new PLC_Device("F1812");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Height_PIN14 = new PLC_Device("F1813");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Height_PIN15 = new PLC_Device("F1814");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Height_PIN16 = new PLC_Device("F1815");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Height_PIN17 = new PLC_Device("F1816");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Height_PIN18 = new PLC_Device("F1817");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Height_PIN19 = new PLC_Device("F1818");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Height_PIN20 = new PLC_Device("F1819");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_Height_下排PIN11 = new PLC_Device("F2139");
        #endregion
        #region 面積上限

        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN01 = new PLC_Device("F1900");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN02 = new PLC_Device("F1901");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN03 = new PLC_Device("F1902");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN04 = new PLC_Device("F1903");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN05 = new PLC_Device("F1904");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN06 = new PLC_Device("F1905");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN07 = new PLC_Device("F1906");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN08 = new PLC_Device("F1907");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN09 = new PLC_Device("F1908");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN10 = new PLC_Device("F1909");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積上限_上排PIN11 = new PLC_Device("F2140");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN11 = new PLC_Device("F1910");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN12 = new PLC_Device("F1911");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN13 = new PLC_Device("F1912");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN14 = new PLC_Device("F1913");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN15 = new PLC_Device("F1914");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN16 = new PLC_Device("F1915");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN17 = new PLC_Device("F1916");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN18 = new PLC_Device("F1917");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN19 = new PLC_Device("F1918");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN20 = new PLC_Device("F1919");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積上限_下排PIN11 = new PLC_Device("F2141");
        #endregion
        #region 面積下限

        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN01 = new PLC_Device("F2000");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN02 = new PLC_Device("F2001");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN03 = new PLC_Device("F2002");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN04 = new PLC_Device("F2003");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN05 = new PLC_Device("F2004");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN06 = new PLC_Device("F2005");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN07 = new PLC_Device("F2006");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN08 = new PLC_Device("F2007");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN09 = new PLC_Device("F2008");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN10 = new PLC_Device("F2009");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積下限_上排PIN11 = new PLC_Device("F2142");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN11 = new PLC_Device("F2010");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN12 = new PLC_Device("F2011");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN13 = new PLC_Device("F2012");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN14 = new PLC_Device("F2013");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN15 = new PLC_Device("F2014");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN16 = new PLC_Device("F2015");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN17 = new PLC_Device("F2016");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN18 = new PLC_Device("F2017");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN19 = new PLC_Device("F2018");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN20 = new PLC_Device("F2019");
        private PLC_Device PLC_Device_CCD02_02_PIN量測參數_面積下限_下排PIN11 = new PLC_Device("F2143");
        #endregion
        AxOvkBase.TxAxHitHandle[] CCD02_02_PIN量測_AxROIBW8_TxAxHitHandle = new AxOvkBase.TxAxHitHandle[22];
        bool[] flag_CCD02_02_PIN量測_AxROIBW8_MouseDown = new bool[22];
        private void H_Canvas_Tech_CCD02_02_PIN量測_量測框調整_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {

            if (PLC_Device_CCD02_02_Main_取像並檢驗.Bool || PLC_Device_CCD02_02_PLC觸發檢測.Bool || PLC_Device_CCD02_02_Main_檢驗一次按鈕.Bool)
            {
                try
                {
                    Graphics g = Graphics.FromHdc((IntPtr)HDC);
                    for (int i = 0; i < this.List_CCD02_02_PIN量測參數_量測點.Length; i++)
                    {
                        DrawingClass.Draw.十字中心(this.List_CCD02_02_PIN量測參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
                    }
                    g.Dispose();
                    g = null;
                }
                catch
                {

                }

            }
            else if (PLC_Device_CCD02_02_Tech_檢驗一次.Bool || PLC_Device_CCD02_02_Tech_取像並檢驗.Bool)
            {
                if (this.PLC_Device_CCD02_02_PIN量測_量測框調整_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        //for (int i = 0; i < this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
                        //{
                        //    if (i < 10)
                        //    {
                        //        this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].Title = string.Format("上排" + "{0}", (i + 1).ToString("00"));
                        //    }
                        //    if (i >= 10)
                        //    {
                        //        this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].Title = string.Format("下排" + "{0}", ((i - 10) + 1).ToString("00"));
                        //    }
                        //    this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].ShowTitle = true;
                        //    this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].ShowPlacement = false;
                        //    this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].DrawRect(HDC, ZoomX, ZoomY, 0, 0, 0x0000FF);
                        //}
                        for (int i = 0; i < this.List_CCD02_02_PIN量測參數_量測點.Length; i++)
                        {
                            DrawingClass.Draw.十字中心(this.List_CCD02_02_PIN量測參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
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
                if (this.PLC_Device_CCD02_02_PIN量測_量測框調整_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        PointF po_str_PIN到基準Y = new PointF(200, 250);
                        Font font = new Font("微軟正黑體", 10);


                        DrawingClass.Draw.水平線段繪製(0, 10000, CCD02_01_水平基準線量測_AxLineMsr.MeasuredSlope, CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotX,
                            CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD02_01_基準線量測_基準線偏移_上排.Value, Color.Blue, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.文字右中繪製("上排輔助線", new PointF((float)this.List_CCD02_02_PIN量測參數_水平度量測顯示點_X[0], CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD02_01_基準線量測_基準線偏移_上排.Value + 20)
                            , new Font("標楷體", 10), Color.White, Color.Blue, g, ZoomX, ZoomY);


                        DrawingClass.Draw.水平線段繪製(0, 10000, CCD02_01_水平基準線量測_AxLineMsr.MeasuredSlope, CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotX,
                            CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD02_01_基準線量測_基準線偏移_下排.Value, Color.Blue, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.文字右中繪製("下排輔助線", new PointF((float)this.List_CCD02_02_PIN量測參數_水平度量測顯示點_X[0], CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD02_01_基準線量測_基準線偏移_下排.Value + 20)
                            , new Font("標楷體", 10), Color.White, Color.Blue, g, ZoomX, ZoomY);


                        if (this.plC_CheckBox_CCD02_02_PIN量測_繪製量測框.Checked)
                        {
                            for (int i = 0; i < this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
                            {
                                if (i < 11)
                                {
                                    this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].Title = string.Format("上排" + "{0}", (i + 1).ToString("00"));
                                }
                                if (i >= 11)
                                {
                                    this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].Title = string.Format("下排" + "{0}", ((i - 10) + 1).ToString("00"));
                                }
                                this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].ShowTitle = true;
                                this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].ShowPlacement = false;
                                this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].DrawFrame(HDC, ZoomX, ZoomY, 0, 0, 0x0000FF);
                            }
                        }
                        if (this.plC_CheckBox_CCD02_02_PIN量測_繪製量測區塊.Checked)
                        {
                            for (int i = 0; i < this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
                            {
                                this.List_CCD02_02_PIN量測_AxObject_區塊分析[i].DrawBlobs(HDC, -1, ZoomX, ZoomY, 0, 0, true, -1);
                            }

                        }
                        for (int i = 0; i < this.List_CCD02_02_PIN量測參數_量測點.Length; i++)
                        {
                            DrawingClass.Draw.十字中心(this.List_CCD02_02_PIN量測參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
                        }
                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }
            }

            this.PLC_Device_CCD02_02_PIN量測_量測框調整_RefreshCanvas.Bool = false;
        }
        private void H_Canvas_Tech_CCD02_02_PIN量測_量測框調整_OnCanvasMouseDownEvent(int x, int y, float ZoomX, float ZoomY, ref int InUsedEventNum, int InUsedCanvasHandle)
        {
            if (this.PLC_Device_CCD02_02_PIN量測_量測框調整.Bool)
            {
                for (int i = 0; i < this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
                {
                    this.CCD02_02_PIN量測_AxROIBW8_TxAxHitHandle[i] = this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].HitTest(x, y, ZoomX, ZoomY, 0, 0);
                    if (this.CCD02_02_PIN量測_AxROIBW8_TxAxHitHandle[i] != AxOvkBase.TxAxHitHandle.AX_HANDLE_NONE)
                    {
                        this.flag_CCD02_02_PIN量測_AxROIBW8_MouseDown[i] = true;
                        InUsedEventNum = 10;
                        return;
                    }
                }

            }

        }
        private void H_Canvas_Tech_CCD02_02_PIN量測_量測框調整_OnCanvasMouseMoveEvent(int x, int y, float ZoomX, float ZoomY)
        {
            for (int i = 0; i < this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {
                if (this.flag_CCD02_02_PIN量測_AxROIBW8_MouseDown[i])
                {
                    this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].DragROI(this.CCD02_02_PIN量測_AxROIBW8_TxAxHitHandle[i], x, y, ZoomX, ZoomY, 0, 0);
                    this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX[i].Value = this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].OrgX;
                    this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY[i].Value = this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].OrgY;
                    this.List_PLC_Device_CCD02_02_PIN量測參數_Width[i].Value = this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].ROIWidth;
                    this.List_PLC_Device_CCD02_02_PIN量測參數_Height[i].Value = this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].ROIHeight;
                }
            }

        }
        private void H_Canvas_Tech_CCD02_02_PIN量測_量測框調整_OnCanvasMouseUpEvent(int x, int y, float ZoomX, float ZoomY)
        {
            for (int i = 0; i < this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {
                this.flag_CCD02_02_PIN量測_AxROIBW8_MouseDown[i] = false;
            }
        }

        int cnt_Program_CCD02_02_PIN量測_量測框調整 = 65534;
        void sub_Program_CCD02_02_PIN量測_量測框調整()
        {
            if (cnt_Program_CCD02_02_PIN量測_量測框調整 == 65534)
            {
                this.h_Canvas_Tech_CCD02_02.OnCanvasDrawEvent += H_Canvas_Tech_CCD02_02_PIN量測_量測框調整_OnCanvasDrawEvent;
                this.h_Canvas_Tech_CCD02_02.OnCanvasMouseDownEvent += H_Canvas_Tech_CCD02_02_PIN量測_量測框調整_OnCanvasMouseDownEvent;
                this.h_Canvas_Tech_CCD02_02.OnCanvasMouseMoveEvent += H_Canvas_Tech_CCD02_02_PIN量測_量測框調整_OnCanvasMouseMoveEvent;
                this.h_Canvas_Tech_CCD02_02.OnCanvasMouseUpEvent += H_Canvas_Tech_CCD02_02_PIN量測_量測框調整_OnCanvasMouseUpEvent;

                this.h_Canvas_Main_CCD02_02_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD02_02_PIN量測_量測框調整_OnCanvasDrawEvent;

                #region 灰階門檻值
                this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN01);
                this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN02);
                this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN03);
                this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN04);
                this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN05);
                this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN06);
                this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN07);
                this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN08);
                this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN09);
                this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN10);
                this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_上排PIN11);
                this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN11);
                this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN12);
                this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN13);
                this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN14);
                this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN15);
                this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN16);
                this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN17);
                this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN18);
                this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN19);
                this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_PIN20);
                this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_02_PIN量測參數_灰階門檻值_下排PIN11);
                #endregion
                #region OrgX
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN01);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN02);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN03);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN04);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN05);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN06);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN07);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN08);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN09);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN10);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgX_上排PIN11);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN11);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN12);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN13);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN14);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN15);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN16);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN17);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN18);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN19);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgX_PIN20);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgX_下排PIN11);
                #endregion
                #region OrgY
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN01);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN02);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN03);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN04);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN05);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN06);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN07);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN08);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN09);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN10);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgY_上排PIN11);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN11);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN12);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN13);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN14);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN15);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN16);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN17);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN18);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN19);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgY_PIN20);
                this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_02_PIN量測參數_OrgY_下排PIN11);
                #endregion
                #region Width
                this.List_PLC_Device_CCD02_02_PIN量測參數_Width.Add(this.PLC_Device_CCD02_02_PIN量測參數_Width_PIN01);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Width.Add(this.PLC_Device_CCD02_02_PIN量測參數_Width_PIN02);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Width.Add(this.PLC_Device_CCD02_02_PIN量測參數_Width_PIN03);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Width.Add(this.PLC_Device_CCD02_02_PIN量測參數_Width_PIN04);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Width.Add(this.PLC_Device_CCD02_02_PIN量測參數_Width_PIN05);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Width.Add(this.PLC_Device_CCD02_02_PIN量測參數_Width_PIN06);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Width.Add(this.PLC_Device_CCD02_02_PIN量測參數_Width_PIN07);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Width.Add(this.PLC_Device_CCD02_02_PIN量測參數_Width_PIN08);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Width.Add(this.PLC_Device_CCD02_02_PIN量測參數_Width_PIN09);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Width.Add(this.PLC_Device_CCD02_02_PIN量測參數_Width_PIN10);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Width.Add(this.PLC_Device_CCD02_02_PIN量測參數_Width_上排PIN11);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Width.Add(this.PLC_Device_CCD02_02_PIN量測參數_Width_PIN11);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Width.Add(this.PLC_Device_CCD02_02_PIN量測參數_Width_PIN12);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Width.Add(this.PLC_Device_CCD02_02_PIN量測參數_Width_PIN13);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Width.Add(this.PLC_Device_CCD02_02_PIN量測參數_Width_PIN14);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Width.Add(this.PLC_Device_CCD02_02_PIN量測參數_Width_PIN15);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Width.Add(this.PLC_Device_CCD02_02_PIN量測參數_Width_PIN16);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Width.Add(this.PLC_Device_CCD02_02_PIN量測參數_Width_PIN17);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Width.Add(this.PLC_Device_CCD02_02_PIN量測參數_Width_PIN18);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Width.Add(this.PLC_Device_CCD02_02_PIN量測參數_Width_PIN19);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Width.Add(this.PLC_Device_CCD02_02_PIN量測參數_Width_PIN20);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Width.Add(this.PLC_Device_CCD02_02_PIN量測參數_Width_下排PIN11);
                #endregion
                #region Height
                this.List_PLC_Device_CCD02_02_PIN量測參數_Height.Add(this.PLC_Device_CCD02_02_PIN量測參數_Height_PIN01);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Height.Add(this.PLC_Device_CCD02_02_PIN量測參數_Height_PIN02);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Height.Add(this.PLC_Device_CCD02_02_PIN量測參數_Height_PIN03);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Height.Add(this.PLC_Device_CCD02_02_PIN量測參數_Height_PIN04);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Height.Add(this.PLC_Device_CCD02_02_PIN量測參數_Height_PIN05);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Height.Add(this.PLC_Device_CCD02_02_PIN量測參數_Height_PIN06);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Height.Add(this.PLC_Device_CCD02_02_PIN量測參數_Height_PIN07);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Height.Add(this.PLC_Device_CCD02_02_PIN量測參數_Height_PIN08);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Height.Add(this.PLC_Device_CCD02_02_PIN量測參數_Height_PIN09);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Height.Add(this.PLC_Device_CCD02_02_PIN量測參數_Height_PIN10);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Height.Add(this.PLC_Device_CCD02_02_PIN量測參數_Height_上排PIN11);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Height.Add(this.PLC_Device_CCD02_02_PIN量測參數_Height_PIN11);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Height.Add(this.PLC_Device_CCD02_02_PIN量測參數_Height_PIN12);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Height.Add(this.PLC_Device_CCD02_02_PIN量測參數_Height_PIN13);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Height.Add(this.PLC_Device_CCD02_02_PIN量測參數_Height_PIN14);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Height.Add(this.PLC_Device_CCD02_02_PIN量測參數_Height_PIN15);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Height.Add(this.PLC_Device_CCD02_02_PIN量測參數_Height_PIN16);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Height.Add(this.PLC_Device_CCD02_02_PIN量測參數_Height_PIN17);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Height.Add(this.PLC_Device_CCD02_02_PIN量測參數_Height_PIN18);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Height.Add(this.PLC_Device_CCD02_02_PIN量測參數_Height_PIN19);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Height.Add(this.PLC_Device_CCD02_02_PIN量測參數_Height_PIN20);
                this.List_PLC_Device_CCD02_02_PIN量測參數_Height.Add(this.PLC_Device_CCD02_02_PIN量測參數_Height_下排PIN11);
                #endregion
                #region 面積上限
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN01);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN02);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN03);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN04);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN05);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN06);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN07);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN08);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN09);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN10);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積上限_上排PIN11);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN11);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN12);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN13);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN14);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN15);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN16);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN17);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN18);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN19);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積上限_PIN20);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積上限_下排PIN11);
                #endregion
                #region 面積下限
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN01);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN02);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN03);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN04);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN05);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN06);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN07);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN08);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN09);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN10);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積下限_上排PIN11);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN11);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN12);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN13);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN14);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN15);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN16);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN17);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN18);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN19);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積下限_PIN20);
                this.List_PLC_Device_CCD02_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_02_PIN量測參數_面積下限_下排PIN11);
                #endregion
                for (int i = 0; i < 22; i++)
                {
                    if (this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值[i].Value == 0) this.List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值[i].Value = 200;
                    if (this.List_PLC_Device_CCD02_02_PIN量測參數_Height[i].Value == 0) this.List_PLC_Device_CCD02_02_PIN量測參數_Height[i].Value = 100;
                    if (this.List_PLC_Device_CCD02_02_PIN量測參數_Width[i].Value == 0) this.List_PLC_Device_CCD02_02_PIN量測參數_Width[i].Value = 100;
                    if (this.List_PLC_Device_CCD02_02_PIN量測參數_Height[i].Value > 500) this.List_PLC_Device_CCD02_02_PIN量測參數_Height[i].Value = 500;
                    if (this.List_PLC_Device_CCD02_02_PIN量測參數_Width[i].Value > 500) this.List_PLC_Device_CCD02_02_PIN量測參數_Width[i].Value = 500;
                }
                PLC_Device_CCD02_02_PIN量測_量測框調整.SetComment("PLC_CCD02_02_PIN量測_量測框調整");
                PLC_Device_CCD02_02_PIN量測_量測框調整按鈕.Bool = false;
                PLC_Device_CCD02_02_PIN量測_量測框調整.Bool = false;
                cnt_Program_CCD02_02_PIN量測_量測框調整 = 65535;
            }
            if (cnt_Program_CCD02_02_PIN量測_量測框調整 == 65535) cnt_Program_CCD02_02_PIN量測_量測框調整 = 1;
            if (cnt_Program_CCD02_02_PIN量測_量測框調整 == 1) cnt_Program_CCD02_02_PIN量測_量測框調整_觸發按下(ref cnt_Program_CCD02_02_PIN量測_量測框調整);
            if (cnt_Program_CCD02_02_PIN量測_量測框調整 == 2) cnt_Program_CCD02_02_PIN量測_量測框調整_檢查按下(ref cnt_Program_CCD02_02_PIN量測_量測框調整);
            if (cnt_Program_CCD02_02_PIN量測_量測框調整 == 3) cnt_Program_CCD02_02_PIN量測_量測框調整_初始化(ref cnt_Program_CCD02_02_PIN量測_量測框調整);
            if (cnt_Program_CCD02_02_PIN量測_量測框調整 == 4) cnt_Program_CCD02_02_PIN量測_量測框調整_座標轉換(ref cnt_Program_CCD02_02_PIN量測_量測框調整);
            if (cnt_Program_CCD02_02_PIN量測_量測框調整 == 5) cnt_Program_CCD02_02_PIN量測_量測框調整_讀取參數(ref cnt_Program_CCD02_02_PIN量測_量測框調整);
            if (cnt_Program_CCD02_02_PIN量測_量測框調整 == 6) cnt_Program_CCD02_02_PIN量測_量測框調整_開始區塊分析(ref cnt_Program_CCD02_02_PIN量測_量測框調整);
            if (cnt_Program_CCD02_02_PIN量測_量測框調整 == 7) cnt_Program_CCD02_02_PIN量測_量測框調整_繪製畫布(ref cnt_Program_CCD02_02_PIN量測_量測框調整);
            if (cnt_Program_CCD02_02_PIN量測_量測框調整 == 8) cnt_Program_CCD02_02_PIN量測_量測框調整 = 65500;
            if (cnt_Program_CCD02_02_PIN量測_量測框調整 > 1) cnt_Program_CCD02_02_PIN量測_量測框調整_檢查放開(ref cnt_Program_CCD02_02_PIN量測_量測框調整);

            if (cnt_Program_CCD02_02_PIN量測_量測框調整 == 65500)
            {
                if (PLC_Device_CCD02_02_計算一次.Bool)
                {
                    PLC_Device_CCD02_02_PIN量測_量測框調整按鈕.Bool = false;
                    PLC_Device_CCD02_02_PIN量測_量測框調整.Bool = false;
                }
                cnt_Program_CCD02_02_PIN量測_量測框調整 = 65535;
            }
        }
        void cnt_Program_CCD02_02_PIN量測_量測框調整_觸發按下(ref int cnt)
        {
            if (PLC_Device_CCD02_02_PIN量測_量測框調整按鈕.Bool)
            {
                PLC_Device_CCD02_02_PIN量測_量測框調整.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD02_02_PIN量測_量測框調整_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD02_02_PIN量測_量測框調整.Bool) cnt++;
        }
        void cnt_Program_CCD02_02_PIN量測_量測框調整_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD02_02_PIN量測_量測框調整按鈕.Bool)
            {
                PLC_Device_CCD02_02_PIN量測_量測框調整.Bool = false;
                cnt = 65500;
            }
        }
        void cnt_Program_CCD02_02_PIN量測_量測框調整_初始化(ref int cnt)
        {
            this.MyTimer_CCD02_02_PIN量測_量測框調整.TickStop();
            this.MyTimer_CCD02_02_PIN量測_量測框調整.StartTickTime(99999);
            this.List_CCD02_02_PIN量測參數_量測點 = new PointF[22];
            this.List_CCD02_02_PIN量測參數_量測點_結果 = new PointF[22];
            this.List_CCD02_02_PIN量測參數_量測點_轉換後座標 = new Point[22];
            this.List_CCD02_02_PIN量測參數_量測點_有無 = new bool[22];
            cnt++;
        }
        void cnt_Program_CCD02_02_PIN量測_量測框調整_座標轉換(ref int cnt)
        {
            if (PLC_Device_CCD02_02_計算一次.Bool)
            {
                //CCD02_02_PIN量測_AxVisionInspectionFrame_量測框調整.RefPointX = 1;
                //CCD02_02_PIN量測_AxVisionInspectionFrame_量測框調整.RefPointY = 1;
                
                //CCD02_02_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentRefPointX = 1;
                //CCD02_02_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentRefPointY = 1;
                CCD02_02_PIN量測_AxVisionInspectionFrame_量測框調整.RefPointX = PLC_Device_CCD02_01_水平基準線量測_量測中心_X.Value;
                CCD02_02_PIN量測_AxVisionInspectionFrame_量測框調整.RefPointY = PLC_Device_CCD02_01_水平基準線量測_量測中心_Y.Value;
                CCD02_02_PIN量測_AxVisionInspectionFrame_量測框調整.RefAngle = 0;
                CCD02_02_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentRefPointX = Point_CCD02_01_中心基準座標_量測點.X;
                CCD02_02_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentRefPointY = Point_CCD02_01_中心基準座標_量測點.Y;
                CCD02_02_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentRefAngle = 0;
                CCD02_02_PIN量測_AxVisionInspectionFrame_量測框調整.NumOfVisionPoints = 22;

                for (int j = 0; j < 22; j++)
                {
                    if (this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX[j].Value == 0) this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX[j].Value = 100;
                    if (this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY[j].Value == 0) this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY[j].Value = 100;
                    if (this.List_PLC_Device_CCD02_02_PIN量測參數_Width[j].Value == 0) this.List_PLC_Device_CCD02_02_PIN量測參數_Width[j].Value = 100;
                    if (this.List_PLC_Device_CCD02_02_PIN量測參數_Height[j].Value == 0) this.List_PLC_Device_CCD02_02_PIN量測參數_Height[j].Value = 100;

                    CCD02_02_PIN量測_AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
                    CCD02_02_PIN量測_AxVisionInspectionFrame_量測框調整.VisionPointX = this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX[j].Value;
                    CCD02_02_PIN量測_AxVisionInspectionFrame_量測框調整.VisionPointY = this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY[j].Value;
                }
                CCD02_02_PIN量測_AxVisionInspectionFrame_量測框調整.EstimateCurrentVisionPoints();
                for (int j = 0; j < 22; j++)
                {
                    CCD02_02_PIN量測_AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
                    List_CCD02_02_PIN量測參數_量測點_轉換後座標[j].X = (int)CCD02_02_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentVisionPointX;
                    List_CCD02_02_PIN量測參數_量測點_轉換後座標[j].Y = (int)CCD02_02_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentVisionPointY;
                }
            }
            cnt++;

        }
        void cnt_Program_CCD02_02_PIN量測_量測框調整_讀取參數(ref int cnt)
        {
            for (int i = 0; i < this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {
                if (this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX[i].Value > 2596) this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX[i].Value = 0;
                if (this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY[i].Value > 1922) this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY[i].Value = 0;
                if (this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX[i].Value < 0) this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX[i].Value = 0;
                if (this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY[i].Value < 0) this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY[i].Value = 0;

                if (this.List_CCD02_02_PIN量測參數_量測點_轉換後座標[i].X > 2596) this.List_CCD02_02_PIN量測參數_量測點_轉換後座標[i].X = 0;
                if (this.List_CCD02_02_PIN量測參數_量測點_轉換後座標[i].Y > 1922) this.List_CCD02_02_PIN量測參數_量測點_轉換後座標[i].Y = 0;
                if (this.List_CCD02_02_PIN量測參數_量測點_轉換後座標[i].X < 0) this.List_CCD02_02_PIN量測參數_量測點_轉換後座標[i].X = 0;
                if (this.List_CCD02_02_PIN量測參數_量測點_轉換後座標[i].Y < 0) this.List_CCD02_02_PIN量測參數_量測點_轉換後座標[i].Y = 0;
            }
            for (int i = 0; i < this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {
                this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].ParentHandle = this.CCD02_02_SrcImageHandle;
                if (PLC_Device_CCD02_02_計算一次.Bool)
                {
                    this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].OrgX = List_CCD02_02_PIN量測參數_量測點_轉換後座標[i].X;
                    this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].OrgY = List_CCD02_02_PIN量測參數_量測點_轉換後座標[i].Y;
                }
                else
                {
                    this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].OrgX = this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX[i].Value;
                    this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].OrgY = this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY[i].Value;
                }
                this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].ROIWidth = this.List_PLC_Device_CCD02_02_PIN量測參數_Width[i].Value;
                this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].ROIHeight = this.List_PLC_Device_CCD02_02_PIN量測參數_Height[i].Value;
                this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].SkewAngle = 0;
            }
            cnt++;
        }
        void cnt_Program_CCD02_02_PIN量測_量測框調整_開始區塊分析(ref int cnt)
        {
            uint object_value = 4294963615;

            for (int i = 0; i < this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {
                this.AxMatch_CCD02_02_PIN相似度測試.SrcImageHandle = this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[0].VegaHandle;
                this.List_CCD02_02_PIN量測_AxObject_區塊分析[i].SrcImageHandle = this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].VegaHandle;
                this.List_CCD02_02_PIN量測_AxObject_區塊分析[i].ObjectClass = AxOvkBlob.TxAxObjClass.AX_OBJECT_DETECT_LIGHTER_CLASS;
                this.List_CCD02_02_PIN量測_AxObject_區塊分析[i].HighThreshold = List_PLC_Device_CCD02_02_PIN量測參數_灰階門檻值[0].Value;
                if (this.CCD02_02_SrcImageHandle != 0)
                {
                    if (this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX[i].Value + this.List_PLC_Device_CCD02_02_PIN量測參數_Width[i].Value < 2596 &&
                        this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY[i].Value + this.List_PLC_Device_CCD02_02_PIN量測參數_Height[i].Value < 1922 &&
                        this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX[i].Value > 0 && this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX[i].Value > 0)
                    {
                        this.List_CCD02_02_PIN量測_AxObject_區塊分析[i].BlobAnalyze(false);
                    }


                }
                this.List_CCD02_02_PIN量測_AxObject_區塊分析[i].CalculateFeatures((int)object_value, -1);
                this.List_CCD02_02_PIN量測_AxObject_區塊分析[i].SortObjects(AxOvkBlob.TxAxObjFeatureSortOrder.AX_OBJECT_SORT_ORDER_LARGE_TO_SMALL, AxOvkBlob.TxAxObjFeature.AX_OBJECT_FEATURE_AREA, 0, -1);
                this.List_CCD02_02_PIN量測_AxObject_區塊分析[i].SelectObjects(AxOvkBlob.TxAxObjFeature.AX_OBJECT_FEATURE_AREA, AxOvkBlob.TxAxObjFeatureOperation.AX_OBJECT_REMOVE_LESS_THAN, this.List_PLC_Device_CCD02_02_PIN量測參數_面積下限[0].Value);
                this.List_CCD02_02_PIN量測_AxObject_區塊分析[i].SelectObjects(AxOvkBlob.TxAxObjFeature.AX_OBJECT_FEATURE_AREA, AxOvkBlob.TxAxObjFeatureOperation.AX_OBJECT_REMOVE_GREAT_THAN, this.List_PLC_Device_CCD02_02_PIN量測參數_面積上限[0].Value);
                if (this.List_CCD02_02_PIN量測_AxObject_區塊分析[i].DetectedNumObjs > 0)
                {
                    this.List_CCD02_02_PIN量測參數_量測點_有無[i] = true;
                    this.List_CCD02_02_PIN量測_AxObject_區塊分析[i].BlobIndex = 0;
                    this.List_CCD02_02_PIN量測參數_量測點[i].X = (float)this.List_CCD02_02_PIN量測_AxObject_區塊分析[i].BlobCentroidX;
                    this.List_CCD02_02_PIN量測參數_量測點[i].X += this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].OrgX;
                    this.List_CCD02_02_PIN量測參數_量測點[i].Y = (float)this.List_CCD02_02_PIN量測_AxObject_區塊分析[i].BlobCentroidY;
                    //this.List_CCD02_02_PIN量測參數_量測點[i].Y = (float)this.List_CCD02_02_PIN量測_AxObject_區塊分析[i].BlobCentroidY - (float)this.List_CCD02_02_PIN量測_AxObject_區塊分析[i].BlobLimBoxHeight / 2;
                    this.List_CCD02_02_PIN量測參數_量測點[i].Y += this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整[i].OrgY;
                }


            }

            cnt++;
        }
        void cnt_Program_CCD02_02_PIN量測_量測框調整_繪製畫布(ref int cnt)
        {
            this.PLC_Device_CCD02_02_PIN量測_量測框調整_RefreshCanvas.Bool = true;
            if (this.PLC_Device_CCD02_02_PIN量測_量測框調整按鈕.Bool && !PLC_Device_CCD02_02_計算一次.Bool)
            {
                this.h_Canvas_Tech_CCD02_02.RefreshCanvas();
            }

            cnt++;
        }





        #endregion
        #region PLC_CCD02_02_PIN量測_檢測距離計算
        private AxOvkMsr.AxPointLineDistanceMsr CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排;
        private AxOvkMsr.AxPointLineDistanceMsr CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排;
        MyTimer MyTimer_CCD02_02_PIN量測_檢測距離計算 = new MyTimer();
        PLC_Device PLC_Device_CCD02_02_PIN量測_檢測距離計算按鈕 = new PLC_Device("S6290");
        PLC_Device PLC_Device_CCD02_02_PIN量測_檢測距離計算 = new PLC_Device("S6285");
        PLC_Device PLC_Device_CCD02_02_PIN量測_檢測距離計算_OK = new PLC_Device("S6286");
        PLC_Device PLC_Device_CCD02_02_PIN量測_檢測距離計算_測試完成 = new PLC_Device("S6287");
        PLC_Device PLC_Device_CCD02_02_PIN量測_檢測距離計算_RefreshCanvas = new PLC_Device("S6288");

        PLC_Device PLC_Device_CCD02_02_PIN量測_水平度量測不測試 = new PLC_Device("S6110");
        PLC_Device PLC_Device_CCD02_02_PIN量測_間距量測不測試 = new PLC_Device("S6111");

        PLC_Device PLC_Device_CCD02_02_PIN量測_左右間距量測標準值 = new PLC_Device("F2300");
        PLC_Device PLC_Device_CCD02_02_PIN量測_左右間距量測上限值 = new PLC_Device("F2301");
        PLC_Device PLC_Device_CCD02_02_PIN量測_左右間距量測下限值 = new PLC_Device("F2302");
        PLC_Device PLC_Device_CCD02_02_PIN量測_上下間距量測標準值 = new PLC_Device("F2303");
        PLC_Device PLC_Device_CCD02_02_PIN量測_上下間距量測上限值 = new PLC_Device("F2304");
        PLC_Device PLC_Device_CCD02_02_PIN量測_上下間距量測下限值 = new PLC_Device("F2305");

        PLC_Device PLC_Device_CCD02_02_PIN量測_上排水平度量測標準值 = new PLC_Device("F2310");
        PLC_Device PLC_Device_CCD02_02_PIN量測_上排水平度量測上限值 = new PLC_Device("F2311");
        PLC_Device PLC_Device_CCD02_02_PIN量測_上排水平度量測下限值 = new PLC_Device("F2312");
        PLC_Device PLC_Device_CCD02_02_PIN量測_下排水平度量測標準值 = new PLC_Device("F2322");
        PLC_Device PLC_Device_CCD02_02_PIN量測_下排水平度量測上限值 = new PLC_Device("F2323");
        PLC_Device PLC_Device_CCD02_02_PIN量測_下排水平度量測下限值 = new PLC_Device("F2324");

        PLC_Device PLC_Device_CCD02_02_PIN量測_水平度量測差值 = new PLC_Device("F2313");
        PLC_Device PLC_Device_CCD02_02_PIN量測_水平度量測差值上限 = new PLC_Device("F2314");
        PLC_Device PLC_Device_CCD02_02_PIN量測_水平度量測差值下限 = new PLC_Device("F2315");
        PLC_Device PLC_Device_CCD02_02_PIN量測_間距上排PIN01到基準數值 = new PLC_Device("F2316");
        PLC_Device PLC_Device_CCD02_02_PIN量測_間距上排PIN01到基準上限 = new PLC_Device("F2317");
        PLC_Device PLC_Device_CCD02_02_PIN量測_間距上排PIN01到基準下限 = new PLC_Device("F2318");
        PLC_Device PLC_Device_CCD02_02_PIN量測_間距下排PIN01到基準數值 = new PLC_Device("F2319");
        PLC_Device PLC_Device_CCD02_02_PIN量測_間距下排PIN01到基準上限 = new PLC_Device("F2320");
        PLC_Device PLC_Device_CCD02_02_PIN量測_間距下排PIN01到基準下限 = new PLC_Device("F2321");



        private List<PLC_Device> List_PLC_Device_CCD02_02_PIN量測參數_間距不測試 = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD02_02_PIN量測參數_左右間距量測值 = new List<PLC_Device>();

        private double[] List_CCD02_02_PIN量測參數_左右間距量測距離 = new double[21];
        private double[] List_CCD02_02_PIN量測參數_上下間距量測距離 = new double[21];
        private double[] List_CCD02_02_PIN量測參數_水平度量測距離 = new double[22];
        private double[] List_CCD02_02_PIN量測參數_上下間格距離 = new double[11];
        private double CCD02_02_PIN量測參數_間距上排PIN01到基準距離 = new double();
        private double CCD02_02_PIN量測參數_間距下排PIN01到基準距離 = new double();

        private bool[] List_CCD02_02_PIN量測參數_量測點_OK = new bool[22];
        private bool[] List_CCD02_02_PIN量測參數_左右間距量測_OK = new bool[21];
        private bool[] List_CCD02_02_PIN量測參數_上下間距量測_OK = new bool[21];
        private bool[] List_CCD02_02_PIN量測參數_水平度量測_OK = new bool[22];
        private bool CCD02_02_PIN量測參數_間距上排PIN01到基準_OK = new bool();
        private bool CCD02_02_PIN量測參數_間距下排PIN01到基準_OK = new bool();

        private double[] List_CCD02_02_PIN量測參數_水平度量測顯示點_X = new double[22];
        private double[] List_CCD02_02_PIN量測參數_水平度量測顯示點_Y = new double[22];
        private void H_Canvas_Tech_CCD02_02_PIN間距量測_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {
            PointF p0;
            PointF p1;
            PointF 上排_p2;
            PointF 上排_p3;
            PointF 下排_p2;
            PointF 下排_p3;
            PointF point;
            PointF point1;
            PointF 上排_to_line_point;
            PointF 下排_to_line_point;
            double 間距;
            double 水平度;
            double 基準線偏移_上排;
            double 基準線偏移_下排;

            if (PLC_Device_CCD02_02_Main_取像並檢驗.Bool || PLC_Device_CCD02_02_PLC觸發檢測.Bool || PLC_Device_CCD02_02_Main_檢驗一次按鈕.Bool)
            {
                基準線偏移_上排 = this.PLC_Device_CCD02_01_基準線量測_基準線偏移_上排.Value / CCD02_比例尺_pixcel_To_mm / 1000;
                基準線偏移_下排 = this.PLC_Device_CCD02_01_基準線量測_基準線偏移_下排.Value / CCD02_比例尺_pixcel_To_mm / 1000;
                try
                {
                    Graphics g = Graphics.FromHdc((IntPtr)HDC);
                    DrawingClass.Draw.十字中心(new PointF(this.Point_CCD02_01_中心基準座標_量測點.X, this.Point_CCD02_01_中心基準座標_量測點.Y), 100, Color.Red, 2, g, ZoomX, ZoomY);
                    #region 左右間距顯示
                    for (int i = 0; i < 21; i++)
                    {
                        p0 = new PointF(this.List_CCD02_02_PIN量測參數_量測點[i].X, this.List_CCD02_02_PIN量測參數_量測點[i].Y);
                        p1 = new PointF(this.List_CCD02_02_PIN量測參數_量測點[i + 1].X, this.List_CCD02_02_PIN量測參數_量測點[i + 1].Y);
                        間距 = List_CCD02_02_PIN量測參數_左右間距量測距離[i];
                        if (i != 10)
                        {
                            if (i <= 10)
                            {
                                if (List_CCD02_02_PIN量測參數_左右間距量測_OK[i])
                                {
                                    DrawingClass.Draw.文字中心繪製("左右間距:", new PointF(1200, 1130), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (間距 / 1D).ToString("0.000")), new PointF((float)((p0.X + p1.X) / 2),
                                        1200), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                                    DrawingClass.Draw.線段繪製(p0, p1, Color.Lime, 1, g, ZoomX, ZoomY);

                                }
                                else
                                {
                                    DrawingClass.Draw.文字中心繪製("左右間距:", new PointF(1200, 1130), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (間距 / 1D).ToString("0.000")), new PointF((float)((p0.X + p1.X) / 2),
                                        1200), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                                    DrawingClass.Draw.線段繪製(p0, p1, Color.Red, 1, g, ZoomX, ZoomY);

                                }
                            }
                            else if (i > 10)
                            {
                                if (List_CCD02_02_PIN量測參數_左右間距量測_OK[i])
                                {
                                    DrawingClass.Draw.文字中心繪製("左右間距:", new PointF(1200, 1130), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (間距 / 1D).ToString("0.000")), new PointF((float)((p0.X + p1.X) / 2),
                                        1300), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                                    DrawingClass.Draw.線段繪製(p0, p1, Color.Lime, 1, g, ZoomX, ZoomY);

                                }
                                else
                                {
                                    DrawingClass.Draw.文字中心繪製("左右間距:", new PointF(1200, 1130), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (間距 / 1D).ToString("0.000")), new PointF((float)((p0.X + p1.X) / 2),
                                        1300), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                                    DrawingClass.Draw.線段繪製(p0, p1, Color.Red, 1, g, ZoomX, ZoomY);

                                }
                            }
                        }

                    }
                    上排_p2 = new PointF(this.List_CCD02_02_PIN量測參數_量測點[0].X, this.List_CCD02_02_PIN量測參數_量測點[0].Y - 50);
                    上排_p3 = new PointF(this.Point_CCD02_01_中心基準座標_量測點.X, this.List_CCD02_02_PIN量測參數_量測點[0].Y - 50);

                    if (CCD02_02_PIN量測參數_間距上排PIN01到基準_OK)
                    {
                        //DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD02_02_PIN量測參數_間距上排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
                        //    (float)((上排_p2.Y + 上排_p3.Y) / 2) - 80), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        //DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
                    }
                    else
                    {
                        DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD02_02_PIN量測參數_間距上排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
                             (float)((上排_p2.Y + 上排_p3.Y) / 2) - 80), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Red, 1, g, ZoomX, ZoomY);
                    }

                    下排_p2 = new PointF(this.List_CCD02_02_PIN量測參數_量測點[11].X, this.List_CCD02_02_PIN量測參數_量測點[11].Y + 500);
                    下排_p3 = new PointF(this.Point_CCD02_01_中心基準座標_量測點.X, this.List_CCD02_02_PIN量測參數_量測點[11].Y + 500);

                    if (CCD02_02_PIN量測參數_間距下排PIN01到基準_OK)
                    {
                        //DrawingClass.Draw.文字中心繪製(string.Format("下PIN01到基準" + "{0}", (CCD02_02_PIN量測參數_間距下排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((下排_p2.X + 下排_p3.X) / 2),
                        //    (float)((下排_p2.Y + 下排_p3.Y) / 2) + 80), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        //DrawingClass.Draw.線段繪製(下排_p2, 下排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
                    }
                    else
                    {
                        DrawingClass.Draw.文字中心繪製(string.Format("下PIN01到基準" + "{0}", (CCD02_02_PIN量測參數_間距下排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((下排_p2.X + 下排_p3.X) / 2),
                            (float)((下排_p2.Y + 下排_p3.Y) / 2) + 80), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        DrawingClass.Draw.線段繪製(下排_p2, 下排_p3, Color.Red, 1, g, ZoomX, ZoomY);
                    }
                    #endregion
                    #region 水平度顯示
                    DrawingClass.Draw.水平線段繪製(0, 10000, CCD02_01_水平基準線量測_AxLineMsr.MeasuredSlope, CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotX,
                        CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotY + 基準線偏移_上排, Color.Blue, 2, g, ZoomX, ZoomY);

                    DrawingClass.Draw.文字右中繪製("上排輔助線", new PointF((float)this.List_CCD02_02_PIN量測參數_水平度量測顯示點_X[0], CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotY + (float)基準線偏移_上排 + 20)
                        , new Font("標楷體", 10), Color.White, Color.Blue, g, ZoomX, ZoomY);


                    DrawingClass.Draw.水平線段繪製(0, 10000, CCD02_01_水平基準線量測_AxLineMsr.MeasuredSlope, CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotX,
                        CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotY + 基準線偏移_下排, Color.Blue, 2, g, ZoomX, ZoomY);
                    DrawingClass.Draw.文字右中繪製("下排輔助線", new PointF((float)this.List_CCD02_02_PIN量測參數_水平度量測顯示點_X[0], CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotY + (float)基準線偏移_下排 + 20)
                        , new Font("標楷體", 10), Color.White, Color.Blue, g, ZoomX, ZoomY);
                    //for (int i = 0; i < 20; i++)
                    //{
                    //    point = new PointF(this.List_CCD02_02_PIN量測參數_量測點[i].X, this.List_CCD02_02_PIN量測參數_量測點[i].Y);

                    //    上排_to_line_point = new PointF((float)this.List_CCD02_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD02_02_PIN量測參數_水平度量測顯示點_Y[i]));
                    //    下排_to_line_point = new PointF((float)this.List_CCD02_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD02_02_PIN量測參數_水平度量測顯示點_Y[i]));
                    //    水平度 = List_CCD02_02_PIN量測參數_水平度量測距離[i];


                    //    if (List_CCD02_02_PIN量測參數_水平度量測_OK[i])
                    //    {
                    //        if (i <= 10)
                    //        {
                    //            DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                    //            new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Yellow, g, ZoomX, ZoomY);

                    //            DrawingClass.Draw.線段繪製(point, 上排_to_line_point, Color.Yellow, 1, g, ZoomX, ZoomY);
                    //        }
                    //        if (i > 10)
                    //        {
                    //            DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                    //            new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Yellow, g, ZoomX, ZoomY);

                    //            DrawingClass.Draw.線段繪製(point, 下排_to_line_point, Color.Yellow, 1, g, ZoomX, ZoomY);
                    //        }

                    //    }
                    //    else
                    //    {
                    //        DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                    //            new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                    //        DrawingClass.Draw.線段繪製(point, 下排_to_line_point, Color.Red, 1, g, ZoomX, ZoomY);

                    //    }


                    //}

                    for (int i = 0; i < 11; i++)
                    {
                        point = new PointF(this.List_CCD02_02_PIN量測參數_量測點[i].X, this.List_CCD02_02_PIN量測參數_量測點[i].Y);
                        point1 = new PointF(this.List_CCD02_02_PIN量測參數_量測點[i + 11].X, this.List_CCD02_02_PIN量測參數_量測點[i + 11].Y);

                        上排_to_line_point = new PointF((float)this.List_CCD02_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD02_02_PIN量測參數_水平度量測顯示點_Y[i]));
                        下排_to_line_point = new PointF((float)this.List_CCD02_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD02_02_PIN量測參數_水平度量測顯示點_Y[i]));
                        水平度 = List_CCD02_02_PIN量測參數_上下間格距離[i];


                        if (List_CCD02_02_PIN量測參數_水平度量測_OK[i])
                        {

                            DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                            new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Yellow, g, ZoomX, ZoomY);

                            DrawingClass.Draw.線段繪製(point, point1, Color.Yellow, 1, g, ZoomX, ZoomY);


                        }
                        else
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                            DrawingClass.Draw.線段繪製(point, point1, Color.Red, 1, g, ZoomX, ZoomY);

                        }


                    }

                    #endregion
                    #region 結果顯示
                    for (int i = 0; i < 21; i++)
                    {
                        if (i != 10)
                        {
                            if (List_CCD02_02_PIN量測參數_左右間距量測_OK[i] && CCD02_02_PIN量測參數_間距上排PIN01到基準_OK)
                            {
                                DrawingClass.Draw.文字左上繪製("間距量測OK!", new PointF(0, 0), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                            }
                            else
                            {
                                DrawingClass.Draw.文字左上繪製("間距量測NG!", new PointF(0, 0), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            }
                        }
                    }
                    for (int i = 0; i < 11; i++)
                    {
                        if (List_CCD02_02_PIN量測參數_水平度量測_OK[i])
                        {
                            DrawingClass.Draw.文字左上繪製("水平度量測OK!", new PointF(0, 200), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                        }
                        else
                        {
                            DrawingClass.Draw.文字左上繪製("水平度量測NG!", new PointF(0, 200), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        }
                    }
                    #endregion
                    g.Dispose();
                    g = null;
                }
                catch
                {

                }

            }
            else if (PLC_Device_CCD02_02_Tech_檢驗一次.Bool || PLC_Device_CCD02_02_Tech_取像並檢驗.Bool)
            {
                基準線偏移_上排 = this.PLC_Device_CCD02_01_基準線量測_基準線偏移_上排.Value / CCD02_比例尺_pixcel_To_mm / 1000;
                基準線偏移_下排 = this.PLC_Device_CCD02_01_基準線量測_基準線偏移_下排.Value / CCD02_比例尺_pixcel_To_mm / 1000;
                if (this.PLC_Device_CCD02_02_PIN量測_檢測距離計算_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        DrawingClass.Draw.十字中心(new PointF(this.Point_CCD02_01_中心基準座標_量測點.X, this.Point_CCD02_01_中心基準座標_量測點.Y), 100, Color.Red, 2, g, ZoomX, ZoomY);
                        #region 左右間距顯示
                        for (int i = 0; i < 21; i++)
                        {
                            p0 = new PointF(this.List_CCD02_02_PIN量測參數_量測點[i].X, this.List_CCD02_02_PIN量測參數_量測點[i].Y);
                            p1 = new PointF(this.List_CCD02_02_PIN量測參數_量測點[i + 1].X, this.List_CCD02_02_PIN量測參數_量測點[i + 1].Y);
                            間距 = List_CCD02_02_PIN量測參數_左右間距量測距離[i];

                            if (i != 10)
                            {
                                if (List_CCD02_02_PIN量測參數_左右間距量測_OK[i])
                                {
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (間距 / 1D).ToString("0.000")), new PointF((float)((p0.X + p1.X) / 2),
                                        (float)((p0.Y + p1.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                                    DrawingClass.Draw.線段繪製(p0, p1, Color.Lime, 1, g, ZoomX, ZoomY);

                                }
                                else
                                {
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (間距 / 1D).ToString("0.000")), new PointF((float)((p0.X + p1.X) / 2),
                                        (float)((p0.Y + p1.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                                    DrawingClass.Draw.線段繪製(p0, p1, Color.Red, 1, g, ZoomX, ZoomY);

                                }
                            }

                        }

                        上排_p2 = new PointF(this.List_CCD02_02_PIN量測參數_量測點[0].X, this.List_CCD02_02_PIN量測參數_量測點[0].Y - 150);
                        上排_p3 = new PointF(this.Point_CCD02_01_中心基準座標_量測點.X, this.List_CCD02_02_PIN量測參數_量測點[0].Y - 150);

                        if (CCD02_02_PIN量測參數_間距上排PIN01到基準_OK)
                        {
                            //DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD02_02_PIN量測參數_間距上排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
                            //    (float)((上排_p2.Y + 上排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            //DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
                        }
                        else
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD02_02_PIN量測參數_間距上排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
    (float)((上排_p2.Y + 上排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Red, 1, g, ZoomX, ZoomY);
                        }

                        下排_p2 = new PointF(this.List_CCD02_02_PIN量測參數_量測點[11].X, this.List_CCD02_02_PIN量測參數_量測點[11].Y + 150);
                        下排_p3 = new PointF(this.Point_CCD02_01_中心基準座標_量測點.X, this.List_CCD02_02_PIN量測參數_量測點[11].Y + 150);

                        if (CCD02_02_PIN量測參數_間距下排PIN01到基準_OK)
                        {
                            //DrawingClass.Draw.文字中心繪製(string.Format("下PIN01到基準" + "{0}", (CCD02_02_PIN量測參數_間距下排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((下排_p2.X + 下排_p3.X) / 2),
                            //    (float)((下排_p2.Y + 下排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            //DrawingClass.Draw.線段繪製(下排_p2, 下排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
                        }
                        else
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("下PIN01到基準" + "{0}", (CCD02_02_PIN量測參數_間距下排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((下排_p2.X + 下排_p3.X) / 2),
    (float)((下排_p2.Y + 下排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(下排_p2, 下排_p3, Color.Red, 1, g, ZoomX, ZoomY);
                        }
                        #endregion
                        #region 水平度顯示
                        DrawingClass.Draw.水平線段繪製(0, 10000, CCD02_01_水平基準線量測_AxLineMsr.MeasuredSlope, CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotX,
                            CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotY + 基準線偏移_上排, Color.Blue, 2, g, ZoomX, ZoomY);

                        DrawingClass.Draw.文字右中繪製("上排輔助線", new PointF((float)this.List_CCD02_02_PIN量測參數_水平度量測顯示點_X[0], CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotY + (float)基準線偏移_上排 + 20)
                            , new Font("標楷體", 10), Color.White, Color.Blue, g, ZoomX, ZoomY);


                        DrawingClass.Draw.水平線段繪製(0, 10000, CCD02_01_水平基準線量測_AxLineMsr.MeasuredSlope, CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotX,
                            CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotY + 基準線偏移_下排, Color.Blue, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.文字右中繪製("下排輔助線", new PointF((float)this.List_CCD02_02_PIN量測參數_水平度量測顯示點_X[0], CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotY + (float)基準線偏移_下排 + 20)
                            , new Font("標楷體", 10), Color.White, Color.Blue, g, ZoomX, ZoomY);
                        //for (int i = 0; i < 22; i++)
                        //{
                        //    point = new PointF(this.List_CCD02_02_PIN量測參數_量測點[i].X, this.List_CCD02_02_PIN量測參數_量測點[i].Y);

                        //    上排_to_line_point = new PointF((float)this.List_CCD02_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD02_02_PIN量測參數_水平度量測顯示點_Y[i]));
                        //    下排_to_line_point = new PointF((float)this.List_CCD02_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD02_02_PIN量測參數_水平度量測顯示點_Y[i]));
                        //    水平度 = List_CCD02_02_PIN量測參數_水平度量測距離[i];


                        //    if (List_CCD02_02_PIN量測參數_水平度量測_OK[i])
                        //    {
                        //        if (i <= 10)
                        //        {
                        //            DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                        //            new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Yellow, g, ZoomX, ZoomY);

                        //            DrawingClass.Draw.線段繪製(point, 上排_to_line_point, Color.Yellow, 1, g, ZoomX, ZoomY);
                        //        }
                        //        if (i > 10)
                        //        {
                        //            DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                        //            new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Yellow, g, ZoomX, ZoomY);

                        //            DrawingClass.Draw.線段繪製(point, 下排_to_line_point, Color.Yellow, 1, g, ZoomX, ZoomY);
                        //        }

                        //    }
                        //    else
                        //    {
                        //        DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                        //            new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                        //        DrawingClass.Draw.線段繪製(point, 下排_to_line_point, Color.Red, 1, g, ZoomX, ZoomY);

                        //    }


                        //}
                        for (int i = 0; i < 11; i++)
                        {
                            point = new PointF(this.List_CCD02_02_PIN量測參數_量測點[i].X, this.List_CCD02_02_PIN量測參數_量測點[i].Y);
                            point1 = new PointF(this.List_CCD02_02_PIN量測參數_量測點[i + 11].X, this.List_CCD02_02_PIN量測參數_量測點[i + 11].Y);

                            上排_to_line_point = new PointF((float)this.List_CCD02_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD02_02_PIN量測參數_水平度量測顯示點_Y[i]));
                            下排_to_line_point = new PointF((float)this.List_CCD02_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD02_02_PIN量測參數_水平度量測顯示點_Y[i]));
                            水平度 = List_CCD02_02_PIN量測參數_上下間格距離[i];


                            if (List_CCD02_02_PIN量測參數_水平度量測_OK[i])
                            {

                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Yellow, g, ZoomX, ZoomY);

                                DrawingClass.Draw.線段繪製(point, point1, Color.Yellow, 1, g, ZoomX, ZoomY);


                            }
                            else
                            {
                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                    new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                                DrawingClass.Draw.線段繪製(point, point1, Color.Red, 1, g, ZoomX, ZoomY);

                            }


                        }
                        #endregion
                        #region 結果顯示
                        for (int i = 0; i < 21; i++)
                        {
                            if (i != 10)
                            {
                                if (List_CCD02_02_PIN量測參數_左右間距量測_OK[i] && CCD02_02_PIN量測參數_間距上排PIN01到基準_OK && CCD02_02_PIN量測參數_間距下排PIN01到基準_OK)
                                {
                                    DrawingClass.Draw.文字左上繪製("間距量測OK!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                                }
                                else
                                {
                                    DrawingClass.Draw.文字左上繪製("間距量測NG!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                                }
                            }
                        }
                        for (int i = 0; i < 11; i++)
                        {
                            if (List_CCD02_02_PIN量測參數_水平度量測_OK[i])
                            {
                                DrawingClass.Draw.文字左上繪製("水平度量測OK!", new PointF(0, 200), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                            }
                            else
                            {
                                DrawingClass.Draw.文字左上繪製("水平度量測NG!", new PointF(0, 200), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            }
                        }
                        #endregion
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
                 基準線偏移_上排 = this.PLC_Device_CCD02_01_基準線量測_基準線偏移_上排.Value / CCD02_比例尺_pixcel_To_mm / 1000;
                 基準線偏移_下排 = this.PLC_Device_CCD02_01_基準線量測_基準線偏移_下排.Value / CCD02_比例尺_pixcel_To_mm / 1000;
                if (this.PLC_Device_CCD02_02_PIN量測_檢測距離計算_RefreshCanvas.Bool && PLC_Device_CCD02_02_PIN量測_檢測距離計算.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);

                        DrawingClass.Draw.十字中心(new PointF(this.Point_CCD02_01_中心基準座標_量測點.X, this.Point_CCD02_01_中心基準座標_量測點.Y), 100, Color.Red, 2, g, ZoomX, ZoomY);
                        #region 左右間距顯示
                        for (int i = 0; i < 21; i++)
                        {
                            p0 = new PointF(this.List_CCD02_02_PIN量測參數_量測點[i].X, this.List_CCD02_02_PIN量測參數_量測點[i].Y);
                            p1 = new PointF(this.List_CCD02_02_PIN量測參數_量測點[i + 1].X, this.List_CCD02_02_PIN量測參數_量測點[i + 1].Y);
                            間距 = List_CCD02_02_PIN量測參數_左右間距量測距離[i];

                            if (i != 10)
                            {
                                if (List_CCD02_02_PIN量測參數_左右間距量測_OK[i])
                                {
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (間距 / 1D).ToString("0.000")), new PointF((float)((p0.X + p1.X) / 2),
                                        (float)((p0.Y + p1.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                                    DrawingClass.Draw.線段繪製(p0, p1, Color.Lime, 1, g, ZoomX, ZoomY);

                                }
                                else
                                {
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (間距 / 1D).ToString("0.000")), new PointF((float)((p0.X + p1.X) / 2),
                                        (float)((p0.Y + p1.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                                    DrawingClass.Draw.線段繪製(p0, p1, Color.Red, 1, g, ZoomX, ZoomY);

                                }
                            }

                        }
                        上排_p2 = new PointF(this.List_CCD02_02_PIN量測參數_量測點[0].X, this.List_CCD02_02_PIN量測參數_量測點[0].Y - 150);
                        上排_p3 = new PointF(this.Point_CCD02_01_中心基準座標_量測點.X, this.List_CCD02_02_PIN量測參數_量測點[0].Y - 150);

                        if (CCD02_02_PIN量測參數_間距上排PIN01到基準_OK)
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD02_02_PIN量測參數_間距上排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
                                (float)((上排_p2.Y + 上排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
                        }
                        else
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD02_02_PIN量測參數_間距上排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
    (float)((上排_p2.Y + 上排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Red, 1, g, ZoomX, ZoomY);
                        }

                        下排_p2 = new PointF(this.List_CCD02_02_PIN量測參數_量測點[11].X, this.List_CCD02_02_PIN量測參數_量測點[11].Y + 150);
                        下排_p3 = new PointF(this.Point_CCD02_01_中心基準座標_量測點.X, this.List_CCD02_02_PIN量測參數_量測點[11].Y + 150);

                        if (CCD02_02_PIN量測參數_間距下排PIN01到基準_OK)
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("下PIN01到基準" + "{0}", (CCD02_02_PIN量測參數_間距下排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((下排_p2.X + 下排_p3.X) / 2),
                                (float)((下排_p2.Y + 下排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(下排_p2, 下排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
                        }
                        else
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("下PIN01到基準" + "{0}", (CCD02_02_PIN量測參數_間距下排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((下排_p2.X + 下排_p3.X) / 2),
    (float)((下排_p2.Y + 下排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(下排_p2, 下排_p3, Color.Red, 1, g, ZoomX, ZoomY);
                        }


                        #endregion
                        #region 水平度顯示
                        DrawingClass.Draw.水平線段繪製(0, 10000, CCD02_01_水平基準線量測_AxLineMsr.MeasuredSlope, CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotX,
                            CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotY + 基準線偏移_上排, Color.Blue, 2, g, ZoomX, ZoomY);

                        DrawingClass.Draw.文字右中繪製("上排輔助線", new PointF((float)this.List_CCD02_02_PIN量測參數_水平度量測顯示點_X[0], CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotY + (float)基準線偏移_上排 + 20)
                            , new Font("標楷體", 10), Color.White, Color.Blue, g, ZoomX, ZoomY);


                        DrawingClass.Draw.水平線段繪製(0, 10000, CCD02_01_水平基準線量測_AxLineMsr.MeasuredSlope, CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotX,
                            CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotY + 基準線偏移_下排, Color.Blue, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.文字右中繪製("下排輔助線", new PointF((float)this.List_CCD02_02_PIN量測參數_水平度量測顯示點_X[0], CCD02_01_水平基準線量測_AxLineMsr.MeasuredPivotY + (float)基準線偏移_下排 + 20)
                            , new Font("標楷體", 10), Color.White, Color.Blue, g, ZoomX, ZoomY);

                        //for (int i = 0; i < 20; i++)
                        //{
                        //    point = new PointF(this.List_CCD02_02_PIN量測參數_量測點[i].X, this.List_CCD02_02_PIN量測參數_量測點[i].Y);

                        //    上排_to_line_point = new PointF((float)this.List_CCD02_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD02_02_PIN量測參數_水平度量測顯示點_Y[i]));
                        //    下排_to_line_point = new PointF((float)this.List_CCD02_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD02_02_PIN量測參數_水平度量測顯示點_Y[i]));
                        //    水平度 = List_CCD02_02_PIN量測參數_水平度量測距離[i];


                        //    if (List_CCD02_02_PIN量測參數_水平度量測_OK[i])
                        //    {
                        //        if (i <= 10)
                        //        {
                        //            DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                        //            new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Yellow, g, ZoomX, ZoomY);

                        //            DrawingClass.Draw.線段繪製(point, 上排_to_line_point, Color.Yellow, 1, g, ZoomX, ZoomY);
                        //        }
                        //        if (i > 10)
                        //        {
                        //            DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                        //            new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Yellow, g, ZoomX, ZoomY);

                        //            DrawingClass.Draw.線段繪製(point, 下排_to_line_point, Color.Yellow, 1, g, ZoomX, ZoomY);
                        //        }

                        //    }
                        //    else
                        //    {
                        //        DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                        //            new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                        //        DrawingClass.Draw.線段繪製(point, 下排_to_line_point, Color.Red, 1, g, ZoomX, ZoomY);

                        //    }


                        //}
                        for (int i = 0; i < 11; i++)
                        {
                            point = new PointF(this.List_CCD02_02_PIN量測參數_量測點[i].X, this.List_CCD02_02_PIN量測參數_量測點[i].Y);
                            point1 = new PointF(this.List_CCD02_02_PIN量測參數_量測點[i + 11].X, this.List_CCD02_02_PIN量測參數_量測點[i + 11].Y);

                            上排_to_line_point = new PointF((float)this.List_CCD02_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD02_02_PIN量測參數_水平度量測顯示點_Y[i]));
                            下排_to_line_point = new PointF((float)this.List_CCD02_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD02_02_PIN量測參數_水平度量測顯示點_Y[i]));
                            水平度 = List_CCD02_02_PIN量測參數_上下間格距離[i];


                            if (List_CCD02_02_PIN量測參數_水平度量測_OK[i])
                            {

                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                    new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Yellow, g, ZoomX, ZoomY);

                                    DrawingClass.Draw.線段繪製(point, point1, Color.Yellow, 1, g, ZoomX, ZoomY);
                                

                            }
                            else
                            {
                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                    new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                                DrawingClass.Draw.線段繪製(point, point1, Color.Red, 1, g, ZoomX, ZoomY);

                            }


                        }
                        #endregion
                        #region 結果顯示

                        for (int i = 0; i < 21; i++)
                        {
                            if (i != 10)
                            {
                                if (List_CCD02_02_PIN量測參數_左右間距量測_OK[i] && CCD02_02_PIN量測參數_間距上排PIN01到基準_OK)
                                {
                                    DrawingClass.Draw.文字左上繪製("間距量測OK!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                                }
                                else
                                {
                                    DrawingClass.Draw.文字左上繪製("間距量測NG!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                                }
                            }
                        }
                        for (int i = 0; i < 11; i++)
                        {
                            if (List_CCD02_02_PIN量測參數_水平度量測_OK[i])
                            {
                                DrawingClass.Draw.文字左上繪製("水平度量測OK!", new PointF(0, 200), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                            }
                            else
                            {
                                DrawingClass.Draw.文字左上繪製("水平度量測NG!", new PointF(0, 200), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            }
                        }
                        #endregion
                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }
                }

            }

            this.PLC_Device_CCD02_02_PIN量測_檢測距離計算_RefreshCanvas.Bool = false;
        }

        int cnt_Program_CCD02_02_PIN量測_檢測距離計算 = 65534;
        void sub_Program_CCD02_02_PIN量測_檢測距離計算()
        {
            if (cnt_Program_CCD02_02_PIN量測_檢測距離計算 == 65534)
            {
                this.h_Canvas_Tech_CCD02_02.OnCanvasDrawEvent += H_Canvas_Tech_CCD02_02_PIN間距量測_OnCanvasDrawEvent;
                this.h_Canvas_Main_CCD02_02_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD02_02_PIN間距量測_OnCanvasDrawEvent;
                PLC_Device_CCD02_02_PIN量測_檢測距離計算.SetComment("PLC_CCD02_02_PIN量測_檢測距離計算");
                PLC_Device_CCD02_02_PIN量測_檢測距離計算.Bool = false;
                PLC_Device_CCD02_02_PIN量測_檢測距離計算按鈕.Bool = false;
                cnt_Program_CCD02_02_PIN量測_檢測距離計算 = 65535;

            }
            if (cnt_Program_CCD02_02_PIN量測_檢測距離計算 == 65535) cnt_Program_CCD02_02_PIN量測_檢測距離計算 = 1;
            if (cnt_Program_CCD02_02_PIN量測_檢測距離計算 == 1) cnt_Program_CCD02_02_PIN量測_檢測距離計算_觸發按下(ref cnt_Program_CCD02_02_PIN量測_檢測距離計算);
            if (cnt_Program_CCD02_02_PIN量測_檢測距離計算 == 2) cnt_Program_CCD02_02_PIN量測_檢測距離計算_檢查按下(ref cnt_Program_CCD02_02_PIN量測_檢測距離計算);
            if (cnt_Program_CCD02_02_PIN量測_檢測距離計算 == 3) cnt_Program_CCD02_02_PIN量測_檢測距離計算_初始化(ref cnt_Program_CCD02_02_PIN量測_檢測距離計算);
            if (cnt_Program_CCD02_02_PIN量測_檢測距離計算 == 4) cnt_Program_CCD02_02_PIN量測_檢測距離計算_數值計算(ref cnt_Program_CCD02_02_PIN量測_檢測距離計算);
            if (cnt_Program_CCD02_02_PIN量測_檢測距離計算 == 5) cnt_Program_CCD02_02_PIN量測_檢測距離計算_量測結果(ref cnt_Program_CCD02_02_PIN量測_檢測距離計算);
            if (cnt_Program_CCD02_02_PIN量測_檢測距離計算 == 6) cnt_Program_CCD02_02_PIN量測_檢測距離計算_繪製畫布(ref cnt_Program_CCD02_02_PIN量測_檢測距離計算);
            if (cnt_Program_CCD02_02_PIN量測_檢測距離計算 == 7) cnt_Program_CCD02_02_PIN量測_檢測距離計算 = 65500;
            if (cnt_Program_CCD02_02_PIN量測_檢測距離計算 > 1) cnt_Program_CCD02_02_PIN量測_檢測距離計算_檢查放開(ref cnt_Program_CCD02_02_PIN量測_檢測距離計算);

            if (cnt_Program_CCD02_02_PIN量測_檢測距離計算 == 65500)
            {
                PLC_Device_CCD02_02_PIN量測_檢測距離計算.Bool = false;
                PLC_Device_CCD02_02_PIN量測_檢測距離計算按鈕.Bool = false;
                cnt_Program_CCD02_02_PIN量測_檢測距離計算 = 65535;
            }
        }
        void cnt_Program_CCD02_02_PIN量測_檢測距離計算_觸發按下(ref int cnt)
        {
            if (PLC_Device_CCD02_02_PIN量測_檢測距離計算按鈕.Bool)
            {
                PLC_Device_CCD02_02_PIN量測_檢測距離計算.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD02_02_PIN量測_檢測距離計算_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD02_02_PIN量測_檢測距離計算.Bool)
            {
                cnt++;
            }

        }
        void cnt_Program_CCD02_02_PIN量測_檢測距離計算_檢查放開(ref int cnt)
        {
            //if (!PLC_Device_CCD02_02_PIN量測_檢測距離計算.Bool) cnt = 65500;
        }
        void cnt_Program_CCD02_02_PIN量測_檢測距離計算_初始化(ref int cnt)
        {
            this.MyTimer_CCD02_02_PIN量測_檢測距離計算.TickStop();
            this.MyTimer_CCD02_02_PIN量測_檢測距離計算.StartTickTime(99999);

            this.List_CCD02_02_PIN量測參數_左右間距量測距離 = new double[21];
            this.List_CCD02_02_PIN量測參數_上下間距量測距離 = new double[21];
            this.List_CCD02_02_PIN量測參數_水平度量測距離 = new double[22];
            this.List_CCD02_02_PIN量測參數_上下間格距離 = new double[11];
            this.CCD02_02_PIN量測參數_間距上排PIN01到基準距離 = new double();
            this.CCD02_02_PIN量測參數_間距下排PIN01到基準距離 = new double();

            this.List_CCD02_02_PIN量測參數_量測點_OK = new bool[22];
            this.List_CCD02_02_PIN量測參數_左右間距量測_OK = new bool[21];
            this.List_CCD02_02_PIN量測參數_上下間距量測_OK = new bool[21];
            this.List_CCD02_02_PIN量測參數_水平度量測_OK = new bool[22];
            this.CCD02_02_PIN量測參數_間距上排PIN01到基準_OK = new bool();
            this.CCD02_02_PIN量測參數_間距下排PIN01到基準_OK = new bool();



            cnt++;
        }
        void cnt_Program_CCD02_02_PIN量測_檢測距離計算_數值計算(ref int cnt)
        {
            double 基準線偏移_上排 = this.PLC_Device_CCD02_01_基準線量測_基準線偏移_上排.Value / CCD02_比例尺_pixcel_To_mm / 1000;
            double 基準線偏移_下排 = this.PLC_Device_CCD02_01_基準線量測_基準線偏移_下排.Value / CCD02_比例尺_pixcel_To_mm / 1000;
            #region 水平度數值計算

            this.CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.LinePivotX = this.CCD02_01_水平基準線量測_AxLineRegression.FittedPivotX;
            this.CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.LinePivotY = this.CCD02_01_水平基準線量測_AxLineRegression.FittedPivotY;
            this.CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.LineHorzVert = AxOvkMsr.TxAxLineHorzVert.AX_LINE_QUASI_HORIZONTAL;
            this.CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.LineSlope = this.CCD02_01_水平基準線量測_AxLineRegression.FittedSlope;

            this.CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.LinePivotX = this.CCD02_01_水平基準線量測_AxLineRegression.FittedPivotX;
            this.CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.LinePivotY = this.CCD02_01_水平基準線量測_AxLineRegression.FittedPivotY;
            this.CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.LineHorzVert = AxOvkMsr.TxAxLineHorzVert.AX_LINE_QUASI_HORIZONTAL;
            this.CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.LineSlope = this.CCD02_01_水平基準線量測_AxLineRegression.FittedSlope;
            for (int i = 0; i < this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {

                if (this.List_CCD02_02_PIN量測參數_量測點_有無[i])
                {
                    if(i <= 10)
                    {
                        this.CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.PivotX = this.List_CCD02_02_PIN量測參數_量測點[i].X;
                        this.CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.PivotY = this.List_CCD02_02_PIN量測參數_量測點[i].Y;
                        this.CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.FindDistance();
                        this.List_CCD02_02_PIN量測參數_水平度量測顯示點_X[i] = CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.ProjectPivotX;
                        this.List_CCD02_02_PIN量測參數_水平度量測顯示點_Y[i] = CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.ProjectPivotY;

                        this.List_CCD02_02_PIN量測參數_水平度量測距離[i] = this.CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.Distance * this.CCD02_比例尺_pixcel_To_mm;
                    }
                    if (i > 10)
                    {
                        this.CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.PivotX = this.List_CCD02_02_PIN量測參數_量測點[i].X;
                        this.CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.PivotY = this.List_CCD02_02_PIN量測參數_量測點[i].Y;
                        this.CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.FindDistance();
                        this.List_CCD02_02_PIN量測參數_水平度量測顯示點_X[i] = CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.ProjectPivotX;
                        this.List_CCD02_02_PIN量測參數_水平度量測顯示點_Y[i] = CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.ProjectPivotY;

                        this.List_CCD02_02_PIN量測參數_水平度量測距離[i] = this.CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.Distance * this.CCD02_比例尺_pixcel_To_mm;
                    }
                }
                List_CCD02_02_PIN量測參數_上下間格距離[0] = Math.Abs(List_CCD02_02_PIN量測參數_水平度量測距離[0] - List_CCD02_02_PIN量測參數_水平度量測距離[11]);
                List_CCD02_02_PIN量測參數_上下間格距離[1] = Math.Abs(List_CCD02_02_PIN量測參數_水平度量測距離[1] - List_CCD02_02_PIN量測參數_水平度量測距離[12]);
                List_CCD02_02_PIN量測參數_上下間格距離[2] = Math.Abs(List_CCD02_02_PIN量測參數_水平度量測距離[2] - List_CCD02_02_PIN量測參數_水平度量測距離[13]);
                List_CCD02_02_PIN量測參數_上下間格距離[3] = Math.Abs(List_CCD02_02_PIN量測參數_水平度量測距離[3] - List_CCD02_02_PIN量測參數_水平度量測距離[14]);
                List_CCD02_02_PIN量測參數_上下間格距離[4] = Math.Abs(List_CCD02_02_PIN量測參數_水平度量測距離[4] - List_CCD02_02_PIN量測參數_水平度量測距離[15]);
                List_CCD02_02_PIN量測參數_上下間格距離[5] = Math.Abs(List_CCD02_02_PIN量測參數_水平度量測距離[5] - List_CCD02_02_PIN量測參數_水平度量測距離[16]);
                List_CCD02_02_PIN量測參數_上下間格距離[6] = Math.Abs(List_CCD02_02_PIN量測參數_水平度量測距離[6] - List_CCD02_02_PIN量測參數_水平度量測距離[17]);
                List_CCD02_02_PIN量測參數_上下間格距離[7] = Math.Abs(List_CCD02_02_PIN量測參數_水平度量測距離[7] - List_CCD02_02_PIN量測參數_水平度量測距離[18]);
                List_CCD02_02_PIN量測參數_上下間格距離[8] = Math.Abs(List_CCD02_02_PIN量測參數_水平度量測距離[8] - List_CCD02_02_PIN量測參數_水平度量測距離[19]);
                List_CCD02_02_PIN量測參數_上下間格距離[9] = Math.Abs(List_CCD02_02_PIN量測參數_水平度量測距離[9] - List_CCD02_02_PIN量測參數_水平度量測距離[20]);
                List_CCD02_02_PIN量測參數_上下間格距離[10] = Math.Abs(List_CCD02_02_PIN量測參數_水平度量測距離[10] - List_CCD02_02_PIN量測參數_水平度量測距離[21]);

            }
            #endregion
            #region 左右間距數值計算
            double distance = 0;
            double 間距Temp1_X = 0;
            double 間距Temp2_X = 0;

            for (int i = 0; i < 21; i++)
            {
                if (this.List_CCD02_02_PIN量測參數_量測點_有無[i] && this.List_CCD02_02_PIN量測參數_量測點_有無[i + 1])
                {

                    間距Temp1_X = this.List_CCD02_02_PIN量測參數_量測點[i].X - this.Point_CCD02_01_中心基準座標_量測點.X;
                    間距Temp2_X = this.List_CCD02_02_PIN量測參數_量測點[i + 1].X - this.Point_CCD02_01_中心基準座標_量測點.X;

                    distance = Math.Abs(間距Temp1_X - 間距Temp2_X);

                    this.List_CCD02_02_PIN量測參數_左右間距量測距離[i] = distance * this.CCD02_比例尺_pixcel_To_mm;
                }
                else
                {
                    PLC_Device_CCD02_02_PIN量測_檢測距離計算_OK.Bool = false;
                    List_CCD02_02_PIN量測參數_量測點_OK[i] = false;
                }

            }
            #endregion
            cnt++;
        }
        void cnt_Program_CCD02_02_PIN量測_檢測距離計算_量測結果(ref int cnt)
        {

            PLC_Device_CCD02_02_PIN量測_檢測距離計算_OK.Bool = true; // 檢測結果初始化
            #region 左右間距量測判斷

            for (int i = 0; i < 21; i++)
            {
                int 標準值 = this.PLC_Device_CCD02_02_PIN量測_左右間距量測標準值.Value;
                int 標準值上限 = this.PLC_Device_CCD02_02_PIN量測_左右間距量測上限值.Value;
                int 標準值下限 = this.PLC_Device_CCD02_02_PIN量測_左右間距量測下限值.Value;
                double 量測距離 = this.List_CCD02_02_PIN量測參數_左右間距量測距離[i];

                量測距離 = 量測距離 * 1000 - 標準值;
                量測距離 /= 1000;
                if (!PLC_Device_CCD02_02_PIN量測_間距量測不測試.Bool)
                {
                    if (this.List_CCD02_02_PIN量測參數_量測點_有無[i])
                    {
                        if (量測距離 >= 0 && i != 10)
                        {
                            if (標準值上限 <= Math.Abs(量測距離) * 1000 || 標準值下限 > Math.Abs(量測距離) * 1000)
                            {
                                this.List_CCD02_02_PIN量測參數_左右間距量測_OK[i] = false;
                                PLC_Device_CCD02_02_PIN量測_檢測距離計算_OK.Bool = false;
                            }
                            else
                            {
                                this.List_CCD02_02_PIN量測參數_左右間距量測_OK[i] = true;
                            }
                        }
                    }
                }
                else
                {
                    PLC_Device_CCD02_02_PIN量測_檢測距離計算_OK.Bool = true;
                    this.List_CCD02_02_PIN量測參數_左右間距量測_OK[i] = true;
                }



                this.List_CCD02_02_PIN量測參數_左右間距量測距離[i] = 量測距離;

            }
            #endregion
            #region 水平度量測判斷
            //for (int i = 0; i < 22; i++)
            //{
            //    int 上排標準值 = this.PLC_Device_CCD02_02_PIN量測_上排水平度量測標準值.Value;
            //    int 上排標準值上限 = this.PLC_Device_CCD02_02_PIN量測_上排水平度量測上限值.Value;
            //    int 上排標準值下限 = this.PLC_Device_CCD02_02_PIN量測_上排水平度量測下限值.Value;
            //    double 上排量測距離 = this.List_CCD02_02_PIN量測參數_水平度量測距離[i];

            //    int 下排標準值 = this.PLC_Device_CCD02_02_PIN量測_下排水平度量測標準值.Value;
            //    int 下排標準值上限 = this.PLC_Device_CCD02_02_PIN量測_下排水平度量測上限值.Value;
            //    int 下排標準值下限 = this.PLC_Device_CCD02_02_PIN量測_下排水平度量測下限值.Value;
            //    double 下排量測距離 = this.List_CCD02_02_PIN量測參數_水平度量測距離[i];

            //    上排量測距離 = 上排量測距離 * 1000 - 上排標準值;
            //    上排量測距離 /= 1000;

            //    下排量測距離 = 下排量測距離 * 1000 - 下排標準值;
            //    下排量測距離 /= 1000;
            //    if (!PLC_Device_CCD02_02_PIN量測_水平度量測不測試.Bool)
            //    {
            //        if (this.List_CCD02_02_PIN量測參數_量測點_有無[i])
            //        {
            //            if (上排量測距離 >= 0 && i < 11)
            //            {
            //                if (上排標準值上限 <= Math.Abs(上排量測距離) * 1000 || 上排標準值下限 > Math.Abs(上排量測距離) * 1000)
            //                {
            //                    this.List_CCD02_02_PIN量測參數_水平度量測_OK[i] = false;
            //                    PLC_Device_CCD02_02_PIN量測_檢測距離計算_OK.Bool = false;
            //                }
            //                else
            //                {
            //                    this.List_CCD02_02_PIN量測參數_水平度量測_OK[i] = true;
            //                }
            //                this.List_CCD02_02_PIN量測參數_水平度量測距離[i] = 上排量測距離;
            //            }
            //            else if (下排量測距離 >= 0 && i >= 11)
            //            {
            //                if (下排標準值上限 <= Math.Abs(下排量測距離) * 1000 || 下排標準值下限 > Math.Abs(下排量測距離) * 1000)
            //                {
            //                    this.List_CCD02_02_PIN量測參數_水平度量測_OK[i] = false;
            //                    PLC_Device_CCD02_02_PIN量測_檢測距離計算_OK.Bool = false;
            //                }
            //                else
            //                {
            //                    this.List_CCD02_02_PIN量測參數_水平度量測_OK[i] = true;
            //                }
            //                this.List_CCD02_02_PIN量測參數_水平度量測距離[i] = 下排量測距離;
            //            }

            //        }
            //    }
            //    else
            //    {
            //        this.List_CCD02_02_PIN量測參數_水平度量測_OK[i] = true;
            //    }
            //    if (PLC_Device_CCD02_02_PIN量測_間距量測不測試.Bool && PLC_Device_CCD02_02_PIN量測_水平度量測不測試.Bool)
            //    {
            //        PLC_Device_CCD02_02_PIN量測_檢測距離計算_OK.Bool = true;
            //    }



            //}
            for (int i = 0; i < 11; i++)
            {
                int 標準值 = this.PLC_Device_CCD02_02_PIN量測_上排水平度量測標準值.Value;
                int 標準值上限 = this.PLC_Device_CCD02_02_PIN量測_上排水平度量測上限值.Value;
                int 標準值下限 = this.PLC_Device_CCD02_02_PIN量測_上排水平度量測下限值.Value;
                double 量測距離 = this.List_CCD02_02_PIN量測參數_上下間格距離[i];

                量測距離 = 量測距離 * 1000 - 標準值;
                量測距離 /= 1000;
                if (!PLC_Device_CCD02_02_PIN量測_水平度量測不測試.Bool)
                {
                    if (this.List_CCD02_02_PIN量測參數_量測點_有無[i])
                    {

                        if (標準值上限 <= Math.Abs(量測距離) * 1000 || 標準值下限 > Math.Abs(量測距離) * 1000)
                        {
                            this.List_CCD02_02_PIN量測參數_水平度量測_OK[i] = false;
                            PLC_Device_CCD02_02_PIN量測_檢測距離計算_OK.Bool = false;
                        }
                        else
                        {
                            this.List_CCD02_02_PIN量測參數_水平度量測_OK[i] = true;
                        }
                        this.List_CCD02_02_PIN量測參數_上下間格距離[i] = 量測距離;

                    }
                }
                else
                {
                    this.List_CCD02_02_PIN量測參數_水平度量測_OK[i] = true;
                }
                if (PLC_Device_CCD02_02_PIN量測_間距量測不測試.Bool && PLC_Device_CCD02_02_PIN量測_水平度量測不測試.Bool)
                {
                    PLC_Device_CCD02_02_PIN量測_檢測距離計算_OK.Bool = true;
                }



            }
            #endregion
            #region 間距上排PIN01到基準線量測

            double temp_上排PIN01到基準 = 0;
            int 間距上排PIN01到基準標準值 = this.PLC_Device_CCD02_02_PIN量測_間距上排PIN01到基準數值.Value;
            int 間距上排PIN01到基準標準值上限 = this.PLC_Device_CCD02_02_PIN量測_間距上排PIN01到基準上限.Value;
            int 間距上排PIN01到基準標準值下限 = this.PLC_Device_CCD02_02_PIN量測_間距上排PIN01到基準下限.Value;


            if (this.List_CCD02_02_PIN量測參數_量測點_有無[0])
            {
                temp_上排PIN01到基準 = Math.Abs(this.List_CCD02_02_PIN量測參數_量測點[0].X - this.Point_CCD02_01_中心基準座標_量測點.X);
                this.CCD02_02_PIN量測參數_間距上排PIN01到基準距離 = temp_上排PIN01到基準 * this.CCD02_比例尺_pixcel_To_mm;
            }
            else
            {
                //PLC_Device_CCD02_02_PIN量測_檢測距離計算_OK.Bool = false;
                CCD02_02_PIN量測參數_間距上排PIN01到基準_OK = false;
            }
            double 間距上排PIN01到基準量測距離 = this.CCD02_02_PIN量測參數_間距上排PIN01到基準距離;


            間距上排PIN01到基準量測距離 = 間距上排PIN01到基準量測距離 * 1000 - 間距上排PIN01到基準標準值;
            間距上排PIN01到基準量測距離 /= 1000;

            if (!PLC_Device_CCD02_02_PIN量測_間距量測不測試.Bool)
            {
                if (this.List_CCD02_02_PIN量測參數_量測點_有無[0])
                {
                    if (間距上排PIN01到基準標準值上限 <= Math.Abs(間距上排PIN01到基準量測距離) * 1000 || 間距上排PIN01到基準標準值下限 >
                        Math.Abs(間距上排PIN01到基準量測距離) * 1000)
                    {
                        this.CCD02_02_PIN量測參數_間距上排PIN01到基準_OK = false;
                        //PLC_Device_CCD02_02_PIN量測_檢測距離計算_OK.Bool = false;
                    }
                    else
                    {
                        this.CCD02_02_PIN量測參數_間距上排PIN01到基準_OK = true;
                    }

                }
                CCD02_02_PIN量測參數_間距上排PIN01到基準距離 = 間距上排PIN01到基準量測距離;
            }
            else
            {
                this.CCD02_02_PIN量測參數_間距上排PIN01到基準_OK = true;
                //this.PLC_Device_CCD02_02_PIN量測_檢測距離計算_OK.Bool = true;
            }
            this.CCD02_02_PIN量測參數_間距上排PIN01到基準_OK = true;
            #endregion
            #region 間距下排PIN01到基準線量測

            double temp_下排PIN01到基準 = 0;
            int 間距下排PIN01到基準標準值 = this.PLC_Device_CCD02_02_PIN量測_間距下排PIN01到基準數值.Value;
            int 間距下排PIN01到基準標準值上限 = this.PLC_Device_CCD02_02_PIN量測_間距下排PIN01到基準上限.Value;
            int 間距下排PIN01到基準標準值下限 = this.PLC_Device_CCD02_02_PIN量測_間距下排PIN01到基準下限.Value;


            if (this.List_CCD02_02_PIN量測參數_量測點_有無[10])
            {
                temp_下排PIN01到基準 = Math.Abs(this.List_CCD02_02_PIN量測參數_量測點[11].X - this.Point_CCD02_01_中心基準座標_量測點.X);
                this.CCD02_02_PIN量測參數_間距下排PIN01到基準距離 = temp_下排PIN01到基準 * this.CCD02_比例尺_pixcel_To_mm;
            }
            else
            {
                //PLC_Device_CCD02_02_PIN量測_檢測距離計算_OK.Bool = false;
                CCD02_02_PIN量測參數_間距下排PIN01到基準_OK = false;
            }
            double 間距下排PIN01到基準量測距離 = this.CCD02_02_PIN量測參數_間距下排PIN01到基準距離;


            間距下排PIN01到基準量測距離 = 間距下排PIN01到基準量測距離 * 1000 - 間距下排PIN01到基準標準值;
            間距下排PIN01到基準量測距離 /= 1000;

            if (!PLC_Device_CCD02_02_PIN量測_間距量測不測試.Bool)
            {
                if (this.List_CCD02_02_PIN量測參數_量測點_有無[11])
                {
                    if (間距下排PIN01到基準標準值上限 <= Math.Abs(間距下排PIN01到基準量測距離) * 1000 || 間距下排PIN01到基準標準值下限 >
                        Math.Abs(間距下排PIN01到基準量測距離) * 1000)
                    {
                        this.CCD02_02_PIN量測參數_間距下排PIN01到基準_OK = false;
                        //PLC_Device_CCD02_02_PIN量測_檢測距離計算_OK.Bool = false;
                    }
                    else
                    {
                        this.CCD02_02_PIN量測參數_間距下排PIN01到基準_OK = true;
                    }

                }
                CCD02_02_PIN量測參數_間距下排PIN01到基準距離 = 間距下排PIN01到基準量測距離;
            }
            else
            {
                this.CCD02_02_PIN量測參數_間距下排PIN01到基準_OK = true;
                //this.PLC_Device_CCD02_02_PIN量測_檢測距離計算_OK.Bool = true;
            }
            this.CCD02_02_PIN量測參數_間距下排PIN01到基準_OK = true;
            #endregion
            cnt++;
        }
        void cnt_Program_CCD02_02_PIN量測_檢測距離計算_繪製畫布(ref int cnt)
        {
            this.PLC_Device_CCD02_02_PIN量測_檢測距離計算_RefreshCanvas.Bool = true;
            if (this.PLC_Device_CCD02_02_PIN量測_檢測距離計算按鈕.Bool && !PLC_Device_CCD02_02_計算一次.Bool)
            {

                this.h_Canvas_Tech_CCD02_02.RefreshCanvas();
            }
            cnt++;
        }
        #endregion
        #region PLC_CCD02_02_PIN正位度量測_設定規範位置
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_設定規範按鈕 = new PLC_Device("S6310");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_設定規範位置 = new PLC_Device("S6305");
        PLC_Device PLC_Device_CCD02_02_PIN設定規範位置_OK = new PLC_Device("S6306");
        PLC_Device PLC_Device_CCD02_02_PIN設定規範位置_測試完成 = new PLC_Device("S6307");
        PLC_Device PLC_Device_CCD02_02_PIN設定規範位置_RefreshCanvas = new PLC_Device("S6308");
        private List<PLC_Device> List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y = new List<PLC_Device>();
        private AxOvkPat.AxVisionInspectionFrame CCD02_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整;

        #region 正位度規範值
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN01 = new PLC_Device("F11300");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN02 = new PLC_Device("F11301");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN03 = new PLC_Device("F11302");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN04 = new PLC_Device("F11303");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN05 = new PLC_Device("F11304");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN06 = new PLC_Device("F11305");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN07 = new PLC_Device("F11306");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN08 = new PLC_Device("F11307");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN09 = new PLC_Device("F11308");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN10 = new PLC_Device("F11309");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_上排PIN11 = new PLC_Device("F11340");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN11 = new PLC_Device("F11310");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN12 = new PLC_Device("F11311");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN13 = new PLC_Device("F11312");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN14 = new PLC_Device("F11313");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN15 = new PLC_Device("F11314");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN16 = new PLC_Device("F11315");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN17 = new PLC_Device("F11316");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN18 = new PLC_Device("F11317");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN19 = new PLC_Device("F11318");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN20 = new PLC_Device("F11319");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_下排PIN11 = new PLC_Device("F11341");

        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN01 = new PLC_Device("F11320");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN02 = new PLC_Device("F11321");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN03 = new PLC_Device("F11322");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN04 = new PLC_Device("F11323");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN05 = new PLC_Device("F11324");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN06 = new PLC_Device("F11325");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN07 = new PLC_Device("F11326");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN08 = new PLC_Device("F11327");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN09 = new PLC_Device("F11328");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN10 = new PLC_Device("F11329");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_上排PIN11 = new PLC_Device("F11342");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN11 = new PLC_Device("F11330");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN12 = new PLC_Device("F11331");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN13 = new PLC_Device("F11332");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN14 = new PLC_Device("F11333");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN15 = new PLC_Device("F11334");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN16 = new PLC_Device("F11335");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN17 = new PLC_Device("F11336");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN18 = new PLC_Device("F11337");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN19 = new PLC_Device("F11338");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN20 = new PLC_Device("F11339");
        PLC_Device PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_下排PIN11 = new PLC_Device("F11343");
        #endregion
        private PointF[] List_CCD02_02_PIN正位度量測參數_規範點 = new PointF[22];
        private PointF[] List_CCD02_02_PIN正位度量測參數_轉換後座標 = new PointF[22];
        private double[] List_CCD02_02_PIN正位度量測參數_正位度規範點_X = new double[22];
        private double[] List_CCD02_02_PIN正位度量測參數_正位度規範點_Y = new double[22];

        int cnt_Program_CCD02_02_PIN正位度量測_設定規範位置 = 65534;

        private void H_Canvas_Tech_CCD02_02_PIN正位度設定規範位置_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {

            if (PLC_Device_CCD02_02_Main_取像並檢驗.Bool || PLC_Device_CCD02_02_PLC觸發檢測.Bool || PLC_Device_CCD02_02_Main_檢驗一次按鈕.Bool)
            {
                try
                {
                    Graphics g = Graphics.FromHdc((IntPtr)HDC);
                    for (int i = 0; i < 22; i++)
                    {
                        DrawingClass.Draw.十字中心(new PointF((float)List_CCD02_02_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD02_02_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
                    }
                    g.Dispose();
                    g = null;
                }
                catch
                {

                }

            }

            else if (PLC_Device_CCD02_02_Tech_檢驗一次.Bool || PLC_Device_CCD02_02_Tech_取像並檢驗.Bool)
            {
                if (this.PLC_Device_CCD02_02_PIN設定規範位置_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        for (int i = 0; i < 22; i++)
                        {
                            DrawingClass.Draw.十字中心(new PointF((float)List_CCD02_02_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD02_02_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
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
                if (this.PLC_Device_CCD02_02_PIN設定規範位置_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        for (int i = 0; i < 22; i++)
                        {
                            DrawingClass.Draw.十字中心(new PointF((float)List_CCD02_02_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD02_02_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
                        }
                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }
                }
            }



            this.PLC_Device_CCD02_02_PIN設定規範位置_RefreshCanvas.Bool = false;
        }
        void sub_Program_CCD02_02_PIN正位度量測_設定規範位置()
        {
            if (cnt_Program_CCD02_02_PIN正位度量測_設定規範位置 == 65534)
            {

                this.h_Canvas_Tech_CCD02_02.OnCanvasDrawEvent += H_Canvas_Tech_CCD02_02_PIN正位度設定規範位置_OnCanvasDrawEvent;
                this.h_Canvas_Main_CCD02_02_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD02_02_PIN正位度設定規範位置_OnCanvasDrawEvent;

                PLC_Device_CCD02_02_PIN正位度量測_設定規範位置.SetComment("PLC_CCD02_02_PIN正位度量測_設定規範位置");
                PLC_Device_CCD02_02_PIN正位度量測_設定規範按鈕.Bool = false;
                PLC_Device_CCD02_02_PIN正位度量測_設定規範位置.Bool = false;
                cnt_Program_CCD02_02_PIN正位度量測_設定規範位置 = 65535;
                #region 正位度規範值
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN01);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN02);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN03);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN04);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN05);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN06);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN07);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN08);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN09);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN10);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_上排PIN11);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN11);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN12);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN13);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN14);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN15);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN16);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN17);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN18);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN19);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_PIN20);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_X_下排PIN11);

                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN01);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN02);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN03);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN04);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN05);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN06);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN07);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN08);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN09);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN10);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_上排PIN11);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN11);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN12);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN13);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN14);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN15);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN16);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN17);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN18);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN19);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_PIN20);
                this.List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD02_02_PIN正位度量測_正位度規範值_Y_下排PIN11);
                #endregion
            }
            if (cnt_Program_CCD02_02_PIN正位度量測_設定規範位置 == 65535) cnt_Program_CCD02_02_PIN正位度量測_設定規範位置 = 1;
            if (cnt_Program_CCD02_02_PIN正位度量測_設定規範位置 == 1) cnt_Program_CCD02_02_PIN正位度量測_設定規範位置_觸發按下(ref cnt_Program_CCD02_02_PIN正位度量測_設定規範位置);
            if (cnt_Program_CCD02_02_PIN正位度量測_設定規範位置 == 2) cnt_Program_CCD02_02_PIN正位度量測_設定規範位置_檢查按下(ref cnt_Program_CCD02_02_PIN正位度量測_設定規範位置);
            if (cnt_Program_CCD02_02_PIN正位度量測_設定規範位置 == 3) cnt_Program_CCD02_02_PIN正位度量測_設定規範位置_初始化(ref cnt_Program_CCD02_02_PIN正位度量測_設定規範位置);
            if (cnt_Program_CCD02_02_PIN正位度量測_設定規範位置 == 4) cnt_Program_CCD02_02_PIN正位度量測_設定規範位置_座標轉換(ref cnt_Program_CCD02_02_PIN正位度量測_設定規範位置);
            if (cnt_Program_CCD02_02_PIN正位度量測_設定規範位置 == 5) cnt_Program_CCD02_02_PIN正位度量測_設定規範位置_讀取參數(ref cnt_Program_CCD02_02_PIN正位度量測_設定規範位置);
            if (cnt_Program_CCD02_02_PIN正位度量測_設定規範位置 == 6) cnt_Program_CCD02_02_PIN正位度量測_設定規範位置_繪製畫布(ref cnt_Program_CCD02_02_PIN正位度量測_設定規範位置);
            if (cnt_Program_CCD02_02_PIN正位度量測_設定規範位置 == 7) cnt_Program_CCD02_02_PIN正位度量測_設定規範位置 = 65500;
            if (cnt_Program_CCD02_02_PIN正位度量測_設定規範位置 > 1) cnt_Program_CCD02_02_PIN正位度量測_設定規範位置_檢查放開(ref cnt_Program_CCD02_02_PIN正位度量測_設定規範位置);

            if (cnt_Program_CCD02_02_PIN正位度量測_設定規範位置 == 65500)
            {
                if (PLC_Device_CCD02_02_計算一次.Bool)
                {
                    PLC_Device_CCD02_02_PIN正位度量測_設定規範按鈕.Bool = false;
                    PLC_Device_CCD02_02_PIN正位度量測_設定規範位置.Bool = false;
                }
                cnt_Program_CCD02_02_PIN正位度量測_設定規範位置 = 65535;
            }
        }
        void cnt_Program_CCD02_02_PIN正位度量測_設定規範位置_觸發按下(ref int cnt)
        {
            if (PLC_Device_CCD02_02_PIN正位度量測_設定規範按鈕.Bool)
            {
                PLC_Device_CCD02_02_PIN正位度量測_設定規範位置.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_02_PIN正位度量測_設定規範位置_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD02_02_PIN正位度量測_設定規範位置.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD02_02_PIN正位度量測_設定規範位置_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD02_02_PIN正位度量測_設定規範按鈕.Bool)
            {
                PLC_Device_CCD02_02_PIN正位度量測_設定規範位置.Bool = false;
                cnt = 65500;
            }
        }
        void cnt_Program_CCD02_02_PIN正位度量測_設定規範位置_初始化(ref int cnt)
        {
            this.List_CCD02_02_PIN正位度量測參數_正位度規範點_X = new double[22];
            this.List_CCD02_02_PIN正位度量測參數_正位度規範點_Y = new double[22];
            this.List_CCD02_02_PIN正位度量測參數_規範點 = new PointF[22];
            this.List_CCD02_02_PIN正位度量測參數_轉換後座標 = new PointF[22];
            cnt++;
        }
        void cnt_Program_CCD02_02_PIN正位度量測_設定規範位置_座標轉換(ref int cnt)
        {
            if (PLC_Device_CCD02_02_計算一次.Bool)
            {
                CCD02_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.RefPointX = PLC_Device_CCD02_01_水平基準線量測_量測中心_X.Value;
                CCD02_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.RefPointY = PLC_Device_CCD02_01_水平基準線量測_量測中心_Y.Value;
                CCD02_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.RefAngle = 0;
                CCD02_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.CurrentRefPointX = Point_CCD02_01_中心基準座標_量測點.X;
                CCD02_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.CurrentRefPointY = Point_CCD02_01_中心基準座標_量測點.Y;
                CCD02_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.CurrentRefAngle = 0;
                CCD02_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.NumOfVisionPoints = 22;

                for (int j = 0; j < 22; j++)
                {

                    CCD02_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
                    CCD02_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointX = (float)(List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X[j].Value);
                    CCD02_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointX = CCD02_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointX / 1;
                    CCD02_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointY = (float)(List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y[j].Value);
                    CCD02_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointY = CCD02_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointY / 1;

                }
                CCD02_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.EstimateCurrentVisionPoints();
                for (int j = 0; j < 22; j++)
                {

                    CCD02_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
                    List_CCD02_02_PIN正位度量測參數_轉換後座標[j].X = (int)CCD02_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.CurrentVisionPointX;
                    List_CCD02_02_PIN正位度量測參數_轉換後座標[j].Y = (int)CCD02_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.CurrentVisionPointY;

                }
            }
            cnt++;
        }
        void cnt_Program_CCD02_02_PIN正位度量測_設定規範位置_讀取參數(ref int cnt)
        {

            for (int i = 0; i < 22; i++)
            {
                if (PLC_Device_CCD02_02_計算一次.Bool)
                {
                    this.List_CCD02_02_PIN正位度量測參數_正位度規範點_X[i] = List_CCD02_02_PIN正位度量測參數_轉換後座標[i].X;
                    this.List_CCD02_02_PIN正位度量測參數_正位度規範點_Y[i] = List_CCD02_02_PIN正位度量測參數_轉換後座標[i].Y;
                }
                else
                {
                    this.List_CCD02_02_PIN正位度量測參數_正位度規範點_X[i] = (float)(List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X[i].Value);
                    this.List_CCD02_02_PIN正位度量測參數_正位度規範點_X[i] = this.List_CCD02_02_PIN正位度量測參數_正位度規範點_X[i] / 1;
                    this.List_CCD02_02_PIN正位度量測參數_正位度規範點_Y[i] = (float)(List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y[i].Value);
                    this.List_CCD02_02_PIN正位度量測參數_正位度規範點_Y[i] = this.List_CCD02_02_PIN正位度量測參數_正位度規範點_Y[i] / 1;
                }
                List_CCD02_02_PIN正位度量測參數_規範點[i].X = (float)this.List_CCD02_02_PIN正位度量測參數_正位度規範點_X[i];
                List_CCD02_02_PIN正位度量測參數_規範點[i].Y = (float)this.List_CCD02_02_PIN正位度量測參數_正位度規範點_Y[i];

            }
            cnt++;
        }
        void cnt_Program_CCD02_02_PIN正位度量測_設定規範位置_繪製畫布(ref int cnt)
        {
            this.PLC_Device_CCD02_02_PIN設定規範位置_RefreshCanvas.Bool = true;
            if (this.PLC_Device_CCD02_02_PIN正位度量測_設定規範按鈕.Bool && !PLC_Device_CCD02_02_計算一次.Bool)
            {
                this.h_Canvas_Tech_CCD02_02.RefreshCanvas();
            }

            cnt++;
        }


        #endregion
        #region PLC_CCD02_02_PIN量測_檢測正位度計算

        MyTimer MyTimer_CCD02_02_PIN量測_檢測正位度計算 = new MyTimer();
        PLC_Device PLC_Device_CCD02_02_PIN量測_檢測正位度計算按鈕 = new PLC_Device("S6330");
        PLC_Device PLC_Device_CCD02_02_PIN量測_檢測正位度計算 = new PLC_Device("S6325");
        PLC_Device PLC_Device_CCD02_02_PIN量測_檢測正位度計算_OK = new PLC_Device("S6326");
        PLC_Device PLC_Device_CCD02_02_PIN量測_檢測正位度計算_測試完成 = new PLC_Device("S6327");
        PLC_Device PLC_Device_CCD02_02_PIN量測_檢測正位度計算_RefreshCanvas = new PLC_Device("S6328");
        PLC_Device PLC_Device_CCD02_02_PIN量測_檢測正位度計算_不測試 = new PLC_Device("S6112");
        PLC_Device PLC_Device_CCD02_02_PIN量測_檢測正位度計算_差值 = new PLC_Device("F15003");

        private double[] List_CCD02_02_PIN正位度量測參數_正位度距離 = new double[22];
        private bool[] List_CCD02_02_PIN正位度量測參數_正位度量測點_OK = new bool[22];


        private void H_Canvas_Tech_CCD02_02_PIN量測正位度_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {
            if (PLC_Device_CCD02_02_Main_取像並檢驗.Bool || PLC_Device_CCD02_02_PLC觸發檢測.Bool || PLC_Device_CCD02_02_Main_檢驗一次按鈕.Bool)
            {
                try
                {
                    Graphics g = Graphics.FromHdc((IntPtr)HDC);
                    Font font = new Font("微軟正黑體", 10);
                    string 正位度差值顯示;
                    for (int i = 0; i < 22; i++)
                    {
                        DrawingClass.Draw.十字中心(new PointF((float)List_CCD02_02_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD02_02_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.十字中心(this.List_CCD02_02_PIN量測參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);

                    }
                    #region 正位度量測結果顯示
                    if (PLC_Device_CCD02_02_PIN量測_檢測正位度計算_OK.Bool)
                    {
                        DrawingClass.Draw.文字左上繪製("正位OK:", new PointF(1200, 0), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                    }
                    else
                    {
                        DrawingClass.Draw.文字左上繪製("正位NG:", new PointF(1200, 0), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                    }
                    #endregion
                    #region PIN正位結果顯示
                    for (int i = 0; i < 22; i++)
                    {

                        if (this.List_CCD02_02_PIN正位度量測參數_正位度量測點_OK[i])
                        {
                            if (i <= 10)
                            {
                                正位度差值顯示 = ("上排:P" + (i + 1).ToString("00:") + this.List_CCD02_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1600, i * 55), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            }

                            if (i >= 11)
                            {
                                正位度差值顯示 = ("下排:P" + ((i - 11) + 1).ToString("00:") + this.List_CCD02_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(2100, (i - 11) * 55), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                            }
                        }
                        else
                        {
                            if (i <= 10)
                            {
                                正位度差值顯示 = ("上排:P" + (i + 1).ToString("00:") + this.List_CCD02_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1600, i * 55), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            }

                            if (i >= 11)
                            {
                                正位度差值顯示 = ("下排:P" + ((i - 11) + 1).ToString("00:") + this.List_CCD02_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(2100, (i - 11) * 55), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                            }

                        }
                    }



                    #endregion

                    g.Dispose();
                    g = null;
                }
                catch
                {

                }

            }
            if (PLC_Device_CCD02_02_Tech_檢驗一次.Bool || PLC_Device_CCD02_02_Tech_取像並檢驗.Bool)
            {
                if (this.PLC_Device_CCD02_02_PIN量測_檢測正位度計算_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);
                        string 正位度差值顯示;
                        for (int i = 0; i < 22; i++)
                        {
                            DrawingClass.Draw.十字中心(new PointF((float)List_CCD02_02_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD02_02_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
                            DrawingClass.Draw.十字中心(this.List_CCD02_02_PIN量測參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);

                        }
                        #region 正位度量測結果顯示
                        if (PLC_Device_CCD02_02_PIN量測_檢測正位度計算_OK.Bool)
                        {
                            DrawingClass.Draw.文字左上繪製("正位度數值OK:", new PointF(1500, 0), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        }
                        else
                        {
                            DrawingClass.Draw.文字左上繪製("正位度數值NG:", new PointF(1500, 0), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        }
                        #endregion
                        #region PIN正位結果顯示
                        for (int i = 0; i < 22; i++)
                        {

                            if (this.List_CCD02_02_PIN正位度量測參數_正位度量測點_OK[i])
                            {
                                if (i <= 10)
                                {
                                    正位度差值顯示 = ("上排:P" + (i + 1).ToString("00:") + this.List_CCD02_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1900, i * 40), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                                }

                                if (i >= 11)
                                {
                                    正位度差值顯示 = ("下排:P" + ((i - 11) + 1).ToString("00:") + this.List_CCD02_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(2200, (i - 11) * 40), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                                }
                            }
                            else
                            {
                                if (i <= 10)
                                {
                                    正位度差值顯示 = ("上排:P" + (i + 1).ToString("00:") + this.List_CCD02_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1900, i * 40), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                                }

                                if (i >= 11)
                                {
                                    正位度差值顯示 = ("下排:P" + ((i - 11) + 1).ToString("00:") + this.List_CCD02_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(2200, (i - 11) * 40), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                                }

                            }


                        }

                        #endregion

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
                if (this.PLC_Device_CCD02_02_PIN量測_檢測正位度計算_RefreshCanvas.Bool && PLC_Device_CCD02_02_PIN量測_檢測正位度計算.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);
                        string 正位度差值顯示;
                        for (int i = 0; i < 22; i++)
                        {
                            DrawingClass.Draw.十字中心(new PointF((float)List_CCD02_02_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD02_02_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
                            DrawingClass.Draw.十字中心(this.List_CCD02_02_PIN量測參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
                        }
                        #region 正位度量測結果顯示
                        if (PLC_Device_CCD02_02_PIN量測_檢測正位度計算_OK.Bool)
                        {
                            DrawingClass.Draw.文字左上繪製("正位度數值OK:", new PointF(1500, 0), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        }
                        else
                        {
                            DrawingClass.Draw.文字左上繪製("正位度數值NG:", new PointF(1500, 0), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        }
                        #endregion
                        #region PIN正位結果顯示
                        for (int i = 0; i < 22; i++)
                        {

                            if (this.List_CCD02_02_PIN正位度量測參數_正位度量測點_OK[i])
                            {
                                if (i <= 10)
                                {
                                    正位度差值顯示 = ("上排:P" + (i + 1).ToString("00:") + this.List_CCD02_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1900, i * 40), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                                }

                                if (i >= 11)
                                {
                                    正位度差值顯示 = ("下排:P" + ((i - 11) + 1).ToString("00:") + this.List_CCD02_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(2200, (i - 11) * 40), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                                }
                            }
                            else
                            {
                                if (i <= 10)
                                {
                                    正位度差值顯示 = ("上排:P" + (i + 1).ToString("00:") + this.List_CCD02_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1900, i * 40), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                                }

                                if (i >= 11)
                                {
                                    正位度差值顯示 = ("下排:P" + ((i - 11) + 1).ToString("00:") + this.List_CCD02_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(2200, (i - 11) * 40), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                                }

                            }


                        }

                        #endregion

                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }

            }

            this.PLC_Device_CCD02_02_PIN量測_檢測正位度計算_RefreshCanvas.Bool = false;
        }

        int cnt_Program_CCD02_02_PIN量測_檢測正位度計算 = 65534;
        void sub_Program_CCD02_02_PIN量測_檢測正位度計算()
        {
            if (cnt_Program_CCD02_02_PIN量測_檢測正位度計算 == 65534)
            {
                this.h_Canvas_Tech_CCD02_02.OnCanvasDrawEvent += H_Canvas_Tech_CCD02_02_PIN量測正位度_OnCanvasDrawEvent;
                this.h_Canvas_Main_CCD02_02_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD02_02_PIN量測正位度_OnCanvasDrawEvent;
                PLC_Device_CCD02_02_PIN量測_檢測正位度計算.SetComment("PLC_CCD02_02_PIN量測_檢測正位度計算");
                PLC_Device_CCD02_02_PIN量測_檢測正位度計算.Bool = false;
                PLC_Device_CCD02_02_PIN量測_檢測正位度計算按鈕.Bool = false;
                cnt_Program_CCD02_02_PIN量測_檢測正位度計算 = 65535;

            }
            if (cnt_Program_CCD02_02_PIN量測_檢測正位度計算 == 65535) cnt_Program_CCD02_02_PIN量測_檢測正位度計算 = 1;
            if (cnt_Program_CCD02_02_PIN量測_檢測正位度計算 == 1) cnt_Program_CCD02_02_PIN量測_檢測正位度計算_觸發按下(ref cnt_Program_CCD02_02_PIN量測_檢測正位度計算);
            if (cnt_Program_CCD02_02_PIN量測_檢測正位度計算 == 2) cnt_Program_CCD02_02_PIN量測_檢測正位度計算_檢查按下(ref cnt_Program_CCD02_02_PIN量測_檢測正位度計算);
            if (cnt_Program_CCD02_02_PIN量測_檢測正位度計算 == 3) cnt_Program_CCD02_02_PIN量測_檢測正位度計算_初始化(ref cnt_Program_CCD02_02_PIN量測_檢測正位度計算);
            if (cnt_Program_CCD02_02_PIN量測_檢測正位度計算 == 4) cnt_Program_CCD02_02_PIN量測_檢測正位度計算_數值計算(ref cnt_Program_CCD02_02_PIN量測_檢測正位度計算);
            if (cnt_Program_CCD02_02_PIN量測_檢測正位度計算 == 5) cnt_Program_CCD02_02_PIN量測_檢測正位度計算_量測結果(ref cnt_Program_CCD02_02_PIN量測_檢測正位度計算);
            if (cnt_Program_CCD02_02_PIN量測_檢測正位度計算 == 6) cnt_Program_CCD02_02_PIN量測_檢測正位度計算_繪製畫布(ref cnt_Program_CCD02_02_PIN量測_檢測正位度計算);
            if (cnt_Program_CCD02_02_PIN量測_檢測正位度計算 == 7) cnt_Program_CCD02_02_PIN量測_檢測正位度計算 = 65500;
            if (cnt_Program_CCD02_02_PIN量測_檢測正位度計算 > 1) cnt_Program_CCD02_02_PIN量測_檢測正位度計算_檢查放開(ref cnt_Program_CCD02_02_PIN量測_檢測正位度計算);

            if (cnt_Program_CCD02_02_PIN量測_檢測正位度計算 == 65500)
            {
                PLC_Device_CCD02_02_PIN量測_檢測正位度計算.Bool = false;
                PLC_Device_CCD02_02_PIN量測_檢測正位度計算按鈕.Bool = false;
                cnt_Program_CCD02_02_PIN量測_檢測正位度計算 = 65535;
            }
        }
        void cnt_Program_CCD02_02_PIN量測_檢測正位度計算_觸發按下(ref int cnt)
        {
            if (PLC_Device_CCD02_02_PIN量測_檢測正位度計算按鈕.Bool)
            {
                PLC_Device_CCD02_02_PIN量測_檢測正位度計算.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_02_PIN量測_檢測正位度計算_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD02_02_PIN量測_檢測正位度計算.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD02_02_PIN量測_檢測正位度計算_檢查放開(ref int cnt)
        {
            //if (!PLC_Device_CCD02_02_PIN量測_檢測正位度計算按鈕.Bool)
            //{
            //    cnt = 65500;
            //}
        }
        void cnt_Program_CCD02_02_PIN量測_檢測正位度計算_初始化(ref int cnt)
        {
            this.MyTimer_CCD02_02_PIN量測_檢測正位度計算.TickStop();
            this.MyTimer_CCD02_02_PIN量測_檢測正位度計算.StartTickTime(99999);
            this.List_CCD02_02_PIN正位度量測參數_正位度距離 = new double[22];
            this.List_CCD02_02_PIN正位度量測參數_正位度量測點_OK = new bool[22];
            cnt++;
        }
        void cnt_Program_CCD02_02_PIN量測_檢測正位度計算_數值計算(ref int cnt)
        {
            double distance = 0;
            double temp_X = 0;
            double temp_Y = 0;
            PLC_Device_CCD02_02_PIN量測_檢測正位度計算_OK.Bool = true;

            for (int i = 0; i < 22; i++)
            {
                if (this.List_CCD02_02_PIN量測參數_量測點_有無[i])
                {
                    temp_X = Math.Pow(this.List_CCD02_02_PIN量測參數_量測點[i].X - this.List_CCD02_02_PIN正位度量測參數_規範點[i].X, 2);
                    temp_Y = Math.Pow(this.List_CCD02_02_PIN量測參數_量測點[i].Y - this.List_CCD02_02_PIN正位度量測參數_規範點[i].Y, 2);

                    distance = Math.Sqrt(temp_X + temp_Y);
                    this.List_CCD02_02_PIN正位度量測參數_正位度距離[i] = distance * this.CCD02_比例尺_pixcel_To_mm;
                }
                else
                {
                    PLC_Device_CCD02_02_PIN量測_檢測正位度計算_OK.Bool = false;
                    List_CCD02_02_PIN正位度量測參數_正位度量測點_OK[i] = false;
                }

            }
            cnt++;
        }
        void cnt_Program_CCD02_02_PIN量測_檢測正位度計算_量測結果(ref int cnt)
        {

            PLC_Device_CCD02_02_PIN量測_檢測正位度計算_OK.Bool = true; // 檢測結果初始化

            for (int i = 0; i < 22; i++)
            {
                int 標準值差值 = this.PLC_Device_CCD02_02_PIN量測_檢測正位度計算_差值.Value;
                double 量測距離 = this.List_CCD02_02_PIN正位度量測參數_正位度距離[i];

                量測距離 = 量測距離 * 1000;
                量測距離 /= 1000;

                if (!PLC_Device_CCD02_02_PIN量測_檢測正位度計算_不測試.Bool)
                {
                    if (this.List_CCD02_02_PIN量測參數_量測點_有無[i])
                    {


                        if (量測距離 >= 0)
                        {
                            if (標準值差值 <= Math.Abs(量測距離) * 1000)
                            {
                                this.List_CCD02_02_PIN正位度量測參數_正位度量測點_OK[i] = false;
                                PLC_Device_CCD02_02_PIN量測_檢測正位度計算_OK.Bool = false;
                            }
                            else
                            {
                                this.List_CCD02_02_PIN正位度量測參數_正位度量測點_OK[i] = true;
                            }
                        }

                    }
                    else
                    {
                        this.List_CCD02_02_PIN正位度量測參數_正位度量測點_OK[i] = false;
                        PLC_Device_CCD02_02_PIN量測_檢測正位度計算_OK.Bool = false;
                    }

                }
                else
                {
                    this.List_CCD02_02_PIN正位度量測參數_正位度量測點_OK[i] = true;
                }

                this.List_CCD02_02_PIN正位度量測參數_正位度距離[i] = 量測距離;
            }
            cnt++;
        }
        void cnt_Program_CCD02_02_PIN量測_檢測正位度計算_繪製畫布(ref int cnt)
        {
            this.PLC_Device_CCD02_02_PIN量測_檢測正位度計算_RefreshCanvas.Bool = true;
            if (this.PLC_Device_CCD02_02_PIN量測_檢測正位度計算按鈕.Bool && !PLC_Device_CCD02_02_計算一次.Bool)
            {

                this.h_Canvas_Tech_CCD02_02.RefreshCanvas();
            }

            cnt++;
        }
        #endregion
        #region PLC_CCD02_02PIN相似度量測

        private AxOvkPat.AxMatch AxMatch_CCD02_02_PIN相似度測試 = new AxOvkPat.AxMatch();
        private AxOvkImage.AxImageCopier AxImageCopier_CCD02_02_PIN相似度測試_GetPattern = new AxOvkImage.AxImageCopier();
        private AxOvkBase.AxImageBW8 CCD02_02_PIN相似度測試_GetPattern_AxImageBW8 = new AxOvkBase.AxImageBW8();

        private PLC_Device PLC_Device_CCD02_02PIN相似度量測按鈕 = new PLC_Device("S6620");
        private PLC_Device PLC_Device_CCD02_02PIN相似度量測 = new PLC_Device("S6621");
        private PLC_Device PLC_Device_CCD02_02PIN相似度量測_OK = new PLC_Device("S6622");
        private PLC_Device PLC_Device_CCD02_02PIN相似度量測_測試完成 = new PLC_Device("S6623");
        private PLC_Device PLC_Device_CCD02_02PIN相似度量測_RefreshCanvas = new PLC_Device("S6624");

        private PLC_Device PLC_Device_CCD02_02PIN相似度量測_樣板取樣細緻度_MinReducedArea = new PLC_Device("F17030");
        private PLC_Device PLC_Device_CCD02_02PIN相似度量測_樣板相似度門檻_MinScore = new PLC_Device("F17031");
        private PLC_Device PLC_Device_CCD02_02_PIN相似度量測_樣本圖片辨識代碼 = new PLC_Device("F17032");
        string CCD02_02_樣板儲存名稱 = "CCD2-2_PAT";
        float[] Match_CCD0202_Score;
        bool[] CCD02_02_PIN相似度_OK;

        private void H_Canvas_Tech_CCD02_02PIN相似度量測_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {
            PointF[] 相似度數值顯示 = new PointF[22];


            if (PLC_Device_CCD02_02_Main_取像並檢驗.Bool || PLC_Device_CCD02_02_PLC觸發檢測.Bool || PLC_Device_CCD02_02_Main_檢驗一次按鈕.Bool)
            {
                if (this.PLC_Device_CCD02_02PIN相似度量測_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        this.AxMatch_CCD02_02_PIN相似度測試.DrawMatchedPattern(HDC, -1, ZoomX, ZoomY, 0, 0);
                        if (PLC_Device_CCD02_02PIN相似度量測_OK.Bool) DrawingClass.Draw.文字左上繪製("PIN相似度OK!", new PointF(0, 300), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        else DrawingClass.Draw.文字左上繪製("PIN相似度NG!", new PointF(0, 300), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);

                        for(int i = 0; i < 22; i++)
                        {
                            相似度數值顯示[i].X =  List_CCD02_02_PIN量測參數_量測點[i].X;
                            相似度數值顯示[i].Y = List_CCD02_02_PIN量測參數_量測點[i].Y - 100;

                            if (CCD02_02_PIN相似度_OK[i]) 
                                DrawingClass.Draw.文字左上繪製((this.Match_CCD0202_Score[i] * 100).ToString("0.0") + "%", 相似度數值顯示[i], new Font("標楷體", 8), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            else DrawingClass.Draw.文字左上繪製((this.Match_CCD0202_Score[i] * 100).ToString("0.0") + "%", 相似度數值顯示[i], new Font("標楷體", 8), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        }

                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }


            }
            else if (PLC_Device_CCD02_02_Tech_檢驗一次.Bool)
            {
                if (this.PLC_Device_CCD02_02PIN相似度量測_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        this.AxMatch_CCD02_02_PIN相似度測試.DrawMatchedPattern(HDC, -1, ZoomX, ZoomY, 0, 0);
                        if (PLC_Device_CCD02_02PIN相似度量測_OK.Bool) DrawingClass.Draw.文字左上繪製("PIN相似度OK!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        else DrawingClass.Draw.文字左上繪製("PIN相似度NG!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        for (int i = 0; i < 22; i++)
                        {
                            相似度數值顯示[i].X = List_CCD02_02_PIN量測參數_量測點[i].X;
                            相似度數值顯示[i].Y = List_CCD02_02_PIN量測參數_量測點[i].Y - 100;
                            if (CCD02_02_PIN相似度_OK[i])
                                DrawingClass.Draw.文字左上繪製((this.Match_CCD0202_Score[i] * 100).ToString("0.0") + "%", 相似度數值顯示[i], new Font("標楷體", 8), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            else DrawingClass.Draw.文字左上繪製((this.Match_CCD0202_Score[i] * 100).ToString("0.0") + "%", 相似度數值顯示[i], new Font("標楷體", 8), Color.Black, Color.Red, g, ZoomX, ZoomY);
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
                if (this.PLC_Device_CCD02_02PIN相似度量測_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        this.AxMatch_CCD02_02_PIN相似度測試.DrawMatchedPattern(HDC, -1, ZoomX, ZoomY, 0, 0);
                        if (PLC_Device_CCD02_02PIN相似度量測_OK.Bool) DrawingClass.Draw.文字左上繪製("PIN相似度OK!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        else DrawingClass.Draw.文字左上繪製("PIN相似度NG!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        for (int i = 0; i < 22; i++)
                        {
                            相似度數值顯示[i].X = List_CCD02_02_PIN量測參數_量測點[i].X;
                            相似度數值顯示[i].Y = List_CCD02_02_PIN量測參數_量測點[i].Y - 100;
                            if (CCD02_02_PIN相似度_OK[i])
                                DrawingClass.Draw.文字左上繪製((this.Match_CCD0202_Score[i] * 100).ToString("0.0") + "%", 相似度數值顯示[i], new Font("標楷體", 8), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            else DrawingClass.Draw.文字左上繪製((this.Match_CCD0202_Score[i] * 100).ToString("0.0") + "%", 相似度數值顯示[i], new Font("標楷體", 8), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        }

                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }
            }

            this.PLC_Device_CCD02_02PIN相似度量測_RefreshCanvas.Bool = false;
        }
        int cnt_Program_CCD02_02PIN相似度量測 = 65534;
        void sub_Program_CCD02_02PIN相似度量測()
        {
            if (cnt_Program_CCD02_02PIN相似度量測 == 65534)
            {
                this.h_Canvas_Tech_CCD02_02.OnCanvasDrawEvent += H_Canvas_Tech_CCD02_02PIN相似度量測_OnCanvasDrawEvent;
                this.h_Canvas_Main_CCD02_02_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD02_02PIN相似度量測_OnCanvasDrawEvent;

                PLC_Device_CCD02_02PIN相似度量測.SetComment("PLC_CCD02_02PIN相似度量測");
                PLC_Device_CCD02_02PIN相似度量測.Bool = false;
                PLC_Device_CCD02_02PIN相似度量測按鈕.Bool = false;
                cnt_Program_CCD02_02PIN相似度量測 = 65535;
            }
            if (cnt_Program_CCD02_02PIN相似度量測 == 65535) cnt_Program_CCD02_02PIN相似度量測 = 1;
            if (cnt_Program_CCD02_02PIN相似度量測 == 1) cnt_Program_CCD02_02PIN相似度量測_檢查按下(ref cnt_Program_CCD02_02PIN相似度量測);
            if (cnt_Program_CCD02_02PIN相似度量測 == 2) cnt_Program_CCD02_02PIN相似度量測_比對範圍設定開始(ref cnt_Program_CCD02_02PIN相似度量測);
            if (cnt_Program_CCD02_02PIN相似度量測 == 3) cnt_Program_CCD02_02PIN相似度量測_比對範圍設定結束(ref cnt_Program_CCD02_02PIN相似度量測);
            if (cnt_Program_CCD02_02PIN相似度量測 == 4) cnt_Program_CCD02_02PIN相似度量測_初始化(ref cnt_Program_CCD02_02PIN相似度量測);
            if (cnt_Program_CCD02_02PIN相似度量測 == 5) cnt_Program_CCD02_02PIN相似度量測_搜尋樣板(ref cnt_Program_CCD02_02PIN相似度量測);
            if (cnt_Program_CCD02_02PIN相似度量測 == 6) cnt_Program_CCD02_02PIN相似度量測_繪製畫布(ref cnt_Program_CCD02_02PIN相似度量測);
            if (cnt_Program_CCD02_02PIN相似度量測 == 7) cnt_Program_CCD02_02PIN相似度量測 = 65500;
            if (cnt_Program_CCD02_02PIN相似度量測 > 1) cnt_Program_CCD02_02PIN相似度量測_檢查放開(ref cnt_Program_CCD02_02PIN相似度量測);

            if (cnt_Program_CCD02_02PIN相似度量測 == 65500)
            {
                PLC_Device_CCD02_02PIN相似度量測.Bool = false;
                PLC_Device_CCD02_02PIN相似度量測按鈕.Bool = false;
                cnt_Program_CCD02_02PIN相似度量測 = 65535;
            }
        }
        void cnt_Program_CCD02_02PIN相似度量測_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD02_02PIN相似度量測按鈕.Bool)
            {
                PLC_Device_CCD02_02PIN相似度量測.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_02PIN相似度量測_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD02_02PIN相似度量測.Bool) cnt = 65500;
        }
        void cnt_Program_CCD02_02PIN相似度量測_比對範圍設定開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_02比對樣板範圍.Bool)
            {
                this.PLC_Device_CCD02_02比對樣板範圍.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_02PIN相似度量測_比對範圍設定結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_02比對樣板範圍.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD02_02PIN相似度量測_初始化(ref int cnt)
        {
            this.Match_CCD0202_Score = new float[22];
            this.CCD02_02_PIN相似度_OK = new bool[22];

            this.PLC_Device_CCD02_02PIN相似度量測_OK.Bool = true;
            this.AxMatch_CCD02_02_PIN相似度測試.DstImageHandle = AxROIBW8_CCD02_02_比對樣板範圍.VegaHandle;
            this.AxMatch_CCD02_02_PIN相似度測試.PositionType = AxOvkPat.TxAxMatchPositionType.AX_MATCH_POSITION_TYPE_CENTER;
            this.AxMatch_CCD02_02_PIN相似度測試.MaxPositions = 22;
            this.AxMatch_CCD02_02_PIN相似度測試.MinScore = (float)PLC_Device_CCD02_02PIN相似度量測_樣板相似度門檻_MinScore.Value / 100;
            this.AxMatch_CCD02_02_PIN相似度測試.Match();
            cnt++;
        }
        void cnt_Program_CCD02_02PIN相似度量測_搜尋樣板(ref int cnt)
        {
            bool effect = this.AxMatch_CCD02_02_PIN相似度測試.EffectMatch;
            int num = this.AxMatch_CCD02_02_PIN相似度測試.NumMatchedPos;
            for (int i = 0; i < 22; i++)
            {
                this.AxMatch_CCD02_02_PIN相似度測試.PatternIndex = i;
                this.Match_CCD0202_Score[i] = this.AxMatch_CCD02_02_PIN相似度測試.MatchedScore;

            }

            cnt++;

        }
        void cnt_Program_CCD02_02PIN相似度量測_繪製畫布(ref int cnt)
        {
            if (CCD02_02_SrcImageHandle != 0)
            {
                for (int i = 0; i < 22; i++)
                {
                    AxMatch_CCD02_02_PIN相似度測試.PatternIndex = i;
                    if (AxMatch_CCD02_02_PIN相似度測試.EffectMatch)
                    {
                        CCD02_02_PIN相似度_OK[i] = true;
                    }
                    else
                    {
                        PLC_Device_CCD02_02PIN相似度量測_OK.Bool = false;
                        CCD02_02_PIN相似度_OK[i] = false;
                    }
                }
                this.PLC_Device_CCD02_02PIN相似度量測_RefreshCanvas.Bool = true;
                if (this.PLC_Device_CCD02_02PIN相似度量測按鈕.Bool && !PLC_Device_CCD02_02_計算一次.Bool)
                {
                    this.h_Canvas_Tech_CCD02_02.RefreshCanvas();
                }
            }

            cnt++;
        }

        #endregion
        #region PLC_CCD02_02比對樣板範圍
        private AxOvkBase.AxROIBW8 AxROIBW8_CCD02_02_比對樣板範圍 = new AxOvkBase.AxROIBW8();
        private PLC_Device PLC_Device_CCD02_02比對樣板範圍按鈕 = new PLC_Device("S6630");
        private PLC_Device PLC_Device_CCD02_02比對樣板範圍 = new PLC_Device("S6631");
        private PLC_Device PLC_Device_CCD02_02比對樣板範圍_OK = new PLC_Device("S6632");
        private PLC_Device PLC_Device_CCD02_02比對樣板範圍_測試完成 = new PLC_Device("S6633");
        private PLC_Device PLC_Device_CCD02_02比對樣板範圍_RefreshCanvas = new PLC_Device("S6634");

        private PLC_Device PLC_Device_CCD02_02比對樣板範圍_CenterX = new PLC_Device("F17035");
        private PLC_Device PLC_Device_CCD02_02比對樣板範圍_CenterY = new PLC_Device("F17036");
        private PLC_Device PLC_Device_CCD02_02比對樣板範圍_Width = new PLC_Device("F17037");
        private PLC_Device PLC_Device_CCD02_02比對樣板範圍_Height = new PLC_Device("F17038");

        private void H_Canvas_Tech_CCD02_02比對樣板範圍_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {

            if (PLC_Device_CCD02_02_Main_取像並檢驗.Bool || PLC_Device_CCD02_02_PLC觸發檢測.Bool || PLC_Device_CCD02_02_Main_檢驗一次按鈕.Bool)
            {
                if (this.PLC_Device_CCD02_02比對樣板範圍_RefreshCanvas.Bool)
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
            else if (PLC_Device_CCD02_02_Tech_檢驗一次.Bool)
            {
                if (this.PLC_Device_CCD02_02比對樣板範圍_RefreshCanvas.Bool)
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
                if (this.PLC_Device_CCD02_02比對樣板範圍_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);
                        this.AxROIBW8_CCD02_02_比對樣板範圍.ShowTitle = true;
                        this.AxROIBW8_CCD02_02_比對樣板範圍.Title = "樣板量測範圍";
                        this.AxROIBW8_CCD02_02_比對樣板範圍.DrawFrame(HDC, ZoomX, ZoomY, 0, 0, 0X0000FF);
                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }
            }

            this.PLC_Device_CCD02_02比對樣板範圍_RefreshCanvas.Bool = false;
        }

        AxOvkBase.TxAxHitHandle CCD02_02樣板比對範圍AxROIBW8_TxAxRoiHitHandle = new AxOvkBase.TxAxHitHandle();
        bool flag_CCD02_02_樣板比對範圍AxROIBW8_MouseDown = new bool();
        private void H_Canvas_Tech_CCD02_02比對樣板範圍_OnCanvasMouseDownEvent(int x, int y, float ZoomX, float ZoomY, ref int InUsedEventNum, int InUsedCanvasHandle)
        {
            if (this.PLC_Device_CCD02_02比對樣板範圍.Bool)
            {
                this.CCD02_02樣板比對範圍AxROIBW8_TxAxRoiHitHandle = this.AxROIBW8_CCD02_02_比對樣板範圍.HitTest(x, y, ZoomX, ZoomY, 0, 0);
                if (this.CCD02_02樣板比對範圍AxROIBW8_TxAxRoiHitHandle != AxOvkBase.TxAxHitHandle.AX_HANDLE_NONE)
                {
                    this.flag_CCD02_02_樣板比對範圍AxROIBW8_MouseDown = true;
                    InUsedEventNum = 10;
                }
            }

        }
        private void H_Canvas_Tech_CCD02_02比對樣板範圍_OnCanvasMouseMoveEvent(int x, int y, float ZoomX, float ZoomY)
        {
            if (this.flag_CCD02_02_樣板比對範圍AxROIBW8_MouseDown)
            {
                this.AxROIBW8_CCD02_02_比對樣板範圍.DragROI(CCD02_02樣板比對範圍AxROIBW8_TxAxRoiHitHandle, x, y, ZoomX, ZoomY, 0, 0);
                this.PLC_Device_CCD02_02比對樣板範圍_CenterX.Value = this.AxROIBW8_CCD02_02_比對樣板範圍.OrgX;
                this.PLC_Device_CCD02_02比對樣板範圍_CenterY.Value = this.AxROIBW8_CCD02_02_比對樣板範圍.OrgY;
                this.PLC_Device_CCD02_02比對樣板範圍_Width.Value = this.AxROIBW8_CCD02_02_比對樣板範圍.ROIWidth;
                this.PLC_Device_CCD02_02比對樣板範圍_Height.Value = this.AxROIBW8_CCD02_02_比對樣板範圍.ROIHeight;

            }

        }
        private void H_Canvas_Tech_CCD02_02比對樣板範圍_OnCanvasMouseUpEvent(int x, int y, float ZoomX, float ZoomY)
        {

            this.flag_CCD02_02_樣板比對範圍AxROIBW8_MouseDown = false;

        }
        int cnt_Program_CCD02_02比對樣板範圍 = 65534;
        void sub_Program_CCD02_02比對樣板範圍()
        {
            if (cnt_Program_CCD02_02比對樣板範圍 == 65534)
            {
                this.h_Canvas_Tech_CCD02_02.OnCanvasDrawEvent += H_Canvas_Tech_CCD02_02比對樣板範圍_OnCanvasDrawEvent;
                this.h_Canvas_Tech_CCD02_02.OnCanvasMouseDownEvent += H_Canvas_Tech_CCD02_02比對樣板範圍_OnCanvasMouseDownEvent;
                this.h_Canvas_Tech_CCD02_02.OnCanvasMouseMoveEvent += H_Canvas_Tech_CCD02_02比對樣板範圍_OnCanvasMouseMoveEvent;
                this.h_Canvas_Tech_CCD02_02.OnCanvasMouseUpEvent += H_Canvas_Tech_CCD02_02比對樣板範圍_OnCanvasMouseUpEvent;

                this.h_Canvas_Main_CCD02_02_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD02_02比對樣板範圍_OnCanvasDrawEvent;


                if (this.PLC_Device_CCD02_02比對樣板範圍_Height.Value == 0) this.PLC_Device_CCD02_02比對樣板範圍_Height.Value = 150;
                if (this.PLC_Device_CCD02_02比對樣板範圍_Width.Value == 0) this.PLC_Device_CCD02_02比對樣板範圍_Width.Value = 150;



                PLC_Device_CCD02_02比對樣板範圍.SetComment("PLC_CCD02_02比對樣板範圍");
                PLC_Device_CCD02_02比對樣板範圍.Bool = false;
                PLC_Device_CCD02_02比對樣板範圍按鈕.Bool = false;
                cnt_Program_CCD02_02比對樣板範圍 = 65535;
            }
            if (cnt_Program_CCD02_02比對樣板範圍 == 65535) cnt_Program_CCD02_02比對樣板範圍 = 1;
            if (cnt_Program_CCD02_02比對樣板範圍 == 1) cnt_Program_CCD02_02比對樣板範圍_檢查按下(ref cnt_Program_CCD02_02比對樣板範圍);
            if (cnt_Program_CCD02_02比對樣板範圍 == 2) cnt_Program_CCD02_02比對樣板範圍_初始化(ref cnt_Program_CCD02_02比對樣板範圍);
            if (cnt_Program_CCD02_02比對樣板範圍 == 3) cnt_Program_CCD02_02比對樣板範圍_生成量測框(ref cnt_Program_CCD02_02比對樣板範圍);
            if (cnt_Program_CCD02_02比對樣板範圍 == 4) cnt_Program_CCD02_02比對樣板範圍_繪製畫布(ref cnt_Program_CCD02_02比對樣板範圍);
            if (cnt_Program_CCD02_02比對樣板範圍 == 5) cnt_Program_CCD02_02比對樣板範圍 = 65500;
            if (cnt_Program_CCD02_02比對樣板範圍 > 1) cnt_Program_CCD02_02比對樣板範圍_檢查放開(ref cnt_Program_CCD02_02比對樣板範圍);

            if (cnt_Program_CCD02_02比對樣板範圍 == 65500)
            {
                PLC_Device_CCD02_02比對樣板範圍.Bool = false;
                cnt_Program_CCD02_02比對樣板範圍 = 65535;
            }
        }
        void cnt_Program_CCD02_02比對樣板範圍_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD02_02比對樣板範圍按鈕.Bool || PLC_Device_CCD02_02PIN相似度量測按鈕.Bool)
            {
                PLC_Device_CCD02_02比對樣板範圍.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_02比對樣板範圍_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD02_02比對樣板範圍.Bool) cnt = 65500;
        }
        void cnt_Program_CCD02_02比對樣板範圍_初始化(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD02_02比對樣板範圍_生成量測框(ref int cnt)
        {

            if (this.PLC_Device_CCD02_02比對樣板範圍_CenterX.Value > 2596) this.PLC_Device_CCD02_02比對樣板範圍_CenterX.Value = 0;
            if (this.PLC_Device_CCD02_02比對樣板範圍_CenterY.Value > 1922) this.PLC_Device_CCD02_02比對樣板範圍_CenterY.Value = 0;
            if (this.PLC_Device_CCD02_02比對樣板範圍_Width.Value < 0) this.PLC_Device_CCD02_02比對樣板範圍_Width.Value = 0;
            if (this.PLC_Device_CCD02_02比對樣板範圍_Height.Value < 0) this.PLC_Device_CCD02_02比對樣板範圍_Height.Value = 0;

            this.AxROIBW8_CCD02_02_比對樣板範圍.ParentHandle = this.CCD02_02_SrcImageHandle;
            this.AxROIBW8_CCD02_02_比對樣板範圍.OrgX = PLC_Device_CCD02_02比對樣板範圍_CenterX.Value;
            this.AxROIBW8_CCD02_02_比對樣板範圍.OrgY = PLC_Device_CCD02_02比對樣板範圍_CenterY.Value;
            this.AxROIBW8_CCD02_02_比對樣板範圍.ROIWidth = PLC_Device_CCD02_02比對樣板範圍_Width.Value;
            this.AxROIBW8_CCD02_02_比對樣板範圍.ROIHeight = PLC_Device_CCD02_02比對樣板範圍_Height.Value;

            cnt++;
        }
        void cnt_Program_CCD02_02比對樣板範圍_繪製畫布(ref int cnt)
        {
            if (CCD02_02_SrcImageHandle != 0)
            {
                this.PLC_Device_CCD02_02比對樣板範圍_RefreshCanvas.Bool = true;
                if (this.PLC_Device_CCD02_02比對樣板範圍按鈕.Bool && !PLC_Device_CCD02_02_計算一次.Bool)
                {
                    this.h_Canvas_Tech_CCD02_02.RefreshCanvas();
                }
            }
            cnt++;
        }
        private void button2_Click(object sender, EventArgs e)
        {

        }
        #region 讀取樣板
        int cnt_Program_CCD02_02_讀取樣板 = 65534;
        void sub_Program_CCD02_02_讀取樣板()
        {
            if (cnt_Program_CCD02_02_讀取樣板 == 65534)
            {
                cnt_Program_CCD02_02_讀取樣板 = 65535;
            }
            if (cnt_Program_CCD02_02_讀取樣板 == 65535) cnt_Program_CCD02_02_讀取樣板 = 1;
            if (cnt_Program_CCD02_02_讀取樣板 == 1) cnt_CCD02_02_讀取樣板_開始讀取(ref cnt_Program_CCD02_02_讀取樣板);
            if (cnt_Program_CCD02_02_讀取樣板 == 2) cnt_Program_CCD02_02_讀取樣板 = 255;
        }
        void cnt_CCD02_02_讀取樣板_開始讀取(ref int cnt)
        {

            AxMatch_CCD02_02_PIN相似度測試.LoadFile(".//" + CCD02_02_樣板儲存名稱);
            if (AxMatch_CCD02_02_PIN相似度測試.IsLearnPattern)
            {
                this.AxImageCopier_CCD02_02_PIN相似度測試_GetPattern.SrcImageHandle = AxMatch_CCD02_02_PIN相似度測試.PatternVegaHandle;
                this.AxImageCopier_CCD02_02_PIN相似度測試_GetPattern.DstImageHandle = this.CCD02_02_PIN相似度測試_GetPattern_AxImageBW8.VegaHandle;
                this.AxImageCopier_CCD02_02_PIN相似度測試_GetPattern.Copy();
                this.CCD02_02_PatternCanvasRefresh(this.CCD02_02_PIN相似度測試_GetPattern_AxImageBW8.VegaHandle, this.CCD02_02_PatternCanvas_ZoomX, this.CCD02_02_PatternCanvas_ZoomY);
            }
            cnt++;
        }
        #endregion
        #endregion

        #region Event
        private void PlC_RJ_Button_CCD02_02_儲存圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (saveImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.h_Canvas_Tech_CCD02_02.SaveImage(saveImageDialog.FileName);
                }
            }));
        }
        private void PlC_RJ_Button_CCD02_02_讀取圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (openImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.CCD02_AxImageBW8.LoadFile(openImageDialog.FileName);
                    try
                    {
                        this.h_Canvas_Tech_CCD02_02.ImageCopy(CCD02_AxImageBW8.VegaHandle);
                        this.CCD02_02_SrcImageHandle = h_Canvas_Tech_CCD02_02.VegaHandle;
                        this.h_Canvas_Tech_CCD02_02.RefreshCanvas();
                    }
                    catch
                    {
                        err_message02_02 = MessageBox.Show("讀取圖片空白", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        if (err_message02_02 == DialogResult.OK)
                        {

                        }
                    }
                }
            }));
        }
        private void PlC_RJ_Button_Main_CCD02_02儲存圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (saveImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.h_Canvas_Main_CCD02_02_檢測畫面.SaveImage(saveImageDialog.FileName);
                }
            }));
        }
        private void PlC_RJ_Button_Main_CCD02_02讀取圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (openImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.CCD02_AxImageBW8.LoadFile(openImageDialog.FileName);
                    try
                    {
                        this.h_Canvas_Main_CCD02_02_檢測畫面.ImageCopy(CCD02_AxImageBW8.VegaHandle);
                        this.CCD02_02_SrcImageHandle = h_Canvas_Main_CCD02_02_檢測畫面.VegaHandle;
                        this.h_Canvas_Main_CCD02_02_檢測畫面.RefreshCanvas();
                    }
                    catch
                    {
                        err_message02_02 = MessageBox.Show("讀取圖片空白", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                        if (err_message02_02 == DialogResult.OK)
                        {

                        }
                    }
                }
            }));
        }
        private void PlC_Button_Main_CCD02_02_ZOOM更新_btnClick(object sender, EventArgs e)
        {
            if (CCD02_02_SrcImageHandle != 0)
            {
                PLC_Device_Main_CCD02_02_ZOOM更新.Bool = true;
                h_Canvas_Main_CCD02_02_檢測畫面.RefreshCanvas();
            }
        }
        private PLC_Device PLC_Device_CCD02_02_PIN量測一鍵排列間距 = new PLC_Device("F4030");
        private void plC_RJ_Button_CCD0202_Tech_PIN量測框一鍵排列_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 22; i++)
            {
                if (i < 11)
                {
                    this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX[i].Value = this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX[0].Value - i * PLC_Device_CCD02_02_PIN量測一鍵排列間距.Value;
                    this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY[i].Value = this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY[0].Value;
                }

                else
                {
                    this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX[i].Value = this.List_PLC_Device_CCD02_02_PIN量測參數_OrgX[11].Value - (i - 11) * PLC_Device_CCD02_02_PIN量測一鍵排列間距.Value;
                    this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY[i].Value = this.List_PLC_Device_CCD02_02_PIN量測參數_OrgY[11].Value;
                }
            }
        }
        private void plC_Button_CCD0202_量測點作為規範點_btnClick(object sender, EventArgs e)
        {
            for (int i = 0; i < 22; i++)
            {
                List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_X[i].Value = (int)this.List_CCD02_02_PIN量測參數_量測點[i].X;
                List_PLC_Device_CCD02_02_PIN正位度量測參數_正位度規範值_Y[i].Value = (int)this.List_CCD02_02_PIN量測參數_量測點[i].Y;
            }
        }
        private void plC_RJ_Button_CCD0202_Tech_PIN量測框大小設為一致_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < 22; i++)
            {
                this.List_PLC_Device_CCD02_02_PIN量測參數_Width[i].Value = this.List_PLC_Device_CCD02_02_PIN量測參數_Width[0].Value;
                this.List_PLC_Device_CCD02_02_PIN量測參數_Height[i].Value = this.List_PLC_Device_CCD02_02_PIN量測參數_Height[0].Value;
            }

        }
        private void plC_Button_CCD02_02_學習樣板按鈕_btnClick(object sender, EventArgs e)
        {
            AxMatch_CCD02_02_PIN相似度測試.MinReducedArea = 144;
            AxMatch_CCD02_02_PIN相似度測試.LearnPattern();
            if (AxMatch_CCD02_02_PIN相似度測試.IsLearnPattern)
            {
                MessageBox.Show("學習樣板成功", "訊息", MessageBoxButtons.OKCancel);
                this.PLC_Device_CCD02_02_PIN相似度量測_樣本圖片辨識代碼.Value = (int)AxMatch_CCD02_02_PIN相似度測試.PatternVegaHandle;
                this.AxImageCopier_CCD02_02_PIN相似度測試_GetPattern.SrcImageHandle = AxMatch_CCD02_02_PIN相似度測試.PatternVegaHandle;
                this.AxImageCopier_CCD02_02_PIN相似度測試_GetPattern.DstImageHandle = this.CCD02_02_PIN相似度測試_GetPattern_AxImageBW8.VegaHandle;
                this.AxImageCopier_CCD02_02_PIN相似度測試_GetPattern.Copy();
                this.CCD02_02_PatternCanvasRefresh(this.CCD02_02_PIN相似度測試_GetPattern_AxImageBW8.VegaHandle, this.CCD02_02_PatternCanvas_ZoomX, this.CCD02_02_PatternCanvas_ZoomY);
                this.AxMatch_CCD02_02_PIN相似度測試.SaveFile(".//" + CCD02_02_樣板儲存名稱);

            }
            else MessageBox.Show("學習樣板失敗!!", "訊息", MessageBoxButtons.OKCancel);

        }
        public delegate void CCD02_02_PatternCanvas_Refresh_EventHandler(long SurfaceHadle, float ZoomX, float ZoomY);
        public event CCD02_02_PatternCanvas_Refresh_EventHandler CCD02_02_PatternCanvas_Refresh_Event;
        void CCD02_02_PatternCanvasRefresh(long SurfaceHadle, float ZoomX, float ZoomY)
        {
            this.CCD02_02_PatternCanvas.ClearCanvas();
            this.CCD02_02_PatternCanvas.DrawSurface(SurfaceHadle, ZoomX, ZoomY, 0, 0);
            //
            if (this.CCD02_02_PatternCanvas_Refresh_Event != null) this.CCD02_02_PatternCanvas_Refresh_Event(SurfaceHadle, ZoomX, ZoomY);
            //
            this.CCD02_02_PatternCanvas.RefreshCanvas();

        }
        #endregion
    }
}

