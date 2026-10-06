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

            Type ItemLoad = assembly.GetType("ExtenContent.Extens.ItemLoader");
            if (ItemLoad == null) return;

            if (Register(nameof(TypeInRange), ItemLoad.GetMethod("TypeInRange"))) return;
            if (Register(nameof(GetItemType), ItemLoad.GetMethod("GetItemType"))) return;
            if (Register(nameof(GetItemKey), ItemLoad.GetMethod("GetItem", new Type[] { typeof(int) }))) return;
            if (Register(nameof(RegisterUnload), ItemLoad.GetMethod("RegisterUnload"))) return;
            if (Register(nameof(GetUnloadItemKey), ItemLoad.GetMethod("GetUnloadItemKey"))) return;
            if (Register(nameof(SetUnloadItem), ItemLoad.GetMethod("SetUnloadItem"))) return;
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
            return true;
        }

        private static bool Invoke<T>(string name, out T val, params object[] args)
        {
            val = default;
            if (mis.TryGetValue(name, out MethodInfo mi) != true) return false;

            val = (T)mi.Invoke(null, args);

            return true;
        }

        public static bool TypeInRange(int type)
        {
            if (Invoke(nameof(TypeInRange), out bool v, type)) return v;

            return false;
        }

        public static int GetItemType(string key)
        {
            int? type = null;

            if (Invoke(nameof(GetItemType), out object v, key))
            {
                type = _ExtenItem_Item.GetValue(v) as int?;
            }

            return type ?? ItemID.None;
        }

        public static string GetItemKey(int type)
        {
            if (Invoke(nameof(GetItemKey), out string v, type)) return v;

            return null;
        }

        public static void RegisterUnload(string key)
        {
            Invoke(nameof(RegisterUnload), out object _, key);
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
        /// 获取扩展物品的key, 没有则获取卸载物品的key, 没有则返回<see langword="null"/>
        /// </summary>
        public static string GetExtenKey(Item item)
        {
            if (TypeInRange(item.type) != true) return null;

            string key = GetItemKey(item.type);
            if (key == null) key = GetUnloadItemKey(item.Name);

            return key;
        }
    }
}
