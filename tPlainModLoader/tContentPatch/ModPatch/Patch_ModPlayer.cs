using System;
using System.Collections.Generic;
using System.Reflection;
using tContentPatch.Patch;
using tContentPatch.Utils;
using Terraria;

namespace tContentPatch.ModPatch
{
    internal class Patch_ModPlayer
    {
        internal static List<ModPlayer> mod = new List<ModPlayer>();

        internal static void Initialize(IAddPatch addPatch)
        {
            Type oType = typeof(Player);
            Type tType = typeof(Patch_ModPlayer);
            Action<string, string, BindingFlags> addPre = (m, m2, bf) =>
            {
                addPatch.AddPrefix(oType.GetMethod(m, bf), tType.GetMethod(m2));
            };
            Action<string, string, BindingFlags> addPo = (m, m2, bf) =>
            {
                addPatch.AddPostfix(oType.GetMethod(m, bf), tType.GetMethod(m2));
            };

            addPre("Update", nameof(UpdatePrefix), BindingFlags.Public | BindingFlags.Instance);
            addPo("Update", nameof(UpdatePostfix), BindingFlags.Public | BindingFlags.Instance);

            addPo("UpdateArmorSets", nameof(UpdateArmorSetsPostfix), BindingFlags.Public | BindingFlags.Instance);
        }

        public static void UpdatePrefix(Player __instance, int i)
        {
            try
            {
                foreach (ModPlayer item in mod) item.UpdatePrefix(__instance, i);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        public static void UpdatePostfix(Player __instance, int i)
        {
            try
            {
                foreach (ModPlayer item in mod) item.UpdatePostfix(__instance, i);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        public static void UpdateArmorSetsPostfix(Player __instance, int i)
        {
            try
            {
                foreach (ModPlayer item in mod) item.UpdateArmorSetsPostfix(__instance, i);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }
    }
}
