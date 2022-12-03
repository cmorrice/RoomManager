using System;
using System.Windows.Threading;
using Intel.Unite.Common.Sensor;
using UnitePlugin.Sensors;
using UnitePlugin.Static;
using UnitePluginTest.Helpers;
using UnitePluginTest.Stubs;
using Xunit;

namespace UnitePluginTest
{
    public class TestSensorHandler
    {
        public Sensor Sensor { get; set; }

        public void MockSensorOnUpdateSensorData(object sender, SensorArgs e)
        {
            Sensor = e.Sensor;
        }
    }


    [Collection("SerialTests")]
    public class SensorsTest
    {
        private readonly TestSensorHandler _testSensorHandler = new TestSensorHandler();

        [Fact]
        [Trait("Category", "MockSensor")]
        public void SensorUpdates()
        {
            var testSensorHandler = new TestSensorHandler();
            MockSensor.UpdateSensorData += testSensorHandler.MockSensorOnUpdateSensorData;
            MockSensor.SendUpdate();
            Assert.Equal(MockSensor.UniqueName, testSensorHandler.Sensor.UniqueName);
        }

        [Fact]
        [Trait("Category", "MockSensorHandler")]
        public void SendToastMessage()
        {
            new ModuleSetup().SetupRuntimeContext(out var displayManager, out var context, out var displayViews, out var sensorManager, out var dispatcher);
            UnitePluginConfig.RuntimeContext = context;
            sensorManager.SensorAdded += new MockSensorHandler().ProcessData;
            const int temp = 72;
            var sensor = MockSensor.GetTempSensor(temp);
            
            sensorManager.Set(sensor);
            Assert.Contains(new ToastMsg {Text = $"Toast Message Mock Sensor Temp: {temp}", VisibilityTime = MockSensorHandler.VisibilityTime, Image = null}, displayManager.ToastMsgList);
        }
        
        [Fact]
        [Trait("Category", "PluginSensorManager")]
        public void UpdateSensorData()
        {
            new ModuleSetup().SetupRuntimeContext(out var displayManager, out var context, out var displayViews, out var sensorManager, out var dispatcher);
            UnitePluginConfig.RuntimeContext = context;

            var pluginSensorManager = new PluginSensorManager(sensorManager);

            sensorManager.SensorAdded += new MockSensorHandler().ProcessData;
            const int temp = 72;
            var sensor = MockSensor.GetTempSensor(temp);

            pluginSensorManager.UpdateSensorData(this, new SensorArgs(sensor));
            Assert.Contains(new ToastMsg { Text = $"Toast Message Mock Sensor Temp: {temp}", VisibilityTime = MockSensorHandler.VisibilityTime, Image = null }, displayManager.ToastMsgList);
        }

        [Fact]
        [Trait("Category", "SensorConfig")]
        public void ConfigAndStartSensor()
        {
            new ModuleSetup().SetupRuntimeContext(out var displayManager, out var context, out var displayViews, out var sensorManager, out var dispatcher);
            UnitePluginConfig.RuntimeContext = context;

            SensorConfig.Setup();
            MockSensor.SendUpdate();

            Assert.NotEmpty(displayManager.ToastMsgList);
        }

        private static void WaitFor(Dispatcher dispatcher)
        {
            dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => { })).Wait();
        }
    }
}
