using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent;

namespace PixelArt.Content
{
    internal class DrawUtils
    {
        public static Point GetWordPoint(Vector2 pWord)
        {
            return new Point((int)Math.Floor(pWord.X / 16), (int)Math.Floor(pWord.Y / 16));
        }

        public static void Draw_rectangle(Vector2 startWord, Vector2 endWord, Color borderColor, Color backgroundColor)
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
