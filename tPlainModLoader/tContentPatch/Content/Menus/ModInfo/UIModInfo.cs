using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using tContentPatch.Content.UI;
using tContentPatch.ModLoad;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;

namespace tContentPatch.Content.Menus.ModInfo
{
    internal class UIModInfo : UIState
    {
        public Action<ModObject> ActionDelMod = null;
        public UIState BackUI = null;
        private ModObject mo = null;
        private UIScrollViewer2 ui_sv = null;
        private Action<string> SetTitle = null;
        private UIButton1 ui_btn3 = null;
        private UIButton1 ui_btn4 = null;

        public UIModInfo()
        {
            UIElement uie = new UIElement();
            uie.Width.Precent = .5f;
            uie.Height.Precent = .8f;
            uie.HAlign = .5f;
            uie.VAlign = .5f;
            uie.SetPadding(10);

            UIPanel ui_panel = new UIPanel();
            ui_panel.Width.Precent = 1;
            ui_panel.Height.Set(-200, 1);
            ui_panel.HAlign = .5f;
            ui_panel.VAlign = .5f;
            ui_panel.SetPadding(20);

            ui_sv = new UIScrollViewer2();
            ui_sv.Width.Precent = 1;
            ui_sv.Height.Precent = 1;
            ui_sv.ItemMargin = 4;

            UITextPanel<string> ui_title = new UITextPanel<string>(string.Empty, 0.8f, true);
            ui_title.Top.Pixels = -60;
            ui_title.HAlign = .5f;
            SetTitle = s => ui_title.SetText(s);

            UIWrapPanel ui_wp = new UIWrapPanel();
            ui_wp.Width.Precent = 1;
            ui_wp.Height.Pixels = 100 - 4;
            ui_wp.Top.Set(-(100 - 4), 1);
            ui_wp.ItemMargin = 2;

            Func<string, Action, UIButton1> getBtn = (text, action) =>
            {
                UIButton1 btn = new UIButton1(text);
                btn.Width.Set(-ui_wp.ItemMargin, 1f / 3);
                btn.OnLeftClick += (e, s) => action();
                return btn;
            };

            UIButton1 ui_btn1 = getBtn("返回", () => Back(BackUI));
            UIButton1 ui_btn2 = getBtn("打开文件夹", () => ModInfo.OpenModDirectory(mo));
            ui_btn3 = getBtn("删除", () =>
            {
                ActionDelMod?.Invoke(mo);
                Back(BackUI);
            });
            ui_btn4 = getBtn("跳转到相关链接", () => ModInfo.JumpTo(mo.info));

            Append(uie);
            uie.Append(ui_panel);
            uie.Append(ui_wp);
            ui_wp.Append(ui_btn1);
            ui_wp.Append(ui_btn2);
            ui_wp.Append(ui_btn3);
            ui_wp.Append(ui_btn4);
            ui_panel.Append(ui_title);
            ui_panel.Append(ui_sv);
        }

        public void InitializeMod(ModObject mo = null, Action<ModObject> ActionDelMod = null)
        {
            this.mo = mo;

            this.ActionDelMod = null;
            ui_btn3.isEnable = false;
            ui_btn4.isEnable = false;
            ui_sv.ClearChild();

            if (this.mo == null)
            {
                SetTitle("模组信息");
                return;
            }

            SetTitle($"模组信息: {this.mo.info?.name ?? this.mo.config.key}");
            ui_btn3.isEnable = ModInfo.GetModIsLoaded(this.mo) == false;
            ui_btn4.isEnable = this.mo.info?.jumpPath != null;

            InitializeInfoText();

            this.ActionDelMod = ActionDelMod;
        }
        private void InitializeInfoText()
        {
            if (mo.info?.description == null) return;

            string text = mo.info.description;

            List<string> texts = new List<string>();

            while (text.Length > 0)
            {
                int start = text.IndexOf('\n');
                int textLen = start == -1 ? text.Length : start;
                int rLen = start == -1 ? text.Length : start + 1;

                string t = text.Substring(0, textLen);

                text = text.Remove(0, rLen);

                texts.Add(t);
            }

            int widthMax = (int)ui_sv.GetInnerDimensions().Width - 40;

            foreach (string s in texts)
            {
                string[] ss = Terraria.Utils.WordwrapString(s, FontAssets.MouseText.Value,
                    widthMax, 16, out int lineAmount);

                for (int i = 0; i < lineAmount + 1; ++i)
                {
                    ui_sv.AddChild(new UIText(ss[i]));
                }
            }
        }

        private void Back(UIState BackUI)
        {
            if (BackUI == null)
            {
                if (Main.gameMenu) Main.menuMode = 0;
                else IngameFancyUI.Close();
            }
            else
            if (Main.gameMenu)
            {
                Main.menuMode = 888;
                Main.MenuUI.SetState(BackUI);
            }
            else
            {
                Main.InGameUI.SetState(BackUI);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (PlayerInput.Triggers.JustPressed.Inventory) Back(BackUI);

            if (ui_btn3.IsMouseHovering && ui_btn3.isEnable == false) DrawTip.SetDraw("卸载模组以删除" );
            if (ui_btn4.IsMouseHovering && ui_btn4.isEnable)
            {
                string path = mo.info?.jumpPath;
                if (path != null) DrawTip.SetDraw($"不要相信可疑链接.跳转到:{path}");
            }
        }
    }
}
