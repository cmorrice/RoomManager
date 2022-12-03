using UnitePlugin.Static;

namespace UnitePlugin.Sensors
{
    /// <summary>
    /// Configures Sensors to send and receive data from Intel.Unite.Common 
    /// </summary>
public static class SensorConfig
{
    public static void Setup()
    {
        SetupPluginSensorManager();
        SetupCoreUpdates();
        SetupLocalSensorUpdates();
    }

    private static void SetupLocalSensorUpdates()
    {
        MockSensor.UpdateSensorData += UnitePluginConfig.PluginSensorManager.UpdateSensorData;
    }

    private static void SetupCoreUpdates()
    {
        UnitePluginConfig.RuntimeContext.SensorManager.SensorAdded += new MockSensorHandler().ProcessData;
    }

    private static void SetupPluginSensorManager()
    {
        UnitePluginConfig.PluginSensorManager = new PluginSensorManager(UnitePluginConfig.RuntimeContext.SensorManager);
    }
}
}