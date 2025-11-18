using Microsoft.Xna.Framework.Graphics;
using PixelArt.Utils;
using System;
using tContentPatch.Content.UI.ModSet;

namespace PixelArt.Content.UI
{
    internal class UIItemValueSliderBind<T> : UIItemValueSlider, IBindUIAVal<T> where T : struct
    {
        private Func<T, float> tTofloat = null;

        public UIItemValueSliderBind(GetSetReset<T> gsr, Func<T, float> tTofloat, Func<float, T> floatTot,
            int min, int max, Texture2D ico = null, string text = null) : base(min, max, ico, text)
        {
            foo(gsr, tTofloat, floatTot);
        }

        public UIItemValueSliderBind(GetSetReset<T> gsr, Func<T, float> tTofloat, Func<float, T> floatTot,
            float min, float max, Texture2D ico = null, string text = null) : base(min, max, ico, text)
        {
            foo(gsr, tTofloat, floatTot);
        }

        private void foo(GetSetReset<T> gsr, Func<T, float> tTofloat, Func<float, T> floatTot)
        {
            this.tTofloat = tTofloat;
            OnValUpdate += v =>
            {
                OnUIUpdate?.Invoke(floatTot(v));
            };

            BindUIAVal.Bind(gsr, this);
        }

        public event Action<T> OnUIUpdate;

        public void SetUIVal(T v) => SetVal(tTofloat(v));
    }
}
