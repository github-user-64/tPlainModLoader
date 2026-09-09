using HarmonyLib;
using System;
using System.Collections.Generic;
using tContentPatch.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.Localization;

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
            mod.ForTry(item => item.UpdatePrefix(__instance, i));
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void UpdatePostfix(Player __instance, int i)
        {
            mod.ForTry(item => item.UpdatePostfix(__instance, i));
        }

        [HarmonyPatch("UpdateEquips")]
        [HarmonyPrefix]
        public static void UpdateEquipsPrefix(Player __instance, int i)
        {
            mod.ForTry(item => item.UpdateEquipsPrefix(__instance, i));
        }

        [HarmonyPatch("UpdateEquips")]
        [HarmonyPostfix]
        public static void UpdateEquipsPostfix(Player __instance, int i)
        {
            mod.ForTry(item => item.UpdateEquipsPostfix(__instance, i));
        }

        [HarmonyPatch("UpdateArmorSets")]
        [HarmonyPostfix]
        public static void UpdateArmorSetsPostfix(Player __instance, int i)
        {
            mod.ForTry(item => item.UpdateArmorSetsPostfix(__instance, i));
        }

        [HarmonyPatch("SavePlayer")]
        [HarmonyPrefix]
        public static void SavePlayerPrefix(PlayerFileData playerFile, bool skipMapSave)
        {
            if (Main.netMode != 0 && Main.netMode != 1) return;

            mod.ForTry(item => item.SavePlayerPrefix(playerFile, skipMapSave));
        }

        [HarmonyPatch("SavePlayer")]
        [HarmonyPostfix]
        public static void SavePlayerPostfix(PlayerFileData playerFile, bool skipMapSave)
        {
            if (Main.netMode != 0 && Main.netMode != 1) return;

            mod.ForTry(item => item.SavePlayerPostfix(playerFile, skipMapSave));
        }

        [HarmonyPatch("DropTombstone")]
        [HarmonyPrefix]
        internal static bool CanDropTombstone(Player __instance, long coinsOwned, NetworkText deathText, int hitDirection)
        {
            return mod.ForTry(item => item.CanDropTombstone(__instance, coinsOwned, deathText, hitDirection));
        }

        [HarmonyPatch("Hurt")]
        [HarmonyPrefix]
        internal static void HurtPrefix(Player __instance, PlayerDeathReason damageSource,
            ref int Damage, ref int hitDirection, ref bool pvp, ref bool quiet, ref bool Crit, ref int cooldownCounter, ref bool dodgeable)
        {
            foreach (PatchPlayer item in mod)
            {
                try
                {
                    item.HurtPrefix(__instance, damageSource,
                       ref Damage, ref hitDirection, ref pvp, ref quiet, ref Crit, ref cooldownCounter, ref dodgeable);
                }
                catch (Exception ex)
                {
                    OutputDebug.OutputException(ex);
                }
            }
        }

        [HarmonyPatch("Hurt")]
        [HarmonyPostfix]
        internal static void HurtPostfix(Player __instance, ref double __result, PlayerDeathReason damageSource,
            int Damage, int hitDirection, bool pvp, bool quiet, bool Crit, int cooldownCounter, bool dodgeable)
        {
            double result = __result;

            mod.ForTry(item => item.HurtPostfix(__instance, ref result, damageSource,
                Damage, hitDirection, pvp, quiet, Crit, cooldownCounter, dodgeable));

            __result = result;
        }

        [HarmonyPatch("KillMe")]
        [HarmonyPrefix]
        public static bool KillMePrefix(Player __instance, PlayerDeathReason damageSource, double dmg, int hitDirection, bool pvp)
        {
            bool ok = true;
            mod.ForTry(item =>
            {
                ok &= item.KillMePrefix(__instance, damageSource, dmg, hitDirection, pvp);
            });

            return ok;
        }

        [HarmonyPatch("KillMe")]
        [HarmonyPostfix]
        public static void KillMePostfix(Player __instance, PlayerDeathReason damageSource, double dmg, int hitDirection, bool pvp)
        {
            mod.ForTry(item => item.KillMePostfix(__instance, damageSource, dmg, hitDirection, pvp));
        }
    }
}
