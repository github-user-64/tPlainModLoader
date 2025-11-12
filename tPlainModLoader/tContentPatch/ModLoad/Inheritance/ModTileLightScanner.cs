using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Utilities;

namespace tContentPatch
{
    public abstract class ModTileLightScanner
    {
        /// <summary>
        /// <see cref="Mod.Loaded"/>后调用
        /// </summary>
        public virtual void Initialize() { }
        public virtual void ApplyTileLightPrefix(Tile tile, int x, int y, ref FastRandom localRandom, ref Vector3 lightColor) { }
    }
}
