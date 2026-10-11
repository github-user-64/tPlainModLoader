using Terraria;

namespace tContentPatch
{
    /// <summary/>
    public abstract class PatchChest
    {
        /// <summary>
        /// <see cref="ModSetting.Load(object)"/>后调用
        /// </summary>
        public virtual void Initialize() { }
        /// <summary>
        /// 设置商店时, <paramref name="type"/>是商店类型
        /// </summary>
        public virtual void SetupShop(Chest This, int type) { }
    }
}
