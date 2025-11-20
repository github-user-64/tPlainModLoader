using System;
using System.IO;
using System.Reflection;
using tContentPatch;
using tContentPatch.Utils;

namespace tPlainModLoader
{
    internal partial class Program
    {
        public static string LauncherFilePath => launchConfig?.config?.LauncherFilePath;
        private static ConfigHelp<LauncherConfig> launchConfig = null;
        private static string ProgramPath = null;

        public static void Main(string[] args)
        {
            try
            {
                ProgramPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (Directory.Exists(ProgramPath) == false)
                {
                    Console.WriteLine($"路径不存在:[{ProgramPath}]");
                    Console.ReadKey(true);
                    return;
                }

                Log.SetPath(Path.Combine(ProgramPath, InfoList.Files.Log));
                Log.Add($"{nameof(Program)}:初始化");
                Console.WriteLine($"初始化");

                Initialize_Config();
                Initialize_AssemblyResolveEvent();
                LaunchGame.Initialize(launchConfig?.config);

                Log.Add($"{nameof(Program)}:初始化完成");
            }
            catch (Exception ex)
            {
                Log.Add($"{nameof(Program)}:初始化失败:{ex}");
                Log.SaveTry();

                Console.WriteLine($"初始化失败:");
                Console.WriteLine($"{ex}");
                Console.ReadKey(true);
                return;
            }

            //

            Log.Add($"{nameof(Program)}:启动目标程序");
            Console.WriteLine("启动目标程序");

            if (LaunchTargetProgram())
            {
                Log.Add($"{nameof(Program)}:启动目标程序成功");
                Console.WriteLine("已启动");
            }
            else
            {
                Log.Add($"{nameof(Program)}:启动目标程序失败");
                Log.SaveTry();

                Console.WriteLine("启动失败");
                return;
            }

            //

            Log.Add($"{nameof(Program)}:初始化内容补丁");
            Console.WriteLine("初始化内容补丁");
            if (Initialize_tContentPatch())
            {
                Log.Add($"{nameof(Program)}:初始化内容补丁成功");
                Console.WriteLine("初始化内容补丁成功");
            }
            else
            {
                Log.Add($"{nameof(Program)}:初始化内容补丁失败");
                Log.SaveTry();
                Console.WriteLine("初始化内容补丁失败");
                return;
            }

            string[] titles = new string[] { "tPlainModLoader", "固态泰拉瑞亚", "固态地面:)", "static tile", "this is 固态硬盘", "泰拉瑞亚!启动!",
                "简易模组加载器的意思是做的很简陋", "试试tModLoader", "你知道吗?传说有个叫tModLoader的比这个好一万倍!",
                "如果tPML崩溃了请冷静, 这是正常现象", "你让我怎么棱镜!", "按[Alt]和[F4]免费领取天顶剑", "Null", "也许tPML做好后1.4.5还没出",
                "修修补补又一年", "世纪之花灯泡",
                "哈!哈!你发现了彩蛋!"};
            Console.Title = titles[new Random().Next(0, titles.Length - 1)];

            while (true)
            {
                string s = Console.ReadLine();
                try { ContentPatch.RunCommand(s); }
                catch (Exception ex)
                {
                    Console.WriteLine($"指令运行失败:{ex.Message}");
                }
            }

            //!
            Console.WriteLine("ok");
            Console.ReadLine();
            //!
        }

        private static bool LaunchTargetProgram()
        {
            try
            {
                LaunchGame.Run(LauncherFilePath, OnProgramExit);
                return true;
            }
            catch (Exception ex)
            {
                Log.Add($"{nameof(Program)}:启动目标程序失败:{ex}");
                Console.WriteLine(ex);
                return false;
            }
        }

        private static void OnProgramExit()
        {
            Log.Add($"{nameof(Program)}:目标程序退出");
            Log.SaveTry();
            Console.WriteLine("目标程序退出");

            Environment.Exit(0);
        }

        private static bool Initialize_tContentPatch()
        {
            Type type = typeof(ContentPatch);
            ContentPatch cp = (ContentPatch)Activator.CreateInstance(type, true);

            while (cp.CanInitialize() == false) ;

            try
            {
                cp.Initialize();
            }
            catch (Exception ex)
            {
                Log.Add($"{nameof(Program)}:初始化内容补丁失败:{ex}");
                Console.WriteLine($"初始化失败:{ex.Message}");
                return false;
            }
            return true;
        }

        private static void Initialize_Config()
        {
            launchConfig = new ConfigHelp<LauncherConfig>(Path.Combine(ProgramPath, "launchConfig.json"));
            launchConfig.UpdateConfig(() => new LauncherConfig());

            Log.Add($"{nameof(Program)}:启动文件位置:{LauncherFilePath}");
        }

        private static void Initialize_AssemblyResolveEvent()
        {
            //用来处理重复加载程序集的问题
            AppDomain.CurrentDomain.AssemblyResolve += (s, e) =>
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

                for (int i = 0; i < assemblies.Length; ++i)
                {
                    if (e.Name == assemblies[i].FullName)
                        return assemblies[i];
                }

                return null;
            };
        }
    }
}
