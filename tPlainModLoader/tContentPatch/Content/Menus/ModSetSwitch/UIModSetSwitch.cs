using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using tContentPatch.Content.UI;
using tContentPatch.ModLoad;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;

namespace tContentPatch.Content.Menus.ModSetSwitch
{
    internal class UIModSetSwitch : UIState
    {
        private UIState backUI = null;
        private Action<List<(string, Action, bool)>> initModList = null;
        private Action<List<(string, Action, bool)>> initModSet = null;

        public UIModSetSwitch()
        {
            UIPanel ui_panel = new UIPanel();
            ui_panel.Width.Pixels = 800;
            ui_panel.Height.Set(-300, 1);
            ui_panel.HAlign = 0.5f;
            ui_panel.VAlign = 0.5f;
            ui_panel.BackgroundColor = Color.MidnightBlue * 0.8f;

            UITextPanel<string> ui_title = new UITextPanel<string>("模组设置", 0.8f, true);
            ui_title.Top.Pixels = -40;
            ui_title.HAlign = 0.5f;
            ui_title.BackgroundColor = new Color(73, 94, 171);

            UIStackPanel ui_sp = new UIStackPanel();
            ui_sp.Width.Precent = 1;
            ui_sp.Height.Precent = 1;
            ui_sp.Horizontal = true;
            ui_sp.IsAutoUpdateSize = true;
            ui_sp.ItemMargin = 10;

            float width = ui_panel.Width.Pixels - ui_panel.PaddingLeft - ui_panel.PaddingRight;
            width -= ui_sp.ItemMargin;
            width /= 2;

            UIModScrollViewer ui_modList = new UIModScrollViewer("模组管理", true);
            ui_modList.Width.Pixels = width;
            ui_modList.Height.Precent = 1;
            initModList = ui_modList.InitializeList;

            UIModScrollViewer ui_modSet = new UIModScrollViewer("设置", false);
            ui_modSet.Width.Pixels = width;
            ui_modSet.Height.Precent = 1;
            initModSet = ui_modSet.InitializeList;

            UIButton1 ui_back = new UIButton1("返回", 0.8f, true);
            ui_back.Top.Set(ui_panel.Height.Pixels / 2 + 4, 1);
            ui_back.HAlign = 0.5f;
            ui_back.PaddingLeft = 50;
            ui_back.PaddingRight = 50;
            ui_back.OnLeftClick += (e, s) => Back(backUI);

            Append(ui_panel);
            Append(ui_back);
            ui_panel.Append(ui_sp);
            ui_panel.Append(ui_title);
            ui_sp.Append(ui_modList);
            ui_sp.Append(ui_modSet);
        }

        public void InitializeModList(UIState backUI, List<ModObject> mos, ModObject open = null)
        {
            this.backUI = backUI;

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


        public void Back(UIState backUI)
        {
            if (backUI == null)
            {
                if (Main.gameMenu) Main.menuMode = 0;
                else IngameFancyUI.Close();
            }
            else
            if (Main.gameMenu)
            {
                Main.menuMode = 888;
                Main.MenuUI.SetState(backUI);
            }
            else
            {
                Main.InGameUI.SetState(backUI);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (PlayerInput.Triggers.JustPressed.Inventory) Back(backUI);
        }
    }
}
