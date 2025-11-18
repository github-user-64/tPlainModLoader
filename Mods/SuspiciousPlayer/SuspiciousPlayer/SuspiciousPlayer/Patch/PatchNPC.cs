using SuspiciousPlayer.Content.Event1;
using tContentPatch;
using tContentPatch.Patch;
using Terraria;
using Terraria.DataStructures;

namespace SuspiciousPlayer.Patch
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
            if (Event.CanSpawnNPC == false)
            {
                if (Type == 415) return false;
                if (Type == 416) return false;
                if (Type == 417) return false;
                if (Type == 418) return false;
                if (Type == 419) return false;
                if (Type == 518) return false;
            }
            if (Event.CanSpawnNPC_SolarCrawltipede == false)
            {
                if (Type == 412) return false;
                //if (Type == 413) return false;
                //if (Type == 414) return false;
            }

            return true;
        }
    }
}
