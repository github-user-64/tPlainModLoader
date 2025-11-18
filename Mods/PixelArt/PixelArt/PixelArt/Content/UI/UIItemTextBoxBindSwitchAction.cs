using Microsoft.Xna.Framework.Graphics;
using PixelArt.Utils;
using System;
using tContentPatch.Content.UI;
using Terraria.UI;

namespace PixelArt.Content.UI
{
    internal class UIItemTextBoxBindSwitchAction<T> : UIItemMouseText
    {
        public UIItemTextBoxBindSwitchAction(Func<bool> func, Action open, Action close,
            GetSetReset<T> gsr, Func<string, T> parseTry,
            Texture2D ico = null, string text = null) : base(ico, text)
        {
            UIElement uie = new UIElement();
            uie.Width.Precent = 0.5f;
            uie.Height.Precent = 1;
            uie.HAlign = 1;

            UISwitchAction ui_s = new UISwitchAction(func, open, close);
            ui_s.Width.Pixels = 30;
            ui_s.Height.Precent = 1;
            ui_s.HAlign = 1;
            ui_s.VAlign = 0.5f;

            UITextBox ui_t = new UITextBoxBind<T>(gsr, parseTry);
            ui_t.Width.Set(-(ui_s.Width.Pixels + 2), 1);
            ui_t.Height.Set(-6, 1);
            ui_t.VAlign = 0.5f;
            ui_t.SetTextScale(0.8f);

            Append(uie);
            uie.Append(ui_t);
            uie.Append(ui_s);
        }
    }
}
