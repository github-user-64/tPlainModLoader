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
        public static readonly Dictionary<string, int> KeyDown = new Dictionary<string, int>();
        /// <summary/>
        public static readonly Dictionary<string, int> KeyDownOld = new Dictionary<string, int>();

        /// <summary>
        /// 光标左键按下<para/>
        /// 和<see cref="IsDown(Keys[])"/>的区别是会收到按键绑定影响
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

        /// <summary>
        /// 是否在输入
        /// </summary>
        public static bool WritingText => PlayerInput.WritingText;

        [HarmonyPatch("UpdateInput")]
        [HarmonyPostfix]
        internal static void UpdateInputPostfix()
        {
            MouseLeft = Main.mouseLeft;
            MouseLeftClick = Main.mouseLeft && Main.mouseLeftRelease;
            MouseRight = Main.mouseRight;
            MouseRightClick = Main.mouseRight && Main.mouseRightRelease;

            KeyDownOld.Clear();
            foreach (KeyValuePair<string, int> i in KeyDown) KeyDownOld.Add(i.Key, i.Value);
            KeyDown.Clear();

            foreach (Keys i in PlayerInput.GetPressedKeys()) AddKey(i.ToString());
            foreach (string i in PlayerInput.MouseKeys) AddKey(i);
        }

        private static void AddKey(string key)
        {
            if (KeyDownOld.TryGetValue(key, out int count) != true) count = 1;
            else if (count < 10000) ++count;

            KeyDown.Add(key, count);
        }

        /// <summary>
        /// 获取<paramref name="key"/>按下了多久<br/>
        /// 第一次为<see langword="1"/><br/>
        /// 没有为<see langword="0"/><br/>
        /// 最高为<see langword="10000"/>
        /// </summary>
        public static int DownCount(string key)
        {
            if (key == null) return 0;

            if (KeyDown.TryGetValue(key, out int count) == true) return count;

            return 0;
        }
        /// <summary>
        /// 详见<see cref="DownCount(string)"/>
        /// </summary>
        public static int DownCount(Keys key) => DownCount(key.ToString());
        /// <summary>
        /// 详见<see cref="DownCount(string)"/>
        /// </summary>
        public static bool DownCount(string key, out int count)
        {
            count = DownCount(key);
            return count > 0;
        }
        /// <summary>
        /// 详见<see cref="DownCount(string)"/>
        /// </summary>
        public static bool DownCount(Keys key, out int count) => DownCount(key.ToString(), out count);

        /// <summary/>
        public static bool IsClick(params string[] keys) => IsDown(keys) && IsUpOld(keys);
        /// <summary/>
        public static bool IsClick(params Keys[] keys) => IsClick(keys.ConvertKey());

        /// <summary/>
        public static bool IsDown(params string[] keys) => KeyDown.KeyContains(keys);
        /// <summary/>
        public static bool IsDown(params Keys[] keys) => IsDown(keys.ConvertKey());

        /// <summary/>
        public static bool IsUp(params string[] keys) => IsDown(keys) != true;
        /// <summary/>
        public static bool IsUp(params Keys[] keys) => IsUp(keys.ConvertKey());

        /// <summary/>
        public static bool IsDownOld(params string[] keys) => KeyDownOld.KeyContains(keys);
        /// <summary/>
        public static bool IsDownOld(params Keys[] keys) => IsDownOld(keys.ConvertKey());

        /// <summary/>
        public static bool IsUpOld(params string[] keys) => IsDownOld(keys) != true;
        /// <summary/>
        public static bool IsUpOld(params Keys[] keys) => IsUpOld(keys.ConvertKey());

        //

        private static bool KeyContains(this Dictionary<string, int> keyState, string[] keys)
        {
            foreach (string key in keys)
            {
                if (key == null) return false;
                if (keyState.ContainsKey(key) != true) return false;
            }

            return true;
        }

        private static string[] ConvertKey(this Keys[] keys)
        {
            string[] ks = new string[keys.Length];
            for (int i = 0; i < ks.Length; ++i) ks[i] = keys[i].ToString();

            return ks;
        }
    }
}
