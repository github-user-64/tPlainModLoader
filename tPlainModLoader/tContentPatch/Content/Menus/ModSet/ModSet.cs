using System.Collections.Generic;
using Terraria;
using Terraria.UI;

namespace tContentPatch.Content.Menus.ModSet
{
    internal class ModSet
    {
        private static UIModSet uistate = null;

        public static void OpenModSetMenu(UIState backUI, List<ModSetting> mss, ModSetting open = null)
        {
            if (Main.dedServ) return;
            if (Main.showSplash) return;

            if (uistate == null)
            {
                uistate = new UIModSet();
            }

            uistate.InitializeSetList(backUI, mss, open);

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

        public static void SaveData(List<ModSetting> mss)
        {
            if (mss == null) return;

            foreach (ModSetting ms in mss)
            {
                try
                {
                    ms.Save();
                }
                catch { }
            }
        }
    }
}
