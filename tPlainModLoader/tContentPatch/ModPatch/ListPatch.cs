using System;
using System.Collections.Generic;
using tContentPatch.Patch;

namespace tContentPatch.ModPatch
{
    internal class ListPatch
    {
        private Dictionary<Type, IPatch> list = new Dictionary<Type, IPatch>();
        private AddPatch patch = null;

        public ListPatch(AddPatch patch)
        {
            this.patch = patch;
        }

        public ClassPatch<T> Get<T>()
        {
            return (ClassPatch<T>)list[typeof(T)];
        }

        public void AddPatchAndInit<T>(ClassPatch<T> patch)
        {
            patch.Initialize(this.patch);
            list.Add(typeof(T), patch);
        }

        public void ClearAllPatch()
        {
            foreach (KeyValuePair<Type, IPatch> i in list)
            {
                i.Value.Clear();
            }
        }
    }
}
