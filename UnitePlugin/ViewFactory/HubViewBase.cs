using System;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Intel.Unite.Common.Context.Hub;
using Intel.Unite.Common.Display;
using Intel.Unite.Common.Display.Hub;
using Intel.Unite.Common.Module.Common;
using UnitePlugin.Constants;
using UnitePlugin.Utility;
using UnitePlugin.ViewModel;
using System.Runtime.Remoting.Proxies;

namespace UnitePlugin.ViewFactory
{
    [Serializable]
    public class HubViewBase : IHubView
    {
        public readonly IHubModuleRuntimeContext _RuntimeContext;

        [field: NonSerialized]
        private readonly Dispatcher _CurrentUiDispatcher;
        public Dispatcher CurrentUiDispatcher { get { return _CurrentUiDispatcher; } }

        private readonly Guid _ViewGuid;
        public Guid ViewGuid => _ViewGuid;

        private DisplayView _DisplayView = new DisplayView { HubAllocationInfo = new HubAllocationInfo { PhysicalDisplay = new PhysicalDisplay() } };
        public DisplayView DisplayView { get { return _DisplayView; } set { _DisplayView = value; } }

        private readonly HubAllocationInfo _HubAllocationInfo;
        public HubAllocationInfo HubAllocationInfo { get { return _HubAllocationInfo; } }

        [field: NonSerialized]
        private readonly Func<FrameworkElement, MarshalNativeHandleContract> _CreateContract;

        [field: NonSerialized]
        protected static readonly Mutex _ViewMutex = new Mutex();

        [field: NonSerialized]
        protected virtual UserControl HubView { get; }
        protected virtual HubViewModel HubViewModel { get; }
        protected virtual HubDisplayViewType HubDisplayViewType { get; }

        public HubViewBase(IHubModuleRuntimeContext runtimeContext, PhysicalDisplay display, Dispatcher currentUiDispatcher, Func<FrameworkElement, MarshalNativeHandleContract> createContract)
        {
            lock (this)
            {
                _ViewGuid = Guid.NewGuid();
                _RuntimeContext = runtimeContext;
                _CurrentUiDispatcher = currentUiDispatcher;
                _CreateContract = createContract;
                _HubAllocationInfo = GetNewHubAllocationInfo(display);
            }

            LogTrace(MethodBase.GetCurrentMethod() + ": " + ObjectExtensions.PropertyList(this));
        }

        private HubAllocationInfo GetNewHubAllocationInfo(PhysicalDisplay display)
        {
            LogTrace(MethodBase.GetCurrentMethod() + ": " + ObjectExtensions.PropertyList(this));

            return new HubAllocationInfo
            {
                FriendlyName = this.GetType().Name,
                ModuleOwnerId = ModuleConstants.ModuleInfo.Id,
                PhysicalDisplay = display,
                ViewType = HubDisplayViewType,
                Tag = ViewGuid,
            };
        }
        public void Allocate()
        {
            _ViewMutex.WaitOne();
            MarshalNativeHandleContract contract = _CreateContract(HubView);
            _ViewMutex.ReleaseMutex();

            _RuntimeContext.DisplayManager.AllocateUiInHubDisplayAsync(
                        contract,
                        _HubAllocationInfo,
                        AllocatedCallBack
                        );
            
            LogTrace(MethodBase.GetCurrentMethod() + ": " + ObjectExtensions.PropertyList(this));
        }

        public void DeAllocate()
        {
            _RuntimeContext.DisplayManager.DeallocateUiFromHubDisplayAsync(
                _DisplayView,
                DeallocatedCallBack
                );

            LogTrace(MethodBase.GetCurrentMethod() + ": " + ObjectExtensions.PropertyList(this));
        }

        public bool Show()
        {
            if (!IsAllocated) Allocate();
            CurrentUiDispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => { })).Wait();
            //Note: because of how the proxy works I haven't been able to figure out a way to set a boolean flag

            LogTrace(MethodBase.GetCurrentMethod() + ": " + ObjectExtensions.PropertyList(this));
            return IsAllocated ? _RuntimeContext.DisplayManager.ShowAllocatedUi(DisplayView) : false;
        }

        public bool IsAllocated => _DisplayView?.HubAllocationInfo?.Tag != null;

        public void AllocatedCallBack(HubAllocationResult hubAllocationResult)
        {
            if (hubAllocationResult.Success)
            {
                AllocatedSuccess(hubAllocationResult.AllocatedView);
            }
            else
            {
                AllocatedFail();
                throw new Exception(hubAllocationResult.ResultType.ToString());
            }

            LogTrace(MethodBase.GetCurrentMethod() + ": " + ObjectExtensions.PropertyList(this));
        }

        public void AllocatedFail()
        {
            lock(this)
            {
            }
        }

        public void AllocatedSuccess(DisplayView allocatedView)
        {
            lock (this)
            {
                DeepCopy.CopyDisplayView(_DisplayView, allocatedView);
            }
        }

        public void DeallocatedCallBack(HubAllocationResult hubAllocationResult)
        {
            LogTrace(MethodBase.GetCurrentMethod() + ": " + ObjectExtensions.PropertyList(this));
            if (hubAllocationResult.Success)
            {
                lock (this)
                {
                }
            }
            else
            {
                throw new Exception(hubAllocationResult.ResultType.ToString());
            }
            LogTrace(MethodBase.GetCurrentMethod() + ": " + ObjectExtensions.PropertyList(this));
        }

        private void LogTrace(string message)
        {
            _RuntimeContext.LogManager.LogMessage(
                ModuleConstants.ModuleInfo.Id, 
                Intel.Unite.Common.Logging.LogLevel.Trace, 
                this.GetType().Name,
                message + Environment.NewLine + this.GetHashCode());
        }
    }
}
