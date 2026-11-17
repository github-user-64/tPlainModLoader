using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.UI;

namespace tContentPatch.Content.UI.Item1
{
    /// <summary>
    /// 组合ui: <see cref="UI.UISwitch"/>, <see cref="UIItemTextBox{T}"/>
    /// </summary>
    public class UIItemSwitchATextBox<T> : UIItemTextBox<T>
    {
        /// <summary/>
        public readonly UIElement UIS = null;
        /// <summary/>
        public readonly UISwitch UISwitch = null;

        /// <summary>
        /// <see cref="UISwitch.IsChecked"/>
        /// </summary>
        public virtual bool IsChecked { get => UISwitch.IsChecked; set => UISwitch.IsChecked = value; }
        /// <summary/>
        public virtual Func<bool> OnGetIsChecked { set => UISwitch.OnGetIsChecked = value; }
        /// <summary/>
        public virtual Action<bool> OnSetIsChecked { set => UISwitch.OnSetIsChecked = value; }
        /// <summary/>
        public virtual Action<bool> OnIsCheckChanged { get => UISwitch.OnIsCheckChanged; set => UISwitch.OnIsCheckChanged = value; }

        /// <summary/>
        public UIItemSwitchATextBox(Texture2D ico = null, string title = null, string defaultDisplayText = null) : base(ico, title, defaultDisplayText)
        {
            UIS = new UIElement();
            UIS.Width.Set(0f, 0.5f);
            UIS.Height.Set(0f, 1f);
            UIS.HAlign = 1f;
            Append(UIS);

            int Width = 30;

            UISwitch = new UISwitch();
            UISwitch.Style = v =>
            {
                UISwitch.Style1(v);
                v.Width.Set(Width, 0f);
                v.Height.Set(0f, 1f);
            };
            UISwitch.HAlign = 1f;
            UISwitch.VAlign = 0.5f;
            UIS.Append(UISwitch);

            UITextBox.Width.Set(-Width - 2f, 1f);
            UITextBox.Height.Set(-8f, 1f);
            UITextBox.HAlign = 0f;
            UITextBox.VAlign = 0.5f;
            UIS.Append(UITextBox);
        }
    }

    /// <inheritdoc/>
    public class UIItemSwitchATextBoxString : UIItemSwitchATextBox<string>
    {
        /// <inheritdoc/>
        public UIItemSwitchATextBoxString(Texture2D ico = null, string title = null, string defaultDisplayText = null) : base(ico, title, defaultDisplayText) { }

        /// <inheritdoc/>
        protected override UITextBox<string> GetUITextBoxValue(string defaultDisplayText = null) => new UITextBoxString(defaultDisplayText);
    }

    /// <inheritdoc/>
    public class UIItemSwitchATextBoxInt : UIItemSwitchATextBox<int>
    {
        /// <inheritdoc/>
        public UIItemSwitchATextBoxInt(Texture2D ico = null, string title = null, string defaultDisplayText = null) : base(ico, title, defaultDisplayText) { }

        /// <inheritdoc/>
        protected override UITextBox<int> GetUITextBoxValue(string defaultDisplayText = null) => new UITextBoxInt(defaultDisplayText);
    }

    /// <inheritdoc/>
    public class UIItemSwitchATextBoxFloat : UIItemSwitchATextBox<float>
    {
        /// <inheritdoc/>
        public UIItemSwitchATextBoxFloat(Texture2D ico = null, string title = null, string defaultDisplayText = null) : base(ico, title, defaultDisplayText) { }

        /// <inheritdoc/>
        protected override UITextBox<float> GetUITextBoxValue(string defaultDisplayText = null) => new UITextBoxFloat(defaultDisplayText);
    }

    /// <inheritdoc/>
    public class UIItemSwitchATextBoxDouble : UIItemSwitchATextBox<double>
    {
        /// <inheritdoc/>
        public UIItemSwitchATextBoxDouble(Texture2D ico = null, string title = null, string defaultDisplayText = null) : base(ico, title, defaultDisplayText) { }

        /// <inheritdoc/>
        protected override UITextBox<double> GetUITextBoxValue(string defaultDisplayText = null) => new UITextBoxDouble(defaultDisplayText);
    }
}
