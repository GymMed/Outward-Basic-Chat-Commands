using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using OutwardModsCommunicator;

namespace OutwardBasicChatCommands.Utility.Helpers
{
    public static class ConfigExporter
    {
        public static ConfigOverrides ExportAllConfigs()
        {
            var root = new ConfigOverrides();

            foreach (var plugin in BepInEx.Bootstrap.Chainloader.PluginInfos.Values)
            {
                var config = plugin.Instance?.Config;
                if (config == null || config.Count == 0)
                    continue;

                var mod = new ModOverride
                {
                    ModGUID = plugin.Metadata.GUID
                };

                foreach (var section in config.Keys.Select(k => k.Section).Distinct())
                {
                    var sectionOverride = new SectionOverride
                    {
                        Name = section
                    };

                    foreach (var entry in config.Where(k => k.Key.Section == section))
                    {
                        sectionOverride.Entries.Add(new EntryOverride
                        {
                            Key = entry.Key.Key,
                            Value = entry.Value.BoxedValue?.ToString() ?? ""
                        });
                    }

                    mod.Sections.Add(sectionOverride);
                }

                if (mod.Sections.Count > 0)
                    root.Mods.Add(mod);
            }

            return root;
        }
    }
}
