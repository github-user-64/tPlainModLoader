using System;
using tContentPatch.Content.UI;
using Terraria.GameContent.UI.Elements;

namespace CreativeInventory.CreativeInventory
{
    /// <summary>
    /// 椭圆轨道加圆点的开关, 圆点在右边是开, 在左边是关
    /// </summary>
    internal class UISearchSwitch : UIMouseText
    {
        public virtual Action<bool> OnIsCheckChanged { get => ui_switch.OnIsCheckChanged; set => ui_switch.OnIsCheckChanged = value; }
        public virtual bool IsChecked { get => ui_switch.IsChecked; set => ui_switch.IsChecked = value; }

        protected UISwitch ui_switch = null;

        public UISearchSwitch(string text)
        {
            ui_switch = new UISwitch();
            ui_switch.HAlign = 1f;
            ui_switch.Style = null;
            UISwitch.Style2(ui_switch);//手动设置, 提前获取开关ui的宽度
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
        }
    }
}
