using tContentPatch;
using tContentPatch.ModLoad;

namespace AccessoryBox
{
    internal class ThisMod : Mod
    {
        public static ModObject mo { get; protected set; } = null;

        public override void Load(ModObject mo)
        {
            ThisMod.mo = mo;
        }
    }
}
