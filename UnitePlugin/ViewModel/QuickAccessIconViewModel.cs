using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;
using Intel.Unite.Common.Display;
using System;
using System.Windows.Input;
using UnitePlugin.Model.EventArguments;
using UnitePlugin.Static;
using UnitePlugin.Utility;

namespace UnitePlugin.ViewModel
{
    [Serializable]
    public class QuickAccessIconViewModel : HubViewModel
    {

        #region Fields
        [field: NonSerialized]
        private EventHandler<HubViewEventArgs> _showQuickAccessControl;
        private ICommand _quickAccessButton_ClickCommand;
        #endregion

        [field: NonSerialized]
        private UniteImage _image;

        public UniteImage Image { get => _image; set => _image = value; }

        #region Properties
        public event EventHandler<HubViewEventArgs> ShowQuickAccessControl
        {
            add => _showQuickAccessControl += value;
            remove => _showQuickAccessControl -= value;
        }
        #endregion

        public QuickAccessIconViewModel() : base()
        {
            MessagingEventBroker.GlobalEventBroker.Register(this);
        }


        public ICommand ShowButton
        {
            get
            {
                return _quickAccessButton_ClickCommand ?? (_quickAccessButton_ClickCommand = new RelayCommand<HubViewEventArgs>(
                           x =>
                           {
                               QuickAccessButton_Click(this, new HubViewEventArgs
                               {
                                   SenderControlIdentifier = ControlIdentifier,
                                   HubViewType = UI.HubView.Type.QuickAccessApp,
                                   HubViewMethod = "Show",
                               });
                           }));
            }
        }

        [EventSubscription("topic://" + "HubViewEventArgs", typeof(OnUserInterface))]
        public void QuickAccessButton_Click(object sender, HubViewEventArgs e)
        {
            _showQuickAccessControl?.Invoke(this, e);
        }
    }
}