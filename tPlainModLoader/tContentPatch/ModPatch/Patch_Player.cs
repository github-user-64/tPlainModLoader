using HarmonyLib;
using System;
using System.Collections.Generic;
using tContentPatch.Utils;
using Terraria;

namespace tContentPatch.ModPatch
{
    [HarmonyPatch(typeof(Player))]
    internal class Patch_Player : ListCopy<PatchPlayer>
    {
        private static List<PatchPlayer> mod = new List<PatchPlayer>();

        public Patch_Player() : base(mod) { }

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        public static void UpdatePrefix(Player __instance, int i)
        {
            try
            {
                mod.For(item => item.UpdatePrefix(__instance, i));
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void UpdatePostfix(Player __instance, int i)
        {
            try
            {
                mod.For(item => item.UpdatePostfix(__instance, i));
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        [HarmonyPatch("UpdateArmorSets")]
        [HarmonyPostfix]
        public static void UpdateArmorSetsPostfix(Player __instance, int i)
        {
            try
            {
                mod.For(item => item.UpdateArmorSetsPostfix(__instance, i));
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }
    }
}
