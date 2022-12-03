using Intel.Unite.Common.Context.Hub;
using Intel.Unite.Common.Display;
using Intel.Unite.Common.Display.Hub;
using Intel.Unite.Common.Module.Common;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using UnitePlugin.Utility;
using UnitePlugin.View;
using UnitePlugin.ViewModel;

namespace UnitePlugin.ViewFactory
{
    [Serializable]
    public class QuickAccessControl : HubViewBase
    {
        [field: NonSerialized]
        private readonly HubDisplayViewType _HubDisplayViewType = HubDisplayViewType.QuickAccessAppView;

        [field: NonSerialized]
        private QuickAccessControlView _QuickAccessControlView;
        public QuickAccessControlViewModel _QuickAccessControlViewModel;

        protected override UserControl HubView => _QuickAccessControlView;
        protected override HubViewModel HubViewModel => _QuickAccessControlViewModel;
        protected override HubDisplayViewType HubDisplayViewType => _HubDisplayViewType;

        public QuickAccessControlViewModel QuickAccessControlViewModel => _QuickAccessControlViewModel;


        public QuickAccessControl(IHubModuleRuntimeContext runtimeContext, Func<FrameworkElement, MarshalNativeHandleContract> createContract, PhysicalDisplay display, Dispatcher currentUiDispatcher, EventHandler<HubViewEventArgs> eventCommandEnvoker)
            : base(runtimeContext, display, currentUiDispatcher, createContract)
        {
            SetQuickAccessControlView(eventCommandEnvoker);
        }

        private void SetCommandEvents(EventHandler<HubViewEventArgs> eventCommandEnvoker)
        {
            //_QuickAccessControlViewModel.ShowQuickAccessControl += eventCommandEnvoker;
        }

        private void SetQuickAccessControlView(EventHandler<HubViewEventArgs> eventCommandEnvoker)
        {
            CurrentUiDispatcher.Invoke(delegate
            {
                _ViewMutex.WaitOne();
                _QuickAccessControlView = new QuickAccessControlView();
                _ViewMutex.ReleaseMutex();
                _QuickAccessControlViewModel = _QuickAccessControlView.DataContext as QuickAccessControlViewModel;
                _QuickAccessControlViewModel.ControlIdentifier = ViewGuid;
                SetCommandEvents(eventCommandEnvoker);
            });
        }
    }
}
