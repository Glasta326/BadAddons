using BadAddons.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

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
            }
        }

    }
}
