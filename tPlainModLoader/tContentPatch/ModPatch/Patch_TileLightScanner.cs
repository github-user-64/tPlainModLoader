using HarmonyLib;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using tContentPatch.Utils;
using Terraria;
using Terraria.Graphics.Light;
using Terraria.Utilities;

namespace tContentPatch.ModPatch
{
    [HarmonyPatch(typeof(TileLightScanner))]
    internal class Patch_TileLightScanner : ListCopy<PatchTileLightScanner>
    {
        private static List<PatchTileLightScanner> mod = new List<PatchTileLightScanner>();

        public Patch_TileLightScanner() : base(mod) { }

        [HarmonyPatch("ApplyTileLight")]
        [HarmonyPrefix]
        public static void ApplyTileLightPrefix(Tile tile, int x, int y, ref FastRandom localRandom, ref Vector3 lightColor)
        {
            foreach (PatchTileLightScanner item in mod)
            {
                try
                {
                    item.ApplyTileLightPrefix(tile, x, y, ref localRandom, ref lightColor);
                }
                catch (Exception ex)
                {
                    OutputDebug.OutputException(ex);
                }
            }
        }

        [HarmonyPatch(nameof(TileLightScanner.GetTileLight))]
        [HarmonyPostfix]
        private static void GetTileLightPostfix(int x, int y, ref Vector3 outputColor)
        {
            foreach (PatchTileLightScanner item in mod)
            {
                try
                {
                    item.GetTileLightPostfix(x, y, ref outputColor);
                }
                catch (Exception ex)
                {
                    OutputDebug.OutputException(ex);
                }
            }
        }
    }
}
