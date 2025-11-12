using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.UI.Chat;

namespace tContentPatch.Content
{
    /// <summary>
    /// 绘制提示, 一个框内有文本
    /// </summary>
    public static class DrawTip
    {
        [HarmonyPatch(typeof(Main), "DoDraw")]
        private class PatchDoDraw
        {
            internal static void Postfix(GameTime gameTime)
            {
                string[] ss = DrawTip.ss;
                DrawTip.ss = null;
                if (ss == null) return;

                PlayerInput.SetZoom_UI();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
                Draw(Main.spriteBatch, ss, color, textColor);
                Main.spriteBatch.End();
            }
        }

        private static Color color = new Color(23, 25, 81, 255) * 0.925f;
        private static Color textColor = Color.White;
        private static string[] ss = null;

        public static void SetDraw(Color color, Color textColor, params string[] ss)
        {
            DrawTip.color = color;
            DrawTip.textColor = textColor;
            DrawTip.ss = ss;
        }

        public static void SetDraw(params string[] ss)
        {
            SetDraw(new Color(23, 25, 81, 255) * 0.925f, Color.White, ss);
        }

        public static void Draw(SpriteBatch spriteBatch, string[] ss)
        {
            Draw(spriteBatch, ss, new Color(23, 25, 81, 255) * 0.925f, Color.White);
        }

        public static void Draw(SpriteBatch spriteBatch, string[] ss, Color color, Color textColor)
        {
            int toolTipDistance = 20;
            Vector2 pos = new Vector2(Main.mouseX + toolTipDistance, Main.mouseY + toolTipDistance);

            Vector2 size = Vector2.Zero;
            foreach (string s in ss)
            {
                Vector2 stringSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, s, Vector2.One, -1f);
                if (stringSize.X > size.X)
                {
                    size.X = stringSize.X;
                }
                size.Y += stringSize.Y;
            }

            int paddinWidth = 14;
            int paddingHeight = 9;

            Terraria.Utils.DrawInvBG(spriteBatch, pos.X, pos.Y,
                size.X + paddinWidth * 2, size.Y + paddingHeight,
                color);

            pos.X += paddinWidth;
            pos.Y += paddingHeight;

            int y = 0;
            foreach (string s in ss)
            {
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, s,
                    pos + new Vector2(0, y), textColor, 0f, Vector2.Zero, Vector2.One);

                y += (int)ChatManager.GetStringSize(FontAssets.MouseText.Value, s, Vector2.One, -1f).Y;
            }
        }
    }
}
