using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using tContentPatch;
using Terraria;
using Terraria.ID;

namespace SuspiciousPlayer.Content.Event1
{
    internal class Event : Mod
    {
        public const int EventState_None = 0;
        public const int EventState_Ready = 1;
        public const int EventState_SpawnTile = 2;
        public const int EventState_SpawnLunarTower = 3;
        /// <summary>
        /// 心脏
        /// </summary>
        public const int EventState_CrimsonHeart = 4;
        /// <summary>
        /// 召唤龙
        /// </summary>
        public const int EventState_CultistRitual = 5;
        /// <summary>
        /// 分裂龙
        /// </summary>
        public const int EventState_CultistDragon = 6;
        /// <summary>
        /// 没护盾
        /// </summary>
        public const int EventState_NoShield = 7;
        /// <summary>
        /// 空灵长枪
        /// </summary>
        public const int EventState_FairyQueenLance = 8;
        public const int EventState_List = 9;
        /// <summary>
        /// 会爆炸的暗黑法阵
        /// </summary>
        public const int EventState_DarkMan = 10;
        /// <summary>
        /// 火球
        /// </summary>
        public const int EventState_Fireball = 11;
        /// <summary>
        /// 太阳舞
        /// </summary>
        public const int EventState_FairyQueenSunDance = 12;
        public const int EventState_LunarTowerDead = 13;
        public const int EventState_Loot = 14;

        public static bool CanSpawnVirtualPlayer = false;
        public static bool CanSpawnNPC = false;
        public static bool CanSpawnNPC_SolarCrawltipede = false;//蜈蚣

        public static int EventState = 0;
        public static Vector2 EventPos = Vector2.Zero;
        public static Player player = null;
        private static Dictionary<int, ActionState> states = null;

        public override void Load()
        {
            states = new Dictionary<int, ActionState>
            {
                { EventState_None, ActionNone.state },
                { EventState_Ready, ActionReady.state },
                { EventState_SpawnTile, ActionSpawnTile.state },
                { EventState_SpawnLunarTower, ActionSpawnLunarTower.state },
                { EventState_CrimsonHeart, ActionCrimsonHeart.state },
                { EventState_CultistRitual, ActionCultistRitual.state },
                { EventState_CultistDragon, ActionCultistDragon.state },
                { EventState_NoShield, ActionNoShield.state },
                { EventState_FairyQueenLance, ActionFairyQueenLance.state },
                { EventState_List, ActionList.state },
                { EventState_DarkMan, ActionDarkMan.state },
                { EventState_Fireball, ActionFireball.state },
                { EventState_FairyQueenSunDance, ActionFairyQueenSunDance.state },
                { EventState_LunarTowerDead, ActionLunarTowerDead.state },
                { EventState_Loot, ActionLoot.state },
            };

            SetEventState(EventState_None);
        }

        public static void Run(Vector2 pos)
        {
            if (EventState != EventState_None) return;

            EventPos = pos;

            SetEventState(EventState_Ready);
        }

        public static void SetEventState(int state)
        {
            states[EventState].End();
            EventState = state;
            states[EventState].Run();
        }
    }

    public class EventMain : PatchMain
    {
        public override void OnEnterWorld()
        {
            Event.SetEventState(Event.EventState_None);
        }

        public override void DoUpdateInWorldPostfix(Stopwatch sw)
        {
            if (Event.EventState == Event.EventState_None) return;

            //柱子死亡
            if (Event.EventState != Event.EventState_Ready &&
                Event.EventState != Event.EventState_LunarTowerDead &&
                Event.EventState != Event.EventState_Loot &&
                ActionSpawnLunarTower.lunarTower?.ai[2] == 1)
            {
                Event.SetEventState(Event.EventState_LunarTowerDead);
                return;
            }

            List<Player> ps = new List<Player>();
            List<float> ds = new List<float>();

            foreach (var i in Main.player)
            {
                if (i.active == false || i.dead) continue;

                float d = Vector2.Distance(i.Center, Event.EventPos);

                if (d > 3000) continue;

                ps.Add(i);
                ds.Add(d);
            }

            Event.player = null;

            //范围内没玩家就退出事件
            if (ps.Count < 1)
            {
                Event.SetEventState(Event.EventState_None);
                return;
            }

            //距离事件位置最近的为目标
            int index = 0;

            if (Main.GameUpdateCount % 60 == 0)
            {
                for (int i = 1; i < ps.Count; i++)
                {
                    if (ds[i] < ds[index]) index = i;
                }
            }

            Event.player = ps[index];

            //跑出圈外有惩罚哦
            if (Main.GameUpdateCount % 30 == 0 && Event.EventState != Event.EventState_Ready)
            {
                foreach (var i in ps)
                {
                    if (ActionSpawnTile.InTile(i.Center)) continue;

                    Vector2 pos = i.Center + (i.velocity * 10);
                    pos.X += Utils.getRand(-50, 50);
                    pos.Y += Utils.getRand(-50, 50);

                    Vector2 v = i.Center - pos;
                    v.Normalize();
                    v *= 5;

                    Projectile.NewProjectile(null, pos, v, ProjectileID.InsanityShadowHostile, 200, 1, ai0: 210);
                    if (Main.netMode == 2) NetMessage.SendData(55, number: i.whoAmI, number2: BuffID.Obstructed, number3: 60);//buff
                    else i.AddBuff(BuffID.Obstructed, 60);
                }
            }
        }
    }
}
