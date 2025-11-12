using System.Collections.Generic;
using System.Diagnostics;
using tContentPatch;
using Terraria;

namespace SuspiciousPlayer.Content.Event1
{
    /// <summary>
    /// 分裂龙
    /// </summary>
    internal class ActionCultistDragon : ModMain
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

            int npcCount = 0;
            foreach (var i in Main.npc)
            {
                UpdateDeadNPC(i);
                if (i.active && i.type == 454) ++npcCount;
            }

            //没龙下一个
            if (npcCount < 1)
            {
                //在几种之间切换
                List<int> list = new List<int>();

                switch (Utils.getRand(0, 3))
                {
                    case 2:
                    case 1:
                        list.Add(Event.EventState_DarkMan);
                        list.Add(Event.EventState_Fireball);
                        break;
                    default:
                        list.Add(Event.EventState_FairyQueenSunDance);
                        list.Add(Event.EventState_FairyQueenLance);
                        list.Add(Event.EventState_FairyQueenLance);
                        break;
                }

                list.Add(Event.EventState_CrimsonHeart);

                ActionList.SetList(list);

                Event.SetEventState(Event.EventState_List);
                return;
            }

            //固定时间内没清空继续生成
            if (++count > 60 * 16)
            {
                Event.SetEventState(Event.EventState_CultistRitual);
                return;
            }
        }

        public void UpdateDeadNPC(NPC npc)
        {
            if (npc == null) return;
            if (npc.active || npc.type != 454) return;

            List<NPC> npcs = new List<NPC>();
            NPC head = npc;
            int life = head.lifeMax / 3;

            if (life < 1)
            {
                head.type = 0;
                return;
            }

            while (true)
            {
                if (npc.ai[0] < 1 || npc.ai[0] >= Main.npc.Length) break;

                NPC next = Main.npc[(int)npc.ai[0]];
                int nextType = next.type;

                if (nextType != 454 &&
                    nextType != 455 &&
                    nextType != 456 &&
                    nextType != 457 &&
                    nextType != 458 &&
                    nextType != 459) break;

                npcs.Add(npc);

                npc = next;
            }

            if (npcs.Count < 6)
            {
                head.type = 0;
                return;
            }

            ActiveNPC(npcs, life);
        }

        //ai: 0下一个,1上一个,3头
        public void ActiveNPC(List<NPC> npcs, int life)
        {
            NPC head = null;
            int next = 0;
            int prev = 0;

            for (int i = 0; i < npcs.Count; ++i)
            {
                NPC n = npcs[i];

                int type = n.type;
                if (i == npcs.Count / 2) type = 459;
                else if (i == npcs.Count / 2 + 1) type = 454;

                next = (int)n.ai[0];
                prev = (int)n.ai[1];

                n.SetDefaults(type);

                if (type == 454)
                {
                    prev = 0;
                    head = n;
                    head.lifeMax = life;
                    head.life = life;
                }
                else if (type == 459)
                {
                    next = 0;
                }

                n.ai[0] = next;
                n.ai[1] = prev;
                n.ai[3] = head?.whoAmI ?? 0;
                n.active = true;
            }

            if (Main.netMode == 2)
            {
                foreach (var i in npcs) NetMessage.SendData(23, number: i.whoAmI);
            }
        }
    }
}
