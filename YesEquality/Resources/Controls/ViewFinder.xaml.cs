using Microsoft.Devices;
using Microsoft.Phone.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Windows.Phone.Media.Capture;

namespace YesEquality.Controls
{
    public partial class ViewFinder : UserControl
    {
        PhotoCaptureDevice captureDevice;
        double orientationAngle = 0.0;
        CameraSensorLocation sensorLocation;
        object parentPage;
        bool previewRunning = false;
        bool commandRunning = false;
        public enum StrechMode
        {
            Uniform,
            UniformFill
        };
        StrechMode mode;
        
        public StrechMode Mode
        {
            get { return mode; }
            set
            {
                if (!commandRunning)
                {
                    mode = value;
                    computeVideoBrushTransform();
                }
            }
        }
    
        public CameraSensorLocation SensorLocation
        {
            get { return sensorLocation; }
            set
            {
                if (!commandRunning)
                {
                    if (!PhotoCamera.IsCameraTypeSupported(CameraType.FrontFacing) && (value == CameraSensorLocation.Front))
                    {
                        sensorLocation = CameraSensorLocation.Back;
                    }
                    else
                    {
                        sensorLocation = value;
                    }
                    
                    if (previewRunning)
                    {
                        initCamera();
                    }
                }
            }
        }

        public void Start()
        {
            previewRunning = true;
            initCamera();
        }

        public void Stop()
        {
            previewRunning = false;
            if (captureDevice != null)
            {
                captureDevice.Dispose();
                captureDevice = null;
            }
        }

        public ViewFinder()
        {
            InitializeComponent();

            Mode = StrechMode.UniformFill;
            this.LayoutUpdated += ViewFinder_LayoutUpdated;
            
            if (DesignerProperties.IsInDesignTool == false)
            {
                AppBootstrapper bootstrapper = Application.Current.Resources["bootstrapper"] as AppBootstrapper;
                var frame = bootstrapper.GetRootFrame();
                frame.OrientationChanged += RootFrame_OrientationChanged;
                frame.Navigating += RootFrame_Navigating;
                frame.Navigated += RootFrame_Navigated;
                Tap += ViewFinder_Tap;
                Loaded += ((s,e) => {
                    parentPage = frame.Content;
                    setPageOrientation(frame.Orientation);
                });
            };
        }

        private async void ViewFinder_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (previewRunning && !commandRunning)
            {
                try
                {
                    commandRunning = true;

                    // Compute vector between preview picture center and inverted transformation center
                    var tmp = ViewFinderBrush.Transform.Inverse.TransformBounds(new Rect(new Point(), ViewFinderCanvas.RenderSize));
                    var dx = captureDevice.PreviewResolution.Width / 2 - (tmp.X + tmp.Width / 2);
                    var dy = captureDevice.PreviewResolution.Height / 2 - (tmp.Y + tmp.Height / 2);
        
                    // Invert tap position
                    var p =  e.GetPosition(this);
                    var pInPreview = ViewFinderBrush.Transform.Inverse.Transform(p);

                    // Transform inverted position to picture reference
                    double X = pInPreview.X + dx;
                    double Y = pInPreview.Y + dy;

                    if (X < 0) X = 0;
                    if (X >= captureDevice.PreviewResolution.Width) X = captureDevice.PreviewResolution.Width - 1;

                    if (Y >= captureDevice.PreviewResolution.Height) Y = captureDevice.PreviewResolution.Height - 1;
                    if (Y < 0) Y = 0;

                    captureDevice.FocusRegion = new Windows.Foundation.Rect(
                        new Windows.Foundation.Point(X, Y),
                        new Windows.Foundation.Size());

                    captureDevice.SetProperty(KnownCameraPhotoProperties.FlashMode, FlashState.Off);
                    await captureDevice.FocusAsync();
                }
                catch (Exception)
                {
                }
                finally
                {
                    commandRunning = false;
                }
            }
        }

        private void RootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (captureDevice != null)
            {
                captureDevice.Dispose();
                captureDevice = null;
            }
        }

        private void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (parentPage != null && parentPage == e.Content)
            {
                if (previewRunning)
                {
                    initCamera();
                }
            }
        }

        private void setPageOrientation(PageOrientation orientation)
        {
            if ((orientation & PageOrientation.Portrait) == PageOrientation.Portrait)
            {
                orientationAngle = 0;
            }
            else if ((orientation & PageOrientation.LandscapeLeft) == PageOrientation.LandscapeLeft)
            {
                orientationAngle = -90;
            }
            else
            {
                orientationAngle = +90;
            }
            
            computeVideoBrushTransform();
        }

        private void RootFrame_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            setPageOrientation(e.Orientation);
        }

        private void ViewFinder_LayoutUpdated(object sender, EventArgs e)
        {
            if (previewRunning)
            {
                computeVideoBrushTransform();
            }
        }

        private async void initCamera()
        {
            if (commandRunning)
            {
                return;
            }
                
            try
            {
                commandRunning = true;

                if (captureDevice != null)
                {
                    captureDevice.Dispose();
                    captureDevice = null;
                }
                
                var SupportedResolutions = PhotoCaptureDevice.GetAvailableCaptureResolutions(sensorLocation).ToArray();
                captureDevice = await PhotoCaptureDevice.OpenAsync(sensorLocation, SupportedResolutions.Last());
                ViewFinderBrush.SetSource(captureDevice);
                computeVideoBrushTransform();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception while start camera device: " + ex.Message);
            }
            finally
            {
                commandRunning = false;
            }
        }

        private void computeVideoBrushTransform()
        {
            if (captureDevice == null)
            {
                return;
            }

            var tmptransform = new RotateTransform() { Angle = orientationAngle + captureDevice.SensorRotationInDegrees };
            var previewSize = tmptransform.TransformBounds(new Rect(new Point(), new Size(captureDevice.PreviewResolution.Width, captureDevice.PreviewResolution.Height)));

            double s1 = ViewFinderCanvas.ActualWidth / previewSize.Width;
            double s2 = ViewFinderCanvas.ActualHeight / previewSize.Height;
            
            double scale = mode == StrechMode.UniformFill ? Math.Max(s1, s2) : Math.Min(s1, s2);

            var t = new TransformGroup();
            if (sensorLocation == CameraSensorLocation.Front)
            {
                t.Children.Add(new CompositeTransform() { Rotation = -(orientationAngle + captureDevice.SensorRotationInDegrees), CenterX = ViewFinderCanvas.ActualWidth / 2, CenterY = ViewFinderCanvas.ActualHeight / 2, ScaleX = scale, ScaleY = scale });
                t.Children.Add(new ScaleTransform() { ScaleX = -1, CenterX = ViewFinderCanvas.ActualWidth / 2, CenterY = ViewFinderCanvas.ActualHeight / 2 });
            }
            else
            {
                t.Children.Add(new CompositeTransform() { Rotation = orientationAngle + captureDevice.SensorRotationInDegrees, CenterX = ViewFinderCanvas.ActualWidth / 2, CenterY = ViewFinderCanvas.ActualHeight / 2, ScaleX = scale, ScaleY = scale });
            }
          
            ViewFinderBrush.Transform = t;
        }
        public async Task<CameraFocusStatus> FocusAsync()
        {
            if (commandRunning)
            {
                return CameraFocusStatus.NotLocked;
            }
                
            try
            {
                commandRunning = true;
                if (captureDevice != null && sensorLocation == CameraSensorLocation.Back)
                {
                    captureDevice.SetProperty(KnownCameraPhotoProperties.FlashMode, FlashState.Off);
                    return await captureDevice.FocusAsync();
                }
            }
            finally
            {
                commandRunning = false;
            }
            
            return CameraFocusStatus.NotLocked;
        }
        public async Task<Stream> TakePicture()
        {
            if (captureDevice == null || commandRunning)
            {
                return null;
            }

            try
            {
                commandRunning = true;
                int angle = (int)(orientationAngle + captureDevice.SensorRotationInDegrees);

                if (sensorLocation == CameraSensorLocation.Front)
                {
                    //angle = -angle;
                }

                captureDevice.SetProperty(KnownCameraGeneralProperties.EncodeWithOrientation, angle);
                captureDevice.SetProperty(KnownCameraGeneralProperties.SpecifiedCaptureOrientation, 0);

                var cameraCaptureSequence = captureDevice.CreateCaptureSequence(1);
                var stream = new MemoryStream();
                cameraCaptureSequence.Frames[0].CaptureStream = stream.AsOutputStream();
                await captureDevice.PrepareCaptureSequenceAsync(cameraCaptureSequence);
                await cameraCaptureSequence.StartCaptureAsync();

                if (sensorLocation == CameraSensorLocation.Back)
                {
                    return stream;
                }
                else
                {
                    var bitmap = new WriteableBitmap(1, 1).FromStream(stream);
                    var flippedBitmap = bitmap.Flip(WriteableBitmapExtensions.FlipMode.Horizontal);
                    var fileStream = new MemoryStream();
                    flippedBitmap.SaveJpeg(fileStream, flippedBitmap.PixelWidth, flippedBitmap.PixelHeight, 100, 100);
                    return fileStream;
                }
            }
            finally
            {
                commandRunning = false;
            }
        }
    }
}
