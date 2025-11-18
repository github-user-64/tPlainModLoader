using Microsoft.Xna.Framework;
using System.Diagnostics;
using tContentPatch;
using Terraria;
using Terraria.ID;

namespace SuspiciousPlayer.Content.Event1
{
    /// <summary>
    /// 无护盾时
    /// </summary>
    internal class ActionNoShield : PatchMain
    {
        public static ActionState state = new ActionState(run);
        private static int timeCount = 0;
        private static int life = 0;

        public static void run()
        {
            timeCount = 0;
            life = ActionSpawnLunarTower.lunarTower.life;
            life -= ActionSpawnLunarTower.lunarTower.lifeMax / 4;
        }

        public override void DoUpdateInWorldPrefix(Stopwatch sw)
        {
            if (state.norun) return;

            //时间到了或者血掉到一定程度
            if (++timeCount > 60 * 7 ||
                ActionSpawnLunarTower.lunarTower.life < life)
            {
                Event.SetEventState(Event.EventState_SpawnLunarTower);
            }

            //星光
            if (Main.GameUpdateCount % 40 == 0)
            {
                Vector2 pos = ActionSpawnLunarTower.lunarTower.Center;
                pos.Y = ActionSpawnLunarTower.lunarTower.position.Y;

                Vector2 v = Vector2.UnitX;
                if (Event.player != null)
                {
                    v = Event.player.Center - pos;
                    v.Normalize();
                    v *= 16;
                }

                Projectile.NewProjectile(null, pos, v, ProjectileID.HallowBossRainbowStreak, 80, 1, ai1: 0.4f);
            }
        }
    }
}
