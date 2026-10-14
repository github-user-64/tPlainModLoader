using System;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace CreativeInventory.CreativeInventory
{
    /// <summary>
    /// 椭圆轨道加圆点的开关, 圆点在右边是开, 在左边是关
    /// </summary>
    internal class UISearchSwitch : UIElement
    {
        public Action<bool> OnValUpdate = null;

        protected UISwitch ui_switch = null;

        public UISearchSwitch(string text)
        {
            ui_switch = new UISwitch();
            ui_switch.HAlign = 1f;
            Append(ui_switch);

            UIText ui_text = new UIText(text, 0.6f);
            ui_text.Width.Set(40, 0);
            ui_text.VAlign = 0.5f;
            ui_text.TextOriginX = 1f;
            Append(ui_text);

            ui_switch.Recalculate();
            ui_text.Recalculate();

            Width.Set(ui_text.Width.Pixels + ui_switch.Width.Pixels + 2, 0);
            Height.Set(ui_switch.Height.Pixels, 0);

            ui_switch.OnValUpdate += v => OnValUpdate?.Invoke(v);
        }

        public void SetVal(bool v) => ui_switch.SetVal(v);
        public bool GetVal() => ui_switch.GetVal();
    }
}
