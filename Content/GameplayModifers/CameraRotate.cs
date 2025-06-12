using BadAddons.Common;
using BadAddons.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics;
using Terraria.ModLoader;

namespace BadAddons.Content.GameplayModifers
{
    internal class CameraRotate : ModSystem
    {
        private bool Disabled => !BadAddonConfig.instance.EnableCameraRotation;
        private CameraModes CameraMode => (CameraModes)BadAddonConfig.instance.CameraRotationMode;

        /// <summary>
        /// Added too <see cref="Rotation"/> every update when Cameramode is flat rotate
        /// </summary>
        private float RotateRate => MathHelper.ToRadians(BadAddonConfig.instance.CameraRotationSpeed);

        enum CameraModes
        {
            RotateMouse = 0,
            RotateBoss = 1,
            RotateFlat = 2
        }

        /// <summary>
        /// Screen will be rotated by this angle
        /// </summary>
        private float Rotation = 0f;

        public override void ModifyScreenPosition()
        {
            if (Disabled)
            {
                Rotation = 0f;
                return;
            }

            switch (CameraMode)
            {
                case CameraModes.RotateMouse:
                    Rotation = Main.LocalPlayer.AngleTo(Main.MouseWorld) + MathHelper.PiOver2; // Add pi/2 so mouse above player evens everything
                    break;
                case CameraModes.RotateBoss:
                    if (!NPCUtils.AnyBossAlive())
                    {
                        Rotation = 0f;
                        break;
                    }
                    Rotation = Main.LocalPlayer.AngleTo(NPCUtils.GetClosestBoss().Center) + MathHelper.PiOver2;
                    break;
                case CameraModes.RotateFlat:
                    Rotation += RotateRate;
                    break;
            }
            base.ModifyScreenPosition();
        }


        
        public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
        {
            // lmao this crashes when unloading so we have to not call when in menu
            if (Main.gameMenu)
            {
                return;
            }
            RotateView(Rotation, ref Transform);
            base.ModifyTransformMatrix(ref Transform);
        }

        /// <summary>
        /// Rotates the camera by a given angle
        /// </summary>
        private static void RotateView(float angle, ref SpriteViewMatrix Transform)
        {
            var type = typeof(SpriteViewMatrix);
            var field = type.GetField("_transformationMatrix", BindingFlags.NonPublic | BindingFlags.Instance);

            Matrix rotation2 = Matrix.CreateRotationZ(angle);
            Matrix translation = Matrix.CreateTranslation(new Vector3(Main.screenWidth / 2, Main.screenHeight / 2, 0));
            Matrix translation2 = Matrix.CreateTranslation(new Vector3(Main.screenWidth / -2, Main.screenHeight / -2, 0));

            field.SetValue(Transform, (translation2 * rotation2) * translation);
        }

        public override void Load()
        {
            On_SpriteViewMatrix.ShouldRebuild += UpdateMatrixFirst;
        }

        private bool UpdateMatrixFirst(On_SpriteViewMatrix.orig_ShouldRebuild orig, SpriteViewMatrix self)
        {
            if (Disabled)
            {
                return orig.Invoke(self);
            }
            return false;
        }
    }
}
