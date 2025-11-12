using CommandHelp;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using tContentPatch.Content.UI.ModSet;
using Terraria;
using Terraria.UI;

namespace SundryTool.Content.Function2
{
    internal class Function : ModPlayer
    {
        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;
        }

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>();
            cos.AddRange(Function_displayPlayMap.GetCO());
            cos.AddRange(Function_lightHack.GetCO());
            cos.AddRange(Function_mapRevealer.GetCO());
            cos.AddRange(Function_displayInfected.GetCO());
            cos.AddRange(Function_particleSpawn.GetCO());

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            UIItemTextBox u = new UIItemTextBox();

            List<UIElement> uis = new List<UIElement>();
            uis.AddRange(Function_displayPlayMap.GetUI());
            uis.AddRange(Function_lightHack.GetUI());
            uis.AddRange(Function_mapRevealer.GetUI());
            uis.AddRange(Function_displayInfected.GetUI());
            uis.AddRange(Function_particleSpawn.GetUI());

            return uis;
        }
    }
}
