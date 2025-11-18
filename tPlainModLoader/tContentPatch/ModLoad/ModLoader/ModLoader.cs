using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tContentPatch.Patch;

namespace tContentPatch.ModLoad
{
    internal partial class ModLoader
    {
        private readonly IModLoader loadInstance = null;
        private readonly string patchId = null;
        private readonly IAddPatch modPatch = null;
        private List<ModObject> modObjects = null;

        private bool isCancel = false;
        private LoadState state = LoadState.None;

        private string stateText = string.Empty;
        private int progressV = 0;
        private int progressMax = 0;

        public ModLoader(IModLoader loadInstance, string patchId)
        {
            if (loadInstance == null) throw new ArgumentNullException(nameof(loadInstance));
            if (patchId == null) throw new ArgumentNullException(nameof(patchId));
            this.loadInstance = loadInstance;
            this.patchId = patchId;

            modPatch = new AddPatch(this.patchId);
        }

        private void UnloadMod(List<ModObject> mods)
        {
            progressV = 0;
            progressMax = (mods?.Count ?? 0) + 1;
            stateText = "卸载";

            try
            {
                if (mods != null)
                {
                    foreach (ModObject mo in mods)
                    {
                        stateText = $"卸载模组:{mo.info?.name ?? mo.config.key}";

                        Utils.ForHelp(mo.inheritance_mod, item => item.Unload(),
                            ex => $"卸载模组[{mo.info?.name ?? mo.config.key}]时失败:{ex.Message}");

                        ++progressV;
                    }
                }
            }
            finally
            {
                stateText = "清理";

                ContentPatch.listPatch.ClearAllPatch();
                PatchUtil.ClearPathc(patchId);

                ++progressV;
            }
        }

        private void CheckLoadCancel()
        {
            if (isCancel) throw new TaskCanceledException();
        }

        private void LoadModSet(ModObject mo, ModSetting ms)
        {
            ms.Load(ms.Read());
        }
    }
}
