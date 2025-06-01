using BadAddons.Core;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using static BadAddons.Content.GameplayModifers.CursorHitbox;
using BadAddons.Content.GameplayModifers;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using BadAddons.Common;

namespace BadAddons.Content.UI
{
    public class MouseHitboxVisual : ModSystem
    {
        /// <summary>
        /// The texture used for displaymodes has a radius of 122 pixels, as they are both 244x244
        /// </summary>
        private const float textureRadius = 122f;
        public float SizeModifer => (float)(BadAddonConfig.instance.CursorHitboxModifer) / 100f;
        public float TextureScaleRatio => textureRadius / CursorHitbox.BaseRadius;
        public Color drawColor => BadAddonConfig.instance.HitboxDisplayColor;

        public enum DisplayModes
        {
            DoNotDisplay = 0,
            DisplayRing = 1,
            DisplayCircle = 2
        }
        public static DisplayModes DisplayMode => (DisplayModes)(BadAddonConfig.instance.HitboxDisplayMode);
        bool DontDraw => Main.gameMenu || DisplayMode == DisplayModes.DoNotDisplay || Main.netMode != NetmodeID.SinglePlayer || Main.mapFullscreen;

        private static Asset<Texture2D> RingTexture;
        private static Asset<Texture2D> CircleTexture;
        public override void SetStaticDefaults()
        {
            RingTexture = ModContent.Request<Texture2D>("Badaddons/Assets/CursorRing");
            CircleTexture = ModContent.Request<Texture2D>("Badaddons/Assets/CursorCircle");
        }

        public override void OnModLoad()
        {
            Main.QueueMainThreadAction(() => Main.OnPostDraw += DrawHitbox);
        }
        public override void OnModUnload()
        {
            Main.OnPostDraw -= DrawHitbox;
        }

        private void DrawHitbox(Microsoft.Xna.Framework.GameTime obj)
        {
            if (DontDraw)
            {
                return;
            }


            Main.spriteBatch.Begin();

            DrawUtils.StartNonPremultipliedSpritebatch();

            CursorPlayer player = Main.LocalPlayer.GetModPlayer<CursorPlayer>();
            Vector2 mouseCenter = Main.MouseWorld + (new Vector2(9, 10) / Main.GameZoomTarget);

            Vector2 drawPos = mouseCenter - Main.screenPosition;

            if (DisplayMode == DisplayModes.DisplayRing)
            {
                Main.spriteBatch.Draw(RingTexture.Value, drawPos, null, drawColor, 0f, RingTexture.Value.Size() * 0.5f, SizeModifer / TextureScaleRatio , SpriteEffects.None, 0f);
            }
            if (DisplayMode == DisplayModes.DisplayCircle)
            {
                Main.spriteBatch.Draw(CircleTexture.Value, drawPos, null, drawColor, 0f, CircleTexture.Value.Size() * 0.5f, SizeModifer / TextureScaleRatio, SpriteEffects.None, 0f);
            }

            DrawUtils.StartVanillaSpritebatch();

            Main.spriteBatch.End();
        }
    }
}
