using Terraria;
using Terraria.UI;

namespace tContentPatch.Content.Menus.CreateMod
{
    internal static class CreateMod
    {
        public static void OpenMenu(UIState backUI = null)
        {
            if (Main.dedServ) return;
            if (Main.showSplash) return;

            GameUitl.OpenMenu(new UICreateMod(backUI));
        }
    }
}
