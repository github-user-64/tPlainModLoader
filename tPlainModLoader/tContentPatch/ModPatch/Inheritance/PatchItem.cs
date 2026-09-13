using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Items;

namespace tContentPatch
{
    /// <summary/>
    public abstract class PatchItem
    {
        /// <summary>
        /// <see cref="ModSetting.Load(object)"/>后调用
        /// </summary>
        public virtual void Initialize() { }
        /// <summary/>
        public virtual void SetDefaultsPrefix(Item This, int Type, ItemVariant variant) { }
        /// <summary/>
        public virtual void SetDefaultsPostfix(Item This, int Type, ItemVariant variant) { }
        /// <summary/>
        public virtual void NewItemPostfix(int __result, IEntitySource source,
            Vector2 center, int type, int stack, int prefix,
            NewItemOwnership ownership,
            Vector2? velocity, Item.NewItemModifier modifier, bool noBroadcast)
        { }
    }
}
