using Microsoft.Xna.Framework.Graphics;
using System;
using tContentPatch.Content.UI.ModSet;

namespace PixelArt.Content.UI
{
    /// <summary>
    /// 项, 文本按钮
    /// </summary>
    internal class UIItemTextButton : UIItem
    {
        /// <summary>
        /// 文本按钮被点击
        /// </summary>
        public Action OnClick = null;

        /// <summary/>
        public UIItemTextButton(string btnText, Texture2D ico = null, string text = null) : base(ico, text)
        {
            UITextButton ui_btn = new UITextButton(btnText, 0.8f);
            ui_btn.HAlign = 1;
            ui_btn.VAlign = 0.5f;
            ui_btn.OnLeftClick += (e, s) => OnClick?.Invoke();

            Append(ui_btn);
        }
    }
}
