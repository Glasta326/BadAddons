using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace BadAddons.Common
{
    // Feels so good to not worry about borrowing code because i'm stealing it from myself
    public static class StringUtils
    {
        /// <summary>
        /// Gets the char bound to a ModKeybind
        /// </summary>
        /// <param name="keybind"></param>
        /// <returns></returns>
        public static string KeybindString(this ModKeybind keybind)
        {
            if (Main.dedServ || keybind is null)
            {
                return "";
            }

            List<string> keys = keybind.GetAssignedKeys();
            if (keys.Count <= 0)
            {
                return "[UNBOUND]";
            }

            else
            {
                string binds = "";
                binds += keys[0];
                for (int i = 1; i < keys.Count(); i++)
                {
                    string key = keys[i];
                    binds += "/";
                    binds += key;
                }
                return binds;
            }
        }
    }
}
