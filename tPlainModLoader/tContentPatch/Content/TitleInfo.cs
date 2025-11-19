using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.OS;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.UI.Chat;

namespace tContentPatch.Content
{
    internal class TitleInfo
    {
        [HarmonyPatch(typeof(Main), "DrawSocialMediaButtons")]
        public class MainPatch_DrawSocialMediaButtons
        {
            private static TitleLinkButton[] buttons = null;

            internal static void Postfix(Color menuColor, float upBump)
            {
                if (buttons == null)
                {
                    buttons = new TitleLinkButton[1];
                    buttons[0] = new TitleLinkButton("https://github.com/github-user-64/tPlainModLoader", "GitHub", 0);
                }

                float off = 6f;
                if (!WorldGen.drunkWorldGen)
                {
                    off += 20;
                }
                if (!WorldGen.drunkWorldGen && Main.menuMode == 0)
                {
                    off += 32;
                    off += 32;

                    Vector2 pos = new Vector2(3, Main.screenHeight - off);
                    foreach (TitleLinkButton button in buttons) button.Draw(Main.spriteBatch, pos);
                }
            }
        }

        [HarmonyPatch(typeof(Main), "DrawVersionNumber")]
        public class MainPatch_DrawVersionNumber
        {
            internal static void Postfix(Color menuColor, float upBump)
            {
                float off = 6f;
                if (!WorldGen.drunkWorldGen && Main.menuMode == 0)
                {
                    off += 32;
                    off += 32;
                }
                if (!WorldGen.drunkWorldGen)
                {
                    off += 20;
                    off += 20;
                    DrawVersionNumber(menuColor, Main.screenHeight - off);
                }
            }
        }

        private static void DrawVersionNumber(Color menuColor, float upBump)
        {
            string text = $"tPlainModLoader v{ContentPatch.VersionTPlainModLoader}";

            Vector2 pos = new Vector2(10, upBump);

            Color colorShadow = Color.Black;
            colorShadow.A = (byte)(colorShadow.A * 0.3f);

            Color color = menuColor;
            color.R = (byte)((byte.MaxValue + color.R) / 2);
            color.G = (byte)((byte.MaxValue + color.R) / 2);
            color.B = color.G;
            color.A = (byte)(color.A * 0.3f);

            TextSnippet[] snippets = ChatManager.ParseMessage(text, color).ToArray();
            ChatManager.ConvertNormalSnippets(snippets);

            ChatManager.DrawColorCodedStringShadow(Main.spriteBatch, FontAssets.MouseText.Value, snippets,
                pos, colorShadow, 0f, Vector2.Zero, Vector2.One);

            ChatManager.DrawColorCodedString(Main.spriteBatch, FontAssets.MouseText.Value, text,
                pos, color, 0f, Vector2.Zero, Vector2.One);
        }

        private class TitleLinkButton
        {
            private static Texture2D texture = null;
            private static int width = -1;
            private static int height = -1;
            static TitleLinkButton()
            {
                texture = Utils.Resource.GetTexture2D($"{nameof(tContentPatch)}.Resources.TitleLinkButtons.png");
                width = texture.Width / 1;
                height = texture.Height / 2;
            }

            public string URL = null;
            public string Tip = null;
            private int index = -1;

            public TitleLinkButton(string url, string tip, int index)
            {
                URL = url;
                Tip = tip;
                this.index = index;
            }

            public void Draw(SpriteBatch spriteBatch, Vector2 pos)
            {
                bool isMouse = false;

                pos.X += width * index;

                if (Main.MouseScreen.Between(pos, pos + new Vector2(width, height)))
                {
                    Main.LocalPlayer.mouseInterface = true;
                    isMouse = true;
                    DrawTip.SetDraw(new Color(23, 25, 81, 255) * 0.925f, Color.Yellow, Tip);
                    TryClicking();
                }

                spriteBatch.Draw(texture,
                    new Rectangle((int)pos.X, (int)pos.Y, width, height),
                    new Rectangle(width * index, isMouse ? height : 0, width, height),
                    Color.White);
            }

            private void TryClicking()
            {
                if (PlayerInput.IgnoreMouseInterface) return;
                if (!Main.mouseLeft || !Main.mouseLeftRelease) return;
                SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
                Main.mouseLeftRelease = false;
                try
                {
                    Platform.Get<IPathService>().OpenURL(URL);
                }
                catch
                {
                    Console.WriteLine("Failed to open link");
                }
            }
        }
    }
}
