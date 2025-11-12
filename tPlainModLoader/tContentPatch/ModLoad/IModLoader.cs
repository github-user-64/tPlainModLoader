using System.Collections.Generic;

namespace tContentPatch.ModLoad
{
    internal interface IModLoader : IModLoaderState
    {
        bool IsLoading();
        List<ModObject> Load();
        void Unload();
        void CancelLoad();
    }

    internal interface IModLoaderState
    {
        void ProgressBar(out int val, out int max);
        string GetTip();
    }

    internal enum LoadState
    {
        None,
        Loading,
        Loaded,
        Canceling,
        Unloading,
    }
}
