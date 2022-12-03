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
    public class Background : HubViewBase
    {
        [field: NonSerialized]
        private readonly HubDisplayViewType _hubDisplayViewType = HubDisplayViewType.BackgroundView;

        [field: NonSerialized]
        private BackgroundView _backgroundView;
        private BackgroundViewModel _backgroundViewModel;

        protected override UserControl HubView => _backgroundView;
        protected override HubViewModel HubViewModel => _backgroundViewModel;
        public override void SetDeallocate()
        {
            IsAllocated = false;
            SetBackgroundView();
        }

        protected override HubDisplayViewType HubDisplayViewType => _hubDisplayViewType;

        public BackgroundViewModel BackgroundViewModel => _backgroundViewModel;


        public Background(IHubModuleRuntimeContext runtimeContext, Func<FrameworkElement, MarshalNativeHandleContract> createContract, PhysicalDisplay display, Dispatcher currentUiDispatcher, EventHandler<HubViewEventArgs> eventCommandInvoker)
            : base(runtimeContext, display, currentUiDispatcher, createContract)
        {
            SetBackgroundView();
        }


        private void SetBackgroundView()
        {
            CurrentUiDispatcher.Invoke(delegate
            {
                _backgroundView = new BackgroundView();

                _backgroundViewModel = _backgroundView.DataContext as BackgroundViewModel;
                _backgroundViewModel.ControlIdentifier = ViewGuid;
            });
        }
    }
}
