using System.Collections.Generic;
using tContentPatch.ModLoad;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.UI;

namespace tContentPatch.Content.Menus.ModSetSwitch
{
    internal class ModSetSwitch
    {
        private static UIModSetSwitch uistate = null;

        public static void OpenModSetSwitchMenu(UIState backUI, List<ModObject> mos = null, ModObject open = null)
        {
            if (Main.dedServ) return;
            if (Main.showSplash) return;

            if (uistate == null)
            {
                uistate = new UIModSetSwitch();
                uistate.NoGamepadSupport = true;//不支持游戏手柄, 取消按移动键鼠标会定位到ui上的功能
            }

            uistate.InitializeModList(backUI, mos, open);

            GameUitl.OpenMenu(uistate);

            SoundEngine.PlaySound(SoundID.MenuOpen);
        }

        public static void OpenModSet(List<ModSetting> mss, ModSetting open = null)
        {
            ModSet.ModSet.OpenModSetMenu(uistate, mss, open);
        }
    }
}
