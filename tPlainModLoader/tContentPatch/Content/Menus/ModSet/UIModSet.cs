using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using tContentPatch.Content.UI;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;

namespace tContentPatch.Content.Menus.ModSet
{
    internal class UIModSet : UIState
    {
        private UIState backUI = null;
        private List<ModSetting> mss = null;
        private ModSetting ms_this = null;
        private int mssIndex = -1;

        private UIList ui_list = null;
        private UITextPanel<string> ui_title = null;
        private UIPanel ui_panel = null;
        private UIElement ui_set = null;
        private UIButton1 btn_save = null;
        private UIButton1 btn_prev = null;
        private UIButton1 btn_next = null;

        public UIModSet()
        {
            ui_list = new UIList();
            ui_list.Width.Pixels = 600;
            ui_list.Height.Set(-100, 1);
            ui_list.HAlign = 0.5f;
            ui_list.VAlign = 0.5f;
            ui_list.ListPadding = 4;

            ui_title = new UITextPanel<string>(string.Empty, 0.8f, true);
            ui_title.HAlign = 0.5f;
            ui_title.BackgroundColor = new Color(73, 94, 171);

            ui_panel = new UIPanel();
            ui_panel.Width.Precent = 1;
            ui_panel.HAlign = 0.5f;
            ui_panel.BackgroundColor = Color.MidnightBlue * 0.8f;

            ui_set = new UIElement();
            ui_set.Width.Precent = 1;
            ui_set.Height.Precent = 1;

            UIElement ui_btns1 = new UIElement();
            ui_btns1.Width.Precent = 1;
            UIElement ui_btns2 = new UIElement();
            ui_btns2.Width.Precent = 1;

            btn_prev = new UIButton1("<");
            btn_prev.Width.Set(-4, 0.5f);
            btn_prev.OnLeftClick += (e, s) => SetItem(mssIndex - 1);

            btn_next = new UIButton1(">");
            btn_next.Width.Set(-4, 0.5f);
            btn_next.HAlign = 1;
            btn_next.OnLeftClick += (e, s) => SetItem(mssIndex + 1);

            UIButton1 btn_back = new UIButton1("返回");
            btn_back.Width.Set(-4, 1 / 3f);
            btn_back.OnLeftClick += (e, s) => Back(backUI);

            btn_save = new UIButton1("保存");
            btn_save.Width.Set(-4, 1 / 3f);
            btn_save.HAlign = 0.5f;
            btn_save.OnLeftClick += (e, s) => ModSet.SaveData(mss);

            UIButton1 btn2_setDeft = new UIButton1("恢复默认");
            btn2_setDeft.Width.Set(-4, 1 / 3f);
            btn2_setDeft.HAlign = 1f;
            btn2_setDeft.OnLeftClick += (e, s) => ms_this?.SetDefault();

            Append(ui_list);
            ui_list.Add(ui_title);
            ui_list.Add(ui_panel);
            ui_list.Add(ui_btns1);
            ui_list.Add(ui_btns2);
            ui_panel.Append(ui_set);
            ui_btns1.Append(btn_prev);
            ui_btns1.Append(btn_next);
            ui_btns2.Append(btn_back);
            ui_btns2.Append(btn_save);
            ui_btns2.Append(btn2_setDeft);
        }

        public void InitializeSetList(UIState backUI, List<ModSetting> mss, ModSetting open = null)
        {
            this.backUI = backUI;
            this.mss = mss;

            if (mss == null)
            {
                ui_title.SetText(string.Empty);
                ui_set.RemoveAllChildren();
                return;
            }

            int openIndex = -1;
            if (mss.Count > 0)
            {
                if (open == null) open = mss.First();
                openIndex = mss.IndexOf(open);
            }

            SetItem(openIndex);
        }

        public void SetItem(int index)
        {
            if (mss?.Count > 0)
            {
                if (index < 0) index = 0;
                else if (index >= mss.Count) index = mss.Count - 1;
            }
            else index = -1;

            mssIndex = index;

            ms_this = mssIndex == -1 ? null : mss[mssIndex];

            //

            ui_title.SetText(ms_this?.Title ?? string.Empty);
            ui_set.RemoveAllChildren();

            UIElement uie = ms_this?.GetUI();
            if (uie != null) ui_set.Append(uie);
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

            //有需要保存的就显示保存按钮
            bool needSave = mss?.FirstOrDefault(i => i.NeedSave) != null;
            btn_save.isDraw = btn_save.isEnable = needSave;

            //

            float height = ui_list.GetInnerDimensions().Height;
            foreach (UIElement i in ui_list)
            {
                if (i == ui_panel) continue;
                height -= i.GetOuterDimensions().Height + ui_list.ListPadding;
                i.UpdateContainer_Height();
            }

            ui_panel.Height.Pixels = height;

            //

            btn_prev.isEnable = mss != null && mssIndex > 0;
            btn_next.isEnable = mss != null && mssIndex < mss.Count - 1;

            //

            if (PlayerInput.Triggers.JustPressed.Inventory) Back(backUI);
        }
    }
}
