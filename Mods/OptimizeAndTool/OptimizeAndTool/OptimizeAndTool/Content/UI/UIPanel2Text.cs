using Microsoft.Xna.Framework;
using System;
using tContentPatch.Content.UI;
using Terraria.GameContent.UI.Elements;

namespace OptimizeAndTool.Content.UI
{
    public class UIPanel2Text : UIPanel2
    {
        public readonly UIText UIText = null;
        public readonly UIMouseText UIMouseText = null;

        public string MouseText { get => UIMouseText.MouseText; set => UIMouseText.MouseText = value; }
        public Action<string> MouseTextStyle { get => UIMouseText.MouseTextStyle; set => UIMouseText.MouseTextStyle = value; }

        public UIPanel2Text(string text, int customBarSize = 2) : base(customBarSize)
        {
            UIText = new UIText(text, 1f, false);
            UIText.HAlign = 0.5f;
            UIText.VAlign = 0.5f;
            Append(UIText);

            UIMouseText = new UIMouseText();
            UIMouseText.Width.Set(0f, 1f);
            UIMouseText.Height.Set(0f, 1f);
            Append(UIMouseText);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MinWidth = UIText.MinWidth;
            MinHeight = UIText.MinHeight;
        }
    }
}
