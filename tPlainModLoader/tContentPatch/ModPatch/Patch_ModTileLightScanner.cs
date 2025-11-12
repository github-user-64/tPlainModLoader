using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using tContentPatch.Patch;
using tContentPatch.Utils;
using Terraria;
using Terraria.Graphics.Light;
using Terraria.Utilities;

namespace tContentPatch.ModPatch
{
    internal class Patch_ModTileLightScanner
    {
        internal static List<ModTileLightScanner> mod = new List<ModTileLightScanner>();

        internal static void Initialize(IAddPatch addPatch)
        {
            Type oType = typeof(TileLightScanner);
            Type tType = typeof(Patch_ModTileLightScanner);
            Action<string, string, BindingFlags> addPre = (m, m2, bf) =>
            {
                addPatch.AddPrefix(oType.GetMethod(m, bf), tType.GetMethod(m2));
            };
            Action<string, string, BindingFlags> addPo = (m, m2, bf) =>
            {
                addPatch.AddPostfix(oType.GetMethod(m, bf), tType.GetMethod(m2));
            };

            addPre("ApplyTileLight", nameof(ApplyTileLightPrefix), BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static void ApplyTileLightPrefix(Tile tile, int x, int y, ref FastRandom localRandom, ref Vector3 lightColor)
        {
            try
            {
                foreach (ModTileLightScanner item in mod) item.ApplyTileLightPrefix(tile, x, y, ref localRandom, ref lightColor);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }
    }
}
