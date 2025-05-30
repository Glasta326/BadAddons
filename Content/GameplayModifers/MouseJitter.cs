using BadAddons.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace BadAddons.Content.GameplayModifers
{
    public class MouseJitter : ModSystem
    {
        // Default values in the config
        public const float DefaultMinJump = 0;
        public const float DefaultMaxJump = 5f;
        public const int DefaultMaxInterval = 600;

        private static bool Disabled => !BadAddonConfig.instance.EnableMouseJitter;

        private static float MinPixels => BadAddonConfig.instance.MinJump / 100f * Main.screenWidth;
        private static float MaxPixels => BadAddonConfig.instance.MaxJump / 100f * Main.screenWidth;


        int interval = 0;
        int timer = 0;
        public override void PreUpdatePlayers()
        {
            if (Disabled)
            {
                return;
            }

            // After x frames, reset timer and choose a new x
            if (timer >= interval)
            {
                timer = 0;
                interval = Main.rand.Next(0, BadAddonConfig.instance.MaxInterval);
                MoveMouse();
            }
            timer++;
        }


        private static void MoveMouse()
        {
            if (Disabled)
            {
                return;
            }

            if (BadAddonConfig.instance.UseRelativeOffset)
            {
                // Store where it is currently
                Vector2 mPos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                // Create an offset vector based on config settings, rotated to a random angle
                Vector2 offset = (new Vector2(0, -1) * Main.rand.NextFloat(MinPixels, MaxPixels)).RotatedByRandom(MathHelper.TwoPi);
                // Apply it to get new position
                mPos += offset;
                Mouse.SetPosition((int)mPos.X, (int)mPos.Y);
            }
            else
            {
                // Choose a random 0.00f - 1.00f and multiply it by screen dimensions to get random screen coords
                float xMulti = Main.rand.NextFloat();
                float yMulti = Main.rand.NextFloat();
                int x = (int)(Main.screenWidth * xMulti);
                int y = (int)(Main.screenHeight * yMulti);

                Mouse.SetPosition(x, y);
            }
        }

    }
}
