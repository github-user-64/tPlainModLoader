using HarmonyLib;
using System;
using System.Collections.Generic;
using Terraria;

namespace tContentPatch.ModPatch
{
    [HarmonyPatch(typeof(WorldGen))]
    internal class Patch_WorldGen : ListCopy<PatchWorldGen>
    {
        private static List<PatchWorldGen> mod = new List<PatchWorldGen>();

        public Patch_WorldGen() : base(mod) { }

        [HarmonyPatch("Convert", new Type[] { typeof(int), typeof(int), typeof(int), typeof(bool), typeof(bool) })]
        [HarmonyPrefix]
        public static bool ConvertPrefix(int i2, int j2, int conversionType, bool tiles, bool walls)
        {
            return mod.ForTry(item => item.CanConvert(i2, j2, conversionType, tiles, walls));
        }

        [HarmonyPatch("UpdateWorld")]
        [HarmonyPrefix]
        public static void UpdateWorldPrefix()
        {
            mod.ForTry(item => item.UpdateWorldPrefix());
        }

        [HarmonyPatch("UpdateWorld")]
        [HarmonyPostfix]
        public static void UpdateWorldPostfix()
        {
            mod.ForTry(item => item.UpdateWorldPostfix());
        }

        [HarmonyPatch("SaveAndQuit")]
        [HarmonyPrefix]
        public static void SaveAndQuitPrefix(Action callback = null)
        {
            mod.ForTry(item => item.SaveAndQuitPrefix(callback));
        }

        [HarmonyPatch("SaveAndQuit")]
        [HarmonyPostfix]
        public static void SaveAndQuitPostfix(Action callback = null)
        {
            mod.ForTry(item => item.SaveAndQuitPostfix(callback));
        }
    }
}
