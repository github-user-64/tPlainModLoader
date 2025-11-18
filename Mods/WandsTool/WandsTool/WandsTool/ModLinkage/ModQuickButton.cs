using System;
using System.Linq;
using tContentPatch;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using WandsTool.Content;

namespace WandsTool.ModLinkage
{
    public class ModQuickButton : Mod
    {
        public static bool IsLinkage = true;

        public override void Loaded()
        {
            if (IsLinkage == false) return;

            System.Collections.Generic.List<tContentPatch.ModLoad.ModObject> mos = ContentPatch.GetModObjects();
            if (mos == null) return;

            tContentPatch.ModLoad.ModObject mo = mos.FirstOrDefault(i => i.assembly?.GetName().Name == "QuickButton");
            if (mo == null) return;

            Type type = mo.assembly.GetType("QuickButton.QuickButton.QuickButton");
            if (type == null) return;

            System.Reflection.MethodInfo mi = type.GetMethod("Add", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            if (mi == null) return;

            UIImage ui_img = new UIImage(tContentPatch.Utils.Resource.GetTexture2D($"{nameof(WandsTool)}.Resources.Wand.png"));
            ui_img.Width.Pixels = 32;
            ui_img.Height.Pixels = 32;
            ui_img.ScaleToFit = true;
            ui_img.OnUpdate += _ =>
            {
                if (ui_img.IsMouseHovering) Main.instance.MouseText($"魔杖-{(gameMain.Wand_isEnable ? "启用" : "禁用")}({WandAction.Count})");
            };
            ui_img.OnLeftClick += (e, s) =>
            {
                SoundEngine.PlaySound(12);

                gameMain.Wand_isEnable = !gameMain.Wand_isEnable;
            };

            mi.Invoke(null, new object[] { "WandsTool.SwitchOpenOrClose", ui_img });
        }
    }
}
