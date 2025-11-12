using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tContentPatch.Content.UI.ModSet;
using Terraria;

namespace SundryTool.Content.UI
{
    internal class UIItemMouseText : UIItem
    {
        public string MouseText = null;

        public UIItemMouseText(Texture2D ico = null, string text = null) : base(ico, text) { }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsMouseHovering == false) return;
            if (MouseText == null) return;

            Main.instance.MouseText(MouseText);
        }
    }
}
