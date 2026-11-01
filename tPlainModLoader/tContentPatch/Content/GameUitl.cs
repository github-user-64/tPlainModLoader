using Terraria;
using Terraria.ID;
using Terraria.UI;

namespace tContentPatch.Content
{
    /// <summary/>
    public static class GameUitl
    {
        /// <summary/>
        public static bool OpenMenu(UIState uistate)
        {
            if (Main.dedServ) return false;
            if (Main.showSplash) return false;

            if (Main.gameMenu)
            {
                Main.menuMode = MenuID.FancyUI;
                Main.MenuUI.SetState(uistate);
            }
            else
            {
                IngameFancyUI.OpenUIState(uistate, true);
            }

            return true;
        }

        /// <summary>
        /// 返回菜单, <paramref name="back"/>为<see langword="null"/>则关闭
        /// </summary>
        public static void BackMenu(UIState back = null)
        {
            if (back == null)
            {
                if (Main.gameMenu) Main.menuMode = MenuID.Title;
                else IngameFancyUI.Close();

                return;
            }

            OpenMenu(back);
        }

        /// <summary>
        /// 返回菜单
        /// </summary>
        public static void BackMenu(int back)
        {
            if (Main.gameMenu) Main.menuMode = back;
            else IngameFancyUI.Close();
        }

        /// <summary>
        /// 能否使用游戏内的ui(在游戏内, 没有打开大地图, 没有打开游戏选择窗口, 没有打开特殊ui)<br/>
        /// 主要用于<see cref="PatchMain.SetupDrawInterfaceLayersPostfix(System.Collections.Generic.List{GameInterfaceLayer})"/>
        /// </summary>
        public static bool CanGameUI()
        {
            return Main.gameMenu != true
                && Main.mapFullscreen != true
                && Main.ingameOptionsWindow != true
                && Main.inFancyUI != true;
        }
    }
}
