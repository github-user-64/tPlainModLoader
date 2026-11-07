using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace tContentPatch.Content.UI.ModSet
{
    /// <summary>
    /// 有图标和文本的项
    /// </summary>
    public class UIItem : UIMouseText
    {
        /// <summary/>
        public Color Color = new Color(73, 94, 171);
        /// <summary/>
        public readonly UIStackPanel UIIcoTitle = null;
        /// <summary/>
        public readonly UIImage UIIco = null;
        /// <summary/>
        public readonly UIText UITitle = null;

        /// <summary/>
        public UIItem(Texture2D ico = null, string title = null)
        {
            Width.Set(0f, 1f);
            Height.Set(30f, 0f);
            PaddingLeft = PaddingRight = 8;
            PaddingTop = PaddingBottom = 0;

            UIIcoTitle = new UIStackPanel();
            UIIcoTitle.Height.Set(0f, 1f);
            UIIcoTitle.Horizontal = true;
            UIIcoTitle.ItemMargin = 6f;
            UIIcoTitle.IsAutoUpdateSize = true;
            Append(UIIcoTitle);

            UIIco = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/OnePixel").Value);
            if (ico != null)
            {
                UIIco.SetImage(ico);
                float v = UIIco.Width.Pixels / UIIco.Height.Pixels;
                UIIco.Height.Pixels = Height.Pixels - PaddingLeft;
                UIIco.Width.Pixels = UIIco.Height.Pixels * v;
                UIIco.VAlign = 0.5f;
                UIIco.ScaleToFit = true;
                UIIcoTitle.Append(UIIco);
            }

            UITitle = new UIText(string.Empty);
            if (title != null)
            {
                UITitle.VAlign = 0.5f;
                UITitle.SetText(title ?? string.Empty, 0.8f, false);
                UIIcoTitle.Append(UITitle);
            }
        }

        /// <inheritdoc/>
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            DrawPanel(spriteBatch);
        }

        /// <summary/>
        public virtual void DrawPanel(SpriteBatch spriteBatch)
        {
            Color color = IsMouseHovering ? Color : Color.MultiplyRGBA(new Color(180, 180, 180));

            CalculatedStyle dim = GetDimensions();

            Texture2D texture = TextureAssets.SettingsPanel.Value;
            int edge = 2;
            int x = (int)dim.X;
            int y = (int)dim.Y;
            int width = (int)dim.Width;
            int height = (int)dim.Height;

            spriteBatch.Draw(texture, new Rectangle(x, y + edge, edge, height - edge * 2),
                new Rectangle(edge, 0, 1, 1), color);
            spriteBatch.Draw(texture, new Rectangle(x + width - edge, y + edge, edge, height - edge * 2),
                new Rectangle(edge, 0, 1, 1), color);

            spriteBatch.Draw(texture, new Rectangle(x + edge, y, width - edge * 2, edge)
                , new Rectangle(edge, 0, 1, 1), color);
            spriteBatch.Draw(texture, new Rectangle(x + edge, y + height - edge, width - edge * 2, edge)
                , new Rectangle(edge, 0, 1, 1), color);

            spriteBatch.Draw(texture,
                new Rectangle(x + edge, y + edge, width - edge * 2, height - edge * 2),
                new Rectangle(edge, edge, texture.Width - edge * 2, texture.Height - edge * 2),
                color);
        }
    }
}
