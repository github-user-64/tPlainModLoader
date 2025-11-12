using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using tContentPatch.ModLoad;
using Terraria;
using Terraria.UI;

namespace tContentPatch.Content.Menus.ModManager
{
    internal class ModManager
    {
        private static LoadConfig loadConfig = null;
        private static UIModManager uistate = null;
        /// <summary>
        /// 与已加载的模组状态同步
        /// </summary>
        private static List<ModObject> mos = null;
        /// <summary>
        /// ui上显示的状态与<see cref="mos"/>不同
        /// </summary>
        private static List<ModObject> mosui = null;

        public static void OpenModManagerMenu(UIState backUI)
        {
            if (Main.dedServ) return;
            if (Main.showSplash) return;

            if (uistate == null)
            {
                loadConfig = new LoadConfig(ContentPatch.ModDirectory);

                uistate = new UIModManager();
            }

            uistate.backUI = backUI;
            RefreshModList();
            MenuUI.Menu.OpenMenu(uistate);
        }

        /// <summary>
        /// <see cref="mos"/>和已加载的模组状态一致, <see cref="mosui"/>和配置文件一致
        /// </summary>
        public static void RefreshModList()
        {
            mos = new List<ModObject>();
            mosui = new List<ModObject>();

            List<ModObject> loaded = ContentPatch.GetModObjects();
            List<ModObject> mosRead = ReadModConfig();

            foreach (ModObject moR in mosRead)
            {
                mosui.Add(ModObject.Copy(moR));

                ModObject loadedmo = loaded?.FirstOrDefault(i => i.config.key == moR.config.key && i.modPath == moR.modPath);
                if (loadedmo == null)
                {
                    moR.config.isEnable = false;
                    mos.Add(moR);
                }
                else
                {
                    mos.Add(loadedmo);
                }
            }

            uistate.InitializeModList(mosui);
        }

        public static void OpenModInfo(ModObject mo)
        {
            ModInfo.ModInfo.OpenModInfoMenu(uistate, mo, DelMod);
        }

        public static void OpenModSetSwitch(ModObject open = null)
        {
            if (mos == null)
            {
                ModSetSwitch.ModSetSwitch.OpenModSetSwitchMenu(uistate);
                return;
            }

            int index = mosui.IndexOf(open);
            if (index == -1) open = null;
            else open = mos[index];

            List<ModObject> mosE = mos.Where(i => i.config.isEnable).ToList();

            ModSetSwitch.ModSetSwitch.OpenModSetSwitchMenu(uistate, mosE, open);
        }

        public static bool GetModIsLoaded(ModObject mo)
        {
            if (mos == null) return false;

            int index = mosui.IndexOf(mo);
            if (index == -1) return false;

            return mos[index].config.isEnable;
        }

        public static void DelMod(ModObject mo)
        {
            if (mos == null) return;

            int index = mosui.IndexOf(mo);
            if (index == -1) return;

            if (mos[index].config.isEnable) return;//原本是启用的

            try
            {
                string path = mos[index].modPath;
                if (Directory.Exists(path) == false) return;
                Directory.Delete(path, true);
                RefreshModList();
            }
            catch { }
        }

        public static void OpenDirectory(string path)
        {
            try
            {
                if (Directory.Exists(path) == false) return;
                Process.Start(path);
            }
            catch { }
        }

        public static void OpenModDirectory()
        {
            try
            {
                if (Directory.Exists(ContentPatch.ModDirectory) == false)
                {
                    Directory.CreateDirectory(ContentPatch.ModDirectory);
                }
            }
            catch { }

            OpenDirectory(ContentPatch.ModDirectory);
        }

        public static void ModEnableReversal(ModObject mo, bool? E = null)
        {
            string filePath = Path.Combine(mo.modPath, InfoList.Files.ModLoadConfig);
            ModConfig mc = null;

            try
            {
                if (File.Exists(filePath) == false)
                {
                    if (Directory.Exists(mo.modPath) == false) return;

                    Utils.MyJson1.Save(mo.config, filePath);
                }

                mc = Utils.MyJson1.Get2<ModConfig>(filePath);

                mc.isEnable = E ?? !mo.config.isEnable;
                Utils.MyJson1.Save(mc, filePath);

                //

                mc = Utils.MyJson1.Get2<ModConfig>(filePath);
                mo.config.isEnable = mc.isEnable;
            }
            catch { }
        }

        public static void ModEnableAll()
        {
            if (mos == null) return;
            foreach (ModObject i in mosui) ModEnableReversal(i, true);
        }

        public static void ModNoEnableAll()
        {
            if (mos == null) return;
            foreach (ModObject i in mosui) ModEnableReversal(i, false);
        }

        public static bool CheckModUpdate()
        {
            if (mos == null) return false;

            for (int i = 0; i < mos.Count; ++i)
            {
                if (mos[i].config.isEnable != mosui[i].config.isEnable) return true;
            }

            return false;
        }

        public static void AgainLoadMod()
        {
            LoaderControl.Load();
        }

        private static List<ModObject> ReadModConfig()
        {
            try
            {
                while (loadConfig.IsLoading()) ;
                return loadConfig.Load();
            }
            catch
            {
                return null;
            }
        }
    }
}
