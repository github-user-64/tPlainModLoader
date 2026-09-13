using Terraria;

namespace tContentPatch
{
    /// <summary/>
    public abstract class PatchRemoteClient
    {
        /// <summary>
        /// <see cref="ModSetting.Load(object)"/>后调用
        /// </summary>
        public virtual void Initialize() { }
        /// <summary>
        /// 重置连接前
        /// <para/>获取离线玩家信息: if (This.IsActive) Main.player[This.Id].name
        /// </summary>
        public virtual void ResetPrefix(RemoteClient This) { }
    }
}
