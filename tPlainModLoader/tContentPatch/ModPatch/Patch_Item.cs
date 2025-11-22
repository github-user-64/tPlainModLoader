using HarmonyLib;
using System;
using System.Collections.Generic;
using tContentPatch.Utils;
using Terraria;

namespace tContentPatch.ModPatch
{
    [HarmonyPatch(typeof(Item))]
    internal class Patch_Item : ListCopy<PatchItem>
    {
        private static List<PatchItem> mod = new List<PatchItem>();

        public Patch_Item() : base(mod) { }

        [HarmonyPatch("UpdateItem")]
        [HarmonyPrefix]
        public static void UpdateItemPrefix(Item __instance, int i)
        {
            try
            {
                mod.For(item => item.UpdateItemPrefix(__instance, i));
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        [HarmonyPatch("UpdateItem")]
        [HarmonyPostfix]
        public static void UpdateItemPostfix(Item __instance, int i)
        {
            try
            {
                mod.For(item => item.UpdateItemPostfix(__instance, i));
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }
    }
}
