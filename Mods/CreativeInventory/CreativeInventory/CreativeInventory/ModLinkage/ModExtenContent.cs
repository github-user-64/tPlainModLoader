using System;
using System.Linq;
using tContentPatch;
using tContentPatch.ModLoad;
using Terraria.ID;

namespace CreativeInventory.ModLinkage
{
    public class ModExtenContent : Mod
    {
        public static bool IsLinkage = true;
        public static int ItemCount { get; protected set; } = ItemID.Count;
        public static int ProjectileCount { get; protected set; } = ProjectileID.Count;

        public override void Loaded()
        {
            if (IsLinkage == false) return;

            System.Collections.Generic.List<ModObject> mos = ContentPatch.GetModObjects();
            if (mos == null) return;

            ModObject mo = mos.FirstOrDefault(i => i.assembly?.GetName().Name == "ExtenContent");
            if (mo == null) return;

            ItemCount = LoadCount(mo, ItemID.Count, "ExtenContent.Extens.ItemLoad", "ItemCount");
            ProjectileCount = LoadCount(mo, ProjectileID.Count, "ExtenContent.Extens.ProjectileLoad", "ProjectileCount");
        }

        protected int LoadCount(ModObject mo, int def, string path, string name)
        {
            Type type = mo.assembly.GetType(path);
            if (type == null) return def;

            System.Reflection.PropertyInfo pi = type.GetProperty(name);
            if (pi == null) return def;

            return pi.GetValue(null) as int? ?? def;
        }
    }
}
