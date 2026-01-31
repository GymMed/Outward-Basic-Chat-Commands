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
    public static class SkillCommandsPublisher
    {
        public static void SendAddSkillsCommand()
        {
            Dictionary<string, (string, string)> parameters = new()
            {
                {
                    "type",
                    ("Optional. Can be provided as int. types: 0 - general data | 1 - active | 2 - passive | 3 - cosmetic", "0")
                },
            };

            Action<Character, Dictionary<string, string>> function = ShowSkillsData;

            var payload = new EventPayload
            {
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandName).key] = "skills",
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandParameters).key] = parameters,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandAction).key] = function,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandDescription).key] = "Shows skills information."
            };

            EventBus.Publish(EventBusPublisher.ChatCommands_Listener, EventBusPublisher.Event_AddCommand, payload);
        }

        public static void ShowSkillsData(Character character, Dictionary<string, string> arguments)
        {
            ChatPanel panel = character?.CharacterUI?.ChatPanel;

            if(panel == null)
            {
                OBCC.LogMessage("EventBusPublisher@SetMaxChatMessages Tried to use missing chatPanel.");
                return;
            }

            arguments.TryGetValue("type", out string typeArg);
            SkillTypes type = (SkillTypes)int.Parse(typeArg);

            if(string.IsNullOrWhiteSpace(typeArg))
            {
                type = SkillTypes.All;
            }

            SkillsHelpers.LogSkillsData(panel, type);
        }
    }
}
