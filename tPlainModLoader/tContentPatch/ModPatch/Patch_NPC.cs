using System;
using System.Collections.Generic;
using System.Reflection;
using tContentPatch.Patch;
using tContentPatch.Utils;
using Terraria;

namespace tContentPatch.ModPatch
{
    internal class Patch_NPC : ClassPatch<PatchNPC>
    {
        private static List<PatchNPC> mod = new List<PatchNPC>();

        public Patch_NPC() : base(mod) { }

        public override void Initialize(IAddPatch addPatch)
        {
            Type oType = typeof(NPC);
            Type tType = typeof(Patch_NPC);
            Action<string, string, BindingFlags> addPre = (m, m2, bf) =>
            {
                addPatch.AddPrefix(oType.GetMethod(m, bf), tType.GetMethod(m2));
            };
            Action<string, string, BindingFlags> addPo = (m, m2, bf) =>
            {
                addPatch.AddPostfix(oType.GetMethod(m, bf), tType.GetMethod(m2));
            };

            addPre("UpdateNPC", nameof(UpdateNPCPrefix), BindingFlags.Public | BindingFlags.Instance);
            addPo("UpdateNPC", nameof(UpdateNPCPostfix), BindingFlags.Public | BindingFlags.Instance);
        }

        public static void UpdateNPCPrefix(NPC __instance, int i)
        {
            try
            {
                foreach (PatchNPC item in mod) item.UpdateNPCPrefix(__instance, i);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        public static void UpdateNPCPostfix(NPC __instance, int i)
        {
            try
            {
                foreach (PatchNPC item in mod) item.UpdateNPCPostfix(__instance, i);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }
    }
}
