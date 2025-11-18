using CommandHelp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OptimizeAndTool.Utils;
using OptimizeAndTool.Utils.quickBuild;
using ReLogic.OS;
using System;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.Audio;
using Terraria.UI;
using Terraria.UI.Chat;

namespace OptimizeAndTool.Content
{
    internal class CopyChat : PatchRemadeChatMonitor
    {
        public static GetSetReset<bool> Enable = new GetSetReset<bool>(true, true);

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get2("copyChat", Enable),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get2(Enable, text: "复制聊天文本"),
            };

            return uis;
        }

        public override void DrawChatPostfix(bool drawingPlayerChat)
        {
            if (Enable.val == false) return;

            int showCount = Patch.Patch_RemadeChatMonitor._showCount;
            int startChatLine = Patch.Patch_RemadeChatMonitor._startChatLine;
            List<ChatMessageContainer> messages = Patch.Patch_RemadeChatMonitor._messages;

            int num = startChatLine;
            int i2 = 0;
            int num3 = 0;
            while (num > 0 && i2 < messages.Count)
            {
                int num4 = Math.Min(num, messages[i2].LineCount);
                num -= num4;
                num3 += num4;
                if (num3 == messages[i2].LineCount)
                {
                    num3 = 0;
                    i2++;
                }
            }

            int i = 0;
            while (i < showCount && i2 < messages?.Count)
            {
                ChatMessageContainer chatMessageContainer = messages[i2];

                if (!chatMessageContainer.Prepared || !(drawingPlayerChat | chatMessageContainer.CanBeShownWhenChatIsClosed))
                {
                    break;
                }

                int size = 21;
                Vector2 pos = new Vector2(88f, (float)(Main.screenHeight - 30 - 28 - i * size));
                ++i;

                Texture2D texture = Main.Assets.Request<Texture2D>("Images/UI/Cursor_7").Value;
                Rectangle rect = new Rectangle((int)pos.X, (int)pos.Y, texture.Width, texture.Height);
                rect.Width = size - 4;
                rect.Height = size - 4;
                rect.X -= rect.Width + 4;

                bool isMouse = false;

                if (Main.mouseX >= rect.X && Main.mouseX <= rect.X + rect.Width)
                {
                    if (Main.mouseY >= rect.Y && Main.mouseY <= rect.Y + rect.Height)
                    {
                        isMouse = true;
                        string text = chatMessageContainer.OriginalText ?? string.Empty;

                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            SoundEngine.PlaySound(12);
                            Platform.Get<IClipboard>().Value = text;
                        }
                        else
                        {
                            tContentPatch.Content.DrawTip.Draw(Main.spriteBatch, new string[] { $"复制[{text}]" });
                        }
                    }
                }

                Main.spriteBatch.Draw(texture, rect, isMouse ? Color.White : Color.White * 0.5f);

                num3++;
                if (num3 >= chatMessageContainer.LineCount)
                {
                    num3 = 0;
                    i2++;
                }
            }
        }
    }
}
