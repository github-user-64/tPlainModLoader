using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Utilities;

namespace tContentPatch
{
    /// <summary/>
    public abstract class PatchTileLightScanner
    {
        /// <summary>
        /// <see cref="ModSetting.Load(object)"/>后调用
        /// </summary>
        public virtual void Initialize() { }
        /// <summary/>
        public virtual void ApplyTileLightPrefix(Tile tile, int x, int y, ref FastRandom localRandom, ref Vector3 lightColor) { }
    }
}
