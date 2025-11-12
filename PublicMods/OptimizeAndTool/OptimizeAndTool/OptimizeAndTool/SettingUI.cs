using OptimizeAndTool.Content;
using OptimizeAndTool.Content.ServerList;
using OptimizeAndTool.Utils.quickBuild;
using System;
using tContentPatch;
using Terraria.UI;

namespace OptimizeAndTool
{
    internal class SettingUI_player : ModSetting
    {
        public class Data
        {
            public bool CleanRepeatChat = true;
            public bool CopyChat = true;
            public bool ServerList = true;
            public bool ItemToolTipAdditional = true;
        }

        public override string Name => "设置";
        public override string Title => "优化和工具: 设置";
        public override string FilePath => "setting.json";
        public override Type DataType => typeof(Data);

        public override void Load(object v)
        {
            if (v is Data data)
            {
                CleanRepeatChat.Enable.val = data.CleanRepeatChat;
                CopyChat.Enable.val = data.CopyChat;
                ServerList.Enable.val = data.ServerList;
                ItemToolTipAdditional.Enable.val = data.ItemToolTipAdditional;
            }

            CleanRepeatChat.Enable.OnValUpdate += _ => NeedSave = true;
            CopyChat.Enable.OnValUpdate += _ => NeedSave = true;
            ServerList.Enable.OnValUpdate += _ => NeedSave = true;
            ItemToolTipAdditional.Enable.OnValUpdate += _ => NeedSave = true;
        }

        public override UIElement GetUI()
        {
            return UIBuild.get3(Function.GetUI());
        }

        public override void SetDefault()
        {
            CleanRepeatChat.Enable.Reset();
            CopyChat.Enable.Reset();
            ServerList.Enable.Reset();
            ItemToolTipAdditional.Enable.Reset();
        }

        public override object GetSaveData()
        {
            return new Data()
            {
                CleanRepeatChat = CleanRepeatChat.Enable.val,
                CopyChat = CopyChat.Enable.val,
                ServerList = ServerList.Enable.val,
                ItemToolTipAdditional = ItemToolTipAdditional.Enable.val,
            };
        }
    }
}
