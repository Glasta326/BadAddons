using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.Utilities;
using Terraria;
using Microsoft.Xna.Framework;

namespace BadAddons.Common
{
    public static class EntityUtils
    {
        /// <summary>
        /// Spawns a SERVER-OWNED PROJECTILE ON THE SERVER <br/>
        /// Used to spawn thigns like boss projectiles, world projectiles, traps. Anything that the player doesn't create themselves really <br/>
        /// Sync code for these projectiles should only be run on the server. Any non-deterministic act should also be synced on the server. <br/>
        /// TLDR : Only call this code that runs in singleplayer or on the server
        /// </summary>
        /// <param name="center">The position of the spawned projectile</param>
        /// <param name="velocity">The velocity the projectile will spawn in</param>
        /// <param name="type">The type of projectile to spawn</param>
        /// <param name="damage">The damage this projectile will deal on hit</param>
        /// <param name="knockback">Knockback strength of this projectile. 0 means no knockback and 1 means maximum strength</param>
        /// <param name="ai0">Sets the Projectile AI[0] parameter</param>
        /// <param name="ai1">Sets the Projectile AI[1] parameter</param>
        /// <param name="ai2">Sets the Projectile AI[2] parameter</param>
        /// <param name="owner">Best left default. Hope you know what you're doing if you set this to anything else</param>
        /// <param name="source">Info about how this projectile was spawned</param>
        public static Projectile NewProjectileWorld(Vector2 center, Vector2 velocity, int type, int damage, float knockback, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f, int owner = 255, IEntitySource source = null)
        {
            // Set source if not provided
            if (source is null)
            {
                source = new EntitySource_WorldEvent();
            }

            // This should never be being called on a client. Only singleplayer or server code
            if (NetUtils.IsMultiplayerClient())
            {
                return null;
            }

            // In singleplayer, the player is the server and client
            if (NetUtils.IsSingleplayerOnly())
            {
                owner = Main.myPlayer;
            }

            // Damage jank
            // We love vanilla!
            damage = (int)(damage * 0.5f);
            if (Main.expertMode)
            {
                damage = (int)(damage * 0.5f);
            }

            int t = Projectile.NewProjectile(source, center.X, center.Y, velocity.X, velocity.Y, type, damage, knockback, owner, ai0, ai1, ai2);

            // Call a sync if this is being run in multiplayer
            if (NetUtils.IsServerOnly())
            {
                if (t >= 0 && t < Main.maxProjectiles)
                {
                    Main.projectile[t].netUpdate = true;
                }
            }

            return Main.projectile[t];
        }


        // My testing of these 2 projectile methods is like 30s in multiplayer with only the client spawner and i'm just going to assume they work
        // If future glasta is reading this, mad because he only just figured out these functions were the problem:
        // LMAO idiot get fucked!!!! <3

        /// <summary>
        /// Spawns a PLAYER-OWNED PROJECTILE FROM THIS CLIENT <br/>
        /// Used to spawn things like weapon projectiles, minions, minion projectiles. Anything that makes sense to be "player owned" <br/>
        /// Remember player-owned projectiles are well, owned by the player. Sync code should run on all? clients. Not just the server. Any non-deterministic act should also be synced from the client to everyone.
        /// </summary>
        /// <param name="center">The position of the spawned projectile</param>
        /// <param name="velocity">The velocity the projectile will spawn in</param>
        /// <param name="type">The type of projectile to spawn</param>
        /// <param name="damage">The damage this projectile will deal on hit</param>
        /// <param name="knockback">Knockback strength of this projectile. 0 means no knockback and 1 means maximum strength</param>
        /// <param name="ai0">Sets the Projectile AI[0] parameter</param>
        /// <param name="ai1">Sets the Projectile AI[1] parameter</param>
        /// <param name="ai2">Sets the Projectile AI[2] parameter</param>
        /// <param name="owner">Best left default. Make sure to check if player.whoami == main.myplayer </param>
        /// <param name="source">Info about how this projectile was spawned</param>
        public static Projectile NewProjectilePlayer(Vector2 center, Vector2 velocity, int type, int damage, float knockback, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f, int owner = -1, IEntitySource source = null)
        {
            // Set source if not provided
            if (source is null)
            {
                source = new EntitySource_WorldEvent();
            }

            // This should only be called in singleplayer and multiplayer clients
            if (NetUtils.IsServerOnly())
            {
                return null;
            }

            if (owner == -1)
            {
                owner = Main.myPlayer;
            }

            int t = Projectile.NewProjectile(source, center.X, center.Y, velocity.X, velocity.Y, type, damage, knockback, owner, ai0, ai1, ai2);

            if (t >= 0 && t < Main.maxProjectiles)
            {
                Main.projectile[t].netUpdate = true;
            }

            return Main.projectile[t];
        }
    }
}
