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


        DialogResult err_message_01_04;

        void Program_CCD01_04()
        {
            CCD01_04_儲存圖片();
            this.sub_Program_CCD01_04_SNAP();
            this.sub_Program_CCD01_04_Tech_檢驗一次();
            this.sub_Program_CCD01_04_計算一次();
            this.sub_Program_CCD01_04_PIN量測_量測框調整();
            this.sub_Program_CCD01_04_PIN量測_檢測距離計算();
            this.sub_Program_CCD01_04_PIN正位度量測_設定規範位置();
            this.sub_Program_CCD01_04_PIN量測_檢測正位度計算();
            this.sub_Program_CCD01_04_PIN量測點量測();
            this.sub_Program_CCD01_04_Main_取像並檢驗();
            this.sub_Program_CCD01_04_Tech_取像並檢驗();
            this.sub_Program_CCD01_04_Main_檢驗一次();

        }

        #region PLC_CCD01_04_SNAP
        PLC_Device PLC_Device_CCD01_04_SNAP_按鈕 = new PLC_Device("M15050");
        PLC_Device PLC_Device_CCD01_04_SNAP = new PLC_Device("M15045");
        PLC_Device PLC_Device_CCD01_04_SNAP_LIVE = new PLC_Device("M15046");
        PLC_Device PLC_Device_CCD01_04_SNAP_電子快門 = new PLC_Device("F9020");
        PLC_Device PLC_Device_CCD01_04_SNAP_視訊增益 = new PLC_Device("F9021");
        PLC_Device PLC_Device_CCD01_04_SNAP_銳利度 = new PLC_Device("F9022");
        PLC_Device PLC_Device_CCD01_04_SNAP_光源亮度_紅正照 = new PLC_Device("F25020");
        PLC_Device PLC_Device_CCD01_04_SNAP_光源亮度_白正照 = new PLC_Device("F25021");
        int cnt_Program_CCD01_04_SNAP = 65534;
        void sub_Program_CCD01_04_SNAP()
        {
            if (cnt_Program_CCD01_04_SNAP == 65534)
            {
                PLC_Device_CCD01_04_SNAP.SetComment("PLC_CCD01_04_SNAP");
                PLC_Device_CCD01_04_SNAP.Bool = false;
                PLC_Device_CCD01_04_SNAP_按鈕.Bool = false;
                cnt_Program_CCD01_04_SNAP = 65535;
            }
            if (cnt_Program_CCD01_04_SNAP == 65535) cnt_Program_CCD01_04_SNAP = 1;
            if (cnt_Program_CCD01_04_SNAP == 1) cnt_Program_CCD01_04_SNAP_檢查按下(ref cnt_Program_CCD01_04_SNAP);
            if (cnt_Program_CCD01_04_SNAP == 2) cnt_Program_CCD01_04_SNAP_初始化(ref cnt_Program_CCD01_04_SNAP);
            if (cnt_Program_CCD01_04_SNAP == 3) cnt_Program_CCD01_04_SNAP_開始取像(ref cnt_Program_CCD01_04_SNAP);
            if (cnt_Program_CCD01_04_SNAP == 4) cnt_Program_CCD01_04_SNAP_取像結束(ref cnt_Program_CCD01_04_SNAP);
            if (cnt_Program_CCD01_04_SNAP == 5) cnt_Program_CCD01_04_SNAP_繪製影像(ref cnt_Program_CCD01_04_SNAP);
            if (cnt_Program_CCD01_04_SNAP == 6) cnt_Program_CCD01_04_SNAP = 65500;
            if (cnt_Program_CCD01_04_SNAP > 1) cnt_Program_CCD01_04_SNAP_檢查放開(ref cnt_Program_CCD01_04_SNAP);

            if (cnt_Program_CCD01_04_SNAP == 65500)
            {
                PLC_Device_CCD01_04_SNAP_按鈕.Bool = false;
                PLC_Device_CCD01_04_SNAP.Bool = false;
                cnt_Program_CCD01_04_SNAP = 65535;
            }
        }
        void cnt_Program_CCD01_04_SNAP_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_04_SNAP_按鈕.Bool)
            {
                PLC_Device_CCD01_04_SNAP.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_04_SNAP_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_04_SNAP.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_04_SNAP_初始化(ref int cnt)
        {
            PLC_Device_CCD01_SNAP_電子快門.Value = PLC_Device_CCD01_04_SNAP_電子快門.Value;
            PLC_Device_CCD01_SNAP_視訊增益.Value = PLC_Device_CCD01_04_SNAP_視訊增益.Value;
            PLC_Device_CCD01_SNAP_銳利度.Value = PLC_Device_CCD01_04_SNAP_銳利度.Value;
            if (PLC_Device_CCD01_04_SNAP_光源亮度_紅正照.Value != 0)
            {
                this.光源控制(enum_光源.CCD01_紅正照, (byte)this.PLC_Device_CCD01_04_SNAP_光源亮度_紅正照.Value);
                this.光源控制(enum_光源.CCD01_紅正照, true);
            }
            else if (this.PLC_Device_CCD01_04_SNAP_光源亮度_紅正照.Value == 0)
            {
                this.光源控制(enum_光源.CCD01_紅正照, (byte)0);
                this.光源控制(enum_光源.CCD01_紅正照, false);
            }
            if (PLC_Device_CCD01_04_SNAP_光源亮度_白正照.Value != 0)
            {
                this.光源控制(enum_光源.CCD01_白正照, (byte)this.PLC_Device_CCD01_04_SNAP_光源亮度_白正照.Value);
                this.光源控制(enum_光源.CCD01_白正照, true);
            }
            else if (this.PLC_Device_CCD01_04_SNAP_光源亮度_白正照.Value == 0)
            {
                this.光源控制(enum_光源.CCD01_白正照, (byte)0);
                this.光源控制(enum_光源.CCD01_白正照, false);
            }
            cnt++;
        }
        void cnt_Program_CCD01_04_SNAP_開始取像(ref int cnt)
        {
            if (!PLC_Device_CCD01_SNAP.Bool)
            {
                PLC_Device_CCD01_SNAP.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_04_SNAP_取像結束(ref int cnt)
        {
            if (!PLC_Device_CCD01_SNAP.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_04_SNAP_繪製影像(ref int cnt)
        {
            this.CCD01_04_SrcImageHandle = this.h_Canvas_Main_CCD01_04_檢測畫面.VegaHandle;
            this.h_Canvas_Main_CCD01_04_檢測畫面.ImageCopy(this.CCD01_AxImageBW8.VegaHandle);

            this.CCD01_04_SrcImageHandle = this.h_Canvas_Tech_CCD01_04.VegaHandle;
            this.h_Canvas_Tech_CCD01_04.ImageCopy(this.CCD01_AxImageBW8.VegaHandle);
            this.h_Canvas_Tech_CCD01_04.SetImageSize(this.h_Canvas_Tech_CCD01_04.ImageWidth, this.h_Canvas_Tech_CCD01_04.ImageHeight);

            if (!PLC_Device_CCD01_04_Tech_取像並檢驗.Bool && !PLC_Device_CCD01_04_Main_取像並檢驗.Bool)
            {
                if (this.PLC_Device_CCD01_04_SNAP.Bool) this.h_Canvas_Tech_CCD01_04.RefreshCanvas();


                if (PLC_Device_CCD01_04_SNAP_LIVE.Bool)
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
        #region PLC_CCD01_04_Main_取像並檢驗
        PLC_Device PLC_Device_CCD01_04_Main_取像並檢驗按鈕 = new PLC_Device("S39940");
        PLC_Device PLC_Device_CCD01_04_Main_取像並檢驗 = new PLC_Device("S39941");
        PLC_Device PLC_Device_CCD01_04_Main_取像並檢驗_OK = new PLC_Device("S39942");
        PLC_Device PLC_Device_CCD01_04_PLC觸發檢測 = new PLC_Device("S39740");
        PLC_Device PLC_Device_CCD01_04_PLC觸發檢測完成 = new PLC_Device("S39741");
        PLC_Device PLC_Device_CCD01_04_Main_取像完成 = new PLC_Device("S39742");
        PLC_Device PLC_Device_CCD01_04_Main_BUSY = new PLC_Device("S39743");
        bool flag_CCD01_04_開始存檔 = false;
        String CCD01_04_原圖位置, CCD01_04_量測圖位置;
        PLC_Device PLC_NumBox_CCD01_04_OK最大儲存張數 = new PLC_Device("F12603");
        PLC_Device PLC_NumBox_CCD01_04_NG最大儲存張數 = new PLC_Device("F12604");
        MyTimer CCD01_04_Init_Timer = new MyTimer();
        int cnt_Program_CCD01_04_Main_取像並檢驗 = 65534;
        void sub_Program_CCD01_04_Main_取像並檢驗()
        {
            if (cnt_Program_CCD01_04_Main_取像並檢驗 == 65534)
            {
                PLC_Device_CCD01_04_Main_取像並檢驗.SetComment("PLC_CCD01_04_Main_取像並檢驗");
                PLC_Device_CCD01_04_Main_BUSY.Bool = false;
                PLC_Device_CCD01_04_Main_取像完成.Bool = false;
                PLC_Device_CCD01_04_Main_取像並檢驗.Bool = false;
                PLC_Device_CCD01_04_PLC觸發檢測.Bool = false;
                PLC_Device_CCD01_04_PLC觸發檢測完成.Bool = false;
                PLC_Device_CCD01_04_Main_取像並檢驗_OK.Bool = false;
                PLC_Device_CCD01_04_Main_取像並檢驗按鈕.Bool = false;
                cnt_Program_CCD01_04_Main_取像並檢驗 = 65535;

            }
            if (cnt_Program_CCD01_04_Main_取像並檢驗 == 65535) cnt_Program_CCD01_04_Main_取像並檢驗 = 1;
            if (cnt_Program_CCD01_04_Main_取像並檢驗 == 1) cnt_Program_CCD01_04_Main_取像並檢驗_檢查按下(ref cnt_Program_CCD01_04_Main_取像並檢驗);
            if (cnt_Program_CCD01_04_Main_取像並檢驗 == 2) cnt_Program_CCD01_04_Main_取像並檢驗_初始化(ref cnt_Program_CCD01_04_Main_取像並檢驗);
            if (cnt_Program_CCD01_04_Main_取像並檢驗 == 3) cnt_Program_CCD01_04_Main_取像並檢驗_開始SNAP(ref cnt_Program_CCD01_04_Main_取像並檢驗);
            if (cnt_Program_CCD01_04_Main_取像並檢驗 == 4) cnt_Program_CCD01_04_Main_取像並檢驗_結束SNAP(ref cnt_Program_CCD01_04_Main_取像並檢驗);
            if (cnt_Program_CCD01_04_Main_取像並檢驗 == 5) cnt_Program_CCD01_04_Main_取像並檢驗_開始計算一次(ref cnt_Program_CCD01_04_Main_取像並檢驗);
            if (cnt_Program_CCD01_04_Main_取像並檢驗 == 6) cnt_Program_CCD01_04_Main_取像並檢驗_結束計算一次(ref cnt_Program_CCD01_04_Main_取像並檢驗);
            if (cnt_Program_CCD01_04_Main_取像並檢驗 == 7) cnt_Program_CCD01_04_Main_取像並檢驗_繪製畫布(ref cnt_Program_CCD01_04_Main_取像並檢驗);
            if (cnt_Program_CCD01_04_Main_取像並檢驗 == 8) cnt_Program_CCD01_04_Main_取像並檢驗_檢查重測次數(ref cnt_Program_CCD01_04_Main_取像並檢驗);
            if (cnt_Program_CCD01_04_Main_取像並檢驗 == 9) cnt_Program_CCD01_04_Main_取像並檢驗 = 65500;
            if (cnt_Program_CCD01_04_Main_取像並檢驗 > 1) cnt_Program_CCD01_04_Main_取像並檢驗_檢查放開(ref cnt_Program_CCD01_04_Main_取像並檢驗);

            if (cnt_Program_CCD01_04_Main_取像並檢驗 == 65500)
            {
                PLC_Device_CCD01_04_Main_BUSY.Bool = false;
                PLC_Device_CCD01_04_Main_取像完成.Bool = false;
                PLC_Device_CCD01_04_Main_取像並檢驗.Bool = false;
                PLC_Device_CCD01_04_PLC觸發檢測.Bool = false;
                PLC_Device_CCD01_04_Main_取像並檢驗按鈕.Bool = false;
                cnt_Program_CCD01_04_Main_取像並檢驗 = 65535;
            }
        }
        void cnt_Program_CCD01_04_Main_取像並檢驗_檢查按下(ref int cnt)
        {

            if (PLC_Device_CCD01_04_Main_取像並檢驗按鈕.Bool || PLC_Device_CCD01_04_PLC觸發檢測.Bool)
            {
                CCD01_04_Init_Timer.TickStop();
                CCD01_04_Init_Timer.StartTickTime(100000);
                PLC_Device_CCD01_04_Main_取像並檢驗.Bool = true;
                cnt++;
            }



        }
        void cnt_Program_CCD01_04_Main_取像並檢驗_檢查放開(ref int cnt)
        {
            //if (!PLC_Device_CCD01_04_Main_取像並檢驗.Bool && !PLC_Device_CCD01_04_PLC觸發檢測.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_04_Main_取像並檢驗_初始化(ref int cnt)
        {
            PLC_Device_CCD01_04_Main_BUSY.Bool = true;
            PLC_Device_CCD01_04_PLC觸發檢測完成.Bool = false;
            cnt++;
        }
        void cnt_Program_CCD01_04_Main_取像並檢驗_開始SNAP(ref int cnt)
        {
            if (!PLC_Device_CCD01_04_SNAP.Bool)
            {
                PLC_Device_CCD01_04_SNAP_按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_04_Main_取像並檢驗_結束SNAP(ref int cnt)
        {
            if (!PLC_Device_CCD01_04_SNAP_按鈕.Bool)
            {
                光源控制(enum_光源.CCD01_紅正照, (byte)0);
                光源控制(enum_光源.CCD01_紅正照, false);
                光源控制(enum_光源.CCD01_白正照, (byte)0);
                光源控制(enum_光源.CCD01_白正照, false);
                PLC_Device_CCD01_04_Main_取像完成.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_04_Main_取像並檢驗_開始計算一次(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_04_計算一次.Bool)
            {

                this.PLC_Device_CCD01_04_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_04_Main_取像並檢驗_結束計算一次(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_04_計算一次.Bool)
            {

                Console.WriteLine($"CCD01_04檢測,耗時 {CCD01_04_Init_Timer.ToString()}");
                cnt++;
            }
        }
        void cnt_Program_CCD01_04_Main_取像並檢驗_繪製畫布(ref int cnt)
        {
            if (CCD01_04_SrcImageHandle != 0)
            {
                this.h_Canvas_Main_CCD01_04_檢測畫面.RefreshCanvas();
                PLC_Device_CCD01_04_PLC觸發檢測完成.Bool = true;
                flag_CCD01_04_開始存檔 = true;
            }
            cnt++;
        }
        void cnt_Program_CCD01_04_Main_取像並檢驗_檢查重測次數(ref int cnt)
        {
            cnt++;
        }
        private void button8_Click(object sender, EventArgs e)
        {
            flag_CCD01_04_開始存檔 = true;
            flag_CCD02_04_開始存檔 = true;
        }
        private void CCD01_04_儲存圖片()
        {
            if (flag_CCD01_04_開始存檔)
            {
                String FilePlaceOK = plC_WordBox_CCD01_04_OK存圖路徑.Text;
                String FileNameOK = "CCD01_04_OK";
                String FilePlaceNG = plC_WordBox_CCD01_04_NG存圖路徑.Text;
                String FileNameNG = "CCD01_04_NG";
                儲存檔案_往後移位(FilePlaceOK, FileNameOK, PLC_NumBox_CCD01_04_OK最大儲存張數.Value);
                儲存檔案_往後移位(FilePlaceNG, FileNameNG, PLC_NumBox_CCD01_04_NG最大儲存張數.Value);
                if (PLC_Device_CCD01_04_Main_取像並檢驗_OK.Bool)
                {
                    整理檔案(FilePlaceOK, FileNameOK, PLC_NumBox_CCD01_04_OK最大儲存張數.Value);
                    FileNameOK = FileNameOK + "_OK";
                    CCD01_04_原圖位置 = CCD01_04_OK儲存檔案檢查(FilePlaceOK, FileNameOK + "_A", PLC_NumBox_CCD01_04_OK最大儲存張數.Value);
                    CCD01_04_量測圖位置 = CCD01_04_原圖位置.Replace("_A", "_B");
                    this.Invoke(new Action(delegate
                    {
                        if (plC_ComboBox_CCD01_04_OK是否存圖.SelectedIndex == 0)
                        {
                            this.h_Canvas_Main_CCD01_04_檢測畫面.SaveImage(CCD01_04_原圖位置);
                        }
                    }));
                }
                else if (!PLC_Device_CCD01_04_Main_取像並檢驗_OK.Bool)
                {
                    整理檔案(FilePlaceNG, FileNameNG, PLC_NumBox_CCD01_04_NG最大儲存張數.Value);
                    FileNameNG = FileNameNG + "_NG";
                    CCD01_04_原圖位置 = CCD01_04_NG儲存檔案檢查(FilePlaceNG, FileNameNG + "_A", PLC_NumBox_CCD01_04_NG最大儲存張數.Value);
                    CCD01_04_量測圖位置 = CCD01_04_原圖位置.Replace("_A", "_B");
                    this.Invoke(new Action(delegate
                    {
                        if (plC_ComboBox_CCD01_04_NG是否存圖.SelectedIndex == 0)
                        {
                            this.h_Canvas_Main_CCD01_04_檢測畫面.SaveImage(CCD01_04_原圖位置);
                        }
                    }));
                }
                flag_CCD01_04_開始存檔 = false;
            }
        }
        #endregion
        #region PLC_CCD01_04_Tech_取像並檢驗
        PLC_Device PLC_Device_CCD01_04_Tech_取像並檢驗按鈕 = new PLC_Device("M15650");
        PLC_Device PLC_Device_CCD01_04_Tech_取像並檢驗 = new PLC_Device("M15645");
        int cnt_Program_CCD01_04_Tech_取像並檢驗 = 65534;
        void sub_Program_CCD01_04_Tech_取像並檢驗()
        {
            if (cnt_Program_CCD01_04_Tech_取像並檢驗 == 65534)
            {
                PLC_Device_CCD01_04_Tech_取像並檢驗.SetComment("PLC_CCD01_04_Tech_取像並檢驗");
                PLC_Device_CCD01_04_Tech_取像並檢驗.Bool = false;
                PLC_Device_CCD01_04_Tech_取像並檢驗按鈕.Bool = false;
                cnt_Program_CCD01_04_Tech_取像並檢驗 = 65535;
            }
            if (cnt_Program_CCD01_04_Tech_取像並檢驗 == 65535) cnt_Program_CCD01_04_Tech_取像並檢驗 = 1;
            if (cnt_Program_CCD01_04_Tech_取像並檢驗 == 1) cnt_Program_CCD01_04_Tech_取像並檢驗_檢查按下(ref cnt_Program_CCD01_04_Tech_取像並檢驗);
            if (cnt_Program_CCD01_04_Tech_取像並檢驗 == 2) cnt_Program_CCD01_04_Tech_取像並檢驗_初始化(ref cnt_Program_CCD01_04_Tech_取像並檢驗);
            if (cnt_Program_CCD01_04_Tech_取像並檢驗 == 3) cnt_Program_CCD01_04_Tech_取像並檢驗_SNAP一次開始(ref cnt_Program_CCD01_04_Tech_取像並檢驗);
            if (cnt_Program_CCD01_04_Tech_取像並檢驗 == 4) cnt_Program_CCD01_04_Tech_取像並檢驗_SNAP一次結束(ref cnt_Program_CCD01_04_Tech_取像並檢驗);
            if (cnt_Program_CCD01_04_Tech_取像並檢驗 == 5) cnt_Program_CCD01_04_Tech_取像並檢驗_計算一次開始(ref cnt_Program_CCD01_04_Tech_取像並檢驗);
            if (cnt_Program_CCD01_04_Tech_取像並檢驗 == 6) cnt_Program_CCD01_04_Tech_取像並檢驗_計算一次結束(ref cnt_Program_CCD01_04_Tech_取像並檢驗);
            if (cnt_Program_CCD01_04_Tech_取像並檢驗 == 7) cnt_Program_CCD01_04_Tech_取像並檢驗_繪製畫布(ref cnt_Program_CCD01_04_Tech_取像並檢驗);
            if (cnt_Program_CCD01_04_Tech_取像並檢驗 == 8) cnt_Program_CCD01_04_Tech_取像並檢驗 = 65500;
            if (cnt_Program_CCD01_04_Tech_取像並檢驗 > 1) cnt_Program_CCD01_04_Tech_取像並檢驗_檢查放開(ref cnt_Program_CCD01_04_Tech_取像並檢驗);

            if (cnt_Program_CCD01_04_Tech_取像並檢驗 == 65500)
            {
                PLC_Device_CCD01_04_Tech_取像並檢驗按鈕.Bool = false;
                PLC_Device_CCD01_04_Tech_取像並檢驗.Bool = false;
                cnt_Program_CCD01_04_Tech_取像並檢驗 = 65535;
            }
        }
        void cnt_Program_CCD01_04_Tech_取像並檢驗_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_04_Tech_取像並檢驗按鈕.Bool)
            {
                PLC_Device_CCD01_04_Tech_取像並檢驗.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_04_Tech_取像並檢驗_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_04_Tech_取像並檢驗.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_04_Tech_取像並檢驗_初始化(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_04_Tech_取像並檢驗_SNAP一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_04_SNAP.Bool)
            {
                this.PLC_Device_CCD01_04_SNAP_按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_04_Tech_取像並檢驗_SNAP一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_04_SNAP_按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_04_Tech_取像並檢驗_計算一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_04_計算一次.Bool)
            {
                this.PLC_Device_CCD01_04_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_04_Tech_取像並檢驗_計算一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_04_計算一次.Bool)
            {

                cnt++;
            }
        }
        void cnt_Program_CCD01_04_Tech_取像並檢驗_繪製畫布(ref int cnt)
        {
            if (CCD01_04_SrcImageHandle != 0)
            {
                this.h_Canvas_Tech_CCD01_04.RefreshCanvas();
            }
            if (PLC_Device_CCD01_04_SNAP_LIVE.Bool)
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
        #region PLC_CCD01_04_Tech_檢驗一次
        PLC_Device PLC_Device_CCD01_04_Tech_檢驗一次按鈕 = new PLC_Device("M15660");
        PLC_Device PLC_Device_CCD01_04_Tech_檢驗一次 = new PLC_Device("M15655");
        int cnt_Program_CCD01_04_Tech_檢驗一次 = 65534;
        void sub_Program_CCD01_04_Tech_檢驗一次()
        {
            if (cnt_Program_CCD01_04_Tech_檢驗一次 == 65534)
            {
                PLC_Device_CCD01_04_Tech_檢驗一次.SetComment("PLC_CCD01_04_Tech_檢驗一次");
                PLC_Device_CCD01_04_Tech_檢驗一次.Bool = false;
                PLC_Device_CCD01_04_Tech_檢驗一次按鈕.Bool = false;
                cnt_Program_CCD01_04_Tech_檢驗一次 = 65535;
            }
            if (cnt_Program_CCD01_04_Tech_檢驗一次 == 65535) cnt_Program_CCD01_04_Tech_檢驗一次 = 1;
            if (cnt_Program_CCD01_04_Tech_檢驗一次 == 1) cnt_Program_CCD01_04_Tech_檢驗一次_檢查按下(ref cnt_Program_CCD01_04_Tech_檢驗一次);
            if (cnt_Program_CCD01_04_Tech_檢驗一次 == 2) cnt_Program_CCD01_04_Tech_檢驗一次_初始化(ref cnt_Program_CCD01_04_Tech_檢驗一次);
            if (cnt_Program_CCD01_04_Tech_檢驗一次 == 3) cnt_Program_CCD01_04_Tech_檢驗一次_計算一次開始(ref cnt_Program_CCD01_04_Tech_檢驗一次);
            if (cnt_Program_CCD01_04_Tech_檢驗一次 == 4) cnt_Program_CCD01_04_Tech_檢驗一次_計算一次結束(ref cnt_Program_CCD01_04_Tech_檢驗一次);
            if (cnt_Program_CCD01_04_Tech_檢驗一次 == 5) cnt_Program_CCD01_04_Tech_檢驗一次_繪製畫布(ref cnt_Program_CCD01_04_Tech_檢驗一次);
            if (cnt_Program_CCD01_04_Tech_檢驗一次 == 6) cnt_Program_CCD01_04_Tech_檢驗一次 = 65500;
            if (cnt_Program_CCD01_04_Tech_檢驗一次 > 1) cnt_Program_CCD01_04_Tech_檢驗一次_檢查放開(ref cnt_Program_CCD01_04_Tech_檢驗一次);

            if (cnt_Program_CCD01_04_Tech_檢驗一次 == 65500)
            {
                PLC_Device_CCD01_04_Tech_檢驗一次按鈕.Bool = false;
                PLC_Device_CCD01_04_Tech_檢驗一次.Bool = false;
                cnt_Program_CCD01_04_Tech_檢驗一次 = 65535;
            }
        }
        void cnt_Program_CCD01_04_Tech_檢驗一次_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_04_Tech_檢驗一次按鈕.Bool)
            {
                PLC_Device_CCD01_04_Tech_檢驗一次.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_04_Tech_檢驗一次_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_04_Tech_檢驗一次.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_04_Tech_檢驗一次_初始化(ref int cnt)
        {

            cnt++;
        }
        void cnt_Program_CCD01_04_Tech_檢驗一次_計算一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_04_計算一次.Bool)
            {
                this.PLC_Device_CCD01_04_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_04_Tech_檢驗一次_計算一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_04_計算一次.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_04_Tech_檢驗一次_繪製畫布(ref int cnt)
        {

            if (CCD01_04_SrcImageHandle != 0)
            {
                this.h_Canvas_Tech_CCD01_04.RefreshCanvas();
            }
            cnt++;
        }

































        #endregion
        #region PLC_CCD01_04_Main_檢驗一次
        PLC_Device PLC_Device_CCD01_04_Main_檢驗一次按鈕 = new PLC_Device("M15806");
        PLC_Device PLC_Device_CCD01_04_Main_檢驗一次 = new PLC_Device("M15807");
        int cnt_Program_CCD01_04_Main_檢驗一次 = 65534;
        void sub_Program_CCD01_04_Main_檢驗一次()
        {
            if (cnt_Program_CCD01_04_Main_檢驗一次 == 65534)
            {
                PLC_Device_CCD01_04_Main_檢驗一次.SetComment("PLC_CCD01_04_Main_檢驗一次");
                PLC_Device_CCD01_04_Main_檢驗一次.Bool = false;
                PLC_Device_CCD01_04_Main_檢驗一次按鈕.Bool = false;
                cnt_Program_CCD01_04_Main_檢驗一次 = 65535;
            }
            if (cnt_Program_CCD01_04_Main_檢驗一次 == 65535) cnt_Program_CCD01_04_Main_檢驗一次 = 1;
            if (cnt_Program_CCD01_04_Main_檢驗一次 == 1) cnt_Program_CCD01_04_Main_檢驗一次_檢查按下(ref cnt_Program_CCD01_04_Main_檢驗一次);
            if (cnt_Program_CCD01_04_Main_檢驗一次 == 2) cnt_Program_CCD01_04_Main_檢驗一次_初始化(ref cnt_Program_CCD01_04_Main_檢驗一次);
            if (cnt_Program_CCD01_04_Main_檢驗一次 == 3) cnt_Program_CCD01_04_Main_檢驗一次_計算一次開始(ref cnt_Program_CCD01_04_Main_檢驗一次);
            if (cnt_Program_CCD01_04_Main_檢驗一次 == 4) cnt_Program_CCD01_04_Main_檢驗一次_計算一次結束(ref cnt_Program_CCD01_04_Main_檢驗一次);
            if (cnt_Program_CCD01_04_Main_檢驗一次 == 5) cnt_Program_CCD01_04_Main_檢驗一次_繪製畫布(ref cnt_Program_CCD01_04_Main_檢驗一次);
            if (cnt_Program_CCD01_04_Main_檢驗一次 == 6) cnt_Program_CCD01_04_Main_檢驗一次 = 65500;
            if (cnt_Program_CCD01_04_Main_檢驗一次 > 1) cnt_Program_CCD01_04_Main_檢驗一次_檢查放開(ref cnt_Program_CCD01_04_Main_檢驗一次);

            if (cnt_Program_CCD01_04_Main_檢驗一次 == 65500)
            {
                PLC_Device_CCD01_04_Main_檢驗一次按鈕.Bool = false;
                PLC_Device_CCD01_04_Main_檢驗一次.Bool = false;
                cnt_Program_CCD01_04_Main_檢驗一次 = 65535;
            }
        }
        void cnt_Program_CCD01_04_Main_檢驗一次_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_04_Main_檢驗一次按鈕.Bool)
            {
                PLC_Device_CCD01_04_Main_檢驗一次.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_04_Main_檢驗一次_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_04_Main_檢驗一次.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_04_Main_檢驗一次_初始化(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_04_Main_檢驗一次_計算一次開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_04_計算一次.Bool)
            {
                this.PLC_Device_CCD01_04_計算一次.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_04_Main_檢驗一次_計算一次結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_04_計算一次.Bool)
            {

                cnt++;
            }
        }
        void cnt_Program_CCD01_04_Main_檢驗一次_繪製畫布(ref int cnt)
        {
            if (CCD01_04_SrcImageHandle != 0)
            {
                this.h_Canvas_Main_CCD01_04_檢測畫面.RefreshCanvas();
            }

            cnt++;
        }
        #endregion
        #region PLC_CCD01_04_計算一次
        PLC_Device PLC_Device_CCD01_04_計算一次 = new PLC_Device("S5025");
        PLC_Device PLC_Device_CCD01_04_計算一次_OK = new PLC_Device("S5026");
        PLC_Device PLC_Device_CCD01_04_計算一次_READY = new PLC_Device("S5027");
        MyTimer MyTimer_CCD01_04_計算一次 = new MyTimer();
        int cnt_Program_CCD01_04_計算一次 = 65534;
        void sub_Program_CCD01_04_計算一次()
        {
            this.PLC_Device_CCD01_04_計算一次_READY.Bool = !this.PLC_Device_CCD01_04_計算一次.Bool;
            if (cnt_Program_CCD01_04_計算一次 == 65534)
            {
                PLC_Device_CCD01_04_計算一次.SetComment("PLC_CCD01_04_計算一次");
                PLC_Device_CCD01_04_計算一次.Bool = false;

                cnt_Program_CCD01_04_計算一次 = 65535;
            }
            if (cnt_Program_CCD01_04_計算一次 == 65535) cnt_Program_CCD01_04_計算一次 = 1;
            if (cnt_Program_CCD01_04_計算一次 == 1) cnt_Program_CCD01_04_計算一次_檢查按下(ref cnt_Program_CCD01_04_計算一次);
            if (cnt_Program_CCD01_04_計算一次 == 2) cnt_Program_CCD01_04_計算一次_初始化(ref cnt_Program_CCD01_04_計算一次);
            if (cnt_Program_CCD01_04_計算一次 == 3) cnt_Program_CCD01_04_計算一次_步驟01開始(ref cnt_Program_CCD01_04_計算一次);
            if (cnt_Program_CCD01_04_計算一次 == 4) cnt_Program_CCD01_04_計算一次_步驟01結束(ref cnt_Program_CCD01_04_計算一次);
            if (cnt_Program_CCD01_04_計算一次 == 5) cnt_Program_CCD01_04_計算一次_步驟02開始(ref cnt_Program_CCD01_04_計算一次);
            if (cnt_Program_CCD01_04_計算一次 == 6) cnt_Program_CCD01_04_計算一次_步驟02結束(ref cnt_Program_CCD01_04_計算一次);
            if (cnt_Program_CCD01_04_計算一次 == 7) cnt_Program_CCD01_04_計算一次_步驟03開始(ref cnt_Program_CCD01_04_計算一次);
            if (cnt_Program_CCD01_04_計算一次 == 8) cnt_Program_CCD01_04_計算一次_步驟03結束(ref cnt_Program_CCD01_04_計算一次);
            if (cnt_Program_CCD01_04_計算一次 == 9) cnt_Program_CCD01_04_計算一次_步驟04開始(ref cnt_Program_CCD01_04_計算一次);
            if (cnt_Program_CCD01_04_計算一次 == 10) cnt_Program_CCD01_04_計算一次_步驟04結束(ref cnt_Program_CCD01_04_計算一次);
            if (cnt_Program_CCD01_04_計算一次 == 11) cnt_Program_CCD01_04_計算一次_步驟05開始(ref cnt_Program_CCD01_04_計算一次);
            if (cnt_Program_CCD01_04_計算一次 == 12) cnt_Program_CCD01_04_計算一次_步驟05結束(ref cnt_Program_CCD01_04_計算一次);
            if (cnt_Program_CCD01_04_計算一次 == 13) cnt_Program_CCD01_04_計算一次_步驟06開始(ref cnt_Program_CCD01_04_計算一次);
            if (cnt_Program_CCD01_04_計算一次 == 14) cnt_Program_CCD01_04_計算一次_步驟06結束(ref cnt_Program_CCD01_04_計算一次);
            if (cnt_Program_CCD01_04_計算一次 == 15) cnt_Program_CCD01_04_計算一次_計算結果(ref cnt_Program_CCD01_04_計算一次);
            if (cnt_Program_CCD01_04_計算一次 == 16) cnt_Program_CCD01_04_計算一次 = 65500;
            if (cnt_Program_CCD01_04_計算一次 > 1) cnt_Program_CCD01_04_計算一次_檢查放開(ref cnt_Program_CCD01_04_計算一次);

            if (cnt_Program_CCD01_04_計算一次 == 65500)
            {
                PLC_Device_CCD01_04_計算一次.Bool = false;
                cnt_Program_CCD01_04_計算一次 = 65535;
            }
        }
        void cnt_Program_CCD01_04_計算一次_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_04_計算一次.Bool) cnt++;
        }
        void cnt_Program_CCD01_04_計算一次_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_04_計算一次.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_04_計算一次_初始化(ref int cnt)
        {
            PLC_Device_CCD01_04_PIN量測_量測框調整.Bool = false;
            PLC_Device_CCD01_04_PIN量測_檢測距離計算.Bool = false;
            PLC_Device_CCD01_04_PIN正位度量測_設定規範位置.Bool = false;
            PLC_Device_CCD01_04_PIN量測_檢測距離計算.Bool = false;
            cnt++;
        }
        void cnt_Program_CCD01_04_計算一次_步驟01開始(ref int cnt)
        {
            this.MyTimer_CCD01_04_計算一次.TickStop();
            this.MyTimer_CCD01_04_計算一次.StartTickTime(99999);
            cnt++;


        }
        void cnt_Program_CCD01_04_計算一次_步驟01結束(ref int cnt)
        {
            cnt++;

        }
        void cnt_Program_CCD01_04_計算一次_步驟02開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_04_PIN量測點_量測框調整按鈕.Bool)
            {
                this.PLC_Device_CCD01_04_PIN量測點_量測框調整按鈕.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_04_計算一次_步驟02結束(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_04_PIN量測點_量測框調整按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_04_計算一次_步驟03開始(ref int cnt)
        {
            if (!PLC_Device_CCD01_04_PIN正位度量測_設定規範按鈕.Bool)
            {
                PLC_Device_CCD01_04_PIN正位度量測_設定規範按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_04_計算一次_步驟03結束(ref int cnt)
        {
            if (!PLC_Device_CCD01_04_PIN正位度量測_設定規範按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_04_計算一次_步驟04開始(ref int cnt)
        {
            if (!PLC_Device_CCD01_04_PIN量測_檢測距離計算按鈕.Bool && !PLC_Device_CCD01_04_PIN量測_檢測正位度計算按鈕.Bool)
            {
                PLC_Device_CCD01_04_PIN量測_檢測距離計算按鈕.Bool = true;
                PLC_Device_CCD01_04_PIN量測_檢測正位度計算按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_04_計算一次_步驟04結束(ref int cnt)
        {
            if (!PLC_Device_CCD01_04_PIN量測_檢測距離計算按鈕.Bool && !PLC_Device_CCD01_04_PIN量測_檢測正位度計算按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_04_計算一次_步驟05開始(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_04_計算一次_步驟05結束(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_04_計算一次_步驟06開始(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_04_計算一次_步驟06結束(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_04_計算一次_計算結果(ref int cnt)
        {
            bool flag = true;
           // if (!this.PLC_Device_CCD01_04_PIN量測_量測框調整_OK.Bool) flag = false;
            if (!this.PLC_Device_CCD01_04_PIN量測_檢測距離計算_OK.Bool) flag = false;
            if (!this.PLC_Device_CCD01_04_PIN量測_檢測正位度計算_OK.Bool) flag = false;
            PLC_Device_CCD01_04_Main_取像並檢驗_OK.Bool = flag;
            this.PLC_Device_CCD01_04_計算一次_OK.Bool = flag;
            //flag_CCD01_04_上端水平度寫入列表資料 = true;
            //flag_CCD01_04_上端間距寫入列表資料 = true;
            //flag_CCD01_04_上端水平度差值寫入列表資料 = true;

            cnt++;
        }





        #endregion
        #region PLC_CCD01_04_PIN量測_量測框調整
        MyTimer MyTimer_CCD01_04_PIN量測_量測框調整 = new MyTimer();
        PLC_Device PLC_Device_CCD01_04_PIN量測_量測框調整按鈕 = new PLC_Device("S6410");
        PLC_Device PLC_Device_CCD01_04_PIN量測_量測框調整 = new PLC_Device("S6405");
        PLC_Device PLC_Device_CCD01_04_PIN量測_量測框調整_OK = new PLC_Device("S6406");
        PLC_Device PLC_Device_CCD01_04_PIN量測_量測框調整_測試完成 = new PLC_Device("S6407");
        PLC_Device PLC_Device_CCD01_04_PIN量測_量測框調整_RefreshCanvas = new PLC_Device("S6408");
        PLC_Device PLC_Device_CCD01_04_PIN量測_有無量測不測試 = new PLC_Device("S6121");
        private List<AxOvkBase.AxROIBW8> List_CCD01_04_PIN量測_AxROIBW8_量測框調整 = new List<AxOvkBase.AxROIBW8>();
        private List<AxOvkBlob.AxObject> List_CCD01_04_PIN量測_AxObject_區塊分析 = new List<AxOvkBlob.AxObject>();
        private AxOvkPat.AxVisionInspectionFrame CCD01_04_PIN量測_AxVisionInspectionFrame_量測框調整;

        private List<PLC_Device> List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值 = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_04_PIN量測參數_OrgX = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_04_PIN量測參數_OrgY = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_04_PIN量測參數_Width = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_04_PIN量測參數_Height = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_04_PIN量測參數_面積上限 = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_04_PIN量測參數_面積下限 = new List<PLC_Device>();
        private PointF[] List_CCD01_04_PIN量測參數_量測點 = new PointF[20];
        private PointF[] List_CCD01_04_PIN量測參數_量測點_結果 = new PointF[20];
        private Point[] List_CCD01_04_PIN量測參數_量測點_轉換後座標 = new Point[20];
        private bool[] List_CCD01_04_PIN量測參數_量測點_有無 = new bool[20];

        #region 灰階門檻值

        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN21 = new PLC_Device("F622");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN22 = new PLC_Device("F623");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN23 = new PLC_Device("F624");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN24 = new PLC_Device("F625");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN25 = new PLC_Device("F626");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN26 = new PLC_Device("F627");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN27 = new PLC_Device("F628");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN28 = new PLC_Device("F629");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN29 = new PLC_Device("F630");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN30 = new PLC_Device("F631");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN31 = new PLC_Device("F632");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN32 = new PLC_Device("F633");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN33 = new PLC_Device("F634");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN34 = new PLC_Device("F635");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN35 = new PLC_Device("F636");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN36 = new PLC_Device("F637");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN37 = new PLC_Device("F638");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN38 = new PLC_Device("F639");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN39 = new PLC_Device("F640");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN40 = new PLC_Device("F641");

        #endregion
        #region OrgX
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN21 = new PLC_Device("F722");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN22 = new PLC_Device("F723");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN23 = new PLC_Device("F724");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN24 = new PLC_Device("F725");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN25 = new PLC_Device("F726");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN26 = new PLC_Device("F727");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN27 = new PLC_Device("F728");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN28 = new PLC_Device("F729");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN29 = new PLC_Device("F730");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN30 = new PLC_Device("F731");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN31 = new PLC_Device("F732");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN32 = new PLC_Device("F733");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN33 = new PLC_Device("F734");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN34 = new PLC_Device("F735");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN35 = new PLC_Device("F736");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN36 = new PLC_Device("F737");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN37 = new PLC_Device("F738");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN38 = new PLC_Device("F739");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN39 = new PLC_Device("F740");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN40 = new PLC_Device("F741");
        #endregion
        #region OrgY
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN21 = new PLC_Device("F822");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN22 = new PLC_Device("F823");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN23 = new PLC_Device("F824");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN24 = new PLC_Device("F825");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN25 = new PLC_Device("F826");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN26 = new PLC_Device("F827");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN27 = new PLC_Device("F828");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN28 = new PLC_Device("F829");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN29 = new PLC_Device("F830");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN30 = new PLC_Device("F831");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN31 = new PLC_Device("F832");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN32 = new PLC_Device("F833");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN33 = new PLC_Device("F834");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN34 = new PLC_Device("F835");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN35 = new PLC_Device("F836");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN36 = new PLC_Device("F837");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN37 = new PLC_Device("F838");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN38 = new PLC_Device("F839");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN39 = new PLC_Device("F840");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN40 = new PLC_Device("F841");
        #endregion
        #region Width
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Width_PIN21 = new PLC_Device("F922");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Width_PIN22 = new PLC_Device("F923");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Width_PIN23 = new PLC_Device("F924");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Width_PIN24 = new PLC_Device("F925");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Width_PIN25 = new PLC_Device("F926");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Width_PIN26 = new PLC_Device("F927");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Width_PIN27 = new PLC_Device("F928");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Width_PIN28 = new PLC_Device("F929");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Width_PIN29 = new PLC_Device("F930");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Width_PIN30 = new PLC_Device("F931");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Width_PIN31 = new PLC_Device("F932");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Width_PIN32 = new PLC_Device("F933");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Width_PIN33 = new PLC_Device("F934");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Width_PIN34 = new PLC_Device("F935");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Width_PIN35 = new PLC_Device("F936");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Width_PIN36 = new PLC_Device("F937");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Width_PIN37 = new PLC_Device("F938");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Width_PIN38 = new PLC_Device("F939");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Width_PIN39 = new PLC_Device("F940");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Width_PIN40 = new PLC_Device("F941");
        #endregion
        #region Height
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Height_PIN21 = new PLC_Device("F1022");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Height_PIN22 = new PLC_Device("F1023");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Height_PIN23 = new PLC_Device("F1024");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Height_PIN24 = new PLC_Device("F1025");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Height_PIN25 = new PLC_Device("F1026");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Height_PIN26 = new PLC_Device("F1027");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Height_PIN27 = new PLC_Device("F1028");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Height_PIN28 = new PLC_Device("F1029");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Height_PIN29 = new PLC_Device("F1030");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Height_PIN30 = new PLC_Device("F1031");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Height_PIN31 = new PLC_Device("F1032");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Height_PIN32 = new PLC_Device("F1033");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Height_PIN33 = new PLC_Device("F1034");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Height_PIN34 = new PLC_Device("F1035");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Height_PIN35 = new PLC_Device("F1036");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Height_PIN36 = new PLC_Device("F1037");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Height_PIN37 = new PLC_Device("F1038");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Height_PIN38 = new PLC_Device("F1039");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Height_PIN39 = new PLC_Device("F1040");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_Height_PIN40 = new PLC_Device("F1041");
        #endregion
        #region 面積上限
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN21 = new PLC_Device("F1122");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN22 = new PLC_Device("F1123");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN23 = new PLC_Device("F1124");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN24 = new PLC_Device("F1125");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN25 = new PLC_Device("F1126");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN26 = new PLC_Device("F1127");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN27 = new PLC_Device("F1128");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN28 = new PLC_Device("F1129");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN29 = new PLC_Device("F1130");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN30 = new PLC_Device("F1131");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN31 = new PLC_Device("F1132");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN32 = new PLC_Device("F1133");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN33 = new PLC_Device("F1134");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN34 = new PLC_Device("F1135");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN35 = new PLC_Device("F1136");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN36 = new PLC_Device("F1137");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN37 = new PLC_Device("F1138");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN38 = new PLC_Device("F1139");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN39 = new PLC_Device("F1140");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN40 = new PLC_Device("F1141");
        #endregion
        #region 面積下限
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN21 = new PLC_Device("F1222");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN22 = new PLC_Device("F1223");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN23 = new PLC_Device("F1224");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN24 = new PLC_Device("F1225");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN25 = new PLC_Device("F1226");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN26 = new PLC_Device("F1227");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN27 = new PLC_Device("F1228");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN28 = new PLC_Device("F1229");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN29 = new PLC_Device("F1230");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN30 = new PLC_Device("F1231");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN31 = new PLC_Device("F1232");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN32 = new PLC_Device("F1233");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN33 = new PLC_Device("F1234");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN34 = new PLC_Device("F1235");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN35 = new PLC_Device("F1236");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN36 = new PLC_Device("F1237");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN37 = new PLC_Device("F1238");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN38 = new PLC_Device("F1239");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN39 = new PLC_Device("F1240");
        private PLC_Device PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN40 = new PLC_Device("F1241");
        #endregion
        AxOvkBase.TxAxHitHandle[] CCD01_04_PIN量測_AxROIBW8_TxAxHitHandle = new AxOvkBase.TxAxHitHandle[20];
        bool[] flag_CCD01_04_PIN量測_AxROIBW8_MouseDown = new bool[20];
        private void H_Canvas_Tech_CCD01_04_PIN量測_量測框調整_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {

            if (PLC_Device_CCD01_04_Main_取像並檢驗.Bool || PLC_Device_CCD01_04_PLC觸發檢測.Bool || PLC_Device_CCD01_04_Main_檢驗一次.Bool)
            {
                try
                {
                    Graphics g = Graphics.FromHdc((IntPtr)HDC);
                    for (int i = 0; i < this.List_CCD01_04_PIN量測參數_量測點.Length; i++)
                    {
                        DrawingClass.Draw.十字中心(this.List_CCD01_04_PIN量測參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
                    }
                    g.Dispose();
                    g = null;
                }
                catch
                {

                }

            }
            else if (PLC_Device_CCD01_04_Tech_檢驗一次.Bool || PLC_Device_CCD01_04_Tech_取像並檢驗.Bool)
            {
                if (this.PLC_Device_CCD01_04_PIN量測_量測框調整_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        for (int i = 0; i < this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整.Count; i++)
                        {
                            if (i < 10)
                            {
                                this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].Title = string.Format("上排" + "{0}", (i + 11).ToString("00"));
                            }
                            if (i >= 10)
                            {
                                this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].Title = string.Format("下排" + "{0}", ((i - 10) + 11).ToString("00"));
                            }
                            this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].ShowTitle = true;
                            this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].ShowPlacement = false;
                            this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].DrawRect(HDC, ZoomX, ZoomY, 0, 0, 0x0000FF);
                        }
                        for (int i = 0; i < this.List_CCD01_04_PIN量測參數_量測點.Length; i++)
                        {
                            DrawingClass.Draw.十字中心(this.List_CCD01_04_PIN量測參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
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
                if (this.PLC_Device_CCD01_04_PIN量測_量測框調整_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        PointF po_str_PIN到基準Y = new PointF(200, 250);
                        Font font = new Font("微軟正黑體", 10);

                        if (this.plC_CheckBox_CCD01_04_PIN量測_繪製量測框.Checked)
                        {
                            for (int i = 0; i < this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整.Count; i++)
                            {
                                if (i < 10)
                                {
                                    this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].Title = string.Format("上排" + "{0}", (i + 11).ToString("00"));
                                }
                                if (i >= 10)
                                {
                                    this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].Title = string.Format("下排" + "{0}", ((i - 10) + 11).ToString("00"));
                                }
                                this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].ShowTitle = true;
                                this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].ShowPlacement = false;
                                this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].DrawFrame(HDC, ZoomX, ZoomY, 0, 0, 0x0000FF);
                            }
                        }
                        if (this.plC_CheckBox_CCD01_04_PIN量測_繪製量測區塊.Checked)
                        {
                            for (int i = 0; i < this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整.Count; i++)
                            {
                                this.List_CCD01_04_PIN量測_AxObject_區塊分析[i].DrawBlobs(HDC, -1, ZoomX, ZoomY, 0, 0, true, -1);
                            }

                        }
                        for (int i = 0; i < this.List_CCD01_04_PIN量測參數_量測點.Length; i++)
                        {
                            DrawingClass.Draw.十字中心(this.List_CCD01_04_PIN量測參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
                        }
                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }
            }

            this.PLC_Device_CCD01_04_PIN量測_量測框調整_RefreshCanvas.Bool = false;
        }
        private void H_Canvas_Tech_CCD01_04_PIN量測_量測框調整_OnCanvasMouseDownEvent(int x, int y, float ZoomX, float ZoomY, ref int InUsedEventNum, int InUsedCanvasHandle)
        {
            if (this.PLC_Device_CCD01_04_PIN量測_量測框調整.Bool)
            {
                for (int i = 0; i < this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整.Count; i++)
                {
                    this.CCD01_04_PIN量測_AxROIBW8_TxAxHitHandle[i] = this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].HitTest(x, y, ZoomX, ZoomY, 0, 0);
                    if (this.CCD01_04_PIN量測_AxROIBW8_TxAxHitHandle[i] != AxOvkBase.TxAxHitHandle.AX_HANDLE_NONE)
                    {
                        this.flag_CCD01_04_PIN量測_AxROIBW8_MouseDown[i] = true;
                        InUsedEventNum = 10;
                        return;
                    }
                }

            }

        }
        private void H_Canvas_Tech_CCD01_04_PIN量測_量測框調整_OnCanvasMouseMoveEvent(int x, int y, float ZoomX, float ZoomY)
        {
            for (int i = 0; i < this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {
                if (this.flag_CCD01_04_PIN量測_AxROIBW8_MouseDown[i])
                {
                    this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].DragROI(this.CCD01_04_PIN量測_AxROIBW8_TxAxHitHandle[i], x, y, ZoomX, ZoomY, 0, 0);
                    this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX[i].Value = this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].OrgX;
                    this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY[i].Value = this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].OrgY;
                    this.List_PLC_Device_CCD01_04_PIN量測參數_Width[i].Value = this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].ROIWidth;
                    this.List_PLC_Device_CCD01_04_PIN量測參數_Height[i].Value = this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].ROIHeight;
                }
            }
        }
        private void H_Canvas_Tech_CCD01_04_PIN量測_量測框調整_OnCanvasMouseUpEvent(int x, int y, float ZoomX, float ZoomY)
        {
            for (int i = 0; i < this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {
                this.flag_CCD01_04_PIN量測_AxROIBW8_MouseDown[i] = false;
            }
        }

        int cnt_Program_CCD01_04_PIN量測_量測框調整 = 65534;
        void sub_Program_CCD01_04_PIN量測_量測框調整()
        {
            if (cnt_Program_CCD01_04_PIN量測_量測框調整 == 65534)
            {
                this.h_Canvas_Tech_CCD01_04.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_04_PIN量測_量測框調整_OnCanvasDrawEvent;
                this.h_Canvas_Tech_CCD01_04.OnCanvasMouseDownEvent += H_Canvas_Tech_CCD01_04_PIN量測_量測框調整_OnCanvasMouseDownEvent;
                this.h_Canvas_Tech_CCD01_04.OnCanvasMouseMoveEvent += H_Canvas_Tech_CCD01_04_PIN量測_量測框調整_OnCanvasMouseMoveEvent;
                this.h_Canvas_Tech_CCD01_04.OnCanvasMouseUpEvent += H_Canvas_Tech_CCD01_04_PIN量測_量測框調整_OnCanvasMouseUpEvent;

                this.h_Canvas_Main_CCD01_04_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_04_PIN量測_量測框調整_OnCanvasDrawEvent;

                #region 灰階門檻值
                this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN21);
                this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN22);
                this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN23);
                this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN24);
                this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN25);
                this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN26);
                this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN27);
                this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN28);
                this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN29);
                this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN30);
                this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN31);
                this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN32);
                this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN33);
                this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN34);
                this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN35);
                this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN36);
                this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN37);
                this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN38);
                this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN39);
                this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值.Add(this.PLC_Device_CCD01_04_PIN量測參數_灰階門檻值_PIN40);
                #endregion
                #region OrgX
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN21);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN22);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN23);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN24);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN25);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN26);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN27);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN28);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN29);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN30);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN31);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN32);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN33);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN34);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN35);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN36);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN37);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN38);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN39);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgX_PIN40);
                #endregion
                #region OrgY
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN21);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN22);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN23);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN24);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN25);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN26);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN27);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN28);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN29);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN30);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN31);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN32);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN33);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN34);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN35);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN36);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN37);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN38);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN39);
                this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY.Add(this.PLC_Device_CCD01_04_PIN量測參數_OrgY_PIN40);
                #endregion
                #region Width
                this.List_PLC_Device_CCD01_04_PIN量測參數_Width.Add(this.PLC_Device_CCD01_04_PIN量測參數_Width_PIN21);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Width.Add(this.PLC_Device_CCD01_04_PIN量測參數_Width_PIN22);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Width.Add(this.PLC_Device_CCD01_04_PIN量測參數_Width_PIN23);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Width.Add(this.PLC_Device_CCD01_04_PIN量測參數_Width_PIN24);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Width.Add(this.PLC_Device_CCD01_04_PIN量測參數_Width_PIN25);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Width.Add(this.PLC_Device_CCD01_04_PIN量測參數_Width_PIN26);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Width.Add(this.PLC_Device_CCD01_04_PIN量測參數_Width_PIN27);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Width.Add(this.PLC_Device_CCD01_04_PIN量測參數_Width_PIN28);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Width.Add(this.PLC_Device_CCD01_04_PIN量測參數_Width_PIN29);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Width.Add(this.PLC_Device_CCD01_04_PIN量測參數_Width_PIN30);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Width.Add(this.PLC_Device_CCD01_04_PIN量測參數_Width_PIN31);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Width.Add(this.PLC_Device_CCD01_04_PIN量測參數_Width_PIN32);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Width.Add(this.PLC_Device_CCD01_04_PIN量測參數_Width_PIN33);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Width.Add(this.PLC_Device_CCD01_04_PIN量測參數_Width_PIN34);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Width.Add(this.PLC_Device_CCD01_04_PIN量測參數_Width_PIN35);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Width.Add(this.PLC_Device_CCD01_04_PIN量測參數_Width_PIN36);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Width.Add(this.PLC_Device_CCD01_04_PIN量測參數_Width_PIN37);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Width.Add(this.PLC_Device_CCD01_04_PIN量測參數_Width_PIN38);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Width.Add(this.PLC_Device_CCD01_04_PIN量測參數_Width_PIN39);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Width.Add(this.PLC_Device_CCD01_04_PIN量測參數_Width_PIN40);
                #endregion
                #region Height
                this.List_PLC_Device_CCD01_04_PIN量測參數_Height.Add(this.PLC_Device_CCD01_04_PIN量測參數_Height_PIN21);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Height.Add(this.PLC_Device_CCD01_04_PIN量測參數_Height_PIN22);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Height.Add(this.PLC_Device_CCD01_04_PIN量測參數_Height_PIN23);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Height.Add(this.PLC_Device_CCD01_04_PIN量測參數_Height_PIN24);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Height.Add(this.PLC_Device_CCD01_04_PIN量測參數_Height_PIN25);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Height.Add(this.PLC_Device_CCD01_04_PIN量測參數_Height_PIN26);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Height.Add(this.PLC_Device_CCD01_04_PIN量測參數_Height_PIN27);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Height.Add(this.PLC_Device_CCD01_04_PIN量測參數_Height_PIN28);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Height.Add(this.PLC_Device_CCD01_04_PIN量測參數_Height_PIN29);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Height.Add(this.PLC_Device_CCD01_04_PIN量測參數_Height_PIN30);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Height.Add(this.PLC_Device_CCD01_04_PIN量測參數_Height_PIN31);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Height.Add(this.PLC_Device_CCD01_04_PIN量測參數_Height_PIN32);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Height.Add(this.PLC_Device_CCD01_04_PIN量測參數_Height_PIN33);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Height.Add(this.PLC_Device_CCD01_04_PIN量測參數_Height_PIN34);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Height.Add(this.PLC_Device_CCD01_04_PIN量測參數_Height_PIN35);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Height.Add(this.PLC_Device_CCD01_04_PIN量測參數_Height_PIN36);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Height.Add(this.PLC_Device_CCD01_04_PIN量測參數_Height_PIN37);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Height.Add(this.PLC_Device_CCD01_04_PIN量測參數_Height_PIN38);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Height.Add(this.PLC_Device_CCD01_04_PIN量測參數_Height_PIN39);
                this.List_PLC_Device_CCD01_04_PIN量測參數_Height.Add(this.PLC_Device_CCD01_04_PIN量測參數_Height_PIN40);
                #endregion
                #region 面積上限
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN21);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN22);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN23);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN24);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN25);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN26);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN27);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN28);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN29);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN30);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN31);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN32);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN33);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN34);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN35);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN36);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN37);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN38);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN39);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積上限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積上限_PIN40);
                #endregion
                #region 面積下限
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN21);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN22);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN23);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN24);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN25);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN26);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN27);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN28);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN29);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN30);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN31);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN32);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN33);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN34);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN35);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN36);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN37);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN38);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN39);
                this.List_PLC_Device_CCD01_04_PIN量測參數_面積下限.Add(this.PLC_Device_CCD01_04_PIN量測參數_面積下限_PIN40);
                #endregion
                for (int i = 0; i < 20; i++)
                {
                    if (this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值[i].Value == 0) this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值[i].Value = 200;
                    if (this.List_PLC_Device_CCD01_04_PIN量測參數_Height[i].Value == 0) this.List_PLC_Device_CCD01_04_PIN量測參數_Height[i].Value = 100;
                    if (this.List_PLC_Device_CCD01_04_PIN量測參數_Width[i].Value == 0) this.List_PLC_Device_CCD01_04_PIN量測參數_Width[i].Value = 100;
                    if (this.List_PLC_Device_CCD01_04_PIN量測參數_Height[i].Value > 500) this.List_PLC_Device_CCD01_04_PIN量測參數_Height[i].Value = 500;
                    if (this.List_PLC_Device_CCD01_04_PIN量測參數_Width[i].Value > 500) this.List_PLC_Device_CCD01_04_PIN量測參數_Width[i].Value = 500;
                }

                PLC_Device_CCD01_04_PIN量測_量測框調整.SetComment("PLC_CCD01_04_PIN量測_量測框調整");
                PLC_Device_CCD01_04_PIN量測_量測框調整.Bool = false;
                PLC_Device_CCD01_04_PIN量測_量測框調整按鈕.Bool = false;
                cnt_Program_CCD01_04_PIN量測_量測框調整 = 65535;
            }
            if (cnt_Program_CCD01_04_PIN量測_量測框調整 == 65535) cnt_Program_CCD01_04_PIN量測_量測框調整 = 1;
            if (cnt_Program_CCD01_04_PIN量測_量測框調整 == 1) cnt_Program_CCD01_04_PIN量測_量測框調整_觸發按下(ref cnt_Program_CCD01_04_PIN量測_量測框調整);
            if (cnt_Program_CCD01_04_PIN量測_量測框調整 == 2) cnt_Program_CCD01_04_PIN量測_量測框調整_檢查按下(ref cnt_Program_CCD01_04_PIN量測_量測框調整);
            if (cnt_Program_CCD01_04_PIN量測_量測框調整 == 3) cnt_Program_CCD01_04_PIN量測_量測框調整_初始化(ref cnt_Program_CCD01_04_PIN量測_量測框調整);
            if (cnt_Program_CCD01_04_PIN量測_量測框調整 == 4) cnt_Program_CCD01_04_PIN量測_量測框調整_座標轉換(ref cnt_Program_CCD01_04_PIN量測_量測框調整);
            if (cnt_Program_CCD01_04_PIN量測_量測框調整 == 5) cnt_Program_CCD01_04_PIN量測_量測框調整_讀取參數(ref cnt_Program_CCD01_04_PIN量測_量測框調整);
            if (cnt_Program_CCD01_04_PIN量測_量測框調整 == 6) cnt_Program_CCD01_04_PIN量測_量測框調整_開始區塊分析(ref cnt_Program_CCD01_04_PIN量測_量測框調整);
            if (cnt_Program_CCD01_04_PIN量測_量測框調整 == 7) cnt_Program_CCD01_04_PIN量測_量測框調整_繪製畫布(ref cnt_Program_CCD01_04_PIN量測_量測框調整);
            if (cnt_Program_CCD01_04_PIN量測_量測框調整 == 8) cnt_Program_CCD01_04_PIN量測_量測框調整 = 65500;
            if (cnt_Program_CCD01_04_PIN量測_量測框調整 > 1) cnt_Program_CCD01_04_PIN量測_量測框調整_檢查放開(ref cnt_Program_CCD01_04_PIN量測_量測框調整);

            if (cnt_Program_CCD01_04_PIN量測_量測框調整 == 65500)
            {
                if(PLC_Device_CCD01_04_計算一次.Bool)
                {
                    PLC_Device_CCD01_04_PIN量測_量測框調整按鈕.Bool = false;
                }
                PLC_Device_CCD01_04_PIN量測_量測框調整.Bool = false;
                cnt_Program_CCD01_04_PIN量測_量測框調整 = 65535;
            }
        }
        void cnt_Program_CCD01_04_PIN量測_量測框調整_觸發按下(ref int cnt)
        {
            if (PLC_Device_CCD01_04_PIN量測_量測框調整按鈕.Bool)
            {
                PLC_Device_CCD01_04_PIN量測_量測框調整.Bool = true;
                cnt++;
            }
            
        }
        void cnt_Program_CCD01_04_PIN量測_量測框調整_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_04_PIN量測_量測框調整.Bool)
            {
                cnt++;
            }

        }
        void cnt_Program_CCD01_04_PIN量測_量測框調整_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_04_PIN量測_量測框調整按鈕.Bool)
            {
                PLC_Device_CCD01_04_PIN量測_量測框調整.Bool = false;
                cnt = 65500;
            }
        }
        void cnt_Program_CCD01_04_PIN量測_量測框調整_初始化(ref int cnt)
        {
            this.MyTimer_CCD01_04_PIN量測_量測框調整.TickStop();
            this.MyTimer_CCD01_04_PIN量測_量測框調整.StartTickTime(99999);
            this.List_CCD01_04_PIN量測參數_量測點 = new PointF[20];
            this.List_CCD01_04_PIN量測參數_量測點_結果 = new PointF[20];
            this.List_CCD01_04_PIN量測參數_量測點_轉換後座標 = new Point[20];
            this.List_CCD01_04_PIN量測參數_量測點_有無 = new bool[20];


            cnt++;
        }
        void cnt_Program_CCD01_04_PIN量測_量測框調整_座標轉換(ref int cnt)
        {
            if (PLC_Device_CCD01_04_計算一次.Bool)
            {
                CCD01_04_PIN量測_AxVisionInspectionFrame_量測框調整.RefPointX = PLC_Device_CCD01_03_水平基準線量測_量測中心_X.Value;
                CCD01_04_PIN量測_AxVisionInspectionFrame_量測框調整.RefPointY = PLC_Device_CCD01_03_水平基準線量測_量測中心_Y.Value;
                CCD01_04_PIN量測_AxVisionInspectionFrame_量測框調整.RefAngle = 0;
                CCD01_04_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentRefPointX = Point_CCD01_03_中心基準座標_量測點.X;
                CCD01_04_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentRefPointY = Point_CCD01_03_中心基準座標_量測點.Y;
                CCD01_04_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentRefAngle = 0;
                CCD01_04_PIN量測_AxVisionInspectionFrame_量測框調整.NumOfVisionPoints = 20;

                for (int j = 0; j < 20; j++)
                {
                    if (this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX[j].Value == 0) this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX[j].Value = 100;
                    if (this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY[j].Value == 0) this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY[j].Value = 100;
                    if (this.List_PLC_Device_CCD01_04_PIN量測參數_Width[j].Value == 0) this.List_PLC_Device_CCD01_04_PIN量測參數_Width[j].Value = 100;
                    if (this.List_PLC_Device_CCD01_04_PIN量測參數_Height[j].Value == 0) this.List_PLC_Device_CCD01_04_PIN量測參數_Height[j].Value = 100;

                    CCD01_04_PIN量測_AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
                    CCD01_04_PIN量測_AxVisionInspectionFrame_量測框調整.VisionPointX = this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX[j].Value;
                    CCD01_04_PIN量測_AxVisionInspectionFrame_量測框調整.VisionPointY = this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY[j].Value;
                }
                CCD01_04_PIN量測_AxVisionInspectionFrame_量測框調整.EstimateCurrentVisionPoints();
                for (int j = 0; j < 20; j++)
                {
                    CCD01_04_PIN量測_AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
                    List_CCD01_04_PIN量測參數_量測點_轉換後座標[j].X = (int)CCD01_04_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentVisionPointX;
                    List_CCD01_04_PIN量測參數_量測點_轉換後座標[j].Y = (int)CCD01_04_PIN量測_AxVisionInspectionFrame_量測框調整.CurrentVisionPointY;
                }
            }
            cnt++;

        }
        void cnt_Program_CCD01_04_PIN量測_量測框調整_讀取參數(ref int cnt)
        {
            for (int i = 0; i < this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {
                if (this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX[i].Value > 2596) this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX[i].Value = 0;
                if (this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY[i].Value > 1922) this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY[i].Value = 0;
                if (this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX[i].Value < 0) this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX[i].Value = 0;
                if (this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY[i].Value < 0) this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY[i].Value = 0;

                if (this.List_CCD01_04_PIN量測參數_量測點_轉換後座標[i].X > 2596) this.List_CCD01_04_PIN量測參數_量測點_轉換後座標[i].X = 0;
                if (this.List_CCD01_04_PIN量測參數_量測點_轉換後座標[i].Y > 1922) this.List_CCD01_04_PIN量測參數_量測點_轉換後座標[i].Y = 0;
                if (this.List_CCD01_04_PIN量測參數_量測點_轉換後座標[i].X < 0) this.List_CCD01_04_PIN量測參數_量測點_轉換後座標[i].X = 0;
                if (this.List_CCD01_04_PIN量測參數_量測點_轉換後座標[i].Y < 0) this.List_CCD01_04_PIN量測參數_量測點_轉換後座標[i].Y = 0;
            }
            for (int i = 0; i < this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {
                this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].ParentHandle = this.CCD01_04_SrcImageHandle;
                if (PLC_Device_CCD01_04_計算一次.Bool)
                {
                    this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].OrgX = List_CCD01_04_PIN量測參數_量測點_轉換後座標[i].X;
                    this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].OrgY = List_CCD01_04_PIN量測參數_量測點_轉換後座標[i].Y;
                }
                else
                {
                    this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].OrgX = this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX[i].Value;
                    this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].OrgY = this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY[i].Value;
                }
                this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].ROIWidth = this.List_PLC_Device_CCD01_04_PIN量測參數_Width[i].Value;
                this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].ROIHeight = this.List_PLC_Device_CCD01_04_PIN量測參數_Height[i].Value;
                this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].SkewAngle = 0;
            }
            cnt++;
        }
        void cnt_Program_CCD01_04_PIN量測_量測框調整_開始區塊分析(ref int cnt)
        {
            uint object_value = 4294963615;

            for (int i = 0; i < this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {

                this.List_CCD01_04_PIN量測_AxObject_區塊分析[i].SrcImageHandle = this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].VegaHandle;
                this.List_CCD01_04_PIN量測_AxObject_區塊分析[i].ObjectClass = AxOvkBlob.TxAxObjClass.AX_OBJECT_DETECT_LIGHTER_CLASS;
                this.List_CCD01_04_PIN量測_AxObject_區塊分析[i].HighThreshold = List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值[0].Value;
                if (this.CCD01_04_SrcImageHandle != 0)
                {
                    if (this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX[i].Value + this.List_PLC_Device_CCD01_04_PIN量測參數_Width[i].Value < 2596 &&
                        this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY[i].Value + this.List_PLC_Device_CCD01_04_PIN量測參數_Height[i].Value < 1922 &&
                        this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX[i].Value > 0 && this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX[i].Value > 0)
                    {
                        this.List_CCD01_04_PIN量測_AxObject_區塊分析[i].BlobAnalyze(false);
                    }


                }
                this.List_CCD01_04_PIN量測_AxObject_區塊分析[i].CalculateFeatures((int)object_value, -1);
                this.List_CCD01_04_PIN量測_AxObject_區塊分析[i].SortObjects(AxOvkBlob.TxAxObjFeatureSortOrder.AX_OBJECT_SORT_ORDER_LARGE_TO_SMALL, AxOvkBlob.TxAxObjFeature.AX_OBJECT_FEATURE_AREA, 0, -1);
                this.List_CCD01_04_PIN量測_AxObject_區塊分析[i].SelectObjects(AxOvkBlob.TxAxObjFeature.AX_OBJECT_FEATURE_AREA, AxOvkBlob.TxAxObjFeatureOperation.AX_OBJECT_REMOVE_LESS_THAN, this.List_PLC_Device_CCD01_04_PIN量測參數_面積下限[0].Value);
                this.List_CCD01_04_PIN量測_AxObject_區塊分析[i].SelectObjects(AxOvkBlob.TxAxObjFeature.AX_OBJECT_FEATURE_AREA, AxOvkBlob.TxAxObjFeatureOperation.AX_OBJECT_REMOVE_GREAT_THAN, this.List_PLC_Device_CCD01_04_PIN量測參數_面積上限[0].Value);
                if (this.List_CCD01_04_PIN量測_AxObject_區塊分析[i].DetectedNumObjs > 0)
                {
                    this.List_CCD01_04_PIN量測參數_量測點_有無[i] = true;
                    this.List_CCD01_04_PIN量測_AxObject_區塊分析[i].BlobIndex = 0;
                    this.List_CCD01_04_PIN量測參數_量測點[i].X = (float)this.List_CCD01_04_PIN量測_AxObject_區塊分析[i].BlobCentroidX;
                    this.List_CCD01_04_PIN量測參數_量測點[i].X += this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].OrgX;
                    //this.List_CCD01_04_PIN量測參數_量測點[i].Y = (float)this.List_CCD01_04_PIN量測_AxObject_區塊分析[i].BlobCentroidY;
                    this.List_CCD01_04_PIN量測參數_量測點[i].Y = (float)this.List_CCD01_04_PIN量測_AxObject_區塊分析[i].BlobCentroidY + (float)this.List_CCD01_04_PIN量測_AxObject_區塊分析[i].BlobLimBoxHeight / 2;
                    this.List_CCD01_04_PIN量測參數_量測點[i].Y += this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整[i].OrgY;
                }


            }

            cnt++;
        }
        void cnt_Program_CCD01_04_PIN量測_量測框調整_繪製畫布(ref int cnt)
        {
            this.PLC_Device_CCD01_04_PIN量測_量測框調整_RefreshCanvas.Bool = true;
            if (this.PLC_Device_CCD01_04_PIN量測_量測框調整按鈕.Bool && !PLC_Device_CCD01_04_計算一次.Bool)
            {
                this.h_Canvas_Tech_CCD01_04.RefreshCanvas();
            }

            cnt++;
        }





        #endregion
        #region PLC_CCD01_04_PIN量測點量測

        private List<AxOvkBase.AxROIBW8> List_CCD01_04_PIN量測點_AxROIBW8_量測框調整 = new List<AxOvkBase.AxROIBW8>();
        private List<AxOvkBase.AxROIBW8> List_CCD01_04_左側端點_AxROIBW8_量測框調整 = new List<AxOvkBase.AxROIBW8>();
        private List<AxOvkMsr.AxLineMsr> List_CCD01_04_左側端點_AxLineMsr_線量測 = new List<AxOvkMsr.AxLineMsr>();

        private List<AxOvkBase.AxROIBW8> List_CCD01_04_塗黑遮罩_AxROIBW8 = new List<AxOvkBase.AxROIBW8>();
        private AxOvkImage.AxImageSetValue CCD01_04_塗黑 = new AxOvkImage.AxImageSetValue();

        private AxOvkPat.AxVisionInspectionFrame CCD01_04_PIN量測點_AxVisionInspectionFrame_量測框調整;


        private List<PLC_Device> PLC_Device_CCD01_04_量測框OrgX = new List<PLC_Device>();
        private List<PLC_Device> PLC_Device_CCD01_04_量測框OrgY = new List<PLC_Device>();
        private List<PLC_Device> PLC_Device_CCD01_04_量測框Height = new List<PLC_Device>();
        private List<PLC_Device> PLC_Device_CCD01_04_量測框Width = new List<PLC_Device>();

        private List<PLC_Device> PLC_Device_CCD01_04_左側端點OrgX = new List<PLC_Device>();
        private List<PLC_Device> PLC_Device_CCD01_04_左側端點OrgY = new List<PLC_Device>();
        private List<PLC_Device> PLC_Device_CCD01_04_左側端點Height = new List<PLC_Device>();
        private List<PLC_Device> PLC_Device_CCD01_04_左側端點Width = new List<PLC_Device>();


        PLC_Device PLC_Device_CCD01_04_PIN量測點_量測框調整按鈕 = new PLC_Device("S6900");
        PLC_Device PLC_Device_CCD01_04_PIN量測點_量測框調整 = new PLC_Device("S6901");
        PLC_Device PLC_Device_CCD01_04_PIN量測點_量測框調整_OK = new PLC_Device("S6902");
        PLC_Device PLC_Device_CCD01_04_PIN量測點_量測框調整_測試完成 = new PLC_Device("S6903");
        PLC_Device PLC_Device_CCD01_04_PIN量測點_量測框調整_RefreshCanvas = new PLC_Device("S6904");
        #region PIN量測點
        PLC_Device PLC_Device_CCD01_04_PIN端點_變化銳利度 = new PLC_Device("F13400");
        PLC_Device PLC_Device_CCD01_04_PIN端點_延伸變化強度 = new PLC_Device("F13401");
        PLC_Device PLC_Device_CCD01_04_PIN端點_灰階變化面積 = new PLC_Device("F13402");
        PLC_Device PLC_Device_CCD01_04_PIN端點_雜訊抑制 = new PLC_Device("F13403");
        PLC_Device PLC_Device_CCD01_04_PIN端點_垂直量測寬度 = new PLC_Device("F13404");
        PLC_Device PLC_Device_CCD01_04_PIN端點_左端量測顏色變化 = new PLC_Device("F13405");
        PLC_Device PLC_Device_CCD01_04_PIN端點_量測密度間隔 = new PLC_Device("F13406");
        PLC_Device PLC_Device_CCD01_04_PIN端點_最佳回歸線計算次數 = new PLC_Device("F13407");
        PLC_Device PLC_Device_CCD01_04_PIN端點_最佳回歸線濾波 = new PLC_Device("F13408");
        PLC_Device PLC_Device_CCD01_04_PIN端點_量測框架方向 = new PLC_Device("F13409");
        PLC_Device PLC_Device_CCD01_04_PIN端點_右端量測顏色變化 = new PLC_Device("F13410");
        #endregion


        #region PLC_Device_CCD01_04_量測框OrgX
        PLC_Device PLC_Device_CCD01_04_量測框OrgX_PIN01 = new PLC_Device("F13411");
        PLC_Device PLC_Device_CCD01_04_量測框OrgX_PIN02 = new PLC_Device("F13412");
        PLC_Device PLC_Device_CCD01_04_量測框OrgX_PIN03 = new PLC_Device("F13413");
        PLC_Device PLC_Device_CCD01_04_量測框OrgX_PIN04 = new PLC_Device("F13414");
        PLC_Device PLC_Device_CCD01_04_量測框OrgX_PIN05 = new PLC_Device("F13415");
        PLC_Device PLC_Device_CCD01_04_量測框OrgX_PIN06 = new PLC_Device("F13416");
        PLC_Device PLC_Device_CCD01_04_量測框OrgX_PIN07 = new PLC_Device("F13417");
        PLC_Device PLC_Device_CCD01_04_量測框OrgX_PIN08 = new PLC_Device("F13418");
        PLC_Device PLC_Device_CCD01_04_量測框OrgX_PIN09 = new PLC_Device("F13419");
        PLC_Device PLC_Device_CCD01_04_量測框OrgX_PIN10 = new PLC_Device("F13420");
        PLC_Device PLC_Device_CCD01_04_量測框OrgX_PIN11 = new PLC_Device("F13421");
        PLC_Device PLC_Device_CCD01_04_量測框OrgX_PIN12 = new PLC_Device("F13422");
        PLC_Device PLC_Device_CCD01_04_量測框OrgX_PIN13 = new PLC_Device("F13423");
        PLC_Device PLC_Device_CCD01_04_量測框OrgX_PIN14 = new PLC_Device("F13424");
        PLC_Device PLC_Device_CCD01_04_量測框OrgX_PIN15 = new PLC_Device("F13425");
        PLC_Device PLC_Device_CCD01_04_量測框OrgX_PIN16 = new PLC_Device("F13426");
        PLC_Device PLC_Device_CCD01_04_量測框OrgX_PIN17 = new PLC_Device("F13427");
        PLC_Device PLC_Device_CCD01_04_量測框OrgX_PIN18 = new PLC_Device("F13428");
        PLC_Device PLC_Device_CCD01_04_量測框OrgX_PIN19 = new PLC_Device("F13429");
        PLC_Device PLC_Device_CCD01_04_量測框OrgX_PIN20 = new PLC_Device("F13430");
        #endregion
        #region PLC_Device_CCD01_04_量測框OrgY
        PLC_Device PLC_Device_CCD01_04_量測框OrgY_PIN01 = new PLC_Device("F13431");
        PLC_Device PLC_Device_CCD01_04_量測框OrgY_PIN02 = new PLC_Device("F13432");
        PLC_Device PLC_Device_CCD01_04_量測框OrgY_PIN03 = new PLC_Device("F13433");
        PLC_Device PLC_Device_CCD01_04_量測框OrgY_PIN04 = new PLC_Device("F13434");
        PLC_Device PLC_Device_CCD01_04_量測框OrgY_PIN05 = new PLC_Device("F13435");
        PLC_Device PLC_Device_CCD01_04_量測框OrgY_PIN06 = new PLC_Device("F13436");
        PLC_Device PLC_Device_CCD01_04_量測框OrgY_PIN07 = new PLC_Device("F13437");
        PLC_Device PLC_Device_CCD01_04_量測框OrgY_PIN08 = new PLC_Device("F13438");
        PLC_Device PLC_Device_CCD01_04_量測框OrgY_PIN09 = new PLC_Device("F13439");
        PLC_Device PLC_Device_CCD01_04_量測框OrgY_PIN10 = new PLC_Device("F13440");
        PLC_Device PLC_Device_CCD01_04_量測框OrgY_PIN11 = new PLC_Device("F13441");
        PLC_Device PLC_Device_CCD01_04_量測框OrgY_PIN12 = new PLC_Device("F13442");
        PLC_Device PLC_Device_CCD01_04_量測框OrgY_PIN13 = new PLC_Device("F13443");
        PLC_Device PLC_Device_CCD01_04_量測框OrgY_PIN14 = new PLC_Device("F13444");
        PLC_Device PLC_Device_CCD01_04_量測框OrgY_PIN15 = new PLC_Device("F13445");
        PLC_Device PLC_Device_CCD01_04_量測框OrgY_PIN16 = new PLC_Device("F13446");
        PLC_Device PLC_Device_CCD01_04_量測框OrgY_PIN17 = new PLC_Device("F13447");
        PLC_Device PLC_Device_CCD01_04_量測框OrgY_PIN18 = new PLC_Device("F13448");
        PLC_Device PLC_Device_CCD01_04_量測框OrgY_PIN19 = new PLC_Device("F13449");
        PLC_Device PLC_Device_CCD01_04_量測框OrgY_PIN20 = new PLC_Device("F13450");
        #endregion
        #region PLC_Device_CCD01_04_量測框Height
        PLC_Device PLC_Device_CCD01_04_量測框Height_PIN01 = new PLC_Device("F13451");
        PLC_Device PLC_Device_CCD01_04_量測框Height_PIN02 = new PLC_Device("F13452");
        PLC_Device PLC_Device_CCD01_04_量測框Height_PIN03 = new PLC_Device("F13453");
        PLC_Device PLC_Device_CCD01_04_量測框Height_PIN04 = new PLC_Device("F13454");
        PLC_Device PLC_Device_CCD01_04_量測框Height_PIN05 = new PLC_Device("F13455");
        PLC_Device PLC_Device_CCD01_04_量測框Height_PIN06 = new PLC_Device("F13456");
        PLC_Device PLC_Device_CCD01_04_量測框Height_PIN07 = new PLC_Device("F13457");
        PLC_Device PLC_Device_CCD01_04_量測框Height_PIN08 = new PLC_Device("F13458");
        PLC_Device PLC_Device_CCD01_04_量測框Height_PIN09 = new PLC_Device("F13459");
        PLC_Device PLC_Device_CCD01_04_量測框Height_PIN10 = new PLC_Device("F13460");
        PLC_Device PLC_Device_CCD01_04_量測框Height_PIN11 = new PLC_Device("F13461");
        PLC_Device PLC_Device_CCD01_04_量測框Height_PIN12 = new PLC_Device("F13462");
        PLC_Device PLC_Device_CCD01_04_量測框Height_PIN13 = new PLC_Device("F13463");
        PLC_Device PLC_Device_CCD01_04_量測框Height_PIN14 = new PLC_Device("F13464");
        PLC_Device PLC_Device_CCD01_04_量測框Height_PIN15 = new PLC_Device("F13465");
        PLC_Device PLC_Device_CCD01_04_量測框Height_PIN16 = new PLC_Device("F13466");
        PLC_Device PLC_Device_CCD01_04_量測框Height_PIN17 = new PLC_Device("F13467");
        PLC_Device PLC_Device_CCD01_04_量測框Height_PIN18 = new PLC_Device("F13468");
        PLC_Device PLC_Device_CCD01_04_量測框Height_PIN19 = new PLC_Device("F13469");
        PLC_Device PLC_Device_CCD01_04_量測框Height_PIN20 = new PLC_Device("F13470");
        #endregion
        #region PLC_Device_CCD01_04_量測框Width
        PLC_Device PLC_Device_CCD01_04_量測框Width_PIN01 = new PLC_Device("F13471");
        PLC_Device PLC_Device_CCD01_04_量測框Width_PIN02 = new PLC_Device("F13472");
        PLC_Device PLC_Device_CCD01_04_量測框Width_PIN03 = new PLC_Device("F13473");
        PLC_Device PLC_Device_CCD01_04_量測框Width_PIN04 = new PLC_Device("F13474");
        PLC_Device PLC_Device_CCD01_04_量測框Width_PIN05 = new PLC_Device("F13475");
        PLC_Device PLC_Device_CCD01_04_量測框Width_PIN06 = new PLC_Device("F13476");
        PLC_Device PLC_Device_CCD01_04_量測框Width_PIN07 = new PLC_Device("F13477");
        PLC_Device PLC_Device_CCD01_04_量測框Width_PIN08 = new PLC_Device("F13478");
        PLC_Device PLC_Device_CCD01_04_量測框Width_PIN09 = new PLC_Device("F13479");
        PLC_Device PLC_Device_CCD01_04_量測框Width_PIN10 = new PLC_Device("F13480");
        PLC_Device PLC_Device_CCD01_04_量測框Width_PIN11 = new PLC_Device("F13481");
        PLC_Device PLC_Device_CCD01_04_量測框Width_PIN12 = new PLC_Device("F13482");
        PLC_Device PLC_Device_CCD01_04_量測框Width_PIN13 = new PLC_Device("F13483");
        PLC_Device PLC_Device_CCD01_04_量測框Width_PIN14 = new PLC_Device("F13484");
        PLC_Device PLC_Device_CCD01_04_量測框Width_PIN15 = new PLC_Device("F13485");
        PLC_Device PLC_Device_CCD01_04_量測框Width_PIN16 = new PLC_Device("F13486");
        PLC_Device PLC_Device_CCD01_04_量測框Width_PIN17 = new PLC_Device("F13487");
        PLC_Device PLC_Device_CCD01_04_量測框Width_PIN18 = new PLC_Device("F13488");
        PLC_Device PLC_Device_CCD01_04_量測框Width_PIN19 = new PLC_Device("F13489");
        PLC_Device PLC_Device_CCD01_04_量測框Width_PIN20 = new PLC_Device("F13490");
        #endregion
        #region PLC_Device_CCD01_04_塗黑遮罩OrgX
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgX_PIN01 = new PLC_Device("F13531");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgX_PIN02 = new PLC_Device("F13532");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgX_PIN03 = new PLC_Device("F13533");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgX_PIN04 = new PLC_Device("F13534");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgX_PIN05 = new PLC_Device("F13535");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgX_PIN06 = new PLC_Device("F13536");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgX_PIN07 = new PLC_Device("F13537");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgX_PIN08 = new PLC_Device("F13538");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgX_PIN09 = new PLC_Device("F13539");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgX_PIN10 = new PLC_Device("F13540");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgX_PIN11 = new PLC_Device("F13541");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgX_PIN12 = new PLC_Device("F13542");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgX_PIN13 = new PLC_Device("F13543");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgX_PIN14 = new PLC_Device("F13544");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgX_PIN15 = new PLC_Device("F13545");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgX_PIN16 = new PLC_Device("F13546");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgX_PIN17 = new PLC_Device("F13547");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgX_PIN18 = new PLC_Device("F13548");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgX_PIN19 = new PLC_Device("F13549");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgX_PIN20 = new PLC_Device("F13450");
        #endregion
        #region PLC_Device_CCD01_04_塗黑遮罩OrgY
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgY_PIN01 = new PLC_Device("F13551");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgY_PIN02 = new PLC_Device("F13552");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgY_PIN03 = new PLC_Device("F13553");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgY_PIN04 = new PLC_Device("F13554");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgY_PIN05 = new PLC_Device("F13555");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgY_PIN06 = new PLC_Device("F13556");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgY_PIN07 = new PLC_Device("F13557");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgY_PIN08 = new PLC_Device("F13558");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgY_PIN09 = new PLC_Device("F13559");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgY_PIN10 = new PLC_Device("F13560");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgY_PIN11 = new PLC_Device("F13561");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgY_PIN12 = new PLC_Device("F13562");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgY_PIN13 = new PLC_Device("F13563");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgY_PIN14 = new PLC_Device("F13564");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgY_PIN15 = new PLC_Device("F13565");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgY_PIN16 = new PLC_Device("F13566");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgY_PIN17 = new PLC_Device("F13567");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgY_PIN18 = new PLC_Device("F13568");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgY_PIN19 = new PLC_Device("F13569");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩OrgY_PIN20 = new PLC_Device("F13570");
        #endregion
        #region PLC_Device_CCD01_04_塗黑遮罩Height
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Height_PIN01 = new PLC_Device("F13571");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Height_PIN02 = new PLC_Device("F13572");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Height_PIN03 = new PLC_Device("F13573");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Height_PIN04 = new PLC_Device("F13574");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Height_PIN05 = new PLC_Device("F13575");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Height_PIN06 = new PLC_Device("F13576");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Height_PIN07 = new PLC_Device("F13577");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Height_PIN08 = new PLC_Device("F13578");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Height_PIN09 = new PLC_Device("F13579");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Height_PIN10 = new PLC_Device("F13580");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Height_PIN11 = new PLC_Device("F13581");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Height_PIN12 = new PLC_Device("F13582");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Height_PIN13 = new PLC_Device("F13583");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Height_PIN14 = new PLC_Device("F13584");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Height_PIN15 = new PLC_Device("F13585");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Height_PIN16 = new PLC_Device("F13586");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Height_PIN17 = new PLC_Device("F13587");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Height_PIN18 = new PLC_Device("F13588");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Height_PIN19 = new PLC_Device("F13589");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Height_PIN20 = new PLC_Device("F13590");
        #endregion
        #region PLC_Device_CCD01_04_塗黑遮罩Width
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Width_PIN01 = new PLC_Device("F13591");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Width_PIN02 = new PLC_Device("F13592");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Width_PIN03 = new PLC_Device("F13593");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Width_PIN04 = new PLC_Device("F13594");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Width_PIN05 = new PLC_Device("F13595");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Width_PIN06 = new PLC_Device("F13596");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Width_PIN07 = new PLC_Device("F13597");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Width_PIN08 = new PLC_Device("F13598");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Width_PIN09 = new PLC_Device("F13599");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Width_PIN10 = new PLC_Device("F13600");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Width_PIN11 = new PLC_Device("F13601");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Width_PIN12 = new PLC_Device("F13602");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Width_PIN13 = new PLC_Device("F13603");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Width_PIN14 = new PLC_Device("F13604");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Width_PIN15 = new PLC_Device("F13605");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Width_PIN16 = new PLC_Device("F13606");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Width_PIN17 = new PLC_Device("F13607");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Width_PIN18 = new PLC_Device("F13608");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Width_PIN19 = new PLC_Device("F13609");
        PLC_Device PLC_Device_CCD01_04_塗黑遮罩Width_PIN20 = new PLC_Device("F13610");
        #endregion

        #region PLC_Device_CCD01_04_左側端點OrgX
        PLC_Device PLC_Device_CCD01_04_左側端點OrgX_PIN01 = new PLC_Device("F13621");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgX_PIN02 = new PLC_Device("F13622");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgX_PIN03 = new PLC_Device("F13623");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgX_PIN04 = new PLC_Device("F13624");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgX_PIN05 = new PLC_Device("F13625");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgX_PIN06 = new PLC_Device("F13626");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgX_PIN07 = new PLC_Device("F13627");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgX_PIN08 = new PLC_Device("F13628");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgX_PIN09 = new PLC_Device("F13629");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgX_PIN10 = new PLC_Device("F13630");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgX_PIN11 = new PLC_Device("F13631");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgX_PIN12 = new PLC_Device("F13632");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgX_PIN13 = new PLC_Device("F13633");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgX_PIN14 = new PLC_Device("F13634");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgX_PIN15 = new PLC_Device("F13635");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgX_PIN16 = new PLC_Device("F13636");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgX_PIN17 = new PLC_Device("F13637");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgX_PIN18 = new PLC_Device("F13638");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgX_PIN19 = new PLC_Device("F13639");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgX_PIN20 = new PLC_Device("F13640");
        #endregion
        #region PLC_Device_CCD01_04_左側端點OrgY
        PLC_Device PLC_Device_CCD01_04_左側端點OrgY_PIN01 = new PLC_Device("F13641");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgY_PIN02 = new PLC_Device("F13642");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgY_PIN03 = new PLC_Device("F13643");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgY_PIN04 = new PLC_Device("F13644");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgY_PIN05 = new PLC_Device("F13645");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgY_PIN06 = new PLC_Device("F13646");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgY_PIN07 = new PLC_Device("F13647");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgY_PIN08 = new PLC_Device("F13648");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgY_PIN09 = new PLC_Device("F13649");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgY_PIN10 = new PLC_Device("F13650");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgY_PIN11 = new PLC_Device("F13651");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgY_PIN12 = new PLC_Device("F13652");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgY_PIN13 = new PLC_Device("F13653");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgY_PIN14 = new PLC_Device("F13654");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgY_PIN15 = new PLC_Device("F13655");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgY_PIN16 = new PLC_Device("F13656");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgY_PIN17 = new PLC_Device("F13657");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgY_PIN18 = new PLC_Device("F13658");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgY_PIN19 = new PLC_Device("F13659");
        PLC_Device PLC_Device_CCD01_04_左側端點OrgY_PIN20 = new PLC_Device("F13660");
        #endregion
        #region PLC_Device_CCD01_04_左側端點Height
        PLC_Device PLC_Device_CCD01_04_左側端點Height_PIN01 = new PLC_Device("F13661");
        PLC_Device PLC_Device_CCD01_04_左側端點Height_PIN02 = new PLC_Device("F13662");
        PLC_Device PLC_Device_CCD01_04_左側端點Height_PIN03 = new PLC_Device("F13663");
        PLC_Device PLC_Device_CCD01_04_左側端點Height_PIN04 = new PLC_Device("F13664");
        PLC_Device PLC_Device_CCD01_04_左側端點Height_PIN05 = new PLC_Device("F13665");
        PLC_Device PLC_Device_CCD01_04_左側端點Height_PIN06 = new PLC_Device("F13666");
        PLC_Device PLC_Device_CCD01_04_左側端點Height_PIN07 = new PLC_Device("F13667");
        PLC_Device PLC_Device_CCD01_04_左側端點Height_PIN08 = new PLC_Device("F13668");
        PLC_Device PLC_Device_CCD01_04_左側端點Height_PIN09 = new PLC_Device("F13669");
        PLC_Device PLC_Device_CCD01_04_左側端點Height_PIN10 = new PLC_Device("F13670");
        PLC_Device PLC_Device_CCD01_04_左側端點Height_PIN11 = new PLC_Device("F13671");
        PLC_Device PLC_Device_CCD01_04_左側端點Height_PIN12 = new PLC_Device("F13672");
        PLC_Device PLC_Device_CCD01_04_左側端點Height_PIN13 = new PLC_Device("F13673");
        PLC_Device PLC_Device_CCD01_04_左側端點Height_PIN14 = new PLC_Device("F13674");
        PLC_Device PLC_Device_CCD01_04_左側端點Height_PIN15 = new PLC_Device("F13675");
        PLC_Device PLC_Device_CCD01_04_左側端點Height_PIN16 = new PLC_Device("F13676");
        PLC_Device PLC_Device_CCD01_04_左側端點Height_PIN17 = new PLC_Device("F13677");
        PLC_Device PLC_Device_CCD01_04_左側端點Height_PIN18 = new PLC_Device("F13678");
        PLC_Device PLC_Device_CCD01_04_左側端點Height_PIN19 = new PLC_Device("F13679");
        PLC_Device PLC_Device_CCD01_04_左側端點Height_PIN20 = new PLC_Device("F13680");
        #endregion
        #region PLC_Device_CCD01_04_左側端點Width
        PLC_Device PLC_Device_CCD01_04_左側端點Width_PIN01 = new PLC_Device("F13681");
        PLC_Device PLC_Device_CCD01_04_左側端點Width_PIN02 = new PLC_Device("F13682");
        PLC_Device PLC_Device_CCD01_04_左側端點Width_PIN03 = new PLC_Device("F13683");
        PLC_Device PLC_Device_CCD01_04_左側端點Width_PIN04 = new PLC_Device("F13684");
        PLC_Device PLC_Device_CCD01_04_左側端點Width_PIN05 = new PLC_Device("F13685");
        PLC_Device PLC_Device_CCD01_04_左側端點Width_PIN06 = new PLC_Device("F13686");
        PLC_Device PLC_Device_CCD01_04_左側端點Width_PIN07 = new PLC_Device("F13687");
        PLC_Device PLC_Device_CCD01_04_左側端點Width_PIN08 = new PLC_Device("F13688");
        PLC_Device PLC_Device_CCD01_04_左側端點Width_PIN09 = new PLC_Device("F13689");
        PLC_Device PLC_Device_CCD01_04_左側端點Width_PIN10 = new PLC_Device("F13690");
        PLC_Device PLC_Device_CCD01_04_左側端點Width_PIN11 = new PLC_Device("F13691");
        PLC_Device PLC_Device_CCD01_04_左側端點Width_PIN12 = new PLC_Device("F13692");
        PLC_Device PLC_Device_CCD01_04_左側端點Width_PIN13 = new PLC_Device("F13693");
        PLC_Device PLC_Device_CCD01_04_左側端點Width_PIN14 = new PLC_Device("F13694");
        PLC_Device PLC_Device_CCD01_04_左側端點Width_PIN15 = new PLC_Device("F13695");
        PLC_Device PLC_Device_CCD01_04_左側端點Width_PIN16 = new PLC_Device("F13696");
        PLC_Device PLC_Device_CCD01_04_左側端點Width_PIN17 = new PLC_Device("F13697");
        PLC_Device PLC_Device_CCD01_04_左側端點Width_PIN18 = new PLC_Device("F13698");
        PLC_Device PLC_Device_CCD01_04_左側端點Width_PIN19 = new PLC_Device("F13699");
        PLC_Device PLC_Device_CCD01_04_左側端點Width_PIN20 = new PLC_Device("F13700");
        #endregion


        private PointF[] List_CCD01_04_PIN量測點參數_量測點 = new PointF[20];
        private PointF[] List_CCD01_04_PIN量測點參數_量測點_結果 = new PointF[20];
        private Point[] List_CCD01_04_PIN量測點參數_量測點_轉換後座標 = new Point[20];
        private bool[] List_CCD01_04_PIN量測點參數_量測點_有無 = new bool[20];

        int cnt_Program_CCD01_04_PIN量測點量測 = 65534;
        void sub_Program_CCD01_04_PIN量測點量測()
        {
            if (cnt_Program_CCD01_04_PIN量測點量測 == 65534)
            {
                this.h_Canvas_Tech_CCD01_04.OnCanvasMouseDownEvent += H_Canvas_Tech_CCD01_04_PIN量測點量測_OnCanvasMouseDownEvent;
                this.h_Canvas_Tech_CCD01_04.OnCanvasMouseMoveEvent += H_Canvas_Tech_CCD01_04_PIN量測點量測_OnCanvasMouseMoveEvent;
                this.h_Canvas_Tech_CCD01_04.OnCanvasMouseUpEvent += H_Canvas_Tech_CCD01_04_PIN量測點量測_OnCanvasMouseUpEvent;
                this.h_Canvas_Tech_CCD01_04.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_04_PIN量測點量測_OnCanvasDrawEvent;
                this.h_Canvas_Main_CCD01_04_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_04_PIN量測點量測_OnCanvasDrawEvent;
                #region Add List
                this.PLC_Device_CCD01_04_量測框OrgX.Add(PLC_Device_CCD01_04_量測框OrgX_PIN01);
                this.PLC_Device_CCD01_04_量測框OrgX.Add(PLC_Device_CCD01_04_量測框OrgX_PIN02);
                this.PLC_Device_CCD01_04_量測框OrgX.Add(PLC_Device_CCD01_04_量測框OrgX_PIN03);
                this.PLC_Device_CCD01_04_量測框OrgX.Add(PLC_Device_CCD01_04_量測框OrgX_PIN04);
                this.PLC_Device_CCD01_04_量測框OrgX.Add(PLC_Device_CCD01_04_量測框OrgX_PIN05);
                this.PLC_Device_CCD01_04_量測框OrgX.Add(PLC_Device_CCD01_04_量測框OrgX_PIN06);
                this.PLC_Device_CCD01_04_量測框OrgX.Add(PLC_Device_CCD01_04_量測框OrgX_PIN07);
                this.PLC_Device_CCD01_04_量測框OrgX.Add(PLC_Device_CCD01_04_量測框OrgX_PIN08);
                this.PLC_Device_CCD01_04_量測框OrgX.Add(PLC_Device_CCD01_04_量測框OrgX_PIN09);
                this.PLC_Device_CCD01_04_量測框OrgX.Add(PLC_Device_CCD01_04_量測框OrgX_PIN10);
                this.PLC_Device_CCD01_04_量測框OrgX.Add(PLC_Device_CCD01_04_量測框OrgX_PIN11);
                this.PLC_Device_CCD01_04_量測框OrgX.Add(PLC_Device_CCD01_04_量測框OrgX_PIN12);
                this.PLC_Device_CCD01_04_量測框OrgX.Add(PLC_Device_CCD01_04_量測框OrgX_PIN13);
                this.PLC_Device_CCD01_04_量測框OrgX.Add(PLC_Device_CCD01_04_量測框OrgX_PIN14);
                this.PLC_Device_CCD01_04_量測框OrgX.Add(PLC_Device_CCD01_04_量測框OrgX_PIN15);
                this.PLC_Device_CCD01_04_量測框OrgX.Add(PLC_Device_CCD01_04_量測框OrgX_PIN16);
                this.PLC_Device_CCD01_04_量測框OrgX.Add(PLC_Device_CCD01_04_量測框OrgX_PIN17);
                this.PLC_Device_CCD01_04_量測框OrgX.Add(PLC_Device_CCD01_04_量測框OrgX_PIN18);
                this.PLC_Device_CCD01_04_量測框OrgX.Add(PLC_Device_CCD01_04_量測框OrgX_PIN19);
                this.PLC_Device_CCD01_04_量測框OrgX.Add(PLC_Device_CCD01_04_量測框OrgX_PIN20);

                this.PLC_Device_CCD01_04_量測框OrgY.Add(PLC_Device_CCD01_04_量測框OrgY_PIN01);
                this.PLC_Device_CCD01_04_量測框OrgY.Add(PLC_Device_CCD01_04_量測框OrgY_PIN02);
                this.PLC_Device_CCD01_04_量測框OrgY.Add(PLC_Device_CCD01_04_量測框OrgY_PIN03);
                this.PLC_Device_CCD01_04_量測框OrgY.Add(PLC_Device_CCD01_04_量測框OrgY_PIN04);
                this.PLC_Device_CCD01_04_量測框OrgY.Add(PLC_Device_CCD01_04_量測框OrgY_PIN05);
                this.PLC_Device_CCD01_04_量測框OrgY.Add(PLC_Device_CCD01_04_量測框OrgY_PIN06);
                this.PLC_Device_CCD01_04_量測框OrgY.Add(PLC_Device_CCD01_04_量測框OrgY_PIN07);
                this.PLC_Device_CCD01_04_量測框OrgY.Add(PLC_Device_CCD01_04_量測框OrgY_PIN08);
                this.PLC_Device_CCD01_04_量測框OrgY.Add(PLC_Device_CCD01_04_量測框OrgY_PIN09);
                this.PLC_Device_CCD01_04_量測框OrgY.Add(PLC_Device_CCD01_04_量測框OrgY_PIN10);
                this.PLC_Device_CCD01_04_量測框OrgY.Add(PLC_Device_CCD01_04_量測框OrgY_PIN11);
                this.PLC_Device_CCD01_04_量測框OrgY.Add(PLC_Device_CCD01_04_量測框OrgY_PIN12);
                this.PLC_Device_CCD01_04_量測框OrgY.Add(PLC_Device_CCD01_04_量測框OrgY_PIN13);
                this.PLC_Device_CCD01_04_量測框OrgY.Add(PLC_Device_CCD01_04_量測框OrgY_PIN14);
                this.PLC_Device_CCD01_04_量測框OrgY.Add(PLC_Device_CCD01_04_量測框OrgY_PIN15);
                this.PLC_Device_CCD01_04_量測框OrgY.Add(PLC_Device_CCD01_04_量測框OrgY_PIN16);
                this.PLC_Device_CCD01_04_量測框OrgY.Add(PLC_Device_CCD01_04_量測框OrgY_PIN17);
                this.PLC_Device_CCD01_04_量測框OrgY.Add(PLC_Device_CCD01_04_量測框OrgY_PIN18);
                this.PLC_Device_CCD01_04_量測框OrgY.Add(PLC_Device_CCD01_04_量測框OrgY_PIN19);
                this.PLC_Device_CCD01_04_量測框OrgY.Add(PLC_Device_CCD01_04_量測框OrgY_PIN20);

                this.PLC_Device_CCD01_04_量測框Height.Add(PLC_Device_CCD01_04_量測框Height_PIN01);
                this.PLC_Device_CCD01_04_量測框Height.Add(PLC_Device_CCD01_04_量測框Height_PIN02);
                this.PLC_Device_CCD01_04_量測框Height.Add(PLC_Device_CCD01_04_量測框Height_PIN03);
                this.PLC_Device_CCD01_04_量測框Height.Add(PLC_Device_CCD01_04_量測框Height_PIN04);
                this.PLC_Device_CCD01_04_量測框Height.Add(PLC_Device_CCD01_04_量測框Height_PIN05);
                this.PLC_Device_CCD01_04_量測框Height.Add(PLC_Device_CCD01_04_量測框Height_PIN06);
                this.PLC_Device_CCD01_04_量測框Height.Add(PLC_Device_CCD01_04_量測框Height_PIN07);
                this.PLC_Device_CCD01_04_量測框Height.Add(PLC_Device_CCD01_04_量測框Height_PIN08);
                this.PLC_Device_CCD01_04_量測框Height.Add(PLC_Device_CCD01_04_量測框Height_PIN09);
                this.PLC_Device_CCD01_04_量測框Height.Add(PLC_Device_CCD01_04_量測框Height_PIN10);
                this.PLC_Device_CCD01_04_量測框Height.Add(PLC_Device_CCD01_04_量測框Height_PIN11);
                this.PLC_Device_CCD01_04_量測框Height.Add(PLC_Device_CCD01_04_量測框Height_PIN12);
                this.PLC_Device_CCD01_04_量測框Height.Add(PLC_Device_CCD01_04_量測框Height_PIN13);
                this.PLC_Device_CCD01_04_量測框Height.Add(PLC_Device_CCD01_04_量測框Height_PIN14);
                this.PLC_Device_CCD01_04_量測框Height.Add(PLC_Device_CCD01_04_量測框Height_PIN15);
                this.PLC_Device_CCD01_04_量測框Height.Add(PLC_Device_CCD01_04_量測框Height_PIN16);
                this.PLC_Device_CCD01_04_量測框Height.Add(PLC_Device_CCD01_04_量測框Height_PIN17);
                this.PLC_Device_CCD01_04_量測框Height.Add(PLC_Device_CCD01_04_量測框Height_PIN18);
                this.PLC_Device_CCD01_04_量測框Height.Add(PLC_Device_CCD01_04_量測框Height_PIN19);
                this.PLC_Device_CCD01_04_量測框Height.Add(PLC_Device_CCD01_04_量測框Height_PIN20);

                this.PLC_Device_CCD01_04_量測框Width.Add(PLC_Device_CCD01_04_量測框Width_PIN01);
                this.PLC_Device_CCD01_04_量測框Width.Add(PLC_Device_CCD01_04_量測框Width_PIN02);
                this.PLC_Device_CCD01_04_量測框Width.Add(PLC_Device_CCD01_04_量測框Width_PIN03);
                this.PLC_Device_CCD01_04_量測框Width.Add(PLC_Device_CCD01_04_量測框Width_PIN04);
                this.PLC_Device_CCD01_04_量測框Width.Add(PLC_Device_CCD01_04_量測框Width_PIN05);
                this.PLC_Device_CCD01_04_量測框Width.Add(PLC_Device_CCD01_04_量測框Width_PIN06);
                this.PLC_Device_CCD01_04_量測框Width.Add(PLC_Device_CCD01_04_量測框Width_PIN07);
                this.PLC_Device_CCD01_04_量測框Width.Add(PLC_Device_CCD01_04_量測框Width_PIN08);
                this.PLC_Device_CCD01_04_量測框Width.Add(PLC_Device_CCD01_04_量測框Width_PIN09);
                this.PLC_Device_CCD01_04_量測框Width.Add(PLC_Device_CCD01_04_量測框Width_PIN10);
                this.PLC_Device_CCD01_04_量測框Width.Add(PLC_Device_CCD01_04_量測框Width_PIN11);
                this.PLC_Device_CCD01_04_量測框Width.Add(PLC_Device_CCD01_04_量測框Width_PIN12);
                this.PLC_Device_CCD01_04_量測框Width.Add(PLC_Device_CCD01_04_量測框Width_PIN13);
                this.PLC_Device_CCD01_04_量測框Width.Add(PLC_Device_CCD01_04_量測框Width_PIN14);
                this.PLC_Device_CCD01_04_量測框Width.Add(PLC_Device_CCD01_04_量測框Width_PIN15);
                this.PLC_Device_CCD01_04_量測框Width.Add(PLC_Device_CCD01_04_量測框Width_PIN16);
                this.PLC_Device_CCD01_04_量測框Width.Add(PLC_Device_CCD01_04_量測框Width_PIN17);
                this.PLC_Device_CCD01_04_量測框Width.Add(PLC_Device_CCD01_04_量測框Width_PIN18);
                this.PLC_Device_CCD01_04_量測框Width.Add(PLC_Device_CCD01_04_量測框Width_PIN19);
                this.PLC_Device_CCD01_04_量測框Width.Add(PLC_Device_CCD01_04_量測框Width_PIN20);

                #endregion

                PLC_Device_CCD01_04_PIN量測點_量測框調整按鈕.Bool = false;
                PLC_Device_CCD01_04_PIN量測點_量測框調整.Bool = false;
                cnt_Program_CCD01_04_PIN量測點量測 = 65535;
            }
            if (cnt_Program_CCD01_04_PIN量測點量測 == 65535) cnt_Program_CCD01_04_PIN量測點量測 = 1;
            if (cnt_Program_CCD01_04_PIN量測點量測 == 1) cnt_Program_CCD01_04_PIN量測點量測_檢查按下(ref cnt_Program_CCD01_04_PIN量測點量測);
            if (cnt_Program_CCD01_04_PIN量測點量測 == 2) cnt_Program_CCD01_04_PIN量測點量測_初始化(ref cnt_Program_CCD01_04_PIN量測點量測);
            if (cnt_Program_CCD01_04_PIN量測點量測 == 3) cnt_Program_CCD01_04_PIN量測點量測_座標轉換(ref cnt_Program_CCD01_04_PIN量測點量測);
            if (cnt_Program_CCD01_04_PIN量測點量測 == 4) cnt_Program_CCD01_04_PIN量測點量測_讀取參數(ref cnt_Program_CCD01_04_PIN量測點量測);
            if (cnt_Program_CCD01_04_PIN量測點量測 == 5) cnt_Program_CCD01_04_PIN量測點量測_開始點量測(ref cnt_Program_CCD01_04_PIN量測點量測);
            if (cnt_Program_CCD01_04_PIN量測點量測 == 6) cnt_Program_CCD01_04_PIN量測點量測_定位量測點(ref cnt_Program_CCD01_04_PIN量測點量測);
            if (cnt_Program_CCD01_04_PIN量測點量測 == 7) cnt_Program_CCD01_04_PIN量測點量測_繪製畫布(ref cnt_Program_CCD01_04_PIN量測點量測);
            if (cnt_Program_CCD01_04_PIN量測點量測 == 8) cnt_Program_CCD01_04_PIN量測點量測 = 65500;
            if (cnt_Program_CCD01_04_PIN量測點量測 > 1) cnt_Program_CCD01_04_PIN量測點量測_檢查放開(ref cnt_Program_CCD01_04_PIN量測點量測);

            if (cnt_Program_CCD01_04_PIN量測點量測 == 65500)
            {
                if (PLC_Device_CCD01_04_計算一次.Bool)
                {
                    PLC_Device_CCD01_04_PIN量測點_量測框調整按鈕.Bool = false;
                    PLC_Device_CCD01_04_PIN量測點_量測框調整.Bool = false;
                }
                cnt_Program_CCD01_04_PIN量測點量測 = 65535;
            }
        }
        AxOvkBase.TxAxHitHandle[] CCD01_04_PIN量測點_AxROIBW8_TxAxHitHandle = new AxOvkBase.TxAxHitHandle[20];
        bool[] flag_CCD01_04_PIN量測點_AxROIBW8_MouseDown = new bool[20];
        bool[] flag_CCD01_04_左側端點_AxROIBW8_MouseDown = new bool[20];
        bool[] flag_CCD01_04_右側端點_AxROIBW8_MouseDown = new bool[20];
        private void H_Canvas_Tech_CCD01_04_PIN量測點量測_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {
            if (PLC_Device_CCD01_04_Main_取像並檢驗.Bool || PLC_Device_CCD01_04_PLC觸發檢測.Bool || PLC_Device_CCD01_04_Main_檢驗一次.Bool)
            {
                try
                {
                    Graphics g = Graphics.FromHdc((IntPtr)HDC);

                    for (int i = 0; i < 20; i++)
                    {
                        if (plC_CheckBox_CCD01_04_PIN線段量測點_繪製量測線段.Checked)
                        {
                            this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].DrawFittedPrimitives(HDC, ZoomX, ZoomY, 0, 0);
                        }
                        DrawingClass.Draw.十字中心(this.List_CCD01_04_PIN量測點參數_量測點[i], 50, Color.Red, 2, g, ZoomX, ZoomY);

                    }
                    g.Dispose();
                    g = null;
                }
                catch
                {

                }

            }
            else if (PLC_Device_CCD01_04_Tech_檢驗一次.Bool || PLC_Device_CCD01_04_Tech_取像並檢驗.Bool)
            {
                if (this.PLC_Device_CCD01_04_PIN量測點_量測框調整_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);

                        for (int i = 0; i < 20; i++)
                        {
                            if (plC_CheckBox_CCD01_04_PIN線段量測點_繪製量測線段.Checked)
                            {
                                this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].DrawFittedPrimitives(HDC, ZoomX, ZoomY, 0, 0);
                            }
                            DrawingClass.Draw.十字中心(this.List_CCD01_04_PIN量測點參數_量測點[i], 50, Color.Red, 2, g, ZoomX, ZoomY);
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
                if (this.PLC_Device_CCD01_04_PIN量測點_量測框調整_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        PointF po_str_PIN到基準Y = new PointF(200, 250);
                        Font font = new Font("微軟正黑體", 10);

                        for (int i = 0; i < 20; i++)
                        {
                            if (plC_CheckBox_CCD01_04_PIN線段量測點_繪製量測框.Checked)
                            {
                                this.List_CCD01_04_PIN量測點_AxROIBW8_量測框調整[i].Title = string.Format("P{0}", (i + 1).ToString("00"));
                                this.List_CCD01_04_PIN量測點_AxROIBW8_量測框調整[i].ShowPlacement = false;
                                this.List_CCD01_04_PIN量測點_AxROIBW8_量測框調整[i].DrawFrame(HDC, ZoomX, ZoomY, 0, 0, 0xFFFF00);
                                this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].DrawFrame(HDC, ZoomX, ZoomY, 0, 0);
                            }

                            if (plC_CheckBox_CCD01_04_PIN線段量測點_繪製量測線段.Checked)
                            {
                                this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].DrawFittedPrimitives(HDC, ZoomX, ZoomY, 0, 0);
                            }
                            if (plC_CheckBox_CCD01_04_PIN線段量測點_繪製量測點.Checked)
                            {
                                this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].DrawPoints(HDC, ZoomX, ZoomY, 0, 0);
                            }
                            DrawingClass.Draw.十字中心(this.List_CCD01_04_PIN量測點參數_量測點[i], 50, Color.Red, 2, g, ZoomX, ZoomY);

                            //this.List_CCD01_04_塗黑遮罩_AxROIBW8[i].DrawFrame(HDC, ZoomX, ZoomY, 0, 0, 0xFF0000);

                        }
                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }


                }
            }

            this.PLC_Device_CCD01_04_PIN量測點_量測框調整_RefreshCanvas.Bool = false;


        }
        private void H_Canvas_Tech_CCD01_04_PIN量測點量測_OnCanvasMouseDownEvent(int x, int y, float ZoomX, float ZoomY, ref int InUsedEventNum, int InUsedCanvasHandle)
        {
            if (this.PLC_Device_CCD01_04_PIN量測點_量測框調整.Bool)
            {
                for (int i = 0; i < 20; i++)
                {
                    this.CCD01_04_PIN量測點_AxROIBW8_TxAxHitHandle[i] = this.List_CCD01_04_PIN量測點_AxROIBW8_量測框調整[i].HitTest(x, y, ZoomX, ZoomY, 0, 0);
                    if (this.CCD01_04_PIN量測點_AxROIBW8_TxAxHitHandle[i] != AxOvkBase.TxAxHitHandle.AX_HANDLE_NONE)
                    {
                        this.flag_CCD01_04_PIN量測點_AxROIBW8_MouseDown[i] = true;
                        InUsedEventNum = 10;
                        return;
                    }
                }

            }

        }
        private void H_Canvas_Tech_CCD01_04_PIN量測點量測_OnCanvasMouseMoveEvent(int x, int y, float ZoomX, float ZoomY)
        {
            for (int i = 0; i < 20; i++)
            {
                if (this.flag_CCD01_04_PIN量測點_AxROIBW8_MouseDown[i])
                {
                    this.List_CCD01_04_PIN量測點_AxROIBW8_量測框調整[i].DragROI(this.CCD01_04_PIN量測點_AxROIBW8_TxAxHitHandle[i], x, y, ZoomX, ZoomY, 0, 0);
                    // this.List_CCD01_04_左側端點_AxROIBW8_量測框調整[i].DragROI(this.CCD01_04_左側端點_AxROIBW8_TxAxHitHandle[i], x, y, ZoomX, ZoomY, 0, 0);
                    this.PLC_Device_CCD01_04_量測框OrgX[i].Value = this.List_CCD01_04_PIN量測點_AxROIBW8_量測框調整[i].OrgX;
                    this.PLC_Device_CCD01_04_量測框OrgY[i].Value = this.List_CCD01_04_PIN量測點_AxROIBW8_量測框調整[i].OrgY;
                    this.PLC_Device_CCD01_04_量測框Width[i].Value = this.List_CCD01_04_PIN量測點_AxROIBW8_量測框調整[i].ROIWidth;
                    this.PLC_Device_CCD01_04_量測框Height[i].Value = this.List_CCD01_04_PIN量測點_AxROIBW8_量測框調整[i].ROIHeight;


                }

            }

        }
        private void H_Canvas_Tech_CCD01_04_PIN量測點量測_OnCanvasMouseUpEvent(int x, int y, float ZoomX, float ZoomY)
        {
            for (int i = 0; i < 20; i++)
            {
                this.flag_CCD01_04_PIN量測點_AxROIBW8_MouseDown[i] = false;
            }
        }

        void cnt_Program_CCD01_04_PIN量測點量測_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_04_PIN量測點_量測框調整按鈕.Bool || PLC_Device_CCD01_04_計算一次.Bool)
            {
                PLC_Device_CCD01_04_PIN量測點_量測框調整.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_04_PIN量測點量測_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_04_PIN量測點_量測框調整.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_04_PIN量測點量測_初始化(ref int cnt)
        {
            this.List_CCD01_04_PIN量測點參數_量測點 = new PointF[20];
            this.List_CCD01_04_PIN量測點參數_量測點_結果 = new PointF[20];
            this.List_CCD01_04_PIN量測點參數_量測點_轉換後座標 = new Point[20];
            this.List_CCD01_04_PIN量測點參數_量測點_有無 = new bool[20];
            cnt++;
        }
        void cnt_Program_CCD01_04_PIN量測點量測_座標轉換(ref int cnt)
        {
            if (PLC_Device_CCD01_04_計算一次.Bool)
            {
                CCD01_04_PIN量測點_AxVisionInspectionFrame_量測框調整.RefPointX = PLC_Device_CCD01_03_水平基準線量測_量測中心_X.Value;
                CCD01_04_PIN量測點_AxVisionInspectionFrame_量測框調整.RefPointY = PLC_Device_CCD01_03_水平基準線量測_量測中心_Y.Value;
                CCD01_04_PIN量測點_AxVisionInspectionFrame_量測框調整.RefAngle = 0;
                CCD01_04_PIN量測點_AxVisionInspectionFrame_量測框調整.CurrentRefPointX = Point_CCD01_03_中心基準座標_量測點.X;
                CCD01_04_PIN量測點_AxVisionInspectionFrame_量測框調整.CurrentRefPointY = Point_CCD01_03_中心基準座標_量測點.Y;
                CCD01_04_PIN量測點_AxVisionInspectionFrame_量測框調整.CurrentRefAngle = 0;
                CCD01_04_PIN量測點_AxVisionInspectionFrame_量測框調整.NumOfVisionPoints = 20;



                for (int j = 0; j < 20; j++)
                {
                    if (this.PLC_Device_CCD01_04_量測框OrgX[j].Value == 0) this.PLC_Device_CCD01_04_量測框OrgX[j].Value = 100;
                    if (this.PLC_Device_CCD01_04_量測框OrgY[j].Value == 0) this.PLC_Device_CCD01_04_量測框OrgY[j].Value = 100;
                    if (this.PLC_Device_CCD01_04_量測框Width[j].Value == 0) this.PLC_Device_CCD01_04_量測框Width[j].Value = 100;
                    if (this.PLC_Device_CCD01_04_量測框Height[j].Value == 0) this.PLC_Device_CCD01_04_量測框Height[j].Value = 100;

                    CCD01_04_PIN量測點_AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
                    CCD01_04_PIN量測點_AxVisionInspectionFrame_量測框調整.VisionPointX = this.PLC_Device_CCD01_04_量測框OrgX[j].Value;
                    CCD01_04_PIN量測點_AxVisionInspectionFrame_量測框調整.VisionPointY = this.PLC_Device_CCD01_04_量測框OrgY[j].Value;
                }
                CCD01_04_PIN量測點_AxVisionInspectionFrame_量測框調整.EstimateCurrentVisionPoints();
                for (int j = 0; j < 20; j++)
                {
                    CCD01_04_PIN量測點_AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
                    List_CCD01_04_PIN量測點參數_量測點_轉換後座標[j].X = (int)CCD01_04_PIN量測點_AxVisionInspectionFrame_量測框調整.CurrentVisionPointX;
                    List_CCD01_04_PIN量測點參數_量測點_轉換後座標[j].Y = (int)CCD01_04_PIN量測點_AxVisionInspectionFrame_量測框調整.CurrentVisionPointY;
                }

            }
            cnt++;

        }
        void cnt_Program_CCD01_04_PIN量測點量測_讀取參數(ref int cnt)
        {
            for (int i = 0; i < 20; i++)
            {
                if (this.PLC_Device_CCD01_04_量測框OrgX[i].Value > 2596) this.PLC_Device_CCD01_04_量測框OrgX[i].Value = 0;
                if (this.PLC_Device_CCD01_04_量測框OrgY[i].Value > 1922) this.PLC_Device_CCD01_04_量測框OrgY[i].Value = 0;
                if (this.PLC_Device_CCD01_04_量測框OrgX[i].Value < 0) this.PLC_Device_CCD01_04_量測框OrgX[i].Value = 0;
                if (this.PLC_Device_CCD01_04_量測框OrgY[i].Value < 0) this.PLC_Device_CCD01_04_量測框OrgY[i].Value = 0;

                if (this.List_CCD01_04_PIN量測點參數_量測點_轉換後座標[i].X > 2596) this.List_CCD01_04_PIN量測點參數_量測點_轉換後座標[i].X = 0;
                if (this.List_CCD01_04_PIN量測點參數_量測點_轉換後座標[i].Y > 1922) this.List_CCD01_04_PIN量測點參數_量測點_轉換後座標[i].Y = 0;
                if (this.List_CCD01_04_PIN量測點參數_量測點_轉換後座標[i].X < 0) this.List_CCD01_04_PIN量測點參數_量測點_轉換後座標[i].X = 0;
                if (this.List_CCD01_04_PIN量測點參數_量測點_轉換後座標[i].Y < 0) this.List_CCD01_04_PIN量測點參數_量測點_轉換後座標[i].Y = 0;
            }
            for (int i = 0; i < 20; i++)
            {
                this.List_CCD01_04_PIN量測點_AxROIBW8_量測框調整[i].ParentHandle = this.CCD01_04_SrcImageHandle;
                if (PLC_Device_CCD01_04_計算一次.Bool)
                {
                    this.List_CCD01_04_PIN量測點_AxROIBW8_量測框調整[i].OrgX = List_CCD01_04_PIN量測點參數_量測點_轉換後座標[i].X;
                    this.List_CCD01_04_PIN量測點_AxROIBW8_量測框調整[i].OrgY = List_CCD01_04_PIN量測點參數_量測點_轉換後座標[i].Y;
                }
                else
                {
                    this.List_CCD01_04_PIN量測點_AxROIBW8_量測框調整[i].OrgX = this.PLC_Device_CCD01_04_量測框OrgX[i].Value;
                    this.List_CCD01_04_PIN量測點_AxROIBW8_量測框調整[i].OrgY = this.PLC_Device_CCD01_04_量測框OrgY[i].Value;
                }

                this.List_CCD01_04_PIN量測點_AxROIBW8_量測框調整[i].ROIWidth = this.PLC_Device_CCD01_04_量測框Width[i].Value;
                this.List_CCD01_04_PIN量測點_AxROIBW8_量測框調整[i].ROIHeight = this.PLC_Device_CCD01_04_量測框Height[i].Value;
                this.List_CCD01_04_PIN量測點_AxROIBW8_量測框調整[i].SkewAngle = 0;
            }


            cnt++;
        }
        void cnt_Program_CCD01_04_PIN量測點量測_開始點量測(ref int cnt)
        {

            for (int i = 0; i < 20; i++)
            {

                //this.CCD01_04_塗黑.SrcImageHandle = this.List_CCD01_04_塗黑遮罩_AxROIBW8[i].VegaHandle;
                //this.List_CCD01_04_塗黑遮罩_AxROIBW8[i].ROIWidth = this.PLC_Device_CCD01_04_量測框Width[i].Value / 2;
                //this.List_CCD01_04_塗黑遮罩_AxROIBW8[i].ROIHeight = this.PLC_Device_CCD01_04_量測框Height[i].Value / 2;
                //this.List_CCD01_04_塗黑遮罩_AxROIBW8[i].SkewAngle = 0;
                //this.CCD01_04_塗黑.Greylevel = 0;
                //this.CCD01_04_塗黑.SetValue();


                this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].SrcImageHandle = this.CCD01_04_SrcImageHandle;
                this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].Hysteresis = PLC_Device_CCD01_04_PIN端點_延伸變化強度.Value;
                this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].DeriThreshold = PLC_Device_CCD01_04_PIN端點_變化銳利度.Value;
                this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].MinGreyStep = PLC_Device_CCD01_04_PIN端點_灰階變化面積.Value;
                this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].SmoothFactor = PLC_Device_CCD01_04_PIN端點_雜訊抑制.Value;
                this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].HalfProfileThickness = PLC_Device_CCD01_04_PIN端點_垂直量測寬度.Value;
                this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].SampleStep = PLC_Device_CCD01_04_PIN端點_量測密度間隔.Value;
                this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].FilterCount = PLC_Device_CCD01_04_PIN端點_最佳回歸線計算次數.Value;
                this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].FilterThreshold = PLC_Device_CCD01_04_PIN端點_最佳回歸線濾波.Value / 10;
                this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].HalfHeight = this.PLC_Device_CCD01_04_量測框Width[i].Value / 2;
                this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].NLineStartX = this.PLC_Device_CCD01_04_量測框OrgX[i].Value + this.PLC_Device_CCD01_04_量測框Width[i].Value / 2;
                this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].NLineStartY = this.PLC_Device_CCD01_04_量測框OrgY[i].Value;
                this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].NLineEndX = this.PLC_Device_CCD01_04_量測框OrgX[i].Value + this.PLC_Device_CCD01_04_量測框Width[i].Value / 2;
                this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].NLineEndY = this.PLC_Device_CCD01_04_量測框OrgY[i].Value + this.PLC_Device_CCD01_04_量測框Height[i].Value;

                this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].EdgeType = (AxOvkMsr.TxAxTransitionType)PLC_Device_CCD01_04_PIN端點_左端量測顏色變化.Value;
                this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].LockedMsrDirection = (AxOvkMsr.TxAxLineMsrLockedMsrDirection)PLC_Device_CCD01_04_PIN端點_量測框架方向.Value; //右
                this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].DetectPrimitives();


            }

            cnt++;
        }
        void cnt_Program_CCD01_04_PIN量測點量測_定位量測點(ref int cnt)
        {
            for (int i = 0; i < 20; i++)
            {

                if (this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].LineIsFitted)
                {
                    this.List_CCD01_04_PIN量測點參數_量測點_有無[i] = true;
                    this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].ValidPointIndex = 0;
                    this.List_CCD01_04_PIN量測點參數_量測點[i].X = (float)this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].ValidPointX;
                    this.List_CCD01_04_PIN量測點參數_量測點[i].Y = (float)this.List_CCD01_04_左側端點_AxLineMsr_線量測[i].ValidPointY;
                }
            }

            cnt++;
        }
        void cnt_Program_CCD01_04_PIN量測點量測_繪製畫布(ref int cnt)
        {
            this.PLC_Device_CCD01_04_PIN量測點_量測框調整_RefreshCanvas.Bool = true;
            if (this.PLC_Device_CCD01_04_PIN量測點_量測框調整按鈕.Bool && !PLC_Device_CCD01_04_計算一次.Bool)
            {
                this.h_Canvas_Tech_CCD01_04.RefreshCanvas();
            }

            cnt++;
        }

        #endregion
        #region PLC_CCD01_04_PIN量測_檢測距離計算
        private AxOvkMsr.AxPointLineDistanceMsr CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_上排;
        private AxOvkMsr.AxPointLineDistanceMsr CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_下排;
        MyTimer MyTimer_CCD01_04_PIN量測_檢測距離計算 = new MyTimer();
        PLC_Device PLC_Device_CCD01_04_PIN量測_檢測距離計算按鈕 = new PLC_Device("S6430");
        PLC_Device PLC_Device_CCD01_04_PIN量測_檢測距離計算 = new PLC_Device("S6425");
        PLC_Device PLC_Device_CCD01_04_PIN量測_檢測距離計算_OK = new PLC_Device("S6426");
        PLC_Device PLC_Device_CCD01_04_PIN量測_檢測距離計算_測試完成 = new PLC_Device("S6427");
        PLC_Device PLC_Device_CCD01_04_PIN量測_檢測距離計算_RefreshCanvas = new PLC_Device("S6428");

        PLC_Device PLC_Device_CCD01_04_PIN量測_水平度量測不測試 = new PLC_Device("S6104");
        PLC_Device PLC_Device_CCD01_04_PIN量測_間距量測不測試 = new PLC_Device("S6105");

        PLC_Device PLC_Device_CCD01_04_PIN量測_左右間距量測標準值 = new PLC_Device("F1350");
        PLC_Device PLC_Device_CCD01_04_PIN量測_左右間距量測上限值 = new PLC_Device("F1351");
        PLC_Device PLC_Device_CCD01_04_PIN量測_左右間距量測下限值 = new PLC_Device("F1352");
        PLC_Device PLC_Device_CCD01_04_PIN量測_上下間距量測標準值 = new PLC_Device("F1353");
        PLC_Device PLC_Device_CCD01_04_PIN量測_上下間距量測上限值 = new PLC_Device("F1354");
        PLC_Device PLC_Device_CCD01_04_PIN量測_上下間距量測下限值 = new PLC_Device("F1355");

        PLC_Device PLC_Device_CCD01_04_PIN量測_上排水平度量測標準值 = new PLC_Device("F1356");
        PLC_Device PLC_Device_CCD01_04_PIN量測_上排水平度量測上限值 = new PLC_Device("F1357");
        PLC_Device PLC_Device_CCD01_04_PIN量測_上排水平度量測下限值 = new PLC_Device("F1358");
        PLC_Device PLC_Device_CCD01_04_PIN量測_下排水平度量測標準值 = new PLC_Device("F1359");
        PLC_Device PLC_Device_CCD01_04_PIN量測_下排水平度量測上限值 = new PLC_Device("F1360");
        PLC_Device PLC_Device_CCD01_04_PIN量測_下排水平度量測下限值 = new PLC_Device("F1361");

        PLC_Device PLC_Device_CCD01_04_PIN量測_水平度量測差值 = new PLC_Device("F1362");
        PLC_Device PLC_Device_CCD01_04_PIN量測_水平度量測差值上限 = new PLC_Device("F1363");
        PLC_Device PLC_Device_CCD01_04_PIN量測_水平度量測差值下限 = new PLC_Device("F1364");

        PLC_Device PLC_Device_CCD01_04_PIN量測_間距上排PIN11到基準數值 = new PLC_Device("F1365");
        PLC_Device PLC_Device_CCD01_04_PIN量測_間距上排PIN11到基準上限 = new PLC_Device("F1366");
        PLC_Device PLC_Device_CCD01_04_PIN量測_間距上排PIN11到基準下限 = new PLC_Device("F1367");
        PLC_Device PLC_Device_CCD01_04_PIN量測_間距下排PIN11到基準數值 = new PLC_Device("F1368");
        PLC_Device PLC_Device_CCD01_04_PIN量測_間距下排PIN11到基準上限 = new PLC_Device("F1369");
        PLC_Device PLC_Device_CCD01_04_PIN量測_間距下排PIN11到基準下限 = new PLC_Device("F1370");

        PLC_Device PLC_Device_CCD01_04_PIN量測_實際間距Pixcel = new PLC_Device("F1380");

        private List<PLC_Device> List_PLC_Device_CCD01_04_PIN量測參數_間距不測試 = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_04_PIN量測參數_左右間距量測值 = new List<PLC_Device>();

        private double[] List_CCD01_04_PIN量測參數_左右間距量測距離 = new double[19];
        private double[] List_CCD01_04_PIN量測參數_上下間距量測距離 = new double[19];
        private double[] List_CCD01_04_PIN量測參數_水平度量測距離 = new double[20];
        private double[] List_CCD01_04_PIN量測參數_上下間格距離 = new double[10];
        private double CCD01_04_PIN量測參數_間距上排PIN11到基準距離 = new double();
        private double CCD01_04_PIN量測參數_間距下排PIN11到基準距離 = new double();

        private bool[] List_CCD01_04_PIN量測參數_量測點_OK = new bool[20];
        private bool[] List_CCD01_04_PIN量測參數_左右間距量測_OK = new bool[19];
        private bool[] List_CCD01_04_PIN量測參數_上下間距量測_OK = new bool[10];
        private bool[] List_CCD01_04_PIN量測參數_水平度量測_OK = new bool[20];
        private bool CCD01_04_PIN量測參數_間距上排PIN11到基準_OK = new bool();
        private bool CCD01_04_PIN量測參數_間距下排PIN11到基準_OK = new bool();

        private double[] List_CCD01_04_PIN量測參數_水平度量測顯示點_X = new double[20];
        private double[] List_CCD01_04_PIN量測參數_水平度量測顯示點_Y = new double[20];

        private void H_Canvas_Tech_CCD01_04_PIN間距量測_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
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

            if (PLC_Device_CCD01_04_Main_取像並檢驗.Bool || PLC_Device_CCD01_04_PLC觸發檢測.Bool || PLC_Device_CCD01_04_Main_檢驗一次.Bool)
            {
                基準線偏移_上排 = this.PLC_Device_CCD01_03_基準線量測_基準線偏移_上排.Value / CCD01_比例尺_pixcel_To_mm / 1000;
                基準線偏移_下排 = this.PLC_Device_CCD01_03_基準線量測_基準線偏移_下排.Value / CCD01_比例尺_pixcel_To_mm / 1000;
                try
                {
                    Graphics g = Graphics.FromHdc((IntPtr)HDC);
                    DrawingClass.Draw.十字中心(new PointF(this.Point_CCD01_03_中心基準座標_量測點.X, this.Point_CCD01_03_中心基準座標_量測點.Y), 100, Color.Red, 2, g, ZoomX, ZoomY);
                    #region 左右間距顯示
                    for (int i = 0; i < 19; i++)
                    {
                        p0 = new PointF(this.List_CCD01_04_PIN量測點參數_量測點[i].X, this.List_CCD01_04_PIN量測點參數_量測點[i].Y);
                        p1 = new PointF(this.List_CCD01_04_PIN量測點參數_量測點[i + 1].X, this.List_CCD01_04_PIN量測點參數_量測點[i + 1].Y);
                        間距 = List_CCD01_04_PIN量測參數_左右間距量測距離[i];
                        if (i != 9)
                        {
                            if (i <= 9)
                            {
                                if (List_CCD01_04_PIN量測參數_左右間距量測_OK[i])
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
                            else if (i > 9)
                            {
                                if (List_CCD01_04_PIN量測參數_左右間距量測_OK[i])
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
                    上排_p2 = new PointF(this.List_CCD01_04_PIN量測點參數_量測點[0].X, this.List_CCD01_04_PIN量測點參數_量測點[0].Y - 50);
                    上排_p3 = new PointF(this.Point_CCD01_03_中心基準座標_量測點.X, this.List_CCD01_04_PIN量測點參數_量測點[0].Y - 50);

                    if (CCD01_04_PIN量測參數_間距上排PIN11到基準_OK)
                    {
                        //DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD01_04_PIN量測參數_間距上排PIN11到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
                        //    (float)((上排_p2.Y + 上排_p3.Y) / 2) - 80), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        //DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
                    }
                    else
                    {
                        DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD01_04_PIN量測參數_間距上排PIN11到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
                             (float)((上排_p2.Y + 上排_p3.Y) / 2) - 80), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Red, 1, g, ZoomX, ZoomY);
                    }

                    下排_p2 = new PointF(this.List_CCD01_04_PIN量測點參數_量測點[10].X, this.List_CCD01_04_PIN量測點參數_量測點[10].Y + 500);
                    下排_p3 = new PointF(this.Point_CCD01_03_中心基準座標_量測點.X, this.List_CCD01_04_PIN量測點參數_量測點[10].Y + 500);

                    if (CCD01_04_PIN量測參數_間距下排PIN11到基準_OK)
                    {
                        //DrawingClass.Draw.文字中心繪製(string.Format("下PIN01到基準" + "{0}", (CCD01_04_PIN量測參數_間距下排PIN11到基準距離 / 1D).ToString("0.000")), new PointF((float)((下排_p2.X + 下排_p3.X) / 2),
                        //    (float)((下排_p2.Y + 下排_p3.Y) / 2) + 80), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        //DrawingClass.Draw.線段繪製(下排_p2, 下排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
                    }
                    else
                    {
                        DrawingClass.Draw.文字中心繪製(string.Format("下PIN01到基準" + "{0}", (CCD01_04_PIN量測參數_間距下排PIN11到基準距離 / 1D).ToString("0.000")), new PointF((float)((下排_p2.X + 下排_p3.X) / 2),
                            (float)((下排_p2.Y + 下排_p3.Y) / 2) + 80), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        DrawingClass.Draw.線段繪製(下排_p2, 下排_p3, Color.Red, 1, g, ZoomX, ZoomY);
                    }
                    #endregion
                    #region 水平度顯示
                    DrawingClass.Draw.水平線段繪製(0, 10000, CCD01_03_水平基準線量測_AxLineMsr.MeasuredSlope, CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotX,
                      CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotY + 基準線偏移_上排, Color.Blue, 2, g, ZoomX, ZoomY);
                    DrawingClass.Draw.文字右中繪製("上排輔助線", new PointF((float)this.List_CCD01_04_PIN量測參數_水平度量測顯示點_X[0], CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotY + (float)基準線偏移_上排 + 20)
                        , new Font("標楷體", 10), Color.White, Color.Blue, g, ZoomX, ZoomY);


                    DrawingClass.Draw.水平線段繪製(0, 10000, CCD01_03_水平基準線量測_AxLineMsr.MeasuredSlope, CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotX,
                        CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotY + 基準線偏移_下排, Color.Blue, 2, g, ZoomX, ZoomY);
                    DrawingClass.Draw.文字右中繪製("下排輔助線", new PointF((float)this.List_CCD01_04_PIN量測參數_水平度量測顯示點_X[0], CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotY + (float)基準線偏移_下排 + 20)
                        , new Font("標楷體", 10), Color.White, Color.Blue, g, ZoomX, ZoomY);

                    for (int i = 0; i < 20; i++)
                    {

                        point = new PointF(this.List_CCD01_04_PIN量測點參數_量測點[i].X, this.List_CCD01_04_PIN量測點參數_量測點[i].Y);

                        上排_to_line_point = new PointF((float)this.List_CCD01_04_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_04_PIN量測參數_水平度量測顯示點_Y[i]));
                        下排_to_line_point = new PointF((float)this.List_CCD01_04_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_04_PIN量測參數_水平度量測顯示點_Y[i]));

                        水平度 = List_CCD01_04_PIN量測參數_水平度量測距離[i];


                        if (List_CCD01_04_PIN量測參數_水平度量測_OK[i])
                        {
                            DrawingClass.Draw.文字中心繪製("到基準線:", new PointF(1200, 1500), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            if (i <= 9)
                            {
                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                new PointF(point.X, 1600), new Font("標楷體", 10), Color.Black, Color.DodgerBlue, g, ZoomX, ZoomY);

                                DrawingClass.Draw.線段繪製(point, 上排_to_line_point, Color.DodgerBlue, 1, g, ZoomX, ZoomY);
                            }
                            if (i > 9)
                            {
                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                new PointF(point.X, 1700), new Font("標楷體", 10), Color.Black, Color.DodgerBlue, g, ZoomX, ZoomY);

                                DrawingClass.Draw.線段繪製(point, 下排_to_line_point, Color.DodgerBlue, 1, g, ZoomX, ZoomY);
                            }

                        }
                        else
                        {
                            DrawingClass.Draw.文字中心繪製("到基準線:", new PointF(1200, 1500), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            if (i <= 9)
                            {
                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                new PointF(point.X, 1600), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                                DrawingClass.Draw.線段繪製(point, 上排_to_line_point, Color.Red, 1, g, ZoomX, ZoomY);
                            }
                            if (i > 9)
                            {
                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                new PointF(point.X, 1700), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                                DrawingClass.Draw.線段繪製(point, 下排_to_line_point, Color.Red, 1, g, ZoomX, ZoomY);
                            }

                        }


                    }
                    for (int i = 0; i < 10; i++)
                    {
                        point = new PointF(this.List_CCD01_04_PIN量測點參數_量測點[i].X, this.List_CCD01_04_PIN量測點參數_量測點[i].Y);
                        point1 = new PointF(this.List_CCD01_04_PIN量測點參數_量測點[i + 10].X, this.List_CCD01_04_PIN量測點參數_量測點[i + 10].Y);

                        上排_to_line_point = new PointF((float)this.List_CCD01_04_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_04_PIN量測參數_水平度量測顯示點_Y[i]));
                        下排_to_line_point = new PointF((float)this.List_CCD01_04_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_04_PIN量測參數_水平度量測顯示點_Y[i]));

                        水平度 = List_CCD01_04_PIN量測參數_上下間格距離[i];
                        if (List_CCD01_04_PIN量測參數_上下間距量測_OK[i])
                        {

                            DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                            new PointF(point.X, point.Y + 400 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Yellow, g, ZoomX, ZoomY);

                            DrawingClass.Draw.線段繪製(point, point1, Color.Yellow, 1, g, ZoomX, ZoomY);

                        }
                        else
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                            new PointF(point.X, point.Y + 400 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                            DrawingClass.Draw.線段繪製(point, point1, Color.Red, 1, g, ZoomX, ZoomY);



                        }

                    }


                    #endregion
                    #region 結果顯示
                    for (int i = 0; i < 19; i++)
                    {
                        if (i != 9)
                        {
                            if (List_CCD01_04_PIN量測參數_左右間距量測_OK[i] && CCD01_04_PIN量測參數_間距上排PIN11到基準_OK)
                            {
                                DrawingClass.Draw.文字左上繪製("間距量測OK!", new PointF(0, 0), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                            }
                            else
                            {
                                DrawingClass.Draw.文字左上繪製("間距量測NG!", new PointF(0, 0), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            }
                        }
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        if (List_CCD01_04_PIN量測參數_水平度量測_OK[i] && List_CCD01_04_PIN量測參數_上下間距量測_OK[i])
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
            else if (PLC_Device_CCD01_04_Tech_檢驗一次.Bool || PLC_Device_CCD01_04_Tech_取像並檢驗.Bool)
            {
                基準線偏移_上排 = this.PLC_Device_CCD01_03_基準線量測_基準線偏移_上排.Value / CCD01_比例尺_pixcel_To_mm / 1000;
                基準線偏移_下排 = this.PLC_Device_CCD01_03_基準線量測_基準線偏移_下排.Value / CCD01_比例尺_pixcel_To_mm / 1000;
                if (this.PLC_Device_CCD01_04_PIN量測_檢測距離計算_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        DrawingClass.Draw.十字中心(new PointF(this.Point_CCD01_03_中心基準座標_量測點.X, this.Point_CCD01_03_中心基準座標_量測點.Y), 100, Color.Red, 2, g, ZoomX, ZoomY);
                        #region 左右間距顯示
                        for (int i = 0; i < 19; i++)
                        {
                            p0 = new PointF(this.List_CCD01_04_PIN量測點參數_量測點[i].X, this.List_CCD01_04_PIN量測點參數_量測點[i].Y);
                            p1 = new PointF(this.List_CCD01_04_PIN量測點參數_量測點[i + 1].X, this.List_CCD01_04_PIN量測點參數_量測點[i + 1].Y);
                            間距 = List_CCD01_04_PIN量測參數_左右間距量測距離[i];

                            if (i != 9)
                            {
                                if (List_CCD01_04_PIN量測參數_左右間距量測_OK[i])
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

                        上排_p2 = new PointF(this.List_CCD01_04_PIN量測點參數_量測點[0].X, this.List_CCD01_04_PIN量測點參數_量測點[0].Y - 150);
                        上排_p3 = new PointF(this.Point_CCD01_03_中心基準座標_量測點.X, this.List_CCD01_04_PIN量測點參數_量測點[0].Y - 150);

                        if (CCD01_04_PIN量測參數_間距上排PIN11到基準_OK)
                        {
                            //DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD01_04_PIN量測參數_間距上排PIN11到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
                            //    (float)((上排_p2.Y + 上排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            //DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
                        }
                        else
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD01_04_PIN量測參數_間距上排PIN11到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
    (float)((上排_p2.Y + 上排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Red, 1, g, ZoomX, ZoomY);
                        }

                        下排_p2 = new PointF(this.List_CCD01_04_PIN量測點參數_量測點[10].X, this.List_CCD01_04_PIN量測點參數_量測點[10].Y + 150);
                        下排_p3 = new PointF(this.Point_CCD01_03_中心基準座標_量測點.X, this.List_CCD01_04_PIN量測點參數_量測點[10].Y + 150);

                        if (CCD01_04_PIN量測參數_間距下排PIN11到基準_OK)
                        {
                            //DrawingClass.Draw.文字中心繪製(string.Format("下PIN01到基準" + "{0}", (CCD01_04_PIN量測參數_間距下排PIN11到基準距離 / 1D).ToString("0.000")), new PointF((float)((下排_p2.X + 下排_p3.X) / 2),
                            //    (float)((下排_p2.Y + 下排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            //DrawingClass.Draw.線段繪製(下排_p2, 下排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
                        }
                        else
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("下PIN01到基準" + "{0}", (CCD01_04_PIN量測參數_間距下排PIN11到基準距離 / 1D).ToString("0.000")), new PointF((float)((下排_p2.X + 下排_p3.X) / 2),
    (float)((下排_p2.Y + 下排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(下排_p2, 下排_p3, Color.Red, 1, g, ZoomX, ZoomY);
                        }
                        #endregion
                        #region 水平度顯示
                        DrawingClass.Draw.水平線段繪製(0, 10000, CCD01_03_水平基準線量測_AxLineMsr.MeasuredSlope, CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotX,
                          CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotY + 基準線偏移_上排, Color.Blue, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.文字右中繪製("上排輔助線", new PointF((float)this.List_CCD01_04_PIN量測參數_水平度量測顯示點_X[0], CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotY + (float)基準線偏移_上排 + 20)
                            , new Font("標楷體", 10), Color.White, Color.Blue, g, ZoomX, ZoomY);


                        DrawingClass.Draw.水平線段繪製(0, 10000, CCD01_03_水平基準線量測_AxLineMsr.MeasuredSlope, CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotX,
                            CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotY + 基準線偏移_下排, Color.Blue, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.文字右中繪製("下排輔助線", new PointF((float)this.List_CCD01_04_PIN量測參數_水平度量測顯示點_X[0], CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotY + (float)基準線偏移_下排 + 20)
                            , new Font("標楷體", 10), Color.White, Color.Blue, g, ZoomX, ZoomY);

                        for (int i = 0; i < 20; i++)
                        {
                            point = new PointF(this.List_CCD01_04_PIN量測點參數_量測點[i].X, this.List_CCD01_04_PIN量測點參數_量測點[i].Y);

                            上排_to_line_point = new PointF((float)this.List_CCD01_04_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_04_PIN量測參數_水平度量測顯示點_Y[i]));
                            下排_to_line_point = new PointF((float)this.List_CCD01_04_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_04_PIN量測參數_水平度量測顯示點_Y[i]));

                            水平度 = List_CCD01_04_PIN量測參數_水平度量測距離[i];


                            if (List_CCD01_04_PIN量測參數_水平度量測_OK[i])
                            {
                                if (i <= 9)
                                {
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                    new PointF(point.X, 1300), new Font("標楷體", 10), Color.Black, Color.DodgerBlue, g, ZoomX, ZoomY);

                                    DrawingClass.Draw.線段繪製(point, 上排_to_line_point, Color.DodgerBlue, 1, g, ZoomX, ZoomY);
                                }
                                if (i > 9)
                                {
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                    new PointF(point.X, 1350), new Font("標楷體", 10), Color.Black, Color.DodgerBlue, g, ZoomX, ZoomY);

                                    DrawingClass.Draw.線段繪製(point, 下排_to_line_point, Color.DodgerBlue, 1, g, ZoomX, ZoomY);
                                }

                            }
                            else
                            {
                                if (i <= 9)
                                {
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                    new PointF(point.X, 1300), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                                    DrawingClass.Draw.線段繪製(point, 上排_to_line_point, Color.Red, 1, g, ZoomX, ZoomY);
                                }
                                if (i > 9)
                                {
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                    new PointF(point.X, 1350), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                                    DrawingClass.Draw.線段繪製(point, 下排_to_line_point, Color.Red, 1, g, ZoomX, ZoomY);
                                }

                            }


                        }
                        for (int i = 0; i < 10; i++)
                        {
                            point = new PointF(this.List_CCD01_04_PIN量測點參數_量測點[i].X, this.List_CCD01_04_PIN量測點參數_量測點[i].Y);
                            point1 = new PointF(this.List_CCD01_04_PIN量測點參數_量測點[i + 10].X, this.List_CCD01_04_PIN量測點參數_量測點[i + 10].Y);

                            上排_to_line_point = new PointF((float)this.List_CCD01_04_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_04_PIN量測參數_水平度量測顯示點_Y[i]));
                            下排_to_line_point = new PointF((float)this.List_CCD01_04_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_04_PIN量測參數_水平度量測顯示點_Y[i]));

                            水平度 = List_CCD01_04_PIN量測參數_上下間格距離[i];
                            if (List_CCD01_04_PIN量測參數_上下間距量測_OK[i])
                            {

                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                new PointF(point.X, point.Y + 400 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Yellow, g, ZoomX, ZoomY);

                                DrawingClass.Draw.線段繪製(point, point1, Color.Yellow, 1, g, ZoomX, ZoomY);

                            }
                            else
                            {
                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                new PointF(point.X, point.Y + 400 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                                DrawingClass.Draw.線段繪製(point, point1, Color.Red, 1, g, ZoomX, ZoomY);



                            }

                        }


                        #endregion
                        #region 結果顯示
                        for (int i = 0; i < 19; i++)
                        {
                            if (i != 9)
                            {
                                if (List_CCD01_04_PIN量測參數_左右間距量測_OK[i] && CCD01_04_PIN量測參數_間距上排PIN11到基準_OK && CCD01_04_PIN量測參數_間距下排PIN11到基準_OK)
                                {
                                    DrawingClass.Draw.文字左上繪製("間距量測OK!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                                }
                                else
                                {
                                    DrawingClass.Draw.文字左上繪製("間距量測NG!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                                }
                            }
                        }
                        for (int i = 0; i < 10; i++)
                        {
                            if (List_CCD01_04_PIN量測參數_水平度量測_OK[i] && List_CCD01_04_PIN量測參數_上下間距量測_OK[i])
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
                基準線偏移_上排 = this.PLC_Device_CCD01_03_基準線量測_基準線偏移_上排.Value / CCD01_比例尺_pixcel_To_mm / 1000;
                基準線偏移_下排 = this.PLC_Device_CCD01_03_基準線量測_基準線偏移_下排.Value / CCD01_比例尺_pixcel_To_mm / 1000;
                if (this.PLC_Device_CCD01_04_PIN量測_檢測距離計算_RefreshCanvas.Bool && PLC_Device_CCD01_04_PIN量測_檢測距離計算.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);

                        DrawingClass.Draw.十字中心(new PointF(this.Point_CCD01_03_中心基準座標_量測點.X, this.Point_CCD01_03_中心基準座標_量測點.Y), 100, Color.Red, 2, g, ZoomX, ZoomY);
                        #region 左右間距顯示
                        for (int i = 0; i < 19; i++)
                        {
                            p0 = new PointF(this.List_CCD01_04_PIN量測點參數_量測點[i].X, this.List_CCD01_04_PIN量測點參數_量測點[i].Y);
                            p1 = new PointF(this.List_CCD01_04_PIN量測點參數_量測點[i + 1].X, this.List_CCD01_04_PIN量測點參數_量測點[i + 1].Y);
                            間距 = List_CCD01_04_PIN量測參數_左右間距量測距離[i];

                            if (i != 9)
                            {
                                if (List_CCD01_04_PIN量測參數_左右間距量測_OK[i])
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
                        上排_p2 = new PointF(this.List_CCD01_04_PIN量測點參數_量測點[0].X, this.List_CCD01_04_PIN量測點參數_量測點[0].Y - 150);
                        上排_p3 = new PointF(this.Point_CCD01_03_中心基準座標_量測點.X, this.List_CCD01_04_PIN量測點參數_量測點[0].Y - 150);

                        if (CCD01_04_PIN量測參數_間距上排PIN11到基準_OK)
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD01_04_PIN量測參數_間距上排PIN11到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
                                (float)((上排_p2.Y + 上排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
                        }
                        else
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("上PIN01到基準" + "{0}", (CCD01_04_PIN量測參數_間距上排PIN11到基準距離 / 1D).ToString("0.000")), new PointF((float)((上排_p2.X + 上排_p3.X) / 2),
    (float)((上排_p2.Y + 上排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(上排_p2, 上排_p3, Color.Red, 1, g, ZoomX, ZoomY);
                        }

                        下排_p2 = new PointF(this.List_CCD01_04_PIN量測點參數_量測點[10].X, this.List_CCD01_04_PIN量測點參數_量測點[10].Y + 150);
                        下排_p3 = new PointF(this.Point_CCD01_03_中心基準座標_量測點.X, this.List_CCD01_04_PIN量測點參數_量測點[10].Y + 150);

                        if (CCD01_04_PIN量測參數_間距下排PIN11到基準_OK)
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("下PIN01到基準" + "{0}", (CCD01_04_PIN量測參數_間距下排PIN11到基準距離 / 1D).ToString("0.000")), new PointF((float)((下排_p2.X + 下排_p3.X) / 2),
                                (float)((下排_p2.Y + 下排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(下排_p2, 下排_p3, Color.Lime, 1, g, ZoomX, ZoomY);
                        }
                        else
                        {
                            DrawingClass.Draw.文字中心繪製(string.Format("下PIN01到基準" + "{0}", (CCD01_04_PIN量測參數_間距下排PIN11到基準距離 / 1D).ToString("0.000")), new PointF((float)((下排_p2.X + 下排_p3.X) / 2),
    (float)((下排_p2.Y + 下排_p3.Y) / 2) + 150 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            DrawingClass.Draw.線段繪製(下排_p2, 下排_p3, Color.Red, 1, g, ZoomX, ZoomY);
                        }


                        #endregion
                        #region 水平度顯示
                        //DrawingClass.Draw.水平線段繪製(0, 10000, CCD01_01_水平基準線量測_AxLineMsr.MeasuredSlope, CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotX,
                        //    CCD01_01_水平基準線量測_AxLineMsr.MeasuredPivotY + this.PLC_Device_CCD01_01_基準線量測_基準線偏移.Value, Color.Yellow, 2, g, ZoomX, ZoomY);

                        DrawingClass.Draw.水平線段繪製(0, 10000, CCD01_03_水平基準線量測_AxLineMsr.MeasuredSlope, CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotX,
                          CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotY + 基準線偏移_上排, Color.Blue, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.文字右中繪製("上排輔助線", new PointF((float)this.List_CCD01_04_PIN量測參數_水平度量測顯示點_X[0], CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotY + (float)基準線偏移_上排 + 20)
                            , new Font("標楷體", 10), Color.White, Color.Blue, g, ZoomX, ZoomY);


                        DrawingClass.Draw.水平線段繪製(0, 10000, CCD01_03_水平基準線量測_AxLineMsr.MeasuredSlope, CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotX,
                            CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotY + 基準線偏移_下排, Color.Blue, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.文字右中繪製("下排輔助線", new PointF((float)this.List_CCD01_04_PIN量測參數_水平度量測顯示點_X[0], CCD01_03_水平基準線量測_AxLineMsr.MeasuredPivotY + (float)基準線偏移_下排 + 20)
                            , new Font("標楷體", 10), Color.White, Color.Blue, g, ZoomX, ZoomY);

                        for (int i = 0; i < 20; i++)
                        {
                            point = new PointF(this.List_CCD01_04_PIN量測點參數_量測點[i].X, this.List_CCD01_04_PIN量測點參數_量測點[i].Y);


                            上排_to_line_point = new PointF((float)this.List_CCD01_04_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_04_PIN量測參數_水平度量測顯示點_Y[i]));
                            下排_to_line_point = new PointF((float)this.List_CCD01_04_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_04_PIN量測參數_水平度量測顯示點_Y[i]));

                            水平度 = List_CCD01_04_PIN量測參數_水平度量測距離[i];


                            if (List_CCD01_04_PIN量測參數_水平度量測_OK[i])
                            {
                                if (i <= 9)
                                {
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                    new PointF(point.X, 1300), new Font("標楷體", 10), Color.Black, Color.DodgerBlue, g, ZoomX, ZoomY);

                                    DrawingClass.Draw.線段繪製(point, 上排_to_line_point, Color.DodgerBlue, 1, g, ZoomX, ZoomY);
                                }
                                if (i > 9)
                                {
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                    new PointF(point.X, 1350), new Font("標楷體", 10), Color.Black, Color.DodgerBlue, g, ZoomX, ZoomY);

                                    DrawingClass.Draw.線段繪製(point, 下排_to_line_point, Color.DodgerBlue, 1, g, ZoomX, ZoomY);
                                }

                            }
                            else
                            {
                                if (i <= 9)
                                {
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                    new PointF(point.X, 1300), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                                    DrawingClass.Draw.線段繪製(point, 上排_to_line_point, Color.Red, 1, g, ZoomX, ZoomY);
                                }
                                if (i > 9)
                                {
                                    DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                    new PointF(point.X, 1350), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                                    DrawingClass.Draw.線段繪製(point, 下排_to_line_point, Color.Red, 1, g, ZoomX, ZoomY);
                                }

                            }


                        }
                        for (int i = 0; i < 10; i++)
                        {
                            point = new PointF(this.List_CCD01_04_PIN量測點參數_量測點[i].X, this.List_CCD01_04_PIN量測點參數_量測點[i].Y);
                            point1 = new PointF(this.List_CCD01_04_PIN量測點參數_量測點[i + 10].X, this.List_CCD01_04_PIN量測點參數_量測點[i + 10].Y);

                            上排_to_line_point = new PointF((float)this.List_CCD01_04_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_04_PIN量測參數_水平度量測顯示點_Y[i]));
                            下排_to_line_point = new PointF((float)this.List_CCD01_04_PIN量測參數_水平度量測顯示點_X[i], (float)(List_CCD01_04_PIN量測參數_水平度量測顯示點_Y[i]));

                            水平度 = List_CCD01_04_PIN量測參數_上下間格距離[i];
                            if (List_CCD01_04_PIN量測參數_上下間距量測_OK[i])
                            {

                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                new PointF(point.X, point.Y + 400 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Yellow, g, ZoomX, ZoomY);

                                DrawingClass.Draw.線段繪製(point, point1, Color.Yellow, 1, g, ZoomX, ZoomY);

                            }
                            else
                            {
                                DrawingClass.Draw.文字中心繪製(string.Format("{0}", (水平度 / 1D).ToString("0.000")),
                                new PointF(point.X, point.Y + 400 * ZoomY), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

                                DrawingClass.Draw.線段繪製(point, point1, Color.Red, 1, g, ZoomX, ZoomY);



                            }

                        }
                        #endregion
                        #region 結果顯示

                        for (int i = 0; i < 19; i++)
                        {
                            if (i != 9)
                            {
                                if (List_CCD01_04_PIN量測參數_左右間距量測_OK[i] && CCD01_04_PIN量測參數_間距上排PIN11到基準_OK)
                                {
                                    DrawingClass.Draw.文字左上繪製("間距量測OK!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                                }
                                else
                                {
                                    DrawingClass.Draw.文字左上繪製("間距量測NG!", new PointF(0, 100), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                                }
                            }
                        }
                        for (int i = 0; i < 10; i++)
                        {
                            if (List_CCD01_04_PIN量測參數_水平度量測_OK[i] && List_CCD01_04_PIN量測參數_上下間距量測_OK[i])
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

            this.PLC_Device_CCD01_04_PIN量測_檢測距離計算_RefreshCanvas.Bool = false;
        }

        int cnt_Program_CCD01_04_PIN量測_檢測距離計算 = 65534;
        void sub_Program_CCD01_04_PIN量測_檢測距離計算()
        {
            if (cnt_Program_CCD01_04_PIN量測_檢測距離計算 == 65534)
            {
                this.h_Canvas_Tech_CCD01_04.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_04_PIN間距量測_OnCanvasDrawEvent;
                this.h_Canvas_Main_CCD01_04_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_04_PIN間距量測_OnCanvasDrawEvent;
                PLC_Device_CCD01_04_PIN量測_檢測距離計算.SetComment("PLC_CCD01_04_PIN量測_檢測距離計算");
                PLC_Device_CCD01_04_PIN量測_檢測距離計算.Bool = false;
                PLC_Device_CCD01_04_PIN量測_檢測距離計算按鈕.Bool = false;
                cnt_Program_CCD01_04_PIN量測_檢測距離計算 = 65535;

            }
            if (cnt_Program_CCD01_04_PIN量測_檢測距離計算 == 65535) cnt_Program_CCD01_04_PIN量測_檢測距離計算 = 1;
            if (cnt_Program_CCD01_04_PIN量測_檢測距離計算 == 1) cnt_Program_CCD01_04_PIN量測_檢測距離計算_觸發按下(ref cnt_Program_CCD01_04_PIN量測_檢測距離計算);
            if (cnt_Program_CCD01_04_PIN量測_檢測距離計算 == 2) cnt_Program_CCD01_04_PIN量測_檢測距離計算_檢查按下(ref cnt_Program_CCD01_04_PIN量測_檢測距離計算);
            if (cnt_Program_CCD01_04_PIN量測_檢測距離計算 == 3) cnt_Program_CCD01_04_PIN量測_檢測距離計算_初始化(ref cnt_Program_CCD01_04_PIN量測_檢測距離計算);
            if (cnt_Program_CCD01_04_PIN量測_檢測距離計算 == 4) cnt_Program_CCD01_04_PIN量測_檢測距離計算_數值計算(ref cnt_Program_CCD01_04_PIN量測_檢測距離計算);
            if (cnt_Program_CCD01_04_PIN量測_檢測距離計算 == 5) cnt_Program_CCD01_04_PIN量測_檢測距離計算_量測結果(ref cnt_Program_CCD01_04_PIN量測_檢測距離計算);
            if (cnt_Program_CCD01_04_PIN量測_檢測距離計算 == 6) cnt_Program_CCD01_04_PIN量測_檢測距離計算_繪製畫布(ref cnt_Program_CCD01_04_PIN量測_檢測距離計算);
            if (cnt_Program_CCD01_04_PIN量測_檢測距離計算 == 7) cnt_Program_CCD01_04_PIN量測_檢測距離計算 = 65500;
            if (cnt_Program_CCD01_04_PIN量測_檢測距離計算 > 1) cnt_Program_CCD01_04_PIN量測_檢測距離計算_檢查放開(ref cnt_Program_CCD01_04_PIN量測_檢測距離計算);

            if (cnt_Program_CCD01_04_PIN量測_檢測距離計算 == 65500)
            {
                PLC_Device_CCD01_04_PIN量測_檢測距離計算.Bool = false;
                PLC_Device_CCD01_04_PIN量測_檢測距離計算按鈕.Bool = false;
                cnt_Program_CCD01_04_PIN量測_檢測距離計算 = 65535;
            }
        }
        void cnt_Program_CCD01_04_PIN量測_檢測距離計算_觸發按下(ref int cnt)
        {
            if (PLC_Device_CCD01_04_PIN量測_檢測距離計算按鈕.Bool)
            {
                PLC_Device_CCD01_04_PIN量測_檢測距離計算.Bool = true;
                cnt++;
            }

        }
        void cnt_Program_CCD01_04_PIN量測_檢測距離計算_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_04_PIN量測_檢測距離計算.Bool)
            {
                cnt++;
            }

        }
        void cnt_Program_CCD01_04_PIN量測_檢測距離計算_檢查放開(ref int cnt)
        {
            //if (!PLC_Device_CCD01_04_PIN量測_檢測距離計算.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_04_PIN量測_檢測距離計算_初始化(ref int cnt)
        {
            this.MyTimer_CCD01_04_PIN量測_檢測距離計算.TickStop();
            this.MyTimer_CCD01_04_PIN量測_檢測距離計算.StartTickTime(99999);

            this.List_CCD01_04_PIN量測參數_左右間距量測距離 = new double[19];
            this.List_CCD01_04_PIN量測參數_上下間距量測距離 = new double[19];
            this.List_CCD01_04_PIN量測參數_水平度量測距離 = new double[20];
            this.List_CCD01_04_PIN量測參數_上下間格距離 = new double[10];
            this.CCD01_04_PIN量測參數_間距上排PIN11到基準距離 = new double();
            this.CCD01_04_PIN量測參數_間距下排PIN11到基準距離 = new double();

            this.List_CCD01_04_PIN量測參數_量測點_OK = new bool[20];
            this.List_CCD01_04_PIN量測參數_左右間距量測_OK = new bool[19];
            this.List_CCD01_04_PIN量測參數_上下間距量測_OK = new bool[19];
            this.List_CCD01_04_PIN量測參數_水平度量測_OK = new bool[20];
            this.CCD01_04_PIN量測參數_間距上排PIN11到基準_OK = new bool();
            this.CCD01_04_PIN量測參數_間距下排PIN11到基準_OK = new bool();
            cnt++;
        }
        void cnt_Program_CCD01_04_PIN量測_檢測距離計算_數值計算(ref int cnt)
        {
            double 基準線偏移_上排 = this.PLC_Device_CCD01_03_基準線量測_基準線偏移_上排.Value / CCD01_比例尺_pixcel_To_mm / 1000;
            double 基準線偏移_下排 = this.PLC_Device_CCD01_03_基準線量測_基準線偏移_下排.Value / CCD01_比例尺_pixcel_To_mm / 1000;
            #region 水平度數值計算
            this.CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.LinePivotX = this.CCD01_03_水平基準線量測_AxLineRegression.FittedPivotX;
            this.CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.LinePivotY = this.CCD01_03_水平基準線量測_AxLineRegression.FittedPivotY + 基準線偏移_上排;
            this.CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.LineHorzVert = AxOvkMsr.TxAxLineHorzVert.AX_LINE_QUASI_HORIZONTAL;
            this.CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.LineSlope = this.CCD01_03_水平基準線量測_AxLineRegression.FittedSlope;

            this.CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.LinePivotX = this.CCD01_03_水平基準線量測_AxLineRegression.FittedPivotX;
            this.CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.LinePivotY = this.CCD01_03_水平基準線量測_AxLineRegression.FittedPivotY + 基準線偏移_下排;
            this.CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.LineHorzVert = AxOvkMsr.TxAxLineHorzVert.AX_LINE_QUASI_HORIZONTAL;
            this.CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.LineSlope = this.CCD01_03_水平基準線量測_AxLineRegression.FittedSlope;
            for (int i = 0; i < this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {
                if (this.List_CCD01_04_PIN量測點參數_量測點_有無[i])
                {
                    if (i <= 9)
                    {
                        this.CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.PivotX = this.List_CCD01_04_PIN量測點參數_量測點[i].X;
                        this.CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.PivotY = this.List_CCD01_04_PIN量測點參數_量測點[i].Y;
                        this.CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.FindDistance();
                        this.List_CCD01_04_PIN量測參數_水平度量測顯示點_X[i] = CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.ProjectPivotX;
                        this.List_CCD01_04_PIN量測參數_水平度量測顯示點_Y[i] = CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.ProjectPivotY;

                        this.List_CCD01_04_PIN量測參數_水平度量測距離[i] = this.CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_上排.Distance * this.CCD01_比例尺_pixcel_To_mm;
                    }
                    if (i > 9)
                    {
                        this.CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.PivotX = this.List_CCD01_04_PIN量測點參數_量測點[i].X;
                        this.CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.PivotY = this.List_CCD01_04_PIN量測點參數_量測點[i].Y;
                        this.CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.FindDistance();
                        this.List_CCD01_04_PIN量測參數_水平度量測顯示點_X[i] = CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.ProjectPivotX;
                        this.List_CCD01_04_PIN量測參數_水平度量測顯示點_Y[i] = CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.ProjectPivotY;

                        this.List_CCD01_04_PIN量測參數_水平度量測距離[i] = this.CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_下排.Distance * this.CCD01_比例尺_pixcel_To_mm;
                    }
                }
                List_CCD01_04_PIN量測參數_上下間格距離[0] = (Math.Abs(List_CCD01_04_PIN量測點參數_量測點[0].Y - List_CCD01_04_PIN量測點參數_量測點[10].Y)) * this.CCD01_比例尺_pixcel_To_mm;
                List_CCD01_04_PIN量測參數_上下間格距離[1] = (Math.Abs(List_CCD01_04_PIN量測點參數_量測點[1].Y - List_CCD01_04_PIN量測點參數_量測點[11].Y)) * this.CCD01_比例尺_pixcel_To_mm;
                List_CCD01_04_PIN量測參數_上下間格距離[2] = (Math.Abs(List_CCD01_04_PIN量測點參數_量測點[2].Y - List_CCD01_04_PIN量測點參數_量測點[12].Y)) * this.CCD01_比例尺_pixcel_To_mm;
                List_CCD01_04_PIN量測參數_上下間格距離[3] = (Math.Abs(List_CCD01_04_PIN量測點參數_量測點[3].Y - List_CCD01_04_PIN量測點參數_量測點[13].Y)) * this.CCD01_比例尺_pixcel_To_mm;
                List_CCD01_04_PIN量測參數_上下間格距離[4] = (Math.Abs(List_CCD01_04_PIN量測點參數_量測點[4].Y - List_CCD01_04_PIN量測點參數_量測點[14].Y)) * this.CCD01_比例尺_pixcel_To_mm;
                List_CCD01_04_PIN量測參數_上下間格距離[5] = (Math.Abs(List_CCD01_04_PIN量測點參數_量測點[5].Y - List_CCD01_04_PIN量測點參數_量測點[15].Y)) * this.CCD01_比例尺_pixcel_To_mm;
                List_CCD01_04_PIN量測參數_上下間格距離[6] = (Math.Abs(List_CCD01_04_PIN量測點參數_量測點[6].Y - List_CCD01_04_PIN量測點參數_量測點[16].Y)) * this.CCD01_比例尺_pixcel_To_mm;
                List_CCD01_04_PIN量測參數_上下間格距離[7] = (Math.Abs(List_CCD01_04_PIN量測點參數_量測點[7].Y - List_CCD01_04_PIN量測點參數_量測點[17].Y)) * this.CCD01_比例尺_pixcel_To_mm;
                List_CCD01_04_PIN量測參數_上下間格距離[8] = (Math.Abs(List_CCD01_04_PIN量測點參數_量測點[8].Y - List_CCD01_04_PIN量測點參數_量測點[18].Y)) * this.CCD01_比例尺_pixcel_To_mm;
                List_CCD01_04_PIN量測參數_上下間格距離[9] = (Math.Abs(List_CCD01_04_PIN量測點參數_量測點[9].Y - List_CCD01_04_PIN量測點參數_量測點[19].Y)) * this.CCD01_比例尺_pixcel_To_mm;


            }
            #endregion
            #region 左右間距數值計算
            double distance = 0;
            double 間距Temp1_X = 0;
            double 間距Temp2_X = 0;

            for (int i = 0; i < 19; i++)
            {
                if (this.List_CCD01_04_PIN量測點參數_量測點_有無[i] && this.List_CCD01_04_PIN量測點參數_量測點_有無[i + 1])
                {

                    間距Temp1_X = this.List_CCD01_04_PIN量測點參數_量測點[i].X - this.Point_CCD01_03_中心基準座標_量測點.X;
                    間距Temp2_X = this.List_CCD01_04_PIN量測點參數_量測點[i + 1].X - this.Point_CCD01_03_中心基準座標_量測點.X;

                    distance = Math.Abs(間距Temp1_X - 間距Temp2_X);

                    this.List_CCD01_04_PIN量測參數_左右間距量測距離[i] = distance * this.CCD01_比例尺_pixcel_To_mm;
                }
                else
                {
                    PLC_Device_CCD01_04_PIN量測_檢測距離計算_OK.Bool = false;
                    List_CCD01_04_PIN量測參數_量測點_OK[i] = false;
                }

            }
            #endregion
            cnt++;
        }
        void cnt_Program_CCD01_04_PIN量測_檢測距離計算_量測結果(ref int cnt)
        {

            PLC_Device_CCD01_04_PIN量測_檢測距離計算_OK.Bool = true; // 檢測結果初始化
            #region 左右間距量測判斷

            for (int i = 0; i < 19; i++)
            {
                int 標準值 = this.PLC_Device_CCD01_04_PIN量測_左右間距量測標準值.Value;
                int 標準值上限 = this.PLC_Device_CCD01_04_PIN量測_左右間距量測上限值.Value;
                int 標準值下限 = this.PLC_Device_CCD01_04_PIN量測_左右間距量測下限值.Value;
                double 量測距離 = this.List_CCD01_04_PIN量測參數_左右間距量測距離[i];

                量測距離 = 量測距離 * 1000 - 標準值;
                量測距離 /= 1000;
                if (!PLC_Device_CCD01_04_PIN量測_間距量測不測試.Bool)
                {
                    if (this.List_CCD01_04_PIN量測點參數_量測點_有無[i])
                    {
                        if (量測距離 >= 0 && i != 9)
                        {
                            if (標準值上限 <= Math.Abs(量測距離) * 1000 || 標準值下限 > Math.Abs(量測距離) * 1000)
                            {
                                this.List_CCD01_04_PIN量測參數_左右間距量測_OK[i] = false;
                                PLC_Device_CCD01_04_PIN量測_檢測距離計算_OK.Bool = false;
                            }
                            else
                            {
                                this.List_CCD01_04_PIN量測參數_左右間距量測_OK[i] = true;
                            }
                        }
                    }
                }
                else
                {
                    PLC_Device_CCD01_04_PIN量測_檢測距離計算_OK.Bool = true;
                    this.List_CCD01_04_PIN量測參數_左右間距量測_OK[i] = true;
                }



                this.List_CCD01_04_PIN量測參數_左右間距量測距離[i] = 量測距離;

            }
            #endregion
            #region 水平度量測判斷
            for (int i = 0; i < 20; i++)
            {
                int 上排標準值 = this.PLC_Device_CCD01_04_PIN量測_上排水平度量測標準值.Value;
                int 上排標準值上限 = this.PLC_Device_CCD01_04_PIN量測_上排水平度量測上限值.Value;
                int 上排標準值下限 = this.PLC_Device_CCD01_04_PIN量測_上排水平度量測下限值.Value;
                double 上排量測距離 = this.List_CCD01_04_PIN量測參數_水平度量測距離[i];

                int 下排標準值 = this.PLC_Device_CCD01_04_PIN量測_下排水平度量測標準值.Value;
                int 下排標準值上限 = this.PLC_Device_CCD01_04_PIN量測_下排水平度量測上限值.Value;
                int 下排標準值下限 = this.PLC_Device_CCD01_04_PIN量測_下排水平度量測下限值.Value;
                double 下排量測距離 = this.List_CCD01_04_PIN量測參數_水平度量測距離[i];

                上排量測距離 = 上排量測距離 * 1000 - 上排標準值;
                上排量測距離 /= 1000;

                下排量測距離 = 下排量測距離 * 1000 - 下排標準值;
                下排量測距離 /= 1000;
                if (!PLC_Device_CCD01_04_PIN量測_水平度量測不測試.Bool)
                {
                    if (this.List_CCD01_04_PIN量測點參數_量測點_有無[i])
                    {
                        if (上排量測距離 >= 0 && i < 10)
                        {
                            if (上排標準值上限 <= Math.Abs(上排量測距離) * 1000 || 上排標準值下限 > Math.Abs(上排量測距離) * 1000)
                            {
                                this.List_CCD01_04_PIN量測參數_水平度量測_OK[i] = false;
                                PLC_Device_CCD01_04_PIN量測_檢測距離計算_OK.Bool = false;
                            }
                            else
                            {
                                this.List_CCD01_04_PIN量測參數_水平度量測_OK[i] = true;
                            }
                            this.List_CCD01_04_PIN量測參數_水平度量測距離[i] = 上排量測距離;
                        }
                        else if (下排量測距離 >= 0 && i >= 10)
                        {
                            if (下排標準值上限 <= Math.Abs(下排量測距離) * 1000 || 下排標準值下限 > Math.Abs(下排量測距離) * 1000)
                            {
                                this.List_CCD01_04_PIN量測參數_水平度量測_OK[i] = false;
                                PLC_Device_CCD01_04_PIN量測_檢測距離計算_OK.Bool = false;
                            }
                            else
                            {
                                this.List_CCD01_04_PIN量測參數_水平度量測_OK[i] = true;
                            }
                            this.List_CCD01_04_PIN量測參數_水平度量測距離[i] = 下排量測距離;
                        }

                    }
                }
                else
                {
                    this.List_CCD01_04_PIN量測參數_水平度量測_OK[i] = true;
                }
                if (PLC_Device_CCD01_04_PIN量測_間距量測不測試.Bool && PLC_Device_CCD01_04_PIN量測_水平度量測不測試.Bool)
                {
                    PLC_Device_CCD01_04_PIN量測_檢測距離計算_OK.Bool = true;
                }



            }
            for (int i = 0; i < 10; i++)
            {
                int 標準值 = this.PLC_Device_CCD01_04_PIN量測_上下間距量測標準值.Value;
                int 標準值上限 = this.PLC_Device_CCD01_04_PIN量測_上下間距量測上限值.Value;
                int 標準值下限 = this.PLC_Device_CCD01_04_PIN量測_上下間距量測下限值.Value;
                double 量測距離 = this.List_CCD01_04_PIN量測參數_上下間格距離[i];

                量測距離 = 量測距離 * 1000 - 標準值;
                量測距離 /= 1000;

                if (!PLC_Device_CCD01_04_PIN量測_水平度量測不測試.Bool)
                {
                    if (this.List_CCD01_04_PIN量測點參數_量測點_有無[i])
                    {
                        if (標準值上限 <= Math.Abs(量測距離) * 1000 || 標準值下限 > Math.Abs(量測距離) * 1000)
                        {
                            this.List_CCD01_04_PIN量測參數_上下間距量測_OK[i] = false;
                            PLC_Device_CCD01_04_PIN量測_檢測距離計算_OK.Bool = false;
                        }
                        else
                        {
                            this.List_CCD01_04_PIN量測參數_上下間距量測_OK[i] = true;
                        }
                        this.List_CCD01_04_PIN量測參數_上下間格距離[i] = 量測距離;
                    }
                }
                else
                {
                    this.List_CCD01_04_PIN量測參數_上下間距量測_OK[i] = true;
                }
                if (PLC_Device_CCD01_04_PIN量測_間距量測不測試.Bool && PLC_Device_CCD01_04_PIN量測_水平度量測不測試.Bool)
                {
                    PLC_Device_CCD01_04_PIN量測_檢測距離計算_OK.Bool = true;
                }



            }
            #endregion
            #region 間距上排PIN11到基準線量測

            double temp_上排PIN11到基準 = 0;
            int 間距上排PIN11到基準標準值 = this.PLC_Device_CCD01_04_PIN量測_間距上排PIN11到基準數值.Value;
            int 間距上排PIN11到基準標準值上限 = this.PLC_Device_CCD01_04_PIN量測_間距上排PIN11到基準上限.Value;
            int 間距上排PIN11到基準標準值下限 = this.PLC_Device_CCD01_04_PIN量測_間距上排PIN11到基準下限.Value;


            if (this.List_CCD01_04_PIN量測點參數_量測點_有無[0])
            {
                temp_上排PIN11到基準 = Math.Abs(this.List_CCD01_04_PIN量測點參數_量測點[0].X - this.Point_CCD01_03_中心基準座標_量測點.X);
                this.CCD01_04_PIN量測參數_間距上排PIN11到基準距離 = temp_上排PIN11到基準 * this.CCD01_比例尺_pixcel_To_mm;
            }
            else
            {
               // PLC_Device_CCD01_04_PIN量測_檢測距離計算_OK.Bool = false;
                CCD01_04_PIN量測參數_間距上排PIN11到基準_OK = false;
            }
            double 間距上排PIN11到基準量測距離 = this.CCD01_04_PIN量測參數_間距上排PIN11到基準距離;


            間距上排PIN11到基準量測距離 = 間距上排PIN11到基準量測距離 * 1000 - 間距上排PIN11到基準標準值;
            間距上排PIN11到基準量測距離 /= 1000;

            if (!PLC_Device_CCD01_04_PIN量測_間距量測不測試.Bool)
            {
                if (this.List_CCD01_04_PIN量測點參數_量測點_有無[0])
                {
                    if (間距上排PIN11到基準標準值上限 <= Math.Abs(間距上排PIN11到基準量測距離) * 1000 || 間距上排PIN11到基準標準值下限 >
                        Math.Abs(間距上排PIN11到基準量測距離) * 1000)
                    {
                        this.CCD01_04_PIN量測參數_間距上排PIN11到基準_OK = false;
                       // PLC_Device_CCD01_04_PIN量測_檢測距離計算_OK.Bool = false;
                    }
                    else
                    {
                        this.CCD01_04_PIN量測參數_間距上排PIN11到基準_OK = true;
                    }

                }
                CCD01_04_PIN量測參數_間距上排PIN11到基準距離 = 間距上排PIN11到基準量測距離;
            }
            else
            {
                this.CCD01_04_PIN量測參數_間距上排PIN11到基準_OK = true;
                //this.PLC_Device_CCD01_04_PIN量測_檢測距離計算_OK.Bool = true;
            }
            this.CCD01_04_PIN量測參數_間距上排PIN11到基準_OK = true;  //pass
            #endregion
            #region 間距下排PIN11到基準線量測

            double temp_下排PIN11到基準 = 0;
            int 間距下排PIN11到基準標準值 = this.PLC_Device_CCD01_04_PIN量測_間距下排PIN11到基準數值.Value;
            int 間距下排PIN11到基準標準值上限 = this.PLC_Device_CCD01_04_PIN量測_間距下排PIN11到基準上限.Value;
            int 間距下排PIN11到基準標準值下限 = this.PLC_Device_CCD01_04_PIN量測_間距下排PIN11到基準下限.Value;


            if (this.List_CCD01_04_PIN量測點參數_量測點_有無[10])
            {
                temp_下排PIN11到基準 = Math.Abs(this.List_CCD01_04_PIN量測點參數_量測點[10].X - this.Point_CCD01_03_中心基準座標_量測點.X);
                this.CCD01_04_PIN量測參數_間距下排PIN11到基準距離 = temp_下排PIN11到基準 * this.CCD01_比例尺_pixcel_To_mm;
            }
            else
            {
               // PLC_Device_CCD01_04_PIN量測_檢測距離計算_OK.Bool = false;
                CCD01_04_PIN量測參數_間距下排PIN11到基準_OK = false;
            }
            double 間距下排PIN11到基準量測距離 = this.CCD01_04_PIN量測參數_間距下排PIN11到基準距離;


            間距下排PIN11到基準量測距離 = 間距下排PIN11到基準量測距離 * 1000 - 間距下排PIN11到基準標準值;
            間距下排PIN11到基準量測距離 /= 1000;

            if (!PLC_Device_CCD01_04_PIN量測_間距量測不測試.Bool)
            {
                if (this.List_CCD01_04_PIN量測點參數_量測點_有無[10])
                {
                    if (間距下排PIN11到基準標準值上限 <= Math.Abs(間距下排PIN11到基準量測距離) * 1000 || 間距下排PIN11到基準標準值下限 >
                        Math.Abs(間距下排PIN11到基準量測距離) * 1000)
                    {
                        this.CCD01_04_PIN量測參數_間距下排PIN11到基準_OK = false;
                        //PLC_Device_CCD01_04_PIN量測_檢測距離計算_OK.Bool = false;
                    }
                    else
                    {
                        this.CCD01_04_PIN量測參數_間距下排PIN11到基準_OK = true;
                    }

                }
                CCD01_04_PIN量測參數_間距下排PIN11到基準距離 = 間距下排PIN11到基準量測距離;
            }
            else
            {
                this.CCD01_04_PIN量測參數_間距下排PIN11到基準_OK = true;
                //this.PLC_Device_CCD01_04_PIN量測_檢測距離計算_OK.Bool = true;
            }
            this.CCD01_04_PIN量測參數_間距下排PIN11到基準_OK = true;
            #endregion
            cnt++;
        }
        void cnt_Program_CCD01_04_PIN量測_檢測距離計算_繪製畫布(ref int cnt)
        {
            this.PLC_Device_CCD01_04_PIN量測_檢測距離計算_RefreshCanvas.Bool = true;
            if (this.PLC_Device_CCD01_04_PIN量測_檢測距離計算按鈕.Bool && !PLC_Device_CCD01_04_計算一次.Bool)
            {

                this.h_Canvas_Tech_CCD01_04.RefreshCanvas();
            }
            cnt++;
        }
        #endregion
        #region PLC_CCD01_04_PIN正位度量測_設定規範位置
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_設定規範按鈕 = new PLC_Device("S6450");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_設定規範位置 = new PLC_Device("S6445");
        PLC_Device PLC_Device_CCD01_04_PIN設定規範位置_OK = new PLC_Device("S6446");
        PLC_Device PLC_Device_CCD01_04_PIN設定規範位置_測試完成 = new PLC_Device("S6447");
        PLC_Device PLC_Device_CCD01_04_PIN設定規範位置_RefreshCanvas = new PLC_Device("S6446");
        private List<PLC_Device> List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X = new List<PLC_Device>();
        private List<PLC_Device> List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y = new List<PLC_Device>();
        private AxOvkPat.AxVisionInspectionFrame CCD01_04_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整;

        #region 正位度規範值

        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN21 = new PLC_Device("F11100");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN22 = new PLC_Device("F11101");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN23 = new PLC_Device("F11102");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN24 = new PLC_Device("F11103");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN25 = new PLC_Device("F11104");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN26 = new PLC_Device("F11105");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN27 = new PLC_Device("F11106");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN28 = new PLC_Device("F11107");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN29 = new PLC_Device("F11108");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN30 = new PLC_Device("F11109");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN31 = new PLC_Device("F11110");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN32 = new PLC_Device("F11111");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN33 = new PLC_Device("F11112");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN34 = new PLC_Device("F11113");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN35 = new PLC_Device("F11114");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN36 = new PLC_Device("F11115");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN37 = new PLC_Device("F11116");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN38 = new PLC_Device("F11117");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN39 = new PLC_Device("F11118");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN40 = new PLC_Device("F11119");

        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN21 = new PLC_Device("F11120");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN22 = new PLC_Device("F11121");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN23 = new PLC_Device("F11122");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN24 = new PLC_Device("F11123");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN25 = new PLC_Device("F11124");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN26 = new PLC_Device("F11125");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN27 = new PLC_Device("F11126");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN28 = new PLC_Device("F11127");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN29 = new PLC_Device("F11128");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN30 = new PLC_Device("F11129");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN31 = new PLC_Device("F11130");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN32 = new PLC_Device("F11131");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN33 = new PLC_Device("F11132");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN34 = new PLC_Device("F11133");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN35 = new PLC_Device("F11134");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN36 = new PLC_Device("F11135");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN37 = new PLC_Device("F11136");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN38 = new PLC_Device("F11137");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN39 = new PLC_Device("F11138");
        PLC_Device PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN40 = new PLC_Device("F11139");
        #endregion
        private PointF[] List_CCD01_04_PIN正位度量測參數_規範點 = new PointF[20];
        private PointF[] List_CCD01_04_PIN正位度量測參數_轉換後座標 = new PointF[20];
        private double[] List_CCD01_04_PIN正位度量測參數_正位度規範點_X = new double[20];
        private double[] List_CCD01_04_PIN正位度量測參數_正位度規範點_Y = new double[20];

        int cnt_Program_CCD01_04_PIN正位度量測_設定規範位置 = 65534;

        private void H_Canvas_Tech_CCD01_04_PIN正位度設定規範位置_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {

            if (PLC_Device_CCD01_04_Main_取像並檢驗.Bool || PLC_Device_CCD01_04_PLC觸發檢測.Bool || PLC_Device_CCD01_04_Main_檢驗一次.Bool)
            {
                try
                {
                    Graphics g = Graphics.FromHdc((IntPtr)HDC);
                    for (int i = 0; i < 20; i++)
                    {
                        DrawingClass.Draw.十字中心(new PointF((float)List_CCD01_04_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD01_04_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
                    }
                    g.Dispose();
                    g = null;
                }
                catch
                {

                }

            }

            else if (PLC_Device_CCD01_04_Tech_檢驗一次.Bool || PLC_Device_CCD01_04_Tech_取像並檢驗.Bool)
            {
                if (this.PLC_Device_CCD01_04_PIN設定規範位置_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        for (int i = 0; i < 20; i++)
                        {
                            DrawingClass.Draw.十字中心(new PointF((float)List_CCD01_04_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD01_04_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
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
                if (this.PLC_Device_CCD01_04_PIN設定規範位置_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        for (int i = 0; i < 20; i++)
                        {
                            DrawingClass.Draw.十字中心(new PointF((float)List_CCD01_04_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD01_04_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
                        }
                        g.Dispose();
                        g = null;
                    }
                    catch
                    {

                    }
                }
            }



            this.PLC_Device_CCD01_04_PIN設定規範位置_RefreshCanvas.Bool = false;
        }
        void sub_Program_CCD01_04_PIN正位度量測_設定規範位置()
        {
            if (cnt_Program_CCD01_04_PIN正位度量測_設定規範位置 == 65534)
            {

                this.h_Canvas_Tech_CCD01_04.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_04_PIN正位度設定規範位置_OnCanvasDrawEvent;
                this.h_Canvas_Main_CCD01_04_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_04_PIN正位度設定規範位置_OnCanvasDrawEvent;

                PLC_Device_CCD01_04_PIN正位度量測_設定規範位置.SetComment("PLC_CCD01_04_PIN正位度量測_設定規範位置");
                PLC_Device_CCD01_04_PIN正位度量測_設定規範按鈕.Bool = false;
                PLC_Device_CCD01_04_PIN正位度量測_設定規範位置.Bool = false;
                cnt_Program_CCD01_04_PIN正位度量測_設定規範位置 = 65535;
                #region 正位度規範值
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN21);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN22);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN23);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN24);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN25);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN26);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN27);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN28);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN29);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN30);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN31);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN32);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN33);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN34);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN35);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN36);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN37);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN38);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN39);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_X_PIN40);

                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN21);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN22);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN23);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN24);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN25);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN26);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN27);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN28);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN29);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN30);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN31);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN32);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN33);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN34);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN35);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN36);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN37);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN38);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN39);
                this.List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y.Add(this.PLC_Device_CCD01_04_PIN正位度量測_正位度規範值_Y_PIN40);
                #endregion
            }
            if (cnt_Program_CCD01_04_PIN正位度量測_設定規範位置 == 65535) cnt_Program_CCD01_04_PIN正位度量測_設定規範位置 = 1;
            if (cnt_Program_CCD01_04_PIN正位度量測_設定規範位置 == 1) cnt_Program_CCD01_04_PIN正位度量測_設定規範位置_觸發按下(ref cnt_Program_CCD01_04_PIN正位度量測_設定規範位置);
            if (cnt_Program_CCD01_04_PIN正位度量測_設定規範位置 == 2) cnt_Program_CCD01_04_PIN正位度量測_設定規範位置_檢查按下(ref cnt_Program_CCD01_04_PIN正位度量測_設定規範位置);
            if (cnt_Program_CCD01_04_PIN正位度量測_設定規範位置 == 3) cnt_Program_CCD01_04_PIN正位度量測_設定規範位置_初始化(ref cnt_Program_CCD01_04_PIN正位度量測_設定規範位置);
            if (cnt_Program_CCD01_04_PIN正位度量測_設定規範位置 == 4) cnt_Program_CCD01_04_PIN正位度量測_設定規範位置_座標轉換(ref cnt_Program_CCD01_04_PIN正位度量測_設定規範位置);
            if (cnt_Program_CCD01_04_PIN正位度量測_設定規範位置 == 5) cnt_Program_CCD01_04_PIN正位度量測_設定規範位置_讀取參數(ref cnt_Program_CCD01_04_PIN正位度量測_設定規範位置);
            if (cnt_Program_CCD01_04_PIN正位度量測_設定規範位置 == 6) cnt_Program_CCD01_04_PIN正位度量測_設定規範位置_繪製畫布(ref cnt_Program_CCD01_04_PIN正位度量測_設定規範位置);
            if (cnt_Program_CCD01_04_PIN正位度量測_設定規範位置 == 7) cnt_Program_CCD01_04_PIN正位度量測_設定規範位置 = 65500;
            if (cnt_Program_CCD01_04_PIN正位度量測_設定規範位置 > 1) cnt_Program_CCD01_04_PIN正位度量測_設定規範位置_檢查放開(ref cnt_Program_CCD01_04_PIN正位度量測_設定規範位置);

            if (cnt_Program_CCD01_04_PIN正位度量測_設定規範位置 == 65500)
            {
                if (PLC_Device_CCD01_04_計算一次.Bool)
                {
                    PLC_Device_CCD01_04_PIN正位度量測_設定規範按鈕.Bool = false;
                    PLC_Device_CCD01_04_PIN正位度量測_設定規範位置.Bool = false;
                }
                cnt_Program_CCD01_04_PIN正位度量測_設定規範位置 = 65535;
            }
        }
        void cnt_Program_CCD01_04_PIN正位度量測_設定規範位置_觸發按下(ref int cnt)
        {
            if (PLC_Device_CCD01_04_PIN正位度量測_設定規範按鈕.Bool)
            {
                PLC_Device_CCD01_04_PIN正位度量測_設定規範位置.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_04_PIN正位度量測_設定規範位置_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_04_PIN正位度量測_設定規範位置.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_04_PIN正位度量測_設定規範位置_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_04_PIN正位度量測_設定規範按鈕.Bool)
            {
                PLC_Device_CCD01_04_PIN正位度量測_設定規範位置.Bool = false;
                cnt = 65500;
            }
        }
        void cnt_Program_CCD01_04_PIN正位度量測_設定規範位置_初始化(ref int cnt)
        {
            this.List_CCD01_04_PIN正位度量測參數_正位度規範點_X = new double[20];
            this.List_CCD01_04_PIN正位度量測參數_正位度規範點_Y = new double[20];
            this.List_CCD01_04_PIN正位度量測參數_規範點 = new PointF[20];
            this.List_CCD01_04_PIN正位度量測參數_轉換後座標 = new PointF[20];
            cnt++;
        }
        void cnt_Program_CCD01_04_PIN正位度量測_設定規範位置_座標轉換(ref int cnt)
        {
            if (PLC_Device_CCD01_04_計算一次.Bool)
            {
                CCD01_04_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.RefPointX = PLC_Device_CCD01_03_水平基準線量測_量測中心_X.Value;
                CCD01_04_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.RefPointY = PLC_Device_CCD01_03_水平基準線量測_量測中心_Y.Value;
                CCD01_04_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.RefAngle = 0;
                CCD01_04_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.CurrentRefPointX = Point_CCD01_03_中心基準座標_量測點.X;
                CCD01_04_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.CurrentRefPointY = Point_CCD01_03_中心基準座標_量測點.Y;
                CCD01_04_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.CurrentRefAngle = 0;
                CCD01_04_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.NumOfVisionPoints = 20;

                for (int j = 0; j < 20; j++)
                {

                    CCD01_04_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
                    CCD01_04_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointX = (float)(List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X[j].Value);
                    CCD01_04_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointX = CCD01_04_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointX / 1;
                    CCD01_04_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointY = (float)(List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y[j].Value);
                    CCD01_04_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointY = CCD01_04_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointY / 1;

                }
                CCD01_04_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.EstimateCurrentVisionPoints();
                for (int j = 0; j < 20; j++)
                {

                    CCD01_04_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.VisionPointIndex = j;
                    List_CCD01_04_PIN正位度量測參數_轉換後座標[j].X = (int)CCD01_04_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.CurrentVisionPointX;
                    List_CCD01_04_PIN正位度量測參數_轉換後座標[j].Y = (int)CCD01_04_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整.CurrentVisionPointY;

                }
            }
            cnt++;
        }
        void cnt_Program_CCD01_04_PIN正位度量測_設定規範位置_讀取參數(ref int cnt)
        {

            for (int i = 0; i < 20; i++)
            {
                if (PLC_Device_CCD01_04_計算一次.Bool)
                {
                    this.List_CCD01_04_PIN正位度量測參數_正位度規範點_X[i] = List_CCD01_04_PIN正位度量測參數_轉換後座標[i].X;
                    this.List_CCD01_04_PIN正位度量測參數_正位度規範點_Y[i] = List_CCD01_04_PIN正位度量測參數_轉換後座標[i].Y;
                }
                else
                {
                    this.List_CCD01_04_PIN正位度量測參數_正位度規範點_X[i] = (float)(List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X[i].Value);
                    this.List_CCD01_04_PIN正位度量測參數_正位度規範點_X[i] = this.List_CCD01_04_PIN正位度量測參數_正位度規範點_X[i] / 1;
                    this.List_CCD01_04_PIN正位度量測參數_正位度規範點_Y[i] = (float)(List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y[i].Value);
                    this.List_CCD01_04_PIN正位度量測參數_正位度規範點_Y[i] = this.List_CCD01_04_PIN正位度量測參數_正位度規範點_Y[i] / 1;
                }
                List_CCD01_04_PIN正位度量測參數_規範點[i].X = (float)this.List_CCD01_04_PIN正位度量測參數_正位度規範點_X[i];
                List_CCD01_04_PIN正位度量測參數_規範點[i].Y = (float)this.List_CCD01_04_PIN正位度量測參數_正位度規範點_Y[i];

            }
            cnt++;
        }
        void cnt_Program_CCD01_04_PIN正位度量測_設定規範位置_繪製畫布(ref int cnt)
        {
            this.PLC_Device_CCD01_04_PIN設定規範位置_RefreshCanvas.Bool = true;
            if (this.PLC_Device_CCD01_04_PIN正位度量測_設定規範按鈕.Bool && !PLC_Device_CCD01_04_計算一次.Bool)
            {
                this.h_Canvas_Tech_CCD01_04.RefreshCanvas();
            }

            cnt++;
        }



        #endregion
        #region PLC_CCD01_04_PIN量測_檢測正位度計算

        MyTimer MyTimer_CCD01_04_PIN量測_檢測正位度計算 = new MyTimer();
        PLC_Device PLC_Device_CCD01_04_PIN量測_檢測正位度計算按鈕 = new PLC_Device("S6470");
        PLC_Device PLC_Device_CCD01_04_PIN量測_檢測正位度計算 = new PLC_Device("S6465");
        PLC_Device PLC_Device_CCD01_04_PIN量測_檢測正位度計算_OK = new PLC_Device("S6466");
        PLC_Device PLC_Device_CCD01_04_PIN量測_檢測正位度計算_測試完成 = new PLC_Device("S6467");
        PLC_Device PLC_Device_CCD01_04_PIN量測_檢測正位度計算_RefreshCanvas = new PLC_Device("S6468");
        PLC_Device PLC_Device_CCD01_04_PIN量測_檢測正位度計算_不測試 = new PLC_Device("S6106");
        PLC_Device PLC_Device_CCD01_04_PIN量測_檢測正位度計算_差值 = new PLC_Device("F15001");

        private double[] List_CCD01_04_PIN正位度量測參數_正位度距離 = new double[20];
        private bool[] List_CCD01_04_PIN正位度量測參數_正位度量測點_OK = new bool[20];


        private void H_Canvas_Tech_CCD01_04_PIN量測正位度_OnCanvasDrawEvent(long HDC, float ZoomX, float ZoomY, int CanvasHandle)
        {
            if (PLC_Device_CCD01_04_Main_取像並檢驗.Bool || PLC_Device_CCD01_04_PLC觸發檢測.Bool || PLC_Device_CCD01_04_Main_檢驗一次.Bool)
            {
                try
                {
                    Graphics g = Graphics.FromHdc((IntPtr)HDC);
                    Font font = new Font("微軟正黑體", 10);
                    string 正位度差值顯示;
                    for (int i = 0; i < 20; i++)
                    {
                        DrawingClass.Draw.十字中心(new PointF((float)List_CCD01_04_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD01_04_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
                        DrawingClass.Draw.十字中心(this.List_CCD01_04_PIN量測點參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);

                    }
                    #region 正位度量測結果顯示
                    if (PLC_Device_CCD01_04_PIN量測_檢測正位度計算_OK.Bool)
                    {
                        DrawingClass.Draw.文字左上繪製("正位OK:", new PointF(1200, 0), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                    }
                    else
                    {
                        DrawingClass.Draw.文字左上繪製("正位NG:", new PointF(1200, 0), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                    }
                    #endregion
                    #region PIN正位結果顯示
                    for (int i = 0; i < 20; i++)
                    {

                        if (this.List_CCD01_04_PIN正位度量測參數_正位度量測點_OK[i])
                        {
                            if (i <= 9)
                            {
                                正位度差值顯示 = ("上:P" + (i + 1).ToString("00:") + this.List_CCD01_04_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1600, i * 55), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                            }

                            if (i >= 10)
                            {
                                正位度差值顯示 = ("下:P" + ((i - 10) + 1).ToString("00:") + this.List_CCD01_04_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(2100, (i - 10) * 55), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                            }
                        }
                        else
                        {
                            if (i <= 9)
                            {
                                正位度差值顯示 = ("上:P" + (i + 1).ToString("00:") + this.List_CCD01_04_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1600, i * 55), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                            }

                            if (i >= 10)
                            {
                                正位度差值顯示 = ("下:P" + ((i - 10) + 1).ToString("00:") + this.List_CCD01_04_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(2100, (i - 10) * 55), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

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
            if (PLC_Device_CCD01_04_Tech_檢驗一次.Bool || PLC_Device_CCD01_04_Tech_取像並檢驗.Bool)
            {
                if (this.PLC_Device_CCD01_04_PIN量測_檢測正位度計算_RefreshCanvas.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);
                        string 正位度差值顯示;
                        for (int i = 0; i < 20; i++)
                        {
                            DrawingClass.Draw.十字中心(new PointF((float)List_CCD01_04_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD01_04_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
                            DrawingClass.Draw.十字中心(this.List_CCD01_04_PIN量測點參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);

                        }
                        #region 正位度量測結果顯示
                        if (PLC_Device_CCD01_04_PIN量測_檢測正位度計算_OK.Bool)
                        {
                            DrawingClass.Draw.文字左上繪製("正位度數值OK:", new PointF(1500, 0), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        }
                        else
                        {
                            DrawingClass.Draw.文字左上繪製("正位度數值NG:", new PointF(1500, 0), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        }
                        #endregion
                        #region PIN正位結果顯示
                        for (int i = 0; i < 20; i++)
                        {

                            if (this.List_CCD01_04_PIN正位度量測參數_正位度量測點_OK[i])
                            {
                                if (i <= 9)
                                {
                                    正位度差值顯示 = ("上排:P" + (i + 1).ToString("00:") + this.List_CCD01_04_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1900, i * 40), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                                }

                                if (i >= 10)
                                {
                                    正位度差值顯示 = ("下排:P" + ((i - 10) + 1).ToString("00:") + this.List_CCD01_04_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(2200, (i - 10) * 40), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                                }
                            }
                            else
                            {
                                if (i <= 9)
                                {
                                    正位度差值顯示 = ("上排:P" + (i + 1).ToString("00:") + this.List_CCD01_04_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1900, i * 40), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                                }

                                if (i >= 10)
                                {
                                    正位度差值顯示 = ("下排:P" + ((i - 10) + 1).ToString("00:") + this.List_CCD01_04_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(2200, (i - 10) * 40), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

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
                if (this.PLC_Device_CCD01_04_PIN量測_檢測正位度計算_RefreshCanvas.Bool && PLC_Device_CCD01_04_PIN量測_檢測正位度計算.Bool)
                {
                    try
                    {
                        Graphics g = Graphics.FromHdc((IntPtr)HDC);
                        Font font = new Font("微軟正黑體", 10);
                        string 正位度差值顯示;
                        for (int i = 0; i < 20; i++)
                        {
                            DrawingClass.Draw.十字中心(new PointF((float)List_CCD01_04_PIN正位度量測參數_正位度規範點_X[i], (float)List_CCD01_04_PIN正位度量測參數_正位度規範點_Y[i]), 50, Color.Red, 2, g, ZoomX, ZoomY);
                            DrawingClass.Draw.十字中心(this.List_CCD01_04_PIN量測點參數_量測點[i], 50, Color.Lime, 2, g, ZoomX, ZoomY);
                        }
                        #region 正位度量測結果顯示
                        if (PLC_Device_CCD01_04_PIN量測_檢測正位度計算_OK.Bool)
                        {
                            DrawingClass.Draw.文字左上繪製("正位度數值OK:", new PointF(1500, 0), new Font("標楷體", 16), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                        }
                        else
                        {
                            DrawingClass.Draw.文字左上繪製("正位度數值NG:", new PointF(1500, 0), new Font("標楷體", 16), Color.Black, Color.Red, g, ZoomX, ZoomY);
                        }
                        #endregion
                        #region PIN正位結果顯示
                        for (int i = 0; i < 20; i++)
                        {

                            if (this.List_CCD01_04_PIN正位度量測參數_正位度量測點_OK[i])
                            {
                                if (i <= 9)
                                {
                                    正位度差值顯示 = ("上排:P" + (i + 1).ToString("00:") + this.List_CCD01_04_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1900, i * 40), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);
                                }

                                if (i >= 10)
                                {
                                    正位度差值顯示 = ("下排:P" + ((i - 10) + 1).ToString("00:") + this.List_CCD01_04_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(2200, (i - 10) * 40), new Font("標楷體", 10), Color.Black, Color.Lime, g, ZoomX, ZoomY);

                                }
                            }
                            else
                            {
                                if (i <= 9)
                                {
                                    正位度差值顯示 = ("上排:P" + (i + 1).ToString("00:") + this.List_CCD01_04_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(1900, i * 40), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);
                                }

                                if (i >= 10)
                                {
                                    正位度差值顯示 = ("下排:P" + ((i - 10) + 1).ToString("00:") + this.List_CCD01_04_PIN正位度量測參數_正位度距離[i].ToString("0.000"));
                                    DrawingClass.Draw.文字左上繪製(正位度差值顯示, new PointF(2200, (i - 10) * 40), new Font("標楷體", 10), Color.Black, Color.Red, g, ZoomX, ZoomY);

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

            this.PLC_Device_CCD01_04_PIN量測_檢測正位度計算_RefreshCanvas.Bool = false;
        }

        int cnt_Program_CCD01_04_PIN量測_檢測正位度計算 = 65534;
        void sub_Program_CCD01_04_PIN量測_檢測正位度計算()
        {
            if (cnt_Program_CCD01_04_PIN量測_檢測正位度計算 == 65534)
            {
                this.h_Canvas_Tech_CCD01_04.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_04_PIN量測正位度_OnCanvasDrawEvent;
                this.h_Canvas_Main_CCD01_04_檢測畫面.OnCanvasDrawEvent += H_Canvas_Tech_CCD01_04_PIN量測正位度_OnCanvasDrawEvent;
                PLC_Device_CCD01_04_PIN量測_檢測正位度計算.SetComment("PLC_CCD01_04_PIN量測_檢測正位度計算");
                PLC_Device_CCD01_04_PIN量測_檢測正位度計算.Bool = false;
                PLC_Device_CCD01_04_PIN量測_檢測正位度計算按鈕.Bool = false;
                cnt_Program_CCD01_04_PIN量測_檢測正位度計算 = 65535;

            }
            if (cnt_Program_CCD01_04_PIN量測_檢測正位度計算 == 65535) cnt_Program_CCD01_04_PIN量測_檢測正位度計算 = 1;
            if (cnt_Program_CCD01_04_PIN量測_檢測正位度計算 == 1) cnt_Program_CCD01_04_PIN量測_檢測正位度計算_觸發按下(ref cnt_Program_CCD01_04_PIN量測_檢測正位度計算);
            if (cnt_Program_CCD01_04_PIN量測_檢測正位度計算 == 2) cnt_Program_CCD01_04_PIN量測_檢測正位度計算_檢查按下(ref cnt_Program_CCD01_04_PIN量測_檢測正位度計算);
            if (cnt_Program_CCD01_04_PIN量測_檢測正位度計算 == 3) cnt_Program_CCD01_04_PIN量測_檢測正位度計算_初始化(ref cnt_Program_CCD01_04_PIN量測_檢測正位度計算);
            if (cnt_Program_CCD01_04_PIN量測_檢測正位度計算 == 4) cnt_Program_CCD01_04_PIN量測_檢測正位度計算_數值計算(ref cnt_Program_CCD01_04_PIN量測_檢測正位度計算);
            if (cnt_Program_CCD01_04_PIN量測_檢測正位度計算 == 5) cnt_Program_CCD01_04_PIN量測_檢測正位度計算_量測結果(ref cnt_Program_CCD01_04_PIN量測_檢測正位度計算);
            if (cnt_Program_CCD01_04_PIN量測_檢測正位度計算 == 6) cnt_Program_CCD01_04_PIN量測_檢測正位度計算_繪製畫布(ref cnt_Program_CCD01_04_PIN量測_檢測正位度計算);
            if (cnt_Program_CCD01_04_PIN量測_檢測正位度計算 == 7) cnt_Program_CCD01_04_PIN量測_檢測正位度計算 = 65500;
            if (cnt_Program_CCD01_04_PIN量測_檢測正位度計算 > 1) cnt_Program_CCD01_04_PIN量測_檢測正位度計算_檢查放開(ref cnt_Program_CCD01_04_PIN量測_檢測正位度計算);

            if (cnt_Program_CCD01_04_PIN量測_檢測正位度計算 == 65500)
            {
                PLC_Device_CCD01_04_PIN量測_檢測正位度計算.Bool = false;
                PLC_Device_CCD01_04_PIN量測_檢測正位度計算按鈕.Bool = false;
                cnt_Program_CCD01_04_PIN量測_檢測正位度計算 = 65535;
            }
        }
        void cnt_Program_CCD01_04_PIN量測_檢測正位度計算_觸發按下(ref int cnt)
        {
            if (PLC_Device_CCD01_04_PIN量測_檢測正位度計算按鈕.Bool)
            {
                PLC_Device_CCD01_04_PIN量測_檢測正位度計算.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_CCD01_04_PIN量測_檢測正位度計算_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_04_PIN量測_檢測正位度計算.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_CCD01_04_PIN量測_檢測正位度計算_檢查放開(ref int cnt)
        {
            //if (!PLC_Device_CCD01_04_PIN量測_檢測正位度計算按鈕.Bool)
            //{
            //    cnt = 65500;
            //}
        }
        void cnt_Program_CCD01_04_PIN量測_檢測正位度計算_初始化(ref int cnt)
        {
            this.MyTimer_CCD01_04_PIN量測_檢測正位度計算.TickStop();
            this.MyTimer_CCD01_04_PIN量測_檢測正位度計算.StartTickTime(99999);
            this.List_CCD01_04_PIN正位度量測參數_正位度距離 = new double[20];
            this.List_CCD01_04_PIN正位度量測參數_正位度量測點_OK = new bool[20];

            cnt++;
        }
        void cnt_Program_CCD01_04_PIN量測_檢測正位度計算_數值計算(ref int cnt)
        {
            double distance = 0;
            double temp_X = 0;
            double temp_Y = 0;
            PLC_Device_CCD01_04_PIN量測_檢測正位度計算_OK.Bool = true;

            for (int i = 0; i < 20; i++)
            {
                if (this.List_CCD01_04_PIN量測點參數_量測點_有無[i])
                {
                    temp_X = Math.Pow(this.List_CCD01_04_PIN量測點參數_量測點[i].X - this.List_CCD01_04_PIN正位度量測參數_規範點[i].X, 2);
                    temp_Y = Math.Pow(this.List_CCD01_04_PIN量測點參數_量測點[i].Y - this.List_CCD01_04_PIN正位度量測參數_規範點[i].Y, 2);

                    distance = Math.Sqrt(temp_X + temp_Y);
                    this.List_CCD01_04_PIN正位度量測參數_正位度距離[i] = distance * this.CCD01_比例尺_pixcel_To_mm;
                }
                else
                {
                    PLC_Device_CCD01_04_PIN量測_檢測正位度計算_OK.Bool = false;
                    List_CCD01_04_PIN正位度量測參數_正位度量測點_OK[i] = false;
                }

            }
            cnt++;
        }
        void cnt_Program_CCD01_04_PIN量測_檢測正位度計算_量測結果(ref int cnt)
        {

            PLC_Device_CCD01_04_PIN量測_檢測正位度計算_OK.Bool = true; // 檢測結果初始化

            for (int i = 0; i < 20; i++)
            {
                int 標準值差值 = this.PLC_Device_CCD01_04_PIN量測_檢測正位度計算_差值.Value;
                double 量測距離 = this.List_CCD01_04_PIN正位度量測參數_正位度距離[i];

                量測距離 = 量測距離 * 1000;
                量測距離 /= 1000;

                if (!PLC_Device_CCD01_04_PIN量測_檢測正位度計算_不測試.Bool)
                {
                    if (this.List_CCD01_04_PIN量測點參數_量測點_有無[i])
                    {


                        if (量測距離 >= 0)
                        {
                            if (標準值差值 <= Math.Abs(量測距離) * 1000)
                            {
                                this.List_CCD01_04_PIN正位度量測參數_正位度量測點_OK[i] = false;
                                PLC_Device_CCD01_04_PIN量測_檢測正位度計算_OK.Bool = false;
                            }
                            else
                            {
                                this.List_CCD01_04_PIN正位度量測參數_正位度量測點_OK[i] = true;
                            }
                        }

                    }
                    else
                    {
                        this.List_CCD01_04_PIN正位度量測參數_正位度量測點_OK[i] = false;
                        PLC_Device_CCD01_04_PIN量測_檢測正位度計算_OK.Bool = false;
                    }

                }
                else
                {
                    this.List_CCD01_04_PIN正位度量測參數_正位度量測點_OK[i] = true;
                }

                this.List_CCD01_04_PIN正位度量測參數_正位度距離[i] = 量測距離;
            }
            cnt++;
        }
        void cnt_Program_CCD01_04_PIN量測_檢測正位度計算_繪製畫布(ref int cnt)
        {
            this.PLC_Device_CCD01_04_PIN量測_檢測正位度計算_RefreshCanvas.Bool = true;
            if (this.PLC_Device_CCD01_04_PIN量測_檢測正位度計算按鈕.Bool && !PLC_Device_CCD01_04_計算一次.Bool)
            {

                this.h_Canvas_Tech_CCD01_04.RefreshCanvas();
            }

            cnt++;
        }
        #endregion

        #region Event

        private void plC_RJ_Button_CCD01_04_儲存圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (saveImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.h_Canvas_Tech_CCD01_04.SaveImage(saveImageDialog.FileName);
                }
            }));
        }
        private void plC_RJ_Button_CCD01_04_讀取圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (openImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.CCD01_AxImageBW8.LoadFile(openImageDialog.FileName);
                    try
                    {
                        this.h_Canvas_Tech_CCD01_04.ImageCopy(CCD01_AxImageBW8.VegaHandle);
                        this.CCD01_04_SrcImageHandle = h_Canvas_Tech_CCD01_04.VegaHandle;
                        this.h_Canvas_Tech_CCD01_04.RefreshCanvas();
                    }
                    catch
                    {
                        err_message_01_04 = MessageBox.Show("讀取圖片空白", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        if (err_message_01_04 == DialogResult.OK)
                        {

                        }
                    }
                }
            }));
        }
        private void PlC_RJ_Button_Main_CCD01_04儲存圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (saveImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.h_Canvas_Main_CCD01_04_檢測畫面.SaveImage(saveImageDialog.FileName);
                }
            }));
        }
        private void PlC_RJ_Button_Main_CCD01_04讀取圖片_MouseClickEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate {
                if (openImageDialog.ShowDialog() == DialogResult.OK)
                {
                    this.CCD01_AxImageBW8.LoadFile(openImageDialog.FileName);
                    try
                    {
                        this.h_Canvas_Main_CCD01_04_檢測畫面.ImageCopy(CCD01_AxImageBW8.VegaHandle);
                        this.CCD01_04_SrcImageHandle = h_Canvas_Main_CCD01_04_檢測畫面.VegaHandle;
                        this.h_Canvas_Main_CCD01_04_檢測畫面.RefreshCanvas();
                    }
                    catch
                    {
                        err_message_01_04 = MessageBox.Show("讀取圖片空白", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        if (err_message_01_04 == DialogResult.OK)
                        {

                        }
                    }
                }
            }));
        }
        private void PlC_Button_Main_CCD01_04_ZOOM更新_btnClick(object sender, EventArgs e)
        {
            if (CCD01_04_SrcImageHandle != 0)
            {
                PLC_Device_Main_CCD01_04_ZOOM更新.Bool = true;
                h_Canvas_Main_CCD01_04_檢測畫面.RefreshCanvas();
            }
        }
        private void PlC_RJ_Button_CCD01_04_Tech_PIN量測框大小設為一致_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 20; i++)
            {
                this.List_PLC_Device_CCD01_04_PIN量測參數_Width[i].Value = this.List_PLC_Device_CCD01_04_PIN量測參數_Width[0].Value;
                this.List_PLC_Device_CCD01_04_PIN量測參數_Height[i].Value = this.List_PLC_Device_CCD01_04_PIN量測參數_Height[0].Value;
                //this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值[i].Value = this.List_PLC_Device_CCD01_04_PIN量測參數_灰階門檻值[0].Value;
                //this.List_PLC_Device_CCD01_04_PIN量測參數_面積上限[i].Value = this.List_PLC_Device_CCD01_04_PIN量測參數_面積上限[0].Value;
                //this.List_PLC_Device_CCD01_04_PIN量測參數_面積下限[i].Value = this.List_PLC_Device_CCD01_04_PIN量測參數_面積下限[0].Value;

            }
        }
        private PLC_Device PLC_Device_CCD01_04_PIN量測一鍵排列間距 = new PLC_Device("F4001");
        private void PlC_RJ_Button_CCD01_04_Tech_PIN量測框一鍵排列_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 20; i++)
            {
                if (i < 10)
                {
                    this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX[i].Value = this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX[0].Value - i * PLC_Device_CCD01_04_PIN量測一鍵排列間距.Value;
                    this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY[i].Value = this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY[0].Value;
                }

                else
                {
                    this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX[i].Value = this.List_PLC_Device_CCD01_04_PIN量測參數_OrgX[10].Value - (i - 10) * PLC_Device_CCD01_04_PIN量測一鍵排列間距.Value;
                    this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY[i].Value = this.List_PLC_Device_CCD01_04_PIN量測參數_OrgY[10].Value;
                }

            }
        }
        private void PlC_Button_CCD01_04_量測點作為規範點_btnClick(object sender, EventArgs e)
        {
            for (int i = 0; i < this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整.Count; i++)
            {
                List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_X[i].Value = (int)this.List_CCD01_04_PIN量測點參數_量測點[i].X;
                List_PLC_Device_CCD01_04_PIN正位度量測參數_正位度規範值_Y[i].Value = (int)this.List_CCD01_04_PIN量測點參數_量測點[i].Y;
            }
        }

        private void plC_RJ_Button_CCD01_04_線段量測框大小設為一致_MouseClickEvent(MouseEventArgs mevent)
        {
            for (int i = 0; i < 20; i++)
            {
                this.PLC_Device_CCD01_04_量測框Width[i].Value = this.PLC_Device_CCD01_04_量測框Width[0].Value;
                this.PLC_Device_CCD01_04_量測框Height[i].Value = this.PLC_Device_CCD01_04_量測框Height[0].Value;

            }
        }

        private void plC_RJ_Button_CCD01_04_線段量測框一鍵排列_MouseClickEvent(MouseEventArgs mevent)
        {
            for (int i = 0; i < 20; i++)
            {
                if (i < 10)
                {
                    this.PLC_Device_CCD01_04_量測框OrgX[i].Value = this.PLC_Device_CCD01_04_量測框OrgX[0].Value - i * PLC_Device_CCD01_04_PIN量測一鍵排列間距.Value;
                    this.PLC_Device_CCD01_04_量測框OrgY[i].Value = this.PLC_Device_CCD01_04_量測框OrgY[0].Value;
                }

                else
                {
                    this.PLC_Device_CCD01_04_量測框OrgX[i].Value = this.PLC_Device_CCD01_04_量測框OrgX[10].Value - (i - 10) * PLC_Device_CCD01_04_PIN量測一鍵排列間距.Value;
                    this.PLC_Device_CCD01_04_量測框OrgY[i].Value = this.PLC_Device_CCD01_04_量測框OrgY[10].Value;
                }

            }
        }
        #endregion
    }
}
