using AccessoryBox.LoadItem;
using System;
using System.Collections.Generic;
using Terraria;

namespace AccessoryBox.Common
{
    internal partial class AccessoryBox : IBoxConsole
    {
        public event Action OnLoaded;
        public event Action<Item> OnAdded;
        public event Action<Item> OnDeled;
        public event Action<Item, Item> OnSetItemed;
        public event Action OnClearItemed;

        public void ClearItem()
        {
            armor.Clear();

            OnClearItemed?.Invoke();
        }

        public void DelItem(Item item)
        {
            armor.Remove(item);

            OnDeled?.Invoke(item);
        }

        public bool GetEnable()
        {
            return Enable;
        }

        public List<Item> GetItems()
        {
            return armor;
        }

        public void Load()
        {
            List<Item> items = ItemDataLoad.LoadData();

            if (items == null)
            {
                items = new List<Item>();
                items.Add(new Item());
                items.Add(new Item());
            }

            LoadItems(items);

            OnLoaded?.Invoke();
        }

        public void Save()
        {
            ItemDataLoad.SaveData(armor);
            Config.Instance?.Save();
        }

        public void SetEnable(bool val)
        {
            Enable = val;
        }

        public void SetItem(Item item, Item val)
        {
            int index = armor.IndexOf(item);
            if (index == -1) return;

            val = val?.Clone() ?? new Item();

            armor[index] = val;

            OnSetItemed?.Invoke(item, val);
        }

        void IBoxConsole.AddItem(Item item)
        {
            item = AddItem(item);
            if (item == null) return;

            OnAdded?.Invoke(item);
        }
    }
}
