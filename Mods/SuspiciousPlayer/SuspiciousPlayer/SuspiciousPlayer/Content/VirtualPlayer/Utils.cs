using Terraria;

namespace SuspiciousPlayer.Content.VirtualPlayer
{
    public partial class Utils
    {
        public static Item[] getPlayerArmor(Player player, int start, int len)
        {
            if (player == null) return null;

            Item[] items = new Item[len];
            for (int i = 0; i < len; ++i) items[i] = player.armor[start + i];

            return items;
        }
        public static Item[] getArmor(Player player)
        {
            return getPlayerArmor(player, 0, 3);
        }
        public static Item[] getArmorVanity(Player player)
        {
            return getPlayerArmor(player, 3 + 7, 3);
        }
        public static Item[] getAccessories(Player player)
        {
            return getPlayerArmor(player, 3, 7);
        }
        public static Item[] getAccessoriesVanity(Player player)
        {
            return getPlayerArmor(player, 3 + 7 + 3, 7);
        }
    }
}
