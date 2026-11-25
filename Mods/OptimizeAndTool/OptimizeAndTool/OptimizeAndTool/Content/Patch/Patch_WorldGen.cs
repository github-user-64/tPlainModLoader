using System;
using tContentPatch;

namespace OptimizeAndTool.Content.Patch
{
    internal class Patch_WorldGen : PatchWorldGen
    {
        public static Action OnSaveAndQuitPrefix = null;
        public override void SaveAndQuitPrefix(Action callback = null)
        {
            OnSaveAndQuitPrefix?.Invoke();
        }
    }
}
