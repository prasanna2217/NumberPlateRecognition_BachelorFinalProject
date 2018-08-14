using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

using DShowNET;
using DShowNET.Device;
namespace video
{
    public partial class MainForm : Form,ISampleGrabberCB
    {
        static int  value = 0;
        /// <summary> flag to detect first Form appearance </summary>
        private bool firstActive;

        /// <summary> base filter of the actually used video devices. </summary>
        private IBaseFilter capFilter;

        /// <summary> graph builder interface. </summary>
        private IGraphBuilder graphBuilder;

        /// <summary> capture graph builder interface. </summary>
        private ICaptureGraphBuilder2 capGraph;
        private ISampleGrabber sampGrabber;

        /// <summary> control interface. </summary>
        private IMediaControl mediaCtrl;

        /// <summary> event interface. </summary>
        private IMediaEventEx mediaEvt;

        /// <summary> video window interface. </summary>
        private IVideoWindow videoWin;

        /// <summary> grabber filter interface. </summary>
        private IBaseFilter baseGrabFlt;

        /// <summary> structure describing the bitmap to grab. </summary>
        private VideoInfoHeader videoInfoHeader;
        private bool captured = true;
        private int bufferedSize;

        /// <summary> buffer for bitmap data. </summary>
        private byte[] savedArray;

        /// <summary> list of installed video devices. </summary>
        private ArrayList capDevices;
        

        private const int WM_GRAPHNOTIFY = 0x00008001;	// message from graph

        private const int WS_CHILD = 0x40000000;	// attributes for video window
        private const int WS_CLIPCHILDREN = 0x02000000;
        private const int WS_CLIPSIBLINGS = 0x04000000;

        /// <summary> event when callback has finished (ISampleGrabberCB.BufferCB). </summary>
        private delegate void CaptureDone();
#if DEBUG
        private int rotCookie = 0;
#endif
        internal enum PlayState
        {
            Init, Stopped, Paused, Running
        }

        public MainForm()
        {
            InitializeComponent();
        }
        
        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            CloseInterfaces();
        }
        
        /// <summary> detect first form appearance, start grabber. </summary>
        //private void MainForm_Activated(object sender, System.EventArgs e)
        private void mainformactive()
        {
            if (firstActive)
                return;
            firstActive = true;

            if (!DsUtils.IsCorrectDirectXVersion())
            {
                MessageBox.Show(this, "DirectX 8.1 NOT installed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.Left = 2000;
                timer1.Enabled = true;
                //value = 2;
                return;
                
                
            }

            if (!DsDev.GetDevicesOfCat(FilterCategory.VideoInputDevice, out capDevices))
            {
                MessageBox.Show(this, "No video capture devices found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.Left = 2000;
                timer1.Enabled =true;
                //value = 2;
                return;
              
                
            }

            DsDevice dev = null;
            if (capDevices.Count == 1)
                dev = capDevices[0] as DsDevice;
            else
            {
                DeviceSelector selector = new DeviceSelector(capDevices);
                selector.ShowDialog(this);
                dev = selector.SelectedDevice;
            }

            if (dev == null)
            {
                //this.Close();
                this.Left = 2000;
                timer1.Enabled = true;
                //value = 2;
                return;

                
            }

            if (!StartupVideo(dev.Mon))
            {
                //{   this.Close();
                //return;
                this.Left = 2000;
                timer1.Enabled = true;
                //value = 2;
                return;
            }
        }

        private void videoPanel_Resize(object sender, System.EventArgs e)
        {
            ResizeVideoWindow();
        }
           
        /// <summary> handler for toolbar button clicks. </summary>
        private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {
            Trace.WriteLine("!!BTN: toolBar_ButtonClick");

            int hr;
            if (sampGrabber == null)
                return;

            if (e.Button == toolBarBtnGrab)
            {
                Trace.WriteLine("!!BTN: toolBarBtnGrab");

                if (savedArray == null)
                {
                    int size = videoInfoHeader.BmiHeader.ImageSize;
                    if ((size < 1000) || (size > 16000000))
                        return;
                    savedArray = new byte[size + 64000];
                }

                toolBarBtnSave.Enabled = false;
                Image old = pictureBox.Image;
                pictureBox.Image = null;
                if (old != null)
                    old.Dispose();

                toolBarBtnGrab.Enabled = false;
                captured = false;
                hr = sampGrabber.SetCallback(this, 1);
            }
            else if (e.Button == toolBarBtnSave)
            {
                Trace.WriteLine("!!BTN: toolBarBtnSave");

               SaveFileDialog sd = new SaveFileDialog();
                
                string filename = "grabbedimage.BMP";
                string path = Application.StartupPath+"\\"+filename;
                //sd.FileName = @"grabbedimage.Bmp";
                //sd.Title = "Save Image as...";
                //sd.Filter = "Bitmap file (*.tiff)|*.tiff";
                //sd.FilterIndex = 1;
                //if (sd.ShowDialog() != DialogResult.OK)
                //    return;

                pictureBox.Image.Save(path, ImageFormat.Bmp);
                Application.Exit();

            }
           

        }

        /// <summary> capture event, triggered by buffer callback. </summary>
        void OnCaptureDone()
        {
            Trace.WriteLine("!!DLG: OnCaptureDone");
            try
            {
                toolBarBtnGrab.Enabled = true;
                int hr;
                if (sampGrabber == null)
                    return;
                hr = sampGrabber.SetCallback(null, 0);

                int w = videoInfoHeader.BmiHeader.Width;
                int h = videoInfoHeader.BmiHeader.Height;
                if (((w & 0x03) != 0) || (w < 32) || (w > 4096) || (h < 32) || (h > 4096))
                    return;
                int stride = w * 3;

                GCHandle handle = GCHandle.Alloc(savedArray, GCHandleType.Pinned);
                int scan0 = (int)handle.AddrOfPinnedObject();
                scan0 += (h - 1) * stride;
                Bitmap b = new Bitmap(w, h, -stride, PixelFormat.Format24bppRgb, (IntPtr)scan0);
                handle.Free();
                savedArray = null;
                Image old = pictureBox.Image;
                pictureBox.Image = b;
                if (old != null)
                    old.Dispose();
                toolBarBtnSave.Enabled = true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(this, "Could not grab picture\r\n" + ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        /// <summary> start all the interfaces, graphs and preview window. </summary>
         bool StartupVideo(UCOMIMoniker mon)
       
        {
            int hr;
            try
            {
                if (!CreateCaptureDevice(mon))
                    return false;

                if (!GetInterfaces())
                    return false;

                if (!SetupGraph())
                    return false;

                if (!SetupVideoWindow())
                    return false;

                 #if DEBUG
                DsROT.AddGraphToRot(graphBuilder, out rotCookie);		// graphBuilder capGraph
                   #endif

                hr = mediaCtrl.Run();
                if (hr < 0)
                    Marshal.ThrowExceptionForHR(hr);

                bool hasTuner = DsUtils.ShowTunerPinDialog(capGraph, capFilter, this.Handle);
               // toolBarBtnTune.Enabled = hasTuner;

                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(this, "Could not start video stream\r\n" + ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false  ;
            }
        }


        /// <summary> make the video preview window to show in videoPanel. </summary>
        bool SetupVideoWindow()
        {
            int hr;
            try
            {
                // Set the video window to be a child of the main window
                hr = videoWin.put_Owner(videoPanel.Handle);
                if (hr < 0)
                    Marshal.ThrowExceptionForHR(hr);

                // Set video window style
                hr = videoWin.put_WindowStyle(WS_CHILD | WS_CLIPCHILDREN);
                if (hr < 0)
                    Marshal.ThrowExceptionForHR(hr);

                // Use helper function to position video window in client rect of owner window
                ResizeVideoWindow();

                // Make the video window visible, now that it is properly positioned
                hr = videoWin.put_Visible(DsHlp.OATRUE);
                if (hr < 0)
                    Marshal.ThrowExceptionForHR(hr);

                hr = mediaEvt.SetNotifyWindow(this.Handle, WM_GRAPHNOTIFY, IntPtr.Zero);
                if (hr < 0)
                    Marshal.ThrowExceptionForHR(hr);
                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(this, "Could not setup video window\r\n" + ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false  ;
            }
        }

        /// <summary> build the capture graph for grabber. </summary>
        bool SetupGraph()
        {
            int hr;
            try
            {
                hr = capGraph.SetFiltergraph(graphBuilder);
                if (hr < 0)
                    Marshal.ThrowExceptionForHR(hr);

                hr = graphBuilder.AddFilter(capFilter, " Video Capture Device");
                if (hr < 0)
                    Marshal.ThrowExceptionForHR(hr);

                DsUtils.ShowCapPinDialog(capGraph, capFilter, this.Handle);

                AMMediaType media = new AMMediaType();
                media.majorType = MediaType.Video;
                media.subType = MediaSubType.RGB24;
                media.formatType = FormatType.VideoInfo;		// ???
                hr = sampGrabber.SetMediaType(media);
                if (hr < 0)
                    Marshal.ThrowExceptionForHR(hr);

                hr = graphBuilder.AddFilter(baseGrabFlt, " Grabber");
                if (hr < 0)
                    Marshal.ThrowExceptionForHR(hr);

                Guid cat = PinCategory.Preview;
                Guid med = MediaType.Video;
                hr = capGraph.RenderStream(ref cat, ref med, capFilter, null, null); // baseGrabFlt 
                if (hr < 0)
                    Marshal.ThrowExceptionForHR(hr);

                cat = PinCategory.Capture;
                med = MediaType.Video;
                hr = capGraph.RenderStream(ref cat, ref med, capFilter, null, baseGrabFlt); // baseGrabFlt 
                if (hr < 0)
                    Marshal.ThrowExceptionForHR(hr);

                media = new AMMediaType();
                hr = sampGrabber.GetConnectedMediaType(media);
                if (hr < 0)
                    Marshal.ThrowExceptionForHR(hr);
                if ((media.formatType != FormatType.VideoInfo) || (media.formatPtr == IntPtr.Zero))
                    throw new NotSupportedException("Unknown Grabber Media Format");

                videoInfoHeader = (VideoInfoHeader)Marshal.PtrToStructure(media.formatPtr, typeof(VideoInfoHeader));
                Marshal.FreeCoTaskMem(media.formatPtr); media.formatPtr = IntPtr.Zero;

                hr = sampGrabber.SetBufferSamples(false);
                if (hr == 0)
                    hr = sampGrabber.SetOneShot(false);
                if (hr == 0)
                    hr = sampGrabber.SetCallback(null, 0);
                if (hr < 0)
                    Marshal.ThrowExceptionForHR(hr);

                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(this, "Could not setup graph\r\n" + ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                
                return false   ;
            }
        }

        /// <summary> create the used COM components and get the interfaces. </summary>
        bool GetInterfaces()
        {
            Type comType = null;
            object comObj = null;
            try
            {
                comType = Type.GetTypeFromCLSID(Clsid.FilterGraph);
                if (comType == null)
                    throw new NotImplementedException(@" FilterGraph not installed/registered!");
                comObj = Activator.CreateInstance(comType);
                graphBuilder = (IGraphBuilder)comObj; comObj = null;

                Guid clsid = Clsid.CaptureGraphBuilder2;
                Guid riid = typeof(ICaptureGraphBuilder2).GUID;
                comObj = DsBugWO.CreateDsInstance(ref clsid, ref riid);
                capGraph = (ICaptureGraphBuilder2)comObj; comObj = null;

                comType = Type.GetTypeFromCLSID(Clsid.SampleGrabber);
                if (comType == null)
                    throw new NotImplementedException(@" SampleGrabber not installed/registered!");
                comObj = Activator.CreateInstance(comType);
                sampGrabber = (ISampleGrabber)comObj; comObj = null;

                mediaCtrl = (IMediaControl)graphBuilder;
                videoWin = (IVideoWindow)graphBuilder;
                mediaEvt = (IMediaEventEx)graphBuilder;
                baseGrabFlt = (IBaseFilter)sampGrabber;
                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(this, "Could not get interfaces\r\n" + ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            finally
            {
                if (comObj != null)
                    Marshal.ReleaseComObject(comObj); comObj = null;
            }
        }

        /// <summary> create the user selected capture device. </summary>
         bool CreateCaptureDevice(UCOMIMoniker mon)
        
        {
             
            object capObj = null;
            try
            {
                Guid gbf = typeof(IBaseFilter).GUID;
                mon.BindToObject(null, null, ref gbf, out capObj);
                capFilter = (IBaseFilter)capObj; capObj = null;
                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(this, "Could not create capture device\r\n" + ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            finally
            {
                if (capObj != null)
                    Marshal.ReleaseComObject(capObj); capObj = null;
            }

        }


        /// <summary> do cleanup and release DirectShow. </summary>
        void CloseInterfaces()
        {
            int hr;
            try
            {
#if DEBUG
                if (rotCookie != 0)
                    DsROT.RemoveGraphFromRot(ref rotCookie);
#endif

                if (mediaCtrl != null)
                {
                    hr = mediaCtrl.Stop();
                    mediaCtrl = null;
                }

                if (mediaEvt != null)
                {
                    hr = mediaEvt.SetNotifyWindow(IntPtr.Zero, WM_GRAPHNOTIFY, IntPtr.Zero);
                    mediaEvt = null;
                }

                if (videoWin != null)
                {
                    hr = videoWin.put_Visible(DsHlp.OAFALSE);
                    hr = videoWin.put_Owner(IntPtr.Zero);
                    videoWin = null;
                }

                baseGrabFlt = null;
                if (sampGrabber != null)
                    Marshal.ReleaseComObject(sampGrabber); sampGrabber = null;

                if (capGraph != null)
                    Marshal.ReleaseComObject(capGraph); capGraph = null;

                if (graphBuilder != null)
                    Marshal.ReleaseComObject(graphBuilder); graphBuilder = null;

                if (capFilter != null)
                    Marshal.ReleaseComObject(capFilter); capFilter = null;

                if (capDevices != null)
                {
                    foreach (DsDevice d in capDevices)
                        d.Dispose();
                    capDevices = null;
                }
            }
            catch (Exception)
            { }
        }


        /// <summary> resize preview video window to fill client area. </summary>
         void ResizeVideoWindow()
        {
            if (videoWin != null)
            {
                Rectangle rc = videoPanel.ClientRectangle;
                videoWin.SetWindowPosition(0, 0, rc.Right, rc.Bottom);
            }
        }

        /// <summary> override window fn to handle graph events. </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_GRAPHNOTIFY)
            {
                if (mediaEvt != null)
                    OnGraphNotify();
                return;
            }
            base.WndProc(ref m);
        }

        /// <summary> graph event (WM_GRAPHNOTIFY) handler. </summary>
        void OnGraphNotify()
        {
            DsEvCode code;
            int p1, p2, hr = 0;
            do
            {
                hr = mediaEvt.GetEvent(out code, out p1, out p2, 0);
                if (hr < 0)
                    break;
                hr = mediaEvt.FreeEventParams(code, p1, p2);
            }
            while (hr == 0);
        }

        /// <summary> sample callback, NOT USED. </summary>
         int ISampleGrabberCB.SampleCB(double SampleTime, IMediaSample pSample)
        {
            Trace.WriteLine("!!CB: ISampleGrabberCB.SampleCB");
            return 0;
        }


        /// <summary> buffer callback, COULD BE FROM FOREIGN THREAD. </summary>
        int ISampleGrabberCB.BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen)
        {
            if (captured || (savedArray == null))
            {
                Trace.WriteLine("!!CB: ISampleGrabberCB.BufferCB");
                return 0;
            }

            captured = true;
            bufferedSize = BufferLen;
            Trace.WriteLine("!!CB: ISampleGrabberCB.BufferCB  !GRAB! size = " + BufferLen.ToString());
            if ((pBuffer != IntPtr.Zero) && (BufferLen > 1000) && (BufferLen <= savedArray.Length))
                Marshal.Copy(pBuffer, savedArray, 0, BufferLen);
            else
                Trace.WriteLine("    !!!GRAB! failed ");
            this.BeginInvoke(new CaptureDone(this.OnCaptureDone));
            return 0;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //CloseInterfaces();
            //UserMain um = new UserMain();
            //um.MdiParent = this.ParentForm;
            //um.Show();
            //return;
            //Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            mainformactive();
            //this.DialogResult = DialogResult.Cancel;
            
        }

        //private void MainForm_Activated(object sender, EventArgs e)
        //{
        //    if (value !=0)
        //    {
        //        MessageBox.Show("The window needs to be closed click ok","Information ",MessageBoxButtons.OK,MessageBoxIcon.Information );
        //        this.Close();
        //    }
        //}

        //private void MainForm_Deactivate(object sender, EventArgs e)
        //{
        //    //this.Dispose();
        //    //this.Close();
        //}

        //private void MainForm_VisibleChanged(object sender, EventArgs e)
        //{
        //    //this.Close();
        //}

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void btngrab_Click(object sender, EventArgs e)
        //{
        //    Trace.WriteLine("!!BTN: toolBar_ButtonClick");

        //    int hr;
        //    if (sampGrabber == null)
        //        return;
        //    Trace.WriteLine("!!BTN: toolBarBtnGrab");

        //    if (savedArray == null)
        //    {
        //        int size = videoInfoHeader.BmiHeader.ImageSize;
        //        if ((size < 1000) || (size > 16000000))
        //            return;
        //        savedArray = new byte[size + 64000];
        //    }

        //    toolBarBtnSave.Enabled = false;
        //    Image old = pictureBox.Image;
        //    pictureBox.Image = null;
        //    if (old != null)
        //        old.Dispose();

        //    toolBarBtnGrab.Enabled = false;
        //    captured = false;
        //    hr = sampGrabber.SetCallback(this, 1);
        //}

        //private void btncrop_Click(object sender, EventArgs e)
        //{
        //    Trace.WriteLine("!!BTN: toolBarBtnSave");

        //    SaveFileDialog sd = new SaveFileDialog();

        //    string filename = "grabbedimage.BMP";
        //    string path = Application.StartupPath + "\\" + filename;
        //    //sd.FileName = @"grabbedimage.Bmp";
        //    //sd.Title = "Save Image as...";
        //    //sd.Filter = "Bitmap file (*.tiff)|*.tiff";
        //    //sd.FilterIndex = 1;
        //    //if (sd.ShowDialog() != DialogResult.OK)
        //    //    return;

        //    pictureBox.Image.Save(path, ImageFormat.Bmp);


        //    //calling the crop form 

        //}
        
        

        
    }
}