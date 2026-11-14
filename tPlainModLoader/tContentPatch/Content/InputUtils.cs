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
        public static readonly HashSet<string> KeyDown = new HashSet<string>();
        /// <summary/>
        public static readonly HashSet<string> KeyDownOld = new HashSet<string>();

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
            foreach (string i in KeyDown) KeyDownOld.Add(i);
            KeyDown.Clear();

            foreach (Keys i in PlayerInput.GetPressedKeys()) KeyDown.Add(i.ToString());
            foreach (string i in PlayerInput.MouseKeys) KeyDown.Add(i);
        }

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

        private static bool KeyContains(this HashSet<string> keyState, string[] keys)
        {
            foreach (string key in keys)
            {
                if (key == null) return false;
                if (keyState.Contains(key) != true) return false;
            }

            return true;
        }

        private static string[] ConvertKey(this Keys[] keys)
        {
            string[] ks = new string[keys.Length];
            for (int i = 0; i < ks.Length; ++i) ks[i] = keys[i].ToString();

            return ks;
        }

        /// <summary>
        /// 第一次按下<see cref="Key">Key</see>时触发, 经过<see cref="CD">CD</see>后一直触发
        /// </summary>
        public class KeyTrigger
        {
            /// <summary/>
            public bool Can = false;
            /// <summary/>
            public string Key = null;
            /// <summary/>
            public uint CD;
            /// <summary/>
            protected int DownTime = 0;

            /// <summary/>
            public KeyTrigger(string key, uint cd = 30)
            {
                Key = key;
                CD = cd;
            }
            /// <summary/>
            public KeyTrigger(Keys key, uint cd = 30) : this(key.ToString(), cd) { }

            /// <summary/>
            public virtual void Update()
            {
                Can = false;

                if (IsDown(Key) != true)
                {
                    DownTime = 0;
                    return;
                }

                if (DownTime == 0 || DownTime > CD) Can = true;

                if (DownTime < CD) ++DownTime;
            }
        }
    }
}
