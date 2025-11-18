using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using tContentPatch;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace test1.VirtualPlayer
{
    internal class VP : ModMain
    {
        public static List<Player> vps = null;

        public override void Initialize()
        {
            vps = new List<Player>();

            PatchNetMessage.OnSyncConnectedPlayer += v =>
            {
                foreach (Player vp in vps)
                {
                    //玩家加入时同步数据到玩家
                    NetMessage.SendData(4, v, number: vp.whoAmI);//外观
                    NetMessage.SendData(16, v, number: vp.whoAmI);//血量
                    NetMessage.SendData(14, v, number: vp.whoAmI, number2: 1);//活动
                    NetMessage.SendData(13, v, number: vp.whoAmI);//控制,属性,位置
                    NetMessage.SendData(30, v, number: vp.whoAmI);//pvp
                    SendItems(vp);
                }
            };
        }

        public override void DoUpdateInWorldPrefix(Stopwatch sw)
        {
            foreach (Player vp in vps)
            {
                vp.active = true;

                if (Main.GameUpdateCount % 30 == 0)
                {
                    NetMessage.SendData(4, -1, vp.whoAmI, number: vp.whoAmI);//外观
                    NetMessage.SendData(16, -1, vp.whoAmI, number: vp.whoAmI);//血量
                }
            }

            //if (Main.GameUpdateCount % 60 == 0) spaw();
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

            vps.Add(player);

            SetVP(player);

            int whoAmI = player.whoAmI;

            if (Main.netMode == 2)
            {
                NetMessage.SendData(4, -1, whoAmI, number: whoAmI);//外观
                NetMessage.SendData(16, -1, whoAmI, number: whoAmI);//血量
                NetMessage.SendData(14, -1, whoAmI, number: whoAmI, number2: 1);//活动
                NetMessage.SendData(13, -1, whoAmI, number: whoAmI);//控制,属性,位置
                NetMessage.SendData(30, -1, whoAmI, number: whoAmI);//pvp
                NetMessage.SendData(42, -1, whoAmI, number: whoAmI);//魔力
                //NetMessage.SendData(50, number: whoAmI);//buff
                //NetMessage.SendData(12, number: whoAmI, number2: (float)PlayerSpawnContext.SpawningIntoWorld);//生成
                SendItems(player);

                NetMessage.SendData(107, -1, whoAmI,
                    NetworkText.FromLiteral($"{player.name}已到达"),
                    255, 100, 100, 200,
                    460);
            }
            else
            {
                Main.NewText($"{player.name}已到达", 100, 100, 200);
            }
        }

        public static void SetVP(Player vp)
        {
            vp.name = $"超级大名字{vps.Count}";
            vp.statLife = vp.statLifeMax = 12345;
            vp.statMana = vp.statManaMax = 200;
            vp.hostile = true;

            vp.hair = 1;
            vp.head = 1;
            vp.body = 1;
            vp.legs = 1;
            vp.hairColor = Color.Red;

            Item[] armorV = Utils.getArmorVanity(vp);
            Item[] accessories = Utils.getAccessories(vp);
            Item[] accessoriesV = Utils.getAccessoriesVanity(vp);
            Item[] dye = vp.dye;

            armorV[0].SetDefaults(2106);//双子魔眼面具
            armorV[1].SetDefaults(1218);//钛金胸甲
            armorV[2].SetDefaults(896);//仙人掌护腿
            accessoriesV[0].SetDefaults(536);//泰坦手套
            accessoriesV[1].SetDefaults(187);//脚蹼
            accessoriesV[2].SetDefaults(5010);//宝藏磁石
            accessoriesV[4].SetDefaults(2609);//猪龙鱼之翼
            accessories[3].SetDefaults(4989);//翱翔徽章
            accessories[4].SetDefaults(4954);//天界星盘
            dye[3].SetDefaults(2876);//淡棕染料
            dye[4].SetDefaults(1049);//淡粉染料
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
