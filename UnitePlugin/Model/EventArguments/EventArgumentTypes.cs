namespace UnitePlugin.Model.EventArguments
{
    /// <summary>
    /// ENUM that holds all Event types for subscription based messages
    /// </summary>
    public enum EventArgumentTypes
    {
        HubViewEventArgs = 300,
        ShowAuthViewEventArgs,
        ShowPartialBackgroundViewEventArgs,
        ShowPresentationViewEventArgs,
        ShowRibbonViewEventArgs,
        ShowStatusImageEventArgs,
        ToggleAuthViewEventArgs,
        ToggleStatusViewEventArgs,
        TogglePresentationViewEventArgs,
        TogglePartialBackgroundViewEventArgs,
    }
}
