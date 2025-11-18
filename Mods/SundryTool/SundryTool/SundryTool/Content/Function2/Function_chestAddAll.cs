using CommandHelp;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.ID;
using Terraria.UI;

namespace SundryTool.Content.Function2
{
    /// <summary>
    /// //NoPublic
    /// </summary>
    internal class Function_chestAddAll : PatchPlayer
    {
        public static GetSetReset<bool> chestAddAll = new GetSetReset<bool>();
        public static GetSetReset<int> chestAddAll_set = new GetSetReset<int>(1, 1, GetSetReset.GetIntFunc(1, ItemID.Count - 1));

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get1("chestAddAll", chestAddAll, chestAddAll_set, new CommandInt()),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get1(chestAddAll, chestAddAll_set, int.Parse, "<int>", text: "填满箱子"),
            };

            return uis;
        }

        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;
            if (chestAddAll.val == false) return;

            int ids = chestAddAll_set.val;

            if (This.chest < 0) return;
            Chest chest = Main.chest[This.chest];

            for (int i = 0; i < chest.item.Length; ++i)
            {
                if (chest.item[i] == null) continue;
                if (chest.item[i].type != 0 && chest.item[i].stack > 0) continue;

                int id = Utils.getRand(1, ids + 1);
                if (ItemID.Sets.Deprecated[i]) id = 1;//已弃用

                Item item = new Item();
                item.SetDefaults(id);
                item.stack = item.maxStack;

                chest.item[i] = item;

                NetMessage.SendData(32, -1, -1, null, This.chest, i);
            }
        }
    }
}
