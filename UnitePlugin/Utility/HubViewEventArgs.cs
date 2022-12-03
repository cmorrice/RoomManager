using System;
using UnitePlugin.ViewFactory;

namespace UnitePlugin.Utility
{
    [Serializable]
    public class HubViewEventArgs : EventArgs
    {
        public Guid SenderControlIdentifier { get; set; }
        public HubView.Type HubViewType { get; set; }
        public String HubViewMethod { get; set; }
    }
}
