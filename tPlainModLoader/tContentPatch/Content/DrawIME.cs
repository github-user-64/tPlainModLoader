using HarmonyLib;
using Microsoft.Xna.Framework;
using Terraria;

namespace tContentPatch.Content
{
    /// <summary>
    /// 绘制输入法
    /// </summary>
    [HarmonyPatch(typeof(Main), "DoDraw")]
    public static class DrawIME
    {
        /// <summary>
        /// 需要绘制输入法
        /// </summary>
        public static bool NeedIME = false;
        /// <summary>
        /// 输入法位置
        /// </summary>
        public static Vector2 IME_P = Vector2.Zero;

        internal static void Postfix(GameTime gameTime)
        {
            if (NeedIME == false) return;

            NeedIME = false;

            Main.instance.SetIMEPanelAnchor(IME_P, 0f);
            Main.instance.DrawIMEPanel();//输入法
        }
    }
}
