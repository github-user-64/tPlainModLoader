using HarmonyLib;
using System.Collections.Generic;
using Terraria;

namespace tContentPatch.ModPatch
{
    [HarmonyPatch(typeof(RemoteClient))]
    internal class Patch_RemoteClient : ListCopy<PatchRemoteClient>
    {
        private static List<PatchRemoteClient> mod = new List<PatchRemoteClient>();

        public Patch_RemoteClient() : base(mod) { }

        [HarmonyPatch("Reset")]
        [HarmonyPrefix]
        public static void ResetPrefix(RemoteClient __instance)
        {
            mod.ForTry(item => item.ResetPrefix(__instance));
        }
    }
}
