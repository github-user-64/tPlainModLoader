using System.IO;
using tContentPatch.Content.Network;
using Terraria.Net;

namespace tContentPatch
{
    /// <summary/>
    public abstract class ModNetPacket
    {
        internal readonly string key = null;

        /// <summary/>
        public ModNetPacket()
        {
            key = GetType().FullName;
        }

        /// <summary>
        /// <see cref="ModSetting.Load(object)"/>后调用
        /// </summary>
        public virtual void Initialize() { }
        /// <summary>
        /// 收到消息时
        /// </summary>
        public abstract void Deserialize(BinaryReader reader, int userId);
        /// <summary>
        /// 收到通知时, 在服务端调用, 说明该玩家用了这个加载器的客户端
        /// </summary>
        public virtual void OnGetNotice(int userId) { }

        /// <summary>
        /// 获取包, 失败返回<see langword="null"/>. 在返回的包里写数据
        /// <para/>发送包使用<see cref="NetManager.Instance"/>里的方法
        /// </summary>
        public NetPacket? GetNetPacket()
        {
            if (RegisterNetModule.Loaded == false) return null;
            NetPacket packet = NetTPMLModule.CreateModPacket(key);

            return packet;
        }
    }
}
