using Terraria;
using Terraria.ID;

namespace tContentPatch
{
    /// <summary/>
    public abstract class PatchWorldGen
    {
        /// <summary>
        /// <see cref="ModSetting.Load(object)"/>后调用
        /// </summary>
        public virtual void Initialize() { }
        /// <summary>
        /// 能否转化为对应生物群系
        /// <para/><paramref name="conversionType"/>对应<see cref="BiomeConversionID"/>
        /// </summary>
        public virtual bool CanConvert(int i2, int j2, int conversionType, bool tiles, bool walls) => true;
        /// <summary>
        /// 在客户端不会调用, <see cref="Main.netMode"/>=1
        /// </summary>
        public virtual void UpdateWorldPrefix() { }
        /// <summary>
        /// 在客户端不会调用, <see cref="Main.netMode"/>=1
        /// </summary>
        public virtual void UpdateWorldPostfix() { }
    }
}
