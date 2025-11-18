using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using tContentPatch;
using Terraria;

namespace test1
{
    internal class PhantasmalSphere : ModMain
    {
        public List<Projectile> ps = new List<Projectile>();
        public Vector2 position = Vector2.Zero;

        public override void DoUpdateInWorldPrefix(Stopwatch sw)
        {
            return;

            Player player = Main.player.FirstOrDefault(i => i.active);
            if (player == null) return;
            player.statLife = player.statLifeMax;
            NetMessage.SendData(66, number: player.whoAmI, number2: player.statLifeMax);

            position += (player.Center - position) / 30;

            while (ps.Count < 8)
            {
                ps.Add(null);
            }

            for (int i = 0; i < ps.Count; ++i)
            {
                if (ps[i] != null && ps[i].active) continue;

                ps[i] = Main.projectile[Projectile.NewProjectile(null, position, Vector2.Zero, 454, 1, 1)];
            }

            for (int i = 0; i < ps.Count; ++i)
            {
                Projectile p = ps[i];

                Vector2 pos = Vector2.UnitX * 800;
                pos = pos.RotatedBy(MathHelper.TwoPi / ps.Count * i);
                pos = pos.RotatedBy(Main.GameUpdateCount * 0.05f);

                p.ai[0] = 30;
                p.position = position + pos;
                p.velocity = Vector2.Normalize(pos.RotatedBy(MathHelper.TwoPi / 4)) * 10;

                NetMessage.SendData(27, -1, -1, null, p.whoAmI);
            }
        }
    }
}
