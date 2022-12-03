using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using Intel.Unite.Common.Context.Hub;
using Intel.Unite.Common.Display;
using Intel.Unite.Common.Module.Common;
using UnitePlugin.Interfaces;
using UnitePlugin.Model.EventArguments;
using UnitePlugin.UI.Factory;

namespace UnitePlugin.UI
{
    public class HubView
    {
        private readonly Dictionary<Type, HubViewFactory> _factories;

        public enum Type
        {
            QuickAccessIcon,
            AuthImage,
            StatusImage,
            Background,
            Presentation,
            //PresentationRibbon,
            PartialBackground,
            QuickAccessApp,
        }

        public HubView()
        {
            _factories = new Dictionary<Type, HubViewFactory>();

            foreach (Type hubViewType in Enum.GetValues(typeof(Type)))
            {
                var factory = (HubViewFactory)Activator.CreateInstance(System.Type.GetType("UnitePlugin.UI.Factory." + Enum.GetName(typeof(Type), hubViewType) + "Factory") ?? throw new InvalidOperationException());
                _factories.Add(hubViewType, factory);
            }
        }

        public IHubView ExecuteCreation(
            Type hubViewType,
            IHubModuleRuntimeContext runtimeContext,
            Func<FrameworkElement, MarshalNativeHandleContract> createContract,
            PhysicalDisplay display,
            Dispatcher currentUiDispatcher,
            EventHandler<HubViewEventArgs> eventCommandInvoker) =>
            _factories[hubViewType].Create(runtimeContext, createContract, display, currentUiDispatcher, eventCommandInvoker);
    }
}
