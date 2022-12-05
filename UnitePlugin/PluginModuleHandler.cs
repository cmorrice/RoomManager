using Intel.Unite.Common.Command;
using Intel.Unite.Common.Context;
using Intel.Unite.Common.Core;
using Intel.Unite.Common.Manifest;
using Intel.Unite.Common.Module.Common;
using Intel.Unite.Common.Module.Feature.Hub;
using System;
using System.Reflection;
using System.Windows.Threading;
using Intel.Unite.Common.Display;
using Intel.Unite.Common.Logging;
using UnitePlugin.ClientUI;
using UnitePlugin.Constants;
using UnitePlugin.Utility;
using UnitePlugin.Model.EventArguments;
using UnitePlugin.Static;
using UnitePlugin.UI;
using Q42.HueApi.Interfaces;
using Q42.HueApi;
using System.Collections.Generic;
using UnitePlugin.Hue;
using Light = UnitePlugin.Hue.Light;
using System.Threading.Tasks;

namespace UnitePlugin
{
    public class PluginModuleHandler : HubFeatureModuleBase
    {
        private string _html = @"<!DOCTYPE html><html><head><title>Error</title><script type='text/javascript'>window.onload=function(){alert();}</script></head><body onclick='alert()'><div>If you're reading this, something went wrong.</div></body></html>";
        
        


        public PluginModuleHandler() : base()
        {
        }

        public PluginModuleHandler(IModuleRuntimeContext runtimeContext) : base(runtimeContext)
        {
            // uncomment below to have debugger run
            //System.Diagnostics.Debugger.Launch();
            ConfigureModuleForClient();
        }

        private void ConfigureModuleForClient()
        {
            FeatureModuleType = FeatureModuleType.Html;
            ModuleImage = UniteImageHelper.GetUniteImageFromResource("/UnitePlugin;component/Images/menu-icon.png", UniteImageType.Png);
            _html = ClientUiSetup.GetHtml();
        }

        public override string HtmlUrlOrContent => _html;


        public override Dispatcher CurrentUiDispatcher
        {
            get => UnitePluginConfig.CurrentUiDispatcher;
            set
            {
                if (UnitePluginConfig.CurrentUiDispatcher == null)
                    UnitePluginConfig.CurrentUiDispatcher = value;
            }
        }

        public override ModuleManifest ModuleManifest => ModuleConstants.ModuleManifest;

        public override ModuleInfo ModuleInfo => ModuleConstants.ModuleInfo;

        public override void IncomingMessage(Message message)
        {
            if (!UnitePluginConfig.Messaging.IsMessageForUnitePlugin(message) &&
                !UnitePluginConfig.Messaging.IsMessageEnumDefined(message)) return;

            RuntimeContext.LogManager.LogMessage(
                ModuleInfo.Id,
                LogLevel.Debug,
                MethodBase.GetCurrentMethod().Name,
                Enum.GetName(typeof(EventArgumentTypes), message.DataType));

            MessagingEventBroker.Process(message);
        }

        public override void Load()
        {
            UnitePluginConfig.RuntimeContext = RuntimeContext;
            UnitePluginConfig.HubViewManager = new HubViewManager(RuntimeContext, CurrentUiDispatcher, CreateContract);

            UnitePluginConfig.HubViewManager.LoadandAllocateForAllDisplays(HubView.Type.QuickAccessIcon);
            UnitePluginConfig.HubViewManager.LoadandAllocateForAllDisplays(HubView.Type.QuickAccessApp);
        }



        #region Methods with no funtionality but Logs Message   
        public override bool OkToSleepDisplay()
        {
            RuntimeContext.LogManager.LogMessage(
                ModuleInfo.Id,
                LogLevel.Debug,
                MethodBase.GetCurrentMethod().Name,
                "Called");
            return true;
        }

        public override void SessionKeyChanged(KeyValuePair sessionKey)
        {
            RuntimeContext.LogManager.LogMessage(
                ModuleInfo.Id,
                LogLevel.Debug,
                MethodBase.GetCurrentMethod().Name,
                "Called");
        }

        public override void Unload()
        {
            RuntimeContext.LogManager.LogMessage(
                ModuleInfo.Id,
                LogLevel.Debug,
                MethodBase.GetCurrentMethod().Name,
                "Called");
        }

        public override void UserConnected(UserInfo userInfo)
        {
            RuntimeContext.LogManager.LogMessage(
                ModuleInfo.Id,
                LogLevel.Debug,
                MethodBase.GetCurrentMethod().Name,
                "Called");
        }

        public override void UserDisconnected(UserInfo userInfo)
        {
            RuntimeContext.LogManager.LogMessage(
                ModuleInfo.Id,
                LogLevel.Debug,
                MethodBase.GetCurrentMethod().Name,
                "Called");
        }

        public override void UserInfoChanged(UserInfo userInfo)
        {
            RuntimeContext.LogManager.LogMessage(
                ModuleInfo.Id,
                LogLevel.Debug,
                MethodBase.GetCurrentMethod().Name,
                "Called");
        }

        public override void HubConnected(HubInfo hubInfo)
        {
            RuntimeContext.LogManager.LogMessage(
                ModuleInfo.Id,
                LogLevel.Debug,
                MethodBase.GetCurrentMethod().Name,
                "Called");
        }

        public override void HubDisconnected(HubInfo hubInfo)
        {
            RuntimeContext.LogManager.LogMessage(
                ModuleInfo.Id,
                LogLevel.Debug,
                MethodBase.GetCurrentMethod().Name,
                "Called");
        }

        public override void HubInfoChanged(HubInfo hubInfo)
        {
            RuntimeContext.LogManager.LogMessage(
                ModuleInfo.Id,
                LogLevel.Debug,
                MethodBase.GetCurrentMethod().Name,
                "Called");
        }
        #endregion

    }
}
