using Microsoft.Xna.Framework.Graphics;
using System;

namespace tContentPatch.Content.UI.ModSet
{
    /// <summary>
    /// 文本框
    /// </summary>
    public class UIItemTextBox : UIItem
    {
        /// <summary/>
        public readonly UITextBox UITextBox = null;
        /// <summary/>
        public string Text { get => UITextBox.Text; set => UITextBox.Text = value; }
        /// <summary/>
        public Action<string> OnSetText { get => UITextBox.OnSetText; set => UITextBox.OnSetText = value; }
        /// <summary/>
        public Func<string> OnGetText { get => UITextBox.OnGetText; set => UITextBox.OnGetText = value; }

        /// <summary/>
        public UIItemTextBox(string defaultDisplayText = null, int textMaxLength = -1,
            Texture2D ico = null, string title = null) : base(ico, title)
        {
            Height.Pixels = 40;

            UITextBox = new UITextBox(defaultDisplayText);
            UITextBox.Width.Set(200f, 0f);
            UITextBox.Height.Set(-16, 1f);
            UITextBox.HAlign = 1f;
            UITextBox.VAlign = 0.5f;
            UITextBox.TextMaxLength = textMaxLength;
            Append(UITextBox);
        }
    }
}
