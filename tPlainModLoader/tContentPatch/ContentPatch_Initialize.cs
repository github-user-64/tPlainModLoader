using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using tContentPatch.Content;
using tContentPatch.Content.Menus.ModLoadException;
using tContentPatch.Content.Menus.ModManager;
using tContentPatch.ModLoad;
using tContentPatch.Patch;
using tContentPatch.Utils;
using Terraria;
using Terraria.ID;

namespace tContentPatch
{
    /// <summary/>
    public partial class ContentPatch
    {
        /// <summary>
        /// 在修补前启动的线程使用的方法还是修补前的, 最好在有线程启动前修补
        /// </summary>
        public void Initialize()
        {
            Log.Add($"{nameof(ContentPatch)}:初始化");
            string p = $"{nameof(ContentPatch)}:版本:{VersionTPlainModLoader}";
            Log.Add(p);
            PrintTry(p);

            if (Instance == null) Instance = this;
            else throw new Exception("不可重复初始化");

            Initialized = false;
            Log.Add($"{nameof(ContentPatch)}:服务端:{Main.dedServ}");

            Initialize_CommandMsg();
            Initialize_ModDirectory();
            Initialize_AddPatch();
            Initialize_ModLoader();
            Initialize_CMD();

            Initialized = true;

            Log.Add($"{nameof(ContentPatch)}:初始化完成");

            if (Main.dedServ)
            {
                LoaderControl.Load();
            }
        }

        private void Initialize_CommandMsg()
        {
            Command.MsgCommand.Initialize();
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
                ModLoadException.OpenModLoadExceptionMenu(e);
                ModLoadException.WaitMenuClose();
                ModManager.OpenModManagerMenu(null);
            };
            //卸载异常时
            LoaderControl.OnModUnload_Exception += (e) =>
            {
                Environment.Exit(0);
            };
        }

        private void CheckNetplayConnect()
        {
            bool has = false;
            if (Netplay.TcpListener != null) has = true;
            else if (Main.netMode == 1) has = true;

            if (has == false) return;

            string s = $"{nameof(ContentPatch)}:在修补前已有线程启动(在多人游戏中或服务端已启动),一些功能会失效";
            Log.Add(s);
            PrintTry(s);
        }

        private void Initialize_AddPatch()
        {
            CheckNetplayConnect();

            gamePatch = new AddPatch(patchId_tContentPatch);
            gamePatch.AllPatch();

            typePatch = new ModPatch.TypePatch();
            typePatch.AddPatch(new ModPatch.Patch_Main());
            typePatch.AddPatch(new ModPatch.Patch_Player());
            typePatch.AddPatch(new ModPatch.Patch_NPC());
            typePatch.AddPatch(new ModPatch.Patch_Item());
            typePatch.AddPatch(new ModPatch.Patch_Projectile());
            typePatch.AddPatch(new ModPatch.Patch_TileLightScanner());
            typePatch.AddPatch(new ModPatch.Patch_RemadeChatMonitor());
            typePatch.AddPatch(new ModPatch.Patch_WorldFile());
            typePatch.AddPatch(new ModPatch.Patch_NetMessage());
            typePatch.AddPatch(new ModPatch.Patch_MessageBuffer());
            typePatch.AddPatch(new ModPatch.Patch_Chest());
            typePatch.AddPatch(new ModPatch.Patch_RemoteClient());
            typePatch.AddPatch(new ModPatch.Patch_WorldGen());
        }

        private void Initialize_ModDirectory()
        {
            Log.Add($"{nameof(ContentPatch)}:初始化模组目录");

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

        private void Initialize_CMD()
        {
            if (Main.dedServ)
            {
                DedServConsoleCommand.Enable = true;
                return;
            }

            _ = Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        System.Threading.Thread.Sleep(1);
                        string s = Console.ReadLine();//注入的没有控制台时这玩意会直接返回null
                        if (s == null) continue;
                        RunCommand(s);
                    }
                    catch (Exception ex)
                    {
                        PrintTry($"指令运行失败:{ex.Message}");
                    }
                }
            });
        }
    }
}
