using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace tContentPatch.Content.UI.ModSet
{
    /// <summary>
    /// 有图标和文本的项
    /// </summary>
    public class UIItem : UIElement
    {
        public Color color = new Color(73, 94, 171);
        private Action<string> setText = null;

        public UIItem(Texture2D ico = null, string text = null)
        {
            Width.Precent = 1;
            Height.Pixels = 30;
            PaddingLeft = PaddingRight = 8;
            PaddingTop = PaddingBottom = 0;

            UIImage ui_ico = null;
            if (ico != null)
            {
                ui_ico = new UIImage(ico);
                float v = ui_ico.Width.Pixels / ui_ico.Height.Pixels;
                ui_ico.Height.Pixels = Height.Pixels - PaddingLeft;
                ui_ico.Width.Pixels = ui_ico.Height.Pixels * v;
                ui_ico.VAlign = 0.5f;
                ui_ico.ScaleToFit = true;
            }

            UIText ui_text = null;
            if (text != null)
            {
                ui_text = new UIText(text, 0.8f);
                ui_text.VAlign = 0.5f;
                if (ui_ico != null) ui_text.Left.Pixels = ui_ico.Width.Pixels + 6;
                setText = v => ui_text.SetText(v);
            }

            if (ui_ico != null) Append(ui_ico);
            if (ui_text != null) Append(ui_text);
        }

        public void SetTitle(string text)
        {
            setText?.Invoke(text);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            Color color = IsMouseHovering ? this.color : this.color.MultiplyRGBA(new Color(180, 180, 180));

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
