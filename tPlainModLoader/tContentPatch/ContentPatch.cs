using System;
using System.Collections.Generic;
using System.Reflection;
using tContentPatch.ModLoad;
using tContentPatch.Patch;
using Terraria;

namespace tContentPatch
{
    public partial class ContentPatch
    {
        /// <summary/>
        public const string VersionTPlainModLoader = "1-beta14-t1.4.5.8";
        /// <summary/>
        public static string ModDirectory { get; private set; } = null;
        /// <summary/>
        public static bool Initialized { get; private set; } = false;
        /// <summary/>
        public static bool NoPublic = true;

        internal const string patchId_tContentPatch = "tContentPatch.gamePatch";
        internal const string patchId_mod = "tContentPatch.mod.patch";

        private static ContentPatch Instance = null;

        private static AddPatch gamePatch = null;
        internal static ModPatch.TypePatch typePatch = null;

        private static FieldInfo reflection_Terraria_Main_inRun = null;


        /// <summary>
        /// 不允许用正常方式创建
        /// </summary>
        private ContentPatch() { }

        /// <summary/>
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

        /// <summary>
        /// 返回复制的已加载模组列表, 加载失败时为null
        /// </summary>
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
        public static void RunCommand(string command)
        {
            Command.ProgramCommand.Run(command);
        }

        /// <summary>
        /// 输出到控制台和其它消息接收端
        /// </summary>
        public static void PrintTry(string s)
        {
            try
            {
                Console.WriteLine(s);
                Command.MsgCommand.SendMsg(s);
            }
            catch { }
        }
    }
}
