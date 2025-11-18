using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using tContentPatch;
using Terraria;
using Terraria.ID;

namespace SuspiciousPlayer.Content.Event1
{
    internal class ActionReady : PatchMain
    {
        public static ActionState state = new ActionState(run, end);
        private static int ReadyCount = 0;

        public static void run()
        {
            Projectile.NewProjectile(null, Event.EventPos, Vector2.Zero, ProjectileID.DD2ElderWins, 0, 0);
            Projectile.NewProjectile(null, Event.EventPos, Vector2.Zero, ProjectileID.DD2ElderWins, 0, 0);
            Projectile.NewProjectile(null, Event.EventPos, Vector2.Zero, ProjectileID.DD2ElderWins, 0, 0);
            ReadyCount = 60 * 2;
        }

        public static void end()
        {
            Player player = Main.player.FirstOrDefault(i =>
            {
                if (i.active == false) return false;
                if (Vector2.Distance(Event.EventPos, i.position) > 3000) return false;
                return true;
            });
            if (player == null) return;
            Event.EventPos = player.Center;
        }

        public override void DoUpdateInWorldPrefix(Stopwatch sw)
        {
            if (state.norun) return;

            if (--ReadyCount < 1) Event.SetEventState(Event.EventState_SpawnTile);
        }
    }
}
