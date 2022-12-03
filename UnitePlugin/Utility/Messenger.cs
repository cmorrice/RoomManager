using Appccelerate.EventBroker;
using Intel.Unite.Common.Command;
using System;
using UnitePlugin.Interfaces;
using UnitePlugin.Static;

namespace UnitePlugin.Utility
{
    public class Messenger<T> : IMessenger
    {
        /// <summary>
        /// Method allowing access to initiate sending messages to subscribers
        /// </summary>
        /// <param name="message">The Message being sent</param>
        public void InvokeSubscriptions(Message message)
        {
            OnReceivedMessage(ConvertMessage<T>.Deserialize(message));
        }

        /// <summary>
        /// Method used to create the event to listening subscribers
        /// </summary>
        /// <param name="o">Deserialized message</param>
        private void OnReceivedMessage(object o)
        {
            MessagingEventBroker.GlobalEventBroker.Fire("topic://" + typeof(T).Name, this, HandlerRestriction.None, this, (EventArgs)o);
        }
    }
}
