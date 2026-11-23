using Microsoft.Xna.Framework;
using tContentPatch.Content.UI;
using Terraria.GameContent.UI.Elements;

namespace OptimizeAndTool.Content.UI
{
    public class UIPanel2Text : UIPanel2
    {
        public readonly UIText UIText = null;

        public UIPanel2Text(string text, int customBarSize = 2) : base(customBarSize)
        {
            UIText = new UIText(text, 1f, false);
            UIText.HAlign = 0.5f;
            UIText.VAlign = 0.5f;
            Append(UIText);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MinWidth = UIText.MinWidth;
            MinHeight = UIText.MinHeight;
        }
    }
}
