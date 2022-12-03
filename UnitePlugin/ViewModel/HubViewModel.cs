using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UnitePlugin.ViewModel
{
    [Serializable]
    public class HubViewModel : INotifyPropertyChanged
    {
        [field: NonSerialized]
        private event PropertyChangedEventHandler _propertyChanged;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add => _propertyChanged += value;
            remove => _propertyChanged -= value;
        }

        public Guid ControlIdentifier { get; set; }

        public HubViewModel()
        { }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
