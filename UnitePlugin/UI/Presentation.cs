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
using UnitePlugin.ViewModel.Controls;

namespace UnitePlugin.UI
{
    [Serializable]
    public class Presentation : HubViewBase
    {
        [field: NonSerialized]
        private readonly HubDisplayViewType _hubDisplayViewType = HubDisplayViewType.PresentationView;

        [field: NonSerialized]
        private PresentationView _presentationView;
        private PresentationViewModel _presentationViewModel;

        protected override UserControl HubView => _presentationView;
        protected override HubViewModel HubViewModel => _presentationViewModel;
        public override void SetDeallocate()
        {
            IsAllocated = false;
            SetPresentationView();
        }

        protected override HubDisplayViewType HubDisplayViewType => _hubDisplayViewType;

        public PresentationViewModel PresentationViewModel => _presentationViewModel;


        public Presentation(IHubModuleRuntimeContext runtimeContext, Func<FrameworkElement, MarshalNativeHandleContract> createContract, PhysicalDisplay display, Dispatcher currentUiDispatcher, EventHandler<HubViewEventArgs> eventCommandInvoker)
            : base(runtimeContext, display, currentUiDispatcher, createContract)
        {
            SetPresentationView();
            //(_presentationView.AllControlsView.PresentationControlView.DataContext as PresentationControlViewModel).Newttt = eventCommandInvoker;
        }


        private void SetPresentationView()
        {
            CurrentUiDispatcher.Invoke(delegate
            {
                _presentationView = new PresentationView();

                _presentationViewModel = _presentationView.DataContext as PresentationViewModel;
                _presentationViewModel.ControlIdentifier = ViewGuid;
            });
        }





    }
}
