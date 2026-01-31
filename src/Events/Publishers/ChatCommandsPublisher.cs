using OutwardBasicChatCommands.Utility.Enums;
using OutwardBasicChatCommands.Utility.Helpers;
using OutwardModsCommunicator.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardBasicChatCommands.Events.Publishers
{
    public static class ChatCommandsPublisher
    {
        public static void SendAddMaxChatMessagesCommand()
        {
            Dictionary<string, (string, string)> parameters = new()
            {
                {
                    "show",
                    ("Optional. Can be provided as bool(true, flase). Should show how many messages are set?", "true")
                },
                {
                    "amount",
                    ("Optional. Can be provided as number. Amount of messages chat panel can contain.", "30")
                }
            };

            Action<Character, Dictionary<string, string>> function = SetMaxChatMessages;

            var payload = new EventPayload
            {
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandName).key] = "maxChatMessages",
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandParameters).key] = parameters,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandAction).key] = function,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandDescription).key] = "Changes the amount of messages chat panel can contain."
            };

            EventBus.Publish(EventBusPublisher.ChatCommands_Listener, EventBusPublisher.Event_AddCommand, payload);
        }

        public static void SetMaxChatMessages(Character character, Dictionary<string, string> arguments)
        {
            ChatPanel panel = character?.CharacterUI?.ChatPanel;

            if(panel == null)
            {
                OBCC.LogMessage("EventBusPublisher@SetMaxChatMessages Tried to use missing chatPanel.");
                return;
            }

            arguments.TryGetValue("show", out string showArg);

            if(string.IsNullOrWhiteSpace(showArg) || string.Equals(showArg, "true"))
            {
                ChatHelpers.SendChatLog(panel, $"Currently chat panel can contain {panel.MaxMessageCount} messages!", ChatLogStatus.Warning);
            }

            arguments.TryGetValue("amount", out string amountArg);

            if(string.IsNullOrWhiteSpace(amountArg))
            {
                ChatHelpers.SendChatLog(panel, $"Argument for amount parameter was not provided! No changes made.", ChatLogStatus.Warning);
                return;
            }

            int.TryParse(amountArg, out int amount);

            if (amount < 0)
            {
                ChatHelpers.SendChatLog(panel, $"Argument amount cannot be negative number!", ChatLogStatus.Error);
                return;
            }

            panel.MaxMessageCount = amount;
            ChatHelpers.SendChatLog(panel, $"Chat panel now will hold {amount} messages!", ChatLogStatus.Success);
        }
    }
}
