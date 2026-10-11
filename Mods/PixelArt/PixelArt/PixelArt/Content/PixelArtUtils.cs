using Microsoft.Xna.Framework;
using PixelArt.Content.ColorToWall;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Terraria;
using Terraria.ID;
using Terraria.Map;

namespace PixelArt.Content
{
    public partial class PixelArt
    {
        private static List<PixelInfo> LoadImgToPixelInfo(string filePath, ref int width, ref int height, IGetColorWall gcw, CancellationToken ct)
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

                        PixelInfo pi = null;

                        if (c.A == byte.MaxValue)
                        {
                            Color color = new Color(c.R, c.G, c.B, c.A);
                            pi = new PixelInfo(color, x, y);
                        }

                        pixelInfos.Add(pi);
                    }
                }
            }

            for (int i = 0; i < pixelInfos.Count; ++i)
            {
                ct.ThrowIfCancellationRequested();

                if (pixelInfos[i] == null) continue;

                ColorWall cw = gcw.Get(pixelInfos[i].color);

                pixelInfos[i].item = cw.item;
                pixelInfos[i].wall = cw.wall;
                pixelInfos[i].paint = cw.paint;
            }

            return pixelInfos;
        }

        private static List<ColorWall> ItemToColorWall(List<Item> items, bool usePaint)
        {
            List<ColorWall> cw = new List<ColorWall>();
            if (items == null) return cw;

            foreach (Item i in items)
            {
                if (i == null) continue;

                ushort type = Terraria.Map.MapHelper.wallLookup[i.createWall];//应该不是tile也不是wall, 而是都有

                MapTile mt = new MapTile();
                mt.Light = byte.MaxValue;
                mt.Type = type;

                int count = usePaint ? PaintID.Old_IlluminantPaint + 1 : 1;

                for (byte j = PaintID.None; j < count; j++)
                {
                    mt.Color = j;

                    Color mapColor = Terraria.Map.MapHelper.GetMapTileXnaColor(mt, 0, 0);//i,j参数暂时没影响
                    if (mapColor.A != byte.MaxValue) continue;

                    cw.Add(new ColorWall(mapColor, i.type, (ushort)i.createWall, j));
                }
            }

            return cw;
        }
    }
}
