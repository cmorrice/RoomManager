using Intel.Unite.Common.Command;
using System;

namespace UnitePluginTest.Stubs
{
    public class StubMessageSender : IMessageSender
    {
        private int _messageSize = 0;

        public event EventHandler<StubMessageSenderEventArgs> MessageAvailable;

        public int MessageSize => _messageSize;

        public bool TrySendMessage(Message message)
        {
            MessageAvailable?.Invoke(this, new StubMessageSenderEventArgs() { AMessage = message });
            return true;
        }
    }

    [Serializable]
    public class StubMessageSenderEventArgs : EventArgs
    {
        public Message AMessage { get; set; }
    }
}