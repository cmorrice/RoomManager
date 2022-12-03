using Intel.Unite.Common.Command;
using Intel.Unite.Common.Context.Hub;
using Intel.Unite.Common.Logging;
using Intel.Unite.Common.Module.Common;
using System;
using System.Windows.Threading;
using UnitePlugin.Constants;
using UnitePlugin.Interfaces;
using UnitePlugin.Model.EventArguments;
using UnitePlugin.Sensors;
using UnitePlugin.Utility;
using UnitePlugin.ViewModel.Controls;
using UnitePlugin.ViewModel.Factory;

namespace UnitePlugin.Static
{
    public static class UnitePluginConfig
    {
        /// <summary>
        /// Static instance of the current UI Dispatcher
        /// </summary>
        public static Dispatcher CurrentUiDispatcher { get; set; }

        /// <summary>
        /// Method instance of the HubRuntimeContext
        /// </summary>
        public static IHubModuleRuntimeContext RuntimeContext { get; set; }

        public static MarshalNativeHandleContract Contract { get; set; }
        public static PluginSensorManager PluginSensorManager { get; internal set; }
        public static IHubViewManager HubViewManager { get; set; }

        public static void SetHubViewManager(IHubViewManager hubViewManager)
        {
            HubViewManager = hubViewManager;
        }
        
        public static class Messaging
        {
            public static bool IsMessageEnumDefined(Message message)
            {
                return typeof(EventArgumentTypes).IsEnumDefined(message.DataType);
            }

            public static bool IsMessageFromClientUnitePlugin(Message message)
            {
                return IsMessageFromUnitePlugin(message) && message.SourceId != RuntimeContext.SessionContext.MyHubInfo.Id;
            }


            public static bool IsMessageFromHubUnitePlugin(Message message)
            {
                return IsMessageFromUnitePlugin(message) && message.SourceId == RuntimeContext.SessionContext.MyHubInfo.Id;
            }

            public static bool IsMessageFromUnitePlugin(Message message)
            {
                return message.SourceModuleId == ModuleConstants.ModuleInfo.Id;
            }

            public static bool IsMessageForUnitePlugin(Message message)
            {
                return message.TargetId == ModuleConstants.ModuleInfo.Id;
            }
        }
    }
}
