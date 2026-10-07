using AccessoryBox.ModLinkage;
using System.Collections.Generic;
using tContentPatch.Utils;
using Terraria;
using Terraria.ID;

namespace AccessoryBox.LoadItem
{
    public static class ItemDataLoad
    {
        private const string File = "items.json";

        public static List<Item> LoadData()
        {
            List<ItemData> data = null;

            ModFile.ReadFileTry(File, file =>
            {
                data = MyJson1.Get2(file, typeof(List<ItemData>)) as List<ItemData>;
                return true;
            }, ThisMod.mo);

            if (data == null) return null;
            return DatasToItems(data);
        }

        public static void SaveData(List<Item> items)
        {
            ModFile.SaveFileTry(File, file =>
            {
                MyJson1.Save(ItemsToDatas(items), file, true);
                return true;
            }, ThisMod.mo);
        }

        public static List<Item> DatasToItems(List<ItemData> data)
        {
            return data.ConvertAll(i =>
            {
                Item item = new Item();

                if (i.key == null)
                {
                    item.SetDefaults(i.type);
                }
                else
                {
                    int type = ModExtenContent.GetExtenItemType(i.key);//获取对应key的扩展物品的type

                    if (type == ItemID.None)
                    {
                        //注册并设为卸载物品
                        ModExtenContent.RegisterUnloadItem(i.key);
                        ModExtenContent.SetUnloadItem(item, i.key);
                    }
                    else
                    {
                        item.SetDefaults(type);
                    }
                }

                item.Prefix(i.prefix);
                return item;
            });
        }

        public static List<ItemData> ItemsToDatas(List<Item> items)
        {
            return items.ConvertAll(i =>
            {
                string key = ModExtenContent.GetExtenItemOrUnLoadItemKey(i);
                if (key == null) return new ItemData(i.type, i.prefix);

                return new ItemData(key, i.prefix);
            });
        }
    }
}
