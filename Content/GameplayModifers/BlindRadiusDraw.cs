using BadAddons.Core;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.GameContent.Events;

namespace BadAddons.Content.GameplayModifers
{
    public class BlindRadiusDraw : ModSystem
    {
        public const float DefaultBlindRadiusPercent = 20f;

        private const int textureDimensions = 800;// 800x800pixels at circle radius for both

        public bool Disabled => !BadAddonConfig.instance.EnableVisionProblems;
        bool DontDraw => Main.gameMenu || Disabled || Main.netMode != NetmodeID.SinglePlayer || Main.mapFullscreen;
        public float ScaleMulti => (float)BadAddonConfig.instance.BlindRadius / 100f;

        private BlindModes DrawMode => (BlindModes)BadAddonConfig.instance.ObscureMode;

        private static Asset<Texture2D> NegativeCircleTexture;
        private static Asset<Texture2D> CircleTexture;

        enum BlindModes
        {
            CantSeeClose = 0,
            CantSeeFar = 1
        }

        public override void SetStaticDefaults()
        {
            NegativeCircleTexture = ModContent.Request<Texture2D>("Badaddons/Assets/NearSight");
            CircleTexture = ModContent.Request<Texture2D>("Badaddons/Assets/FarSight");
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseIndex = layers.FindIndex(layer => layer.Name == "Vanilla: Invasion Progress Bars");
            if (mouseIndex != -1)
            {
                layers.Insert(mouseIndex, new LegacyGameInterfaceLayer("VisionBlindRadius", delegate ()
                {
                    DrawBlindArea(Main.LocalPlayer);
                    return true;
                }, InterfaceScaleType.None));
            }
        }

        public void DrawBlindArea(Player player)
        {

            if (DontDraw)
            {
                return;
            }

            if (DrawMode == BlindModes.CantSeeClose)
            {
                Vector2 drawPos = Main.ScreenSize.ToVector2() / 2f;
                float drawScale = ((float)Main.screenWidth / textureDimensions) * ScaleMulti;

                Main.spriteBatch.Draw(CircleTexture.Value, drawPos, null, Color.White, 0f, CircleTexture.Size() * 0.5f, drawScale, SpriteEffects.None, 0f);
            }

            else if (DrawMode == BlindModes.CantSeeFar)
            {
                //taken from vanilla
                Color color = Color.Black;
                int num = (int)((float)Main.screenWidth * (ScaleMulti/2));
                int num2 = 0;

                Rectangle rect = Main.player[Main.myPlayer].getRect();
                rect.Inflate((num - rect.Width) / 1, (num - rect.Height) / 1 + num2 / 1);
                rect.Offset(-(int)Main.screenPosition.X, -(int)Main.screenPosition.Y + (int)Main.player[Main.myPlayer].gfxOffY - num2);

                Rectangle destinationRectangle = Rectangle.Union(new Rectangle(0, 0, 1, 1), new Rectangle(rect.Right - 1, rect.Top - 1, 1, 1));
                Rectangle destinationRectangle2 = Rectangle.Union(new Rectangle(Main.screenWidth - 1, 0, 1, 1), new Rectangle(rect.Right, rect.Bottom - 1, 1, 1));
                Rectangle destinationRectangle3 = Rectangle.Union(new Rectangle(Main.screenWidth - 1, Main.screenHeight - 1, 1, 1), new Rectangle(rect.Left, rect.Bottom, 1, 1));
                Rectangle destinationRectangle4 = Rectangle.Union(new Rectangle(0, Main.screenHeight - 1, 1, 1), new Rectangle(rect.Left - 1, rect.Top, 1, 1));

                Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle, (Rectangle?)new Rectangle(0, 0, 1, 1), color);
                Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle2, (Rectangle?)new Rectangle(0, 0, 1, 1), color);
                Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle3, (Rectangle?)new Rectangle(0, 0, 1, 1), color);
                Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle4, (Rectangle?)new Rectangle(0, 0, 1, 1), color);
                Main.spriteBatch.Draw(TextureAssets.Extra[49].Value, rect, color);
            }
        }
    }
}
