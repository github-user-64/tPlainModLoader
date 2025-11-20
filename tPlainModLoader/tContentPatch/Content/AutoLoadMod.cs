using HarmonyLib;
using tContentPatch.ModLoad;
using Terraria;

namespace tContentPatch.Content
{
    internal class AutoLoadMod
    {
        private static bool oneLoadMod = true;

        [HarmonyPatch(typeof(Main), "Update")]
        private class Main_Update
        {
            static Main_Update()
            {
                LoaderControl.OnModLoad_Start += _ => oneLoadMod = false;
            }

            internal static void Prefix(object gameTime)
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
}
