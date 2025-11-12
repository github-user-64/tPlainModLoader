using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using tContentPatch.Utils;

namespace tContentPatch.ModLoad
{
    internal partial class LoadConfig : IModLoader
    {
        public readonly string ConfigDirectory = null;

        private bool isCancel = false;
        private LoadState state = LoadState.None;

        private string stateText = string.Empty;
        private int progressV = 0;
        private int progressMax = 0;


        public LoadConfig(string configDirectory) { ConfigDirectory = configDirectory; }


        private List<ModObject> LoadModConfigList(string modsRootPath)
        {
            if (Directory.Exists(modsRootPath) == false) return new List<ModObject>();

            DirectoryInfo[] dis = new DirectoryInfo(modsRootPath).GetDirectories();

            progressV = 0;
            progressMax = dis.Length;
            stateText = "加载模组配置";

            List<ModObject> mos = new List<ModObject>();

            foreach (DirectoryInfo di in dis)
            {
                CheckLoadCancel();

                bool addProgress = true;

                try
                {
                    if (di == null) continue;

                    stateText = $"加载模组配置:{di.Name}";

                    string filePath = Path.Combine(di.FullName, InfoList.Files.ModLoadConfig);

                    ModConfig config = LoadModConfig(filePath);

                    if (config == null) continue;
                    if (config.key == null) continue;
                    //未启用的也存着

                    mos.Add(new ModObject(config) { modPath = di.FullName });
                }
                catch
                {
                    addProgress = false;
                }
                finally
                {
                    if (addProgress) ++progressV;
                }
            }

            return mos;
        }

        private void LoadModInfo(List<ModObject> mos)
        {
            foreach (ModObject mo in mos)
            {
                try
                {
                    string filePath = Path.Combine(mo.modPath, InfoList.Files.ModInfo);
                    if (File.Exists(filePath) == false) continue;
                    ModInfo mi = MyJson1.Get2<ModInfo>(filePath);
                    mo.info = mi;
                }
                catch { }
            }
        }

        private ModConfig LoadModConfig(string filePath)
        {
            ConfigHelp<ModConfig> config = new ConfigHelp<ModConfig>(filePath);
            config.UpdateConfig();

            return config.config;
        }

        private void CheckLoadCancel()
        {
            if (isCancel) throw new TaskCanceledException();
        }
    }
}
