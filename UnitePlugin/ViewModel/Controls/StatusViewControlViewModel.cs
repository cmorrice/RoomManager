using System;
using System.Windows.Input;
using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;
using UnitePlugin.Model.Command;
using UnitePlugin.Model.EventArguments;
using UnitePlugin.Static;
using UnitePlugin.Utility;

namespace UnitePlugin.ViewModel.Controls
{
    [Serializable]
    public class StatusViewControlViewModel : HubViewModel
    {
        #region Fields
        private EventHandler<ShowStatusImageEventArgs> _showStatusImage;
        public readonly BoolToStringConverter HubViewMethodBoolToStringConverter = new BoolToStringConverter("Allocate", "DeAllocate");
        private ICommand _showStatusImageButton_ClickCommand;
        #endregion

        #region Properties
        public event EventHandler<ShowStatusImageEventArgs> ShowStatusImage
        {
            add => _showStatusImage += value;
            remove => _showStatusImage -= value;
        }

        public bool IsAllocated { get; set; }

        public bool IsAllViewAllocated => UnitePluginConfig.HubViewManager.IsAllViewsAllocated(UI.HubView.Type.StatusImage);

        public string ButtonText => HubViewMethodBoolToStringConverter.Convert(!IsAllocated) + " StatusImage";

        public ICommand ShowStatusImageButton_ClickCommand
        {
            get
            {
                return _showStatusImageButton_ClickCommand ?? (_showStatusImageButton_ClickCommand = new RelayCommand<StatusViewControlViewModel>(
                           x =>
                           {
                               ShowStatusImageButton_SendMsgAndClick(this, new ToggleStatusViewEventArgs { SourceGuid = x.ControlIdentifier });
                           }, x => UnitePluginConfig.HubViewManager.DoAllViewsHaveSameIsAllocated(UI.HubView.Type.AuthImage)));
            }
        }

        #endregion

        public StatusViewControlViewModel()
        {
            MessagingEventBroker.GlobalEventBroker.Register(this);
            _showStatusImage += UnitePluginConfig.HubViewManager.EventCommandInvoker;
            CommandManager.InvalidateRequerySuggested();
        }


        #region Methods
 
        public void ShowStatusImageButton_SendMsgAndClick(object sender, ToggleStatusViewEventArgs eventArgs)
        {
            UnitePluginConfig.RuntimeContext.MessageSender.TrySendMessage(new CommandWraper<ToggleStatusViewEventArgs>(eventArgs).ToMessage());
        }

        [EventSubscription("topic://" + "ToggleStatusViewEventArgs", typeof(OnUserInterface))]
        public void ShowStatusImageButton_Click(object sender, ToggleStatusViewEventArgs eventArgs)
        {
            var localArgs = new ShowStatusImageEventArgs
            {
                SenderControlIdentifier = eventArgs.SourceGuid,
                HubViewType = UI.HubView.Type.StatusImage,
                HubViewMethod = HubViewMethodBoolToStringConverter.Convert(!IsAllViewAllocated),
                IsOnAllDisplays = true,
            };

            IsAllocated = !IsAllViewAllocated;
            _showStatusImage?.Invoke(this, localArgs);
            NotifyPropertyChanged("ButtonText");
        }


        #endregion

    }
}
