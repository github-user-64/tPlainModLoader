using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace tContentPatch.Content.UI.ModSet
{
    /// <inheritdoc/>
    public class UIItemValueSliderInt : UIItemValueSlider<int>
    {
        /// <inheritdoc/>
        public override int Value { get => UV.Val; set => UV.SetUIVal((int)MathHelper.Clamp(value, Min, Max)); }
        /// <summary/>
        protected readonly float Proportion = 0;

        /// <inheritdoc/>
        public UIItemValueSliderInt(int min, int max, Texture2D ico = null, string title = null) : base(min, max, ico, title)
        {
            if (Max > Min != true) throw new Exception("max需大于min");
            Proportion = Math.Abs(Max - Min) / 1f;
        }

        /// <inheritdoc/>
        protected override float GetValProportion() => (Value - Min) / Proportion;

        /// <inheritdoc/>
        protected override void SetValProportion(float v) => Value = (int)(Min + v * Proportion);
    }
}
