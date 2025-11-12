using System.Collections.Generic;
using tContentPatch.Content.UI;

namespace PixelArt.Content
{
    internal class Window : UIWindow
    {
        public Window(string title, int width, int height) : base(title, width, height)
        {
            Children.PaddingTop = Children.PaddingBottom = 4;

            UIScrollViewer2 sv = new UIScrollViewer2();
            sv.Width.Precent = 1;
            sv.Height.Precent = 1;

            List<Terraria.UI.UIElement> list = PixelArt.GetUI();

            foreach (Terraria.UI.UIElement ui in list)
            {
                sv.AddChild(ui);
            }

            Children.Append(sv);
        }
    }
}
