using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardBasicChatCommands.Utility.Helpers
{
    public static class EnchantmentRecipesHelpers
    {
        public static HashSet<EnchantmentRecipeItem> GetAllEnchantmentRecipeItems()
        {
            HashSet<EnchantmentRecipeItem> enchantmentRecipeItems = new();

            foreach(KeyValuePair<string, Item> itemPrefab in ResourcesPrefabManager.ITEM_PREFABS)
            {
                if(itemPrefab.Value is EnchantmentRecipeItem enchantmentItem)
                {
                    enchantmentRecipeItems.Add(enchantmentItem);                   
                }
            }

            return enchantmentRecipeItems;
        }

        public static void PrintBrokenEnchantmentRecipeItems(ChatPanel panel, IEnumerable<EnchantmentRecipeItem> enchantmentItems)
        {
            if(panel == null)
            {
                OBCC.LogMessage("EnchantmentCommandsPublisher@PrintBrokenEnchantmentRecipeItems Tried to use missing chatPanel.");
                return;
            }

            ChatHelpers.SendChatLog(panel, "EnchantmentRecipeItems");
            ChatHelpers.SendChatLog(panel, "Name RecipesCount ID!", Enums.ChatLogStatus.Success);

            foreach (var item in enchantmentItems)
            {
                if(item.Recipes.Count() < 1)
                    ChatHelpers.SendChatLog(panel, $"{item.GetLocalizedName()} {item.Recipes.Count()} {item.ItemID}!", Enums.ChatLogStatus.Warning);
            }
        }

        public static void PrintBrokenEnchantmentRecipeItemsCount(ChatPanel panel, IEnumerable<EnchantmentRecipeItem> enchantmentItems)
        {
            if(panel == null)
            {
                OBCC.LogMessage("EnchantmentCommandsPublisher@PrintBrokenEnchantmentRecipeItemsCount Tried to use missing chatPanel.");
                return;
            }

            int broken = 0;

            foreach (var item in enchantmentItems)
            {
                if (item.Recipes.Count() < 1)
                    broken++;
            }
            ChatHelpers.SendChatLog(panel, $"Total {broken} broken EnchantmentRecipeItems");
        }

        public static HashSet<EnchantmentRecipe> GetBrokenEnchantmentRecipes()
        {
            HashSet<EnchantmentRecipe> recipes = new();

            foreach (var item in RecipeManager.Instance.GetEnchantmentRecipes())
            {
                var enchantment = ResourcesPrefabManager.Instance.GetEnchantmentPrefab(item.ResultID);

                if (enchantment == null)
                    recipes.Add(item);
            }

            return recipes;
        }

        public static void PrintBrokenEnchantmentRecipes(ChatPanel panel, IEnumerable<EnchantmentRecipe> enchantmentRecipes)
        {
            if(panel == null)
            {
                OBCC.LogMessage("EnchantmentCommandsPublisher@PrintBrokenEnchantmentRecipes Tried to use missing chatPanel.");
                return;
            }

            ChatHelpers.SendChatLog(panel, "EnchantmentRecipes");
            ChatHelpers.SendChatLog(panel, "Name ID!", Enums.ChatLogStatus.Success);

            foreach (var item in enchantmentRecipes)
            {
                ChatHelpers.SendChatLog(panel, $"{item.name} {item.ResultID}!", Enums.ChatLogStatus.Warning);
            }
        }

        public static void PrintEnchantmentRecipeItems(ChatPanel panel, IEnumerable<EnchantmentRecipeItem> enchantmentItems)
        {
            if(panel == null)
            {
                OBCC.LogMessage("EnchantmentCommandsPublisher@PrintEnchantmentRecipeItems Tried to use missing chatPanel.");
                return;
            }

            ChatHelpers.SendChatLog(panel, "Name RecipesCount ID!");

            foreach (var item in enchantmentItems)
            {
                ChatHelpers.SendChatLog(panel, $"{item.GetLocalizedName()} {item.Recipes.Count()} {item.ItemID}!");
            }
        }

        public static void PrintEnchantmentRecipes(ChatPanel panel, IEnumerable<EnchantmentRecipe> enchantmentRecipes)
        {
            if(panel == null)
            {
                OBCC.LogMessage("EnchantmentCommandsPublisher@PrintEnchantmentRecipes Tried to use missing chatPanel.");
                return;
            }

            ChatHelpers.SendChatLog(panel, "Name RecipeID | Enchantment EnchantmentID!");

            foreach (var item in enchantmentRecipes)
            {
                var enchantment = ResourcesPrefabManager.Instance.GetEnchantmentPrefab(item.ResultID);

                if(enchantment != null)
                    ChatHelpers.SendChatLog(panel, $"{item.name} {item.RecipeID} | {enchantment.Name} {item.ResultID}!");
                else
                    ChatHelpers.SendChatLog(panel, $"{item.name} {item.RecipeID} | null {item.ResultID}!");
            }
        }

        public static void PrintEnchantments(ChatPanel panel, IEnumerable<Enchantment> enchantments)
        {
            if(panel == null)
            {
                OBCC.LogMessage("EnchantmentCommandsPublisher@PrintEnchantments Tried to use missing chatPanel.");
                return;
            }

            ChatHelpers.SendChatLog(panel, "Name ID | Description!");

            foreach (var item in enchantments)
            {
                if(!string.IsNullOrEmpty(item.Description))
                    ChatHelpers.SendChatLog(panel, $"{item.Name} {item.PresetID} | {item.Description}!");
                else
                    ChatHelpers.SendChatLog(panel, $"{item.Name} {item.PresetID}!");
            }
        }
    }
}
