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
    internal class Patch_TileLightScanner : ClassPatch<PatchTileLightScanner>
    {
        private static List<PatchTileLightScanner> mod = new List<PatchTileLightScanner>();

        public Patch_TileLightScanner() : base(mod) { }

        public override void Initialize(IAddPatch addPatch)
        {
            Type oType = typeof(TileLightScanner);
            Type tType = typeof(Patch_TileLightScanner);
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
                foreach (PatchTileLightScanner item in mod) item.ApplyTileLightPrefix(tile, x, y, ref localRandom, ref lightColor);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }
    }
}
