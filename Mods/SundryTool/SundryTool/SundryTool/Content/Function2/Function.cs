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
    internal class Function : PatchPlayer
    {
        public static GetSetReset<int> functionDamage = new GetSetReset<int>();
        public static GetSetReset<bool> aimAdvance = new GetSetReset<bool>();
        public static GetSetReset<float> aimAdvance_val = new GetSetReset<float>(38, 38);

        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;
        }

        public static List<CommandObject> GetCO()
        {
            bool ok = Function1.Function.NoPublic == false;

            List<CommandObject> cos = new List<CommandObject>();
            if (ok) cos.Add(new CommandHRA<int>("functionDamage", functionDamage, new CommandInt()));
            if (ok) cos.Add(CommandBuild.get1("aimAdvance", aimAdvance, aimAdvance_val, new CommandFloat()));
            if (ok) cos.AddRange(Function_newProjectileToPlay.GetCO());
            if (ok) cos.AddRange(Function_newItemToPlay.GetCO());
            if (ok) cos.AddRange(Function_mouseToPlayAngle.GetCO());
            if (ok) cos.AddRange(Function_damagePlay.GetCO());
            if (ok) cos.AddRange(Function_damagePlay2.GetCO());
            if (ok) cos.AddRange(Function_playToPlay.GetCO());
            cos.AddRange(Function_displayPlayMap.GetCO());
            cos.AddRange(Function_lightHack.GetCO());
            cos.AddRange(Function_mapRevealer.GetCO());
            cos.AddRange(Function_displayInfected.GetCO());
            if (ok) cos.AddRange(Function_chestAutoOpen.GetCO());
            if (ok) cos.AddRange(Function_chestAddAll.GetCO());
            if (ok) cos.AddRange(Function_chestDelAll.GetCO());
            cos.AddRange(Function_particleSpawn.GetCO());

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            bool ok = Function1.Function.NoPublic == false;

            List<UIElement> uis = new List<UIElement>();
            if (ok) uis.Add(new UI.UIItemTextBoxBind<int>(functionDamage, int.Parse, null, "功能伤害"){ MouseText = "影响部分功能的伤害<int>" });
            if (ok) uis.Add(UIBuild.get1(aimAdvance, aimAdvance_val, float.Parse, "应用于[鼠标指向玩家方向,伤害玩家]<float>", null, "预瞄"));
            if (ok) uis.AddRange(Function_newProjectileToPlay.GetUI());
            if (ok) uis.AddRange(Function_newItemToPlay.GetUI());
            if (ok) uis.AddRange(Function_mouseToPlayAngle.GetUI());
            if (ok) uis.AddRange(Function_damagePlay.GetUI());
            if (ok) uis.AddRange(Function_damagePlay2.GetUI());
            if (ok) uis.AddRange(Function_playToPlay.GetUI());
            uis.AddRange(Function_displayPlayMap.GetUI());
            uis.AddRange(Function_lightHack.GetUI());
            uis.AddRange(Function_mapRevealer.GetUI());
            uis.AddRange(Function_displayInfected.GetUI());
            if (ok) uis.AddRange(Function_chestAutoOpen.GetUI());
            if (ok) uis.AddRange(Function_chestAddAll.GetUI());
            if (ok) uis.AddRange(Function_chestDelAll.GetUI());
            uis.AddRange(Function_particleSpawn.GetUI());

            return uis;
        }
    }
}
