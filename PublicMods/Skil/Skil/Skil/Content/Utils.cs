using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace Skil.Content
{
    public partial class Utils
    {
        public class CDTime
        {
            protected int cd_max = 0;
            protected int cd = 0;
            public bool Ok { get; protected set; } = false;

            public CDTime(int cd_max)
            {
                this.cd_max = cd_max;
                resume();
            }

            public void next()
            {
                if (cd > cd_max) cd = cd_max;
                if (cd > 0)
                {
                    Ok = false;
                    cd -= (int)(1000 / 60f);
                    return;
                }

                cd = cd_max;
                Ok = true;
            }

            public void resume()
            {
                Ok = false;
                cd = cd_max;
            }

            public void setCD(int cd_max)
            {
                this.cd_max = cd_max;
            }
        }

        public static int getRand(int v, int v2)//v=1,v2=2,return 1
        {
            return Terraria.Main.rand.Next(Math.Min(v, v2), Math.Max(v, v2));
        }

        public static float getRandFloat()//0.9-0.1
        {
            return Terraria.Main.rand.NextFloat();
        }

        /// <summary>
        /// cdTime(true:cd结束, false:还在cd)
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="cdMax"></param>
        /// <returns></returns>
        public static bool cdTime(ref int cd, int cdMax)
        {
            if (cd > 0)
            {
                cd -= (int)(1000 / 60f);
                return false;
            }
            cd = cdMax;
            return true;
        }

        public static Item getSelectedItem(Player player)
        {
            if (player == null || player.inventory == null) return null;
            if (0 > player.selectedItem || player.selectedItem >= player.inventory.Length) return null;

            return player.inventory[player.selectedItem];
        }

        public static Player getPlayer(int index)
        {
            if (Terraria.Main.player == null) return null;
            if (0 > index || index > Terraria.Main.player.Length - 1) return null;

            return Terraria.Main.player[index];
        }

        public static Player getPlayer_cd(int index, ref int cd, int cdMax)
        {
            if (cdTime(ref cd, cdMax) == false) return null;

            return getPlayer(index);
        }

        public static Player getPlayer_hostile(Vector2 position, Player player = null)//player为排除的玩家
        {
            Player targetPlayer = null;

            for (int i = 0; i < Main.player?.Length; ++i)
            {
                Player p = Main.player[i];

                if (p == null) continue;
                if (p.active == false) continue;
                if (p.hostile == false) continue;
                if (p.dead == true) continue;
                if (player != null && p.whoAmI == player.whoAmI) continue;

                if (targetPlayer != null)
                {
                    if (position.Distance(p.position) > position.Distance(targetPlayer.position)) continue;
                }

                targetPlayer = p;
            }

            return targetPlayer;
        }

        public static NPC getNpc(Vector2 position, int width, int height, int distanceMax, bool Sight = true)//获取敌怪npc, Sight: 需要有视线
        {
            NPC target = null;
            float distance = 0;

            for (int i = 0; i < Terraria.Main.npc?.Length; ++i)
            {
                NPC npc = Terraria.Main.npc[i];
                if (npc == null) continue;
                if (npc.active == false) continue;
                if (npc.CanBeChasedBy() == false) continue;//是敌怪

                float d = position.Distance(npc.position);

                if (d > distanceMax) continue;
                if (target != null && d > distance) continue;

                if (Sight &&
                    Collision.CanHit(position, width, height,
                    npc.position, npc.width, npc.height) == false) continue;

                distance = d;
                target = npc;
            }

            return target;
        }

        public static bool projExist(Projectile p, int type, Player player)//射弹是否还在
        {
            if (p == null || player == null) return false;
            if (Terraria.Main.projectile == null) return false;
            if (Array.IndexOf(Terraria.Main.projectile, p) == -1) return false;
            if (p.type != type) return false;
            if (p.active == false) return false;
            if (p.owner != player.whoAmI) return false;

            return true;
        }

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
    }
}
