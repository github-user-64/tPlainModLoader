using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace tContentPatch.Content.UI.ModSet
{
    /// <summary>
    /// 大标题
    /// </summary>
    public class UIItemTitle : UIElement
    {
        public UIItemTitle(Texture2D ico = null, string text = null)
        {
            Width.Precent = 1;
            Height.Pixels = 34;
            PaddingLeft = PaddingRight = 4;
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
                ui_text = new UIText(text);
                ui_text.VAlign = 0.5f;
                if (ui_ico != null) ui_text.Left.Pixels = ui_ico.Width.Pixels + 10;
            }

            if (ui_ico != null) Append(ui_ico);
            if (ui_text != null) Append(ui_text);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            CalculatedStyle dim = GetDimensions();
            Vector2 pos = new Vector2(dim.X + 10, dim.Y + dim.Height);
            Rectangle rect = new Rectangle(0, 0, (int)dim.Width - 20, 1);

            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, pos, rect, Color.White);
        }
    }
}
