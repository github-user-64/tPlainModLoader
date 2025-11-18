using Microsoft.Xna.Framework;
using System.Diagnostics;
using tContentPatch;
using Terraria;
using Terraria.ID;

namespace SuspiciousPlayer.Content.Event1
{
    /// <summary>
    /// 火球
    /// </summary>
    internal class ActionFireball : PatchMain
    {
        public static ActionState state = new ActionState(run);
        private static int count = 0;

        public static void run()
        {
            count = 0;
        }

        public override void DoUpdateInWorldPrefix(Stopwatch sw)
        {
            if (state.norun) return;

            if (Main.GameUpdateCount % 2 != 0) return;

            float what = count % 2 == 0 ? count : -count;
            what *= MathHelper.TwoPi / 360;

            Vector2 pos = ActionSpawnLunarTower.lunarTower.Center;
            pos.Y = ActionSpawnLunarTower.lunarTower.position.Y;
            Vector2 v = -Vector2.UnitY.RotatedBy(what);
            v *= 10;

            Projectile.NewProjectile(null, pos, v, ProjectileID.DD2BetsyFireball, 150, 1, ai0: -30);

            if (++count > 45) Event.SetEventState(Event.EventState_List);
        }
    }
}
