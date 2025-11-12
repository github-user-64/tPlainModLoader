using Microsoft.Xna.Framework.Graphics;
using System;

namespace tContentPatch.Content.UI.ModSet
{
    /// <summary>
    /// 开关
    /// </summary>
    public class UIItemSwitch : UIItem
    {
        public Action<bool> OnValUpdate = null;
        private UISwitch ui_s = null;

        public UIItemSwitch(Texture2D ico = null, string text = null) : base(ico, text)
        {
            ui_s = new UISwitch();
            ui_s.Height.Precent = 1;
            ui_s.HAlign = 1;
            ui_s.VAlign = 0.5f;
            ui_s.OnValUpdate += v => OnValUpdate?.Invoke(v);

            Append(ui_s);
        }

        public void SetVal(bool v)
        {
            ui_s.SetVal(v);
        }

        public bool GetVal()
        {
            return ui_s.GetVal();
        }
    }
}
