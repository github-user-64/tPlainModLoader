using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using tContentPatch.Patch;
using tContentPatch.Utils;
using Terraria.GameContent.UI.Chat;

namespace tContentPatch.ModPatch
{
    internal class Patch_RemadeChatMonitor : ClassPatch<PatchRemadeChatMonitor>
    {
        private static List<PatchRemadeChatMonitor> mod = new List<PatchRemadeChatMonitor>();

        public Patch_RemadeChatMonitor() : base(mod) { }

        public override void Initialize(IAddPatch addPatch)
        {
            Type oType = typeof(RemadeChatMonitor);
            Type tType = typeof(Patch_RemadeChatMonitor);
            Action<string, string, BindingFlags> addPre = (m, m2, bf) =>
            {
                addPatch.AddPrefix(oType.GetMethod(m, bf), tType.GetMethod(m2));
            };
            Action<string, string, BindingFlags> addPo = (m, m2, bf) =>
            {
                addPatch.AddPostfix(oType.GetMethod(m, bf), tType.GetMethod(m2));
            };

            addPo("DrawChat", nameof(DrawChatPostfix), BindingFlags.Public | BindingFlags.Instance);

            addPre("AddNewMessage", nameof(AddNewMessagePrefix), BindingFlags.Public | BindingFlags.Instance);
        }

        public static void DrawChatPostfix(bool drawingPlayerChat)
        {
            try
            {
                foreach (PatchRemadeChatMonitor item in mod) item.DrawChatPostfix(drawingPlayerChat);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        public static void AddNewMessagePrefix(ref string text, Color color, int widthLimitInPixels = -1)
        {
            try
            {
                foreach (PatchRemadeChatMonitor item in mod) item.AddNewMessagePrefix(ref text, color, widthLimitInPixels);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }
    }
}
