using System;
using System.Collections.Generic;
using tContentPatch;

namespace OptimizeAndTool.Content.ServerList
{
    internal class Setting : ModSetting
    {
        public override bool HasUI => false;
        public override string FilePath => "ServerList.json";
        public override Type DataType => typeof(List<ServerInfo>);
        private List<ServerInfo> data = null;

        public override void Load(object v)
        {
            if (v is List<ServerInfo> d)
            {
                data = d;
            }
            else
            {
                data = new List<ServerInfo>();
                NeedSave = true;
                Save();
            }

            ServerList.Initialize(data);

            ServerList.OnSave = () =>
            {
                NeedSave = true;
                Save();
            };
        }

        public override object GetSaveData() => data;
    }
}
