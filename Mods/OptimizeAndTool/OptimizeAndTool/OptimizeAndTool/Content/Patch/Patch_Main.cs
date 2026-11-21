using HarmonyLib;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace OptimizeAndTool.Content.Patch
{
    [HarmonyPatch(typeof(Main))]
    internal static class Patch_Main
    {
        [HarmonyPatch("DoDraw")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> TranspilerDoDraw(IEnumerable<CodeInstruction> instructions)
        {
            return PatchGameViewMatrixZoomLimit.TranspilerDoDraw(instructions);
        }

        [HarmonyPatch(nameof(Main.UpdateTimeRate))]
        [HarmonyPostfix]
        public static void UpdateTimeRatePostfix()
        {
            TimeSet.UpdateTimeRate();
        }

        [HarmonyPatch(nameof(Main.SelectPlayer))]
        [HarmonyPostfix]
        public static void SelectPlayerPostfix()
        {
            if (Main.menuMode != MenuID.ServerIP) return;

            if (ServerList.ServerList.Enable.val == true) ServerList.ServerList.OpenMenu();
        }
    }
}
