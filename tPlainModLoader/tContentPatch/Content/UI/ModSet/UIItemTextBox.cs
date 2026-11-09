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
        public readonly UITextBoxValue<T> UITextBox = null;
        /// <summary/>
        public T Value { get => UITextBox.Value; set => UITextBox.Value = value; }
        /// <summary/>
        public Func<T> OnGetValue { set => UITextBox.OnGetValue = value; }
        /// <summary/>
        public Action<T> OnSetValue { set => UITextBox.OnSetValue = value; }

        /// <summary/>
        public virtual Func<T, string> ValueToText { get => UITextBox.ValueToText; set => UITextBox.ValueToText = value; }
        /// <summary/>
        public virtual Func<string, T> TextToValue { get => UITextBox.TextToValue; set => UITextBox.TextToValue = value; }

        /// <summary/>
        public UIItemTextBox(Texture2D ico = null, string title = null) : base(ico, title)
        {
            UITextBox = GetUITextBoxValue();
            UITextBox.Width.Set(0f, 0.5f);
            UITextBox.Height.Set(-8, 1f);
            UITextBox.HAlign = 1f;
            UITextBox.VAlign = 0.5f;
            Append(UITextBox);
        }

        /// <summary/>
        protected virtual UITextBoxValue<T> GetUITextBoxValue() => new UITextBoxValue<T>();
    }

    /// <inheritdoc/>
    public class UIItemTextBoxString : UIItemTextBox<string>
    {
        /// <inheritdoc/>
        public UIItemTextBoxString(Texture2D ico = null, string title = null) : base(ico, title) { }

        /// <inheritdoc/>
        protected override UITextBoxValue<string> GetUITextBoxValue() => new UITextBoxString();
    }

    /// <inheritdoc/>
    public class UIItemTextBoxInt : UIItemTextBox<int>
    {
        /// <inheritdoc/>
        public UIItemTextBoxInt(Texture2D ico = null, string title = null) : base(ico, title) { }

        /// <inheritdoc/>
        protected override UITextBoxValue<int> GetUITextBoxValue() => new UITextBoxInt();
    }

    /// <inheritdoc/>
    public class UIItemTextBoxFloat : UIItemTextBox<float>
    {
        /// <inheritdoc/>
        public UIItemTextBoxFloat(Texture2D ico = null, string title = null) : base(ico, title) { }

        /// <inheritdoc/>
        protected override UITextBoxValue<float> GetUITextBoxValue() => new UITextBoxFloat();
    }

    /// <inheritdoc/>
    public class UIItemTextBoxDouble : UIItemTextBox<double>
    {
        /// <inheritdoc/>
        public UIItemTextBoxDouble(Texture2D ico = null, string title = null) : base(ico, title) { }

        /// <inheritdoc/>
        protected override UITextBoxValue<double> GetUITextBoxValue() => new UITextBoxDouble();
    }
}
