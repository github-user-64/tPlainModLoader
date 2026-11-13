using Microsoft.Xna.Framework.Graphics;
using System;
using tContentPatch;
using tContentPatch.Content.UI.ModSet;
using Terraria;
using Terraria.UI;

namespace CreativeInventory
{
    internal class Setting : ModSetting
    {
        public override string Name => "设置";
        public override string Title => "创造物品栏: 设置";
        public override string FilePath => "Setting.json";
        public override Type DataType => typeof(bool);
        private bool data = true;

        public override void Load(object v)
        {
            data = v as bool? ?? true;
            NeedSave = false;

            ModLinkage.ModQuickButton.IsLinkage = data;
        }

        public override UIElement GetUI()
        {
            UIItemSwitch ui = new UIItemSwitch(
                Main.Assets.Request<Texture2D>("Images/Item_306", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
                "添加到快捷按钮");
            ui.MouseText = "重新加载模组生效";
            ui.OnGetIsChecked = () => data;
            ui.OnSetIsChecked = v => data = v;
            ui.IsChecked = data;
            ui.OnIsCheckChanged += _ => NeedSave = true;

            return ui;
        }

        public override object GetSaveData() => data;

        public override void SetDefault() => data = true;
    }
}
