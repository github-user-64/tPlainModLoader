using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Linq;
using tContentPatch;
using Terraria;
using Terraria.ID;

namespace SuspiciousPlayer.Content.Event1
{
    /// <summary>
    /// 召唤龙
    /// </summary>
    internal class ActionCultistRitual : PatchMain
    {
        public static ActionState state = new ActionState(run, end);
        private static Projectile fazhen = null;

        public static void run()
        {
            fazhen = null;
        }

        public static void end()
        {
            NPC n = Main.npc.FirstOrDefault(i => i.active && i.type == NPCID.CultistBoss);
            if (n == null) return;
            n.active = false;

            if (Main.netMode == 2) NetMessage.SendData(MessageID.SyncNPC, number: n.whoAmI);
        }

        public override void DoUpdateInWorldPrefix(Stopwatch sw)
        {
            if (state.norun) return;

            NPC n = Main.npc.FirstOrDefault(i => i.active && i.type == NPCID.CultistBoss);
            if (n == null)
            {
                n = Main.npc[NPC.NewNPC(null, (int)Event.EventPos.X, (int)Event.EventPos.Y, NPCID.CultistBoss)];
            }

            n.ai[0] = 5;//召唤法阵状态
            n.ai[1] = 31;//31时处于隐身状态, 且不会召唤法阵

            if (Main.netMode == 2) NetMessage.SendData(MessageID.SyncNPC, number: n.whoAmI);

            if (fazhen == null)
                fazhen = Main.projectile[Projectile.NewProjectile(null, Event.EventPos, Vector2.Zero, ProjectileID.CultistRitual, 0, 0, ai1: n.whoAmI)];

            if (fazhen.active == false || fazhen.type != ProjectileID.CultistRitual)
            {
                Event.SetEventState(Event.EventState_CultistDragon);
            }
        }
    }
}
