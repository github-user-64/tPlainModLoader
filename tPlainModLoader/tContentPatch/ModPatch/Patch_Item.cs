using System;
using System.Collections.Generic;
using System.Reflection;
using tContentPatch.Patch;
using tContentPatch.Utils;
using Terraria;

namespace tContentPatch.ModPatch
{
    internal class Patch_Item : ClassPatch<PatchItem>
    {
        private static List<PatchItem> mod = new List<PatchItem>();

        public Patch_Item() : base(mod) { }

        public override void Initialize(IAddPatch addPatch)
        {
            Type oType = typeof(Item);
            Type tType = typeof(Patch_Item);
            Action<string, string, BindingFlags> addPre = (m, m2, bf) =>
            {
                addPatch.AddPrefix(oType.GetMethod(m, bf), tType.GetMethod(m2));
            };
            Action<string, string, BindingFlags> addPo = (m, m2, bf) =>
            {
                addPatch.AddPostfix(oType.GetMethod(m, bf), tType.GetMethod(m2));
            };

            addPre("UpdateItem", nameof(UpdateItemPrefix), BindingFlags.Public | BindingFlags.Instance);
            addPo("UpdateItem", nameof(UpdateItemPostfix), BindingFlags.Public | BindingFlags.Instance);
        }

        public static void UpdateItemPrefix(Item __instance, int i)
        {
            try
            {
                foreach (PatchItem item in mod) item.UpdateItemPrefix(__instance, i);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        public static void UpdateItemPostfix(Item __instance, int i)
        {
            try
            {
                foreach (PatchItem item in mod) item.UpdateItemPostfix(__instance, i);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }
    }
}
