using Intel.Unite.Common.Configuration;
using Intel.Unite.Common.Core;
using System;
using System.Collections.Generic;

namespace UnitePluginTestApp.Manager
{
    public class ConfigurationManager : IConfigurationManager
    {
        public bool IsConfigurationLoaded => true;

        public event EventHandler ConfigurationLoaded = delegate { };
        public event EventHandler ConfigurationUpdated = delegate { };
        public event EventHandler ModulesUpdated = delegate { };
        public event EventHandler CommonPropertiesUpdated = delegate { };

        public List<ConfigurationProperty> GetAllModuleProperties()
        {
            return new List<ConfigurationProperty>();
        }

        public List<KeyValuePair> GetCommonProperties()
        {
            return new List<KeyValuePair>();
        }

        public KeyValuePair GetCommonProperty(string key)
        {
            return new KeyValuePair();
        }

        public List<ConfigurationProperty> GetModuleProperties(Guid moduleId)
        {
            return new List<ConfigurationProperty>();
        }

        public ConfigurationProperty GetModuleProperty(Guid moduleId, string key)
        {
            return new ConfigurationProperty() { Property = new KeyValuePair("DefaultCamera", "0") };
        }
    }
}
