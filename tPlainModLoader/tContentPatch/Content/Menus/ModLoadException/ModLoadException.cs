using System;
using System.Diagnostics;
using System.IO;
using Terraria;
using Terraria.UI;

namespace tContentPatch.Content.Menus.ModLoadException
{
    internal class ModLoadException : UIState
    {
        private static UIModLoadException uistate = null;
        private static bool menuIsClose = true;

        public static void OpenModLoadExceptionMenu(Exception ex)
        {
            if (Main.dedServ) return;
            if (Main.showSplash) return;

            if (uistate == null)
            {
                uistate = new UIModLoadException(() => menuIsClose = true);
            }

            menuIsClose = false;

            uistate.InitializeException(ex);
            MenuUI.Menu.OpenMenu(uistate);
        }

        public static void WaitMenuClose()
        {
            while (menuIsClose == false) ;
        }

        public static void OpenModDirectory()
        {
            if (Directory.Exists(ContentPatch.ModDirectory) == false) return;

            try
            {
                Process.Start(ContentPatch.ModDirectory);
            }
            catch { }
        }
    }
}
