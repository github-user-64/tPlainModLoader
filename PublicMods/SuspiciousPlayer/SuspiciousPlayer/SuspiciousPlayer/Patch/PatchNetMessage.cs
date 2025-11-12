using System;
using tContentPatch;
using tContentPatch.Patch;

namespace SuspiciousPlayer.Patch
{
    internal class PatchNetMessage : Mod
    {
        public static Action<int> OnSyncConnectedPlayer = null;

        public override void AddPatch(IAddPatch addPatch)
        {
            addPatch.AddPostfix(typeof(Terraria.NetMessage).GetMethod("SyncConnectedPlayer"),
                typeof(PatchNetMessage).GetMethod("SyncConnectedPlayer"));
        }

        public static void SyncConnectedPlayer(int plr)
        {
            OnSyncConnectedPlayer?.Invoke(plr);
        }
    }
}
