using System;
using System.Collections.Generic;
using tContentPatch;
using tContentPatch.Content.UI;
using tContentPatch.Content.UI.ModSet;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace QuickButton.QuickButton
{
    public class Setting : ModSetting
    {
        public class Data
        {
            public int btnPos = 0;
            public List<string> keyOrder = null;
        }

        public override string Name => "按钮设置";
        public override string Title => "快捷按钮: 按钮设置";
        public override string FilePath => "buttonSet.json";
        public override Type DataType => typeof(Data);
        private Data data = null;
        private Action<Data> updateUI = null;

        public override void Load(object v)
        {
            data = v as Data;
            if (data == null)
            {
                SetDefault();
                Save();
            }

            QuickButton.SetPos(data.btnPos);
            QuickButton.Initialize_Key(data.keyOrder);
        }

        public override UIElement GetUI()
        {
            updateUI = null;

            UIScrollViewer2 sv = new UIScrollViewer2();
            sv.Width.Precent = 1;
            sv.Height.Precent = 1;
            sv.ItemMargin = 4;

            //复位按钮
            UIItem btnPosDefault = new UIItem();
            btnPosDefault.Append(new UIText("按钮复位") { HAlign = 0.5f, VAlign = 0.5f });
            btnPosDefault.OnLeftClick += (e, s) =>
            {
                SoundEngine.PlaySound(12);
                QuickButton.SetPos(data.btnPos);
            };

            //按钮位置
            UIItemValueSlider item_vs = new UIItemValueSlider(0, 1, text: "按钮初始位置");
            item_vs.SetVal(data.btnPos);
            item_vs.FloatToString += v =>
            {
                switch (v)
                {
                    case 0: return "左边居中";
                    case 1: return "物品栏右侧";
                    default: return string.Empty;
                }
            };
            item_vs.OnValUpdate += v =>
            {
                data.btnPos = (int)v;
                NeedSave = true;
            };
            updateUI += d => item_vs.SetVal(d.btnPos);

            //排序
            UIItemTitle item_title = new UIItemTitle(text: "排序, 左键拖动");

            UIKeyOrder item_keyOrder = new UIKeyOrder(QuickButton.GetContainsKey());
            item_keyOrder.OnExchange += (v1, v2) =>
            {
                QuickButton.Exchange(v1, v2);
                data.keyOrder = QuickButton.GetAllKey();
                NeedSave = true;
            };

            sv.AddChild(btnPosDefault);
            sv.AddChild(item_vs);
            sv.AddChild(item_title);
            sv.AddChild(item_keyOrder);

            return sv;
        }

        public override void SetDefault()
        {
            data = new Data();
            data.btnPos = 0;
            data.keyOrder = null;
            NeedSave = true;

            updateUI?.Invoke(data);
        }

        public override object GetSaveData() => data;
    }
}
