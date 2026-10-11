using ChatAi.Content.UI;
using ChatAi.Utils;
using ChatAi.Utils.quickBuild;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using tContentPatch;
using Terraria;
using Terraria.GameContent.NetModules;
using Terraria.Net;
using Terraria.UI;
using Terraria.UI.Chat;

namespace ChatAi.Content
{
    internal class GameChatAi : PatchRemadeChatMonitor
    {
        public static GetSetReset<bool> Enable = new GetSetReset<bool>();
        public static GetSetReset<bool> DisplayText = new GetSetReset<bool>();
        public static GetSetReset<string> StringKey = new GetSetReset<string>("ss", "ss", v => v ?? string.Empty);//文本触发关键字
        public static GetSetReset<string> ChatHead = new GetSetReset<string>("imai", "imai");
        public static GetSetReset<int> AiType = new GetSetReset<int>(0, 0, GetSetReset.GetIntFunc(0, 1));
        private static int requestCount = 0;
        private static string print_oldText = null;

        public static List<UIElement> GetUI() => new List<UIElement>()
        {
            UIBuild.get2(Enable, text: "聊天Ai"),
            UIBuild.get2(DisplayText, text: "输出请求文本"),
            new UIItemTextBoxBind<string>(StringKey, v => v, text:"关键字"){ MouseText = "<string>" },
            new UIItemTextBoxBind<string>(ChatHead, v => v, text:"头文本"){ MouseText = "<string>" },
            new UIItemValueSliderBind<int>(AiType, v => v, v => (int)v, 0, 1, text: "类型")
            {
                FloatToString = v =>
                {
                    if (v == 0) return "普通模型";
                    if (v == 1) return "连续对话";
                    return "-";
                }
            }
        };

        public override void Initialize()
        {
            Enable.OnValUpdate += v =>
            {
                if (v) StatusIsEnable();
            };
            StringKey.OnValUpdate += v =>
            {
                printToGame($"[c/ffff00:关键字改为\"{StringKey.val}\"]");
            };

            ChatAI.CanRequest += () => requestCount < 1;
            ChatAI.RequestStart += () => ++requestCount;
            ChatAI.RequestEnd += () =>
            {
                _ = Task.Run(async () =>
                {
                    await Task.Delay(3000);
                    if (requestCount > 0) --requestCount;
                });
            };
            ChatAI.ChatResponse += (v) =>
            {
                outputText(v);
            };
            ChatAI.RequestTimeout += () =>
            {
                printToGame("[c/ffff00:请求超时(估计人家不在]");
            };
        }

        public override void AddNewMessagePrefix(ref string text, Color color, int widthLimitInPixels = -1)
        {
            if (Enable.val) inputText(text);
        }

        public static void inputText(string text)
        {
            if (print_oldText == text) return;
            print_oldText = null;

            text = getRequestText(text);

            if (text == null) return;

            if (DisplayText.val) ContentPatch.PrintTry($"请求文本: [{text}]");

            request(text);
        }

        private static string getRequestText(string text)
        {
            if (text == null) return null;

            string head = StringKey.val;

            if (text.Length < head.Length + 1) return null;

            //

            int index = text.IndexOf(head);
            if (index == -1) return null;

            index += head.Length;
            if (index >= text.Length) return null;

            //

            int index2 = text.IndexOf(head, index);
            if (index2 == -1) return null;

            //

            text = text.Substring(index, index2 - index);

            return text;
        }

        private static void request(string text)
        {
            if (text == null) return;

            if (text.Length > 50)
            {
                printToGame("[c/ffff00:文本过长(太长了啦, 笨蛋]");
                return;
            }

            text = text.Trim();
            if (text.Length < 1) return;

            //"\\s"匹配任何空白字符，包括空格、制表符、换页符等
            if (Regex.Replace(text, "\\s", "").Length < 1) return;//去除空字符后如果啥都没有就退出

            _ = Task.Run(() =>
            {
                ChatAI.InputAsync(text, AiType.val);
            });
        }

        private static void outputText(string text)
        {
            if (text == null) return;
            if (text.Length < 1) return;

            int len = 50;
            for (int i = len; i < text.Length; i += len)
            {
                string insertT = "\n";

                text = text.Insert(i, insertT);

                i += insertT.Length;
            }

            string[] texts = text.Split('\n');
            text = string.Empty;
            for (int i = 0; i < texts.Length; ++i)
            {
                if (texts[i] == null || texts[i].Length < 1) continue;

                if (text == string.Empty && ChatHead.val != null && ChatHead.val.Length > 0)
                {
                    text = $"\n<{ChatHead.val}>: [c/ffff00:{texts[i]}]";
                }
                else
                {
                    text += $"\n[c/ffff00:{texts[i]}]";
                }
            }

            printToGame(text);
        }

        private static void printToGame(string text)
        {
            if (text == null) return;

            print_oldText = text;

            if (Main.netMode == 0)
            {
                Main.NewText(text);
            }
            else
            {
                NetPacket packet = NetTextModule.SerializeClientMessage(ChatManager.Commands.CreateOutgoingMessage(text));
                NetManager.Instance.SendToServer(packet);
            }
        }

        //

        private static int notEnable_time = 0;

        private class notEnableCount : PatchMain
        {
            public override void UpdatePostfix(GameTime gameTime)
            {
                if (notEnable_time < 60) ++notEnable_time;
            }
        }

        public static void StatusIsEnable()
        {
            if (notEnable_time >= 60)
            {
                printToGame($"[c/ffff00:聊天功能启用啦, 在聊天文本前后加上\"{StringKey.val}\"即可聊天, 间隔3秒才能继续发哦]");
            }
            notEnable_time = 0;
        }
    }
}
