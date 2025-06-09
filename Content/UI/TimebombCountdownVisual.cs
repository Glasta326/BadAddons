using BadAddons.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ReLogic.Graphics;
using Terraria.GameContent;
using BadAddons.Content.GameplayModifers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BadAddons.Content.UI
{
    internal class TimebombCountdownVisual : ModSystem
    {

        bool DontDraw => Main.gameMenu || !BadAddonConfig.instance.EnableTimeBomb || !(BadAddonConfig.instance.DrawTimer && BadAddonConfig.instance.EnableTimeBomb) || Main.netMode != NetmodeID.SinglePlayer || Main.mapFullscreen;

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

            DynamicSpriteFont font = FontAssets.MouseText.Value;
            string text = Main.LocalPlayer.GetModPlayer<TimeBomb>().TimeLeft.ToString();
            Vector2 size = font.MeasureString(text);
            Vector2 drawPos = Main.LocalPlayer.Top + new Vector2(0, -40) - Main.screenPosition;
            Main.spriteBatch.DrawString(font, text, drawPos, Color.White, 0f, size * new Vector2(0.5f, 0f), 2f, SpriteEffects.None, 0f);

            Main.spriteBatch.End();
        }

    }
}
