using System;
using System.Linq;
using System.Reflection;
using tContentPatch;
using tContentPatch.ModLoad;
using Terraria.ID;

namespace Skil.ModLinkage
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

            ItemCount = LoadCount(assembly, ItemID.Count, "ExtenContent.Extens.ItemLoader", "ItemCount");
            Print($"{nameof(ItemCount)}:{ItemCount}");

            ProjectileCount = LoadCount(assembly, ProjectileID.Count, "ExtenContent.Extens.ProjectileLoader", "ProjectileCount");
            Print($"{nameof(ProjectileCount)}:{ProjectileCount}");
        }

        protected void Print(string s)
        {
            ContentPatch.PrintTry($"{nameof(Skil)}:模组联动:扩展内容:{s}");
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
