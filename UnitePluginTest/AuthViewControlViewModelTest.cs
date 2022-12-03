using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;
using Intel.Unite.Common.Command;
using Intel.Unite.Common.Command.Serialize;
using System;
using Moq;
using UnitePlugin.Constants;
using UnitePlugin.Interfaces;
using UnitePlugin.Model.EventArguments;
using UnitePlugin.Static;
using UnitePlugin.Utility;
using UnitePlugin.ViewModel.Controls;
using UnitePluginTest.Helpers;
using UnitePluginTest.Stubs;
using Xunit;

namespace UnitePluginTest
{
    [Collection("SerialTests")]
    public class AuthViewControlViewModelTest
    {

        /// <summary>
        /// Auth View Control Tests 
        /// </summary>

        [Fact]
        [Trait("Category", "AuthViewControl")]
        public void AuthView_Button_Click()
        {
            new ModuleSetup().SetupRuntimeContext(out var displayManager, out var context, out var displayViews, out var sensorManager, out var dispatcher);
            UnitePluginConfig.RuntimeContext = context;
            UnitePluginConfig.HubViewManager = new Mock<IHubViewManager>().Object;

            UnitePluginConfig.SetHubViewManager(UnitePluginConfig.HubViewManager);


            StubMessageSenderEventArgs args = null;
            ((StubMessageSender) context.MessageSender).MessageAvailable += (sender, e) =>
            {
                args = e;
            };

            var authViewEventArgsSubscriber = new AuthViewEventArgsSubscriber(false);
            var authViewControlVm = new AuthViewControlViewModel();

            authViewControlVm.ShowAuthViewButton_ClickCommand.Execute(authViewControlVm);

            var expectedMessage = new Message
            {
                Priority = MessagePriority.High,
                Data = new JsonCommandSerializer().Serialize(new ToggleAuthViewEventArgs()),
                DataType = (int)Enum.Parse(typeof(EventArgumentTypes), "ToggleAuthViewEventArgs"),
                SourceModuleId = ModuleConstants.ModuleInfo.Id,
                TargetId = MessageConstants.TargetLocalhostId,
                TargetModuleId = ModuleConstants.ModuleInfo.Id,
            };

            AssertMessageEqual(expectedMessage, args.AMessage);
        }

        [Fact]
        [Trait("Category", "AuthViewControl")]
        public void AuthView_IncomingMessage()
        {
            new ModuleSetup().SetupRuntimeContext(out var displayManager, out var context, out var displayViews, out var sensorManager, out var dispatcher);
            UnitePluginConfig.RuntimeContext = context;
            UnitePluginConfig.HubViewManager = new HubViewManager(context, dispatcher, StubContractCreator.CreateContract);
            StubMessageSenderEventArgs args = null;
            ((StubMessageSender) context.MessageSender).MessageAvailable += (sender, e) =>
            {
                args = e;
            };

            var authViewEventArgsSubscriber = new AuthViewEventArgsSubscriber(false);
            var authViewControlVm = new AuthViewControlViewModel();

            var message = new Message
            {
                Priority = MessagePriority.High,
                Data = new JsonCommandSerializer().Serialize(new ShowAuthViewEventArgs
                {
                    ViewModel = null,
                    SenderControlIdentifier = Guid.NewGuid(),
                    HubViewType = UnitePlugin.UI.HubView.Type.AuthImage,
                    HubViewMethod = "Allocate",
                    IsOnAllDisplays = true,
                }),
                DataType = (int)Enum.Parse(typeof(EventArgumentTypes), "ShowAuthViewEventArgs"),
                SourceModuleId = ModuleConstants.ModuleInfo.Id,
                TargetId = MessageConstants.TargetLocalhostId,
                TargetModuleId = ModuleConstants.ModuleInfo.Id,
            };

            MessagingEventBroker.Process(message);

            Assert.Null(args);
            Assert.True(authViewEventArgsSubscriber.Fired);
        }

        private void AssertMessageEqual(Message expectedMessage, Message actualMessage)
        {
            Assert.Equal(expectedMessage.Priority, actualMessage.Priority);
            Assert.Equal(expectedMessage.DataType, actualMessage.DataType);
            Assert.Equal(expectedMessage.SourceModuleId, actualMessage.SourceModuleId);
        }

        public class AuthViewEventArgsSubscriber
        {
            public bool Fired = false;

            public AuthViewEventArgsSubscriber(bool fired)
            {
                Fired = fired;
                MessagingEventBroker.GlobalEventBroker.Register(this);
            }


            [EventSubscription("topic://" + "ShowAuthViewEventArgs", typeof(OnUserInterface))]
            public void UpdateParticipants(object sender, ShowAuthViewEventArgs eArgs)
            {
                Fired = true;
            }
        }

    }
}
