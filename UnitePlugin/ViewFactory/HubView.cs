using Intel.Unite.Common.Context.Hub;
using Intel.Unite.Common.Display;
using Intel.Unite.Common.Module.Common;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using UnitePlugin.Utility;

namespace UnitePlugin.ViewFactory
{
    public class HubView
    {
        private readonly Dictionary<Type, HubViewFactory> _factories;

        public enum Type
        {
            QuickAccessIcon,
            QuickAccessControl,
        }

        public HubView()
        {
            _factories = new Dictionary<Type, HubViewFactory>();

            foreach (Type hubViewType in Enum.GetValues(typeof(Type)))
            {
                var factory = (HubViewFactory)Activator.CreateInstance(System.Type.GetType("UnitePlugin.ViewFactory." + Enum.GetName(typeof(Type), hubViewType) + "Factory"));
                _factories.Add(hubViewType, factory);
            }
        }

        public IHubView ExecuteCreation(
            Type hubViewType, 
            IHubModuleRuntimeContext runtimeContext, 
            Func<FrameworkElement, MarshalNativeHandleContract> createContract, 
            PhysicalDisplay display, 
            Dispatcher currentUiDispatcher,
             EventHandler<HubViewEventArgs> eventCommandEnvoker) => 
            _factories[hubViewType].Create(runtimeContext, createContract, display, currentUiDispatcher, eventCommandEnvoker);

    }
}
