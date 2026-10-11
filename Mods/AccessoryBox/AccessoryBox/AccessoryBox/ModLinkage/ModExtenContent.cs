using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using tContentPatch;
using tContentPatch.ModLoad;
using Terraria;
using Terraria.ID;

namespace AccessoryBox.ModLinkage
{
    internal class ModExtenContent : Mod
    {
        private readonly static Dictionary<string, MethodInfo> mis = new Dictionary<string, MethodInfo>();
        private static PropertyInfo _ExtenItem_Item = null;

        public override void Loaded()
        {
            List<ModObject> mos = ContentPatch.GetModObjects();
            if (mos == null) return;

            ModObject mo = mos.FirstOrDefault(i => i.config.key == "StaticTile.ExtenContent");
            if (mo == null) return;

            Assembly assembly = (Assembly)mo.assembly?.GetType("ExtenContentPatch.ThisMod")?.GetProperty("assembly")?.GetValue(null);
            if (assembly == null) return;

            Type ExtenItem = assembly.GetType("ExtenContent.Extens.ExtenItem");
            if (ExtenItem == null) return;

            _ExtenItem_Item = ExtenItem.GetProperty("Item");
            if (_ExtenItem_Item == null) return;

            Type ExtenManag = assembly.GetType("ExtenContent.Extens.ExtenManag");
            if (ExtenManag == null) return;

            if (Register(nameof(GetExtenItemType), ExtenManag.GetMethod("GetExtenItemType"))) return;
            if (Register(nameof(GetExtenItemKey), ExtenManag.GetMethod("GetExtenItemKey"))) return;
            if (Register(nameof(RegisterUnloadItem), ExtenManag.GetMethod("RegisterUnloadItem"))) return;
            if (Register(nameof(GetUnloadItemKey), ExtenManag.GetMethod("GetUnloadItemKey"))) return;
            if (Register(nameof(SetUnloadItem), ExtenManag.GetMethod("SetUnloadItem"))) return;
        }

        public override void Unload()
        {
            mis.Clear();
        }

        private static bool Register(string key, MethodInfo mi)
        {
            if (mi == null)
            {
                mis.Clear();
                return true;
            }
            mis.Add(key, mi);
            return false;
        }

        private static bool Invoke<T>(string name, out T val, params object[] args)
        {
            val = default;
            if (mis.TryGetValue(name, out MethodInfo mi) != true) return false;

            val = (T)mi.Invoke(null, args);

            return true;
        }

        public static int GetExtenItemType(string key)
        {
            if (Invoke(nameof(GetExtenItemType), out int v, key)) return v;

            return ItemID.None;
        }

        public static string GetExtenItemKey(int type)
        {
            if (Invoke(nameof(GetExtenItemKey), out string v, type)) return v;

            return null;
        }

        public static void RegisterUnloadItem(string key)
        {
            Invoke(nameof(RegisterUnloadItem), out object _, key);
        }

        public static string GetUnloadItemKey(string key)
        {
            if (Invoke(nameof(GetUnloadItemKey), out string v, key)) return v;

            return null;
        }

        public static void SetUnloadItem(Item item, string key)
        {
            Invoke(nameof(SetUnloadItem), out object _, item, key);
        }

        /// <summary>
        /// 获取卸载物品的key, 没有则获取扩展物品的key, 没有则返回<see langword="null"/>
        /// </summary>
        public static string GetExtenItemOrUnLoadItemKey(Item item)
        {
            string key = GetUnloadItemKey(item.Name);
            if (key == null) key = GetExtenItemKey(item.type);

            return key;
        }
    }
}
