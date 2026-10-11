using Terraria;
using Terraria.Localization;

namespace tContentPatch
{
    /// <summary/>
    public abstract class PatchNetMessage
    {
        /// <summary>
        /// <see cref="ModSetting.Load(object)"/>后调用
        /// </summary>
        public virtual void Initialize() { }
        /// <summary>
        /// <see cref="NetMessage.SendData(int, int, int, NetworkText, int, float, float, float, int, int, int)"/>前调用
        /// </summary>
        public virtual void SendDataPrefix(int msgType, int remoteClient, int ignoreClient, NetworkText text,
            int number, float number2, float number3, float number4, int number5, int number6, int number7)
        { }
        /// <summary>
        /// <see cref="NetMessage.SendData(int, int, int, NetworkText, int, float, float, float, int, int, int)"/>后调用
        /// </summary>
        public virtual void SendDataPostfix(int msgType, int remoteClient, int ignoreClient, NetworkText text,
            int number, float number2, float number3, float number4, int number5, int number6, int number7)
        { }
        /// <summary>
        /// 服务端在同步已连接玩家前
        /// </summary>
        public virtual void SyncConnectedPlayerPrefix(int plr) { }
        /// <summary>
        /// 服务端在同步已连接玩家后
        /// </summary>
        public virtual void SyncConnectedPlayerPostfix(int plr) { }
        /// <summary>
        /// 服务端在同步断开连接玩家前
        /// <para/>可能会失效, 建议在<see cref="SyncOnePlayerPrefix(int, int, int)"/>中判断玩家的<see cref="Player.active"/>
        /// </summary>
        public virtual void SyncDisconnectedPlayerPrefix(int plr) { }
        /// <summary>
        /// 服务端在同步断开连接玩家后
        /// <para/>可能会失效, 建议在<see cref="SyncOnePlayerPostfix(int, int, int)"/>中判断玩家的<see cref="Player.active"/>
        /// </summary>
        public virtual void SyncDisconnectedPlayerPostfix(int plr) { }
        /// <summary>
        /// 服务端同步玩家前
        /// <para/>如果是同步离线玩家, 那么在此之前玩家的信息会被清除
        /// <para/>如需获取离线玩家的信息请使用<see cref="PatchRemoteClient.ResetPrefix(RemoteClient)"/>
        /// </summary>
        public virtual void SyncOnePlayerPrefix(int plr, int toWho, int fromWho) { }
        /// <summary>
        /// 服务端同步玩家后
        /// </summary>
        public virtual void SyncOnePlayerPostfix(int plr, int toWho, int fromWho) { }
    }
}
