using CommandHelp;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.UI;

namespace SundryTool.Content.PlayerModify
{
    internal class ValSet : PatchPlayer
    {
        public static GetSetReset<bool> damage = new GetSetReset<bool>();
        public static GetSetReset<float> damage_val = new GetSetReset<float>();
        public static GetSetReset<bool> armorPenetration = new GetSetReset<bool>();
        public static GetSetReset<int> armorPenetration_val = new GetSetReset<int>();
        public static GetSetReset<bool> maxMinions = new GetSetReset<bool>();
        public static GetSetReset<int> maxMinions_val = new GetSetReset<int>();
        public static GetSetReset<bool> endurance = new GetSetReset<bool>();
        public static GetSetReset<float> endurance_val = new GetSetReset<float>();

        public override void UpdateArmorSetsPostfix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;

            if (damage.val)
            {
                This.magicDamage += damage_val.val;
                This.meleeDamage += damage_val.val;
                This.rangedDamage += damage_val.val;
                This.minionDamage += damage_val.val;
            }

            if (armorPenetration.val)
            {
                This.armorPenetration = armorPenetration_val.val;
            }

            if (maxMinions.val)
            {
                This.maxMinions = maxMinions_val.val;
                This.maxTurrets = maxMinions_val.val;
            }

            if (endurance.val)
            {
                This.endurance = endurance_val.val;
            }
        }

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get1("damage", damage, damage_val, new CommandFloat()),
                CommandBuild.get1("armorPenetration", armorPenetration, armorPenetration_val, new CommandInt()),
                CommandBuild.get1("maxMinions", maxMinions, maxMinions_val, new CommandInt()),
                CommandBuild.get1("endurance", endurance, endurance_val, new CommandFloat()),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get1(damage, damage_val, float.Parse, "<float>", "Images/Buff_180", "伤害倍率"),
                UIBuild.get1(armorPenetration, armorPenetration_val, int.Parse, "<float>", "Images/Buff_159", "穿甲"),
                UIBuild.get1(maxMinions, maxMinions_val, int.Parse, "<int>", "Images/Buff_150", "召唤物上限"),
                UIBuild.get1(endurance, endurance_val, float.Parse, "<float>", "Images/Buff_114", "减伤"),
            };

            return uis;
        }
    }
}
