using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.UI;

namespace tContentPatch.Content.UI.ModSet
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
        /// <summary/>
        public bool IsChecked { get => UISwitch.IsChecked; set => UISwitch.IsChecked = value; }
        /// <summary/>
        public Func<bool> OnGetIsChecked { set => UISwitch.OnGetIsChecked = value; }
        /// <summary/>
        public Action<bool> OnSetIsChecked { set => UISwitch.OnSetIsChecked = value; }

        /// <summary/>
        public UIItemSwitchATextBox(Texture2D ico = null, string title = null) : base(ico, title)
        {
            UIS = new UIElement();
            UIS.Width.Set(0f, 0.5f);
            UIS.Height.Set(0f, 1f);
            UIS.HAlign = 1f;
            Append(UIS);

            UISwitch = new UISwitch();
            UISwitch.Width.Set(30f, 0f);
            UISwitch.HAlign = 1f;
            UIS.Append(UISwitch);

            UITextBox.Width.Set(-UISwitch.Width.Pixels - 2f, 1f);
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
        public UIItemSwitchATextBoxString(Texture2D ico = null, string title = null) : base(ico, title) { }

        /// <inheritdoc/>
        protected override UITextBoxValue<string> GetUITextBoxValue() => new UITextBoxString();
    }

    /// <inheritdoc/>
    public class UIItemSwitchATextBoxInt : UIItemSwitchATextBox<int>
    {
        /// <inheritdoc/>
        public UIItemSwitchATextBoxInt(Texture2D ico = null, string title = null) : base(ico, title) { }

        /// <inheritdoc/>
        protected override UITextBoxValue<int> GetUITextBoxValue() => new UITextBoxInt();
    }

    /// <inheritdoc/>
    public class UIItemSwitchATextBoxFloat : UIItemSwitchATextBox<float>
    {
        /// <inheritdoc/>
        public UIItemSwitchATextBoxFloat(Texture2D ico = null, string title = null) : base(ico, title) { }

        /// <inheritdoc/>
        protected override UITextBoxValue<float> GetUITextBoxValue() => new UITextBoxFloat();
    }

    /// <inheritdoc/>
    public class UIItemSwitchATextBoxDouble : UIItemSwitchATextBox<double>
    {
        /// <inheritdoc/>
        public UIItemSwitchATextBoxDouble(Texture2D ico = null, string title = null) : base(ico, title) { }

        /// <inheritdoc/>
        protected override UITextBoxValue<double> GetUITextBoxValue() => new UITextBoxDouble();
    }
}
