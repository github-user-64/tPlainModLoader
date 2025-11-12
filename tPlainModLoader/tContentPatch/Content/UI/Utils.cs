using System;
using Terraria.UI;

namespace tContentPatch.Content.UI
{
    public static class Utils
    {
        /// <summary>
        /// 将容器的大小设为子集的最大占位
        /// </summary>
        /// <param name="uie"></param>
        public static void UpdateContainer_Height(this UIElement uie)
        {
            float max = 0;

            foreach (UIElement element in uie.Children)
            {
                float v = element.Top.GetValue(uie.GetInnerDimensions().Height);
                float size = Math.Max(element.GetOuterDimensions().Height, element.Height.Pixels + element.MarginTop + element.MarginBottom);

                max = Math.Max(max, v + size);
            }

            uie.Height.Set(max, 0);
            uie.Recalculate();
        }

        /// <summary>
        /// 将容器的大小设为子集的最大占位
        /// </summary>
        /// <param name="uie"></param>
        public static void UpdateContainer_Width(this UIElement uie)
        {
            float max = 0;

            foreach (UIElement element in uie.Children)
            {
                float v = element.Left.GetValue(uie.GetInnerDimensions().Width);
                float size = Math.Max(element.GetOuterDimensions().Width, element.Width.Pixels + element.MarginLeft + element.MarginRight);

                max = Math.Max(max, v + size);
            }

            uie.Width.Set(max, 0);
            uie.Recalculate();
        }
    }
}
