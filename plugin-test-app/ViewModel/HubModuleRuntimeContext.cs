using Intel.Unite.Common.Calendar;
using Intel.Unite.Common.Command;
using Intel.Unite.Common.Configuration;
using Intel.Unite.Common.Context;
using Intel.Unite.Common.Context.Hub;
using Intel.Unite.Common.Display.Hub;
using Intel.Unite.Common.Kpi;
using Intel.Unite.Common.Logging;
using Intel.Unite.Common.Module.Common.ErrorHandling;
using Intel.Unite.Common.Sensor;
using Intel.Unite.Common.Telemetry;
using System;
using System.Windows;
using System.Windows.Input;
using UnitePluginTestApp.Manager;
using UnitePluginTestApp.Utility;

namespace UnitePluginTestApp.ViewModel
{
    /// <summary>
    /// This class exposes the runtime context for the Hub Plugin components.
    /// </summary>
    [Serializable]
    public class HubModuleRuntimeContext : ModuleRuntimeContext, IHubModuleRuntimeContext
    {
        #region Private Fields

        private static readonly object _sync = new object();
        private static HubModuleRuntimeContext _instance;
        private static IHubSessionContext _sessionContext;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the singleton instance for HubPluginRuntimeContext class.
        /// </summary>
        public new static HubModuleRuntimeContext Instance
        {
            get
            {
                lock (_sync)
                {
                    if (_instance == null)
                    {
                        _instance = new HubModuleRuntimeContext();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Gets or sets the Hub runtime instance of IConfigurationManager.
        /// </summary>
        public new IConfigurationManager ConfigurationManager { get; set; }

        /// <summary>
        /// Gets or sets the Hub runtime instance of IMessageSender.
        /// </summary>
        public new IMessageSender MessageSender { get; set; }

        /// <summary>
        /// Gets or sets the Hub runtime instance of IHubDisplayManager.
        /// </summary>
        public new IHubDisplayManager DisplayManager { get; set; }

        /// <summary>
        /// Gets or sets the Hub runtime instance of ILog.
        /// </summary>
        public new IModuleLoggingManager LogManager { get; set; }

        /// <summary>
        /// Gets or sets the Hub runtime instance of ITelemetry.
        /// </summary>
        public new IModuleTelemetry TelemetryManager { get; set; }

        /// <summary>
        /// Gets or sets the Hub runtime instance of IKpiManager.
        /// </summary>
        public new IKpiManager KpiManager { get; set; }

        /// <summary>
        /// Gets or sets the Hub runtime instance of ICalendarManager.
        /// </summary>
        public new ICalendarManager CalendarManager { get; set; }

        /// <summary>
        /// Gets or sets the Hub runtime instance of ISessionContext.
        /// </summary>
        public new IHubSessionContext SessionContext
        {
            get
            {
                if(_sessionContext == null)
                {
                    lock(_sync)
                    {
                        _sessionContext = new HubSessionContext();
                    }
                }

                return _sessionContext;
            }
            set => _sessionContext = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public new ModuleErrorHandlingService ModuleErrorHandling
        {
            get => _moduleErrorHandling;
            set => _moduleErrorHandling = value;
        }

        #endregion

        #region button commands

        [field: NonSerialized]
        private EventHandler _showQuickAccessLayer;
        public event EventHandler ShowQuickAccessLayer
        {
            add => _showQuickAccessLayer += value;
            remove => _showQuickAccessLayer -= value;
        }
        private ICommand _showQuickAccessLayer_ClickCommand;
        [field: NonSerialized]
        private ModuleErrorHandlingService _moduleErrorHandling;

        public ICommand ShowQuickAccessLayerClickCommand
        {
            get
            {
                return _showQuickAccessLayer_ClickCommand ?? (_showQuickAccessLayer_ClickCommand = new RelayCommand(
                    x =>
                    {
                        ShowQuickAccessLayer_Click(this, null);
                    }));
            }
        }

        public ISensorManager SensorManager { get; set; }

        public void ShowQuickAccessLayer_Click(object sender, RoutedEventArgs e)
        {
            //ShowQuickAccessLayer?.Invoke(this, e);
            var hubDisplayManager = (HubDisplayManager)DisplayManager;
            hubDisplayManager.ShowQuickAccessLayer();
        }
        #endregion
    }
}