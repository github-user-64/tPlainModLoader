using tContentPatch;
using tContentPatch.ModLoad;

namespace OptimizeAndTool
{
    internal class ThisMod : Mod
    {
        public static ModObject mo { get; private set; } = null;
        public readonly static string ModName = "优化和工具";

        public override void Load(ModObject mo)
        {
            ThisMod.mo = mo;
        }
    }
}
