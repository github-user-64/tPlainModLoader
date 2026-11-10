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
}
