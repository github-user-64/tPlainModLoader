using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;

namespace tContentPatch.Content.UI.ModSet
{
    /// <summary>
    ///由拖动条调整值
    /// </summary>
    public class UIItemValueSlider : UIItem
    {
        public Action<float> OnValUpdate = null;
        /// <summary>
        /// 值会显示在标题右方, <see cref="FloatToString"/>则用于修改显示的文本, 为<see langword="null"/>时显示原本的值
        /// </summary>
        public Func<float, string> FloatToString {
            get => _floatToString;
            set
            {
                _floatToString = value;
                if (_floatToString != null) UpdateFloatToString();
            }
        }
        private Func<float, string> _floatToString = null;
        private string text = null;
        private float min = 0;
        private float max = 0;
        private float val = 0;
        private float proportion = 0;
        private bool isInt = false;
        private bool hasSetVal = false;

        public UIItemValueSlider(int min, int max, Texture2D ico = null, string text = null) : this((float)min, max, ico, text)
        {
            isInt = true;
        }

        public UIItemValueSlider(float min, float max, Texture2D ico = null, string text = null) : base(ico, text)
        {
            PaddingRight = 0;

            this.min = min;
            this.max = max;
            this.text = text;
            proportion = Math.Abs(max - min) / 1;

            Height.Pixels = TextureAssets.ColorSlider.Value.Height + 10;

            UIColoredSlider cs = new UIColoredSlider(null,
                () => val,
                (v) => SetVal(min + v * proportion),
                () => { },
                (v) => Color.Black * v,
                Color.Transparent);
            cs.HAlign = 1;

            Append(cs);
        }

        /// <summary>
        /// 由0-1转到min-max
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public float GetVal(float v)
        {
            if (isInt)
                return (float)Math.Floor(min + v * proportion);
            else
                return min + v * proportion;
        }
        /// <summary>
        /// 由min-max转到0-1, 并赋值到ui
        /// </summary>
        /// <param name="v"></param>
        public void SetVal(float v)
        {
            if (min > v) v = min;
            else
            if (max < v) v = max;

            v += -min;
            v /= proportion;

            if (isInt) v = (float)(Math.Floor(v * proportion) / proportion);

            if (v == val && hasSetVal) return;
            hasSetVal = true;
            val = v;

            float getV = GetVal(val);
            OnValUpdate?.Invoke(getV);
            if (text != null) UpdateFloatToString();
        }

        public void UpdateFloatToString()
        {
            float getV = GetVal(val);

            if (FloatToString == null) SetTitle($"{text}: {getV}");
            else SetTitle($"{text}: {FloatToString.Invoke(getV)}");
        }
    }
}
