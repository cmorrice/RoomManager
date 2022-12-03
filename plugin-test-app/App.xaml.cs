using Intel.Unite.Common.Module.Feature.Hub;
using System.Windows;
using UnitePlugin;
using UnitePluginTestApp.Manager;
using UnitePluginTestApp.ViewModel;

namespace UnitePluginTestApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = new MainWindow();

            ((HubModuleRuntimeContext) mainWindow.DataContext).DisplayManager = new HubDisplayManager();
            ((HubModuleRuntimeContext) mainWindow.DataContext).LogManager = new HubLogManager();
            ((HubModuleRuntimeContext) mainWindow.DataContext).SessionContext = new HubSessionContext();
            ((HubModuleRuntimeContext) mainWindow.DataContext).ConfigurationManager = new ConfigurationManager();
            ((HubModuleRuntimeContext) mainWindow.DataContext).SensorManager = new SensorManager();

            var messageSender = new MessageSender();
            ((HubModuleRuntimeContext)mainWindow.DataContext).MessageSender = messageSender;

            HubFeatureModuleBase module = new PluginModuleHandler((HubModuleRuntimeContext) mainWindow.DataContext)
            {
                CurrentUiDispatcher = Current.Dispatcher
            };

            messageSender.OnMessage = module.IncomingMessage;

            mainWindow.Show();
            module.Load();



        }
    }
}
