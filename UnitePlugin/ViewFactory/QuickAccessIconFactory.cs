using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Intel.Unite.Common.Context.Hub;
using Intel.Unite.Common.Display;
using Intel.Unite.Common.Module.Common;
using UnitePlugin.Constants;
using UnitePlugin.Utility;

namespace UnitePlugin.ViewFactory
{
    public class QuickAccessIconFactory : HubViewFactory
    {
        public override IHubView Create(IHubModuleRuntimeContext runtimeContext, Func<FrameworkElement, MarshalNativeHandleContract> createContract, PhysicalDisplay display, Dispatcher currentUiDispatcher, EventHandler<HubViewEventArgs> eventCommandEnvoker)
        {
            runtimeContext.LogManager.LogMessage(
                ModuleConstants.ModuleInfo.Id,
                Intel.Unite.Common.Logging.LogLevel.Trace,
                this.GetType().Name,
                MethodBase.GetCurrentMethod() + Environment.NewLine + this.GetHashCode());

            return new QuickAccessIcon(runtimeContext, createContract, display, currentUiDispatcher, eventCommandEnvoker);
        }
         
    }
}
