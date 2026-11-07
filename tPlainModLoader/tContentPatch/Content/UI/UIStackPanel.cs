using Microsoft.Xna.Framework;
using Terraria.UI;

namespace tContentPatch.Content.UI
{
    /// <summary>
    /// 竖向或横向排列子ui
    /// </summary>
    public class UIStackPanel : UIElement
    {
        /// <summary/>
        public float ItemMargin = 0f;
        /// <summary>
        /// 横向
        /// </summary>
        public bool Horizontal = false;
        /// <summary>
        /// 按<see cref="Horizontal"/>方向自动更新ui的宽或高
        /// </summary>
        public bool IsAutoUpdateSize = false;

        /// <inheritdoc/>
        public override void RecalculateChildren()
        {
            base.RecalculateChildren();

            float v = 0f;

            foreach (UIElement i in Children)
            {
                if (i == null) continue;

                if (Horizontal)
                {
                    i.Left.Set(v, 0f);
                    v += i.GetOuterDimensions().Width;
                }
                else
                {
                    i.Top.Set(v, 0f);
                    v += i.GetOuterDimensions().Height;
                }

                v += ItemMargin;
            }
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsAutoUpdateSize == false) return;

            if (Horizontal) this.UpdateContainer_Width();
            else this.UpdateContainer_Height();
        }
    }
}
