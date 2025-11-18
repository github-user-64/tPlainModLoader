using System;
using System.Collections.Generic;
using System.Reflection;
using tContentPatch.Patch;
using tContentPatch.Utils;
using Terraria;

namespace tContentPatch.ModPatch
{
    internal class Patch_Projectile : ClassPatch<PatchProjectile>
    {
        private static List<PatchProjectile> mod = new List<PatchProjectile>();

        public Patch_Projectile() : base(mod) { }

        public override void Initialize(IAddPatch addPatch)
        {
            Type oType = typeof(Projectile);
            Type tType = typeof(Patch_Projectile);
            Action<string, string, BindingFlags> addPre = (m, m2, bf) =>
            {
                addPatch.AddPrefix(oType.GetMethod(m, bf), tType.GetMethod(m2));
            };
            Action<string, string, BindingFlags> addPo = (m, m2, bf) =>
            {
                addPatch.AddPostfix(oType.GetMethod(m, bf), tType.GetMethod(m2));
            };

            addPre("Update", nameof(UpdatePrefix), BindingFlags.Public | BindingFlags.Instance);
            addPo("Update", nameof(UpdatePostfix), BindingFlags.Public | BindingFlags.Instance);

            addPre("Kill", nameof(KillPrefix), BindingFlags.Public | BindingFlags.Instance);
        }

        public static void UpdatePrefix(Projectile __instance, int i)
        {
            try
            {
                foreach (PatchProjectile item in mod) item.UpdatePrefix(__instance, i);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        public static void UpdatePostfix(Projectile __instance, int i)
        {
            try
            {
                foreach (PatchProjectile item in mod) item.UpdatePostfix(__instance, i);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        public static void KillPrefix(Projectile __instance)
        {
            try
            {
                foreach (PatchProjectile item in mod) item.KillPrefix(__instance);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }
    }
}
