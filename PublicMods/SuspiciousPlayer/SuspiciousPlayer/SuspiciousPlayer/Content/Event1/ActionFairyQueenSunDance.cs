using Microsoft.Xna.Framework;
using System.Diagnostics;
using tContentPatch;
using Terraria;
using Terraria.ID;

namespace SuspiciousPlayer.Content.Event1
{
    /// <summary>
    /// 太阳舞
    /// </summary>
    internal class ActionFairyQueenSunDance : ModMain
    {
        public static ActionState state = new ActionState(run);
        private static int count = 0;

        public static void run()
        {
            count = 120;
        }

        public override void DoUpdateInWorldPrefix(Stopwatch sw)
        {
            if (state.norun) return;

            if (--count < 1)
            {
                if (count < -60 * 4) Event.SetEventState(Event.EventState_List);
                return;
            }

            if (Main.GameUpdateCount % 2 != 0) return;

            Vector2 pos = ActionSpawnLunarTower.lunarTower.Center;
            pos.Y = ActionSpawnLunarTower.lunarTower.position.Y;
            if (count > 60) pos.Y += (count - 60) * 7;

            Projectile.NewProjectile(null, pos, Vector2.Zero, ProjectileID.FairyQueenSunDance,
                80, 1, ai0: 4.6f);

            if (--count < 1) Event.SetEventState(Event.EventState_List);
        }
    }
}
