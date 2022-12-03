using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;
using Intel.Unite.Common.Command;
using System;
using Intel.Unite.Common.Command.Serialize;
using Unite.Test.Extensions.TestFixtures;
using Xunit;
using UnitePlugin.Static;
using UnitePlugin.Model.Command;
using UnitePlugin.Constants;
using UnitePlugin.Model.EventArguments;
using UnitePluginTest.Stubs;
using UnitePluginTest.Helpers;

namespace UnitePluginTest
{
    [Collection("Ui collection")]
    public class MessagingTest
    {
        private readonly UiThreadFixture _uiThreadFixture;

        public MessagingTest(UiThreadFixture uiThreadFixture)
        {
            _uiThreadFixture = uiThreadFixture;
        }

        /// <summary>
        /// Targets base MessagingEventBroker has fired Test
        /// </summary>
        [Fact]
        [Trait("Category", "Messaging")]
        public void Broker_ProcessMessage_Fired()
        {
            var subscriberTest = new SubscriberTest(false);
            Assert.False(subscriberTest.Fired);

            MessagingEventBroker.Process(new BaseCommand<HubViewEventArgs>(new JsonCommandSerializer(),
                new HubViewEventArgs { IsOnAllDisplays = true },
                ModuleConstants.ModuleInfo.Id).ToMessage());
            Assert.True(subscriberTest.Fired, "Expected fire event true");
        }

        /// <summary>
        /// Targets base MessagingEventBroker test data Sanity Test
        /// </summary>
        [Fact]
        [Trait("Category", "Messaging")]
        public void Broker_ProcessMessage_DataCheck()
        {
            var subscriberTest = new SubscriberTest(false);
            Assert.False(subscriberTest.Fired);
            Assert.True(subscriberTest.TestBaseProperties());

            HubViewEventArgs te = new HubViewEventArgs
            {
                IsOnAllDisplays = true,
                HubViewMethod = "Incoming Message"
            };
            MessagingEventBroker.Process(new BaseCommand<HubViewEventArgs>(new JsonCommandSerializer(),
                te,
                ModuleConstants.ModuleInfo.Id).ToMessage());

            Assert.True(subscriberTest.Fired);
            Assert.True(subscriberTest.TestProperties(te));


        }

        [Fact]
        [Trait("Category", "Messaging")]
        public void TrySendMessageCheck()
        {
            // setup
            var messageSender = new StubMessageSender();

            var message = new Message();
            StubMessageSenderEventArgs args = null;
            messageSender.MessageAvailable += (sender, e) =>
             {
                 args = e;
             };

            // action
            messageSender.TrySendMessage(message);

            // test
            Assert.True(TestHelper.Compare.AreMessagesEqual(message, args.AMessage));
        }

        [Fact]
        [Trait("Category", "Messaging")]
        public void CorrectMessageGenerated()
        {
            var hubViewEventArgs = new HubViewEventArgs()
            {
                HubViewMethod = "Show",
                HubViewType = UnitePlugin.UI.HubView.Type.QuickAccessIcon,
                IsOnAllDisplays = false,
                SenderControlIdentifier = new Guid(),
            };

            var targetMessage = new Message
            {
                Priority = MessagePriority.High,
                Data = new JsonCommandSerializer().Serialize(new HubViewEventArgs()),
                DataType = (int)Enum.Parse(typeof(EventArgumentTypes), "HubViewEventArgs"),
                SourceModuleId = ModuleConstants.ModuleInfo.Id,
                TargetId = MessageConstants.TargetLocalhostId,
                TargetModuleId = ModuleConstants.ModuleInfo.Id,
            };

            var message = new BaseCommand<HubViewEventArgs>(new JsonCommandSerializer(), hubViewEventArgs, ModuleConstants.ModuleInfo.Id).ToMessage();

            Assert.True(TestHelper.Compare.AreMessagesEqual(targetMessage, message));

        }

        //public static void WaitFor(Dispatcher dispatcher)
        //{
        //    dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => { })).Wait();
        //}




        public class SubscriberTest
        {
            public bool Fired = false;
            public string Data = string.Empty;
            public int DataType = -1;
            public MessagePriority Priority = MessagePriority.Normal;
            public Guid SourceModuleId;
            public Guid TargetId;
            public Guid TargetModuleId;

            public SubscriberTest(bool fired)
            {
                Fired = fired;
                MessagingEventBroker.GlobalEventBroker.Register(this);
            }

            public bool TestBaseProperties()
            {
                bool propsCorrectlySet = true;

                if (Fired) propsCorrectlySet = false;
                if (Data != string.Empty) propsCorrectlySet = false;
                if (DataType != -1) propsCorrectlySet = false;
                if (Priority != MessagePriority.Normal) propsCorrectlySet = false;

                if (SourceModuleId != Guid.Empty) propsCorrectlySet = false;
                if (TargetId != Guid.Empty) propsCorrectlySet = false;
                if (TargetModuleId != Guid.Empty) propsCorrectlySet = false;

                return propsCorrectlySet;
            }

            public bool TestProperties(HubViewEventArgs eArgs)
            {
                if (!Fired) return false;
                if (Data != eArgs.HubViewMethod) return false;
                return true;
            }

            [EventSubscription("topic://" + "ShowStatusImageEventArgs", typeof(OnUserInterface))]
            public void ShowStatusImageEventArgs_Fired(object sender, ShowStatusImageEventArgs eArgs)
            {
                Fired = true;
            }

            [EventSubscription("topic://" + "HubViewEventArgs", typeof(OnUserInterface))]
            public void UpdateParticipants(object sender, HubViewEventArgs eArgs)
            {
                Fired = true;
                Data = eArgs.HubViewMethod;
            }
        }

    }
}