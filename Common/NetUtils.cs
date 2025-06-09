using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;

namespace BadAddons.Common
{
    /// <summary>
    /// Methods to help with multiplayer compatablitity
    /// </summary>
    internal class NetUtils
    {

        #region Client Recognition

        /// <summary>
        /// Returns true if the code is being run by the server, or if the world is in singleplayer
        /// </summary>
        public static bool IsServerOrSingleplayer()
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                return true;
            }
            if (Main.netMode == NetmodeID.Server)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true if the code is run by a local client
        /// </summary>
        /// <returns></returns>
        public static bool IsMultiplayerClient()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true ONLY on the server in mp
        /// </summary>
        /// <returns></returns>
        public static bool IsServerOnly()
        {
            return Main.netMode == NetmodeID.Server;
        }

        /// <summary>
        /// Returns true ONLY in singleplayer
        /// </summary>
        /// <returns></returns>
        public static bool IsSingleplayerOnly()
        {
            return Main.netMode == NetmodeID.SinglePlayer;
        }


        #endregion

        /// <summary>
        /// Gets the number of players active in this server
        /// </summary>
        /// <returns></returns>
        public static int CountPlayers()
        {
            int x = 0;
            foreach (var player in Main.ActivePlayers)
            {
                x++;
            }
            return x;
        }

    }
}