using OutwardBasicChatCommands.Utility.Enums;
using OutwardModsCommunicator.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardBasicChatCommands.Events
{
    public static class EventBusSubscriber
    {
        public const string Event_AddedChatCommand = "ChatCommandsManager@AddChatCommand_After";
        public const string Event_RemovedChatCommand = "ChatCommandsManager@RemoveChatCommand_After";

        public static void AddSubscribers()
        {
            EventBus.Subscribe(EventBusPublisher.ChatCommands_Listener, Event_AddedChatCommand, AddedChatCommand);
            EventBus.Subscribe(EventBusPublisher.ChatCommands_Listener, Event_RemovedChatCommand, RemovedChatCommand);
        }

        public static void AddedChatCommand(EventPayload payload)
        {
            if (payload == null) return;

            (string key, Type type) nameParameter = ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandName);
            string commandName = payload.Get<string>(nameParameter.key, null);

            OBCC.LogMessage($"Added command {nameParameter.key}");
        }

        public static void RemovedChatCommand(EventPayload payload)
        {
            if (payload == null) return;

            (string key, Type type) nameParameter = ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandName);
            string commandName = payload.Get<string>(nameParameter.key, null);

            OBCC.LogMessage($"Removed command {nameParameter.key}");
        }
    }
}
