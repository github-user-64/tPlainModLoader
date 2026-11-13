using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.UI;

namespace tContentPatch.Content.UI.Item1
{
    /// <summary>
    /// 大标题
    /// </summary>
    public class UIItemTitle : UIItem
    {
        /// <summary/>
        public UIItemTitle(Texture2D ico = null, string title = null) : base(ico, title)
        {
            Width.Set(0, 1f);
            Height.Set(32f, 0f);
            PaddingLeft = PaddingRight = 4;

            Color = Color.White;

            UITitle.SetText(title, 1f, false);
        }

        /// <inheritdoc/>
        public override void DrawPanel(SpriteBatch spriteBatch)
        {
            CalculatedStyle dim = GetDimensions();
            Vector2 pos = new Vector2(dim.X + 10, dim.Y + dim.Height);
            Rectangle rect = new Rectangle(0, 0, (int)dim.Width - 20, 1);

            spriteBatch.Draw(TextureAssets.MagicPixel.Value, pos, rect, Color);
        }
    }
}
