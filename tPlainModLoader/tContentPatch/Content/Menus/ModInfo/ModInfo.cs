using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using tContentPatch.ModLoad;
using Terraria;
using Terraria.UI;

namespace tContentPatch.Content.Menus.ModInfo
{
    internal class ModInfo
    {
        private static UIModInfo uistate = null;

        public static void OpenModInfoMenu(UIState backUI, ModObject mo = null, Action<ModObject> ActionDelMod = null)
        {
            if (Main.dedServ) return;
            if (Main.showSplash) return;

            if (uistate == null)
            {
                uistate = new UIModInfo();
            }
            uistate = new UIModInfo();
            uistate.BackUI = backUI;
            uistate.InitializeMod(mo, ActionDelMod);
            MenuUI.Menu.OpenMenu(uistate);
        }

        public static void OpenModDirectory(ModObject mo)
        {
            try
            {
                if (mo == null) return;
                if (Directory.Exists(mo.modPath) == false) return;
                Process.Start(mo.modPath);
            }
            catch { }
        }

        public static bool GetModIsLoaded(ModObject mo)
        {
            if (mo == null) return false;

            System.Collections.Generic.List<ModObject> mos = ContentPatch.GetModObjects();

            mo = mos?.FirstOrDefault(i => i.config.key == mo.config.key && i.modPath == mo.modPath);
            if (mo == null) return false;

            return mo.config.isEnable;
        }

        public static void JumpTo(ModLoad.ModInfo mi = null)
        {
            string path = mi?.jumpPath;
            if (path == null) return;

            try { _ = Process.Start(path); }
            catch { }
        }
    }
}
