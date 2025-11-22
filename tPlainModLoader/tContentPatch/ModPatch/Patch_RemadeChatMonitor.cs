using HarmonyLib;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using tContentPatch.Utils;
using Terraria.GameContent.UI.Chat;

namespace tContentPatch.ModPatch
{
    [HarmonyPatch(typeof(RemadeChatMonitor))]
    internal class Patch_RemadeChatMonitor : ListCopy<PatchRemadeChatMonitor>
    {
        private static List<PatchRemadeChatMonitor> mod = new List<PatchRemadeChatMonitor>();

        public Patch_RemadeChatMonitor() : base(mod) { }

        [HarmonyPatch("DrawChat")]
        [HarmonyPostfix]
        public static void DrawChatPostfix(bool drawingPlayerChat)
        {
            try
            {
                mod.For(item => item.DrawChatPostfix(drawingPlayerChat));
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        [HarmonyPatch("AddNewMessage")]
        [HarmonyPrefix]
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
