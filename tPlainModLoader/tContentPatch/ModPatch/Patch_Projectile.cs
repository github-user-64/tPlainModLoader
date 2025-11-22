using HarmonyLib;
using System;
using System.Collections.Generic;
using tContentPatch.Utils;
using Terraria;

namespace tContentPatch.ModPatch
{
    [HarmonyPatch(typeof(Projectile))]
    internal class Patch_Projectile : ListCopy<PatchProjectile>
    {
        private static List<PatchProjectile> mod = new List<PatchProjectile>();

        public Patch_Projectile() : base(mod) { }

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        public static void UpdatePrefix(Projectile __instance, int i)
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
        public static void UpdatePostfix(Projectile __instance, int i)
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

        [HarmonyPatch("Kill")]
        [HarmonyPrefix]
        public static void KillPrefix(Projectile __instance)
        {
            try
            {
                mod.For(item => item.KillPrefix(__instance));
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }
    }
}
