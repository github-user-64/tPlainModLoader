using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;

namespace tContentPatch.Content.UI
{
    /// <summary/>
    public class UIPanel2 : UIPanel
    {
        private static readonly Texture2D OnePixel = TextureAssets.MagicPixel.Value;

        /// <summary/>
        protected int barSize = 0;

        /// <summary/>
        public UIPanel2(int customBarSize = 2)
        {
            barSize = customBarSize;

            SetPadding(barSize);
        }

        /// <inheritdoc/>
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Rectangle dim = GetDimensions().ToRectangle();

            int width = dim.Width - (barSize * 2);
            int height = dim.Height - (barSize * 2);

            Draw(spriteBatch, BackgroundColor, new Rectangle(dim.X + barSize, dim.Y + barSize, width, height));

            Draw(spriteBatch, BorderColor, new Rectangle(dim.X + barSize, dim.Y, width, barSize));
            Draw(spriteBatch, BorderColor, new Rectangle(dim.X + barSize, dim.Y + barSize + height, width, barSize));

            Draw(spriteBatch, BorderColor, new Rectangle(dim.X, dim.Y + barSize, barSize, height));
            Draw(spriteBatch, BorderColor, new Rectangle(dim.X + barSize + width, dim.Y + barSize, barSize, height));
        }

        private void Draw(SpriteBatch sb, Color color, Rectangle rect)
        {
            sb.Draw(OnePixel, rect, new Rectangle(0, 0, 1, 1), color);
        }
    }
}
