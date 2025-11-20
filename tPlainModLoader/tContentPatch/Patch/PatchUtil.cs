using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace tContentPatch.Patch
{
    internal static class PatchUtil
    {
        private static List<Harmony> harmonyList = new List<Harmony>();

        internal static void AddPatch(string patchId, MethodBase original, MethodInfo method, HarmonyPatchType harmonyPatchType)
        {
            if (patchId == null) throw new ArgumentNullException(nameof(patchId));
            if (original == null) throw new ArgumentNullException(nameof(original));
            if (method == null) throw new ArgumentNullException(nameof(method));

            Harmony harmony = harmonyList.Find(i => i.Id == patchId);
            if (harmony == null)
            {
                harmony = new Harmony(patchId);
                harmonyList.Add(harmony);
            }

            switch (harmonyPatchType)
            {
                case HarmonyPatchType.Prefix: harmony.Patch(original, prefix: method); break;
                case HarmonyPatchType.Postfix: harmony.Patch(original, postfix: method); break;
                default: break;
            }
        }

        internal static void AllPatch(string patchId)
        {
            if (patchId == null) throw new ArgumentNullException(nameof(patchId));

            Harmony harmony = harmonyList.Find(i => i.Id == patchId);
            if (harmony == null)
            {
                harmony = new Harmony(patchId);
                harmonyList.Add(harmony);
            }

            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        internal static void AddPatchPrefix(string patchId, MethodBase original, MethodInfo prefix)
        {
            AddPatch(patchId, original, prefix, HarmonyPatchType.Prefix);
        }

        internal static void AddPatchPostfix(string patchId, MethodBase original, MethodInfo postfix)
        {
            AddPatch(patchId, original, postfix, HarmonyPatchType.Postfix);
        }

        internal static void ClearPathc(string patchId)
        {
            Harmony harmony = harmonyList.FirstOrDefault(i => i.Id == patchId);
            if (harmony == null) return;

            harmony.UnpatchAll(patchId);

            harmonyList.Remove(harmony);
        }

        internal static Harmony GetHarmony(string patchId)
        {
            return harmonyList.FirstOrDefault(i => i.Id == patchId);
        }
    }
}
