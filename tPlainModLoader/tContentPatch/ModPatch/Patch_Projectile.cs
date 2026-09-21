using HarmonyLib;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;

namespace tContentPatch.ModPatch
{
    [HarmonyPatch(typeof(Projectile))]
    internal class Patch_Projectile : ListCopy<PatchProjectile>
    {
        private static List<PatchProjectile> mod = new List<PatchProjectile>();

        public Patch_Projectile() : base(mod) { }

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        public static void UpdatePrefix(Projectile __instance, int i)
        {
            mod.ForTry(item => item.UpdatePrefix(__instance, i));
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void UpdatePostfix(Projectile __instance, int i)
        {
            mod.ForTry(item => item.UpdatePostfix(__instance, i));
        }

        [HarmonyPatch("Kill")]
        [HarmonyPrefix]
        public static void KillPrefix(Projectile __instance)
        {
            mod.ForTry(item => item.KillPrefix(__instance));
        }

        [HarmonyPatch("Kill")]
        [HarmonyPostfix]
        public static void KillPostfix(Projectile __instance)
        {
            mod.ForTry(item => item.KillPostfix(__instance));
        }

        [HarmonyPatch("SetDefaults")]
        [HarmonyPrefix]
        public static void SetDefaultsPrefix(Projectile __instance, int Type)
        {
            mod.ForTry(item => item.SetDefaultsPrefix(__instance, Type));
        }

        [HarmonyPatch("SetDefaults")]
        [HarmonyPostfix]
        public static void SetDefaultsPostfix(Projectile __instance, int Type)
        {
            mod.ForTry(item => item.SetDefaultsPostfix(__instance, Type));
        }

        [HarmonyPatch("NewProjectile", new Type[]
        {
            typeof(IEntitySource),
            typeof(float), typeof(float), typeof(float), typeof(float),
            typeof(int), typeof(int), typeof(float), typeof(int),
            typeof(float),typeof(float),typeof(float),
            typeof(NewProjectileModifier)
        })]
        [HarmonyPostfix]
        public static void NewProjectilePostfix(int __result,
            IEntitySource spawnSource,
            float X, float Y, float SpeedX, float SpeedY,
            int Type, int Damage, float KnockBack, int Owner,
            float ai0, float ai1, float ai2,
            NewProjectileModifier modifer)
        {
            mod.ForTry(item => item.NewProjectilePostfix(
                __result, spawnSource, X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1, ai2, modifer));
        }

        [HarmonyPatch("AI_203_GetLightningColor")]
        [HarmonyPostfix]
        public static void AI_203_GetLightningColor(Projectile __instance, ref Color __result)
        {
            Color color = __result;

            mod.ForTry(item =>
            {
                color = item.AI_203_GetLightningColor(__instance, color);
            });

            __result = color;
        }

        [HarmonyPatch("AI")]
        [HarmonyPostfix]
        public static void AIPostfix(Projectile __instance)
        {
            mod.ForTry(item => item.AIPostfix(__instance));
        }
    }
}
