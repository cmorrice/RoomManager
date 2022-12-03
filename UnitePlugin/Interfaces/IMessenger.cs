using Intel.Unite.Common.Command;

namespace UnitePlugin.Interfaces
{
    public interface IMessenger
    {
        /// <summary>
        /// Interface to Invoke messages to subcribers.
        /// </summary>
        /// <param name="message"></param>
        void InvokeSubscriptions(Message message);
    }
}
