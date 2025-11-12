using Microsoft.Xna.Framework;

namespace tContentPatch.Content.UI
{
    public class UIStackPanel : Terraria.UI.UIElement
    {
        public float ItemMargin = 0f;
        public bool Horizontal = false;
        public bool IsAutoUpdateSize = false;

        public override void RecalculateChildren()
        {
            base.RecalculateChildren();

            float v = 0f;

            foreach (Terraria.UI.UIElement i in Children)
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsAutoUpdateSize == false) return;

            if (Horizontal) this.UpdateContainer_Width();
            else this.UpdateContainer_Height();
        }
    }
}
