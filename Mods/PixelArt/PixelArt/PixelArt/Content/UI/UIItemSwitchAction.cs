using Microsoft.Xna.Framework.Graphics;
using System;

namespace PixelArt.Content.UI
{
    internal class UIItemSwitchAction : UIItemMouseText
    {
        public UIItemSwitchAction(Func<bool> func, Action open, Action close, Texture2D ico = null, string text = null) : base(ico, text)
        {
            UISwitchAction ui_sa = new UISwitchAction(func, open, close);
            ui_sa.Height.Precent = 1;
            ui_sa.HAlign = 1;
            ui_sa.VAlign = 0.5f;

            Append(ui_sa);
        }
    }
}
