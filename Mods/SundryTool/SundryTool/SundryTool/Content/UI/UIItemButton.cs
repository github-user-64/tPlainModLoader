using Microsoft.Xna.Framework.Graphics;
using System;

namespace SundryTool.Content.UI
{
    internal class UIItemButton : UIItemMouseText
    {
        public Action OnClick = null;

        public UIItemButton(string btnText, Texture2D ico = null, string text = null) : base(ico, text)
        {
            UITextButton ui_btn = new UITextButton(btnText, 0.8f);
            ui_btn.HAlign = 1;
            ui_btn.VAlign = 0.5f;
            ui_btn.OnLeftClick += (e, s) => OnClick?.Invoke();

            Append(ui_btn);
        }
    }
}
