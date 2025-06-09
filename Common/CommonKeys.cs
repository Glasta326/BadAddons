using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadAddons.Common
{
    /// <summary>
    /// A registry of common values such as file paths to useful assets, or specific values used in many places
    /// </summary>
    public static class CommonKeys
    {
        /// <summary>
        /// The path to the invisible texture file used for when you don't want to draw something but are forced to anyway
        /// </summary>
        public readonly static string InvisibleTexturePath = "BadAddons/Assets/InvisibleTexture";

        /// <summary>
        /// Path to the generic placeholder texture
        /// </summary>
        public readonly static string PlaceholderTexturePath = "BadAddons/Assets/PlaceholderTexture";
    }
}
