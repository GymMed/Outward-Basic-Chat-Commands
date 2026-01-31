using OutwardBasicChatCommands.Utility.Enums;
using OutwardBasicChatCommands.Utility.Helpers;
using OutwardModsCommunicator.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OutwardBasicChatCommands.Events.Publishers
{
    public static class TimeCommandsPublisher
    {
        public static void SendPictureModeCommand()
        {
            Dictionary<string, (string, string)> parameters = new()
            {
                {
                    "isActive",
                    ("Optional. By default sets opposite mode. Removes/adds character and pauses the game for picture capture. Ex.:/setPictureMode true", null)
                }
            };

            Action<Character, Dictionary<string, string>> function = SetPictureMode;

            var payload = new EventPayload
            {
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandName).key] = "setPictureMode",
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandParameters).key] = parameters,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandAction).key] = function,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandDescription).key] = "Pauses/resumes the game time."
            };

            EventBus.Publish(EventBusPublisher.ChatCommands_Listener, EventBusPublisher.Event_AddCommand, payload);
        }

        public static void SetPictureMode(Character character, Dictionary<string, string> arguments)
        {
            ChatPanel panel = character?.CharacterUI?.ChatPanel;

            if(panel == null)
            {
                OBCC.LogMessage("TimeCommandsPublisher@SetPictureMode Tried to use missing chatPanel.");
                return;
            }

            arguments.TryGetValue("isActive", out string mode);

            if(string.IsNullOrWhiteSpace(mode))
            {
                float num = 1E-06f;
                bool isPaused = Time.timeScale != num;
                string pauseWord = isPaused ? "paused" : "resumed";
                character.CharacterUI.PauseMenu.TogglePause();
                character.CharacterUI.PauseMenu.OnToggleChat();
                ChatHelpers.SendChatLog(panel, $"Successfully {pauseWord} world time!", ChatLogStatus.Success);
                return;
            }

            if (string.Equals(mode, "true"))
            {
                character.gameObject.SetActive(false);
                PauseMenu.Pause(true);
                ChatHelpers.SendChatLog(panel, $"Successfully paused the game!", ChatLogStatus.Success);
            }
            else
            {
                character.gameObject.SetActive(true);
                PauseMenu.Pause(false);
                ChatHelpers.SendChatLog(panel, $"Successfully resumed the game!", ChatLogStatus.Success);
            }

            character.CharacterUI.PauseMenu.OnToggleChat();
        }

        public static void SendAddSetTimeCommand()
        {
            Dictionary<string, (string, string)> parameters = new()
            {
                {
                    "time",
                    ("Required. Set minutes for current time. Ex.: setTime 05:50", null)
                }
            };

            Action<Character, Dictionary<string, string>> function = SetTime;

            var payload = new EventPayload
            {
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandName).key] = "setTime",
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandParameters).key] = parameters,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandAction).key] = function,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandDescription).key] = "Sets current time.",
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandRequiresDebugMode).key] = true
            };

            EventBus.Publish(EventBusPublisher.ChatCommands_Listener, EventBusPublisher.Event_AddCommand, payload);
        }

        public static void SetTime(Character character, Dictionary<string, string> arguments)
        {
            ChatPanel panel = character?.CharacterUI?.ChatPanel;

            if(panel == null)
            {
                OBCC.LogMessage("TimeCommandsPublisher@SetTime Tried to use missing chatPanel.");
                return;
            }

            arguments.TryGetValue("time", out string time);

            if(string.IsNullOrWhiteSpace(time))
            {
                ChatHelpers.SendChatLog(panel, "Time argument is required!", ChatLogStatus.Error);
                return;
            }

            string[] timeParts = time.Split(':');

            if(timeParts.Length > 2)
            {
                ChatHelpers.SendChatLog(panel, "Time argument only needs hours:minutes!", ChatLogStatus.Error);
                return;
            }

            if(timeParts.Length < 1)
            {
                ChatHelpers.SendChatLog(panel, "Time argument lacks on of the parameters hours:minutes!", ChatLogStatus.Error);
                return;
            }

            int.TryParse(timeParts[0], out int hours);

            if (!TryValidateNumber(panel, hours, 23, 0, "Hours"))
                return;

            int.TryParse(timeParts[1], out int minutes);

            if (!TryValidateNumber(panel, minutes, 59, 0, "Minutes"))
                return;

            TimeHelpers.SetTime(hours, minutes);
            ChatHelpers.SendChatLog(panel, $"Successfully set time to {hours}:{minutes}!", ChatLogStatus.Success);
        }

        public static bool TryValidateNumber(ChatPanel panel, int number, int max, int min, string variableName)
        {
            if(number < min)
            {
                ChatHelpers.SendChatLog(panel, $"{variableName} can not be lower than {min}!", ChatLogStatus.Error);
                return false;
            }

            if(number > max)
            {
                ChatHelpers.SendChatLog(panel, $"{variableName} can not exceed {max}!", ChatLogStatus.Error);
                return false;
            }

            return true;
        }

        public static void SendAddSetMinutesCommand()
        {
            Dictionary<string, (string, string)> parameters = new()
            {
                {
                    "minutes",
                    ("Required. Set minutes for current time. Ex.: /setMinutes 55", null)
                }
            };

            Action<Character, Dictionary<string, string>> function = SetMinutes;

            var payload = new EventPayload
            {
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandName).key] = "setMinutes",
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandParameters).key] = parameters,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandAction).key] = function,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandDescription).key] = "Sets current hour minutes.",
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandRequiresDebugMode).key] = true
            };

            EventBus.Publish(EventBusPublisher.ChatCommands_Listener, EventBusPublisher.Event_AddCommand, payload);
        }

        public static void SetMinutes(Character character, Dictionary<string, string> arguments)
        {
            ChatPanel panel = character?.CharacterUI?.ChatPanel;

            if(panel == null)
            {
                OBCC.LogMessage("TimeCommandsPublisher@SetMinutes Tried to use missing chatPanel.");
                return;
            }

            arguments.TryGetValue("minutes", out string minutes);

            if(string.IsNullOrWhiteSpace(minutes))
            {
                ChatHelpers.SendChatLog(panel, "Minutes argument is required!", ChatLogStatus.Error);
                return;
            }

            int.TryParse(minutes, out int minutesInt);

            if(minutesInt < 0)
            {
                ChatHelpers.SendChatLog(panel, "Minutes can not be negative!", ChatLogStatus.Error);
                return;
            }

            if(minutesInt > 60)
            {
                ChatHelpers.SendChatLog(panel, "Minutes can not exceed 60!", ChatLogStatus.Error);
                return;
            }

            TimeHelpers.SetMinutes(minutesInt);
            ChatHelpers.SendChatLog(panel, $"Successfully set minutes to {minutes}!", ChatLogStatus.Success);
        }
    }
}
