using Microsoft.Xna.Framework;
using System;

namespace tContentPatch.Content.UI
{
    public partial class UITextBox
    {
        /// <summary/>
        public Utils.UIVal<string> UV = null;
        /// <summary>
        /// 会为<see langword="null"/>
        /// </summary>
        public virtual string Text
        {
            get => UV.Val;
            set => UV.SetUIVal(value);
        }
        /// <summary/>
        public virtual Func<string> OnGetText { set => UV.OnGetVal = value; }
        /// <summary/>
        public virtual Action<string> OnSetText { set => UV.OnSetVal = value; }
        /// <summary/>
        public virtual Action<string> OnTextChanged { get => UV.OnValChanged; set => UV.OnValChanged = value; }

        /// <summary/>
        public Color UITextColor = Color.White;
        /// <summary/>
        public Color UITextColorDeft = Color.Gray;
        /// <summary/>
        public Color UITextShadowColor = Color.Black;
        /// <summary/>
        public Color UITextShadowColorDeft = Color.Black;
        /// <summary>
        /// 设置样式
        /// </summary>
        public Action<UITextBox> Style = Style1;
        private bool StyleLoaded = false;

        /// <summary>
        /// 焦点
        /// </summary>
        public virtual bool Focus
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
        private bool focus = false;
        /// <summary/>
        public Action OnGotFocus = null;
        /// <summary/>
        public Action OnLostFocus = null;

        /// <summary>
        /// 默认显示文本
        /// </summary>
        public string DefaultDisplayText = null;

        /// <summary>
        /// 显示文本<br/>
        /// 会为<see langword="null"/><br/>
        /// <see cref="UIText"/>显示此文本<br/>
        /// 编辑此文本会更新到<see cref="Text"/>
        /// </summary>
        protected virtual string DisplayText
        {
            get => displayText;
            set
            {
                displayText = value;
                UpdateTextWhenDisplayText();
            }
        }
        private string displayText = null;
    }
}
