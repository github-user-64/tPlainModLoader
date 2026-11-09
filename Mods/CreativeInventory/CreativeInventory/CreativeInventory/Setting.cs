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
        public override string FilePath => "setting.json";
        public override Type DataType => typeof(bool);
        private bool date = true;

        public override void Load(object v)
        {
            if (v == null)
            {
                SetDefault();
                Save();
            }
            else
            {
                date = (bool)v;
            }

            ModLinkage.ModQuickButton.IsLinkage = date;
        }

        public override object GetSaveData() => date;

        public override void SetDefault()
        {
            date = true;
            NeedSave = true;
        }

        public override UIElement GetUI()
        {
            UIItemSwitch ui = new UIItemSwitch(
                Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
                "添加到快捷按钮");
            ui.MouseText = "重新加载模组生效";
            ui.OnGetIsChecked = () => date;
            ui.OnSetIsChecked = v =>
            {
                if (date == v) return;
                date = v;
                NeedSave = true;
            };

            return ui;
        }
    }
}
