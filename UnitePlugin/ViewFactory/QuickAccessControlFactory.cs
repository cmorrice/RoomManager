using System;
using System.Windows;
using System.Windows.Threading;
using Intel.Unite.Common.Context.Hub;
using Intel.Unite.Common.Display;
using Intel.Unite.Common.Module.Common;
using UnitePlugin.Utility;

namespace UnitePlugin.ViewFactory
{
    public class QuickAccessControlFactory : HubViewFactory
    {
        public override IHubView Create(IHubModuleRuntimeContext runtimeContext, Func<FrameworkElement, MarshalNativeHandleContract> createContract, PhysicalDisplay display, Dispatcher currentUiDispatcher, EventHandler<HubViewEventArgs> eventCommandEnvoker) =>
            new QuickAccessControl(runtimeContext, createContract, display, currentUiDispatcher, eventCommandEnvoker);
    }
}
