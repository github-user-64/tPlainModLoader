using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;

namespace tContentPatch.Content.Menus.ModLoadingMenu
{
    internal class UIProgressBar : UIElement
    {
        private int val = 0;
        private int max = 1;

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            Vector2 off = new Vector2(dimensions.X, dimensions.Y);
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, off, new Rectangle(0, 0, (int)dimensions.Width, (int)dimensions.Height), Color.MidnightBlue);

            float len = (dimensions.Width - 4) / max * val;

            Vector2 start = new Vector2(2, 2);
            Rectangle rect = new Rectangle(0, 0, (int)len, (int)dimensions.Height - 4);

            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, start + off, rect, Color.Yellow);
        }

        public void SetVal(int val, int max)
        {
            if (max < 1) max = 1;
            if (val > max) val = max;
            this.val = val;
            this.max = max;
        }
    }
}
