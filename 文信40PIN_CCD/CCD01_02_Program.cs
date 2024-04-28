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


        DialogResult err_message01_02;

        void Program_CCD01_02()
        {
            this.CCD01_02_儲存圖片();
            this.sub_Program_CCD01_02_SNAP();
            this.sub_Program_CCD01_02_Tech_檢驗一次();
            this.sub_Program_CCD01_02_計算一次();
            //this.sub_Program_CCD01_02_PIN量測_量測框調整();
            this.sub_Program_CCD01_02_PIN量測_檢測距離計算();
            this.sub_Program_CCD01_02_PIN正位度量測_設定規範位置();
            this.sub_Program_CCD01_02_PIN量測_檢測正位度計算();
            this.sub_Program_CCD01_02_Tech_取像並檢驗();
            this.sub_Program_CCD01_02_Main_取像並檢驗();
            this.sub_Program_CCD01_02_PIN量測點量測();
            this.sub_Program_CCD01_02_Main_檢驗一次();

        }
        #region PLC_CCD01_02_SNAP
        PLC_Device PLC_Device_CCD01_02_SNAP_按鈕 = new PLC_Device("M15030");
        PLC_Device PLC_Device_CCD01_02_SNAP = new PLC_Device("M15025");
        PLC_Device PLC_Device_CCD01_02_SNAP_LIVE = new PLC_Device("M15026");
        PLC_Device PLC_Device_CCD01_02_SNAP_電子快門 = new PLC_Device("F9010");
        PLC_Device PLC_Device_CCD01_02_SNAP_視訊增益 = new PLC_Device("F9011");
        PLC_Device PLC_Device_CCD01_02_SNAP_銳利度 = new PLC_Device("F9012");
        PLC_Device PLC_Device_CCD01_02_SNAP_光源亮度_紅正照 = new PLC_Device("F25010");
        PLC_Device PLC_Device_CCD01_02_SNAP_光源亮度_白正照 = new PLC_Device("F25011");
        MyTimer CCD01_02_Snap_Timer = new MyTimer();
        int cnt_Program_CCD01_02_SNAP = 65534;
        void sub_Program_CCD01_02_SNAP()
        {
            if (cnt_Program_CCD01_02_SNAP == 65534)
            {
                PLC_Device_CCD01_02_SNAP.SetComment("PLC_CCD01_02_SNAP");
                PLC_Device_CCD01_02_SNAP.Bool = false;
                PLC_Device_CCD01_02_SNAP_按鈕.Bool = false;
                cnt_Program_CCD01_02_SNAP = 65535;
            }
            if (cnt_Program_CCD01_02_SNAP == 65535) cnt_Program_CCD01_02_SNAP = 1;
            if (cnt_Program_CCD01_02_SNAP == 1) cnt_Program_CCD01_02_SNAP_檢查按下(ref cnt_Program_CCD01_02_SNAP);
            if (cnt_Program_CCD01_02_SNAP == 2) cnt_Program_CCD01_02_SNAP_初始化(ref cnt_Program_CCD01_02_SNAP);
            if (cnt_Program_CCD01_02_SNAP == 3) cnt_Program_CCD01_02_SNAP_開始取像(ref cnt_Program_CCD01_02_SNAP);
            if (cnt_Program_CCD01_02_SNAP == 4) cnt_Program_CCD01_02_SNAP_取像結束(ref cnt_Program_CCD01_02_SNAP);
            if (cnt_Program_CCD01_02_SNAP == 5) cnt_Program_CCD01_02_SNAP_繪製影像(ref cnt_Program_CCD01_02_SNAP);
            if (cnt_Program_CCD01_02_SNAP == 6) cnt_Program_CCD01_02_SNAP = 65500;
            if (cnt_Program_CCD01_02_SNAP > 1) cnt_Program_CCD01_02_SNAP_檢查放開(ref cnt_Program_CCD01_02_SNAP);

            if (cnt_Program_CCD01_02_SNAP == 65500)
            {
                PLC_Device_CCD01_02_SNAP_按鈕.Bool = false;
                PLC_Device_CCD01_02_SNAP.Bool = false;
                cnt_Program_CCD01_02_SNAP = 65535;
            }
        }
        void cnt_Program_CCD01_02_SNAP_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_02_SNAP_按鈕.Bool)
            {
                PLC_Device_CCD01_02_SNAP.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_02_SNAP_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_02_SNAP.Bool)
            {
                cnt = 65500;
            }
        }
        void cnt_Program_CCD01_02_SNAP_初始化(ref int cnt)
        {
            CCD01_02_Snap_Timer.TickStop();
            CCD01_02_Snap_Timer.StartTickTime(10000);
            PLC_Device_CCD01_SNAP_電子快門.Value = PLC_Device_CCD01_02_SNAP_電子快門.Value;
            PLC_Device_CCD01_SNAP_視訊增益.Value = PLC_Device_CCD01_02_SNAP_視訊增益.Value;
            PLC_Device_CCD01_SNAP_銳利度.Value = PLC_Device_CCD01_02_SNAP_銳利度.Value;

            if (PLC_Device_CCD01_02_SNAP_光源亮度_紅正照.Value != 0)
            {
                this.光源控制(enum_光源.CCD01_紅正照, (byte)this.PLC_Device_CCD01_02_SNAP_光源亮度_紅正照.Value);
                this.光源控制(enum_光源.CCD01_紅正照, true);
            }
            else if (this.PLC_Device_CCD01_02_SNAP_光源亮度_紅正照.Value == 0)
            {
                this.光源控制(enum_光源.CCD01_紅正照, (byte)0);
                this.光源控制(enum_光源.CCD01_紅正照, false);
            }
            if (PLC_Device_CCD01_02_SNAP_光源亮度_白正照.Value != 0)
            {
                this.光源控制(enum_光源.CCD01_白正照, (byte)this.PLC_Device_CCD01_02_SNAP_光源亮度_白正照.Value);
                this.光源控制(enum_光源.CCD01_白正照, true);
            }
            else if (this.PLC_Device_CCD01_02_SNAP_光源亮度_白正照.Value == 0)
            {
                this.光源控制(enum_光源.CCD01_白正照, (byte)0);
                this.光源控制(enum_光源.CCD01_白正照, false);
            }
            cnt++;
        }
        void cnt_Program_CCD01_02_SNAP_開始取像(ref int cnt)
        {
            if (!PLC_Device_CCD01_SNAP.Bool)
            {
                PLC_Device_CCD01_SNAP.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_02_SNAP_取像結束(ref int cnt)
        {
            if (!PLC_Device_CCD01_SNAP.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_02_SNAP_繪製影像(ref int cnt)
        {
            this.CCD01_02_SrcImageHandle = this.h_Canvas_Tech_CCD01_02.VegaHandle;
            this.h_Canvas_Tech_CCD01_02.ImageCopy(this.CCD01_AxImageBW8.VegaHandle);

            this.CCD01_02_SrcImageHandle = this.h_Canvas_Main_CCD01_02_檢測畫面.VegaHandle;
            this.h_Canvas_Main_CCD01_02_檢測畫面.ImageCopy(this.CCD01_AxImageBW8.VegaHandle);
            this.h_Canvas_Tech_CCD01_02.SetImageSize(this.h_Canvas_Tech_CCD01_02.ImageWidth, this.h_Canvas_Tech_CCD01_02.ImageHeight);

            if (!PLC_Device_CCD01_02_Tech_取像並檢驗.Bool && !PLC_Device_CCD01_02_Main_取像並檢驗.Bool)
            {
                if (this.PLC_Device_CCD01_02_SNAP.Bool) this.h_Canvas_Tech_CCD01_02.RefreshCanvas();


                if (PLC_Device_CCD01_02_SNAP_LIVE.Bool)
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
            Console.WriteLine($"CCD01_02，COST{CCD01_02_Snap_Timer.ToString()}");

        }





        #endregion
        #region PLC_CCD01_02_Main_取像並檢驗
        PLC_Device PLC_Device_CCD01_02_Main_取像並檢驗按鈕 = new PLC_Device("S39910");
        PLC_Device PLC_Device_CCD01_02_Main_取像並檢驗 = new PLC_Device("S39911");
        PLC_Device PLC_Device_CCD01_02_Main_取像並檢驗_OK = new PLC_Device("S39912");
        PLC_Device PLC_Device_CCD01_02_PLC觸發檢測 = new PLC_Device("S39710");
        PLC_Device PLC_Device_CCD01_02_PLC觸發檢測完成 = new PLC_Device("S39711");
        PLC_Device PLC_Device_CCD01_02_Main_取像完成 = new PLC_Device("S39712");
        PLC_Device PLC_Device_CCD01_02_Main_BUSY = new PLC_Device("S39713");
        bool flag_CCD01_02_開始存檔 = false;
        String CCD01_02_原圖位置, CCD01_02_量測圖位置;
        PLC_Device PLC_NumBox_CCD01_02_OK最大儲存張數 = new PLC_Device("F13203");
        PLC_Device PLC_NumBox_CCD01_02_NG最大儲存張數 = new PLC_Device("F13204");
        MyTimer CCD01_02_Init_Timer = new MyTimer();
        int cnt_Program_CCD01_02_Main_取像並檢驗 = 65534;
        void sub_Program_CCD01_02_Main_取像並檢驗()
        {
            if (cnt_Program_CCD01_02_Main_取像並檢驗 == 65534)
            {
                PLC_Device_CCD01_02_Main_取像並檢驗.SetComment("PLC_CCD01_02_Main_取像並檢驗");
                PLC_Device_CCD01_02_Main_BUSY.Bool = false;
                PLC_Device_CCD01_02_Main_取像完成.Bool = false;
                PLC_Device_CCD01_02_Main_取像並檢驗.Bool = false;
                PLC_Device_CCD01_02_PLC觸發檢測.Bool = false;
                PLC_Device_CCD01_02_PLC觸發檢測完成.Bool = false;
                PLC_Device_CCD01_02_Main_取像並檢驗_OK.Bool = false;
                PLC_Device_CCD01_02_Main_取像並檢驗按鈕.Bool = false;
                cnt_Program_CCD01_02_Main_取像並檢驗 = 65535;

            }
            if (cnt_Program_CCD01_02_Main_取像並檢驗 == 65535) cnt_Program_CCD01_02_Main_取像並檢驗 = 1;
            if (cnt_Program_CCD01_02_Main_取像並檢驗 == 1) cnt_Program_CCD01_02_Main_取像並檢驗_檢查按下(ref cnt_Program_CCD01_02_Main_取像並檢驗);
            if (cnt_Program_CCD01_02_Main_取像並檢驗 == 2) cnt_Program_CCD01_02_Main_取像並檢驗_初始化(ref cnt_Program_CCD01_02_Main_取像並檢驗);
            if (cnt_Program_CCD01_02_Main_取像並檢驗 == 3) cnt_Program_CCD01_02_Main_取像並檢驗_開始SNAP(ref cnt_Program_CCD01_02_Main_取像並檢驗);
            if (cnt_Program_CCD01_02_Main_取像並檢驗 == 4) cnt_Program_CCD01_02_Main_取像並檢驗_結束SNAP(ref cnt_Program_CCD01_02_Main_取像並檢驗);
            if (cnt_Program_CCD01_02_Main_取像並檢驗 == 5) cnt_Program_CCD01_02_Main_取像並檢驗_開始計算一次(ref cnt_Program_CCD01_02_Main_取像並檢驗);
            if (cnt_Program_CCD01_02_Main_取像並檢驗 == 6) cnt_Program_CCD01_02_Main_取像並檢驗_結束計算一次(ref cnt_Program_CCD01_02_Main_取像並檢驗);
            if (cnt_Program_CCD01_02_Main_取像並檢驗 == 7) cnt_Program_CCD01_02_Main_取像並檢驗_繪製畫布(ref cnt_Program_CCD01_02_Main_取像並檢驗);
            if (cnt_Program_CCD01_02_Main_取像並檢驗 == 8) cnt_Program_CCD01_02_Main_取像並檢驗_檢查重測次數(ref cnt_Program_CCD01_02_Main_取像並檢驗);
            if (cnt_Program_CCD01_02_Main_取像並檢驗 == 9) cnt_Program_CCD01_02_Main_取像並檢驗 = 65500;
            if (cnt_Program_CCD01_02_Main_取像並檢驗 > 1) cnt_Program_CCD01_02_Main_取像並檢驗_檢查放開(ref cnt_Program_CCD01_02_Main_取像並檢驗);

            if (cnt_Program_CCD01_02_Main_取像並檢驗 == 65500)
            {
                PLC_Device_CCD01_02_Main_取像完成.Bool = false;
                PLC_Device_CCD01_02_Main_取像並檢驗.Bool = false;
                PLC_Device_CCD01_02_PLC觸發檢測.Bool = false;
                PLC_Device_CCD01_02_Main_BUSY.Bool = false;
                PLC_Device_CCD01_02_Main_取像並檢驗按鈕.Bool = false;
                cnt_Program_CCD01_02_Main_取像並檢驗 = 65535;
            }
        }
        void cnt_Program_CCD01_02_Main_取像並檢驗_檢查按下(ref int cnt)
        {

            if (PLC_Device_CCD01_02_Main_取像並檢驗按鈕.Bool || PLC_Device_CCD01_02_PLC觸發檢測.Bool)
            {
                CCD01_02_Init_Timer.TickStop();
                CCD01_02_Init_Timer.StartTickTime(100000);

                PLC_Device_CCD01_02_Main_取像並檢驗.Bool = true;
                cnt++;
            }



        }
        void cnt_Program_CCD01_02_Main_取像並檢驗_檢查放開(ref int cnt)
        {
            //if (!PLC_Device_CCD01_02_Main_取像並檢驗.Bool && !PLC_Device_CCD01_02_PLC觸發檢測.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_02_Main_取像並檢驗_初始化(ref int cnt)
        {
            PLC_Device_CCD01_02_PLC觸發檢測完成.Bool = false;
            PLC_Device_CCD01_02_Main_BUSY.Bool = true;
            cnt++;
        }
        void cnt_Program_CCD01_02_Main_取像並檢驗_開始SNAP(ref int cnt)
        {
            if (!PLC_Device_CCD01_02_SNAP.Bool)
            {
                PLC_Device_CCD01_02_SNAP_按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_02_Main_取像並檢驗_結束SNAP(ref int cnt)
        {
            if (!PLC_Device_CCD01_02_SNAP_按鈕.Bool)
            {
                光源控制(enum_光源.CCD01_紅正照, (byte)0);
                光源控制(enum_光源.CCD01_紅正照, false);
                光源控制(enum_光源.CCD01_白正照, (byte)0);
                光源控制(enum_光源.CCD01_白正照, false);
                PLC_Device_CCD01_02_Main_取像完成.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_02_Main_取像並檢驗_開始計算一次(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_02_計算一次.Bool)
            {

                this.PLC_Device_CCD01_02_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_02_Main_取像並檢驗_結束計算一次(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_02_計算一次.Bool)
            {

                Console.WriteLine($"CCD01_02檢測,耗時 {CCD01_02_Init_Timer.ToString()}");
                cnt++;
            }
        }
        void cnt_Program_CCD01_02_Main_取像並檢驗_繪製畫布(ref int cnt)
        {

            if (CCD01_02_SrcImageHandle != 0)
            {
                this.h_Canvas_Main_CCD01_02_檢測畫面.RefreshCanvas();
                PLC_Device_CCD01_02_PLC觸發檢測完成.Bool = true;
                flag_CCD01_02_開始存檔 = true;
            }
            cnt++;
        }
        void cnt_Program_CCD01_02_Main_取像並檢驗_檢查重測次數(ref int cnt)
        {
            cnt++;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            flag_CCD01_02_開始存檔 = true;
            flag_CCD02_02_開始存檔 = true;
        }
        private void CCD01_02_儲存圖片()
        {
            if (flag_CCD01_02_開始存檔)
            {
                String FilePlaceOK = plC_WordBox_CCD01_02_OK存圖路徑.Text;
                String FileNameOK = "CCD01_02_OK";
                String FilePlaceNG = plC_WordBox_CCD01_02_NG存圖路徑.Text;
                String FileNameNG = "CCD01_02_NG";
                儲存檔案_往後移位(FilePlaceOK, FileNameOK, PLC_NumBox_CCD01_02_OK最大儲存張數.Value);
                儲存檔案_往後移位(FilePlaceNG, FileNameNG, PLC_NumBox_CCD01_02_NG最大儲存張數.Value);
                if (PLC_Device_CCD01_02_Main_取像並檢驗_OK.Bool)
                {
                    整理檔案(FilePlaceOK, FileNameOK, PLC_NumBox_CCD01_02_OK最大儲存張數.Value);
                    FileNameOK = FileNameOK + "_OK";
                    CCD01_02_原圖位置 = CCD01_02_OK儲存檔案檢查(FilePlaceOK, FileNameOK + "_A", PLC_NumBox_CCD01_02_OK最大儲存張數.Value);
                    CCD01_02_量測圖位置 = CCD01_02_原圖位置.Replace("_A", "_B");
                    this.Invoke(new Action(delegate
                    {
                        if (plC_ComboBox_CCD01_02_OK是否存圖.SelectedIndex == 0)
                        {
                            this.h_Canvas_Main_CCD01_02_檢測畫面.SaveImage(CCD01_02_原圖位置);
                        }
                    }));
                }
                else if (!PLC_Device_CCD01_02_Main_取像並檢驗_OK.Bool)
                {
                    整理檔案(FilePlaceNG, FileNameNG, PLC_NumBox_CCD01_02_NG最大儲存張數.Value);
                    FileNameNG = FileNameNG + "_NG";
                    CCD01_02_原圖位置 = CCD01_02_NG儲存檔案檢查(FilePlaceNG, FileNameNG + "_A", PLC_NumBox_CCD01_02_NG最大儲存張數.Value);
                    CCD01_02_量測圖位置 = CCD01_02_原圖位置.Replace("_A", "_B");
                    this.Invoke(new Action(delegate
                    {
                        if (plC_ComboBox_CCD01_02_NG是否存圖.SelectedIndex == 0)
                        {
                            this.h_Canvas_Main_CCD01_02_檢測畫面.SaveImage(CCD01_02_原圖位置);
                        }
                    }));
                }
                flag_CCD01_02_開始存檔 = false;
            }
        }
        #endregion
        #region PLC_CCD01_02_Tech_取像並檢驗
        PLC_Device PLC_Device_CCD01_02_Tech_取像並檢驗按鈕 = new PLC_Device("M15630");
        PLC_Device PLC_Device_CCD01_02_Tech_取像並檢驗 = new PLC_Device("M15625");
        MyTimer CCD01_02_Tech_Timer = new MyTimer();
        int cnt_Program_CCD01_02_Tech_取像並檢驗 = 65534;
        void sub_Program_CCD01_02_Tech_取像並檢驗()
        {
            if (cnt_Program_CCD01_02_Tech_取像並檢驗 == 65534)
            {
                PLC_Device_CCD01_02_Tech_取像並檢驗.SetComment("PLC_CCD01_02_Tech_取像並檢驗");
                PLC_Device_CCD01_02_Tech_取像並檢驗按鈕.Bool = false;
                PLC_Device_CCD01_02_Tech_取像並檢驗.Bool = false;
                cnt_Program_CCD01_02_Tech_取像並檢驗 = 65535;
            }
            if (cnt_Program_CCD01_02_Tech_取像並檢驗 == 65535) cnt_Program_CCD01_02_Tech_取像並檢驗 = 1;
            if (cnt_Program_CCD01_02_Tech_取像並檢驗 == 1) cnt_Program_CCD01_02_Tech_取像並檢驗_檢查按下(ref cnt_Program_CCD01_02_Tech_取像並檢驗);
            if (cnt_Program_CCD01_02_Tech_取像並檢驗 == 2) cnt_Program_CCD01_02_Tech_取像並檢驗_初始化(ref cnt_Program_CCD01_02_Tech_取像並檢驗);
            if (cnt_Program_CCD01_02_Tech_取像並檢驗 == 3) cnt_Program_CCD01_02_Tech_取像並檢驗_SNAP一次開始(ref cnt_Program_CCD01_02_Tech_取像並檢驗);
            if (cnt_Program_CCD01_02_Tech_取像並檢驗 == 4) cnt_Program_CCD01_02_Tech_取像並檢驗_SNAP一次結束(ref cnt_Program_CCD01_02_Tech_取像並檢驗);
            if (cnt_Program_CCD01_02_Tech_取像並檢驗 == 5) cnt_Program_CCD01_02_Tech_取像並檢驗_計算一次開始(ref cnt_Program_CCD01_02_Tech_取像並檢驗);
            if (cnt_Program_CCD01_02_Tech_取像並檢驗 == 6) cnt_Program_CCD01_02_Tech_取像並檢驗_計算一次結束(ref cnt_Program_CCD01_02_Tech_取像並檢驗);
            if (cnt_Program_CCD01_02_Tech_取像並檢驗 == 7) cnt_Program_CCD01_02_Tech_取像並檢驗_繪製畫布(ref cnt_Program_CCD01_02_Tech_取像並檢驗);
            if (cnt_Program_CCD01_02_Tech_取像並檢驗 == 8) cnt_Program_CCD01_02_Tech_取像並檢驗 = 65500;
            if (cnt_Program_CCD01_02_Tech_取像並檢驗 > 1) cnt_Program_CCD01_02_Tech_取像並檢驗_檢查放開(ref cnt_Program_CCD01_02_Tech_取像並檢驗);

            if (cnt_Program_CCD01_02_Tech_取像並檢驗 == 65500)
            {
                PLC_Device_CCD01_02_Tech_取像並檢驗按鈕.Bool = false;
                PLC_Device_CCD01_02_Tech_取像並檢驗.Bool = false;
                cnt_Program_CCD01_02_Tech_取像並檢驗 = 65535;
            }
        }
        void cnt_Program_CCD01_02_Tech_取像並檢驗_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_02_Tech_取像並檢驗按鈕.Bool)
            {
                PLC_Device_CCD01_02_Tech_取像並檢驗.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_02_Tech_取像並檢驗_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_02_Tech_取像並檢驗.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_02_Tech_取像並檢驗_初始化(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_02_Tech_取像並檢驗_SNAP一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_02_SNAP.Bool)
            {
                this.PLC_Device_CCD01_02_SNAP_按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_02_Tech_取像並檢驗_SNAP一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_02_SNAP_按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_02_Tech_取像並檢驗_計算一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_02_計算一次.Bool)
            {
                this.PLC_Device_CCD01_02_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_02_Tech_取像並檢驗_計算一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_02_計算一次.Bool)
            {

                cnt++;
            }
        }
        void cnt_Program_CCD01_02_Tech_取像並檢驗_繪製畫布(ref int cnt)
        {
            if (CCD01_02_SrcImageHandle != 0)
            {
                this.h_Canvas_Tech_CCD01_02.RefreshCanvas();
            }
            if (PLC_Device_CCD01_02_SNAP_LIVE.Bool)
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

            Console.WriteLine($"CCD0101檢測,耗時 {CCD01_02_Tech_Timer.ToString()}");
        }

































        #endregion
        #region PLC_CCD01_02_Tech_檢驗一次
        PLC_Device PLC_Device_CCD01_02_Tech_檢驗一次按鈕 = new PLC_Device("M15330");
        PLC_Device PLC_Device_CCD01_02_Tech_檢驗一次 = new PLC_Device("M15325");
        int cnt_Program_CCD01_02_Tech_檢驗一次 = 65534;
        void sub_Program_CCD01_02_Tech_檢驗一次()
        {
            if (cnt_Program_CCD01_02_Tech_檢驗一次 == 65534)
            {
                PLC_Device_CCD01_02_Tech_檢驗一次.SetComment("PLC_CCD01_02_Tech_檢驗一次");
                PLC_Device_CCD01_02_Tech_檢驗一次按鈕.Bool = false;
                PLC_Device_CCD01_02_Tech_檢驗一次.Bool = false;
                cnt_Program_CCD01_02_Tech_檢驗一次 = 65535;
            }
            if (cnt_Program_CCD01_02_Tech_檢驗一次 == 65535) cnt_Program_CCD01_02_Tech_檢驗一次 = 1;
            if (cnt_Program_CCD01_02_Tech_檢驗一次 == 1) cnt_Program_CCD01_02_Tech_檢驗一次_檢查按下(ref cnt_Program_CCD01_02_Tech_檢驗一次);
            if (cnt_Program_CCD01_02_Tech_檢驗一次 == 2) cnt_Program_CCD01_02_Tech_檢驗一次_初始化(ref cnt_Program_CCD01_02_Tech_檢驗一次);
            if (cnt_Program_CCD01_02_Tech_檢驗一次 == 3) cnt_Program_CCD01_02_Tech_檢驗一次_計算一次開始(ref cnt_Program_CCD01_02_Tech_檢驗一次);
            if (cnt_Program_CCD01_02_Tech_檢驗一次 == 4) cnt_Program_CCD01_02_Tech_檢驗一次_計算一次結束(ref cnt_Program_CCD01_02_Tech_檢驗一次);
            if (cnt_Program_CCD01_02_Tech_檢驗一次 == 5) cnt_Program_CCD01_02_Tech_檢驗一次_繪製畫布(ref cnt_Program_CCD01_02_Tech_檢驗一次);
            if (cnt_Program_CCD01_02_Tech_檢驗一次 == 6) cnt_Program_CCD01_02_Tech_檢驗一次 = 65500;
            if (cnt_Program_CCD01_02_Tech_檢驗一次 > 1) cnt_Program_CCD01_02_Tech_檢驗一次_檢查放開(ref cnt_Program_CCD01_02_Tech_檢驗一次);

            if (cnt_Program_CCD01_02_Tech_檢驗一次 == 65500)
            {
                PLC_Device_CCD01_02_Tech_檢驗一次按鈕.Bool = false;
                PLC_Device_CCD01_02_Tech_檢驗一次.Bool = false;
                cnt_Program_CCD01_02_Tech_檢驗一次 = 65535;
            }
        }
        void cnt_Program_CCD01_02_Tech_檢驗一次_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_02_Tech_檢驗一次按鈕.Bool)
            {
                PLC_Device_CCD01_02_Tech_檢驗一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_02_Tech_檢驗一次_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_02_Tech_檢驗一次.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_02_Tech_檢驗一次_初始化(ref int cnt)
        {
            CCD01_02_Tech_Timer.TickStop();
            CCD01_02_Tech_Timer.StartTickTime(1000000);
            cnt++;
        }
        void cnt_Program_CCD01_02_Tech_檢驗一次_計算一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_02_計算一次.Bool)
            {
                this.PLC_Device_CCD01_02_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_02_Tech_檢驗一次_計算一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_02_計算一次.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_02_Tech_檢驗一次_繪製畫布(ref int cnt)
        {

            if (CCD01_02_SrcImageHandle != 0)
            {
                Console.WriteLine($"CCD0101檢驗一次,耗時 {CCD01_02_Tech_Timer.ToString()}");
                this.h_Canvas_Tech_CCD01_02.RefreshCanvas();
            }
            cnt++;
        }

        #endregion
        #region PLC_CCD01_02_Main_檢驗一次
        PLC_Device PLC_Device_CCD01_02_Main_檢驗一次按鈕 = new PLC_Device("M15802");
        PLC_Device PLC_Device_CCD01_02_Main_檢驗一次 = new PLC_Device("M15803");
        int cnt_Program_CCD01_02_Main_檢驗一次 = 65534;
        void sub_Program_CCD01_02_Main_檢驗一次()
        {
            if (cnt_Program_CCD01_02_Main_檢驗一次 == 65534)
            {
                PLC_Device_CCD01_02_Main_檢驗一次.SetComment("PLC_CCD01_02_Main_檢驗一次");
                PLC_Device_CCD01_02_Main_檢驗一次.Bool = false;
                PLC_Device_CCD01_02_Main_檢驗一次按鈕.Bool = false;
                cnt_Program_CCD01_02_Main_檢驗一次 = 65535;
            }
            if (cnt_Program_CCD01_02_Main_檢驗一次 == 65535) cnt_Program_CCD01_02_Main_檢驗一次 = 1;
            if (cnt_Program_CCD01_02_Main_檢驗一次 == 1) cnt_Program_CCD01_02_Main_檢驗一次_檢查按下(ref cnt_Program_CCD01_02_Main_檢驗一次);
            if (cnt_Program_CCD01_02_Main_檢驗一次 == 2) cnt_Program_CCD01_02_Main_檢驗一次_初始化(ref cnt_Program_CCD01_02_Main_檢驗一次);
            if (cnt_Program_CCD01_02_Main_檢驗一次 == 3) cnt_Program_CCD01_02_Main_檢驗一次_計算一次開始(ref cnt_Program_CCD01_02_Main_檢驗一次);
            if (cnt_Program_CCD01_02_Main_檢驗一次 == 4) cnt_Program_CCD01_02_Main_檢驗一次_計算一次結束(ref cnt_Program_CCD01_02_Main_檢驗一次);
            if (cnt_Program_CCD01_02_Main_檢驗一次 == 5) cnt_Program_CCD01_02_Main_檢驗一次_繪製畫布(ref cnt_Program_CCD01_02_Main_檢驗一次);
            if (cnt_Program_CCD01_02_Main_檢驗一次 == 6) cnt_Program_CCD01_02_Main_檢驗一次 = 65500;
            if (cnt_Program_CCD01_02_Main_檢驗一次 > 1) cnt_Program_CCD01_02_Main_檢驗一次_檢查放開(ref cnt_Program_CCD01_02_Main_檢驗一次);

            if (cnt_Program_CCD01_02_Main_檢驗一次 == 65500)
            {
                PLC_Device_CCD01_02_Main_檢驗一次按鈕.Bool = false;
                PLC_Device_CCD01_02_Main_檢驗一次.Bool = false;
                cnt_Program_CCD01_02_Main_檢驗一次 = 65535;
            }
        }
        void cnt_Program_CCD01_02_Main_檢驗一次_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_02_Main_檢驗一次按鈕.Bool)
            {
                PLC_Device_CCD01_02_Main_檢驗一次.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_02_Main_檢驗一次_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_02_Main_檢驗一次.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_02_Main_檢驗一次_初始化(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_02_Main_檢驗一次_計算一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_02_計算一次.Bool)
            {
                this.PLC_Device_CCD01_02_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_02_Main_檢驗一次_計算一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_02_計算一次.Bool)
            {

                cnt++;
            }
        }
        void cnt_Program_CCD01_02_Main_檢驗一次_繪製畫布(ref int cnt)
        {
            if (CCD01_02_SrcImageHandle != 0)
            {
                this.h_Canvas_Main_CCD01_02_檢測畫面.RefreshCanvas();
            }

            cnt++;
        }
        #endregion
        #region PLC_CCD01_02_計算一次
        PLC_Device PLC_Device_CCD01_02_計算一次 = new PLC_Device("S5015");
        PLC_Device PLC_Device_CCD01_02_計算一次_OK = new PLC_Device("S5016");
        PLC_Device PLC_Device_CCD01_02_計算一次_READY = new PLC_Device("S5017");
        MyTimer MyTimer_CCD01_02_計算一次 = new MyTimer();
        int cnt_Program_CCD01_02_計算一次 = 65534;
        void sub_Program_CCD01_02_計算一次()
        {
            this.PLC_Device_CCD01_02_計算一次_READY.Bool = !this.PLC_Device_CCD01_02_計算一次.Bool;
            if (cnt_Program_CCD01_02_計算一次 == 65534)
            {
                PLC_Device_CCD01_02_計算一次.SetComment("PLC_CCD01_02_計算一次");
                PLC_Device_CCD01_02_計算一次.Bool = false;

                cnt_Program_CCD01_02_計算一次 = 65535;
            }
            if (cnt_Program_CCD01_02_計算一次 == 65535) cnt_Program_CCD01_02_計算一次 = 1;
            if (cnt_Program_CCD01_02_計算一次 == 1) cnt_Program_CCD01_02_計算一次_檢查按下(ref cnt_Program_CCD01_02_計算一次);
            if (cnt_Program_CCD01_02_計算一次 == 2) cnt_Program_CCD01_02_計算一次_初始化(ref cnt_Program_CCD01_02_計算一次);
            if (cnt_Program_CCD01_02_計算一次 == 3) cnt_Program_CCD01_02_計算一次_步驟01開始(ref cnt_Program_CCD01_02_計算一次);
            if (cnt_Program_CCD01_02_計算一次 == 4) cnt_Program_CCD01_02_計算一次_步驟01結束(ref cnt_Program_CCD01_02_計算一次);
            if (cnt_Program_CCD01_02_計算一次 == 5) cnt_Program_CCD01_02_計算一次_步驟02開始(ref cnt_Program_CCD01_02_計算一次);
            if (cnt_Program_CCD01_02_計算一次 == 6) cnt_Program_CCD01_02_計算一次_步驟02結束(ref cnt_Program_CCD01_02_計算一次);
            if (cnt_Program_CCD01_02_計算一次 == 7) cnt_Program_CCD01_02_計算一次_步驟03開始(ref cnt_Program_CCD01_02_計算一次);
            if (cnt_Program_CCD01_02_計算一次 == 8) cnt_Program_CCD01_02_計算一次_步驟03結束(ref cnt_Program_CCD01_02_計算一次);
            if (cnt_Program_CCD01_02_計算一次 == 9) cnt_Program_CCD01_02_計算一次_步驟04開始(ref cnt_Program_CCD01_02_計算一次);
            if (cnt_Program_CCD01_02_計算一次 == 10) cnt_Program_CCD01_02_計算一次_步驟04結束(ref cnt_Program_CCD01_02_計算一次);
            if (cnt_Program_CCD01_02_計算一次 == 11) cnt_Program_CCD01_02_計算一次_步驟05開始(ref cnt_Program_CCD01_02_計算一次);
            if (cnt_Program_CCD01_02_計算一次 == 12) cnt_Program_CCD01_02_計算一次_步驟05結束(ref cnt_Program_CCD01_02_計算一次);
            if (cnt_Program_CCD01_02_計算一次 == 13) cnt_Program_CCD01_02_計算一次_步驟06開始(ref cnt_Program_CCD01_02_計算一次);
            if (cnt_Program_CCD01_02_計算一次 == 14) cnt_Program_CCD01_02_計算一次_步驟06結束(ref cnt_Program_CCD01_02_計算一次);
            if (cnt_Program_CCD01_02_計算一次 == 15) cnt_Program_CCD01_02_計算一次_計算結果(ref cnt_Program_CCD01_02_計算一次);
            if (cnt_Program_CCD01_02_計算一次 == 16) cnt_Program_CCD01_02_計算一次 = 65500;
            if (cnt_Program_CCD01_02_計算一次 > 1) cnt_Program_CCD01_02_計算一次_檢查放開(ref cnt_Program_CCD01_02_計算一次);

            if (cnt_Program_CCD01_02_計算一次 == 65500)
            {
                PLC_Device_CCD01_02_計算一次.Bool = false;
                cnt_Program_CCD01_02_計算一次 = 65535;
            }
        }
        void cnt_Program_CCD01_02_計算一次_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_02_計算一次.Bool) cnt++;
        }
        void cnt_Program_CCD01_02_計算一次_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_02_計算一次.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_02_計算一次_初始化(ref int cnt)
        {
            PLC_Device_CCD01_02_PIN量測_量測框調整.Bool = false;
            PLC_Device_CCD01_02_PIN量測_檢測距離計算.Bool = false;
            PLC_Device_CCD01_02_PIN正位度量測_設定規範位置.Bool = false;
            PLC_Device_CCD01_02_PIN量測_檢測距離計算.Bool = false;
            cnt++;
        }
        void cnt_Program_CCD01_02_計算一次_步驟01開始(ref int cnt)
        {
            this.MyTimer_CCD01_02_計算一次.TickStop();
            this.MyTimer_CCD01_02_計算一次.StartTickTime(99999);
            cnt++;
            
          
        }
        void cnt_Program_CCD01_02_計算一次_步驟01結束(ref int cnt)
        {
            cnt++;
                      
        }
        void cnt_Program_CCD01_02_計算一次_步驟02開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_02_PIN量測點_量測框調整按鈕.Bool)
            {
                this.PLC_Device_CCD01_02_PIN量測點_量測框調整按鈕.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_02_計算一次_步驟02結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_02_PIN量測點_量測框調整按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_02_計算一次_步驟03開始(ref int cnt)
        {
            if (!PLC_Device_CCD01_02_PIN正位度量測_設定規範按鈕.Bool)
            {
                PLC_Device_CCD01_02_PIN正位度量測_設定規範按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_02_計算一次_步驟03結束(ref int cnt)
        {
            if (!PLC_Device_CCD01_02_PIN正位度量測_設定規範按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_02_計算一次_步驟04開始(ref int cnt)
        {
            if (!PLC_Device_CCD01_02_PIN量測_檢測距離計算按鈕.Bool && !PLC_Device_CCD01_02_PIN量測_檢測正位度計算按鈕.Bool)
            {
                PLC_Device_CCD01_02_PIN量測_檢測距離計算按鈕.Bool = true;
                PLC_Device_CCD01_02_PIN量測_檢測正位度計算按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_02_計算一次_步驟04結束(ref int cnt)
        {
            if (!PLC_Device_CCD01_02_PIN量測_檢測距離計算按鈕.Bool && !PLC_Device_CCD01_02_PIN量測_檢測正位度計算按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_02_計算一次_步驟05開始(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_02_計算一次_步驟05結束(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_02_計算一次_步驟06開始(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_02_計算一次_步驟06結束(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_02_計算一次_計算結果(ref int cnt)
        {
            bool flag = true;
            //if (!this.PLC_Device_CCD01_02_PIN量測_量測框調整_OK.Bool) flag = false;
            if (!this.PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool) flag = false;
            if (!this.PLC_Device_CCD01_02_PIN量測_檢測正位度計算_OK.Bool) flag = false;
            this.PLC_Device_CCD01_02_計算一次_OK.Bool = flag;
            this.PLC_Device_CCD01_02_Main_取像並檢驗_OK.Bool = flag;
            //flag_CCD01_02_上端水平度寫入列表資料 = true;
            //flag_CCD01_02_上端間距寫入列表資料 = true;
            //flag_CCD01_02_上端水平度差值寫入列表資料 = true;

            cnt++;
        }





        #endregion


        #region PLC_CCD01_02_PIN量測點量測

        private List<AxOvkBase.AxROIBW8> List_CCD01_02_PIN量測點_AxROIBW8_量測框調整 = new List<AxOvkBase.AxROIBW8>();
        private List<AxOvkBase.AxROIBW8> List_CCD01_02_左側端點_AxROIBW8_量測框調整 = new List<AxOvkBase.AxROIBW8>();
        private List<AxOvkMsr.AxLineMsr> List_CCD01_02_左側端點_AxLineMsr_線量測 = new List<AxOvkMsr.AxLineMsr>();

        private List<AxOvkBase.AxROIBW8> List_CCD01_02_塗黑遮罩_AxROIBW8 = new List<AxOvkBase.AxROIBW8>();
        private AxOvkImage.AxImageSetValue CCD01_02_塗黑 = new AxOvkImage.AxImageSetValue();

        private AxOvkPat.AxVisionInspectionFrame CCD01_02_PIN量測點_AxVisionInspectionFrame_量測框調整;


        private List<PLC_Device> PLC_Device_CCD01_02_量測框OrgX = new List<PLC_Device>();
        private List<PLC_Device> PLC_Device_CCD01_02_量測框OrgY = new List<PLC_Device>();
        private List<PLC_Device> PLC_Device_CCD01_02_量測框Height = new List<PLC_Device>();
        private List<PLC_Device> PLC_Device_CCD01_02_量測框Width = new List<PLC_Device>();

        private List<PLC_Device> PLC_Device_CCD01_02_左側端點OrgX = new List<PLC_Device>();
        private List<PLC_Device> PLC_Device_CCD01_02_左側端點OrgY = new List<PLC_Device>();
        private List<PLC_Device> PLC_Device_CCD01_02_左側端點Height = new List<PLC_Device>();
        private List<PLC_Device> PLC_Device_CCD01_02_左側端點Width = new List<PLC_Device>();


        PLC_Device PLC_Device_CCD01_02_PIN量測點_量測框調整按鈕 = new PLC_Device("S6820");
        PLC_Device PLC_Device_CCD01_02_PIN量測點_量測框調整 = new PLC_Device("S6821");
        PLC_Device PLC_Device_CCD01_02_PIN量測點_量測框調整_OK = new PLC_Device("S6822");
        PLC_Device PLC_Device_CCD01_02_PIN量測點_量測框調整_測試完成 = new PLC_Device("S6823");
        PLC_Device PLC_Device_CCD01_02_PIN量測點_量測框調整_RefreshCanvas = new PLC_Device("S6824");
        #region PIN量測點
        PLC_Device PLC_Device_CCD01_02_PIN端點_變化銳利度 = new PLC_Device("F3400");
        PLC_Device PLC_Device_CCD01_02_PIN端點_延伸變化強度 = new PLC_Device("F3401");
        PLC_Device PLC_Device_CCD01_02_PIN端點_灰階變化面積 = new PLC_Device("F3402");
        PLC_Device PLC_Device_CCD01_02_PIN端點_雜訊抑制 = new PLC_Device("F3403");
        PLC_Device PLC_Device_CCD01_02_PIN端點_垂直量測寬度 = new PLC_Device("F3404");
        PLC_Device PLC_Device_CCD01_02_PIN端點_左端量測顏色變化 = new PLC_Device("F3405");
        PLC_Device PLC_Device_CCD01_02_PIN端點_量測密度間隔 = new PLC_Device("F3406");
        PLC_Device PLC_Device_CCD01_02_PIN端點_最佳回歸線計算次數 = new PLC_Device("F3407");
        PLC_Device PLC_Device_CCD01_02_PIN端點_最佳回歸線濾波 = new PLC_Device("F3408");
        PLC_Device PLC_Device_CCD01_02_PIN端點_量測框架方向 = new PLC_Device("F3409");
        PLC_Device PLC_Device_CCD01_02_PIN端點_右端量測顏色變化 = new PLC_Device("F3410");
        #endregion


        #region PLC_Device_CCD01_02_量測框OrgX
        PLC_Device PLC_Device_CCD01_02_量測框OrgX_PIN01 = new PLC_Device("F3411");
        PLC_Device PLC_Device_CCD01_02_量測框OrgX_PIN02 = new PLC_Device("F3412");
        PLC_Device PLC_Device_CCD01_02_量測框OrgX_PIN03 = new PLC_Device("F3413");
        PLC_Device PLC_Device_CCD01_02_量測框OrgX_PIN04 = new PLC_Device("F3414");
        PLC_Device PLC_Device_CCD01_02_量測框OrgX_PIN05 = new PLC_Device("F3415");
        PLC_Device PLC_Device_CCD01_02_量測框OrgX_PIN06 = new PLC_Device("F3416");
        PLC_Device PLC_Device_CCD01_02_量測框OrgX_PIN07 = new PLC_Device("F3417");
        PLC_Device PLC_Device_CCD01_02_量測框OrgX_PIN08 = new PLC_Device("F3418");
        PLC_Device PLC_Device_CCD01_02_量測框OrgX_PIN09 = new PLC_Device("F3419");
        PLC_Device PLC_Device_CCD01_02_量測框OrgX_PIN10 = new PLC_Device("F3420");
        PLC_Device PLC_Device_CCD01_02_量測框OrgX_上排PIN11 = new PLC_Device("F3701");
        PLC_Device PLC_Device_CCD01_02_量測框OrgX_PIN11 = new PLC_Device("F3421");
        PLC_Device PLC_Device_CCD01_02_量測框OrgX_PIN12 = new PLC_Device("F3422");
        PLC_Device PLC_Device_CCD01_02_量測框OrgX_PIN13 = new PLC_Device("F3423");
        PLC_Device PLC_Device_CCD01_02_量測框OrgX_PIN14 = new PLC_Device("F3424");
        PLC_Device PLC_Device_CCD01_02_量測框OrgX_PIN15 = new PLC_Device("F3425");
        PLC_Device PLC_Device_CCD01_02_量測框OrgX_PIN16 = new PLC_Device("F3426");
        PLC_Device PLC_Device_CCD01_02_量測框OrgX_PIN17 = new PLC_Device("F3427");
        PLC_Device PLC_Device_CCD01_02_量測框OrgX_PIN18 = new PLC_Device("F3428");
        PLC_Device PLC_Device_CCD01_02_量測框OrgX_PIN19 = new PLC_Device("F3429");
        PLC_Device PLC_Device_CCD01_02_量測框OrgX_PIN20 = new PLC_Device("F3430");
        PLC_Device PLC_Device_CCD01_02_量測框OrgX_下排PIN11 = new PLC_Device("F3702");

        #endregion
        #region PLC_Device_CCD01_02_量測框OrgY
        PLC_Device PLC_Device_CCD01_02_量測框OrgY_PIN01 = new PLC_Device("F3431");
        PLC_Device PLC_Device_CCD01_02_量測框OrgY_PIN02 = new PLC_Device("F3432");
        PLC_Device PLC_Device_CCD01_02_量測框OrgY_PIN03 = new PLC_Device("F3433");
        PLC_Device PLC_Device_CCD01_02_量測框OrgY_PIN04 = new PLC_Device("F3434");
        PLC_Device PLC_Device_CCD01_02_量測框OrgY_PIN05 = new PLC_Device("F3435");
        PLC_Device PLC_Device_CCD01_02_量測框OrgY_PIN06 = new PLC_Device("F3436");
        PLC_Device PLC_Device_CCD01_02_量測框OrgY_PIN07 = new PLC_Device("F3437");
        PLC_Device PLC_Device_CCD01_02_量測框OrgY_PIN08 = new PLC_Device("F3438");
        PLC_Device PLC_Device_CCD01_02_量測框OrgY_PIN09 = new PLC_Device("F3439");
        PLC_Device PLC_Device_CCD01_02_量測框OrgY_PIN10 = new PLC_Device("F3440");
        PLC_Device PLC_Device_CCD01_02_量測框OrgY_上排PIN11 = new PLC_Device("F3703");
        PLC_Device PLC_Device_CCD01_02_量測框OrgY_PIN11 = new PLC_Device("F3441");
        PLC_Device PLC_Device_CCD01_02_量測框OrgY_PIN12 = new PLC_Device("F3442");
        PLC_Device PLC_Device_CCD01_02_量測框OrgY_PIN13 = new PLC_Device("F3443");
        PLC_Device PLC_Device_CCD01_02_量測框OrgY_PIN14 = new PLC_Device("F3444");
        PLC_Device PLC_Device_CCD01_02_量測框OrgY_PIN15 = new PLC_Device("F3445");
        PLC_Device PLC_Device_CCD01_02_量測框OrgY_PIN16 = new PLC_Device("F3446");
        PLC_Device PLC_Device_CCD01_02_量測框OrgY_PIN17 = new PLC_Device("F3447");
        PLC_Device PLC_Device_CCD01_02_量測框OrgY_PIN18 = new PLC_Device("F3448");
        PLC_Device PLC_Device_CCD01_02_量測框OrgY_PIN19 = new PLC_Device("F3449");
        PLC_Device PLC_Device_CCD01_02_量測框OrgY_PIN20 = new PLC_Device("F3450");
        PLC_Device PLC_Device_CCD01_02_量測框OrgY_下排PIN11 = new PLC_Device("F3704");
        #endregion
        #region PLC_Device_CCD01_02_量測框Height
        PLC_Device PLC_Device_CCD01_02_量測框Height_PIN01 = new PLC_Device("F3451");
        PLC_Device PLC_Device_CCD01_02_量測框Height_PIN02 = new PLC_Device("F3452");
        PLC_Device PLC_Device_CCD01_02_量測框Height_PIN03 = new PLC_Device("F3453");
        PLC_Device PLC_Device_CCD01_02_量測框Height_PIN04 = new PLC_Device("F3454");
        PLC_Device PLC_Device_CCD01_02_量測框Height_PIN05 = new PLC_Device("F3455");
        PLC_Device PLC_Device_CCD01_02_量測框Height_PIN06 = new PLC_Device("F3456");
        PLC_Device PLC_Device_CCD01_02_量測框Height_PIN07 = new PLC_Device("F3457");
        PLC_Device PLC_Device_CCD01_02_量測框Height_PIN08 = new PLC_Device("F3458");
        PLC_Device PLC_Device_CCD01_02_量測框Height_PIN09 = new PLC_Device("F3459");
        PLC_Device PLC_Device_CCD01_02_量測框Height_PIN10 = new PLC_Device("F3460");
        PLC_Device PLC_Device_CCD01_02_量測框Height_上排PIN11 = new PLC_Device("F3705");
        PLC_Device PLC_Device_CCD01_02_量測框Height_PIN11 = new PLC_Device("F3461");
        PLC_Device PLC_Device_CCD01_02_量測框Height_PIN12 = new PLC_Device("F3462");
        PLC_Device PLC_Device_CCD01_02_量測框Height_PIN13 = new PLC_Device("F3463");
        PLC_Device PLC_Device_CCD01_02_量測框Height_PIN14 = new PLC_Device("F3464");
        PLC_Device PLC_Device_CCD01_02_量測框Height_PIN15 = new PLC_Device("F3465");
        PLC_Device PLC_Device_CCD01_02_量測框Height_PIN16 = new PLC_Device("F3466");
        PLC_Device PLC_Device_CCD01_02_量測框Height_PIN17 = new PLC_Device("F3467");
        PLC_Device PLC_Device_CCD01_02_量測框Height_PIN18 = new PLC_Device("F3468");
        PLC_Device PLC_Device_CCD01_02_量測框Height_PIN19 = new PLC_Device("F3469");
        PLC_Device PLC_Device_CCD01_02_量測框Height_PIN20 = new PLC_Device("F3470");
        PLC_Device PLC_Device_CCD01_02_量測框Height_下排PIN11 = new PLC_Device("F3706");
        #endregion
        #region PLC_Device_CCD01_02_量測框Width
        PLC_Device PLC_Device_CCD01_02_量測框Width_PIN01 = new PLC_Device("F3471");
        PLC_Device PLC_Device_CCD01_02_量測框Width_PIN02 = new PLC_Device("F3472");
        PLC_Device PLC_Device_CCD01_02_量測框Width_PIN03 = new PLC_Device("F3473");
        PLC_Device PLC_Device_CCD01_02_量測框Width_PIN04 = new PLC_Device("F3474");
        PLC_Device PLC_Device_CCD01_02_量測框Width_PIN05 = new PLC_Device("F3475");
        PLC_Device PLC_Device_CCD01_02_量測框Width_PIN06 = new PLC_Device("F3476");
        PLC_Device PLC_Device_CCD01_02_量測框Width_PIN07 = new PLC_Device("F3477");
        PLC_Device PLC_Device_CCD01_02_量測框Width_PIN08 = new PLC_Device("F3478");
        PLC_Device PLC_Device_CCD01_02_量測框Width_PIN09 = new PLC_Device("F3479");
        PLC_Device PLC_Device_CCD01_02_量測框Width_PIN10 = new PLC_Device("F3480");
        PLC_Device PLC_Device_CCD01_02_量測框Width_上排PIN11 = new PLC_Device("F3707");
        PLC_Device PLC_Device_CCD01_02_量測框Width_PIN11 = new PLC_Device("F3481");
        PLC_Device PLC_Device_CCD01_02_量測框Width_PIN12 = new PLC_Device("F3482");
        PLC_Device PLC_Device_CCD01_02_量測框Width_PIN13 = new PLC_Device("F3483");
        PLC_Device PLC_Device_CCD01_02_量測框Width_PIN14 = new PLC_Device("F3484");
        PLC_Device PLC_Device_CCD01_02_量測框Width_PIN15 = new PLC_Device("F3485");
        PLC_Device PLC_Device_CCD01_02_量測框Width_PIN16 = new PLC_Device("F3486");
        PLC_Device PLC_Device_CCD01_02_量測框Width_PIN17 = new PLC_Device("F3487");
        PLC_Device PLC_Device_CCD01_02_量測框Width_PIN18 = new PLC_Device("F3488");
        PLC_Device PLC_Device_CCD01_02_量測框Width_PIN19 = new PLC_Device("F3489");
        PLC_Device PLC_Device_CCD01_02_量測框Width_PIN20 = new PLC_Device("F3490");
        PLC_Device PLC_Device_CCD01_02_量測框Width_下排PIN11 = new PLC_Device("F3708");

        #endregion
        #region PLC_Device_CCD01_02_塗黑遮罩OrgX
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgX_PIN01 = new PLC_Device("F3531");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgX_PIN02 = new PLC_Device("F3532");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgX_PIN03 = new PLC_Device("F3533");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgX_PIN04 = new PLC_Device("F3534");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgX_PIN05 = new PLC_Device("F3535");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgX_PIN06 = new PLC_Device("F3536");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgX_PIN07 = new PLC_Device("F3537");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgX_PIN08 = new PLC_Device("F3538");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgX_PIN09 = new PLC_Device("F3539");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgX_PIN10 = new PLC_Device("F3540");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgX_PIN11 = new PLC_Device("F3541");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgX_PIN12 = new PLC_Device("F3542");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgX_PIN13 = new PLC_Device("F3543");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgX_PIN14 = new PLC_Device("F3544");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgX_PIN15 = new PLC_Device("F3545");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgX_PIN16 = new PLC_Device("F3546");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgX_PIN17 = new PLC_Device("F3547");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgX_PIN18 = new PLC_Device("F3548");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgX_PIN19 = new PLC_Device("F3549");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgX_PIN20 = new PLC_Device("F3450");
        #endregion
        #region PLC_Device_CCD01_02_塗黑遮罩OrgY
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgY_PIN01 = new PLC_Device("F3551");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgY_PIN02 = new PLC_Device("F3552");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgY_PIN03 = new PLC_Device("F3553");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgY_PIN04 = new PLC_Device("F3554");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgY_PIN05 = new PLC_Device("F3555");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgY_PIN06 = new PLC_Device("F3556");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgY_PIN07 = new PLC_Device("F3557");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgY_PIN08 = new PLC_Device("F3558");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgY_PIN09 = new PLC_Device("F3559");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgY_PIN10 = new PLC_Device("F3560");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgY_PIN11 = new PLC_Device("F3561");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgY_PIN12 = new PLC_Device("F3562");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgY_PIN13 = new PLC_Device("F3563");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgY_PIN14 = new PLC_Device("F3564");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgY_PIN15 = new PLC_Device("F3565");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgY_PIN16 = new PLC_Device("F3566");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgY_PIN17 = new PLC_Device("F3567");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgY_PIN18 = new PLC_Device("F3568");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgY_PIN19 = new PLC_Device("F3569");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩OrgY_PIN20 = new PLC_Device("F3570");
        #endregion
        #region PLC_Device_CCD01_02_塗黑遮罩Height
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Height_PIN01 = new PLC_Device("F3571");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Height_PIN02 = new PLC_Device("F3572");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Height_PIN03 = new PLC_Device("F3573");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Height_PIN04 = new PLC_Device("F3574");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Height_PIN05 = new PLC_Device("F3575");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Height_PIN06 = new PLC_Device("F3576");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Height_PIN07 = new PLC_Device("F3577");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Height_PIN08 = new PLC_Device("F3578");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Height_PIN09 = new PLC_Device("F3579");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Height_PIN10 = new PLC_Device("F3580");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Height_PIN11 = new PLC_Device("F3581");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Height_PIN12 = new PLC_Device("F3582");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Height_PIN13 = new PLC_Device("F3583");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Height_PIN14 = new PLC_Device("F3584");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Height_PIN15 = new PLC_Device("F3585");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Height_PIN16 = new PLC_Device("F3586");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Height_PIN17 = new PLC_Device("F3587");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Height_PIN18 = new PLC_Device("F3588");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Height_PIN19 = new PLC_Device("F3589");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Height_PIN20 = new PLC_Device("F3590");
        #endregion
        #region PLC_Device_CCD01_02_塗黑遮罩Width
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Width_PIN01 = new PLC_Device("F3591");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Width_PIN02 = new PLC_Device("F3592");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Width_PIN03 = new PLC_Device("F3593");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Width_PIN04 = new PLC_Device("F3594");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Width_PIN05 = new PLC_Device("F3595");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Width_PIN06 = new PLC_Device("F3596");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Width_PIN07 = new PLC_Device("F3597");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Width_PIN08 = new PLC_Device("F3598");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Width_PIN09 = new PLC_Device("F3599");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Width_PIN10 = new PLC_Device("F3600");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Width_PIN11 = new PLC_Device("F3601");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Width_PIN12 = new PLC_Device("F3602");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Width_PIN13 = new PLC_Device("F3603");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Width_PIN14 = new PLC_Device("F3604");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Width_PIN15 = new PLC_Device("F3605");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Width_PIN16 = new PLC_Device("F3606");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Width_PIN17 = new PLC_Device("F3607");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Width_PIN18 = new PLC_Device("F3608");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Width_PIN19 = new PLC_Device("F3609");
        PLC_Device PLC_Device_CCD01_02_塗黑遮罩Width_PIN20 = new PLC_Device("F3610");
        #endregion

        #region PLC_Device_CCD01_02_左側端點OrgX
        PLC_Device PLC_Device_CCD01_02_左側端點OrgX_PIN01 = new PLC_Device("F3621");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgX_PIN02 = new PLC_Device("F3622");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgX_PIN03 = new PLC_Device("F3623");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgX_PIN04 = new PLC_Device("F3624");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgX_PIN05 = new PLC_Device("F3625");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgX_PIN06 = new PLC_Device("F3626");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgX_PIN07 = new PLC_Device("F3627");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgX_PIN08 = new PLC_Device("F3628");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgX_PIN09 = new PLC_Device("F3629");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgX_PIN10 = new PLC_Device("F3630");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgX_PIN11 = new PLC_Device("F3631");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgX_PIN12 = new PLC_Device("F3632");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgX_PIN13 = new PLC_Device("F3633");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgX_PIN14 = new PLC_Device("F3634");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgX_PIN15 = new PLC_Device("F3635");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgX_PIN16 = new PLC_Device("F3636");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgX_PIN17 = new PLC_Device("F3637");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgX_PIN18 = new PLC_Device("F3638");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgX_PIN19 = new PLC_Device("F3639");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgX_PIN20 = new PLC_Device("F3640");
        #endregion
        #region PLC_Device_CCD01_02_左側端點OrgY
        PLC_Device PLC_Device_CCD01_02_左側端點OrgY_PIN01 = new PLC_Device("F3641");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgY_PIN02 = new PLC_Device("F3642");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgY_PIN03 = new PLC_Device("F3643");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgY_PIN04 = new PLC_Device("F3644");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgY_PIN05 = new PLC_Device("F3645");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgY_PIN06 = new PLC_Device("F3646");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgY_PIN07 = new PLC_Device("F3647");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgY_PIN08 = new PLC_Device("F3648");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgY_PIN09 = new PLC_Device("F3649");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgY_PIN10 = new PLC_Device("F3650");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgY_PIN11 = new PLC_Device("F3651");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgY_PIN12 = new PLC_Device("F3652");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgY_PIN13 = new PLC_Device("F3653");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgY_PIN14 = new PLC_Device("F3654");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgY_PIN15 = new PLC_Device("F3655");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgY_PIN16 = new PLC_Device("F3656");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgY_PIN17 = new PLC_Device("F3657");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgY_PIN18 = new PLC_Device("F3658");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgY_PIN19 = new PLC_Device("F3659");
        PLC_Device PLC_Device_CCD01_02_左側端點OrgY_PIN20 = new PLC_Device("F3660");
        #endregion
        #region PLC_Device_CCD01_02_左側端點Height
        PLC_Device PLC_Device_CCD01_02_左側端點Height_PIN01 = new PLC_Device("F3661");
        PLC_Device PLC_Device_CCD01_02_左側端點Height_PIN02 = new PLC_Device("F3662");
        PLC_Device PLC_Device_CCD01_02_左側端點Height_PIN03 = new PLC_Device("F3663");
        PLC_Device PLC_Device_CCD01_02_左側端點Height_PIN04 = new PLC_Device("F3664");
        PLC_Device PLC_Device_CCD01_02_左側端點Height_PIN05 = new PLC_Device("F3665");
        PLC_Device PLC_Device_CCD01_02_左側端點Height_PIN06 = new PLC_Device("F3666");
        PLC_Device PLC_Device_CCD01_02_左側端點Height_PIN07 = new PLC_Device("F3667");
        PLC_Device PLC_Device_CCD01_02_左側端點Height_PIN08 = new PLC_Device("F3668");
        PLC_Device PLC_Device_CCD01_02_左側端點Height_PIN09 = new PLC_Device("F3669");
        PLC_Device PLC_Device_CCD01_02_左側端點Height_PIN10 = new PLC_Device("F3670");
        PLC_Device PLC_Device_CCD01_02_左側端點Height_PIN11 = new PLC_Device("F3671");
        PLC_Device PLC_Device_CCD01_02_左側端點Height_PIN12 = new PLC_Device("F3672");
        PLC_Device PLC_Device_CCD01_02_左側端點Height_PIN13 = new PLC_Device("F3673");
        PLC_Device PLC_Device_CCD01_02_左側端點Height_PIN14 = new PLC_Device("F3674");
        PLC_Device PLC_Device_CCD01_02_左側端點Height_PIN15 = new PLC_Device("F3675");
        PLC_Device PLC_Device_CCD01_02_左側端點Height_PIN16 = new PLC_Device("F3676");
        PLC_Device PLC_Device_CCD01_02_左側端點Height_PIN17 = new PLC_Device("F3677");
        PLC_Device PLC_Device_CCD01_02_左側端點Height_PIN18 = new PLC_Device("F3678");
        PLC_Device PLC_Device_CCD01_02_左側端點Height_PIN19 = new PLC_Device("F3679");
        PLC_Device PLC_Device_CCD01_02_左側端點Height_PIN20 = new PLC_Device("F3680");
        #endregion
        #region PLC_Device_CCD01_02_左側端點Width
        PLC_Device PLC_Device_CCD01_02_左側端點Width_PIN01 = new PLC_Device("F3681");
        PLC_Device PLC_Device_CCD01_02_左側端點Width_PIN02 = new PLC_Device("F3682");
        PLC_Device PLC_Device_CCD01_02_左側端點Width_PIN03 = new PLC_Device("F3683");
        PLC_Device PLC_Device_CCD01_02_左側端點Width_PIN04 = new PLC_Device("F3684");
        PLC_Device PLC_Device_CCD01_02_左側端點Width_PIN05 = new PLC_Device("F3685");
        PLC_Device PLC_Device_CCD01_02_左側端點Width_PIN06 = new PLC_Device("F3686");
        PLC_Device PLC_Device_CCD01_02_左側端點Width_PIN07 = new PLC_Device("F3687");
        PLC_Device PLC_Device_CCD01_02_左側端點Width_PIN08 = new PLC_Device("F3688");
        PLC_Device PLC_Device_CCD01_02_左側端點Width_PIN09 = new PLC_Device("F3689");
        PLC_Device PLC_Device_CCD01_02_左側端點Width_PIN10 = new PLC_Device("F3690");
        PLC_Device PLC_Device_CCD01_02_左側端點Width_PIN11 = new PLC_Device("F3691");
        PLC_Device PLC_Device_CCD01_02_左側端點Width_PIN12 = new PLC_Device("F3692");
        PLC_Device PLC_Device_CCD01_02_左側端點Width_PIN13 = new PLC_Device("F3693");
        PLC_Device PLC_Device_CCD01_02_左側端點Width_PIN14 = new PLC_Device("F3694");
        PLC_Device PLC_Device_CCD01_02_左側端點Width_PIN15 = new PLC_Device("F3695");
        PLC_Device PLC_Device_CCD01_02_左側端點Width_PIN16 = new PLC_Device("F3696");
        PLC_Device PLC_Device_CCD01_02_左側端點Width_PIN17 = new PLC_Device("F3697");
        PLC_Device PLC_Device_CCD01_02_左側端點Width_PIN18 = new PLC_Device("F3698");
        PLC_Device PLC_Device_CCD01_02_左側端點Width_PIN19 = new PLC_Device("F3699");
        PLC_Device PLC_Device_CCD01_02_左側端點Width_PIN20 = new PLC_Device("F3700");
        #endregion


        private PointF[] List_CCD01_02_PIN量測點參數_量測點 = new PointF[22];
        private PointF[] List_CCD01_02_PIN量測點參數_量測點_結果 = new PointF[22];
        private Point[] List_CCD01_02_PIN量測點參數_量測點_轉換後座標 = new Point[22];
        private bool[] List_CCD01_02_PIN量測點參數_量測點_有無 = new bool[22];

        int cnt_Program_CCD01_02_PIN量測點量測 = 65534;
        void sub_Program_CCD01_02_PIN量測點量測()
        {
            if (cnt_Program_CCD01_02_PIN量測點量測 == 65534)
            {
                this.h_Canvas_Tech_CCD01_02.OnCanvasMouseDownEvent += H_Canvas_Tech_CCD01_02_PIN量測點量測_OnCanvasMouseDownEvent;
                this.h_Canvas_Tech_CCD01_02.OnCanvasMouseMoveEvent += H_Canvas_Tech_CCD01_02_PIN量測點量測_OnCanvasMouseMoveEvent;
                this.h_Canvas_Tech_CCD01_02.OnCanvasMouseUpEvent += H_Canvas_Tech_CCD01_02_PIN量測點量測_OnCanvasMouseUpEvent;
                this.h_Canvas_Tech_CCD01_02.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_02_PIN量測點量測_OnCanvasDrawEvent;
                this.h_Canvas_Main_CCD01_02_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_02_PIN量測點量測_OnCanvasDrawEvent;
                #region Add List
                this.PLC_Device_CCD01_02_量測框OrgX.Add(PLC_Device_CCD01_02_量測框OrgX_PIN01);
                this.PLC_Device_CCD01_02_量測框OrgX.Add(PLC_Device_CCD01_02_量測框OrgX_PIN02);
                this.PLC_Device_CCD01_02_量測框OrgX.Add(PLC_Device_CCD01_02_量測框OrgX_PIN03);
                this.PLC_Device_CCD01_02_量測框OrgX.Add(PLC_Device_CCD01_02_量測框OrgX_PIN04);
                this.PLC_Device_CCD01_02_量測框OrgX.Add(PLC_Device_CCD01_02_量測框OrgX_PIN05);
                this.PLC_Device_CCD01_02_量測框OrgX.Add(PLC_Device_CCD01_02_量測框OrgX_PIN06);
                this.PLC_Device_CCD01_02_量測框OrgX.Add(PLC_Device_CCD01_02_量測框OrgX_PIN07);
                this.PLC_Device_CCD01_02_量測框OrgX.Add(PLC_Device_CCD01_02_量測框OrgX_PIN08);
                this.PLC_Device_CCD01_02_量測框OrgX.Add(PLC_Device_CCD01_02_量測框OrgX_PIN09);
                this.PLC_Device_CCD01_02_量測框OrgX.Add(PLC_Device_CCD01_02_量測框OrgX_PIN10);
                this.PLC_Device_CCD01_02_量測框OrgX.Add(PLC_Device_CCD01_02_量測框OrgX_上排PIN11);
                this.PLC_Device_CCD01_02_量測框OrgX.Add(PLC_Device_CCD01_02_量測框OrgX_PIN11);
                this.PLC_Device_CCD01_02_量測框OrgX.Add(PLC_Device_CCD01_02_量測框OrgX_PIN12);
                this.PLC_Device_CCD01_02_量測框OrgX.Add(PLC_Device_CCD01_02_量測框OrgX_PIN13);
                this.PLC_Device_CCD01_02_量測框OrgX.Add(PLC_Device_CCD01_02_量測框OrgX_PIN14);
                this.PLC_Device_CCD01_02_量測框OrgX.Add(PLC_Device_CCD01_02_量測框OrgX_PIN15);
                this.PLC_Device_CCD01_02_量測框OrgX.Add(PLC_Device_CCD01_02_量測框OrgX_PIN16);
                this.PLC_Device_CCD01_02_量測框OrgX.Add(PLC_Device_CCD01_02_量測框OrgX_PIN17);
                this.PLC_Device_CCD01_02_量測框OrgX.Add(PLC_Device_CCD01_02_量測框OrgX_PIN18);
                this.PLC_Device_CCD01_02_量測框OrgX.Add(PLC_Device_CCD01_02_量測框OrgX_PIN19);
                this.PLC_Device_CCD01_02_量測框OrgX.Add(PLC_Device_CCD01_02_量測框OrgX_PIN20);
                this.PLC_Device_CCD01_02_量測框OrgX.Add(PLC_Device_CCD01_02_量測框OrgX_下排PIN11);

                this.PLC_Device_CCD01_02_量測框OrgY.Add(PLC_Device_CCD01_02_量測框OrgY_PIN01);
                this.PLC_Device_CCD01_02_量測框OrgY.Add(PLC_Device_CCD01_02_量測框OrgY_PIN02);
                this.PLC_Device_CCD01_02_量測框OrgY.Add(PLC_Device_CCD01_02_量測框OrgY_PIN03);
                this.PLC_Device_CCD01_02_量測框OrgY.Add(PLC_Device_CCD01_02_量測框OrgY_PIN04);
                this.PLC_Device_CCD01_02_量測框OrgY.Add(PLC_Device_CCD01_02_量測框OrgY_PIN05);
                this.PLC_Device_CCD01_02_量測框OrgY.Add(PLC_Device_CCD01_02_量測框OrgY_PIN06);
                this.PLC_Device_CCD01_02_量測框OrgY.Add(PLC_Device_CCD01_02_量測框OrgY_PIN07);
                this.PLC_Device_CCD01_02_量測框OrgY.Add(PLC_Device_CCD01_02_量測框OrgY_PIN08);
                this.PLC_Device_CCD01_02_量測框OrgY.Add(PLC_Device_CCD01_02_量測框OrgY_PIN09);
                this.PLC_Device_CCD01_02_量測框OrgY.Add(PLC_Device_CCD01_02_量測框OrgY_PIN10);
                this.PLC_Device_CCD01_02_量測框OrgY.Add(PLC_Device_CCD01_02_量測框OrgY_上排PIN11);
                this.PLC_Device_CCD01_02_量測框OrgY.Add(PLC_Device_CCD01_02_量測框OrgY_PIN11);
                this.PLC_Device_CCD01_02_量測框OrgY.Add(PLC_Device_CCD01_02_量測框OrgY_PIN12);
                this.PLC_Device_CCD01_02_量測框OrgY.Add(PLC_Device_CCD01_02_量測框OrgY_PIN13);
                this.PLC_Device_CCD01_02_量測框OrgY.Add(PLC_Device_CCD01_02_量測框OrgY_PIN14);
                this.PLC_Device_CCD01_02_量測框OrgY.Add(PLC_Device_CCD01_02_量測框OrgY_PIN15);
                this.PLC_Device_CCD01_02_量測框OrgY.Add(PLC_Device_CCD01_02_量測框OrgY_PIN16);
                this.PLC_Device_CCD01_02_量測框OrgY.Add(PLC_Device_CCD01_02_量測框OrgY_PIN17);
                this.PLC_Device_CCD01_02_量測框OrgY.Add(PLC_Device_CCD01_02_量測框OrgY_PIN18);
                this.PLC_Device_CCD01_02_量測框OrgY.Add(PLC_Device_CCD01_02_量測框OrgY_PIN19);
                this.PLC_Device_CCD01_02_量測框OrgY.Add(PLC_Device_CCD01_02_量測框OrgY_PIN20);
                this.PLC_Device_CCD01_02_量測框OrgY.Add(PLC_Device_CCD01_02_量測框OrgY_下排PIN11);

                this.PLC_Device_CCD01_02_量測框Height.Add(PLC_Device_CCD01_02_量測框Height_PIN01);
                this.PLC_Device_CCD01_02_量測框Height.Add(PLC_Device_CCD01_02_量測框Height_PIN02);
                this.PLC_Device_CCD01_02_量測框Height.Add(PLC_Device_CCD01_02_量測框Height_PIN03);
                this.PLC_Device_CCD01_02_量測框Height.Add(PLC_Device_CCD01_02_量測框Height_PIN04);
                this.PLC_Device_CCD01_02_量測框Height.Add(PLC_Device_CCD01_02_量測框Height_PIN05);
                this.PLC_Device_CCD01_02_量測框Height.Add(PLC_Device_CCD01_02_量測框Height_PIN06);
                this.PLC_Device_CCD01_02_量測框Height.Add(PLC_Device_CCD01_02_量測框Height_PIN07);
                this.PLC_Device_CCD01_02_量測框Height.Add(PLC_Device_CCD01_02_量測框Height_PIN08);
                this.PLC_Device_CCD01_02_量測框Height.Add(PLC_Device_CCD01_02_量測框Height_PIN09);
                this.PLC_Device_CCD01_02_量測框Height.Add(PLC_Device_CCD01_02_量測框Height_PIN10);
                this.PLC_Device_CCD01_02_量測框Height.Add(PLC_Device_CCD01_02_量測框Height_上排PIN11);
                this.PLC_Device_CCD01_02_量測框Height.Add(PLC_Device_CCD01_02_量測框Height_PIN11);
                this.PLC_Device_CCD01_02_量測框Height.Add(PLC_Device_CCD01_02_量測框Height_PIN12);
                this.PLC_Device_CCD01_02_量測框Height.Add(PLC_Device_CCD01_02_量測框Height_PIN13);
                this.PLC_Device_CCD01_02_量測框Height.Add(PLC_Device_CCD01_02_量測框Height_PIN14);
                this.PLC_Device_CCD01_02_量測框Height.Add(PLC_Device_CCD01_02_量測框Height_PIN15);
                this.PLC_Device_CCD01_02_量測框Height.Add(PLC_Device_CCD01_02_量測框Height_PIN16);
                this.PLC_Device_CCD01_02_量測框Height.Add(PLC_Device_CCD01_02_量測框Height_PIN17);
                this.PLC_Device_CCD01_02_量測框Height.Add(PLC_Device_CCD01_02_量測框Height_PIN18);
                this.PLC_Device_CCD01_02_量測框Height.Add(PLC_Device_CCD01_02_量測框Height_PIN19);
                this.PLC_Device_CCD01_02_量測框Height.Add(PLC_Device_CCD01_02_量測框Height_PIN20);
                this.PLC_Device_CCD01_02_量測框Height.Add(PLC_Device_CCD01_02_量測框Height_下排PIN11);

                this.PLC_Device_CCD01_02_量測框Width.Add(PLC_Device_CCD01_02_量測框Width_PIN01);
                this.PLC_Device_CCD01_02_量測框Width.Add(PLC_Device_CCD01_02_量測框Width_PIN02);
                this.PLC_Device_CCD01_02_量測框Width.Add(PLC_Device_CCD01_02_量測框Width_PIN03);
                this.PLC_Device_CCD01_02_量測框Width.Add(PLC_Device_CCD01_02_量測框Width_PIN04);
                this.PLC_Device_CCD01_02_量測框Width.Add(PLC_Device_CCD01_02_量測框Width_PIN05);
                this.PLC_Device_CCD01_02_量測框Width.Add(PLC_Device_CCD01_02_量測框Width_PIN06);
                this.PLC_Device_CCD01_02_量測框Width.Add(PLC_Device_CCD01_02_量測框Width_PIN07);
                this.PLC_Device_CCD01_02_量測框Width.Add(PLC_Device_CCD01_02_量測框Width_PIN08);
                this.PLC_Device_CCD01_02_量測框Width.Add(PLC_Device_CCD01_02_量測框Width_PIN09);
                this.PLC_Device_CCD01_02_量測框Width.Add(PLC_Device_CCD01_02_量測框Width_PIN10);
                this.PLC_Device_CCD01_02_量測框Width.Add(PLC_Device_CCD01_02_量測框Width_上排PIN11);
                this.PLC_Device_CCD01_02_量測框Width.Add(PLC_Device_CCD01_02_量測框Width_PIN11);
                this.PLC_Device_CCD01_02_量測框Width.Add(PLC_Device_CCD01_02_量測框Width_PIN12);
                this.PLC_Device_CCD01_02_量測框Width.Add(PLC_Device_CCD01_02_量測框Width_PIN13);
                this.PLC_Device_CCD01_02_量測框Width.Add(PLC_Device_CCD01_02_量測框Width_PIN14);
                this.PLC_Device_CCD01_02_量測框Width.Add(PLC_Device_CCD01_02_量測框Width_PIN15);
                this.PLC_Device_CCD01_02_量測框Width.Add(PLC_Device_CCD01_02_量測框Width_PIN16);
                this.PLC_Device_CCD01_02_量測框Width.Add(PLC_Device_CCD01_02_量測框Width_PIN17);
                this.PLC_Device_CCD01_02_量測框Width.Add(PLC_Device_CCD01_02_量測框Width_PIN18);
                this.PLC_Device_CCD01_02_量測框Width.Add(PLC_Device_CCD01_02_量測框Width_PIN19);
                this.PLC_Device_CCD01_02_量測框Width.Add(PLC_Device_CCD01_02_量測框Width_PIN20);
                this.PLC_Device_CCD01_02_量測框Width.Add(PLC_Device_CCD01_02_量測框Width_下排PIN11);

                #endregion

                PLC_Device_CCD01_02_PIN量測點_量測框調整按鈕.Bool = false;
                PLC_Device_CCD01_02_PIN量測點_量測框調整.Bool = false;
                cnt_Program_CCD01_02_PIN量測點量測 = 65535;
            }
            if (cnt_Program_CCD01_02_PIN量測點量測 == 65535) cnt_Program_CCD01_02_PIN量測點量測 = 1;
            if (cnt_Program_CCD01_02_PIN量測點量測 == 1) cnt_Program_CCD01_02_PIN量測點量測_檢查按下(ref cnt_Program_CCD01_02_PIN量測點量測);
            if (cnt_Program_CCD01_02_PIN量測點量測 == 2) cnt_Program_CCD01_02_PIN量測點量測_初始化(ref cnt_Program_CCD01_02_PIN量測點量測);
            if (cnt_Program_CCD01_02_PIN量測點量測 == 3) cnt_Program_CCD01_02_PIN量測點量測_座標轉換(ref cnt_Program_CCD01_02_PIN量測點量測);
            if (cnt_Program_CCD01_02_PIN量測點量測 == 4) cnt_Program_CCD01_02_PIN量測點量測_讀取參數(ref cnt_Program_CCD01_02_PIN量測點量測);
            if (cnt_Program_CCD01_02_PIN量測點量測 == 5) cnt_Program_CCD01_02_PIN量測點量測_開始點量測(ref cnt_Program_CCD01_02_PIN量測點量測);
            if (cnt_Program_CCD01_02_PIN量測點量測 == 6) cnt_Program_CCD01_02_PIN量測點量測_定位量測點(ref cnt_Program_CCD01_02_PIN量測點量測);
            if (cnt_Program_CCD01_02_PIN量測點量測 == 7) cnt_Program_CCD01_02_PIN量測點量測_繪製畫布(ref cnt_Program_CCD01_02_PIN量測點量測);
            if (cnt_Program_CCD01_02_PIN量測點量測 == 8) cnt_Program_CCD01_02_PIN量測點量測 = 65500;
            if (cnt_Program_CCD01_02_PIN量測點量測 > 1) cnt_Program_CCD01_02_PIN量測點量測_檢查放開(ref cnt_Program_CCD01_02_PIN量測點量測);

            if (cnt_Program_CCD01_02_PIN量測點量測 == 65500)
            {
                if (PLC_Device_CCD01_02_計算一次.Bool)
                {
                    PLC_Device_CCD01_02_PIN量測點_量測框調整按鈕.Bool = false;
                    PLC_Device_CCD01_02_PIN量測點_量測框調整.Bool = false;
                }
                cnt_Program_CCD01_02_PIN量測點量測 = 65535;
            }
        }
        AxOvkBase.TxAxHitHandle[] CCD01_02_PIN量測點_AxROIBW8_TxAxHitHandle = new AxOvkBase.TxAxHitHandle[22];
        bool[] flag_CCD01_02_PIN量測點_AxROIBW8_MouseDown = new bool[22];
        bool[] flag_CCD01_02_左側端點_AxROIBW8_MouseDown = new bool[22];
        bool[] flag_CCD01_02_右側端點_AxROIBW8_MouseDown = new bool[22];
        private void H_Canvas_Tech_CCD01_02_PIN量測點量測_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {
            if (PLC_Device_CCD01_02_Main_取像並檢驗.Bool || PLC_Device_CCD01_02_PLC觸發檢測.Bool || PLC_Device_CCD01_02_Main_檢驗一次按鈕.Bool)
            {
                try
                {
                    Graphics g = Graphics.FromHdc((IntPtr)HDC);

                    for (int i = 0; i < 22; i++)
                    {

                        if (plC_CheckBox_CCD01_02_線段量測點_繪製量測線段.Checked)
                        {
                            this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].DrawFittedPrimitives(HDC, ZoomX, ZoomY, 0, 0);
                        }

                        DrawingClass.Draw.十字中心(this.List_CCD01_02_PIN量測點參數_量測點[i], 50, Color.Red, 2, g, ZoomX, ZoomY);

                    }
                    g.Dispose();
                    g = null;
                }
                catch
                {

                }

            }
            else if (PLC_Device_CCD01_02_Tech_檢驗一次.Bool || PLC_Device_CCD01_02_Tech_取像並檢驗.Bool)
            {
                if (this.PLC_Device_CCD01_02_PIN量測點_量測框調整_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);

                        for (int i = 0; i < 22; i++)
                        {
                            if (plC_CheckBox_CCD01_02_線段量測點_繪製量測線段.Checked)
                            {
                                this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].DrawFittedPrimitives(HDC, ZoomX, ZoomY, 0, 0);
                            }
                            DrawingClass.Draw.十字中心(this.List_CCD01_02_PIN量測點參數_量測點[i], 50, Color.Red, 2, g, ZoomX, ZoomY);

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
                if (this.PLC_Device_CCD01_02_PIN量測點_量測框調整_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        PointF po_str_PIN到基準Y = new PointF(200, 250);
                        Font font = new Font("微軟正黑體", 10);

                        for (int i = 0; i < 22; i++)
                        {
                            if(plC_CheckBox_CCD01_02_PIN線段量測點_繪製量測框.Checked)
                            {
                                this.List_CCD01_02_PIN量測點_AxROIBW8_量測框調整[i].Title = string.Format("P{0}", (i + 1).ToString("00"));
                                this.List_CCD01_02_PIN量測點_AxROIBW8_量測框調整[i].ShowPlacement = false;
                                this.List_CCD01_02_PIN量測點_AxROIBW8_量測框調整[i].DrawFrame(HDC, ZoomX, ZoomY, 0, 0, 0xFFFF00);
                                this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].DrawFrame(HDC, ZoomX, ZoomY, 0, 0);
                            }

                            if (plC_CheckBox_CCD01_02_線段量測點_繪製量測線段.Checked)
                            {
                                this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].DrawFittedPrimitives(HDC, ZoomX, ZoomY, 0, 0);
                            }
                            if (plC_CheckBox_CCD01_02_線段量測點_繪製量測點.Checked)
                            {
                                this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].DrawPoints(HDC, ZoomX, ZoomY, 0, 0);
                            }
                            DrawingClass.Draw.十字中心(this.List_CCD01_02_PIN量測點參數_量測點[i], 50, Color.Red, 2, g, ZoomX, ZoomY);
                            
                            //this.List_CCD01_02_塗黑遮罩_AxROIBW8[i].DrawFrame(HDC, ZoomX, ZoomY, 0, 0, 0xFF0000);

                        }
                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }
            }

            this.PLC_Device_CCD01_02_PIN量測點_量測框調整_RefreshCanvas.Bool = false;


        }
        private void H_Canvas_Tech_CCD01_02_PIN量測點量測_OnCanvasMouseDownEvent(int x, int y, float ZoomX, float ZoomY, ref int InUsedEventNum, int InUsedCanvasHandle)
        {
            if (this.PLC_Device_CCD01_02_PIN量測點_量測框調整.Bool)
            {
                for (int i = 0; i < 22; i++)
                {
                    this.CCD01_02_PIN量測點_AxROIBW8_TxAxHitHandle[i] = this.List_CCD01_02_PIN量測點_AxROIBW8_量測框調整[i].HitTest(x, y, ZoomX, ZoomY, 0, 0);
                    if (this.CCD01_02_PIN量測點_AxROIBW8_TxAxHitHandle[i] != AxOvkBase.TxAxHitHandle.AX_HANDLE_NONE)
                    {
                        this.flag_CCD01_02_PIN量測點_AxROIBW8_MouseDown[i] = true;
                        InUsedEventNum = 10;
                        return;
                    }
                }

            }

        }
        private void H_Canvas_Tech_CCD01_02_PIN量測點量測_OnCanvasMouseMoveEvent(int x, int y, float ZoomX, float ZoomY)
        {
            for (int i = 0; i < 22; i++)
            {
                if (this.flag_CCD01_02_PIN量測點_AxROIBW8_MouseDown[i])
                {
                    this.List_CCD01_02_PIN量測點_AxROIBW8_量測框調整[i].DragROI(this.CCD01_02_PIN量測點_AxROIBW8_TxAxHitHandle[i], x, y, ZoomX, ZoomY, 0, 0);
                   // this.List_CCD01_02_左側端點_AxROIBW8_量測框調整[i].DragROI(this.CCD01_02_左側端點_AxROIBW8_TxAxHitHandle[i], x, y, ZoomX, ZoomY, 0, 0);
                    this.PLC_Device_CCD01_02_量測框OrgX[i].Value = this.List_CCD01_02_PIN量測點_AxROIBW8_量測框調整[i].OrgX;
                    this.PLC_Device_CCD01_02_量測框OrgY[i].Value = this.List_CCD01_02_PIN量測點_AxROIBW8_量測框調整[i].OrgY;
                    this.PLC_Device_CCD01_02_量測框Width[i].Value = this.List_CCD01_02_PIN量測點_AxROIBW8_量測框調整[i].ROIWidth;
                    this.PLC_Device_CCD01_02_量測框Height[i].Value = this.List_CCD01_02_PIN量測點_AxROIBW8_量測框調整[i].ROIHeight;


                }

            }

        }
        private void H_Canvas_Tech_CCD01_02_PIN量測點量測_OnCanvasMouseUpEvent(int x, int y, float ZoomX, float ZoomY)
        {
            for (int i = 0; i < 22; i++)
            {
                this.flag_CCD01_02_PIN量測點_AxROIBW8_MouseDown[i] = false;
            }
        }

        void cnt_Program_CCD01_02_PIN量測點量測_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_02_PIN量測點_量測框調整按鈕.Bool || PLC_Device_CCD01_02_計算一次.Bool)
            {
                PLC_Device_CCD01_02_PIN量測點_量測框調整.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_02_PIN量測點量測_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_02_PIN量測點_量測框調整.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_02_PIN量測點量測_初始化(ref int cnt)
        {
            this.List_CCD01_02_PIN量測點參數_量測點 = new PointF[22];
            this.List_CCD01_02_PIN量測點參數_量測點_結果 = new PointF[22];
            this.List_CCD01_02_PIN量測點參數_量測點_轉換後座標 = new Point[22];
            this.List_CCD01_02_PIN量測點參數_量測點_有無 = new bool[22];
            cnt++;
        }
        void cnt_Program_CCD01_02_PIN量測點量測_座標轉換(ref int cnt)
        {
            if (PLC_Device_CCD01_02_計算一次.Bool)
            {
                CCD01_02_PIN量測點_AxVisionInspectionFrame_量測框調整.RefPointX = PLC_Device_CCD01_01_水平基準線量測_量測中心_X.Value;
                CCD01_02_PIN量測點_AxVisionInspectionFrame_量測框調整.RefPointY = PLC_Device_CCD01_01_水平基準線量測_量測中心_Y.Value;
                CCD01_02_PIN量測點_AxVisionInspectionFrame_量測框調整.RefAngle = 0;
                CCD01_02_PIN量測點_AxVisionInspectionFrame_量測框調整.CurrentRefPointX = Point_CCD01_01_中心基準座標_量測點.X;
                CCD01_02_PIN量測點_AxVisionInspectionFrame_量測框調整.CurrentRefPointY = Point_CCD01_01_中心基準座標_量測點.Y;
                CCD01_02_PIN量測點_AxVisionInspectionFrame_量測框調整.CurrentRefAngle = 0;
                CCD01_02_PIN量測點_AxVisionInspectionFrame_量測框調整.NumOfVisionPoints = 22;



                for (int j = 0; j < 22; j++)
                {
                    if (this.PLC_Device_CCD01_02_量測框OrgX[j].Value == 0) this.PLC_Device_CCD01_02_量測框OrgX[j].Value = 100;
                    if (this.PLC_Device_CCD01_02_量測框OrgY[j].Value == 0) this.PLC_Device_CCD01_02_量測框OrgY[j].Value = 100;
                    if (this.PLC_Device_CCD01_02_量測框Width[j].Value == 0) this.PLC_Device_CCD01_02_量測框Width[j].Value = 100;
                    if (this.PLC_Device_CCD01_02_量測框Height[j].Value == 0) this.PLC_Device_CCD01_02_量測框Height[j].Value = 100;

                    CCD01_02_PIN量測點_AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
                    CCD01_02_PIN量測點_AxVisionInspectionFrame_量測框調整.VisionPointX = this.PLC_Device_CCD01_02_量測框OrgX[j].Value;
                    CCD01_02_PIN量測點_AxVisionInspectionFrame_量測框調整.VisionPointY = this.PLC_Device_CCD01_02_量測框OrgY[j].Value;
                }
                CCD01_02_PIN量測點_AxVisionInspectionFrame_量測框調整.EstimateCurrentVisionPoints();
                for (int j = 0; j < 22; j++)
                {
                    CCD01_02_PIN量測點_AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
                    List_CCD01_02_PIN量測點參數_量測點_轉換後座標[j].X = (int)CCD01_02_PIN量測點_AxVisionInspectionFrame_量測框調整.CurrentVisionPointX;
                    List_CCD01_02_PIN量測點參數_量測點_轉換後座標[j].Y = (int)CCD01_02_PIN量測點_AxVisionInspectionFrame_量測框調整.CurrentVisionPointY;
                }

            }
            cnt++;

        }
        void cnt_Program_CCD01_02_PIN量測點量測_讀取參數(ref int cnt)
        {
            for (int i = 0; i < 22; i++)
            {
                if (this.PLC_Device_CCD01_02_量測框OrgX[i].Value > 2596) this.PLC_Device_CCD01_02_量測框OrgX[i].Value = 0;
                if (this.PLC_Device_CCD01_02_量測框OrgY[i].Value > 1922) this.PLC_Device_CCD01_02_量測框OrgY[i].Value = 0;
                if (this.PLC_Device_CCD01_02_量測框OrgX[i].Value < 0) this.PLC_Device_CCD01_02_量測框OrgX[i].Value = 0;
                if (this.PLC_Device_CCD01_02_量測框OrgY[i].Value < 0) this.PLC_Device_CCD01_02_量測框OrgY[i].Value = 0;

                if (this.List_CCD01_02_PIN量測點參數_量測點_轉換後座標[i].X > 2596) this.List_CCD01_02_PIN量測點參數_量測點_轉換後座標[i].X = 0;
                if (this.List_CCD01_02_PIN量測點參數_量測點_轉換後座標[i].Y > 1922) this.List_CCD01_02_PIN量測點參數_量測點_轉換後座標[i].Y = 0;
                if (this.List_CCD01_02_PIN量測點參數_量測點_轉換後座標[i].X < 0) this.List_CCD01_02_PIN量測點參數_量測點_轉換後座標[i].X = 0;
                if (this.List_CCD01_02_PIN量測點參數_量測點_轉換後座標[i].Y < 0) this.List_CCD01_02_PIN量測點參數_量測點_轉換後座標[i].Y = 0;
            }
            for (int i = 0; i < 22; i++)
            {
                this.List_CCD01_02_PIN量測點_AxROIBW8_量測框調整[i].ParentHandle = this.CCD01_02_SrcImageHandle;
                if (PLC_Device_CCD01_02_計算一次.Bool)
                {
                    this.List_CCD01_02_PIN量測點_AxROIBW8_量測框調整[i].OrgX = List_CCD01_02_PIN量測點參數_量測點_轉換後座標[i].X;
                    this.List_CCD01_02_PIN量測點_AxROIBW8_量測框調整[i].OrgY = List_CCD01_02_PIN量測點參數_量測點_轉換後座標[i].Y;
                }
                else
                {
                    this.List_CCD01_02_PIN量測點_AxROIBW8_量測框調整[i].OrgX = this.PLC_Device_CCD01_02_量測框OrgX[i].Value;
                    this.List_CCD01_02_PIN量測點_AxROIBW8_量測框調整[i].OrgY = this.PLC_Device_CCD01_02_量測框OrgY[i].Value;
                }

                this.List_CCD01_02_PIN量測點_AxROIBW8_量測框調整[i].ROIWidth = this.PLC_Device_CCD01_02_量測框Width[i].Value;
                this.List_CCD01_02_PIN量測點_AxROIBW8_量測框調整[i].ROIHeight = this.PLC_Device_CCD01_02_量測框Height[i].Value;
                this.List_CCD01_02_PIN量測點_AxROIBW8_量測框調整[i].SkewAngle = 0;
            }


            cnt++;
        }
        void cnt_Program_CCD01_02_PIN量測點量測_開始點量測(ref int cnt)
        {

            for (int i = 0; i < 22; i++)
            {

                //this.CCD01_02_塗黑.SrcImageHandle = this.List_CCD01_02_塗黑遮罩_AxROIBW8[i].VegaHandle;
                //this.List_CCD01_02_塗黑遮罩_AxROIBW8[i].ROIWidth = this.PLC_Device_CCD01_02_量測框Width[i].Value / 2;
                //this.List_CCD01_02_塗黑遮罩_AxROIBW8[i].ROIHeight = this.PLC_Device_CCD01_02_量測框Height[i].Value / 2;
                //this.List_CCD01_02_塗黑遮罩_AxROIBW8[i].SkewAngle = 0;
                //this.CCD01_02_塗黑.Greylevel = 0;
                //this.CCD01_02_塗黑.SetValue();


                this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].SrcImageHandle = this.CCD01_02_SrcImageHandle;
                this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].Hysteresis = PLC_Device_CCD01_02_PIN端點_延伸變化強度.Value;
                this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].DeriThreshold = PLC_Device_CCD01_02_PIN端點_變化銳利度.Value;
                this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].MinGreyStep = PLC_Device_CCD01_02_PIN端點_灰階變化面積.Value;
                this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].SmoothFactor = PLC_Device_CCD01_02_PIN端點_雜訊抑制.Value;
                this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].HalfProfileThickness = PLC_Device_CCD01_02_PIN端點_垂直量測寬度.Value;
                this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].SampleStep = PLC_Device_CCD01_02_PIN端點_量測密度間隔.Value;
                this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].FilterCount = PLC_Device_CCD01_02_PIN端點_最佳回歸線計算次數.Value;
                this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].FilterThreshold = PLC_Device_CCD01_02_PIN端點_最佳回歸線濾波.Value / 10;
                this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].HalfHeight = this.PLC_Device_CCD01_02_量測框Width[i].Value / 2;
                this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].NLineStartX = this.PLC_Device_CCD01_02_量測框OrgX[i].Value + this.PLC_Device_CCD01_02_量測框Width[i].Value / 2;
                this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].NLineStartY = this.PLC_Device_CCD01_02_量測框OrgY[i].Value;
                this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].NLineEndX = this.PLC_Device_CCD01_02_量測框OrgX[i].Value + this.PLC_Device_CCD01_02_量測框Width[i].Value / 2;
                this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].NLineEndY = this.PLC_Device_CCD01_02_量測框OrgY[i].Value + this.PLC_Device_CCD01_02_量測框Height[i].Value;

                this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].EdgeType = (AxOvkMsr.TxAxTransitionType)PLC_Device_CCD01_02_PIN端點_左端量測顏色變化.Value;
                this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].LockedMsrDirection = (AxOvkMsr.TxAxLineMsrLockedMsrDirection)PLC_Device_CCD01_02_PIN端點_量測框架方向.Value; //右
                this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].DetectPrimitives();


            }

            cnt++;
        }
        void cnt_Program_CCD01_02_PIN量測點量測_定位量測點(ref int cnt)
        {
            for (int i = 0; i < 22; i++)
            {

                if (this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].LineIsFitted)
                {
                    this.List_CCD01_02_PIN量測點參數_量測點_有無[i] = true;
                    this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].ValidPointIndex = 0;
                    this.List_CCD01_02_PIN量測點參數_量測點[i].X = (float)this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].ValidPointX;
                    this.List_CCD01_02_PIN量測點參數_量測點[i].Y = (float)this.List_CCD01_02_左側端點_AxLineMsr_線量測[i].ValidPointY;
                }
            }

            cnt++;
        }
        void cnt_Program_CCD01_02_PIN量測點量測_繪製畫布(ref int cnt)
        {
            this.PLC_Device_CCD01_02_PIN量測點_量測框調整_RefreshCanvas.Bool = true;
            if (this.PLC_Device_CCD01_02_PIN量測點_量測框調整按鈕.Bool && !PLC_Device_CCD01_02_計算一次.Bool)
            {
                this.h_Canvas_Tech_CCD01_02.RefreshCanvas();
            }

            cnt++;
        }

        #endregion
        #region PLC_CCD01_02_PIN量測_量測框調整
        MyTimer MyTimer_CCD01_02_PIN量測_量測框調整 = new MyTimer();
        PLC_Device PLC_Device_CCD01_02_PIN量測_量測框調整按鈕 = new PLC_Device("S6030");
        PLC_Device PLC_Device_CCD01_02_PIN量測_量測框調整 = new PLC_Device("S6025");
        PLC_Device PLC_Device_CCD01_02_PIN量測_量測框調整_OK = new PLC_Device("S6026");
        PLC_Device PLC_Device_CCD01_02_PIN量測_量測框調整_測試完成 = new PLC_Device("S6027");
        PLC_Device PLC_Device_CCD01_02_PIN量測_量測框調整_RefreshCanvas = new PLC_Device("S6028");

        private List<AxOvkBase.AxROIBW8> List_CCD01_02_PIN量測_AxROIBW8_量測框調整 = new List<AxOvkBase.AxROIBW8>();
        private List<AxOvkBlob.AxObject> List_CCD01_02_PIN量測_AxObject_區塊分析 = new List<AxOvkBlob.AxObject>();
        private AxOvkPat.AxVisionInspectionFrame CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整;

        private List<PLC_Device> List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值 = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_02_PIN量測參數_OrgX = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_02_PIN量測參數_OrgY = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_02_PIN量測參數_Width = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_02_PIN量測參數_Height = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_02_PIN量測參數_面積上限 = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_02_PIN量測參數_面積下限 = new List<PLC_Device>();
        private PointF[] List_CCD01_02_PIN量測參數_量測點 = new PointF[20];
        private PointF[] List_CCD01_02_PIN量測參數_量測點_結果 = new PointF[20];
        private Point[] List_CCD01_02_PIN量測參數_量測點_轉換後座標 = new Point[20];
        private bool[] List_CCD01_02_PIN量測參數_量測點_有無 = new bool[20];

        #region 灰階門檻值
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN01 = new PLC_Device("F600");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN02 = new PLC_Device("F601");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN03 = new PLC_Device("F602");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN04 = new PLC_Device("F603");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN05 = new PLC_Device("F604");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN06 = new PLC_Device("F605");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN07 = new PLC_Device("F606");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN08 = new PLC_Device("F607");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN09 = new PLC_Device("F608");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN10 = new PLC_Device("F609");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN11 = new PLC_Device("F610");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN12 = new PLC_Device("F611");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN13 = new PLC_Device("F612");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN14 = new PLC_Device("F613");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN15 = new PLC_Device("F614");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN16 = new PLC_Device("F615");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN17 = new PLC_Device("F616");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN18 = new PLC_Device("F617");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN19 = new PLC_Device("F618");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN20 = new PLC_Device("F619");
        #endregion
        #region OrgX
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN01 = new PLC_Device("F700");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN02 = new PLC_Device("F701");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN03 = new PLC_Device("F702");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN04 = new PLC_Device("F703");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN05 = new PLC_Device("F704");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN06 = new PLC_Device("F705");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN07 = new PLC_Device("F706");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN08 = new PLC_Device("F707");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN09 = new PLC_Device("F708");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN10 = new PLC_Device("F709");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN11 = new PLC_Device("F710");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN12 = new PLC_Device("F711");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN13 = new PLC_Device("F712");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN14 = new PLC_Device("F713");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN15 = new PLC_Device("F714");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN16 = new PLC_Device("F715");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN17 = new PLC_Device("F716");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN18 = new PLC_Device("F717");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN19 = new PLC_Device("F718");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN20 = new PLC_Device("F719");
        #endregion
        #region OrgY
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN01 = new PLC_Device("F800");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN02 = new PLC_Device("F801");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN03 = new PLC_Device("F802");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN04 = new PLC_Device("F803");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN05 = new PLC_Device("F804");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN06 = new PLC_Device("F805");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN07 = new PLC_Device("F806");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN08 = new PLC_Device("F807");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN09 = new PLC_Device("F808");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN10 = new PLC_Device("F809");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN11 = new PLC_Device("F810");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN12 = new PLC_Device("F811");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN13 = new PLC_Device("F812");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN14 = new PLC_Device("F813");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN15 = new PLC_Device("F814");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN16 = new PLC_Device("F815");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN17 = new PLC_Device("F816");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN18 = new PLC_Device("F817");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN19 = new PLC_Device("F818");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN20 = new PLC_Device("F819");
        #endregion
        #region Width
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN01 = new PLC_Device("F900");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN02 = new PLC_Device("F901");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN03 = new PLC_Device("F902");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN04 = new PLC_Device("F903");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN05 = new PLC_Device("F904");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN06 = new PLC_Device("F905");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN07 = new PLC_Device("F906");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN08 = new PLC_Device("F907");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN09 = new PLC_Device("F908");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN10 = new PLC_Device("F909");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN11 = new PLC_Device("F910");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN12 = new PLC_Device("F911");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN13 = new PLC_Device("F912");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN14 = new PLC_Device("F913");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN15 = new PLC_Device("F914");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN16 = new PLC_Device("F915");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN17 = new PLC_Device("F916");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN18 = new PLC_Device("F917");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN19 = new PLC_Device("F918");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN20 = new PLC_Device("F919");
        #endregion
        #region Height
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN01 = new PLC_Device("F1000");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN02 = new PLC_Device("F1001");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN03 = new PLC_Device("F1002");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN04 = new PLC_Device("F1003");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN05 = new PLC_Device("F1004");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN06 = new PLC_Device("F1005");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN07 = new PLC_Device("F1006");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN08 = new PLC_Device("F1007");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN09 = new PLC_Device("F1008");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN10 = new PLC_Device("F1009");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN11 = new PLC_Device("F1010");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN12 = new PLC_Device("F1011");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN13 = new PLC_Device("F1012");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN14 = new PLC_Device("F1013");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN15 = new PLC_Device("F1014");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN16 = new PLC_Device("F1015");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN17 = new PLC_Device("F1016");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN18 = new PLC_Device("F1017");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN19 = new PLC_Device("F1018");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN20 = new PLC_Device("F1019");
        #endregion
        #region 面積上限

        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN01 = new PLC_Device("F1100");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN02 = new PLC_Device("F1101");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN03 = new PLC_Device("F1102");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN04 = new PLC_Device("F1103");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN05 = new PLC_Device("F1104");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN06 = new PLC_Device("F1105");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN07 = new PLC_Device("F1106");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN08 = new PLC_Device("F1107");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN09 = new PLC_Device("F1108");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN10 = new PLC_Device("F1109");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN11 = new PLC_Device("F1110");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN12 = new PLC_Device("F1111");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN13 = new PLC_Device("F1112");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN14 = new PLC_Device("F1113");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN15 = new PLC_Device("F1114");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN16 = new PLC_Device("F1115");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN17 = new PLC_Device("F1116");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN18 = new PLC_Device("F1117");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN19 = new PLC_Device("F1118");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN20 = new PLC_Device("F1119");
        #endregion
        #region 面積下限

        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN01 = new PLC_Device("F1200");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN02 = new PLC_Device("F1201");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN03 = new PLC_Device("F1202");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN04 = new PLC_Device("F1203");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN05 = new PLC_Device("F1204");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN06 = new PLC_Device("F1205");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN07 = new PLC_Device("F1206");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN08 = new PLC_Device("F1207");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN09 = new PLC_Device("F1208");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN10 = new PLC_Device("F1209");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN11 = new PLC_Device("F1210");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN12 = new PLC_Device("F1211");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN13 = new PLC_Device("F1212");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN14 = new PLC_Device("F1213");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN15 = new PLC_Device("F1214");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN16 = new PLC_Device("F1215");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN17 = new PLC_Device("F1216");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN18 = new PLC_Device("F1217");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN19 = new PLC_Device("F1218");
        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN20 = new PLC_Device("F1219");
        #endregion

        AxOvkBase.TxAxHitHandle[] CCD01_02_PIN量測_AxROIBW8_TxAxHitHandle = new AxOvkBase.TxAxHitHandle[20];
        bool[] flag_CCD01_02_PIN量測_AxROIBW8_MouseDown = new bool[20];
        private void H_Canvas_Tech_CCD01_02_PIN量測_量測框調整_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {

            if (PLC_Device_CCD01_02_Main_取像並檢驗.Bool || PLC_Device_CCD01_02_PLC觸發檢測.Bool || PLC_Device_CCD01_02_Main_檢驗一次按鈕.Bool)
            {
                try
                {
                    Graphics g = Graphics.FromHdc((IntPtr)HDC);
                    for (int i = 0; i < this.List_CCD01_02_PIN量測參數_量測點.Length; i++)
                    {
                        DrawingClass.Draw.十字中心(this.List_CCD01_02_PIN量測參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
                    }
                    g.Dispose();
                    g = null;
                }
                catch
                {

                }

            }
            else if (PLC_Device_CCD01_02_Tech_檢驗一次.Bool || PLC_Device_CCD01_02_Tech_取像並檢驗.Bool)
            {
                if (this.PLC_Device_CCD01_02_PIN量測_量測框調整_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        for (int i = 0; i < 22; i++)
                        {
                            if (i < 10)
                            {
                                this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].Title = string.Format("上排" + "{0}", (i + 1).ToString("00"));
                            }
                            if (i >= 10)
                            {
                                this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].Title = string.Format("下排" + "{0}", ((i - 10) + 1).ToString("00"));
                            }
                            this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].ShowTitle = true;
                            this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].ShowPlacement = false;
                            this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].DrawRect(HDC, ZoomX, ZoomY, 0, 0, 0x0000FF);
                        }
                        for (int i = 0; i < this.List_CCD01_02_PIN量測參數_量測點.Length; i++)
                        {
                            DrawingClass.Draw.十字中心(this.List_CCD01_02_PIN量測參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
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
                if (this.PLC_Device_CCD01_02_PIN量測_量測框調整_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        PointF po_str_PIN到基準Y = new PointF(200, 250);
                        Font font = new Font("微軟正黑體", 10);

                        if (this.plC_CheckBox_CCD01_02_PIN量測_繪製量測框.Checked)
                        {
                            for (int i = 0; i < 22; i++)
                            {
                                if (i < 10)
                                {
                                    this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].Title = string.Format("上排" + "{0}", (i + 1).ToString("00"));
                                }
                                if (i >= 10)
                                {
                                    this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].Title = string.Format("下排" + "{0}", ((i - 10) + 1).ToString("00"));
                                }
                                this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].ShowTitle = true;
                                this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].ShowPlacement = false;
                                this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].DrawFrame(HDC, ZoomX, ZoomY, 0, 0, 0x0000FF);
                            }
                        }
                        if (this.plC_CheckBox_CCD01_02_PIN量測_繪製量測區塊.Checked)
                        {
                            for (int i = 0; i < this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
                            {
                                this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].DrawBlobs(HDC, -1, ZoomX, ZoomY, 0, 0, true, -1);
                            }

                        }
                        for (int i = 0; i < this.List_CCD01_02_PIN量測參數_量測點.Length; i++)
                        {
                            DrawingClass.Draw.十字中心(this.List_CCD01_02_PIN量測參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
                        }
                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }
            }

            this.PLC_Device_CCD01_02_PIN量測_量測框調整_RefreshCanvas.Bool = false;
        }
        private void H_Canvas_Tech_CCD01_02_PIN量測_量測框調整_OnCanvasMouseDownEvent(int x, int y, float ZoomX, float ZoomY, ref int InUsedEventNum, int InUsedCanvasHandle)
        {
            if (this.PLC_Device_CCD01_02_PIN量測_量測框調整.Bool)
            {
                for (int i = 0; i < this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
                {
                    this.CCD01_02_PIN量測_AxROIBW8_TxAxHitHandle[i] = this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].HitTest(x, y, ZoomX, ZoomY, 0, 0);
                    if (this.CCD01_02_PIN量測_AxROIBW8_TxAxHitHandle[i] != AxOvkBase.TxAxHitHandle.AX_HANDLE_NONE)
                    {
                        this.flag_CCD01_02_PIN量測_AxROIBW8_MouseDown[i] = true;
                        InUsedEventNum = 10;
                        return;
                    }
                }

            }

        }
        private void H_Canvas_Tech_CCD01_02_PIN量測_量測框調整_OnCanvasMouseMoveEvent(int x, int y, float ZoomX, float ZoomY)
        {
            for (int i = 0; i < this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {
                if (this.flag_CCD01_02_PIN量測_AxROIBW8_MouseDown[i])
                {
                    this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].DragROI(this.CCD01_02_PIN量測_AxROIBW8_TxAxHitHandle[i], x, y, ZoomX, ZoomY, 0, 0);
                    this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[i].Value = this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].OrgX;
                    this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[i].Value = this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].OrgY;
                    this.List_PLC_Device_CCD01_02_PIN量測參數_Width[i].Value = this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].ROIWidth;
                    this.List_PLC_Device_CCD01_02_PIN量測參數_Height[i].Value = this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].ROIHeight;
                }
            }

        }
        private void H_Canvas_Tech_CCD01_02_PIN量測_量測框調整_OnCanvasMouseUpEvent(int x, int y, float ZoomX, float ZoomY)
        {
            for (int i = 0; i < this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {
                this.flag_CCD01_02_PIN量測_AxROIBW8_MouseDown[i] = false;
            }
        }

        int cnt_Program_CCD01_02_PIN量測_量測框調整 = 65534;
        void sub_Program_CCD01_02_PIN量測_量測框調整()
        {
            if (cnt_Program_CCD01_02_PIN量測_量測框調整 == 65534)
            {
                this.h_Canvas_Tech_CCD01_02.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_02_PIN量測_量測框調整_OnCanvasDrawEvent;
                this.h_Canvas_Tech_CCD01_02.OnCanvasMouseDownEvent += H_Canvas_Tech_CCD01_02_PIN量測_量測框調整_OnCanvasMouseDownEvent;
                this.h_Canvas_Tech_CCD01_02.OnCanvasMouseMoveEvent += H_Canvas_Tech_CCD01_02_PIN量測_量測框調整_OnCanvasMouseMoveEvent;
                this.h_Canvas_Tech_CCD01_02.OnCanvasMouseUpEvent += H_Canvas_Tech_CCD01_02_PIN量測_量測框調整_OnCanvasMouseUpEvent;

                this.h_Canvas_Main_CCD01_02_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_02_PIN量測_量測框調整_OnCanvasDrawEvent;

                #region 灰階門檻值
                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN01);
                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN02);
                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN03);
                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN04);
                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN05);
                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN06);
                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN07);
                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN08);
                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN09);
                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN10);
                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN11);
                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN12);
                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN13);
                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN14);
                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN15);
                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN16);
                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN17);
                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN18);
                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN19);
                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN20);
                #endregion
                #region OrgX
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN01);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN02);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN03);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN04);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN05);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN06);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN07);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN08);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN09);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN10);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN11);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN12);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN13);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN14);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN15);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN16);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN17);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN18);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN19);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN20);
                #endregion
                #region OrgY
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN01);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN02);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN03);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN04);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN05);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN06);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN07);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN08);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN09);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN10);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN11);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN12);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN13);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN14);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN15);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN16);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN17);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN18);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN19);
                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN20);
                #endregion
                #region Width
                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN01);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN02);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN03);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN04);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN05);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN06);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN07);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN08);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN09);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN10);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN11);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN12);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN13);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN14);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN15);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN16);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN17);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN18);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN19);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN20);
                #endregion
                #region Height
                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN01);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN02);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN03);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN04);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN05);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN06);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN07);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN08);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN09);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN10);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN11);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN12);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN13);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN14);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN15);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN16);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN17);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN18);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN19);
                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN20);
                #endregion
                #region 面積上限
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN01);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN02);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN03);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN04);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN05);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN06);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN07);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN08);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN09);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN10);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN11);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN12);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN13);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN14);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN15);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN16);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN17);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN18);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN19);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN20);
                #endregion
                #region 面積下限
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN01);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN02);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN03);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN04);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN05);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN06);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN07);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN08);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN09);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN10);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN11);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN12);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN13);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN14);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN15);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN16);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN17);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN18);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN19);
                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN20);
                #endregion
                for (int i = 0; i < 20; i++)
                {
                    if (this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值[i].Value == 0) this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值[i].Value = 200;
                    if (this.List_PLC_Device_CCD01_02_PIN量測參數_Height[i].Value == 0) this.List_PLC_Device_CCD01_02_PIN量測參數_Height[i].Value = 100;
                    if (this.List_PLC_Device_CCD01_02_PIN量測參數_Width[i].Value == 0) this.List_PLC_Device_CCD01_02_PIN量測參數_Width[i].Value = 100;
                    if (this.List_PLC_Device_CCD01_02_PIN量測參數_Height[i].Value > 500) this.List_PLC_Device_CCD01_02_PIN量測參數_Height[i].Value = 500;
                    if (this.List_PLC_Device_CCD01_02_PIN量測參數_Width[i].Value > 500) this.List_PLC_Device_CCD01_02_PIN量測參數_Width[i].Value = 500;
                }
                PLC_Device_CCD01_02_PIN量測_量測框調整.SetComment("PLC_CCD01_02_PIN量測_量測框調整");
                PLC_Device_CCD01_02_PIN量測_量測框調整按鈕.Bool = false;
                PLC_Device_CCD01_02_PIN量測_量測框調整.Bool = false;
                cnt_Program_CCD01_02_PIN量測_量測框調整 = 65535;
            }
            if (cnt_Program_CCD01_02_PIN量測_量測框調整 == 65535) cnt_Program_CCD01_02_PIN量測_量測框調整 = 1;
            if (cnt_Program_CCD01_02_PIN量測_量測框調整 == 1) cnt_Program_CCD01_02_PIN量測_量測框調整_觸發按下(ref cnt_Program_CCD01_02_PIN量測_量測框調整);
            if (cnt_Program_CCD01_02_PIN量測_量測框調整 == 2) cnt_Program_CCD01_02_PIN量測_量測框調整_檢查按下(ref cnt_Program_CCD01_02_PIN量測_量測框調整);
            if (cnt_Program_CCD01_02_PIN量測_量測框調整 == 3) cnt_Program_CCD01_02_PIN量測_量測框調整_初始化(ref cnt_Program_CCD01_02_PIN量測_量測框調整);
            if (cnt_Program_CCD01_02_PIN量測_量測框調整 == 4) cnt_Program_CCD01_02_PIN量測_量測框調整_座標轉換(ref cnt_Program_CCD01_02_PIN量測_量測框調整);
            if (cnt_Program_CCD01_02_PIN量測_量測框調整 == 5) cnt_Program_CCD01_02_PIN量測_量測框調整_讀取參數(ref cnt_Program_CCD01_02_PIN量測_量測框調整);
            if (cnt_Program_CCD01_02_PIN量測_量測框調整 == 6) cnt_Program_CCD01_02_PIN量測_量測框調整_開始區塊分析(ref cnt_Program_CCD01_02_PIN量測_量測框調整);
            if (cnt_Program_CCD01_02_PIN量測_量測框調整 == 7) cnt_Program_CCD01_02_PIN量測_量測框調整_繪製畫布(ref cnt_Program_CCD01_02_PIN量測_量測框調整);
            if (cnt_Program_CCD01_02_PIN量測_量測框調整 == 8) cnt_Program_CCD01_02_PIN量測_量測框調整 = 65500;
            if (cnt_Program_CCD01_02_PIN量測_量測框調整 > 1) cnt_Program_CCD01_02_PIN量測_量測框調整_檢查放開(ref cnt_Program_CCD01_02_PIN量測_量測框調整);

            if (cnt_Program_CCD01_02_PIN量測_量測框調整 == 65500)
            {
                if (PLC_Device_CCD01_02_計算一次.Bool)
                {
                    PLC_Device_CCD01_02_PIN量測_量測框調整按鈕.Bool = false;
                    PLC_Device_CCD01_02_PIN量測_量測框調整.Bool = false;
                }
                cnt_Program_CCD01_02_PIN量測_量測框調整 = 65535;
            }
        }
        void cnt_Program_CCD01_02_PIN量測_量測框調整_觸發按下(ref int cnt)
        {
            if (PLC_Device_CCD01_02_PIN量測_量測框調整按鈕.Bool)
            {
                PLC_Device_CCD01_02_PIN量測_量測框調整.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_02_PIN量測_量測框調整_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_02_PIN量測_量測框調整.Bool) cnt++;
        }
        void cnt_Program_CCD01_02_PIN量測_量測框調整_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_02_PIN量測_量測框調整按鈕.Bool)
            {
                PLC_Device_CCD01_02_PIN量測_量測框調整.Bool = false;
                cnt = 65500;
            }
        }
        void cnt_Program_CCD01_02_PIN量測_量測框調整_初始化(ref int cnt)
        {
            this.MyTimer_CCD01_02_PIN量測_量測框調整.TickStop();
            this.MyTimer_CCD01_02_PIN量測_量測框調整.StartTickTime(99999);
            this.List_CCD01_02_PIN量測參數_量測點 = new PointF[20];
            this.List_CCD01_02_PIN量測參數_量測點_結果 = new PointF[20];
            this.List_CCD01_02_PIN量測參數_量測點_轉換後座標 = new Point[20];
            this.List_CCD01_02_PIN量測參數_量測點_有無 = new bool[20];
            cnt++;
        }
        void cnt_Program_CCD01_02_PIN量測_量測框調整_座標轉換(ref int cnt)
        {
            if (PLC_Device_CCD01_02_計算一次.Bool)
            {
                CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.RefPointX = PLC_Device_CCD01_01_水平基準線量測_量測中心_X.Value;
                CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.RefPointY = PLC_Device_CCD01_01_水平基準線量測_量測中心_Y.Value;
                CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.RefAngle = 0;
                CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentRefPointX = Point_CCD01_01_中心基準座標_量測點.X;
                CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentRefPointY = Point_CCD01_01_中心基準座標_量測點.Y;
                CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentRefAngle = 0;
                CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.NumOfVisionPoints = 20;

                for (int j = 0; j < 20; j++)
                {
                    if (this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[j].Value == 0) this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[j].Value = 100;
                    if (this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[j].Value == 0) this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[j].Value = 100;
                    if (this.List_PLC_Device_CCD01_02_PIN量測參數_Width[j].Value == 0) this.List_PLC_Device_CCD01_02_PIN量測參數_Width[j].Value = 100;
                    if (this.List_PLC_Device_CCD01_02_PIN量測參數_Height[j].Value == 0) this.List_PLC_Device_CCD01_02_PIN量測參數_Height[j].Value = 100;

                    CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
                    CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.VisionPointX = this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[j].Value;
                    CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.VisionPointY = this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[j].Value;
                }
                CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.EstimateCurrentVisionPoints();
                for (int j = 0; j < 20; j++)
                {
                    CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
                    List_CCD01_02_PIN量測參數_量測點_轉換後座標[j].X = (int)CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentVisionPointX;
                    List_CCD01_02_PIN量測參數_量測點_轉換後座標[j].Y = (int)CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentVisionPointY;
                }
            }
            cnt++;

        }
        void cnt_Program_CCD01_02_PIN量測_量測框調整_讀取參數(ref int cnt)
        {
            for (int i = 0; i < this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {
                if (this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[i].Value > 2596) this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[i].Value = 0;
                if (this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[i].Value > 1922) this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[i].Value = 0;
                if (this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[i].Value < 0) this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[i].Value = 0;
                if (this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[i].Value < 0) this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[i].Value = 0;

                if (this.List_CCD01_02_PIN量測參數_量測點_轉換後座標[i].X > 2596) this.List_CCD01_02_PIN量測參數_量測點_轉換後座標[i].X = 0;
                if (this.List_CCD01_02_PIN量測參數_量測點_轉換後座標[i].Y > 1922) this.List_CCD01_02_PIN量測參數_量測點_轉換後座標[i].Y = 0;
                if (this.List_CCD01_02_PIN量測參數_量測點_轉換後座標[i].X < 0) this.List_CCD01_02_PIN量測參數_量測點_轉換後座標[i].X = 0;
                if (this.List_CCD01_02_PIN量測參數_量測點_轉換後座標[i].Y < 0) this.List_CCD01_02_PIN量測參數_量測點_轉換後座標[i].Y = 0;
            }
            for (int i = 0; i < this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {
                this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].ParentHandle = this.CCD01_02_SrcImageHandle;
                if (PLC_Device_CCD01_02_計算一次.Bool)
                {
                    this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].OrgX = List_CCD01_02_PIN量測參數_量測點_轉換後座標[i].X;
                    this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].OrgY = List_CCD01_02_PIN量測參數_量測點_轉換後座標[i].Y;
                }
                else
                {
                    this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].OrgX = this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[i].Value;
                    this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].OrgY = this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[i].Value;
                }
                this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].ROIWidth = this.List_PLC_Device_CCD01_02_PIN量測參數_Width[i].Value;
                this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].ROIHeight = this.List_PLC_Device_CCD01_02_PIN量測參數_Height[i].Value;
                this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].SkewAngle = 0;
            }
            cnt++;
        }
        void cnt_Program_CCD01_02_PIN量測_量測框調整_開始區塊分析(ref int cnt)
        {
            uint object_value = 4294963615;

            for (int i = 0; i < this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {

                this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].SrcImageHandle = this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].VegaHandle;
                this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].ObjectClass = AxOvkBlob.TxAxObjClass.AX_OBJECT_DETECT_LIGHTER_CLASS;
                this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].HighThreshold = List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值[0].Value;
                if (this.CCD01_02_SrcImageHandle != 0)
                {
                    if (this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[i].Value + this.List_PLC_Device_CCD01_02_PIN量測參數_Width[i].Value < 2596 &&
                        this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[i].Value + this.List_PLC_Device_CCD01_02_PIN量測參數_Height[i].Value < 1922 &&
                        this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[i].Value > 0 && this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[i].Value > 0)
                    {
                        this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].BlobAnalyze(false);
                    }


                }
                this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].CalculateFeatures((int)object_value, -1);
                this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].SortObjects(AxOvkBlob.TxAxObjFeatureSortOrder.AX_OBJECT_SORT_ORDER_LARGE_TO_SMALL, AxOvkBlob.TxAxObjFeature.AX_OBJECT_FEATURE_AREA, 0, -1);
                this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].SelectObjects(AxOvkBlob.TxAxObjFeature.AX_OBJECT_FEATURE_AREA, AxOvkBlob.TxAxObjFeatureOperation.AX_OBJECT_REMOVE_LESS_THAN, this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限[0].Value);
                this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].SelectObjects(AxOvkBlob.TxAxObjFeature.AX_OBJECT_FEATURE_AREA, AxOvkBlob.TxAxObjFeatureOperation.AX_OBJECT_REMOVE_GREAT_THAN, this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限[0].Value);
                if (this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].DetectedNumObjs > 0)
                {
                    this.List_CCD01_02_PIN量測參數_量測點_有無[i] = true;
                    this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].BlobIndex = 0;
                    this.List_CCD01_02_PIN量測參數_量測點[i].X = (float)this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].BlobCentroidX;
                    this.List_CCD01_02_PIN量測參數_量測點[i].X += this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].OrgX;
                    //this.List_CCD01_02_PIN量測參數_量測點[i].Y = (float)this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].BlobCentroidY;
                    this.List_CCD01_02_PIN量測參數_量測點[i].Y = (float)this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].BlobCentroidY + (float)this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].BlobLimBoxHeight / 2;
                    this.List_CCD01_02_PIN量測參數_量測點[i].Y += this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].OrgY;
                }


            }

            cnt++;
        }
        void cnt_Program_CCD01_02_PIN量測_量測框調整_繪製畫布(ref int cnt)
        {
            this.PLC_Device_CCD01_02_PIN量測_量測框調整_RefreshCanvas.Bool = true;
            if (this.PLC_Device_CCD01_02_PIN量測_量測框調整按鈕.Bool && !PLC_Device_CCD01_02_計算一次.Bool)
            {
                this.h_Canvas_Tech_CCD01_02.RefreshCanvas();
            }

            cnt++;
        }





        #endregion
        #region PLC_CCD01_02_PIN量測_檢測距離計算
        private AxOvkMsr.AxPointLineDistanceMsr CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排;
        private AxOvkMsr.AxPointLineDistanceMsr CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排;
        MyTimer MyTimer_CCD01_02_PIN量測_檢測距離計算 = new MyTimer();
        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測距離計算按鈕 = new PLC_Device("S6050");
        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測距離計算 = new PLC_Device("S6045");
        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK = new PLC_Device("S6046");
        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測距離計算_測試完成 = new PLC_Device("S6047");
        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測距離計算_RefreshCanvas = new PLC_Device("S6048");

        PLC_Device PLC_Device_CCD01_02_PIN量測_水平度量測不測試 = new PLC_Device("S6100");
        PLC_Device PLC_Device_CCD01_02_PIN量測_間距量測不測試 = new PLC_Device("S6101");

        PLC_Device PLC_Device_CCD01_02_PIN量測_左右間距量測標準值 = new PLC_Device("F1300");
        PLC_Device PLC_Device_CCD01_02_PIN量測_左右間距量測上限值 = new PLC_Device("F1301");
        PLC_Device PLC_Device_CCD01_02_PIN量測_左右間距量測下限值 = new PLC_Device("F1302");
        PLC_Device PLC_Device_CCD01_02_PIN量測_上下間距量測標準值 = new PLC_Device("F1303");
        PLC_Device PLC_Device_CCD01_02_PIN量測_上下間距量測上限值 = new PLC_Device("F1304");
        PLC_Device PLC_Device_CCD01_02_PIN量測_上下間距量測下限值 = new PLC_Device("F1305");

        PLC_Device PLC_Device_CCD01_02_PIN量測_上排水平度量測標準值 = new PLC_Device("F1310");
        PLC_Device PLC_Device_CCD01_02_PIN量測_上排水平度量測上限值 = new PLC_Device("F1311");
        PLC_Device PLC_Device_CCD01_02_PIN量測_上排水平度量測下限值 = new PLC_Device("F1312");
        PLC_Device PLC_Device_CCD01_02_PIN量測_下排水平度量測標準值 = new PLC_Device("F1322");
        PLC_Device PLC_Device_CCD01_02_PIN量測_下排水平度量測上限值 = new PLC_Device("F1323");
        PLC_Device PLC_Device_CCD01_02_PIN量測_下排水平度量測下限值 = new PLC_Device("F1324");

        PLC_Device PLC_Device_CCD01_02_PIN量測_水平度量測差值 = new PLC_Device("F1313");
        PLC_Device PLC_Device_CCD01_02_PIN量測_水平度量測差值上限 = new PLC_Device("F1314");
        PLC_Device PLC_Device_CCD01_02_PIN量測_水平度量測差值下限 = new PLC_Device("F1315");
        PLC_Device PLC_Device_CCD01_02_PIN量測_間距上排PIN01到基準數值 = new PLC_Device("F1316");
        PLC_Device PLC_Device_CCD01_02_PIN量測_間距上排PIN01到基準上限 = new PLC_Device("F1317");
        PLC_Device PLC_Device_CCD01_02_PIN量測_間距上排PIN01到基準下限 = new PLC_Device("F1318");
        PLC_Device PLC_Device_CCD01_02_PIN量測_間距下排PIN01到基準數值 = new PLC_Device("F1319");
        PLC_Device PLC_Device_CCD01_02_PIN量測_間距下排PIN01到基準上限 = new PLC_Device("F1320");
        PLC_Device PLC_Device_CCD01_02_PIN量測_間距下排PIN01到基準下限 = new PLC_Device("F1321");



        private List<PLC_Device> List_PLC_Device_CCD01_02_PIN量測參數_間距不測試 = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_02_PIN量測參數_左右間距量測值 = new List<PLC_Device>();

        private double[] List_CCD01_02_PIN量測參數_左右間距量測距離 = new double[21];
        private double[] List_CCD01_02_PIN量測參數_上下間距量測距離 = new double[21];
        private double[] List_CCD01_02_PIN量測參數_水平度量測距離 = new double[22];
        private double[] List_CCD01_02_PIN量測參數_上下間格距離 = new double[11];
        private double CCD01_02_PIN量測參數_間距上排PIN01到基準距離 = new double();
        private double CCD01_02_PIN量測參數_間距下排PIN01到基準距離 = new double();

        private bool[] List_CCD01_02_PIN量測參數_量測點_OK = new bool[22];
        private bool[] List_CCD01_02_PIN量測參數_左右間距量測_OK = new bool[21];
        private bool[] List_CCD01_02_PIN量測參數_上下間距量測_OK = new bool[21];
        private bool[] List_CCD01_02_PIN量測參數_水平度量測_OK = new bool[22];
        private bool CCD01_02_PIN量測參數_間距上排PIN01到基準_OK = new bool();
        private bool CCD01_02_PIN量測參數_間距下排PIN01到基準_OK = new bool();

        private double[] List_CCD01_02_PIN量測參數_水平度量測顯示點_X = new double[22];
        private double[] List_CCD01_02_PIN量測參數_水平度量測顯示點_Y = new double[22];
        private void H_Canvas_Tech_CCD01_02_PIN間距量測_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
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

            if (PLC_Device_CCD01_02_Main_取像並檢驗.Bool || PLC_Device_CCD01_02_PLC觸發檢測.Bool || PLC_Device_CCD01_02_Main_檢驗一次按鈕.Bool)
            {
                基準線偏移_上排 = this.PLC_Device_CCD01_01_基準線量測_基準線偏移_上排.Value / CCD01_比例尺_pixcel_To_mm / 1000;
                基準線偏移_下排 = this.PLC_Device_CCD01_01_基準線量測_基準線偏移_下排.Value / CCD01_比例尺_pixcel_To_mm / 1000;
                try
                {
                    Graphics g = Graphics.FromHdc((IntPtr)HDC);
                    DrawingClass.Draw.十字中心(new PointF(this.Point_CCD01_01_中心基準座標_量測點.X, this.Point_CCD01_01_中心基準座標_量測點.Y), 100, Color.Red, 2, g, ZoomX, ZoomY);
                    #region 左右間距顯示
                    for (int i = 0; i < 21; i++)
                    {
                        p0 = new PointF(this.List_CCD01_02_PIN量測點參數_量測點[i].X, this.List_CCD01_02_PIN量測點參數_量測點[i].Y);
                        p1 = new PointF(this.List_CCD01_02_PIN量測點參數_量測點[i + 1].X, this.List_CCD01_02_PIN量測點參數_量測點[i + 1].Y);
                        間距 = List_CCD01_02_PIN量測參數_左右間距量測距離[i];
                        if (i != 10)
                        {
                            if (i <= 10)
                            {
                                if (List_CCD01_02_PIN量測參數_左右間距量測_OK[i])
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
                                if (List_CCD01_02_PIN量測參數_左右間距量測_OK[i])
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
                    上排_p2 = new PointF(this.List_CCD01_02_PIN量測點參數_量測點[0].X, this.List_CCD01_02_PIN量測點參數_量測點[0].Y - 50);
                    上排_p3 = new PointF(this.Point_CCD01_01_中心基準座標_量測點.X, this.List_CCD01_02_PIN量測點參數_量測點[0].Y - 50);

                    if (CCD01_02_PIN量測參數_間距上排PIN01到基準_OK)
                    {
                        //DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD01_02_PIN量測參數_間距上排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
                        //    (float)((上排_p2.Y + 上排_p3.Y) / 2) - 80), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        //DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
                    }
                    else
                    {
                        DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD01_02_PIN量測參數_間距上排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
                             (float)((上排_p2.Y + 上排_p3.Y) / 2) - 80), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Red, 1, g, ZoomX, ZoomY);
                    }

                    下排_p2 = new PointF(this.List_CCD01_02_PIN量測點參數_量測點[10].X, this.List_CCD01_02_PIN量測點參數_量測點[10].Y + 500);
                    下排_p3 = new PointF(this.Point_CCD01_01_中心基準座標_量測點.X, this.List_CCD01_02_PIN量測點參數_量測點[10].Y + 500);

                    if (CCD01_02_PIN量測參數_間距下排PIN01到基準_OK)
                    {
                        //DrawingClass.Draw.文字中心繪製(string.Format("下PIN01到基準" + "{0}", (CCD01_02_PIN量測參數_間距下排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((下排_p2.X + 下排_p3.X) / 2),
                        //    (float)((下排_p2.Y + 下排_p3.Y) / 2) + 80), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        //DrawingClass.Draw.線段繪製(下排_p2, 下排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
                    }
                    else
                    {
                        DrawingClass.Draw.文字中心繪製(string.Format("下PIN01到基準" + "{0}", (CCD01_02_PIN量測參數_間距下排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((下排_p2.X + 下排_p3.X) / 2),
                            (float)((下排_p2.Y + 下排_p3.Y) / 2) + 80), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        DrawingClass.Draw.線段繪製(下排_p2, 下排_p3, Color.Red, 1, g, ZoomX, ZoomY);
                    }
                    #endregion
                    #region 水平度輔顯示
                    DrawingClass.Draw.水平線段繪製(0, 10000, CCD01_01_水平基準線量測_AxLineMsr.MeasuredSlope, CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotX,
                      CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotY + 基準線偏移_上排, Color.Blue, 2, g, ZoomX, ZoomY);
                    DrawingClass.Draw.文字右中繪製("上排輔助線", new PointF((float)this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[0], CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotY + (float)基準線偏移_上排 + 20)
                        , new Font("標楷體", 10), Color.White, Color.Blue, g, ZoomX, ZoomY);


                    DrawingClass.Draw.水平線段繪製(0, 10000, CCD01_01_水平基準線量測_AxLineMsr.MeasuredSlope, CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotX,
                        CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotY + 基準線偏移_下排, Color.Blue, 2, g, ZoomX, ZoomY);
                    DrawingClass.Draw.文字右中繪製("下排輔助線", new PointF((float)this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[0], CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotY + (float)基準線偏移_下排 + 20)
                        , new Font("標楷體", 10), Color.White, Color.Blue, g, ZoomX, ZoomY);
                    #region 到基準線距離
                    for (int i = 0; i < 22; i++)
                    {
                        point = new PointF(this.List_CCD01_02_PIN量測點參數_量測點[i].X, this.List_CCD01_02_PIN量測點參數_量測點[i].Y);

                        上排_to_line_point = new PointF((float)this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_02_PIN量測參數_水平度量測顯示點_Y[i]));
                        下排_to_line_point = new PointF((float)this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_02_PIN量測參數_水平度量測顯示點_Y[i]));

                        水平度 = List_CCD01_02_PIN量測參數_水平度量測距離[i];


                        if (List_CCD01_02_PIN量測參數_水平度量測_OK[i])
                        {
                            DrawingClass.Draw.文字中心繪製("到基準線:", new PointF(1200, 1500), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            if (i <= 10)
                            {
                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                new PointF(point.X, 1600), new Font("標楷體", 10), Color.Black, Color.DodgerBlue, g, ZoomX, ZoomY);

                                DrawingClass.Draw.線段繪製(point, 上排_to_line_point, Color.DodgerBlue, 1, g, ZoomX, ZoomY);
                            }
                            if (i > 10)
                            {
                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                new PointF(point.X, 1700), new Font("標楷體", 10), Color.Black, Color.DodgerBlue, g, ZoomX, ZoomY);

                                DrawingClass.Draw.線段繪製(point, 下排_to_line_point, Color.DodgerBlue, 1, g, ZoomX, ZoomY);
                            }

                        }
                        else
                        {
                            DrawingClass.Draw.文字中心繪製("到基準線:", new PointF(1200, 1500), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            if (i <= 10)
                            {
                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                new PointF(point.X, 1600), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                                DrawingClass.Draw.線段繪製(point, 上排_to_line_point, Color.Red, 1, g, ZoomX, ZoomY);
                            }
                            if (i > 10)
                            {
                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                new PointF(point.X, 1700), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                                DrawingClass.Draw.線段繪製(point, 下排_to_line_point, Color.Red, 1, g, ZoomX, ZoomY);
                            }

                        }


                    }
                    #endregion
                    #region 上下間距
                    for (int i = 0; i < 11; i++)
                    {
                        point = new PointF(this.List_CCD01_02_PIN量測點參數_量測點[i].X, this.List_CCD01_02_PIN量測點參數_量測點[i].Y);
                        point1 = new PointF(this.List_CCD01_02_PIN量測點參數_量測點[i + 11].X, this.List_CCD01_02_PIN量測點參數_量測點[i + 11].Y);

                        上排_to_line_point = new PointF((float)this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_02_PIN量測參數_水平度量測顯示點_Y[i]));
                        下排_to_line_point = new PointF((float)this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_02_PIN量測參數_水平度量測顯示點_Y[i]));

                        水平度 = List_CCD01_02_PIN量測參數_上下間格距離[i];
                        if (List_CCD01_02_PIN量測參數_上下間距量測_OK[i])
                        {

                            DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                            new PointF(point.X, point.Y + 450 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Yellow, g, ZoomX, ZoomY);

                            DrawingClass.Draw.線段繪製(point, point1, Color.Yellow, 1, g, ZoomX, ZoomY);

                        }
                        else
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                            new PointF(point.X, point.Y + 450 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                            DrawingClass.Draw.線段繪製(point, point1, Color.Red, 1, g, ZoomX, ZoomY);



                        }

                    }
                    #endregion


                    #endregion
                    #region 結果顯示
                    for (int i = 0; i < 21; i++)
                    {
                        if (i != 10)
                        {
                            if (List_CCD01_02_PIN量測參數_左右間距量測_OK[i] && CCD01_02_PIN量測參數_間距上排PIN01到基準_OK)
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
                        if (List_CCD01_02_PIN量測參數_水平度量測_OK[i] && List_CCD01_02_PIN量測參數_上下間距量測_OK[i])
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
            else if (PLC_Device_CCD01_02_Tech_檢驗一次.Bool || PLC_Device_CCD01_02_Tech_取像並檢驗.Bool)
            {
                基準線偏移_上排 = this.PLC_Device_CCD01_01_基準線量測_基準線偏移_上排.Value / CCD01_比例尺_pixcel_To_mm / 1000;
                基準線偏移_下排 = this.PLC_Device_CCD01_01_基準線量測_基準線偏移_下排.Value / CCD01_比例尺_pixcel_To_mm / 1000;
                if (this.PLC_Device_CCD01_02_PIN量測_檢測距離計算_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        DrawingClass.Draw.十字中心(new PointF(this.Point_CCD01_01_中心基準座標_量測點.X, this.Point_CCD01_01_中心基準座標_量測點.Y), 100, Color.Red, 2, g, ZoomX, ZoomY);
                        #region 左右間距顯示
                        for (int i = 0; i < 21; i++)
                        {
                            p0 = new PointF(this.List_CCD01_02_PIN量測點參數_量測點[i].X, this.List_CCD01_02_PIN量測點參數_量測點[i].Y);
                            p1 = new PointF(this.List_CCD01_02_PIN量測點參數_量測點[i + 1].X, this.List_CCD01_02_PIN量測點參數_量測點[i + 1].Y);
                            間距 = List_CCD01_02_PIN量測參數_左右間距量測距離[i];

                            if (i != 10)
                            {
                                if (List_CCD01_02_PIN量測參數_左右間距量測_OK[i])
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

                        上排_p2 = new PointF(this.List_CCD01_02_PIN量測點參數_量測點[0].X, this.List_CCD01_02_PIN量測點參數_量測點[0].Y - 150);
                        上排_p3 = new PointF(this.Point_CCD01_01_中心基準座標_量測點.X, this.List_CCD01_02_PIN量測點參數_量測點[0].Y - 150);

                        if (CCD01_02_PIN量測參數_間距上排PIN01到基準_OK)
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD01_02_PIN量測參數_間距上排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
                                (float)((上排_p2.Y + 上排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
                        }
                        else
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD01_02_PIN量測參數_間距上排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
    (float)((上排_p2.Y + 上排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Red, 1, g, ZoomX, ZoomY);
                        }

                        下排_p2 = new PointF(this.List_CCD01_02_PIN量測點參數_量測點[10].X, this.List_CCD01_02_PIN量測點參數_量測點[10].Y + 150);
                        下排_p3 = new PointF(this.Point_CCD01_01_中心基準座標_量測點.X, this.List_CCD01_02_PIN量測點參數_量測點[10].Y + 150);

                        if (CCD01_02_PIN量測參數_間距下排PIN01到基準_OK)
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("下PIN01到基準" + "{0}", (CCD01_02_PIN量測參數_間距下排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((下排_p2.X + 下排_p3.X) / 2),
                                (float)((下排_p2.Y + 下排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(下排_p2, 下排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
                        }
                        else
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("下PIN01到基準" + "{0}", (CCD01_02_PIN量測參數_間距下排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((下排_p2.X + 下排_p3.X) / 2),
    (float)((下排_p2.Y + 下排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(下排_p2, 下排_p3, Color.Red, 1, g, ZoomX, ZoomY);
                        }
                        #endregion
                        #region 水平度顯示
                        DrawingClass.Draw.水平線段繪製(0, 10000, CCD01_01_水平基準線量測_AxLineMsr.MeasuredSlope, CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotX,
                          CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotY + 基準線偏移_上排, Color.Blue, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.文字右中繪製("上排輔助線", new PointF((float)this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[0], CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotY + (float)基準線偏移_上排 + 20)
                            , new Font("標楷體", 10), Color.White, Color.Blue, g, ZoomX, ZoomY);


                        DrawingClass.Draw.水平線段繪製(0, 10000, CCD01_01_水平基準線量測_AxLineMsr.MeasuredSlope, CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotX,
                            CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotY + 基準線偏移_下排, Color.Blue, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.文字右中繪製("下排輔助線", new PointF((float)this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[0], CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotY + (float)基準線偏移_下排 + 20)
                            , new Font("標楷體", 10), Color.White, Color.Blue, g, ZoomX, ZoomY);

                        #region 到基準線距離
                        for (int i = 0; i < 22; i++)
                        {
                            point = new PointF(this.List_CCD01_02_PIN量測點參數_量測點[i].X, this.List_CCD01_02_PIN量測點參數_量測點[i].Y);

                            上排_to_line_point = new PointF((float)this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_02_PIN量測參數_水平度量測顯示點_Y[i]));
                            下排_to_line_point = new PointF((float)this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_02_PIN量測參數_水平度量測顯示點_Y[i]));

                            水平度 = List_CCD01_02_PIN量測參數_水平度量測距離[i];


                            if (List_CCD01_02_PIN量測參數_水平度量測_OK[i])
                            {
                                if (i <= 10)
                                {
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                    new PointF(point.X, 1300), new Font("標楷體", 10), Color.Black, Color.DodgerBlue, g, ZoomX, ZoomY);

                                    DrawingClass.Draw.線段繪製(point, 上排_to_line_point, Color.DodgerBlue, 1, g, ZoomX, ZoomY);
                                }
                                if (i > 10)
                                {
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                    new PointF(point.X, 1350), new Font("標楷體", 10), Color.Black, Color.DodgerBlue, g, ZoomX, ZoomY);

                                    DrawingClass.Draw.線段繪製(point, 下排_to_line_point, Color.DodgerBlue, 1, g, ZoomX, ZoomY);
                                }

                            }
                            else
                            {
                                if (i <= 10)
                                {
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                    new PointF(point.X, 1300), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                                    DrawingClass.Draw.線段繪製(point, 上排_to_line_point, Color.Red, 1, g, ZoomX, ZoomY);
                                }
                                if (i > 10)
                                {
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                    new PointF(point.X, 1350), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                                    DrawingClass.Draw.線段繪製(point, 下排_to_line_point, Color.Red, 1, g, ZoomX, ZoomY);
                                }

                            }


                        }
                        #endregion
                        #region 上下間距
                        for (int i = 0; i < 11; i++)
                        {
                            point = new PointF(this.List_CCD01_02_PIN量測點參數_量測點[i].X, this.List_CCD01_02_PIN量測點參數_量測點[i].Y);
                            point1 = new PointF(this.List_CCD01_02_PIN量測點參數_量測點[i + 11].X, this.List_CCD01_02_PIN量測點參數_量測點[i + 11].Y);

                            上排_to_line_point = new PointF((float)this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_02_PIN量測參數_水平度量測顯示點_Y[i]));
                            下排_to_line_point = new PointF((float)this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_02_PIN量測參數_水平度量測顯示點_Y[i]));

                            水平度 = List_CCD01_02_PIN量測參數_上下間格距離[i];
                            if (List_CCD01_02_PIN量測參數_上下間距量測_OK[i])
                            {

                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                new PointF(point.X, point.Y + 450 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Yellow, g, ZoomX, ZoomY);

                                DrawingClass.Draw.線段繪製(point, point1, Color.DodgerBlue, 1, g, ZoomX, ZoomY);

                            }
                            else
                            {
                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                new PointF(point.X, point.Y + 450 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                                DrawingClass.Draw.線段繪製(point, point1, Color.Red, 1, g, ZoomX, ZoomY);



                            }

                        }
                        #endregion

                        #endregion
                        #region 結果顯示
                        for (int i = 0; i < 21; i++)
                        {
                            if (i != 10)
                            {
                                if (List_CCD01_02_PIN量測參數_左右間距量測_OK[i] && CCD01_02_PIN量測參數_間距上排PIN01到基準_OK && CCD01_02_PIN量測參數_間距下排PIN01到基準_OK)
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
                            if (List_CCD01_02_PIN量測參數_水平度量測_OK[i] && List_CCD01_02_PIN量測參數_上下間距量測_OK[i])
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
                if (this.PLC_Device_CCD01_02_PIN量測_檢測距離計算_RefreshCanvas.Bool && PLC_Device_CCD01_02_PIN量測_檢測距離計算.Bool)
                {
                    基準線偏移_上排 = this.PLC_Device_CCD01_01_基準線量測_基準線偏移_上排.Value / CCD01_比例尺_pixcel_To_mm / 1000;
                    基準線偏移_下排 = this.PLC_Device_CCD01_01_基準線量測_基準線偏移_下排.Value / CCD01_比例尺_pixcel_To_mm / 1000;
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);

                        DrawingClass.Draw.十字中心(new PointF(this.Point_CCD01_01_中心基準座標_量測點.X, this.Point_CCD01_01_中心基準座標_量測點.Y), 100, Color.Red, 2, g, ZoomX, ZoomY);
                        #region 左右間距顯示
                        for (int i = 0; i < 21; i++)
                        {
                            p0 = new PointF(this.List_CCD01_02_PIN量測點參數_量測點[i].X, this.List_CCD01_02_PIN量測點參數_量測點[i].Y);
                            p1 = new PointF(this.List_CCD01_02_PIN量測點參數_量測點[i + 1].X, this.List_CCD01_02_PIN量測點參數_量測點[i + 1].Y);
                            間距 = List_CCD01_02_PIN量測參數_左右間距量測距離[i];

                            if (i != 10)
                            {
                                if (List_CCD01_02_PIN量測參數_左右間距量測_OK[i])
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
                        上排_p2 = new PointF(this.List_CCD01_02_PIN量測點參數_量測點[0].X, this.List_CCD01_02_PIN量測點參數_量測點[0].Y - 150);
                        上排_p3 = new PointF(this.Point_CCD01_01_中心基準座標_量測點.X, this.List_CCD01_02_PIN量測點參數_量測點[0].Y - 150);

                        if (CCD01_02_PIN量測參數_間距上排PIN01到基準_OK)
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD01_02_PIN量測參數_間距上排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
                                (float)((上排_p2.Y + 上排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
                        }
                        else
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD01_02_PIN量測參數_間距上排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
    (float)((上排_p2.Y + 上排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Red, 1, g, ZoomX, ZoomY);
                        }

                        下排_p2 = new PointF(this.List_CCD01_02_PIN量測點參數_量測點[10].X, this.List_CCD01_02_PIN量測點參數_量測點[10].Y + 150);
                        下排_p3 = new PointF(this.Point_CCD01_01_中心基準座標_量測點.X, this.List_CCD01_02_PIN量測點參數_量測點[10].Y + 150);

                        if (CCD01_02_PIN量測參數_間距下排PIN01到基準_OK)
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("下PIN01到基準" + "{0}", (CCD01_02_PIN量測參數_間距下排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((下排_p2.X + 下排_p3.X) / 2),
                                (float)((下排_p2.Y + 下排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(下排_p2, 下排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
                        }
                        else
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("下PIN01到基準" + "{0}", (CCD01_02_PIN量測參數_間距下排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((下排_p2.X + 下排_p3.X) / 2),
    (float)((下排_p2.Y + 下排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(下排_p2, 下排_p3, Color.Red, 1, g, ZoomX, ZoomY);
                        }


                        #endregion
                        #region 水平度顯示
                        //DrawingClass.Draw.水平線段繪製(0, 10000, CCD01_01_水平基準線量測_AxLineMsr.MeasuredSlope, CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotX,
                        //    CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD01_01_基準線量測_基準線偏移.Value, Color.Yellow, 2, g, ZoomX, ZoomY);

                        DrawingClass.Draw.水平線段繪製(0, 10000, CCD01_01_水平基準線量測_AxLineMsr.MeasuredSlope, CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotX,
                          CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotY + 基準線偏移_上排, Color.Blue, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.文字右中繪製("上排輔助線", new PointF((float)this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[0], CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotY + (float)基準線偏移_上排 + 20)
                            , new Font("標楷體", 10), Color.White, Color.Blue, g, ZoomX, ZoomY);


                        DrawingClass.Draw.水平線段繪製(0, 10000, CCD01_01_水平基準線量測_AxLineMsr.MeasuredSlope, CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotX,
                            CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotY + 基準線偏移_下排, Color.Blue, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.文字右中繪製("下排輔助線", new PointF((float)this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[0], CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotY + (float)基準線偏移_下排 + 20)
                            , new Font("標楷體", 10), Color.White, Color.Blue, g, ZoomX, ZoomY);

                        #region 到基準線距離
                        for (int i = 0; i < 22; i++)
                        {
                            point = new PointF(this.List_CCD01_02_PIN量測點參數_量測點[i].X, this.List_CCD01_02_PIN量測點參數_量測點[i].Y);

                            上排_to_line_point = new PointF((float)this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_02_PIN量測參數_水平度量測顯示點_Y[i]));
                            下排_to_line_point = new PointF((float)this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_02_PIN量測參數_水平度量測顯示點_Y[i]));

                            水平度 = List_CCD01_02_PIN量測參數_水平度量測距離[i];


                            if (List_CCD01_02_PIN量測參數_水平度量測_OK[i])
                            {
                                if (i <= 10)
                                {
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                    new PointF(point.X, 1300), new Font("標楷體", 10), Color.Black, Color.DodgerBlue, g, ZoomX, ZoomY);

                                    DrawingClass.Draw.線段繪製(point, 上排_to_line_point, Color.DodgerBlue, 1, g, ZoomX, ZoomY);
                                }
                                if (i > 10)
                                {
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                    new PointF(point.X, 1350), new Font("標楷體", 10), Color.Black, Color.DodgerBlue, g, ZoomX, ZoomY);

                                    DrawingClass.Draw.線段繪製(point, 下排_to_line_point, Color.DodgerBlue, 1, g, ZoomX, ZoomY);
                                }

                            }
                            else
                            {
                                if (i <= 10)
                                {
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                    new PointF(point.X, 1300), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                                    DrawingClass.Draw.線段繪製(point, 上排_to_line_point, Color.Red, 1, g, ZoomX, ZoomY);
                                }
                                if (i > 10)
                                {
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                    new PointF(point.X, 1350), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                                    DrawingClass.Draw.線段繪製(point, 下排_to_line_point, Color.Red, 1, g, ZoomX, ZoomY);
                                }

                            }


                        }
                        #endregion
                        #region 上下間距
                        for (int i = 0; i < 11; i++)
                        {
                            point = new PointF(this.List_CCD01_02_PIN量測點參數_量測點[i].X, this.List_CCD01_02_PIN量測點參數_量測點[i].Y);
                            point1 = new PointF(this.List_CCD01_02_PIN量測點參數_量測點[i + 11].X, this.List_CCD01_02_PIN量測點參數_量測點[i + 11].Y);

                            上排_to_line_point = new PointF((float)this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_02_PIN量測參數_水平度量測顯示點_Y[i]));
                            下排_to_line_point = new PointF((float)this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_02_PIN量測參數_水平度量測顯示點_Y[i]));

                            水平度 = List_CCD01_02_PIN量測參數_上下間格距離[i];
                            if (List_CCD01_02_PIN量測參數_上下間距量測_OK[i])
                            {

                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                new PointF(point.X, point.Y + 450 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Yellow, g, ZoomX, ZoomY);

                                DrawingClass.Draw.線段繪製(point, point1, Color.Yellow, 1, g, ZoomX, ZoomY);

                            }
                            else
                            {
                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                new PointF(point.X, point.Y + 450 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                                DrawingClass.Draw.線段繪製(point, point1, Color.Red, 1, g, ZoomX, ZoomY);



                            }

                        }
                        #endregion
                        #endregion
                        #region 結果顯示

                        for (int i = 0; i < 21; i++)
                        {
                            if (i != 10)
                            {
                                if (List_CCD01_02_PIN量測參數_左右間距量測_OK[i] && CCD01_02_PIN量測參數_間距上排PIN01到基準_OK)
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
                            if (List_CCD01_02_PIN量測參數_水平度量測_OK[i] && List_CCD01_02_PIN量測參數_上下間距量測_OK[i])
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

            this.PLC_Device_CCD01_02_PIN量測_檢測距離計算_RefreshCanvas.Bool = false;
        }

        int cnt_Program_CCD01_02_PIN量測_檢測距離計算 = 65534;
        void sub_Program_CCD01_02_PIN量測_檢測距離計算()
        {
            if (cnt_Program_CCD01_02_PIN量測_檢測距離計算 == 65534)
            {
                this.h_Canvas_Tech_CCD01_02.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_02_PIN間距量測_OnCanvasDrawEvent;
                this.h_Canvas_Main_CCD01_02_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_02_PIN間距量測_OnCanvasDrawEvent;
                PLC_Device_CCD01_02_PIN量測_檢測距離計算.SetComment("PLC_CCD01_02_PIN量測_檢測距離計算");
                PLC_Device_CCD01_02_PIN量測_檢測距離計算.Bool = false;
                PLC_Device_CCD01_02_PIN量測_檢測距離計算按鈕.Bool = false;
                cnt_Program_CCD01_02_PIN量測_檢測距離計算 = 65535;

            }
            if (cnt_Program_CCD01_02_PIN量測_檢測距離計算 == 65535) cnt_Program_CCD01_02_PIN量測_檢測距離計算 = 1;
            if (cnt_Program_CCD01_02_PIN量測_檢測距離計算 == 1) cnt_Program_CCD01_02_PIN量測_檢測距離計算_觸發按下(ref cnt_Program_CCD01_02_PIN量測_檢測距離計算);
            if (cnt_Program_CCD01_02_PIN量測_檢測距離計算 == 2) cnt_Program_CCD01_02_PIN量測_檢測距離計算_檢查按下(ref cnt_Program_CCD01_02_PIN量測_檢測距離計算);
            if (cnt_Program_CCD01_02_PIN量測_檢測距離計算 == 3) cnt_Program_CCD01_02_PIN量測_檢測距離計算_初始化(ref cnt_Program_CCD01_02_PIN量測_檢測距離計算);
            if (cnt_Program_CCD01_02_PIN量測_檢測距離計算 == 4) cnt_Program_CCD01_02_PIN量測_檢測距離計算_數值計算(ref cnt_Program_CCD01_02_PIN量測_檢測距離計算);
            if (cnt_Program_CCD01_02_PIN量測_檢測距離計算 == 5) cnt_Program_CCD01_02_PIN量測_檢測距離計算_量測結果(ref cnt_Program_CCD01_02_PIN量測_檢測距離計算);
            if (cnt_Program_CCD01_02_PIN量測_檢測距離計算 == 6) cnt_Program_CCD01_02_PIN量測_檢測距離計算_繪製畫布(ref cnt_Program_CCD01_02_PIN量測_檢測距離計算);
            if (cnt_Program_CCD01_02_PIN量測_檢測距離計算 == 7) cnt_Program_CCD01_02_PIN量測_檢測距離計算 = 65500;
            if (cnt_Program_CCD01_02_PIN量測_檢測距離計算 > 1) cnt_Program_CCD01_02_PIN量測_檢測距離計算_檢查放開(ref cnt_Program_CCD01_02_PIN量測_檢測距離計算);

            if (cnt_Program_CCD01_02_PIN量測_檢測距離計算 == 65500)
            {
                PLC_Device_CCD01_02_PIN量測_檢測距離計算.Bool = false;
                PLC_Device_CCD01_02_PIN量測_檢測距離計算按鈕.Bool = false;
                cnt_Program_CCD01_02_PIN量測_檢測距離計算 = 65535;
            }
        }
        void cnt_Program_CCD01_02_PIN量測_檢測距離計算_觸發按下(ref int cnt)
        {
            if (PLC_Device_CCD01_02_PIN量測_檢測距離計算按鈕.Bool)
            {
                PLC_Device_CCD01_02_PIN量測_檢測距離計算.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_02_PIN量測_檢測距離計算_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_02_PIN量測_檢測距離計算.Bool)
            {
                cnt++;
            }

        }
        void cnt_Program_CCD01_02_PIN量測_檢測距離計算_檢查放開(ref int cnt)
        {
            //if (!PLC_Device_CCD01_02_PIN量測_檢測距離計算.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_02_PIN量測_檢測距離計算_初始化(ref int cnt)
        {
            this.MyTimer_CCD01_02_PIN量測_檢測距離計算.TickStop();
            this.MyTimer_CCD01_02_PIN量測_檢測距離計算.StartTickTime(99999);

            this.List_CCD01_02_PIN量測參數_左右間距量測距離 = new double[21];
            this.List_CCD01_02_PIN量測參數_上下間距量測距離 = new double[21];
            this.List_CCD01_02_PIN量測參數_水平度量測距離 = new double[22];
            this.List_CCD01_02_PIN量測參數_上下間格距離 = new double[11];
            this.CCD01_02_PIN量測參數_間距上排PIN01到基準距離 = new double();
            this.CCD01_02_PIN量測參數_間距下排PIN01到基準距離 = new double();

            this.List_CCD01_02_PIN量測參數_量測點_OK = new bool[22];
            this.List_CCD01_02_PIN量測參數_左右間距量測_OK = new bool[21];
            this.List_CCD01_02_PIN量測參數_上下間距量測_OK = new bool[11];
            this.List_CCD01_02_PIN量測參數_水平度量測_OK = new bool[22];
            this.CCD01_02_PIN量測參數_間距上排PIN01到基準_OK = new bool();
            this.CCD01_02_PIN量測參數_間距下排PIN01到基準_OK = new bool();
            cnt++;
        }
        void cnt_Program_CCD01_02_PIN量測_檢測距離計算_數值計算(ref int cnt)
        {
            double 基準線偏移_上排 = this.PLC_Device_CCD01_01_基準線量測_基準線偏移_上排.Value / CCD01_比例尺_pixcel_To_mm / 1000;
            double 基準線偏移_下排 = this.PLC_Device_CCD01_01_基準線量測_基準線偏移_下排.Value / CCD01_比例尺_pixcel_To_mm / 1000;
            #region 水平度數值計算
            this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.LinePivotX = this.CCD01_01_水平基準線量測_AxLineRegression.FittedPivotX;
            this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.LinePivotY = this.CCD01_01_水平基準線量測_AxLineRegression.FittedPivotY + 基準線偏移_上排;
            this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.LineHorzVert = AxOvkMsr.TxAxLineHorzVert.AX_LINE_QUASI_HORIZONTAL;
            this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.LineSlope = this.CCD01_01_水平基準線量測_AxLineRegression.FittedSlope;

            this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.LinePivotX = this.CCD01_01_水平基準線量測_AxLineRegression.FittedPivotX;
            this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.LinePivotY = this.CCD01_01_水平基準線量測_AxLineRegression.FittedPivotY + 基準線偏移_下排;
            this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.LineHorzVert = AxOvkMsr.TxAxLineHorzVert.AX_LINE_QUASI_HORIZONTAL;
            this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.LineSlope = this.CCD01_01_水平基準線量測_AxLineRegression.FittedSlope;
            for (int i = 0; i < 22; i++)
            {
                if (this.List_CCD01_02_PIN量測點參數_量測點_有無[i])
                {
                    if (i <= 10)
                    {
                        this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.PivotX = this.List_CCD01_02_PIN量測點參數_量測點[i].X;
                        this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.PivotY = this.List_CCD01_02_PIN量測點參數_量測點[i].Y;
                        this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.FindDistance();
                        this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[i] = CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.ProjectPivotX;
                        this.List_CCD01_02_PIN量測參數_水平度量測顯示點_Y[i] = CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.ProjectPivotY;

                        this.List_CCD01_02_PIN量測參數_水平度量測距離[i] = this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.Distance * this.CCD01_比例尺_pixcel_To_mm;
                    }
                    if (i > 10)
                    {
                        this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.PivotX = this.List_CCD01_02_PIN量測點參數_量測點[i].X;
                        this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.PivotY = this.List_CCD01_02_PIN量測點參數_量測點[i].Y;
                        this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.FindDistance();
                        this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[i] = CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.ProjectPivotX;
                        this.List_CCD01_02_PIN量測參數_水平度量測顯示點_Y[i] = CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.ProjectPivotY;

                        this.List_CCD01_02_PIN量測參數_水平度量測距離[i] = this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.Distance * this.CCD01_比例尺_pixcel_To_mm;
                    }
                }

                List_CCD01_02_PIN量測參數_上下間格距離[0] = (Math.Abs(List_CCD01_02_PIN量測點參數_量測點[0].Y - List_CCD01_02_PIN量測點參數_量測點[11].Y)) * this.CCD01_比例尺_pixcel_To_mm;
                List_CCD01_02_PIN量測參數_上下間格距離[1] = (Math.Abs(List_CCD01_02_PIN量測點參數_量測點[1].Y - List_CCD01_02_PIN量測點參數_量測點[12].Y)) *this.CCD01_比例尺_pixcel_To_mm;
                List_CCD01_02_PIN量測參數_上下間格距離[2] = (Math.Abs(List_CCD01_02_PIN量測點參數_量測點[2].Y - List_CCD01_02_PIN量測點參數_量測點[13].Y)) *this.CCD01_比例尺_pixcel_To_mm;
                List_CCD01_02_PIN量測參數_上下間格距離[3] = (Math.Abs(List_CCD01_02_PIN量測點參數_量測點[3].Y - List_CCD01_02_PIN量測點參數_量測點[14].Y)) *this.CCD01_比例尺_pixcel_To_mm;
                List_CCD01_02_PIN量測參數_上下間格距離[4] = (Math.Abs(List_CCD01_02_PIN量測點參數_量測點[4].Y - List_CCD01_02_PIN量測點參數_量測點[15].Y)) *this.CCD01_比例尺_pixcel_To_mm;
                List_CCD01_02_PIN量測參數_上下間格距離[5] = (Math.Abs(List_CCD01_02_PIN量測點參數_量測點[5].Y - List_CCD01_02_PIN量測點參數_量測點[16].Y)) *this.CCD01_比例尺_pixcel_To_mm;
                List_CCD01_02_PIN量測參數_上下間格距離[6] = (Math.Abs(List_CCD01_02_PIN量測點參數_量測點[6].Y - List_CCD01_02_PIN量測點參數_量測點[17].Y)) *this.CCD01_比例尺_pixcel_To_mm;
                List_CCD01_02_PIN量測參數_上下間格距離[7] = (Math.Abs(List_CCD01_02_PIN量測點參數_量測點[7].Y - List_CCD01_02_PIN量測點參數_量測點[18].Y)) *this.CCD01_比例尺_pixcel_To_mm;
                List_CCD01_02_PIN量測參數_上下間格距離[8] = (Math.Abs(List_CCD01_02_PIN量測點參數_量測點[8].Y - List_CCD01_02_PIN量測點參數_量測點[19].Y)) *this.CCD01_比例尺_pixcel_To_mm;
                List_CCD01_02_PIN量測參數_上下間格距離[9] = (Math.Abs(List_CCD01_02_PIN量測點參數_量測點[9].Y - List_CCD01_02_PIN量測點參數_量測點[20].Y)) *this.CCD01_比例尺_pixcel_To_mm;
                List_CCD01_02_PIN量測參數_上下間格距離[10] = (Math.Abs(List_CCD01_02_PIN量測點參數_量測點[10].Y - List_CCD01_02_PIN量測點參數_量測點[21].Y)) *this.CCD01_比例尺_pixcel_To_mm;
            }



                    
            #endregion
            #region 左右間距數值計算
            double distance = 0;
            double 間距Temp1_X = 0;
            double 間距Temp2_X = 0;

            for (int i = 0; i < 21; i++)
            {
                if (this.List_CCD01_02_PIN量測點參數_量測點_有無[i] && this.List_CCD01_02_PIN量測點參數_量測點_有無[i + 1])
                {

                    間距Temp1_X = this.List_CCD01_02_PIN量測點參數_量測點[i].X - this.Point_CCD01_01_中心基準座標_量測點.X;
                    間距Temp2_X = this.List_CCD01_02_PIN量測點參數_量測點[i + 1].X - this.Point_CCD01_01_中心基準座標_量測點.X;

                    distance = Math.Abs(間距Temp1_X - 間距Temp2_X);

                    this.List_CCD01_02_PIN量測參數_左右間距量測距離[i] = distance * this.CCD01_比例尺_pixcel_To_mm;
                }
                else
                {
                    PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = false;
                    List_CCD01_02_PIN量測參數_量測點_OK[i] = false;
                }

            }
            #endregion
            cnt++;
        }
        void cnt_Program_CCD01_02_PIN量測_檢測距離計算_量測結果(ref int cnt)
        {

            PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = true; // 檢測結果初始化
            #region 左右間距量測判斷

            for (int i = 0; i < 21; i++)
            {
                int 標準值 = this.PLC_Device_CCD01_02_PIN量測_左右間距量測標準值.Value;
                int 標準值上限 = this.PLC_Device_CCD01_02_PIN量測_左右間距量測上限值.Value;
                int 標準值下限 = this.PLC_Device_CCD01_02_PIN量測_左右間距量測下限值.Value;
                double 量測距離 = this.List_CCD01_02_PIN量測參數_左右間距量測距離[i];

                量測距離 = 量測距離 * 1000 - 標準值;
                量測距離 /= 1000;
                if (!PLC_Device_CCD01_02_PIN量測_間距量測不測試.Bool)
                {
                    if (this.List_CCD01_02_PIN量測點參數_量測點_有無[i])
                    {
                        if (量測距離 >= 0 && i != 10)
                        {
                            if (標準值上限 <= Math.Abs(量測距離) * 1000 || 標準值下限 > Math.Abs(量測距離) * 1000)
                            {
                                this.List_CCD01_02_PIN量測參數_左右間距量測_OK[i] = false;
                                PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = false;
                            }
                            else
                            {
                                this.List_CCD01_02_PIN量測參數_左右間距量測_OK[i] = true;
                            }
                        }
                    }
                }
                else
                {
                    PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = true;
                    this.List_CCD01_02_PIN量測參數_左右間距量測_OK[i] = true;
                }



                this.List_CCD01_02_PIN量測參數_左右間距量測距離[i] = 量測距離;

            }
            #endregion
            #region 水平度量測判斷
            #region 上下排水平度
            for (int i = 0; i < 22; i++)
            {
                int 上排標準值 = this.PLC_Device_CCD01_02_PIN量測_上排水平度量測標準值.Value;
                int 上排標準值上限 = this.PLC_Device_CCD01_02_PIN量測_上排水平度量測上限值.Value;
                int 上排標準值下限 = this.PLC_Device_CCD01_02_PIN量測_上排水平度量測下限值.Value;
                double 上排量測距離 = this.List_CCD01_02_PIN量測參數_水平度量測距離[i];

                int 下排標準值 = this.PLC_Device_CCD01_02_PIN量測_下排水平度量測標準值.Value;
                int 下排標準值上限 = this.PLC_Device_CCD01_02_PIN量測_下排水平度量測上限值.Value;
                int 下排標準值下限 = this.PLC_Device_CCD01_02_PIN量測_下排水平度量測下限值.Value;
                double 下排量測距離 = this.List_CCD01_02_PIN量測參數_水平度量測距離[i];

                上排量測距離 = 上排量測距離 * 1000 - 上排標準值;
                上排量測距離 /= 1000;

                下排量測距離 = 下排量測距離 * 1000 - 下排標準值;
                下排量測距離 /= 1000;
                if (!PLC_Device_CCD01_02_PIN量測_水平度量測不測試.Bool)
                {
                    if (this.List_CCD01_02_PIN量測點參數_量測點_有無[i])
                    {
                        if (上排量測距離 >= 0 && i < 11)
                        {
                            if (上排標準值上限 <= Math.Abs(上排量測距離) * 1000 || 上排標準值下限 > Math.Abs(上排量測距離) * 1000)
                            {
                                this.List_CCD01_02_PIN量測參數_水平度量測_OK[i] = false;
                                PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = false;
                            }
                            else
                            {
                                this.List_CCD01_02_PIN量測參數_水平度量測_OK[i] = true;
                            }
                            this.List_CCD01_02_PIN量測參數_水平度量測距離[i] = 上排量測距離;
                        }
                        else if (下排量測距離 >= 0 && i >= 11)
                        {
                            if (下排標準值上限 <= Math.Abs(下排量測距離) * 1000 || 下排標準值下限 > Math.Abs(下排量測距離) * 1000)
                            {
                                this.List_CCD01_02_PIN量測參數_水平度量測_OK[i] = false;
                                PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = false;
                            }
                            else
                            {
                                this.List_CCD01_02_PIN量測參數_水平度量測_OK[i] = true;
                            }
                            this.List_CCD01_02_PIN量測參數_水平度量測距離[i] = 下排量測距離;
                        }

                    }
                }
                else
                {
                    this.List_CCD01_02_PIN量測參數_水平度量測_OK[i] = true;
                }
                if (PLC_Device_CCD01_02_PIN量測_間距量測不測試.Bool && PLC_Device_CCD01_02_PIN量測_水平度量測不測試.Bool)
                {
                    PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = true;
                }



            }
            #endregion
            #region 上下間距
            for (int i = 0; i < 11; i++)
            {
                int 標準值 = this.PLC_Device_CCD01_02_PIN量測_上下間距量測標準值.Value;
                int 標準值上限 = this.PLC_Device_CCD01_02_PIN量測_上下間距量測上限值.Value;
                int 標準值下限 = this.PLC_Device_CCD01_02_PIN量測_上下間距量測下限值.Value;
                double 量測距離 = this.List_CCD01_02_PIN量測參數_上下間格距離[i];

                量測距離 = 量測距離 * 1000 - 標準值;
                量測距離 /= 1000;
                if (!PLC_Device_CCD01_02_PIN量測_水平度量測不測試.Bool)
                {
                    if (this.List_CCD01_02_PIN量測點參數_量測點_有無[i])
                    {

                        if (標準值上限 <= Math.Abs(量測距離) * 1000 || 標準值下限 > Math.Abs(量測距離) * 1000)
                        {
                            this.List_CCD01_02_PIN量測參數_上下間距量測_OK[i] = false;
                            PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = false;
                        }
                        else
                        {
                            this.List_CCD01_02_PIN量測參數_上下間距量測_OK[i] = true;
                        }
                        this.List_CCD01_02_PIN量測參數_上下間格距離[i] = 量測距離;

                    }
                }
                else
                {
                    this.List_CCD01_02_PIN量測參數_上下間距量測_OK[i] = true;
                }
                if (PLC_Device_CCD01_02_PIN量測_間距量測不測試.Bool && PLC_Device_CCD01_02_PIN量測_水平度量測不測試.Bool)
                {
                    PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = true;
                }



            }
            #endregion
            #endregion
            #region 間距上排PIN01到基準線量測

            double temp_上排PIN01到基準 = 0;
            int 間距上排PIN01到基準標準值 = this.PLC_Device_CCD01_02_PIN量測_間距上排PIN01到基準數值.Value;
            int 間距上排PIN01到基準標準值上限 = this.PLC_Device_CCD01_02_PIN量測_間距上排PIN01到基準上限.Value;
            int 間距上排PIN01到基準標準值下限 = this.PLC_Device_CCD01_02_PIN量測_間距上排PIN01到基準下限.Value;


            if (this.List_CCD01_02_PIN量測點參數_量測點_有無[0])
            {
                temp_上排PIN01到基準 = Math.Abs(this.List_CCD01_02_PIN量測點參數_量測點[0].X - this.Point_CCD01_01_中心基準座標_量測點.X);
                this.CCD01_02_PIN量測參數_間距上排PIN01到基準距離 = temp_上排PIN01到基準 * this.CCD01_比例尺_pixcel_To_mm;
            }
            else
            {
               // PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = false;
                CCD01_02_PIN量測參數_間距上排PIN01到基準_OK = false;
            }
            double 間距上排PIN01到基準量測距離 = this.CCD01_02_PIN量測參數_間距上排PIN01到基準距離;


            間距上排PIN01到基準量測距離 = 間距上排PIN01到基準量測距離 * 1000 - 間距上排PIN01到基準標準值;
            間距上排PIN01到基準量測距離 /= 1000;

            if (!PLC_Device_CCD01_02_PIN量測_間距量測不測試.Bool)
            {
                if (this.List_CCD01_02_PIN量測點參數_量測點_有無[0])
                {
                    if (間距上排PIN01到基準標準值上限 <= Math.Abs(間距上排PIN01到基準量測距離) * 1000 || 間距上排PIN01到基準標準值下限 >
                        Math.Abs(間距上排PIN01到基準量測距離) * 1000)
                    {
                        this.CCD01_02_PIN量測參數_間距上排PIN01到基準_OK = false;
                        //PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = false;
                    }
                    else
                    {
                        this.CCD01_02_PIN量測參數_間距上排PIN01到基準_OK = true;
                    }

                }
                CCD01_02_PIN量測參數_間距上排PIN01到基準距離 = 間距上排PIN01到基準量測距離;
            }
            else
            {
                this.CCD01_02_PIN量測參數_間距上排PIN01到基準_OK = true;
                //this.PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = true;
            }
            this.CCD01_02_PIN量測參數_間距上排PIN01到基準_OK = true; //pass
            #endregion
            #region 間距下排PIN01到基準線量測

            double temp_下排PIN01到基準 = 0;
            int 間距下排PIN01到基準標準值 = this.PLC_Device_CCD01_02_PIN量測_間距下排PIN01到基準數值.Value;
            int 間距下排PIN01到基準標準值上限 = this.PLC_Device_CCD01_02_PIN量測_間距下排PIN01到基準上限.Value;
            int 間距下排PIN01到基準標準值下限 = this.PLC_Device_CCD01_02_PIN量測_間距下排PIN01到基準下限.Value;


            if (this.List_CCD01_02_PIN量測點參數_量測點_有無[11])
            {
                temp_下排PIN01到基準 = Math.Abs(this.List_CCD01_02_PIN量測點參數_量測點[10].X - this.Point_CCD01_01_中心基準座標_量測點.X);
                this.CCD01_02_PIN量測參數_間距下排PIN01到基準距離 = temp_下排PIN01到基準 * this.CCD01_比例尺_pixcel_To_mm;
            }
            else
            {
                //PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = false;
                CCD01_02_PIN量測參數_間距下排PIN01到基準_OK = false;
            }
            double 間距下排PIN01到基準量測距離 = this.CCD01_02_PIN量測參數_間距下排PIN01到基準距離;


            間距下排PIN01到基準量測距離 = 間距下排PIN01到基準量測距離 * 1000 - 間距下排PIN01到基準標準值;
            間距下排PIN01到基準量測距離 /= 1000;

            if (!PLC_Device_CCD01_02_PIN量測_間距量測不測試.Bool)
            {
                if (this.List_CCD01_02_PIN量測點參數_量測點_有無[11])
                {
                    if (間距下排PIN01到基準標準值上限 <= Math.Abs(間距下排PIN01到基準量測距離) * 1000 || 間距下排PIN01到基準標準值下限 >
                        Math.Abs(間距下排PIN01到基準量測距離) * 1000)
                    {
                        this.CCD01_02_PIN量測參數_間距下排PIN01到基準_OK = false;
                        //PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = false;
                    }
                    else
                    {
                        this.CCD01_02_PIN量測參數_間距下排PIN01到基準_OK = true;
                    }

                }
                CCD01_02_PIN量測參數_間距下排PIN01到基準距離 = 間距下排PIN01到基準量測距離;
            }
            else
            {
                this.CCD01_02_PIN量測參數_間距下排PIN01到基準_OK = true;
                //this.PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = true;
            }
            this.CCD01_02_PIN量測參數_間距下排PIN01到基準_OK = true;
            #endregion
            cnt++;
        }
        void cnt_Program_CCD01_02_PIN量測_檢測距離計算_繪製畫布(ref int cnt)
        {
            this.PLC_Device_CCD01_02_PIN量測_檢測距離計算_RefreshCanvas.Bool = true;
            if (this.PLC_Device_CCD01_02_PIN量測_檢測距離計算按鈕.Bool && !PLC_Device_CCD01_02_計算一次.Bool)
            {

                this.h_Canvas_Tech_CCD01_02.RefreshCanvas();
            }
            cnt++;
        }
        #endregion
        #region PLC_CCD01_02_PIN正位度量測_設定規範位置
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_設定規範按鈕 = new PLC_Device("S6070");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_設定規範位置 = new PLC_Device("S6065");
        PLC_Device PLC_Device_CCD01_02_PIN設定規範位置_OK = new PLC_Device("S6066");
        PLC_Device PLC_Device_CCD01_02_PIN設定規範位置_測試完成 = new PLC_Device("S6067");
        PLC_Device PLC_Device_CCD01_02_PIN設定規範位置_RefreshCanvas = new PLC_Device("S6066");
        private List<PLC_Device> List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y = new List<PLC_Device>();
        private AxOvkPat.AxVisionInspectionFrame CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整;

        #region 正位度規範值
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN01 = new PLC_Device("F11000");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN02 = new PLC_Device("F11001");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN03 = new PLC_Device("F11002");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN04 = new PLC_Device("F11003");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN05 = new PLC_Device("F11004");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN06 = new PLC_Device("F11005");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN07 = new PLC_Device("F11006");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN08 = new PLC_Device("F11007");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN09 = new PLC_Device("F11008");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN10 = new PLC_Device("F11009");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_上排PIN11 = new PLC_Device("F11040");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN11 = new PLC_Device("F11010");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN12 = new PLC_Device("F11011");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN13 = new PLC_Device("F11012");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN14 = new PLC_Device("F11013");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN15 = new PLC_Device("F11014");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN16 = new PLC_Device("F11015");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN17 = new PLC_Device("F11016");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN18 = new PLC_Device("F11017");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN19 = new PLC_Device("F11018");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN20 = new PLC_Device("F11019");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_下排PIN11 = new PLC_Device("F11041");

        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN01 = new PLC_Device("F11020");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN02 = new PLC_Device("F11021");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN03 = new PLC_Device("F11022");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN04 = new PLC_Device("F11023");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN05 = new PLC_Device("F11024");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN06 = new PLC_Device("F11025");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN07 = new PLC_Device("F11026");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN08 = new PLC_Device("F11027");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN09 = new PLC_Device("F11028");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN10 = new PLC_Device("F11029");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_上排PIN11 = new PLC_Device("F11042");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN11 = new PLC_Device("F11030");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN12 = new PLC_Device("F11031");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN13 = new PLC_Device("F11032");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN14 = new PLC_Device("F11033");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN15 = new PLC_Device("F11034");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN16 = new PLC_Device("F11035");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN17 = new PLC_Device("F11036");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN18 = new PLC_Device("F11037");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN19 = new PLC_Device("F11038");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN20 = new PLC_Device("F11039");
        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_下排PIN11 = new PLC_Device("F11043");
        #endregion
        private PointF[] List_CCD01_02_PIN正位度量測參數_規範點 = new PointF[22];
        private PointF[] List_CCD01_02_PIN正位度量測參數_轉換後座標 = new PointF[22];
        private double[] List_CCD01_02_PIN正位度量測參數_正位度規範點_X = new double[22];
        private double[] List_CCD01_02_PIN正位度量測參數_正位度規範點_Y = new double[22];

        int cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 = 65534;

        private void H_Canvas_Tech_CCD01_02_PIN正位度設定規範位置_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {

            if (PLC_Device_CCD01_02_Main_取像並檢驗.Bool || PLC_Device_CCD01_02_PLC觸發檢測.Bool || PLC_Device_CCD01_02_Main_檢驗一次按鈕.Bool)
            {
                try
                {
                    Graphics g = Graphics.FromHdc((IntPtr)HDC);
                    for (int i = 0; i < 22; i++)
                    {
                        DrawingClass.Draw.十字中心(new PointF((float)List_CCD01_02_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD01_02_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
                    }
                    g.Dispose();
                    g = null;
                }
                catch
                {

                }

            }

            else if (PLC_Device_CCD01_02_Tech_檢驗一次.Bool || PLC_Device_CCD01_02_Tech_取像並檢驗.Bool)
            {
                if (this.PLC_Device_CCD01_02_PIN設定規範位置_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        for (int i = 0; i < 22; i++)
                        {
                            DrawingClass.Draw.十字中心(new PointF((float)List_CCD01_02_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD01_02_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
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
                if (this.PLC_Device_CCD01_02_PIN設定規範位置_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        for (int i = 0; i < 22; i++)
                        {
                            DrawingClass.Draw.十字中心(new PointF((float)List_CCD01_02_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD01_02_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
                        }
                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }
                }
            }



            this.PLC_Device_CCD01_02_PIN設定規範位置_RefreshCanvas.Bool = false;
        }
        void sub_Program_CCD01_02_PIN正位度量測_設定規範位置()
        {
            if (cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 == 65534)
            {

                this.h_Canvas_Tech_CCD01_02.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_02_PIN正位度設定規範位置_OnCanvasDrawEvent;
                this.h_Canvas_Main_CCD01_02_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_02_PIN正位度設定規範位置_OnCanvasDrawEvent;

                PLC_Device_CCD01_02_PIN正位度量測_設定規範位置.SetComment("PLC_CCD01_02_PIN正位度量測_設定規範位置");
                PLC_Device_CCD01_02_PIN正位度量測_設定規範按鈕.Bool = false;
                PLC_Device_CCD01_02_PIN正位度量測_設定規範位置.Bool = false;
                cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 = 65535;
                #region 正位度規範值
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN01);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN02);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN03);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN04);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN05);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN06);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN07);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN08);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN09);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN10);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_上排PIN11);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN11);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN12);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN13);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN14);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN15);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN16);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN17);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN18);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN19);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN20);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_下排PIN11);

                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN01);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN02);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN03);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN04);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN05);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN06);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN07);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN08);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN09);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN10);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_上排PIN11);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN11);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN12);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN13);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN14);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN15);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN16);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN17);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN18);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN19);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN20);
                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_下排PIN11);
                #endregion
            }
            if (cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 == 65535) cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 = 1;
            if (cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 == 1) cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_觸發按下(ref cnt_Program_CCD01_02_PIN正位度量測_設定規範位置);
            if (cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 == 2) cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_檢查按下(ref cnt_Program_CCD01_02_PIN正位度量測_設定規範位置);
            if (cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 == 3) cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_初始化(ref cnt_Program_CCD01_02_PIN正位度量測_設定規範位置);
            if (cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 == 4) cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_座標轉換(ref cnt_Program_CCD01_02_PIN正位度量測_設定規範位置);
            if (cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 == 5) cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_讀取參數(ref cnt_Program_CCD01_02_PIN正位度量測_設定規範位置);
            if (cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 == 6) cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_繪製畫布(ref cnt_Program_CCD01_02_PIN正位度量測_設定規範位置);
            if (cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 == 7) cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 = 65500;
            if (cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 > 1) cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_檢查放開(ref cnt_Program_CCD01_02_PIN正位度量測_設定規範位置);

            if (cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 == 65500)
            {
                if (PLC_Device_CCD01_02_計算一次.Bool)
                {
                    PLC_Device_CCD01_02_PIN正位度量測_設定規範按鈕.Bool = false;
                    PLC_Device_CCD01_02_PIN正位度量測_設定規範位置.Bool = false;
                }
                cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 = 65535;
            }
        }
        void cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_觸發按下(ref int cnt)
        {
            if (PLC_Device_CCD01_02_PIN正位度量測_設定規範按鈕.Bool)
            {
                PLC_Device_CCD01_02_PIN正位度量測_設定規範位置.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_02_PIN正位度量測_設定規範位置.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_02_PIN正位度量測_設定規範按鈕.Bool)
            {
                PLC_Device_CCD01_02_PIN正位度量測_設定規範位置.Bool = false;
                cnt = 65500;
            }
        }
        void cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_初始化(ref int cnt)
        {
            this.List_CCD01_02_PIN正位度量測參數_正位度規範點_X = new double[22];
            this.List_CCD01_02_PIN正位度量測參數_正位度規範點_Y = new double[22];
            this.List_CCD01_02_PIN正位度量測參數_規範點 = new PointF[22];
            this.List_CCD01_02_PIN正位度量測參數_轉換後座標 = new PointF[22];
            cnt++;
        }
        void cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_座標轉換(ref int cnt)
        {
            if (PLC_Device_CCD01_02_計算一次.Bool)
            {
                CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.RefPointX = PLC_Device_CCD01_01_水平基準線量測_量測中心_X.Value;
                CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.RefPointY = PLC_Device_CCD01_01_水平基準線量測_量測中心_Y.Value;
                CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.RefAngle = 0;
                CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.CurrentRefPointX = Point_CCD01_01_中心基準座標_量測點.X;
                CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.CurrentRefPointY = Point_CCD01_01_中心基準座標_量測點.Y;
                CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.CurrentRefAngle = 0;
                CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.NumOfVisionPoints = 22;

                for (int j = 0; j < 22; j++)
                {

                    CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
                    CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointX = (float)(List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X[j].Value);
                    CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointX = CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointX / 1;
                    CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointY = (float)(List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y[j].Value);
                    CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointY = CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointY / 1;

                }
                CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.EstimateCurrentVisionPoints();
                for (int j = 0; j < 22; j++)
                {

                    CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
                    List_CCD01_02_PIN正位度量測參數_轉換後座標[j].X = (int)CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.CurrentVisionPointX;
                    List_CCD01_02_PIN正位度量測參數_轉換後座標[j].Y = (int)CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.CurrentVisionPointY;

                }
            }
            cnt++;
        }
        void cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_讀取參數(ref int cnt)
        {

            for (int i = 0; i < 22; i++)
            {
                if (PLC_Device_CCD01_02_計算一次.Bool)
                {
                    this.List_CCD01_02_PIN正位度量測參數_正位度規範點_X[i] = List_CCD01_02_PIN正位度量測參數_轉換後座標[i].X;
                    this.List_CCD01_02_PIN正位度量測參數_正位度規範點_Y[i] = List_CCD01_02_PIN正位度量測參數_轉換後座標[i].Y;
                }
                else
                {
                    this.List_CCD01_02_PIN正位度量測參數_正位度規範點_X[i] = (float)(List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X[i].Value);
                    this.List_CCD01_02_PIN正位度量測參數_正位度規範點_X[i] = this.List_CCD01_02_PIN正位度量測參數_正位度規範點_X[i] / 1;
                    this.List_CCD01_02_PIN正位度量測參數_正位度規範點_Y[i] = (float)(List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y[i].Value);
                    this.List_CCD01_02_PIN正位度量測參數_正位度規範點_Y[i] = this.List_CCD01_02_PIN正位度量測參數_正位度規範點_Y[i] / 1;
                }
                List_CCD01_02_PIN正位度量測參數_規範點[i].X = (float)this.List_CCD01_02_PIN正位度量測參數_正位度規範點_X[i];
                List_CCD01_02_PIN正位度量測參數_規範點[i].Y = (float)this.List_CCD01_02_PIN正位度量測參數_正位度規範點_Y[i];

            }
            cnt++;
        }
        void cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_繪製畫布(ref int cnt)
        {
            this.PLC_Device_CCD01_02_PIN設定規範位置_RefreshCanvas.Bool = true;
            if (this.PLC_Device_CCD01_02_PIN正位度量測_設定規範按鈕.Bool && !PLC_Device_CCD01_02_計算一次.Bool)
            {
                this.h_Canvas_Tech_CCD01_02.RefreshCanvas();
            }

            cnt++;
        }



        #endregion
        #region PLC_CCD01_02_PIN量測_檢測正位度計算

        MyTimer MyTimer_CCD01_02_PIN量測_檢測正位度計算 = new MyTimer();
        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測正位度計算按鈕 = new PLC_Device("S6090");
        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測正位度計算 = new PLC_Device("S6085");
        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測正位度計算_OK = new PLC_Device("S6086");
        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測正位度計算_測試完成 = new PLC_Device("S6087");
        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測正位度計算_RefreshCanvas = new PLC_Device("S6088");
        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測正位度計算_不測試 = new PLC_Device("S6102");
        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測正位度計算_差值 = new PLC_Device("F15000");

        private double[] List_CCD01_02_PIN正位度量測參數_正位度距離 = new double[22];
        private bool[] List_CCD01_02_PIN正位度量測參數_正位度量測點_OK = new bool[22];


        private void H_Canvas_Tech_CCD01_02_PIN量測正位度_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {
            if (PLC_Device_CCD01_02_Main_取像並檢驗.Bool || PLC_Device_CCD01_02_PLC觸發檢測.Bool || PLC_Device_CCD01_02_Main_檢驗一次按鈕.Bool)
            {
                try
                {
                    Graphics g = Graphics.FromHdc((IntPtr)HDC);
                    Font font = new Font("微軟正黑體", 10);
                    string 正位度差值顯示;
                    for (int i = 0; i < 22; i++)
                    {
                        DrawingClass.Draw.十字中心(new PointF((float)List_CCD01_02_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD01_02_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.十字中心(this.List_CCD01_02_PIN量測點參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);

                    }
                    #region 正位度量測結果顯示
                    if (PLC_Device_CCD01_02_PIN量測_檢測正位度計算_OK.Bool)
                    {
                        DrawingClass.Draw.文字左上繪製("正位OK:", new PointF(1200, 0), new Font("標楷體", 14), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                    }
                    else
                    {
                        DrawingClass.Draw.文字左上繪製("正位NG:", new PointF(1200, 0), new Font("標楷體", 14), Color.Black, Color.Red, g, ZoomX, ZoomY);
                    }
                    #endregion
                    #region PIN正位結果顯示
                    for (int i = 0; i < 22; i++)
                    {

                        if (this.List_CCD01_02_PIN正位度量測參數_正位度量測點_OK[i])
                        {
                            if (i <= 10)
                            {
                                正位度差值顯示 = ("上:P" + (i + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1600, i * 55), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            }

                            if (i >= 11)
                            {
                                正位度差值顯示 = ("下:P" + ((i - 11) + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(2100, (i - 11) * 55), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                            }
                        }
                        else
                        {
                            if (i <= 10)
                            {
                                正位度差值顯示 = ("上:P" + (i + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1600, i * 55), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            }

                            if (i >= 11)
                            {
                                正位度差值顯示 = ("下:P" + ((i - 11) + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
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
            if (PLC_Device_CCD01_02_Tech_檢驗一次.Bool || PLC_Device_CCD01_02_Tech_取像並檢驗.Bool)
            {
                if (this.PLC_Device_CCD01_02_PIN量測_檢測正位度計算_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);
                        string 正位度差值顯示;
                        for (int i = 0; i < 22; i++)
                        {
                            DrawingClass.Draw.十字中心(new PointF((float)List_CCD01_02_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD01_02_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
                            DrawingClass.Draw.十字中心(this.List_CCD01_02_PIN量測點參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);

                        }
                        #region 正位度量測結果顯示
                        if (PLC_Device_CCD01_02_PIN量測_檢測正位度計算_OK.Bool)
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

                            if (this.List_CCD01_02_PIN正位度量測參數_正位度量測點_OK[i])
                            {
                                if (i <= 10)
                                {
                                    正位度差值顯示 = ("上排:P" + (i + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1900, i * 40), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                                }

                                if (i >= 11)
                                {
                                    正位度差值顯示 = ("下排:P" + ((i - 11) + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(2200, (i - 11) * 40), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                                }
                            }
                            else
                            {
                                if (i <= 10)
                                {
                                    正位度差值顯示 = ("上排:P" + (i + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1900, i * 40), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                                }

                                if (i >= 11)
                                {
                                    正位度差值顯示 = ("下排:P" + ((i - 11) + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
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
                if (this.PLC_Device_CCD01_02_PIN量測_檢測正位度計算_RefreshCanvas.Bool && PLC_Device_CCD01_02_PIN量測_檢測正位度計算.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);
                        string 正位度差值顯示;
                        for (int i = 0; i < 22; i++)
                        {
                            DrawingClass.Draw.十字中心(new PointF((float)List_CCD01_02_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD01_02_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
                            DrawingClass.Draw.十字中心(this.List_CCD01_02_PIN量測點參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
                        }
                        #region 正位度量測結果顯示
                        if (PLC_Device_CCD01_02_PIN量測_檢測正位度計算_OK.Bool)
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

                            if (this.List_CCD01_02_PIN正位度量測參數_正位度量測點_OK[i])
                            {
                                if (i <= 10)
                                {
                                    正位度差值顯示 = ("上排:P" + (i + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1900, i * 40), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                                }

                                if (i >= 11)
                                {
                                    正位度差值顯示 = ("下排:P" + ((i - 11) + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(2200, (i - 11) * 40), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                                }
                            }
                            else
                            {
                                if (i <= 10)
                                {
                                    正位度差值顯示 = ("上排:P" + (i + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1900, i * 40), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                                }

                                if (i >= 11)
                                {
                                    正位度差值顯示 = ("下排:P" + ((i - 11) + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
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

            this.PLC_Device_CCD01_02_PIN量測_檢測正位度計算_RefreshCanvas.Bool = false;
        }

        int cnt_Program_CCD01_02_PIN量測_檢測正位度計算 = 65534;
        void sub_Program_CCD01_02_PIN量測_檢測正位度計算()
        {
            if (cnt_Program_CCD01_02_PIN量測_檢測正位度計算 == 65534)
            {
                this.h_Canvas_Tech_CCD01_02.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_02_PIN量測正位度_OnCanvasDrawEvent;
                this.h_Canvas_Main_CCD01_02_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_02_PIN量測正位度_OnCanvasDrawEvent;
                PLC_Device_CCD01_02_PIN量測_檢測正位度計算.SetComment("PLC_CCD01_02_PIN量測_檢測正位度計算");
                PLC_Device_CCD01_02_PIN量測_檢測正位度計算.Bool = false;
                PLC_Device_CCD01_02_PIN量測_檢測正位度計算按鈕.Bool = false;
                cnt_Program_CCD01_02_PIN量測_檢測正位度計算 = 65535;

            }
            if (cnt_Program_CCD01_02_PIN量測_檢測正位度計算 == 65535) cnt_Program_CCD01_02_PIN量測_檢測正位度計算 = 1;
            if (cnt_Program_CCD01_02_PIN量測_檢測正位度計算 == 1) cnt_Program_CCD01_02_PIN量測_檢測正位度計算_觸發按下(ref cnt_Program_CCD01_02_PIN量測_檢測正位度計算);
            if (cnt_Program_CCD01_02_PIN量測_檢測正位度計算 == 2) cnt_Program_CCD01_02_PIN量測_檢測正位度計算_檢查按下(ref cnt_Program_CCD01_02_PIN量測_檢測正位度計算);
            if (cnt_Program_CCD01_02_PIN量測_檢測正位度計算 == 3) cnt_Program_CCD01_02_PIN量測_檢測正位度計算_初始化(ref cnt_Program_CCD01_02_PIN量測_檢測正位度計算);
            if (cnt_Program_CCD01_02_PIN量測_檢測正位度計算 == 4) cnt_Program_CCD01_02_PIN量測_檢測正位度計算_數值計算(ref cnt_Program_CCD01_02_PIN量測_檢測正位度計算);
            if (cnt_Program_CCD01_02_PIN量測_檢測正位度計算 == 5) cnt_Program_CCD01_02_PIN量測_檢測正位度計算_量測結果(ref cnt_Program_CCD01_02_PIN量測_檢測正位度計算);
            if (cnt_Program_CCD01_02_PIN量測_檢測正位度計算 == 6) cnt_Program_CCD01_02_PIN量測_檢測正位度計算_繪製畫布(ref cnt_Program_CCD01_02_PIN量測_檢測正位度計算);
            if (cnt_Program_CCD01_02_PIN量測_檢測正位度計算 == 7) cnt_Program_CCD01_02_PIN量測_檢測正位度計算 = 65500;
            if (cnt_Program_CCD01_02_PIN量測_檢測正位度計算 > 1) cnt_Program_CCD01_02_PIN量測_檢測正位度計算_檢查放開(ref cnt_Program_CCD01_02_PIN量測_檢測正位度計算);

            if (cnt_Program_CCD01_02_PIN量測_檢測正位度計算 == 65500)
            {
                PLC_Device_CCD01_02_PIN量測_檢測正位度計算.Bool = false;
                PLC_Device_CCD01_02_PIN量測_檢測正位度計算按鈕.Bool = false;
                cnt_Program_CCD01_02_PIN量測_檢測正位度計算 = 65535;
            }
        }
        void cnt_Program_CCD01_02_PIN量測_檢測正位度計算_觸發按下(ref int cnt)
        {
            if (PLC_Device_CCD01_02_PIN量測_檢測正位度計算按鈕.Bool)
            {
                PLC_Device_CCD01_02_PIN量測_檢測正位度計算.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_02_PIN量測_檢測正位度計算_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_02_PIN量測_檢測正位度計算.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_02_PIN量測_檢測正位度計算_檢查放開(ref int cnt)
        {
            //if (!PLC_Device_CCD01_02_PIN量測_檢測正位度計算按鈕.Bool)
            //{
            //    cnt = 65500;
            //}
        }
        void cnt_Program_CCD01_02_PIN量測_檢測正位度計算_初始化(ref int cnt)
        {
            this.MyTimer_CCD01_02_PIN量測_檢測正位度計算.TickStop();
            this.MyTimer_CCD01_02_PIN量測_檢測正位度計算.StartTickTime(99999);
            this.List_CCD01_02_PIN正位度量測參數_正位度距離 = new double[22];
            this.List_CCD01_02_PIN正位度量測參數_正位度量測點_OK = new bool[22];

            cnt++;
        }
        void cnt_Program_CCD01_02_PIN量測_檢測正位度計算_數值計算(ref int cnt)
        {
            double distance = 0;
            double temp_X = 0;
            double temp_Y = 0;
            PLC_Device_CCD01_02_PIN量測_檢測正位度計算_OK.Bool = true;

            for (int i = 0; i < 22; i++)
            {
                if (this.List_CCD01_02_PIN量測點參數_量測點_有無[i])
                {
                    temp_X = Math.Pow(this.List_CCD01_02_PIN量測點參數_量測點[i].X - this.List_CCD01_02_PIN正位度量測參數_規範點[i].X, 2);
                    temp_Y = Math.Pow(this.List_CCD01_02_PIN量測點參數_量測點[i].Y - this.List_CCD01_02_PIN正位度量測參數_規範點[i].Y, 2);

                    distance = Math.Sqrt(temp_X + temp_Y);
                    this.List_CCD01_02_PIN正位度量測參數_正位度距離[i] = distance * this.CCD01_比例尺_pixcel_To_mm;
                }
                else
                {
                    PLC_Device_CCD01_02_PIN量測_檢測正位度計算_OK.Bool = false;
                    List_CCD01_02_PIN正位度量測參數_正位度量測點_OK[i] = false;
                }

            }
            cnt++;
        }
        void cnt_Program_CCD01_02_PIN量測_檢測正位度計算_量測結果(ref int cnt)
        {

            PLC_Device_CCD01_02_PIN量測_檢測正位度計算_OK.Bool = true; // 檢測結果初始化

            for (int i = 0; i < 22; i++)
            {
                int 標準值差值 = this.PLC_Device_CCD01_02_PIN量測_檢測正位度計算_差值.Value;
                double 量測距離 = this.List_CCD01_02_PIN正位度量測參數_正位度距離[i];

                量測距離 = 量測距離 * 1000;
                量測距離 /= 1000;

                if (!PLC_Device_CCD01_02_PIN量測_檢測正位度計算_不測試.Bool)
                {
                    if (this.List_CCD01_02_PIN量測點參數_量測點_有無[i])
                    {


                        if (量測距離 >= 0)
                        {
                            if (標準值差值 <= Math.Abs(量測距離) * 1000)
                            {
                                this.List_CCD01_02_PIN正位度量測參數_正位度量測點_OK[i] = false;
                                PLC_Device_CCD01_02_PIN量測_檢測正位度計算_OK.Bool = false;
                            }
                            else
                            {
                                this.List_CCD01_02_PIN正位度量測參數_正位度量測點_OK[i] = true;
                            }
                        }

                    }
                    else
                    {
                        this.List_CCD01_02_PIN正位度量測參數_正位度量測點_OK[i] = false;
                        PLC_Device_CCD01_02_PIN量測_檢測正位度計算_OK.Bool = false;
                    }

                }
                else
                {
                    this.List_CCD01_02_PIN正位度量測參數_正位度量測點_OK[i] = true;
                }

                this.List_CCD01_02_PIN正位度量測參數_正位度距離[i] = 量測距離;
            }
            cnt++;
        }
        void cnt_Program_CCD01_02_PIN量測_檢測正位度計算_繪製畫布(ref int cnt)
        {
            this.PLC_Device_CCD01_02_PIN量測_檢測正位度計算_RefreshCanvas.Bool = true;
            if (this.PLC_Device_CCD01_02_PIN量測_檢測正位度計算按鈕.Bool && !PLC_Device_CCD01_02_計算一次.Bool)
            {

                this.h_Canvas_Tech_CCD01_02.RefreshCanvas();
            }

            cnt++;
        }
        #endregion
        #region Event
        private void PlC_RJ_Button_CCD01_02_儲存圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (saveImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.h_Canvas_Tech_CCD01_02.SaveImage(saveImageDialog.FileName);
                }
            }));
        }
        private void PlC_RJ_Button_CCD01_02_讀取圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (openImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.CCD01_AxImageBW8.LoadFile(openImageDialog.FileName);
                    try
                    {
                        this.h_Canvas_Tech_CCD01_02.ImageCopy(CCD01_AxImageBW8.VegaHandle);
                        this.CCD01_02_SrcImageHandle = h_Canvas_Tech_CCD01_02.VegaHandle;
                        this.h_Canvas_Tech_CCD01_02.RefreshCanvas();
                    }
                    catch
                    {
                        err_message01_02 = MessageBox.Show("讀取圖片空白", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        if (err_message01_02 == DialogResult.OK)
                        {

                        }
                    }
                }
            }));
        }
        private void PlC_RJ_Button_Main_CCD01_02儲存圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (saveImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.h_Canvas_Main_CCD01_02_檢測畫面.SaveImage(saveImageDialog.FileName);
                }
            }));
        }
        private void PlC_RJ_Button_Main_CCD01_02讀取圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (openImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.CCD01_AxImageBW8.LoadFile(openImageDialog.FileName);
                    try
                    {
                        this.h_Canvas_Main_CCD01_02_檢測畫面.ImageCopy(CCD01_AxImageBW8.VegaHandle);
                        this.CCD01_02_SrcImageHandle = h_Canvas_Main_CCD01_02_檢測畫面.VegaHandle;
                        this.h_Canvas_Main_CCD01_02_檢測畫面.RefreshCanvas();
                    }
                    catch
                    {
                        err_message01_02 = MessageBox.Show("讀取圖片空白", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                        if (err_message01_02 == DialogResult.OK)
                        {

                        }
                    }
                }
            }));
        }
        private void PlC_Button_Main_CCD01_02_ZOOM更新_btnClick(object sender, EventArgs e)
        {
            if (CCD01_02_SrcImageHandle != 0)
            {
                PLC_Device_Main_CCD01_02_ZOOM更新.Bool = true;
                h_Canvas_Main_CCD01_02_檢測畫面.RefreshCanvas();
            }
        }

        private void plC_RJ_Button_CCD01_02_Tech_PIN量測框大小設為一致_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 22; i++)
            {
                this.List_PLC_Device_CCD01_02_PIN量測參數_Width[i].Value = this.List_PLC_Device_CCD01_02_PIN量測參數_Width[0].Value;
                this.List_PLC_Device_CCD01_02_PIN量測參數_Height[i].Value = this.List_PLC_Device_CCD01_02_PIN量測參數_Height[0].Value;
                //this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值[i].Value = this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值[0].Value;
                //this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限[i].Value = this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限[0].Value;
                //this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限[i].Value = this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限[0].Value;

            }
        }
        private PLC_Device PLC_Device_CCD01_02_PIN量測一鍵排列間距 = new PLC_Device("F4000");
        private void plC_RJ_Button_CCD01_02_Tech_PIN量測框一鍵排列_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 22; i++)
            {
                if (i < 11)
                {
                    this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[i].Value = this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[0].Value - i * PLC_Device_CCD01_02_PIN量測一鍵排列間距.Value;
                    this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[i].Value = this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[0].Value;

                }

                else
                {
                    this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[i].Value = this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[11].Value - (i - 11) * PLC_Device_CCD01_02_PIN量測一鍵排列間距.Value;
                    this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[i].Value = this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[11].Value;

                }

            }
        }
        private void PlC_Button_CCD01_02_量測點作為規範點_btnClick(object sender, EventArgs e)
        {
            for (int i = 0; i < 22; i++)
            {
                List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X[i].Value = (int)this.List_CCD01_02_PIN量測點參數_量測點[i].X;
                List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y[i].Value = (int)this.List_CCD01_02_PIN量測點參數_量測點[i].Y;
            }
        }

        private PLC_Device PLC_Device_CCD01_02_線段量測框一鍵排列間距 = new PLC_Device("F4002");
        private void plC_RJ_Button_CCD01_02_線段量測框大小設為一致_MouseClickEvent(MouseEventArgs mevent)
        {
            for (int i = 0; i < 22; i++)
            {
                this.PLC_Device_CCD01_02_量測框Width[i].Value = this.PLC_Device_CCD01_02_量測框Width[0].Value;
                this.PLC_Device_CCD01_02_量測框Height[i].Value = this.PLC_Device_CCD01_02_量測框Height[0].Value;

            }
            
        }

        private void plC_RJ_Button_CCD01_02_線段量測框一鍵排列_MouseClickEvent(MouseEventArgs mevent)
        {
            for (int i = 0; i < 22; i++)
            {
                if (i < 11)
                {
                    this.PLC_Device_CCD01_02_量測框OrgX[i].Value = this.PLC_Device_CCD01_02_量測框OrgX[0].Value - i * PLC_Device_CCD01_02_線段量測框一鍵排列間距.Value;
                    this.PLC_Device_CCD01_02_量測框OrgY[i].Value = this.PLC_Device_CCD01_02_量測框OrgY[0].Value;
                }

                else
                {
                    this.PLC_Device_CCD01_02_量測框OrgX[i].Value = this.PLC_Device_CCD01_02_量測框OrgX[11].Value - (i - 11) * PLC_Device_CCD01_02_線段量測框一鍵排列間距.Value;
                    this.PLC_Device_CCD01_02_量測框OrgY[i].Value = this.PLC_Device_CCD01_02_量測框OrgY[11].Value;
                }

            }
        }
        #endregion

        //        #region PLC_CCD01_02_PIN量測_量測框調整
        //        MyTimer MyTimer_CCD01_02_PIN量測_量測框調整 = new MyTimer();
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_量測框調整按鈕 = new PLC_Device("S6030");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_量測框調整 = new PLC_Device("S6025");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_量測框調整_OK = new PLC_Device("S6026");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_量測框調整_測試完成 = new PLC_Device("S6027");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_量測框調整_RefreshCanvas = new PLC_Device("S6028");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_有無量測不測試 = new PLC_Device("S6120");
        //        private List<AxOvkBase.AxROIBW8> List_CCD01_02_PIN量測_AxROIBW8_量測框調整 = new List<AxOvkBase.AxROIBW8>();
        //        private List<AxOvkBlob.AxObject> List_CCD01_02_PIN量測_AxObject_區塊分析 = new List<AxOvkBlob.AxObject>();
        //        private List<AxOvkMsr.AxAngleMsr> List_CCD01_02_PIN量測_AxAngleMsr_量測框調整 = new List<AxOvkMsr.AxAngleMsr>();
        //        private AxOvkPat.AxVisionInspectionFrame CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整;

        //        private PointF[] List_CCD01_02_PIN量測參數_量測點 = new PointF[20];
        //        private PointF[] List_CCD01_02_PIN量測參數_量測點_結果 = new PointF[20];
        //        private Point[] List_CCD01_02_PIN量測參數_量測點_轉換後座標 = new Point[20];
        //        private bool[] List_CCD01_02_PIN量測參數_量測點_有無 = new bool[20];
        //        #region BLOB量測框調整
        //        private List<PLC_Device> List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值 = new List<PLC_Device>();
        //        private List<PLC_Device> List_PLC_Device_CCD01_02_PIN量測參數_OrgX = new List<PLC_Device>();
        //        private List<PLC_Device> List_PLC_Device_CCD01_02_PIN量測參數_OrgY = new List<PLC_Device>();
        //        private List<PLC_Device> List_PLC_Device_CCD01_02_PIN量測參數_Width = new List<PLC_Device>();
        //        private List<PLC_Device> List_PLC_Device_CCD01_02_PIN量測參數_Height = new List<PLC_Device>();
        //        private List<PLC_Device> List_PLC_Device_CCD01_02_PIN量測參數_面積上限 = new List<PLC_Device>();
        //        private List<PLC_Device> List_PLC_Device_CCD01_02_PIN量測參數_面積下限 = new List<PLC_Device>();
        //        #region 灰階門檻值
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN01 = new PLC_Device("F600");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN02 = new PLC_Device("F601");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN03 = new PLC_Device("F602");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN04 = new PLC_Device("F603");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN05 = new PLC_Device("F604");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN06 = new PLC_Device("F605");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN07 = new PLC_Device("F606");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN08 = new PLC_Device("F607");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN09 = new PLC_Device("F608");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN10 = new PLC_Device("F609");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN11 = new PLC_Device("F610");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN12 = new PLC_Device("F611");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN13 = new PLC_Device("F612");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN14 = new PLC_Device("F613");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN15 = new PLC_Device("F614");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN16 = new PLC_Device("F615");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN17 = new PLC_Device("F616");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN18 = new PLC_Device("F617");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN19 = new PLC_Device("F618");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN20 = new PLC_Device("F619");
        //        #endregion
        //        #region OrgX
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN01 = new PLC_Device("F700");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN02 = new PLC_Device("F701");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN03 = new PLC_Device("F702");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN04 = new PLC_Device("F703");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN05 = new PLC_Device("F704");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN06 = new PLC_Device("F705");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN07 = new PLC_Device("F706");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN08 = new PLC_Device("F707");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN09 = new PLC_Device("F708");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN10 = new PLC_Device("F709");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN11 = new PLC_Device("F710");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN12 = new PLC_Device("F711");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN13 = new PLC_Device("F712");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN14 = new PLC_Device("F713");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN15 = new PLC_Device("F714");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN16 = new PLC_Device("F715");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN17 = new PLC_Device("F716");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN18 = new PLC_Device("F717");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN19 = new PLC_Device("F718");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN20 = new PLC_Device("F719");
        //        #endregion
        //        #region OrgY
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN01 = new PLC_Device("F800");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN02 = new PLC_Device("F801");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN03 = new PLC_Device("F802");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN04 = new PLC_Device("F803");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN05 = new PLC_Device("F804");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN06 = new PLC_Device("F805");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN07 = new PLC_Device("F806");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN08 = new PLC_Device("F807");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN09 = new PLC_Device("F808");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN10 = new PLC_Device("F809");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN11 = new PLC_Device("F810");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN12 = new PLC_Device("F811");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN13 = new PLC_Device("F812");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN14 = new PLC_Device("F813");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN15 = new PLC_Device("F814");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN16 = new PLC_Device("F815");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN17 = new PLC_Device("F816");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN18 = new PLC_Device("F817");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN19 = new PLC_Device("F818");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN20 = new PLC_Device("F819");
        //        #endregion
        //        #region Width
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN01 = new PLC_Device("F900");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN02 = new PLC_Device("F901");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN03 = new PLC_Device("F902");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN04 = new PLC_Device("F903");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN05 = new PLC_Device("F904");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN06 = new PLC_Device("F905");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN07 = new PLC_Device("F906");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN08 = new PLC_Device("F907");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN09 = new PLC_Device("F908");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN10 = new PLC_Device("F909");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN11 = new PLC_Device("F910");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN12 = new PLC_Device("F911");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN13 = new PLC_Device("F912");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN14 = new PLC_Device("F913");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN15 = new PLC_Device("F914");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN16 = new PLC_Device("F915");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN17 = new PLC_Device("F916");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN18 = new PLC_Device("F917");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN19 = new PLC_Device("F918");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Width_PIN20 = new PLC_Device("F919");
        //        #endregion
        //        #region Height
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN01 = new PLC_Device("F1000");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN02 = new PLC_Device("F1001");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN03 = new PLC_Device("F1002");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN04 = new PLC_Device("F1003");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN05 = new PLC_Device("F1004");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN06 = new PLC_Device("F1005");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN07 = new PLC_Device("F1006");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN08 = new PLC_Device("F1007");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN09 = new PLC_Device("F1008");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN10 = new PLC_Device("F1009");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN11 = new PLC_Device("F1010");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN12 = new PLC_Device("F1011");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN13 = new PLC_Device("F1012");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN14 = new PLC_Device("F1013");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN15 = new PLC_Device("F1014");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN16 = new PLC_Device("F1015");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN17 = new PLC_Device("F1016");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN18 = new PLC_Device("F1017");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN19 = new PLC_Device("F1018");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Height_PIN20 = new PLC_Device("F1019");
        //        #endregion
        //        #region 面積上限

        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN01 = new PLC_Device("F1100");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN02 = new PLC_Device("F1101");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN03 = new PLC_Device("F1102");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN04 = new PLC_Device("F1103");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN05 = new PLC_Device("F1104");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN06 = new PLC_Device("F1105");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN07 = new PLC_Device("F1106");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN08 = new PLC_Device("F1107");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN09 = new PLC_Device("F1108");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN10 = new PLC_Device("F1109");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN11 = new PLC_Device("F1110");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN12 = new PLC_Device("F1111");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN13 = new PLC_Device("F1112");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN14 = new PLC_Device("F1113");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN15 = new PLC_Device("F1114");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN16 = new PLC_Device("F1115");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN17 = new PLC_Device("F1116");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN18 = new PLC_Device("F1117");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN19 = new PLC_Device("F1118");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN20 = new PLC_Device("F1119");
        //        #endregion
        //        #region 面積下限

        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN01 = new PLC_Device("F1200");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN02 = new PLC_Device("F1201");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN03 = new PLC_Device("F1202");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN04 = new PLC_Device("F1203");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN05 = new PLC_Device("F1204");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN06 = new PLC_Device("F1205");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN07 = new PLC_Device("F1206");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN08 = new PLC_Device("F1207");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN09 = new PLC_Device("F1208");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN10 = new PLC_Device("F1209");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN11 = new PLC_Device("F1210");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN12 = new PLC_Device("F1211");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN13 = new PLC_Device("F1212");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN14 = new PLC_Device("F1213");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN15 = new PLC_Device("F1214");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN16 = new PLC_Device("F1215");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN17 = new PLC_Device("F1216");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN18 = new PLC_Device("F1217");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN19 = new PLC_Device("F1218");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN20 = new PLC_Device("F1219");
        //        #endregion
        //        #endregion
        //        #region GAP量測框調整
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_量測框架ProbeCenterX = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_量測框架ProbeCenterY = new List<PLC_Device>();

        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line1變化銳利度 = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line1變化強度門檻值 = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line1灰階面化面積 = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line1垂直量測寬度 = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line1雜訊抑制 = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line1框架測密度間隔 = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻 = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數 = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line1量測顏色變化 = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line1量測路徑一半長度 = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line1框架起始Tip1X = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line1框架終止Tip2X = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y = new List<PLC_Device>();


        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line2變化銳利度 = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line2變化強度門檻值 = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line2灰階面化面積 = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line2垂直量測寬度 = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line2雜訊抑制 = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line2框架測密度間隔 = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻 = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數 = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line2量測顏色變化 = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line2量測路徑一半長度 = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line2框架起始Tip1X = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line2框架終止Tip2X = new List<PLC_Device>();
        //        private List<PLC_Device> List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y = new List<PLC_Device>();
        //        #region GAP量測框中心
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN01 = new PLC_Device("F12401");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN02 = new PLC_Device("F12402");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN03 = new PLC_Device("F12403");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN04 = new PLC_Device("F12404");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN05 = new PLC_Device("F12405");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN06 = new PLC_Device("F12406");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN07 = new PLC_Device("F12407");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN08 = new PLC_Device("F12408");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN09 = new PLC_Device("F12409");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN10 = new PLC_Device("F12410");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN11 = new PLC_Device("F12411");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN12 = new PLC_Device("F12412");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN13 = new PLC_Device("F12413");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN14 = new PLC_Device("F12414");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN15 = new PLC_Device("F12415");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN16 = new PLC_Device("F12416");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN17 = new PLC_Device("F12417");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN18 = new PLC_Device("F12418");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN19 = new PLC_Device("F12419");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN20 = new PLC_Device("F12420");

        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN01 = new PLC_Device("F12421");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN02 = new PLC_Device("F12422");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN03 = new PLC_Device("F12423");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN04 = new PLC_Device("F12424");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN05 = new PLC_Device("F12425");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN06 = new PLC_Device("F12426");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN07 = new PLC_Device("F12427");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN08 = new PLC_Device("F12428");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN09 = new PLC_Device("F12429");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN10 = new PLC_Device("F12430");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN11 = new PLC_Device("F12431");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN12 = new PLC_Device("F12432");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN13 = new PLC_Device("F12433");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN14 = new PLC_Device("F12434");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN15 = new PLC_Device("F12435");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN16 = new PLC_Device("F12436");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN17 = new PLC_Device("F12437");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN18 = new PLC_Device("F12438");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN19 = new PLC_Device("F12439");
        //        private PLC_Device CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN20 = new PLC_Device("F12440");
        //        #endregion
        //        #region Line1變化銳利度
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN01 = new PLC_Device("F12001");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN02 = new PLC_Device("F12002");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN03 = new PLC_Device("F12003");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN04 = new PLC_Device("F12004");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN05 = new PLC_Device("F12005");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN06 = new PLC_Device("F12006");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN07 = new PLC_Device("F12007");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN08 = new PLC_Device("F12008");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN09 = new PLC_Device("F12009");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN10 = new PLC_Device("F12010");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN11 = new PLC_Device("F12011");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN12 = new PLC_Device("F12012");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN13 = new PLC_Device("F12013");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN14 = new PLC_Device("F12014");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN15 = new PLC_Device("F12015");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN16 = new PLC_Device("F12016");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN17 = new PLC_Device("F12017");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN18 = new PLC_Device("F12018");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN19 = new PLC_Device("F12019");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN20 = new PLC_Device("F12020");
        //        #endregion
        //        #region Line1變化強度門檻值
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN01 = new PLC_Device("F12021");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN02 = new PLC_Device("F12022");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN03 = new PLC_Device("F12023");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN04 = new PLC_Device("F12024");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN05 = new PLC_Device("F12025");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN06 = new PLC_Device("F12026");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN07 = new PLC_Device("F12027");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN08 = new PLC_Device("F12028");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN09 = new PLC_Device("F12029");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN10 = new PLC_Device("F12030");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN11 = new PLC_Device("F12031");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN12 = new PLC_Device("F12032");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN13 = new PLC_Device("F12033");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN14 = new PLC_Device("F12034");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN15 = new PLC_Device("F12035");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN16 = new PLC_Device("F12036");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN17 = new PLC_Device("F12037");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN18 = new PLC_Device("F12038");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN19 = new PLC_Device("F12039");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN20 = new PLC_Device("F12040");
        //        #endregion
        //        #region Line1灰階面化面積
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN01 = new PLC_Device("F12041");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN02 = new PLC_Device("F12042");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN03 = new PLC_Device("F12043");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN04 = new PLC_Device("F12044");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN05 = new PLC_Device("F12045");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN06 = new PLC_Device("F12046");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN07 = new PLC_Device("F12047");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN08 = new PLC_Device("F12048");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN09 = new PLC_Device("F12049");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN10 = new PLC_Device("F12050");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN11 = new PLC_Device("F12051");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN12 = new PLC_Device("F12052");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN13 = new PLC_Device("F12053");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN14 = new PLC_Device("F12054");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN15 = new PLC_Device("F12055");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN16 = new PLC_Device("F12056");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN17 = new PLC_Device("F12057");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN18 = new PLC_Device("F12058");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN19 = new PLC_Device("F12059");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN20 = new PLC_Device("F12060");
        //        #endregion
        //        #region Line1垂直量測寬度
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN01 = new PLC_Device("F12061");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN02 = new PLC_Device("F12062");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN03 = new PLC_Device("F12063");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN04 = new PLC_Device("F12064");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN05 = new PLC_Device("F12065");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN06 = new PLC_Device("F12066");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN07 = new PLC_Device("F12067");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN08 = new PLC_Device("F12068");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN09 = new PLC_Device("F12069");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN10 = new PLC_Device("F12070");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN11 = new PLC_Device("F12071");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN12 = new PLC_Device("F12072");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN13 = new PLC_Device("F12073");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN14 = new PLC_Device("F12074");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN15 = new PLC_Device("F12075");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN16 = new PLC_Device("F12076");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN17 = new PLC_Device("F12077");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN18 = new PLC_Device("F12078");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN19 = new PLC_Device("F12079");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN20 = new PLC_Device("F12080");
        //        #endregion
        //        #region Line1雜訊抑制
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN01 = new PLC_Device("F12471");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN02 = new PLC_Device("F12472");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN03 = new PLC_Device("F12473");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN04 = new PLC_Device("F12474");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN05 = new PLC_Device("F12475");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN06 = new PLC_Device("F12476");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN07 = new PLC_Device("F12477");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN08 = new PLC_Device("F12478");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN09 = new PLC_Device("F12479");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN10 = new PLC_Device("F12480");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN11 = new PLC_Device("F12481");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN12 = new PLC_Device("F12482");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN13 = new PLC_Device("F12483");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN14 = new PLC_Device("F12484");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN15 = new PLC_Device("F12485");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN16 = new PLC_Device("F12486");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN17 = new PLC_Device("F12487");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN18 = new PLC_Device("F12488");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN19 = new PLC_Device("F12489");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN20 = new PLC_Device("F12490");
        //        #endregion
        //        #region Line1框架測密度間隔
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN01 = new PLC_Device("F12091");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN02 = new PLC_Device("F12092");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN03 = new PLC_Device("F12093");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN04 = new PLC_Device("F12094");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN05 = new PLC_Device("F12095");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN06 = new PLC_Device("F12096");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN07 = new PLC_Device("F12097");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN08 = new PLC_Device("F12098");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN09 = new PLC_Device("F12099");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN10 = new PLC_Device("F12100");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN11 = new PLC_Device("F12101");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN12 = new PLC_Device("F12102");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN13 = new PLC_Device("F12103");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN14 = new PLC_Device("F12104");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN15 = new PLC_Device("F12105");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN16 = new PLC_Device("F12106");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN17 = new PLC_Device("F12107");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN18 = new PLC_Device("F12108");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN19 = new PLC_Device("F12109");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN20 = new PLC_Device("F12110");
        //        #endregion
        //        #region Line1最佳回歸線過濾門檻
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN01 = new PLC_Device("F12111");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN02 = new PLC_Device("F12112");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN03 = new PLC_Device("F12113");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN04 = new PLC_Device("F12114");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN05 = new PLC_Device("F12115");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN06 = new PLC_Device("F12116");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN07 = new PLC_Device("F12117");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN08 = new PLC_Device("F12118");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN09 = new PLC_Device("F12119");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN10 = new PLC_Device("F12120");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN11 = new PLC_Device("F12121");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN12 = new PLC_Device("F12122");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN13 = new PLC_Device("F12123");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN14 = new PLC_Device("F12124");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN15 = new PLC_Device("F12125");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN16 = new PLC_Device("F12126");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN17 = new PLC_Device("F12127");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN18 = new PLC_Device("F12128");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN19 = new PLC_Device("F12129");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN20 = new PLC_Device("F12130");
        //        #endregion
        //        #region Line1最佳回歸線計算次數
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN01 = new PLC_Device("F12131");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN02 = new PLC_Device("F12132");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN03 = new PLC_Device("F12133");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN04 = new PLC_Device("F12134");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN05 = new PLC_Device("F12135");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN06 = new PLC_Device("F12136");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN07 = new PLC_Device("F12137");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN08 = new PLC_Device("F12138");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN09 = new PLC_Device("F12139");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN10 = new PLC_Device("F12140");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN11 = new PLC_Device("F12141");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN12 = new PLC_Device("F12142");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN13 = new PLC_Device("F12143");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN14 = new PLC_Device("F12144");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN15 = new PLC_Device("F12145");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN16 = new PLC_Device("F12146");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN17 = new PLC_Device("F12147");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN18 = new PLC_Device("F12148");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN19 = new PLC_Device("F12149");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN20 = new PLC_Device("F12150");
        //        #endregion
        //        #region Line1量測顏色變化
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN01 = new PLC_Device("F12151");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN02 = new PLC_Device("F12152");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN03 = new PLC_Device("F12153");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN04 = new PLC_Device("F12154");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN05 = new PLC_Device("F12155");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN06 = new PLC_Device("F12156");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN07 = new PLC_Device("F12157");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN08 = new PLC_Device("F12158");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN09 = new PLC_Device("F12159");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN10 = new PLC_Device("F12160");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN11 = new PLC_Device("F12161");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN12 = new PLC_Device("F12162");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN13 = new PLC_Device("F12163");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN14 = new PLC_Device("F12164");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN15 = new PLC_Device("F12165");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN16 = new PLC_Device("F12166");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN17 = new PLC_Device("F12167");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN18 = new PLC_Device("F12168");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN19 = new PLC_Device("F12169");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN20 = new PLC_Device("F12170");
        //        #endregion
        //        #region Line1量測路徑一半長度
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN01 = new PLC_Device("F12451");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN02 = new PLC_Device("F12452");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN03 = new PLC_Device("F12453");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN04 = new PLC_Device("F12454");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN05 = new PLC_Device("F12455");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN06 = new PLC_Device("F12456");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN07 = new PLC_Device("F12457");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN08 = new PLC_Device("F12458");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN09 = new PLC_Device("F12459");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN10 = new PLC_Device("F12460");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN11 = new PLC_Device("F12461");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN12 = new PLC_Device("F12462");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN13 = new PLC_Device("F12463");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN14 = new PLC_Device("F12464");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN15 = new PLC_Device("F12465");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN16 = new PLC_Device("F12466");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN17 = new PLC_Device("F12467");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN18 = new PLC_Device("F12468");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN19 = new PLC_Device("F12469");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN20 = new PLC_Device("F12470");
        //        #endregion
        //        #region Line1框架起始Tip1X
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN01 = new PLC_Device("F12201");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN02 = new PLC_Device("F12202");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN03 = new PLC_Device("F12203");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN04 = new PLC_Device("F12204");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN05 = new PLC_Device("F12205");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN06 = new PLC_Device("F12206");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN07 = new PLC_Device("F12207");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN08 = new PLC_Device("F12208");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN09 = new PLC_Device("F12209");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN10 = new PLC_Device("F12210");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN11 = new PLC_Device("F12211");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN12 = new PLC_Device("F12212");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN13 = new PLC_Device("F12213");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN14 = new PLC_Device("F12214");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN15 = new PLC_Device("F12215");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN16 = new PLC_Device("F12216");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN17 = new PLC_Device("F12217");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN18 = new PLC_Device("F12218");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN19 = new PLC_Device("F12219");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN20 = new PLC_Device("F12220");
        //        #endregion
        //        #region Line1框架起始Tip1Y
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN01 = new PLC_Device("F12221");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN02 = new PLC_Device("F12222");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN03 = new PLC_Device("F12223");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN04 = new PLC_Device("F12224");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN05 = new PLC_Device("F12225");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN06 = new PLC_Device("F12226");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN07 = new PLC_Device("F12227");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN08 = new PLC_Device("F12228");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN09 = new PLC_Device("F12229");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN10 = new PLC_Device("F12230");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN11 = new PLC_Device("F12231");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN12 = new PLC_Device("F12232");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN13 = new PLC_Device("F12233");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN14 = new PLC_Device("F12234");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN15 = new PLC_Device("F12235");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN16 = new PLC_Device("F12236");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN17 = new PLC_Device("F12237");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN18 = new PLC_Device("F12238");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN19 = new PLC_Device("F12239");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN20 = new PLC_Device("F12240");
        //        #endregion
        //        #region Line1框架終止Tip2X
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN01 = new PLC_Device("F12241");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN02 = new PLC_Device("F12242");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN03 = new PLC_Device("F12243");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN04 = new PLC_Device("F12244");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN05 = new PLC_Device("F12245");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN06 = new PLC_Device("F12246");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN07 = new PLC_Device("F12247");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN08 = new PLC_Device("F12248");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN09 = new PLC_Device("F12249");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN10 = new PLC_Device("F12250");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN11 = new PLC_Device("F12251");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN12 = new PLC_Device("F12252");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN13 = new PLC_Device("F12253");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN14 = new PLC_Device("F12254");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN15 = new PLC_Device("F12255");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN16 = new PLC_Device("F12256");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN17 = new PLC_Device("F12257");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN18 = new PLC_Device("F12258");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN19 = new PLC_Device("F12259");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN20 = new PLC_Device("F12260");
        //        #endregion
        //        #region Line1框架終止Tip2Y
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN01 = new PLC_Device("F12261");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN02 = new PLC_Device("F12262");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN03 = new PLC_Device("F12263");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN04 = new PLC_Device("F12264");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN05 = new PLC_Device("F12265");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN06 = new PLC_Device("F12266");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN07 = new PLC_Device("F12267");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN08 = new PLC_Device("F12268");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN09 = new PLC_Device("F12269");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN10 = new PLC_Device("F12270");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN11 = new PLC_Device("F12271");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN12 = new PLC_Device("F12272");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN13 = new PLC_Device("F12273");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN14 = new PLC_Device("F12274");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN15 = new PLC_Device("F12275");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN16 = new PLC_Device("F12276");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN17 = new PLC_Device("F12277");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN18 = new PLC_Device("F12278");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN19 = new PLC_Device("F12279");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN20 = new PLC_Device("F12280");
        //        #endregion


        //        #region Line2變化銳利度
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN01 = new PLC_Device("F12601");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN02 = new PLC_Device("F12602");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN03 = new PLC_Device("F12603");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN04 = new PLC_Device("F12604");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN05 = new PLC_Device("F12605");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN06 = new PLC_Device("F12606");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN07 = new PLC_Device("F12607");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN08 = new PLC_Device("F12608");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN09 = new PLC_Device("F12609");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN10 = new PLC_Device("F12610");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN11 = new PLC_Device("F12611");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN12 = new PLC_Device("F12612");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN13 = new PLC_Device("F12613");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN14 = new PLC_Device("F12614");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN15 = new PLC_Device("F12615");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN16 = new PLC_Device("F12616");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN17 = new PLC_Device("F12617");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN18 = new PLC_Device("F12618");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN19 = new PLC_Device("F12619");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN20 = new PLC_Device("F12620");
        //        #endregion
        //        #region Line2變化強度門檻值
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN01 = new PLC_Device("F12621");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN02 = new PLC_Device("F12622");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN03 = new PLC_Device("F12623");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN04 = new PLC_Device("F12624");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN05 = new PLC_Device("F12625");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN06 = new PLC_Device("F12626");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN07 = new PLC_Device("F12627");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN08 = new PLC_Device("F12628");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN09 = new PLC_Device("F12629");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN10 = new PLC_Device("F12630");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN11 = new PLC_Device("F12631");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN12 = new PLC_Device("F12632");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN13 = new PLC_Device("F12633");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN14 = new PLC_Device("F12634");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN15 = new PLC_Device("F12635");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN16 = new PLC_Device("F12636");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN17 = new PLC_Device("F12637");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN18 = new PLC_Device("F12638");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN19 = new PLC_Device("F12639");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN20 = new PLC_Device("F12640");
        //        #endregion
        //        #region Line2灰階面化面積
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN01 = new PLC_Device("F12641");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN02 = new PLC_Device("F12642");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN03 = new PLC_Device("F12643");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN04 = new PLC_Device("F12644");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN05 = new PLC_Device("F12645");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN06 = new PLC_Device("F12646");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN07 = new PLC_Device("F12647");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN08 = new PLC_Device("F12648");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN09 = new PLC_Device("F12649");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN10 = new PLC_Device("F12650");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN11 = new PLC_Device("F12651");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN12 = new PLC_Device("F12652");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN13 = new PLC_Device("F12653");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN14 = new PLC_Device("F12654");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN15 = new PLC_Device("F12655");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN16 = new PLC_Device("F12656");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN17 = new PLC_Device("F12657");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN18 = new PLC_Device("F12658");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN19 = new PLC_Device("F12659");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN20 = new PLC_Device("F12660");
        //        #endregion
        //        #region Line2垂直量測寬度
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN01 = new PLC_Device("F12661");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN02 = new PLC_Device("F12662");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN03 = new PLC_Device("F12663");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN04 = new PLC_Device("F12664");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN05 = new PLC_Device("F12665");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN06 = new PLC_Device("F12666");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN07 = new PLC_Device("F12667");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN08 = new PLC_Device("F12668");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN09 = new PLC_Device("F12669");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN10 = new PLC_Device("F12670");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN11 = new PLC_Device("F12671");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN12 = new PLC_Device("F12672");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN13 = new PLC_Device("F12673");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN14 = new PLC_Device("F12674");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN15 = new PLC_Device("F12675");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN16 = new PLC_Device("F12676");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN17 = new PLC_Device("F12677");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN18 = new PLC_Device("F12678");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN19 = new PLC_Device("F12679");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN20 = new PLC_Device("F12680");
        //        #endregion
        //        #region Line2雜訊抑制
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN01 = new PLC_Device("F12491");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN02 = new PLC_Device("F12492");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN03 = new PLC_Device("F12493");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN04 = new PLC_Device("F12494");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN05 = new PLC_Device("F12495");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN06 = new PLC_Device("F12496");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN07 = new PLC_Device("F12497");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN08 = new PLC_Device("F12498");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN09 = new PLC_Device("F12499");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN10 = new PLC_Device("F12500");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN11 = new PLC_Device("F12501");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN12 = new PLC_Device("F12502");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN13 = new PLC_Device("F12503");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN14 = new PLC_Device("F12504");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN15 = new PLC_Device("F12505");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN16 = new PLC_Device("F12506");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN17 = new PLC_Device("F12507");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN18 = new PLC_Device("F12508");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN19 = new PLC_Device("F12509");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN20 = new PLC_Device("F12510");
        //        #endregion
        //        #region Line2框架測密度間隔
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN01 = new PLC_Device("F12691");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN02 = new PLC_Device("F12692");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN03 = new PLC_Device("F12693");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN04 = new PLC_Device("F12694");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN05 = new PLC_Device("F12695");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN06 = new PLC_Device("F12696");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN07 = new PLC_Device("F12697");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN08 = new PLC_Device("F12698");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN09 = new PLC_Device("F12699");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN10 = new PLC_Device("F12700");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN11 = new PLC_Device("F12701");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN12 = new PLC_Device("F12702");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN13 = new PLC_Device("F12703");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN14 = new PLC_Device("F12704");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN15 = new PLC_Device("F12705");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN16 = new PLC_Device("F12706");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN17 = new PLC_Device("F12707");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN18 = new PLC_Device("F12708");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN19 = new PLC_Device("F12709");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN20 = new PLC_Device("F12710");
        //        #endregion
        //        #region Line2最佳回歸線過濾門檻
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN01 = new PLC_Device("F12711");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN02 = new PLC_Device("F12712");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN03 = new PLC_Device("F12713");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN04 = new PLC_Device("F12714");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN05 = new PLC_Device("F12715");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN06 = new PLC_Device("F12716");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN07 = new PLC_Device("F12717");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN08 = new PLC_Device("F12718");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN09 = new PLC_Device("F12719");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN10 = new PLC_Device("F12720");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN11 = new PLC_Device("F12721");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN12 = new PLC_Device("F12722");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN13 = new PLC_Device("F12723");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN14 = new PLC_Device("F12724");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN15 = new PLC_Device("F12725");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN16 = new PLC_Device("F12726");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN17 = new PLC_Device("F12727");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN18 = new PLC_Device("F12728");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN19 = new PLC_Device("F12729");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN20 = new PLC_Device("F12730");
        //        #endregion
        //        #region Line2最佳回歸線計算次數
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN01 = new PLC_Device("F12731");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN02 = new PLC_Device("F12732");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN03 = new PLC_Device("F12733");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN04 = new PLC_Device("F12734");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN05 = new PLC_Device("F12735");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN06 = new PLC_Device("F12736");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN07 = new PLC_Device("F12737");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN08 = new PLC_Device("F12738");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN09 = new PLC_Device("F12739");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN10 = new PLC_Device("F12740");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN11 = new PLC_Device("F12741");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN12 = new PLC_Device("F12742");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN13 = new PLC_Device("F12743");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN14 = new PLC_Device("F12744");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN15 = new PLC_Device("F12745");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN16 = new PLC_Device("F12746");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN17 = new PLC_Device("F12747");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN18 = new PLC_Device("F12748");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN19 = new PLC_Device("F12749");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN20 = new PLC_Device("F12750");
        //        #endregion
        //        #region Line2量測顏色變化
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN01 = new PLC_Device("F12751");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN02 = new PLC_Device("F12752");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN03 = new PLC_Device("F12753");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN04 = new PLC_Device("F12754");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN05 = new PLC_Device("F12755");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN06 = new PLC_Device("F12756");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN07 = new PLC_Device("F12757");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN08 = new PLC_Device("F12758");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN09 = new PLC_Device("F12759");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN10 = new PLC_Device("F12760");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN11 = new PLC_Device("F12761");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN12 = new PLC_Device("F12762");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN13 = new PLC_Device("F12763");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN14 = new PLC_Device("F12764");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN15 = new PLC_Device("F12765");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN16 = new PLC_Device("F12766");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN17 = new PLC_Device("F12767");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN18 = new PLC_Device("F12768");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN19 = new PLC_Device("F12769");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN20 = new PLC_Device("F12770");
        //        #endregion
        //        #region Line2量測路徑一半長度
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN01 = new PLC_Device("F12771");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN02 = new PLC_Device("F12772");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN03 = new PLC_Device("F12773");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN04 = new PLC_Device("F12774");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN05 = new PLC_Device("F12775");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN06 = new PLC_Device("F12776");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN07 = new PLC_Device("F12777");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN08 = new PLC_Device("F12778");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN09 = new PLC_Device("F12779");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN10 = new PLC_Device("F12780");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN11 = new PLC_Device("F12781");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN12 = new PLC_Device("F12782");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN13 = new PLC_Device("F12783");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN14 = new PLC_Device("F12784");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN15 = new PLC_Device("F12785");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN16 = new PLC_Device("F12786");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN17 = new PLC_Device("F12787");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN18 = new PLC_Device("F12788");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN19 = new PLC_Device("F12789");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN20 = new PLC_Device("F12790");
        //        #endregion
        //        #region Line2框架起始Tip1X
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN01 = new PLC_Device("F12301");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN02 = new PLC_Device("F12302");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN03 = new PLC_Device("F12303");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN04 = new PLC_Device("F12304");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN05 = new PLC_Device("F12305");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN06 = new PLC_Device("F12306");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN07 = new PLC_Device("F12307");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN08 = new PLC_Device("F12308");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN09 = new PLC_Device("F12309");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN10 = new PLC_Device("F12310");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN11 = new PLC_Device("F12311");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN12 = new PLC_Device("F12312");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN13 = new PLC_Device("F12313");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN14 = new PLC_Device("F12314");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN15 = new PLC_Device("F12315");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN16 = new PLC_Device("F12316");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN17 = new PLC_Device("F12317");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN18 = new PLC_Device("F12318");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN19 = new PLC_Device("F12319");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN20 = new PLC_Device("F12320");
        //        #endregion
        //        #region Line2框架起始Tip1Y
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN01 = new PLC_Device("F12321");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN02 = new PLC_Device("F12322");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN03 = new PLC_Device("F12323");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN04 = new PLC_Device("F12324");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN05 = new PLC_Device("F12325");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN06 = new PLC_Device("F12326");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN07 = new PLC_Device("F12327");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN08 = new PLC_Device("F12328");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN09 = new PLC_Device("F12329");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN10 = new PLC_Device("F12330");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN11 = new PLC_Device("F12331");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN12 = new PLC_Device("F12332");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN13 = new PLC_Device("F12333");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN14 = new PLC_Device("F12334");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN15 = new PLC_Device("F12335");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN16 = new PLC_Device("F12336");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN17 = new PLC_Device("F12337");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN18 = new PLC_Device("F12338");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN19 = new PLC_Device("F12339");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN20 = new PLC_Device("F12340");
        //        #endregion
        //        #region Line2框架終止Tip2X
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN01 = new PLC_Device("F12341");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN02 = new PLC_Device("F12342");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN03 = new PLC_Device("F12343");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN04 = new PLC_Device("F12344");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN05 = new PLC_Device("F12345");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN06 = new PLC_Device("F12346");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN07 = new PLC_Device("F12347");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN08 = new PLC_Device("F12348");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN09 = new PLC_Device("F12349");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN10 = new PLC_Device("F12350");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN11 = new PLC_Device("F12351");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN12 = new PLC_Device("F12352");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN13 = new PLC_Device("F12353");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN14 = new PLC_Device("F12354");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN15 = new PLC_Device("F12355");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN16 = new PLC_Device("F12356");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN17 = new PLC_Device("F12357");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN18 = new PLC_Device("F12358");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN19 = new PLC_Device("F12359");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN20 = new PLC_Device("F12360");
        //        #endregion
        //        #region Line2框架終止Tip2Y
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN01 = new PLC_Device("F12361");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN02 = new PLC_Device("F12362");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN03 = new PLC_Device("F12363");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN04 = new PLC_Device("F12364");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN05 = new PLC_Device("F12365");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN06 = new PLC_Device("F12366");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN07 = new PLC_Device("F12367");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN08 = new PLC_Device("F12368");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN09 = new PLC_Device("F12369");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN10 = new PLC_Device("F12370");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN11 = new PLC_Device("F12371");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN12 = new PLC_Device("F12372");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN13 = new PLC_Device("F12373");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN14 = new PLC_Device("F12374");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN15 = new PLC_Device("F12375");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN16 = new PLC_Device("F12376");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN17 = new PLC_Device("F12377");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN18 = new PLC_Device("F12378");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN19 = new PLC_Device("F12379");
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN20 = new PLC_Device("F12380");
        //        #endregion
        //        #endregion

        //        AxOvkBase.TxAxHitHandle[] CCD01_02_PIN量測_AxROIBW8_TxAxHitHandle = new AxOvkBase.TxAxHitHandle[20];
        //        bool[] flag_CCD01_02_PIN量測_AxROIBW8_MouseDown = new bool[20];
        //        AxOvkMsr.TxAxAngleMsrDragHandle[] CCD01_02_PIN量測_AxOvkMsr_TxAxAngleMsrDragHandle = new AxOvkMsr.TxAxAngleMsrDragHandle[20];
        //        bool[] flag_CCD01_02_PIN量測_AxOvkMsr_MouseDown = new bool[20];
        //        private void H_Canvas_Tech_CCD01_02_PIN量測_量測框調整_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        //        {

        //            if (PLC_Device_CCD01_02_Main_取像並檢驗.Bool || PLC_Device_CCD01_02_PLC觸發檢測.Bool)
        //            {
        //                try
        //                {
        //                    Graphics g = Graphics.FromHdc((IntPtr)HDC);
        //                    for (int i = 0; i < this.List_CCD01_02_PIN量測參數_量測點.Length; i++)
        //                    {
        //                        DrawingClass.Draw.十字中心(this.List_CCD01_02_PIN量測參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
        //                    }
        //                    g.Dispose();
        //                    g = null;
        //                }
        //                catch
        //                {

        //                }

        //            }
        //            else if (PLC_Device_CCD01_02_Tech_檢驗一次.Bool || PLC_Device_CCD01_02_Tech_取像並檢驗.Bool)
        //            {
        //                if (this.PLC_Device_CCD01_02_PIN量測_量測框調整_RefreshCanvas.Bool)
        //                {
        //                    try
        //                    {
        //                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
        //                        for (int i = 0; i < this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
        //                        {
        //                            if (i < 10)
        //                            {
        //                                this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].Title = string.Format("上排" + "{0}", (i + 1).ToString("00"));
        //                            }
        //                            if (i >= 10)
        //                            {
        //                                this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].Title = string.Format("下排" + "{0}", ((i - 10) + 1).ToString("00"));
        //                            }
        //                            this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].ShowTitle = true;
        //                            this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].ShowPlacement = false;
        //                            this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].DrawRect(HDC, ZoomX, ZoomY, 0, 0, 0x0000FF);
        //                        }
        //                        for (int i = 0; i < this.List_CCD01_02_PIN量測參數_量測點.Length; i++)
        //                        {
        //                            DrawingClass.Draw.十字中心(this.List_CCD01_02_PIN量測參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
        //                        }
        //                        g.Dispose();
        //                        g = null;
        //                    }
        //                    catch
        //                    {

        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (this.PLC_Device_CCD01_02_PIN量測_量測框調整_RefreshCanvas.Bool)
        //                {
        //                    try
        //                    {
        //                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
        //                        PointF po_str_PIN到基準Y = new PointF(200, 250);
        //                        Font font = new Font("微軟正黑體", 10);
        //                        for (int i = 0; i < List_CCD01_02_PIN量測_AxAngleMsr_量測框調整.Count; i++)
        //                        {
        //                            if (i < 10)
        //                            {
        //                                this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Title = string.Format("上排" + "{0}", (i + 1).ToString("00"));
        //                            }
        //                            if (i >= 10)
        //                            {
        //                                this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Title = string.Format("下排" + "{0}", ((i - 10) + 1).ToString("00"));
        //                            }

        //                        }
        //                        if(this.plC_CheckBox_CCD01_02_PIN量測GAP_繪製量測框.Checked)
        //                        {
        //                            for(int i = 0; i < List_CCD01_02_PIN量測_AxAngleMsr_量測框調整.Count;i++)
        //                            {
        //                                this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].DrawFrame(HDC, ZoomX, ZoomY, 0, 0);
        //                                this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].DrawFittedCenter(HDC, ZoomX, ZoomY, 0, 0, 0x0000FF);
        //                            }
        //                        }
        //                        if (this.plC_CheckBox_CCD01_02_PIN量測GAP_繪製量測線段.Checked)
        //                        {
        //                            for (int i = 0; i < List_CCD01_02_PIN量測_AxAngleMsr_量測框調整.Count; i++)
        //                            {
        //                                this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].DrawFittedPrimitives(HDC, ZoomX, ZoomY, 0, 0);
        //                                //this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].DrawPivotsAndGaps(HDC, ZoomX, ZoomY, 0, 0);
        //                            }

        //                        }
        //                        if (this.plC_CheckBox_CCD01_02_PIN量測GAP_繪製量測點.Checked)
        //                        {
        //                            for (int i = 0; i < List_CCD01_02_PIN量測_AxAngleMsr_量測框調整.Count; i++)
        //                            {
        //                                this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].DrawPoints(HDC, ZoomX, ZoomY, 0, 0);
        //                            }

        //                        }

        //                        if (this.plC_CheckBox_CCD01_02_PIN量測_繪製量測框.Checked)
        //                        {
        //                            for (int i = 0; i < this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
        //                            {
        //                                if(i < 10)
        //                                {
        //                                    this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].Title = string.Format("上排" + "{0}", (i + 1).ToString("00"));
        //                                }
        //                                if(i >= 10)
        //                                {
        //                                    this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].Title = string.Format("下排" + "{0}", ((i - 10) + 1).ToString("00"));
        //                                }
        //                                this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].ShowTitle = true;
        //                                this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].ShowPlacement = false;                               
        //                                this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].DrawFrame(HDC, ZoomX, ZoomY, 0, 0, 0x0000FF);
        //                            }
        //                        }
        //                        if (this.plC_CheckBox_CCD01_02_PIN量測_繪製量測區塊.Checked)
        //                        {
        //                            for (int i = 0; i < this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
        //                            {
        //                                this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].DrawBlobs(HDC, -1, ZoomX, ZoomY, 0, 0, true, -1);
        //                            }

        //                        }
        //                        for (int i = 0; i < this.List_CCD01_02_PIN量測參數_量測點.Length; i++)
        //                        {
        //                            DrawingClass.Draw.十字中心(this.List_CCD01_02_PIN量測參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
        //                        }
        //                        g.Dispose();
        //                        g = null;
        //                    }
        //                    catch
        //                    {

        //                    }


        //                }
        //            }

        //            this.PLC_Device_CCD01_02_PIN量測_量測框調整_RefreshCanvas.Bool = false;
        //        }
        //        private void H_Canvas_Tech_CCD01_02_PIN量測_量測框調整_OnCanvasMouseDownEvent(int x, int y, float ZoomX, float ZoomY, ref int InUsedEventNum, int InUsedCanvasHandle)
        //        {
        //            if (this.PLC_Device_CCD01_02_PIN量測_量測框調整.Bool)
        //            {
        //                #region BLOB量測框調整
        //                for (int i = 0; i < this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
        //                {
        //                    this.CCD01_02_PIN量測_AxROIBW8_TxAxHitHandle[i] = this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].HitTest(x, y, ZoomX, ZoomY, 0, 0);
        //                    if (this.CCD01_02_PIN量測_AxROIBW8_TxAxHitHandle[i] != AxOvkBase.TxAxHitHandle.AX_HANDLE_NONE)
        //                    {
        //                        this.flag_CCD01_02_PIN量測_AxROIBW8_MouseDown[i] = true;
        //                        InUsedEventNum = 10;
        //                        return;
        //                    }
        //                }
        //                #endregion
        //                #region GAP量測框調整
        //                for (int i = 0; i < this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整.Count; i++)
        //                {
        //                    this.CCD01_02_PIN量測_AxOvkMsr_TxAxAngleMsrDragHandle[i] = this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].HitTest(x, y, ZoomX, ZoomY, 0, 0);
        //                    if (this.CCD01_02_PIN量測_AxOvkMsr_TxAxAngleMsrDragHandle[i] != AxOvkMsr.TxAxAngleMsrDragHandle.AX_ANGLEMSR_NONE)
        //                    {
        //                        this.flag_CCD01_02_PIN量測_AxOvkMsr_MouseDown[i] = true;
        //                        InUsedEventNum = 10;
        //                        return;
        //                    }
        //                }
        //                #endregion
        //            }

        //        }
        //        private void H_Canvas_Tech_CCD01_02_PIN量測_量測框調整_OnCanvasMouseMoveEvent(int x, int y, float ZoomX, float ZoomY)
        //        {
        //            #region BLOB量測框調整
        //            for (int i = 0; i < this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
        //            {
        //                if (this.flag_CCD01_02_PIN量測_AxROIBW8_MouseDown[i])
        //                {
        //                    this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].DragROI(this.CCD01_02_PIN量測_AxROIBW8_TxAxHitHandle[i], x, y, ZoomX, ZoomY, 0, 0);
        //                    this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[i].Value = this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].OrgX;
        //                    this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[i].Value = this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].OrgY;
        //                    this.List_PLC_Device_CCD01_02_PIN量測參數_Width[i].Value = this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].ROIWidth;
        //                    this.List_PLC_Device_CCD01_02_PIN量測參數_Height[i].Value = this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].ROIHeight;
        //                }
        //            }
        //            #endregion
        //            #region GAP量測框調整
        //            for (int i = 0; i < this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整.Count; i++)
        //            {
        //                if (this.flag_CCD01_02_PIN量測_AxOvkMsr_MouseDown[i])
        //                {
        //                    this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].DragFrame(this.CCD01_02_PIN量測_AxOvkMsr_TxAxAngleMsrDragHandle[i], x, y, ZoomX, ZoomY, 0, 0);
        //                    this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterX[i].Value = List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].ProbeCenterX;
        //                    this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterY[i].Value = List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].ProbeCenterY;

        //                    this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1X[i].Value = List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line1Tip1X;
        //                    this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y[i].Value = List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line1Tip1Y;
        //                    this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2X[i].Value = List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line1Tip2X;
        //                    this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y[i].Value = List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line1Tip2Y;


        //                    this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1X[i].Value = List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line2Tip1X;
        //                    this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y[i].Value = List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line2Tip1Y;
        //                    this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2X[i].Value = List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line2Tip2X;
        //                    this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y[i].Value = List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line2Tip2Y;

        //                }
        //            }
        //            #endregion
        //        }
        //        private void H_Canvas_Tech_CCD01_02_PIN量測_量測框調整_OnCanvasMouseUpEvent(int x, int y, float ZoomX, float ZoomY)
        //        {
        //            #region BLOB量測框調整
        //            for (int i = 0; i < this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
        //            {
        //                this.flag_CCD01_02_PIN量測_AxROIBW8_MouseDown[i] = false;
        //            }
        //            #endregion
        //            #region GAP量測框調整
        //            for (int i = 0; i < this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整.Count; i++)
        //            {
        //                this.flag_CCD01_02_PIN量測_AxOvkMsr_MouseDown[i] = false;
        //            }

        //            #endregion

        //        }

        //        int cnt_Program_CCD01_02_PIN量測_量測框調整 = 65534;
        //        void sub_Program_CCD01_02_PIN量測_量測框調整()
        //        {
        //            if (cnt_Program_CCD01_02_PIN量測_量測框調整 == 65534)
        //            {
        //                this.h_Canvas_Tech_CCD01_02.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_02_PIN量測_量測框調整_OnCanvasDrawEvent;
        //                this.h_Canvas_Tech_CCD01_02.OnCanvasMouseDownEvent += H_Canvas_Tech_CCD01_02_PIN量測_量測框調整_OnCanvasMouseDownEvent;
        //                this.h_Canvas_Tech_CCD01_02.OnCanvasMouseMoveEvent += H_Canvas_Tech_CCD01_02_PIN量測_量測框調整_OnCanvasMouseMoveEvent;
        //                this.h_Canvas_Tech_CCD01_02.OnCanvasMouseUpEvent += H_Canvas_Tech_CCD01_02_PIN量測_量測框調整_OnCanvasMouseUpEvent;

        //                this.h_Canvas_Main_CCD01_02_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_02_PIN量測_量測框調整_OnCanvasDrawEvent;
        //                #region BLOB量測框調整
        //                #region 灰階門檻值
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN01);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN02);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN03);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN04);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN05);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN06);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN07);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN08);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN09);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN10);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN11);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN12);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN13);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN14);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN15);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN16);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN17);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN18);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN19);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_02_PIN量測參數_灰階門檻值_PIN20);
        //                #endregion
        //                #region OrgX
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN01);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN02);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN03);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN04);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN05);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN06);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN07);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN08);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN09);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN10);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN11);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN12);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN13);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN14);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN15);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN16);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN17);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN18);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN19);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgX_PIN20);
        //                #endregion
        //                #region OrgY
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN01);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN02);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN03);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN04);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN05);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN06);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN07);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN08);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN09);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN10);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN11);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN12);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN13);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN14);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN15);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN16);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN17);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN18);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN19);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_02_PIN量測參數_OrgY_PIN20);
        //                #endregion
        //                #region Width
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN01);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN02);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN03);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN04);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN05);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN06);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN07);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN08);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN09);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN10);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN11);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN12);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN13);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN14);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN15);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN16);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN17);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN18);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN19);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Width.Add(this.PLC_Device_CCD01_02_PIN量測參數_Width_PIN20);
        //                #endregion
        //                #region Height
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN01);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN02);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN03);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN04);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN05);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN06);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN07);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN08);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN09);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN10);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN11);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN12);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN13);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN14);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN15);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN16);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN17);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN18);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN19);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_Height.Add(this.PLC_Device_CCD01_02_PIN量測參數_Height_PIN20);
        //                #endregion
        //                #region 面積上限
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN01);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN02);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN03);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN04);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN05);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN06);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN07);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN08);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN09);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN10);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN11);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN12);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN13);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN14);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN15);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN16);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN17);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN18);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN19);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積上限_PIN20);
        //                #endregion
        //                #region 面積下限
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN01);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN02);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN03);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN04);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN05);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN06);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN07);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN08);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN09);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN10);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN11);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN12);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN13);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN14);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN15);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN16);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN17);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN18);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN19);
        //                this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_02_PIN量測參數_面積下限_PIN20);
        //                #endregion

        //                for (int i = 0; i < 20; i++)
        //                {
        //                    if (this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值[i].Value == 0) this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值[i].Value = 200;
        //                    if (this.List_PLC_Device_CCD01_02_PIN量測參數_Height[i].Value == 0) this.List_PLC_Device_CCD01_02_PIN量測參數_Height[i].Value = 100;
        //                    if (this.List_PLC_Device_CCD01_02_PIN量測參數_Width[i].Value == 0) this.List_PLC_Device_CCD01_02_PIN量測參數_Width[i].Value = 100;
        //                    if (this.List_PLC_Device_CCD01_02_PIN量測參數_Height[i].Value > 500) this.List_PLC_Device_CCD01_02_PIN量測參數_Height[i].Value = 500;
        //                    if (this.List_PLC_Device_CCD01_02_PIN量測參數_Width[i].Value > 500) this.List_PLC_Device_CCD01_02_PIN量測參數_Width[i].Value = 500;
        //                }
        //                #endregion
        //                #region GAP量測框調整
        //                #region 量測框架ProbeCenterX
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterX.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN01);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterX.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN02);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterX.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN03);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterX.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN04);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterX.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN05);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterX.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN06);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterX.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN07);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterX.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN08);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterX.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN09);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterX.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN10);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterX.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN11);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterX.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN12);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterX.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN13);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterX.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN14);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterX.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN15);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterX.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN16);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterX.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN17);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterX.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN18);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterX.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN19);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterX.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterX_PIN20);
        //                #endregion
        //                #region 量測框架ProbeCenterY
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterY.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN01);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterY.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN02);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterY.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN03);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterY.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN04);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterY.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN05);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterY.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN06);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterY.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN07);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterY.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN08);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterY.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN09);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterY.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN10);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterY.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN11);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterY.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN12);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterY.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN13);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterY.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN14);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterY.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN15);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterY.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN16);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterY.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN17);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterY.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN18);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterY.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN19);
        //                List_CCD01_02_PIN量測參數_量測框架ProbeCenterY.Add(CCD01_02_PIN量測參數_量測框架ProbeCenterY_PIN20);
        //                #endregion

        //                #region Line1變化銳利度
        //                List_CCD01_02_PIN量測參數_Line1變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN01);
        //                List_CCD01_02_PIN量測參數_Line1變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN02);
        //                List_CCD01_02_PIN量測參數_Line1變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN03);
        //                List_CCD01_02_PIN量測參數_Line1變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN04);
        //                List_CCD01_02_PIN量測參數_Line1變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN05);
        //                List_CCD01_02_PIN量測參數_Line1變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN06);
        //                List_CCD01_02_PIN量測參數_Line1變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN07);
        //                List_CCD01_02_PIN量測參數_Line1變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN08);
        //                List_CCD01_02_PIN量測參數_Line1變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN09);
        //                List_CCD01_02_PIN量測參數_Line1變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN10);
        //                List_CCD01_02_PIN量測參數_Line1變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN11);
        //                List_CCD01_02_PIN量測參數_Line1變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN12);
        //                List_CCD01_02_PIN量測參數_Line1變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN13);
        //                List_CCD01_02_PIN量測參數_Line1變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN14);
        //                List_CCD01_02_PIN量測參數_Line1變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN15);
        //                List_CCD01_02_PIN量測參數_Line1變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN16);
        //                List_CCD01_02_PIN量測參數_Line1變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN17);
        //                List_CCD01_02_PIN量測參數_Line1變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN18);
        //                List_CCD01_02_PIN量測參數_Line1變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN19);
        //                List_CCD01_02_PIN量測參數_Line1變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化銳利度_PIN20);
        //                #endregion
        //                #region Line1變化強度門檻值
        //                List_CCD01_02_PIN量測參數_Line1變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN01);
        //                List_CCD01_02_PIN量測參數_Line1變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN02);
        //                List_CCD01_02_PIN量測參數_Line1變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN03);
        //                List_CCD01_02_PIN量測參數_Line1變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN04);
        //                List_CCD01_02_PIN量測參數_Line1變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN05);
        //                List_CCD01_02_PIN量測參數_Line1變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN06);
        //                List_CCD01_02_PIN量測參數_Line1變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN07);
        //                List_CCD01_02_PIN量測參數_Line1變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN08);
        //                List_CCD01_02_PIN量測參數_Line1變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN09);
        //                List_CCD01_02_PIN量測參數_Line1變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN10);
        //                List_CCD01_02_PIN量測參數_Line1變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN11);
        //                List_CCD01_02_PIN量測參數_Line1變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN12);
        //                List_CCD01_02_PIN量測參數_Line1變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN13);
        //                List_CCD01_02_PIN量測參數_Line1變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN14);
        //                List_CCD01_02_PIN量測參數_Line1變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN15);
        //                List_CCD01_02_PIN量測參數_Line1變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN16);
        //                List_CCD01_02_PIN量測參數_Line1變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN17);
        //                List_CCD01_02_PIN量測參數_Line1變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN18);
        //                List_CCD01_02_PIN量測參數_Line1變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN19);
        //                List_CCD01_02_PIN量測參數_Line1變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line1變化強度門檻值_PIN20);
        //                #endregion
        //                #region Line1灰階面化面積
        //                List_CCD01_02_PIN量測參數_Line1灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN01);
        //                List_CCD01_02_PIN量測參數_Line1灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN02);
        //                List_CCD01_02_PIN量測參數_Line1灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN03);
        //                List_CCD01_02_PIN量測參數_Line1灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN04);
        //                List_CCD01_02_PIN量測參數_Line1灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN05);
        //                List_CCD01_02_PIN量測參數_Line1灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN06);
        //                List_CCD01_02_PIN量測參數_Line1灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN07);
        //                List_CCD01_02_PIN量測參數_Line1灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN08);
        //                List_CCD01_02_PIN量測參數_Line1灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN09);
        //                List_CCD01_02_PIN量測參數_Line1灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN10);
        //                List_CCD01_02_PIN量測參數_Line1灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN11);
        //                List_CCD01_02_PIN量測參數_Line1灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN12);
        //                List_CCD01_02_PIN量測參數_Line1灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN13);
        //                List_CCD01_02_PIN量測參數_Line1灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN14);
        //                List_CCD01_02_PIN量測參數_Line1灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN15);
        //                List_CCD01_02_PIN量測參數_Line1灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN16);
        //                List_CCD01_02_PIN量測參數_Line1灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN17);
        //                List_CCD01_02_PIN量測參數_Line1灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN18);
        //                List_CCD01_02_PIN量測參數_Line1灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN19);
        //                List_CCD01_02_PIN量測參數_Line1灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line1灰階面化面積_PIN20);
        //                #endregion
        //                #region Line1垂直量測寬度
        //                List_CCD01_02_PIN量測參數_Line1垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN01);
        //                List_CCD01_02_PIN量測參數_Line1垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN02);
        //                List_CCD01_02_PIN量測參數_Line1垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN03);
        //                List_CCD01_02_PIN量測參數_Line1垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN04);
        //                List_CCD01_02_PIN量測參數_Line1垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN05);
        //                List_CCD01_02_PIN量測參數_Line1垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN06);
        //                List_CCD01_02_PIN量測參數_Line1垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN07);
        //                List_CCD01_02_PIN量測參數_Line1垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN08);
        //                List_CCD01_02_PIN量測參數_Line1垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN09);
        //                List_CCD01_02_PIN量測參數_Line1垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN10);
        //                List_CCD01_02_PIN量測參數_Line1垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN11);
        //                List_CCD01_02_PIN量測參數_Line1垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN12);
        //                List_CCD01_02_PIN量測參數_Line1垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN13);
        //                List_CCD01_02_PIN量測參數_Line1垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN14);
        //                List_CCD01_02_PIN量測參數_Line1垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN15);
        //                List_CCD01_02_PIN量測參數_Line1垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN16);
        //                List_CCD01_02_PIN量測參數_Line1垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN17);
        //                List_CCD01_02_PIN量測參數_Line1垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN18);
        //                List_CCD01_02_PIN量測參數_Line1垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN19);
        //                List_CCD01_02_PIN量測參數_Line1垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1垂直量測寬度_PIN20);
        //                #endregion
        //                #region Line1雜訊抑制
        //                List_CCD01_02_PIN量測參數_Line1雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN01);
        //                List_CCD01_02_PIN量測參數_Line1雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN02);
        //                List_CCD01_02_PIN量測參數_Line1雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN03);
        //                List_CCD01_02_PIN量測參數_Line1雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN04);
        //                List_CCD01_02_PIN量測參數_Line1雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN05);
        //                List_CCD01_02_PIN量測參數_Line1雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN06);
        //                List_CCD01_02_PIN量測參數_Line1雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN07);
        //                List_CCD01_02_PIN量測參數_Line1雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN08);
        //                List_CCD01_02_PIN量測參數_Line1雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN09);
        //                List_CCD01_02_PIN量測參數_Line1雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN10);
        //                List_CCD01_02_PIN量測參數_Line1雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN11);
        //                List_CCD01_02_PIN量測參數_Line1雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN12);
        //                List_CCD01_02_PIN量測參數_Line1雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN13);
        //                List_CCD01_02_PIN量測參數_Line1雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN14);
        //                List_CCD01_02_PIN量測參數_Line1雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN15);
        //                List_CCD01_02_PIN量測參數_Line1雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN16);
        //                List_CCD01_02_PIN量測參數_Line1雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN17);
        //                List_CCD01_02_PIN量測參數_Line1雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN18);
        //                List_CCD01_02_PIN量測參數_Line1雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN19);
        //                List_CCD01_02_PIN量測參數_Line1雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line1雜訊抑制_PIN20);
        //                #endregion
        //                #region Line1框架測密度間隔
        //                List_CCD01_02_PIN量測參數_Line1框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN01);
        //                List_CCD01_02_PIN量測參數_Line1框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN02);
        //                List_CCD01_02_PIN量測參數_Line1框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN03);
        //                List_CCD01_02_PIN量測參數_Line1框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN04);
        //                List_CCD01_02_PIN量測參數_Line1框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN05);
        //                List_CCD01_02_PIN量測參數_Line1框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN06);
        //                List_CCD01_02_PIN量測參數_Line1框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN07);
        //                List_CCD01_02_PIN量測參數_Line1框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN08);
        //                List_CCD01_02_PIN量測參數_Line1框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN09);
        //                List_CCD01_02_PIN量測參數_Line1框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN10);
        //                List_CCD01_02_PIN量測參數_Line1框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN11);
        //                List_CCD01_02_PIN量測參數_Line1框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN12);
        //                List_CCD01_02_PIN量測參數_Line1框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN13);
        //                List_CCD01_02_PIN量測參數_Line1框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN14);
        //                List_CCD01_02_PIN量測參數_Line1框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN15);
        //                List_CCD01_02_PIN量測參數_Line1框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN16);
        //                List_CCD01_02_PIN量測參數_Line1框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN17);
        //                List_CCD01_02_PIN量測參數_Line1框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN18);
        //                List_CCD01_02_PIN量測參數_Line1框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN19);
        //                List_CCD01_02_PIN量測參數_Line1框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架測密度間隔_PIN20);
        //                #endregion
        //                #region Line1最佳回歸線過濾門檻
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN01);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN02);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN03);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN04);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN05);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN06);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN07);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN08);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN09);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN10);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN11);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN12);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN13);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN14);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN15);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN16);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN17);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN18);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN19);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻_PIN20);
        //                #endregion
        //                #region Line1最佳回歸線計算次數
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN01);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN02);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN03);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN04);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN05);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN06);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN07);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN08);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN09);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN10);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN11);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN12);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN13);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN14);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN15);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN16);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN17);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN18);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN19);
        //                List_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數_PIN20);
        //                #endregion
        //                #region Line1量測顏色變化
        //                List_CCD01_02_PIN量測參數_Line1量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN01);
        //                List_CCD01_02_PIN量測參數_Line1量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN02);
        //                List_CCD01_02_PIN量測參數_Line1量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN03);
        //                List_CCD01_02_PIN量測參數_Line1量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN04);
        //                List_CCD01_02_PIN量測參數_Line1量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN05);
        //                List_CCD01_02_PIN量測參數_Line1量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN06);
        //                List_CCD01_02_PIN量測參數_Line1量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN07);
        //                List_CCD01_02_PIN量測參數_Line1量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN08);
        //                List_CCD01_02_PIN量測參數_Line1量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN09);
        //                List_CCD01_02_PIN量測參數_Line1量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN10);
        //                List_CCD01_02_PIN量測參數_Line1量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN11);
        //                List_CCD01_02_PIN量測參數_Line1量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN12);
        //                List_CCD01_02_PIN量測參數_Line1量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN13);
        //                List_CCD01_02_PIN量測參數_Line1量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN14);
        //                List_CCD01_02_PIN量測參數_Line1量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN15);
        //                List_CCD01_02_PIN量測參數_Line1量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN16);
        //                List_CCD01_02_PIN量測參數_Line1量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN17);
        //                List_CCD01_02_PIN量測參數_Line1量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN18);
        //                List_CCD01_02_PIN量測參數_Line1量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN19);
        //                List_CCD01_02_PIN量測參數_Line1量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測顏色變化_PIN20);
        //                #endregion                   
        //                #region Line1量測路徑一半長度
        //                List_CCD01_02_PIN量測參數_Line1量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN01);
        //                List_CCD01_02_PIN量測參數_Line1量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN02);
        //                List_CCD01_02_PIN量測參數_Line1量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN03);
        //                List_CCD01_02_PIN量測參數_Line1量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN04);
        //                List_CCD01_02_PIN量測參數_Line1量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN05);
        //                List_CCD01_02_PIN量測參數_Line1量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN06);
        //                List_CCD01_02_PIN量測參數_Line1量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN07);
        //                List_CCD01_02_PIN量測參數_Line1量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN08);
        //                List_CCD01_02_PIN量測參數_Line1量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN09);
        //                List_CCD01_02_PIN量測參數_Line1量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN10);
        //                List_CCD01_02_PIN量測參數_Line1量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN11);
        //                List_CCD01_02_PIN量測參數_Line1量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN12);
        //                List_CCD01_02_PIN量測參數_Line1量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN13);
        //                List_CCD01_02_PIN量測參數_Line1量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN14);
        //                List_CCD01_02_PIN量測參數_Line1量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN15);
        //                List_CCD01_02_PIN量測參數_Line1量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN16);
        //                List_CCD01_02_PIN量測參數_Line1量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN17);
        //                List_CCD01_02_PIN量測參數_Line1量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN18);
        //                List_CCD01_02_PIN量測參數_Line1量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN19);
        //                List_CCD01_02_PIN量測參數_Line1量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line1量測路徑一半長度_PIN20);
        //                #endregion
        //                #region Line1框架起始Tip1X
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN01);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN02);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN03);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN04);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN05);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN06);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN07);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN08);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN09);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN10);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN11);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN12);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN13);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN14);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN15);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN16);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN17);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN18);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN19);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1X_PIN20);
        //                #endregion
        //                #region Line1框架起始Tip1Y
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN01);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN02);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN03);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN04);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN05);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN06);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN07);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN08);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN09);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN10);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN11);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN12);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN13);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN14);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN15);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN16);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN17);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN18);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN19);
        //                List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架起始Tip1Y_PIN20);
        //                #endregion
        //                #region Line1框架終止Tip2X
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN01);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN02);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN03);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN04);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN05);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN06);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN07);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN08);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN09);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN10);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN11);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN12);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN13);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN14);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN15);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN16);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN17);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN18);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN19);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2X_PIN20);
        //                #endregion
        //                #region Line1框架終止Tip2Y
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN01);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN02);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN03);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN04);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN05);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN06);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN07);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN08);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN09);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN10);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN11);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN12);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN13);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN14);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN15);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN16);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN17);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN18);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN19);
        //                List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line1框架終止Tip2Y_PIN20);
        //                #endregion

        //                #region Line2變化銳利度
        //                List_CCD01_02_PIN量測參數_Line2變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN01);
        //                List_CCD01_02_PIN量測參數_Line2變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN02);
        //                List_CCD01_02_PIN量測參數_Line2變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN03);
        //                List_CCD01_02_PIN量測參數_Line2變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN04);
        //                List_CCD01_02_PIN量測參數_Line2變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN05);
        //                List_CCD01_02_PIN量測參數_Line2變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN06);
        //                List_CCD01_02_PIN量測參數_Line2變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN07);
        //                List_CCD01_02_PIN量測參數_Line2變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN08);
        //                List_CCD01_02_PIN量測參數_Line2變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN09);
        //                List_CCD01_02_PIN量測參數_Line2變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN10);
        //                List_CCD01_02_PIN量測參數_Line2變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN11);
        //                List_CCD01_02_PIN量測參數_Line2變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN12);
        //                List_CCD01_02_PIN量測參數_Line2變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN13);
        //                List_CCD01_02_PIN量測參數_Line2變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN14);
        //                List_CCD01_02_PIN量測參數_Line2變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN15);
        //                List_CCD01_02_PIN量測參數_Line2變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN16);
        //                List_CCD01_02_PIN量測參數_Line2變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN17);
        //                List_CCD01_02_PIN量測參數_Line2變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN18);
        //                List_CCD01_02_PIN量測參數_Line2變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN19);
        //                List_CCD01_02_PIN量測參數_Line2變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN20);
        //                #endregion
        //                #region Line2變化強度門檻值
        //                List_CCD01_02_PIN量測參數_Line2變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN01);
        //                List_CCD01_02_PIN量測參數_Line2變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN02);
        //                List_CCD01_02_PIN量測參數_Line2變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN03);
        //                List_CCD01_02_PIN量測參數_Line2變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN04);
        //                List_CCD01_02_PIN量測參數_Line2變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN05);
        //                List_CCD01_02_PIN量測參數_Line2變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN06);
        //                List_CCD01_02_PIN量測參數_Line2變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN07);
        //                List_CCD01_02_PIN量測參數_Line2變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN08);
        //                List_CCD01_02_PIN量測參數_Line2變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN09);
        //                List_CCD01_02_PIN量測參數_Line2變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN10);
        //                List_CCD01_02_PIN量測參數_Line2變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN11);
        //                List_CCD01_02_PIN量測參數_Line2變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN12);
        //                List_CCD01_02_PIN量測參數_Line2變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN13);
        //                List_CCD01_02_PIN量測參數_Line2變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN14);
        //                List_CCD01_02_PIN量測參數_Line2變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN15);
        //                List_CCD01_02_PIN量測參數_Line2變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN16);
        //                List_CCD01_02_PIN量測參數_Line2變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN17);
        //                List_CCD01_02_PIN量測參數_Line2變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN18);
        //                List_CCD01_02_PIN量測參數_Line2變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN19);
        //                List_CCD01_02_PIN量測參數_Line2變化強度門檻值.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化強度門檻值_PIN20);
        //                #endregion
        //                #region Line2灰階面化面積
        //                List_CCD01_02_PIN量測參數_Line2灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN01);
        //                List_CCD01_02_PIN量測參數_Line2灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN02);
        //                List_CCD01_02_PIN量測參數_Line2灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN03);
        //                List_CCD01_02_PIN量測參數_Line2灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN04);
        //                List_CCD01_02_PIN量測參數_Line2灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN05);
        //                List_CCD01_02_PIN量測參數_Line2灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN06);
        //                List_CCD01_02_PIN量測參數_Line2灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN07);
        //                List_CCD01_02_PIN量測參數_Line2灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN08);
        //                List_CCD01_02_PIN量測參數_Line2灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN09);
        //                List_CCD01_02_PIN量測參數_Line2灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN10);
        //                List_CCD01_02_PIN量測參數_Line2灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN11);
        //                List_CCD01_02_PIN量測參數_Line2灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN12);
        //                List_CCD01_02_PIN量測參數_Line2灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN13);
        //                List_CCD01_02_PIN量測參數_Line2灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN14);
        //                List_CCD01_02_PIN量測參數_Line2灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN15);
        //                List_CCD01_02_PIN量測參數_Line2灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN16);
        //                List_CCD01_02_PIN量測參數_Line2灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN17);
        //                List_CCD01_02_PIN量測參數_Line2灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN18);
        //                List_CCD01_02_PIN量測參數_Line2灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN19);
        //                List_CCD01_02_PIN量測參數_Line2灰階面化面積.Add(PLC_Device_CCD01_02_PIN量測參數_Line2灰階面化面積_PIN20);
        //                #endregion
        //                #region Line2垂直量測寬度
        //                List_CCD01_02_PIN量測參數_Line2垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN01);
        //                List_CCD01_02_PIN量測參數_Line2垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN02);
        //                List_CCD01_02_PIN量測參數_Line2垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN03);
        //                List_CCD01_02_PIN量測參數_Line2垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN04);
        //                List_CCD01_02_PIN量測參數_Line2垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN05);
        //                List_CCD01_02_PIN量測參數_Line2垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN06);
        //                List_CCD01_02_PIN量測參數_Line2垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN07);
        //                List_CCD01_02_PIN量測參數_Line2垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN08);
        //                List_CCD01_02_PIN量測參數_Line2垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN09);
        //                List_CCD01_02_PIN量測參數_Line2垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN10);
        //                List_CCD01_02_PIN量測參數_Line2垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN11);
        //                List_CCD01_02_PIN量測參數_Line2垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN12);
        //                List_CCD01_02_PIN量測參數_Line2垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN13);
        //                List_CCD01_02_PIN量測參數_Line2垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN14);
        //                List_CCD01_02_PIN量測參數_Line2垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN15);
        //                List_CCD01_02_PIN量測參數_Line2垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN16);
        //                List_CCD01_02_PIN量測參數_Line2垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN17);
        //                List_CCD01_02_PIN量測參數_Line2垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN18);
        //                List_CCD01_02_PIN量測參數_Line2垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN19);
        //                List_CCD01_02_PIN量測參數_Line2垂直量測寬度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2垂直量測寬度_PIN20);
        //                #endregion
        //                #region Line2雜訊抑制
        //                List_CCD01_02_PIN量測參數_Line2雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN01);
        //                List_CCD01_02_PIN量測參數_Line2雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN02);
        //                List_CCD01_02_PIN量測參數_Line2雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN03);
        //                List_CCD01_02_PIN量測參數_Line2雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN04);
        //                List_CCD01_02_PIN量測參數_Line2雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN05);
        //                List_CCD01_02_PIN量測參數_Line2雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN06);
        //                List_CCD01_02_PIN量測參數_Line2雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN07);
        //                List_CCD01_02_PIN量測參數_Line2雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN08);
        //                List_CCD01_02_PIN量測參數_Line2雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN09);
        //                List_CCD01_02_PIN量測參數_Line2雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN10);
        //                List_CCD01_02_PIN量測參數_Line2雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN11);
        //                List_CCD01_02_PIN量測參數_Line2雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN12);
        //                List_CCD01_02_PIN量測參數_Line2雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN13);
        //                List_CCD01_02_PIN量測參數_Line2雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN14);
        //                List_CCD01_02_PIN量測參數_Line2雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN15);
        //                List_CCD01_02_PIN量測參數_Line2雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN16);
        //                List_CCD01_02_PIN量測參數_Line2雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN17);
        //                List_CCD01_02_PIN量測參數_Line2雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN18);
        //                List_CCD01_02_PIN量測參數_Line2雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN19);
        //                List_CCD01_02_PIN量測參數_Line2雜訊抑制.Add(PLC_Device_CCD01_02_PIN量測參數_Line2雜訊抑制_PIN20);
        //                #endregion
        //                #region Line2框架測密度間隔
        //                List_CCD01_02_PIN量測參數_Line2框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN01);
        //                List_CCD01_02_PIN量測參數_Line2框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN02);
        //                List_CCD01_02_PIN量測參數_Line2框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN03);
        //                List_CCD01_02_PIN量測參數_Line2框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN04);
        //                List_CCD01_02_PIN量測參數_Line2框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN05);
        //                List_CCD01_02_PIN量測參數_Line2框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN06);
        //                List_CCD01_02_PIN量測參數_Line2框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN07);
        //                List_CCD01_02_PIN量測參數_Line2框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN08);
        //                List_CCD01_02_PIN量測參數_Line2框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN09);
        //                List_CCD01_02_PIN量測參數_Line2框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN10);
        //                List_CCD01_02_PIN量測參數_Line2框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN11);
        //                List_CCD01_02_PIN量測參數_Line2框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN12);
        //                List_CCD01_02_PIN量測參數_Line2框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN13);
        //                List_CCD01_02_PIN量測參數_Line2框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN14);
        //                List_CCD01_02_PIN量測參數_Line2框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN15);
        //                List_CCD01_02_PIN量測參數_Line2框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN16);
        //                List_CCD01_02_PIN量測參數_Line2框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN17);
        //                List_CCD01_02_PIN量測參數_Line2框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN18);
        //                List_CCD01_02_PIN量測參數_Line2框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN19);
        //                List_CCD01_02_PIN量測參數_Line2框架測密度間隔.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架測密度間隔_PIN20);
        //                #endregion
        //                #region Line2最佳回歸線過濾門檻
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN01);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN02);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN03);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN04);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN05);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN06);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN07);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN08);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN09);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN10);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN11);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN12);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN13);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN14);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN15);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN16);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN17);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN18);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻_PIN19);
        //                List_CCD01_02_PIN量測參數_Line2變化銳利度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2變化銳利度_PIN20);
        //                #endregion
        //                #region Line2最佳回歸線計算次數
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN01);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN02);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN03);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN04);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN05);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN06);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN07);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN08);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN09);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN10);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN11);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN12);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN13);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN14);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN15);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN16);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN17);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN18);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN19);
        //                List_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數.Add(PLC_Device_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數_PIN20);
        //                #endregion
        //                #region Line2量測顏色變化
        //                List_CCD01_02_PIN量測參數_Line2量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN01);
        //                List_CCD01_02_PIN量測參數_Line2量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN02);
        //                List_CCD01_02_PIN量測參數_Line2量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN03);
        //                List_CCD01_02_PIN量測參數_Line2量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN04);
        //                List_CCD01_02_PIN量測參數_Line2量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN05);
        //                List_CCD01_02_PIN量測參數_Line2量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN06);
        //                List_CCD01_02_PIN量測參數_Line2量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN07);
        //                List_CCD01_02_PIN量測參數_Line2量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN08);
        //                List_CCD01_02_PIN量測參數_Line2量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN09);
        //                List_CCD01_02_PIN量測參數_Line2量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN10);
        //                List_CCD01_02_PIN量測參數_Line2量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN11);
        //                List_CCD01_02_PIN量測參數_Line2量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN12);
        //                List_CCD01_02_PIN量測參數_Line2量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN13);
        //                List_CCD01_02_PIN量測參數_Line2量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN14);
        //                List_CCD01_02_PIN量測參數_Line2量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN15);
        //                List_CCD01_02_PIN量測參數_Line2量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN16);
        //                List_CCD01_02_PIN量測參數_Line2量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN17);
        //                List_CCD01_02_PIN量測參數_Line2量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN18);
        //                List_CCD01_02_PIN量測參數_Line2量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN19);
        //                List_CCD01_02_PIN量測參數_Line2量測顏色變化.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測顏色變化_PIN20);
        //                #endregion
        //                #region Line2量測路徑一半長度
        //                List_CCD01_02_PIN量測參數_Line2量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN01);
        //                List_CCD01_02_PIN量測參數_Line2量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN02);
        //                List_CCD01_02_PIN量測參數_Line2量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN03);
        //                List_CCD01_02_PIN量測參數_Line2量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN04);
        //                List_CCD01_02_PIN量測參數_Line2量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN05);
        //                List_CCD01_02_PIN量測參數_Line2量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN06);
        //                List_CCD01_02_PIN量測參數_Line2量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN07);
        //                List_CCD01_02_PIN量測參數_Line2量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN08);
        //                List_CCD01_02_PIN量測參數_Line2量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN09);
        //                List_CCD01_02_PIN量測參數_Line2量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN10);
        //                List_CCD01_02_PIN量測參數_Line2量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN11);
        //                List_CCD01_02_PIN量測參數_Line2量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN12);
        //                List_CCD01_02_PIN量測參數_Line2量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN13);
        //                List_CCD01_02_PIN量測參數_Line2量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN14);
        //                List_CCD01_02_PIN量測參數_Line2量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN15);
        //                List_CCD01_02_PIN量測參數_Line2量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN16);
        //                List_CCD01_02_PIN量測參數_Line2量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN17);
        //                List_CCD01_02_PIN量測參數_Line2量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN18);
        //                List_CCD01_02_PIN量測參數_Line2量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN19);
        //                List_CCD01_02_PIN量測參數_Line2量測路徑一半長度.Add(PLC_Device_CCD01_02_PIN量測參數_Line2量測路徑一半長度_PIN20);
        //                #endregion
        //                #region Line2框架起始Tip1X
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN01);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN02);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN03);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN04);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN05);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN06);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN07);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN08);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN09);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN10);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN11);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN12);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN13);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN14);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN15);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN16);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN17);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN18);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN19);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1X_PIN20);
        //                #endregion
        //                #region Line2框架起始Tip1Y
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN01);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN02);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN03);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN04);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN05);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN06);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN07);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN08);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN09);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN10);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN11);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN12);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN13);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN14);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN15);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN16);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN17);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN18);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN19);
        //                List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架起始Tip1Y_PIN20);
        //                #endregion
        //                #region Line2框架終止Tip2X
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN01);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN02);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN03);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN04);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN05);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN06);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN07);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN08);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN09);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN10);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN11);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN12);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN13);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN14);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN15);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN16);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN17);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN18);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN19);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2X.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2X_PIN20);
        //                #endregion
        //                #region Line2框架終止Tip2Y
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN01);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN02);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN03);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN04);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN05);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN06);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN07);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN08);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN09);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN10);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN11);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN12);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN13);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN14);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN15);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN16);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN17);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN18);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN19);
        //                List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y.Add(PLC_Device_CCD01_02_PIN量測參數_Line2框架終止Tip2Y_PIN20);
        //                #endregion
        //                #endregion
        //                PLC_Device_CCD01_02_PIN量測_量測框調整.SetComment("PLC_CCD01_02_PIN量測_量測框調整");
        //                PLC_Device_CCD01_02_PIN量測_量測框調整按鈕.Bool = false;
        //                PLC_Device_CCD01_02_PIN量測_量測框調整.Bool = false;
        //                cnt_Program_CCD01_02_PIN量測_量測框調整 = 65535;
        //            }
        //            if (cnt_Program_CCD01_02_PIN量測_量測框調整 == 65535) cnt_Program_CCD01_02_PIN量測_量測框調整 = 1;
        //            if (cnt_Program_CCD01_02_PIN量測_量測框調整 == 1) cnt_Program_CCD01_02_PIN量測_量測框調整_觸發按下(ref cnt_Program_CCD01_02_PIN量測_量測框調整);
        //            if (cnt_Program_CCD01_02_PIN量測_量測框調整 == 2) cnt_Program_CCD01_02_PIN量測_量測框調整_檢查按下(ref cnt_Program_CCD01_02_PIN量測_量測框調整);
        //            if (cnt_Program_CCD01_02_PIN量測_量測框調整 == 3) cnt_Program_CCD01_02_PIN量測_量測框調整_初始化(ref cnt_Program_CCD01_02_PIN量測_量測框調整);
        //            if (cnt_Program_CCD01_02_PIN量測_量測框調整 == 4) cnt_Program_CCD01_02_PIN量測_量測框調整_座標轉換(ref cnt_Program_CCD01_02_PIN量測_量測框調整);
        //            if (cnt_Program_CCD01_02_PIN量測_量測框調整 == 5) cnt_Program_CCD01_02_PIN量測_量測框調整_讀取參數(ref cnt_Program_CCD01_02_PIN量測_量測框調整);
        //            if (cnt_Program_CCD01_02_PIN量測_量測框調整 == 6) cnt_Program_CCD01_02_PIN量測_量測框調整_量測框初始化(ref cnt_Program_CCD01_02_PIN量測_量測框調整);
        //            if (cnt_Program_CCD01_02_PIN量測_量測框調整 == 7) cnt_Program_CCD01_02_PIN量測_量測框調整_計算中心點(ref cnt_Program_CCD01_02_PIN量測_量測框調整);
        //            if (cnt_Program_CCD01_02_PIN量測_量測框調整 == 8) cnt_Program_CCD01_02_PIN量測_量測框調整_繪製畫布(ref cnt_Program_CCD01_02_PIN量測_量測框調整);
        //            if (cnt_Program_CCD01_02_PIN量測_量測框調整 == 9) cnt_Program_CCD01_02_PIN量測_量測框調整 = 65500;
        //            if (cnt_Program_CCD01_02_PIN量測_量測框調整 > 1) cnt_Program_CCD01_02_PIN量測_量測框調整_檢查放開(ref cnt_Program_CCD01_02_PIN量測_量測框調整);

        //            if (cnt_Program_CCD01_02_PIN量測_量測框調整 == 65500)
        //            {
        //                cnt_Program_CCD01_02_PIN量測_量測框調整 = 65535;
        //            }
        //        }
        //        void cnt_Program_CCD01_02_PIN量測_量測框調整_觸發按下(ref int cnt)
        //        {
        //            if (PLC_Device_CCD01_02_PIN量測_量測框調整按鈕.Bool || PLC_Device_CCD01_02_計算一次.Bool)
        //            {
        //                PLC_Device_CCD01_02_PIN量測_量測框調整.Bool = true;
        //                cnt++;
        //            }

        //        }
        //        void cnt_Program_CCD01_02_PIN量測_量測框調整_檢查按下(ref int cnt)
        //        {
        //            if (PLC_Device_CCD01_02_PIN量測_量測框調整.Bool) cnt++;
        //        }
        //        void cnt_Program_CCD01_02_PIN量測_量測框調整_檢查放開(ref int cnt)
        //        {
        //            if (!PLC_Device_CCD01_02_PIN量測_量測框調整按鈕.Bool)
        //            {
        //                PLC_Device_CCD01_02_PIN量測_量測框調整.Bool = false;
        //                cnt = 65500;
        //            }
        //        }
        //        void cnt_Program_CCD01_02_PIN量測_量測框調整_初始化(ref int cnt)
        //        {
        //            this.MyTimer_CCD01_02_PIN量測_量測框調整.TickStop();
        //            this.MyTimer_CCD01_02_PIN量測_量測框調整.StartTickTime(99999);
        //            this.List_CCD01_02_PIN量測參數_量測點 = new PointF[20];
        //            this.List_CCD01_02_PIN量測參數_量測點_結果 = new PointF[20];
        //            this.List_CCD01_02_PIN量測參數_量測點_轉換後座標 = new Point[20];
        //            this.List_CCD01_02_PIN量測參數_量測點_有無 = new bool[20];
        //           // this.CCD01_02_SrcImageHandle = h_Canvas_Tech_CCD01_02.VegaHandle;
        //            cnt++;
        //        }
        //        void cnt_Program_CCD01_02_PIN量測_量測框調整_座標轉換(ref int cnt)
        //        {
        //            if (PLC_Device_CCD01_02_計算一次.Bool)
        //            {
        //                CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.RefPointX = PLC_Device_CCD01_01_水平基準線量測_量測中心_X.Value;
        //                CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.RefPointY = PLC_Device_CCD01_01_水平基準線量測_量測中心_Y.Value;
        //                CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.RefAngle = 0;
        //                CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentRefPointX = Point_CCD01_01_中心基準座標_量測點.X;
        //                CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentRefPointY = Point_CCD01_01_中心基準座標_量測點.Y;
        //                CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentRefAngle = 0;
        //                CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.NumOfVisionPoints = 20;

        //                for (int j = 0; j < 20; j++)
        //                {
        //                    //if (this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[j].Value == 0) this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[j].Value = 100;
        //                    //if (this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[j].Value == 0) this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[j].Value = 100;
        //                    //if (this.List_PLC_Device_CCD01_02_PIN量測參數_Width[j].Value == 0) this.List_PLC_Device_CCD01_02_PIN量測參數_Width[j].Value = 100;
        //                    //if (this.List_PLC_Device_CCD01_02_PIN量測參數_Height[j].Value == 0) this.List_PLC_Device_CCD01_02_PIN量測參數_Height[j].Value = 100;

        //                    if (this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterX[j].Value == 0) this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterX[j].Value = 100;
        //                    if (this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterY[j].Value == 0) this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterY[j].Value = 100;
        //                    if (this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1X[j].Value == 0) this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1X[j].Value = 100;
        //                    if (this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y[j].Value == 0) this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y[j].Value = 100;
        //                    if (this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2X[j].Value == 0) this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2X[j].Value = 120;
        //                    if (this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y[j].Value == 0) this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y[j].Value = 120;
        //                    if (this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1X[j].Value == 0) this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1X[j].Value = 100;
        //                    if (this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y[j].Value == 0) this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y[j].Value = 100;
        //                    if (this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2X[j].Value == 0) this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2X[j].Value = 120;
        //                    if (this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y[j].Value == 0) this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y[j].Value = 120;

        //                    CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
        //                    //CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.VisionPointX = this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[j].Value;
        //                    //CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.VisionPointY = this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[j].Value;
        //                    CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.VisionPointX = this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterX[j].Value;
        //                    CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.VisionPointY = this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterY[j].Value;
        //                }
        //                CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.EstimateCurrentVisionPoints();
        //                for (int j = 0; j < 20; j++)
        //                {
        //                    CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
        //                    List_CCD01_02_PIN量測參數_量測點_轉換後座標[j].X = (int)CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentVisionPointX;
        //                    List_CCD01_02_PIN量測參數_量測點_轉換後座標[j].Y = (int)CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentVisionPointY;
        //                }
        //            }
        //            cnt++;

        //        }
        //        void cnt_Program_CCD01_02_PIN量測_量測框調整_讀取參數(ref int cnt)
        //        {
        //            for (int i = 0; i < this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
        //            {
        //                //if (this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[i].Value > 2596) this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[i].Value = 0;
        //                //if (this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[i].Value > 1922) this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[i].Value = 0;
        //                //if (this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[i].Value < 0) this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[i].Value = 0;
        //                //if (this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[i].Value < 0) this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[i].Value = 0;


        //            }
        //            for (int i = 0; i < this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整.Count; i++)
        //            {
        //                if (this.List_CCD01_02_PIN量測參數_量測點_轉換後座標[i].X > 2596) this.List_CCD01_02_PIN量測參數_量測點_轉換後座標[i].X = 0;
        //                if (this.List_CCD01_02_PIN量測參數_量測點_轉換後座標[i].Y > 1922) this.List_CCD01_02_PIN量測參數_量測點_轉換後座標[i].Y = 0;
        //                if (this.List_CCD01_02_PIN量測參數_量測點_轉換後座標[i].X < 0) this.List_CCD01_02_PIN量測參數_量測點_轉換後座標[i].X = 0;
        //                if (this.List_CCD01_02_PIN量測參數_量測點_轉換後座標[i].Y < 0) this.List_CCD01_02_PIN量測參數_量測點_轉換後座標[i].Y = 0;

        //                this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].SrcImageHandle = this.CCD01_02_SrcImageHandle;
        //                if (PLC_Device_CCD01_02_計算一次.Bool)
        //                {
        //                    this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].ProbeCenterX = List_CCD01_02_PIN量測參數_量測點_轉換後座標[i].X;
        //                    this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].ProbeCenterY = List_CCD01_02_PIN量測參數_量測點_轉換後座標[i].Y;
        //                }
        //                else
        //                {
        //                    this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].ProbeCenterX = this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterX[i].Value;
        //                    this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].ProbeCenterY = this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterY[i].Value;
        //                }
        //                this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line1Tip1X = this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1X[i].Value;
        //                this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line1Tip1Y = this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y[i].Value;
        //                this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line1Tip2X = this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2X[i].Value;
        //                this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line1Tip2Y = this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y[i].Value;
        //                this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line2Tip1X = this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1X[i].Value;
        //                this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line2Tip1Y = this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y[i].Value;
        //                this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line2Tip2X = this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2X[i].Value;
        //                this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line2Tip2Y = this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y[i].Value;
        //            }
        //            cnt++;
        //        }
        //        //void cnt_Program_CCD01_02_PIN量測_量測框調整_開始區塊分析(ref int cnt)
        //        //{
        //        //    uint object_value = 4294963615;

        //        //    for (int i = 0; i < this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
        //        //    {

        //        //        this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].SrcImageHandle = this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].VegaHandle;
        //        //        this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].ObjectClass = AxOvkBlob.TxAxObjClass.AX_OBJECT_DETECT_LIGHTER_CLASS;
        //        //        this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].HighThreshold = List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值[0].Value;
        //        //        if (this.CCD01_02_SrcImageHandle != 0)
        //        //        {
        //        //            if (this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[i].Value + this.List_PLC_Device_CCD01_02_PIN量測參數_Width[i].Value < 2596 &&
        //        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[i].Value + this.List_PLC_Device_CCD01_02_PIN量測參數_Height[i].Value < 1922 &&
        //        //                this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[i].Value > 0 && this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[i].Value > 0)
        //        //            {
        //        //                this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].BlobAnalyze(false);
        //        //            }


        //        //        }
        //        //        this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].CalculateFeatures((int)object_value, -1);
        //        //        this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].SortObjects(AxOvkBlob.TxAxObjFeatureSortOrder.AX_OBJECT_SORT_ORDER_LARGE_TO_SMALL, AxOvkBlob.TxAxObjFeature.AX_OBJECT_FEATURE_AREA, 0, -1);
        //        //        this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].SelectObjects(AxOvkBlob.TxAxObjFeature.AX_OBJECT_FEATURE_AREA, AxOvkBlob.TxAxObjFeatureOperation.AX_OBJECT_REMOVE_LESS_THAN, this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限[0].Value);
        //        //        this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].SelectObjects(AxOvkBlob.TxAxObjFeature.AX_OBJECT_FEATURE_AREA, AxOvkBlob.TxAxObjFeatureOperation.AX_OBJECT_REMOVE_GREAT_THAN, this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限[0].Value);
        //        //        if (this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].DetectedNumObjs > 0)
        //        //        {
        //        //            this.List_CCD01_02_PIN量測參數_量測點_有無[i] = true;
        //        //            this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].BlobIndex = 0;
        //        //            this.List_CCD01_02_PIN量測參數_量測點[i].X = (float)this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].BlobCentroidX;
        //        //            this.List_CCD01_02_PIN量測參數_量測點[i].X += this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].OrgX;
        //        //            this.List_CCD01_02_PIN量測參數_量測點[i].Y = (float)this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].BlobCentroidY;
        //        //            //this.List_CCD01_02_PIN量測參數_量測點[i].Y = (float)this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].BlobCentroidY - (float)this.List_CCD01_02_PIN量測_AxObject_區塊分析[i].BlobLimBoxHeight / 2;
        //        //            this.List_CCD01_02_PIN量測參數_量測點[i].Y += this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整[i].OrgY;
        //        //        }


        //        //    }

        //        //    cnt++;
        //        //}
        //        void cnt_Program_CCD01_02_PIN量測_量測框調整_量測框初始化(ref int cnt)
        //        {
        //            for (int i = 0; i < this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整.Count; i++)
        //            {
        //                List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line1DeriThreshold = List_CCD01_02_PIN量測參數_Line1變化銳利度[0].Value;
        //                List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line1Hysteresis = List_CCD01_02_PIN量測參數_Line1變化強度門檻值[0].Value;
        //                List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line1MinGreyStep = List_CCD01_02_PIN量測參數_Line1灰階面化面積[0].Value;
        //                List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line1SmoothFactor = List_CCD01_02_PIN量測參數_Line1雜訊抑制[0].Value;
        //                List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line1HalfProfileThickness = List_CCD01_02_PIN量測參數_Line1垂直量測寬度[0].Value;
        //                List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line1SampleStep = List_CCD01_02_PIN量測參數_Line1框架測密度間隔[0].Value;
        //                List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line1FilterCount = List_CCD01_02_PIN量測參數_Line1最佳回歸線計算次數[0].Value;
        //                List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line1FilterThreshold = List_CCD01_02_PIN量測參數_Line1最佳回歸線過濾門檻[0].Value;
        //                List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line1HalfHeight = List_CCD01_02_PIN量測參數_Line1量測路徑一半長度[0].Value;
        //                List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line1EdgeType = (AxOvkMsr.TxAxTransitionType)List_CCD01_02_PIN量測參數_Line1量測顏色變化[0].Value;

        //                List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line2DeriThreshold = List_CCD01_02_PIN量測參數_Line2變化銳利度[0].Value;
        //                List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line2Hysteresis = List_CCD01_02_PIN量測參數_Line2變化強度門檻值[0].Value;
        //                List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line2MinGreyStep = List_CCD01_02_PIN量測參數_Line2灰階面化面積[0].Value;
        //                List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line2SmoothFactor = List_CCD01_02_PIN量測參數_Line2雜訊抑制[0].Value;
        //                List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line2HalfProfileThickness = List_CCD01_02_PIN量測參數_Line2垂直量測寬度[0].Value;
        //                List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line2SampleStep = List_CCD01_02_PIN量測參數_Line2框架測密度間隔[0].Value;
        //                List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line2FilterCount = List_CCD01_02_PIN量測參數_Line2最佳回歸線計算次數[0].Value;
        //                List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line2FilterThreshold = List_CCD01_02_PIN量測參數_Line2最佳回歸線過濾門檻[0].Value;
        //                List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line2HalfHeight = List_CCD01_02_PIN量測參數_Line2量測路徑一半長度[0].Value;
        //                List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].Line2EdgeType = (AxOvkMsr.TxAxTransitionType)List_CCD01_02_PIN量測參數_Line2量測顏色變化[0].Value;

        //                List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].PivotX = List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].ProbeCenterX;
        //                List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].PivotY = List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].ProbeCenterY + 40;


        //                List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].DetectPrimitives();

        //            }

        //                cnt++;
        //        }
        //        void cnt_Program_CCD01_02_PIN量測_量測框調整_計算中心點(ref int cnt)
        //        {
        //            PLC_Device_CCD01_02_PIN量測_量測框調整_OK.Bool = true;
        //            for (int i = 0; i < this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整.Count; i++)
        //            {
        //                if(!PLC_Device_CCD01_02_PIN量測_有無量測不測試.Bool)
        //                {
        //                    if (List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].MeasuredGap1IsFound)
        //                    {
        //                        this.List_CCD01_02_PIN量測參數_量測點_有無[i] = true;
        //                        List_CCD01_02_PIN量測參數_量測點[i].X = (float)List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].MeasuredGap1CenterX;
        //                        List_CCD01_02_PIN量測參數_量測點[i].Y = (float)List_CCD01_02_PIN量測_AxAngleMsr_量測框調整[i].MeasuredGap1CenterY;
        //                    }
        //                    else PLC_Device_CCD01_02_PIN量測_量測框調整_OK.Bool = false;
        //                }
        //                else PLC_Device_CCD01_02_PIN量測_量測框調整_OK.Bool = true;
        //            }
        //            cnt++;

        //        }
        //        void cnt_Program_CCD01_02_PIN量測_量測框調整_繪製畫布(ref int cnt)
        //        {
        //            this.PLC_Device_CCD01_02_PIN量測_量測框調整_RefreshCanvas.Bool = true;
        //            if (this.PLC_Device_CCD01_02_PIN量測_量測框調整按鈕.Bool && !PLC_Device_CCD01_02_計算一次.Bool)
        //            {
        //                this.h_Canvas_Tech_CCD01_02.RefreshCanvas();
        //            }

        //            cnt++;
        //        }



        //        #endregion
        //        #region PLC_CCD01_02_PIN量測_檢測距離計算
        //        private AxOvkMsr.AxPointLineDistanceMsr CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測;
        //        MyTimer MyTimer_CCD01_02_PIN量測_檢測距離計算 = new MyTimer();
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測距離計算按鈕 = new PLC_Device("S6050");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測距離計算 = new PLC_Device("S6045");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK = new PLC_Device("S6046");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測距離計算_測試完成 = new PLC_Device("S6047");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測距離計算_RefreshCanvas = new PLC_Device("S6048");

        //        PLC_Device PLC_Device_CCD01_02_PIN量測_水平度量測不測試 = new PLC_Device("S6100");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_間距量測不測試 = new PLC_Device("S6101");

        //        PLC_Device PLC_Device_CCD01_02_PIN量測_左右間距量測標準值 = new PLC_Device("F1300");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_左右間距量測上限值 = new PLC_Device("F1301");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_左右間距量測下限值 = new PLC_Device("F1302");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_上下間距量測標準值 = new PLC_Device("F1303");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_上下間距量測上限值 = new PLC_Device("F1304");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_上下間距量測下限值 = new PLC_Device("F1305");

        //        PLC_Device PLC_Device_CCD01_02_PIN量測_上排水平度量測標準值 = new PLC_Device("F1310");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_上排水平度量測上限值 = new PLC_Device("F1311");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_上排水平度量測下限值 = new PLC_Device("F1312");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_下排水平度量測標準值 = new PLC_Device("F1322");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_下排水平度量測上限值 = new PLC_Device("F1323");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_下排水平度量測下限值 = new PLC_Device("F1324");

        //        PLC_Device PLC_Device_CCD01_02_PIN量測_水平度量測差值 = new PLC_Device("F1313");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_水平度量測差值上限 = new PLC_Device("F1314");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_水平度量測差值下限 = new PLC_Device("F1315");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_間距上排PIN01到基準數值 = new PLC_Device("F1316");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_間距上排PIN01到基準上限 = new PLC_Device("F1317");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_間距上排PIN01到基準下限 = new PLC_Device("F1318");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_間距下排PIN01到基準數值 = new PLC_Device("F1319");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_間距下排PIN01到基準上限 = new PLC_Device("F1320");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_間距下排PIN01到基準下限 = new PLC_Device("F1321");



        //        private List<PLC_Device> List_PLC_Device_CCD01_02_PIN量測參數_間距不測試 = new List<PLC_Device>();
        //        private List<PLC_Device> List_PLC_Device_CCD01_02_PIN量測參數_左右間距量測值 = new List<PLC_Device>();

        //        private double[] List_CCD01_02_PIN量測參數_左右間距量測距離 = new double[19];
        //        private double[] List_CCD01_02_PIN量測參數_上下間距量測距離 = new double[19];
        //        private double[] List_CCD01_02_PIN量測參數_水平度量測距離 = new double[20];
        //        private double CCD01_02_PIN量測參數_間距上排PIN01到基準距離 = new double();
        //        private double CCD01_02_PIN量測參數_間距下排PIN01到基準距離 = new double();

        //        private bool[] List_CCD01_02_PIN量測參數_量測點_OK = new bool[20];
        //        private bool[] List_CCD01_02_PIN量測參數_左右間距量測_OK = new bool[19];
        //        private bool[] List_CCD01_02_PIN量測參數_上下間距量測_OK = new bool[19];
        //        private bool[] List_CCD01_02_PIN量測參數_水平度量測_OK = new bool[20];
        //        private bool CCD01_02_PIN量測參數_間距上排PIN01到基準_OK = new bool();
        //        private bool CCD01_02_PIN量測參數_間距下排PIN01到基準_OK = new bool();

        //        private double[] List_CCD01_02_PIN量測參數_水平度量測顯示點_X = new double[20];
        //        private double[] List_CCD01_02_PIN量測參數_水平度量測顯示點_Y = new double[20];
        //        private void H_Canvas_Tech_CCD01_02_PIN間距量測_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        //        {
        //            PointF p0;
        //            PointF p1;
        //            PointF 上排_p2;
        //            PointF 上排_p3;
        //            PointF 下排_p2;
        //            PointF 下排_p3;
        //            PointF point;
        //            PointF to_line_point;
        //            double 間距;
        //            double 水平度;

        //            if (PLC_Device_CCD01_02_Main_取像並檢驗.Bool || PLC_Device_CCD01_02_PLC觸發檢測.Bool)
        //            {
        //                try
        //                {
        //                    Graphics g = Graphics.FromHdc((IntPtr)HDC);
        //                    DrawingClass.Draw.十字中心(new PointF(this.Point_CCD01_01_中心基準座標_量測點.X, this.Point_CCD01_01_中心基準座標_量測點.Y), 100, Color.Red, 2, g, ZoomX, ZoomY);
        //                    #region 左右間距顯示
        //                    for (int i = 0; i < 19; i++)
        //                    {
        //                        p0 = new PointF(this.List_CCD01_02_PIN量測參數_量測點[i].X, this.List_CCD01_02_PIN量測參數_量測點[i].Y);
        //                        p1 = new PointF(this.List_CCD01_02_PIN量測參數_量測點[i + 1].X, this.List_CCD01_02_PIN量測參數_量測點[i + 1].Y);
        //                        間距 = List_CCD01_02_PIN量測參數_左右間距量測距離[i];

        //                        if (i != 9)
        //                        {
        //                            if (List_CCD01_02_PIN量測參數_左右間距量測_OK[i])
        //                            {
        //                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (間距 / 1D).ToString("0.000")), new PointF((float)((p0.X + p1.X) / 2),
        //                                    (float)((p0.Y + p1.Y) / 2) + 150 * ZoomY), new Font("標楷體",10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
        //                                DrawingClass.Draw.線段繪製(p0, p1, Color.Lime, 1, g, ZoomX, ZoomY);

        //                            }
        //                            else
        //                            {
        //                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (間距 / 1D).ToString("0.000")), new PointF((float)((p0.X + p1.X) / 2),
        //                                    (float)((p0.Y + p1.Y) / 2) + 150 * ZoomY), new Font("標楷體",10), Color.Black, Color.Red, g, ZoomX, ZoomY);
        //                                DrawingClass.Draw.線段繪製(p0, p1, Color.Red, 1, g, ZoomX, ZoomY);

        //                            }
        //                        }

        //                    }
        //                    上排_p2 = new PointF(this.List_CCD01_02_PIN量測參數_量測點[0].X, this.List_CCD01_02_PIN量測參數_量測點[0].Y - 150);
        //                    上排_p3 = new PointF(this.Point_CCD01_01_中心基準座標_量測點.X, this.List_CCD01_02_PIN量測參數_量測點[0].Y - 150);

        //                    if (CCD01_02_PIN量測參數_間距上排PIN01到基準_OK)
        //                    {
        //                        DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD01_02_PIN量測參數_間距上排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
        //                            (float)((上排_p2.Y + 上排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
        //                        DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
        //                    }
        //                    else
        //                    {
        //                        DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD01_02_PIN量測參數_間距上排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
        //(float)((上排_p2.Y + 上排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
        //                        DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Red, 1, g, ZoomX, ZoomY);
        //                    }

        //                    下排_p2 = new PointF(this.List_CCD01_02_PIN量測參數_量測點[10].X, this.List_CCD01_02_PIN量測參數_量測點[10].Y + 150);
        //                    下排_p3 = new PointF(this.Point_CCD01_01_中心基準座標_量測點.X, this.List_CCD01_02_PIN量測參數_量測點[10].Y + 150);

        //                    if (CCD01_02_PIN量測參數_間距下排PIN01到基準_OK)
        //                    {
        //                        DrawingClass.Draw.文字中心繪製(string.Format("下PIN01到基準" + "{0}", (CCD01_02_PIN量測參數_間距下排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((下排_p2.X + 下排_p3.X) / 2),
        //                            (float)((下排_p2.Y + 下排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
        //                        DrawingClass.Draw.線段繪製(下排_p2, 下排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
        //                    }
        //                    else
        //                    {
        //                        DrawingClass.Draw.文字中心繪製(string.Format("下PIN01到基準" + "{0}", (CCD01_02_PIN量測參數_間距下排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((下排_p2.X + 下排_p3.X) / 2),
        //(float)((下排_p2.Y + 下排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
        //                        DrawingClass.Draw.線段繪製(下排_p2, 下排_p3, Color.Red, 1, g, ZoomX, ZoomY);
        //                    }
        //                    #endregion
        //                    #region 水平度顯示
        //                    DrawingClass.Draw.水平線段繪製(0, 10000, CCD01_01_水平基準線量測_AxLineMsr.MeasuredSlope, CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotX,
        //                        CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD01_01_基準線量測_基準線偏移.Value, Color.Yellow, 2, g, ZoomX, ZoomY);

        //                    for (int i = 0; i < 20; i++)
        //                    {
        //                        point = new PointF(this.List_CCD01_02_PIN量測參數_量測點[i].X, this.List_CCD01_02_PIN量測參數_量測點[i].Y);

        //                        to_line_point = new PointF((float)this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_02_PIN量測參數_水平度量測顯示點_Y[i]) + this.PLC_Device_CCD01_01_基準線量測_基準線偏移.Value);

        //                        水平度 = List_CCD01_02_PIN量測參數_水平度量測距離[i];


        //                        if (List_CCD01_02_PIN量測參數_水平度量測_OK[i])
        //                        {
        //                            DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
        //                                new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體",10), Color.Black, Color.Yellow, g, ZoomX, ZoomY);

        //                            DrawingClass.Draw.線段繪製(point, to_line_point, Color.Yellow, 1, g, ZoomX, ZoomY);

        //                        }
        //                        else
        //                        {
        //                            DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
        //                                new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體",10), Color.Black, Color.Red, g, ZoomX, ZoomY);

        //                            DrawingClass.Draw.線段繪製(point, to_line_point, Color.Red, 1, g, ZoomX, ZoomY);

        //                        }


        //                    }



        //                    #endregion
        //                    #region 結果顯示
        //                    for (int i = 0; i < 19; i++)
        //                    {
        //                        if (i != 9)
        //                        {
        //                            if (List_CCD01_02_PIN量測參數_左右間距量測_OK[i] && CCD01_02_PIN量測參數_間距上排PIN01到基準_OK)
        //                            {
        //                                DrawingClass.Draw.文字左上繪製("間距量測OK!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);

        //                            }
        //                            else
        //                            {
        //                                DrawingClass.Draw.文字左上繪製("間距量測NG!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
        //                            }
        //                        }
        //                    }
        //                    for (int i = 0; i < 20; i++)
        //                    {
        //                        if (List_CCD01_02_PIN量測參數_水平度量測_OK[i])
        //                        {
        //                            DrawingClass.Draw.文字左上繪製("水平度量測OK!", new PointF(0, 200), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);

        //                        }
        //                        else
        //                        {
        //                            DrawingClass.Draw.文字左上繪製("水平度量測NG!", new PointF(0, 200), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
        //                        }
        //                    }
        //                    #endregion
        //                    g.Dispose();
        //                    g = null;
        //                }
        //                catch
        //                {

        //                }

        //            }
        //            else if(PLC_Device_CCD01_02_Tech_檢驗一次.Bool || PLC_Device_CCD01_02_Tech_取像並檢驗.Bool)
        //            {
        //                if (this.PLC_Device_CCD01_02_PIN量測_檢測距離計算_RefreshCanvas.Bool)
        //                {
        //                    try
        //                    {
        //                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
        //                        DrawingClass.Draw.十字中心(new PointF(this.Point_CCD01_01_中心基準座標_量測點.X, this.Point_CCD01_01_中心基準座標_量測點.Y), 100, Color.Red, 2, g, ZoomX, ZoomY);
        //                        #region 左右間距顯示
        //                        for (int i = 0; i < 19; i++)
        //                        {
        //                            p0 = new PointF(this.List_CCD01_02_PIN量測參數_量測點[i].X, this.List_CCD01_02_PIN量測參數_量測點[i].Y);
        //                            p1 = new PointF(this.List_CCD01_02_PIN量測參數_量測點[i + 1].X, this.List_CCD01_02_PIN量測參數_量測點[i + 1].Y);
        //                            間距 = List_CCD01_02_PIN量測參數_左右間距量測距離[i];

        //                            if (i != 9)
        //                            {
        //                                if (List_CCD01_02_PIN量測參數_左右間距量測_OK[i])
        //                                {
        //                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (間距 / 1D).ToString("0.000")), new PointF((float)((p0.X + p1.X) / 2),
        //                                        (float)((p0.Y + p1.Y) / 2) + 150 * ZoomY), new Font("標楷體",10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
        //                                    DrawingClass.Draw.線段繪製(p0, p1, Color.Lime, 1, g, ZoomX, ZoomY);

        //                                }
        //                                else
        //                                {
        //                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (間距 / 1D).ToString("0.000")), new PointF((float)((p0.X + p1.X) / 2),
        //                                        (float)((p0.Y + p1.Y) / 2) + 150 * ZoomY), new Font("標楷體",10), Color.Black, Color.Red, g, ZoomX, ZoomY);
        //                                    DrawingClass.Draw.線段繪製(p0, p1, Color.Red, 1, g, ZoomX, ZoomY);

        //                                }
        //                            }

        //                        }

        //                        上排_p2 = new PointF(this.List_CCD01_02_PIN量測參數_量測點[0].X, this.List_CCD01_02_PIN量測參數_量測點[0].Y - 150);
        //                        上排_p3 = new PointF(this.Point_CCD01_01_中心基準座標_量測點.X, this.List_CCD01_02_PIN量測參數_量測點[0].Y - 150);

        //                        if (CCD01_02_PIN量測參數_間距上排PIN01到基準_OK)
        //                        {
        //                            DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD01_02_PIN量測參數_間距上排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
        //                                (float)((上排_p2.Y + 上排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
        //                            DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
        //                        }
        //                        else
        //                        {
        //                            DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD01_02_PIN量測參數_間距上排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
        //    (float)((上排_p2.Y + 上排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
        //                            DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Red, 1, g, ZoomX, ZoomY);
        //                        }

        //                        下排_p2 = new PointF(this.List_CCD01_02_PIN量測參數_量測點[10].X, this.List_CCD01_02_PIN量測參數_量測點[10].Y + 150);
        //                        下排_p3 = new PointF(this.Point_CCD01_01_中心基準座標_量測點.X, this.List_CCD01_02_PIN量測參數_量測點[10].Y + 150);

        //                        if (CCD01_02_PIN量測參數_間距下排PIN01到基準_OK)
        //                        {
        //                            DrawingClass.Draw.文字中心繪製(string.Format("下PIN01到基準" + "{0}", (CCD01_02_PIN量測參數_間距下排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((下排_p2.X + 下排_p3.X) / 2),
        //                                (float)((下排_p2.Y + 下排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
        //                            DrawingClass.Draw.線段繪製(下排_p2, 下排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
        //                        }
        //                        else
        //                        {
        //                            DrawingClass.Draw.文字中心繪製(string.Format("下PIN01到基準" + "{0}", (CCD01_02_PIN量測參數_間距下排PIN01到基準距離 / 1D).ToString("0.000")), new PointF((float)((下排_p2.X + 下排_p3.X) / 2),
        //    (float)((下排_p2.Y + 下排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
        //                            DrawingClass.Draw.線段繪製(下排_p2, 下排_p3, Color.Red, 1, g, ZoomX, ZoomY);
        //                        }
        //                        #endregion
        //                        #region 水平度顯示
        //                        DrawingClass.Draw.水平線段繪製(0, 10000, CCD01_01_水平基準線量測_AxLineMsr.MeasuredSlope, CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotX,
        //                            CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD01_01_基準線量測_基準線偏移.Value, Color.Yellow, 2, g, ZoomX, ZoomY);

        //                        for (int i = 0; i < 20; i++)
        //                        {
        //                            point = new PointF(this.List_CCD01_02_PIN量測參數_量測點[i].X, this.List_CCD01_02_PIN量測參數_量測點[i].Y);

        //                            to_line_point = new PointF((float)this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_02_PIN量測參數_水平度量測顯示點_Y[i]) + this.PLC_Device_CCD01_01_基準線量測_基準線偏移.Value);

        //                            水平度 = List_CCD01_02_PIN量測參數_水平度量測距離[i];


        //                            if (List_CCD01_02_PIN量測參數_水平度量測_OK[i])
        //                            {
        //                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
        //                                    new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體",10), Color.Black, Color.Yellow, g, ZoomX, ZoomY);

        //                                DrawingClass.Draw.線段繪製(point, to_line_point, Color.Yellow, 1, g, ZoomX, ZoomY);

        //                            }
        //                            else
        //                            {
        //                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
        //                                    new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體",10), Color.Black, Color.Red, g, ZoomX, ZoomY);

        //                                DrawingClass.Draw.線段繪製(point, to_line_point, Color.Red, 1, g, ZoomX, ZoomY);

        //                            }


        //                        }



        //                        #endregion
        //                        #region 結果顯示
        //                        for (int i = 0; i < 19; i++)
        //                        {
        //                            if (i != 9)
        //                            {
        //                                if (List_CCD01_02_PIN量測參數_左右間距量測_OK[i] && CCD01_02_PIN量測參數_間距上排PIN01到基準_OK && CCD01_02_PIN量測參數_間距下排PIN01到基準_OK)
        //                                {
        //                                    DrawingClass.Draw.文字左上繪製("間距量測OK!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);

        //                                }
        //                                else
        //                                {
        //                                    DrawingClass.Draw.文字左上繪製("間距量測NG!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
        //                                }
        //                            }
        //                        }
        //                        for (int i = 0; i < 20; i++)
        //                        {
        //                            if (List_CCD01_02_PIN量測參數_水平度量測_OK[i])
        //                            {
        //                                DrawingClass.Draw.文字左上繪製("水平度量測OK!", new PointF(0, 200), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);

        //                            }
        //                            else
        //                            {
        //                                DrawingClass.Draw.文字左上繪製("水平度量測NG!", new PointF(0, 200), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
        //                            }
        //                        }
        //                        #endregion
        //                        g.Dispose();
        //                        g = null;
        //                    }
        //                    catch
        //                    {

        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (this.PLC_Device_CCD01_02_PIN量測_檢測距離計算_RefreshCanvas.Bool && PLC_Device_CCD01_02_PIN量測_檢測距離計算.Bool)
        //                {
        //                    try
        //                    {
        //                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
        //                        Font font = new Font("微軟正黑體", 10);

        //                        DrawingClass.Draw.十字中心(new PointF(this.Point_CCD01_01_中心基準座標_量測點.X, this.Point_CCD01_01_中心基準座標_量測點.Y), 100, Color.Red, 2, g, ZoomX, ZoomY);
        //                        #region 左右間距顯示
        //                        for (int i = 0; i < 19; i++)
        //                        {
        //                            p0 = new PointF(this.List_CCD01_02_PIN量測參數_量測點[i].X, this.List_CCD01_02_PIN量測參數_量測點[i].Y);
        //                            p1 = new PointF(this.List_CCD01_02_PIN量測參數_量測點[i + 1].X, this.List_CCD01_02_PIN量測參數_量測點[i + 1].Y);
        //                            間距 = List_CCD01_02_PIN量測參數_左右間距量測距離[i];

        //                            if (i != 9)
        //                            {
        //                                if (List_CCD01_02_PIN量測參數_左右間距量測_OK[i])
        //                                {
        //                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (間距 / 1D).ToString("0.000")), new PointF((float)((p0.X + p1.X) / 2),
        //                                        (float)((p0.Y + p1.Y) / 2) + 150 * ZoomY), new Font("標楷體",10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
        //                                    DrawingClass.Draw.線段繪製(p0, p1, Color.Lime, 1, g, ZoomX, ZoomY);

        //                                }
        //                                else
        //                                {
        //                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (間距 / 1D).ToString("0.000")), new PointF((float)((p0.X + p1.X) / 2),
        //                                        (float)((p0.Y + p1.Y) / 2) + 150 * ZoomY), new Font("標楷體",10), Color.Black, Color.Red, g, ZoomX, ZoomY);
        //                                    DrawingClass.Draw.線段繪製(p0, p1, Color.Red, 1, g, ZoomX, ZoomY);

        //                                }
        //                            }

        //                        }

        //                        #endregion
        //                        #region 水平度顯示
        //                        //DrawingClass.Draw.水平線段繪製(0, 10000, CCD01_01_水平基準線量測_AxLineMsr.MeasuredSlope, CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotX,
        //                        //    CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD01_01_基準線量測_基準線偏移.Value, Color.Yellow, 2, g, ZoomX, ZoomY);

        //                        //for (int i = 0; i < 20; i++)
        //                        //{
        //                        //    point = new PointF(this.List_CCD01_02_PIN量測參數_量測點[i].X, this.List_CCD01_02_PIN量測參數_量測點[i].Y);

        //                        //    to_line_point = new PointF((float)this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_02_PIN量測參數_水平度量測顯示點_Y[i]) + this.PLC_Device_CCD01_01_基準線量測_基準線偏移.Value);

        //                        //    水平度 = List_CCD01_02_PIN量測參數_水平度量測距離[i];


        //                        //    if (List_CCD01_02_PIN量測參數_水平度量測_OK[i])
        //                        //    {
        //                        //        DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
        //                        //            new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體",10), Color.Black, Color.Yellow, g, ZoomX, ZoomY);

        //                        //        DrawingClass.Draw.線段繪製(point, to_line_point, Color.Yellow, 1, g, ZoomX, ZoomY);

        //                        //    }
        //                        //    else
        //                        //    {
        //                        //        DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
        //                        //            new PointF(point.X, point.Y + 250 * ZoomY), new Font("標楷體",10), Color.Black, Color.Red, g, ZoomX, ZoomY);

        //                        //        DrawingClass.Draw.線段繪製(point, to_line_point, Color.Red, 1, g, ZoomX, ZoomY);

        //                        //    }


        //                        //}



        //                        #endregion
        //                        #region 結果顯示

        //                        for (int i = 0; i < 19; i++)
        //                        {
        //                            if (i != 9)
        //                            {
        //                                if (List_CCD01_02_PIN量測參數_左右間距量測_OK[i])
        //                                {
        //                                    DrawingClass.Draw.文字左上繪製("間距量測OK!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);

        //                                }
        //                                else
        //                                {
        //                                    DrawingClass.Draw.文字左上繪製("間距量測NG!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
        //                                }
        //                            }
        //                        }
        //                        //for (int i = 0; i < 20; i++)
        //                        //{
        //                        //    if (List_CCD01_02_PIN量測參數_水平度量測_OK[i])
        //                        //    {
        //                        //        DrawingClass.Draw.文字左上繪製("水平度量測OK!", new PointF(0, 200), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);

        //                        //    }
        //                        //    else
        //                        //    {
        //                        //        DrawingClass.Draw.文字左上繪製("水平度量測NG!", new PointF(0, 200), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
        //                        //    }
        //                        //}
        //                        #endregion
        //                        g.Dispose();
        //                        g = null;
        //                    }
        //                    catch
        //                    {

        //                    }
        //                }

        //            }

        //            this.PLC_Device_CCD01_02_PIN量測_檢測距離計算_RefreshCanvas.Bool = false;
        //        }

        //        int cnt_Program_CCD01_02_PIN量測_檢測距離計算 = 65534;
        //        void sub_Program_CCD01_02_PIN量測_檢測距離計算()
        //        {
        //            if (cnt_Program_CCD01_02_PIN量測_檢測距離計算 == 65534)
        //            {
        //                this.h_Canvas_Tech_CCD01_02.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_02_PIN間距量測_OnCanvasDrawEvent;
        //                this.h_Canvas_Main_CCD01_02_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_02_PIN間距量測_OnCanvasDrawEvent;
        //                PLC_Device_CCD01_02_PIN量測_檢測距離計算.SetComment("PLC_CCD01_02_PIN量測_檢測距離計算");
        //                PLC_Device_CCD01_02_PIN量測_檢測距離計算.Bool = false;
        //                PLC_Device_CCD01_02_PIN量測_檢測距離計算按鈕.Bool = false;
        //                cnt_Program_CCD01_02_PIN量測_檢測距離計算 = 65535;

        //            }
        //            if (cnt_Program_CCD01_02_PIN量測_檢測距離計算 == 65535) cnt_Program_CCD01_02_PIN量測_檢測距離計算 = 1;
        //            if (cnt_Program_CCD01_02_PIN量測_檢測距離計算 == 1) cnt_Program_CCD01_02_PIN量測_檢測距離計算_觸發按下(ref cnt_Program_CCD01_02_PIN量測_檢測距離計算);
        //            if (cnt_Program_CCD01_02_PIN量測_檢測距離計算 == 2) cnt_Program_CCD01_02_PIN量測_檢測距離計算_檢查按下(ref cnt_Program_CCD01_02_PIN量測_檢測距離計算);
        //            if (cnt_Program_CCD01_02_PIN量測_檢測距離計算 == 3) cnt_Program_CCD01_02_PIN量測_檢測距離計算_初始化(ref cnt_Program_CCD01_02_PIN量測_檢測距離計算);
        //            if (cnt_Program_CCD01_02_PIN量測_檢測距離計算 == 4) cnt_Program_CCD01_02_PIN量測_檢測距離計算_數值計算(ref cnt_Program_CCD01_02_PIN量測_檢測距離計算);
        //            if (cnt_Program_CCD01_02_PIN量測_檢測距離計算 == 5) cnt_Program_CCD01_02_PIN量測_檢測距離計算_量測結果(ref cnt_Program_CCD01_02_PIN量測_檢測距離計算);
        //            if (cnt_Program_CCD01_02_PIN量測_檢測距離計算 == 6) cnt_Program_CCD01_02_PIN量測_檢測距離計算_繪製畫布(ref cnt_Program_CCD01_02_PIN量測_檢測距離計算);
        //            if (cnt_Program_CCD01_02_PIN量測_檢測距離計算 == 7) cnt_Program_CCD01_02_PIN量測_檢測距離計算 = 65500;
        //            if (cnt_Program_CCD01_02_PIN量測_檢測距離計算 > 1) cnt_Program_CCD01_02_PIN量測_檢測距離計算_檢查放開(ref cnt_Program_CCD01_02_PIN量測_檢測距離計算);

        //            if (cnt_Program_CCD01_02_PIN量測_檢測距離計算 == 65500)
        //            {
        //                PLC_Device_CCD01_02_PIN量測_檢測距離計算.Bool = false;
        //                PLC_Device_CCD01_02_PIN量測_檢測距離計算按鈕.Bool = false;
        //                cnt_Program_CCD01_02_PIN量測_檢測距離計算 = 65535;
        //            }
        //        }
        //        void cnt_Program_CCD01_02_PIN量測_檢測距離計算_觸發按下(ref int cnt)
        //        {
        //            if (PLC_Device_CCD01_02_PIN量測_檢測距離計算按鈕.Bool || PLC_Device_CCD01_02_計算一次.Bool)
        //            {
        //                PLC_Device_CCD01_02_PIN量測_檢測距離計算.Bool = true;
        //                cnt++;
        //            }

        //        }
        //        void cnt_Program_CCD01_02_PIN量測_檢測距離計算_檢查按下(ref int cnt)
        //        {
        //            if (PLC_Device_CCD01_02_PIN量測_檢測距離計算.Bool)
        //            {
        //                cnt++;
        //            }

        //        }
        //        void cnt_Program_CCD01_02_PIN量測_檢測距離計算_檢查放開(ref int cnt)
        //        {
        //            //if (!PLC_Device_CCD01_02_PIN量測_檢測距離計算.Bool) cnt = 65500;
        //        }
        //        void cnt_Program_CCD01_02_PIN量測_檢測距離計算_初始化(ref int cnt)
        //        {
        //            this.MyTimer_CCD01_02_PIN量測_檢測距離計算.TickStop();
        //            this.MyTimer_CCD01_02_PIN量測_檢測距離計算.StartTickTime(99999);

        //            this.List_CCD01_02_PIN量測參數_左右間距量測距離 = new double[19];
        //            this.List_CCD01_02_PIN量測參數_上下間距量測距離 = new double[19];
        //            this.List_CCD01_02_PIN量測參數_水平度量測距離 = new double[20];
        //            this.CCD01_02_PIN量測參數_間距上排PIN01到基準距離 = new double();
        //            this.CCD01_02_PIN量測參數_間距下排PIN01到基準距離 = new double();

        //            this.List_CCD01_02_PIN量測參數_量測點_OK = new bool[20];
        //            this.List_CCD01_02_PIN量測參數_左右間距量測_OK = new bool[19];
        //            this.List_CCD01_02_PIN量測參數_上下間距量測_OK = new bool[19];
        //            this.List_CCD01_02_PIN量測參數_水平度量測_OK = new bool[20];
        //            this.CCD01_02_PIN量測參數_間距上排PIN01到基準_OK = new bool();
        //            this.CCD01_02_PIN量測參數_間距下排PIN01到基準_OK = new bool();




        //            cnt++;
        //        }
        //        void cnt_Program_CCD01_02_PIN量測_檢測距離計算_數值計算(ref int cnt)
        //        {
        //            #region 水平度數值計算
        //            //this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測.LinePivotX = this.CCD01_01_水平基準線量測_AxLineRegression.FittedPivotX;
        //            //this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測.LinePivotY = this.CCD01_01_水平基準線量測_AxLineRegression.FittedPivotY;
        //            //this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測.LineHorzVert = AxOvkMsr.TxAxLineHorzVert.AX_LINE_QUASI_HORIZONTAL;
        //            //this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測.LineSlope = this.CCD01_01_水平基準線量測_AxLineRegression.FittedSlope;
        //            //for (int i = 0; i < this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整.Count; i++)
        //            //{
        //            //        this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測.PivotX = this.List_CCD01_02_PIN量測參數_量測點[i].X;
        //            //        this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測.PivotY = this.List_CCD01_02_PIN量測參數_量測點[i].Y;
        //            //        this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測.FindDistance();
        //            //        this.List_CCD01_02_PIN量測參數_水平度量測顯示點_X[i] = CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測.ProjectPivotX;
        //            //        this.List_CCD01_02_PIN量測參數_水平度量測顯示點_Y[i] = CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測.ProjectPivotY;
        //            //        this.List_CCD01_02_PIN量測參數_水平度量測距離[i] = this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測.Distance * this.CCD01_比例尺_pixcel_To_mm;

        //            //}
        //            #endregion
        //            #region 左右間距數值計算
        //            double distance = 0;
        //            double 間距Temp1_X = 0;
        //            double 間距Temp2_X = 0;

        //            for (int i = 0; i < 19; i++)
        //            {
        //                if (this.List_CCD01_02_PIN量測參數_量測點_有無[i] && this.List_CCD01_02_PIN量測參數_量測點_有無[i + 1])
        //                {

        //                    間距Temp1_X = this.List_CCD01_02_PIN量測參數_量測點[i].X - this.Point_CCD01_01_中心基準座標_量測點.X;
        //                    間距Temp2_X = this.List_CCD01_02_PIN量測參數_量測點[i + 1].X - this.Point_CCD01_01_中心基準座標_量測點.X;

        //                    distance = Math.Abs(間距Temp1_X - 間距Temp2_X);

        //                    this.List_CCD01_02_PIN量測參數_左右間距量測距離[i] = distance * this.CCD01_比例尺_pixcel_To_mm;
        //                }
        //                else
        //                {
        //                    PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = false;
        //                    List_CCD01_02_PIN量測參數_量測點_OK[i] = false;
        //                }

        //            }
        //            #endregion
        //            cnt++;
        //        }
        //        void cnt_Program_CCD01_02_PIN量測_檢測距離計算_量測結果(ref int cnt)
        //        {

        //            PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = true; // 檢測結果初始化
        //            #region 左右間距量測判斷

        //            for (int i = 0; i < 19; i++)
        //            {
        //                int 標準值 = this.PLC_Device_CCD01_02_PIN量測_左右間距量測標準值.Value;
        //                int 標準值上限 = this.PLC_Device_CCD01_02_PIN量測_左右間距量測上限值.Value;
        //                int 標準值下限 = this.PLC_Device_CCD01_02_PIN量測_左右間距量測下限值.Value;
        //                double 量測距離 = this.List_CCD01_02_PIN量測參數_左右間距量測距離[i];

        //                量測距離 = 量測距離 * 1000 - 標準值;
        //                量測距離 /= 1000;
        //                if (!PLC_Device_CCD01_02_PIN量測_間距量測不測試.Bool)
        //                {
        //                    if (this.List_CCD01_02_PIN量測參數_量測點_有無[i])
        //                    {
        //                        if (量測距離 >= 0 && i != 9)
        //                        {
        //                            if (標準值上限 <= Math.Abs(量測距離) * 1000 || 標準值下限 > Math.Abs(量測距離) * 1000)
        //                            {
        //                                this.List_CCD01_02_PIN量測參數_左右間距量測_OK[i] = false;
        //                                PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = false;
        //                            }
        //                            else
        //                            {
        //                                this.List_CCD01_02_PIN量測參數_左右間距量測_OK[i] = true;
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    this.List_CCD01_02_PIN量測參數_左右間距量測_OK[i] = true;
        //                }



        //                this.List_CCD01_02_PIN量測參數_左右間距量測距離[i] = 量測距離;

        //            }
        //            #endregion
        //            #region 水平度量測判斷
        //            //for (int i = 0; i < 20; i++)
        //            //{
        //            //    int 上排標準值 = this.PLC_Device_CCD01_02_PIN量測_上排水平度量測標準值.Value;
        //            //    int 上排標準值上限 = this.PLC_Device_CCD01_02_PIN量測_上排水平度量測上限值.Value;
        //            //    int 上排標準值下限 = this.PLC_Device_CCD01_02_PIN量測_上排水平度量測下限值.Value;
        //            //    double 上排量測距離 = this.List_CCD01_02_PIN量測參數_水平度量測距離[i];

        //            //    int 下排標準值 = this.PLC_Device_CCD01_02_PIN量測_下排水平度量測標準值.Value;
        //            //    int 下排標準值上限 = this.PLC_Device_CCD01_02_PIN量測_下排水平度量測上限值.Value;
        //            //    int 下排標準值下限 = this.PLC_Device_CCD01_02_PIN量測_下排水平度量測下限值.Value;
        //            //    double 下排量測距離 = this.List_CCD01_02_PIN量測參數_水平度量測距離[i];

        //            //    上排量測距離 = 上排量測距離 * 1000 - 上排標準值;
        //            //    上排量測距離 /= 1000;

        //            //    下排量測距離 = 下排量測距離 * 1000 - 下排標準值;
        //            //    下排量測距離 /= 1000;
        //            //    if (!PLC_Device_CCD01_02_PIN量測_水平度量測不測試.Bool)
        //            //    {
        //            //        if (this.List_CCD01_02_PIN量測參數_量測點_有無[i])
        //            //        {
        //            //            if (上排量測距離 >= 0 && i < 10)
        //            //            {
        //            //                if (上排標準值上限 <= Math.Abs(上排量測距離) * 1000 || 上排標準值下限 > Math.Abs(上排量測距離) * 1000)
        //            //                {
        //            //                    this.List_CCD01_02_PIN量測參數_水平度量測_OK[i] = false;
        //            //                    PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = false;
        //            //                }
        //            //                else
        //            //                {
        //            //                    this.List_CCD01_02_PIN量測參數_水平度量測_OK[i] = true;
        //            //                }
        //            //                this.List_CCD01_02_PIN量測參數_水平度量測距離[i] = 上排量測距離;
        //            //            }
        //            //            else if (下排量測距離 >= 0 && i >= 10)
        //            //            {
        //            //                if (下排標準值上限 <= Math.Abs(下排量測距離) * 1000 || 下排標準值下限 > Math.Abs(下排量測距離) * 1000)
        //            //                {
        //            //                    this.List_CCD01_02_PIN量測參數_水平度量測_OK[i] = false;
        //            //                    PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = false;
        //            //                }
        //            //                else
        //            //                {
        //            //                    this.List_CCD01_02_PIN量測參數_水平度量測_OK[i] = true;
        //            //                }
        //            //                this.List_CCD01_02_PIN量測參數_水平度量測距離[i] = 下排量測距離;
        //            //            }

        //            //        }
        //            //    }
        //            //    else
        //            //    {
        //            //        this.List_CCD01_02_PIN量測參數_水平度量測_OK[i] = true;
        //            //    }
        //            //    if (PLC_Device_CCD01_02_PIN量測_間距量測不測試.Bool && PLC_Device_CCD01_02_PIN量測_水平度量測不測試.Bool)
        //            //    {
        //            //        PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = true;
        //            //    }



        //            //}
        //            #endregion
        //            #region 間距上排PIN01到基準線量測

        //            //double temp_上排PIN01到基準 = 0;
        //            //int 間距上排PIN01到基準標準值 = this.PLC_Device_CCD01_02_PIN量測_間距上排PIN01到基準數值.Value;
        //            //int 間距上排PIN01到基準標準值上限 = this.PLC_Device_CCD01_02_PIN量測_間距上排PIN01到基準上限.Value;
        //            //int 間距上排PIN01到基準標準值下限 = this.PLC_Device_CCD01_02_PIN量測_間距上排PIN01到基準下限.Value;


        //            //if (this.List_CCD01_02_PIN量測參數_量測點_有無[0])
        //            //{
        //            //    temp_上排PIN01到基準 = Math.Abs(this.List_CCD01_02_PIN量測參數_量測點[0].X - this.Point_CCD01_01_中心基準座標_量測點.X);
        //            //    this.CCD01_02_PIN量測參數_間距上排PIN01到基準距離 = temp_上排PIN01到基準 * this.CCD01_比例尺_pixcel_To_mm;
        //            //}
        //            //else
        //            //{
        //            //    PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = false;
        //            //    CCD01_02_PIN量測參數_間距上排PIN01到基準_OK = false;
        //            //}
        //            //double 間距上排PIN01到基準量測距離 = this.CCD01_02_PIN量測參數_間距上排PIN01到基準距離;


        //            //間距上排PIN01到基準量測距離 = 間距上排PIN01到基準量測距離 * 1000 - 間距上排PIN01到基準標準值;
        //            //間距上排PIN01到基準量測距離 /= 1000;

        //            //if (!PLC_Device_CCD01_02_PIN量測_間距量測不測試.Bool)
        //            //{
        //            //    if (this.List_CCD01_02_PIN量測參數_量測點_有無[0])
        //            //    {
        //            //        if (間距上排PIN01到基準標準值上限 <= Math.Abs(間距上排PIN01到基準量測距離) * 1000 || 間距上排PIN01到基準標準值下限 >
        //            //            Math.Abs(間距上排PIN01到基準量測距離) * 1000)
        //            //        {
        //            //            this.CCD01_02_PIN量測參數_間距上排PIN01到基準_OK = false;
        //            //            PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = false;
        //            //        }
        //            //        else
        //            //        {
        //            //            this.CCD01_02_PIN量測參數_間距上排PIN01到基準_OK = true;
        //            //        }

        //            //    }
        //            //    CCD01_02_PIN量測參數_間距上排PIN01到基準距離 = 間距上排PIN01到基準量測距離;
        //            //}
        //            //else
        //            //{
        //            //    this.CCD01_02_PIN量測參數_間距上排PIN01到基準_OK = true;
        //            //    this.PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = true;
        //            //}

        //            #endregion
        //            #region 間距下排PIN01到基準線量測

        //            //double temp_下排PIN01到基準 = 0;
        //            //int 間距下排PIN01到基準標準值 = this.PLC_Device_CCD01_02_PIN量測_間距下排PIN01到基準數值.Value;
        //            //int 間距下排PIN01到基準標準值上限 = this.PLC_Device_CCD01_02_PIN量測_間距下排PIN01到基準上限.Value;
        //            //int 間距下排PIN01到基準標準值下限 = this.PLC_Device_CCD01_02_PIN量測_間距下排PIN01到基準下限.Value;


        //            //if (this.List_CCD01_02_PIN量測參數_量測點_有無[10])
        //            //{
        //            //    temp_下排PIN01到基準 = Math.Abs(this.List_CCD01_02_PIN量測參數_量測點[10].X - this.Point_CCD01_01_中心基準座標_量測點.X);
        //            //    this.CCD01_02_PIN量測參數_間距下排PIN01到基準距離 = temp_下排PIN01到基準 * this.CCD01_比例尺_pixcel_To_mm;
        //            //}
        //            //else
        //            //{
        //            //    PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = false;
        //            //    CCD01_02_PIN量測參數_間距下排PIN01到基準_OK = false;
        //            //}
        //            //double 間距下排PIN01到基準量測距離 = this.CCD01_02_PIN量測參數_間距下排PIN01到基準距離;


        //            //間距下排PIN01到基準量測距離 = 間距下排PIN01到基準量測距離 * 1000 - 間距下排PIN01到基準標準值;
        //            //間距下排PIN01到基準量測距離 /= 1000;

        //            //if (!PLC_Device_CCD01_02_PIN量測_間距量測不測試.Bool)
        //            //{
        //            //    if (this.List_CCD01_02_PIN量測參數_量測點_有無[10])
        //            //    {
        //            //        if (間距下排PIN01到基準標準值上限 <= Math.Abs(間距下排PIN01到基準量測距離) * 1000 || 間距下排PIN01到基準標準值下限 >
        //            //            Math.Abs(間距下排PIN01到基準量測距離) * 1000)
        //            //        {
        //            //            this.CCD01_02_PIN量測參數_間距下排PIN01到基準_OK = false;
        //            //            PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = false;
        //            //        }
        //            //        else
        //            //        {
        //            //            this.CCD01_02_PIN量測參數_間距下排PIN01到基準_OK = true;
        //            //        }

        //            //    }
        //            //    CCD01_02_PIN量測參數_間距下排PIN01到基準距離 = 間距下排PIN01到基準量測距離;
        //            //}
        //            //else
        //            //{
        //            //    this.CCD01_02_PIN量測參數_間距下排PIN01到基準_OK = true;
        //            //    this.PLC_Device_CCD01_02_PIN量測_檢測距離計算_OK.Bool = true;
        //            //}

        //            #endregion
        //            cnt++;
        //        }
        //        void cnt_Program_CCD01_02_PIN量測_檢測距離計算_繪製畫布(ref int cnt)
        //        {
        //            this.PLC_Device_CCD01_02_PIN量測_檢測距離計算_RefreshCanvas.Bool = true;
        //            if (this.PLC_Device_CCD01_02_PIN量測_檢測距離計算按鈕.Bool && !PLC_Device_CCD01_02_計算一次.Bool)
        //            {

        //                this.h_Canvas_Tech_CCD01_02.RefreshCanvas();
        //            }
        //            cnt++;
        //        }
        //        #endregion
        //        #region PLC_CCD01_02_PIN正位度量測_設定規範位置
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_設定規範按鈕 = new PLC_Device("S6070");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_設定規範位置 = new PLC_Device("S6065");
        //        PLC_Device PLC_Device_CCD01_02_PIN設定規範位置_OK = new PLC_Device("S6066");
        //        PLC_Device PLC_Device_CCD01_02_PIN設定規範位置_測試完成 = new PLC_Device("S6067");
        //        PLC_Device PLC_Device_CCD01_02_PIN設定規範位置_RefreshCanvas = new PLC_Device("S6066");
        //        private List<PLC_Device> List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X = new List<PLC_Device>();
        //        private List<PLC_Device> List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y = new List<PLC_Device>();
        //        private AxOvkPat.AxVisionInspectionFrame CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整;

        //        #region 正位度規範值
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN01 = new PLC_Device("F11000");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN02 = new PLC_Device("F11001");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN03 = new PLC_Device("F11002");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN04 = new PLC_Device("F11003");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN05 = new PLC_Device("F11004");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN06 = new PLC_Device("F11005");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN07 = new PLC_Device("F11006");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN08 = new PLC_Device("F11007");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN09 = new PLC_Device("F11008");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN10 = new PLC_Device("F11009");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN11 = new PLC_Device("F11010");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN12 = new PLC_Device("F11011");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN13 = new PLC_Device("F11012");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN14 = new PLC_Device("F11013");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN15 = new PLC_Device("F11014");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN16 = new PLC_Device("F11015");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN17 = new PLC_Device("F11016");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN18 = new PLC_Device("F11017");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN19 = new PLC_Device("F11018");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN20 = new PLC_Device("F11019");

        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN01 = new PLC_Device("F11020");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN02 = new PLC_Device("F11021");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN03 = new PLC_Device("F11022");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN04 = new PLC_Device("F11023");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN05 = new PLC_Device("F11024");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN06 = new PLC_Device("F11025");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN07 = new PLC_Device("F11026");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN08 = new PLC_Device("F11027");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN09 = new PLC_Device("F11028");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN10 = new PLC_Device("F11029");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN11 = new PLC_Device("F11030");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN12 = new PLC_Device("F11031");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN13 = new PLC_Device("F11032");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN14 = new PLC_Device("F11033");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN15 = new PLC_Device("F11034");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN16 = new PLC_Device("F11035");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN17 = new PLC_Device("F11036");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN18 = new PLC_Device("F11037");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN19 = new PLC_Device("F11038");
        //        PLC_Device PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN20 = new PLC_Device("F11039");
        //        #endregion
        //        private PointF[] List_CCD01_02_PIN正位度量測參數_規範點 = new PointF[20];
        //        private PointF[] List_CCD01_02_PIN正位度量測參數_轉換後座標 = new PointF[20];
        //        private double[] List_CCD01_02_PIN正位度量測參數_正位度規範點_X = new double[20];
        //        private double[] List_CCD01_02_PIN正位度量測參數_正位度規範點_Y = new double[20];

        //        int cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 = 65534;

        //        private void H_Canvas_Tech_CCD01_02_PIN正位度設定規範位置_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        //        {

        //            if (PLC_Device_CCD01_02_Main_取像並檢驗.Bool || PLC_Device_CCD01_02_PLC觸發檢測.Bool)
        //            {
        //                try
        //                {
        //                    Graphics g = Graphics.FromHdc((IntPtr)HDC);
        //                    for (int i = 0; i < 20; i++)
        //                    {
        //                        DrawingClass.Draw.十字中心(new PointF((float)List_CCD01_02_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD01_02_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
        //                    }
        //                    g.Dispose();
        //                    g = null;
        //                }
        //                catch
        //                {

        //                }

        //            }

        //            else if(PLC_Device_CCD01_02_Tech_檢驗一次.Bool || PLC_Device_CCD01_02_Tech_取像並檢驗.Bool)
        //            {
        //                if (this.PLC_Device_CCD01_02_PIN設定規範位置_RefreshCanvas.Bool)
        //                {
        //                    try
        //                    {
        //                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
        //                        for (int i = 0; i < 20; i++)
        //                        {
        //                            DrawingClass.Draw.十字中心(new PointF((float)List_CCD01_02_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD01_02_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
        //                        }
        //                        g.Dispose();
        //                        g = null;
        //                    }
        //                    catch
        //                    {

        //                    }
        //                }
        //            }

        //            else
        //            {
        //                if (this.PLC_Device_CCD01_02_PIN設定規範位置_RefreshCanvas.Bool)
        //                {
        //                    try
        //                    {
        //                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
        //                        for (int i = 0; i < 20; i++)
        //                        {
        //                            DrawingClass.Draw.十字中心(new PointF((float)List_CCD01_02_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD01_02_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
        //                        }
        //                        g.Dispose();
        //                        g = null;
        //                    }
        //                    catch
        //                    {

        //                    }
        //                }
        //            }



        //            this.PLC_Device_CCD01_02_PIN設定規範位置_RefreshCanvas.Bool = false;
        //        }
        //        void sub_Program_CCD01_02_PIN正位度量測_設定規範位置()
        //        {
        //            if (cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 == 65534)
        //            {

        //                this.h_Canvas_Tech_CCD01_02.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_02_PIN正位度設定規範位置_OnCanvasDrawEvent;
        //                this.h_Canvas_Main_CCD01_02_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_02_PIN正位度設定規範位置_OnCanvasDrawEvent;

        //                PLC_Device_CCD01_02_PIN正位度量測_設定規範位置.SetComment("PLC_CCD01_02_PIN正位度量測_設定規範位置");
        //                PLC_Device_CCD01_02_PIN正位度量測_設定規範按鈕.Bool = false;
        //                PLC_Device_CCD01_02_PIN正位度量測_設定規範位置.Bool = false;
        //                cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 = 65535;
        //                #region 正位度規範值
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN01);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN02);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN03);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN04);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN05);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN06);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN07);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN08);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN09);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN10);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN11);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN12);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN13);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN14);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN15);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN16);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN17);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN18);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN19);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_X_PIN20);

        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN01);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN02);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN03);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN04);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN05);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN06);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN07);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN08);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN09);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN10);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN11);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN12);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN13);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN14);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN15);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN16);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN17);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN18);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN19);
        //                this.List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_02_PIN正位度量測_正位度規範值_Y_PIN20);
        //                #endregion
        //            }
        //            if (cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 == 65535) cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 = 1;
        //            if (cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 == 1) cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_觸發按下(ref cnt_Program_CCD01_02_PIN正位度量測_設定規範位置);
        //            if (cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 == 2) cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_檢查按下(ref cnt_Program_CCD01_02_PIN正位度量測_設定規範位置);
        //            if (cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 == 3) cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_初始化(ref cnt_Program_CCD01_02_PIN正位度量測_設定規範位置);
        //            if (cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 == 4) cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_座標轉換(ref cnt_Program_CCD01_02_PIN正位度量測_設定規範位置);
        //            if (cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 == 5) cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_讀取參數(ref cnt_Program_CCD01_02_PIN正位度量測_設定規範位置);
        //            if (cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 == 6) cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_繪製畫布(ref cnt_Program_CCD01_02_PIN正位度量測_設定規範位置);
        //            if (cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 == 7) cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 = 65500;
        //            if (cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 > 1) cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_檢查放開(ref cnt_Program_CCD01_02_PIN正位度量測_設定規範位置);

        //            if (cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 == 65500)
        //            {
        //                cnt_Program_CCD01_02_PIN正位度量測_設定規範位置 = 65535;
        //            }
        //        }
        //        void cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_觸發按下(ref int cnt)
        //        {
        //            if (PLC_Device_CCD01_02_PIN正位度量測_設定規範按鈕.Bool || PLC_Device_CCD01_02_計算一次.Bool)
        //            {
        //                PLC_Device_CCD01_02_PIN正位度量測_設定規範位置.Bool = true;
        //                cnt++;
        //            }
        //        }
        //        void cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_檢查按下(ref int cnt)
        //        {
        //            if (PLC_Device_CCD01_02_PIN正位度量測_設定規範位置.Bool)
        //            {
        //                cnt++;
        //            }
        //        }
        //        void cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_檢查放開(ref int cnt)
        //        {
        //            if (!PLC_Device_CCD01_02_PIN正位度量測_設定規範按鈕.Bool)
        //            {
        //                PLC_Device_CCD01_02_PIN正位度量測_設定規範位置.Bool = false;
        //                cnt = 65500;
        //            }
        //        }
        //        void cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_初始化(ref int cnt)
        //        {
        //            this.List_CCD01_02_PIN正位度量測參數_正位度規範點_X = new double[20];
        //            this.List_CCD01_02_PIN正位度量測參數_正位度規範點_Y = new double[20];
        //            this.List_CCD01_02_PIN正位度量測參數_規範點 = new PointF[20];
        //            this.List_CCD01_02_PIN正位度量測參數_轉換後座標 = new PointF[20];

        //            cnt++;
        //        }
        //        void cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_座標轉換(ref int cnt)
        //        {
        //            if (PLC_Device_CCD01_02_計算一次.Bool)
        //            {
        //                CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.RefPointX = PLC_Device_CCD01_01_水平基準線量測_量測中心_X.Value;
        //                CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.RefPointY = PLC_Device_CCD01_01_水平基準線量測_量測中心_Y.Value;
        //                CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.RefAngle = 0;
        //                CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.CurrentRefPointX = Point_CCD01_01_中心基準座標_量測點.X;
        //                CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.CurrentRefPointY = Point_CCD01_01_中心基準座標_量測點.Y;
        //                CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.CurrentRefAngle = 0;
        //                CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.NumOfVisionPoints = 20;

        //                for (int j = 0; j < 20; j++)
        //                {

        //                    CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
        //                    CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointX = (float)(List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X[j].Value);
        //                    CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointX = CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointX / 1;
        //                    CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointY = (float)(List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y[j].Value);
        //                    CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointY = CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointY / 1;

        //                }
        //                CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.EstimateCurrentVisionPoints();
        //                for (int j = 0; j < 20; j++)
        //                {

        //                    CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
        //                    List_CCD01_02_PIN正位度量測參數_轉換後座標[j].X = (int)CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.CurrentVisionPointX;
        //                    List_CCD01_02_PIN正位度量測參數_轉換後座標[j].Y = (int)CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.CurrentVisionPointY;

        //                }
        //            }
        //            cnt++;
        //        }
        //        void cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_讀取參數(ref int cnt)
        //        {

        //            for (int i = 0; i < 20; i++)
        //            {
        //                if (PLC_Device_CCD01_02_計算一次.Bool)
        //                {
        //                    this.List_CCD01_02_PIN正位度量測參數_正位度規範點_X[i] = List_CCD01_02_PIN正位度量測參數_轉換後座標[i].X;
        //                    this.List_CCD01_02_PIN正位度量測參數_正位度規範點_Y[i] = List_CCD01_02_PIN正位度量測參數_轉換後座標[i].Y;
        //                }
        //                else
        //                {
        //                    this.List_CCD01_02_PIN正位度量測參數_正位度規範點_X[i] = (float)(List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X[i].Value);
        //                    this.List_CCD01_02_PIN正位度量測參數_正位度規範點_X[i] = this.List_CCD01_02_PIN正位度量測參數_正位度規範點_X[i] / 1;
        //                    this.List_CCD01_02_PIN正位度量測參數_正位度規範點_Y[i] = (float)(List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y[i].Value);
        //                    this.List_CCD01_02_PIN正位度量測參數_正位度規範點_Y[i] = this.List_CCD01_02_PIN正位度量測參數_正位度規範點_Y[i] / 1;
        //                }
        //                List_CCD01_02_PIN正位度量測參數_規範點[i].X = (float)this.List_CCD01_02_PIN正位度量測參數_正位度規範點_X[i];
        //                List_CCD01_02_PIN正位度量測參數_規範點[i].Y = (float)this.List_CCD01_02_PIN正位度量測參數_正位度規範點_Y[i];

        //            }
        //            cnt++;
        //        }
        //        void cnt_Program_CCD01_02_PIN正位度量測_設定規範位置_繪製畫布(ref int cnt)
        //        {
        //            this.PLC_Device_CCD01_02_PIN設定規範位置_RefreshCanvas.Bool = true;
        //            if (this.PLC_Device_CCD01_02_PIN正位度量測_設定規範按鈕.Bool && !PLC_Device_CCD01_02_計算一次.Bool)
        //            {
        //                this.h_Canvas_Tech_CCD01_02.RefreshCanvas();
        //            }

        //            cnt++;
        //        }



        //        #endregion
        //        #region PLC_CCD01_02_PIN量測_檢測正位度計算

        //        MyTimer MyTimer_CCD01_02_PIN量測_檢測正位度計算 = new MyTimer();
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測正位度計算按鈕 = new PLC_Device("S6090");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測正位度計算 = new PLC_Device("S6085");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測正位度計算_OK = new PLC_Device("S6086");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測正位度計算_測試完成 = new PLC_Device("S6087");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測正位度計算_RefreshCanvas = new PLC_Device("S6088");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測正位度計算_不測試 = new PLC_Device("S6102");
        //        PLC_Device PLC_Device_CCD01_02_PIN量測_檢測正位度計算_差值 = new PLC_Device("F15000");

        //        private double[] List_CCD01_02_PIN正位度量測參數_正位度距離 = new double[20];
        //        private bool[] List_CCD01_02_PIN正位度量測參數_正位度量測點_OK = new bool[20];


        //        private void H_Canvas_Tech_CCD01_02_PIN量測正位度_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        //        {
        //            if (PLC_Device_CCD01_02_Main_取像並檢驗.Bool || PLC_Device_CCD01_02_PLC觸發檢測.Bool)
        //            {
        //                try
        //                {
        //                    Graphics g = Graphics.FromHdc((IntPtr)HDC);
        //                    Font font = new Font("微軟正黑體", 10);
        //                    string 正位度差值顯示;
        //                    for (int i = 0; i < 20; i++)
        //                    {
        //                        DrawingClass.Draw.十字中心(new PointF((float)List_CCD01_02_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD01_02_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
        //                        DrawingClass.Draw.十字中心(this.List_CCD01_02_PIN量測參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);

        //                    }
        //                    #region 正位度量測結果顯示
        //                    if (PLC_Device_CCD01_02_PIN量測_檢測正位度計算_OK.Bool)
        //                    {
        //                        DrawingClass.Draw.文字左上繪製("正位度數值OK:", new PointF(1500, 0), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
        //                    }
        //                    else
        //                    {
        //                        DrawingClass.Draw.文字左上繪製("正位度數值NG:", new PointF(1500, 0), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
        //                    }
        //                    #endregion
        //                    #region PIN正位結果顯示
        //                    for (int i = 0; i < 20; i++)
        //                    {

        //                            if (this.List_CCD01_02_PIN正位度量測參數_正位度量測點_OK[i])
        //                            {
        //                                if (i <= 9)
        //                                {
        //                                    正位度差值顯示 = ("上排:P" + (i + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
        //                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1900, i * 40), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
        //                                }

        //                                if (i >= 10)
        //                                {
        //                                    正位度差值顯示 = ("下排:P" + ((i - 10) + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
        //                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(2200, (i - 10) * 40), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);

        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (i <= 9)
        //                                {
        //                                    正位度差值顯示 = ("上排:P" + (i + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
        //                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1900, i * 40), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
        //                                }

        //                                if (i >= 10)
        //                                {
        //                                    正位度差值顯示 = ("下排:P" + ((i - 10) + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
        //                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(2200, (i - 10) * 40), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

        //                                }

        //                            }
        //                        }



        //                    #endregion

        //                    g.Dispose();
        //                    g = null;
        //                }
        //                catch
        //                {

        //                }

        //            }
        //            if (PLC_Device_CCD01_02_Tech_檢驗一次.Bool || PLC_Device_CCD01_02_Tech_取像並檢驗.Bool)
        //            {
        //                if (this.PLC_Device_CCD01_02_PIN量測_檢測正位度計算_RefreshCanvas.Bool)
        //                {
        //                    try
        //                    {
        //                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
        //                        Font font = new Font("微軟正黑體", 10);
        //                        string 正位度差值顯示;
        //                        for (int i = 0; i < 20; i++)
        //                        {
        //                            DrawingClass.Draw.十字中心(new PointF((float)List_CCD01_02_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD01_02_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
        //                            DrawingClass.Draw.十字中心(this.List_CCD01_02_PIN量測參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);

        //                        }
        //                        #region 正位度量測結果顯示
        //                        if (PLC_Device_CCD01_02_PIN量測_檢測正位度計算_OK.Bool)
        //                        {
        //                            DrawingClass.Draw.文字左上繪製("正位度數值OK:", new PointF(1500, 0), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
        //                        }
        //                        else
        //                        {
        //                            DrawingClass.Draw.文字左上繪製("正位度數值NG:", new PointF(1500, 0), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
        //                        }
        //                        #endregion
        //                        #region PIN正位結果顯示
        //                        for (int i = 0; i < 20; i++)
        //                        {

        //                            if (this.List_CCD01_02_PIN正位度量測參數_正位度量測點_OK[i])
        //                            {
        //                                if (i <= 9)
        //                                {
        //                                    正位度差值顯示 = ("上排:P" + (i + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
        //                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1900, i * 40), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
        //                                }

        //                                if (i >= 10)
        //                                {
        //                                    正位度差值顯示 = ("下排:P" + ((i - 10) + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
        //                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(2200, (i - 10) * 40), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);

        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (i <= 9)
        //                                {
        //                                    正位度差值顯示 = ("上排:P" + (i + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
        //                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1900, i * 40), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
        //                                }

        //                                if (i >= 10)
        //                                {
        //                                    正位度差值顯示 = ("下排:P" + ((i - 10) + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
        //                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(2200, (i - 10) * 40), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

        //                                }

        //                            }


        //                        }

        //                        #endregion

        //                        g.Dispose();
        //                        g = null;
        //                    }
        //                    catch
        //                    {

        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (this.PLC_Device_CCD01_02_PIN量測_檢測正位度計算_RefreshCanvas.Bool && PLC_Device_CCD01_02_PIN量測_檢測正位度計算.Bool)
        //                {
        //                    try
        //                    {
        //                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
        //                        Font font = new Font("微軟正黑體", 10);
        //                        string 正位度差值顯示;
        //                        for (int i = 0; i < 20; i++)
        //                        {
        //                            DrawingClass.Draw.十字中心(new PointF((float)List_CCD01_02_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD01_02_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
        //                            DrawingClass.Draw.十字中心(this.List_CCD01_02_PIN量測參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
        //                        }
        //                        #region 正位度量測結果顯示
        //                        if (PLC_Device_CCD01_02_PIN量測_檢測正位度計算_OK.Bool)
        //                        {
        //                            DrawingClass.Draw.文字左上繪製("正位度數值OK:", new PointF(1500, 0), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
        //                        }
        //                        else
        //                        {
        //                            DrawingClass.Draw.文字左上繪製("正位度數值NG:", new PointF(1500, 0), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
        //                        }
        //                        #endregion
        //                        #region PIN正位結果顯示
        //                        for (int i = 0; i < 20; i++)
        //                        {

        //                                if (this.List_CCD01_02_PIN正位度量測參數_正位度量測點_OK[i])
        //                                {
        //                                    if (i <= 9)
        //                                    {
        //                                        正位度差值顯示 = ("上排:P" + (i + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
        //                                        DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1900, i * 40), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
        //                                    }

        //                                    if (i >= 10)
        //                                    {
        //                                        正位度差值顯示 = ("下排:P" + ((i - 10) + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
        //                                        DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(2200, (i - 10) * 40), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);

        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    if (i <= 9)
        //                                    {
        //                                        正位度差值顯示 = ("上排:P" + (i + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
        //                                        DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1900, i * 40), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
        //                                    }

        //                                    if (i >= 10)
        //                                    {
        //                                        正位度差值顯示 = ("下排:P" + ((i - 10) + 1).ToString("00:") + this.List_CCD01_02_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
        //                                        DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(2200, (i - 10) * 40), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

        //                                    }

        //                                }


        //                        }

        //                        #endregion

        //                        g.Dispose();
        //                        g = null;
        //                    }
        //                    catch
        //                    {

        //                    }


        //                }

        //            }

        //            this.PLC_Device_CCD01_02_PIN量測_檢測正位度計算_RefreshCanvas.Bool = false;
        //        }

        //        int cnt_Program_CCD01_02_PIN量測_檢測正位度計算 = 65534;
        //        void sub_Program_CCD01_02_PIN量測_檢測正位度計算()
        //        {
        //            if (cnt_Program_CCD01_02_PIN量測_檢測正位度計算 == 65534)
        //            {
        //                this.h_Canvas_Tech_CCD01_02.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_02_PIN量測正位度_OnCanvasDrawEvent;
        //                this.h_Canvas_Main_CCD01_02_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_02_PIN量測正位度_OnCanvasDrawEvent;
        //                PLC_Device_CCD01_02_PIN量測_檢測正位度計算.SetComment("PLC_CCD01_02_PIN量測_檢測正位度計算");
        //                PLC_Device_CCD01_02_PIN量測_檢測正位度計算.Bool = false;
        //                PLC_Device_CCD01_02_PIN量測_檢測正位度計算按鈕.Bool = false;
        //                cnt_Program_CCD01_02_PIN量測_檢測正位度計算 = 65535;

        //            }
        //            if (cnt_Program_CCD01_02_PIN量測_檢測正位度計算 == 65535) cnt_Program_CCD01_02_PIN量測_檢測正位度計算 = 1;
        //            if (cnt_Program_CCD01_02_PIN量測_檢測正位度計算 == 1) cnt_Program_CCD01_02_PIN量測_檢測正位度計算_觸發按下(ref cnt_Program_CCD01_02_PIN量測_檢測正位度計算);
        //            if (cnt_Program_CCD01_02_PIN量測_檢測正位度計算 == 2) cnt_Program_CCD01_02_PIN量測_檢測正位度計算_檢查按下(ref cnt_Program_CCD01_02_PIN量測_檢測正位度計算);
        //            if (cnt_Program_CCD01_02_PIN量測_檢測正位度計算 == 3) cnt_Program_CCD01_02_PIN量測_檢測正位度計算_初始化(ref cnt_Program_CCD01_02_PIN量測_檢測正位度計算);
        //            if (cnt_Program_CCD01_02_PIN量測_檢測正位度計算 == 4) cnt_Program_CCD01_02_PIN量測_檢測正位度計算_數值計算(ref cnt_Program_CCD01_02_PIN量測_檢測正位度計算);
        //            if (cnt_Program_CCD01_02_PIN量測_檢測正位度計算 == 5) cnt_Program_CCD01_02_PIN量測_檢測正位度計算_量測結果(ref cnt_Program_CCD01_02_PIN量測_檢測正位度計算);
        //            if (cnt_Program_CCD01_02_PIN量測_檢測正位度計算 == 6) cnt_Program_CCD01_02_PIN量測_檢測正位度計算_繪製畫布(ref cnt_Program_CCD01_02_PIN量測_檢測正位度計算);
        //            if (cnt_Program_CCD01_02_PIN量測_檢測正位度計算 == 7) cnt_Program_CCD01_02_PIN量測_檢測正位度計算 = 65500;
        //            if (cnt_Program_CCD01_02_PIN量測_檢測正位度計算 > 1) cnt_Program_CCD01_02_PIN量測_檢測正位度計算_檢查放開(ref cnt_Program_CCD01_02_PIN量測_檢測正位度計算);

        //            if (cnt_Program_CCD01_02_PIN量測_檢測正位度計算 == 65500)
        //            {
        //                PLC_Device_CCD01_02_PIN量測_檢測正位度計算.Bool = false;
        //                PLC_Device_CCD01_02_PIN量測_檢測正位度計算按鈕.Bool = false;
        //                cnt_Program_CCD01_02_PIN量測_檢測正位度計算 = 65535;
        //            }
        //        }
        //        void cnt_Program_CCD01_02_PIN量測_檢測正位度計算_觸發按下(ref int cnt)
        //        {
        //            if (PLC_Device_CCD01_02_PIN量測_檢測正位度計算按鈕.Bool || PLC_Device_CCD01_02_計算一次.Bool)
        //            {
        //                PLC_Device_CCD01_02_PIN量測_檢測正位度計算.Bool = true;
        //                cnt++;
        //            }
        //        }
        //        void cnt_Program_CCD01_02_PIN量測_檢測正位度計算_檢查按下(ref int cnt)
        //        {
        //            if (PLC_Device_CCD01_02_PIN量測_檢測正位度計算.Bool)
        //            {
        //                cnt++;
        //            }
        //        }
        //        void cnt_Program_CCD01_02_PIN量測_檢測正位度計算_檢查放開(ref int cnt)
        //        {
        //            //if (!PLC_Device_CCD01_02_PIN量測_檢測正位度計算按鈕.Bool)
        //            //{
        //            //    cnt = 65500;
        //            //}
        //        }
        //        void cnt_Program_CCD01_02_PIN量測_檢測正位度計算_初始化(ref int cnt)
        //        {
        //            this.MyTimer_CCD01_02_PIN量測_檢測正位度計算.TickStop();
        //            this.MyTimer_CCD01_02_PIN量測_檢測正位度計算.StartTickTime(99999);
        //            this.List_CCD01_02_PIN正位度量測參數_正位度距離 = new double[20];
        //            this.List_CCD01_02_PIN正位度量測參數_正位度量測點_OK = new bool[20];

        //            cnt++;
        //        }
        //        void cnt_Program_CCD01_02_PIN量測_檢測正位度計算_數值計算(ref int cnt)
        //        {
        //            double distance = 0;
        //            double temp_X = 0;
        //            double temp_Y = 0;
        //            PLC_Device_CCD01_02_PIN量測_檢測正位度計算_OK.Bool = true;

        //            for (int i = 0; i < 20; i++)
        //            {
        //                if (this.List_CCD01_02_PIN量測參數_量測點_有無[i])
        //                {
        //                    temp_X = Math.Pow(this.List_CCD01_02_PIN量測參數_量測點[i].X - this.List_CCD01_02_PIN正位度量測參數_規範點[i].X, 2);
        //                    temp_Y = Math.Pow(this.List_CCD01_02_PIN量測參數_量測點[i].Y - this.List_CCD01_02_PIN正位度量測參數_規範點[i].Y, 2);

        //                    distance = Math.Sqrt(temp_X + temp_Y);
        //                    this.List_CCD01_02_PIN正位度量測參數_正位度距離[i] = distance * this.CCD01_比例尺_pixcel_To_mm;
        //                }
        //                else
        //                {
        //                    PLC_Device_CCD01_02_PIN量測_檢測正位度計算_OK.Bool = false;
        //                    List_CCD01_02_PIN正位度量測參數_正位度量測點_OK[i] = false;
        //                }

        //            }
        //            cnt++;
        //        }
        //        void cnt_Program_CCD01_02_PIN量測_檢測正位度計算_量測結果(ref int cnt)
        //        {

        //            PLC_Device_CCD01_02_PIN量測_檢測正位度計算_OK.Bool = true; // 檢測結果初始化

        //            for (int i = 0; i < 20; i++)
        //            {
        //                int 標準值差值 = this.PLC_Device_CCD01_02_PIN量測_檢測正位度計算_差值.Value;
        //                double 量測距離 = this.List_CCD01_02_PIN正位度量測參數_正位度距離[i];

        //                量測距離 = 量測距離 * 1000;
        //                量測距離 /= 1000;

        //                if (!PLC_Device_CCD01_02_PIN量測_檢測正位度計算_不測試.Bool)
        //                {
        //                    if (this.List_CCD01_02_PIN量測參數_量測點_有無[i])
        //                    {


        //                        if (量測距離 >= 0)
        //                        {
        //                            if (標準值差值 <= Math.Abs(量測距離) * 1000)
        //                            {
        //                                this.List_CCD01_02_PIN正位度量測參數_正位度量測點_OK[i] = false;
        //                                PLC_Device_CCD01_02_PIN量測_檢測正位度計算_OK.Bool = false;
        //                            }
        //                            else
        //                            {
        //                                this.List_CCD01_02_PIN正位度量測參數_正位度量測點_OK[i] = true;
        //                            }
        //                        }

        //                    }
        //                    else
        //                    {
        //                        this.List_CCD01_02_PIN正位度量測參數_正位度量測點_OK[i] = false;
        //                        PLC_Device_CCD01_02_PIN量測_檢測正位度計算_OK.Bool = false;
        //                    }

        //                }
        //                else
        //                {
        //                    this.List_CCD01_02_PIN正位度量測參數_正位度量測點_OK[i] = true;
        //                }

        //                this.List_CCD01_02_PIN正位度量測參數_正位度距離[i] = 量測距離;
        //            }
        //            cnt++;
        //        }
        //        void cnt_Program_CCD01_02_PIN量測_檢測正位度計算_繪製畫布(ref int cnt)
        //        {
        //            this.PLC_Device_CCD01_02_PIN量測_檢測正位度計算_RefreshCanvas.Bool = true;
        //            if (this.PLC_Device_CCD01_02_PIN量測_檢測正位度計算按鈕.Bool && !PLC_Device_CCD01_02_計算一次.Bool)
        //            {

        //                this.h_Canvas_Tech_CCD01_02.RefreshCanvas();
        //            }

        //            cnt++;
        //        }
        //        #endregion
        //        #region Event
        //        private void PlC_RJ_Button_CCD01_02_儲存圖片_MouseClickEvent(MouseEventArgs mevent)
        //        {
        //            this.Invoke(new Action(delegate {
        //                if (saveImageDialog.ShowDialog() == DialogResult.OK)
        //                {
        //                    this.h_Canvas_Tech_CCD01_02.SaveImage(saveImageDialog.FileName);
        //                }
        //            }));
        //        }
        //        private void PlC_RJ_Button_CCD01_02_讀取圖片_MouseClickEvent(MouseEventArgs mevent)
        //        {
        //            this.Invoke(new Action(delegate {
        //                if (openImageDialog.ShowDialog() == DialogResult.OK)
        //                {
        //                    this.CCD01_AxImageBW8.LoadFile(openImageDialog.FileName);
        //                    try
        //                    {
        //                        this.h_Canvas_Tech_CCD01_02.ImageCopy(CCD01_AxImageBW8.VegaHandle);
        //                        this.CCD01_02_SrcImageHandle = h_Canvas_Tech_CCD01_02.VegaHandle;
        //                        this.h_Canvas_Tech_CCD01_02.RefreshCanvas();
        //                    }
        //                    catch
        //                    {
        //                        err_message01_02 = MessageBox.Show("讀取圖片空白", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        //                        if (err_message01_02 == DialogResult.OK)
        //                        {

        //                        }
        //                    }
        //                }
        //            }));
        //        }
        //        private void PlC_RJ_Button_Main_CCD01_02儲存圖片_MouseClickEvent(MouseEventArgs mevent)
        //        {
        //            this.Invoke(new Action(delegate {
        //                if (saveImageDialog.ShowDialog() == DialogResult.OK)
        //                {
        //                    this.h_Canvas_Main_CCD01_02_檢測畫面.SaveImage(saveImageDialog.FileName);
        //                }
        //            }));
        //        }
        //        private void PlC_RJ_Button_Main_CCD01_02讀取圖片_MouseClickEvent(MouseEventArgs mevent)
        //        {
        //            this.Invoke(new Action(delegate {
        //                if (openImageDialog.ShowDialog() == DialogResult.OK)
        //                {
        //                    this.CCD01_AxImageBW8.LoadFile(openImageDialog.FileName);
        //                    try
        //                    {
        //                        this.h_Canvas_Main_CCD01_02_檢測畫面.ImageCopy(CCD01_AxImageBW8.VegaHandle);
        //                        this.CCD01_02_SrcImageHandle = h_Canvas_Main_CCD01_02_檢測畫面.VegaHandle;
        //                        this.h_Canvas_Main_CCD01_02_檢測畫面.RefreshCanvas();
        //                    }
        //                    catch
        //                    {
        //                        err_message01_02 = MessageBox.Show("讀取圖片空白", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

        //                        if (err_message01_02 == DialogResult.OK)
        //                        {

        //                        }
        //                    }
        //                }
        //            }));
        //        }
        //        private void PlC_Button_Main_CCD01_02_ZOOM更新_btnClick(object sender, EventArgs e)
        //        {
        //            if (CCD01_02_SrcImageHandle != 0)
        //            {
        //                PLC_Device_Main_CCD01_02_ZOOM更新.Bool = true;
        //                h_Canvas_Main_CCD01_02_檢測畫面.RefreshCanvas();
        //            }
        //        }
        //        private void plC_RJ_Button_CCD01_02_Tech_PIN量測框大小設為一致_Click(object sender, EventArgs e)
        //        {
        //            for (int i = 0; i < 20; i++)
        //            {
        //                //this.List_PLC_Device_CCD01_02_PIN量測參數_Width[i].Value = this.List_PLC_Device_CCD01_02_PIN量測參數_Width[0].Value;
        //                //this.List_PLC_Device_CCD01_02_PIN量測參數_Height[i].Value = this.List_PLC_Device_CCD01_02_PIN量測參數_Height[0].Value;
        //                //this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值[i].Value = this.List_PLC_Device_CCD01_02_PIN量測參數_灰階門檻值[0].Value;
        //                //this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限[i].Value = this.List_PLC_Device_CCD01_02_PIN量測參數_面積上限[0].Value;
        //                //this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限[i].Value = this.List_PLC_Device_CCD01_02_PIN量測參數_面積下限[0].Value;
        //                this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterX[10].Value = this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterX[0].Value;
        //                this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterY[10].Value = this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterY[0].Value + 300;
        //                this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1X[10].Value = this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1X[0].Value;
        //                this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y[10].Value = this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y[0].Value + 300;
        //                this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2X[10].Value = this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2X[0].Value;
        //                this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y[10].Value = this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y[0].Value + 300;
        //                this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1X[10].Value = this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1X[0].Value;
        //                this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y[10].Value = this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y[0].Value + 300;
        //                this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2X[10].Value = this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2X[0].Value;
        //                this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y[10].Value = this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y[0].Value + 300;
        //            }
        //        }
        //        private PLC_Device PLC_Device_CCD01_02_PIN量測一鍵排列間距 = new PLC_Device("F4000");
        //        private void plC_RJ_Button_CCD01_02_Tech_PIN量測框一鍵排列_Click(object sender, EventArgs e)
        //        {
        //            for (int i = 0; i < 20; i++)
        //            {
        //                //if (i < 10)
        //                //{
        //                //    this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[i].Value = this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[0].Value - i * PLC_Device_CCD01_02_PIN量測一鍵排列間距.Value;
        //                //    this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[i].Value = this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[0].Value;
        //                //}

        //                //else
        //                //{
        //                //    this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[i].Value = this.List_PLC_Device_CCD01_02_PIN量測參數_OrgX[10].Value - (i - 10) * PLC_Device_CCD01_02_PIN量測一鍵排列間距.Value;
        //                //    this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[i].Value = this.List_PLC_Device_CCD01_02_PIN量測參數_OrgY[10].Value;
        //                //}

        //                if (i < 10)
        //                {
        //                    this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterX[i].Value = this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterX[0].Value - i * PLC_Device_CCD01_02_PIN量測一鍵排列間距.Value;
        //                    this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterY[i].Value = this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterY[0].Value;
        //                    this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1X[i].Value = this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1X[0].Value - i * PLC_Device_CCD01_02_PIN量測一鍵排列間距.Value;
        //                    this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y[i].Value = this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y[0].Value;
        //                    this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2X[i].Value = this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2X[0].Value - i * PLC_Device_CCD01_02_PIN量測一鍵排列間距.Value;
        //                    this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y[i].Value = this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y[0].Value;
        //                    this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1X[i].Value = this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1X[0].Value - i * PLC_Device_CCD01_02_PIN量測一鍵排列間距.Value;
        //                    this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y[i].Value = this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y[0].Value;
        //                    this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2X[i].Value = this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2X[0].Value - i * PLC_Device_CCD01_02_PIN量測一鍵排列間距.Value;
        //                    this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y[i].Value = this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y[0].Value;
        //                }

        //                else
        //                {
        //                    this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterX[i].Value = this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterX[10].Value - (i - 10) * PLC_Device_CCD01_02_PIN量測一鍵排列間距.Value;
        //                    this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterY[i].Value = this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterY[10].Value;
        //                    this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1X[i].Value = this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1X[10].Value - (i - 10) * PLC_Device_CCD01_02_PIN量測一鍵排列間距.Value;
        //                    this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y[i].Value = this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y[10].Value;
        //                    this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2X[i].Value = this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2X[10].Value - (i - 10) * PLC_Device_CCD01_02_PIN量測一鍵排列間距.Value;
        //                    this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y[i].Value = this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y[10].Value;
        //                    this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1X[i].Value = this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1X[10].Value - (i - 10) * PLC_Device_CCD01_02_PIN量測一鍵排列間距.Value;
        //                    this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y[i].Value = this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y[10].Value;
        //                    this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2X[i].Value = this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2X[10].Value - (i - 10) * PLC_Device_CCD01_02_PIN量測一鍵排列間距.Value;
        //                    this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y[i].Value = this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y[10].Value;
        //                }

        //            }
        //        }
        //        private void plC_RJ_Button_CCD01_02_GAP初始化量測框_MouseClickEvent(MouseEventArgs mevent)
        //        {
        //            GAP初始化量測框();
        //        }
        //        private void GAP初始化量測框()
        //        {
        //            this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterX[0].Value = 2340;
        //            this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterY[0].Value = 690;
        //            this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1X[0].Value = 2265;
        //            this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y[0].Value = 775;
        //            this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2X[0].Value = 2265;
        //            this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y[0].Value = 689;
        //            this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1X[0].Value = 2414;
        //            this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y[0].Value = 691;
        //            this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2X[0].Value = 2416;
        //            this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y[0].Value = 776;

        //            this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterX[10].Value = 2370;
        //            this.List_CCD01_02_PIN量測參數_量測框架ProbeCenterY[10].Value = 1053;
        //            this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1X[10].Value = 2305;
        //            this.List_CCD01_02_PIN量測參數_Line1框架起始Tip1Y[10].Value = 1142;
        //            this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2X[10].Value = 2299;
        //            this.List_CCD01_02_PIN量測參數_Line1框架終止Tip2Y[10].Value = 1053;
        //            this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1X[10].Value = 2441;
        //            this.List_CCD01_02_PIN量測參數_Line2框架起始Tip1Y[10].Value = 1053;
        //            this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2X[10].Value = 2444;
        //            this.List_CCD01_02_PIN量測參數_Line2框架終止Tip2Y[10].Value = 1137;
        //        }
        //        private void PlC_Button_CCD01_02_量測點作為規範點_btnClick(object sender, EventArgs e)
        //        {
        //            for (int i = 0; i < 20; i++)
        //            {
        //                List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_X[i].Value = (int)this.List_CCD01_02_PIN量測參數_量測點[i].X;
        //                List_PLC_Device_CCD01_02_PIN正位度量測參數_正位度規範值_Y[i].Value = (int)this.List_CCD01_02_PIN量測參數_量測點[i].Y;
        //            }
        //        }

        //        #endregion
    }
}
