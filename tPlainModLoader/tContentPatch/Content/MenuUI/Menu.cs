using Terraria;
using Terraria.UI;

namespace tContentPatch.Content.MenuUI
{
    public class Menu
    {
        public static bool OpenMenu(UIState uistate, bool inGame = false)
        {
            if (Main.dedServ) return false;
            if (Main.showSplash) return false;

            if (inGame)
            {
                Main.InGameUI.SetState(uistate);
            }
            else
            {
                Main.menuMode = 888;
                Main.MenuUI.SetState(uistate);
            }

            return true;
        }

        public static bool OpenInGameMenu(UIState uistate)
        {
            return OpenMenu(uistate, true);
        }
    }
}
