using System;
using System.Collections.Generic;
using Intel.Unite.Common.Sensor;

namespace UnitePluginTestApp.Manager
{
    internal class SensorManager : ISensorManager
    {
        public event EventHandler<SensorArgs> SensorRemoved = delegate { };
        public event EventHandler<SensorArgs> SensorAdded = delegate { };

        public List<Sensor> Get()
        {
            return new List<Sensor>();
        }

        public List<Sensor> Get(byte type)
        {
            return new List<Sensor>();
        }

        public bool RemoveSensor(string sensorUniqueName, Guid sensorId, Guid moduleId, byte type)
        {
            return true;
        }

        public void Set(Sensor sensor)
        {
            SensorAdded.Invoke(this, new SensorArgs(sensor));
        }
    }
}