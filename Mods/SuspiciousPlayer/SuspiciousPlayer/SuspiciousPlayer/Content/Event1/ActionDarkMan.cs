using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using tContentPatch;
using Terraria;
using Terraria.ID;

namespace SuspiciousPlayer.Content.Event1
{
    /// <summary>
    /// 会爆炸的暗黑法阵
    /// </summary>
    internal class ActionDarkMan : PatchMain
    {
        public static ActionState state = new ActionState(run);
        private static int count = 0;
        private static int countTime = 0;
        private static List<Projectile> ps = new List<Projectile>();

        public static void run()
        {
            Projectile.NewProjectile(null,
                new Vector2(ActionSpawnLunarTower.lunarTower.Center.X, ActionSpawnLunarTower.lunarTower.position.Y), Vector2.Zero,
                ProjectileID.DD2DarkMageHeal, 0, 0);

            int projSize = 64 * 64;
            count = ActionSpawnTile.width * ActionSpawnTile.height * 16 / projSize;
            if (count < 10) count = 10;
            else if (count > 30) count = 30;
            countTime = 30;
            ps.Clear();
        }

        public override void DoUpdateInWorldPrefix(Stopwatch sw)
        {
            if (state.norun) return;

            for (int i = 0; i < ps.Count; i++)
            {
                if (ps[i].active && ps[i].type == ProjectileID.DD2DarkMageRaise) continue;

                Boom(ps[i].Center);

                ps.RemoveAt(i--);
            }

            if (count < 1)
            {
                if (ps.Count < 1) Event.SetEventState(Event.EventState_List);
                return;
            }

            if (countTime-- > 0) return;
            
            Vector2 pos = Vector2.Zero;
            if (count < 7 && Event.player != null)
            {
                pos = Vector2.UnitX * Utils.getRand(130, 130);
                pos = pos.RotatedBy(MathHelper.TwoPi / 6 * count);
                pos += Event.player.Center;
                countTime = 4;
            }
            else
            {
                pos = new Vector2(ActionSpawnTile.spawnX, ActionSpawnTile.spawnY) * 16;
                pos.X += Utils.getRand(50, ActionSpawnTile.width * 16 - 50);
                pos.Y += Utils.getRand(50, ActionSpawnTile.height * 16 - 50);
                countTime = 10;
            }
            
            ps.Add(Main.projectile[Projectile.NewProjectile(null, pos, Vector2.Zero,
                ProjectileID.DD2DarkMageRaise, 0, 0)]);

            --count;
        }

        public void Boom(Vector2 pos)
        {
            Projectile.NewProjectile(null, pos, Vector2.Zero, ProjectileID.TNTBarrel, 200, 1);

            float v = MathHelper.TwoPi / 10;
            for (int i = 0; i < 10; ++i)
            {
                Projectile.NewProjectile(null, pos, Vector2.UnitX.RotatedBy(v * i), ProjectileID.SharpTears, 0, 0,
                    ai1: Utils.getRand(5, 11) / 10f);
            }
        }
    }
}
