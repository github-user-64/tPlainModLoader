using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using tContentPatch;
using Terraria;
using Terraria.UI;

namespace Skil.ModLinkage
{
    public class ModQuickSetting : Mod
    {
        public static bool IsLinkage_skil1_QuickSetting = true;
        public static bool IsLinkage_skil2_QuickSetting = true;
        public static bool IsLinkage_skil3_QuickSetting = true;
        public static bool IsLinkage_skil4_QuickSetting = true;

        public override void Loaded()
        {
            List<tContentPatch.ModLoad.ModObject> mos = ContentPatch.GetModObjects();
            if (mos == null) return;

            tContentPatch.ModLoad.ModObject mo = mos.FirstOrDefault(i => i.config.key == "StaticTile.QuickSetting");
            if (mo == null) return;

            Type type = mo.assembly.GetType("QuickSetting.QuickSetting.QuickSetting");
            if (type == null) return;

            System.Reflection.MethodInfo mi = type.GetMethod("AddItem", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            if (mi == null) return;

            if (IsLinkage_skil1_QuickSetting)
            {
                AddItem(mi, "Images/Item_3366", "技能列表1", Content.SkilListControl1.GetUI());
            }

            if (IsLinkage_skil2_QuickSetting)
            {
                AddItem(mi, "Images/Item_3366", "技能列表2", Content.SkilListControl2.GetUI());
            }

            if (IsLinkage_skil3_QuickSetting)
            {
                AddItem(mi, "Images/Item_3366", "技能列表3", Content.SkilListControl3.GetUI());
            }

            if (IsLinkage_skil3_QuickSetting)
            {
                AddItem(mi, "Images/Item_3366", "技能列表4", Content.SkilListControl4.GetUI());
            }
        }

        private static void AddItem(System.Reflection.MethodInfo mi, string ico, string text, List<UIElement> uis)
        {
            Texture2D texture = Main.Assets.Request<Texture2D>(ico, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            foreach (UIElement ui in uis) mi.Invoke(null, new object[] { texture, text, ui });
        }
    }
}
