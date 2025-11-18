using tContentPatch;
using tContentPatch.Patch;
using Terraria;
using Terraria.DataStructures;

namespace test1
{
    internal class PatchNPC : Mod
    {
        public override void AddPatch(IAddPatch addPatch)
        {
            addPatch.AddPrefix(typeof(NPC).GetMethod("NewNPC"),
                typeof(PatchNPC).GetMethod("NewNPC"));
        }

        public static bool NewNPC(IEntitySource source, int X, int Y, int Type, int Start = 0, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f, float ai3 = 0f, int Target = 255)
        {
            return true;
        }
    }
}
