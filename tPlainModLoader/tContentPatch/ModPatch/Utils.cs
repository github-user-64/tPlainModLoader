using System;
using System.Collections.Generic;

namespace tContentPatch.ModPatch
{
    internal abstract class ListCopy<T> : IListPlain
    {
        public List<T> list = null;

        public ListCopy(List<T> list)
        {
            this.list = list;
        }

        public virtual void Clear()
        {
            list.Clear();
        }
        public virtual void AddRange(List<T> list)
        {
            if (list == null) return;
            this.list.AddRange(list);
        }

        void IListPlain.AddRange(object list)
        {
            AddRange((List<T>)list);
        }
    }

    internal static class ListHelp
    {
        public static void For<T>(this List<T> list, Action<T> action)
        {
            foreach (T item in list) action(item);
        }
    }
}
