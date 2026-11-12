using Microsoft.Xna.Framework;
using System;

namespace tContentPatch.Content.UI
{
    public partial class UITextBox
    {
        /// <summary/>
        public Color UITextColor = Color.White;
        /// <summary/>
        public Color UITextColorDeft = Color.Gray;
        /// <summary/>
        public Color UITextShadowColor = Color.Black;
        /// <summary/>
        public Color UITextShadowColorDeft = Color.Black;

        /// <summary/>
        public Utils.UIVal<string> UV = null;
        /// <summary/>
        public string Text
        {
            get => UV.Val;
            set => UV.SetUIVal(value);
        }
        /// <summary/>
        public Func<string> OnGetText { set => UV.OnGetVal = value; }
        /// <summary/>
        public Action<string> OnSetText { set => UV.OnSetVal = value; }
        /// <summary/>
        public Action<string> OnTextChanged { get => UV.OnValChanged; set => UV.OnValChanged = value; }

        /// <summary>
        /// 最大输入长度, 小于<see langword="0"/>为不限
        /// </summary>
        public int MaxInputLength = -1;
        /// <summary>
        /// 及时更新文本, 在显示文本更新时更新<see cref="Text"/>
        /// </summary>
        public bool IsUpdateTextWhenDisplayText = true;
        /// <summary>
        /// 失焦更新文本, 在失去焦点时更新<see cref="Text"/>
        /// </summary>
        public bool IsUpdateTextWhenLostFocus = false;
        /// <summary>
        /// 在<see cref="Focus"/>为<see langword="true"/>时不更新显示文本
        /// </summary>
        public bool NoUpdateDisplayTextWhenHasFocus = false;

        private bool focus = false;
        /// <summary/>
        public bool Focus
        {
            get => focus;
            set
            {
                if (focus == value) return;
                focus = value;
                if (Focus) GotFocus();
                else LostFocus();
            }
        }
        /// <summary/>
        public Action OnGotFocus = null;
        /// <summary/>
        public Action OnLostFocus = null;

        private string defaultDisplayText = null;
        /// <summary/>
        public string DefaultDisplayText
        {
            get => defaultDisplayText;
            set => defaultDisplayText = value;
        }

        private string displayText = null;
        /// <summary/>
        protected string DisplayText
        {
            get => displayText;
            set
            {
                displayText = value;
                UpdateTextWhenDisplayText();
            }
        }
    }
}
