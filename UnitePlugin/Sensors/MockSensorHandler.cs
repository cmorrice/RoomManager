using System.Linq;
using Intel.Unite.Common.Module.Common;
using Intel.Unite.Common.Sensor;
using UnitePlugin.Static;

namespace UnitePlugin.Sensors
{
    /// <summary>
    /// Handles updates to sensor
    /// </summary>
public class MockSensorHandler : MarshalByRefObjectBase
    {
    public const int VisibilityTime = 10;

    /// <summary>
    /// When this method is added the RuntimeContext.SensorManager.SensorAdded event,
    /// Every time a the sensor is updated a toast msg will appear
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void ProcessData(object sender, SensorArgs e)
    {
        if (e.Sensor.UniqueName != MockSensor.UniqueName) return;
        var temp = e.Sensor.KeyValueProperties.FirstOrDefault(x => x.Key == "Value")?.Value;
        UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Toast Message Mock Sensor Temp: {temp}", VisibilityTime);
    }
}
}