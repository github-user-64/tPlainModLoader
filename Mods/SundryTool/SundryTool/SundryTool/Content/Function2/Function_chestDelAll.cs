using CommandHelp;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.UI;

namespace SundryTool.Content.Function2
{
    /// <summary>
    /// //NoPublic
    /// </summary>
    internal class Function_chestDelAll : PatchPlayer
    {
        public static GetSetReset<bool> chestDelAll = new GetSetReset<bool>();

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get2("chestDelAll", chestDelAll),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get2(chestDelAll, text: "清空箱子"),
            };

            return uis;
        }

        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;
            if (chestDelAll.val == false) return;

            if (This.chest < 0) return;
            Chest chest = Main.chest[This.chest];

            for (int i = 0; i < chest.item.Length; ++i)
            {
                if (chest.item[i] == null) continue;
                if (chest.item[i].stack < 1) continue;

                chest.item[i] = new Item();

                NetMessage.SendData(32, -1, -1, null, This.chest, i);
            }
        }
    }
}
