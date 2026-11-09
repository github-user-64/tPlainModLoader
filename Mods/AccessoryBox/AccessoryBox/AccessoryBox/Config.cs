using System;
using tContentPatch;
using tContentPatch.Content.UI.ModSet;
using Terraria.UI;

namespace AccessoryBox
{
    internal class Config : ModSetting
    {
        public override string Name => "设置";
        public override string Title => "饰品箱: 设置";
        public override string FilePath => "setting.json";
        public override Type DataType => typeof(bool);
        public static Config Instance { get; protected set; } = null;
        private bool date = true;

        public override void Load(object v)
        {
            Instance = this;

            if (v == null)
            {
                SetDefault();
                NeedSave = true;
                Save();
            }
            else
            {
                date = (bool)v;
            }
        }

        public override object GetSaveData() => date;

        public override void SetDefault()
        {
            SetVal(true);
        }

        public override UIElement GetUI()
        {
            UIItemSwitch ui = new UIItemSwitch(null, "启用");
            ui.OnGetIsChecked = () => date;
            ui.OnSetIsChecked = SetVal;

            return ui;
        }

        public void SetVal(bool v)
        {
            if (date == v) return;
            date = v;
            NeedSave = true;
        }

        public bool GetVal() => date;
    }
}
