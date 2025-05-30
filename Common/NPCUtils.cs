
using Microsoft.Xna.Framework;
using Terraria;

namespace BadAddons.Common
{
    public static class NPCUtils
    {

        public static bool AnyBossAlive()
        {
            foreach (var npc in Main.ActiveNPCs)
            {
                if (npc.boss)
                {
                    return true;
                }
            }
            return false;
        }

        public static NPC GetClosestNPC()
        {
            int closest = 0;
            float bestDist = float.PositiveInfinity;


            foreach (var npc in Main.ActiveNPCs)
            {
                float dist = Vector2.DistanceSquared(Main.LocalPlayer.Center, npc.Center);
                if (dist < bestDist)
                {
                    bestDist = dist;
                    closest = npc.whoAmI;
                }
            }

            return Main.npc[closest];
        }

        public static NPC GetClosestBoss()
        {
            int closest = 0;
            float bestDist = float.PositiveInfinity;


            foreach (var npc in Main.ActiveNPCs)
            {
                float dist = Vector2.DistanceSquared(Main.LocalPlayer.Center, npc.Center);
                if (dist < bestDist && npc.boss)
                {
                    bestDist = dist;
                    closest = npc.whoAmI;
                }
            }

            return Main.npc[closest];
        }

    }
}
