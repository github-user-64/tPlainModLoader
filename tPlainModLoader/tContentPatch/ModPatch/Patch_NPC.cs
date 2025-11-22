using HarmonyLib;
using System;
using System.Collections.Generic;
using tContentPatch.Utils;
using Terraria;

namespace tContentPatch.ModPatch
{
    [HarmonyPatch(typeof(NPC))]
    internal class Patch_NPC : ListCopy<PatchNPC>
    {
        private static List<PatchNPC> mod = new List<PatchNPC>();

        public Patch_NPC() : base(mod) { }

        [HarmonyPatch("UpdateNPC")]
        [HarmonyPrefix]
        public static void UpdateNPCPrefix(NPC __instance, int i)
        {
            try
            {
                mod.For(item => item.UpdateNPCPrefix(__instance, i));
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        [HarmonyPatch("UpdateNPC")]
        [HarmonyPostfix]
        public static void UpdateNPCPostfix(NPC __instance, int i)
        {
            try
            {
                mod.For(item => item.UpdateNPCPostfix(__instance, i));
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }
    }
}
