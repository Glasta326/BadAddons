using BadAddons.Content.GameplayModifers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace BadAddons.Core
{
    [BackgroundColor(20, 65, 121, 233)]
    public class BadAddonConfig : ModConfig
    {
        public static BadAddonConfig instance => ModContent.GetInstance<BadAddonConfig>();
        public override ConfigScope Mode => ConfigScope.ClientSide;
        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message) => true;

        /// <summary>
        /// Force the player to read the text and disable the message
        /// </summary>
        [DefaultValue(true)]
        [BackgroundColor(43, 180, 203, 192)]
        public bool DisplayConfigWarning { get; set; }

        [Header("MouseTeleport")]
        #region Mouse teleport

        /// <summary>
        /// Hard toggle for mouse jittering
        /// </summary>
        [DefaultValue(false)]
        [BackgroundColor(43, 180, 203, 192)]
        public bool EnableMouseJitter { get; set; }

        /// <summary>
        /// The mouse will teleport some distance in a direction based on where it currently is, <br/>
        /// Otherwise the mouse just teleports to a random position on screen
        /// </summary>
        [DefaultValue(true)]
        [BackgroundColor(43, 180, 203, 192)]
        public bool UseRelativeOffset { get; set; }

        /// <summary>
        /// The minimum distance the mouse will teleport as a % of screen width
        /// </summary>
        [BackgroundColor(43, 180, 203, 192)]
        [SliderColor(43, 180, 203, 192)]
        [DefaultValue(MouseJitter.DefaultMinJump)]
        [Range(0, 100f)]
        [Increment(1f)]
        public float MinJump { get; set; }

        /// <summary>
        /// The maximum distance the mouse will teleport as a % of screen width
        /// </summary>
        [BackgroundColor(43, 180, 203, 192)]
        [SliderColor(43, 180, 203, 192)]
        [DefaultValue(MouseJitter.DefaultMaxJump)]
        [Range(0, 100f)]
        [Increment(1f)]
        public float MaxJump { get; set; }

        /// <summary>
        /// The maxmimum amount of frames between each teleport
        /// </summary>
        [BackgroundColor(43, 180, 203, 192)]
        [DefaultValue(MouseJitter.DefaultMaxInterval)]
        [Range(0, 3600)]
        public int MaxInterval { get; set; }

        #endregion

        [Header("CameraRotation")]
        #region CameraRotations

        /// <summary>
        /// Hard toggle for camera rotation
        /// </summary>
        [DefaultValue(false)]
        [BackgroundColor(43, 180, 203, 192)]
        public bool EnableCameraRotation { get; set; }

        /// <summary>
        /// 0 : Camera rotates based on angle to the mouse 
        /// 1 : Camera rotates based on angle to the nearest boss
        /// 2 : Camera rotates at a fixed rate
        /// </summary>
        [BackgroundColor(43, 180, 203, 192)]
        [SliderColor(43, 180, 203, 192)]
        [DefaultValue(0)]
        [Range(0, 2)]
        [Increment(1)]
        [Slider]
        [DrawTicks]
        public int CameraRotationMode { get; set; }

        /// <summary>
        /// The angle (degrees) the screen wil rotate by each frame
        /// </summary>
        [BackgroundColor(43, 180, 203, 192)]
        [DefaultValue(0f)]
        [Range(0, 60f)]
        public float CameraRotationSpeed { get; set; }

        #endregion


    }
}
