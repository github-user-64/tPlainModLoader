using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using tContentPatch;
using tContentPatch.Utils;

namespace tPlainModLoader
{
    internal class LaunchGame
    {
        internal static Assembly GameAssembly { get; private set; } = null;

        internal static void Initialize(LauncherConfig launchConfig)
        {
            Initialize_CurrentDirectory(launchConfig);
            Initialize_AssemblyResolveEvent();
            Initialize_Microsoft_Xna_Framework_TitleLocation();
        }

        private static void Initialize_CurrentDirectory(LauncherConfig launchConfig)
        {
            string file = launchConfig?.LauncherFilePath;
            string dir = Path.GetDirectoryName(file);

            if (file != null)
            {
                if (File.Exists(file) == false) throw new Exception($"启动文件不存在[{file}]");
            }
            else
            {
                file = null;
                dir = Path.GetFullPath("../Terraria");

                if (Directory.Exists(dir) == false) throw new Exception($"目录不存在[{dir}]");

                string[] fileNames = { "Terraria.exe", "Terraria_v1.4.4.9.exe" };
                foreach (string i in fileNames)
                {
                    string s = Path.Combine(dir, i);
                    if (File.Exists(s) == false) continue;
                    file = s;
                }

                if (file == null) throw new Exception($"在目录中找不到启动文件[{dir}]");

                if (launchConfig != null) launchConfig.LauncherFilePath = file;
            }

            Directory.SetCurrentDirectory(dir);

            Console.WriteLine($"启动文件路径[{file}]");
            Console.WriteLine($"工作目录[{Directory.GetCurrentDirectory()}]");
        }

        private static void Initialize_AssemblyResolveEvent()
        {
            //处理服务端名称不同问题
            AppDomain.CurrentDomain.AssemblyResolve += (s, e) =>
            {
                if (new AssemblyName(e.Name).Name == "Terraria")
                    return GameAssembly;
                else return null;
            };

            //从工作目录找匹配的dll
            AppDomain.CurrentDomain.AssemblyResolve += (s, e) =>
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), new AssemblyName(e.Name).Name + ".dll");

                if (File.Exists(filePath) == false) return null;

                return Assembly.LoadFile(filePath);
            };
        }

        private static void Initialize_Microsoft_Xna_Framework_TitleLocation()
        {
            string assemblieName = typeof(Vector2).Assembly.GetName().Name;

            if (AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(i => i.GetName().Name == assemblieName) == null)
            {
                Assembly.Load("Microsoft.Xna.Framework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553");
            }

            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().First(i => i.GetName().Name == assemblieName);
            Type type = assembly.GetType("Microsoft.Xna.Framework.TitleLocation");
            type.GetField("_titleLocation", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, Directory.GetCurrentDirectory());
        }

        public static void Run(string launchFilePath, Action runExit)
        {
            Assembly.LoadFile(launchFilePath);

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            GameAssembly = assemblies.First(i => i.GetName().Name == "Terraria" || i.GetName().Name == "TerrariaServer");

            Type launchType = GameAssembly.GetType("Terraria.WindowsLaunch");
            MethodInfo launchMethodInfo = launchType.GetMethod("Main", BindingFlags.Static | BindingFlags.NonPublic);

            Task task = Task.Run(() =>
            {
                try
                {
                    launchMethodInfo.Invoke(null, new object[] { new string[0] });

                    runExit?.Invoke();
                }
                catch (Exception ex)
                {
                    Log.Add($"{nameof(LaunchGame)}:目标程序运行异常:{ex}");
                    Console.WriteLine("目标程序运行异常:");
                    Console.WriteLine($"{ex}");
                    runExit?.Invoke();
                }
            });
        }
    }
}
