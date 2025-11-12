using System;
using tContentPatch.ModLoad;
using Terraria;

namespace tContentPatch.Content.Menus.ModLoadingMenu
{
    internal class ModLoadingMenu
    {
        private static UILoadProgressBar uistate = null;

        internal static void OpenLoadMenu(IModLoaderState modLoaderState, Action cancelLoad)
        {
            if (Main.dedServ) return;
            if (Main.showSplash) return;

            if (uistate == null)
            {
                uistate = new UILoadProgressBar();
            }

            if (modLoaderState != null)
            {
                uistate.InitializeLoader(modLoaderState, cancelLoad);
                MenuUI.Menu.OpenMenu(uistate);
            }
        }
    }
}
