using Intel.Unite.Common.Display;
using Intel.Unite.Common.Manifest;
using Intel.Unite.Common.Module.Common;
using Intel.Unite.Common.Module.Feature.Hub;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using Unite.Test.Extensions.TestFixtures;
using UnitePlugin;
using Xunit;
using UnitePluginTest.Stubs;

namespace UnitePluginTest
{
    [Collection("Ui collection")]
    public class PluginModuleHandlerTest
    {
        private readonly UiThreadFixture _uiThreadFixture;

        public PluginModuleHandlerTest(UiThreadFixture uiThreadFixture)
        {
            _uiThreadFixture = uiThreadFixture;
        }

        [Fact]
        [Trait("Category", "Initialize_Plugin")]
        public void HasConstructorForManifestTool()
        {
            PluginModuleHandler module = new PluginModuleHandler();
            Assert.IsAssignableFrom<HubFeatureModuleBase>(module);
            Assert.NotNull(module);
        }

        [Fact]
        [Trait("Category", "Initialize_Plugin")]
        public void HasValid_ModuleInfo()
        {
            PluginModuleHandler module = new PluginModuleHandler();
            Assert.NotNull(module.ModuleInfo);
            Assert.NotNull(module.ModuleInfo.Copyright);
            Assert.NotNull(module.ModuleInfo.Description);
            Assert.True(module.ModuleInfo.Id != Guid.Empty, "guid should not be emtpty");
            Assert.True(module.ModuleInfo.ModuleType == ModuleType.Feature);
            Assert.NotNull(module.ModuleInfo.Name);
            Assert.True(module.ModuleInfo.SupportedPlatforms != ModuleSupportedPlatform.Undefined, "Supported Plaforms should not be undefined");
            Assert.NotNull(module.ModuleInfo.Vendor);
            Assert.NotNull(module.ModuleInfo.Version);
        }

        [Fact]
        [Trait("Category", "Initialize_Plugin")]
        public void HasValid_ModuleManifest()
        {
            PluginModuleHandler module = new PluginModuleHandler();
            Assert.NotNull(module.ModuleManifest);
            Assert.NotNull(module.ModuleManifest.Description);
            Assert.NotNull(module.ModuleManifest.EntryPoint);
            Assert.NotEmpty(module.ModuleManifest.Files.Windows);
            //Assert.NotNull(module.ModuleManifest.Installers); Can be Null
            Assert.NotNull(module.ModuleManifest.MinimumUniteVersion);
            Assert.True(module.ModuleManifest.ModuleId != Guid.Empty, "guid should not be emtpty");
            Assert.True(module.ModuleManifest.ModuleType == ModuleType.Feature);
            Assert.NotNull(module.ModuleManifest.ModuleVersion);
            Assert.NotNull(module.ModuleManifest.Name);
            Assert.True(module.ModuleManifest.Owner == UniteModuleOwner.Hub);
            //Assert.NotNull(module.ModuleManifest.Settings); Can be Null
        }

        [Fact]
        [Trait("Category", "Initialize_Plugin")]
        public void HasSameGUID_ModuleInfo_ModuleManifest()
        {
            PluginModuleHandler module = new PluginModuleHandler();
            Assert.Equal(module.ModuleInfo.Id, module.ModuleManifest.ModuleId);
        }

        [Fact]
        [Trait("Category", "Load_Module")]
        public async Task LoadMultipleDisplays()
        {
            await _uiThreadFixture.StartStaTask(() =>
            {

                StubHubDisplayManager displayManager = new StubHubDisplayManager(Dispatcher.CurrentDispatcher);
            StubModuleLoggingManager logManager = new StubModuleLoggingManager();
            var sensormanager = new StubSensorManager();
            StubRuntimeContext context = new StubRuntimeContext
            {
                DisplayManager = displayManager,
                LogManager = logManager,
                SensorManager = sensormanager,
            };

            StubDisplayViews displayViews = new StubDisplayViews();
            displayManager.ViewAllocated += displayViews.Add;
            displayManager.AvailableDisplays = new Collection<PhysicalDisplay>
            {
                new PhysicalDisplay
                {
                    Id = new Guid{ },
                    IsPrimary = true,
                },
                new PhysicalDisplay
                {
                    Id = new Guid{ },
                    IsPrimary = true,
                },
            };

            PluginModuleHandler module = new PluginModuleHandler(context)
            {
                CurrentUiDispatcher = Dispatcher.CurrentDispatcher
            };

            module.Load();

            WaitFor(Dispatcher.CurrentDispatcher);

            var viewTypesAllocated = displayViews.DisplayViews.GroupBy(view => view.HubAllocationInfo.ViewType)
                .Select(group => group.First()).Count();
            Assert.True(displayViews.DisplayViews.Count == viewTypesAllocated * displayManager.AvailableDisplays.Count, "Views should be allocated");
            });
        }


        private static void WaitFor(Dispatcher dispatcher)
        {
            dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => { })).Wait();
        }

    }
}
