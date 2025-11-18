using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using tContentPatch;
using Terraria.UI;

namespace QuickSetting.QuickSetting
{
    public class QuickSetting : Mod
    {
        public static Action<string> OnAddItem = null;
        public static Action<string, string> OnSwitchItem = null;
        private static UIQuickSetting ui_qs = null;
        private static List<string> keyOrder = null;
        private static string key = null;

        public override void Load()
        {
            ui_qs = new UIQuickSetting("设置", 350, 600);
            ui_qs.OnAddItem += (s1) =>
            {
                OnAddItem?.Invoke(s1);
                if (keyOrder != null) ui_qs.KeyOrder(keyOrder);
            };
            ui_qs.OnSwitchItem += (s1, s2) =>
            {
                OnSwitchItem?.Invoke(s1, s2);
                if (keyOrder != null) ui_qs.KeyOrder(keyOrder);
            };
        }

        public static void SwitchOpenOrClose()
        {
            if (ui_qs.IsOpen) ui_qs.Close();
            else ui_qs.Open(ModifyInterfaceLayers.ui_state);
        }

        public static void AddItem(Texture2D ico, string name, UIElement uie)
        {
            ui_qs.AddItem(ico, name, uie);
        }

        public static void SetKeyOrder(List<string> keyOrder)
        {
            QuickSetting.keyOrder = keyOrder;
        }

        public static void SetBind(string key)
        {
            ListenInput.DelListenInput(QuickSetting.key, foo);

            QuickSetting.key = key;

            ListenInput.AddListenInput(QuickSetting.key, foo);
        }

        private static void foo(bool isOne)
        {
            if (isOne) SwitchOpenOrClose();
        }
    }
}
