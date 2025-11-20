using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
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
                Log.Add($"{nameof(tPlainModLoaderInjector)}");
                Log.Add($"{nameof(Program)}:初始化");
                Console.WriteLine($"初始化");

                Initialize_Config();
                Initialize_Inject();

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

            Console.Title = "tPlainModLoaderInjector";
            int pid = -1;

            try
            {
                Log.Add($"{nameof(Program)}:选择目标程序pid");
                Console.WriteLine("选择目标程序pid");
                pid = SwitchPID();
            }
            catch (Exception ex)
            {
                Log.Add($"{nameof(Program)}:选择失败:{ex}");
                Log.SaveTry();

                Console.WriteLine($"选择失败:");
                Console.WriteLine($"{ex}");
                Console.ReadKey(true);
                return;
            }

            try
            {
                Log.Add($"{nameof(Program)}:尝试注入:{pid}");
                Console.WriteLine($"尝试注入:{pid}");

                int state = InjectorGame.Injector(pid, InjectDllFilePath);
                string stateString = null;
                switch (state)
                {
                    case 0: stateString = "注入失败"; break;
                    case 1: stateString = "注入成功"; break;
                    case 2: stateString = "已注入"; break;
                    case -1: stateString = "附加到进程失败"; break;
                    case -2: stateString = "初始化内容失败"; break;
                    default: stateString = $"未知状态[{state}]"; break;
                }

                Log.Add($"{nameof(Program)}:{stateString}");
                Console.WriteLine(stateString);
            }
            catch (Exception ex)
            {
                Log.Add($"{nameof(Program)}:注入失败:{ex}");
                Log.SaveTry();

                Console.WriteLine($"注入失败:");
                Console.WriteLine($"{ex}");
                Console.ReadKey(true);
                return;
            }

            CommandPipe.Initialize();
            CommandPipe.Run();

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

            Log.Add($"{nameof(Program)}:{s}");
            Console.WriteLine(s);
        }

        private static void Initialize_Inject()
        {
            InjectDllFilePath = Path.Combine(ProgramPath, "tPlainModLoaderInjector.exe");
        }

        private static int SwitchPID()
        {
            Console.WriteLine("查找符合条件的程序");
            System.Collections.Generic.List<Process> ids = ProcessUtils.GetProcessPID(InjectorProgramName);
            if (ids == null || ids.Count < 1) throw new Exception("找不到符合条件的程序");

            int index = 0;

            Func<int, string> get = i => $"{i}:名称[{ids[i].ProcessName}],pid[{ids[i].Id}]";
            while (true)
            {
                Console.Clear();
                Console.WriteLine("输入要附加到第几个,什么都不输入以确认");
                Console.WriteLine($"当前选择: {get(index)}");

                for (int i = 0; i < ids.Count; ++i)
                {
                    Console.WriteLine(get(i));
                }

                string s = Console.ReadLine();
                if (s == null) continue;
                if (s == string.Empty) break;

                int.TryParse(s, out index);
                if (index < 0) index = 0;
                else if (index >= ids.Count) index = ids.Count - 1;
            }

            Console.Clear();

            return ids[index].Id;
        }
    }
}
