using System;
using System.Collections.Generic;

namespace tContentPatch.ModLoad
{
    internal partial class LoadConfig : IModLoader
    {
        public List<ModObject> Load()
        {
            try
            {
                if (state == LoadState.Loading) throw new Exception("加载中不可再次加载");
                if (state == LoadState.Unloading) throw new Exception("卸载中不可加载");

                state = LoadState.Loading;

                progressV = 0;
                progressMax = 0;
                stateText = string.Empty;

                List<ModObject> mos = LoadModConfigList(ConfigDirectory);
                CheckLoadCancel();

                LoadModInfo(mos);
                CheckLoadCancel();

                stateText = "完成";
                return mos;
            }
            finally
            {
                state = LoadState.Loaded;
            }
        }

        public void Unload() { }

        public void CancelLoad()
        {
            if (state == LoadState.Unloading) return;

            isCancel = true;
            while (state == LoadState.Loading) ;

            isCancel = false;
            state = LoadState.None;
        }

        public string GetTip() => stateText ?? string.Empty;

        public bool IsLoading() => state == LoadState.Loading;

        public void ProgressBar(out int val, out int max)
        {
            val = progressV;
            max = progressMax;
        }

    }
}
