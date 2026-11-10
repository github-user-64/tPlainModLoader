using Newtonsoft.Json;
using System;
using tContentPatch;
using tContentPatch.Content.UI;
using tContentPatch.Content.UI.ModSet;
using Terraria.UI;

namespace WandsTool
{
    internal class ModConfig : ModSetting
    {
        public class Data
        {
            [JsonProperty("消耗物品")]
            public bool ConsumablesItem = true;
        }

        public override string Name => "设置";
        public override string Title => "魔杖工具: 设置";
        public override string FilePath => "模组配置.txt";
        public override Type DataType => typeof(Data);
        private static Data data = null;

        public override void Load(object v)
        {
            if (v is Data data)
            {
                ModConfig.data = data;
            }
            else
            {
                SetDefault();
                Save();
            }
        }

        public override void SetDefault()
        {
            data = new Data();
            NeedSave = true;
        }

        public override object GetSaveData() => data;

        public override UIElement GetUI()
        {
            UIScrollViewer2 sv = new UIScrollViewer2();
            sv.Width.Set(0, 1);
            sv.Height.Set(0, 1);

            UIItemSwitch s = new UIItemSwitch(null, "消耗物品");
            s.MouseText = "仅对方块和墙有效";
            s.OnGetIsChecked = () => data.ConsumablesItem;
            s.OnSetIsChecked = v =>
            {
                if (data == null) return;
                if (data.ConsumablesItem == v) return;
                data.ConsumablesItem = v;
                NeedSave = true;
            };
            sv.AddChild(s);

            return sv;
        }

        public static bool IsConsumablesItem()
        {
            if (data == null) data = new Data();
            return data.ConsumablesItem;
        }
    }
}
