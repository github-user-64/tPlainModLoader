using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Reflection;
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
            if (Main.dedServ) return;
            if (IsLinkage == false) return;

            System.Collections.Generic.List<tContentPatch.ModLoad.ModObject> mos = ContentPatch.GetModObjects();
            if (mos == null) return;

            tContentPatch.ModLoad.ModObject mo = mos.FirstOrDefault(i => i.assembly?.GetName().Name == "QuickButton");
            if (mo == null) return;

            Type type = mo.assembly.GetType("QuickButton.QuickButton.QuickButton");
            if (type == null) return;

            MethodInfo mi = type.GetMethod("Add", BindingFlags.Static | BindingFlags.Public);
            if (mi == null) return;

            UIImage ui_img = new UIImage(tContentPatch.Utils.AssemblyResource.Load<Texture2D>(
                $"{nameof(WandsTool)}.Resources.Wand.png",
                Assembly.GetExecutingAssembly()));
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
