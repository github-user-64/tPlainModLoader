using tContentPatch;
using Terraria;
using Terraria.ID;

namespace CreativeInventory
{
    public class itemsSort_initialize : ModMain
    {
        public override void OnEnterWorld()
        {
            itemsSort.initialize();
        }
    }

    public partial class itemsSort
    {
        public static bool Loaded { get; protected set; } = false;
        /// <summary>
        /// 全部id
        /// </summary>
        public static ItemSort<int> ID { get; protected set; } = null;
        protected static bool ID_initialized1 = false;

        public static void initialize()
        {
            Loaded = false;
            ID_initialized1 = false;

            ID = null;
            ID = new ItemSort<int>();
            ID.ItemsSort.Add(ID_Weapon, new ItemSort<int>());
            ID.ItemsSort.Add(ID_Tool, new ItemSort<int>());
            ID.ItemsSort.Add(ID_ToolKit, new ItemSort<int>());
            ID.ItemsSort.Add(ID_Armor, new ItemSort<int>());
            ID.ItemsSort.Add(ID_Ammo, new ItemSort<int>());
            ID.ItemsSort.Add(ID_Accessorie, new ItemSort<int>());
            ID.ItemsSort.Add(ID_Tile, new ItemSort<int>());
            ID.ItemsSort.Add(ID_Buff, new ItemSort<int>());
            ID.ItemsSort.Add(ID_BossSpawn, new ItemSort<int>());
            ID.ItemsSort.Add(ID_Consumable, new ItemSort<int>());
            ID.ItemsSort.Add(ID_Other, new ItemSort<int>());

            ID.ItemsSort[ID_Weapon].ItemsSort.Add(ID_Weapon_Melee, new ItemSort<int>());
            ID.ItemsSort[ID_Weapon].ItemsSort.Add(ID_Weapon_Ranged, new ItemSort<int>());
            ID.ItemsSort[ID_Weapon].ItemsSort.Add(ID_Weapon_Magic, new ItemSort<int>());
            ID.ItemsSort[ID_Weapon].ItemsSort.Add(ID_Weapon_Summon, new ItemSort<int>());
            ID.ItemsSort[ID_Tool].ItemsSort.Add(ID_Tool_pick, new ItemSort<int>());
            ID.ItemsSort[ID_Tool].ItemsSort.Add(ID_Tool_axe, new ItemSort<int>());
            ID.ItemsSort[ID_Tool].ItemsSort.Add(ID_Tool_hammer, new ItemSort<int>());
            ID.ItemsSort[ID_ToolKit].ItemsSort.Add(ID_ToolKit_fishingPole, new ItemSort<int>());
            ID.ItemsSort[ID_ToolKit].ItemsSort.Add(ID_ToolKit_hook, new ItemSort<int>());
            ID.ItemsSort[ID_ToolKit].ItemsSort.Add(ID_ToolKit_mount, new ItemSort<int>());
            ID.ItemsSort[ID_ToolKit].ItemsSort.Add(ID_ToolKit_pet, new ItemSort<int>());
            ID.ItemsSort[ID_Armor].ItemsSort.Add(ID_Armor_head, new ItemSort<int>());
            ID.ItemsSort[ID_Armor].ItemsSort.Add(ID_Armor_body, new ItemSort<int>());
            ID.ItemsSort[ID_Armor].ItemsSort.Add(ID_Armor_leg, new ItemSort<int>());
            ID.ItemsSort[ID_Armor].ItemsSort.Add(ID_Armor_vanity, new ItemSort<int>());
            ID.ItemsSort[ID_Tile].ItemsSort.Add(ID_Tile_tile, new ItemSort<int>());
            ID.ItemsSort[ID_Tile].ItemsSort.Add(ID_Tile_wall, new ItemSort<int>());

            ID_initialized1 = true;

            load_id();

            Loaded = true;
        }

        protected static void load_id()
        {
            Item item = new Item();
            int exception_id = 0;
            int exception_sort = 0;

            for (int i = 1; i < ItemID.Count; ++i)
            {
                if (ItemID.Sets.Deprecated[i]) continue;//已弃用
                item.SetDefaults(i);
                if (item.type < 1 || item.type >= ItemID.Count)
                {
                    ++exception_id;
                    continue;
                }

                try
                {
                    load_id_sort(item);
                }
                catch
                {
                    ++exception_sort;
                }
            }
        }

        protected static void load_id_sort(Item i)
        {
            #region 工具
            if (i.pick > 0)//稿
            {
                ItemsSort_Gets2(ID_Tool, ID_Tool_pick)?.Items?.Add(i.type);
            }
            else if (i.axe > 0)//斧
            {
                ItemsSort_Gets2(ID_Tool, ID_Tool_axe)?.Items?.Add(i.type);
            }
            else if (i.hammer > 0)//锤
            {
                ItemsSort_Gets2(ID_Tool, ID_Tool_hammer)?.Items?.Add(i.type);
            }
            #endregion
            else if (i.ammo != AmmoID.None)//弹药
            {
                ItemsSort_Gets2(ID_Ammo)?.Items?.Add(i.type);
            }
            #region 武器
            else if (i.melee)
            {
                ItemsSort_Gets2(ID_Weapon, ID_Weapon_Melee)?.Items?.Add(i.type);
            }
            else if (i.ranged)
            {
                ItemsSort_Gets2(ID_Weapon, ID_Weapon_Ranged)?.Items?.Add(i.type);
            }
            else if (i.magic)
            {
                ItemsSort_Gets2(ID_Weapon, ID_Weapon_Magic)?.Items?.Add(i.type);
            }
            else if (i.summon)
            {
                ItemsSort_Gets2(ID_Weapon, ID_Weapon_Summon)?.Items?.Add(i.type);
            }
            #endregion
            #region 装备
            else if (i.fishingPole > 0)//钓竿
            {
                ItemsSort_Gets2(ID_ToolKit, ID_ToolKit_fishingPole)?.Items?.Add(i.type);
            }
            else if (load_id_sort_a1(i.shoot, Terraria.Main.projHook))//钩爪
            {
                ItemsSort_Gets2(ID_ToolKit, ID_ToolKit_hook)?.Items?.Add(i.type);
            }
            else if (i.mountType != -1)//坐骑
            {
                ItemsSort_Gets2(ID_ToolKit, ID_ToolKit_mount)?.Items?.Add(i.type);
            }
            else if (load_id_sort_a1(i.buffType, Terraria.Main.vanityPet) || load_id_sort_a1(i.buffType, Terraria.Main.lightPet))//宠物
            {
                ItemsSort_Gets2(ID_ToolKit, ID_ToolKit_pet)?.Items?.Add(i.type);
            }
            #endregion
            #region 盔甲
            else if (i.vanity)//时装
            {
                ItemsSort_Gets2(ID_Armor, ID_Armor_vanity)?.Items?.Add(i.type);
            }
            else if (i.headSlot != -1)//头
            {
                ItemsSort_Gets2(ID_Armor, ID_Armor_head)?.Items?.Add(i.type);
            }
            else if (i.bodySlot != -1)//身
            {
                ItemsSort_Gets2(ID_Armor, ID_Armor_body)?.Items?.Add(i.type);
            }
            else if (i.legSlot != -1)//腿
            {
                ItemsSort_Gets2(ID_Armor, ID_Armor_leg)?.Items?.Add(i.type);
            }
            #endregion
            else if (i.accessory)//饰品
            {
                ItemsSort_Gets2(ID_Accessorie)?.Items?.Add(i.type);
            }
            else if (i.createTile != -1)//方块
            {
                ItemsSort_Gets2(ID_Tile, ID_Tile_tile)?.Items?.Add(i.type);
            }
            else if (i.createWall != -1)//墙
            {
                ItemsSort_Gets2(ID_Tile, ID_Tile_wall)?.Items?.Add(i.type);
            }
            else if (i.type <= ItemID.Sets.SortingPriorityBossSpawns?.Length
                && ItemID.Sets.SortingPriorityBossSpawns[i.type] != -1)//boss召唤物
            {
                ItemsSort_Gets2(ID_BossSpawn)?.Items?.Add(i.type);
            }
            else if (i.buffType != 0)//药水
            {
                ItemsSort_Gets2(ID_Buff)?.Items?.Add(i.type);
            }
            else if (i.consumable)//消耗品
            {
                ItemsSort_Gets2(ID_Consumable)?.Items?.Add(i.type);
            }
            else//其它
            {
                ItemsSort_Gets2(ID_Other)?.Items?.Add(i.type);
            }
        }
        protected static bool load_id_sort_a1(int index, bool[] bs)
        {
            if (bs == null) return false;
            if (0 > index || index >= bs.Length) return false;

            return bs[index];
        }

        public static ItemSort<int> ItemsSort_Gets(int id1, int? id2 = null)
        {
            if (!Loaded) return null;

            return ItemsSort_Gets2(id1, id2);
        }

        protected static ItemSort<int> ItemsSort_Gets2(int id1, int? id2 = null)
        {
            if (!ID_initialized1) return null;
            if (id1 < 0 || (id2 != null && id2.Value < 0)) return null;

            if (id1 > ID.ItemsSort.Count) return null;

            if (id2 != null)
            {
                if (id2 > ID.ItemsSort[id1].ItemsSort.Count) return null;

                return ID.ItemsSort[id1].ItemsSort[id2.Value];
            }

            return ID.ItemsSort[id1];
        }
    }
}
