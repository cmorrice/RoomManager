using System;
using Intel.Unite.Common.Logging;

namespace UnitePluginTestApp.Manager
{
    internal class HubLogManager : IModuleLoggingManager
    {
        public void LogException(Guid moduleId, string source, string message, Exception ex)
        {
            Console.Write(String.Format("{0} {1} {2} {3} {4}", moduleId, source, source, message, ex));
        }

        public void LogMessage(Guid moduleId, LogLevel severity, string source, string message, DateTime timestamp)
        {
            Console.Write(String.Format("{0} {1} {2} {3} {4}", moduleId, severity, source, message, timestamp));
        }

        public void LogMessage(Guid moduleId, LogLevel severity, string source, string message)
        {
            Console.Write(String.Format("{0} {1} {2} {3}", moduleId, severity, source, message));
        }
    }
}