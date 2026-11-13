using Microsoft.Xna.Framework.Graphics;
using System;
using tContentPatch;
using tContentPatch.Content.UI.ModSet;
using Terraria;
using Terraria.UI;

namespace QuickSetting
{
    public class SettingKeyBind : ModSetting
    {
        public override string Name => "按键绑定";
        public override string Title => "快捷设置: 按键绑定";
        public override string FilePath => "KeyBind.json";
        public override Type DataType => typeof(string);
        public static string key = null;

        public override void Load(object v)
        {
            key = v as string;
            NeedSave = false;
        }

        public override UIElement GetUI()
        {
            UIItemKeybinding ui = new UIItemKeybinding(
                Main.Assets.Request<Texture2D>("Images/UI/ChestCraft_0", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
                "开关设置窗口");
            ui.OnGetBind = () => key;
            ui.OnSetBind = v => key = v;
            ui.Bind = key;
            ui.OnBindChanged += _ => NeedSave = true;

            return ui;
        }

        public override object GetSaveData() => key;

        public override void SetDefault() => key = null;
    }
}
