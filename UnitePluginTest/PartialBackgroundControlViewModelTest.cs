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
    public class PartialBackgroundControlViewModelTest
    {

        /// <summary>
        /// PartialBackground View Control Tests 
        /// </summary>


        [Fact]
        [Trait("Category", "PartialBackgroundViewControl")]
        public void PartialBackgroundView_Button_Click()
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

            var partialBackgroundViewEventArgsSubscriber = new PartialBackgroundViewEventArgsSubscriber(false);
            var partialBackgroundViewControlVm = new PartialBackgroundControlViewModel();

            partialBackgroundViewControlVm.ShowPartialBackgroundViewButton_ClickCommand.Execute(partialBackgroundViewControlVm);

            var expectedMessage = new Message
            {
                Priority = MessagePriority.High,
                Data = new JsonCommandSerializer().Serialize(new TogglePartialBackgroundViewEventArgs()),
                DataType = (int)Enum.Parse(typeof(EventArgumentTypes), "TogglePartialBackgroundViewEventArgs"),
                SourceModuleId = ModuleConstants.ModuleInfo.Id,
                TargetId = MessageConstants.TargetLocalhostId,
                TargetModuleId = ModuleConstants.ModuleInfo.Id,
            };

            EqualHelper.AssertMessage(expectedMessage, args.AMessage);
        }

        [Fact]
        [Trait("Category", "PartialBackgroundViewControl")]
        public void PartialBackgroundView_IncomingMessage()
        {
            new ModuleSetup().SetupRuntimeContext(out var displayManager, out var context, out var displayViews, out var sensorManager, out var dispatcher);
            UnitePluginConfig.RuntimeContext = context;
            UnitePluginConfig.HubViewManager = new HubViewManager(context, dispatcher, StubContractCreator.CreateContract);
            StubMessageSenderEventArgs args = null;
            ((StubMessageSender) context.MessageSender).MessageAvailable += (sender, e) =>
            {
                args = e;
            };

            var partialBackgroundViewEventArgsSubscriber = new PartialBackgroundViewEventArgsSubscriber(false);
            var partialBackgroundViewControlVm = new PartialBackgroundControlViewModel();

            var message = new Message
            {
                Priority = MessagePriority.High,
                Data = new JsonCommandSerializer().Serialize(new ShowPartialBackgroundViewEventArgs
                {
                    ViewModel = null,
                    SenderControlIdentifier = Guid.NewGuid(),
                    HubViewType = UnitePlugin.UI.HubView.Type.PartialBackground,
                    HubViewMethod = "Allocate",
                    IsOnAllDisplays = true,
                }),
                DataType = (int)Enum.Parse(typeof(EventArgumentTypes), "ShowPartialBackgroundViewEventArgs"),
                SourceModuleId = ModuleConstants.ModuleInfo.Id,
                TargetId = MessageConstants.TargetLocalhostId,
                TargetModuleId = ModuleConstants.ModuleInfo.Id,
            };

            MessagingEventBroker.Process(message);

            Assert.Null(args);
            Assert.True(partialBackgroundViewEventArgsSubscriber.Fired);
        }

        public class PartialBackgroundViewEventArgsSubscriber
        {
            public bool Fired = false;

            public PartialBackgroundViewEventArgsSubscriber(bool fired)
            {
                Fired = fired;
                MessagingEventBroker.GlobalEventBroker.Register(this);
            }


            [EventSubscription("topic://" + "ShowPartialBackgroundViewEventArgs", typeof(OnPublisher))]
            public void UpdateParticipants(object sender, ShowPartialBackgroundViewEventArgs eArgs)
            {
                Fired = true;
            }
        }

    }
}
