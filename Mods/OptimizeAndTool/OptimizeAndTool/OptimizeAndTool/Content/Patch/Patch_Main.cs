using HarmonyLib;
using System.Collections.Generic;
using Terraria;
using Terraria.UI.Gamepad;

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

        /// <summary>
        /// 重置ui导航, 取消按移动键鼠标会移到不同ui上的功能
        /// </summary>
        private static void ResetUILink()
        {
            int start = 3000;//未知位置, 连个id字段都找不到

            UILinkPointNavigator.ChangePoint(start);//重置到第一个

            UILinkPoint link = UILinkPointNavigator.Points[start];
            link.Position = default;//空位置, 会导致光标移到屏幕左上角
            link.Unlink();//禁用其它方向
        }
    }
}
