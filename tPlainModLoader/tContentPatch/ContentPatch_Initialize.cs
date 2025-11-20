using System;
using System.IO;
using System.Reflection;
using tContentPatch.Content.Menus.ModLoadException;
using tContentPatch.Content.Menus.ModManager;
using tContentPatch.ModLoad;
using tContentPatch.Patch;
using tContentPatch.Utils;
using Terraria;
using Terraria.ID;

namespace tContentPatch
{
    public partial class ContentPatch
    {
        public void Initialize(bool pipe = false)
        {
            if (Instance == null) Instance = this;
            else throw new Exception("不可重复初始化");

            Initialized = false;

            Command.Pipe.Initialize(pipe);
            Initialize_ModDirectory();
            Initialize_AddPatch();
            Initialize_ModLoader();

            Initialized = true;

            if (Main.dedServ)
            {
                LoaderControl.Load();
            }
        }

        private void Initialize_ModLoader()
        {
            LoadConfig lc = new LoadConfig(ModDirectory);
            LoadAssembly la = new LoadAssembly(lc);
            Intercept intercept = new Intercept(la);
            LoadInstance li = new LoadInstance(intercept);//在实例化模组对象前拦截
            ModLoader ml = new ModLoader(li, patchId_mod);

            LoaderControl.SetModLoader(ml, intercept);
            //加载时
            LoaderControl.OnModLoad_Start += (e) => Content.Menus.ModLoadingMenu.ModLoadingMenu.OpenLoadMenu(e, LoaderControl.CancelLoad);
            //加载完成时
            LoaderControl.OnModLoad_Ok += () =>
            {
                if (Main.netMode != 0 && Main.netMode != 1) return;
                Main.menuMode = MenuID.Title;
            };
            //取消时
            LoaderControl.OnModLoad_Cancel += (e) => Content.Menus.ModLoadingMenu.ModLoadingMenu.OpenLoadMenu(e, LoaderControl.CancelLoad);
            //取消完成
            LoaderControl.OnModLoad_Canceled += () => ModManager.OpenModManagerMenu(null);
            //加载异常时
            LoaderControl.OnModLoad_Exception += (e) =>
            {
                Log.SaveTry();
                ModLoadException.OpenModLoadExceptionMenu(e);
                ModLoadException.WaitMenuClose();
                ModManager.OpenModManagerMenu(null);
            };
            //卸载异常时
            LoaderControl.OnModUnload_Exception += (e) =>
            {
                Log.SaveTry();
                Environment.Exit(0);
            };
        }

        private void Initialize_AddPatch()
        {
            gamePatch = new AddPatch(patchId_tContentPatch);
            gamePatch.AllPatch();

            listPatch = new ModPatch.ListPatch(gamePatch);
            listPatch.AddPatchAndInit(new ModPatch.Patch_Main());
            listPatch.AddPatchAndInit(new ModPatch.Patch_Player());
            listPatch.AddPatchAndInit(new ModPatch.Patch_NPC());
            listPatch.AddPatchAndInit(new ModPatch.Patch_Item());
            listPatch.AddPatchAndInit(new ModPatch.Patch_Projectile());
            listPatch.AddPatchAndInit(new ModPatch.Patch_TileLightScanner());
            listPatch.AddPatchAndInit(new ModPatch.Patch_RemadeChatMonitor());
        }

        private void Initialize_ModDirectory()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (Directory.Exists(path) == false) throw new Exception($"目录不存在[{path}]");

            ModDirectory = Path.Combine(path, InfoList.Directorys.Mods);

            if (Directory.Exists(ModDirectory) == false)
            {
                Directory.CreateDirectory(ModDirectory);
            }

            if (Directory.Exists(ModDirectory) == false) throw new Exception($"目录不存在[{ModDirectory}]");

            Log.Add($"{nameof(ContentPatch)}:模组目录:{ModDirectory}");
        }
    }
}
