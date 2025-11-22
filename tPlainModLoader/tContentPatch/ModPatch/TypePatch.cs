using System;
using System.Collections.Generic;

namespace tContentPatch.ModPatch
{
    internal class TypePatch
    {
        private Dictionary<Type, IListPlain> list = new Dictionary<Type, IListPlain>();

        public ListCopy<T> Get<T>()
        {
            return (ListCopy<T>)list[typeof(T)];
        }

        public void AddPatch<T>(ListCopy<T> patch)
        {
            list.Add(typeof(T), patch);
        }

        public void ClearAllPatch()
        {
            foreach (KeyValuePair<Type, IListPlain> i in list)
            {
                i.Value.Clear();
            }
        }
    }
}
