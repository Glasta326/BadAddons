using BadAddons.Common;
using BadAddons.Core;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace BadAddons.Content.UI
{
    // This is mostly just based on how WOTG does it but without all the fancy fading shit
    public class UISystem : ModSystem
    {
        bool DontDraw => Main.gameMenu || !BadAddonConfig.instance.DisplayConfigWarning || Main.playerInventory || Main.gamePaused || Main.netMode != NetmodeID.SinglePlayer || Main.mapFullscreen;

        public override void OnModLoad()
        {
            Main.QueueMainThreadAction(() => Main.OnPostDraw += DrawText);   
        }
        public override void OnModUnload()
        {
            Main.OnPostDraw -= DrawText;
        }

        private void DrawText(Microsoft.Xna.Framework.GameTime obj)
        {
            if (DontDraw)
            {
                return;
            }


            Main.spriteBatch.Begin();
            DynamicSpriteFont font = FontAssets.DeathText.Value;
            string message = Language.GetTextValue($"Mods.BadAddons.UI.DisplayConfigMessage", BadAddonKeybinds.ResetAllSettingsKey.KeybindString());
            Main.NewText(message);
            int i = 0;
            foreach (string line in Utils.WordwrapString(message,font,(int)(Main.screenWidth/1.5f),20, out _))
            {
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                Vector2 dialogSize = font.MeasureString(line);
                Vector2 dialogDrawPosition = Main.ScreenSize.ToVector2() * 0.5f - Vector2.UnitY * 150f;
                dialogDrawPosition.Y += i * 54f;
                Vector2 dialogOrigin = dialogSize * new Vector2(0.5f, 0f);

                for (int j = 0; j < 4; j++)
                {
                    Main.spriteBatch.DrawString(font, line, dialogDrawPosition + (MathHelper.TwoPi * j / 4f).ToRotationVector2() * 2f, Color.Black, 0f, dialogOrigin, 1f, 0, 0f);
                }
                    
                Main.spriteBatch.DrawString(font, line, dialogDrawPosition, Color.AliceBlue, 0f, dialogOrigin, 1f, 0, 0f);
                i++;
            }

            Main.spriteBatch.End();
        }
    }
}
