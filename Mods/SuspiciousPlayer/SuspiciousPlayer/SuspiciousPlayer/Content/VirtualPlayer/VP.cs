using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using tContentPatch;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;

namespace SuspiciousPlayer.Content.VirtualPlayer
{
    internal class VP : PatchMain
    {
        public static List<Player> vps = null;

        public override void Initialize()
        {
            vps = new List<Player>();

            Patch.PatchNetMessage.OnSyncConnectedPlayer += v =>
            {
                foreach (Player vp in vps)
                {
                    //玩家加入时同步数据到玩家
                    NetMessage.SendData(4, v, number: vp.whoAmI);//外观
                    NetMessage.SendData(16, v, number: vp.whoAmI);//血量
                    NetMessage.SendData(14, v, number: vp.whoAmI, number2: 1);//活动
                    NetMessage.SendData(13, v, number: vp.whoAmI);//控制,属性,位置
                    NetMessage.SendData(30, v, number: vp.whoAmI);//pvp
                    NetMessage.SendData(4, number: vp.whoAmI);//外观
                    NetMessage.SendData(16, number: vp.whoAmI);//血量
                    SendItems(vp);
                }
            };
        }

        public override void OnEnterWorld()
        {
            vps.Clear();
        }

        public override void DoUpdateInWorldPrefix(Stopwatch sw)
        {
            if (Main.netMode != 0 && Main.netMode != 2) return;

            for (int i = 0; i < vps.Count; i++)
            {
                Player vp = vps[i];
                if (vp.statLife > 0 && vp.dead == false) continue;

                vp.statLife = 0;
                vp.dead = false;
                vp.KillMe(PlayerDeathReason.ByOther(1), vp.statLifeMax, 0, false);
                if (Main.netMode == 2) NetMessage.SendPlayerDeath(vp.whoAmI, PlayerDeathReason.ByOther(1), vp.statLifeMax, 0, false);

                vps.Remove(vp);
                --i;
            }

            if (vps.Count < 1 && Main.GameUpdateCount % (60 * 5) == 0  && Event1.Event.CanSpawnVirtualPlayer) spaw();

            foreach (Player vp in vps)
            {
                vp.active = true;
                if (Main.GameUpdateCount % 15 == 0 && Main.netMode == 2)
                {
                    NetMessage.SendData(13, -1, vp.whoAmI, number: vp.whoAmI);//控制,属性,位置
                    NetMessage.SendData(16, -1, vp.whoAmI, number: vp.whoAmI);//血量
                }
            }
        }

        public static void spaw()
        {
            Player player = null;
            for (int i = Main.player.Length - 2; i >= 0; i--)
            {
                if (Main.player[i].active) continue;
                if (vps.Contains(Main.player[i])) continue;
                player = Main.player[i];
                break;
            }
            if (player == null) return;

            SetVP(player);
            //player.position.X = Main.spawnTileX * 16;
            //player.position.Y = Main.spawnTileY * 16 - 100;
            for (int i = 0; i < player.buffType.Length; ++i)
            {
                player.buffTime[i] = 0;
                player.buffType[i] = 0;
            }
            player.Spawn(new PlayerSpawnContext());

            vps.Add(player);

            int whoAmI = player.whoAmI;

            if (Main.netMode == 2)
            {
                NetMessage.SendData(4, -1, whoAmI, number: whoAmI);//外观
                NetMessage.SendData(16, -1, whoAmI, number: whoAmI);//血量
                NetMessage.SendData(14, -1, whoAmI, number: whoAmI, number2: 1);//活动
                NetMessage.SendData(13, -1, whoAmI, number: whoAmI);//控制,属性,位置
                NetMessage.SendData(30, -1, whoAmI, number: whoAmI);//pvp
                NetMessage.SendData(42, -1, whoAmI, number: whoAmI);//魔力
                NetMessage.SendData(50, number: whoAmI);//buff
                NetMessage.SendData(12, number: whoAmI, number2: (float)PlayerSpawnContext.ReviveFromDeath);//生成
                SendItems(player);

                NetMessage.SendData(107, -1, whoAmI,
                    NetworkText.FromLiteral($"{player.name}已到达"),
                    255, 50, 125, 255,
                    460);
            }
            else
            {
                Main.NewText($"{player.name}已到达", 50, 125, 255);
            }
        }

        public static void SetVP(Player vp)
        {
            vp.name = "固态地面";
            vp.statLife = vp.statLifeMax = 1234;
            vp.statMana = vp.statManaMax = 200;
            vp.hostile = true;

            vp.hair = 0;
            vp.head = 0;
            vp.body = 0;
            vp.legs = 0;
            vp.skinColor = Color.RoyalBlue;

            Item[] accessories = Utils.getAccessories(vp);

            accessories[0].SetDefaults(1613);//十字章护盾
        }

        public static void SendItems(Player player)
        {
            for (int i = 0; i < player.armor.Length; i++)
            {
                NetMessage.SendData(5, -1, player.whoAmI, null, player.whoAmI,
                    PlayerItemSlotID.Armor0 + i, player.armor[i].prefix);
            }
        }
    }
}
