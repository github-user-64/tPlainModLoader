using Microsoft.Xna.Framework.Graphics;
using PixelArt.Utils;
using System;
using tContentPatch.Content.UI;
using Terraria.UI;

namespace PixelArt.Content.UI
{
    internal class UIItemTextBoxASwitchBind<T> : UIItemMouseText, IBindUIAVal<bool>
    {
        private UISwitch ui_s = null;
        private UITextBox ui_t = null;

        public UIItemTextBoxASwitchBind(GetSetReset<bool> gsr, GetSetReset<T> setGsr, Func<string, T> parseTry,
            Texture2D ico = null, string text = null) : base(ico, text)
        {
            UIElement uie = new UIElement();
            uie.Width.Precent = 0.5f;
            uie.Height.Precent = 1;
            uie.HAlign = 1;

            ui_s = new UISwitch();
            ui_s.Width.Pixels = 30;
            ui_s.Height.Precent = 1;
            ui_s.HAlign = 1;
            ui_s.VAlign = 0.5f;
            ui_s.OnValUpdate += v => OnUIUpdate?.Invoke(v);

            ui_t = new UITextBoxBind<T>(setGsr, parseTry);
            ui_t.Width.Set(-(ui_s.Width.Pixels + 2), 1);
            ui_t.Height.Set(-6, 1);
            ui_t.VAlign = 0.5f;

            Append(uie);
            uie.Append(ui_t);
            uie.Append(ui_s);

            BindUIAVal.Bind(gsr, this);
        }

        public event Action<bool> OnUIUpdate;

        public void SetUIVal(bool v) => ui_s.SetVal(v);
    }
}
