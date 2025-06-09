using BadAddons.Common;
using BadAddons.Content.Projectiles;
using BadAddons.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BadAddons.Content.GameplayModifers
{
    public class TimeBomb : ModPlayer
    {
        public const int DefaultTimerMax = 10;

        public const int DefaultMaxSpawnRadius = 22;

        public const int DefaultMinSpawnRadius = 10;

        public bool Disabled => !BadAddonConfig.instance.EnableTimeBomb || Player.dead || Player.respawnTimer > 0;
        private bool OnlyBossfight => BadAddonConfig.instance.OnlyBossfight;

        public bool ActiveNow = false;

        public int TimerMax => BadAddonConfig.instance.TimerMax;


        public int MaxSpawnRadius => BadAddonConfig.instance.MaxSpawnRadius;
        public int MinSpawnRadius => BadAddonConfig.instance.MinSpawnRadius;

        // I cant do TimeLeft = TimerMax for some reason so whatever
        public int TimeLeft = BadAddonConfig.instance.TimerMax;
        public int FrameCounter = 60;

        public override void SetStaticDefaults()
        {
            ActiveNow = false;
            TimeLeft = TimerMax;
            FrameCounter = 60;
        }

        public override void PreUpdate()
        {
            ActiveNow = false;
            if (Disabled)
            {
                TimeLeft = TimerMax;
                FrameCounter = 60;
                return;
            }

            if (OnlyBossfight)
            {
                bool active = NPCUtils.AnyBossAlive();
                if (!active)
                {
                    TimeLeft = TimerMax;
                    FrameCounter = 60;
                    return;
                }
            }
            ActiveNow = true;

            if (Player.ownedProjectileCounts[ModContent.ProjectileType<TimeBombPickup>()] < 1)
            {
                float dist = Main.rand.NextFloat(MinSpawnRadius, MaxSpawnRadius) * 16;

                Vector2 spawnpos = Player.Center + new Vector2(0, -dist).RotatedByRandom(MathHelper.TwoPi);

                EntityUtils.NewProjectilePlayer(spawnpos, Vector2.Zero, ModContent.ProjectileType<TimeBombPickup>(), 0, 0f);
            }


            // we ignore the framecounter because the framecounter's second long duration is accounted for in the max value of the timer. the INSTANT timeleft is 0, you're fucked
            if (TimeLeft == 0)
            {
                KillPlayer(this.Player);
            }

            if (TimeLeft > 0)
            {
                FrameCounter--;
                if (FrameCounter <= 0)
                {
                    FrameCounter = 60;
                    TimeLeft--;
                }
            }
        }


        public void AddTime(int seconds)
        {
            // Reset the frame counter so the player gets the full second and not like 5 frames of it lol
            // Imagine getting the pickup and seeing the countdown go from 10 to 9 instantly because framecounter was 2 or smth
            FrameCounter = 60;

            TimeLeft += seconds;
            TimeLeft = (int)MathHelper.Clamp(TimeLeft, 0, TimerMax); // dont overcap or anything smh
        }

        public void KillPlayer(Player player)
        {
            NetworkText deathreason = Language.GetText($"Mods.BadAddons.UI.TimeBombDeathMessage").ToNetworkText(new object[] {Player.name});

            // Only reason i dont do Player.kill is so i can live in godmode for testing
            Player.Hurt(PlayerDeathReason.ByCustomReason(deathreason), 12504, 1, dodgeable: false, armorPenetration: 999999);
            player.immuneTime = 0;
        }
    }
}
