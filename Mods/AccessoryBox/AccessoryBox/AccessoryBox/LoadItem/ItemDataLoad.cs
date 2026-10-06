using AccessoryBox.ModLinkage;
using System.Collections.Generic;
using tContentPatch;
using tContentPatch.Utils;
using Terraria;
using Terraria.ID;

namespace AccessoryBox.LoadItem
{
    public static class ItemDataLoad
    {
        private class EnterWorldLoad : PatchMain
        {
            public override void OnEnterWorld() => LoadData();
        }

        private const string File = "items.json";

        public static void LoadData()
        {
            List<ItemData> data = null;

            ModFile.ReadFileTry(File, file =>
            {
                data = MyJson1.Get2(file, typeof(ItemData)) as List<ItemData>;
                return true;
            }, ThisMod.mo);

            if (data == null)
            {
                data = new List<ItemData>();
                data.Add(new ItemData());
                data.Add(new ItemData());
            }

            Common.AccessoryBox.LoadItems(DatasToItems(data));
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
                    int type = ModExtenContent.GetItemType(i.key);

                    if (type == ItemID.None)
                    {
                        ModExtenContent.RegisterUnload(i.key);
                        ModExtenContent.SetUnloadItem(item, i.key);
                    }
                    else
                    {
                        item.SetDefaults(i.type);
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
                string key = ModExtenContent.GetExtenKey(i);
                if (key == null) return new ItemData(i.type, i.prefix);

                return new ItemData(key, i.prefix);
            });
        }
    }
}
