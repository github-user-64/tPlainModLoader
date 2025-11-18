using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using tContentPatch;
using Terraria;

namespace test1
{
    internal class CultistDragon : ModMain
    {
        public override void DoUpdateInWorldPrefix(Stopwatch sw)
        {
            Player player = Main.player.FirstOrDefault(i => i.active);
            if (player == null) return;

            NetMessage.SendData(66, number: player.whoAmI, number2: player.statLifeMax);

            //if (Main.mouseRight == false || Main.GameUpdateCount % 30 != 0) return;
            if (player.controlJump == false) return;
            if (Main.npc.FirstOrDefault(i => i.active && i.type == 454) != null) return;

            NPC head = Main.npc[NPC.NewNPC(null, (int)player.position.X, (int)player.position.Y, 454)];
        }

        public override void DoUpdateInWorldPostfix(Stopwatch sw)
        {
            foreach (var i in Main.npc)
            {
                UpdateDeadNPC(i);
            }
        }

        public void UpdateDeadNPC(NPC npc)
        {
            if (npc == null) return;
            if (npc.active || npc.type != 454) return;

            List<NPC> npcs = new List<NPC>();
            NPC head = npc;

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

            ActiveNPC(npcs);
        }

        //ai: 0下一个,1上一个,3头
        public void ActiveNPC(List<NPC> npcs)
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
