using System;
using System.Collections.Generic;
using System.Reflection;
using System.Timers;
using Intel.Unite.Common.Logging;
using Intel.Unite.Common.Sensor;
using UnitePlugin.Static;
using UnitePlugin.Utility;

namespace UnitePlugin.Sensors
{
    /// <summary>
    /// Generates random temperature data to demonstrate Sensor Data Within Unite
    /// </summary>
    public static class MockSensor
    {
        private const int _maxChange = 3;
        private const int _interval = 5000;

        private static readonly CryptoStrongRandom _random = new CryptoStrongRandom();
        private static readonly Timer _timer = new Timer(Interval);
        
        private static int _temp = 72;

        public static event EventHandler<SensorArgs> UpdateSensorData;
        public static string UniqueName { get; } = "UnitePlugin_Temp_Probe_1";
        public static int Interval => _interval;

        /// <summary>
        /// Starts the timer which will periodical update sensor information
        /// </summary>
        public static void Start()
        {
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Start();
        }

        /// <summary>
        /// Stops generating sensor Information
        /// </summary>
        public static void Stop()
        {
            _timer.Elapsed -= OnTimedEvent;
            _timer.AutoReset = false;
            _timer.Stop();
        }

        /// <summary>
        /// A one time call to update sensor information
        /// </summary>
        public static void SendUpdate()
        {
            UnitePluginConfig.RuntimeContext.LogManager.LogMessage(Constants.ModuleConstants.ModuleInfo.Id, LogLevel.Trace, "MockSensor", MethodBase.GetCurrentMethod().ToString());
            _temp += _random.Next(_maxChange) - _random.Next(_maxChange);
            UpdateSensorData?.Invoke(null, new SensorArgs(GetTempSensor(_temp)));
        }

        /// <summary>
        /// The event which is triggered by the timer,
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            SendUpdate();
        }

        /// <summary>
        /// Returns the Sensor class with the appropriate fields filled out.
        /// </summary>
        /// <param name="temp"></param>
        /// <returns><see cref="Sensor"/></returns>
        public static Sensor GetTempSensor(int temp)
        {
            return new Sensor
            {
                FriendlyName = "Unite Plugin Temperature",
                Expiration = DateTime.Now.AddHours(24),
                Id = Guid.NewGuid(),
                KeyValueProperties = new List<SensorKeyValue> {new SensorKeyValue{Key = "Value", Value = temp.ToString(), ValueType = SensorValueType.Int}},
                ModuleId =  Guid.NewGuid(), //TODO  add correct module id
                Type = (int)UniteSensorType.Temperature,
                UniqueName = UniqueName
            };
        }
    }
}
