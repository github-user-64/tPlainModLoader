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
    internal class Function_chestAutoOpen : PatchPlayer
    {
        public static GetSetReset<bool> chestAutoOpen = new GetSetReset<bool>();

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get2("chestAutoOpen", chestAutoOpen),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get2(chestAutoOpen, text: "打开最近箱子"),
            };

            return uis;
        }

        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;
            if (chestAutoOpen.val == false) return;

            int id = Utils.getNearbyChestIndex(This.Center);
            if (id == -1) return;
            if (This.chest == id) return;

            if (Main.netMode == 0)
            {
                This.chest = id;
            }
            else
            if (Main.netMode == 1)
            {
                if (Main.GameUpdateCount % 60 != 0) return;
                NetMessage.SendData(31, -1, -1, null, Main.chest[id].x, Main.chest[id].y);
            }
        }
    }
}
