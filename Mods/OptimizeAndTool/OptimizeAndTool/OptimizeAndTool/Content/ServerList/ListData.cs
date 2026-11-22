using OptimizeAndTool.Utils;
using System;
using System.Collections.Generic;
using tContentPatch;
using tContentPatch.Utils;

namespace OptimizeAndTool.Content.ServerList
{
    public class ListData : ModSetting, ListData.IControl
    {
        #region
        public override bool HasUI => false;
        public override string FilePath => "ServerList.json";
        public override Type DataType => typeof(List<ServerInfo>);

        public override object Read()
        {
            object v = null;

            ModFile.ReadFileTry(FilePath, file =>
            {
                v = ServerInfo.Read<List<ServerInfo>>(file);
                return true;
            });

            return v;
        }

        public override void Load(object v)
        {
            List<ServerInfo> data = v as List<ServerInfo>;

            if (data != null) data.RemoveAll(i => i == null);
            else data = new List<ServerInfo>();

            Data = data;

            ServerList.SetControl(this);
        }
        #endregion

        #region
        public interface IControl
        {
            List<ServerInfo> Data { get; }
            void Save();
            void Load();
            ServerInfo GetDefault();

            /// <summary>
            /// 将<paramref name="data"/>添加到<paramref name="index"/>处<br/>
            /// <paramref name="index"/>会被限制在范围内<br/>
            /// 添加成功返回插入的位置, 否则返回<see langword="-1"/>
            /// </summary>
            int Add(ServerInfo data, int index);
            void Del(ServerInfo data);
            void OffUp(ServerInfo data);
            void OffDown(ServerInfo data);
        }

        /// <summary>
        /// 不为<see langword="null"/>
        /// </summary>
        public virtual List<ServerInfo> Data { get; protected set; } = new List<ServerInfo>();

        public virtual void Load() => Load(Read());

        public override void Save()
        {
            ModFile.SaveFileTry(FilePath, file =>
            {
                ServerInfo.Save(file, Data);
                return true;
            });
        }

        public virtual ServerInfo GetDefault() => ServerInfo.GetEmpty();
        public virtual int Add(ServerInfo data, int index)
        {
            if (data == null) return -1;

            return Data.Add(data, index);
        }
        public virtual void Del(ServerInfo data) => Data.Remove(data);
        public virtual void OffUp(ServerInfo data) => Data.OffUp(data);
        public virtual void OffDown(ServerInfo data) => Data.OffDown(data);
        #endregion
    }
}
