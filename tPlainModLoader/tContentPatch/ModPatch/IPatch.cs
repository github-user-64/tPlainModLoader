using System.Collections.Generic;
using tContentPatch.Patch;

namespace tContentPatch.ModPatch
{
    internal interface IPatch
    {
        void Initialize(IAddPatch addPatch);
        void Clear();
    }

    internal abstract class ClassPatch<T> : IPatch
    {
        public List<T> list = null;

        public ClassPatch(List<T> list)
        {
            this.list = list;
        }

        public abstract void Initialize(IAddPatch addPatch);
        public virtual void Clear()
        {
            list.Clear();
        }
        public virtual void AddRange(List<T> list)
        {
            if (list == null) return;
            this.list.AddRange(list);
        }
    }
}
