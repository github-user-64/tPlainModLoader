using Microsoft.Xna.Framework.Graphics;
using SundryTool.Utils;
using System;
using tContentPatch.Content.UI;

namespace SundryTool.Content.UI
{
    internal class UIItemSwitchBind : UIItemMouseText, IBindUIAVal<bool>
    {
        private UISwitch ui_s = null;

        public UIItemSwitchBind(GetSetReset<bool> gsr, Texture2D ico = null, string text = null) : base(ico, text)
        {
            ui_s = new UISwitch();
            ui_s.Height.Precent = 1f;
            ui_s.HAlign = 1f;
            ui_s.VAlign = 0.5f;
            ui_s.OnValUpdate = v => OnUIUpdate?.Invoke(v);

            Append(ui_s);

            BindUIAVal.Bind(gsr, this);
        }

        public event Action<bool> OnUIUpdate;

        public void SetUIVal(bool v) => ui_s.SetVal(v);
    }
}
