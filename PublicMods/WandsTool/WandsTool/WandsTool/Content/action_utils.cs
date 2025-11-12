using Terraria;
using Terraria.ID;

namespace WandsTool.Content
{
    public partial class action
    {
        public static void updateData_placeTile(int x, int y)
        {
            if (WorldGen.InWorld(x, y) == false) return;

            Tile tile = Main.tile[x, y];
            if (tile == null) return;

            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 1, x, y, tile.type);

            if (tile.halfBrick())
                NetMessage.SendData(17, -1, -1, null, 7, x, y, 1f, 0, 0, 0);
            else if (tile.slope() > 0)
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 14, x, y, tile.slope());
        }

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
