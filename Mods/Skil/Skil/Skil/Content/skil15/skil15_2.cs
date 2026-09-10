using Skil.Utils;
using Skil.Utils.quickBuild;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.UI;

namespace Skil.Content.skil15
{
    //圣骑士锤tp
    internal static class skil15_2
    {
        public static GetSetReset<bool> Enable = new GetSetReset<bool>();

        public static List<UIElement> GetUI()
        {
            return new List<UIElement>()
            {
                UIBuild.get2(Enable, null, "Images/Item_1513", "瞬移"),
            };
        }

        public static void Update(Projectile proj, Player localPlayer)
        {
            if (Enable.val == false) return;
            if (proj.owner != localPlayer.whoAmI) return;
            if (proj.localAI[2] == 1) return;

            localPlayer.Center = proj.Center;
            NetMessage.SendData(MessageID.PlayerControls, number: localPlayer.whoAmI);
        }
    }
}
