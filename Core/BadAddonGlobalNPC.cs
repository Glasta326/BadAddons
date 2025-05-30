using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace BadAddons.Core
{
    public class BadAddonGlobalNPC : GlobalNPC
    {
        // Disable health bars when screen rotation is a factor, because they trivialise aiming
        public override bool? DrawHealthBar(NPC npc, byte hbPosition, ref float scale, ref Vector2 position)
        {
            if (BadAddonConfig.instance.EnableCameraRotation)
            {
                return false;
            }
            return true;
        }
    }
}
