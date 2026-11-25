using OptimizeAndTool.Content;
using OptimizeAndTool.Content.ServerList;
using OptimizeAndTool.Utils.quickBuild;
using System;
using tContentPatch;
using Terraria.UI;

namespace OptimizeAndTool
{
    internal class SettingUI : ModSetting
    {
        public class Data
        {
            public bool CleanRepeatChat = true;
            public bool CopyChat = true;
            public bool ServerList = true;
            public int ServerListItemStyle = 0;
            public bool ItemToolTipAdditional = true;
        }

        public override string Name => "设置";
        public override string Title => $"{ThisMod.ModName}: 设置";
        public override string FilePath => "Setting.json";
        public override Type DataType => typeof(Data);

        protected static SettingUI Instance = null;

        public override void Load(object v)
        {
            Instance = this;

            if (v is Data data)
            {
                CleanRepeatChat.Enable.val = data.CleanRepeatChat;
                CopyChat.Enable.val = data.CopyChat;
                ServerList.Enable.val = data.ServerList;
                ServerList.ItemStyle.val = data.ServerListItemStyle;
                ItemToolTipAdditional.Enable.val = data.ItemToolTipAdditional;
            }

            CleanRepeatChat.Enable.OnValUpdate += _ => NeedSave = true;
            CopyChat.Enable.OnValUpdate += _ => NeedSave = true;
            ServerList.Enable.OnValUpdate += _ => NeedSave = true;
            ServerList.ItemStyle.OnValUpdate += _ => NeedSave = true;
            ItemToolTipAdditional.Enable.OnValUpdate += _ => NeedSave = true;
        }

        public override void SetDefault()
        {
            CleanRepeatChat.Enable.Reset();
            CopyChat.Enable.Reset();
            ServerList.Enable.Reset();
            ServerList.ItemStyle.Reset();
            ItemToolTipAdditional.Enable.Reset();
        }

        public override object GetSaveData()
        {
            return new Data()
            {
                CleanRepeatChat = CleanRepeatChat.Enable.val,
                CopyChat = CopyChat.Enable.val,
                ServerList = ServerList.Enable.val,
                ServerListItemStyle = ServerList.ItemStyle.val,
                ItemToolTipAdditional = ItemToolTipAdditional.Enable.val,
            };
        }

        public override UIElement GetUI()
        {
            return UIBuild.get3(Function.GetUI());
        }

        private class AutoSave_Unload : Mod
        {
            public override void Unload() => Instance?.Save();
        }
        private class AutoSave_SaveSettings : PatchMain
        {
            public override void SaveSettingsPostfix() => Instance?.Save();
        }
    }
}
