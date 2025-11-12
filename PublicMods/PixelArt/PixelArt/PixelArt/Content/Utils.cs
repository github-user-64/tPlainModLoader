using Terraria;
using Terraria.ID;

namespace PixelArt.Content
{
    public class Utils
    {
        public static void updateData_placeWall(int x, int y)
        {
            if (WorldGen.InWorld(x, y) == false) return;

            Tile tile = Main.tile[x, y];
            if (tile == null) return;

            if (tile.wall > 0)
            {
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 3, x, y, tile.wall);
            }
        }
    }
}
