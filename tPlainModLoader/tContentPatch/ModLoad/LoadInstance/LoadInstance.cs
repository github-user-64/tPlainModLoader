using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace tContentPatch.ModLoad
{
    internal partial class LoadInstance
    {
        private readonly IModLoader loadAssembly = null;

        private bool isCancel = false;
        private LoadState state = LoadState.None;

        private string stateText = string.Empty;
        private int progressV = 0;
        private int progressMax = 0;

        public LoadInstance(IModLoader loadAssembly)
        {
            if (loadAssembly == null) throw new ArgumentNullException(nameof(loadAssembly));
            this.loadAssembly = loadAssembly;
        }

        private void CreateModInstance(List<ModObject> mos)
        {
            progressV = 0;
            progressMax = mos.Count;
            stateText = "创建模组实例";

            foreach (ModObject mo in mos)
            {
                try
                {
                    if (mo.assembly == null) continue;

                    stateText = $"创建模组实例:{mo.info?.name ?? mo.config.key}";

                    CheckLoadCancel();
                    mo.inheritance_mod = Utils.CreateInstance<Mod>(mo.assembly);

                    CheckLoadCancel();
                    mo.inheritance_setting = Utils.CreateInstance<ModSetting>(mo.assembly);

                    CheckLoadCancel();
                    mo.inheritance_modMain = Utils.CreateInstance<ModMain>(mo.assembly);

                    CheckLoadCancel();
                    mo.inheritance_modPlayer = Utils.CreateInstance<ModPlayer>(mo.assembly);

                    CheckLoadCancel();
                    mo.inheritance_modNPC = Utils.CreateInstance<ModNPC>(mo.assembly);

                    CheckLoadCancel();
                    mo.inheritance_modItem = Utils.CreateInstance<ModItem>(mo.assembly);

                    CheckLoadCancel();
                    mo.inheritance_modProjectile = Utils.CreateInstance<ModProjectile>(mo.assembly);

                    CheckLoadCancel();
                    mo.inheritance_tileLightScanner = Utils.CreateInstance<ModTileLightScanner>(mo.assembly);

                    CheckLoadCancel();
                    mo.inheritance_remadeChatMonitor = Utils.CreateInstance<ModRemadeChatMonitor>(mo.assembly);
                }
                finally
                {
                    ++progressV;
                }
            }
        }

        private void CheckLoadCancel()
        {
            if (isCancel) throw new TaskCanceledException();
        }
    }
}
