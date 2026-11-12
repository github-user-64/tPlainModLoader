using HarmonyLib;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;

namespace tContentPatch.Content
{
    /// <summary>
    /// 输入工具
    /// </summary>
    [HarmonyPatch(typeof(PlayerInput))]
    public static class InputUtils
    {
        /// <summary>
        /// 鼠标左
        /// </summary>
        public static readonly string KeyMouseLeft = "Mouse1";
        /// <summary>
        /// 鼠标右
        /// </summary>
        public static readonly string KeyMouseRight = "Mouse2";

        /// <summary/>
        public static readonly List<string> KeyDown = new List<string>();
        /// <summary/>
        public static readonly List<string> KeyDownOld = new List<string>();

        /// <summary>
        /// 光标左键按下<para/>
        /// 和<see cref="IsDown(Keys)"/>的区别是会收到按键绑定影响
        /// </summary>
        public static bool MouseLeft { get; private set; } = false;
        /// <summary>
        /// 光标左键第一次按下
        /// </summary>
        public static bool MouseLeftClick { get; private set; } = false;
        /// <summary>
        /// 光标右键按下
        /// </summary>
        public static bool MouseRight { get; private set; } = false;
        /// <summary>
        /// 光标右键第一次按下
        /// </summary>
        public static bool MouseRightClick { get; private set; } = false;

        [HarmonyPatch("UpdateInput")]
        [HarmonyPostfix]
        internal static void UpdateInputPostfix()
        {
            MouseLeft = Main.mouseLeft;
            MouseLeftClick = Main.mouseLeft && Main.mouseLeftRelease;
            MouseRight = Main.mouseRight;
            MouseRightClick = Main.mouseRight && Main.mouseRightRelease;

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
        public static bool IsClick(Keys key) => IsClick(key.ToString());

        /// <summary/>
        public static bool IsDown(string key)
        {
            if (key == null) return false;

            return KeyDown.Contains(key);
        }
        /// <summary/>
        public static bool IsDown(Keys key) => IsDown(key.ToString());

        /// <summary/>
        public static bool IsUp(string key)
        {
            return IsDown(key) != true;
        }
        /// <summary/>
        public static bool IsUp(Keys key) => IsUp(key.ToString());

        /// <summary/>
        public static bool IsDownOld(string key)
        {
            if (key == null) return false;

            return KeyDownOld.Contains(key);
        }
        /// <summary/>
        public static bool IsDownOld(Keys key) => IsDownOld(key.ToString());

        /// <summary/>
        public static bool IsUpOld(string key)
        {
            return IsDownOld(key) != true;
        }
        /// <summary/>
        public static bool IsUpOld(Keys key) => IsUpOld(key.ToString());
    }
}
