using System;
using System.Reflection;
using Intel.Unite.Common.Logging;
using Intel.Unite.Common.Sensor;
using UnitePlugin.Static;

namespace UnitePlugin.Sensors
{
    /// <summary>
    /// Managers the sensor information from the plugin
    /// </summary>
    public class PluginSensorManager
    {
        public ISensorManager SensorManager { get; }

        /// <summary>
        /// Constructor which allocates the Intel.Unite.Common sensor manager
        /// </summary>
        /// <param name="sensorManager" cref="ISensorManager"></param>
        public PluginSensorManager(ISensorManager sensorManager)
        {
            SensorManager = sensorManager;
        }

        /// <summary>
        /// A method to attach to a plugin specific event which will route to SensorManager.Set
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void UpdateSensorData(object sender, SensorArgs args)
        {
            UnitePluginConfig.RuntimeContext.LogManager.LogMessage(Constants.ModuleConstants.ModuleInfo.Id, LogLevel.Trace, this.GetType().Name , MethodBase.GetCurrentMethod().ToString());
            try
            {
                SensorManager.Set(args.Sensor);
            }
            catch (Exception e)
            {
                UnitePluginConfig.RuntimeContext.LogManager.LogException(Constants.ModuleConstants.ModuleInfo.Id, MethodBase.GetCurrentMethod().ToString(), "error setting sensor" ,e);
            }
            
        }
    }
}