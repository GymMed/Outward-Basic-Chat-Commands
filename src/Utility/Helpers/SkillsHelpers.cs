using OutwardBasicChatCommands.Utility.Data;
using OutwardBasicChatCommands.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardBasicChatCommands.Utility.Helpers
{
    public static class SkillsHelpers
    {
        public static void LogSkillsData(ChatPanel panel, SkillTypes category)
        {
            switch(category)
            {
                case SkillTypes.Active:
                    {
                        List<Skill> activeSkills = GetActiveSkills();
                        PrintSkillsAsData(panel, activeSkills);
                        ChatHelpers.SendChatLog(panel, $"Total {activeSkills.Count()} active skills!", ChatLogStatus.Success);
                        break;
                    }
                case SkillTypes.Passive:
                    {
                        List<Skill> passiveSkills = GetPassiveSkills();
                        PrintSkillsAsData(panel, passiveSkills);
                        ChatHelpers.SendChatLog(panel, $"Total {passiveSkills.Count()} passive skills!", ChatLogStatus.Success);
                        break;
                    }
                case SkillTypes.Cosmetic:
                    {
                        List<Skill> cosmeticSkills = GetCosmeticSkills();
                        PrintSkillsAsData(panel, cosmeticSkills);
                        ChatHelpers.SendChatLog(panel, $"Total {cosmeticSkills.Count()} cosmetic skills!", ChatLogStatus.Success);
                        break;
                    }
                case SkillTypes.All:
                default:
                    {
                        SkillsData data = GetSkillsData();

                        int totalActive = data.ActiveSkills.Count();
                        int totalPassive = data.PassiveSkills.Count();
                        int totalCosmetic = data.CosmeticSkills.Count();

                        ChatHelpers.SendChatLog(panel, $"Total {totalActive} active skills!", ChatLogStatus.Success);
                        ChatHelpers.SendChatLog(panel, $"Total {totalPassive} passive skills!", ChatLogStatus.Success);
                        ChatHelpers.SendChatLog(panel, $"Total {totalCosmetic} cosmetic skills!", ChatLogStatus.Success);
                        ChatHelpers.SendChatLog(panel, $"Total {totalActive + totalPassive + totalCosmetic} skills!", ChatLogStatus.Success);
                        break;
                    }
            }
        }

        public static void PrintSkillsAsData(ChatPanel panel, IEnumerable<Skill> skills)
        {
            foreach(var skill in skills)
            {
                ChatHelpers.SendChatLog(panel, $"{skill.Name} {skill.ItemID}!");
            }
        }

        public static SkillsData GetSkillsData()
        {
            List<Skill> activeSkills = new();
            List<Skill> passiveSkills = new();
            List<Skill> cosmeticSkills = new();

            foreach (KeyValuePair<string, Item> itemPair in ResourcesPrefabManager.ITEM_PREFABS)
            {
                if (itemPair.Value is Skill skill)
                {
                    if (skill.IsPassive)
                    {
                        passiveSkills.Add(skill);
                        continue;
                    }

                    if(skill.IsCosmetic)
                    {
                        cosmeticSkills.Add(skill);
                        continue;
                    }
                    activeSkills.Add(skill);
                }
            }

            return new SkillsData(activeSkills, passiveSkills, cosmeticSkills);
        }

        public static List<Skill> GetActiveSkills()
        {
            List<Skill> skills = new();

            foreach (KeyValuePair<string, Item> itemPair in ResourcesPrefabManager.ITEM_PREFABS)
            {
                if (itemPair.Value is Skill skill)
                {
                    if (skill.IsPassive || skill.IsCosmetic)
                    {
                        continue;
                    }
                    skills.Add(skill);
                }
            }

            return skills;
        }

        public static List<Skill> GetPassiveSkills()
        {
            List<Skill> skills = new();

            foreach (KeyValuePair<string, Item> itemPair in ResourcesPrefabManager.ITEM_PREFABS)
            {
                if (itemPair.Value is Skill skill)
                {
                    if (skill.IsPassive)
                    {
                        skills.Add(skill);
                    }
                }
            }

            return skills;
        }

        public static List<Skill> GetCosmeticSkills()
        {
            List<Skill> skills = new();

            foreach (KeyValuePair<string, Item> itemPair in ResourcesPrefabManager.ITEM_PREFABS)
            {
                if (itemPair.Value is Skill skill)
                {
                    if (skill.IsCosmetic)
                    {
                        skills.Add(skill);
                    }
                }
            }

            return skills;
        }
    }
}
