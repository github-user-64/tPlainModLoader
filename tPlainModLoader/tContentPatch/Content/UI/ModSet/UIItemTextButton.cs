using Microsoft.Xna.Framework.Graphics;
using System;

namespace tContentPatch.Content.UI.ModSet
{
    /// <summary>
    /// 文本按钮
    /// </summary>
    public class UIItemTextButton : UIItem
    {
        /// <summary/>
        public readonly UITextButton UIButton = null;

        /// <summary/>
        public Action OnClick { get => UIButton.OnClick; set => UIButton.OnClick = value; }

        /// <summary/>
        public UIItemTextButton(Texture2D ico = null, string title = null, string buttonText = null) : base(ico, title)
        {
            UIButton = new UITextButton(buttonText);
            UIButton.Height.Set(0f, 1f);
            UIButton.HAlign = 1f;
            UIButton.VAlign = 0.5f;
            Append(UIButton);
        }
    }
}
