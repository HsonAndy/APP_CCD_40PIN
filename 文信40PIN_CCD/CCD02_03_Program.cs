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


        DialogResult err_message02_03;

        void Program_CCD02_03()
        {
            this.CCD02_03_儲存圖片();
            this.sub_Program_CCD02_03_SNAP();
            this.sub_Program_CCD02_03_Tech_檢驗一次();
            this.sub_Program_CCD02_03_計算一次();
            this.sub_Program_CCD02_03_Tech_取像並檢驗();
            this.sub_Program_CCD02_03_Main_取像並檢驗();
            this.sub_Program_CCD02_03_基準線量測();
            this.sub_Program_CCD02_03_PIN量測_量測框調整();
            this.sub_Program_CCD02_03_PIN量測_檢測距離計算();
            this.sub_Program_CCD02_03_Main_檢驗一次();
        }

        #region PLC_CCD02_03_SNAP
        PLC_Device PLC_Device_CCD02_03_SNAP_按鈕 = new PLC_Device("M15250");
        PLC_Device PLC_Device_CCD02_03_SNAP = new PLC_Device("M15245");
        PLC_Device PLC_Device_CCD02_03_SNAP_LIVE = new PLC_Device("M15246");
        PLC_Device PLC_Device_CCD02_03_SNAP_電子快門 = new PLC_Device("F9170");
        PLC_Device PLC_Device_CCD02_03_SNAP_視訊增益 = new PLC_Device("F9171");
        PLC_Device PLC_Device_CCD02_03_SNAP_銳利度 = new PLC_Device("F9172");
        PLC_Device PLC_Device_CCD02_03_SNAP_光源亮度_紅正照 = new PLC_Device("F25144");

        int cnt_Program_CCD02_03_SNAP = 65534;
        void sub_Program_CCD02_03_SNAP()
        {
            if (cnt_Program_CCD02_03_SNAP == 65534)
            {
                PLC_Device_CCD02_03_SNAP.SetComment("PLC_CCD02_03_SNAP");
                PLC_Device_CCD02_03_SNAP.Bool = false;
                PLC_Device_CCD02_03_SNAP_按鈕.Bool = false;
                cnt_Program_CCD02_03_SNAP = 65535;
            }
            if (cnt_Program_CCD02_03_SNAP == 65535) cnt_Program_CCD02_03_SNAP = 1;
            if (cnt_Program_CCD02_03_SNAP == 1) cnt_Program_CCD02_03_SNAP_檢查按下(ref cnt_Program_CCD02_03_SNAP);
            if (cnt_Program_CCD02_03_SNAP == 2) cnt_Program_CCD02_03_SNAP_初始化(ref cnt_Program_CCD02_03_SNAP);
            if (cnt_Program_CCD02_03_SNAP == 3) cnt_Program_CCD02_03_SNAP_開始取像(ref cnt_Program_CCD02_03_SNAP);
            if (cnt_Program_CCD02_03_SNAP == 4) cnt_Program_CCD02_03_SNAP_取像結束(ref cnt_Program_CCD02_03_SNAP);
            if (cnt_Program_CCD02_03_SNAP == 5) cnt_Program_CCD02_03_SNAP_繪製影像(ref cnt_Program_CCD02_03_SNAP);
            if (cnt_Program_CCD02_03_SNAP == 6) cnt_Program_CCD02_03_SNAP = 65500;
            if (cnt_Program_CCD02_03_SNAP > 1) cnt_Program_CCD02_03_SNAP_檢查放開(ref cnt_Program_CCD02_03_SNAP);

            if (cnt_Program_CCD02_03_SNAP == 65500)
            {
                PLC_Device_CCD02_03_SNAP_按鈕.Bool = false;
                PLC_Device_CCD02_03_SNAP.Bool = false;
                cnt_Program_CCD02_03_SNAP = 65535;
            }
        }
        void cnt_Program_CCD02_03_SNAP_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD02_03_SNAP_按鈕.Bool)
            {
                PLC_Device_CCD02_03_SNAP.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_03_SNAP_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD02_03_SNAP.Bool) cnt = 65500;
        }
        void cnt_Program_CCD02_03_SNAP_初始化(ref int cnt)
        {
            PLC_Device_CCD02_SNAP_電子快門.Value = PLC_Device_CCD02_03_SNAP_電子快門.Value;
            PLC_Device_CCD02_SNAP_視訊增益.Value = PLC_Device_CCD02_03_SNAP_視訊增益.Value;
            PLC_Device_CCD02_SNAP_銳利度.Value = PLC_Device_CCD02_03_SNAP_銳利度.Value;
            if (PLC_Device_CCD02_03_SNAP_光源亮度_紅正照.Value != 0)
            {
                this.光源控制(enum_光源.CCD02_紅正照, (byte)this.PLC_Device_CCD02_03_SNAP_光源亮度_紅正照.Value);
                this.光源控制(enum_光源.CCD02_紅正照, true);
            }
            else if (this.PLC_Device_CCD02_03_SNAP_光源亮度_紅正照.Value == 0)
            {
                this.光源控制(enum_光源.CCD02_紅正照, (byte)0);
                this.光源控制(enum_光源.CCD02_紅正照, false);
            }
            cnt++;
        }
        void cnt_Program_CCD02_03_SNAP_開始取像(ref int cnt)
        {
            if (!PLC_Device_CCD02_SNAP.Bool)
            {
                PLC_Device_CCD02_SNAP.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_03_SNAP_取像結束(ref int cnt)
        {
            if (!PLC_Device_CCD02_SNAP.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD02_03_SNAP_繪製影像(ref int cnt)
        {
            this.CCD02_03_SrcImageHandle = this.h_Canvas_Main_CCD02_03_檢測畫面.VegaHandle;
            this.h_Canvas_Main_CCD02_03_檢測畫面.ImageCopy(this.CCD02_AxImageBW8.VegaHandle);

            this.CCD02_03_SrcImageHandle = this.h_Canvas_Tech_CCD02_03.VegaHandle;
            this.h_Canvas_Tech_CCD02_03.ImageCopy(this.CCD02_AxImageBW8.VegaHandle);
            this.h_Canvas_Tech_CCD02_03.SetImageSize(this.h_Canvas_Tech_CCD02_03.ImageWidth, this.h_Canvas_Tech_CCD02_03.ImageHeight);

            if (!PLC_Device_CCD02_03_Tech_取像並檢驗.Bool && !PLC_Device_CCD02_03_Main_取像並檢驗.Bool)
            {
                if (this.PLC_Device_CCD02_03_SNAP.Bool) this.h_Canvas_Tech_CCD02_03.RefreshCanvas();


                if (PLC_Device_CCD02_03_SNAP_LIVE.Bool)
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
        #region PLC_CCD02_03_Main_取像並檢驗
        PLC_Device PLC_Device_CCD02_03_Main_取像並檢驗按鈕 = new PLC_Device("S39970");
        PLC_Device PLC_Device_CCD02_03_Main_取像並檢驗 = new PLC_Device("S39971");
        PLC_Device PLC_Device_CCD02_03_Main_取像並檢驗_OK = new PLC_Device("S39972");
        PLC_Device PLC_Device_CCD02_03_PLC觸發檢測 = new PLC_Device("S39770");
        PLC_Device PLC_Device_CCD02_03_PLC觸發檢測完成 = new PLC_Device("S39771");
        PLC_Device PLC_Device_CCD02_03_Main_取像完成 = new PLC_Device("S39772");
        PLC_Device PLC_Device_CCD02_03_Main_BUSY = new PLC_Device("S39773");
        bool flag_CCD02_03_開始存檔 = false;
        String CCD02_03_原圖位置, CCD02_03_量測圖位置;
        PLC_Device PLC_NumBox_CCD02_03_OK最大儲存張數 = new PLC_Device("F14403");
        PLC_Device PLC_NumBox_CCD02_03_NG最大儲存張數 = new PLC_Device("F14404");

        MyTimer CCD02_03_Init_Timer = new MyTimer();
        int cnt_Program_CCD02_03_Main_取像並檢驗 = 65534;
        void sub_Program_CCD02_03_Main_取像並檢驗()
        {
            if (cnt_Program_CCD02_03_Main_取像並檢驗 == 65534)
            {
                PLC_Device_CCD02_03_Main_取像並檢驗.SetComment("PLC_CCD02_03_Main_取像並檢驗");
                PLC_Device_CCD02_03_Main_BUSY.Bool = false;
                PLC_Device_CCD02_03_Main_取像完成.Bool = false;
                PLC_Device_CCD02_03_Main_取像並檢驗.Bool = false;
                PLC_Device_CCD02_03_PLC觸發檢測.Bool = false;
                PLC_Device_CCD02_03_PLC觸發檢測完成.Bool = false;
                PLC_Device_CCD02_03_Main_取像並檢驗_OK.Bool = false;
                PLC_Device_CCD02_03_Main_取像並檢驗按鈕.Bool = false;
                cnt_Program_CCD02_03_Main_取像並檢驗 = 65535;

            }
            if (cnt_Program_CCD02_03_Main_取像並檢驗 == 65535) cnt_Program_CCD02_03_Main_取像並檢驗 = 1;
            if (cnt_Program_CCD02_03_Main_取像並檢驗 == 1) cnt_Program_CCD02_03_Main_取像並檢驗_檢查按下(ref cnt_Program_CCD02_03_Main_取像並檢驗);
            if (cnt_Program_CCD02_03_Main_取像並檢驗 == 2) cnt_Program_CCD02_03_Main_取像並檢驗_初始化(ref cnt_Program_CCD02_03_Main_取像並檢驗);
            if (cnt_Program_CCD02_03_Main_取像並檢驗 == 3) cnt_Program_CCD02_03_Main_取像並檢驗_開始SNAP(ref cnt_Program_CCD02_03_Main_取像並檢驗);
            if (cnt_Program_CCD02_03_Main_取像並檢驗 == 4) cnt_Program_CCD02_03_Main_取像並檢驗_結束SNAP(ref cnt_Program_CCD02_03_Main_取像並檢驗);
            if (cnt_Program_CCD02_03_Main_取像並檢驗 == 5) cnt_Program_CCD02_03_Main_取像並檢驗_開始計算一次(ref cnt_Program_CCD02_03_Main_取像並檢驗);
            if (cnt_Program_CCD02_03_Main_取像並檢驗 == 6) cnt_Program_CCD02_03_Main_取像並檢驗_結束計算一次(ref cnt_Program_CCD02_03_Main_取像並檢驗);
            if (cnt_Program_CCD02_03_Main_取像並檢驗 == 7) cnt_Program_CCD02_03_Main_取像並檢驗_繪製畫布(ref cnt_Program_CCD02_03_Main_取像並檢驗);
            if (cnt_Program_CCD02_03_Main_取像並檢驗 == 8) cnt_Program_CCD02_03_Main_取像並檢驗_檢查重測次數(ref cnt_Program_CCD02_03_Main_取像並檢驗);
            if (cnt_Program_CCD02_03_Main_取像並檢驗 == 9) cnt_Program_CCD02_03_Main_取像並檢驗 = 65500;
            if (cnt_Program_CCD02_03_Main_取像並檢驗 > 1) cnt_Program_CCD02_03_Main_取像並檢驗_檢查放開(ref cnt_Program_CCD02_03_Main_取像並檢驗);

            if (cnt_Program_CCD02_03_Main_取像並檢驗 == 65500)
            {
                PLC_Device_CCD02_03_Main_BUSY.Bool = false;
                PLC_Device_CCD02_03_Main_取像完成.Bool = false;
                PLC_Device_CCD02_03_Main_取像並檢驗.Bool = false;
                PLC_Device_CCD02_03_PLC觸發檢測.Bool = false;
                PLC_Device_CCD02_03_Main_取像並檢驗按鈕.Bool = false;
                cnt_Program_CCD02_03_Main_取像並檢驗 = 65535;
            }
        }
        void cnt_Program_CCD02_03_Main_取像並檢驗_檢查按下(ref int cnt)
        {

            if (PLC_Device_CCD02_03_Main_取像並檢驗按鈕.Bool || PLC_Device_CCD02_03_PLC觸發檢測.Bool)
            {
                CCD02_03_Init_Timer.TickStop();
                CCD02_03_Init_Timer.StartTickTime(100000);
                PLC_Device_CCD02_03_Main_取像並檢驗.Bool = true;
                cnt++;
            }



        }
        void cnt_Program_CCD02_03_Main_取像並檢驗_檢查放開(ref int cnt)
        {
            //if (!PLC_Device_CCD02_03_Main_取像並檢驗.Bool && !PLC_Device_CCD02_03_PLC觸發檢測.Bool) cnt = 65500;
        }
        void cnt_Program_CCD02_03_Main_取像並檢驗_初始化(ref int cnt)
        {
            PLC_Device_CCD02_03_Main_BUSY.Bool = true;
            PLC_Device_CCD02_03_PLC觸發檢測完成.Bool = false;
            cnt++;
        }
        void cnt_Program_CCD02_03_Main_取像並檢驗_開始SNAP(ref int cnt)
        {
            if (!PLC_Device_CCD02_03_SNAP.Bool)
            {
                PLC_Device_CCD02_03_SNAP_按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_03_Main_取像並檢驗_結束SNAP(ref int cnt)
        {
            if (!PLC_Device_CCD02_03_SNAP_按鈕.Bool)
            {
                光源控制(enum_光源.CCD02_紅正照, (byte)0);
                光源控制(enum_光源.CCD02_紅正照, false);
                PLC_Device_CCD02_03_Main_取像完成.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD02_03_Main_取像並檢驗_開始計算一次(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_03_計算一次.Bool)
            {

                this.PLC_Device_CCD02_03_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_03_Main_取像並檢驗_結束計算一次(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_03_計算一次.Bool)
            {
                Console.WriteLine($"CCD02_03檢測,耗時 {CCD02_03_Init_Timer.ToString()}");
                cnt++;
            }
        }
        void cnt_Program_CCD02_03_Main_取像並檢驗_繪製畫布(ref int cnt)
        {

            if (CCD02_03_SrcImageHandle != 0)
            {
                this.h_Canvas_Main_CCD02_03_檢測畫面.RefreshCanvas();
                PLC_Device_CCD02_03_PLC觸發檢測完成.Bool = true;
                flag_CCD02_03_開始存檔 = true;
            }
            cnt++;
        }
        void cnt_Program_CCD02_03_Main_取像並檢驗_檢查重測次數(ref int cnt)
        {
            cnt++;
        }
        private void CCD02_03_儲存圖片()
        {
            if (flag_CCD02_03_開始存檔)
            {
                String FilePlaceOK = plC_WordBox_CCD02_03_OK存圖路徑.Text;
                String FileNameOK = "CCD02_03_OK";
                String FilePlaceNG = plC_WordBox_CCD02_03_NG存圖路徑.Text;
                String FileNameNG = "CCD02_03_NG";
                儲存檔案_往後移位(FilePlaceOK, FileNameOK, PLC_NumBox_CCD02_03_OK最大儲存張數.Value);
                儲存檔案_往後移位(FilePlaceNG, FileNameNG, PLC_NumBox_CCD02_03_NG最大儲存張數.Value);
                if (PLC_Device_CCD02_03_Main_取像並檢驗_OK.Bool)
                {
                    整理檔案(FilePlaceOK, FileNameOK, PLC_NumBox_CCD02_03_OK最大儲存張數.Value);
                    FileNameOK = FileNameOK + "_OK";
                    CCD02_03_原圖位置 = CCD02_03_OK儲存檔案檢查(FilePlaceOK, FileNameOK + "_A", PLC_NumBox_CCD02_03_OK最大儲存張數.Value);
                    CCD02_03_量測圖位置 = CCD02_03_原圖位置.Replace("_A", "_B");
                    this.Invoke(new Action(delegate
                    {
                        if (plC_ComboBox_CCD02_03_OK是否存圖.SelectedIndex == 0)
                        {
                            this.h_Canvas_Main_CCD02_03_檢測畫面.SaveImage(CCD02_03_原圖位置);
                        }
                    }));
                }
                else if (!PLC_Device_CCD02_03_Main_取像並檢驗_OK.Bool)
                {
                    整理檔案(FilePlaceNG, FileNameNG, PLC_NumBox_CCD02_03_NG最大儲存張數.Value);
                    FileNameNG = FileNameNG + "_NG";
                    CCD02_03_原圖位置 = CCD02_03_NG儲存檔案檢查(FilePlaceNG, FileNameNG + "_A", PLC_NumBox_CCD02_03_NG最大儲存張數.Value);
                    CCD02_03_量測圖位置 = CCD02_03_原圖位置.Replace("_A", "_B");
                    this.Invoke(new Action(delegate
                    {
                        if (plC_ComboBox_CCD02_03_NG是否存圖.SelectedIndex == 0)
                        {
                            this.h_Canvas_Main_CCD02_03_檢測畫面.SaveImage(CCD02_03_原圖位置);
                        }
                    }));
                }
                flag_CCD02_03_開始存檔 = false;
            }
        }
        #endregion
        #region PLC_CCD02_03_Tech_取像並檢驗
        PLC_Device PLC_Device_CCD02_03_Tech_取像並檢驗按鈕 = new PLC_Device("M15350");
        PLC_Device PLC_Device_CCD02_03_Tech_取像並檢驗 = new PLC_Device("M15345");
        int cnt_Program_CCD02_03_Tech_取像並檢驗 = 65534;
        void sub_Program_CCD02_03_Tech_取像並檢驗()
        {
            if (cnt_Program_CCD02_03_Tech_取像並檢驗 == 65534)
            {
                PLC_Device_CCD02_03_Tech_取像並檢驗.SetComment("PLC_CCD02_03_Tech_取像並檢驗");
                PLC_Device_CCD02_03_Tech_取像並檢驗.Bool = false;
                cnt_Program_CCD02_03_Tech_取像並檢驗 = 65535;
            }
            if (cnt_Program_CCD02_03_Tech_取像並檢驗 == 65535) cnt_Program_CCD02_03_Tech_取像並檢驗 = 1;
            if (cnt_Program_CCD02_03_Tech_取像並檢驗 == 1) cnt_Program_CCD02_03_Tech_取像並檢驗_檢查按下(ref cnt_Program_CCD02_03_Tech_取像並檢驗);
            if (cnt_Program_CCD02_03_Tech_取像並檢驗 == 2) cnt_Program_CCD02_03_Tech_取像並檢驗_初始化(ref cnt_Program_CCD02_03_Tech_取像並檢驗);
            if (cnt_Program_CCD02_03_Tech_取像並檢驗 == 3) cnt_Program_CCD02_03_Tech_取像並檢驗_SNAP一次開始(ref cnt_Program_CCD02_03_Tech_取像並檢驗);
            if (cnt_Program_CCD02_03_Tech_取像並檢驗 == 4) cnt_Program_CCD02_03_Tech_取像並檢驗_SNAP一次結束(ref cnt_Program_CCD02_03_Tech_取像並檢驗);
            if (cnt_Program_CCD02_03_Tech_取像並檢驗 == 5) cnt_Program_CCD02_03_Tech_取像並檢驗_計算一次開始(ref cnt_Program_CCD02_03_Tech_取像並檢驗);
            if (cnt_Program_CCD02_03_Tech_取像並檢驗 == 6) cnt_Program_CCD02_03_Tech_取像並檢驗_計算一次結束(ref cnt_Program_CCD02_03_Tech_取像並檢驗);
            if (cnt_Program_CCD02_03_Tech_取像並檢驗 == 7) cnt_Program_CCD02_03_Tech_取像並檢驗_繪製畫布(ref cnt_Program_CCD02_03_Tech_取像並檢驗);
            if (cnt_Program_CCD02_03_Tech_取像並檢驗 == 8) cnt_Program_CCD02_03_Tech_取像並檢驗 = 65500;
            if (cnt_Program_CCD02_03_Tech_取像並檢驗 > 1) cnt_Program_CCD02_03_Tech_取像並檢驗_檢查放開(ref cnt_Program_CCD02_03_Tech_取像並檢驗);

            if (cnt_Program_CCD02_03_Tech_取像並檢驗 == 65500)
            {
                PLC_Device_CCD02_03_Tech_取像並檢驗按鈕.Bool = false;
                PLC_Device_CCD02_03_Tech_取像並檢驗.Bool = false;
                cnt_Program_CCD02_03_Tech_取像並檢驗 = 65535;
            }
        }
        void cnt_Program_CCD02_03_Tech_取像並檢驗_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD02_03_Tech_取像並檢驗按鈕.Bool)
            {
                PLC_Device_CCD02_03_Tech_取像並檢驗.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD02_03_Tech_取像並檢驗_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD02_03_Tech_取像並檢驗.Bool) cnt = 65500;
        }
        void cnt_Program_CCD02_03_Tech_取像並檢驗_初始化(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD02_03_Tech_取像並檢驗_SNAP一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_03_SNAP.Bool)
            {
                this.PLC_Device_CCD02_03_SNAP_按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_03_Tech_取像並檢驗_SNAP一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_03_SNAP_按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD02_03_Tech_取像並檢驗_計算一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_03_計算一次.Bool)
            {
                this.PLC_Device_CCD02_03_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_03_Tech_取像並檢驗_計算一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_03_計算一次.Bool)
            {

                cnt++;
            }
        }
        void cnt_Program_CCD02_03_Tech_取像並檢驗_繪製畫布(ref int cnt)
        {
            if (CCD02_03_SrcImageHandle != 0)
            {
                this.h_Canvas_Tech_CCD02_03.RefreshCanvas();
            }
            if (PLC_Device_CCD02_03_SNAP_LIVE.Bool)
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

        #endregion
        #region PLC_CCD02_03_Tech_檢驗一次
        PLC_Device PLC_Device_CCD02_03_Tech_檢驗一次按鈕 = new PLC_Device("M15550");
        PLC_Device PLC_Device_CCD02_03_Tech_檢驗一次 = new PLC_Device("M15545");
        int cnt_Program_CCD02_03_Tech_檢驗一次 = 65534;
        void sub_Program_CCD02_03_Tech_檢驗一次()
        {
            if (cnt_Program_CCD02_03_Tech_檢驗一次 == 65534)
            {
                PLC_Device_CCD02_03_Tech_檢驗一次.SetComment("PLC_CCD02_03_Tech_檢驗一次");
                PLC_Device_CCD02_03_Tech_檢驗一次按鈕.Bool = false;
                PLC_Device_CCD02_03_Tech_檢驗一次.Bool = false;
                cnt_Program_CCD02_03_Tech_檢驗一次 = 65535;
            }
            if (cnt_Program_CCD02_03_Tech_檢驗一次 == 65535) cnt_Program_CCD02_03_Tech_檢驗一次 = 1;
            if (cnt_Program_CCD02_03_Tech_檢驗一次 == 1) cnt_Program_CCD02_03_Tech_檢驗一次_檢查按下(ref cnt_Program_CCD02_03_Tech_檢驗一次);
            if (cnt_Program_CCD02_03_Tech_檢驗一次 == 2) cnt_Program_CCD02_03_Tech_檢驗一次_初始化(ref cnt_Program_CCD02_03_Tech_檢驗一次);
            if (cnt_Program_CCD02_03_Tech_檢驗一次 == 3) cnt_Program_CCD02_03_Tech_檢驗一次_計算一次開始(ref cnt_Program_CCD02_03_Tech_檢驗一次);
            if (cnt_Program_CCD02_03_Tech_檢驗一次 == 4) cnt_Program_CCD02_03_Tech_檢驗一次_計算一次結束(ref cnt_Program_CCD02_03_Tech_檢驗一次);
            if (cnt_Program_CCD02_03_Tech_檢驗一次 == 5) cnt_Program_CCD02_03_Tech_檢驗一次_繪製畫布(ref cnt_Program_CCD02_03_Tech_檢驗一次);
            if (cnt_Program_CCD02_03_Tech_檢驗一次 == 6) cnt_Program_CCD02_03_Tech_檢驗一次 = 65500;
            if (cnt_Program_CCD02_03_Tech_檢驗一次 > 1) cnt_Program_CCD02_03_Tech_檢驗一次_檢查放開(ref cnt_Program_CCD02_03_Tech_檢驗一次);

            if (cnt_Program_CCD02_03_Tech_檢驗一次 == 65500)
            {
                PLC_Device_CCD02_03_Tech_檢驗一次按鈕.Bool = false;
                PLC_Device_CCD02_03_Tech_檢驗一次.Bool = false;
                cnt_Program_CCD02_03_Tech_檢驗一次 = 65535;
            }
        }
        void cnt_Program_CCD02_03_Tech_檢驗一次_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD02_03_Tech_檢驗一次按鈕.Bool)
            {
                PLC_Device_CCD02_03_Tech_檢驗一次.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD02_03_Tech_檢驗一次_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD02_03_Tech_檢驗一次.Bool) cnt = 65500;
        }
        void cnt_Program_CCD02_03_Tech_檢驗一次_初始化(ref int cnt)
        {

            cnt++;
        }
        void cnt_Program_CCD02_03_Tech_檢驗一次_計算一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_03_計算一次.Bool)
            {
                this.PLC_Device_CCD02_03_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_03_Tech_檢驗一次_計算一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_03_計算一次.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD02_03_Tech_檢驗一次_繪製畫布(ref int cnt)
        {

            if (CCD02_03_SrcImageHandle != 0)
            {
                this.h_Canvas_Tech_CCD02_03.RefreshCanvas();
            }
            cnt++;
        }

        #endregion
        #region PLC_CCD02_03_Main_檢驗一次
        PLC_Device PLC_Device_CCD02_03_Main_檢驗一次按鈕 = new PLC_Device("M15814");
        PLC_Device PLC_Device_CCD02_03_Main_檢驗一次 = new PLC_Device("M15815");
        int cnt_Program_CCD02_03_Main_檢驗一次 = 65534;
        void sub_Program_CCD02_03_Main_檢驗一次()
        {
            if (cnt_Program_CCD02_03_Main_檢驗一次 == 65534)
            {
                PLC_Device_CCD02_03_Main_檢驗一次.SetComment("PLC_CCD02_03_Main_檢驗一次");
                PLC_Device_CCD02_03_Main_檢驗一次.Bool = false;
                PLC_Device_CCD02_03_Main_檢驗一次按鈕.Bool = false;
                cnt_Program_CCD02_03_Main_檢驗一次 = 65535;
            }
            if (cnt_Program_CCD02_03_Main_檢驗一次 == 65535) cnt_Program_CCD02_03_Main_檢驗一次 = 1;
            if (cnt_Program_CCD02_03_Main_檢驗一次 == 1) cnt_Program_CCD02_03_Main_檢驗一次_檢查按下(ref cnt_Program_CCD02_03_Main_檢驗一次);
            if (cnt_Program_CCD02_03_Main_檢驗一次 == 2) cnt_Program_CCD02_03_Main_檢驗一次_初始化(ref cnt_Program_CCD02_03_Main_檢驗一次);
            if (cnt_Program_CCD02_03_Main_檢驗一次 == 3) cnt_Program_CCD02_03_Main_檢驗一次_計算一次開始(ref cnt_Program_CCD02_03_Main_檢驗一次);
            if (cnt_Program_CCD02_03_Main_檢驗一次 == 4) cnt_Program_CCD02_03_Main_檢驗一次_計算一次結束(ref cnt_Program_CCD02_03_Main_檢驗一次);
            if (cnt_Program_CCD02_03_Main_檢驗一次 == 5) cnt_Program_CCD02_03_Main_檢驗一次_繪製畫布(ref cnt_Program_CCD02_03_Main_檢驗一次);
            if (cnt_Program_CCD02_03_Main_檢驗一次 == 6) cnt_Program_CCD02_03_Main_檢驗一次 = 65500;
            if (cnt_Program_CCD02_03_Main_檢驗一次 > 1) cnt_Program_CCD02_03_Main_檢驗一次_檢查放開(ref cnt_Program_CCD02_03_Main_檢驗一次);

            if (cnt_Program_CCD02_03_Main_檢驗一次 == 65500)
            {
                PLC_Device_CCD02_03_Main_檢驗一次按鈕.Bool = false;
                PLC_Device_CCD02_03_Main_檢驗一次.Bool = false;
                cnt_Program_CCD02_03_Main_檢驗一次 = 65535;
            }
        }
        void cnt_Program_CCD02_03_Main_檢驗一次_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD02_03_Main_檢驗一次按鈕.Bool)
            {
                PLC_Device_CCD02_03_Main_檢驗一次.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD02_03_Main_檢驗一次_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD02_03_Main_檢驗一次.Bool) cnt = 65500;
        }
        void cnt_Program_CCD02_03_Main_檢驗一次_初始化(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD02_03_Main_檢驗一次_計算一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_03_計算一次.Bool)
            {
                this.PLC_Device_CCD02_03_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_03_Main_檢驗一次_計算一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_03_計算一次.Bool)
            {

                cnt++;
            }
        }
        void cnt_Program_CCD02_03_Main_檢驗一次_繪製畫布(ref int cnt)
        {
            if (CCD02_03_SrcImageHandle != 0)
            {
                this.h_Canvas_Main_CCD02_03_檢測畫面.RefreshCanvas();
            }

            cnt++;
        }
        #endregion
        #region PLC_CCD02_03_計算一次
        PLC_Device PLC_Device_CCD02_03_計算一次 = new PLC_Device("S5085");
        PLC_Device PLC_Device_CCD02_03_計算一次_OK = new PLC_Device("S5086");
        PLC_Device PLC_Device_CCD02_03_計算一次_READY = new PLC_Device("S5087");
        MyTimer MyTimer_CCD02_03_計算一次 = new MyTimer();
        int cnt_Program_CCD02_03_計算一次 = 65534;
        void sub_Program_CCD02_03_計算一次()
        {
            this.PLC_Device_CCD02_03_計算一次_READY.Bool = !this.PLC_Device_CCD02_03_計算一次.Bool;
            if (cnt_Program_CCD02_03_計算一次 == 65534)
            {
                PLC_Device_CCD02_03_計算一次.SetComment("PLC_CCD02_03_計算一次");
                PLC_Device_CCD02_03_計算一次.Bool = false;

                cnt_Program_CCD02_03_計算一次 = 65535;
            }
            if (cnt_Program_CCD02_03_計算一次 == 65535) cnt_Program_CCD02_03_計算一次 = 1;
            if (cnt_Program_CCD02_03_計算一次 == 1) cnt_Program_CCD02_03_計算一次_檢查按下(ref cnt_Program_CCD02_03_計算一次);
            if (cnt_Program_CCD02_03_計算一次 == 2) cnt_Program_CCD02_03_計算一次_初始化(ref cnt_Program_CCD02_03_計算一次);
            if (cnt_Program_CCD02_03_計算一次 == 3) cnt_Program_CCD02_03_計算一次_步驟01開始(ref cnt_Program_CCD02_03_計算一次);
            if (cnt_Program_CCD02_03_計算一次 == 4) cnt_Program_CCD02_03_計算一次_步驟01結束(ref cnt_Program_CCD02_03_計算一次);
            if (cnt_Program_CCD02_03_計算一次 == 5) cnt_Program_CCD02_03_計算一次_步驟02開始(ref cnt_Program_CCD02_03_計算一次);
            if (cnt_Program_CCD02_03_計算一次 == 6) cnt_Program_CCD02_03_計算一次_步驟02結束(ref cnt_Program_CCD02_03_計算一次);
            if (cnt_Program_CCD02_03_計算一次 == 7) cnt_Program_CCD02_03_計算一次_步驟03開始(ref cnt_Program_CCD02_03_計算一次);
            if (cnt_Program_CCD02_03_計算一次 == 8) cnt_Program_CCD02_03_計算一次_步驟03結束(ref cnt_Program_CCD02_03_計算一次);
            if (cnt_Program_CCD02_03_計算一次 == 9) cnt_Program_CCD02_03_計算一次_步驟04開始(ref cnt_Program_CCD02_03_計算一次);
            if (cnt_Program_CCD02_03_計算一次 == 10) cnt_Program_CCD02_03_計算一次_步驟04結束(ref cnt_Program_CCD02_03_計算一次);
            if (cnt_Program_CCD02_03_計算一次 == 11) cnt_Program_CCD02_03_計算一次_步驟05開始(ref cnt_Program_CCD02_03_計算一次);
            if (cnt_Program_CCD02_03_計算一次 == 12) cnt_Program_CCD02_03_計算一次_步驟05結束(ref cnt_Program_CCD02_03_計算一次);
            if (cnt_Program_CCD02_03_計算一次 == 13) cnt_Program_CCD02_03_計算一次_步驟06開始(ref cnt_Program_CCD02_03_計算一次);
            if (cnt_Program_CCD02_03_計算一次 == 14) cnt_Program_CCD02_03_計算一次_步驟06結束(ref cnt_Program_CCD02_03_計算一次);
            if (cnt_Program_CCD02_03_計算一次 == 15) cnt_Program_CCD02_03_計算一次_計算結果(ref cnt_Program_CCD02_03_計算一次);
            if (cnt_Program_CCD02_03_計算一次 == 16) cnt_Program_CCD02_03_計算一次 = 65500;
            if (cnt_Program_CCD02_03_計算一次 > 1) cnt_Program_CCD02_03_計算一次_檢查放開(ref cnt_Program_CCD02_03_計算一次);

            if (cnt_Program_CCD02_03_計算一次 == 65500)
            {
                PLC_Device_CCD02_03_計算一次.Bool = false;
                cnt_Program_CCD02_03_計算一次 = 65535;
            }
        }
        void cnt_Program_CCD02_03_計算一次_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD02_03_計算一次.Bool) cnt++;
        }
        void cnt_Program_CCD02_03_計算一次_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD02_03_計算一次.Bool) cnt = 65500;
        }
        void cnt_Program_CCD02_03_計算一次_初始化(ref int cnt)
        {
            PLC_Device_CCD02_03_基準線量測.Bool = false;
            PLC_Device_CCD02_03_PIN量測_量測框調整.Bool = false;
            PLC_Device_CCD02_03_PIN量測_檢測距離計算.Bool = false;
            PLC_Device_CCD02_03_PIN量測_檢測距離計算.Bool = false;
            cnt++;
        }
        void cnt_Program_CCD02_03_計算一次_步驟01開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_03_基準線量測按鈕.Bool)
            {
                this.PLC_Device_CCD02_03_基準線量測按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_03_計算一次_步驟01結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_03_基準線量測按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD02_03_計算一次_步驟02開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_03_PIN量測_量測框調整.Bool)
            {
                this.PLC_Device_CCD02_03_PIN量測_量測框調整.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD02_03_計算一次_步驟02結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD02_03_PIN量測_量測框調整.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD02_03_計算一次_步驟03開始(ref int cnt)
        {
            cnt++;        
        }
        void cnt_Program_CCD02_03_計算一次_步驟03結束(ref int cnt)
        {
            cnt++;     
        }
        void cnt_Program_CCD02_03_計算一次_步驟04開始(ref int cnt)
        {
            if (!PLC_Device_CCD02_03_PIN量測_檢測距離計算.Bool)
            {
                PLC_Device_CCD02_03_PIN量測_檢測距離計算.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_03_計算一次_步驟04結束(ref int cnt)
        {
            if (!PLC_Device_CCD02_03_PIN量測_檢測距離計算.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD02_03_計算一次_步驟05開始(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD02_03_計算一次_步驟05結束(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD02_03_計算一次_步驟06開始(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD02_03_計算一次_步驟06結束(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD02_03_計算一次_計算結果(ref int cnt)
        {
            bool flag = true;
            if (!this.PLC_Device_CCD02_03_基準線量測_OK.Bool) flag = false;
            if (!this.PLC_Device_CCD02_03_PIN量測_檢測距離計算_OK.Bool) flag = false;
            this.PLC_Device_CCD02_03_Main_取像並檢驗_OK.Bool = flag;
            this.PLC_Device_CCD02_03_計算一次_OK.Bool = flag;
            //flag_CCD02_03_上端水平度寫入列表資料 = true;
            //flag_CCD02_03_上端間距寫入列表資料 = true;
            //flag_CCD02_03_上端水平度差值寫入列表資料 = true;

            cnt++;
        }

        #endregion
        #region PLC_CCD02_03_基準線量測
        AxOvkMsr.AxLineMsr CCD02_03_水平基準線量測_AxLineMsr;
        AxOvkMsr.AxLineRegression CCD02_03_水平基準線量測_AxLineRegression;
        AxOvkMsr.AxLineMsr CCD02_03_垂直基準線量測_AxLineMsr;
        AxOvkMsr.AxLineRegression CCD02_03_垂直基準線量測_AxLineRegression;
        AxOvkMsr.AxIntersectionMsr CCD02_03_基準線量測_AxIntersectionMsr;
        private PointF Point_CCD02_03_中心基準座標_量測點 = new PointF();
        PLC_Device PLC_Device_CCD02_03_基準線量測按鈕 = new PLC_Device("S6720");
        PLC_Device PLC_Device_CCD02_03_基準線量測 = new PLC_Device("S6715");
        PLC_Device PLC_Device_CCD02_03_基準線量測_OK = new PLC_Device("S6716");
        PLC_Device PLC_Device_CCD02_03_基準線量測_測試完成 = new PLC_Device("S6717");
        PLC_Device PLC_Device_CCD02_03_基準線量測_RefreshCanvas = new PLC_Device("S6718");

        PLC_Device PLC_Device_CCD02_03_基準線量測_變化銳利度 = new PLC_Device("F18300");
        PLC_Device PLC_Device_CCD02_03_基準線量測_延伸變化強度 = new PLC_Device("F18301");
        PLC_Device PLC_Device_CCD02_03_基準線量測_灰階變化面積 = new PLC_Device("F18302");
        PLC_Device PLC_Device_CCD02_03_基準線量測_雜訊抑制 = new PLC_Device("F18303");
        PLC_Device PLC_Device_CCD02_03_基準線量測_最佳回歸線計算次數 = new PLC_Device("F18304");
        PLC_Device PLC_Device_CCD02_03_基準線量測_最佳回歸線濾波 = new PLC_Device("F18305");
        PLC_Device PLC_Device_CCD02_03_基準線量測_量測顏色變化 = new PLC_Device("F18310");
        PLC_Device PLC_Device_CCD02_03_基準線量測_基準線偏移 = new PLC_Device("F18311");
        PLC_Device PLC_Device_CCD02_03_基準線量測_基準線偏移_上排 = new PLC_Device("F18318");
        PLC_Device PLC_Device_CCD02_03_基準線量測_基準線偏移_下排 = new PLC_Device("F18319");

        PLC_Device PLC_Device_CCD02_03_水平基準線量測_量測框起點X座標 = new PLC_Device("F18306");
        PLC_Device PLC_Device_CCD02_03_水平基準線量測_量測框起點Y座標 = new PLC_Device("F18307");
        PLC_Device PLC_Device_CCD02_03_水平基準線量測_量測框終點X座標 = new PLC_Device("F18308");
        PLC_Device PLC_Device_CCD02_03_水平基準線量測_量測框終點Y座標 = new PLC_Device("F18309");
        PLC_Device PLC_Device_CCD02_03_水平基準線量測_量測高度 = new PLC_Device("F18312");
        PLC_Device PLC_Device_CCD02_03_水平基準線量測_量測中心_X = new PLC_Device("F18320");
        PLC_Device PLC_Device_CCD02_03_水平基準線量測_量測中心_Y = new PLC_Device("F18321");

        PLC_Device PLC_Device_CCD02_03_垂直基準線量測_量測框起點X座標 = new PLC_Device("F18313");
        PLC_Device PLC_Device_CCD02_03_垂直基準線量測_量測框起點Y座標 = new PLC_Device("F18314");
        PLC_Device PLC_Device_CCD02_03_垂直基準線量測_量測框終點X座標 = new PLC_Device("F18315");
        PLC_Device PLC_Device_CCD02_03_垂直基準線量測_量測框終點Y座標 = new PLC_Device("F18316");
        PLC_Device PLC_Device_CCD02_03_垂直基準線量測_量測高度 = new PLC_Device("F18317");



        private void H_Canvas_Tech_CCD02_03_基準線量測_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {
            try
            {
                PointF 水平量測中心 = new PointF(Point_CCD02_03_中心基準座標_量測點.X, Point_CCD02_03_中心基準座標_量測點.Y);

                if (PLC_Device_CCD02_03_Main_取像並檢驗.Bool || PLC_Device_CCD02_03_PLC觸發檢測.Bool || PLC_Device_CCD02_03_Main_檢驗一次.Bool)
                {
                    if (this.PLC_Device_CCD02_03_基準線量測_RefreshCanvas.Bool)
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);

                        DrawingClass.Draw.水平線段繪製(0, 10000, CCD02_03_水平基準線量測_AxLineMsr.MeasuredSlope, CCD02_03_水平基準線量測_AxLineMsr.MeasuredPivotX, CCD02_03_水平基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD02_03_基準線量測_基準線偏移.Value, Color.Lime, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.垂直線段繪製(0, 10000, CCD02_03_垂直基準線量測_AxLineMsr.MeasuredSlope, CCD02_03_垂直基準線量測_AxLineMsr.MeasuredPivotX, CCD02_03_垂直基準線量測_AxLineMsr.MeasuredPivotY, Color.Lime, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.十字中心(水平量測中心, 100, Color.Red, 2, g, ZoomX, ZoomY);
                        g.Dispose();
                        g = null;
                    }
                }
                else if (PLC_Device_CCD02_03_Tech_檢驗一次.Bool)
                {
                    if (this.PLC_Device_CCD02_03_基準線量測_RefreshCanvas.Bool)
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);

                        DrawingClass.Draw.水平線段繪製(0, 10000, CCD02_03_水平基準線量測_AxLineMsr.MeasuredSlope, CCD02_03_水平基準線量測_AxLineMsr.MeasuredPivotX, CCD02_03_水平基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD02_03_基準線量測_基準線偏移.Value, Color.Lime, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.垂直線段繪製(0, 10000, CCD02_03_垂直基準線量測_AxLineMsr.MeasuredSlope, CCD02_03_垂直基準線量測_AxLineMsr.MeasuredPivotX, CCD02_03_垂直基準線量測_AxLineMsr.MeasuredPivotY, Color.Lime, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.十字中心(水平量測中心, 100, Color.Red, 2, g, ZoomX, ZoomY);
                        if (PLC_Device_CCD02_03_基準線量測_OK.Bool)
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
                    if (this.PLC_Device_CCD02_03_基準線量測_RefreshCanvas.Bool)
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);


                        if (this.plC_CheckBox_CCD02_03_基準線量測_繪製量測框.Checked)
                        {
                            this.CCD02_03_水平基準線量測_AxLineMsr.Title = ("水平基準線");
                            this.CCD02_03_水平基準線量測_AxLineMsr.DrawFrame(HDC, ZoomX, ZoomY, 0, 0);
                            this.CCD02_03_垂直基準線量測_AxLineMsr.Title = ("垂直基準線");
                            this.CCD02_03_垂直基準線量測_AxLineMsr.DrawFrame(HDC, ZoomX, ZoomY, 0, 0);
                        }
                        if (this.plC_CheckBox_CCD02_03_基準線量測_繪製量測線段.Checked)
                        {
                            this.CCD02_03_水平基準線量測_AxLineMsr.DrawFittedPrimitives(HDC, ZoomX, ZoomY, 0, 0);
                            this.CCD02_03_垂直基準線量測_AxLineMsr.DrawFittedPrimitives(HDC, ZoomX, ZoomY, 0, 0);
                            //DrawingClass.Draw.水平線段繪製(0, 10000, CCD02_03_水平基準線量測_AxLineMsr.MeasuredSlope, CCD02_03_水平基準線量測_AxLineMsr.MeasuredPivotX, CCD02_03_水平基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD02_03_基準線量測_基準線偏移.Value, Color.Yellow, 2, g, ZoomX, ZoomY);
                            //DrawingClass.Draw.垂直線段繪製(0, 10000, CCD02_03_垂直基準線量測_AxLineMsr.MeasuredSlope, CCD02_03_垂直基準線量測_AxLineMsr.MeasuredPivotX, CCD02_03_垂直基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD02_03_基準線量測_基準線偏移.Value, Color.Yellow, 2, g, ZoomX, ZoomY);
                        }
                        if (this.plC_CheckBox_CCD02_03_基準線量測_繪製量測點.Checked)
                        {
                            this.CCD02_03_水平基準線量測_AxLineMsr.DrawPoints(HDC, ZoomX, ZoomY, 0, 0);
                            this.CCD02_03_垂直基準線量測_AxLineMsr.DrawPoints(HDC, ZoomX, ZoomY, 0, 0);
                        }
                        DrawingClass.Draw.十字中心(水平量測中心, 100, Color.Red, 2, g, ZoomX, ZoomY);


                        if (PLC_Device_CCD02_03_基準線量測_OK.Bool)
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

            this.PLC_Device_CCD02_03_基準線量測_RefreshCanvas.Bool = false;
        }
        private AxOvkMsr.TxAxLineMsrDragHandle CCD02_03_AxOvkMsr_水平基準線量測_TxAxLineMsrDragHandle = new AxOvkMsr.TxAxLineMsrDragHandle();
        private bool flag_CCD02_03_AxOvkMsr_水平基準線量測_MouseDown = false;
        private AxOvkMsr.TxAxLineMsrDragHandle CCD02_03_AxOvkMsr_垂直基準線量測_TxAxLineMsrDragHandle = new AxOvkMsr.TxAxLineMsrDragHandle();
        private bool flag_CCD02_03_AxOvkMsr_垂直基準線量測_MouseDown = false;

        private void H_Canvas_Tech_CCD02_03_基準線量測_OnCanvasMouseDownEvent(int x, int y, float ZoomX, float ZoomY, ref int InUsedEventNum, int InUsedCanvasHandle)
        {

            if (this.PLC_Device_CCD02_03_基準線量測.Bool)
            {
                this.CCD02_03_AxOvkMsr_水平基準線量測_TxAxLineMsrDragHandle = this.CCD02_03_水平基準線量測_AxLineMsr.HitTest(x, y, ZoomX, ZoomY, 0, 0);
                if (this.CCD02_03_AxOvkMsr_水平基準線量測_TxAxLineMsrDragHandle != AxOvkMsr.TxAxLineMsrDragHandle.AX_LINEMSR_NONE)
                {
                    this.flag_CCD02_03_AxOvkMsr_水平基準線量測_MouseDown = true;
                    InUsedEventNum = 10;
                }

                this.CCD02_03_AxOvkMsr_垂直基準線量測_TxAxLineMsrDragHandle = this.CCD02_03_垂直基準線量測_AxLineMsr.HitTest(x, y, ZoomX, ZoomY, 0, 0);
                if (this.CCD02_03_AxOvkMsr_垂直基準線量測_TxAxLineMsrDragHandle != AxOvkMsr.TxAxLineMsrDragHandle.AX_LINEMSR_NONE)
                {
                    this.flag_CCD02_03_AxOvkMsr_垂直基準線量測_MouseDown = true;
                    InUsedEventNum = 10;
                }
            }

        }
        private void H_Canvas_Tech_CCD02_03_基準線量測_OnCanvasMouseMoveEvent(int x, int y, float ZoomX, float ZoomY)
        {
            if (this.flag_CCD02_03_AxOvkMsr_水平基準線量測_MouseDown)
            {
                this.CCD02_03_水平基準線量測_AxLineMsr.DragFrame(this.CCD02_03_AxOvkMsr_水平基準線量測_TxAxLineMsrDragHandle, x, y, ZoomX, ZoomY, 0, 0);
                this.PLC_Device_CCD02_03_水平基準線量測_量測框起點X座標.Value = CCD02_03_水平基準線量測_AxLineMsr.NLineStartX;
                this.PLC_Device_CCD02_03_水平基準線量測_量測框起點Y座標.Value = CCD02_03_水平基準線量測_AxLineMsr.NLineStartY;
                this.PLC_Device_CCD02_03_水平基準線量測_量測框終點X座標.Value = CCD02_03_水平基準線量測_AxLineMsr.NLineEndX;
                this.PLC_Device_CCD02_03_水平基準線量測_量測框終點Y座標.Value = CCD02_03_水平基準線量測_AxLineMsr.NLineEndY;
                this.PLC_Device_CCD02_03_水平基準線量測_量測高度.Value = CCD02_03_水平基準線量測_AxLineMsr.HalfHeight;
            }

            if (this.flag_CCD02_03_AxOvkMsr_垂直基準線量測_MouseDown)
            {
                this.CCD02_03_垂直基準線量測_AxLineMsr.DragFrame(this.CCD02_03_AxOvkMsr_垂直基準線量測_TxAxLineMsrDragHandle, x, y, ZoomX, ZoomY, 0, 0);
                this.PLC_Device_CCD02_03_垂直基準線量測_量測框起點X座標.Value = CCD02_03_垂直基準線量測_AxLineMsr.NLineStartX;
                this.PLC_Device_CCD02_03_垂直基準線量測_量測框起點Y座標.Value = CCD02_03_垂直基準線量測_AxLineMsr.NLineStartY;
                this.PLC_Device_CCD02_03_垂直基準線量測_量測框終點X座標.Value = CCD02_03_垂直基準線量測_AxLineMsr.NLineEndX;
                this.PLC_Device_CCD02_03_垂直基準線量測_量測框終點Y座標.Value = CCD02_03_垂直基準線量測_AxLineMsr.NLineEndY;
                this.PLC_Device_CCD02_03_垂直基準線量測_量測高度.Value = CCD02_03_垂直基準線量測_AxLineMsr.HalfHeight;
            }


        }
        private void H_Canvas_Tech_CCD02_03_基準線量測_OnCanvasMouseUpEvent(int x, int y, float ZoomX, float ZoomY)
        {
            this.flag_CCD02_03_AxOvkMsr_水平基準線量測_MouseDown = false;
            this.flag_CCD02_03_AxOvkMsr_垂直基準線量測_MouseDown = false;
        }
        int cnt_Program_CCD02_03_基準線量測 = 65534;
        void sub_Program_CCD02_03_基準線量測()
        {
            if (cnt_Program_CCD02_03_基準線量測 == 65534)
            {
                this.h_Canvas_Tech_CCD02_03.OnCanvasMouseDownEvent += H_Canvas_Tech_CCD02_03_基準線量測_OnCanvasMouseDownEvent;
                this.h_Canvas_Tech_CCD02_03.OnCanvasMouseMoveEvent += H_Canvas_Tech_CCD02_03_基準線量測_OnCanvasMouseMoveEvent;
                this.h_Canvas_Tech_CCD02_03.OnCanvasMouseUpEvent += H_Canvas_Tech_CCD02_03_基準線量測_OnCanvasMouseUpEvent;
                this.h_Canvas_Tech_CCD02_03.OnCanvasDrawEvent += H_Canvas_Tech_CCD02_03_基準線量測_OnCanvasDrawEvent;

                this.h_Canvas_Main_CCD02_03_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD02_03_基準線量測_OnCanvasDrawEvent;
                PLC_Device_CCD02_03_基準線量測.SetComment("PLC_CCD02_03_基準線量測");
                PLC_Device_CCD02_03_基準線量測.Bool = false;
                PLC_Device_CCD02_03_基準線量測按鈕.Bool = false;
                cnt_Program_CCD02_03_基準線量測 = 65535;
            }
            if (cnt_Program_CCD02_03_基準線量測 == 65535) cnt_Program_CCD02_03_基準線量測 = 1;
            if (cnt_Program_CCD02_03_基準線量測 == 1) cnt_Program_CCD02_03_基準線量測_檢查按下(ref cnt_Program_CCD02_03_基準線量測);
            if (cnt_Program_CCD02_03_基準線量測 == 2) cnt_Program_CCD02_03_基準線量測_初始化(ref cnt_Program_CCD02_03_基準線量測);
            if (cnt_Program_CCD02_03_基準線量測 == 3) cnt_Program_CCD02_03_基準線量測_開始量測(ref cnt_Program_CCD02_03_基準線量測);
            if (cnt_Program_CCD02_03_基準線量測 == 4) cnt_Program_CCD02_03_基準線量測_兩線交點(ref cnt_Program_CCD02_03_基準線量測);
            if (cnt_Program_CCD02_03_基準線量測 == 5) cnt_Program_CCD02_03_基準線量測_兩線交點量測(ref cnt_Program_CCD02_03_基準線量測);
            if (cnt_Program_CCD02_03_基準線量測 == 6) cnt_Program_CCD02_03_基準線量測_開始繪製(ref cnt_Program_CCD02_03_基準線量測);
            if (cnt_Program_CCD02_03_基準線量測 == 7) cnt_Program_CCD02_03_基準線量測 = 65500;
            if (cnt_Program_CCD02_03_基準線量測 > 1) cnt_Program_CCD02_03_基準線量測_檢查放開(ref cnt_Program_CCD02_03_基準線量測);

            if (cnt_Program_CCD02_03_基準線量測 == 65500)
            {
                if (PLC_Device_CCD02_03_計算一次.Bool)
                {
                    PLC_Device_CCD02_03_基準線量測按鈕.Bool = false;
                }
                PLC_Device_CCD02_03_基準線量測.Bool = false;
                cnt_Program_CCD02_03_基準線量測 = 65535;
            }
        }
        void cnt_Program_CCD02_03_基準線量測_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD02_03_基準線量測按鈕.Bool)
            {
                PLC_Device_CCD02_03_基準線量測.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD02_03_基準線量測_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD02_03_基準線量測.Bool) cnt = 65500;
        }
        void cnt_Program_CCD02_03_基準線量測_初始化(ref int cnt)
        {
            this.PLC_Device_CCD02_03_基準線量測_OK.Bool = false;

            this.CCD02_03_水平基準線量測_AxLineMsr.SrcImageHandle = this.CCD02_03_SrcImageHandle;
            this.CCD02_03_水平基準線量測_AxLineMsr.Hysteresis = PLC_Device_CCD02_03_基準線量測_延伸變化強度.Value;
            this.CCD02_03_水平基準線量測_AxLineMsr.DeriThreshold = PLC_Device_CCD02_03_基準線量測_變化銳利度.Value;
            this.CCD02_03_水平基準線量測_AxLineMsr.MinGreyStep = PLC_Device_CCD02_03_基準線量測_灰階變化面積.Value;
            this.CCD02_03_水平基準線量測_AxLineMsr.SmoothFactor = PLC_Device_CCD02_03_基準線量測_雜訊抑制.Value;
            this.CCD02_03_水平基準線量測_AxLineMsr.HalfProfileThickness = 5;
            this.CCD02_03_水平基準線量測_AxLineMsr.SampleStep = 1;
            this.CCD02_03_水平基準線量測_AxLineMsr.FilterCount = PLC_Device_CCD02_03_基準線量測_最佳回歸線計算次數.Value;
            this.CCD02_03_水平基準線量測_AxLineMsr.FilterThreshold = PLC_Device_CCD02_03_基準線量測_最佳回歸線濾波.Value / 10;

            if (this.PLC_Device_CCD02_03_水平基準線量測_量測框起點X座標.Value == 0 && this.PLC_Device_CCD02_03_水平基準線量測_量測框終點X座標.Value == 0)
            {
                this.PLC_Device_CCD02_03_水平基準線量測_量測框起點X座標.Value = 100;
                this.PLC_Device_CCD02_03_水平基準線量測_量測框終點X座標.Value = 100;
            }
            if (this.PLC_Device_CCD02_03_水平基準線量測_量測框起點Y座標.Value == 0 && this.PLC_Device_CCD02_03_水平基準線量測_量測框終點Y座標.Value == 0)
            {
                this.PLC_Device_CCD02_03_水平基準線量測_量測框起點Y座標.Value = 200;
                this.PLC_Device_CCD02_03_水平基準線量測_量測框終點Y座標.Value = 200;
            }
            if (this.PLC_Device_CCD02_03_水平基準線量測_量測高度.Value == 0)
            {
                this.PLC_Device_CCD02_03_水平基準線量測_量測高度.Value = 100;
            }

            this.CCD02_03_水平基準線量測_AxLineMsr.NLineStartX = PLC_Device_CCD02_03_水平基準線量測_量測框起點X座標.Value;
            this.CCD02_03_水平基準線量測_AxLineMsr.NLineStartY = PLC_Device_CCD02_03_水平基準線量測_量測框起點Y座標.Value;
            this.CCD02_03_水平基準線量測_AxLineMsr.NLineEndX = PLC_Device_CCD02_03_水平基準線量測_量測框終點X座標.Value;
            this.CCD02_03_水平基準線量測_AxLineMsr.NLineEndY = PLC_Device_CCD02_03_水平基準線量測_量測框終點Y座標.Value;
            this.CCD02_03_水平基準線量測_AxLineMsr.HalfHeight = PLC_Device_CCD02_03_水平基準線量測_量測高度.Value;

            this.CCD02_03_水平基準線量測_AxLineMsr.EdgeType = (AxOvkMsr.TxAxTransitionType)PLC_Device_CCD02_03_基準線量測_量測顏色變化.Value;
            this.CCD02_03_水平基準線量測_AxLineMsr.LockedMsrDirection = AxOvkMsr.TxAxLineMsrLockedMsrDirection.AX_LINEMSR_LOCKED_CLOCKWISE;


            this.CCD02_03_垂直基準線量測_AxLineMsr.SrcImageHandle = this.CCD02_03_SrcImageHandle;
            this.CCD02_03_垂直基準線量測_AxLineMsr.Hysteresis = PLC_Device_CCD02_03_基準線量測_延伸變化強度.Value;
            this.CCD02_03_垂直基準線量測_AxLineMsr.DeriThreshold = PLC_Device_CCD02_03_基準線量測_變化銳利度.Value;
            this.CCD02_03_垂直基準線量測_AxLineMsr.MinGreyStep = PLC_Device_CCD02_03_基準線量測_灰階變化面積.Value;
            this.CCD02_03_垂直基準線量測_AxLineMsr.SmoothFactor = PLC_Device_CCD02_03_基準線量測_雜訊抑制.Value;
            this.CCD02_03_垂直基準線量測_AxLineMsr.HalfProfileThickness = 5;
            this.CCD02_03_垂直基準線量測_AxLineMsr.SampleStep = 1;
            this.CCD02_03_垂直基準線量測_AxLineMsr.FilterCount = PLC_Device_CCD02_03_基準線量測_最佳回歸線計算次數.Value;
            this.CCD02_03_垂直基準線量測_AxLineMsr.FilterThreshold = PLC_Device_CCD02_03_基準線量測_最佳回歸線濾波.Value / 10;

            if (this.PLC_Device_CCD02_03_垂直基準線量測_量測框起點X座標.Value == 0 && this.PLC_Device_CCD02_03_垂直基準線量測_量測框終點X座標.Value == 0)
            {
                this.PLC_Device_CCD02_03_垂直基準線量測_量測框起點X座標.Value = 100;
                this.PLC_Device_CCD02_03_垂直基準線量測_量測框終點X座標.Value = 100;
            }
            if (this.PLC_Device_CCD02_03_垂直基準線量測_量測框起點Y座標.Value == 0 && this.PLC_Device_CCD02_03_垂直基準線量測_量測框終點Y座標.Value == 0)
            {
                this.PLC_Device_CCD02_03_垂直基準線量測_量測框起點Y座標.Value = 200;
                this.PLC_Device_CCD02_03_垂直基準線量測_量測框終點Y座標.Value = 200;
            }
            if (this.PLC_Device_CCD02_03_垂直基準線量測_量測高度.Value == 0)
            {
                this.PLC_Device_CCD02_03_垂直基準線量測_量測高度.Value = 100;
            }

            this.CCD02_03_垂直基準線量測_AxLineMsr.NLineStartX = PLC_Device_CCD02_03_垂直基準線量測_量測框起點X座標.Value;
            this.CCD02_03_垂直基準線量測_AxLineMsr.NLineStartY = PLC_Device_CCD02_03_垂直基準線量測_量測框起點Y座標.Value;
            this.CCD02_03_垂直基準線量測_AxLineMsr.NLineEndX = PLC_Device_CCD02_03_垂直基準線量測_量測框終點X座標.Value;
            this.CCD02_03_垂直基準線量測_AxLineMsr.NLineEndY = PLC_Device_CCD02_03_垂直基準線量測_量測框終點Y座標.Value;
            this.CCD02_03_垂直基準線量測_AxLineMsr.HalfHeight = PLC_Device_CCD02_03_垂直基準線量測_量測高度.Value;

            this.CCD02_03_垂直基準線量測_AxLineMsr.EdgeType = (AxOvkMsr.TxAxTransitionType)PLC_Device_CCD02_03_基準線量測_量測顏色變化.Value;
            this.CCD02_03_垂直基準線量測_AxLineMsr.LockedMsrDirection = AxOvkMsr.TxAxLineMsrLockedMsrDirection.AX_LINEMSR_LOCKED_CLOCKWISE;
            cnt++;

        }
        void cnt_Program_CCD02_03_基準線量測_開始量測(ref int cnt)
        {
            if (CCD02_03_SrcImageHandle != 0)
            {
                this.CCD02_03_水平基準線量測_AxLineMsr.DetectPrimitives();
                this.CCD02_03_垂直基準線量測_AxLineMsr.DetectPrimitives();
            }

            if (this.CCD02_03_水平基準線量測_AxLineMsr.LineIsFitted && this.CCD02_03_垂直基準線量測_AxLineMsr.LineIsFitted)
            {

                PointF 水平量測點p1 = new PointF();
                PointF 水平量測點p2 = new PointF();

                CCD02_03_水平基準線量測_AxLineMsr.ValidPointIndex = 0;
                水平量測點p1.X = (int)CCD02_03_水平基準線量測_AxLineMsr.ValidPointX;
                水平量測點p1.Y = (int)CCD02_03_水平基準線量測_AxLineMsr.ValidPointY;
                CCD02_03_水平基準線量測_AxLineMsr.ValidPointIndex = CCD02_03_水平基準線量測_AxLineMsr.ValidPointCount;
                水平量測點p2.X = (int)CCD02_03_水平基準線量測_AxLineMsr.ValidPointX;
                水平量測點p2.Y = (int)CCD02_03_水平基準線量測_AxLineMsr.ValidPointY;
                //Point_CCD02_03_中心基準座標_量測點.X = (int)((水平量測點p1.X + 水平量測點p2.X) / 2);
                //Point_CCD02_03_中心基準座標_量測點.Y = (int)((水平量測點p1.Y + 水平量測點p2.Y) / 2);

                PointF 水平p1 = new PointF();
                PointF 水平p2 = new PointF();
                double 水平confB;
                double 水平Slope = this.CCD02_03_水平基準線量測_AxLineMsr.MeasuredSlope;
                double 水平PivotX = this.CCD02_03_水平基準線量測_AxLineMsr.MeasuredPivotX;
                double 水平PivotY = this.CCD02_03_水平基準線量測_AxLineMsr.MeasuredPivotY;
                水平confB = Conf0Msr(水平Slope, 水平PivotX, 水平PivotY);
                水平p1.X = 1;
                水平p1.Y = (float)FunctionMsr_Y(水平confB, 水平Slope, 水平p1.X);
                水平p2.X = 10000;
                水平p2.Y = (float)FunctionMsr_Y(水平confB, 水平Slope, 水平p2.X);
                水平p1 = new PointF((水平p1.X), (水平p1.Y));
                水平p2 = new PointF((水平p2.X), (水平p2.Y));

                this.CCD02_03_水平基準線量測_AxLineRegression.RegressionOrientation = AxOvkMsr.TxAxLineRegressionOrientation.AX_QUASI_HORIZONTAL_REGRESSION;
                this.CCD02_03_水平基準線量測_AxLineRegression.PointIndex = 0;
                this.CCD02_03_水平基準線量測_AxLineRegression.PointX = 水平p1.X;
                this.CCD02_03_水平基準線量測_AxLineRegression.PointY = 水平p1.Y + (this.PLC_Device_CCD02_03_基準線量測_基準線偏移.Value / 1000) * CCD02_比例尺_mm_To_pixcel;
                this.CCD02_03_水平基準線量測_AxLineRegression.PointIndex = 1;
                this.CCD02_03_水平基準線量測_AxLineRegression.PointX = 水平p2.X;
                this.CCD02_03_水平基準線量測_AxLineRegression.PointY = 水平p2.Y + (this.PLC_Device_CCD02_03_基準線量測_基準線偏移.Value / 1000) * CCD02_比例尺_mm_To_pixcel;
                this.CCD02_03_水平基準線量測_AxLineRegression.DetectPrimitives();

                PointF 垂直p1 = new PointF();
                PointF 垂直p2 = new PointF();
                double 垂直confB;
                double 垂直Slope = this.CCD02_03_垂直基準線量測_AxLineMsr.MeasuredSlope;
                double 垂直PivotX = this.CCD02_03_垂直基準線量測_AxLineMsr.MeasuredPivotX;
                double 垂直PivotY = this.CCD02_03_垂直基準線量測_AxLineMsr.MeasuredPivotY;
                垂直confB = Conf0Msr(垂直Slope, 垂直PivotX, 垂直PivotY);
                垂直p1.X = (float)FunctionMsr_Y(垂直confB, 垂直Slope, 垂直p1.X);
                垂直p1.Y = 1;
                垂直p2.X = (float)FunctionMsr_Y(垂直confB, 垂直Slope, 垂直p2.X);
                垂直p2.Y = 10000;
                垂直p1 = new PointF((垂直p1.X), (垂直p1.Y));
                垂直p2 = new PointF((垂直p2.X), (垂直p2.Y));

                this.CCD02_03_垂直基準線量測_AxLineRegression.RegressionOrientation = AxOvkMsr.TxAxLineRegressionOrientation.AX_QUASI_VERTICAL_REGRESSION;
                this.CCD02_03_垂直基準線量測_AxLineRegression.PointIndex = 0;
                this.CCD02_03_垂直基準線量測_AxLineRegression.PointX = 垂直p1.X;
                this.CCD02_03_垂直基準線量測_AxLineRegression.PointY = 垂直p1.Y;
                this.CCD02_03_垂直基準線量測_AxLineRegression.PointIndex = 1;
                this.CCD02_03_垂直基準線量測_AxLineRegression.PointX = 垂直p2.X;
                this.CCD02_03_垂直基準線量測_AxLineRegression.PointY = 垂直p2.Y;
                this.CCD02_03_垂直基準線量測_AxLineRegression.DetectPrimitives();

                this.PLC_Device_CCD02_03_基準線量測_OK.Bool = true;
            }

            cnt++;
        }
        void cnt_Program_CCD02_03_基準線量測_兩線交點(ref int cnt)
        {
            CCD02_03_基準線量測_AxIntersectionMsr.Line1HorzVert = AxOvkMsr.TxAxLineHorzVert.AX_LINE_QUASI_HORIZONTAL;
            CCD02_03_基準線量測_AxIntersectionMsr.Line1PivotX = CCD02_03_水平基準線量測_AxLineMsr.MeasuredPivotX;
            CCD02_03_基準線量測_AxIntersectionMsr.Line1PivotY = CCD02_03_水平基準線量測_AxLineMsr.MeasuredPivotY;
            CCD02_03_基準線量測_AxIntersectionMsr.Line1Slope = CCD02_03_水平基準線量測_AxLineMsr.MeasuredSlope;

            CCD02_03_基準線量測_AxIntersectionMsr.Line2HorzVert = AxOvkMsr.TxAxLineHorzVert.AX_LINE_QUASI_VERTICAL;
            CCD02_03_基準線量測_AxIntersectionMsr.Line2PivotX = CCD02_03_垂直基準線量測_AxLineMsr.MeasuredPivotX;
            CCD02_03_基準線量測_AxIntersectionMsr.Line2PivotY = CCD02_03_垂直基準線量測_AxLineMsr.MeasuredPivotY;
            CCD02_03_基準線量測_AxIntersectionMsr.Line2Slope = CCD02_03_垂直基準線量測_AxLineMsr.MeasuredSlope;

            CCD02_03_基準線量測_AxIntersectionMsr.FindIntersection();

            cnt++;
        }
        void cnt_Program_CCD02_03_基準線量測_兩線交點量測(ref int cnt)
        {
            Point_CCD02_03_中心基準座標_量測點.X = (float)CCD02_03_基準線量測_AxIntersectionMsr.IntersectionX;
            Point_CCD02_03_中心基準座標_量測點.Y = (float)CCD02_03_基準線量測_AxIntersectionMsr.IntersectionY;

            if (!PLC_Device_CCD02_03_計算一次.Bool)
            {
                PLC_Device_CCD02_03_水平基準線量測_量測中心_X.Value = (int)CCD02_03_基準線量測_AxIntersectionMsr.IntersectionX;
                PLC_Device_CCD02_03_水平基準線量測_量測中心_Y.Value = (int)CCD02_03_基準線量測_AxIntersectionMsr.IntersectionY;
                //PLC_Device_CCD02_03_水平基準線量測_量測中心_X.Value = 2199;
                //PLC_Device_CCD02_03_水平基準線量測_量測中心_Y.Value = 1175;
            }

            cnt++;
        }
        void cnt_Program_CCD02_03_基準線量測_開始繪製(ref int cnt)
        {

            this.PLC_Device_CCD02_03_基準線量測_RefreshCanvas.Bool = true;
            if (this.PLC_Device_CCD02_03_基準線量測按鈕.Bool && !PLC_Device_CCD02_03_計算一次.Bool)
            {
                this.h_Canvas_Tech_CCD02_03.RefreshCanvas();
            }
            cnt++;
        }

        #endregion
        #region PLC_CCD02_03_PIN量測_量測框調整
        MyTimer MyTimer_CCD02_03_PIN量測_量測框調整 = new MyTimer();
        PLC_Device PLC_Device_CCD02_03_PIN量測_量測框調整按鈕 = new PLC_Device("S6580");
        PLC_Device PLC_Device_CCD02_03_PIN量測_量測框調整 = new PLC_Device("S6581");
        PLC_Device PLC_Device_CCD02_03_PIN量測_量測框調整_OK = new PLC_Device("S6582");
        PLC_Device PLC_Device_CCD02_03_PIN量測_量測框調整_測試完成 = new PLC_Device("S6583");
        PLC_Device PLC_Device_CCD02_03_PIN量測_量測框調整_RefreshCanvas = new PLC_Device("S6584");
        PLC_Device PLC_Device_CCD02_03_PIN量測_有無量測不測試 = new PLC_Device("S6585");
        private List<AxOvkBase.AxROIBW8> List_CCD02_03_PIN量測_AxROIBW8_量測框調整 = new List<AxOvkBase.AxROIBW8>();
        private List<AxOvkBlob.AxObject> List_CCD02_03_PIN量測_AxObject_區塊分析 = new List<AxOvkBlob.AxObject>();
        private AxOvkPat.AxVisionInspectionFrame CCD02_03_PIN量測_AxVisionInspectionFrame_量測框調整;

        private List<PLC_Device> List_PLC_Device_CCD02_03_PIN量測參數_灰階門檻值 = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD02_03_PIN量測參數_OrgX = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD02_03_PIN量測參數_OrgY = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD02_03_PIN量測參數_Width = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD02_03_PIN量測參數_Height = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD02_03_PIN量測參數_面積上限 = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD02_03_PIN量測參數_面積下限 = new List<PLC_Device>();
        private PointF[] List_CCD02_03_PIN量測參數_量測點 = new PointF[9];
        private PointF[] List_CCD02_03_PIN量測參數_量測點_結果 = new PointF[9];
        private Point[] List_CCD02_03_PIN量測參數_量測點_轉換後座標 = new Point[9];
        private bool[] List_CCD02_03_PIN量測參數_量測點_有無 = new bool[9];
        #region 灰階門檻值
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_灰階門檻值_PIN01 = new PLC_Device("F7300");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_灰階門檻值_PIN02 = new PLC_Device("F7301");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_灰階門檻值_PIN03 = new PLC_Device("F7302");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_灰階門檻值_PIN04 = new PLC_Device("F7303");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_灰階門檻值_PIN05 = new PLC_Device("F7304");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_灰階門檻值_PIN06 = new PLC_Device("F7305");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_灰階門檻值_PIN07 = new PLC_Device("F7306");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_灰階門檻值_PIN08 = new PLC_Device("F7307");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_灰階門檻值_PIN09 = new PLC_Device("F7308");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_灰階門檻值_PIN10 = new PLC_Device("F7309");
        #endregion
        #region OrgX
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_OrgX_PIN01 = new PLC_Device("F7310");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_OrgX_PIN02 = new PLC_Device("F7311");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_OrgX_PIN03 = new PLC_Device("F7312");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_OrgX_PIN04 = new PLC_Device("F7313");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_OrgX_PIN05 = new PLC_Device("F7314");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_OrgX_PIN06 = new PLC_Device("F7315");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_OrgX_PIN07 = new PLC_Device("F7316");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_OrgX_PIN08 = new PLC_Device("F7317");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_OrgX_PIN09 = new PLC_Device("F7318");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_OrgX_PIN10 = new PLC_Device("F7319");
        #endregion
        #region OrgY
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_OrgY_PIN01 = new PLC_Device("F7320");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_OrgY_PIN02 = new PLC_Device("F7321");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_OrgY_PIN03 = new PLC_Device("F7322");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_OrgY_PIN04 = new PLC_Device("F7323");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_OrgY_PIN05 = new PLC_Device("F7324");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_OrgY_PIN06 = new PLC_Device("F7325");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_OrgY_PIN07 = new PLC_Device("F7326");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_OrgY_PIN08 = new PLC_Device("F7327");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_OrgY_PIN09 = new PLC_Device("F7328");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_OrgY_PIN10 = new PLC_Device("F7329");
        #endregion
        #region Width
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_Width_PIN01 = new PLC_Device("F7330");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_Width_PIN02 = new PLC_Device("F7331");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_Width_PIN03 = new PLC_Device("F7332");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_Width_PIN04 = new PLC_Device("F7333");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_Width_PIN05 = new PLC_Device("F7334");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_Width_PIN06 = new PLC_Device("F7335");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_Width_PIN07 = new PLC_Device("F7336");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_Width_PIN08 = new PLC_Device("F7337");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_Width_PIN09 = new PLC_Device("F7338");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_Width_PIN10 = new PLC_Device("F7339");
        #endregion
        #region Height
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_Height_PIN01 = new PLC_Device("F7340");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_Height_PIN02 = new PLC_Device("F7341");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_Height_PIN03 = new PLC_Device("F7342");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_Height_PIN04 = new PLC_Device("F7343");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_Height_PIN05 = new PLC_Device("F7344");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_Height_PIN06 = new PLC_Device("F7345");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_Height_PIN07 = new PLC_Device("F7346");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_Height_PIN08 = new PLC_Device("F7347");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_Height_PIN09 = new PLC_Device("F7348");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_Height_PIN10 = new PLC_Device("F7349");
        #endregion
        #region 面積上限

        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_面積上限_PIN01 = new PLC_Device("F7350");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_面積上限_PIN02 = new PLC_Device("F7351");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_面積上限_PIN03 = new PLC_Device("F7352");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_面積上限_PIN04 = new PLC_Device("F7353");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_面積上限_PIN05 = new PLC_Device("F7354");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_面積上限_PIN06 = new PLC_Device("F7355");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_面積上限_PIN07 = new PLC_Device("F7356");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_面積上限_PIN08 = new PLC_Device("F7357");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_面積上限_PIN09 = new PLC_Device("F7358");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_面積上限_PIN10 = new PLC_Device("F7359");
        #endregion
        #region 面積下限

        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_面積下限_PIN01 = new PLC_Device("F7360");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_面積下限_PIN02 = new PLC_Device("F7361");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_面積下限_PIN03 = new PLC_Device("F7362");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_面積下限_PIN04 = new PLC_Device("F7363");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_面積下限_PIN05 = new PLC_Device("F7364");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_面積下限_PIN06 = new PLC_Device("F7365");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_面積下限_PIN07 = new PLC_Device("F7366");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_面積下限_PIN08 = new PLC_Device("F7367");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_面積下限_PIN09 = new PLC_Device("F7368");
        private PLC_Device PLC_Device_CCD02_03_PIN量測參數_面積下限_PIN10 = new PLC_Device("F7369");
        #endregion
        AxOvkBase.TxAxHitHandle[] CCD02_03_PIN量測_AxROIBW8_TxAxHitHandle = new AxOvkBase.TxAxHitHandle[9];
        bool[] flag_CCD02_03_PIN量測_AxROIBW8_MouseDown = new bool[9];
        private void H_Canvas_Tech_CCD02_03_PIN量測_量測框調整_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {

            if (PLC_Device_CCD02_03_Main_取像並檢驗.Bool || PLC_Device_CCD02_03_PLC觸發檢測.Bool || PLC_Device_CCD02_03_Main_檢驗一次.Bool)
            {
                try
                {
                    Graphics g = Graphics.FromHdc((IntPtr)HDC);
                    for (int i = 0; i < this.List_CCD02_03_PIN量測參數_量測點.Length; i++)
                    {
                        DrawingClass.Draw.十字中心(this.List_CCD02_03_PIN量測參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
                    }
                    g.Dispose();
                    g = null;
                }
                catch
                {

                }

            }
            else if (PLC_Device_CCD02_03_Tech_檢驗一次.Bool || PLC_Device_CCD02_03_Tech_取像並檢驗.Bool)
            {
                if (this.PLC_Device_CCD02_03_PIN量測_量測框調整_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        for (int i = 0; i < this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整.Count; i++)
                        {

                            this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].Title = string.Format("PIN" + "{0}", (i + 1).ToString("00"));
                            this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].ShowTitle = true;
                            this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].ShowPlacement = false;
                            this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].DrawRect(HDC, ZoomX, ZoomY, 0, 0, 0x0000FF);
                        }
                        for (int i = 0; i < this.List_CCD02_03_PIN量測參數_量測點.Length; i++)
                        {
                            DrawingClass.Draw.十字中心(this.List_CCD02_03_PIN量測參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
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
                if (this.PLC_Device_CCD02_03_PIN量測_量測框調整_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        PointF po_str_PIN到基準Y = new PointF(200, 250);
                        Font font = new Font("微軟正黑體", 10);

                        if (this.plC_CheckBox_CCD02_03_PIN量測_繪製量測框.Checked)
                        {
                            for (int i = 0; i < this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整.Count; i++)
                            {

                                this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].Title = string.Format("PIN" + "{0}", (i + 1).ToString("00"));
                                this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].ShowTitle = true;
                                this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].ShowPlacement = false;
                                this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].DrawFrame(HDC, ZoomX, ZoomY, 0, 0, 0x0000FF);
                            }
                        }
                        if (this.plC_CheckBox_CCD02_03_PIN量測_繪製量測區塊.Checked)
                        {
                            for (int i = 0; i < this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整.Count; i++)
                            {
                                this.List_CCD02_03_PIN量測_AxObject_區塊分析[i].DrawBlobs(HDC, -1, ZoomX, ZoomY, 0, 0, true, -1);
                            }

                        }
                        for (int i = 0; i < this.List_CCD02_03_PIN量測參數_量測點.Length; i++)
                        {
                            DrawingClass.Draw.十字中心(this.List_CCD02_03_PIN量測參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
                        }
                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }
            }

            this.PLC_Device_CCD02_03_PIN量測_量測框調整_RefreshCanvas.Bool = false;
        }
        private void H_Canvas_Tech_CCD02_03_PIN量測_量測框調整_OnCanvasMouseDownEvent(int x, int y, float ZoomX, float ZoomY, ref int InUsedEventNum, int InUsedCanvasHandle)
        {
            if (this.PLC_Device_CCD02_03_PIN量測_量測框調整.Bool)
            {
                for (int i = 0; i < this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整.Count; i++)
                {
                    this.CCD02_03_PIN量測_AxROIBW8_TxAxHitHandle[i] = this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].HitTest(x, y, ZoomX, ZoomY, 0, 0);
                    if (this.CCD02_03_PIN量測_AxROIBW8_TxAxHitHandle[i] != AxOvkBase.TxAxHitHandle.AX_HANDLE_NONE)
                    {
                        this.flag_CCD02_03_PIN量測_AxROIBW8_MouseDown[i] = true;
                        InUsedEventNum = 10;
                        return;
                    }
                }

            }

        }
        private void H_Canvas_Tech_CCD02_03_PIN量測_量測框調整_OnCanvasMouseMoveEvent(int x, int y, float ZoomX, float ZoomY)
        {
            for (int i = 0; i < this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {
                if (this.flag_CCD02_03_PIN量測_AxROIBW8_MouseDown[i])
                {
                    this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].DragROI(this.CCD02_03_PIN量測_AxROIBW8_TxAxHitHandle[i], x, y, ZoomX, ZoomY, 0, 0);
                    this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX[i].Value = this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].OrgX;
                    this.List_PLC_Device_CCD02_03_PIN量測參數_OrgY[i].Value = this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].OrgY;
                    this.List_PLC_Device_CCD02_03_PIN量測參數_Width[i].Value = this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].ROIWidth;
                    this.List_PLC_Device_CCD02_03_PIN量測參數_Height[i].Value = this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].ROIHeight;
                }
            }

        }
        private void H_Canvas_Tech_CCD02_03_PIN量測_量測框調整_OnCanvasMouseUpEvent(int x, int y, float ZoomX, float ZoomY)
        {
            for (int i = 0; i < this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {
                this.flag_CCD02_03_PIN量測_AxROIBW8_MouseDown[i] = false;
            }
        }

        int cnt_Program_CCD02_03_PIN量測_量測框調整 = 65534;
        void sub_Program_CCD02_03_PIN量測_量測框調整()
        {
            if (cnt_Program_CCD02_03_PIN量測_量測框調整 == 65534)
            {
                this.h_Canvas_Tech_CCD02_03.OnCanvasDrawEvent += H_Canvas_Tech_CCD02_03_PIN量測_量測框調整_OnCanvasDrawEvent;
                this.h_Canvas_Tech_CCD02_03.OnCanvasMouseDownEvent += H_Canvas_Tech_CCD02_03_PIN量測_量測框調整_OnCanvasMouseDownEvent;
                this.h_Canvas_Tech_CCD02_03.OnCanvasMouseMoveEvent += H_Canvas_Tech_CCD02_03_PIN量測_量測框調整_OnCanvasMouseMoveEvent;
                this.h_Canvas_Tech_CCD02_03.OnCanvasMouseUpEvent += H_Canvas_Tech_CCD02_03_PIN量測_量測框調整_OnCanvasMouseUpEvent;

                this.h_Canvas_Main_CCD02_03_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD02_03_PIN量測_量測框調整_OnCanvasDrawEvent;

                #region 灰階門檻值
                this.List_PLC_Device_CCD02_03_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_03_PIN量測參數_灰階門檻值_PIN01);
                this.List_PLC_Device_CCD02_03_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_03_PIN量測參數_灰階門檻值_PIN02);
                this.List_PLC_Device_CCD02_03_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_03_PIN量測參數_灰階門檻值_PIN03);
                this.List_PLC_Device_CCD02_03_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_03_PIN量測參數_灰階門檻值_PIN04);
                this.List_PLC_Device_CCD02_03_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_03_PIN量測參數_灰階門檻值_PIN05);
                this.List_PLC_Device_CCD02_03_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_03_PIN量測參數_灰階門檻值_PIN06);
                this.List_PLC_Device_CCD02_03_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_03_PIN量測參數_灰階門檻值_PIN07);
                this.List_PLC_Device_CCD02_03_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_03_PIN量測參數_灰階門檻值_PIN08);
                this.List_PLC_Device_CCD02_03_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_03_PIN量測參數_灰階門檻值_PIN09);
                this.List_PLC_Device_CCD02_03_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD02_03_PIN量測參數_灰階門檻值_PIN10);
                #endregion
                #region OrgX
                this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_03_PIN量測參數_OrgX_PIN01);
                this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_03_PIN量測參數_OrgX_PIN02);
                this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_03_PIN量測參數_OrgX_PIN03);
                this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_03_PIN量測參數_OrgX_PIN04);
                this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_03_PIN量測參數_OrgX_PIN05);
                this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_03_PIN量測參數_OrgX_PIN06);
                this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_03_PIN量測參數_OrgX_PIN07);
                this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_03_PIN量測參數_OrgX_PIN08);
                this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_03_PIN量測參數_OrgX_PIN09);
                this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX.Add(this.PLC_Device_CCD02_03_PIN量測參數_OrgX_PIN10);
                #endregion
                #region OrgY
                this.List_PLC_Device_CCD02_03_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_03_PIN量測參數_OrgY_PIN01);
                this.List_PLC_Device_CCD02_03_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_03_PIN量測參數_OrgY_PIN02);
                this.List_PLC_Device_CCD02_03_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_03_PIN量測參數_OrgY_PIN03);
                this.List_PLC_Device_CCD02_03_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_03_PIN量測參數_OrgY_PIN04);
                this.List_PLC_Device_CCD02_03_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_03_PIN量測參數_OrgY_PIN05);
                this.List_PLC_Device_CCD02_03_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_03_PIN量測參數_OrgY_PIN06);
                this.List_PLC_Device_CCD02_03_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_03_PIN量測參數_OrgY_PIN07);
                this.List_PLC_Device_CCD02_03_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_03_PIN量測參數_OrgY_PIN08);
                this.List_PLC_Device_CCD02_03_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_03_PIN量測參數_OrgY_PIN09);
                this.List_PLC_Device_CCD02_03_PIN量測參數_OrgY.Add(this.PLC_Device_CCD02_03_PIN量測參數_OrgY_PIN10);
                #endregion
                #region Width
                this.List_PLC_Device_CCD02_03_PIN量測參數_Width.Add(this.PLC_Device_CCD02_03_PIN量測參數_Width_PIN01);
                this.List_PLC_Device_CCD02_03_PIN量測參數_Width.Add(this.PLC_Device_CCD02_03_PIN量測參數_Width_PIN02);
                this.List_PLC_Device_CCD02_03_PIN量測參數_Width.Add(this.PLC_Device_CCD02_03_PIN量測參數_Width_PIN03);
                this.List_PLC_Device_CCD02_03_PIN量測參數_Width.Add(this.PLC_Device_CCD02_03_PIN量測參數_Width_PIN04);
                this.List_PLC_Device_CCD02_03_PIN量測參數_Width.Add(this.PLC_Device_CCD02_03_PIN量測參數_Width_PIN05);
                this.List_PLC_Device_CCD02_03_PIN量測參數_Width.Add(this.PLC_Device_CCD02_03_PIN量測參數_Width_PIN06);
                this.List_PLC_Device_CCD02_03_PIN量測參數_Width.Add(this.PLC_Device_CCD02_03_PIN量測參數_Width_PIN07);
                this.List_PLC_Device_CCD02_03_PIN量測參數_Width.Add(this.PLC_Device_CCD02_03_PIN量測參數_Width_PIN08);
                this.List_PLC_Device_CCD02_03_PIN量測參數_Width.Add(this.PLC_Device_CCD02_03_PIN量測參數_Width_PIN09);
                this.List_PLC_Device_CCD02_03_PIN量測參數_Width.Add(this.PLC_Device_CCD02_03_PIN量測參數_Width_PIN10);
                #endregion
                #region Height
                this.List_PLC_Device_CCD02_03_PIN量測參數_Height.Add(this.PLC_Device_CCD02_03_PIN量測參數_Height_PIN01);
                this.List_PLC_Device_CCD02_03_PIN量測參數_Height.Add(this.PLC_Device_CCD02_03_PIN量測參數_Height_PIN02);
                this.List_PLC_Device_CCD02_03_PIN量測參數_Height.Add(this.PLC_Device_CCD02_03_PIN量測參數_Height_PIN03);
                this.List_PLC_Device_CCD02_03_PIN量測參數_Height.Add(this.PLC_Device_CCD02_03_PIN量測參數_Height_PIN04);
                this.List_PLC_Device_CCD02_03_PIN量測參數_Height.Add(this.PLC_Device_CCD02_03_PIN量測參數_Height_PIN05);
                this.List_PLC_Device_CCD02_03_PIN量測參數_Height.Add(this.PLC_Device_CCD02_03_PIN量測參數_Height_PIN06);
                this.List_PLC_Device_CCD02_03_PIN量測參數_Height.Add(this.PLC_Device_CCD02_03_PIN量測參數_Height_PIN07);
                this.List_PLC_Device_CCD02_03_PIN量測參數_Height.Add(this.PLC_Device_CCD02_03_PIN量測參數_Height_PIN08);
                this.List_PLC_Device_CCD02_03_PIN量測參數_Height.Add(this.PLC_Device_CCD02_03_PIN量測參數_Height_PIN09);
                this.List_PLC_Device_CCD02_03_PIN量測參數_Height.Add(this.PLC_Device_CCD02_03_PIN量測參數_Height_PIN10);
                #endregion
                #region 面積上限
                this.List_PLC_Device_CCD02_03_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_03_PIN量測參數_面積上限_PIN01);
                this.List_PLC_Device_CCD02_03_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_03_PIN量測參數_面積上限_PIN02);
                this.List_PLC_Device_CCD02_03_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_03_PIN量測參數_面積上限_PIN03);
                this.List_PLC_Device_CCD02_03_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_03_PIN量測參數_面積上限_PIN04);
                this.List_PLC_Device_CCD02_03_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_03_PIN量測參數_面積上限_PIN05);
                this.List_PLC_Device_CCD02_03_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_03_PIN量測參數_面積上限_PIN06);
                this.List_PLC_Device_CCD02_03_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_03_PIN量測參數_面積上限_PIN07);
                this.List_PLC_Device_CCD02_03_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_03_PIN量測參數_面積上限_PIN08);
                this.List_PLC_Device_CCD02_03_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_03_PIN量測參數_面積上限_PIN09);
                this.List_PLC_Device_CCD02_03_PIN量測參數_面積上限.Add(this.PLC_Device_CCD02_03_PIN量測參數_面積上限_PIN10);
                #endregion
                #region 面積下限
                this.List_PLC_Device_CCD02_03_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_03_PIN量測參數_面積下限_PIN01);
                this.List_PLC_Device_CCD02_03_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_03_PIN量測參數_面積下限_PIN02);
                this.List_PLC_Device_CCD02_03_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_03_PIN量測參數_面積下限_PIN03);
                this.List_PLC_Device_CCD02_03_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_03_PIN量測參數_面積下限_PIN04);
                this.List_PLC_Device_CCD02_03_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_03_PIN量測參數_面積下限_PIN05);
                this.List_PLC_Device_CCD02_03_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_03_PIN量測參數_面積下限_PIN06);
                this.List_PLC_Device_CCD02_03_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_03_PIN量測參數_面積下限_PIN07);
                this.List_PLC_Device_CCD02_03_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_03_PIN量測參數_面積下限_PIN08);
                this.List_PLC_Device_CCD02_03_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_03_PIN量測參數_面積下限_PIN09);
                this.List_PLC_Device_CCD02_03_PIN量測參數_面積下限.Add(this.PLC_Device_CCD02_03_PIN量測參數_面積下限_PIN10);
                #endregion
                for (int i = 0; i < 9; i++)
                {
                    if (this.List_PLC_Device_CCD02_03_PIN量測參數_灰階門檻值[i].Value == 0) this.List_PLC_Device_CCD02_03_PIN量測參數_灰階門檻值[i].Value = 200;
                    if (this.List_PLC_Device_CCD02_03_PIN量測參數_Height[i].Value == 0) this.List_PLC_Device_CCD02_03_PIN量測參數_Height[i].Value = 100;
                    if (this.List_PLC_Device_CCD02_03_PIN量測參數_Width[i].Value == 0) this.List_PLC_Device_CCD02_03_PIN量測參數_Width[i].Value = 100;
                    if (this.List_PLC_Device_CCD02_03_PIN量測參數_Height[i].Value > 500) this.List_PLC_Device_CCD02_03_PIN量測參數_Height[i].Value = 500;
                    if (this.List_PLC_Device_CCD02_03_PIN量測參數_Width[i].Value > 500) this.List_PLC_Device_CCD02_03_PIN量測參數_Width[i].Value = 500;
                }
                PLC_Device_CCD02_03_PIN量測_量測框調整.SetComment("PLC_CCD02_03_PIN量測_量測框調整");
                PLC_Device_CCD02_03_PIN量測_量測框調整按鈕.Bool = false;
                PLC_Device_CCD02_03_PIN量測_量測框調整.Bool = false;
                cnt_Program_CCD02_03_PIN量測_量測框調整 = 65535;
            }
            if (cnt_Program_CCD02_03_PIN量測_量測框調整 == 65535) cnt_Program_CCD02_03_PIN量測_量測框調整 = 1;
            if (cnt_Program_CCD02_03_PIN量測_量測框調整 == 1) cnt_Program_CCD02_03_PIN量測_量測框調整_觸發按下(ref cnt_Program_CCD02_03_PIN量測_量測框調整);
            if (cnt_Program_CCD02_03_PIN量測_量測框調整 == 2) cnt_Program_CCD02_03_PIN量測_量測框調整_檢查按下(ref cnt_Program_CCD02_03_PIN量測_量測框調整);
            if (cnt_Program_CCD02_03_PIN量測_量測框調整 == 3) cnt_Program_CCD02_03_PIN量測_量測框調整_初始化(ref cnt_Program_CCD02_03_PIN量測_量測框調整);
            if (cnt_Program_CCD02_03_PIN量測_量測框調整 == 4) cnt_Program_CCD02_03_PIN量測_量測框調整_座標轉換(ref cnt_Program_CCD02_03_PIN量測_量測框調整);
            if (cnt_Program_CCD02_03_PIN量測_量測框調整 == 5) cnt_Program_CCD02_03_PIN量測_量測框調整_讀取參數(ref cnt_Program_CCD02_03_PIN量測_量測框調整);
            if (cnt_Program_CCD02_03_PIN量測_量測框調整 == 6) cnt_Program_CCD02_03_PIN量測_量測框調整_開始區塊分析(ref cnt_Program_CCD02_03_PIN量測_量測框調整);
            if (cnt_Program_CCD02_03_PIN量測_量測框調整 == 7) cnt_Program_CCD02_03_PIN量測_量測框調整_繪製畫布(ref cnt_Program_CCD02_03_PIN量測_量測框調整);
            if (cnt_Program_CCD02_03_PIN量測_量測框調整 == 8) cnt_Program_CCD02_03_PIN量測_量測框調整 = 65500;
            if (cnt_Program_CCD02_03_PIN量測_量測框調整 > 1) cnt_Program_CCD02_03_PIN量測_量測框調整_檢查放開(ref cnt_Program_CCD02_03_PIN量測_量測框調整);

            if (cnt_Program_CCD02_03_PIN量測_量測框調整 == 65500)
            {
                cnt_Program_CCD02_03_PIN量測_量測框調整 = 65535;
            }
        }
        void cnt_Program_CCD02_03_PIN量測_量測框調整_觸發按下(ref int cnt)
        {
            if (PLC_Device_CCD02_03_PIN量測_量測框調整按鈕.Bool || PLC_Device_CCD02_03_計算一次.Bool)
            {
                PLC_Device_CCD02_03_PIN量測_量測框調整.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD02_03_PIN量測_量測框調整_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD02_03_PIN量測_量測框調整.Bool) cnt++;
        }
        void cnt_Program_CCD02_03_PIN量測_量測框調整_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD02_03_PIN量測_量測框調整按鈕.Bool)
            {
                PLC_Device_CCD02_03_PIN量測_量測框調整.Bool = false;
                cnt = 65500;
            }
        }
        void cnt_Program_CCD02_03_PIN量測_量測框調整_初始化(ref int cnt)
        {
            this.MyTimer_CCD02_03_PIN量測_量測框調整.TickStop();
            this.MyTimer_CCD02_03_PIN量測_量測框調整.StartTickTime(99999);
            this.List_CCD02_03_PIN量測參數_量測點 = new PointF[9];
            this.List_CCD02_03_PIN量測參數_量測點_結果 = new PointF[9];
            this.List_CCD02_03_PIN量測參數_量測點_轉換後座標 = new Point[9];
            this.List_CCD02_03_PIN量測參數_量測點_有無 = new bool[9];
            cnt++;
        }
        void cnt_Program_CCD02_03_PIN量測_量測框調整_座標轉換(ref int cnt)
        {
            if (PLC_Device_CCD02_03_計算一次.Bool)
            {
                CCD02_03_PIN量測_AxVisionInspectionFrame_量測框調整.RefPointX = PLC_Device_CCD02_03_水平基準線量測_量測中心_X.Value;
                CCD02_03_PIN量測_AxVisionInspectionFrame_量測框調整.RefPointY = PLC_Device_CCD02_03_水平基準線量測_量測中心_Y.Value;
                CCD02_03_PIN量測_AxVisionInspectionFrame_量測框調整.RefAngle = 0;
                CCD02_03_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentRefPointX = Point_CCD02_03_中心基準座標_量測點.X;
                CCD02_03_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentRefPointY = Point_CCD02_03_中心基準座標_量測點.Y;
                CCD02_03_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentRefAngle = 0;
                CCD02_03_PIN量測_AxVisionInspectionFrame_量測框調整.NumOfVisionPoints = 10;

                for (int j = 0; j < 9; j++)
                {
                    if (this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX[j].Value == 0) this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX[j].Value = 100;
                    if (this.List_PLC_Device_CCD02_03_PIN量測參數_OrgY[j].Value == 0) this.List_PLC_Device_CCD02_03_PIN量測參數_OrgY[j].Value = 100;
                    if (this.List_PLC_Device_CCD02_03_PIN量測參數_Width[j].Value == 0) this.List_PLC_Device_CCD02_03_PIN量測參數_Width[j].Value = 100;
                    if (this.List_PLC_Device_CCD02_03_PIN量測參數_Height[j].Value == 0) this.List_PLC_Device_CCD02_03_PIN量測參數_Height[j].Value = 100;

                    CCD02_03_PIN量測_AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
                    CCD02_03_PIN量測_AxVisionInspectionFrame_量測框調整.VisionPointX = this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX[j].Value;
                    CCD02_03_PIN量測_AxVisionInspectionFrame_量測框調整.VisionPointY = this.List_PLC_Device_CCD02_03_PIN量測參數_OrgY[j].Value;
                }
                CCD02_03_PIN量測_AxVisionInspectionFrame_量測框調整.EstimateCurrentVisionPoints();
                for (int j = 0; j < 9; j++)
                {
                    CCD02_03_PIN量測_AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
                    List_CCD02_03_PIN量測參數_量測點_轉換後座標[j].X = (int)CCD02_03_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentVisionPointX;
                    List_CCD02_03_PIN量測參數_量測點_轉換後座標[j].Y = (int)CCD02_03_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentVisionPointY;
                }
            }
            cnt++;

        }
        void cnt_Program_CCD02_03_PIN量測_量測框調整_讀取參數(ref int cnt)
        {
            for (int i = 0; i < this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {
                if (this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX[i].Value > 2596) this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX[i].Value = 0;
                if (this.List_PLC_Device_CCD02_03_PIN量測參數_OrgY[i].Value > 1922) this.List_PLC_Device_CCD02_03_PIN量測參數_OrgY[i].Value = 0;
                if (this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX[i].Value < 0) this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX[i].Value = 0;
                if (this.List_PLC_Device_CCD02_03_PIN量測參數_OrgY[i].Value < 0) this.List_PLC_Device_CCD02_03_PIN量測參數_OrgY[i].Value = 0;

                if (this.List_CCD02_03_PIN量測參數_量測點_轉換後座標[i].X > 2596) this.List_CCD02_03_PIN量測參數_量測點_轉換後座標[i].X = 0;
                if (this.List_CCD02_03_PIN量測參數_量測點_轉換後座標[i].Y > 1922) this.List_CCD02_03_PIN量測參數_量測點_轉換後座標[i].Y = 0;
                if (this.List_CCD02_03_PIN量測參數_量測點_轉換後座標[i].X < 0) this.List_CCD02_03_PIN量測參數_量測點_轉換後座標[i].X = 0;
                if (this.List_CCD02_03_PIN量測參數_量測點_轉換後座標[i].Y < 0) this.List_CCD02_03_PIN量測參數_量測點_轉換後座標[i].Y = 0;
            }
            for (int i = 0; i < this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {
                this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].ParentHandle = this.CCD02_03_SrcImageHandle;
                if (PLC_Device_CCD02_03_計算一次.Bool)
                {
                    this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].OrgX = List_CCD02_03_PIN量測參數_量測點_轉換後座標[i].X;
                    this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].OrgY = List_CCD02_03_PIN量測參數_量測點_轉換後座標[i].Y;
                }
                else
                {
                    this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].OrgX = this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX[i].Value;
                    this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].OrgY = this.List_PLC_Device_CCD02_03_PIN量測參數_OrgY[i].Value;
                }
                this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].ROIWidth = this.List_PLC_Device_CCD02_03_PIN量測參數_Width[i].Value;
                this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].ROIHeight = this.List_PLC_Device_CCD02_03_PIN量測參數_Height[i].Value;
                this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].SkewAngle = 0;
            }
            cnt++;
        }
        void cnt_Program_CCD02_03_PIN量測_量測框調整_開始區塊分析(ref int cnt)
        {
            uint object_value = 4294963615;

            for (int i = 0; i < this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {

                this.List_CCD02_03_PIN量測_AxObject_區塊分析[i].SrcImageHandle = this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].VegaHandle;
                this.List_CCD02_03_PIN量測_AxObject_區塊分析[i].ObjectClass = AxOvkBlob.TxAxObjClass.AX_OBJECT_DETECT_LIGHTER_CLASS;
                this.List_CCD02_03_PIN量測_AxObject_區塊分析[i].HighThreshold = List_PLC_Device_CCD02_03_PIN量測參數_灰階門檻值[0].Value;
                if (this.CCD02_03_SrcImageHandle != 0)
                {
                    if (this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX[i].Value + this.List_PLC_Device_CCD02_03_PIN量測參數_Width[i].Value < 2596 &&
                        this.List_PLC_Device_CCD02_03_PIN量測參數_OrgY[i].Value + this.List_PLC_Device_CCD02_03_PIN量測參數_Height[i].Value < 1922 &&
                        this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX[i].Value > 0 && this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX[i].Value > 0)
                    {
                        this.List_CCD02_03_PIN量測_AxObject_區塊分析[i].BlobAnalyze(false);
                    }


                }
                this.List_CCD02_03_PIN量測_AxObject_區塊分析[i].CalculateFeatures((int)object_value, -1);
                this.List_CCD02_03_PIN量測_AxObject_區塊分析[i].SortObjects(AxOvkBlob.TxAxObjFeatureSortOrder.AX_OBJECT_SORT_ORDER_LARGE_TO_SMALL, AxOvkBlob.TxAxObjFeature.AX_OBJECT_FEATURE_AREA, 0, -1);
                this.List_CCD02_03_PIN量測_AxObject_區塊分析[i].SelectObjects(AxOvkBlob.TxAxObjFeature.AX_OBJECT_FEATURE_AREA, AxOvkBlob.TxAxObjFeatureOperation.AX_OBJECT_REMOVE_LESS_THAN, this.List_PLC_Device_CCD02_03_PIN量測參數_面積下限[0].Value);
                this.List_CCD02_03_PIN量測_AxObject_區塊分析[i].SelectObjects(AxOvkBlob.TxAxObjFeature.AX_OBJECT_FEATURE_AREA, AxOvkBlob.TxAxObjFeatureOperation.AX_OBJECT_REMOVE_GREAT_THAN, this.List_PLC_Device_CCD02_03_PIN量測參數_面積上限[0].Value);
                if (this.List_CCD02_03_PIN量測_AxObject_區塊分析[i].DetectedNumObjs > 0)
                {
                    this.List_CCD02_03_PIN量測參數_量測點_有無[i] = true;
                    this.List_CCD02_03_PIN量測_AxObject_區塊分析[i].BlobIndex = 0;
                    this.List_CCD02_03_PIN量測參數_量測點[i].X = (float)this.List_CCD02_03_PIN量測_AxObject_區塊分析[i].BlobCentroidX;
                    this.List_CCD02_03_PIN量測參數_量測點[i].X += this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].OrgX;
                    //this.List_CCD02_03_PIN量測參數_量測點[i].Y = (float)this.List_CCD02_03_PIN量測_AxObject_區塊分析[i].BlobCentroidY;
                    this.List_CCD02_03_PIN量測參數_量測點[i].Y = (float)this.List_CCD02_03_PIN量測_AxObject_區塊分析[i].BlobCentroidY + (float)this.List_CCD02_03_PIN量測_AxObject_區塊分析[i].BlobLimBoxHeight / 2;
                    this.List_CCD02_03_PIN量測參數_量測點[i].Y += this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整[i].OrgY;
                }


            }

            cnt++;
        }
        void cnt_Program_CCD02_03_PIN量測_量測框調整_繪製畫布(ref int cnt)
        {
            this.PLC_Device_CCD02_03_PIN量測_量測框調整_RefreshCanvas.Bool = true;
            if (this.PLC_Device_CCD02_03_PIN量測_量測框調整按鈕.Bool && !PLC_Device_CCD02_03_計算一次.Bool)
            {
                this.h_Canvas_Tech_CCD02_03.RefreshCanvas();
            }

            cnt++;
        }





        #endregion
        #region PLC_CCD02_03_PIN量測_檢測距離計算
        private AxOvkMsr.AxPointLineDistanceMsr CCD02_03_PIN量測_AxPointLineDistanceMsr_線到點量測;
        MyTimer MyTimer_CCD02_03_PIN量測_檢測距離計算 = new MyTimer();
        PLC_Device PLC_Device_CCD02_03_PIN量測_檢測距離計算按鈕 = new PLC_Device("S6590");
        PLC_Device PLC_Device_CCD02_03_PIN量測_檢測距離計算 = new PLC_Device("S6591");
        PLC_Device PLC_Device_CCD02_03_PIN量測_檢測距離計算_OK = new PLC_Device("S6592");
        PLC_Device PLC_Device_CCD02_03_PIN量測_檢測距離計算_測試完成 = new PLC_Device("S6593");
        PLC_Device PLC_Device_CCD02_03_PIN量測_檢測距離計算_RefreshCanvas = new PLC_Device("S6594");

        PLC_Device PLC_Device_CCD02_03_PIN量測_水平度量測不測試 = new PLC_Device("S6595");
        PLC_Device PLC_Device_CCD02_03_PIN量測_間距量測不測試 = new PLC_Device("S6596");

        PLC_Device PLC_Device_CCD02_03_PIN量測_上排水平度量測標準值 = new PLC_Device("F7370");
        PLC_Device PLC_Device_CCD02_03_PIN量測_上排水平度量測上限值 = new PLC_Device("F7371");
        PLC_Device PLC_Device_CCD02_03_PIN量測_上排水平度量測下限值 = new PLC_Device("F7372");
        PLC_Device PLC_Device_CCD02_03_PIN量測_左右間距量測標準值 = new PLC_Device("F7373");
        PLC_Device PLC_Device_CCD02_03_PIN量測_左右間距量測上限值 = new PLC_Device("F7374");
        PLC_Device PLC_Device_CCD02_03_PIN量測_左右間距量測下限值 = new PLC_Device("F7375");
        PLC_Device PLC_Device_CCD02_03_PIN量測_間距上排PIN01到基準數值 = new PLC_Device("F7376");
        PLC_Device PLC_Device_CCD02_03_PIN量測_間距上排PIN01到基準上限 = new PLC_Device("F7377");
        PLC_Device PLC_Device_CCD02_03_PIN量測_間距上排PIN01到基準下限 = new PLC_Device("F7378");


        private List<PLC_Device> List_PLC_Device_CCD02_03_PIN量測參數_間距不測試 = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD02_03_PIN量測參數_左右間距量測值 = new List<PLC_Device>();

        private double[] List_CCD02_03_PIN量測參數_左右間距量測距離 = new double[8];
        private double[] List_CCD02_03_PIN量測參數_水平度量測距離 = new double[9];
        private double CCD02_03_PIN量測參數_間距上排PIN01到基準距離 = new double();
        private bool[] List_CCD02_03_PIN量測參數_量測點_OK = new bool[9];
        private bool[] List_CCD02_03_PIN量測參數_左右間距量測_OK = new bool[8];
        private bool[] List_CCD02_03_PIN量測參數_水平度量測_OK = new bool[9];
        private bool CCD02_03_PIN量測參數_間距上排PIN01到基準_OK = new bool();

        private double[] List_CCD02_03_PIN量測參數_水平度量測顯示點_X = new double[9];
        private double[] List_CCD02_03_PIN量測參數_水平度量測顯示點_Y = new double[9];
        private void H_Canvas_Tech_CCD02_03_PIN間距量測_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {
            PointF p0;
            PointF p1;
            PointF 上排_p2;
            PointF 上排_p3;
            PointF 下排_p2;
            PointF 下排_p3;
            PointF point;
            PointF to_line_point;
            double 間距;
            double 水平度;

            if (PLC_Device_CCD02_03_Main_取像並檢驗.Bool || PLC_Device_CCD02_03_PLC觸發檢測.Bool || PLC_Device_CCD02_03_Main_檢驗一次.Bool)
            {
                try
                {
                    Graphics g = Graphics.FromHdc((IntPtr)HDC);
                    DrawingClass.Draw.十字中心(new PointF(this.Point_CCD02_03_中心基準座標_量測點.X, this.Point_CCD02_03_中心基準座標_量測點.Y), 100, Color.Red, 2, g, ZoomX, ZoomY);
                    #region 左右間距顯示
                    for (int i = 0; i < 8; i++)
                    {
                        p0 = new PointF(this.List_CCD02_03_PIN量測參數_量測點[i].X, this.List_CCD02_03_PIN量測參數_量測點[i].Y);
                        p1 = new PointF(this.List_CCD02_03_PIN量測參數_量測點[i + 1].X, this.List_CCD02_03_PIN量測參數_量測點[i + 1].Y);
                        間距 = List_CCD02_03_PIN量測參數_左右間距量測距離[i];
                        if (List_CCD02_03_PIN量測參數_左右間距量測_OK[i])
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
                    上排_p2 = new PointF(this.List_CCD02_03_PIN量測參數_量測點[0].X, this.List_CCD02_03_PIN量測參數_量測點[0].Y - 150);
                    上排_p3 = new PointF(this.Point_CCD02_03_中心基準座標_量測點.X, this.List_CCD02_03_PIN量測參數_量測點[0].Y - 150);

                    if (CCD02_03_PIN量測參數_間距上排PIN01到基準_OK)
                    {
                        DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD02_03_PIN量測參數_間距上排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
                            (float)((上排_p2.Y + 上排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
                    }
                    else
                    {
                        DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD02_03_PIN量測參數_間距上排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
(float)((上排_p2.Y + 上排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Red, 1, g, ZoomX, ZoomY);
                    }

                    #endregion
                    #region 水平度顯示
                    DrawingClass.Draw.水平線段繪製(0, 10000, CCD02_03_水平基準線量測_AxLineMsr.MeasuredSlope, CCD02_03_水平基準線量測_AxLineMsr.MeasuredPivotX,
                        CCD02_03_水平基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD02_03_基準線量測_基準線偏移.Value, Color.Yellow, 2, g, ZoomX, ZoomY);

                    for (int i = 0; i < 9; i++)
                    {
                        point = new PointF(this.List_CCD02_03_PIN量測參數_量測點[i].X, this.List_CCD02_03_PIN量測參數_量測點[i].Y);

                        to_line_point = new PointF((float)this.List_CCD02_03_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD02_03_PIN量測參數_水平度量測顯示點_Y[i]) + this.PLC_Device_CCD02_03_基準線量測_基準線偏移.Value);

                        水平度 = List_CCD02_03_PIN量測參數_水平度量測距離[i];


                        if (List_CCD02_03_PIN量測參數_水平度量測_OK[i])
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                new PointF(point.X, point.Y + 650 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Yellow, g, ZoomX, ZoomY);

                            DrawingClass.Draw.線段繪製(point, to_line_point, Color.Yellow, 1, g, ZoomX, ZoomY);

                        }
                        else
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                new PointF(point.X, point.Y + 600 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                            DrawingClass.Draw.線段繪製(point, to_line_point, Color.Red, 1, g, ZoomX, ZoomY);

                        }


                    }



                    #endregion
                    #region 結果顯示
                    for (int i = 0; i < 8; i++)
                    {
                        if (List_CCD02_03_PIN量測參數_左右間距量測_OK[i] && CCD02_03_PIN量測參數_間距上排PIN01到基準_OK)
                        {
                            DrawingClass.Draw.文字左上繪製("間距量測OK!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                        }
                        else
                        {
                            DrawingClass.Draw.文字左上繪製("間距量測NG!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        }
                    }
                    for (int i = 0; i < 9; i++)
                    {
                        if (List_CCD02_03_PIN量測參數_水平度量測_OK[i])
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
            else if (PLC_Device_CCD02_03_Tech_檢驗一次.Bool || PLC_Device_CCD02_03_Tech_取像並檢驗.Bool)
            {
                if (this.PLC_Device_CCD02_03_PIN量測_檢測距離計算_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        DrawingClass.Draw.十字中心(new PointF(this.Point_CCD02_03_中心基準座標_量測點.X, this.Point_CCD02_03_中心基準座標_量測點.Y), 100, Color.Red, 2, g, ZoomX, ZoomY);
                        #region 左右間距顯示
                        for (int i = 0; i < 8; i++)
                        {
                            p0 = new PointF(this.List_CCD02_03_PIN量測參數_量測點[i].X, this.List_CCD02_03_PIN量測參數_量測點[i].Y);
                            p1 = new PointF(this.List_CCD02_03_PIN量測參數_量測點[i + 1].X, this.List_CCD02_03_PIN量測參數_量測點[i + 1].Y);
                            間距 = List_CCD02_03_PIN量測參數_左右間距量測距離[i];
                            if (List_CCD02_03_PIN量測參數_左右間距量測_OK[i])
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

                        上排_p2 = new PointF(this.List_CCD02_03_PIN量測參數_量測點[0].X, this.List_CCD02_03_PIN量測參數_量測點[0].Y - 150);
                        上排_p3 = new PointF(this.Point_CCD02_03_中心基準座標_量測點.X, this.List_CCD02_03_PIN量測參數_量測點[0].Y - 150);

                        if (CCD02_03_PIN量測參數_間距上排PIN01到基準_OK)
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD02_03_PIN量測參數_間距上排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
                                (float)((上排_p2.Y + 上排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
                        }
                        else
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD02_03_PIN量測參數_間距上排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
    (float)((上排_p2.Y + 上排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Red, 1, g, ZoomX, ZoomY);
                        }
                        #endregion
                        #region 水平度顯示
                        DrawingClass.Draw.水平線段繪製(0, 10000, CCD02_03_水平基準線量測_AxLineMsr.MeasuredSlope, CCD02_03_水平基準線量測_AxLineMsr.MeasuredPivotX,
                            CCD02_03_水平基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD02_03_基準線量測_基準線偏移.Value, Color.Yellow, 2, g, ZoomX, ZoomY);

                        for (int i = 0; i < 9; i++)
                        {
                            point = new PointF(this.List_CCD02_03_PIN量測參數_量測點[i].X, this.List_CCD02_03_PIN量測參數_量測點[i].Y);

                            to_line_point = new PointF((float)this.List_CCD02_03_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD02_03_PIN量測參數_水平度量測顯示點_Y[i]) + this.PLC_Device_CCD02_03_基準線量測_基準線偏移.Value);

                            水平度 = List_CCD02_03_PIN量測參數_水平度量測距離[i];


                            if (List_CCD02_03_PIN量測參數_水平度量測_OK[i])
                            {
                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                    new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Yellow, g, ZoomX, ZoomY);

                                DrawingClass.Draw.線段繪製(point, to_line_point, Color.Yellow, 1, g, ZoomX, ZoomY);

                            }
                            else
                            {
                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                    new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                                DrawingClass.Draw.線段繪製(point, to_line_point, Color.Red, 1, g, ZoomX, ZoomY);

                            }


                        }



                        #endregion
                        #region 結果顯示
                        for (int i = 0; i < 8; i++)
                        {

                            if (List_CCD02_03_PIN量測參數_左右間距量測_OK[i] && CCD02_03_PIN量測參數_間距上排PIN01到基準_OK)
                            {
                                DrawingClass.Draw.文字左上繪製("間距量測OK!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                            }
                            else
                            {
                                DrawingClass.Draw.文字左上繪製("間距量測NG!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            }

                        }
                        for (int i = 0; i < 9; i++)
                        {
                            if (List_CCD02_03_PIN量測參數_水平度量測_OK[i])
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
                if (this.PLC_Device_CCD02_03_PIN量測_檢測距離計算_RefreshCanvas.Bool && PLC_Device_CCD02_03_PIN量測_檢測距離計算.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);

                        DrawingClass.Draw.十字中心(new PointF(this.Point_CCD02_03_中心基準座標_量測點.X, this.Point_CCD02_03_中心基準座標_量測點.Y), 100, Color.Red, 2, g, ZoomX, ZoomY);
                        #region 左右間距顯示
                        for (int i = 0; i < 8; i++)
                        {
                            p0 = new PointF(this.List_CCD02_03_PIN量測參數_量測點[i].X, this.List_CCD02_03_PIN量測參數_量測點[i].Y);
                            p1 = new PointF(this.List_CCD02_03_PIN量測參數_量測點[i + 1].X, this.List_CCD02_03_PIN量測參數_量測點[i + 1].Y);
                            間距 = List_CCD02_03_PIN量測參數_左右間距量測距離[i];


                            if (List_CCD02_03_PIN量測參數_左右間距量測_OK[i])
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
                        上排_p2 = new PointF(this.List_CCD02_03_PIN量測參數_量測點[0].X, this.List_CCD02_03_PIN量測參數_量測點[0].Y - 150);
                        上排_p3 = new PointF(this.Point_CCD02_03_中心基準座標_量測點.X, this.List_CCD02_03_PIN量測參數_量測點[0].Y - 150);

                        if (CCD02_03_PIN量測參數_間距上排PIN01到基準_OK)
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD02_03_PIN量測參數_間距上排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
                                (float)((上排_p2.Y + 上排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
                        }
                        else
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD02_03_PIN量測參數_間距上排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
    (float)((上排_p2.Y + 上排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Red, 1, g, ZoomX, ZoomY);
                        }

                        #endregion
                        #region 水平度顯示
                        DrawingClass.Draw.水平線段繪製(0, 10000, CCD02_03_水平基準線量測_AxLineMsr.MeasuredSlope, CCD02_03_水平基準線量測_AxLineMsr.MeasuredPivotX,
                            CCD02_03_水平基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD02_03_基準線量測_基準線偏移.Value, Color.Yellow, 2, g, ZoomX, ZoomY);

                        for (int i = 0; i < 9; i++)
                        {
                            point = new PointF(this.List_CCD02_03_PIN量測參數_量測點[i].X, this.List_CCD02_03_PIN量測參數_量測點[i].Y);
                            to_line_point = new PointF((float)this.List_CCD02_03_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD02_03_PIN量測參數_水平度量測顯示點_Y[i]) + this.PLC_Device_CCD02_03_基準線量測_基準線偏移.Value);

                            水平度 = List_CCD02_03_PIN量測參數_水平度量測距離[i];

                            if (List_CCD02_03_PIN量測參數_水平度量測_OK[i])
                            {
                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                    new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Yellow, g, ZoomX, ZoomY);
                                DrawingClass.Draw.線段繪製(point, to_line_point, Color.Yellow, 1, g, ZoomX, ZoomY);

                            }
                            else
                            {
                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                    new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                                DrawingClass.Draw.線段繪製(point, to_line_point, Color.Red, 1, g, ZoomX, ZoomY);

                            }


                        }



                        #endregion
                        #region 結果顯示

                        for (int i = 0; i < 8; i++)
                        {
                            if (List_CCD02_03_PIN量測參數_左右間距量測_OK[i] && CCD02_03_PIN量測參數_間距上排PIN01到基準_OK)
                            {
                                DrawingClass.Draw.文字左上繪製("間距量測OK!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            }
                            else
                            {
                                DrawingClass.Draw.文字左上繪製("間距量測NG!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            }
                        }
                        for (int i = 0; i < 9; i++)
                        {
                            if (List_CCD02_03_PIN量測參數_水平度量測_OK[i])
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

            this.PLC_Device_CCD02_03_PIN量測_檢測距離計算_RefreshCanvas.Bool = false;
        }

        int cnt_Program_CCD02_03_PIN量測_檢測距離計算 = 65534;
        void sub_Program_CCD02_03_PIN量測_檢測距離計算()
        {
            if (cnt_Program_CCD02_03_PIN量測_檢測距離計算 == 65534)
            {
                this.h_Canvas_Tech_CCD02_03.OnCanvasDrawEvent += H_Canvas_Tech_CCD02_03_PIN間距量測_OnCanvasDrawEvent;
                this.h_Canvas_Main_CCD02_03_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD02_03_PIN間距量測_OnCanvasDrawEvent;
                PLC_Device_CCD02_03_PIN量測_檢測距離計算.SetComment("PLC_CCD02_03_PIN量測_檢測距離計算");
                PLC_Device_CCD02_03_PIN量測_檢測距離計算.Bool = false;
                PLC_Device_CCD02_03_PIN量測_檢測距離計算按鈕.Bool = false;
                cnt_Program_CCD02_03_PIN量測_檢測距離計算 = 65535;

            }
            if (cnt_Program_CCD02_03_PIN量測_檢測距離計算 == 65535) cnt_Program_CCD02_03_PIN量測_檢測距離計算 = 1;
            if (cnt_Program_CCD02_03_PIN量測_檢測距離計算 == 1) cnt_Program_CCD02_03_PIN量測_檢測距離計算_觸發按下(ref cnt_Program_CCD02_03_PIN量測_檢測距離計算);
            if (cnt_Program_CCD02_03_PIN量測_檢測距離計算 == 2) cnt_Program_CCD02_03_PIN量測_檢測距離計算_檢查按下(ref cnt_Program_CCD02_03_PIN量測_檢測距離計算);
            if (cnt_Program_CCD02_03_PIN量測_檢測距離計算 == 3) cnt_Program_CCD02_03_PIN量測_檢測距離計算_初始化(ref cnt_Program_CCD02_03_PIN量測_檢測距離計算);
            if (cnt_Program_CCD02_03_PIN量測_檢測距離計算 == 4) cnt_Program_CCD02_03_PIN量測_檢測距離計算_數值計算(ref cnt_Program_CCD02_03_PIN量測_檢測距離計算);
            if (cnt_Program_CCD02_03_PIN量測_檢測距離計算 == 5) cnt_Program_CCD02_03_PIN量測_檢測距離計算_量測結果(ref cnt_Program_CCD02_03_PIN量測_檢測距離計算);
            if (cnt_Program_CCD02_03_PIN量測_檢測距離計算 == 6) cnt_Program_CCD02_03_PIN量測_檢測距離計算_繪製畫布(ref cnt_Program_CCD02_03_PIN量測_檢測距離計算);
            if (cnt_Program_CCD02_03_PIN量測_檢測距離計算 == 7) cnt_Program_CCD02_03_PIN量測_檢測距離計算 = 65500;
            if (cnt_Program_CCD02_03_PIN量測_檢測距離計算 > 1) cnt_Program_CCD02_03_PIN量測_檢測距離計算_檢查放開(ref cnt_Program_CCD02_03_PIN量測_檢測距離計算);

            if (cnt_Program_CCD02_03_PIN量測_檢測距離計算 == 65500)
            {
                PLC_Device_CCD02_03_PIN量測_檢測距離計算.Bool = false;
                PLC_Device_CCD02_03_PIN量測_檢測距離計算按鈕.Bool = false;
                cnt_Program_CCD02_03_PIN量測_檢測距離計算 = 65535;
            }
        }
        void cnt_Program_CCD02_03_PIN量測_檢測距離計算_觸發按下(ref int cnt)
        {
            if (PLC_Device_CCD02_03_PIN量測_檢測距離計算按鈕.Bool || PLC_Device_CCD02_03_計算一次.Bool)
            {
                PLC_Device_CCD02_03_PIN量測_檢測距離計算.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD02_03_PIN量測_檢測距離計算_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD02_03_PIN量測_檢測距離計算.Bool)
            {
                cnt++;
            }

        }
        void cnt_Program_CCD02_03_PIN量測_檢測距離計算_檢查放開(ref int cnt)
        {
            //if (!PLC_Device_CCD02_03_PIN量測_檢測距離計算.Bool) cnt = 65500;
        }
        void cnt_Program_CCD02_03_PIN量測_檢測距離計算_初始化(ref int cnt)
        {
            this.MyTimer_CCD02_03_PIN量測_檢測距離計算.TickStop();
            this.MyTimer_CCD02_03_PIN量測_檢測距離計算.StartTickTime(99999);

            this.List_CCD02_03_PIN量測參數_左右間距量測距離 = new double[8];
            this.List_CCD02_03_PIN量測參數_水平度量測距離 = new double[9];
            this.CCD02_03_PIN量測參數_間距上排PIN01到基準距離 = new double();

            this.List_CCD02_03_PIN量測參數_量測點_OK = new bool[9];
            this.List_CCD02_03_PIN量測參數_左右間距量測_OK = new bool[8];
            this.List_CCD02_03_PIN量測參數_水平度量測_OK = new bool[9];
            this.CCD02_03_PIN量測參數_間距上排PIN01到基準_OK = new bool();

            cnt++;
        }
        void cnt_Program_CCD02_03_PIN量測_檢測距離計算_數值計算(ref int cnt)
        {
            #region 水平度數值計算
            this.CCD02_03_PIN量測_AxPointLineDistanceMsr_線到點量測.LinePivotX = this.CCD02_03_水平基準線量測_AxLineRegression.FittedPivotX;
            this.CCD02_03_PIN量測_AxPointLineDistanceMsr_線到點量測.LinePivotY = this.CCD02_03_水平基準線量測_AxLineRegression.FittedPivotY;
            this.CCD02_03_PIN量測_AxPointLineDistanceMsr_線到點量測.LineHorzVert = AxOvkMsr.TxAxLineHorzVert.AX_LINE_QUASI_HORIZONTAL;
            this.CCD02_03_PIN量測_AxPointLineDistanceMsr_線到點量測.LineSlope = this.CCD02_03_水平基準線量測_AxLineRegression.FittedSlope;
            for (int i = 0; i < this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {
                if (this.List_CCD02_03_PIN量測參數_量測點_有無[i])
                {
                    this.CCD02_03_PIN量測_AxPointLineDistanceMsr_線到點量測.PivotX = this.List_CCD02_03_PIN量測參數_量測點[i].X;
                    this.CCD02_03_PIN量測_AxPointLineDistanceMsr_線到點量測.PivotY = this.List_CCD02_03_PIN量測參數_量測點[i].Y;
                    this.CCD02_03_PIN量測_AxPointLineDistanceMsr_線到點量測.FindDistance();
                    this.List_CCD02_03_PIN量測參數_水平度量測顯示點_X[i] = CCD02_03_PIN量測_AxPointLineDistanceMsr_線到點量測.ProjectPivotX;
                    this.List_CCD02_03_PIN量測參數_水平度量測顯示點_Y[i] = CCD02_03_PIN量測_AxPointLineDistanceMsr_線到點量測.ProjectPivotY;
                    this.List_CCD02_03_PIN量測參數_水平度量測距離[i] = this.CCD02_03_PIN量測_AxPointLineDistanceMsr_線到點量測.Distance * this.CCD02_比例尺_pixcel_To_mm;
                }

            }
            #endregion
            #region 左右間距數值計算
            double distance = 0;
            double 間距Temp1_X = 0;
            double 間距Temp2_X = 0;

            for (int i = 0; i < 8; i++)
            {
                if (this.List_CCD02_03_PIN量測參數_量測點_有無[i] && this.List_CCD02_03_PIN量測參數_量測點_有無[i + 1])
                {

                    間距Temp1_X = this.List_CCD02_03_PIN量測參數_量測點[i].X - this.Point_CCD02_03_中心基準座標_量測點.X;
                    間距Temp2_X = this.List_CCD02_03_PIN量測參數_量測點[i + 1].X - this.Point_CCD02_03_中心基準座標_量測點.X;

                    distance = Math.Abs(間距Temp1_X - 間距Temp2_X);

                    this.List_CCD02_03_PIN量測參數_左右間距量測距離[i] = distance * this.CCD02_比例尺_pixcel_To_mm;
                }
                else
                {
                    PLC_Device_CCD02_03_PIN量測_檢測距離計算_OK.Bool = false;
                    List_CCD02_03_PIN量測參數_量測點_OK[i] = false;
                }

            }
            #endregion
            cnt++;
        }
        void cnt_Program_CCD02_03_PIN量測_檢測距離計算_量測結果(ref int cnt)
        {

            PLC_Device_CCD02_03_PIN量測_檢測距離計算_OK.Bool = true; // 檢測結果初始化
            #region 左右間距量測判斷

            for (int i = 0; i < 8; i++)
            {
                int 標準值 = this.PLC_Device_CCD02_03_PIN量測_左右間距量測標準值.Value;
                int 標準值上限 = this.PLC_Device_CCD02_03_PIN量測_左右間距量測上限值.Value;
                int 標準值下限 = this.PLC_Device_CCD02_03_PIN量測_左右間距量測下限值.Value;
                double 量測距離 = this.List_CCD02_03_PIN量測參數_左右間距量測距離[i];

                量測距離 = 量測距離 * 1000 - 標準值;
                量測距離 /= 1000;
                if (!PLC_Device_CCD02_03_PIN量測_間距量測不測試.Bool)
                {
                    if (this.List_CCD02_03_PIN量測參數_量測點_有無[i])
                    {
                        if (量測距離 >= 0 && i != 9)
                        {
                            if (標準值上限 <= Math.Abs(量測距離) * 1000 || 標準值下限 > Math.Abs(量測距離) * 1000)
                            {
                                this.List_CCD02_03_PIN量測參數_左右間距量測_OK[i] = false;
                                PLC_Device_CCD02_03_PIN量測_檢測距離計算_OK.Bool = false;
                            }
                            else
                            {
                                this.List_CCD02_03_PIN量測參數_左右間距量測_OK[i] = true;
                            }
                        }
                    }
                }
                else
                {
                    this.List_CCD02_03_PIN量測參數_左右間距量測_OK[i] = true;
                }

                this.List_CCD02_03_PIN量測參數_左右間距量測距離[i] = 量測距離;
            }
            #endregion
            #region 水平度量測判斷
            for (int i = 0; i < 9; i++)
            {
                int 上排標準值 = this.PLC_Device_CCD02_03_PIN量測_上排水平度量測標準值.Value;
                int 上排標準值上限 = this.PLC_Device_CCD02_03_PIN量測_上排水平度量測上限值.Value;
                int 上排標準值下限 = this.PLC_Device_CCD02_03_PIN量測_上排水平度量測下限值.Value;
                double 上排量測距離 = this.List_CCD02_03_PIN量測參數_水平度量測距離[i];

                上排量測距離 = 上排量測距離 * 1000 - 上排標準值;
                上排量測距離 /= 1000;

                if (!PLC_Device_CCD02_03_PIN量測_水平度量測不測試.Bool)
                {
                    if (this.List_CCD02_03_PIN量測參數_量測點_有無[i])
                    {
                        if (上排量測距離 >= 0 && i < 10)
                        {
                            if (上排標準值上限 <= Math.Abs(上排量測距離) * 1000 || 上排標準值下限 > Math.Abs(上排量測距離) * 1000)
                            {
                                this.List_CCD02_03_PIN量測參數_水平度量測_OK[i] = false;
                                PLC_Device_CCD02_03_PIN量測_檢測距離計算_OK.Bool = false;
                            }
                            else
                            {
                                this.List_CCD02_03_PIN量測參數_水平度量測_OK[i] = true;
                            }
                            this.List_CCD02_03_PIN量測參數_水平度量測距離[i] = 上排量測距離;
                        }

                    }
                }
                else
                {
                    this.List_CCD02_03_PIN量測參數_水平度量測_OK[i] = true;
                }
                if (PLC_Device_CCD02_03_PIN量測_間距量測不測試.Bool && PLC_Device_CCD02_03_PIN量測_水平度量測不測試.Bool)
                {
                    PLC_Device_CCD02_03_PIN量測_檢測距離計算_OK.Bool = true;
                }



            }
            #endregion
            #region 間距上排PIN01到基準線量測

            double temp_上排PIN01到基準 = 0;
            int 間距上排PIN01到基準標準值 = this.PLC_Device_CCD02_03_PIN量測_間距上排PIN01到基準數值.Value;
            int 間距上排PIN01到基準標準值上限 = this.PLC_Device_CCD02_03_PIN量測_間距上排PIN01到基準上限.Value;
            int 間距上排PIN01到基準標準值下限 = this.PLC_Device_CCD02_03_PIN量測_間距上排PIN01到基準下限.Value;


            if (this.List_CCD02_03_PIN量測參數_量測點_有無[0])
            {
                temp_上排PIN01到基準 = Math.Abs(this.List_CCD02_03_PIN量測參數_量測點[0].X - this.Point_CCD02_03_中心基準座標_量測點.X);
                this.CCD02_03_PIN量測參數_間距上排PIN01到基準距離 = temp_上排PIN01到基準 * this.CCD02_比例尺_pixcel_To_mm;
            }
            else
            {
                PLC_Device_CCD02_03_PIN量測_檢測距離計算_OK.Bool = false;
                CCD02_03_PIN量測參數_間距上排PIN01到基準_OK = false;
            }
            double 間距上排PIN01到基準量測距離 = this.CCD02_03_PIN量測參數_間距上排PIN01到基準距離;


            間距上排PIN01到基準量測距離 = 間距上排PIN01到基準量測距離 * 1000 - 間距上排PIN01到基準標準值;
            間距上排PIN01到基準量測距離 /= 1000;

            if (!PLC_Device_CCD02_03_PIN量測_間距量測不測試.Bool)
            {
                if (this.List_CCD02_03_PIN量測參數_量測點_有無[0])
                {
                    if (間距上排PIN01到基準標準值上限 <= Math.Abs(間距上排PIN01到基準量測距離) * 1000 || 間距上排PIN01到基準標準值下限 >
                        Math.Abs(間距上排PIN01到基準量測距離) * 1000)
                    {
                        this.CCD02_03_PIN量測參數_間距上排PIN01到基準_OK = false;
                        PLC_Device_CCD02_03_PIN量測_檢測距離計算_OK.Bool = false;
                    }
                    else
                    {
                        this.CCD02_03_PIN量測參數_間距上排PIN01到基準_OK = true;
                    }

                }
                CCD02_03_PIN量測參數_間距上排PIN01到基準距離 = 間距上排PIN01到基準量測距離;
            }
            else
            {
                this.CCD02_03_PIN量測參數_間距上排PIN01到基準_OK = true;
                this.PLC_Device_CCD02_03_PIN量測_檢測距離計算_OK.Bool = true;
            }

            #endregion
            cnt++;
        }
        void cnt_Program_CCD02_03_PIN量測_檢測距離計算_繪製畫布(ref int cnt)
        {
            this.PLC_Device_CCD02_03_PIN量測_檢測距離計算_RefreshCanvas.Bool = true;
            if (this.PLC_Device_CCD02_03_PIN量測_檢測距離計算按鈕.Bool && !PLC_Device_CCD02_03_計算一次.Bool)
            {

                this.h_Canvas_Tech_CCD02_03.RefreshCanvas();
            }
            cnt++;
        }
        #endregion

        #region Event

        private void plC_RJ_Button_CCD02_03_儲存圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (saveImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.h_Canvas_Tech_CCD02_03.SaveImage(saveImageDialog.FileName);
                }
            }));
        }
        private void plC_RJ_Button_CCD02_03_讀取圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (openImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.CCD02_AxImageBW8.LoadFile(openImageDialog.FileName);
                    try
                    {
                        this.h_Canvas_Tech_CCD02_03.ImageCopy(CCD02_AxImageBW8.VegaHandle);
                        this.CCD02_03_SrcImageHandle = h_Canvas_Tech_CCD02_03.VegaHandle;
                        this.h_Canvas_Tech_CCD02_03.RefreshCanvas();
                    }
                    catch
                    {
                        err_message02_03 = MessageBox.Show("讀取圖片空白", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        if (err_message02_03 == DialogResult.OK)
                        {

                        }
                    }
                }
            }));
        }
        private void PlC_RJ_Button_Main_CCD02_03儲存圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (saveImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.h_Canvas_Main_CCD02_03_檢測畫面.SaveImage(saveImageDialog.FileName);
                }
            }));
        }
        private void PlC_RJ_Button_Main_CCD02_03讀取圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (openImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.CCD02_AxImageBW8.LoadFile(openImageDialog.FileName);
                    try
                    {
                        this.h_Canvas_Main_CCD02_03_檢測畫面.ImageCopy(CCD02_AxImageBW8.VegaHandle);
                        this.CCD02_03_SrcImageHandle = h_Canvas_Main_CCD02_03_檢測畫面.VegaHandle;
                        this.h_Canvas_Main_CCD02_03_檢測畫面.RefreshCanvas();
                    }
                    catch
                    {
                        err_message02_03 = MessageBox.Show("讀取圖片空白", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                        if (err_message02_03 == DialogResult.OK)
                        {

                        }
                    }
                }
            }));
        }
        private void PlC_Button_Main_CCD02_03_ZOOM更新_btnClick(object sender, EventArgs e)
        {
            if (CCD02_03_SrcImageHandle != 0)
            {
                PLC_Device_Main_CCD02_03_ZOOM更新.Bool = true;
                h_Canvas_Main_CCD02_03_檢測畫面.RefreshCanvas();
            }
        }
        private PLC_Device PLC_Device_CCD02_03_PIN量測一鍵排列間距 = new PLC_Device("F4011");        

        private void plC_RJ_Button_CCD0203_Tech_PIN量測框一鍵排列_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 9; i++)
            {
                this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX[i].Value = this.List_PLC_Device_CCD02_03_PIN量測參數_OrgX[0].Value - i * PLC_Device_CCD02_03_PIN量測一鍵排列間距.Value;
                this.List_PLC_Device_CCD02_03_PIN量測參數_OrgY[i].Value = this.List_PLC_Device_CCD02_03_PIN量測參數_OrgY[0].Value;
            }
        }
        private void plC_RJ_Button_CCD0203_Tech_PIN量測框大小設為一致_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 9; i++)
            {
                this.List_PLC_Device_CCD02_03_PIN量測參數_Width[i].Value = this.List_PLC_Device_CCD02_03_PIN量測參數_Width[0].Value;
                this.List_PLC_Device_CCD02_03_PIN量測參數_Height[i].Value = this.List_PLC_Device_CCD02_03_PIN量測參數_Height[0].Value;
            }
        }

        #endregion
    }
}
