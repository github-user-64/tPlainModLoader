using Microsoft.Xna.Framework.Graphics;
using SundryTool.Utils;
using System;

namespace SundryTool.Content.UI
{
    internal class UIItemTextBoxBind<T> : UIItemMouseText
    {
        public UIItemTextBoxBind(GetSetReset<T> gsr, Func<string, T> parseTry,
            Texture2D ico = null, string text = null) : base(ico, text)
        {
            UITextBoxBind<T> ui_t = new UITextBoxBind<T>(gsr, parseTry);
            ui_t.Width.Set(0, 0.5f);
            ui_t.Height.Set(-6, 1);
            ui_t.HAlign = 1;
            ui_t.VAlign = 0.5f;

            Append(ui_t);
        }
    }
}
