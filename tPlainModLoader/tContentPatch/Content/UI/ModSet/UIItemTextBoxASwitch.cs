using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.UI;

namespace tContentPatch.Content.UI.ModSet
{
    /// <summary>
    /// 组合ui: <see cref="UI.UITextBox"/>, <see cref="UI.UISwitch"/>
    /// </summary>
    public class UIItemTextBoxASwitch : UIItem
    {
        /// <summary/>
        public readonly UIElement UIS = null;

        /// <summary/>
        public readonly UISwitch UISwitch = null;
        /// <summary/>
        public bool IsChecked { get => UISwitch.IsChecked; set => UISwitch.IsChecked = value; }
        /// <summary/>
        public Action<bool> OnSetIsChecked { get => UISwitch.OnSetIsChecked; set => UISwitch.OnSetIsChecked = value; }
        /// <summary/>
        public Func<bool> OnGetIsChecked { get => UISwitch.OnGetIsChecked; set => UISwitch.OnGetIsChecked = value; }

        /// <summary/>
        public readonly UITextBox UITextBox = null;
        /// <summary/>
        public string Text { get => UITextBox.Text; set => UITextBox.Text = value; }
        /// <summary/>
        public Action<string> OnSetText { get => UITextBox.OnSetText; set => UITextBox.OnSetText = value; }
        /// <summary/>
        public Func<string> OnGetText { get => UITextBox.OnGetText; set => UITextBox.OnGetText = value; }

        /// <summary/>
        public UIItemTextBoxASwitch(Texture2D ico = null, string title = null) : base(ico, title)
        {
            UIS = new UIElement();
            UIS.Width.Set(0f, 0.6f);
            UIS.Height.Set(0f, 1f);
            UIS.HAlign = 1f;
            Append(UIS);

            UISwitch = new UISwitch();
            UISwitch.Width.Set(30f, 0f);
            UISwitch.HAlign = 1f;
            UIS.Append(UISwitch);

            UITextBox = new UITextBox();
            UITextBox.Width.Set(-UISwitch.Width.Pixels - 2f, 1f);
            UITextBox.Height.Set(-6f, 1f);
            UITextBox.VAlign = 0.5f;
            UIS.Append(UITextBox);
        }
    }
}
