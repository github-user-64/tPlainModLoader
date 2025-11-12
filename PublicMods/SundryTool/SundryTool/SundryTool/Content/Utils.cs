using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace SundryTool.Content
{
    public partial class Utils
    {
        public static Vector2 aimAdvance(Vector2 position, float velocity, Vector2 targetP, Vector2 targetV)//预瞄
        {
            Vector2 targetP2 = targetP;
            float len = 0;
            float lenT = Math.Abs(position.Length() - targetP2.Length());

            for (int i = 0; i < 120; ++i)
            {
                if (len < lenT)
                {
                    len += velocity;

                    targetP2 += targetV;

                    lenT = Math.Abs(position.Length() - targetP2.Length());
                }
                else if (len > lenT - 5)
                {
                    if (i == 0) return targetP2;

                    Vector2 targetP2_old = targetP2 -= targetV;
                    float len_old = Math.Abs(len - velocity);
                    float lenT_old = Math.Abs(position.Length() - targetP2_old.Length());
                    float gap = Math.Abs(len - lenT);
                    float gap_old = Math.Abs(len_old - lenT_old);

                    return gap < gap_old ? targetP2 : targetP2_old;
                }
                else
                {
                    return targetP2;
                }
            }

            return targetP2;
        }

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

        public static void OutputPlayerName(int i)
        {
            string msg = null;

            if (i < 0 || i >= Main.player.Length)
            {
                msg = $"[{i}] no player";
                Console.WriteLine(msg);
                Main.NewText(msg);
                return;
            }

            Player player = Main.player[i];
            msg = $"[{player.name}]";
            if (player.active == false) msg += " no active";
            Console.WriteLine(msg);
            Main.NewText(msg);
        }

        public static int getNearbyChestIndex(Vector2 position)
        {
            if (position.HasNaNs()) return -1;

            int chest_i = -1;
            float chest_d = 0;

            for (int i = 0; i < Main.chest.Length; ++i)
            {
                Chest chest = Main.chest[i];
                if (chest == null) continue;

                float d = Vector2.Distance(new Vector2(chest.x, chest.y) * 16, position);
                if (d > 16 * 10) continue;

                if (chest_i != -1 && chest_d <= d) continue;

                chest_i = i;
                chest_d = d;
            }

            return chest_i;
        }

        public static int getRand(int v, int v2)//v=1,v2=2,return 1
        {
            return Main.rand.Next(Math.Min(v, v2), Math.Max(v, v2));
        }
    }
}
