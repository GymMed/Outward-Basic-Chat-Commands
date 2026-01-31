using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardBasicChatCommands.Utility.Data
{
    public struct SkillsData
    {
        public IEnumerable<Skill> ActiveSkills;
        public IEnumerable<Skill> PassiveSkills;
        public IEnumerable<Skill> CosmeticSkills;

        public SkillsData(IEnumerable<Skill> activeSkills, IEnumerable<Skill> passiveSkills, IEnumerable<Skill> cosmeticSkills)
        {
            ActiveSkills = activeSkills;
            PassiveSkills = passiveSkills;
            CosmeticSkills = cosmeticSkills;
        }
    }
}
