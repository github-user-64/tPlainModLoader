using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;

namespace WandsTool.Content
{
    public class WandUtils
    {
        public static Point GetWordPoint(Vector2 pWord)
        {
            return new Point((int)Math.Floor(pWord.X / 16), (int)Math.Floor(pWord.Y / 16));
        }

        public static List<Point> GetShapes_line(Vector2 startWord, Vector2 endWord)
        {
            if (startWord.HasNaNs() || endWord.HasNaNs()) return null;
            if (startWord == endWord) return new List<Point>() { GetWordPoint(startWord) };

            List<Point> points = new List<Point>();
            Vector2 p = startWord;
            Vector2 v = Vector2.Normalize(endWord - startWord) * 16;
            float distance = startWord.Distance(endWord);
            Point? oldPoint = null;

            while (true)
            {
                Point point = GetWordPoint(p);

                if (point != oldPoint)
                {
                    points.Add(point);
                    oldPoint = point;
                }

                if (startWord.Distance(p) >= distance) break;

                p += v;
                if (startWord.Distance(p) > distance) p = endWord;
            }

            return points;
        }

        public static List<Point> GetShapes_Circular(Vector2 startWord, Vector2 endWord)
        {
            if (startWord.HasNaNs() || endWord.HasNaNs()) return null;
            if (startWord == endWord) return new List<Point>() { GetWordPoint(startWord) };

            List<Point> points = new List<Point>();
            Vector2? oldV = null;

            float A = Math.Abs(startWord.X - endWord.X);//横轴
            float B = Math.Abs(startWord.Y - endWord.Y);//竖轴
            //float l;
            //if (A > B)
            //{
            //    l = 2 * MathHelper.Pi * B + 4 * (A - B);
            //}
            //else
            //{
            //    l = 2 * MathHelper.Pi * A + 4 * (B - A);
            //}
            //int count = (int)(l / 16);
            //Main.NewText(count.ToString());
            //for (int i = 0; i < count; ++i)
            for (int i = 0; i < 361; ++i)
            {
                float dCA = (MathHelper.TwoPi / 360) * i;//弧度
                float R = A * B / (float)Math.Sqrt(Math.Pow(A * Math.Sin(dCA), 2) + Math.Pow(B * Math.Cos(dCA), 2)); //计算对应角度的半径
                float x = R * (float)Math.Cos(dCA);
                float y = R * (float)Math.Sin(dCA);

                if (oldV == null) oldV = new Vector2(x, y);

                List<Point> ps = GetShapes_line(startWord + oldV.Value, startWord + new Vector2(x, y));

                oldV = new Vector2(x, y);

                for (int i2 = 0; i2 < ps?.Count; ++i2)
                {
                    if (points.Contains(ps[i2]) == true) continue;

                    points.Add(ps[i2]);
                }
            }

            return points;
        }

        public static List<Point> GetShapes_Rectangle(Vector2 startWord, Vector2 endWord)
        {
            if (startWord.HasNaNs() || endWord.HasNaNs()) return null;
            if (startWord == endWord) return new List<Point>() { GetWordPoint(startWord) };

            Vector2 _v = startWord;
            startWord = new Vector2(Math.Min(startWord.X, endWord.X), Math.Min(startWord.Y, endWord.Y));
            endWord = new Vector2(Math.Max(_v.X, endWord.X), Math.Max(_v.Y, endWord.Y));

            List<Point> points = new List<Point>();
            Point start = GetWordPoint(startWord);
            Point end = GetWordPoint(endWord);

            int rectW = end.X - start.X + 1;
            int rectH = end.Y - start.Y + 1;

            for (int y = 0; y < rectH; ++y)
            {
                for (int x = 0; x < rectW; ++x)
                {
                    points.Add(new Point(start.X + x, start.Y + y));
                }
            }

            return points;
        }

        protected static void DrawShapes(List<Point> shapes, Vector2 startWord, Vector2 endWord, Color borderColor, Color backgroundColor)
        {
            if (startWord.HasNaNs() || endWord.HasNaNs()) return;

            for (int i = 0; i < shapes?.Count; ++i)
            {
                if (shapes[i] == null) continue;

                Vector2 positionWord = new Vector2(shapes[i].X * 16, shapes[i].Y * 16);

                Rectangle? rect = new Rectangle?(new Rectangle(0, 0, 16, 16));
                Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, positionWord - Main.screenPosition, rect, backgroundColor);

                Point top = shapes[i];
                Point bottom = shapes[i];
                Point left = shapes[i];
                Point right = shapes[i];
                top.Y -= 1;
                bottom.Y += 1;
                left.X -= 1;
                right.X += 1;

                if (shapes.Contains(top) == false)
                {
                    Terraria.Utils.DrawLine(Main.spriteBatch,
                        new Vector2(positionWord.X, positionWord.Y), new Vector2(positionWord.X + 16, positionWord.Y),
                        borderColor, borderColor, 2f);
                }
                if (shapes.Contains(bottom) == false)
                {
                    Terraria.Utils.DrawLine(Main.spriteBatch,
                        new Vector2(positionWord.X, positionWord.Y + 16), new Vector2(positionWord.X + 16, positionWord.Y + 16),
                        borderColor, borderColor, 2f);
                }
                if (shapes.Contains(left) == false)
                {
                    Terraria.Utils.DrawLine(Main.spriteBatch,
                        new Vector2(positionWord.X, positionWord.Y), new Vector2(positionWord.X, positionWord.Y + 16),
                        borderColor, borderColor, 2f);
                }
                if (shapes.Contains(right) == false)
                {
                    Terraria.Utils.DrawLine(Main.spriteBatch,
                        new Vector2(positionWord.X + 16, positionWord.Y), new Vector2(positionWord.X + 16, positionWord.Y + 16),
                        borderColor, borderColor, 2f);
                }
            }
        }
        public static void Draw_line(List<Point> shapes, Vector2 startWord, Vector2 endWord, Color borderColor, Color backgroundColor)
        {
            DrawShapes(shapes, startWord, endWord, borderColor, backgroundColor);
        }

        public static void Draw_circular(List<Point> shapes, Vector2 startWord, Vector2 endWord, Color borderColor, Color backgroundColor)
        {
            DrawShapes(shapes, startWord, endWord, borderColor, backgroundColor);
        }

        public static void Draw_rectangle(List<Point> shapes, Vector2 startWord, Vector2 endWord, Color borderColor, Color backgroundColor)
        {
            if (startWord.HasNaNs() || endWord.HasNaNs()) return;

            Vector2 _v = startWord;
            startWord = new Vector2(Math.Min(startWord.X, endWord.X), Math.Min(startWord.Y, endWord.Y));
            endWord = new Vector2(Math.Max(_v.X, endWord.X), Math.Max(_v.Y, endWord.Y));

            Point startP = GetWordPoint(startWord);
            Point endP = GetWordPoint(endWord);

            Vector2 p1Word = new Vector2(startP.X * 16, startP.Y * 16);
            Vector2 p2Word = new Vector2(endP.X * 16 + 16, startP.Y * 16);
            Vector2 p3Word = new Vector2(startP.X * 16, endP.Y * 16 + 16);
            Vector2 p4Word = new Vector2(endP.X * 16 + 16, endP.Y * 16 + 16);

            Rectangle? rect = new Rectangle?(new Rectangle(0, 0, (int)(p4Word.X - p1Word.X), (int)(p4Word.Y - p1Word.Y)));
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, p1Word - Main.screenPosition, rect, backgroundColor);

            Terraria.Utils.DrawLine(Main.spriteBatch, p1Word, p2Word, borderColor, borderColor, 2f);
            Terraria.Utils.DrawLine(Main.spriteBatch, p3Word, p4Word, borderColor, borderColor, 2f);
            Terraria.Utils.DrawLine(Main.spriteBatch, p1Word, p3Word, borderColor, borderColor, 2f);
            Terraria.Utils.DrawLine(Main.spriteBatch, p2Word, p4Word, borderColor, borderColor, 2f);
        }
    }
}
