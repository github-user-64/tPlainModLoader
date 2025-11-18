using SundryTool.Utils;
using System;
using tContentPatch.Content.UI;

namespace SundryTool.Content.UI
{
    internal class UITextBoxBind<T> : UITextBox, IBindUIAVal<T>
    {
        public UITextBoxBind(GetSetReset<T> gsr, Func<string, T> parseTry,
            string text_default = "") : base(text_default)
        {
            OnLostFocus += () =>
            {
                try
                {
                    T parseV = parseTry(Text);
                    if (parseV == null && gsr == null) return;
                    if (parseV?.Equals(gsr.val) == true) return;
                    OnUIUpdate?.Invoke(parseV);
                }
                catch
                {
                    SetUIVal(gsr.val);
                    return;
                }
            };

            BindUIAVal.Bind(gsr, this);
        }

        public event Action<T> OnUIUpdate;

        public void SetUIVal(T v) => Text = v?.ToString();
    }
}
