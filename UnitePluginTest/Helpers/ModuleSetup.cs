using Intel.Unite.Common.Display;
using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using UnitePlugin.Static;
using UnitePluginTest.Stubs;

namespace UnitePluginTest.Helpers
{
    //TODO create stub of ViewManager so it doesn't use the UI thread
    /// <summary>
    /// Performs setup of runtime resources
    /// </summary>
    public class ModuleSetup
    {
        public void SetupRuntimeContext(
            out StubHubDisplayManager displayManager,
            out StubRuntimeContext context,
            out StubDisplayViews displayViews,
            out StubSensorManager sensorManager,
            out Dispatcher dispatcher)
        {
            sensorManager = new StubSensorManager();
            dispatcher = Dispatcher.CurrentDispatcher;

            StubContractCreator.SetUpDispatcher(dispatcher);

            displayManager = new StubHubDisplayManager(dispatcher);
            var logManager = new StubModuleLoggingManager();
            var messageSender = new StubMessageSender();
            context = new StubRuntimeContext
            {
                DisplayManager = displayManager,
                LogManager = logManager,
                MessageSender = messageSender,
                SensorManager = sensorManager,
            };
            displayViews = new StubDisplayViews();
            displayManager.ViewAllocated += displayViews.Add;
            displayManager.ViewDeallocated += displayViews.Remove;
            displayManager.AvailableDisplays = new Collection<PhysicalDisplay>
            {
                new PhysicalDisplay
                {
                    Id = Guid.NewGuid(),
                    IsPrimary = true,
                },
                new PhysicalDisplay
                {
                    Id = Guid.NewGuid(),
                    IsPrimary = true,
                },
            };

            UnitePluginConfig.RuntimeContext = context;
        }
    }
}
