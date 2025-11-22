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
            try
            {
                foreach (PatchTileLightScanner item in mod) item.ApplyTileLightPrefix(tile, x, y, ref localRandom, ref lightColor);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }
    }
}
