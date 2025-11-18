using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativeInventory
{
    public class ItemSort<T>
    {
        public readonly List<T> Items = null;
        public readonly Dictionary<int, ItemSort<T>> ItemsSort = null;


        public ItemSort()
        {
            Items = new List<T>();
            ItemsSort = new Dictionary<int, ItemSort<T>>();
        }

        public void for_ItemsAll(Action<T> action)
        {
            if (action == null) return;

            for (int i = 0; i < Items?.Count; ++i)
            {
                action.Invoke(Items[i]);
            }

            for (int i = 0; i < ItemsSort?.Count; ++i)
            {
                ItemsSort?.ElementAt(i).Value?.for_ItemsAll(action);
            }
        }
    }
}
