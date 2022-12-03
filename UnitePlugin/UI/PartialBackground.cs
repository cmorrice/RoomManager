using Intel.Unite.Common.Context.Hub;
using Intel.Unite.Common.Display;
using Intel.Unite.Common.Display.Hub;
using Intel.Unite.Common.Module.Common;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using UnitePlugin.Model.EventArguments;
using UnitePlugin.View;
using UnitePlugin.ViewModel;

namespace UnitePlugin.UI
{
    [Serializable]
    public class PartialBackground : HubViewBase
    {
        [field: NonSerialized]
        private readonly HubDisplayViewType _hubDisplayViewType = HubDisplayViewType.PartialBackgroundView;

        [field: NonSerialized]
        private PartialBackgroundView _partialBackgroundView;
        private PartialBackgroundViewModel _partialBackgroundViewModel;

        protected override UserControl HubView => _partialBackgroundView;
        protected override HubViewModel HubViewModel => _partialBackgroundViewModel;
        public override void SetDeallocate()
        {
            IsAllocated = false;
            SetPartialBackgroundView(); //resets view
        }

        protected override HubDisplayViewType HubDisplayViewType => _hubDisplayViewType;

        public PartialBackgroundViewModel PartialBackgroundViewModel => _partialBackgroundViewModel;


        public PartialBackground(IHubModuleRuntimeContext runtimeContext, Func<FrameworkElement, MarshalNativeHandleContract> createContract, PhysicalDisplay display, Dispatcher currentUiDispatcher, EventHandler<HubViewEventArgs> eventCommandInvoker)
            : base(runtimeContext, display, currentUiDispatcher, createContract)
        {
            SetPartialBackgroundView();
        }


        private void SetPartialBackgroundView()
        {
            CurrentUiDispatcher.Invoke(delegate
            {
                _partialBackgroundView = new PartialBackgroundView();

                _partialBackgroundViewModel = _partialBackgroundView.DataContext as PartialBackgroundViewModel;
                _partialBackgroundViewModel.ControlIdentifier = ViewGuid;
            });
        }
    }
}
