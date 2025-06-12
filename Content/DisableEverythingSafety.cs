using BadAddons.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace BadAddons.Content
{
    public class DisableEverythingSafety : ModPlayer
    {

        public override void PostUpdateMiscEffects()
        {
            if (BadAddonKeybinds.ResetAllSettingsKey.JustPressed)
            {
                BadAddonConfig.instance.EnableCameraRotation = false;
                BadAddonConfig.instance.EnableMouseJitter = false;
                BadAddonConfig.instance.EnableCursorHitbox = false;
                BadAddonConfig.instance.EnableTimeBomb = false;
                BadAddonConfig.instance.EnableVisionProblems = false;

                // Save settings
                BadAddonConfig.instance.OnChanged();
                // Why this fuck is this not public
                var type = typeof(ConfigManager);
                var method = type.GetMethod("Save", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                method.Invoke(null, new object[] { BadAddonConfig.instance });
            }
        }

    }
}
