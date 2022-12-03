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
    public class QuickAccessIcon : HubViewBase
    {
        [field: NonSerialized]
        private readonly HubDisplayViewType _HubDisplayViewType = HubDisplayViewType.QuickAccessAppIconView;

        [field: NonSerialized]
        private QuickAccessIconView _QuickAccessIconView;
        public QuickAccessIconViewModel _QuickAccessIconViewModel;

        protected override UserControl HubView => _QuickAccessIconView;
        protected override HubViewModel HubViewModel => _QuickAccessIconViewModel;
        protected override HubDisplayViewType HubDisplayViewType => _HubDisplayViewType;

        public QuickAccessIconViewModel QuickAccessIconViewModel => _QuickAccessIconViewModel;


        public QuickAccessIcon(IHubModuleRuntimeContext runtimeContext, Func<FrameworkElement, MarshalNativeHandleContract> createContract, PhysicalDisplay display, Dispatcher currentUiDispatcher, EventHandler<HubViewEventArgs> eventCommandEnvoker) 
            : base(runtimeContext, display, currentUiDispatcher, createContract)
        {
            SetQuickAccessIconView(eventCommandEnvoker);
        }

        private void SetCommandEvents(EventHandler<HubViewEventArgs> eventCommandEnvoker)
        {
            _QuickAccessIconViewModel.ShowQuickAccessControl += eventCommandEnvoker;
        }

        private void SetQuickAccessIconView(EventHandler<HubViewEventArgs> eventCommandEnvoker)
        {
            CurrentUiDispatcher.Invoke(delegate
            {
                _ViewMutex.WaitOne();
                _QuickAccessIconView = new QuickAccessIconView();
                _ViewMutex.ReleaseMutex();
                _QuickAccessIconViewModel = _QuickAccessIconView.DataContext as QuickAccessIconViewModel;
                _QuickAccessIconViewModel.ControlIdentifier = ViewGuid;
                SetCommandEvents(eventCommandEnvoker);
            });
        }
    }
}
