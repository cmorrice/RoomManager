using System;
using System.Collections.Generic;
using Intel.Unite.Common.Sensor;

namespace UnitePluginTest.Stubs
{
    public class StubSensorManager : ISensorManager 
    {
        public void Set(Sensor sensor)
        {
            SensorAdded?.Invoke(this, new SensorArgs(sensor));
        }

        public List<Sensor> Get()
        {
            throw new NotImplementedException();
        }

        public List<Sensor> Get(byte type)
        {
            throw new NotImplementedException();
        }

        public bool RemoveSensor(string sensorUniqueName, Guid sensorId, Guid moduleId, byte type)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<SensorArgs> SensorRemoved = delegate { };
        public event EventHandler<SensorArgs> SensorAdded;
    }
}