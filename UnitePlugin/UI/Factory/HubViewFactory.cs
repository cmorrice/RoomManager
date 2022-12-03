using System;
using System.Windows;
using System.Windows.Threading;
using Intel.Unite.Common.Context.Hub;
using Intel.Unite.Common.Display;
using Intel.Unite.Common.Module.Common;
using UnitePlugin.Interfaces;
using UnitePlugin.Model.EventArguments;

namespace UnitePlugin.UI.Factory
{
    public abstract class HubViewFactory
    {
        public abstract IHubView Create(IHubModuleRuntimeContext runtimeContext, Func<FrameworkElement, MarshalNativeHandleContract> createContract, PhysicalDisplay display, Dispatcher currentUiDispatcher, EventHandler<HubViewEventArgs> eventCommandInvoker);
    }
}