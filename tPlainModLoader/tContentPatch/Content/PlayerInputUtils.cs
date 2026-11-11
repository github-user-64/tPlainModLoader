using HarmonyLib;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;

namespace tContentPatch.Content
{
    /// <summary>
    /// 玩家输入工具
    /// </summary>
    [HarmonyPatch(typeof(Main))]
    public static class PlayerInputUtils
    {
        /// <summary/>
        public static readonly List<string> KeyDown = new List<string>();
        /// <summary/>
        public static readonly List<string> KeyDownOld = new List<string>();

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        internal static void UpdatePrefix(GameTime gameTime)
        {
            KeyDownOld.Clear();
            foreach (string i in KeyDown) KeyDownOld.Add(i);
            KeyDown.Clear();

            KeyDown.AddRange(PlayerInput.GetPressedKeys().ConvertAll(i => i.ToString()));
            KeyDown.AddRange(PlayerInput.MouseKeys);
        }

        /// <summary/>
        public static bool IsClick(string key)
        {
            if (IsDown(key) != true) return false;
            return IsUpOld(key);
        }

        /// <summary/>
        public static bool IsDown(string key)
        {
            if (key == null) return false;

            return KeyDown.Contains(key);
        }

        /// <summary/>
        public static bool IsUp(string key)
        {
            return IsDown(key) != true;
        }

        /// <summary/>
        public static bool IsDownOld(string key)
        {
            if (key == null) return false;

            return KeyDownOld.Contains(key);
        }

        /// <summary/>
        public static bool IsUpOld(string key)
        {
            return IsDownOld(key) != true;
        }
    }
}
