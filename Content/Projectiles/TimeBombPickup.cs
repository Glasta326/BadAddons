using BadAddons.Common;
using BadAddons.Content.GameplayModifers;
using BadAddons.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BadAddons.Content.Projectiles
{
    public class TimeBombPickup : ModProjectile
    {
        public int PickupTimeAdd => BadAddonConfig.instance.TimerAdd;


        public override string Texture => "BadAddons/Assets/TimeBombPickup";

        public override void SetDefaults()
        {
            Projectile.damage = 0;
            Projectile.knockBack = 0;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.width = 40;
            Projectile.height = 55;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (owner.GetModPlayer<TimeBomb>().ActiveNow)
            {
                Projectile.timeLeft = 3;
            }
            else
            {
                Projectile.Kill();
                Projectile.active = false;
            }

            if (owner.Hitbox.Intersects(Projectile.Hitbox))
            {
                owner.GetModPlayer<TimeBomb>().AddTime(PickupTimeAdd);
                SoundEngine.PlaySound(SoundID.ResearchComplete, Projectile.Center);

                Projectile.Kill();
                Projectile.active = false;
            }
        }


        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawpos = Projectile.Center - Main.screenPosition;

            drawpos.Y += MathF.Sin(Main.GlobalTimeWrappedHourly * 2) * 8;

            Asset<Texture2D> texture = Terraria.GameContent.TextureAssets.Projectile[ModContent.ProjectileType<TimeBombPickup>()];
            Main.spriteBatch.Draw(texture.Value, drawpos, null, Color.White, 0f, texture.Size() * 0.5f, 0.5f, SpriteEffects.None, 0f);
            return false;
        }
    }
}
