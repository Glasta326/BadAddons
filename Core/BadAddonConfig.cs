using BadAddons.Content.GameplayModifers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
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

        public override void OnChanged()
        {
            // Refresh bomb countdown
            Main.LocalPlayer.GetModPlayer<TimeBomb>().AddTime(TimerMax);
        }

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

        [Header("CursorHitbox")]
        #region Cursor hitbox

        /// <summary>
        /// Hard toggle for cursor hitbox
        /// </summary>
        [DefaultValue(false)]
        [BackgroundColor(43, 180, 203, 192)]
        public bool EnableCursorHitbox { get; set; }

        /// <summary>
        /// % modifer for the cursor's hitbox size
        /// </summary>
        [BackgroundColor(43, 180, 203, 192)]
        [SliderColor(43, 180, 203, 192)]
        [DefaultValue(100)]
        [Range(1, 1000)]
        [Increment(25)]
        [Slider]
        [DrawTicks]
        public int CursorHitboxModifer { get; set; }

        /// <summary>
        /// When toggled on, the collision checks for any points of a hitbox intersecting with the mouse, rather than just the hitbox's center
        /// Think of it like touhou collision vs avernus collision
        /// </summary>
        [DefaultValue(false)]
        [BackgroundColor(43, 180, 203, 192)]
        public bool AccurateHitboxes { get; set; }

        /// <summary>
        /// 0 : Does not display hitbox
        /// 1 : Displays a ring around the cursor
        /// 2 : Displays a full circle around the cursor
        /// </summary>
        [BackgroundColor(43, 180, 203, 192)]
        [SliderColor(43, 180, 203, 192)]
        [DefaultValue(0)]
        [Range(0, 2)]
        [Increment(1)]
        [Slider]
        [DrawTicks]
        public int HitboxDisplayMode { get; set; }


        /// <summary>
        /// Determines the color <see cref="HitboxDisplayMode"/> would use <br/>
        /// </summary>
        [BackgroundColor(43, 180, 203, 192)]
        [DefaultValue(typeof(Color), "255, 0, 0, 200")]
        public Color HitboxDisplayColor { get; set; }

        #endregion

        [Header("TimeBomb")]
        #region TimeBomb

        /// <summary>
        /// Hard toggle for the Time bomb
        /// </summary>
        [DefaultValue(false)]
        [BackgroundColor(43, 180, 203, 192)]
        public bool EnableTimeBomb { get; set; }

        /// <summary>
        /// Only active during bossfights
        /// </summary>
        [DefaultValue(false)]
        [BackgroundColor(43, 180, 203, 192)]
        public bool OnlyBossfight { get; set; }

        /// <summary>
        /// Draws a number above the player indicating how long is left
        /// </summary>
        [DefaultValue(false)]
        [BackgroundColor(43, 180, 203, 192)]
        public bool DrawTimer { get; set; }

        /// <summary>
        /// The maximum value of the countdown timer (seconds). Will start at this once enabled
        /// </summary>
        [BackgroundColor(43, 180, 203, 192)]
        [DefaultValue(TimeBomb.DefaultTimerMax)]
        public int TimerMax { get; set; }

        /// <summary>
        /// How much time (seconds) is restored to the countdown. 
        /// </summary>
        [BackgroundColor(43, 180, 203, 192)]
        [DefaultValue(TimeBomb.DefaultTimerMax)] // Default behaviour restores it to the maximum
        public int TimerAdd { get; set; }

        /// <summary>
        /// The maximum radius (Tiles) away the timer pickup can spawn
        /// </summary>
        [BackgroundColor(43, 180, 203, 192)]
        [DefaultValue(TimeBomb.DefaultMaxSpawnRadius)]
        public int MaxSpawnRadius { get; set; }

        /// <summary>
        /// The minimum radius (Tiles) away the timer pickup can spawn
        /// </summary>
        [BackgroundColor(43, 180, 203, 192)]
        [DefaultValue(TimeBomb.DefaultMinSpawnRadius)]
        public int MinSpawnRadius { get; set; }

        #endregion
    }
}
