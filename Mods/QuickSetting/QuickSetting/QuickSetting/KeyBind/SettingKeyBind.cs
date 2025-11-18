using Microsoft.Xna.Framework.Graphics;
using System;
using tContentPatch;
using Terraria;
using Terraria.UI;

namespace QuickSetting.KeyBind
{
    public class SettingKeyBind : ModSetting
    {
        public override string Name => "按键绑定";
        public override string Title => "快速设置: 按键绑定";
        public override string FilePath => "keyBind.json";
        public override Type DataType => typeof(string);
        private static string key = null;
        private static Action<string> updateUI = null;

        public override void Load(object v)
        {
            key = v as string;

            QuickSetting.QuickSetting.SetBind(key);
        }

        public override UIElement GetUI()
        {
            updateUI = null;

            UIKeyBind ui_item = new UIKeyBind(Main.Assets.Request<Texture2D>("Images/UI/Camera_1", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, "设置窗口");
            ui_item.SetKey(key);
            ui_item.OnKeyUpdate += s =>
            {
                key = s;
                NeedSave = true;
                QuickSetting.QuickSetting.SetBind(key);
            };

            updateUI += s =>
            {
                ui_item.SetKey(s);
            };

            return ui_item;
        }

        public override object GetSaveData() => key;

        public override void SetDefault()
        {
            key = null;
            NeedSave = true;
            QuickSetting.QuickSetting.SetBind(key);
            updateUI?.Invoke(key);
        }
    }
}
