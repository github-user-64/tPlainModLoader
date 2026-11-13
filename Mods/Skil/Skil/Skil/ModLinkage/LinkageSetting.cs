using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using tContentPatch;
using tContentPatch.Content.UI;
using tContentPatch.Content.UI.Item1;
using Terraria;
using Terraria.UI;

namespace Skil.ModLinkage
{
    internal class LinkageSetting : ModSetting
    {
        public override string Name => "模组联动设置";
        public override string Title => "魔法和技能: 模组联动设置";
        public override string FilePath => "LinkageSetting.json";
        public override Type DataType => typeof(List<bool>);
        private List<bool> data = null;
        private List<(Action<bool>, string, string)> asd = null;

        public override void Load(object v)
        {
            asd = new List<(Action<bool>, string, string)>()
            {
                (a => ModQuickSetting.IsLinkage_skil1_QuickSetting = a, "Images/Item_3366", "技能列表1: 添加到快捷设置"),
                (a => ModQuickSetting.IsLinkage_skil2_QuickSetting = a, "Images/Item_3366", "技能列表2: 添加到快捷设置"),
                (a => ModQuickSetting.IsLinkage_skil3_QuickSetting = a, "Images/Item_3366", "技能列表3: 添加到快捷设置"),
                (a => ModQuickSetting.IsLinkage_skil4_QuickSetting = a, "Images/Item_3366", "技能列表4: 添加到快捷设置"),
                (a => ModQuickSetting.IsLinkage_skil5_QuickSetting = a, "Images/Item_3366", "技能列表5: 添加到快捷设置"),
            };

            if (v is List<bool> d)
            {
                while (d.Count < asd.Count) d.Add(true);
                data = d;
                NeedSave = true;
                Save();
            }
            else
            {
                SetDefault();
                Save();
            }

            for (int i = 0; i < asd.Count; ++i)
            {
                asd[i].Item1(data[i]);
            }
        }

        public override object GetSaveData() => data;

        public override void SetDefault()
        {
            data = Enumerable.Repeat(true, asd.Count).ToList();
            NeedSave = true;
        }

        public override UIElement GetUI()
        {
            UIStackPanel ui_sp = new UIStackPanel();
            ui_sp.Width.Precent = 1;
            ui_sp.IsAutoUpdateSize = true;

            for (int i = 0; i < data.Count; ++i)
            {
                int index = i;
                UIItemSwitch ui = new UIItemSwitch(
                    Main.Assets.Request<Texture2D>(asd[i].Item2, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, asd[i].Item3);
                ui.MouseText = "重新加载模组生效";
                ui.OnGetIsChecked = () => data[index];
                ui.OnSetIsChecked = v =>
                {
                    if (data[index] == v) return;
                    data[index] = v;
                    NeedSave = true;
                };

                ui_sp.Append(ui);
            }

            return ui_sp;
        }
    }
}
