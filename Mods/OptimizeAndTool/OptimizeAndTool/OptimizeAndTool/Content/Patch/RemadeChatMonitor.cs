using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using tContentPatch;
using tContentPatch.Patch;
using Terraria.UI.Chat;
using Terraria.GameContent.UI.Chat;

namespace OptimizeAndTool.Content.Patch
{
    internal class Patch_RemadeChatMonitor : Mod
    {
        private static List<ChatMessageContainer> __messages = null;
        public static List<ChatMessageContainer> _messages
        {
            get
            {
                if (__messages != null) return __messages;

                __messages = ReflectionHelp(nameof(_messages)) as List<ChatMessageContainer>;

                return __messages;
            }
        }
        private static int __showCount = 0;
        public static int _showCount
        { 
            get
            {
                int? v = ReflectionHelp(nameof(_showCount)) as int?;
                if (v == null) return __showCount;
                __showCount = v.Value;

                return __showCount;
            }
        }
        private static int __startChatLine = 0;
        public static int _startChatLine
        {
            get
            {
                int? v = ReflectionHelp(nameof(_startChatLine)) as int?;
                if (v == null) return __startChatLine;
                __startChatLine = v.Value;

                return __startChatLine;
            }
        }

        private static object ReflectionHelp(string name)
        {
            Type type = typeof(RemadeChatMonitor);
            if (type == null) return null;

            System.Reflection.FieldInfo fieldInfo = type.GetField(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (fieldInfo == null) return null;

            return fieldInfo.GetValue(Terraria.Main.chatMonitor);
        }

        public override void AddPatch(IAddPatch addPatch)
        {
            addPatch.AddPrefix(typeof(RemadeChatMonitor).GetMethod(nameof(RemadeChatMonitor.AddNewMessage)),
                typeof(Patch_RemadeChatMonitor).GetMethod(nameof(AddNewMessageCan)));
        }

        public static bool AddNewMessageCan(string text, Color color, int widthLimitInPixels = -1)
        {
            try
            {
                bool returnV = true;

                returnV &= CleanRepeatChat.OnAddNewMessage(text, color, widthLimitInPixels);

                return returnV;
            }
            catch (Exception ex)
            {
                Terraria.Main.NewText($"{nameof(Patch_RemadeChatMonitor)}.{nameof(AddNewMessageCan)}:{ex.Message}");

                return true;
            }
        }
    }
}
