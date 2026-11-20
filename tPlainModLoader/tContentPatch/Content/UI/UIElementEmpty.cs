using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace tContentPatch.Content.UI
{
    /// <summary>
    /// 用于占位, 一条线
    /// </summary>
    public class UIElementEmpty : UIElement
    {
        /// <summary/>
        protected Texture2D texture = null;

        /// <summary/>
        public UIElementEmpty(Texture2D texture = null)
        {
            if (texture == null) texture = Main.Assets.Request<Texture2D>("Images/UI/Divider").Value;
            this.texture = texture;

            Width.Set(0f, 1f);
            Height.Set(texture.Height, 0f);
        }

        /// <inheritdoc/>
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, GetDimensions().ToRectangle(), Color.White);
        }
    }
}
