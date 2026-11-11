using Microsoft.Xna.Framework.Graphics;
using System;

namespace tContentPatch.Content.UI.ModSet
{
    /// <summary>
    /// 输入输出<typeparamref name="T"/>的文本框
    /// </summary>
    public class UIItemTextBox<T> : UIItem
    {
        /// <summary/>
        public readonly UITextBox<T> UITextBox = null;

        /// <summary/>
        public T Value { get => UITextBox.Value; set => UITextBox.Value = value; }
        /// <summary/>
        public Func<T> OnGetValue { set => UITextBox.OnGetValue = value; }
        /// <summary/>
        public Action<T> OnSetValue { set => UITextBox.OnSetValue = value; }
        /// <summary/>
        public Action<T> OnValueChanged { get => UITextBox.OnValueChanged; set => UITextBox.OnValueChanged = value; }

        /// <summary/>
        public virtual Func<T, string> ValueToText { get => UITextBox.ValueToText; set => UITextBox.ValueToText = value; }
        /// <summary/>
        public virtual Func<string, T> TextToValue { get => UITextBox.TextToValue; set => UITextBox.TextToValue = value; }

        /// <summary/>
        public UIItemTextBox(Texture2D ico = null, string title = null, string defaultDisplayText = null) : base(ico, title)
        {
            UITextBox = GetUITextBoxValue(defaultDisplayText);
            UITextBox.Width.Set(0f, 0.5f);
            UITextBox.Height.Set(-8, 1f);
            UITextBox.HAlign = 1f;
            UITextBox.VAlign = 0.5f;
            Append(UITextBox);
        }

        /// <summary/>
        protected virtual UITextBox<T> GetUITextBoxValue(string defaultDisplayText = null) => new UITextBox<T>(defaultDisplayText);
    }

    /// <inheritdoc/>
    public class UIItemTextBoxString : UIItemTextBox<string>
    {
        /// <inheritdoc/>
        public UIItemTextBoxString(Texture2D ico = null, string title = null, string defaultDisplayText = null) : base(ico, title, defaultDisplayText) { }

        /// <inheritdoc/>
        protected override UITextBox<string> GetUITextBoxValue(string defaultDisplayText = null) => new UITextBoxString();
    }

    /// <inheritdoc/>
    public class UIItemTextBoxInt : UIItemTextBox<int>
    {
        /// <inheritdoc/>
        public UIItemTextBoxInt(Texture2D ico = null, string title = null, string defaultDisplayText = null) : base(ico, title, defaultDisplayText) { }

        /// <inheritdoc/>
        protected override UITextBox<int> GetUITextBoxValue(string defaultDisplayText = null) => new UITextBoxInt();
    }

    /// <inheritdoc/>
    public class UIItemTextBoxFloat : UIItemTextBox<float>
    {
        /// <inheritdoc/>
        public UIItemTextBoxFloat(Texture2D ico = null, string title = null, string defaultDisplayText = null) : base(ico, title, defaultDisplayText) { }

        /// <inheritdoc/>
        protected override UITextBox<float> GetUITextBoxValue(string defaultDisplayText = null) => new UITextBoxFloat();
    }

    /// <inheritdoc/>
    public class UIItemTextBoxDouble : UIItemTextBox<double>
    {
        /// <inheritdoc/>
        public UIItemTextBoxDouble(Texture2D ico = null, string title = null, string defaultDisplayText = null) : base(ico, title, defaultDisplayText) { }

        /// <inheritdoc/>
        protected override UITextBox<double> GetUITextBoxValue(string defaultDisplayText = null) => new UITextBoxDouble();
    }
}
