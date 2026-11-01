using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria.UI;

namespace tContentPatch.Content.UI.ModSet
{
    /// <summary>
    /// 文本框
    /// </summary>
    public class UIItemTextBox : UIItem
    {
        /// <summary/>
        public Action<string> OnTextChanged = null;
        /// <summary/>
        public Action OnLostFocus = null;
        /// <summary/>
        public StyleDimension TextBoxWidth { set => textBox.Width = value; }
        /// <summary/>
        public bool Focus => textBox.Focus;
        private UITextBox textBox = null;

        /// <summary/>
        public UIItemTextBox(string text_default = "", int Text_MaxLength = -1,
            Texture2D ico = null, string text = null) : base(ico, text)
        {
            Height.Pixels = 40;

            textBox = new UITextBox(text_default);
            textBox.Width.Pixels = 200;
            textBox.Height.Set(-16, 1);
            textBox.HAlign = 1;
            textBox.VAlign = 0.5f;
            textBox.Text_MaxLength = Text_MaxLength;
            textBox.OnLostFocus += () => OnLostFocus?.Invoke();
            textBox.OnTextChanged += s => OnTextChanged?.Invoke(s);
            textBox.OnLeftClick += (e, s) => SoundEngine.PlaySound(SoundID.MenuTick);

            Append(textBox);
        }

        /// <summary/>
        public void SetText(string s)
        {
            textBox.Text = s;
        }

        /// <summary/>
        public string GetText()
        {
            return textBox.Text;
        }
    }
}
