using System;

namespace tContentPatch.Content.UI
{
    /// <summary>
    /// 输入输出<typeparamref name="T"/>的文本框
    /// </summary>
    public class UITextBox<T> : UIMouseText
    {
        /// <summary/>
        public T Value
        {
            get => TextToValue(UITextsBox.Text);
            set => UITextsBox.Text = ValueToText(value);
        }
        /// <summary/>
        public Func<T> OnGetValue { set => UITextsBox.OnGetText = () => ValueToText(value()); }
        /// <summary/>
        public Action<T> OnSetValue { set => UITextsBox.OnSetText = v => value(TextToValue(v)); }
        /// <summary/>
        public Action<T> OnValueChanged = null;
        /// <summary/>
        public virtual Func<T, string> ValueToText { get; set; } = _ => throw new NotImplementedException();
        /// <summary/>
        public virtual Func<string, T> TextToValue { get; set; } = _ => throw new NotImplementedException();

        /// <summary/>
        public readonly UITextBox UITextsBox = null;

        /// <summary/>
        public UITextBox(string defaultDisplayText = null)
        {
            UITextsBox = new UITextBox(defaultDisplayText);
            UITextsBox.IsUpdateTextWhenDisplayText = false;
            UITextsBox.IsUpdateTextWhenLostFocus = true;
            UITextsBox.NoUpdateDisplayTextWhenHasFocus = true;
            UITextsBox.OnTextChanged += v => OnValueChanged?.Invoke(TextToValue(v));
            Append(UITextsBox);

            Width = UITextsBox.Width;
            Height = UITextsBox.Height;
            UITextsBox.Width.Set(0f, 1f);
            UITextsBox.Height.Set(0f, 1f);
        }
    }
}
