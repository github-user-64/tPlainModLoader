using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using tContentPatch;
using Terraria;
using Terraria.ID;

namespace SuspiciousPlayer.Content.Event1
{
    /// <summary>
    /// 生成柱子, 再次生成则回复护盾
    /// </summary>
    internal class ActionSpawnLunarTower : PatchMain
    {
        //NPC.ShieldStrengthTowerVortex = 0;
        //NPC.ShieldStrengthTowerStardust = 0;
        //NPC.ShieldStrengthTowerNebula = 0;
        //NPC.ShieldStrengthTowerSolar = 0;
        //422 493 507 517

        //NPC.ShieldStrengthTowerStardust影响盾血条
        //如果NPC.ShieldStrengthTowerStardust为0时
        //如果ai[3] 大于1，那么ai[3] 会变大之后归零
        //此时ai[3] 影响盾绘制

        public static ActionState state = new ActionState(run);
        public static NPC lunarTower = null;

        public static void run()
        {
            Spaw();
        }

        public static void Spaw()
        {
            if (lunarTower == null || lunarTower.active == false || lunarTower.type != NPCID.LunarTowerSolar || Main.npc.Contains(lunarTower) == false)
            {
                lunarTower = Main.npc.FirstOrDefault(i => i.active && i.type == NPCID.LunarTowerSolar);
                if (lunarTower == null)
                    lunarTower = Main.npc[NPC.NewNPC(null, (int)Event.EventPos.X, (int)Event.EventPos.Y, NPCID.LunarTowerSolar)];
            }

            Vector2 pos = lunarTower.Center;
            pos.X = Event.EventPos.X;
            if (ActionSpawnTile.InTile(pos) == false) pos.Y = Event.EventPos.Y;
            lunarTower.Center = pos;

            lunarTower.ai[3] = 100;
            NPC.ShieldStrengthTowerSolar = 0;

            if (Main.netMode == 2)
            {
                NetMessage.SendData(MessageID.UpdateTowerShieldStrengths);
                NetMessage.SendData(MessageID.SyncNPC, number: lunarTower.whoAmI);
            }
        }

        public override void DoUpdateInWorldPrefix(Stopwatch sw)
        {
            if (state.norun) return;

            //缩小护盾
            if (lunarTower.ai[3] < 0)
            {
                lunarTower.ai[3] = 0;
                NPC.ShieldStrengthTowerSolar = NPC.LunarShieldPowerMax;

                if (Main.netMode == 2)
                {
                    NetMessage.SendData(MessageID.UpdateTowerShieldStrengths);
                    NetMessage.SendData(MessageID.SyncNPC, number: lunarTower.whoAmI);
                }

                Event.SetEventState(Event.EventState_CultistRitual);
            }
            else
            {
                lunarTower.ai[3] -= 2;

                if (Main.netMode == 2) NetMessage.SendData(MessageID.SyncNPC, number: lunarTower.whoAmI);
            }
        }

        public static void ClearNPC()
        {
            if (lunarTower == null) return;
            lunarTower.active = false;

            if (Main.netMode == 2) NetMessage.SendData(MessageID.SyncNPC, number: lunarTower.whoAmI);

            lunarTower = null;
        }
    }
}
