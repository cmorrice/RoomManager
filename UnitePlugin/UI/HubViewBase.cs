using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Intel.Unite.Common.Context.Hub;
using Intel.Unite.Common.Display;
using Intel.Unite.Common.Display.Hub;
using Intel.Unite.Common.Module.Common;
using UnitePlugin.Static;
using UnitePlugin.Utility;
using UnitePlugin.ViewModel;

namespace UnitePlugin.UI
{
    [Serializable]
    public abstract class HubViewBase : ViewBase
    {
        [field: NonSerialized]
        private UserControl _hubView;
        
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        protected virtual UserControl HubView { get => _hubView; set => _hubView = value; }
        
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        protected virtual HubViewModel HubViewModel { get; set; }


        protected HubViewBase(IHubModuleRuntimeContext runtimeContext, PhysicalDisplay display, Dispatcher currentUiDispatcher, Func<FrameworkElement, MarshalNativeHandleContract> createContract) :
            base(runtimeContext, display, currentUiDispatcher, createContract)
        { }


        public override void Allocate()
        {
            new Task(delegate() 
            {
                DisplayManagerEvent.WaitOne();
                if (IsAllocated)
                {
                    DisplayManagerEvent.Set();
                    return;
                }
                UnitePluginConfig.Contract = CreateContract(HubView); 
                
                RuntimeContext.DisplayManager.AllocateUiInHubDisplayAsync(
                        UnitePluginConfig.Contract,
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
        
        // All CallBacks must be public
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
        
        // Must be public
        public override bool AllocatedFail()
        {
            lock (this)
            {
                IsAllocated = false;
            }

            return true;
        }
        
        // Must be public
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

        public abstract void SetDeallocate();
    }
}
