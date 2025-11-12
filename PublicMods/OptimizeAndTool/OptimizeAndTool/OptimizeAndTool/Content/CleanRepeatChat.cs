using CommandHelp;
using Microsoft.Xna.Framework;
using OptimizeAndTool.Utils;
using OptimizeAndTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria.UI;
using Terraria.UI.Chat;

namespace OptimizeAndTool.Content
{
    internal class CleanRepeatChat : Mod
    {
        public static GetSetReset<bool> Enable = new GetSetReset<bool>(true, true);
        private static string cleanRepeatText_oldText = null;
        private static int cleanRepeatText_oldTextCount = 0;

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get2("cleanRepeatChat", Enable),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get2(Enable, "重新加载模组后该功能会失控", text: "清除重复聊天"),
            };

            return uis;
        }

        public static bool OnAddNewMessage(string text, Color color, int widthLimitInPixels = -1)
        {
            if (Enable.val == false)
            {
                cleanRepeatText_oldTextCount = 0;
                return true;
            }

            return cleanRepeatText(text, color, widthLimitInPixels);
        }

        private static bool cleanRepeatText(string text, Color color, int widthLimitInPixels = -1)
        {
            List<ChatMessageContainer> chats = Patch.Patch_RemadeChatMonitor._messages;
            if (chats == null) return true;

            if (chats.Count < 1)
            {
                cleanRepeatText_oldText = text;
                cleanRepeatText_oldTextCount = 1;
                return true;
            }

            if (cleanRepeatText_oldTextCount < 1)
            {
                cleanRepeatText_oldText = chats[0].OriginalText;
                cleanRepeatText_oldTextCount = 1;
            }

            if (cleanRepeatText_oldText == text)
            {
                ++cleanRepeatText_oldTextCount;

                chats[0].SetContents($"{text} x{cleanRepeatText_oldTextCount}", color, widthLimitInPixels);

                return false;
            }

            cleanRepeatText_oldText = text;
            cleanRepeatText_oldTextCount = 1;

            return true;
        }
    }
}
