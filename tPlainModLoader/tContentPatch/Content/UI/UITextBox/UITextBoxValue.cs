using System;

namespace tContentPatch.Content.UI
{
    /// <summary>
    /// 输入输出<typeparamref name="T"/>的文本框
    /// </summary>
    public class UITextBoxValue<T> : UIMouseText
    {
        /// <summary/>
        public T Value
        {
            get => TextToValue(UITextBox.Text);
            set => UITextBox.Text = ValueToText(value);
        }
        /// <summary/>
        public Func<T> OnGetValue { set => UITextBox.OnGetText = () => ValueToText(value()); }
        /// <summary/>
        public Action<T> OnSetValue { set => UITextBox.OnSetText = v => value(TextToValue(v)); }
        /// <summary/>
        protected Action<T> _OnValueChanged = null;
        /// <summary/>
        public Action<T> OnValueChanged
        {
            get => _OnValueChanged;
            set
            {
                _OnValueChanged = value;
                UITextBox.OnTextChanged = v => _OnValueChanged(TextToValue(v));
            }
        }

        /// <summary/>
        public virtual Func<T, string> ValueToText { get; set; } = _ => throw new NotImplementedException();
        /// <summary/>
        public virtual Func<string, T> TextToValue { get; set; } = _ => throw new NotImplementedException();

        /// <summary/>
        public readonly UITextBox UITextBox = null;

        /// <summary/>
        public UITextBoxValue(string defaultDisplayText = null)
        {
            UITextBox = new UITextBox(defaultDisplayText);
            UITextBox.IsUpdateTextWhenDisplayText = false;
            UITextBox.IsUpdateTextWhenLostFocus = true;
            UITextBox.NoUpdateDisplayTextWhenHasFocus = true;
            Append(UITextBox);

            Width = UITextBox.Width;
            Height = UITextBox.Height;
            UITextBox.Width.Set(0f, 1f);
            UITextBox.Height.Set(0f, 1f);
        }
    }

    /// <inheritdoc/>
    public class UITextBoxString : UITextBoxValue<string>
    {
        /// <inheritdoc/>
        public UITextBoxString(string defaultDisplayText = null) : base(defaultDisplayText) { }

        /// <inheritdoc/>
        public override Func<string, string> ValueToText { get; set; } = v => v;
        /// <inheritdoc/>
        public override Func<string, string> TextToValue { get; set; } = v => v;
    }

    /// <inheritdoc/>
    public class UITextBoxInt : UITextBoxValue<int>
    {
        /// <inheritdoc/>
        public UITextBoxInt(string defaultDisplayText = null) : base(defaultDisplayText) { }

        /// <inheritdoc/>
        public override Func<int, string> ValueToText { get; set; } = v => v.ToString();
        /// <inheritdoc/>
        public override Func<string, int> TextToValue { get; set; } = v =>
        {
            int.TryParse(v, out int rv);
            return rv;
        };
    }

    /// <inheritdoc/>
    public class UITextBoxFloat : UITextBoxValue<float>
    {
        /// <inheritdoc/>
        public UITextBoxFloat(string defaultDisplayText = null) : base(defaultDisplayText) { }

        /// <inheritdoc/>
        public override Func<float, string> ValueToText { get; set; } = v => v.ToString();
        /// <inheritdoc/>
        public override Func<string, float> TextToValue { get; set; } = v =>
        {
            float.TryParse(v, out float rv);
            return rv;
        };
    }

    /// <inheritdoc/>
    public class UITextBoxDouble : UITextBoxValue<double>
    {
        /// <inheritdoc/>
        public UITextBoxDouble(string defaultDisplayText = null) : base(defaultDisplayText) { }

        /// <inheritdoc/>
        public override Func<double, string> ValueToText { get; set; } = v => v.ToString();
        /// <inheritdoc/>
        public override Func<string, double> TextToValue { get; set; } = v =>
        {
            double.TryParse(v, out double rv);
            return rv;
        };
    }
}
