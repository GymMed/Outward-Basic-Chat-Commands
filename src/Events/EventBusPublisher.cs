using Mono.Cecil;
using OutwardBasicChatCommands.Events.Publishers;
using OutwardBasicChatCommands.Managers;
using OutwardBasicChatCommands.Utility.Enums;
using OutwardBasicChatCommands.Utility.Helpers;
using OutwardModsCommunicator.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardBasicChatCommands.Events
{
    public static class EventBusPublisher
    {
        public const string Event_AddCommand = "ChatCommandsManager@AddChatCommand";
        public const string Event_RemoveCommand = "ChatCommandsManager@RemoveChatCommand";

        //other mods listener uid
        public const string ChatCommands_Listener = "gymmed.chat_commands_manager_*";

        public static void SendCommands()
        {
            EnchantmentCommandsPublisher.SendAddEnchantmentRecipeItemsCommand();
            EnchantmentCommandsPublisher.SendAddEnchantmentRecipesCommand();
            EnchantmentCommandsPublisher.SendAddEnchantmentsCommand();
            EnchantmentCommandsPublisher.SendAddBrokenEnchantmentsCommand();

            FollowCommandsPublisher.SendAddFollowChatCommand();

            TimeCommandsPublisher.SendAddSetTimeCommand();
            TimeCommandsPublisher.SendAddSetMinutesCommand();
            //TimeCommandsPublisher.SendPictureModeCommand();

            ChatCommandsPublisher.SendAddMaxChatMessagesCommand();

            SkillCommandsPublisher.SendAddSkillsCommand();

            CharacterVisualsPublisher.SendAddSetVisualsCommand();

            ModsCommunicatorPublisher.SendCfgToXmlCommand();
        }
    }
}
