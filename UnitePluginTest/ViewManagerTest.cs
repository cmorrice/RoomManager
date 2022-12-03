using System;
using System.Linq;
using System.Windows.Threading;
using UnitePlugin.Utility;
using UnitePlugin.ViewFactory;
using UnitePluginTest.Stubs;
using Xunit;
using UnitePluginTest.Helpers;

namespace UnitePluginTest
{
    public class ViewManagerTest
    {
        private static void LoadAllViewsForAllDisplays(StubHubDisplayManager displayManager, StubRuntimeContext context, Dispatcher dispatcher, HubViewManager hubViewManager)
        {
            foreach (HubView.Type hubViewType in Enum.GetValues(typeof(HubView.Type)))
            {
                displayManager.AvailableDisplays.ToList().ForEach(display =>
                    hubViewManager.LoadHubView(hubViewType, context, StubContractCreator.CreateContract, display, dispatcher)
                );
            }
        }

        private static HubView.Type GetRandomHubViewType()
        {
            Array values = Enum.GetValues(typeof(HubView.Type));
            Random random = new Random();
            HubView.Type randomHubViewType = (HubView.Type)values.GetValue(random.Next(values.Length));
            return randomHubViewType;
        }


        [Fact]
        [Trait("Category", "ViewManagerTest")]
        public void Constructor()
        {
            ModuleSetup.SetupRuntimeContext(out StubHubDisplayManager displayManager, out StubRuntimeContext context, out StubDisplayViews displayViews, out Dispatcher dispatcher);
            HubViewManager hubViewManager = new HubViewManager(context, dispatcher, StubContractCreator.CreateContract);
            Assert.NotNull(hubViewManager);
        }

        [StaFact]
        [Trait("Category", "ViewManagerTest")]
        public void LoadView()
        {
            ModuleSetup.SetupRuntimeContext(out StubHubDisplayManager displayManager, out StubRuntimeContext context, out StubDisplayViews displayViews, out Dispatcher dispatcher);
            HubViewManager hubViewManager = new HubViewManager(context, dispatcher, StubContractCreator.CreateContract);


            LoadAllViewsForAllDisplays(displayManager, context, dispatcher, hubViewManager);

            Assert.Equal(displayManager.AvailableDisplays.Count * Enum.GetValues(typeof(HubView.Type)).Length, hubViewManager.HubViews.Count);
        }

        [StaFact]
        [Trait("Category", "ViewManagerTest")]
        public void AllocateView()
        {
            ModuleSetup.SetupRuntimeContext(out StubHubDisplayManager displayManager, out StubRuntimeContext context, out StubDisplayViews displayViews, out Dispatcher dispatcher);
            HubViewManager hubViewManager = new HubViewManager(context, dispatcher, StubContractCreator.CreateContract);

            LoadAllViewsForAllDisplays(displayManager, context, dispatcher, hubViewManager);

            hubViewManager.Allocate(GetRandomHubViewType());
            WaitFor(dispatcher);

            Assert.Equal(displayManager.AvailableDisplays.Count, displayViews.displayViews.Count);
        }

        [StaFact]
        [Trait("Category", "ViewManagerTest")]
        public void ShowView()
        {
            ModuleSetup.SetupRuntimeContext(out StubHubDisplayManager displayManager, out StubRuntimeContext context, out StubDisplayViews displayViews, out Dispatcher dispatcher);
            HubViewManager hubViewManager = new HubViewManager(context, dispatcher, StubContractCreator.CreateContract);

            LoadAllViewsForAllDisplays(displayManager, context, dispatcher, hubViewManager);

            hubViewManager.Allocate(GetRandomHubViewType());
            WaitFor(dispatcher);

            displayViews.displayViews.ForEach(view => Assert.True(hubViewManager.Show((Guid)view.HubAllocationInfo.Tag)));
            WaitFor(dispatcher);

            Assert.Equal(displayManager.AvailableDisplays.Count, displayViews.displayViews.Count);
        }

        [StaFact]
        [Trait("Category", "ViewManagerTest")]
        public void DeAllocateView()
        {
            ModuleSetup.SetupRuntimeContext(out StubHubDisplayManager displayManager, out StubRuntimeContext context, out StubDisplayViews displayViews, out Dispatcher dispatcher);
            HubViewManager hubViewManager = new HubViewManager(context, dispatcher, StubContractCreator.CreateContract);

            LoadAllViewsForAllDisplays(displayManager, context, dispatcher, hubViewManager);

            HubView.Type randomHubViewType = GetRandomHubViewType();

            hubViewManager.Allocate(randomHubViewType);
            WaitFor(dispatcher);

            Assert.Equal(displayManager.AvailableDisplays.Count, displayViews.displayViews.Count);
            hubViewManager.DeAllocate(randomHubViewType);

            WaitFor(dispatcher);

            Assert.Empty(displayViews.displayViews);
        }

        private static void WaitFor(Dispatcher dispatcher)
        {
            dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => { })).Wait();
        }
    }
}
