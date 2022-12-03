using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Imaging;
using Intel.Unite.Common.Display;
using Intel.Unite.Common.Display.Hub;
using Intel.Unite.Common.Module.Common;
using System.Linq;
using System.AddIn.Pipeline;
using UnitePluginTestApp.UniteCore;
using UnitePluginTestApp.ViewModel;

namespace UnitePluginTestApp.Manager
{
    internal class HubDisplayManager : IHubDisplayManager
    {
        #region private properties
        private readonly Collection<HubScreen> _hubScreens = new Collection<HubScreen>();
        private const int _toastMessageMaxVisibilityTime = 10000;

        public HubDisplayManager()
        {
            SetHubScreens();
        }
        #endregion

        public Collection<DisplayView> DisplayViews { get; set; } = new Collection<DisplayView>();

        public Collection<PhysicalDisplay> AvailableDisplays
        {
            get
            {
                if (_hubScreens.Count == 0) SetHubScreens();
                return new Collection<PhysicalDisplay>(_hubScreens.Select(x => x.PhysicalDisplayInfo).ToList());
            }
        }

        public int ToastMessageMaxVisibilityTime => _toastMessageMaxVisibilityTime;

        public Collection<Guid> DisplayIdsWithActivePartialBackground { get; } = new Collection<Guid>();

        internal void ShowQuickAccessLayer()
        {
            _hubScreens.ToList().ForEach(screen =>
            {
                screen.QuickAccessLayer.Show();
                screen.QuickAccessLayer.Focus();
            });
        }

        public event EventHandler DisconnectAllEndSession = delegate { };
        public event EventHandler<Collection<PhysicalDisplay>> DisplaysChanged = delegate { };
        public event EventHandler<DisplayView> ViewAllocated = delegate { };
        public event EventHandler<Guid> ViewDeallocated = delegate { };
        public event EventHandler<DisplayView> ViewUpdated = delegate { };
        public event EventHandler<PhysicalDisplay> PartialBackgroundActive = delegate { };
        public event EventHandler<PhysicalDisplay> PartialBackgroundInactive = delegate { };

        public bool ActivateLocalAnnotationWindow(bool active, Guid displayViewId)
        {
            throw new NotImplementedException();
        }

        public void AllocateUiInHubDisplayAsync(FrameworkElement uiElement, HubAllocationInfo hubAllocationInfo, Action<HubAllocationResult> allocateCallback, GetScreenShot getScreenShotDelegate = null, bool createAnnotationWindow = false, bool allowRemoteAnnotations = false)
        {
            throw new NotImplementedException();
        }

        public void AllocateUiInHubDisplayAsync(MarshalNativeHandleContract uiElement, HubAllocationInfo hubAllocationInfo, Action<HubAllocationResult> allocateCallback, GetScreenShot getScreenShotDelegate = null, bool createAnnotationWindow = false, bool allowRemoteAnnotations = false)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(delegate
            {
                var hubAllocationResult = AllocateUiInHubScreen(uiElement, hubAllocationInfo);

                allocateCallback(hubAllocationResult);
                if (!hubAllocationResult.Success) return;
                DisplayViews.Add(hubAllocationResult.AllocatedView);
                ViewAllocated?.Invoke(this, hubAllocationResult.AllocatedView);
            }));
        }

        private HubAllocationResult AllocateUiInHubScreen(MarshalNativeHandleContract uiElement, HubAllocationInfo hubAllocationInfo)
        {
            return GetHubScreenFor(hubAllocationInfo).AllocateFrameworkElement(
                FrameworkElementAdapters.ContractToViewAdapter(uiElement), hubAllocationInfo);
        }

        private HubScreen GetHubScreenFor(HubAllocationInfo hubAllocationInfo)
        {
            return GetHubScreenFor(hubAllocationInfo.PhysicalDisplay.Id);
        }

        private HubScreen GetHubScreenFor(Guid physicalDisplayId)
        {
            return _hubScreens.FirstOrDefault(screen => screen.PhysicalDisplayInfo.Id == physicalDisplayId);
        }

        public void AllocateUiInHubDisplayAsync(UniteImage image, HubAllocationInfo hubAllocationInfo, Action<HubAllocationResult> allocateCallback)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(delegate
            {
                var hubAllocationResult = AllocateImageInHubScreen(image, hubAllocationInfo);

                allocateCallback(hubAllocationResult);
                if (!hubAllocationResult.Success) return;
                DisplayViews.Add(hubAllocationResult.AllocatedView);
                ViewAllocated?.Invoke(this, hubAllocationResult.AllocatedView);
            }));
        }

        private HubAllocationResult AllocateImageInHubScreen(UniteImage image, HubAllocationInfo hubAllocationInfo)
        {
            return GetHubScreenFor(hubAllocationInfo).AllocateImage(image, hubAllocationInfo);
        }

        public bool ChangeColorForLocalAnnotationWindow(int color, Guid displayViewId)
        {
            throw new NotImplementedException();
        }

        public bool ChangeFadeOutForLocalAnnotationWindow(bool fadeOut, Guid displayViewId)
        {
            throw new NotImplementedException();
        }

        public void CloseMenu(Guid physicalDisplayId)
        {
            GetHubScreenFor(physicalDisplayId).QuickAccessLayer.Hide();
        }

        public void DeallocateUiFromHubDisplayAsync(DisplayView allocatedDisplayView, Action<HubAllocationResult> deallocateCallback)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(delegate
            {
                var hubAllocationResult = DeAllocateUiInHubScreen(allocatedDisplayView);

                deallocateCallback(hubAllocationResult);
                if (!hubAllocationResult.Success) return;
                DisplayViews.Remove(hubAllocationResult.AllocatedView);
                ViewDeallocated?.Invoke(this, allocatedDisplayView.Id);
            }));
        }

        private HubAllocationResult DeAllocateUiInHubScreen(DisplayView allocatedDisplayView)
        {
            return GetHubScreenFor(allocatedDisplayView.HubAllocationInfo.PhysicalDisplay.Id).DeAllocateDisplayView(allocatedDisplayView);
        }

        public void DrawAnnotation(Guid displayViewId, Stroke stroke, Guid requesterId)
        {
            throw new NotImplementedException();
        }

        public bool GenerateTakeOverToken(out TakeOverToken takeOverToken)
        {
            throw new NotImplementedException();
        }

        public Collection<DisplayView> GetAllDisplayViews()
        {
            return DisplayViews;
        }
        
        public Collection<DisplayView> GetAllDisplayViews(HubDisplayViewType type)
        {
            throw new NotImplementedException();
        }

        public Collection<DisplayView> GetAllDisplayViews(Guid physicalDisplay)
        {
            throw new NotImplementedException();
        }

        public Collection<DisplayView> GetAllDisplayViews(Guid physicalDisplay, HubDisplayViewType type)
        {
            throw new NotImplementedException();
        }

        public DisplayView GetDisplayView(Guid displayViewId)
        {
            throw new NotImplementedException();
        }

        public byte[] GetDisplayViewScreenShot(Guid displayViewId)
        {
            throw new NotImplementedException();
        }

        public void HideHubScreen(Guid physicalDisplayId, bool showAuthView)
        {
            throw new NotImplementedException();
        }

        public void OpenMenu(Guid physicalDisplayId, HubDisplayMenuView menuView)
        {
            throw new NotImplementedException();
        }

        public bool RegisterPresentationWindow(IntPtr windowHandle, Guid displayId)
        {
            throw new NotImplementedException();
        }

        public bool ResetDisplays()
        {
            throw new NotImplementedException();
        }

        public bool ShowAllocatedUi(DisplayView displayView, DisplayView ribbon = null)
        {
            var result = false;

            Application.Current.Dispatcher.BeginInvoke(new Action(delegate
            {
                result = ShowUiInHubScreen(displayView, ribbon);
            }));

            return result;
        }

        private bool ShowUiInHubScreen(DisplayView displayView, DisplayView ribbon)
        {
            return GetHubScreenFor(displayView.HubAllocationInfo).ShowAllocatedUi(displayView, ribbon);
        }

        public void ShowHubScreen(Guid physicalDisplayId)
        {
            throw new NotImplementedException();
        }

        public bool TakeOver(Guid displayViewId, Guid takeOverToken)
        {
            throw new NotImplementedException();
        }

        public bool TryDeallocateUisFromFaultyModule(Guid ownerId)
        {
            throw new NotImplementedException();
        }

        public bool TryDeallocateUisFromFualtyModule(Guid ownerId)
        {
            throw new NotImplementedException();
        }

        public bool TryDeallocateUisOfModule(Guid moduleOwnerId)
        {
            throw new NotImplementedException();
        }

        public bool TryShowToastMessage(string text, int visibilityTime, BitmapImage image)
        {
            Console.Write(String.Format("{0} {1} {2} {3}", string.Empty, visibilityTime, string.Empty, text));
            return true;
        }

        public bool TryShowToastMessage(string text, int visibilityTime)
        {
            return TryShowToastMessage(text, visibilityTime, null);
        }

        public bool UpdateAllowRemoteAnnotations(Guid displayId, bool allowAnnotations)
        {
            throw new NotImplementedException();
        }

        public void UpdateUiImage(UniteImage image, DisplayView displayView, Action<HubAllocationResult> allocateCallbacko)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(delegate
            {
                var hubAllocationResult = UpdateImageInHubScreen(image, displayView);

                if (hubAllocationResult.Success) ViewAllocated?.Invoke(this, hubAllocationResult.AllocatedView);
            }));
        }

        private HubAllocationResult UpdateImageInHubScreen(UniteImage image, DisplayView displayView)
        {
            return GetHubScreenFor(displayView.HubAllocationInfo).UpdateImage(image, displayView, displayView.HubAllocationInfo);
        }

        public void SetHubScreens()
        {
            System.Windows.Forms.Screen.AllScreens.ToList().ForEach
            (
                screen => AddDisplay(screen)
            );
        }

        private void AddDisplay(System.Windows.Forms.Screen screen)
        {
            _hubScreens.Add(new HubScreen(
                GetDisplayMatching(screen),
                screen));
        }

        private static PhysicalDisplay GetDisplayMatching(System.Windows.Forms.Screen screen)
        {
            return NativeScreen.GetMonitors().FirstOrDefault(display => display.Name == screen.DeviceName);
        }
    }
}