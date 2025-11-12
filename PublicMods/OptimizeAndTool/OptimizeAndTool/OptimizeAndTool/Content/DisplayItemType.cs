using System;
using tContentPatch;

namespace OptimizeAndTool.Content
{
    internal class DisplayItemType : Mod
    {
        private int r = 85;
        private int g = 85 * 2;
        private int b = 85 * 3;

        public override void Load()
        {
            ItemToolTipAdditional.ItemInfo.Add(item =>
            {
                if (item == null) return null;

                int t255 = 255 * 2;

                string R = Convert.ToString(r > 255 ? t255 - r : r, 16).PadLeft(2, '0');
                string G = Convert.ToString(g > 255 ? t255 - g : g, 16).PadLeft(2, '0');
                string B = Convert.ToString(b > 255 ? t255 - b : b, 16).PadLeft(2, '0');

                int add = 255 / 60;

                r += add;
                g += add;
                b += add;
                if (r > t255) r -= t255;
                if (g > t255) g -= t255;
                if (b > t255) b -= t255;

                return new string[] { $"物品ID: [c/{R}{G}{B}:{item.type}]" };
            });
        }
    }
}
