using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using tContentPatch;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;

namespace SuspiciousPlayer.Content.Event1
{
    /// <summary>
    /// 心脏
    /// </summary>
    internal class ActionCrimsonHeart : PatchMain
    {
        public static ActionState state = new ActionState(run, end);
        private static List<Point> poss = new List<Point>();
        private static int shadowOrbCount = 0;
        private static int countMax = 1;
        private static int countTime = 0;

        public static void run()
        {
            poss.Clear();
            countMax = 0;
            countTime = 60 * 4;
            countTime += 60 * 3 * 4;

            int v = ActionSpawnTile.width / (1 + 4);
            AddShadowOrb_crimson(ActionSpawnTile.spawnX + v * 1, ActionSpawnTile.spawnY + (200 / 16));
            AddShadowOrb_crimson(ActionSpawnTile.spawnX + v * 2, ActionSpawnTile.spawnY + (300 / 16));
            AddShadowOrb_crimson(ActionSpawnTile.spawnX + v * 3, ActionSpawnTile.spawnY + (300 / 16));
            AddShadowOrb_crimson(ActionSpawnTile.spawnX + v * 4, ActionSpawnTile.spawnY + (200 / 16));

            shadowOrbCount = WorldGen.shadowOrbCount;
            WorldGen.shadowOrbCount = 0;

            Vector2 pos = ActionSpawnLunarTower.lunarTower.Center;
            Event.CanSpawnNPC_SolarCrawltipede = true;
            NPC.NewNPC(null, (int)pos.X, (int)pos.Y, NPCID.SolarCrawltipedeHead);
            Event.CanSpawnNPC_SolarCrawltipede = false;

            if (Main.netMode == 2)
            {
                NetMessage.SendData(107, -1, -1,
                    NetworkText.FromLiteral("猩红的心脏跳动着"),
                    255, 220, 20, 60,
                    460);
            }
            else
            {
                Main.NewText("猩红的心脏跳动着", 220, 20, 60);
            }
        }

        public static void end()
        {
            ClearHeart();
            Event.CanSpawnNPC_SolarCrawltipede = false;
            WorldGen.shadowOrbCount = shadowOrbCount;
        }

        public override void DoUpdateInWorldPrefix(Stopwatch sw)
        {
            if (state.norun) return;

            for (int i = 0; i < poss.Count; )
            {
                Tile tile = Main.tile[poss[i].X, poss[i].Y];
                if (tile.type == TileID.ShadowOrbs && tile.active())
                {
                    ++i;
                    continue;
                }
                poss.RemoveAt(i);

                WorldGen.shadowOrbCount = 0;
            }

            if (poss.Count != NPC.ShieldStrengthTowerSolar)
            {
                NPC.ShieldStrengthTowerSolar = poss.Count;
                
                if (Main.netMode == 2) NetMessage.SendData(MessageID.UpdateTowerShieldStrengths);
            }
            
            //心脏没了
            if (poss.Count < 1)
            {
                ActionSpawnLunarTower.lunarTower.ai[3] = 1;
                if (Main.netMode == 2) NetMessage.SendData(MessageID.SyncNPC, number: ActionSpawnLunarTower.lunarTower.whoAmI);

                Event.SetEventState(Event.EventState_NoShield);
                return;
            }

            //心脏声音
            if (Main.GameUpdateCount % 60 * 3 == 0)
            {
                if (Main.netMode == 2)
                {
                    NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(Event.EventPos,
                        365, SoundID.NPCHit20.Style));
                }
                else
                {
                    SoundEngine.PlaySound(SoundID.NPCHit20);
                }
            }

            if (countTime-- < 1)
            {
                countTime = 60 * 5;
                if (++countMax > 3) countMax = 3;
            }

            //蜈蚣少于上限就主动召唤
            if (Main.GameUpdateCount % (60 * 5) == 0)
            {
                int count = Enumerable.Count(Main.npc, i => i.active && i.type == NPCID.SolarCrawltipedeHead);
                if (count >= countMax) return;

                int x = (int)ActionSpawnLunarTower.lunarTower.Center.X;
                int y = (int)ActionSpawnLunarTower.lunarTower.Center.Y;

                Event.CanSpawnNPC_SolarCrawltipede = true;
                NPC.NewNPC(null, x, y, NPCID.SolarCrawltipedeHead);
                Event.CanSpawnNPC_SolarCrawltipede = false;
            }
        }

        public static void AddShadowOrb_crimson(int x, int y)
        {
            if (x < 10 || x > Main.maxTilesX - 10 || y < 10 || y > Main.maxTilesY - 10)
            {
                return;
            }
            for (int i = x - 1; i < x + 1; i++)
            {
                for (int j = y - 1; j < y + 1; j++)
                {
                    if (Main.tile[i, j] == null) return;
                    if (Main.tile[i, j].active() && Main.tile[i, j].type == TileID.ShadowOrbs)
                    {
                        poss.Add(new Point(x, y));
                        return;
                    }
                }
            }
            short num = 36;
            Main.tile[x - 1, y - 1].active(active: true);
            Main.tile[x - 1, y - 1].type = TileID.ShadowOrbs;
            Main.tile[x - 1, y - 1].frameX = num;
            Main.tile[x - 1, y - 1].frameY = 0;
            Main.tile[x, y - 1].active(active: true);
            Main.tile[x, y - 1].type = TileID.ShadowOrbs;
            Main.tile[x, y - 1].frameX = (short)(18 + num);
            Main.tile[x, y - 1].frameY = 0;
            Main.tile[x - 1, y].active(active: true);
            Main.tile[x - 1, y].type = TileID.ShadowOrbs;
            Main.tile[x - 1, y].frameX = num;
            Main.tile[x - 1, y].frameY = 18;
            Main.tile[x, y].active(active: true);
            Main.tile[x, y].type = TileID.ShadowOrbs;
            Main.tile[x, y].frameX = (short)(18 + num);
            Main.tile[x, y].frameY = 18;

            poss.Add(new Point(x, y));

            if (Main.netMode == 2) NetMessage.SendTileSquare(-1, x - 1, y - 1, 2, 2);
        }

        public static void ClearHeart()
        {
            while (poss.Count > 0)
            {
                int x = poss[0].X;
                int y = poss[0].Y;

                poss.RemoveAt(0);

                Tile tile = Main.tile[x, y];
                if (tile.type != TileID.ShadowOrbs) continue;
                if (tile.active() == false) continue;

                Main.tile[x - 1, y - 1].active(false);
                Main.tile[x, y - 1].active(false);
                Main.tile[x - 1, y].active(false);
                Main.tile[x, y].active(false);

                WorldGen.shadowOrbCount = 0;

                if (Main.netMode == 2) NetMessage.SendTileSquare(-1, x - 1, y - 1, 2, 2);
            }
        }
    }
}
