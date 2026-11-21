using OptimizeAndTool.Utils;
using System;
using System.Collections.Generic;
using tContentPatch;

namespace OptimizeAndTool.Content.ServerList
{
    public class ListData : ModSetting, ListData.IControl
    {
        #region
        public override bool HasUI => false;
        public override string FilePath => "ServerList.json";
        public override Type DataType => typeof(List<ServerInfo>);

        public override void Load(object v)
        {
            List<ServerInfo> data = v as List<ServerInfo>;

            if (data != null) data.RemoveAll(i => i == null);
            else data = new List<ServerInfo>();

            Data = data;

            ServerList.SetControl(this);
        }

        public override object GetSaveData() => Data;
        #endregion

        #region
        public interface IControl
        {
            List<ServerInfo> Data { get; set; }
            void Save();
            void Load();
            ServerInfo GetDefault();

            /// <summary>
            /// 将<paramref name="data"/>添加到<paramref name="index"/>处<br/>
            /// <paramref name="index"/>会被限制在范围内
            /// </summary>
            void Add(ServerInfo data, int index);
            void Del(ServerInfo data);
            void OffUp(ServerInfo data);
            void OffDown(ServerInfo data);
        }

        /// <summary>
        /// 不为<see langword="null"/>
        /// </summary>
        public virtual List<ServerInfo> Data { get; set; } = new List<ServerInfo>();

        public virtual void Load() => Load(Read());

        public override void Save()
        {
            NeedSave = true;
            base.Save();
        }

        public virtual ServerInfo GetDefault() => ServerInfo.GetEmpty();
        public virtual void Add(ServerInfo data, int index)
        {
            if (data != null) Data.Add(data, index);
        }
        public virtual void Del(ServerInfo data) => Data.Remove(data);
        public virtual void OffUp(ServerInfo data) => Data.OffUp(data);
        public virtual void OffDown(ServerInfo data) => Data.OffDown(data);
        #endregion
    }
}
