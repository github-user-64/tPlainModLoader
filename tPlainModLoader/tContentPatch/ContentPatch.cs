using System;
using System.Collections.Generic;
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
        public static string ModDirectory { get; private set; } = null;

        internal const string patchId_tContentPatch = "tContentPatch.gamePatch";
        internal const string patchId_mod = "tContentPatch.mod.patch";

        private static ContentPatch Instance = null;

        private static AddPatch gamePatch = null;

        private static FieldInfo reflection_Terraria_Main_inRun = null;


        private ContentPatch() { }


        public void Initialize()
        {
            if (Instance == null) Instance = this;
            else throw new Exception("不可重复初始化");

            Initialize_ModDirectory();
            Initialize_AddPatch();

            LoadConfig lc = new LoadConfig(ModDirectory);
            LoadAssembly la = new LoadAssembly(lc);
            Intercept intercept = new Intercept(la);
            LoadInstance li = new LoadInstance(intercept);//在实例化模组对象前拦截
            ModLoader ml = new ModLoader(li, patchId_mod);

            LoaderControl.SetModLoader(ml, intercept);
            //加载时
            LoaderControl.OnModLoad_Start += (e) => Content.Menus.ModLoadingMenu.ModLoadingMenu.OpenLoadMenu(e, LoaderControl.CancelLoad);
            //加载完成时
            LoaderControl.OnModLoad_Ok += () => Main.menuMode = MenuID.Title;
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
                Log.SaveTry();
                Environment.Exit(0);
            };

            if (Main.dedServ)
            {
                LoaderControl.Load();
            }
        }

        public bool CanInitialize()
        {
            if (Main.dedServ)
            {
                return Netplay.Disconnect == false;
            }
            else
            {
                //服务端的Main.instance被赋值前Main.dedServ会先为true
                if (Main.instance == null) return false;

                if (reflection_Terraria_Main_inRun == null)
                {
                    Type type = typeof(Main).BaseType;
                    reflection_Terraria_Main_inRun = type.GetField("inRun", BindingFlags.NonPublic | BindingFlags.Instance);

                }
                bool inRun = (bool)reflection_Terraria_Main_inRun.GetValue(Main.instance);

                return inRun;
            }
        }

        private static void Initialize_AddPatch()
        {
            gamePatch = new AddPatch(patchId_tContentPatch);
            gamePatch.AllPatch();
            ModPatch.Patch_ModMain.Initialize(gamePatch);
            ModPatch.Patch_ModPlayer.Initialize(gamePatch);
            ModPatch.Patch_ModNPC.Initialize(gamePatch);
            ModPatch.Patch_ModItem.Initialize(gamePatch);
            ModPatch.Patch_ModProjectile.Initialize(gamePatch);
            ModPatch.Patch_ModTileLightScanner.Initialize(gamePatch);
            ModPatch.Patch_ModRemadeChatMonitor.Initialize(gamePatch);
        }

        private static void Initialize_ModDirectory()
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

        /// <summary>
        /// 返回复制的已加载模组列表, 加载失败时为null
        /// </summary>
        /// <returns></returns>
        public static List<ModObject> GetModObjects()
        {
            List<ModObject> mos = LoaderControl.GetModObjects();
            if (mos == null) return null;

            List<ModObject> rmos = new List<ModObject>();

            foreach (ModObject mo in mos)
            {
                rmos.Add(ModObject.Copy(mo));
            }

            return rmos;
        }

        /// <summary>
        /// 用已有的指令列表运行指令
        /// </summary>
        /// <param name="command"></param>
        public static void RunCommand(string command)
        {
            Command.ProgramCommand.Run(command);
        }
    }
}
