using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BadAddons.Core
{
    public class BadAddonKeybinds : ModSystem
    {
        public static ModKeybind ResetAllSettingsKey { get; private set; }

        public override void Load()
        {
            ResetAllSettingsKey = KeybindLoader.RegisterKeybind(Mod, Language.GetTextValue($"Mods.BadAddons.Keybinds.DisableAllModifers"), "R");
        }

        public override void Unload()
        {
            ResetAllSettingsKey = null;
        }
    }
}
