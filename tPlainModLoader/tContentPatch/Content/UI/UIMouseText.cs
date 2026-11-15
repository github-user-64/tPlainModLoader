using Microsoft.Xna.Framework;
using System;
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
        public Action<string> MouseTextStyle = MouseTextStyle1;

        /// <summary/>
        public virtual void ApplyMouseText(string text = null)
        {
            MouseTextStyle?.Invoke(text);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsMouseHovering) ApplyMouseText(MouseText);
        }

        /// <summary/>
        public static void MouseTextStyle1(string text = null)
        {
            if (text != null) Main.instance.MouseText(text);
        }

        /// <summary/>
        public static void MouseTextStyle2(string text = null)
        {
            if (text != null) DrawTip.SetDraw(text);
        }
    }
}
