using System;
using System.Linq;
using System.Reflection;
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

            ModObject mo = mos.FirstOrDefault(i => i.config.key == "StaticTile.ExtenContent");
            if (mo == null) return;

            Assembly assembly = (Assembly)mo.assembly?.GetType("ExtenContentPatch.ThisMod")?.GetProperty("assembly")?.GetValue(null);
            if (assembly == null) return;

            ItemCount = LoadCount(assembly, ItemID.Count, "ExtenContent.Extens.ItemLoad", "ItemCount");
            ContentPatch.PrintTry($"{nameof(CreativeInventory)}:ModLinkage:ExtenContent:{nameof(ItemCount)}:{ItemCount}");

            ProjectileCount = LoadCount(assembly, ProjectileID.Count, "ExtenContent.Extens.ProjectileLoad", "ProjectileCount");
            ContentPatch.PrintTry($"{nameof(CreativeInventory)}:ModLinkage:ExtenContent:{nameof(ProjectileCount)}:{ProjectileCount}");
        }

        protected int LoadCount(Assembly assembly, int def, string path, string name)
        {
            Type type = assembly.GetType(path);
            if (type == null) return def;

            PropertyInfo pi = type.GetProperty(name);
            if (pi == null) return def;

            return pi.GetValue(null) as int? ?? def;
        }
    }
}
