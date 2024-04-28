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

namespace 文信40PIN_CCD
{
    public partial class Form1 : Form
    {
        private PLC_Device PLC_Device_CCD01_比例尺_mm_pixcel = new PLC_Device("F220");
        private PLC_Device PLC_Device_CCD01_比例尺_mm_pixcel_buff = new PLC_Device("F221");
        private void button_CCD01_比例尺轉換確認_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("確定轉換比例尺?", "確認", MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                MessageBox.Show("轉換成功");
                PLC_Device_CCD01_比例尺_mm_pixcel.Value = PLC_Device_CCD01_比例尺_mm_pixcel_buff.Value;
            }
            else
            {
                MessageBox.Show("取消");
                PLC_Device_CCD01_比例尺_mm_pixcel.Value = PLC_Device_CCD01_比例尺_mm_pixcel.Value;

            }
        }
        private double CCD01_比例尺_pixcel_To_mm
        {
            get
            {
                //return 1;
                return (double)PLC_Device_CCD01_比例尺_mm_pixcel.Value / Math.Pow(10, 6);
            }
        }
        private double CCD01_比例尺_mm_To_pixcel
        {
            get
            {
                return 1;
                return (double)PLC_Device_CCD01_比例尺_mm_pixcel.Value / Math.Pow(10, 6);
            }
        }
        bool flag_CCD01_取像完成 = false;
        private AxOvkBase.AxImageBW8 CCD01_AxImageBW8_ORG = new AxOvkBase.AxImageBW8();
        private AxOvkBase.AxImageBW8 CCD01_AxImageBW8 = new AxOvkBase.AxImageBW8();
        private AxOvkImage.AxImageCopier CCD01_AxImageCopier = new AxOvkImage.AxImageCopier();
        private long CCD01_01_SrcImageHandle;
        private long CCD01_02_SrcImageHandle;
        private long CCD01_03_SrcImageHandle;
        private long CCD01_04_SrcImageHandle;

        #region PLC_CCD01_SNAP
        PLC_Device PLC_Device_CCD01_SNAP = new PLC_Device("S39800");
        PLC_Device PLC_Device_CCD01_SNAP開啟硬體觸發 = new PLC_Device("S39810");
        PLC_Device PLC_Device_CCD01_SNAP_電子快門 = new PLC_Device("F10100");
        PLC_Device PLC_Device_CCD01_SNAP_視訊增益 = new PLC_Device("F10101");
        PLC_Device PLC_Device_CCD01_SNAP_銳利度 = new PLC_Device("F10102");
        PLC_Device PLC_Device_CCD01_SNAP_取像時間 = new PLC_Device("F10103");
        PLC_Device PLC_Device_CCD01_SNAP_ActiveHandle = new PLC_Device("F10104");
        MyTimer MyTimer_CCD01_01_取像時間 = new MyTimer();
        int cnt_Program_CCD01_SNAP = 65534;

        void sub_Program_CCD01_SNAP()
        {

            if (cnt_Program_CCD01_SNAP == 65534)
            {
                this.plC_MindVision_Camera_UI_CCD01.OnSufaceDrawEvent += plC_MindVision_Camera_UI_CCD01_OnSufaceDrawEvent;

                PLC_Device_CCD01_SNAP.SetComment("PLC_CCD01_SNAP");
                PLC_Device_CCD01_SNAP.Bool = false;
                cnt_Program_CCD01_SNAP = 65535;
            }
            if (cnt_Program_CCD01_SNAP == 65535) cnt_Program_CCD01_SNAP = 1;
            if (cnt_Program_CCD01_SNAP == 1) cnt_Program_CCD01_SNAP_檢查按下(ref cnt_Program_CCD01_SNAP);
            if (cnt_Program_CCD01_SNAP == 2) cnt_Program_CCD01_SNAP_初始化(ref cnt_Program_CCD01_SNAP);
            if (cnt_Program_CCD01_SNAP == 3) cnt_Program_CCD01_SNAP_觸發一次(ref cnt_Program_CCD01_SNAP);
            if (cnt_Program_CCD01_SNAP == 4) cnt_Program_CCD01_SNAP_觸發完成(ref cnt_Program_CCD01_SNAP);
            if (cnt_Program_CCD01_SNAP == 5) cnt_Program_CCD01_SNAP_等待取像完成(ref cnt_Program_CCD01_SNAP);
            if (cnt_Program_CCD01_SNAP == 6) cnt_Program_CCD01_SNAP_取像畫面更新(ref cnt_Program_CCD01_SNAP);
            if (cnt_Program_CCD01_SNAP == 7) cnt_Program_CCD01_SNAP_取像完成(ref cnt_Program_CCD01_SNAP);
            if (cnt_Program_CCD01_SNAP == 8) cnt_Program_CCD01_SNAP = 65500;
            if (cnt_Program_CCD01_SNAP > 1) cnt_Program_CCD01_SNAP_檢查放開(ref cnt_Program_CCD01_SNAP);

            if (cnt_Program_CCD01_SNAP == 65500)
            {
                PLC_Device_CCD01_SNAP.Bool = false;
                cnt_Program_CCD01_SNAP = 65535;
            }
        }
        void cnt_Program_CCD01_SNAP_檢查按下(ref int cnt)
        {
            if (PLC_Device_CCD01_SNAP.Bool) cnt++;
        }
        void cnt_Program_CCD01_SNAP_檢查放開(ref int cnt)
        {
            if (!PLC_Device_CCD01_SNAP.Bool) cnt = 65500;
        }
        void cnt_Program_CCD01_SNAP_初始化(ref int cnt)
        {

            this.plC_MindVision_Camera_UI_CCD01.Set_Config();
            this.flag_CCD01_取像完成 = false;
            this.CCD01_AxImageBW8_ORG.SetSurfacePtr(this.plC_MindVision_Camera_UI_CCD01.ImageWidth, this.plC_MindVision_Camera_UI_CCD01.ImageHeight, this.plC_MindVision_Camera_UI_CCD01.ActiveSurfaceHandle);
            this.plC_MindVision_Camera_UI_CCD01.StreamIsSuspend = false;

            cnt++;
        }
        void cnt_Program_CCD01_SNAP_觸發一次(ref int cnt)
        {
            if (!this.plC_MindVision_Camera_UI_CCD01.StreamIsSuspend)
            {
                this.CCD觸發(enum_CCD觸發.CCD01, true);
                cnt++;
            }
        }
        void cnt_Program_CCD01_SNAP_觸發完成(ref int cnt)
        {
            if (this.plC_MindVision_Camera_UI_CCD01.StreamIsSuspend)
            {
                this.CCD觸發(enum_CCD觸發.CCD01, false);
                cnt++;
            }
        }
        void cnt_Program_CCD01_SNAP_等待取像完成(ref int cnt)
        {
            cnt++;

        }
        void cnt_Program_CCD01_SNAP_取像畫面更新(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_CCD01_SNAP_取像完成(ref int cnt)
        {
            cnt++;

        }
        private void plC_MindVision_Camera_UI_CCD011_OnSufaceDrawEvent(long ActiveSurfaceHandle, int ImageWidth, int ImageHeight)
        {

        }
        private void plC_MindVision_Camera_UI_CCD01_OnSufaceDrawEvent(long ActiveSurfaceHandle, int ImageWidth, int ImageHeight)
        {
            this.CCD01_AxImageBW8_ORG.SetSurfacePtr(ImageWidth, ImageHeight, ActiveSurfaceHandle);
            this.CCD01_AxImageCopier.SrcImageHandle = this.CCD01_AxImageBW8_ORG.VegaHandle;
            this.CCD01_AxImageCopier.DstImageHandle = this.CCD01_AxImageBW8.VegaHandle;
            this.CCD01_AxImageCopier.Copy();

            this.flag_CCD01_取像完成 = true;
        }
        #endregion
        #region PLC_PLC_CCD01_Main_全檢測一次
        PLC_Device PLC_CCD01_Main_全檢測一次 = new PLC_Device("S39990");
        PLC_Device PLC_CCD01_Main_全檢測一次_OK = new PLC_Device("S39991");
        Task Task_PLC_CCD01_Main_全檢測一次;
        MyTimer MyTimer_PLC_CCD01_Main_全檢測一次_ = new MyTimer();
        int cnt_Program_PLC_CCD01_Main_全檢測一次 = 65534;
        void sub_Program_PLC_CCD01_Main_全檢測一次()
        {
            if (cnt_Program_PLC_CCD01_Main_全檢測一次 == 65534)
            {
                PLC_CCD01_Main_全檢測一次.SetComment("PLC_PLC_CCD01_Main_全檢測一次");
                PLC_CCD01_Main_全檢測一次_OK.SetComment("PLC_PLC_CCD01_Main_全檢測一次_OK");
                PLC_CCD01_Main_全檢測一次.Bool = false;
                cnt_Program_PLC_CCD01_Main_全檢測一次 = 65535;
            }
            if (cnt_Program_PLC_CCD01_Main_全檢測一次 == 65535) cnt_Program_PLC_CCD01_Main_全檢測一次 = 1;
            if (cnt_Program_PLC_CCD01_Main_全檢測一次 == 1) cnt_Program_PLC_CCD01_Main_全檢測一次_檢查按下(ref cnt_Program_PLC_CCD01_Main_全檢測一次);
            if (cnt_Program_PLC_CCD01_Main_全檢測一次 == 2) cnt_Program_PLC_CCD01_Main_全檢測一次_初始化(ref cnt_Program_PLC_CCD01_Main_全檢測一次);
            if (cnt_Program_PLC_CCD01_Main_全檢測一次 == 3) cnt_Program_PLC_CCD01_Main_全檢測一次_CCD01_01檢測開始(ref cnt_Program_PLC_CCD01_Main_全檢測一次);
            if (cnt_Program_PLC_CCD01_Main_全檢測一次 == 4) cnt_Program_PLC_CCD01_Main_全檢測一次_CCD01_01檢測結束(ref cnt_Program_PLC_CCD01_Main_全檢測一次);
            if (cnt_Program_PLC_CCD01_Main_全檢測一次 == 5) cnt_Program_PLC_CCD01_Main_全檢測一次_CCD01_02檢測開始(ref cnt_Program_PLC_CCD01_Main_全檢測一次);
            if (cnt_Program_PLC_CCD01_Main_全檢測一次 == 6) cnt_Program_PLC_CCD01_Main_全檢測一次_CCD01_02檢測結束(ref cnt_Program_PLC_CCD01_Main_全檢測一次);
            if (cnt_Program_PLC_CCD01_Main_全檢測一次 == 7) cnt_Program_PLC_CCD01_Main_全檢測一次_CCD01_03檢測開始(ref cnt_Program_PLC_CCD01_Main_全檢測一次);
            if (cnt_Program_PLC_CCD01_Main_全檢測一次 == 8) cnt_Program_PLC_CCD01_Main_全檢測一次_CCD01_03檢測結束(ref cnt_Program_PLC_CCD01_Main_全檢測一次);
            if (cnt_Program_PLC_CCD01_Main_全檢測一次 == 9) cnt_Program_PLC_CCD01_Main_全檢測一次_CCD01_04檢測開始(ref cnt_Program_PLC_CCD01_Main_全檢測一次);
            if (cnt_Program_PLC_CCD01_Main_全檢測一次 == 10) cnt_Program_PLC_CCD01_Main_全檢測一次_CCD01_04檢測結束(ref cnt_Program_PLC_CCD01_Main_全檢測一次);
            if (cnt_Program_PLC_CCD01_Main_全檢測一次 == 11) cnt_Program_PLC_CCD01_Main_全檢測一次_繪製畫布(ref cnt_Program_PLC_CCD01_Main_全檢測一次);
            if (cnt_Program_PLC_CCD01_Main_全檢測一次 == 12) cnt_Program_PLC_CCD01_Main_全檢測一次 = 65500;
            if (cnt_Program_PLC_CCD01_Main_全檢測一次 > 1) cnt_Program_PLC_CCD01_Main_全檢測一次_檢查放開(ref cnt_Program_PLC_CCD01_Main_全檢測一次);

            if (cnt_Program_PLC_CCD01_Main_全檢測一次 == 65500)
            {
                PLC_CCD01_Main_全檢測一次.Bool = false;
                PLC_CCD01_Main_全檢測一次_OK.Bool = false;
                cnt_Program_PLC_CCD01_Main_全檢測一次 = 65535;
            }
        }
        void cnt_Program_PLC_CCD01_Main_全檢測一次_檢查按下(ref int cnt)
        {
            if (PLC_CCD01_Main_全檢測一次.Bool) cnt++;
        }
        void cnt_Program_PLC_CCD01_Main_全檢測一次_檢查放開(ref int cnt)
        {
            if (!PLC_CCD01_Main_全檢測一次.Bool) cnt = 65500;
        }
        void cnt_Program_PLC_CCD01_Main_全檢測一次_初始化(ref int cnt)
        {
            MyTimer_PLC_CCD01_Main_全檢測一次_.TickStop();
            MyTimer_PLC_CCD01_Main_全檢測一次_.StartTickTime(100000);
            cnt++;
        }
        void cnt_Program_PLC_CCD01_Main_全檢測一次_CCD01_01檢測開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_01_Main_取像並檢驗.Bool)
            {
                this.PLC_Device_CCD01_01_Main_取像並檢驗.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_PLC_CCD01_Main_全檢測一次_CCD01_01檢測結束(ref int cnt)
        {
            if (!PLC_Device_CCD01_01_Main_取像並檢驗.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_PLC_CCD01_Main_全檢測一次_CCD01_02檢測開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_02_Main_取像並檢驗.Bool)
            {
                this.PLC_Device_CCD01_02_Main_取像並檢驗.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_PLC_CCD01_Main_全檢測一次_CCD01_02檢測結束(ref int cnt)
        {
            if (!PLC_Device_CCD01_02_Main_取像並檢驗.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_PLC_CCD01_Main_全檢測一次_CCD01_03檢測開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_03_Main_取像並檢驗.Bool)
            {
                this.PLC_Device_CCD01_03_Main_取像並檢驗.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_PLC_CCD01_Main_全檢測一次_CCD01_03檢測結束(ref int cnt)
        {
            if (!PLC_Device_CCD01_03_Main_取像並檢驗.Bool)
            {
                cnt++;
            }
        }

        void cnt_Program_PLC_CCD01_Main_全檢測一次_CCD01_04檢測開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_04_Main_取像並檢驗.Bool)
            {
                this.PLC_Device_CCD01_04_Main_取像並檢驗.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_PLC_CCD01_Main_全檢測一次_CCD01_04檢測結束(ref int cnt)
        {
            if (!PLC_Device_CCD01_04_Main_取像並檢驗.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_PLC_CCD01_Main_全檢測一次_繪製畫布(ref int cnt)
        {

            Console.WriteLine($"全檢測,耗時 {MyTimer_PLC_CCD01_Main_全檢測一次_.ToString()}");
            cnt++;
        }
        #endregion
        #region PLC_PLC_CCD01_Main_全Snap一次
        PLC_Device PLC_CCD01_Main_全Snap一次 = new PLC_Device("S39995");
        PLC_Device PLC_CCD01_Main_全Snap一次_OK = new PLC_Device("S39996");
        Task Task_PLC_CCD01_Main_全Snap一次;
        MyTimer MyTimer_PLC_CCD01_Main_全Snap一次_ = new MyTimer();
        int cnt_Program_PLC_CCD01_Main_全Snap一次 = 65534;
        void sub_Program_PLC_CCD01_Main_全Snap一次()
        {
            if (cnt_Program_PLC_CCD01_Main_全Snap一次 == 65534)
            {
                PLC_CCD01_Main_全Snap一次.SetComment("PLC_PLC_CCD01_Main_全Snap一次");
                PLC_CCD01_Main_全Snap一次_OK.SetComment("PLC_PLC_CCD01_Main_全Snap一次_OK");
                PLC_CCD01_Main_全Snap一次.Bool = false;
                cnt_Program_PLC_CCD01_Main_全Snap一次 = 65535;
            }
            if (cnt_Program_PLC_CCD01_Main_全Snap一次 == 65535) cnt_Program_PLC_CCD01_Main_全Snap一次 = 1;
            if (cnt_Program_PLC_CCD01_Main_全Snap一次 == 1) cnt_Program_PLC_CCD01_Main_全Snap一次_檢查按下(ref cnt_Program_PLC_CCD01_Main_全Snap一次);
            if (cnt_Program_PLC_CCD01_Main_全Snap一次 == 2) cnt_Program_PLC_CCD01_Main_全Snap一次_初始化(ref cnt_Program_PLC_CCD01_Main_全Snap一次);
            if (cnt_Program_PLC_CCD01_Main_全Snap一次 == 3) cnt_Program_PLC_CCD01_Main_全Snap一次_CCD01_01檢測開始(ref cnt_Program_PLC_CCD01_Main_全Snap一次);
            if (cnt_Program_PLC_CCD01_Main_全Snap一次 == 4) cnt_Program_PLC_CCD01_Main_全Snap一次_CCD01_01檢測結束(ref cnt_Program_PLC_CCD01_Main_全Snap一次);
            if (cnt_Program_PLC_CCD01_Main_全Snap一次 == 5) cnt_Program_PLC_CCD01_Main_全Snap一次_CCD01_02檢測開始(ref cnt_Program_PLC_CCD01_Main_全Snap一次);
            if (cnt_Program_PLC_CCD01_Main_全Snap一次 == 6) cnt_Program_PLC_CCD01_Main_全Snap一次_CCD01_02檢測結束(ref cnt_Program_PLC_CCD01_Main_全Snap一次);
            if (cnt_Program_PLC_CCD01_Main_全Snap一次 == 7) cnt_Program_PLC_CCD01_Main_全Snap一次_CCD01_03檢測開始(ref cnt_Program_PLC_CCD01_Main_全Snap一次);
            if (cnt_Program_PLC_CCD01_Main_全Snap一次 == 8) cnt_Program_PLC_CCD01_Main_全Snap一次_CCD01_03檢測結束(ref cnt_Program_PLC_CCD01_Main_全Snap一次);
            if (cnt_Program_PLC_CCD01_Main_全Snap一次 == 9) cnt_Program_PLC_CCD01_Main_全Snap一次_CCD01_04檢測開始(ref cnt_Program_PLC_CCD01_Main_全Snap一次);
            if (cnt_Program_PLC_CCD01_Main_全Snap一次 == 10) cnt_Program_PLC_CCD01_Main_全Snap一次_CCD01_04檢測結束(ref cnt_Program_PLC_CCD01_Main_全Snap一次);
            if (cnt_Program_PLC_CCD01_Main_全Snap一次 == 11) cnt_Program_PLC_CCD01_Main_全Snap一次_繪製畫布(ref cnt_Program_PLC_CCD01_Main_全Snap一次);
            if (cnt_Program_PLC_CCD01_Main_全Snap一次 == 12) cnt_Program_PLC_CCD01_Main_全Snap一次 = 65500;
            if (cnt_Program_PLC_CCD01_Main_全Snap一次 > 1) cnt_Program_PLC_CCD01_Main_全Snap一次_檢查放開(ref cnt_Program_PLC_CCD01_Main_全Snap一次);

            if (cnt_Program_PLC_CCD01_Main_全Snap一次 == 65500)
            {
                PLC_CCD01_Main_全Snap一次.Bool = false;
                PLC_CCD01_Main_全Snap一次_OK.Bool = false;
                cnt_Program_PLC_CCD01_Main_全Snap一次 = 65535;
            }
        }
        void cnt_Program_PLC_CCD01_Main_全Snap一次_檢查按下(ref int cnt)
        {
            if (PLC_CCD01_Main_全Snap一次.Bool) cnt++;
        }
        void cnt_Program_PLC_CCD01_Main_全Snap一次_檢查放開(ref int cnt)
        {
            if (!PLC_CCD01_Main_全Snap一次.Bool) cnt = 65500;
        }
        void cnt_Program_PLC_CCD01_Main_全Snap一次_初始化(ref int cnt)
        {
            MyTimer_PLC_CCD01_Main_全Snap一次_.TickStop();
            MyTimer_PLC_CCD01_Main_全Snap一次_.StartTickTime(100000);
            cnt++;
        }
        void cnt_Program_PLC_CCD01_Main_全Snap一次_CCD01_01檢測開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_01_SNAP_按鈕.Bool)
            {
                this.PLC_Device_CCD01_01_SNAP_按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_PLC_CCD01_Main_全Snap一次_CCD01_01檢測結束(ref int cnt)
        {
            if (!PLC_Device_CCD01_01_SNAP_按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_PLC_CCD01_Main_全Snap一次_CCD01_02檢測開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_02_SNAP_按鈕.Bool)
            {
                this.PLC_Device_CCD01_02_SNAP_按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_PLC_CCD01_Main_全Snap一次_CCD01_02檢測結束(ref int cnt)
        {
            if (!PLC_Device_CCD01_02_SNAP_按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_PLC_CCD01_Main_全Snap一次_CCD01_03檢測開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_03_SNAP_按鈕.Bool)
            {
                this.PLC_Device_CCD01_03_SNAP_按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_PLC_CCD01_Main_全Snap一次_CCD01_03檢測結束(ref int cnt)
        {
            if (!PLC_Device_CCD01_03_SNAP_按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_PLC_CCD01_Main_全Snap一次_CCD01_04檢測開始(ref int cnt)
        {
            if (!this.PLC_Device_CCD01_04_SNAP_按鈕.Bool)
            {
                this.PLC_Device_CCD01_04_SNAP_按鈕.Bool = true;
                cnt++;
            }
        }
        void cnt_Program_PLC_CCD01_Main_全Snap一次_CCD01_04檢測結束(ref int cnt)
        {
            if (!PLC_Device_CCD01_04_SNAP_按鈕.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_PLC_CCD01_Main_全Snap一次_繪製畫布(ref int cnt)
        {

            Console.WriteLine($"全Snap,耗時 {MyTimer_PLC_CCD01_Main_全Snap一次_.ToString()}");
            cnt++;
        }
        #endregion
    }
}
