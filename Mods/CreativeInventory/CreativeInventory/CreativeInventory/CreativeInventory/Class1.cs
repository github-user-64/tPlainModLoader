using System;

namespace CreativeInventory.CreativeInventory
{
    public static class UIElement_Utils
    {
        public static void Width_Stretch(this Terraria.UI.UIElement ui)
        {
            if (ui.Parent == null) return;

            ui.Width.Set(ui.Parent.GetDimensions().Width
                - ui.Parent.PaddingLeft - ui.PaddingRight
                - ui.MarginLeft - ui.MarginRight, 0);
        }

        public static void Height_Stretch(this Terraria.UI.UIElement ui)
        {
            if (ui.Parent == null) return;

            ui.Height.Set(ui.Parent.GetInnerDimensions().Height
                //- ui.Parent.PaddingTop - ui.Parent.PaddingBottom
                - ui.MarginTop - ui.MarginBottom, 0);
        }

        public static void UpdateSize_Width(this Terraria.UI.UIElement ui)
        {
            float width = 0;

            foreach (Terraria.UI.UIElement element in ui.Children)
            {
                float ew = Math.Max(element.Width.Pixels, element.GetDimensions().Width);

                width = Math.Max(width,
                    ui.PaddingLeft
                    + element.Left.Pixels + element.MarginLeft + ew + element.MarginRight
                    + element.PaddingRight);
            }

            ui.Width.Set(width, 0);
        }

        public static void UpdateSize_Height(this Terraria.UI.UIElement ui)
        {
            float height = 0;

            foreach (Terraria.UI.UIElement element in ui.Children)
            {
                float eh = Math.Max(element.Height.Pixels, element.GetDimensions().Height);

                height = Math.Max(height,
                    ui.PaddingTop
                    + element.Top.Pixels + element.MarginTop + eh + element.MarginBottom
                    + ui.PaddingBottom);
            }

            ui.Height.Set(height, 0);
        }
    }
}
