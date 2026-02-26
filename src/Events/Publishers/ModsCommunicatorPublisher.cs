using OutwardBasicChatCommands.Managers;
using OutwardBasicChatCommands.Utility.Enums;
using OutwardBasicChatCommands.Utility.Helpers;
using OutwardModsCommunicator;
using OutwardModsCommunicator.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardBasicChatCommands.Events.Publishers
{
    public static class ModsCommunicatorPublisher
    {
        public static void SendCfgToXmlCommand()
        {
            Dictionary<string, (string, string)> parameters = new()
            {
                {
                    "filePath",
                    ("Optional. Stores xml to provided file path.", null)
                }
            };

            Action<Character, Dictionary<string, string>> function = ConvertConfigsToXml;

            var payload = new EventPayload
            {
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandName).key] = "cfgToXml",
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandParameters).key] = parameters,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandAction).key] = function,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandDescription).key] = "Saves your current " +
                "configuration as an XML file for the ModsCommunicator mod. Intended mainly for mod developers. Regular users " +
                "should only lock the values they need, as locking everything may lead to unintended gameplay changes."
            };

            EventBus.Publish(EventBusPublisher.ChatCommands_Listener, EventBusPublisher.Event_AddCommand, payload);
        }

        public static void ConvertConfigsToXml(Character owner, Dictionary<string, string> arguments)
        {
            ChatPanel panel = owner?.CharacterUI?.ChatPanel;

            if(panel == null)
            {
                OBCC.LogMessage("ModsCommunicatorPublisher@ConvertConfigsToXml Tried to use missing chatPanel.");
                return;
            }

            arguments.TryGetValue("filePath", out string filePath);

            if(string.IsNullOrWhiteSpace(filePath))
            {
                ChatHelpers.SendChatLog(panel, $"You did not provide filePath varialbe. We will store xml to \"{PathsManager.Instance.xmlFilePath}\"", ChatLogStatus.Warning);
                filePath = PathsManager.Instance.xmlFilePath;
            }

            ConfigOverrides configs = ConfigExporter.ExportAllConfigs();

            if(DataSerializer.SaveToXml(configs, filePath))
                ChatHelpers.SendChatLog(panel, $"XML file successfully saved to \"{filePath}\". Refer to the ModsCommunicator documentation for instructions on where to place it to reliably sync or override mod configurations.", ChatLogStatus.Success);
            else
                ChatHelpers.SendChatLog(panel, $"Failed to store xml to \"{filePath}\"! Check Logs for more information.", ChatLogStatus.Error);
        }
    }
}
