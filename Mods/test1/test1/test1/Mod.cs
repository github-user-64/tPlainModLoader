using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Linq;
using tContentPatch;
using Terraria;

namespace test1
{
    public class test1 : ModMain
    {
        //NPC.ShieldStrengthTowerVortex = 0;
        //NPC.ShieldStrengthTowerStardust = 0;
        //NPC.ShieldStrengthTowerNebula = 0;
        //NPC.ShieldStrengthTowerSolar = 0;
        //422 493 507 517
        ////
        //NPC.ShieldStrengthTowerStardust影响盾血条
        //如果NPC.ShieldStrengthTowerStardust为0时
        //如果ai[3] 大于1，那么ai[3] 会变大之后归零
        //此时ai[3] 影响盾绘制
        public bool canSpaw = true;
        public bool isSpaw = false;
        public NPC npc = null;

        public override void DoUpdateInWorldPrefix(Stopwatch sw)
        {
            return;

            if (canSpaw)
            {
                Spaw();
            }
            if (isSpaw)
            {
                UpdateNPC();
            }
        }

        public void Spaw()
        {
            Player player = Main.player.FirstOrDefault(i => i.active);
            if (player == null) return;
            player.statLife = player.statLifeMax;
            NetMessage.SendData(66, number: player.whoAmI, number2: player.statLifeMax);

            Vector2 pos = player.position;
            pos.Y -= 100;

            foreach (var i in Main.npc) if (i.active && i.type == 517) i.active = false;

            npc = Main.npc[NPC.NewNPC(null, (int)pos.X, (int)pos.Y, 517)];
            npc.ai[3] = 100;
            npc.life = npc.lifeMax = 123456;

            canSpaw = false;
            isSpaw = true;
        }

        public void UpdateNPC()
        {
            if (npc == null || npc.active == false)
            {
                canSpaw = true;
                isSpaw = false;
                return;
            }

            NPC.ShieldStrengthTowerSolar = 0;

            npc.ai[3] -= 2;
            if (npc.ai[3] < 0)
            {
                npc.ai[3] = 0;
                npc.active = false;
            }

            NetMessage.SendData(23, number:npc.whoAmI);
        }
    }
}
