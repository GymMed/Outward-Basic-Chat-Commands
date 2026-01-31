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
    public static class EnchantmentCommandsPublisher
    {
        public static void SendAddEnchantmentsCommand()
        {
            Dictionary<string, (string, string)> parameters = new()
            {
                {
                    "short",
                    ("Optional. Can be provided as true or anything else. If it is true provides count of all available Enchantments count", "false")
                }
            };

            Action<Character, Dictionary<string, string>> function = EnchantmentsInformation;

            var payload = new EventPayload
            {
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandName).key] = "enchantments",
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandParameters).key] = parameters,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandAction).key] = function,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandDescription).key] = "Provides information about all enchantments."
            };

            EventBus.Publish(EventBusPublisher.ChatCommands_Listener, EventBusPublisher.Event_AddCommand, payload);
        }

        public static void EnchantmentsInformation(Character character, Dictionary<string, string> arguments)
        {
            ChatPanel panel = character?.CharacterUI?.ChatPanel;

            if(panel == null)
            {
                OBCC.LogMessage("EventBusPublisher@EnchantmentsInformation Tried to use missing chatPanel.");
                return;
            }

            arguments.TryGetValue("short", out string listIsShort);
            List<Enchantment> enchantments = ResourcesPrefabManager.ENCHANTMENT_PREFABS.Values.ToList();

            if(string.IsNullOrWhiteSpace(listIsShort) || !string.Equals(listIsShort, "true"))
            {
                EnchantmentRecipesHelpers.PrintEnchantments(panel, enchantments);
            }

            ChatHelpers.SendChatLog(panel, $"Total {enchantments.Count()} Enchantments.");
        }

        public static void SendAddEnchantmentRecipesCommand()
        {
            Dictionary<string, (string, string)> parameters = new()
            {
                {
                    "short",
                    ("Optional. Can be provided as true or anything else. If it is true provides count of all available EnchantmentRecipes count", "false")
                }
            };

            Action<Character, Dictionary<string, string>> function = EnchantmentRecipesInformation;

            var payload = new EventPayload
            {
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandName).key] = "enchantmentRecipes",
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandParameters).key] = parameters,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandAction).key] = function,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandDescription).key] = "Provides information about all enchantment recipes."
            };

            EventBus.Publish(EventBusPublisher.ChatCommands_Listener, EventBusPublisher.Event_AddCommand, payload);
        }

        public static void EnchantmentRecipesInformation(Character character, Dictionary<string, string> arguments)
        {
            ChatPanel panel = character?.CharacterUI?.ChatPanel;

            if(panel == null)
            {
                OBCC.LogMessage("EventBusPublisher@EnchantmentRecipesInformation Tried to use missing chatPanel.");
                return;
            }

            arguments.TryGetValue("short", out string listIsShort);
            List<EnchantmentRecipe> enchantmentRecipes = RecipeManager.Instance.GetEnchantmentRecipes();

            if(string.IsNullOrWhiteSpace(listIsShort) || !string.Equals(listIsShort, "true"))
            {
                EnchantmentRecipesHelpers.PrintEnchantmentRecipes(panel, enchantmentRecipes);
            }

            ChatHelpers.SendChatLog(panel, $"Total {enchantmentRecipes.Count()} EnchantmentRecipes.");
        }

        public static void SendAddEnchantmentRecipeItemsCommand()
        {
            Dictionary<string, (string, string)> parameters = new()
            {
                {
                    "short",
                    ("Optional. Can be provided as true or anything else. If it is true provides count of all available EnchantmentRecipeItems count", "false")
                }
            };

            Action<Character, Dictionary<string, string>> function = EnchantmentRecipeItemsInformation;

            var payload = new EventPayload
            {
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandName).key] = "enchantmentRecipeItems",
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandParameters).key] = parameters,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandAction).key] = function,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandDescription).key] = "Provides information about all enchantment recipe items."
            };

            EventBus.Publish(EventBusPublisher.ChatCommands_Listener, EventBusPublisher.Event_AddCommand, payload);
        }

        public static void EnchantmentRecipeItemsInformation(Character character, Dictionary<string, string> arguments)
        {
            ChatPanel panel = character?.CharacterUI?.ChatPanel;

            if(panel == null)
            {
                OBCC.LogMessage("EventBusPublisher@EnchantmentRecipeItemsInformation Tried to use missing chatPanel.");
                return;
            }

            arguments.TryGetValue("short", out string listIsShort);
            HashSet<EnchantmentRecipeItem> enchantmentItems = EnchantmentRecipesHelpers.GetAllEnchantmentRecipeItems();

            if(string.IsNullOrWhiteSpace(listIsShort) || !string.Equals(listIsShort, "true"))
            {
                EnchantmentRecipesHelpers.PrintEnchantmentRecipeItems(panel, enchantmentItems);
            }

            ChatHelpers.SendChatLog(panel, $"Total {enchantmentItems.Count()} EnchantmentRecipeItems.");
        }

        public static void SendAddBrokenEnchantmentsCommand()
        {
            Dictionary<string, (string, string)> parameters = new()
            {
                {
                    "short",
                    ("Optional. Can be provided as true or anything else. If it is true provides count of all broken enchantments", "false")
                }
            };

            Action<Character, Dictionary<string, string>> function = FindBrokenEnchantments;

            var payload = new EventPayload
            {
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandName).key] = "brokenEnchantments",
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandParameters).key] = parameters,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandAction).key] = function,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandDescription).key] = "Provides information about all incorrectly added enchantments by modders."
            };

            EventBus.Publish(EventBusPublisher.ChatCommands_Listener, EventBusPublisher.Event_AddCommand, payload);
        }

        public static void FindBrokenEnchantments(Character character, Dictionary<string, string> arguments)
        {
            ChatPanel panel = character?.CharacterUI?.ChatPanel;

            if(panel == null)
            {
                OBCC.LogMessage("EventBusPublisher@FindBrokenEnchantments Tried to use missing chatPanel.");
                return;
            }

            arguments.TryGetValue("short", out string listIsShort);
            HashSet<EnchantmentRecipeItem> enchantmentItems = EnchantmentRecipesHelpers.GetAllEnchantmentRecipeItems();
            HashSet<EnchantmentRecipe> recipes = EnchantmentRecipesHelpers.GetBrokenEnchantmentRecipes();

            if(!string.IsNullOrWhiteSpace(listIsShort) && string.Equals(listIsShort, "true"))
            {
                EnchantmentRecipesHelpers.PrintBrokenEnchantmentRecipeItemsCount(panel, enchantmentItems);
                ChatHelpers.SendChatLog(panel, $"Total {recipes.Count()} broken EnchantmentRecipes.");
                return;
            }

            EnchantmentRecipesHelpers.PrintBrokenEnchantmentRecipeItems(panel, enchantmentItems);
            EnchantmentRecipesHelpers.PrintBrokenEnchantmentRecipes(panel, recipes);
        }
    }
}
