using Microsoft.Xna.Framework;
using System;
using Terraria.UI;

namespace tContentPatch.Content.UI
{
    /// <summary/>
    public partial class UITextBox : UIElement
    {
        /// <summary/>
        public Utils.UIVal<string> UV = null;
        /// <summary/>
        public Action<string> OnSetText { get => UV.OnSetVal; set => UV.OnSetVal = value; }
        /// <summary/>
        public Func<string> OnGetText { get => UV.OnGetVal; set => UV.OnGetVal = value; }
        /// <summary/>
        public string Text
        {
            get => UV.Val;
            set => UV.SetUIVal(CheckSetText(value));
        }

        /// <summary/>
        public Color UITextColor;
        /// <summary/>
        public Color UITextColorDeft;
        /// <summary/>
        public Color UITextShadowColor;
        /// <summary/>
        public Color UITextShadowColorDeft;
        /// <summary/>
        public Action OnGotFocus = null;
        /// <summary/>
        public Action OnLostFocus = null;
        /// <summary>
        /// 文本最大长度, 小于<see langword="0"/>为不限
        /// </summary>
        public int TextMaxLength = -1;

        private bool focus = false;
        /// <summary/>
        public bool Focus
        {
            get => focus;
            set
            {
                if (focus == value) return;
                focus = value;
                (focus ? OnGotFocus : OnLostFocus)?.Invoke();
            }
        }
        private string defaultDisplayText = null;
        /// <summary/>
        public string DefaultDisplayText
        {
            get => defaultDisplayText;
            set => defaultDisplayText = value;
        }
    }
}
