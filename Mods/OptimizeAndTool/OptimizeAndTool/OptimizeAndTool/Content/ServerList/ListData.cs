using System;
using System.Collections.Generic;
using tContentPatch;

namespace OptimizeAndTool.Content.ServerList
{
    public class ListData : ModSetting, ListData.IControl
    {
        public interface IControl
        {
            List<ServerInfo> Data { get; set; }
            void Save();
            void Load();
        }

        public override bool HasUI => false;
        public override string FilePath => "ServerList.json";
        public override Type DataType => typeof(List<ServerInfo>);

        /// <summary>
        /// 不为<see langword="null"/>
        /// </summary>
        public List<ServerInfo> Data { get; set; } = new List<ServerInfo>();

        public void Load() => Load(Read());
        public override void Save()
        {
            NeedSave = true;
            base.Save();
        }

        public override void Load(object v)
        {
            if (v is List<ServerInfo> data) Data = data ?? new List<ServerInfo>();

            ServerList.SetControl(this);
        }

        public override object GetSaveData() => Data;

    }
}
