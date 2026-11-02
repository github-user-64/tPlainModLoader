using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using tContentPatch.Content.UI;
using tContentPatch.ModLoad;
using Terraria.UI;

namespace tContentPatch.Content.Menus.ModSetSwitch
{
    internal class UIModSetSwitch : UIMenuPanel
    {
        private Action<List<(string, Action, bool)>> initModList = null;
        private Action<List<(string, Action, bool)>> initModSet = null;

        public UIModSetSwitch() : base("模组设置")
        {
            MinWidth.Set(700f, 0f);
            MaxWidth.Set(900f, 0f);
            UIPanel.BackgroundColor = Color.MidnightBlue * 0.8f;

            UIModScrollViewer ui_modList = new UIModScrollViewer("模组管理", true);
            ui_modList.Width.Set(-5f, 0.5f);
            ui_modList.Height.Set(0f, 1f);
            ui_modList.HAlign = 0f;
            UIPanel.Append(ui_modList);
            initModList = ui_modList.InitializeList;

            UIModScrollViewer ui_modSet = new UIModScrollViewer("设置", false);
            ui_modSet.Width.Set(-5f, 0.5f);
            ui_modSet.Height.Set(-5f, 1f);
            ui_modSet.HAlign = 1f;
            UIPanel.Append(ui_modSet);
            initModSet = ui_modSet.InitializeList;

            UIButton1 ui_back = new UIButton1("返回", 0.8f, true);
            ui_back.PaddingLeft = 50f;
            ui_back.PaddingRight = 50f;
            ui_back.OnLeftClick += (e, s) => Back(BackUI);
            AddBtn(ui_back);
            ui_back.Width.Set(0f, 0f);
            ui_back.HAlign = 0.5f;
        }

        public void InitializeModList(UIState backUI, List<ModObject> mos, ModObject open = null)
        {
            BackUI = backUI;

            initModSet(null);

            if (mos == null)
            {
                initModList(null);
                return;
            }

            List<(string, Action, bool)> list = new List<(string, Action, bool)>();

            foreach (ModObject mo in mos)
            {
                (string, Action, bool) v = (null, null, false);

                v.Item1 = $"{mo.info?.name ?? mo.config.key}";

                bool? hasui = mo.inheritance_setting?.Exists(i => i.HasUI);

                if (hasui == true)
                {
                    v.Item2 = () => InitializeModSetList(mo);

                    if (mo == open) v.Item3 = true;
                }

                list.Add(v);
            }

            initModList(list);
        }

        private void InitializeModSetList(ModObject mo)
        {
            List<ModSetting> mss = mo.inheritance_setting.Where(i => i.HasUI).ToList();
            List<(string, Action, bool)> list = new List<(string, Action, bool)>();

            foreach (ModSetting ms in mss)
            {
                (string, Action, bool) v = (ms.Name, null, false);
                v.Item2 = () => ModSetSwitch.OpenModSet(mss, ms);

                list.Add(v);
            }

            initModSet(list);
        }
    }
}
