using System.Collections.Generic;
using tContentPatch.ModLoad;
using Terraria;
using Terraria.UI;

namespace tContentPatch.Content.Menus.ModSetSwitch
{
    internal class ModSetSwitch
    {
        private static UIModSetSwitch uistate = null;

        public static void OpenModSetSwitchMenu(UIState backUI, List<ModObject> mos = null, ModObject open = null)
        {
            if (Main.dedServ) return;
            if (Main.showSplash) return;

            if (uistate == null)
            {
                uistate = new UIModSetSwitch();
            }

            uistate.InitializeModList(backUI, mos, open);
            if (Main.gameMenu)
            {
                MenuUI.Menu.OpenMenu(uistate);
            }
            else
            {
                IngameFancyUI.CoverNextFrame();
                Main.playerInventory = false;
                Main.editChest = false;
                Main.npcChatText = "";
                Main.inFancyUI = true;
                Main.ClosePlayerChat();
                Main.chatText = "";
                Main.InGameUI.SetState(uistate);
            }
        }

        public static void OpenModSet(List<ModSetting> mss, ModSetting open = null)
        {
            ModSet.ModSet.OpenModSetMenu(uistate, mss, open);
        }
    }
}
