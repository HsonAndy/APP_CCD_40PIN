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
using AxAxOvkBase;
using AxAxOvkImage;
using SQLUI;
using System.IO;
using AxOvkImage;
namespace 文信40PIN_CCD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        MyThread MyThread_Program_CCD01;
        MyThread MyThread_Canvas;
        MyThread MyThread_Program_CCD01_SNAP;
        MyThread MyThread_Program_CCD02;
        MyThread MyThread_Program_CCD02_SNAP;
        MyThread MyThread_Counter;

        private Basic.MySerialPort mySerial_counter = new MySerialPort();
        private MyTimer MyTimer_counter = new MyTimer();

        AxAxOvkBase.AxAxCanvas CCD01_01_PatternCanvas = new AxAxOvkBase.AxAxCanvas();
        AxAxOvkBase.AxAxCanvas CCD01_03_PatternCanvas = new AxAxOvkBase.AxAxCanvas();
        AxAxOvkBase.AxAxCanvas CCD02_02_PatternCanvas = new AxAxOvkBase.AxAxCanvas();
        AxAxOvkBase.AxAxCanvas CCD02_04_PatternCanvas = new AxAxOvkBase.AxAxCanvas();

        float CCD01_01_PatternCanvas_ZoomX = 0;
        float CCD01_01_PatternCanvas_ZoomY = 0;
        float CCD01_03_PatternCanvas_ZoomX = 0;
        float CCD01_03_PatternCanvas_ZoomY = 0;
        float CCD02_02_PatternCanvas_ZoomX = 0;
        float CCD02_02_PatternCanvas_ZoomY = 0;
        float CCD02_04_PatternCanvas_ZoomX = 0;
        float CCD02_04_PatternCanvas_ZoomY = 0;
        #region 建立相機宣告
        PLC_Device PLC_Device_相機序號 = new PLC_Device("F0");
        PLC_Device PLC_Device_相機初始化 = new PLC_Device("S10040");
        PLC_Device PLC_Device_相機已建立 = new PLC_Device("S10041");
        PLC_Device PLC_Device_光源控制初始化 = new PLC_Device("S10020");
        PLC_Device PLC_Device_光源控制已建立 = new PLC_Device("S10021");
        PLC_Device PLC_Device_CCD01相機已連線 = new PLC_Device("S10050");
        PLC_Device PLC_Device_CCD01相機開啟 = new PLC_Device("S10051");
        PLC_Device PLC_Device_CCD02相機已連線 = new PLC_Device("S10060");
        PLC_Device PLC_Device_CCD02相機開啟 = new PLC_Device("S10061");
        #endregion
        #region 建立Main畫布宣告
        PLC_Device PLC_Device_Main_CCD01_01_ZOOM_X = new PLC_Device("F30201");
        PLC_Device PLC_Device_Main_CCD01_01_ZOOM_Y = new PLC_Device("F30202");
        PLC_Device PLC_Device_Main_CCD01_02_ZOOM_X = new PLC_Device("F30211");
        PLC_Device PLC_Device_Main_CCD01_02_ZOOM_Y = new PLC_Device("F30212");
        PLC_Device PLC_Device_Main_CCD01_03_ZOOM_X = new PLC_Device("F30221");
        PLC_Device PLC_Device_Main_CCD01_03_ZOOM_Y = new PLC_Device("F30222");
        PLC_Device PLC_Device_Main_CCD01_04_ZOOM_X = new PLC_Device("F30241");
        PLC_Device PLC_Device_Main_CCD01_04_ZOOM_Y = new PLC_Device("F30242");

        PLC_Device PLC_Device_Main_CCD02_01_ZOOM_X = new PLC_Device("F30301");
        PLC_Device PLC_Device_Main_CCD02_01_ZOOM_Y = new PLC_Device("F30302");
        PLC_Device PLC_Device_Main_CCD02_02_ZOOM_X = new PLC_Device("F30311");
        PLC_Device PLC_Device_Main_CCD02_02_ZOOM_Y = new PLC_Device("F30312");
        PLC_Device PLC_Device_Main_CCD02_03_ZOOM_X = new PLC_Device("F30321");
        PLC_Device PLC_Device_Main_CCD02_03_ZOOM_Y = new PLC_Device("F30322");
        PLC_Device PLC_Device_Main_CCD02_04_ZOOM_X = new PLC_Device("F30331");
        PLC_Device PLC_Device_Main_CCD02_04_ZOOM_Y = new PLC_Device("F30332");

        PLC_Device PLC_Device_Main_CCD01_01_ZOOM更新 = new PLC_Device("S30200");
        PLC_Device PLC_Device_Main_CCD01_02_ZOOM更新 = new PLC_Device("S30210");
        PLC_Device PLC_Device_Main_CCD01_03_ZOOM更新 = new PLC_Device("S30220");
        PLC_Device PLC_Device_Main_CCD01_04_ZOOM更新 = new PLC_Device("S30240");

        PLC_Device PLC_Device_Main_CCD02_01_ZOOM更新 = new PLC_Device("S30300");
        PLC_Device PLC_Device_Main_CCD02_02_ZOOM更新 = new PLC_Device("S30310");
        PLC_Device PLC_Device_Main_CCD02_03_ZOOM更新 = new PLC_Device("S30320");
        PLC_Device PLC_Device_Main_CCD02_04_ZOOM更新 = new PLC_Device("S30330");
        #endregion
        private AxOvkBase.AxImageBW8 CCD01_01_AxImageBW8;
        private void CCD01_Init()
        {
            this.CCD01_01_AxImageBW8 = new AxOvkBase.AxImageBW8();
            for (int i = 0; i < 1; i++)
            {
                this.List_CCD01_01_基準圓量測_AxCircleROIBW8_量測框調整.Add(new AxOvkBase.AxROIBW8());
                this.List_CCD01_01_基準圓量測_AxObject_區塊分析.Add(new AxOvkBlob.AxObject());
            }
            if (this.CCD01_01基準圓AxVisionInspectionFrame_量測框調整 == null) this.CCD01_01基準圓AxVisionInspectionFrame_量測框調整 = new AxOvkPat.AxVisionInspectionFrame();
            if (this.AxMatch_CCD01_01_圓柱相似度測試 == null) this.AxMatch_CCD01_01_圓柱相似度測試 = new AxOvkPat.AxMatch();
            if (this.AxROIBW8_CCD01_01_比對樣板範圍 == null) this.AxROIBW8_CCD01_01_比對樣板範圍 = new AxOvkBase.AxROIBW8();
            if (this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整 == null) this.CCD01_01_基準圓直徑量測_AxCircleMsr_量測框調整 = new AxOvkMsr.AxCircleMsr();
            if (this.CCD01_01基準圓直徑AxVisionInspectionFrame_量測框調整 == null) this.CCD01_01基準圓直徑AxVisionInspectionFrame_量測框調整 = new AxOvkPat.AxVisionInspectionFrame();

            this.CCD01_01_PatternCanvas_ZoomX = (float)((float)this.CCD01_01_PatternCanvas.CanvasWidth / (float)this.CCD01_01_PatternCanvas.Width);
            this.CCD01_01_PatternCanvas_ZoomY = (float)((float)this.CCD01_01_PatternCanvas.CanvasHeight / (float)this.CCD01_01_PatternCanvas.Height);

            #region 基準線
            if (this.CCD01_01_水平基準線量測_AxLineMsr == null) this.CCD01_01_水平基準線量測_AxLineMsr = new AxOvkMsr.AxLineMsr();
            if (this.CCD01_01_水平基準線量測_AxLineRegression == null) this.CCD01_01_水平基準線量測_AxLineRegression = new AxOvkMsr.AxLineRegression();
            if (this.CCD01_01_垂直基準線量測_AxLineMsr == null) this.CCD01_01_垂直基準線量測_AxLineMsr = new AxOvkMsr.AxLineMsr();
            if (this.CCD01_01_垂直基準線量測_AxLineRegression == null) this.CCD01_01_垂直基準線量測_AxLineRegression = new AxOvkMsr.AxLineRegression();
            if (this.CCD01_01_基準線量測_AxIntersectionMsr == null) this.CCD01_01_基準線量測_AxIntersectionMsr = new AxOvkMsr.AxIntersectionMsr();
            #endregion
            #region CCD01_02_PIN量測
            if (this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排 == null) this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排 = new AxOvkMsr.AxPointLineDistanceMsr();
            if (this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排 == null) this.CCD01_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排 = new AxOvkMsr.AxPointLineDistanceMsr();
            if (this.CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整 == null) this.CCD01_02_PIN量測_AxVisionInspectionFrame_量測框調整 = new AxOvkPat.AxVisionInspectionFrame();
            if (this.CCD01_02_PIN量測點_AxVisionInspectionFrame_量測框調整 == null) this.CCD01_02_PIN量測點_AxVisionInspectionFrame_量測框調整 = new AxOvkPat.AxVisionInspectionFrame();
            if (this.CCD01_02_塗黑 == null) this.CCD01_02_塗黑 = new AxOvkImage.AxImageSetValue();
            for (int i = 0; i < 22; i++)
            {
                //this.List_CCD01_02_PIN量測_AxROIBW8_量測框調整.Add(new AxOvkBase.AxROIBW8());
                //this.List_CCD01_02_PIN量測_AxObject_區塊分析.Add(new AxOvkBlob.AxObject());
                this.List_CCD01_02_PIN量測點_AxROIBW8_量測框調整.Add(new AxOvkBase.AxROIBW8());
                this.List_CCD01_02_左側端點_AxROIBW8_量測框調整.Add(new AxOvkBase.AxROIBW8());
                this.List_CCD01_02_左側端點_AxLineMsr_線量測.Add(new AxOvkMsr.AxLineMsr());
                this.List_CCD01_02_塗黑遮罩_AxROIBW8.Add(new AxOvkBase.AxROIBW8());
                //this.List_CCD01_02_PIN量測_AxAngleMsr_量測框調整.Add(new AxOvkMsr.AxAngleMsr());
            }
            #endregion
            #region CCD01_04_PIN量測
            if (this.CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_上排 == null) this.CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_上排 = new AxOvkMsr.AxPointLineDistanceMsr();
            if (this.CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_下排 == null) this.CCD01_04_PIN量測_AxPointLineDistanceMsr_線到點量測_下排 = new AxOvkMsr.AxPointLineDistanceMsr();
            if (this.CCD01_04_PIN量測_AxVisionInspectionFrame_量測框調整 == null) this.CCD01_04_PIN量測_AxVisionInspectionFrame_量測框調整 = new AxOvkPat.AxVisionInspectionFrame();
            if (this.CCD01_04_PIN量測點_AxVisionInspectionFrame_量測框調整 == null) this.CCD01_04_PIN量測點_AxVisionInspectionFrame_量測框調整 = new AxOvkPat.AxVisionInspectionFrame();
            if (this.CCD01_04_塗黑 == null) this.CCD01_04_塗黑 = new AxOvkImage.AxImageSetValue();
            for (int i = 0; i < 20; i++)
            {
                this.List_CCD01_04_PIN量測_AxROIBW8_量測框調整.Add(new AxOvkBase.AxROIBW8());
                this.List_CCD01_04_PIN量測_AxObject_區塊分析.Add(new AxOvkBlob.AxObject());
                this.List_CCD01_04_PIN量測點_AxROIBW8_量測框調整.Add(new AxOvkBase.AxROIBW8());
                this.List_CCD01_04_左側端點_AxROIBW8_量測框調整.Add(new AxOvkBase.AxROIBW8());
                this.List_CCD01_04_左側端點_AxLineMsr_線量測.Add(new AxOvkMsr.AxLineMsr());
                this.List_CCD01_04_塗黑遮罩_AxROIBW8.Add(new AxOvkBase.AxROIBW8());
            }
            #endregion
            #region CCD01_03_PIN量測

            if (this.CCD01_03_水平基準線量測_AxLineMsr == null) this.CCD01_03_水平基準線量測_AxLineMsr = new AxOvkMsr.AxLineMsr();
            if (this.CCD01_03_水平基準線量測_AxLineRegression == null) this.CCD01_03_水平基準線量測_AxLineRegression = new AxOvkMsr.AxLineRegression();
            if (this.CCD01_03_垂直基準線量測_AxLineMsr == null) this.CCD01_03_垂直基準線量測_AxLineMsr = new AxOvkMsr.AxLineMsr();
            if (this.CCD01_03_垂直基準線量測_AxLineRegression == null) this.CCD01_03_垂直基準線量測_AxLineRegression = new AxOvkMsr.AxLineRegression();
            if (this.CCD01_03_基準線量測_AxIntersectionMsr == null) this.CCD01_03_基準線量測_AxIntersectionMsr = new AxOvkMsr.AxIntersectionMsr();

            for (int i = 0; i < 1; i++)
            {
                this.List_CCD01_03_基準圓量測_AxCircleROIBW8_量測框調整.Add(new AxOvkBase.AxROIBW8());
                this.List_CCD01_03_基準圓量測_AxObject_區塊分析.Add(new AxOvkBlob.AxObject());
            }
            if (this.CCD01_03基準圓AxVisionInspectionFrame_量測框調整 == null) this.CCD01_03基準圓AxVisionInspectionFrame_量測框調整 = new AxOvkPat.AxVisionInspectionFrame();
            if (this.AxMatch_CCD01_03_圓柱相似度測試 == null) this.AxMatch_CCD01_03_圓柱相似度測試 = new AxOvkPat.AxMatch();
            if (this.AxROIBW8_CCD01_03_比對樣板範圍 == null) this.AxROIBW8_CCD01_03_比對樣板範圍 = new AxOvkBase.AxROIBW8();
            if (this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整 == null) this.CCD01_03_基準圓直徑量測_AxGapMsr_量測框調整 = new AxOvkMsr.AxGapMsr();
            if (this.CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整 == null) this.CCD01_03基準圓直徑AxVisionInspectionFrame_量測框調整 = new AxOvkPat.AxVisionInspectionFrame();
            this.CCD01_03_PatternCanvas_ZoomX = (float)((float)this.CCD01_03_PatternCanvas.CanvasWidth / (float)this.CCD01_03_PatternCanvas.Width);
            this.CCD01_03_PatternCanvas_ZoomY = (float)((float)this.CCD01_03_PatternCanvas.CanvasHeight / (float)this.CCD01_03_PatternCanvas.Height);
            #endregion

            if (CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整 == null) CCD01_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整 = new AxOvkPat.AxVisionInspectionFrame();
            if (CCD01_04_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整 == null) CCD01_04_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整 = new AxOvkPat.AxVisionInspectionFrame();

        }
        private void CCD02_Init()
        { 
            #region 基準線
            if (this.CCD02_01_水平基準線量測_AxLineMsr == null) this.CCD02_01_水平基準線量測_AxLineMsr = new AxOvkMsr.AxLineMsr();
            if (this.CCD02_01_水平基準線量測_AxLineRegression == null) this.CCD02_01_水平基準線量測_AxLineRegression = new AxOvkMsr.AxLineRegression();
            if (this.CCD02_01_垂直基準線量測_AxLineMsr == null) this.CCD02_01_垂直基準線量測_AxLineMsr = new AxOvkMsr.AxLineMsr();
            if (this.CCD02_01_垂直基準線量測_AxLineRegression == null) this.CCD02_01_垂直基準線量測_AxLineRegression = new AxOvkMsr.AxLineRegression();
            if (this.CCD02_01_基準線量測_AxIntersectionMsr == null) this.CCD02_01_基準線量測_AxIntersectionMsr = new AxOvkMsr.AxIntersectionMsr();

            #endregion
            #region CCD02_01_PIN量測
            if (this.CCD02_01_PIN量測_AxPointLineDistanceMsr_線到點量測 == null) this.CCD02_01_PIN量測_AxPointLineDistanceMsr_線到點量測 = new AxOvkMsr.AxPointLineDistanceMsr();
            if (this.CCD02_01_PIN量測_AxVisionInspectionFrame_量測框調整 == null) this.CCD02_01_PIN量測_AxVisionInspectionFrame_量測框調整 = new AxOvkPat.AxVisionInspectionFrame();
            for (int i = 0; i < 10; i++)
            {
                this.List_CCD02_01_PIN量測_AxROIBW8_量測框調整.Add(new AxOvkBase.AxROIBW8());
                this.List_CCD02_01_PIN量測_AxObject_區塊分析.Add(new AxOvkBlob.AxObject());
            }

            #endregion
            #region CCD02_02_PIN量測
            if (this.CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排 == null) this.CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_上排 = new AxOvkMsr.AxPointLineDistanceMsr();
            if (this.CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排 == null) this.CCD02_02_PIN量測_AxPointLineDistanceMsr_線到點量測_下排 = new AxOvkMsr.AxPointLineDistanceMsr();
            if (this.CCD02_02_PIN量測_AxVisionInspectionFrame_量測框調整 == null) this.CCD02_02_PIN量測_AxVisionInspectionFrame_量測框調整 = new AxOvkPat.AxVisionInspectionFrame();
            if (CCD02_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整 == null) CCD02_02_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整 = new AxOvkPat.AxVisionInspectionFrame();
            if (this.AxMatch_CCD02_02_PIN相似度測試 == null) this.AxMatch_CCD02_02_PIN相似度測試 = new AxOvkPat.AxMatch();
            if (this.AxROIBW8_CCD02_02_比對樣板範圍 == null) this.AxROIBW8_CCD02_02_比對樣板範圍 = new AxOvkBase.AxROIBW8();
            for (int i = 0; i < 22; i++)
            {
                this.List_CCD02_02_PIN量測_AxROIBW8_量測框調整.Add(new AxOvkBase.AxROIBW8());
                this.List_CCD02_02_PIN量測_AxObject_區塊分析.Add(new AxOvkBlob.AxObject());
            }
            this.CCD02_02_PatternCanvas_ZoomX = (float)((float)this.CCD02_02_PatternCanvas.CanvasWidth / (float)this.CCD02_02_PatternCanvas.Width);
            this.CCD02_02_PatternCanvas_ZoomY = (float)((float)this.CCD02_02_PatternCanvas.CanvasHeight / (float)this.CCD02_02_PatternCanvas.Height);

            #endregion
            #region CCD02_03_PIN量測

            if (this.CCD02_03_水平基準線量測_AxLineMsr == null) this.CCD02_03_水平基準線量測_AxLineMsr = new AxOvkMsr.AxLineMsr();
            if (this.CCD02_03_水平基準線量測_AxLineRegression == null) this.CCD02_03_水平基準線量測_AxLineRegression = new AxOvkMsr.AxLineRegression();
            if (this.CCD02_03_垂直基準線量測_AxLineMsr == null) this.CCD02_03_垂直基準線量測_AxLineMsr = new AxOvkMsr.AxLineMsr();
            if (this.CCD02_03_垂直基準線量測_AxLineRegression == null) this.CCD02_03_垂直基準線量測_AxLineRegression = new AxOvkMsr.AxLineRegression();
            if (this.CCD02_03_基準線量測_AxIntersectionMsr == null) this.CCD02_03_基準線量測_AxIntersectionMsr = new AxOvkMsr.AxIntersectionMsr();

            if (this.CCD02_03_PIN量測_AxPointLineDistanceMsr_線到點量測 == null) this.CCD02_03_PIN量測_AxPointLineDistanceMsr_線到點量測 = new AxOvkMsr.AxPointLineDistanceMsr();
            if (this.CCD02_03_PIN量測_AxVisionInspectionFrame_量測框調整 == null) this.CCD02_03_PIN量測_AxVisionInspectionFrame_量測框調整 = new AxOvkPat.AxVisionInspectionFrame();
            for (int i = 0; i < 9; i++)
            {
                this.List_CCD02_03_PIN量測_AxROIBW8_量測框調整.Add(new AxOvkBase.AxROIBW8());
                this.List_CCD02_03_PIN量測_AxObject_區塊分析.Add(new AxOvkBlob.AxObject());
            }

            #endregion
            #region CCD02_04_PIN量測
            if (this.CCD02_04_PIN量測_AxPointLineDistanceMsr_線到點量測_上排 == null) this.CCD02_04_PIN量測_AxPointLineDistanceMsr_線到點量測_上排 = new AxOvkMsr.AxPointLineDistanceMsr();
            if (this.CCD02_04_PIN量測_AxPointLineDistanceMsr_線到點量測_下排 == null) this.CCD02_04_PIN量測_AxPointLineDistanceMsr_線到點量測_下排 = new AxOvkMsr.AxPointLineDistanceMsr();
            if (this.CCD02_04_PIN量測_AxVisionInspectionFrame_量測框調整 == null) this.CCD02_04_PIN量測_AxVisionInspectionFrame_量測框調整 = new AxOvkPat.AxVisionInspectionFrame();
            if (CCD02_04_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整 == null) CCD02_04_PIN正位度量測參數_AxVisionInspectionFrame_量測框調整 = new AxOvkPat.AxVisionInspectionFrame();
            for (int i = 0; i < 20; i++)
            {
                this.List_CCD02_04_PIN量測_AxROIBW8_量測框調整.Add(new AxOvkBase.AxROIBW8());
                this.List_CCD02_04_PIN量測_AxObject_區塊分析.Add(new AxOvkBlob.AxObject());
            }
            this.CCD02_04_PatternCanvas_ZoomX = (float)((float)this.CCD02_04_PatternCanvas.CanvasWidth / (float)this.CCD02_04_PatternCanvas.Width);
            this.CCD02_04_PatternCanvas_ZoomY = (float)((float)this.CCD02_04_PatternCanvas.CanvasHeight / (float)this.CCD02_04_PatternCanvas.Height);
            #endregion

        }
        enum enum_CCD觸發
        {
            CCD01 = 22,
            CCD02 = 23,
        }
        private void CCD觸發(enum_CCD觸發 enum_CCD觸發, bool state)
        {
            this.ioC12801.SetOutput(0, (int)enum_CCD觸發, state);
        }
        enum enum_光源
        {
            CCD01_紅正照,
            CCD02_紅正照,
            CCD01_白正照,
        }
        private void 光源控制(enum_光源 enum_光源, bool state)
        {
            if (enum_光源 == enum_光源.CCD01_紅正照)
            {
                this.dmC1000B1.SetOutput(0, 13, !state);
            }
            else if (enum_光源 == enum_光源.CCD02_紅正照)
            {
                this.dmC1000B1.SetOutput(0, 14, !state);
            }
            else if (enum_光源 == enum_光源.CCD01_白正照)
            {
                this.dmC1000B1.SetOutput(0, 15, !state);
            }

        }
        private void 光源控制(enum_光源 enum_光源, byte value)
        {
            if (enum_光源 == enum_光源.CCD01_紅正照)
            {
                this.lD_NP24DV_4T1.Set_Chanel_LightValue(LightControlUI.LD_NP24DV_4T.enum_Chanle.CH01, value);
            }
            else if (enum_光源 == enum_光源.CCD02_紅正照)
            {
                this.lD_NP24DV_4T1.Set_Chanel_LightValue(LightControlUI.LD_NP24DV_4T.enum_Chanle.CH02, value);
            }
            else if (enum_光源 == enum_光源.CCD01_白正照)
            {
                this.lD_NP24DV_4T1.Set_Chanel_LightValue(LightControlUI.LD_NP24DV_4T.enum_Chanle.CH03, value);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            #region 延伸螢幕
            System.Windows.Forms.Screen firstScreen = System.Windows.Forms.Screen.AllScreens.FirstOrDefault();
            System.Windows.Forms.Screen secondScreen = System.Windows.Forms.Screen.AllScreens.FirstOrDefault(s => !s.Primary);
            
            if (secondScreen != null)
            {
                // 計算新的視窗位置和大小
                Rectangle newBounds = new Rectangle(firstScreen.Bounds.Location,
                                                    new Size(firstScreen.Bounds.Width + secondScreen.Bounds.Width,
                                                             Math.Max(firstScreen.Bounds.Height, secondScreen.Bounds.Height)));
                // 設定視窗的位置和大小
                this.SetBounds(newBounds.Left, newBounds.Top, newBounds.Width, newBounds.Height);
            }
            #endregion
            MyMessageBox.form = this.FindForm();
            plC_UI_Init1.Run(this.FindForm(), lowerMachine_Panel1);

            ((System.ComponentModel.ISupportInitialize)(this.CCD01_01_PatternCanvas)).BeginInit();
            panel_CCD01_01_學習樣板圖片.Controls.Add(this.CCD01_01_PatternCanvas);
            ((System.ComponentModel.ISupportInitialize)(this.CCD01_01_PatternCanvas)).EndInit();
            this.CCD01_01_PatternCanvas.Dock = DockStyle.Fill;
            this.CCD01_01_PatternCanvas.Location = new System.Drawing.Point(0, 0);
            this.CCD01_01_PatternCanvas.CanvasWidth = this.panel_CCD01_01_學習樣板圖片.Width;
            this.CCD01_01_PatternCanvas.CanvasHeight = this.panel_CCD01_01_學習樣板圖片.Height;
            this.CCD01_01_PatternCanvas.RefreshCanvas();

            ((System.ComponentModel.ISupportInitialize)(this.CCD01_03_PatternCanvas)).BeginInit();
            panel_CCD01_03_學習樣板圖片.Controls.Add(this.CCD01_03_PatternCanvas);
            ((System.ComponentModel.ISupportInitialize)(this.CCD01_03_PatternCanvas)).EndInit();
            this.CCD01_03_PatternCanvas.Dock = DockStyle.Fill;
            this.CCD01_03_PatternCanvas.Location = new System.Drawing.Point(0, 0);
            this.CCD01_03_PatternCanvas.CanvasWidth = this.panel_CCD01_03_學習樣板圖片.Width;
            this.CCD01_03_PatternCanvas.CanvasHeight = this.panel_CCD01_03_學習樣板圖片.Height;
            this.CCD01_03_PatternCanvas.RefreshCanvas();


            ((System.ComponentModel.ISupportInitialize)(this.CCD02_02_PatternCanvas)).BeginInit();
            panel_CCD02_02_學習樣板圖片.Controls.Add(this.CCD02_02_PatternCanvas);
            ((System.ComponentModel.ISupportInitialize)(this.CCD02_02_PatternCanvas)).EndInit();
            this.CCD02_02_PatternCanvas.Dock = DockStyle.Fill;
            this.CCD02_02_PatternCanvas.Location = new System.Drawing.Point(0, 0);
            this.CCD02_02_PatternCanvas.CanvasWidth = this.panel_CCD02_02_學習樣板圖片.Width;
            this.CCD02_02_PatternCanvas.CanvasHeight = this.panel_CCD02_02_學習樣板圖片.Height;
            this.CCD02_02_PatternCanvas.RefreshCanvas();

            ((System.ComponentModel.ISupportInitialize)(this.CCD02_04_PatternCanvas)).BeginInit();
            panel_CCD02_04_學習樣板圖片.Controls.Add(this.CCD02_04_PatternCanvas);
            ((System.ComponentModel.ISupportInitialize)(this.CCD02_04_PatternCanvas)).EndInit();
            this.CCD02_04_PatternCanvas.Dock = DockStyle.Fill;
            this.CCD02_04_PatternCanvas.Location = new System.Drawing.Point(0, 0);
            this.CCD02_04_PatternCanvas.CanvasWidth = this.panel_CCD02_04_學習樣板圖片.Width;
            this.CCD02_04_PatternCanvas.CanvasHeight = this.panel_CCD02_04_學習樣板圖片.Height;
            this.CCD02_04_PatternCanvas.RefreshCanvas();


        }
        private double FunctionMsr_Y(double conf0, double conf1, double X)
        {
            double Y;

            // Y=conf0 * X + conf1;

            Y = ((X * conf1) + conf0);

            return Y;
        }
        private double Conf0Msr(double conf1, double X, double Y)
        {
            double conf0;

            conf0 = Y - conf1 * X;

            return conf0;
        }

        #region PLC_Method
        PLC_Device PLC_Device_Method = new PLC_Device("");
        PLC_Device PLC_Device_Method_OK = new PLC_Device("");
        Task Task_Method;
        MyTimer MyTimer_Method_結束延遲 = new MyTimer();
        int cnt_Program_Method = 65534;
        void sub_Program_Method()
        {
            if (cnt_Program_Method == 65534)
            {
                this.MyTimer_Method_結束延遲.StartTickTime(10000);
                PLC_Device_Method.SetComment("PLC_Method");
                PLC_Device_Method_OK.SetComment("PLC_Method_OK");
                PLC_Device_Method.Bool = false;
                cnt_Program_Method = 65535;
            }
            if (cnt_Program_Method == 65535) cnt_Program_Method = 1;
            if (cnt_Program_Method == 1) cnt_Program_Method_檢查按下(ref cnt_Program_Method);
            if (cnt_Program_Method == 2) cnt_Program_Method_初始化(ref cnt_Program_Method);
            if (cnt_Program_Method == 3) cnt_Program_Method = 65500;
            if (cnt_Program_Method > 1) cnt_Program_Method_檢查放開(ref cnt_Program_Method);

            if (cnt_Program_Method == 65500)
            {
                this.MyTimer_Method_結束延遲.TickStop();
                this.MyTimer_Method_結束延遲.StartTickTime(10000);
                PLC_Device_Method.Bool = false;
                PLC_Device_Method_OK.Bool = false;
                cnt_Program_Method = 65535;
            }
        }
        void cnt_Program_Method_檢查按下(ref int cnt)
        {
            if (PLC_Device_Method.Bool) cnt++;
        }
        void cnt_Program_Method_檢查放開(ref int cnt)
        {
            if (!PLC_Device_Method.Bool) cnt = 65500;
        }
        void cnt_Program_Method_初始化(ref int cnt)
        {
            if (this.MyTimer_Method_結束延遲.IsTimeOut())
            {
                if (Task_Method == null)
                {
                    Task_Method = new Task(new Action(delegate { }));
                }
                if (Task_Method.Status == TaskStatus.RanToCompletion)
                {
                    Task_Method = new Task(new Action(delegate { }));
                }
                if (Task_Method.Status == TaskStatus.Created)
                {
                    Task_Method.Start();
                }
                cnt++;
            }
        }








        #endregion
        #region 視窗關閉輸出程式
        List<PLC_Device> PLC_輸出 = new List<PLC_Device>();
        List<PLC_Device> PLC_按鈕 = new List<PLC_Device>();
        List<PLC_Device> PLC_按鈕按下 = new List<PLC_Device>();
        #region Y0~Y77
        PLC_Device PLC_輸出_Y00 = new PLC_Device("Y00");
        PLC_Device PLC_輸出_Y01 = new PLC_Device("Y01");
        PLC_Device PLC_輸出_Y02 = new PLC_Device("Y02");
        PLC_Device PLC_輸出_Y03 = new PLC_Device("Y03");
        PLC_Device PLC_輸出_Y04 = new PLC_Device("Y04");
        PLC_Device PLC_輸出_Y05 = new PLC_Device("Y05");
        PLC_Device PLC_輸出_Y06 = new PLC_Device("Y06");
        PLC_Device PLC_輸出_Y07 = new PLC_Device("Y07");

        PLC_Device PLC_輸出_Y10 = new PLC_Device("Y10");
        PLC_Device PLC_輸出_Y11 = new PLC_Device("Y11");
        PLC_Device PLC_輸出_Y12 = new PLC_Device("Y12");
        PLC_Device PLC_輸出_Y13 = new PLC_Device("Y13");
        PLC_Device PLC_輸出_Y14 = new PLC_Device("Y14");
        PLC_Device PLC_輸出_Y15 = new PLC_Device("Y15");
        PLC_Device PLC_輸出_Y16 = new PLC_Device("Y16");
        PLC_Device PLC_輸出_Y17 = new PLC_Device("Y17");

        PLC_Device PLC_輸出_Y20 = new PLC_Device("Y20");
        PLC_Device PLC_輸出_Y21 = new PLC_Device("Y21");
        PLC_Device PLC_輸出_Y22 = new PLC_Device("Y22");
        PLC_Device PLC_輸出_Y23 = new PLC_Device("Y23");
        PLC_Device PLC_輸出_Y24 = new PLC_Device("Y24");
        PLC_Device PLC_輸出_Y25 = new PLC_Device("Y25");
        PLC_Device PLC_輸出_Y26 = new PLC_Device("Y26");
        PLC_Device PLC_輸出_Y27 = new PLC_Device("Y27");

        PLC_Device PLC_輸出_Y30 = new PLC_Device("Y30");
        PLC_Device PLC_輸出_Y31 = new PLC_Device("Y31");
        PLC_Device PLC_輸出_Y32 = new PLC_Device("Y32");
        PLC_Device PLC_輸出_Y33 = new PLC_Device("Y33");
        PLC_Device PLC_輸出_Y34 = new PLC_Device("Y34");
        PLC_Device PLC_輸出_Y35 = new PLC_Device("Y35");
        PLC_Device PLC_輸出_Y36 = new PLC_Device("Y36");
        PLC_Device PLC_輸出_Y37 = new PLC_Device("Y37");

        PLC_Device PLC_輸出_Y40 = new PLC_Device("Y40");
        PLC_Device PLC_輸出_Y41 = new PLC_Device("Y41");
        PLC_Device PLC_輸出_Y42 = new PLC_Device("Y42");
        PLC_Device PLC_輸出_Y43 = new PLC_Device("Y43");
        PLC_Device PLC_輸出_Y44 = new PLC_Device("Y44");
        PLC_Device PLC_輸出_Y45 = new PLC_Device("Y45");
        PLC_Device PLC_輸出_Y46 = new PLC_Device("Y46");
        PLC_Device PLC_輸出_Y47 = new PLC_Device("Y47");

        PLC_Device PLC_輸出_Y50 = new PLC_Device("Y50");
        PLC_Device PLC_輸出_Y51 = new PLC_Device("Y51");
        PLC_Device PLC_輸出_Y52 = new PLC_Device("Y52");
        PLC_Device PLC_輸出_Y53 = new PLC_Device("Y53");
        PLC_Device PLC_輸出_Y54 = new PLC_Device("Y54");
        PLC_Device PLC_輸出_Y55 = new PLC_Device("Y55");
        PLC_Device PLC_輸出_Y56 = new PLC_Device("Y56");
        PLC_Device PLC_輸出_Y57 = new PLC_Device("Y57");

        PLC_Device PLC_輸出_Y60 = new PLC_Device("Y60");
        PLC_Device PLC_輸出_Y61 = new PLC_Device("Y61");
        PLC_Device PLC_輸出_Y62 = new PLC_Device("Y62");
        PLC_Device PLC_輸出_Y63 = new PLC_Device("Y63");
        PLC_Device PLC_輸出_Y64 = new PLC_Device("Y64");
        PLC_Device PLC_輸出_Y65 = new PLC_Device("Y65");
        PLC_Device PLC_輸出_Y66 = new PLC_Device("Y66");
        PLC_Device PLC_輸出_Y67 = new PLC_Device("Y67");

        PLC_Device PLC_輸出_Y70 = new PLC_Device("Y70");
        PLC_Device PLC_輸出_Y71 = new PLC_Device("Y71");
        PLC_Device PLC_輸出_Y72 = new PLC_Device("Y72");
        PLC_Device PLC_輸出_Y73 = new PLC_Device("Y73");
        PLC_Device PLC_輸出_Y74 = new PLC_Device("Y74");
        PLC_Device PLC_輸出_Y75 = new PLC_Device("Y75");
        PLC_Device PLC_輸出_Y76 = new PLC_Device("Y76");
        PLC_Device PLC_輸出_Y77 = new PLC_Device("Y77");
        #endregion
        #region S0~S77
        PLC_Device PLC_按鈕_S00 = new PLC_Device("S00");
        PLC_Device PLC_按鈕_S01 = new PLC_Device("S01");
        PLC_Device PLC_按鈕_S02 = new PLC_Device("S02");
        PLC_Device PLC_按鈕_S03 = new PLC_Device("S03");
        PLC_Device PLC_按鈕_S04 = new PLC_Device("S04");
        PLC_Device PLC_按鈕_S05 = new PLC_Device("S05");
        PLC_Device PLC_按鈕_S06 = new PLC_Device("S06");
        PLC_Device PLC_按鈕_S07 = new PLC_Device("S07");

        PLC_Device PLC_按鈕_S10 = new PLC_Device("S10");
        PLC_Device PLC_按鈕_S11 = new PLC_Device("S11");
        PLC_Device PLC_按鈕_S12 = new PLC_Device("S12");
        PLC_Device PLC_按鈕_S13 = new PLC_Device("S13");
        PLC_Device PLC_按鈕_S14 = new PLC_Device("S14");
        PLC_Device PLC_按鈕_S15 = new PLC_Device("S15");
        PLC_Device PLC_按鈕_S16 = new PLC_Device("S16");
        PLC_Device PLC_按鈕_S17 = new PLC_Device("S17");

        PLC_Device PLC_按鈕_S20 = new PLC_Device("S20");
        PLC_Device PLC_按鈕_S21 = new PLC_Device("S21");
        PLC_Device PLC_按鈕_S22 = new PLC_Device("S22");
        PLC_Device PLC_按鈕_S23 = new PLC_Device("S23");
        PLC_Device PLC_按鈕_S24 = new PLC_Device("S24");
        PLC_Device PLC_按鈕_S25 = new PLC_Device("S25");
        PLC_Device PLC_按鈕_S26 = new PLC_Device("S26");
        PLC_Device PLC_按鈕_S27 = new PLC_Device("S27");

        PLC_Device PLC_按鈕_S30 = new PLC_Device("S30");
        PLC_Device PLC_按鈕_S31 = new PLC_Device("S31");
        PLC_Device PLC_按鈕_S32 = new PLC_Device("S32");
        PLC_Device PLC_按鈕_S33 = new PLC_Device("S33");
        PLC_Device PLC_按鈕_S34 = new PLC_Device("S34");
        PLC_Device PLC_按鈕_S35 = new PLC_Device("S35");
        PLC_Device PLC_按鈕_S36 = new PLC_Device("S36");
        PLC_Device PLC_按鈕_S37 = new PLC_Device("S37");

        PLC_Device PLC_按鈕_S40 = new PLC_Device("S40");
        PLC_Device PLC_按鈕_S41 = new PLC_Device("S41");
        PLC_Device PLC_按鈕_S42 = new PLC_Device("S42");
        PLC_Device PLC_按鈕_S43 = new PLC_Device("S43");
        PLC_Device PLC_按鈕_S44 = new PLC_Device("S44");
        PLC_Device PLC_按鈕_S45 = new PLC_Device("S45");
        PLC_Device PLC_按鈕_S46 = new PLC_Device("S46");
        PLC_Device PLC_按鈕_S47 = new PLC_Device("S47");

        PLC_Device PLC_按鈕_S50 = new PLC_Device("S50");
        PLC_Device PLC_按鈕_S51 = new PLC_Device("S51");
        PLC_Device PLC_按鈕_S52 = new PLC_Device("S52");
        PLC_Device PLC_按鈕_S53 = new PLC_Device("S53");
        PLC_Device PLC_按鈕_S54 = new PLC_Device("S54");
        PLC_Device PLC_按鈕_S55 = new PLC_Device("S55");
        PLC_Device PLC_按鈕_S56 = new PLC_Device("S56");
        PLC_Device PLC_按鈕_S57 = new PLC_Device("S57");

        PLC_Device PLC_按鈕_S60 = new PLC_Device("S60");
        PLC_Device PLC_按鈕_S61 = new PLC_Device("S61");
        PLC_Device PLC_按鈕_S62 = new PLC_Device("S62");
        PLC_Device PLC_按鈕_S63 = new PLC_Device("S63");
        PLC_Device PLC_按鈕_S64 = new PLC_Device("S64");
        PLC_Device PLC_按鈕_S65 = new PLC_Device("S65");
        PLC_Device PLC_按鈕_S66 = new PLC_Device("S66");
        PLC_Device PLC_按鈕_S67 = new PLC_Device("S67");

        PLC_Device PLC_按鈕_S70 = new PLC_Device("S70");
        PLC_Device PLC_按鈕_S71 = new PLC_Device("S71");
        PLC_Device PLC_按鈕_S72 = new PLC_Device("S72");
        PLC_Device PLC_按鈕_S73 = new PLC_Device("S73");
        PLC_Device PLC_按鈕_S74 = new PLC_Device("S74");
        PLC_Device PLC_按鈕_S75 = new PLC_Device("S75");
        PLC_Device PLC_按鈕_S76 = new PLC_Device("S76");
        PLC_Device PLC_按鈕_S77 = new PLC_Device("S77");
        #endregion
        #region S1000~S1315
        PLC_Device PLC_按鈕按下_S1000 = new PLC_Device("S1000");
        PLC_Device PLC_按鈕按下_S1005 = new PLC_Device("S1005");
        PLC_Device PLC_按鈕按下_S1010 = new PLC_Device("S1010");
        PLC_Device PLC_按鈕按下_S1015 = new PLC_Device("S1015");
        PLC_Device PLC_按鈕按下_S1020 = new PLC_Device("S1020");
        PLC_Device PLC_按鈕按下_S1025 = new PLC_Device("S1025");
        PLC_Device PLC_按鈕按下_S1030 = new PLC_Device("S1030");
        PLC_Device PLC_按鈕按下_S1035 = new PLC_Device("S1035");

        PLC_Device PLC_按鈕按下_S1040 = new PLC_Device("S1040");
        PLC_Device PLC_按鈕按下_S1045 = new PLC_Device("S1045");
        PLC_Device PLC_按鈕按下_S1050 = new PLC_Device("S1050");
        PLC_Device PLC_按鈕按下_S1055 = new PLC_Device("S1055");
        PLC_Device PLC_按鈕按下_S1060 = new PLC_Device("S1060");
        PLC_Device PLC_按鈕按下_S1065 = new PLC_Device("S1065");
        PLC_Device PLC_按鈕按下_S1070 = new PLC_Device("S1070");
        PLC_Device PLC_按鈕按下_S1075 = new PLC_Device("S1075");

        PLC_Device PLC_按鈕按下_S1080 = new PLC_Device("S1080");
        PLC_Device PLC_按鈕按下_S1085 = new PLC_Device("S1085");
        PLC_Device PLC_按鈕按下_S1090 = new PLC_Device("S1090");
        PLC_Device PLC_按鈕按下_S1095 = new PLC_Device("S1095");
        PLC_Device PLC_按鈕按下_S1100 = new PLC_Device("S1100");
        PLC_Device PLC_按鈕按下_S1105 = new PLC_Device("S1105");
        PLC_Device PLC_按鈕按下_S1110 = new PLC_Device("S1110");
        PLC_Device PLC_按鈕按下_S1115 = new PLC_Device("S1115");

        PLC_Device PLC_按鈕按下_S1120 = new PLC_Device("S1120");
        PLC_Device PLC_按鈕按下_S1125 = new PLC_Device("S1125");
        PLC_Device PLC_按鈕按下_S1130 = new PLC_Device("S1130");
        PLC_Device PLC_按鈕按下_S1135 = new PLC_Device("S1135");
        PLC_Device PLC_按鈕按下_S1140 = new PLC_Device("S1140");
        PLC_Device PLC_按鈕按下_S1145 = new PLC_Device("S1145");
        PLC_Device PLC_按鈕按下_S1150 = new PLC_Device("S1150");
        PLC_Device PLC_按鈕按下_S1155 = new PLC_Device("S1155");

        PLC_Device PLC_按鈕按下_S1160 = new PLC_Device("S1160");
        PLC_Device PLC_按鈕按下_S1165 = new PLC_Device("S1165");
        PLC_Device PLC_按鈕按下_S1170 = new PLC_Device("S1170");
        PLC_Device PLC_按鈕按下_S1175 = new PLC_Device("S1175");
        PLC_Device PLC_按鈕按下_S1180 = new PLC_Device("S1180");
        PLC_Device PLC_按鈕按下_S1185 = new PLC_Device("S1185");
        PLC_Device PLC_按鈕按下_S1190 = new PLC_Device("S1190");
        PLC_Device PLC_按鈕按下_S1195 = new PLC_Device("S1195");

        PLC_Device PLC_按鈕按下_S1200 = new PLC_Device("S1200");
        PLC_Device PLC_按鈕按下_S1205 = new PLC_Device("S1205");
        PLC_Device PLC_按鈕按下_S1210 = new PLC_Device("S1210");
        PLC_Device PLC_按鈕按下_S1215 = new PLC_Device("S1215");
        PLC_Device PLC_按鈕按下_S1220 = new PLC_Device("S1220");
        PLC_Device PLC_按鈕按下_S1225 = new PLC_Device("S1225");
        PLC_Device PLC_按鈕按下_S1230 = new PLC_Device("S1230");
        PLC_Device PLC_按鈕按下_S1235 = new PLC_Device("S1235");

        PLC_Device PLC_按鈕按下_S1240 = new PLC_Device("S1240");
        PLC_Device PLC_按鈕按下_S1245 = new PLC_Device("S1245");
        PLC_Device PLC_按鈕按下_S1250 = new PLC_Device("S1250");
        PLC_Device PLC_按鈕按下_S1255 = new PLC_Device("S1255");
        PLC_Device PLC_按鈕按下_S1260 = new PLC_Device("S1260");
        PLC_Device PLC_按鈕按下_S1265 = new PLC_Device("S1265");
        PLC_Device PLC_按鈕按下_S1270 = new PLC_Device("S1270");
        PLC_Device PLC_按鈕按下_S1275 = new PLC_Device("S1275");

        PLC_Device PLC_按鈕按下_S1280 = new PLC_Device("S1280");
        PLC_Device PLC_按鈕按下_S1285 = new PLC_Device("S1285");
        PLC_Device PLC_按鈕按下_S1290 = new PLC_Device("S1290");
        PLC_Device PLC_按鈕按下_S1295 = new PLC_Device("S1295");
        PLC_Device PLC_按鈕按下_S1300 = new PLC_Device("S1300");
        PLC_Device PLC_按鈕按下_S1305 = new PLC_Device("S1305");
        PLC_Device PLC_按鈕按下_S1310 = new PLC_Device("S1310");
        PLC_Device PLC_按鈕按下_S1315 = new PLC_Device("S1315");
        #endregion
        bool flag_OutputClose = false;
        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            #region Y0~Y77
            PLC_輸出.Add(PLC_輸出_Y00);
            PLC_輸出.Add(PLC_輸出_Y01);
            PLC_輸出.Add(PLC_輸出_Y02);
            PLC_輸出.Add(PLC_輸出_Y03);
            PLC_輸出.Add(PLC_輸出_Y04);
            PLC_輸出.Add(PLC_輸出_Y05);
            PLC_輸出.Add(PLC_輸出_Y06);
            PLC_輸出.Add(PLC_輸出_Y07);

            PLC_輸出.Add(PLC_輸出_Y10);
            PLC_輸出.Add(PLC_輸出_Y11);
            PLC_輸出.Add(PLC_輸出_Y12);
            PLC_輸出.Add(PLC_輸出_Y13);
            PLC_輸出.Add(PLC_輸出_Y14);
            PLC_輸出.Add(PLC_輸出_Y15);
            PLC_輸出.Add(PLC_輸出_Y16);
            PLC_輸出.Add(PLC_輸出_Y17);

            PLC_輸出.Add(PLC_輸出_Y20);
            PLC_輸出.Add(PLC_輸出_Y21);
            PLC_輸出.Add(PLC_輸出_Y22);
            PLC_輸出.Add(PLC_輸出_Y23);
            PLC_輸出.Add(PLC_輸出_Y24);
            PLC_輸出.Add(PLC_輸出_Y25);
            PLC_輸出.Add(PLC_輸出_Y26);
            PLC_輸出.Add(PLC_輸出_Y27);

            PLC_輸出.Add(PLC_輸出_Y30);
            PLC_輸出.Add(PLC_輸出_Y31);
            PLC_輸出.Add(PLC_輸出_Y32);
            PLC_輸出.Add(PLC_輸出_Y33);
            PLC_輸出.Add(PLC_輸出_Y34);
            PLC_輸出.Add(PLC_輸出_Y35);
            PLC_輸出.Add(PLC_輸出_Y36);
            PLC_輸出.Add(PLC_輸出_Y37);

            PLC_輸出.Add(PLC_輸出_Y40);
            PLC_輸出.Add(PLC_輸出_Y41);
            PLC_輸出.Add(PLC_輸出_Y42);
            PLC_輸出.Add(PLC_輸出_Y43);
            PLC_輸出.Add(PLC_輸出_Y44);
            PLC_輸出.Add(PLC_輸出_Y45);
            PLC_輸出.Add(PLC_輸出_Y46);
            PLC_輸出.Add(PLC_輸出_Y47);

            PLC_輸出.Add(PLC_輸出_Y50);
            PLC_輸出.Add(PLC_輸出_Y51);
            PLC_輸出.Add(PLC_輸出_Y52);
            PLC_輸出.Add(PLC_輸出_Y53);
            PLC_輸出.Add(PLC_輸出_Y54);
            PLC_輸出.Add(PLC_輸出_Y55);
            PLC_輸出.Add(PLC_輸出_Y56);
            PLC_輸出.Add(PLC_輸出_Y57);

            PLC_輸出.Add(PLC_輸出_Y60);
            PLC_輸出.Add(PLC_輸出_Y61);
            PLC_輸出.Add(PLC_輸出_Y62);
            PLC_輸出.Add(PLC_輸出_Y63);
            PLC_輸出.Add(PLC_輸出_Y64);
            PLC_輸出.Add(PLC_輸出_Y65);
            PLC_輸出.Add(PLC_輸出_Y66);
            PLC_輸出.Add(PLC_輸出_Y67);

            PLC_輸出.Add(PLC_輸出_Y70);
            PLC_輸出.Add(PLC_輸出_Y71);
            PLC_輸出.Add(PLC_輸出_Y72);
            PLC_輸出.Add(PLC_輸出_Y73);
            PLC_輸出.Add(PLC_輸出_Y74);
            PLC_輸出.Add(PLC_輸出_Y75);
            PLC_輸出.Add(PLC_輸出_Y76);
            PLC_輸出.Add(PLC_輸出_Y77);
            #endregion
            #region S0~S77
            PLC_按鈕.Add(PLC_按鈕_S00);
            PLC_按鈕.Add(PLC_按鈕_S01);
            PLC_按鈕.Add(PLC_按鈕_S02);
            PLC_按鈕.Add(PLC_按鈕_S03);
            PLC_按鈕.Add(PLC_按鈕_S04);
            PLC_按鈕.Add(PLC_按鈕_S05);
            PLC_按鈕.Add(PLC_按鈕_S06);
            PLC_按鈕.Add(PLC_按鈕_S07);

            PLC_按鈕.Add(PLC_按鈕_S10);
            PLC_按鈕.Add(PLC_按鈕_S11);
            PLC_按鈕.Add(PLC_按鈕_S12);
            PLC_按鈕.Add(PLC_按鈕_S13);
            PLC_按鈕.Add(PLC_按鈕_S14);
            PLC_按鈕.Add(PLC_按鈕_S15);
            PLC_按鈕.Add(PLC_按鈕_S16);
            PLC_按鈕.Add(PLC_按鈕_S17);

            PLC_按鈕.Add(PLC_按鈕_S20);
            PLC_按鈕.Add(PLC_按鈕_S21);
            PLC_按鈕.Add(PLC_按鈕_S22);
            PLC_按鈕.Add(PLC_按鈕_S23);
            PLC_按鈕.Add(PLC_按鈕_S24);
            PLC_按鈕.Add(PLC_按鈕_S25);
            PLC_按鈕.Add(PLC_按鈕_S26);
            PLC_按鈕.Add(PLC_按鈕_S27);

            PLC_按鈕.Add(PLC_按鈕_S30);
            PLC_按鈕.Add(PLC_按鈕_S31);
            PLC_按鈕.Add(PLC_按鈕_S32);
            PLC_按鈕.Add(PLC_按鈕_S33);
            PLC_按鈕.Add(PLC_按鈕_S34);
            PLC_按鈕.Add(PLC_按鈕_S35);
            PLC_按鈕.Add(PLC_按鈕_S36);
            PLC_按鈕.Add(PLC_按鈕_S37);

            PLC_按鈕.Add(PLC_按鈕_S40);
            PLC_按鈕.Add(PLC_按鈕_S41);
            PLC_按鈕.Add(PLC_按鈕_S42);
            PLC_按鈕.Add(PLC_按鈕_S43);
            PLC_按鈕.Add(PLC_按鈕_S44);
            PLC_按鈕.Add(PLC_按鈕_S45);
            PLC_按鈕.Add(PLC_按鈕_S46);
            PLC_按鈕.Add(PLC_按鈕_S47);

            PLC_按鈕.Add(PLC_按鈕_S50);
            PLC_按鈕.Add(PLC_按鈕_S51);
            PLC_按鈕.Add(PLC_按鈕_S52);
            PLC_按鈕.Add(PLC_按鈕_S53);
            PLC_按鈕.Add(PLC_按鈕_S54);
            PLC_按鈕.Add(PLC_按鈕_S55);
            PLC_按鈕.Add(PLC_按鈕_S56);
            PLC_按鈕.Add(PLC_按鈕_S57);

            PLC_按鈕.Add(PLC_按鈕_S60);
            PLC_按鈕.Add(PLC_按鈕_S61);
            PLC_按鈕.Add(PLC_按鈕_S62);
            PLC_按鈕.Add(PLC_按鈕_S63);
            PLC_按鈕.Add(PLC_按鈕_S64);
            PLC_按鈕.Add(PLC_按鈕_S65);
            PLC_按鈕.Add(PLC_按鈕_S66);
            PLC_按鈕.Add(PLC_按鈕_S67);

            PLC_按鈕.Add(PLC_按鈕_S70);
            PLC_按鈕.Add(PLC_按鈕_S71);
            PLC_按鈕.Add(PLC_按鈕_S72);
            PLC_按鈕.Add(PLC_按鈕_S73);
            PLC_按鈕.Add(PLC_按鈕_S74);
            PLC_按鈕.Add(PLC_按鈕_S75);
            PLC_按鈕.Add(PLC_按鈕_S76);
            PLC_按鈕.Add(PLC_按鈕_S77);
            #endregion
            #region S1000~S1315
            PLC_按鈕按下.Add(PLC_按鈕按下_S1000);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1005);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1010);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1015);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1020);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1025);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1030);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1035);

            PLC_按鈕按下.Add(PLC_按鈕按下_S1040);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1045);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1050);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1055);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1060);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1065);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1070);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1075);

            PLC_按鈕按下.Add(PLC_按鈕按下_S1080);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1085);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1090);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1095);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1100);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1105);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1110);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1115);

            PLC_按鈕按下.Add(PLC_按鈕按下_S1120);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1125);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1130);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1135);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1140);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1145);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1150);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1155);

            PLC_按鈕按下.Add(PLC_按鈕按下_S1160);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1165);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1170);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1175);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1180);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1185);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1190);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1195);

            PLC_按鈕按下.Add(PLC_按鈕按下_S1200);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1205);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1210);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1215);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1220);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1225);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1230);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1235);

            PLC_按鈕按下.Add(PLC_按鈕按下_S1240);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1245);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1250);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1255);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1260);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1265);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1270);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1275);

            PLC_按鈕按下.Add(PLC_按鈕按下_S1280);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1285);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1290);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1295);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1300);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1305);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1310);
            PLC_按鈕按下.Add(PLC_按鈕按下_S1315);


            #endregion
            while (true)
            {
                for (int i = 0; i < 64; i++)
                {
                    PLC_輸出[i].Bool = false;
                    PLC_按鈕[i].Bool = false;
                    PLC_按鈕按下[i].Bool = false;
                    if (PLC_輸出[i].Bool || PLC_按鈕[i].Bool || PLC_按鈕按下[i].Bool) flag_OutputClose = true;
                }
                if (!flag_OutputClose)
                {
                    break;
                }

            }

        }















        #endregion
        #region 測PIN程式
        private char[] char_PIN_Counter;
        private string str_PIN_Counter;
        private byte[] byte_PIN_Counter;
        int cnt_counter = 1;
        //private List<byte> byte_PIN_Counter = new List<byte>();
        private PLC_Device PLC_Device_上端入料測PIN_Counter = new PLC_Device("D4000");
        private PLC_Device PLC_Device_上端成品測PIN_Counter = new PLC_Device("D4001");
        private PLC_Device PLC_Device_下端入料測PIN_Counter = new PLC_Device("D4002");
        private PLC_Device PLC_Device_下端成品測PIN_Counter = new PLC_Device("D4003");
        int StartIndex;
        int EndIndex;


        private void sub_PIN_Counter()
        {
            try
            {
                byte_PIN_Counter = mySerial_counter.ReadByte();
                if (cnt_counter >= 65500)
                {
                    byte_PIN_Counter = null;
                    mySerial_counter.ClearReadByte();
                    cnt_counter = 1;
                }
                if (cnt_counter == 1)
                {
                    if (byte_PIN_Counter != null)
                    {
                        MyTimer_counter.TickStop();
                        MyTimer_counter.StartTickTime(200);
                        cnt_counter++;
                    }
                }
                if (cnt_counter == 2)
                {
                    if (MyTimer_counter.IsTimeOut())
                    {
                        cnt_counter = 65500;
                    }
                    if (byte_PIN_Counter != null)
                    {
                        if (byte_PIN_Counter.Length == 18)
                        {
                            for (int i = 0; i < 18; i++)
                            {
                                if (byte_PIN_Counter[i] == 2)
                                {
                                    StartIndex = 2;
                                    Console.WriteLine($"起始碼 : {StartIndex}");
                                    cnt_counter++;
                                    break;
                                }
                            }

                        }
                        else cnt_counter = 65500;
                    }

                }
                if (cnt_counter == 3)
                {
                    if (byte_PIN_Counter != null)
                    {
                        for (int i = 0; i < 18; i++)
                        {
                            if (byte_PIN_Counter[i] == 9)
                            {
                                EndIndex = 9;
                                Console.WriteLine($"結束碼 : {EndIndex}");
                                for (int k = 0; k < 16; k++)
                                {
                                    Console.Write(byte_PIN_Counter[k]);
                                    Console.WriteLine();
                                    PLC_Device_上端入料測PIN_Counter.Value = (byte_PIN_Counter[1] - 48) * 10;
                                    PLC_Device_上端入料測PIN_Counter.Value += byte_PIN_Counter[2] - 48;

                                    PLC_Device_上端成品測PIN_Counter.Value = (byte_PIN_Counter[5] - 48) * 10;
                                    PLC_Device_上端成品測PIN_Counter.Value += byte_PIN_Counter[6] - 48;

                                    PLC_Device_下端入料測PIN_Counter.Value = (byte_PIN_Counter[9] - 48) * 10;
                                    PLC_Device_下端入料測PIN_Counter.Value += byte_PIN_Counter[10] - 48;

                                    PLC_Device_下端成品測PIN_Counter.Value = (byte_PIN_Counter[13] - 48) * 10;
                                    PLC_Device_下端成品測PIN_Counter.Value += byte_PIN_Counter[14] - 48;
                                }
                                cnt_counter++;
                                break;
                            }
                        }
                    }

                }
                if (cnt_counter == 4) cnt_counter = 65500;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }


        }
        private void plC_UI_Init1_UI_Finished_Event()
        {
            //this.PLC_Device_相機序號.Value = 1711170006;
            this.PLC_Device_相機序號.Value = 1710250008;
            if (!PLC_Device_相機已建立.Bool) PLC_Device_相機初始化.Bool = true;
            if (!PLC_Device_CCD01相機開啟.Bool) PLC_Device_CCD01相機已連線.Bool = true;
            if (!PLC_Device_CCD02相機開啟.Bool) PLC_Device_CCD02相機已連線.Bool = true;
            if (!PLC_Device_光源控制已建立.Bool) PLC_Device_光源控制初始化.Bool = true;

            this.mySerial_counter.Init("COM11", 115200, 8, System.IO.Ports.Parity.None, System.IO.Ports.StopBits.One);
            this.mySerial_counter.SerialPortOpen();

            this.PLC_Device_Main_CCD01_01_ZOOM_X.Value = 100;
            this.PLC_Device_Main_CCD01_02_ZOOM_X.Value = 100;
            this.PLC_Device_Main_CCD01_03_ZOOM_X.Value = 100;
            this.PLC_Device_Main_CCD01_04_ZOOM_X.Value = 100;
            this.PLC_Device_Main_CCD01_04_ZOOM_X.Value = 100;

            this.PLC_Device_Main_CCD01_01_ZOOM_Y.Value = 100;
            this.PLC_Device_Main_CCD01_02_ZOOM_Y.Value = 100;
            this.PLC_Device_Main_CCD01_03_ZOOM_Y.Value = 100;
            this.PLC_Device_Main_CCD01_04_ZOOM_Y.Value = 100;
            this.PLC_Device_Main_CCD01_04_ZOOM_Y.Value = 100;

            this.PLC_Device_Main_CCD02_01_ZOOM_X.Value = 100;
            this.PLC_Device_Main_CCD02_02_ZOOM_X.Value = 100;
            this.PLC_Device_Main_CCD02_03_ZOOM_X.Value = 100;
            this.PLC_Device_Main_CCD02_04_ZOOM_X.Value = 100;

            this.PLC_Device_Main_CCD02_01_ZOOM_Y.Value = 100;
            this.PLC_Device_Main_CCD02_02_ZOOM_Y.Value = 100;
            this.PLC_Device_Main_CCD02_03_ZOOM_Y.Value = 100;
            this.PLC_Device_Main_CCD02_04_ZOOM_Y.Value = 100;

            PLC_UI_Init.Set_PLC_ScreenPage(panel_Main01, this.plC_ScreenPage_Main01);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_Main02, this.plC_ScreenPage_Main02);

            this.MyThread_Program_CCD01_SNAP = new MyThread(this.FindForm());
            this.MyThread_Program_CCD01_SNAP.Add_Method(this.plC_MindVision_Camera_UI_CCD01.Method);
            this.MyThread_Program_CCD01_SNAP.Add_Method(this.sub_Program_CCD01_SNAP);
            this.MyThread_Program_CCD01_SNAP.Add_Method(this.儲存圖片_產生日期時間);
            this.MyThread_Program_CCD01_SNAP.SetSleepTime(1);
            this.MyThread_Program_CCD01_SNAP.AutoRun(true);
            this.MyThread_Program_CCD01_SNAP.AutoStop(false);
            this.MyThread_Program_CCD01_SNAP.Trigger();

            this.MyThread_Program_CCD01 = new MyThread(this.FindForm());
            this.MyThread_Program_CCD01.Add_Method(this.Program_CCD01_01);
            this.MyThread_Program_CCD01.Add_Method(this.Program_CCD01_02);
            this.MyThread_Program_CCD01.Add_Method(this.Program_CCD01_03);
            this.MyThread_Program_CCD01.Add_Method(this.Program_CCD01_04);
            this.MyThread_Program_CCD01.SetSleepTime(1);
            this.MyThread_Program_CCD01.AutoRun(true);
            this.MyThread_Program_CCD01.AutoStop(false);
            this.MyThread_Program_CCD01.Trigger();

            this.MyThread_Program_CCD02_SNAP = new MyThread(this.FindForm());
            this.MyThread_Program_CCD02_SNAP.Add_Method(this.plC_MindVision_Camera_UI2_CCD02.Method);
            this.MyThread_Program_CCD02_SNAP.Add_Method(this.sub_Program_CCD02_SNAP);
            this.MyThread_Program_CCD02_SNAP.SetSleepTime(1);
            this.MyThread_Program_CCD02_SNAP.AutoRun(true);
            this.MyThread_Program_CCD02_SNAP.AutoStop(false);
            this.MyThread_Program_CCD02_SNAP.Trigger();

            this.MyThread_Program_CCD02 = new MyThread(this.FindForm());
            this.MyThread_Program_CCD02.Add_Method(this.Program_CCD02_01);
            this.MyThread_Program_CCD02.Add_Method(this.Program_CCD02_02);
            this.MyThread_Program_CCD02.Add_Method(this.Program_CCD02_03);
            this.MyThread_Program_CCD02.Add_Method(this.Program_CCD02_04);
            this.MyThread_Program_CCD02.SetSleepTime(1);
            this.MyThread_Program_CCD02.AutoRun(true);
            this.MyThread_Program_CCD02.AutoStop(false);
            this.MyThread_Program_CCD02.Trigger();

            this.MyThread_Canvas = new MyThread(this.FindForm());
            this.MyThread_Canvas.Add_Method(this.h_Canvas_Tech_CCD01_01.Get_Method());
            this.MyThread_Canvas.Add_Method(this.h_Canvas_Tech_CCD01_02.Get_Method());
            this.MyThread_Canvas.Add_Method(this.h_Canvas_Tech_CCD01_04.Get_Method());
            this.MyThread_Canvas.Add_Method(this.h_Canvas_Tech_CCD01_03.Get_Method());

            this.MyThread_Canvas.Add_Method(this.h_Canvas_Tech_CCD02_01.Get_Method());
            this.MyThread_Canvas.Add_Method(this.h_Canvas_Tech_CCD02_02.Get_Method());
            this.MyThread_Canvas.Add_Method(this.h_Canvas_Tech_CCD02_04.Get_Method());
            this.MyThread_Canvas.Add_Method(this.h_Canvas_Tech_CCD02_03.Get_Method());
            this.MyThread_Canvas.SetSleepTime(1);
            this.MyThread_Canvas.AutoRun(true);
            this.MyThread_Canvas.AutoStop(false);
            this.MyThread_Canvas.Trigger();

            this.MyThread_Counter = new MyThread(this.FindForm());
            this.MyThread_Canvas.Add_Method(this.sub_PIN_Counter);
            this.MyThread_Canvas.SetSleepTime(1);
            this.MyThread_Canvas.AutoRun(true);
            this.MyThread_Canvas.AutoStop(false);
            this.MyThread_Canvas.Trigger();
        }

        private void tabControl8_SelectedIndexChanged(object sender, EventArgs e)
        {
            AxMatch_CCD02_02_PIN相似度測試.LoadFile(".//" + CCD02_02_樣板儲存名稱);
            if (AxMatch_CCD02_02_PIN相似度測試.IsLearnPattern)
            {
                this.AxImageCopier_CCD02_02_PIN相似度測試_GetPattern.SrcImageHandle = AxMatch_CCD02_02_PIN相似度測試.PatternVegaHandle;
                this.AxImageCopier_CCD02_02_PIN相似度測試_GetPattern.DstImageHandle = this.CCD02_02_PIN相似度測試_GetPattern_AxImageBW8.VegaHandle;
                this.AxImageCopier_CCD02_02_PIN相似度測試_GetPattern.Copy();
                this.CCD02_02_PatternCanvasRefresh(this.CCD02_02_PIN相似度測試_GetPattern_AxImageBW8.VegaHandle, this.CCD02_02_PatternCanvas_ZoomX, this.CCD02_02_PatternCanvas_ZoomY);
            }
        }
        private void tabControl5_SelectedIndexChanged(object sender, EventArgs e)
        {
            AxMatch_CCD02_04_PIN相似度測試.LoadFile(".//" + CCD02_04_樣板儲存名稱);
            if (AxMatch_CCD02_04_PIN相似度測試.IsLearnPattern)
            {
                this.AxImageCopier_CCD02_04_PIN相似度測試_GetPattern.SrcImageHandle = AxMatch_CCD02_04_PIN相似度測試.PatternVegaHandle;
                this.AxImageCopier_CCD02_04_PIN相似度測試_GetPattern.DstImageHandle = this.CCD02_04_PIN相似度測試_GetPattern_AxImageBW8.VegaHandle;
                this.AxImageCopier_CCD02_04_PIN相似度測試_GetPattern.Copy();
                this.CCD02_04_PatternCanvasRefresh(this.CCD02_04_PIN相似度測試_GetPattern_AxImageBW8.VegaHandle, this.CCD02_04_PatternCanvas_ZoomX, this.CCD02_04_PatternCanvas_ZoomY);
            }
        }
        private void plC_AlarmFlow1_Load(object sender, EventArgs e)
        {

        }
        #endregion

        DateTime Date = DateTime.Now;
        String Time, Time_Short;
        String 年, 月, 日, 時, 分, 秒;
        void 儲存圖片_產生日期時間()
        {
            Date = DateTime.Now;
            年 = Date.Year.ToString();
            月 = Date.Month.ToString();
            日 = Date.Day.ToString();
            時 = Date.Hour.ToString();
            分 = Date.Minute.ToString();
            秒 = Date.Second.ToString();
            if (年.Length < 2) 年 = "0" + 年;
            if (年.Length < 2) 年 = "0" + 年;
            if (月.Length < 2) 月 = "0" + 月;
            if (月.Length < 2) 月 = "0" + 月;
            if (日.Length < 2) 日 = "0" + 日;
            if (日.Length < 2) 日 = "0" + 日;
            if (時.Length < 2) 時 = "0" + 時;
            if (時.Length < 2) 時 = "0" + 時;
            if (分.Length < 2) 分 = "0" + 分;
            if (分.Length < 2) 分 = "0" + 分;
            if (秒.Length < 2) 秒 = "0" + 秒;
            if (秒.Length < 2) 秒 = "0" + 秒;
            Time = 年 + "年" + 月 + "月" + 日 + "日" + "-" + 時 + "時" + 分 + "分" + 秒 + "秒";
            Time_Short = 年 + 月 + 日 + "-" + 時 + 分 + 秒;
        }

        private void button_CCD01_01_影像儲存_OK瀏覽_Click_1(object sender, EventArgs e)
        {
            if (folderBrowserDialog_選擇路徑.ShowDialog(this) == DialogResult.OK)
            {
                plC_WordBox_CCD01_01_OK存圖路徑.Text = folderBrowserDialog_選擇路徑.SelectedPath;
                DriveInfo drive = new DriveInfo(plC_WordBox_CCD01_01_OK存圖路徑.Text);
                double TEST = Math.Round((drive.AvailableFreeSpace / 1024D / 1024D / 1024D), 2) * 100;
                plC_NumBox_CCD01_01_磁碟容量.Value = (int)TEST;
            }
        }
        private void button_CCD01_02_影像儲存_OK瀏覽_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog_選擇路徑.ShowDialog(this) == DialogResult.OK)
            {
                plC_WordBox_CCD01_02_OK存圖路徑.Text = folderBrowserDialog_選擇路徑.SelectedPath;
                DriveInfo drive = new DriveInfo(plC_WordBox_CCD01_02_OK存圖路徑.Text);
                double TEST = Math.Round((drive.AvailableFreeSpace / 1024D / 1024D / 1024D), 2) * 100;
                plC_NumBox_CCD01_02_磁碟容量.Value = (int)TEST;
            }

        }
        private void button_CCD01_03_影像儲存_OK瀏覽_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog_選擇路徑.ShowDialog(this) == DialogResult.OK)
            {
                plC_WordBox_CCD01_03_OK存圖路徑.Text = folderBrowserDialog_選擇路徑.SelectedPath;
                DriveInfo drive = new DriveInfo(plC_WordBox_CCD01_03_OK存圖路徑.Text);
                double TEST = Math.Round((drive.AvailableFreeSpace / 1024D / 1024D / 1024D), 2) * 100;
                plC_NumBox_CCD01_03_磁碟容量.Value = (int)TEST;
            }
        }
        private void button_CCD01_04_影像儲存_OK瀏覽_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog_選擇路徑.ShowDialog(this) == DialogResult.OK)
            {
                plC_WordBox_CCD01_04_OK存圖路徑.Text = folderBrowserDialog_選擇路徑.SelectedPath;
                DriveInfo drive = new DriveInfo(plC_WordBox_CCD01_04_OK存圖路徑.Text);
                double TEST = Math.Round((drive.AvailableFreeSpace / 1024D / 1024D / 1024D), 2) * 100;
                plC_NumBox_CCD01_04_磁碟容量.Value = (int)TEST;
            }
        }

        private void button_CCD02_01_影像儲存_OK瀏覽_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog_選擇路徑.ShowDialog(this) == DialogResult.OK)
            {
                plC_WordBox_CCD02_01_OK存圖路徑.Text = folderBrowserDialog_選擇路徑.SelectedPath;
                DriveInfo drive = new DriveInfo(plC_WordBox_CCD02_01_OK存圖路徑.Text);
                double TEST = Math.Round((drive.AvailableFreeSpace / 1024D / 1024D / 1024D), 2) * 100;
                plC_NumBox_CCD02_01_磁碟容量.Value = (int)TEST;
            }
        }
        private void button_CCD02_02_影像儲存_OK瀏覽_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog_選擇路徑.ShowDialog(this) == DialogResult.OK)
            {
                plC_WordBox_CCD02_02_OK存圖路徑.Text = folderBrowserDialog_選擇路徑.SelectedPath;
                DriveInfo drive = new DriveInfo(plC_WordBox_CCD02_02_OK存圖路徑.Text);
                double TEST = Math.Round((drive.AvailableFreeSpace / 1024D / 1024D / 1024D), 2) * 100;
                plC_NumBox_CCD02_02_磁碟容量.Value = (int)TEST;
            }
        }
        private void button_CCD02_03_影像儲存_OK瀏覽_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog_選擇路徑.ShowDialog(this) == DialogResult.OK)
            {
                plC_WordBox_CCD02_03_OK存圖路徑.Text = folderBrowserDialog_選擇路徑.SelectedPath;
                DriveInfo drive = new DriveInfo(plC_WordBox_CCD02_03_OK存圖路徑.Text);
                double TEST = Math.Round((drive.AvailableFreeSpace / 1024D / 1024D / 1024D), 2) * 100;
                plC_NumBox_CCD02_03_磁碟容量.Value = (int)TEST;
            }
        }
        private void button_CCD02_04_影像儲存_OK瀏覽_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog_選擇路徑.ShowDialog(this) == DialogResult.OK)
            {
                plC_WordBox_CCD02_04_OK存圖路徑.Text = folderBrowserDialog_選擇路徑.SelectedPath;
                DriveInfo drive = new DriveInfo(plC_WordBox_CCD02_04_OK存圖路徑.Text);
                double TEST = Math.Round((drive.AvailableFreeSpace / 1024D / 1024D / 1024D), 2) * 100;
                plC_NumBox_CCD02_04_磁碟容量.Value = (int)TEST;
            }
        }

        private void button_CCD01_01_影像儲存_NG瀏覽_Click_1(object sender, EventArgs e)
        {
            if (folderBrowserDialog_選擇路徑.ShowDialog(this) == DialogResult.OK)
            {
                plC_WordBox_CCD01_01_NG存圖路徑.Text = folderBrowserDialog_選擇路徑.SelectedPath;
                DriveInfo drive = new DriveInfo(plC_WordBox_CCD01_01_NG存圖路徑.Text);
                double TEST = Math.Round((drive.AvailableFreeSpace / 1024D / 1024D / 1024D), 2) * 100;
                plC_NumBox_CCD01_01_磁碟容量.Value = (int)TEST;
            }
        }
        private void button_CCD01_02_影像儲存_NG瀏覽_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog_選擇路徑.ShowDialog(this) == DialogResult.OK)
            {
                plC_WordBox_CCD01_02_NG存圖路徑.Text = folderBrowserDialog_選擇路徑.SelectedPath;
                DriveInfo drive = new DriveInfo(plC_WordBox_CCD01_02_NG存圖路徑.Text);
                double TEST = Math.Round((drive.AvailableFreeSpace / 1024D / 1024D / 1024D), 2) * 100;
                plC_NumBox_CCD01_02_磁碟容量.Value = (int)TEST; 
            }
        }
        private void button_CCD01_03_影像儲存_NG瀏覽_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog_選擇路徑.ShowDialog(this) == DialogResult.OK)
            {
                plC_WordBox_CCD01_03_NG存圖路徑.Text = folderBrowserDialog_選擇路徑.SelectedPath;
                DriveInfo drive = new DriveInfo(plC_WordBox_CCD01_03_NG存圖路徑.Text);
                double TEST = Math.Round((drive.AvailableFreeSpace / 1024D / 1024D / 1024D), 2) * 100;
                plC_NumBox_CCD01_03_磁碟容量.Value = (int)TEST;
            }
        }
        private void button_CCD01_04_影像儲存_NG瀏覽_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog_選擇路徑.ShowDialog(this) == DialogResult.OK)
            {
                plC_WordBox_CCD01_04_NG存圖路徑.Text = folderBrowserDialog_選擇路徑.SelectedPath;
                DriveInfo drive = new DriveInfo(plC_WordBox_CCD01_04_NG存圖路徑.Text);
                double TEST = Math.Round((drive.AvailableFreeSpace / 1024D / 1024D / 1024D), 2) * 100;
                plC_NumBox_CCD01_04_磁碟容量.Value = (int)TEST;
                
            }
        }

        private void button_CCD02_01_影像儲存_NG瀏覽_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog_選擇路徑.ShowDialog(this) == DialogResult.OK)
            {
                plC_WordBox_CCD02_01_NG存圖路徑.Text = folderBrowserDialog_選擇路徑.SelectedPath;
                DriveInfo drive = new DriveInfo(plC_WordBox_CCD02_01_NG存圖路徑.Text);
                double TEST = Math.Round((drive.AvailableFreeSpace / 1024D / 1024D / 1024D), 2) * 100;
                plC_NumBox_CCD02_01_磁碟容量.Value = (int)TEST;
            }
        }
        private void button_CCD02_02_影像儲存_NG瀏覽_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog_選擇路徑.ShowDialog(this) == DialogResult.OK)
            {
                plC_WordBox_CCD02_02_NG存圖路徑.Text = folderBrowserDialog_選擇路徑.SelectedPath;
                DriveInfo drive = new DriveInfo(plC_WordBox_CCD02_02_NG存圖路徑.Text);
                double TEST = Math.Round((drive.AvailableFreeSpace / 1024D / 1024D / 1024D), 2) * 100;
                plC_NumBox_CCD02_02_磁碟容量.Value = (int)TEST;
            }
        }
        private void button_CCD02_03_影像儲存_NG瀏覽_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog_選擇路徑.ShowDialog(this) == DialogResult.OK)
            {
                plC_WordBox_CCD02_03_NG存圖路徑.Text = folderBrowserDialog_選擇路徑.SelectedPath;
                DriveInfo drive = new DriveInfo(plC_WordBox_CCD02_03_NG存圖路徑.Text);
                double TEST = Math.Round((drive.AvailableFreeSpace / 1024D / 1024D / 1024D), 2) * 100;
                plC_NumBox_CCD02_03_磁碟容量.Value = (int)TEST;
            }
        }
        private void button_CCD02_04_影像儲存_NG瀏覽_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog_選擇路徑.ShowDialog(this) == DialogResult.OK)
            {
                plC_WordBox_CCD02_04_NG存圖路徑.Text = folderBrowserDialog_選擇路徑.SelectedPath;
                DriveInfo drive = new DriveInfo(plC_WordBox_CCD02_04_NG存圖路徑.Text);
                double TEST = Math.Round((drive.AvailableFreeSpace / 1024D / 1024D / 1024D), 2) * 100;
                plC_NumBox_CCD02_04_磁碟容量.Value = (int)TEST;
            }
        }


        public string CCD01_01_OK儲存檔案檢查(String FilePlace, String OK_NG, int MaxNumFile)
        {

            Directory.CreateDirectory(FilePlace);
            DirectoryInfo di = new DirectoryInfo(@FilePlace);

            FileInfo[] info = di.GetFiles("*" + OK_NG + "*.bmp");
            int NumOfFile = info.Length;
            DriveInfo drive = new DriveInfo(plC_WordBox_CCD01_01_OK存圖路徑.Text);
            double 儲存容量 = drive.AvailableFreeSpace / 1024D / 1024D / 1024D;

            FileInfo[] 檢查重複檔案 = di.GetFiles(Time_Short + "-" + "*" + OK_NG + "*.bmp");
            string 儲存位置 = FilePlace + @"\" + Time_Short + "-" + (檢查重複檔案.Length + 1).ToString() + "_" + OK_NG + ".bmp";
            if (儲存容量 <= 0.5)
            {
                MessageBox.Show("硬碟:<" + drive.Name + ">" + " 儲存空間異常‧" + "\r\n" + "剩餘 :【" + Math.Round(儲存容量, 2) + "GB 】,需要容量 :【100 GB 】", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if ((NumOfFile >= MaxNumFile) && (MaxNumFile != 0))
            {
                MessageBox.Show("注意!!已到達儲存張數上限值‧" + "\r\n" + "\r\n" + "目前檔案 : " + "<" + 儲存位置 + ">" + "\r\n" + "目前張數 : " + (info.Length + 1) + " 張", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return 儲存位置;
        }
        public string CCD01_01_NG儲存檔案檢查(String FilePlace, String OK_NG, int MaxNumFile)
        {

            Directory.CreateDirectory(FilePlace);
            DirectoryInfo di = new DirectoryInfo(@FilePlace);

            FileInfo[] info = di.GetFiles("*" + OK_NG + "*.bmp");
            int NumOfFile = info.Length;
            DriveInfo drive = new DriveInfo(plC_WordBox_CCD01_01_NG存圖路徑.Text);
            double 儲存容量 = drive.AvailableFreeSpace / 1024D / 1024D / 1024D;

            FileInfo[] 檢查重複檔案 = di.GetFiles(Time_Short + "-" + "*" + OK_NG + "*.bmp");
            string 儲存位置 = FilePlace + @"\" + Time_Short + "-" + (檢查重複檔案.Length + 1).ToString() + "_" + OK_NG + ".bmp";
            if (儲存容量 <= 0.5)
            {
                MessageBox.Show("硬碟:<" + drive.Name + ">" + " 儲存空間異常‧" + "\r\n" + "剩餘 :【" + Math.Round(儲存容量, 2) + "GB 】,需要容量 :【100 GB 】", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if ((NumOfFile >= MaxNumFile) && (MaxNumFile != 0))
            {
                MessageBox.Show("注意!!已到達儲存張數上限值‧" + "\r\n" + "\r\n" + "目前檔案 : " + "<" + 儲存位置 + ">" + "\r\n" + "目前張數 : " + (info.Length + 1) + " 張", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return 儲存位置;
        }
        public string CCD01_02_OK儲存檔案檢查(String FilePlace, String OK_NG, int MaxNumFile)
        {

            Directory.CreateDirectory(FilePlace);
            DirectoryInfo di = new DirectoryInfo(@FilePlace);

            FileInfo[] info = di.GetFiles("*" + OK_NG + "*.bmp");
            int NumOfFile = info.Length;
            DriveInfo drive = new DriveInfo(plC_WordBox_CCD01_02_OK存圖路徑.Text);
            double 儲存容量 = drive.AvailableFreeSpace / 1024D / 1024D / 1024D;

            FileInfo[] 檢查重複檔案 = di.GetFiles(Time_Short + "-" + "*" + OK_NG + "*.bmp");
            string 儲存位置 = FilePlace + @"\" + Time_Short + "-" + (檢查重複檔案.Length + 1).ToString() + "_" + OK_NG + ".bmp";
            if (儲存容量 <= 0.5)
            {
                MessageBox.Show("硬碟:<" + drive.Name + ">" + " 儲存空間異常‧" + "\r\n" + "剩餘 :【" + Math.Round(儲存容量, 2) + "GB 】,需要容量 :【100 GB 】", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if ((NumOfFile >= MaxNumFile) && (MaxNumFile != 0))
            {
                MessageBox.Show("注意!!已到達儲存張數上限值‧" + "\r\n" + "\r\n" + "目前檔案 : " + "<" + 儲存位置 + ">" + "\r\n" + "目前張數 : " + (info.Length + 1) + " 張", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return 儲存位置;
        }
        public string CCD01_02_NG儲存檔案檢查(String FilePlace, String OK_NG, int MaxNumFile)
        {

            Directory.CreateDirectory(FilePlace);
            DirectoryInfo di = new DirectoryInfo(@FilePlace);

            FileInfo[] info = di.GetFiles("*" + OK_NG + "*.bmp");
            int NumOfFile = info.Length;
            DriveInfo drive = new DriveInfo(plC_WordBox_CCD01_02_NG存圖路徑.Text);
            double 儲存容量 = drive.AvailableFreeSpace / 1024D / 1024D / 1024D;

            FileInfo[] 檢查重複檔案 = di.GetFiles(Time_Short + "-" + "*" + OK_NG + "*.bmp");
            string 儲存位置 = FilePlace + @"\" + Time_Short + "-" + (檢查重複檔案.Length + 1).ToString() + "_" + OK_NG + ".bmp";
            if (儲存容量 <= 0.5)
            {
                MessageBox.Show("硬碟:<" + drive.Name + ">" + " 儲存空間異常‧" + "\r\n" + "剩餘 :【" + Math.Round(儲存容量, 2) + "GB 】,需要容量 :【100 GB 】", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if ((NumOfFile >= MaxNumFile) && (MaxNumFile != 0))
            {
                MessageBox.Show("注意!!已到達儲存張數上限值‧" + "\r\n" + "\r\n" + "目前檔案 : " + "<" + 儲存位置 + ">" + "\r\n" + "目前張數 : " + (info.Length + 1) + " 張", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return 儲存位置;
        }
        public string CCD01_03_OK儲存檔案檢查(String FilePlace, String OK_NG, int MaxNumFile)
        {

            Directory.CreateDirectory(FilePlace);
            DirectoryInfo di = new DirectoryInfo(@FilePlace);

            FileInfo[] info = di.GetFiles("*" + OK_NG + "*.bmp");
            int NumOfFile = info.Length;
            DriveInfo drive = new DriveInfo(plC_WordBox_CCD01_03_OK存圖路徑.Text);
            double 儲存容量 = drive.AvailableFreeSpace / 1024D / 1024D / 1024D;

            FileInfo[] 檢查重複檔案 = di.GetFiles(Time_Short + "-" + "*" + OK_NG + "*.bmp");
            string 儲存位置 = FilePlace + @"\" + Time_Short + "-" + (檢查重複檔案.Length + 1).ToString() + "_" + OK_NG + ".bmp";
            if (儲存容量 <= 0.5)
            {
                MessageBox.Show("硬碟:<" + drive.Name + ">" + " 儲存空間異常‧" + "\r\n" + "剩餘 :【" + Math.Round(儲存容量, 2) + "GB 】,需要容量 :【100 GB 】", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if ((NumOfFile >= MaxNumFile) && (MaxNumFile != 0))
            {
                MessageBox.Show("注意!!已到達儲存張數上限值‧" + "\r\n" + "\r\n" + "目前檔案 : " + "<" + 儲存位置 + ">" + "\r\n" + "目前張數 : " + (info.Length + 1) + " 張", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return 儲存位置;
        }
        public string CCD01_03_NG儲存檔案檢查(String FilePlace, String OK_NG, int MaxNumFile)
        {

            Directory.CreateDirectory(FilePlace);
            DirectoryInfo di = new DirectoryInfo(@FilePlace);

            FileInfo[] info = di.GetFiles("*" + OK_NG + "*.bmp");
            int NumOfFile = info.Length;
            DriveInfo drive = new DriveInfo(plC_WordBox_CCD01_03_NG存圖路徑.Text);
            double 儲存容量 = drive.AvailableFreeSpace / 1024D / 1024D / 1024D;

            FileInfo[] 檢查重複檔案 = di.GetFiles(Time_Short + "-" + "*" + OK_NG + "*.bmp");
            string 儲存位置 = FilePlace + @"\" + Time_Short + "-" + (檢查重複檔案.Length + 1).ToString() + "_" + OK_NG + ".bmp";
            if (儲存容量 <= 0.5)
            {
                MessageBox.Show("硬碟:<" + drive.Name + ">" + " 儲存空間異常‧" + "\r\n" + "剩餘 :【" + Math.Round(儲存容量, 2) + "GB 】,需要容量 :【100 GB 】", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if ((NumOfFile >= MaxNumFile) && (MaxNumFile != 0))
            {
                MessageBox.Show("注意!!已到達儲存張數上限值‧" + "\r\n" + "\r\n" + "目前檔案 : " + "<" + 儲存位置 + ">" + "\r\n" + "目前張數 : " + (info.Length + 1) + " 張", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return 儲存位置;
        }
        public string CCD01_04_OK儲存檔案檢查(String FilePlace, String OK_NG, int MaxNumFile)
        {

            Directory.CreateDirectory(FilePlace);
            DirectoryInfo di = new DirectoryInfo(@FilePlace);

            FileInfo[] info = di.GetFiles("*" + OK_NG + "*.bmp");
            int NumOfFile = info.Length;
            DriveInfo drive = new DriveInfo(plC_WordBox_CCD01_04_OK存圖路徑.Text);
            double 儲存容量 = drive.AvailableFreeSpace / 1024D / 1024D / 1024D;

            FileInfo[] 檢查重複檔案 = di.GetFiles(Time_Short + "-" + "*" + OK_NG + "*.bmp");
            string 儲存位置 = FilePlace + @"\" + Time_Short + "-" + (檢查重複檔案.Length + 1).ToString() + "_" + OK_NG + ".bmp";
            if (儲存容量 <= 0.5)
            {
                MessageBox.Show("硬碟:<" + drive.Name + ">" + " 儲存空間異常‧" + "\r\n" + "剩餘 :【" + Math.Round(儲存容量, 2) + "GB 】,需要容量 :【100 GB 】", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if ((NumOfFile >= MaxNumFile) && (MaxNumFile != 0))
            {
                MessageBox.Show("注意!!已到達儲存張數上限值‧" + "\r\n" + "\r\n" + "目前檔案 : " + "<" + 儲存位置 + ">" + "\r\n" + "目前張數 : " + (info.Length + 1) + " 張", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return 儲存位置;
        }
        public string CCD01_04_NG儲存檔案檢查(String FilePlace, String OK_NG, int MaxNumFile)
        {

            Directory.CreateDirectory(FilePlace);
            DirectoryInfo di = new DirectoryInfo(@FilePlace);

            FileInfo[] info = di.GetFiles("*" + OK_NG + "*.bmp");
            int NumOfFile = info.Length;
            DriveInfo drive = new DriveInfo(plC_WordBox_CCD01_04_NG存圖路徑.Text);
            double 儲存容量 = drive.AvailableFreeSpace / 1024D / 1024D / 1024D;

            FileInfo[] 檢查重複檔案 = di.GetFiles(Time_Short + "-" + "*" + OK_NG + "*.bmp");
            string 儲存位置 = FilePlace + @"\" + Time_Short + "-" + (檢查重複檔案.Length + 1).ToString() + "_" + OK_NG + ".bmp";
            if (儲存容量 <= 0.5)
            {
                MessageBox.Show("硬碟:<" + drive.Name + ">" + " 儲存空間異常‧" + "\r\n" + "剩餘 :【" + Math.Round(儲存容量, 2) + "GB 】,需要容量 :【100 GB 】", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if ((NumOfFile >= MaxNumFile) && (MaxNumFile != 0))
            {
                MessageBox.Show("注意!!已到達儲存張數上限值‧" + "\r\n" + "\r\n" + "目前檔案 : " + "<" + 儲存位置 + ">" + "\r\n" + "目前張數 : " + (info.Length + 1) + " 張", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return 儲存位置;
        }

        public string CCD02_01_OK儲存檔案檢查(String FilePlace, String OK_NG, int MaxNumFile)
        {

            Directory.CreateDirectory(FilePlace);
            DirectoryInfo di = new DirectoryInfo(@FilePlace);

            FileInfo[] info = di.GetFiles("*" + OK_NG + "*.bmp");
            int NumOfFile = info.Length;
            DriveInfo drive = new DriveInfo(plC_WordBox_CCD02_01_OK存圖路徑.Text);
            double 儲存容量 = drive.AvailableFreeSpace / 1024D / 1024D / 1024D;

            FileInfo[] 檢查重複檔案 = di.GetFiles(Time_Short + "-" + "*" + OK_NG + "*.bmp");
            string 儲存位置 = FilePlace + @"\" + Time_Short + "-" + (檢查重複檔案.Length + 1).ToString() + "_" + OK_NG + ".bmp";
            if (儲存容量 <= 0.5)
            {
                MessageBox.Show("硬碟:<" + drive.Name + ">" + " 儲存空間異常‧" + "\r\n" + "剩餘 :【" + Math.Round(儲存容量, 2) + "GB 】,需要容量 :【100 GB 】", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if ((NumOfFile >= MaxNumFile) && (MaxNumFile != 0))
            {
                MessageBox.Show("注意!!已到達儲存張數上限值‧" + "\r\n" + "\r\n" + "目前檔案 : " + "<" + 儲存位置 + ">" + "\r\n" + "目前張數 : " + (info.Length + 1) + " 張", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return 儲存位置;
        }
        public string CCD02_01_NG儲存檔案檢查(String FilePlace, String OK_NG, int MaxNumFile)
        {

            Directory.CreateDirectory(FilePlace);
            DirectoryInfo di = new DirectoryInfo(@FilePlace);

            FileInfo[] info = di.GetFiles("*" + OK_NG + "*.bmp");
            int NumOfFile = info.Length;
            DriveInfo drive = new DriveInfo(plC_WordBox_CCD02_01_NG存圖路徑.Text);
            double 儲存容量 = drive.AvailableFreeSpace / 1024D / 1024D / 1024D;

            FileInfo[] 檢查重複檔案 = di.GetFiles(Time_Short + "-" + "*" + OK_NG + "*.bmp");
            string 儲存位置 = FilePlace + @"\" + Time_Short + "-" + (檢查重複檔案.Length + 1).ToString() + "_" + OK_NG + ".bmp";
            if (儲存容量 <= 0.5)
            {
                MessageBox.Show("硬碟:<" + drive.Name + ">" + " 儲存空間異常‧" + "\r\n" + "剩餘 :【" + Math.Round(儲存容量, 2) + "GB 】,需要容量 :【100 GB 】", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if ((NumOfFile >= MaxNumFile) && (MaxNumFile != 0))
            {
                MessageBox.Show("注意!!已到達儲存張數上限值‧" + "\r\n" + "\r\n" + "目前檔案 : " + "<" + 儲存位置 + ">" + "\r\n" + "目前張數 : " + (info.Length + 1) + " 張", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return 儲存位置;
        }
        public string CCD02_02_OK儲存檔案檢查(String FilePlace, String OK_NG, int MaxNumFile)
        {

            Directory.CreateDirectory(FilePlace);
            DirectoryInfo di = new DirectoryInfo(@FilePlace);

            FileInfo[] info = di.GetFiles("*" + OK_NG + "*.bmp");
            int NumOfFile = info.Length;
            DriveInfo drive = new DriveInfo(plC_WordBox_CCD02_02_OK存圖路徑.Text);
            double 儲存容量 = drive.AvailableFreeSpace / 1024D / 1024D / 1024D;

            FileInfo[] 檢查重複檔案 = di.GetFiles(Time_Short + "-" + "*" + OK_NG + "*.bmp");
            string 儲存位置 = FilePlace + @"\" + Time_Short + "-" + (檢查重複檔案.Length + 1).ToString() + "_" + OK_NG + ".bmp";
            if (儲存容量 <= 0.5)
            {
                MessageBox.Show("硬碟:<" + drive.Name + ">" + " 儲存空間異常‧" + "\r\n" + "剩餘 :【" + Math.Round(儲存容量, 2) + "GB 】,需要容量 :【100 GB 】", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if ((NumOfFile >= MaxNumFile) && (MaxNumFile != 0))
            {
                MessageBox.Show("注意!!已到達儲存張數上限值‧" + "\r\n" + "\r\n" + "目前檔案 : " + "<" + 儲存位置 + ">" + "\r\n" + "目前張數 : " + (info.Length + 1) + " 張", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return 儲存位置;
        }
        public string CCD02_02_NG儲存檔案檢查(String FilePlace, String OK_NG, int MaxNumFile)
        {

            Directory.CreateDirectory(FilePlace);
            DirectoryInfo di = new DirectoryInfo(@FilePlace);

            FileInfo[] info = di.GetFiles("*" + OK_NG + "*.bmp");
            int NumOfFile = info.Length;
            DriveInfo drive = new DriveInfo(plC_WordBox_CCD02_02_NG存圖路徑.Text);
            double 儲存容量 = drive.AvailableFreeSpace / 1024D / 1024D / 1024D;

            FileInfo[] 檢查重複檔案 = di.GetFiles(Time_Short + "-" + "*" + OK_NG + "*.bmp");
            string 儲存位置 = FilePlace + @"\" + Time_Short + "-" + (檢查重複檔案.Length + 1).ToString() + "_" + OK_NG + ".bmp";
            if (儲存容量 <= 0.5)
            {
                MessageBox.Show("硬碟:<" + drive.Name + ">" + " 儲存空間異常‧" + "\r\n" + "剩餘 :【" + Math.Round(儲存容量, 2) + "GB 】,需要容量 :【100 GB 】", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if ((NumOfFile >= MaxNumFile) && (MaxNumFile != 0))
            {
                MessageBox.Show("注意!!已到達儲存張數上限值‧" + "\r\n" + "\r\n" + "目前檔案 : " + "<" + 儲存位置 + ">" + "\r\n" + "目前張數 : " + (info.Length + 1) + " 張", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return 儲存位置;
        }

        private void label389_Click(object sender, EventArgs e)
        {

        }

        public string CCD02_03_OK儲存檔案檢查(String FilePlace, String OK_NG, int MaxNumFile)
        {

            Directory.CreateDirectory(FilePlace);
            DirectoryInfo di = new DirectoryInfo(@FilePlace);

            FileInfo[] info = di.GetFiles("*" + OK_NG + "*.bmp");
            int NumOfFile = info.Length;
            DriveInfo drive = new DriveInfo(plC_WordBox_CCD02_03_OK存圖路徑.Text);
            double 儲存容量 = drive.AvailableFreeSpace / 1024D / 1024D / 1024D;

            FileInfo[] 檢查重複檔案 = di.GetFiles(Time_Short + "-" + "*" + OK_NG + "*.bmp");
            string 儲存位置 = FilePlace + @"\" + Time_Short + "-" + (檢查重複檔案.Length + 1).ToString() + "_" + OK_NG + ".bmp";
            if (儲存容量 <= 0.5)
            {
                MessageBox.Show("硬碟:<" + drive.Name + ">" + " 儲存空間異常‧" + "\r\n" + "剩餘 :【" + Math.Round(儲存容量, 2) + "GB 】,需要容量 :【100 GB 】", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if ((NumOfFile >= MaxNumFile) && (MaxNumFile != 0))
            {
                MessageBox.Show("注意!!已到達儲存張數上限值‧" + "\r\n" + "\r\n" + "目前檔案 : " + "<" + 儲存位置 + ">" + "\r\n" + "目前張數 : " + (info.Length + 1) + " 張", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return 儲存位置;
        }
        public string CCD02_03_NG儲存檔案檢查(String FilePlace, String OK_NG, int MaxNumFile)
        {

            Directory.CreateDirectory(FilePlace);
            DirectoryInfo di = new DirectoryInfo(@FilePlace);

            FileInfo[] info = di.GetFiles("*" + OK_NG + "*.bmp");
            int NumOfFile = info.Length;
            DriveInfo drive = new DriveInfo(plC_WordBox_CCD02_03_NG存圖路徑.Text);
            double 儲存容量 = drive.AvailableFreeSpace / 1024D / 1024D / 1024D;

            FileInfo[] 檢查重複檔案 = di.GetFiles(Time_Short + "-" + "*" + OK_NG + "*.bmp");
            string 儲存位置 = FilePlace + @"\" + Time_Short + "-" + (檢查重複檔案.Length + 1).ToString() + "_" + OK_NG + ".bmp";
            if (儲存容量 <= 0.5)
            {
                MessageBox.Show("硬碟:<" + drive.Name + ">" + " 儲存空間異常‧" + "\r\n" + "剩餘 :【" + Math.Round(儲存容量, 2) + "GB 】,需要容量 :【100 GB 】", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if ((NumOfFile >= MaxNumFile) && (MaxNumFile != 0))
            {
                MessageBox.Show("注意!!已到達儲存張數上限值‧" + "\r\n" + "\r\n" + "目前檔案 : " + "<" + 儲存位置 + ">" + "\r\n" + "目前張數 : " + (info.Length + 1) + " 張", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return 儲存位置;
        }
        public string CCD02_04_OK儲存檔案檢查(String FilePlace, String OK_NG, int MaxNumFile)
        {

            Directory.CreateDirectory(FilePlace);
            DirectoryInfo di = new DirectoryInfo(@FilePlace);

            FileInfo[] info = di.GetFiles("*" + OK_NG + "*.bmp");
            int NumOfFile = info.Length;
            DriveInfo drive = new DriveInfo(plC_WordBox_CCD02_04_OK存圖路徑.Text);
            double 儲存容量 = drive.AvailableFreeSpace / 1024D / 1024D / 1024D;

            FileInfo[] 檢查重複檔案 = di.GetFiles(Time_Short + "-" + "*" + OK_NG + "*.bmp");
            string 儲存位置 = FilePlace + @"\" + Time_Short + "-" + (檢查重複檔案.Length + 1).ToString() + "_" + OK_NG + ".bmp";
            if (儲存容量 <= 0.5)
            {
                MessageBox.Show("硬碟:<" + drive.Name + ">" + " 儲存空間異常‧" + "\r\n" + "剩餘 :【" + Math.Round(儲存容量, 2) + "GB 】,需要容量 :【100 GB 】", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if ((NumOfFile >= MaxNumFile) && (MaxNumFile != 0))
            {
                MessageBox.Show("注意!!已到達儲存張數上限值‧" + "\r\n" + "\r\n" + "目前檔案 : " + "<" + 儲存位置 + ">" + "\r\n" + "目前張數 : " + (info.Length + 1) + " 張", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return 儲存位置;
        }
        public string CCD02_04_NG儲存檔案檢查(String FilePlace, String OK_NG, int MaxNumFile)
        {

            Directory.CreateDirectory(FilePlace);
            DirectoryInfo di = new DirectoryInfo(@FilePlace);

            FileInfo[] info = di.GetFiles("*" + OK_NG + "*.bmp");
            int NumOfFile = info.Length;
            DriveInfo drive = new DriveInfo(plC_WordBox_CCD02_04_NG存圖路徑.Text);
            double 儲存容量 = drive.AvailableFreeSpace / 1024D / 1024D / 1024D;

            FileInfo[] 檢查重複檔案 = di.GetFiles(Time_Short + "-" + "*" + OK_NG + "*.bmp");
            string 儲存位置 = FilePlace + @"\" + Time_Short + "-" + (檢查重複檔案.Length + 1).ToString() + "_" + OK_NG + ".bmp";
            if (儲存容量 <= 0.5)
            {
                MessageBox.Show("硬碟:<" + drive.Name + ">" + " 儲存空間異常‧" + "\r\n" + "剩餘 :【" + Math.Round(儲存容量, 2) + "GB 】,需要容量 :【100 GB 】", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if ((NumOfFile >= MaxNumFile) && (MaxNumFile != 0))
            {
                MessageBox.Show("注意!!已到達儲存張數上限值‧" + "\r\n" + "\r\n" + "目前檔案 : " + "<" + 儲存位置 + ">" + "\r\n" + "目前張數 : " + (info.Length + 1) + " 張", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return 儲存位置;
        }

        private bool 整理檔案(String FilePlace, String FileName, int MaxNumFile)
        {
            bool FLAG = false;


            while (true)
            {
                DirectoryInfo di = new DirectoryInfo(@FilePlace);
                FileInfo[] info = di.GetFiles("*" + FileName + "*.bmp");
                int NumOfFile = info.Length;
                if (NumOfFile >= MaxNumFile && NumOfFile != 0)
                {
                    try
                    {
                        info[0].Delete();
                    }
                    catch
                    {

                    }

                }
                else { break; }
            }


            return FLAG;
        }
        public bool 儲存檔案_往後移位(String FilePlace, String FileName, int MaxNumFile)
        {
            bool FLAG = false;
            Directory.CreateDirectory(FilePlace);
            DirectoryInfo di = new DirectoryInfo(@FilePlace);
            FileInfo[] info = di.GetFiles(FileName + "*.bmp");
            int NumOfFile = info.Length;

            for (int i = NumOfFile; i >= 0; i--)
            {
                if (i > MaxNumFile)
                {
                    info[i - 1].Delete();
                }
                else
                {
                    if (i > 0)
                    {
                        String Num = (i + 1).ToString();
                        if (Num.Length < 6) Num = "0" + Num;
                        if (Num.Length < 6) Num = "0" + Num;
                        if (Num.Length < 6) Num = "0" + Num;
                        if (Num.Length < 6) Num = "0" + Num;
                        if (Num.Length < 6) Num = "0" + Num;
                        if (Num.Length < 6) Num = "0" + Num;
                        Num = "-" + Num;
                        String NewFileName = FileName + Num.ToString();
                        try { info[i - 1].MoveTo(Path.Combine(info[i - 1].DirectoryName, NewFileName + info[i - 1].Extension)); }
                        catch { }
                    }
                }
            }
            return FLAG;
        }
    }
}
