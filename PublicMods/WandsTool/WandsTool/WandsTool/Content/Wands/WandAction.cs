using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.ID;

namespace WandsTool.Content
{
    public class WandAction
    {
        public enum BlockType
        {
            /// <summary>
            /// 固体
            /// </summary>
            Solid,
            /// <summary>
            /// 半砖
            /// </summary>
            HalfBlock,
            SlopeUpLeft,
            SlopeUpRight,
            SlopeDownLeft,
            SlopeDownRight,
        }

        protected struct tile
        {
            public tile(int x, int y, bool isTile, bool isWall, BlockType bt)
            {
                this.x = x;
                this.y = y;
                this.isTile = isTile;
                this.isWall = isWall;
                this.bt = bt;
                toolMode = 0;
            }

            public tile(int x, int y, Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode toolMode)
            {
                this.x = x;
                this.y = y;
                this.isTile = false;
                this.isWall = false;
                this.bt = 0;
                this.toolMode = toolMode;
            }

            public int x;
            public int y;
            public bool isTile;
            public bool isWall;
            public BlockType bt;
            public Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode toolMode;
        }

        protected static List<tile> tilePlace = null;
        protected static List<tile> tileKill = null;
        protected static List<tile> wirePlace = null;
        protected static List<tile> wireKill = null;
        protected static List<Projectile> wireLinePlaceAndKill = null;
        public static int Count { get; protected set; } = 0;


        public static void Update(int updateCount = -1)
        {
            if (gameMain.Wand_UpdateCount < 1) gameMain.Wand_UpdateCount = 1;
            if (updateCount < 1) updateCount = gameMain.Wand_UpdateCount;

            if (tilePlace == null) tilePlace = new List<tile>();
            if (tileKill == null) tileKill = new List<tile>();
            if (wirePlace == null) wirePlace = new List<tile>();
            if (wireKill == null) wireKill = new List<tile>();
            if (wireLinePlaceAndKill == null) wireLinePlaceAndKill = new List<Projectile>();

            if (tilePlace.Count > 0)
            {
                Terraria.Player player = Main.LocalPlayer;
                if (player != null)
                {
                    if (tilePlace[0].isTile) placeTile(tilePlace[0], player);
                    if (tilePlace[0].isWall) placeWall(tilePlace[0], player);
                }

                tilePlace.RemoveAt(0);
            }
            
            if (tileKill.Count > 0)
            {
                killTile(tileKill[0]);

                tileKill.RemoveAt(0);
            }

            if (wirePlace.Count > 0)
            {
                placeWire(wirePlace[0]);

                wirePlace.RemoveAt(0);
            }

            if (wireKill.Count > 0)
            {
                killWire(wireKill[0]);

                wireKill.RemoveAt(0);
            }

            if (wireLinePlaceAndKill.Count > 0)
            {
                Player player = Main.LocalPlayer;

                if (player != null)
                {
                    wireLineAction(wireLinePlaceAndKill[0], player);
                }

                wireLinePlaceAndKill.RemoveAt(0);
            }

            Count = tilePlace.Count + tileKill.Count + wirePlace.Count + wireKill.Count + wireLinePlaceAndKill.Count;

            if (--updateCount > 0) Update(updateCount);
        }

        private static void placeTile(tile t, Player player)
        {
            if (canTile(t) == false) return;

            Tile tile = Main.tile[t.x, t.y];
            Item item = FirstItem_Tile(player);

            if (item == null) return;

            if (tile?.active() == true)//有方块
            {
                if (tile.type == item.createTile)//相同方块
                {
                    SetSlopeFor(t.x, t.y, t.bt);
                    return;
                }

                killTile(new tile(t.x, t.y, true, false, default));
                bool v = WorldGen.PlaceTile(t.x, t.y, item.createTile, true, true, player.whoAmI, item.placeStyle);

                if (v) SetSlopeFor(t.x, t.y, t.bt);
            }
            else
            {
                bool v = WorldGen.PlaceTile(t.x, t.y, item.createTile, true, true, player.whoAmI, item.placeStyle);
                if (v) SetSlopeFor(t.x, t.y, t.bt);
            }

            action.updateData_placeTile(t.x, t.y);
        }
        private static void placeWall(tile t, Player player)
        {
            if (canTile(t) == false) return;

            Tile tile = Main.tile[t.x, t.y];
            Item item = FirstItem_Wall(player);

            if (item == null) return;
            if (item.createWall == tile.wall) return;

            if (tile?.wall > 0)
            {
                WorldGen.KillWall(t.x, t.y);
            }

            WorldGen.PlaceWall(t.x, t.y, item.createWall);
            WorldGen.SquareWallFrame(t.x, t.y, false);

            action.updateData_placeWall(t.x, t.y);
        }

        private static void killTile(tile t)
        {
            if (canTile(t) == false) return;

            Tile tile = Main.tile[t.x, t.y];

            if (t.isTile && tile?.type >= 0)
            {
                WorldGen.KillTile(t.x, t.y);
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 4, t.x, t.y);
            }
            if (t.isWall && tile?.wall > 0)
            {
                WorldGen.KillWall(t.x, t.y);
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 2, t.x, t.y);
            }
        }

        public static void SetSlopeFor(int x, int y, BlockType bt)
        {
            Tile tile = Main.tile[x, y];
            if (WorldGen.CanPoundTile(x, y))//能否被锤击
            {
                switch (bt)
                {
                    case BlockType.Solid: WorldGen.SlopeTile(x, y, 0); break;
                    case BlockType.SlopeDownLeft: WorldGen.SlopeTile(x, y, 1); break;
                    case BlockType.SlopeDownRight: WorldGen.SlopeTile(x, y, 2); break;
                    case BlockType.SlopeUpLeft: WorldGen.SlopeTile(x, y, 3); break;
                    case BlockType.SlopeUpRight: WorldGen.SlopeTile(x, y, 4); break;
                    case BlockType.HalfBlock: tile?.halfBrick(true); break;
                    default: break;
                }

                WorldGen.SquareTileFrame(x, y, false);
            }
        }

        private static void placeWire(tile t)
        {
            if (canTile(t) == false) return;

            if (t.toolMode.HasFlag(Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode.Red))
            {
                WorldGen.PlaceWire(t.x, t.y);
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 5, t.x, t.y);
            }
            if (t.toolMode.HasFlag(Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode.Green))
            {
                WorldGen.PlaceWire3(t.x, t.y);
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 12, t.x, t.y);
            }
            if (t.toolMode.HasFlag(Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode.Blue))
            {
                WorldGen.PlaceWire2(t.x, t.y);
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 10, t.x, t.y);
            }
            if (t.toolMode.HasFlag(Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode.Yellow))
            {
                WorldGen.PlaceWire4(t.x, t.y);
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 16, t.x, t.y);
            }
            if (t.toolMode.HasFlag(Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode.Actuator))
            {
                WorldGen.PlaceActuator(t.x, t.y);
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 8, t.x, t.y);
            }
        }

        private static void killWire(tile t)
        {
            if (canTile(t) == false) return;

            if (t.toolMode.HasFlag(Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode.Red))
            {
                WorldGen.KillWire(t.x, t.y);
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 6, t.x, t.y);
            }
            if (t.toolMode.HasFlag(Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode.Green))
            {
                WorldGen.KillWire3(t.x, t.y);
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 13, t.x, t.y);
            }
            if (t.toolMode.HasFlag(Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode.Blue))
            {
                WorldGen.KillWire2(t.x, t.y);
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 11, t.x, t.y);
            }
            if (t.toolMode.HasFlag(Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode.Yellow))
            {
                WorldGen.KillWire4(t.x, t.y);
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 17, t.x, t.y);
            }
            if (t.toolMode.HasFlag(Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode.Actuator))
            {
                WorldGen.KillActuator(t.x, t.y);
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 9, t.x, t.y);
            }
        }

        private static void wireLineAction(Projectile proj, Player player)
        {
            Point ps = new Vector2(proj.ai[0], proj.ai[1]).ToPoint();
            Point pe = proj.Center.ToTileCoordinates();

            if (Main.netMode == 1)
            {
                NetMessage.SendData(109, -1, -1, null, ps.X, ps.Y, pe.X, pe.Y, (int)WiresUI.Settings.ToolMode);
            }
            else
            {
                Wiring.MassWireOperation(ps, pe, player);
            }
        }

        public static void AddTile(List<Point> tile, bool isTile, bool isWall, BlockType bt)
        {
            for (int i = 0; i < tile.Count; ++i)
            {
                tilePlace.Add(new tile(tile[i].X, tile[i].Y, isTile, isWall, bt));
            }
        }

        public static void DelTile(List<Point> tile, bool isTile, bool isWall)
        {
            bool hasNull = false;
            Vector2 oldP = Vector2.Zero;
            Vector2 newP = Vector2.Zero;

            for (int i = 0; i < tile.Count; ++i)
            {
                tile t = new tile(tile[i].X, tile[i].Y, isTile, isWall, BlockType.Solid);

                if (canTile(t) == false) continue;

                Tile T = Main.tile[t.x, t.y];

                if (T == null)
                {
                    if (!hasNull)
                    {
                        hasNull = true;
                        oldP = new Vector2(t.x, t.y);
                    }
                    newP = new Vector2(t.x, t.y);
                    continue;
                }

                if ((!isTile || !T.active()) &&
                    (!isWall || !(T.wall > 0))) continue;

                tileKill.Add(t);
            }

            if (hasNull)
            {
                Main.Pings.Add(oldP);
                Main.Pings.Add(newP);
                Main.NewText("部分方块未加载, 靠近未加载区域来加载(大致范围已在地图上标记)");
            }
        }

        public static void AddWire(List<Point> wire, Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode toolMode)
        {
            for (int i = 0; i < wire.Count; ++i)
            {
                wirePlace.Add(new tile(wire[i].X, wire[i].Y, toolMode));
            }
        }

        public static void DelWire(List<Point> wire, Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode toolMode)
        {
            for (int i = 0; i < wire.Count; ++i)
            {
                tile t = new tile(wire[i].X, wire[i].Y, toolMode);

                if (canTile(t) == false) continue;

                Tile T = Main.tile[t.x, t.y];

                if (!(T.wire() && toolMode.HasFlag(Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode.Red)) &&
                    !(T.wire3() && toolMode.HasFlag(Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode.Green)) &&
                    !(T.wire2() && toolMode.HasFlag(Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode.Blue)) &&
                    !(T.wire4() && toolMode.HasFlag(Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode.Yellow)) &&
                    !(T.actuator() && toolMode.HasFlag(Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode.Actuator))) continue;

                wireKill.Add(t);
            }
        }

        public static void AddWireLine(Vector2 ps, Vector2 pe, Terraria.GameContent.UI.WiresUI.Settings.MultiToolMode toolMode)
        {
            if (ps.HasNaNs() || pe.HasNaNs()) return;

            Vector2 v = ps;
            ps = new Vector2(Math.Min(ps.X, pe.X), Math.Min(ps.Y, pe.Y));
            pe = new Vector2(Math.Max(v.X, pe.X), Math.Max(v.Y, pe.Y));

            Point psp = ps.ToTileCoordinates();
            Point pep = pe.ToTileCoordinates();

            if (canTile(new tile() { x = psp.X, y = psp.Y }) == false) return;
            if (canTile(new tile() { x = pep.X, y = pep.Y }) == false) return;

            int height = pep.Y - psp.Y + 1;

            for (int i = 0; i < height; ++i)
            {
                Projectile proj = new Projectile();

                proj.Center = new Vector2(ps.X, ps.Y + i * 16);
                proj.ai = new float[3];
                proj.ai[0] = pep.X;
                proj.ai[1] = psp.Y + i;

                wireLinePlaceAndKill.Add(proj);
            }

            WiresUI.Settings.ToolMode = toolMode;
        }

        private static bool canTile(tile t)
        {
            return t.x >= 0 && t.x < Main.tile.GetLength(0) && t.y >= 0 && t.y < Main.tile.GetLength(1);
        }

        private static Item FirstItem_Tile(Player player)
        {
            return FirstItem_TileOrWall(player, true);
        }

        private static Item FirstItem_Wall(Player player)
        {
            return FirstItem_TileOrWall(player, false);
        }

        private static Item FirstItem_TileOrWall(Player player, bool isTile)
        {
            if (player?.inventory == null) return null;

            if (isTile)
            {
                if (player.inventory[player.selectedItem]?.createTile >= 0) return player.HeldItem;
            }
            else
            {
                if (player.inventory[player.selectedItem]?.createWall > 0) return player.HeldItem;
            }

            for (int i = 0; i < player.inventory.Length; ++i)
            {
                if (isTile)
                {
                    if (player.inventory[i]?.createTile >= 0) return player.inventory[i];
                }
                else
                {
                    if (player.inventory[i]?.createWall > 0) return player.inventory[i];
                }
            }

            return null;
        }
    }
}
