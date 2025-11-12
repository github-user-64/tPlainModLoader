using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using tContentPatch;
using Terraria;
using Terraria.UI;

namespace OptimizeAndTool.ModLinkage
{
    public class ModQuickSetting : Mod
    {
        public static bool IsLinkage_OptimizeAndTool_QuickSetting = true;

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

            if (IsLinkage_OptimizeAndTool_QuickSetting)
            {
                AddItem(mi, "Images/Item_3611", "优化和工具", Content.Function.GetUI());
            }
        }

        private static void AddItem(System.Reflection.MethodInfo mi, string ico, string text, List<UIElement> uis)
        {
            Texture2D texture = Main.Assets.Request<Texture2D>(ico, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            foreach (UIElement ui in uis) mi.Invoke(null, new object[] { texture, text, ui });
        }
    }
}
