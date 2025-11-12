using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using tContentPatch.Content.UI;
using tContentPatch.ModLoad;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;

namespace tContentPatch.Content.Menus.ModManager
{
    internal class UIModManager : UIState
    {
        public static readonly Color color_isEnable = Color.Green;
        public static readonly Color color_isEnable_false = Color.Red;

        public UIState backUI = null;

        private UIPanel ui_panel = null;
        private UITextPanel<string> ui_panel_title = null;
        private UIScrollViewer2 ui_sv = null;

        private UIWrapPanel ui_buttons_wrap = null;
        private UIButton1[] ui_buttons = null;

        public UIModManager()
        {
            ui_panel = new UIPanel();
            ui_panel.Width.Pixels = 600;
            ui_panel.Height.Set(-300, 1);
            ui_panel.HAlign = 0.5f;
            ui_panel.VAlign = 0.5f;
            ui_panel.BackgroundColor = new Color(31, 41, 71, 200);

            ui_panel_title = new UITextPanel<string>("模组列表", 0.8f, true);
            ui_panel_title.HAlign = 0.5f;
            ui_panel_title.BackgroundColor = new Color(73, 94, 171);

            ui_sv = new UIScrollViewer2();
            ui_sv.Width.Precent = 1;
            ui_sv.Height.Set(-10, 1);
            ui_sv.Top.Pixels = 10;
            ui_sv.ItemMargin = 4;

            ui_buttons_wrap = new UIWrapPanel();
            ui_buttons_wrap.Width.Pixels = 600;
            ui_buttons_wrap.Top.Set(ui_panel.Height.Pixels / 2 + 4, 1);
            ui_buttons_wrap.HAlign = 0.5f;
            ui_buttons_wrap.ItemMargin = 4;

            ui_buttons = new UIButton1[]
            {
                new UIButton1("启用全部模组"),
                new UIButton1("禁用全部模组"),
                new UIButton1("重新加载模组"),
                new UIButton1("返回"),
                new UIButton1("打开模组文件夹"),
                new UIButton1("模组设置"),
            };

            ui_buttons[0].TextColor = color_isEnable;
            ui_buttons[0].OnLeftClick += (e, s) => ModManager.ModEnableAll();
            ui_buttons[1].TextColor = color_isEnable_false;
            ui_buttons[1].OnLeftClick += (e, s) => ModManager.ModNoEnableAll();
            ui_buttons[2].OnLeftClick += (e, s) => ModManager.AgainLoadMod();
            ui_buttons[2].OnUpdate += (e) => { if (ui_buttons[2].IsMouseHovering) DrawTip.SetDraw("建议重启程序"); };
            ui_buttons[3].OnLeftClick += (e, s) => Button_Back_Click(backUI);
            ui_buttons[3].OnUpdate += (e) => { if (ui_buttons[3].IsMouseHovering && ModManager.CheckModUpdate()) DrawTip.SetDraw("需要重新加载, 建议重启程序"); };
            ui_buttons[4].OnLeftClick += (e, s) => ModManager.OpenModDirectory();
            ui_buttons[5].OnLeftClick += (e, s) => ModManager.OpenModSetSwitch();

            Append(ui_panel);
            Append(ui_buttons_wrap);
            ui_panel.Append(ui_panel_title);
            ui_panel.Append(ui_sv);

            foreach (UIButton1 i in ui_buttons) ui_buttons_wrap.Append(i);
        }

        public void InitializeModList(List<ModObject> mos)
        {
            ui_sv.ClearChild();
            if (mos != null)
            {
                foreach (ModObject mo in mos) if (mo.config.isEnable) ui_sv.AddChild(new UIModItem(this, mo));
                foreach (ModObject mo in mos) if (!mo.config.isEnable) ui_sv.AddChild(new UIModItem(this, mo));
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            ui_panel_title.Top.Pixels = -ui_panel_title.GetOuterDimensions().Height / 2 - 20;

            foreach (UIButton1 i in ui_buttons) i.Width.Pixels = (int)(ui_buttons_wrap.GetInnerDimensions().Width - ((3 - 1) * ui_buttons_wrap.ItemMargin)) / 3;

            ui_buttons_wrap.UpdateContainer_Height();

            if (PlayerInput.Triggers.JustPressed.Inventory) Button_Back_Click(backUI);
        }

        private void Button_Back_Click(UIState backUi)
        {
            if (ModManager.CheckModUpdate())
            {
                ModManager.AgainLoadMod();
                return;
            }

            if (backUi == null)
            {
                Main.menuMode = 0;
                return;
            }

            Main.menuMode = 888;
            Main.MenuUI.SetState(backUi);
        }
    }
}
