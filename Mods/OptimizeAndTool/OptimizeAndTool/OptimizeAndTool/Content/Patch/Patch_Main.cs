using HarmonyLib;
using System.Collections.Generic;
using Terraria;

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

        [HarmonyPatch("UpdateTimeRate")]
        [HarmonyPostfix]
        public static void UpdateTimeRatePostfix()
        {
            TimeSet.UpdateTimeRate();
        }
    }
}
