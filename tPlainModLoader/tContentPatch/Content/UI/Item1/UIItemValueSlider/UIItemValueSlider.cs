using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;

namespace tContentPatch.Content.UI.Item1
{
    /// <summary>
    ///由拖动条调整值
    /// </summary>
    public abstract class UIItemValueSlider<T> : UIItem
    {
        /// <summary>
        /// 值会显示在标题右方, <see cref="ValueToString"/>则用于修改显示的文本, 为<see langword="null"/>时不显示
        /// </summary>
        public Func<T, string> ValueToString
        {
            get => _floatToString;
            set
            {
                _floatToString = value;
                if (_floatToString != null) UpdateValueToString();
            }
        }
        private Func<T, string> _floatToString = v => $"{v}";
        /// <summary/>
        public readonly T Min;
        /// <summary/>
        public readonly T Max;

        /// <summary/>
        public readonly Utils.UIVal<T> UV = null;
        /// <summary/>
        public abstract T Value { get; set; }
        /// <summary/>
        public Func<T> OnGetValue { set => UV.OnGetVal = value; }
        /// <summary/>
        public Action<T> OnSetValue { set => UV.OnSetVal = value; }
        /// <summary/>
        public Action<T> OnValueChanged { get => UV.OnValChanged; set => UV.OnValChanged = value; }

        /// <summary/>
        public readonly UIText UIDisplayVal = null;
        /// <summary/>
        public readonly UIColoredSlider UISlider = null;

        /// <summary/>
        public UIItemValueSlider(T min, T max, Texture2D ico = null, string title = null) : base(ico, title)
        {
            Min = min;
            Max = max;

            Height.Set(TextureAssets.ColorSlider.Value.Height + 10f, 0f);
            PaddingRight = 0f;

            UIDisplayVal = new UIText(string.Empty, 0.8f, false);
            UIDisplayVal.VAlign = 0.5f;
            UIIcoTitle.Append(UIDisplayVal);
            UIIcoTitle.ItemMargin = 2f;

            UISlider = new UIColoredSlider(null, GetValProportion, SetValProportion,
                () => { },
                (v) => Color.Black * v,
                Color.Transparent);
            UISlider.HAlign = 1f;
            Append(UISlider);

            UV = new Utils.UIVal<T>(UpdateValueToString);
            UV.SetUIVal(Min, true);
        }

        /// <summary/>
        public virtual void UpdateValueToString()
        {
            if (ValueToString == null) UIDisplayVal.SetText(string.Empty);
            else UIDisplayVal.SetText($": {ValueToString.Invoke(Value)}");
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UV.UpdateUI();
        }

        /// <summary>
        /// 由min-max转到0-1
        /// </summary>
        protected abstract float GetValProportion();
        /// <summary>
        /// 由0-1转到min-max
        /// </summary>
        protected abstract void SetValProportion(float v);
    }
}
