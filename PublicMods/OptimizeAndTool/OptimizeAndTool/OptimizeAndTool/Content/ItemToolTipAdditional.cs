using CommandHelp;
using Microsoft.Xna.Framework.Input;
using OptimizeAndTool.Utils;
using OptimizeAndTool.Utils.quickBuild;
using System;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;

namespace OptimizeAndTool.Content
{
    public class ItemToolTipAdditional : ModMain
    {
        public static GetSetReset<bool> Enable = new GetSetReset<bool>(true, true);

        public static List<Func<Item, string[]>> ItemInfo { get; private set; } = new List<Func<Item, string[]>>();

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get2("itemTip", Enable),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get2(Enable, text: "物品额外提示"),
            };

            return uis;
        }

        public override void MouseText_DrawItemTooltip_GetLinesInfoPostfix(Item item, ref int yoyoLogo, ref int researchLine, ref float oldKB, ref int numLines, ref string[] toolTipLine, ref bool[] preFixLine, ref bool[] badPreFixLine)
        {
            if (Enable.val == false) return;

            bool isOpen = PlayerInput.GetPressedKeys().Contains(Keys.LeftShift);

            if (isOpen == false)
            {
                if (numLines < toolTipLine.Length == false) return;

                toolTipLine[numLines] = $"[c/aaaaaa:按住<{Keys.LeftShift}>显示更多信息]";
                numLines++;

                return;
            }

            foreach (Func<Item, string[]> i in ItemInfo)
            {
                if (i == null) continue;
                string[] info = i.Invoke(item);
                if (info == null) continue;

                foreach (string s in info)
                {
                    if (numLines < toolTipLine.Length == false) return;
                    if (s == null) continue;

                    toolTipLine[numLines] = s;
                    numLines++;
                }
            }
        }
    }
}
