using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace tContentPatch.ModLoad
{
    internal partial class LoadAssembly
    {
        private readonly IModLoader loadConfig = null;

        private bool isCancel = false;
        private LoadState state = LoadState.None;

        private string stateText = string.Empty;
        private int progressV = 0;
        private int progressMax = 0;


        public LoadAssembly(IModLoader loadConfig)
        {
            if (loadConfig == null) throw new ArgumentNullException(nameof(loadConfig));
            this.loadConfig = loadConfig;
        }

        private void CheckFrontMod(List<ModObject> mos)
        {
            progressV = 0;
            progressMax = mos.Count;
            stateText = "检查前置模组";

            foreach (ModObject mo in mos)
            {
                CheckLoadCancel();

                stateText = $"检查前置模组:{mo.info?.name ?? mo.config.key}";

                Console.WriteLine($"已加载模组配置:{mo.info?.name ?? mo.config.key}");

                if (mo.config.frontModKeys == null) continue;

                foreach (string key in mo.config.frontModKeys)
                {
                    if (mos.Exists(i => i.config.key == key)) continue;

                    throw new Exception($"模组[{mo.info?.name ?? mo.config.key}]的前置未加载:[{key}]");
                }

                ++progressV;
            }
        }

        private void LoadModAssemblyList(List<ModObject> mos)
        {
            progressV = 0;
            progressMax = mos.Count;
            stateText = "加载程序集";

            foreach (ModObject mo in mos)
            {
                CheckLoadCancel();

                string dllPath = mo.config.dllPath;
                if (dllPath == null) continue;
                string filePath = Path.Combine(mo.modPath, dllPath);

                if (File.Exists(filePath) == false) throw new Exception($"dll文件缺失:[{filePath}]");
                if (mo.config.dllPath == null) mo.config.dllPath = dllPath;

                stateText = $"加载程序集:{mo.info?.name ?? mo.config.key}";

                //Assembly.LoadFile加载的程序集如果是同一个文件的话, 在当前应用程序域里就不会有重复的程序集
                //但在模组加载器为主项目的情况下, 重复加载模组会出现模组的state变量没变, 还是重新加载模组之前的值
                //应该是Assembly.LoadFile加载的文件没变化时就直接返回已有的程序集
                //而用模组为主项目的情况下没出现这情况, 因该是运行时模组文件重新生成, 所以Assembly.LoadFile就会重新加载
                //所以使用Assembly.Load(File.ReadAllBytes())加载, 防止出现这情况
                //注意, 使用Assembly.Load(File.ReadAllBytes())加载会导致加载的程序集获取不到Location

                //mo.assembly = Assembly.LoadFile(filePath);
                mo.assembly = Assembly.Load(File.ReadAllBytes(filePath));

                ++progressV;
                Console.WriteLine($"已加载程序集:{filePath}");
            }
        }

        private void CheckLoadCancel()
        {
            if (isCancel) throw new TaskCanceledException();
        }
    }
}
