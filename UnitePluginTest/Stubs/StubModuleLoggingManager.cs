using System;
using System.Collections.Generic;
using Intel.Unite.Common.Logging;

namespace UnitePluginTest.Stubs
{
    internal class StubModuleLoggingManager : IModuleLoggingManager
    {
        // ReSharper disable once CollectionNeverQueried.Local
        // Might implement in the future
        private readonly List<string> _log = new List<string>();

        public void LogException(Guid moduleId, string source, string message, Exception ex)
        {
            var text = moduleId.ToString() + " " + source + " " + message + " " + ex;
            _log.Add(text);
            //Console.Write(text);
        }

        public void LogMessage(Guid moduleId, LogLevel severity, string source, string message, DateTime timestamp)
        {
            var text = moduleId.ToString() + " " + source + " " + message;
            _log.Add(text);
            //Console.Write(text);
        }

        public void LogMessage(Guid moduleId, LogLevel severity, string source, string message)
        {
            var text = moduleId.ToString() + " " + source + " " + message;
            _log.Add(text);
            //Console.Write(text);
        }
    }
}