using CommandHelp;
using SundryTool.Utils.quickBuild;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;

namespace SundryTool.Content.Function1
{
    internal partial class Function_skin
    {
        public static void skinClear()
        {
            Player player = Main.LocalPlayer;

            Action<Item[]> clear = (items) =>
            {
                for (int i = 0; i < items.Length; ++i) items[i].SetDefaults(0);
            };

            clear(Utils.getArmorVanity(player));
            clear(Utils.getAccessories(player));
            clear(Utils.getAccessoriesVanity(player));
            clear(player.dye);
        }

        public static void skin1()
        {
            Player player = Main.LocalPlayer;

            Item[] armorV = Utils.getArmorVanity(player);
            Item[] accessories = Utils.getAccessories(player);
            Item[] accessoriesV = Utils.getAccessoriesVanity(player);
            Item[] dye = player.dye;

            armorV[0].SetDefaults(1857);//杰克南瓜灯面具
            armorV[1].SetDefaults(1820);//死神长袍
            accessoriesV[0].SetDefaults(3992);//狂战士手套
            accessoriesV[1].SetDefaults(554);//十字项链
            accessories[2].SetDefaults(4989);//翱翔徽章
            accessories[3].SetDefaults(3883);//双足翼龙之翼
            dye[1].SetDefaults(3556);//午夜彩虹染料
            dye[3].SetDefaults(2864);//火星染料
        }

        public static void skin2()
        {
            Player player = Main.LocalPlayer;

            Item[] armorV = Utils.getArmorVanity(player);
            Item[] accessories = Utils.getAccessories(player);
            Item[] accessoriesV = Utils.getAccessoriesVanity(player);
            Item[] dye = player.dye;

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

        public static List<CommandObject> GetCO()
        {
            CommandObject co = new CommandObject("skin");
            co.SubCommand.Add(tContentPatch.Command.Utils.GetCO_OutputCOList(co.SubCommand));

            CommandMethod clear = new CommandMethod("clear");
            clear.Runing += v => skinClear();
            co.SubCommand.Add(clear);

            CommandMethod skin_1 = new CommandMethod("skin1");
            skin_1.Runing += v => skin1();
            co.SubCommand.Add(skin_1);

            CommandMethod skin_2 = new CommandMethod("skin2");
            skin_2.Runing += v => skin2();
            co.SubCommand.Add(skin_2);

            List<CommandObject> cos = new List<CommandObject>
            {
                co,
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get4("清空", skinClear, null, "Images/Buff", "皮肤清空"),
                UIBuild.get4("穿上", skin1, null, "Images/Buff_299", "皮肤1"),
                UIBuild.get4("穿上", skin2, null, "Images/Buff_287", "皮肤2"),
            };

            return uis;
        }
    }
}
