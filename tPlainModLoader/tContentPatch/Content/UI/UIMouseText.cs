using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;

namespace tContentPatch.Content.UI
{
    /// <summary>
    /// 鼠标在上方时的鼠标文本
    /// </summary>
    public class UIMouseText : UIElement
    {
        /// <summary/>
        public string MouseText = null;

        /// <summary/>
        public virtual void ApplyMouseText(string text = null)
        {
            if (MouseText == null) return;

            Main.instance.MouseText(MouseText);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsMouseHovering) ApplyMouseText();
        }
    }
}
