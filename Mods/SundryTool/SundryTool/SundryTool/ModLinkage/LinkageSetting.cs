using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using tContentPatch;
using tContentPatch.Content.UI;
using tContentPatch.Content.UI.ModSet;
using Terraria;
using Terraria.UI;

namespace SundryTool.ModLinkage
{
    internal class LinkageSetting : ModSetting
    {
        public override string Name => "模组联动设置";
        public override string Title => "杂项功能: 模组联动设置";
        public override string FilePath => "LinkageSetting.json";
        public override Type DataType => typeof(List<bool>);
        private List<bool> data = null;
        private List<Action<bool>> updateUi = null;
        private List<(Action<bool>, string, string)> asd = null;

        public override void Load(object v)
        {
            asd = new List<(Action<bool>, string, string)>()
            {
                (a => ModQuickSetting.IsLinkage_PlayerModify_QuickSetting = a, "Images/Item_1746", "玩家属性: 添加到快捷设置"),
                (a => ModQuickSetting.IsLinkage_HeldItemModify_QuickSetting = a, "Images/Item_3258", "手持物品属性: 添加到快捷设置"),
                (a => ModQuickSetting.IsLinkage_Function1_QuickSetting = a, "Images/Item_1959", "其它功能1: 添加到快捷设置"),
                (a => ModQuickSetting.IsLinkage_Function2_QuickSetting = a, "Images/Item_1959", "其它功能2: 添加到快捷设置"),
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

            for (int i = 0; i < updateUi?.Count; ++i) updateUi[i].Invoke(data[i]);
        }

        public override UIElement GetUI()
        {
            updateUi = new List<Action<bool>>();

            UIStackPanel ui_sp = new UIStackPanel();
            ui_sp.Width.Precent = 1;
            ui_sp.IsAutoUpdateSize = true;

            for (int i = 0; i < data.Count; ++i)
            {
                int i_ = i;
                UIItemSwitch ui = new UIItemSwitch(
                    Main.Assets.Request<Texture2D>(asd[i].Item2, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, asd[i].Item3);
                ui.OnValUpdate += v =>
                {
                    if (data[i_] == v) return;
                    data[i_] = v;
                    NeedSave = true;
                };
                ui.OnUpdate += _ => { if (ui.IsMouseHovering) Main.instance.MouseText("重新加载模组生效"); };
                ui.SetVal(data[i_]);

                updateUi.Add(ui.SetVal);
                ui_sp.Append(ui);
            }

            return ui_sp;
        }
    }
}
