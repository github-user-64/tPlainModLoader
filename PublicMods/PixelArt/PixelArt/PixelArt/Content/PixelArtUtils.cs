using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;

namespace PixelArt.Content
{
    public partial class PixelArt
    {
        //获取两个颜色的 RGB 分量差之和
        public static int ColorDifference(Color color1, Color color2)
        {
            int rDiff = Math.Abs(color1.R - color2.R);
            int gDiff = Math.Abs(color1.G - color2.G);
            int bDiff = Math.Abs(color1.B - color2.B);
            return rDiff + gDiff + bDiff;
        }

        private static List<PixelInfo> LoadImgToPixelInfo(string filePath, ref int width, ref int height)
        {
            if (File.Exists(filePath) == false)
            {
                throw new Exception($"文件[{filePath}]不存在");
            }

            List<PixelInfo> pixelInfos = new List<PixelInfo>();

            using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(filePath))
            {
                width = bitmap.Width;
                height = bitmap.Height;

                for (int y = 0; y < height; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        System.Drawing.Color c = bitmap.GetPixel(x, y);
                        if (c.A != 255) continue;
                        Color color = new Color(c.R, c.G, c.B, c.A);

                        pixelInfos.Add(new PixelInfo(color, x, y));
                    }
                }
            }
            
            return pixelInfos;
        }

        private static Item LookupColorSimilarWallItem(Color color)
        {
            int difference = -1;
            Item item = null;

            for (int i = 1; i < wallItemIds.Count; ++i)
            {
                int createWall = wallItemIds[i].createWall;

                ushort wallId = Terraria.Map.MapHelper.wallLookup[createWall];
                Color mapColor = MapHelper.colorLookup[wallId];
                if (mapColor.A != 255) continue;

                int s = ColorDifference(color, mapColor);
                if (s == 0)
                {
                    return wallItemIds[i];
                }
                if (difference == -1)
                {
                    difference = s;
                    item = wallItemIds[i];
                    continue;
                }
                if (difference > s)
                {
                    difference = s;
                    item = wallItemIds[i];
                }
            }

            return item;
        }
    }
}
