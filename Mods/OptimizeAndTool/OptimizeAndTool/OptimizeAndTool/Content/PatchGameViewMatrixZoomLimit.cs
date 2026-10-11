using CommandHelp;
using HarmonyLib;
using Microsoft.Xna.Framework;
using OptimizeAndTool.Utils;
using OptimizeAndTool.Utils.quickBuild;
using System.Collections.Generic;
using System.Reflection.Emit;
using tContentPatch;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;

namespace OptimizeAndTool.Content
{
    internal static class PatchGameViewMatrixZoomLimit
    {
        public static GetSetReset<bool> Enable = new GetSetReset<bool>(false, false);
        public static GetSetReset<float> Scale = new GetSetReset<float>(1f, 1f, ScaleClamp);
        public static GetSetReset<bool> Light = new GetSetReset<bool>(false, false);

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get3("zoom", Enable,
                new CommandHRA<float>("scale", Scale, new CommandFloat()),
                CommandBuild.get3("light", Light)),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get1(Enable, Scale, float.Parse, mouseText: "缩放值<float>", text: "取消限制"),
                UIBuild.get2(Light, mouseText: "照明设置为复古或迷幻可看到范围外图格", text: "透视照明"),
            };

            return uis;
        }

        private class PMain : PatchMain
        {
            public override void DoUpdateInWorldPrefix()
            {
                if (Enable.val == false) return;

                if (PlayerInput.Triggers.Current.ViewZoomIn)
                {
                    Scale.val += Scale.val * 0.01f;
                }
                else if (PlayerInput.Triggers.Current.ViewZoomOut)
                {
                    Scale.val -= Scale.val * 0.01f;
                }

                //Lighting.Mode = LightMode.Retro;//照明:复古, 迷幻
                Main.GameZoomTarget = Scale.val;
            }
        }

        public static IEnumerable<CodeInstruction> TranspilerDoDraw(IEnumerable<CodeInstruction> instructions)
        {
            CodeMatcher codeMatcher = new CodeMatcher(instructions);

            codeMatcher.MatchStartForward(
               new CodeMatch(OpCodes.Ldsfld, typeof(Main).GetField(nameof(Main.ForcedMinimumZoom))),
               new CodeMatch(OpCodes.Ldsfld, typeof(Main).GetField(nameof(Main.GameZoomTarget))),
               new CodeMatch(OpCodes.Ldc_R4, 1f),
               new CodeMatch(OpCodes.Ldc_R4, 2f),
               new CodeMatch(OpCodes.Call, typeof(MathHelper).GetMethod(nameof(MathHelper.Clamp))),
               new CodeMatch(OpCodes.Mul)
               )
               .ThrowIfInvalid("找不到IL位置")
               .Advance(4)
               .RemoveInstructions(1)
               .InsertAndAdvance(
               new CodeInstruction(OpCodes.Call, typeof(PatchGameViewMatrixZoomLimit).GetMethod(nameof(PatchClamp)))
               );

            return codeMatcher.Instructions();
        }

        public static float ScaleClamp(float value)
        {
            return MathHelper.Clamp(value, 0.1f, 100f);
        }

        public static float PatchClamp(float value, float min, float max)
        {
            if (Enable.val == false) return MathHelper.Clamp(value, min, max);

            return ScaleClamp(value);
        }

        public static bool GetColorPrefix(ref Color __result)
        {
            if (Light.val == false) return true;

            __result = Color.White;
            return false;
        }
    }
}
