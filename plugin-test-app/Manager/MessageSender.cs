using System;
using Intel.Unite.Common.Command;

namespace UnitePluginTestApp.Manager
{
    internal class MessageSender : IMessageSender
    {
        public delegate void OnMessageDelegate(Message message);
        public OnMessageDelegate OnMessage;

        public int MessageSize => 0;
        
        public bool TrySendMessage(Message message)
        {
            OnMessage(message);
            return true;
        }
    }
}
