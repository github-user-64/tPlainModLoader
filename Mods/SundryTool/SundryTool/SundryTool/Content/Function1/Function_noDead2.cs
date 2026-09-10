using CommandHelp;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.DataStructures;
using Terraria.UI;

namespace SundryTool.Content.Function1
{
    internal class Function_noDead2 : PatchPlayer
    {
        public static GetSetReset<bool> noDead2 = new GetSetReset<bool>();

        public override bool KillMePrefix(Player This, PlayerDeathReason damageSource, double dmg, int hitDirection, bool pvp)
        {
            if (This != Main.LocalPlayer) return true;
            return noDead2.val == false;
        }

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get2("noDead2", noDead2),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get2(noDead2, "重新加载模组后该功能会失控", "Images/Buff_58", "不死2"),
            };

            return uis;
        }
    }
}
