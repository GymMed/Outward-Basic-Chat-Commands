using OutwardBasicChatCommands.Managers;
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
    public static class FollowCommandsPublisher
    {
        public static void SendAddFollowChatCommand()
        {
            Dictionary<string, (string, string)> parameters = new()
            {
                {
                    "characterName",
                    ("Optional. Tries to follow character by name. If not provided tries to find first local character or lobby character.", null)
                }
            };

            Action<Character, Dictionary<string, string>> function = FollowCharacter;

            var payload = new EventPayload
            {
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandName).key] = "follow",
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandParameters).key] = parameters,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandAction).key] = function,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandDescription).key] = "Tries to find a player to follow. Searching is done starting from local characters and moving to lobby characters."
            };

            EventBus.Publish(EventBusPublisher.ChatCommands_Listener, EventBusPublisher.Event_AddCommand, payload);
        }

        public static void FollowCharacter(Character follower, Dictionary<string, string> arguments)
        {
            ChatPanel panel = follower?.CharacterUI?.ChatPanel;

            if(panel == null)
            {
                OBCC.LogMessage("EventBusPublisher@SendAddFollowChatCommand Tried to use missing chatPanel.");
                return;
            }

            arguments.TryGetValue("characterName", out string charName);

            if(string.IsNullOrWhiteSpace(charName))
            {
                UID mainChar = panel.LocalCharacter.UID;

                Character first = CharacterManager.Instance.GetFirstLocalCharacter();            

                if(first != null && first.UID != mainChar)
                {
                    FollowerDataManager.Instance.TryToFollow(panel.LocalCharacter, first);
                    return;
                }

                Character second = CharacterManager.Instance.GetSecondLocalCharacter();            

                if(second != null && second.UID != mainChar)
                {
                    FollowerDataManager.Instance.TryToFollow(panel.LocalCharacter, second);
                    return;
                }

                Character other = CharacterHelpers.TryToFindOtherCharacterInLobby(panel.LocalCharacter);

                if(other != null)
                {
                    FollowerDataManager.Instance.TryToFollow(panel.LocalCharacter, other);
                    return;
                }

                ChatHelpers.SendChatLog(panel, "You are playing alone. There is nothing to follow.");
                return;
            }

            Character otherByName = CharacterHelpers.TryToFindOtherCharacterInLobby(panel.LocalCharacter, charName);

            if(otherByName != null)
            {
                FollowerDataManager.Instance.TryToFollow(panel.LocalCharacter, otherByName);
                return;
            }

            ChatHelpers.SendChatLog(panel, $"Could not find other player by name {charName}!");
        }
    }
}
