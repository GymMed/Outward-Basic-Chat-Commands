using OutwardBasicChatCommands.Utility.Enemies.Visuals;
using OutwardBasicChatCommands.Utility.Enums;
using OutwardBasicChatCommands.Utility.Helpers;
using OutwardModsCommunicator.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OutwardBasicChatCommands.Events.Publishers
{
    public class CharacterVisualsPublisher
    {
        public static void SendAddSetVisualsCommand()
        {
            Dictionary<string, (string, string)> parameters = new()
            {
                {
                    "race",
                    ("Required. Changes character race. 1 - Auraian | 2 - Tramon | 3 - Kazite. Ex.:/setVisuals 2", null)
                },
                {
                    "hairStyle",
                    ("Required. Changes character hair style. Ex.:/setVisuals 2 15", null)
                },
                {
                    "hairColor",
                    ("Required. Changes character hair color. Ex.:/setVisuals 2 15 11", null)
                },
                {
                    "headVariation",
                    ("Required. Changes character hair variation. Ex.:/setVisuals 2 15 11 6", null)
                },
                {
                    "gender",
                    ("Optional. Changes character gender. 1 - Female | 2 - Male. Ex.:/setVisuals 2 15 11 6 1", null)
                }
            };

            Action<Character, Dictionary<string, string>> function = SetVisuals;

            var payload = new EventPayload
            {
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandName).key] = "setVisuals",
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandParameters).key] = parameters,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandAction).key] = function,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandDescription).key] = "Changes how your character looks.",
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandRequiresDebugMode).key] = true
            };

            EventBus.Publish(EventBusPublisher.ChatCommands_Listener, EventBusPublisher.Event_AddCommand, payload);
        }

        public static void SetVisuals(Character character, Dictionary<string, string> arguments)
        {
            try
            {
                ChatPanel panel = character?.CharacterUI?.ChatPanel;

                if (panel == null)
                {
                    OBCC.LogMessage("CharacterVisualsPublisher@SetVisuals Tried to use missing chatPanel.");
                    return;
                }

                arguments.TryGetValue("race", out string race);

                ChatHelpers.SendChatLog(panel, $"Race {race}", ChatLogStatus.Warning);

                if (!ValidateIntFromString(panel, race, "Race", out int raceIndex, 3, 1))
                    return;

                RacesHelper.races.TryGetValue((Races)(raceIndex - 1), out RaceData raceData);
                GenderData genderData = default;

                arguments.TryGetValue("gender", out string gender);

                if (ValidateIntFromString(panel, gender, "Gender", out int genderIndex, 2, 1, true))
                {
                    if (genderIndex == 1)
                    {
                        genderData = raceData.GetGender(Character.Gender.Female);
                        genderIndex = (int)Character.Gender.Female;
                    }
                    else if (genderIndex == 2)
                    {
                        genderData = raceData.GetGender(Character.Gender.Male);
                        genderIndex = (int)Character.Gender.Male;
                    }
                    else
                    {
                        genderData = raceData.GetGender(character.VisualData.Gender);
                        genderIndex = character.VisualData.Gender == Character.Gender.Female ? 1 : 0;
                    }
                }
                else
                {
                    genderData = raceData.GetGender(character.VisualData.Gender);
                    genderIndex = character.VisualData.Gender == Character.Gender.Female ? 1 : 0;
                }

                arguments.TryGetValue("hairStyle", out string hairStyle);
                ChatHelpers.SendChatLog(panel, $"hairStyle {hairStyle}", ChatLogStatus.Warning);

                if (!ValidateIntFromString(panel, hairStyle, "Hair Style", out int hairStyleIndex, genderData.TotalHairStyles, 1))
                    return;

                arguments.TryGetValue("hairColor", out string hairColor);
                ChatHelpers.SendChatLog(panel, $"hairColor {hairColor}", ChatLogStatus.Warning);

                if (!ValidateIntFromString(panel, hairColor, "Hair Color", out int hairColorIndex, genderData.TotalHairColors, 1))
                    return;

                arguments.TryGetValue("headVariation", out string headVariation);
                ChatHelpers.SendChatLog(panel, $"headVariation {headVariation}", ChatLogStatus.Warning);

                if (!ValidateIntFromString(panel, headVariation, "Head Variation", out int headVariationIndex, genderData.TotalHeadVariations, 1))
                    return;

                GameObject currentHair = character.Visuals.DefaultHairVisuals?.gameObject;
                GameObject currentHead = character.Visuals.DefaultHeadVisuals?.gameObject;
                GameObject currentBody = character.Visuals.DefaultBodyVisuals?.gameObject;
                GameObject currentFeets = character.Visuals.DefaultFootVisuals?.gameObject;

                GameObject currentArmor = character.Visuals.ActiveVisualsBody?.gameObject;
                GameObject currentBoots = character.Visuals.ActiveVisualsFoot?.gameObject;

                if(currentHair != null)
                    currentHair.SetActive(false);

                if(currentHead != null)
                    currentHead.SetActive(false);

                if(currentBody != null)
                    currentBody.SetActive(false);

                if(currentFeets != null)
                    currentFeets.SetActive(false);

                if(currentArmor != null)
                    currentArmor.SetActive(false);

                if(currentBoots != null)
                    currentBoots.SetActive(false);

                int raceIndexMinusOne = raceIndex - 1;
                int hairStyleIndexMinusOne = hairStyleIndex - 1;
                int hairColorIndexMinusOne = hairColorIndex - 1;
                int headVariationIndexMinusOne = headVariationIndex - 1;

                character.Visuals.ResetDataCharacterCreation();

                character.VisualData.Gender = (Character.Gender)genderIndex;
#if DEBUG
                ChatHelpers.SendChatLog(panel, $"Set gender: {(Character.Gender)genderIndex} index: {genderIndex}", ChatLogStatus.Success);
#else
                ChatHelpers.SendChatLog(panel, $"Set gender: {(Character.Gender)genderIndex}", ChatLogStatus.Success);
#endif

                character.Visuals.LoadCharacterCreationHead(raceIndexMinusOne, genderIndex, headVariationIndexMinusOne);
                character.Visuals.LoadCharacterCreationHair(hairStyleIndexMinusOne, hairColorIndexMinusOne);
                character.Visuals.LoadCharacterCreationBody(genderIndex, raceIndexMinusOne);
                character.Visuals.LoadCharacterCreationBoots(genderIndex, raceIndexMinusOne);

                character.VisualData.SkinIndex = raceIndexMinusOne;
                character.VisualData.HairStyleIndex = hairStyleIndexMinusOne;
                character.VisualData.HairColorIndex = hairColorIndexMinusOne;
                character.VisualData.HeadVariationIndex = headVariationIndexMinusOne;

                character.Visuals.m_helmVisualChanged = true;
                character.Visuals.m_armorVisualChanged = true;
                character.Visuals.m_footVisualChanged = true;

                ChatHelpers.SendChatLog(panel, $"Changed Character Visual Data.", ChatLogStatus.Success);
            }
            catch(Exception Ex)
            {
                OBCC.LogMessage($"While setting visuals we encontered an error:{Ex.Message}");
            }
        }

        public static bool ValidateIntFromString(ChatPanel panel,  string variable, string variableName, out int result, int max = 3, int min = 0, bool silent = false)
        {
            result = -1;

            if(string.IsNullOrWhiteSpace(variable))
            {
                if(!silent)
                    ChatHelpers.SendChatLog(panel, $"{variableName} argument is required!", ChatLogStatus.Error);
                return false;
            }

            int.TryParse(variable, out int index);
            result = index;

            if(index < min)
            {
                if(!silent)
                    ChatHelpers.SendChatLog(panel, $"{variableName} argument value is too small! Minimum {min} is allowed.", ChatLogStatus.Error);
                return false;
            }

            if(index > max)
            {
                if(!silent)
                    ChatHelpers.SendChatLog(panel, $"{variableName} argument value is too big! Maximum {max} is allowed.", ChatLogStatus.Error);
                return false;
            }

            return true;
        }
    }
}
