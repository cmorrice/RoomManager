using System;

namespace UnitePlugin.ViewModel
{
    [Serializable]
    public class PresentationViewModel : HubViewModel
    {
        public string Title { get; set; } = "Presentation View";
    }
}