using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using Intel.Unite.Common.Display;
using Intel.Unite.Common.Display.Hub;
using Intel.Unite.Common.Module.Common;

namespace UnitePluginTest.Stubs
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// A stub for the HubDisplayManager runtime object.
    /// </summary>
    ///
    /// <remarks>
    /// This class extends MarshalByRefObjectBase, which sets up a Lifetime Service Object with the policy
    /// to live forever.
    /// </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class StubHubDisplayManager : MarshalByRefObjectBase, IHubDisplayManager
    {
        public List<ToastMsg> ToastMsgList = new List<ToastMsg>();


        /// <summary>
        /// Constructor for the DisplayManager stub object.  Takes the threading Dispatcher as an argument.
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <seealso cref="System.Windows.Threading.Dispatcher"/>
        public StubHubDisplayManager( Dispatcher dispatcher)
        {
            Dispatcher = dispatcher;
        }

        /// <summary>
        /// Store for the collection of available PhysicalDisplay objects
        /// </summary>
        public Collection<PhysicalDisplay> AvailableDisplays {get;set;}

        /// <summary>
        /// Field holding value for the maximum visibility time of toast messages.
        /// </summary>
        public int ToastMessageMaxVisibilityTime => throw new NotImplementedException();

        /// <summary>
        /// Field for holding Guids of DisplayIds that have active PartialBackground.
        /// </summary>
        /// 
        /// <remarks>
        /// This Field is present for completeness in making a stub for the IHubDisplayManager interface
        /// but it isn't used anywhere in plugins yet.
        /// </remarks>
        public Collection<Guid> DisplayIdsWithActivePartialBackground => throw new NotImplementedException();

        /// <summary>
        /// Property holding the Dispatcher for this object
        /// </summary>
        private Dispatcher Dispatcher { get; }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler DisconnectAllEndSession = delegate { };
        public event EventHandler<Collection<PhysicalDisplay>> DisplaysChanged = delegate { };
        public event EventHandler<DisplayView> ViewAllocated;
        public event EventHandler<Guid> ViewDeallocated;
        public event EventHandler<DisplayView> ViewUpdated;
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
            Dispatcher.BeginInvoke(new Action(delegate
            {
                var result = SuccessfulAllocateResult(hubAllocationInfo);

                var hubAllocationResult = new HubAllocationResult
                {
                    AllocatedView = result.AllocatedView,
                    Success = false,
                    ResultType = HubAllocationResultType.InternalError,
                };

                allocateCallback(result);
                ViewAllocated?.Invoke(this, result.AllocatedView);
            }));
        }

        private void SetSuccess(HubAllocationResult hubAllocationResult)
        {
            hubAllocationResult.Success = true;
            hubAllocationResult.ResultType = HubAllocationResultType.Success;
        }

        //create a new guid on allocate
        private static HubAllocationResult SuccessfulAllocateResult(HubAllocationInfo hubAllocationInfo)
        {
            //Guid test = hubAllocationInfo.Id;

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

        //don't create a new guid on deallocate
        private static HubAllocationResult SuccessfulDeallocateResult(DisplayView dv)
        {
            return new HubAllocationResult
            {
                Success = true,
                ResultType = HubAllocationResultType.Success,
                AllocatedView = new DisplayView
                {
                    Id = dv.Id,
                    AllowRemoteAnnotations = false,
                    HubAllocationInfo = new HubAllocationInfo
                    {
                        PhysicalDisplay = dv.HubAllocationInfo.PhysicalDisplay,
                        ModuleOwnerId = dv.HubAllocationInfo.ModuleOwnerId,
                        FriendlyName = dv.HubAllocationInfo.FriendlyName,
                        ViewType = dv.HubAllocationInfo.ViewType,
                        HubInfo = dv.HubAllocationInfo.HubInfo,
                        UserInfo = dv.HubAllocationInfo.UserInfo,
                        Tag = dv.HubAllocationInfo.Tag,
                    }
                }
            };
        }

        public void AllocateUiInHubDisplayAsync(UniteImage image, HubAllocationInfo hubAllocationInfo, Action<HubAllocationResult> allocateCallback)
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                var result = SuccessfulAllocateResult(hubAllocationInfo);

                var hubAllocationResult = new HubAllocationResult
                {
                    AllocatedView = result.AllocatedView,
                    Success = false,
                    ResultType = HubAllocationResultType.InternalError,
                };

                allocateCallback(result);
                ViewAllocated?.Invoke(this, result.AllocatedView);
            }));
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
            throw new NotImplementedException();
        }

        public void DeallocateUiFromHubDisplayAsync(DisplayView allocatedDisplayView, Action<HubAllocationResult> deallocateCallback)
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                //var hubAllocationResult = new HubAllocationResult
                //{
                //    AllocatedView = allocatedDisplayView,
                //    Success = false,
                //    ResultType = HubAllocationResultType.InternalError,
                //};

                var result = SuccessfulDeallocateResult(allocatedDisplayView);

                deallocateCallback(result);
                ViewDeallocated?.Invoke(this, result.AllocatedView.Id);
            }));
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
            throw new NotImplementedException();
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
            ViewUpdated?.Invoke(this, displayView);
            return true;
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

        public bool TryShowToastMessage(string text, int visibilityTime, System.Windows.Media.Imaging.BitmapImage image)
        {
            ToastMsgList.Add(new ToastMsg { Text = text, VisibilityTime = visibilityTime, Image = image});
            return true;
        }

        public bool TryShowToastMessage(string text, int visibilityTime)
        {
            ToastMsgList.Add(new ToastMsg {Text = text, VisibilityTime = visibilityTime});
            return true;
        }

        public bool UpdateAllowRemoteAnnotations(Guid displayId, bool allowAnnotations)
        {
            throw new NotImplementedException();
        }

        public void UpdateUiImage(UniteImage image, DisplayView displayView, Action<HubAllocationResult> allocateCallBack)
        {
            throw new NotImplementedException();
        }
    }

    public class ToastMsg
    {
        public string Text;
        public int VisibilityTime;
        public System.Windows.Media.Imaging.BitmapImage Image;

        protected bool Equals(ToastMsg other)
        {
            return Text == other.Text && VisibilityTime == other.VisibilityTime && Equals(Image, other.Image);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ToastMsg) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Text != null ? Text.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ VisibilityTime;
                hashCode = (hashCode * 397) ^ (Image != null ? Image.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}