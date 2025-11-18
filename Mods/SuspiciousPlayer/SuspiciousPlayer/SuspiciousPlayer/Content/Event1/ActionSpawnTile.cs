using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using tContentPatch;
using Terraria;
using Terraria.ID;

namespace SuspiciousPlayer.Content.Event1
{
    internal class ActionSpawnTile : PatchMain
    {
        public static ActionState state = new ActionState(run);
        public static int width = 100;
        public static int height = 100;
        public static int spawnX = 0;
        public static int spawnY = 0;
        private static int x = 0;
        private static int y = 0;
        private static List<Point> poss = new List<Point>();

        public static void run()
        {
            poss.Clear();
            x = 0;
            y = 0;
            spawnX = (int)((Event.EventPos.X / 16) - width / 2);
            spawnY = (int)((Event.EventPos.Y / 16) - height / 2);
        }

        public override void DoUpdateInWorldPrefix(Stopwatch sw)
        {
            if (state.norun) return;

            if (x < width) ++x;
            else if (y < height) ++y;
            else
            {
                Event.SetEventState(Event.EventState_SpawnLunarTower);
                return;
            }

            PlaceTile(spawnX + x, spawnY + y);
            PlaceTile(spawnX + width - x, spawnY + height - y);
        }

        public static void ClearTile()
        {
            foreach (Point pos in poss)
            {
                Tile tile = Main.tile[pos.X, pos.Y];
                if (tile?.active() == false) continue;
                if (tile.type != TileID.LihzahrdBrick) continue;

                WorldGen.KillTile(pos.X, pos.Y, noItem: true);
                if (Main.netMode == 2) NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 4, pos.X, pos.Y);
            }

            poss.Clear();
        }

        public static void PlaceTile(int x, int y)
        {
            if (WorldGen.InWorld(x, y) == false) return;

            Tile tile = Main.tile[x, y];
            if (tile == null) return;
            if (tile.active()) return;

            WorldGen.PlaceTile(x, y, TileID.LihzahrdBrick, true);
            if (Main.netMode == 2) NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 1, x, y, tile.type);

            poss.Add(new Point(x, y));
        }

        public static bool InTile(Vector2 pos)
        {
            if (pos.X < (spawnX - 2) * 16) return false;
            if (pos.Y < (spawnY - 2) * 16) return false;
            if (pos.X > (spawnX + width + 2) * 16) return false;
            if (pos.Y > (spawnY + height + 2) * 16) return false;
            return true;
        }
    }
}
