using System;
using System.Globalization;
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
    public class PartialBackgroundControlViewModel : HubViewModel
    {
        #region Fields
        [field: NonSerialized]
        private EventHandler<ShowPartialBackgroundViewEventArgs> _showPartialBackgroundView;
        [field: NonSerialized]
        public readonly BoolToStringConverter HubViewMethodBoolToStringConverter = new BoolToStringConverter("Allocate", "DeAllocate");

        private ICommand _showPartialBackgroundViewButton_ClickCommand;
        #endregion

        #region Properties
        public event EventHandler<ShowPartialBackgroundViewEventArgs> ShowPartialBackgroundView
        {
            add => _showPartialBackgroundView += value;
            remove => _showPartialBackgroundView -= value;
        }

        public bool IsAllocated { get; set; }

        public bool IsAllViewAllocated => UnitePluginConfig.HubViewManager.IsAllViewsAllocated(UI.HubView.Type.PartialBackground);

        public string ButtonText => HubViewMethodBoolToStringConverter.Convert(!IsAllocated) + " PartialBackground";

        public ICommand ShowPartialBackgroundViewButton_ClickCommand
        {
            get
            {
                return _showPartialBackgroundViewButton_ClickCommand ?? (_showPartialBackgroundViewButton_ClickCommand = new RelayCommand<PartialBackgroundControlViewModel>(
                           x =>
                           {
                               ShowPartialBackgroundViewButton_SendMsgAndClick(this, new TogglePartialBackgroundViewEventArgs { SourceGuid = x.ControlIdentifier });
                           }, x => UnitePluginConfig.HubViewManager.DoAllViewsHaveSameIsAllocated(UI.HubView.Type.AuthImage)));
            }
        }
        #endregion

        public PartialBackgroundControlViewModel()
        {
            MessagingEventBroker.GlobalEventBroker.Register(this);
            _showPartialBackgroundView += UnitePluginConfig.HubViewManager.EventCommandInvoker;
            CommandManager.InvalidateRequerySuggested();
        }


        #region Methods

        public void ShowPartialBackgroundViewButton_SendMsgAndClick(object sender, TogglePartialBackgroundViewEventArgs eventArgs)
        {
            UnitePluginConfig.RuntimeContext.MessageSender.TrySendMessage(
                new CommandWraper<TogglePartialBackgroundViewEventArgs>(eventArgs).ToMessage());
        }

        [EventSubscription("topic://" + "TogglePartialBackgroundViewEventArgs", typeof(OnPublisher))]
        public void ShowPartialBackgroundViewButton_Click(object sender, TogglePartialBackgroundViewEventArgs eventArgs)
        {
            var localArgs = new ShowPartialBackgroundViewEventArgs
            {
                SenderControlIdentifier = eventArgs.SourceGuid,
                HubViewType = UI.HubView.Type.PartialBackground,
                HubViewMethod = HubViewMethodBoolToStringConverter.Convert(!IsAllViewAllocated),
                IsOnAllDisplays = true,
            };

            IsAllocated = !IsAllViewAllocated;
            _showPartialBackgroundView?.Invoke(this, localArgs);
            NotifyPropertyChanged("ButtonText");
        }

        #endregion

    }
}
