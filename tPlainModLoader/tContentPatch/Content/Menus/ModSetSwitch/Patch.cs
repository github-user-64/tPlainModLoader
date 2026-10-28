using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Terraria;
using Terraria.Audio;
using Terraria.Social.Base;

namespace tContentPatch.Content.Menus.ModSetSwitch
{
    [HarmonyPatch(typeof(IngameOptions), nameof(IngameOptions.Draw))]
    internal static class Patch
    {
        private static float scale = 0f;
        private static readonly int index_num10 = 21;
        private static readonly int index_num8 = 18;

        //[HarmonyDebug]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpiler_Draw(IEnumerable<CodeInstruction> instructions)
        {
            CodeMatcher codeMatcher = new CodeMatcher(instructions);

            CodeMatch[] cs = null;

            #region 改数量
            cs = new CodeMatch[]
            {
                new CodeMatch(OpCodes.Callvirt, typeof(NetSocialModule).GetMethod(nameof(NetSocialModule.CanInvite))),
                new CodeMatch(OpCodes.Br_S),
                new CodeMatch(OpCodes.Ldc_I4_0),
                new CodeMatch(OpCodes.Stloc_S),//16

                //int num7 = (flag5 ? 1 : 0);
                new CodeMatch(OpCodes.Ldloc_S),//16
                new CodeMatch(OpCodes.Brtrue_S),
                new CodeMatch(OpCodes.Ldc_I4_0),
                new CodeMatch(OpCodes.Br_S),
                new CodeMatch(OpCodes.Ldc_I4_1),
                new CodeMatch(OpCodes.Stloc_S),//17

                //int num8 = 5 + num7 + 2;
                new CodeMatch(OpCodes.Ldc_I4_5),
                new CodeMatch(OpCodes.Ldloc_S),//17
                new CodeMatch(OpCodes.Add),
                new CodeMatch(OpCodes.Ldc_I4_2),
                new CodeMatch(OpCodes.Add),
                new CodeMatch(i => i.opcode == OpCodes.Stloc_S && (i.operand as LocalBuilder)?.LocalIndex == index_num8),
            };

            codeMatcher.MatchStartForward(cs);
            codeMatcher.Advance(cs.Length - 1);//在add前
            if (codeMatcher.IsInvalid)
            {
                ContentPatch.PrintTry($"无法在游戏选择窗口插入选项:找不到IL位置");
                return instructions;
            }

            int count = 1;//数量加1
            codeMatcher.InsertAndAdvance(
                new CodeMatch(OpCodes.Ldc_I4_S, count),
                new CodeMatch(OpCodes.Add));
            #endregion

            #region 插入
            cs = new CodeMatch[]
            {
                new CodeMatch(OpCodes.Ldloc_S),
                new CodeMatch(OpCodes.Brfalse_S),

                new CodeMatch(OpCodes.Ldarg_1),
                new CodeMatch(OpCodes.Ldsfld, typeof(Lang).GetField(nameof(Lang.menu))),
                new CodeMatch(OpCodes.Ldc_I4, 147),//147
                new CodeMatch(OpCodes.Ldelem_Ref),
                new CodeMatch(OpCodes.Callvirt),
                new CodeMatch(i => i.opcode == OpCodes.Ldloc_S && (i.operand as LocalBuilder)?.LocalIndex == index_num10),
            };

            codeMatcher.MatchStartForward(cs);
            if (codeMatcher.IsInvalid)
            {
                ContentPatch.PrintTry($"无法在游戏选择窗口插入选项:找不到IL位置");
                return instructions;
            }

            codeMatcher.InsertAndAdvance(
                new CodeMatch(OpCodes.Ldarg_1),
                new CodeMatch(OpCodes.Ldloca_S, index_num10),
                new CodeMatch(OpCodes.Ldloc_S, 19),
                new CodeMatch(OpCodes.Ldloc_S, 20),
                new CodeMatch(OpCodes.Call, typeof(Patch).GetMethod(nameof(DrawLeftSide), BindingFlags.Static | BindingFlags.NonPublic)));
            #endregion

            return codeMatcher.Instructions();
        }

        private static void DrawLeftSide(SpriteBatch sb, ref int i, Vector2 anchor, Vector2 offset)
        {
            float minscale = 0.7f;
            float maxscale = 0.8f;

            Color color = Color.Lerp(Color.Gray, Color.White, (scale - minscale) / (maxscale - minscale));

            Vector2 vector = Terraria.Utils.DrawBorderStringBig(sb, "模组设置", anchor + offset * (1 + i), color, scale, 0.5f, 0.5f);
            bool flag2 = new Rectangle((int)anchor.X - (int)vector.X / 2, (int)anchor.Y + (int)(offset.Y * (float)(1 + i)) - (int)vector.Y / 2, (int)vector.X, (int)vector.Y).Contains(new Point(Main.mouseX, Main.mouseY));

            ++i;

            float add = flag2 ? 0.01f : -0.01f;
            scale += add;
            scale = MathHelper.Clamp(scale, minscale, maxscale);

            if (flag2 && Main.mouseLeft && Main.mouseLeftRelease)
            {
                SoundEngine.PlaySound(10);
                ModSetSwitch.OpenModSetSwitchMenu(null, ContentPatch.GetModObjects());
            }
        }
    }
}
