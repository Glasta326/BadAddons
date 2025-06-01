using BadAddons.Common;
using BadAddons.Core;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Terraria.UI;

namespace BadAddons.Content.GameplayModifers
{
    public class CursorHitbox : ModSystem
    {
        public bool Disabled => !BadAddonConfig.instance.EnableCursorHitbox;

        public bool AccurateHitbox => BadAddonConfig.instance.AccurateHitboxes;
        public float SizeModifer => (float)(BadAddonConfig.instance.CursorHitboxModifer)/100f;
        public const float BaseRadius = 9.5f;

        


        public enum HitboxModes
        {
            CursorOnly = 0,
            CursorAndPlayer = 1
        }

        public override void PreUpdatePlayers()
        {
            if (Disabled)
            {
                return;
            }

            // Set the hitbox into place
            CursorPlayer player = Main.LocalPlayer.GetModPlayer<CursorPlayer>();
            Vector2 mouseCenter = Main.MouseWorld + (new Vector2(9, 10) / Main.GameZoomTarget);
            player.CursorHitbox.Center = mouseCenter;
            player.CursorHitbox.Radius = BaseRadius * SizeModifer;

            #region Main cases

            foreach (Projectile p in Main.ActiveProjectiles)
            {
                if (AccurateHitbox)
                {
                    // Gets the point on the edge of the circle closest to the hitbox, and as soon as that overlaps with the hitbox it triggers collision
                    // Still not "perfect" collision, but great compromise between accuracy and runtime
                    Vector2 point1 = player.CursorHitbox.ClosestPointOnEdge(p.Hitbox.Center());
                    if (p.Hitbox.Contains((int)point1.X,(int)point1.Y))
                    {
                        HitPlayer(player.Player,p, player.CursorHitbox.Center);
                    }
                }
                else
                {
                    // Just checks if the hitbox center is inside the circle
                    if (player.CursorHitbox.Contains(p.Hitbox.Center.ToVector2()))
                    {
                        HitPlayer(player.Player, p, player.CursorHitbox.Center);
                    }
                }
                // After hit, 99% of entities will give the player some immunity frames,
                // if any iframes have been given, there's no way to hit the player again for atleast this frame,
                // so we can cancel the checks early if the player has iframes
                if (player.Player.immuneTime > 0)
                {
                    break;
                }
            }
            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (AccurateHitbox)
                {
                    // Gets the point on the edge of the circle closest to the hitbox, and as soon as that overlaps with the hitbox it triggers collision
                    // Still not "perfect" collision, but great compromise between accuracy and runtime
                    Vector2 point1 = player.CursorHitbox.ClosestPointOnEdge(npc.Hitbox.Center());
                    if (npc.Hitbox.Contains((int)point1.X, (int)point1.Y))
                    {
                        HitPlayer(player.Player, npc, player.CursorHitbox.Center);
                    }
                }
                else
                {
                    // Just checks if the hitbox center is inside the circle
                    if (player.CursorHitbox.Contains(npc.Hitbox.Center.ToVector2()))
                    {
                        HitPlayer(player.Player, npc, player.CursorHitbox.Center);
                    }
                }
                // After hit, 99% of entities will give the player some immunity frames,
                // if any iframes have been given, there's no way to hit the player again for atleast this frame,
                // so we can cancel the checks early if the player has iframes
                if (player.Player.immuneTime > 0)
                {
                    break;
                }
            }

            #endregion

            #region Niche cases

            // Lava
            // We need to use a rectangle for collision, but we want the rectangle to represent the circle as best as possible
            // if the rectangle goes outside the circle that would be stupid, so we need a circumscribed circle 
            // see https://www.omnicalculator.com/math/square-in-a-circle for visual

            Player thisPlayer = player.Player;
            float x = MathF.Sqrt(player.CursorHitbox.Radius) / 1.4142135623730950f; // SqrtR / Sqrt2
            if (Collision.LavaCollision(player.CursorHitbox.Center + new Vector2(-x, -x), (int)x * 2, (int)x * 2)) // Is is half width of square
            {
                // This is done once first because there's no way to stop the else condition that fills up lavatime again when the player is out of lava so this just counteracts it
                // This does mean if the player AND the player's cursor is in lava then it will drain twice as fast but honestly that makes sense so its a feature
                if (thisPlayer.lavaTime > 0)
                {
                    thisPlayer.lavaTime--;
                }

                // Taken from line 26173 to 26216 of Player.cs
                if (!thisPlayer.lavaImmune && Main.myPlayer == thisPlayer.whoAmI && thisPlayer.hurtCooldowns[4] <= 0)
                {
                    if (thisPlayer.lavaTime > 0)
                    {
                        thisPlayer.lavaTime--;
                    }
                    else
                    {
                        int num94 = 80;
                        int num95 = 420;
                        if (Main.remixWorld)
                        {
                            num94 = 200;
                            num95 = 630;
                        }
                        if (!thisPlayer.ashWoodBonus || !thisPlayer.lavaRose)
                        {
                            if (thisPlayer.ashWoodBonus)
                            {
                                if (Main.remixWorld)
                                {
                                    num94 = 145;
                                }
                                num94 /= 2;
                                num95 -= 210;
                            }
                            if (thisPlayer.lavaRose)
                            {
                                num94 -= 45;
                                num95 -= 210;
                            }
                            if (num94 > 0)
                            {
                                thisPlayer.Hurt(PlayerDeathReason.ByOther(2), num94, 0, pvp: false, quiet: false, 4);
                            }
                            if (num95 > 0)
                            {
                                thisPlayer.AddBuff(24, num95);
                            }
                        }
                    }
                }
                thisPlayer.lavaWet = true;

            }

            // Water
            // Adapted from CheckDrowning()
            if (Collision.DrownCollision(player.CursorHitbox.Center + new Vector2(-x, -x), (int)x * 2, (int)x * 2))
            {
                bool flag = true;
                if (thisPlayer.gills)
                {
                    flag = Main.getGoodWorld && !flag;
                }
                if (thisPlayer.shimmering)
                {
                    flag = false;
                }
                if (thisPlayer.mount.Active && thisPlayer.mount.Type == 4)
                {
                    flag = false;
                }
                if (Main.myPlayer == thisPlayer.whoAmI)
                {
                    if (thisPlayer.accMerman)
                    {
                        if (flag)
                        {
                            thisPlayer.merman = true;
                        }
                        flag = false;
                    }
                    if (flag)
                    {
                        if (Main.rand.NextBool())
                        {
                            thisPlayer.breath--;
                        }
                        if (thisPlayer.breath == 0)
                        {
                            SoundEngine.PlaySound(SoundID.Drown);
                        }
                        if (thisPlayer.breath <= 0)
                        {
                            thisPlayer.lifeRegenTime = 0f;
                            thisPlayer.breath = 0;
                            thisPlayer.statLife -= 2;
                            if (thisPlayer.statLife <= 0)
                            {
                                thisPlayer.statLife = 0;
                                thisPlayer.KillMe(PlayerDeathReason.ByOther(1), 10.0, 0);
                            }
                        }
                    }
                }
                if (flag && Main.rand.NextBool(20) && !thisPlayer.lavaWet && !thisPlayer.honeyWet)
                {
                    int num4 = 0;
                    if (thisPlayer.gravDir == -1f)
                    {
                        num4 += thisPlayer.height - 12;
                    }
                    Dust.NewDust(player.CursorHitbox.Center, (int)x, (int)x, DustID.BreatheBubble, 0f, 0f, 0, default(Color), 1.2f);
                }
                // Like with lava, there's no way to disable the player's check for being "out of water therefor breath go up", so we just counteract it with numbers,
                // Also like with lava, player should have 2x breath loss if both cursor and player are underwater
                thisPlayer.breath -= 3;
            }

            #endregion
        }

        private void HitPlayer(Player player, Entity e, Vector2 centerPos)
        {
            if (e is Projectile projectile)
            {
                if (projectile.friendly || projectile.damage <= 0 || !projectile.active)
                {
                    return;
                }
                if (projectile.hostile && projectile.damage >= 0 && projectile.active)
                {
                    player.Hurt(PlayerDeathReason.ByProjectile(player.whoAmI, projectile.owner), projectile.damage, projectile.Center.X > centerPos.X ? -1 : 1, dodgeable: projectile.IsDamageDodgable(), armorPenetration: projectile.ArmorPenetration, knockback: projectile.knockBack);
                }
            }
            if (e is NPC npc)
            {
                if (npc.friendly || npc.damage <= 0 || !npc.active)
                {
                    return;
                }
                if (npc.damage >= 0 && npc.active)
                {
                    player.Hurt(PlayerDeathReason.ByNPC(npc.whoAmI), npc.damage, npc.Center.X > centerPos.X ? -1 : 1, dodgeable: npc.IsDamageDodgeable());
                }
            }
        }


    }

    public class CursorPlayer : ModPlayer
    {
        public Circle CursorHitbox;

        public override void OnEnterWorld()
        {
            CursorHitbox = Circle.Empty;
        }
    }
}
