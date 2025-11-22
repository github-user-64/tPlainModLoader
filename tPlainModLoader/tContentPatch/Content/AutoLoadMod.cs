using HarmonyLib;
using Microsoft.Xna.Framework;
using tContentPatch.ModLoad;
using Terraria;

namespace tContentPatch.Content
{
    [HarmonyPatch(typeof(Main), "Update")]
    internal class AutoLoadMod
    {
        private static bool oneLoadMod = true;

        static AutoLoadMod()
        {
            LoaderControl.OnModLoad_Start += _ => oneLoadMod = false;
        }

        internal static void Prefix(GameTime gameTime)
        {
            if (oneLoadMod == false) return;
            if (Main.dedServ) return;
            if (Main.showSplash) return;

            if (Main.gameMenu == false) return;
            if (Main.menuMode != Terraria.ID.MenuID.Title) return;
            if (LoaderControl.CanLoad == false) return;

            oneLoadMod = false;
            LoaderControl.Load();
        }
    }
}
