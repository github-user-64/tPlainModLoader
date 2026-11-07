using Microsoft.Xna.Framework;
using System;
using Terraria.UI;

namespace tContentPatch.Content.UI
{
    /// <summary>
    /// 向右排列子ui, 超出宽度换行
    /// </summary>
    public class UIWrapPanel : UIElement
    {
        /// <summary/>
        public float ItemMargin = 0f;
        /// <summary>
        /// 自动更新ui的高
        /// </summary>
        public bool IsAutoUpdateSize = false;

        /// <inheritdoc/>
        public override void Recalculate()
        {
            float x = 0;
            float y = 0;
            float i_HeightMax = 0;

            foreach (UIElement i in Children)
            {
                if (x + i.GetOuterDimensions().Width > GetInnerDimensions().Width)
                {
                    x = 0;
                    y += i_HeightMax + ItemMargin;

                    i_HeightMax = 0;
                }

                i.Left.Set(x, 0);
                i.Top.Set(y, 0);

                i_HeightMax = Math.Max(i_HeightMax, i.GetOuterDimensions().Height);

                x += ItemMargin + i.GetOuterDimensions().Width;
            }

            base.Recalculate();
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsAutoUpdateSize == false) return;

            this.UpdateContainer_Height();
        }
    }
}
