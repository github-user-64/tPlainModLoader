using tContentPatch;
using tContentPatch.ModLoad;

namespace OptimizeAndTool
{
    internal class ThisMod : Mod
    {
        public static ModObject mo { get; private set; } = null;

        public override void Load(ModObject mo)
        {
            ThisMod.mo = mo;
        }
    }
}
