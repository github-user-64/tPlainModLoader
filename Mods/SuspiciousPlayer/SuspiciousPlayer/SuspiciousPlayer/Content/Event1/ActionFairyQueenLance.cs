using Microsoft.Xna.Framework;
using System.Diagnostics;
using tContentPatch;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace SuspiciousPlayer.Content.Event1
{
    /// <summary>
    /// 空灵长枪
    /// </summary>
    internal class ActionFairyQueenLance : PatchMain
    {
        public static ActionState state = new ActionState(run);
        private static Vector2 pos = Vector2.Zero;
        private static float v = 0;
        private static Vector2 off = Vector2.Zero;
        private static int direction = 0;
        private static int count = 0;
        private static int cd = 0;

        public static void run()
        {
            float margin = 50;

            count = (int)(ActionSpawnTile.height * 16 / margin);
            cd = 60;

            pos = new Vector2(ActionSpawnTile.spawnX, ActionSpawnTile.spawnY) * 16 - Event.EventPos;
            pos.X -= margin / 2;
            pos.Y -= margin / 2;
            pos.Y -= margin * 2;//为下面留空间
            off = Vector2.UnitY * margin;

            direction += Utils.getRand(1, 4);
            if (direction > 3) direction -= 4;

            float r = MathHelper.TwoPi / 4;
            r *= direction;

            pos = pos.RotatedBy(r);
            off = off.RotatedBy(r);
            v = r;

            pos += Event.EventPos;

            if (Main.netMode == 2)
            {
                NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(Event.EventPos,
                    350, SoundID.NPCDeath7.Style));
            }
            else
            {
                SoundEngine.PlaySound(SoundID.NPCDeath7);
            }
        }

        public override void DoUpdateInWorldPrefix(Stopwatch sw)
        {
            if (state.norun) return;

            if (count < 1)
            {
                if (cd-- > 0) return;

                Event.SetEventState(Event.EventState_List);
            }
            else
            {
                if (Main.GameUpdateCount % 4 != 0) return;

                Projectile.NewProjectile(null, pos, Vector2.Zero, ProjectileID.FairyQueenLance, 150, 1, ai0: v);
                pos += off;

                --count;
            }
        }
    }
}
