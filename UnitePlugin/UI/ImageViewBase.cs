using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Intel.Unite.Common.Context.Hub;
using Intel.Unite.Common.Display;
using Intel.Unite.Common.Display.Hub;
using Intel.Unite.Common.Module.Common;
using UnitePlugin.Utility;

namespace UnitePlugin.UI
{
    [Serializable]
    public abstract class ImageViewBase : ViewBase
    {
        [field: NonSerialized]
        private UniteImage _image;

        [field: NonSerialized]
        private readonly UniteImageHelper _uniteImageHelper;

        protected UniteImage Image { get => _image; set => _image = value; }

        public UniteImageHelper UniteImageHelper => _uniteImageHelper;

        protected ImageViewBase(IHubModuleRuntimeContext runtimeContext, PhysicalDisplay display, Dispatcher currentUiDispatcher, Func<FrameworkElement, MarshalNativeHandleContract> createContract) : 
            base(runtimeContext, display, currentUiDispatcher, createContract)
        {
            _uniteImageHelper = new UniteImageHelper();
        }

        /// <summary>
        /// Allocation of the UniteImage view
        ///     AllocateUiInHubDisplayAsync uses UniteImage instead of a DisplayView
        /// </summary>
        public override void Allocate()
        {
            new Task(delegate ()
            {
                DisplayManagerEvent.WaitOne();
                if (IsAllocated)
                {
                    DisplayManagerEvent.Set();
                    return;
                }
                RuntimeContext.DisplayManager.AllocateUiInHubDisplayAsync(
                            this.Image,
                            HubAllocationInfo,
                            AllocatedCallBack
                            );
            }).Start();
        }

        public override void DeAllocate()
        {
            new Task(delegate ()
            {
                DisplayManagerEvent.WaitOne();
                if (!IsAllocated)
                {
                    DisplayManagerEvent.Set();
                    return;
                }
                RuntimeContext.DisplayManager.DeallocateUiFromHubDisplayAsync(
                    DisplayView,
                    DeallocateCallBack
                );
            }).Start();
        }

        public override bool Show()
        {
            if (!IsAllocated) Allocate();

            new Task(delegate ()
            {
                DisplayManagerEvent.WaitOne();
                RuntimeContext.DisplayManager.ShowAllocatedUi(DisplayView);
                DisplayManagerEvent.Set();
            }).Start();

            return true;
        }

        public override void AllocatedCallBack(HubAllocationResult hubAllocationResult)
        {
            new Task(delegate ()
            {
                if (hubAllocationResult.Success)
                {
                    AllocatedSuccess(hubAllocationResult.AllocatedView);
                }
                else
                {
                    AllocatedFail();
                    DisplayManagerEvent.Set();
                    throw new Exception(hubAllocationResult.ResultType.ToString());
                }
                DisplayManagerEvent.Set();
            }).Start();
        }

        public override bool AllocatedFail()
        {
            lock (this)
            {
                IsAllocated = false;
            }

            return true;
        }

        public override void AllocatedSuccess(DisplayView allocatedView)
        {
            lock (this)
            {
                DeepCopy.CopyDisplayView(DisplayView, allocatedView);
                IsAllocated = true;
            }
        }

        public override void DeallocateCallBack(HubAllocationResult hubAllocationResult)
        {
            new Task(delegate ()
            {
                if (hubAllocationResult.Success)
                {
                    lock (this)
                    {
                        SetDeallocate();
                    }
                }
                else
                {
                    DisplayManagerEvent.Set();
                    throw new Exception(hubAllocationResult.ResultType.ToString());
                }

                DisplayManagerEvent.Set();
            }).Start();
        }

        private void SetDeallocate()
        {
            IsAllocated = false;
        }
    }
}
