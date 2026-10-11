using Microsoft.Xna.Framework;
using tContentPatch;
using Terraria;
using Terraria.ID;

namespace SuspiciousPlayer.Content.Event1
{
    internal class ActionReady : PatchMain
    {
        public static ActionState state = new ActionState(run);
        private static int ReadyCount = 0;

        public static void run()
        {
            Projectile.NewProjectile(null, Event.EventPos, Vector2.Zero, ProjectileID.DD2ElderWins, 0, 0);
            Projectile.NewProjectile(null, Event.EventPos, Vector2.Zero, ProjectileID.DD2ElderWins, 0, 0);
            Projectile.NewProjectile(null, Event.EventPos, Vector2.Zero, ProjectileID.DD2ElderWins, 0, 0);
            ReadyCount = 60 * 2;
        }

        public override void DoUpdateInWorldPrefix()
        {
            if (state.norun) return;

            if (--ReadyCount < 1) Event.SetEventState(Event.EventState_SpawnTile);
        }
    }
}
