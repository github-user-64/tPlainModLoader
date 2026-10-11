using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace PixelArt.Content.Utils
{
    internal static class TileUtils
    {
        private static bool ActionSetItem(Player player, int item, bool setItem, Func<bool> func)
        {
            int slot = PlayerItemSlotID.Inventory0 + player.selectedItem;
            if (setItem) ClientSendToServer.SendSyncEquipment(item, slot, player.whoAmI);

            bool rv = func();

            if (setItem) NetMessage.TrySendData(MessageID.SyncEquipment, -1, -1, null, player.whoAmI, slot);

            return rv;
        }

        public static bool PlaceWall(int x, int y, ushort wall, int item, Player player, bool setItem)
        {
            if (WorldGen.InWorld(x, y) == false) return false;
            if (wall == WallID.None) return false;

            Tile tile = Main.tile[x, y];
            bool replace = false;
            if (tile?.wall > WallID.None)
            {
                if (tile.wall == wall) return false;
                replace = true;
            }

            return ActionSetItem(player, item, setItem, () =>
            {
                if (replace)
                {
                    WorldGen.ReplaceWall(x, y, wall);
                    if (Main.netMode == 1) NetMessage.TrySendData(MessageID.TileManipulation, -1, -1, null, 22, x, y, wall);
                }
                else
                {
                    WorldGen.PlaceWall(x, y, wall, true);
                    if (Main.netMode == 1) NetMessage.TrySendData(MessageID.TileManipulation, -1, -1, null, 3, x, y, wall);
                }

                return true;
            });
        }

        public static bool PaintWall(int x, int y, byte paint, Player player, bool setItem)
        {
            if (WorldGen.InWorld(x, y) == false) return false;
            if (paint == PaintID.None) return false;

            Tile tile = Main.tile[x, y];
            if (tile?.wallColor() == paint) return false;

            return ActionSetItem(player, ItemID.PaintRoller, setItem, () =>
            {
                if (Main.netMode == 1) ClientSendToServer.SendPlayerControls(player.whoAmI, new Point(x, y).ToWorldCoordinates());

                WorldGen.paintWall(x, y, paint, true, false);

                return true;
            });
        }
    }
}
