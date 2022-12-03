using System;

namespace UnitePlugin.Model.EventArguments
{
    /// <summary>
    /// EventArg that is used when a plugin attempts to Allocate, Deallocate or Show a Unite View.
    /// </summary>
    [Serializable]
    public class HubViewEventArgs : EventArgs
    {
        public bool IsOnAllDisplays { get; set; }           // Sets wheteher this EventArg should be applied to all available views
        public Guid SenderControlIdentifier { get; set; }   // Sets the Guid for the sender
        public UI.HubView.Type HubViewType { get; set; }    // Target HubView to interact with
        public String HubViewMethod { get; set; }           // Target method: "Allocate", "DeAllocate" or "Show"
    }
}