using Microsoft.Xna.Framework;
using System.Diagnostics;
using tContentPatch;
using Terraria;
using Terraria.ID;

namespace SuspiciousPlayer.Content.Event1
{
    /// <summary>
    /// 柱子死亡
    /// </summary>
    internal class ActionLunarTowerDead : PatchMain
    {
        public static ActionState state = new ActionState(run);
        public static Vector2 pos = Vector2.Zero;

        public static void run()
        {
            pos = ActionSpawnLunarTower.lunarTower.Center;

            Projectile.NewProjectile(null, pos, Vector2.Zero,
                ProjectileID.HallowBossDeathAurora, 0, 0);
        }

        public override void DoUpdateInWorldPrefix(Stopwatch sw)
        {
            if (state.norun) return;

            if (ActionSpawnLunarTower.lunarTower.active == false)
            {
                Event.SetEventState(Event.EventState_Loot);
                return;
            }
        }
    }
}
