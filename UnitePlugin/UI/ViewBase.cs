using Intel.Unite.Common.Context.Hub;
using Intel.Unite.Common.Display;
using Intel.Unite.Common.Display.Hub;
using Intel.Unite.Common.Module.Common;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using UnitePlugin.Constants;
using UnitePlugin.Interfaces;

namespace UnitePlugin.UI
{
    [Serializable]
    public abstract class ViewBase : MarshalByRefObjectBase, IHubView
    {

        #region Fields
        [field: NonSerialized]
        private Dispatcher _dispatcher;

        [field: NonSerialized]
        private Func<FrameworkElement, MarshalNativeHandleContract> _createContext;

        [field: NonSerialized]
        private AutoResetEvent _displayManagerCallback = new AutoResetEvent(true);
        #endregion


        #region Properties

        protected Dispatcher CurrentUiDispatcher { get => _dispatcher; set => _dispatcher = value; }

        public Guid ViewGuid { get; set; }

        public Func<FrameworkElement, MarshalNativeHandleContract> CreateContract { get => _createContext; set => _createContext = value; }

        public bool IsAllocated { get; set; } = false;

        public DisplayView DisplayView { get; set; } = new DisplayView { HubAllocationInfo = new HubAllocationInfo { PhysicalDisplay = new PhysicalDisplay() } };

        public HubAllocationInfo HubAllocationInfo { get; set; }

        protected virtual HubDisplayViewType HubDisplayViewType { get; set; }

        protected IHubModuleRuntimeContext RuntimeContext { get; set; }

        protected AutoResetEvent DisplayManagerEvent { get => _displayManagerCallback; }

        #endregion


        public ViewBase(IHubModuleRuntimeContext runtimeContext, PhysicalDisplay display, Dispatcher currentUiDispatcher, Func<FrameworkElement, MarshalNativeHandleContract> createContract)
        {
            lock (this)
            {
                ViewGuid = Guid.NewGuid();
                RuntimeContext = runtimeContext;
                CurrentUiDispatcher = currentUiDispatcher;
                HubAllocationInfo = GetNewHubAllocationInfo(display);
                CreateContract = createContract;
            }
        }


        public abstract void Allocate();

        public abstract void DeAllocate();

        public abstract bool Show();

        public abstract void AllocatedCallBack(HubAllocationResult hubAllocationResult);

        public abstract bool AllocatedFail();

        public abstract void AllocatedSuccess(DisplayView allocatedView);

        public abstract void DeallocateCallBack(HubAllocationResult hubAllocationResult);

        public HubAllocationInfo GetNewHubAllocationInfo(PhysicalDisplay display)
        {
            return new HubAllocationInfo
            {
                FriendlyName = GetType().Name,
                ModuleOwnerId = ModuleConstants.ModuleInfo.Id,
                PhysicalDisplay = display,
                ViewType = HubDisplayViewType,
                Tag = ViewGuid,
            };
        }
    }
}
