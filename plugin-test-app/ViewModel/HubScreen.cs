using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Intel.Unite.Common.Display;
using Intel.Unite.Common.Display.Hub;
using UnitePluginTestApp.Layer;

namespace UnitePluginTestApp.ViewModel
{
    /// <summary>
    /// Hub Screen 
    /// </summary>
    public class HubScreen
    {
        #region Layers

        private Window HubDisplay { get; set; }
		public QuickAccessLayer QuickAccessLayer { get; private set; }

        private PresentationLayer PresentationLayers { get; set; }

        private PartialBackgroundLayer PartialBackgroundLayers { get; set; }

        #endregion

        #region Public Properties
        public PhysicalDisplay PhysicalDisplayInfo { get; }
        private System.Windows.Forms.Screen CurrentScreen { get; set; }
        #endregion

        #region Private Properties

        private double _dpiX = 1;
        private double _dpiY = 1;

        #endregion

        #region Constructor

        /// <summary>
        /// HubScreen Constructor 
        /// </summary>
        /// <param name="physicalDisplayInfo">Physical Display Information</param>
        /// <param name="currentScreen">Current Screen</param>
        public HubScreen(PhysicalDisplay physicalDisplayInfo, System.Windows.Forms.Screen currentScreen)
        {
            //Get Screen Information
            CurrentScreen = currentScreen;
            PhysicalDisplayInfo = physicalDisplayInfo;

            HubDisplay = new Window
            {
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                Background = Brushes.Transparent,
                Visibility = Visibility.Collapsed,
                ShowInTaskbar = false
            };
            HubDisplay.Show();

            SetPhysicalDisplayLayer();

            QuickAccessLayer = new QuickAccessLayer();
            PresentationLayers = new PresentationLayer(physicalDisplayInfo);
            PartialBackgroundLayers = new PartialBackgroundLayer();

            InitializeScreen();
        }

        public HubScreen()
        {
        }

        private void InitializeScreen()
        {
            try
            {
                QuickAccessLayer.Show();
                QuickAccessLayer.Focus();
                QuickAccessLayer.Activate();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + " " + e.StackTrace);
            }
        }

        private void SetPhysicalDisplayLayer()
        {
            var windowHandleSource = PresentationSource.FromVisual(HubDisplay) as HwndSource;
            if (windowHandleSource?.CompositionTarget != null)
            {
                var screenMatrix = windowHandleSource.CompositionTarget.TransformToDevice;
                _dpiX = screenMatrix.M11;
                _dpiY = screenMatrix.M22;
            }
            if (PhysicalDisplayInfo.Size == null)
                return;
            PhysicalDisplayInfo.Size.X = (int)(HubDisplay.Left = CurrentScreen.Bounds.Left / _dpiX);
            PhysicalDisplayInfo.Size.Y = (int)(HubDisplay.Top = CurrentScreen.Bounds.Top / _dpiY);
            PhysicalDisplayInfo.Size.Width = (int)(HubDisplay.Width = CurrentScreen.Bounds.Width / _dpiX);
            PhysicalDisplayInfo.Size.Height = (int)(HubDisplay.Height = CurrentScreen.Bounds.Height / _dpiY);
        }

        public HubAllocationResult AllocateFrameworkElement(FrameworkElement uiControl, HubAllocationInfo hubAllocationInfo)
        {
            var result = SuccessfulResult(hubAllocationInfo);
            switch (hubAllocationInfo.ViewType)
            {
                case HubDisplayViewType.PresentationView:
                    PresentationLayers.Allocate(uiControl, result.AllocatedView.Id);
                    break;
                case HubDisplayViewType.BackgroundView:
                    break;
                case HubDisplayViewType.AuthView:
                    break;
                case HubDisplayViewType.StatusView:
                    QuickAccessLayer.AllocateStatus(uiControl, result.AllocatedView.Id);  
                    break;
                case HubDisplayViewType.PresentationRibbonView:
                    break;
                case HubDisplayViewType.PartialBackgroundView:
                    PartialBackgroundLayers.Allocate(uiControl, result.AllocatedView.Id);
                    break;
                case HubDisplayViewType.QuickAccessAppView:
                    QuickAccessLayer.AllocateControl(uiControl, result.AllocatedView.Id);
                    break;
                case HubDisplayViewType.QuickAccessAppIconView:
                    QuickAccessLayer.AllocateIcon(uiControl, result.AllocatedView.Id);
                    break;
                default:
                    break;
            }
            return result;
        }

        private static HubAllocationResult SuccessfulResult(HubAllocationInfo hubAllocationInfo)
        {
            return new HubAllocationResult
            {
                Success = true,
                ResultType = HubAllocationResultType.Success,
                AllocatedView = new DisplayView
                {
                    Id = Guid.NewGuid(),
                    AllowRemoteAnnotations = false,
                    HubAllocationInfo = new HubAllocationInfo
                    {
                        PhysicalDisplay = hubAllocationInfo.PhysicalDisplay,
                        ModuleOwnerId = hubAllocationInfo.ModuleOwnerId,
                        FriendlyName = hubAllocationInfo.FriendlyName,
                        ViewType = hubAllocationInfo.ViewType,
                        HubInfo = hubAllocationInfo.HubInfo,
                        UserInfo = hubAllocationInfo.UserInfo,
                        Tag = hubAllocationInfo.Tag,
                    }
                }
            };
        }

        public HubAllocationResult AllocateImage(UniteImage image, HubAllocationInfo hubAllocationInfo)
        {
            var ele = MakeFrameworkelementFromImage(image);
            var result = SuccessfulResult(hubAllocationInfo);

            switch (hubAllocationInfo.ViewType)
            {
                case HubDisplayViewType.BackgroundView:
                    break;
                case HubDisplayViewType.AuthView:
                    break;
                case HubDisplayViewType.StatusView:
                    QuickAccessLayer.AllocateStatus(ele, result.AllocatedView.Id);
                    break;
                case HubDisplayViewType.PresentationView:
                    break;
                case HubDisplayViewType.PresentationRibbonView:
                    break;
                case HubDisplayViewType.PartialBackgroundView:
                    break;
                case HubDisplayViewType.QuickAccessAppView:
                    break;
                case HubDisplayViewType.QuickAccessAppIconView:
                    break;
                default:
                    break;
            }

            return result;
        }

        internal HubAllocationResult UpdateImage(UniteImage image, DisplayView dv, HubAllocationInfo hubAllocationInfo)
        {
            var ele = MakeFrameworkelementFromImage(image);
            ele.Tag = dv;

            switch (hubAllocationInfo.ViewType)
            {
                case HubDisplayViewType.BackgroundView:
                    break;
                case HubDisplayViewType.AuthView:
                    break;
                case HubDisplayViewType.StatusView:
                    break;
                case HubDisplayViewType.PresentationView:
                    break;
                case HubDisplayViewType.PresentationRibbonView:
                    break;
                case HubDisplayViewType.PartialBackgroundView:
                    break;
                case HubDisplayViewType.QuickAccessAppView:
                    break;
                case HubDisplayViewType.QuickAccessAppIconView:
                    break;
                default:
                    break;
            }

            return SuccessfulResult(hubAllocationInfo);
        }

        private static FrameworkElement MakeFrameworkelementFromImage(UniteImage image)
        {
            var frameworkElement = new Grid {Margin = new Thickness(5, 10, 5, 10)};
            var imageControl = new Image();
            ImageSource imgSource = null;
            try
            {
                if (image.Data != null && image.Data.Length > 0)
                {
                    using (var stream = new MemoryStream(image.Data))
                    {
                        var img = new BitmapImage();
                        img.BeginInit();
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.StreamSource = stream;
                        img.EndInit();
                        imgSource = img;
                    }
                }
                imageControl.Source = imgSource;
                imageControl.Stretch = Stretch.Fill;
                frameworkElement.Children.Add(imageControl);
            }
            catch (Exception e)
            {
                throw new ArgumentNullException(@"Invalid value found for UniteImage: Error trying to convert to image control", e);
            }
            return frameworkElement;
        }

        internal HubAllocationResult DeAllocateDisplayView(DisplayView allocatedDisplayView)
        {
            var result = SuccessfulResult(allocatedDisplayView.HubAllocationInfo);

            switch (allocatedDisplayView.HubAllocationInfo.ViewType)
            {
                case HubDisplayViewType.BackgroundView:
                    break;
                case HubDisplayViewType.AuthView:
                    break;
                case HubDisplayViewType.StatusView:
                    QuickAccessLayer.statusView.DeAllocate(allocatedDisplayView.Id);
                    break;
                case HubDisplayViewType.PresentationView:
                    PresentationLayers.DeAllocate(allocatedDisplayView);
                    break;
                case HubDisplayViewType.PresentationRibbonView:
                    break;
                case HubDisplayViewType.PartialBackgroundView:
                    PartialBackgroundLayers.DeAllocate(allocatedDisplayView.Id);
                    break;
                case HubDisplayViewType.QuickAccessAppView:
                    throw new InvalidOperationException("Invalid call: HubDisplayViewType.QuickAccessAppHubView cannot be deallocated by Unite Core.");
                case HubDisplayViewType.QuickAccessAppIconView:
                    throw new InvalidOperationException("Invalid call: HubDisplayViewType.QuickAccessAppIconHubView cannot be deallocated by Unite Core.");
                default:
                    break;
            }

            return result;
            //return SuccessfulResult(allocatedDisplayView.HubAllocationInfo);
        }

        private void OpenQuickAccessView(object sender, EventArgs e)
        {
            if (Application.Current.Dispatcher != null)
                Application.Current.Dispatcher.BeginInvoke(new Action(delegate { QuickAccessLayer.Show(); }));
        }

        public bool ShowAllocatedUi(DisplayView displayView, DisplayView ribbon)
        {
            var result = false;
            try
            {
                switch (displayView.HubAllocationInfo.ViewType)
                {
                    case HubDisplayViewType.BackgroundView:
                        break;
                    case HubDisplayViewType.AuthView:
                        break;
                    case HubDisplayViewType.StatusView:
                        break;
                    case HubDisplayViewType.PresentationView:
                        break;
                    case HubDisplayViewType.PresentationRibbonView:
                        break;
                    case HubDisplayViewType.PartialBackgroundView:
                        break;
                    case HubDisplayViewType.QuickAccessAppView:
                        result = QuickAccessLayer.ShowQuickAccessAppView(displayView);
                        break;
                    case HubDisplayViewType.QuickAccessAppIconView:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        #endregion
    }
}