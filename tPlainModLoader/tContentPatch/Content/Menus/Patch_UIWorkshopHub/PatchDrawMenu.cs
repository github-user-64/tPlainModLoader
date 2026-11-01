using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using tContentPatch.Utils;
using Terraria;
using Terraria.Social;

namespace tContentPatch.Content.Menus.Patch_UIWorkshopHub
{
    [HarmonyPatch(typeof(Main), "DrawMenu")]
    internal static class PatchDrawMenu
    {
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpiler_DrawMenu(IEnumerable<CodeInstruction> instructions)
        {
            CodeMatcher codeMatcher = new CodeMatcher(instructions);

            CodeMatch[] cs = new CodeMatch[]
            {
                new CodeMatch(OpCodes.Ldsfld, typeof(SocialAPI).GetField(nameof(SocialAPI.Workshop))),
                new CodeMatch(OpCodes.Brfalse_S),
                new CodeMatch(OpCodes.Ldloc_S),
                new CodeMatch(OpCodes.Ldloc_S),
                new CodeMatch(OpCodes.Ldstr, "UI.Workshop"),
            };

            codeMatcher.MatchStartForward(cs);
            if (codeMatcher.IsInvalid)
            {
                string s = $"无法修改主菜单创意工坊选项:找不到IL位置";
                Log.Add(s);
                ContentPatch.PrintTry(s);
                return instructions;
            }

            codeMatcher.RemoveInstructions(2);

            return codeMatcher.Instructions();
        }
    }
}
