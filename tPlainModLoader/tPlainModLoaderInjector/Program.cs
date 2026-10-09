using System;
using System.IO;
using System.Reflection;
using System.Text;
using tContentPatch;
using tContentPatch.Utils;

namespace tPlainModLoaderInjector
{
    internal partial class Program
    {
        public static string[] InjectorProgramName => launchConfig?.config?.InjectorProgramName;
        private static ConfigHelp<LauncherConfig> launchConfig = null;
        public static string ProgramPath = null;
        public static string InjectDllFilePath = null;

        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

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
                Log.Clear();
                DateTime time = DateTime.Now;
                Log.Add($"{nameof(Program)}:{time.Year}.{time.Month}.{time.Day}");
                Log.Add($"{nameof(tPlainModLoaderInjector)}:初始化");
                Console.WriteLine($"初始化");

                Initialize_Config();
                Initialize_Inject();

                Log.Add($"{nameof(tPlainModLoaderInjector)}:初始化完成");
            }
            catch (Exception ex)
            {
                Log.Add($"{nameof(tPlainModLoaderInjector)}:初始化失败:{ex}");

                Console.WriteLine($"初始化失败:");
                Console.WriteLine($"{ex}");
                Console.ReadKey(true);
                return;
            }

            Console.Title = "tPlainModLoaderInjector";
            int pid = -1;

            try
            {
                Log.Add($"{nameof(tPlainModLoaderInjector)}:选择目标程序pid");
                Console.WriteLine("选择目标程序pid");
                pid = ProcessUtils.SwitchPID(InjectorProgramName);
            }
            catch (Exception ex)
            {
                Log.Add($"{nameof(tPlainModLoaderInjector)}:选择失败:{ex}");

                Console.WriteLine($"选择失败:");
                Console.WriteLine($"{ex}");
                Console.ReadKey(true);
                return;
            }

            int commandPort = -1;//接收指令的端口

            try
            {
                Log.Add($"{nameof(tPlainModLoaderInjector)}:尝试注入:{pid}");
                Console.WriteLine($"尝试注入:{pid}");

                int state = InjectorGame.Injector(pid, InjectDllFilePath);
                string stateString = null;
                switch (state)
                {
                    case 0: stateString = "注入失败"; break;
                    //case 1: stateString = "注入成功"; break;
                    //case 2: stateString = "已注入"; break;
                    case -1: stateString = "附加到进程失败"; break;
                    case -2: stateString = "初始化内容失败"; break;
                    case -3: stateString = "注入成功或已注入,但未启用指令接收"; break;
                    default: stateString = $"未知状态[{state}]"; break;
                }
                if (state > 1)
                {
                    stateString = $"注入成功或已注入,接收指令的端口[{state}]";
                    commandPort = state;
                }

                Log.Add($"{nameof(tPlainModLoaderInjector)}:{stateString}");
                Console.WriteLine(stateString);
            }
            catch (Exception ex)
            {
                Log.Add($"{nameof(tPlainModLoaderInjector)}:注入失败:{ex}");

                Console.WriteLine($"注入失败:");
                Console.WriteLine($"{ex}");
                Console.ReadKey(true);
                return;
            }

            CommandTCP.Initialize(commandPort);
            CommandTCP.Run();

            #region !
            //Console.WriteLine("ok");
            //Console.ReadLine();
            #endregion
        }

        private static void Initialize_Config()
        {
            launchConfig = new ConfigHelp<LauncherConfig>(Path.Combine(ProgramPath, "launchConfig.json"));
            launchConfig.UpdateConfig(() => new LauncherConfig());

            if (InjectorProgramName == null) throw new Exception("注入程序名列表为[null]");
            if (InjectorProgramName.Length < 1) throw new Exception($"注入程序名列表数量为[{InjectorProgramName.Length}]");

            string s = null;
            foreach (string i in InjectorProgramName)
            {
                if (s == null) s = $"[{i}]";
                else s = $"{s},[{i}]";
            }
            s = $"注入程序名列表:{s}";

            Log.Add($"{nameof(tPlainModLoaderInjector)}:{s}");
            Console.WriteLine(s);
        }

        private static void Initialize_Inject()
        {
            InjectDllFilePath = Path.Combine(ProgramPath, "tPlainModLoaderInjector.exe");
        }
    }
}
