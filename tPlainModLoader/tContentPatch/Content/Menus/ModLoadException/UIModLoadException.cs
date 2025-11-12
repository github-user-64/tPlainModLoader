using Microsoft.Xna.Framework;
using System;
using tContentPatch.Content.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace tContentPatch.Content.Menus.ModLoadException
{
    internal class UIModLoadException : UIState
    {
        private UIStackPanel ui_button_sp = null;
        private UIStackPanel ui_text_sp = null;

        public UIModLoadException(Action onClose = null)
        {
            UIPanel ui_panel = new UIPanel();
            ui_panel.Width.Set(-100, 1);
            ui_panel.Height.Set(-250, 1);
            ui_panel.HAlign = 0.5f;
            ui_panel.VAlign = 0.5f;

            UITextPanel<string> ui_panel_title = new UITextPanel<string>("模组加载失败", 0.8f, true);
            ui_panel_title.Top.Pixels = -50;
            ui_panel_title.HAlign = 0.5f;
            ui_panel_title.BackgroundColor = new Color(73, 94, 171);

            UIScrollbar ui_text_sb = new UIScrollbar();
            ui_text_sb.Height.Set(-10, 1);
            ui_text_sb.Top.Pixels = 10;
            ui_text_sb.HAlign = 1;

            UIList ui_text_list = new UIList();
            ui_text_list.Width.Set(-25, 1);
            ui_text_list.Height.Set(-10, 1);
            ui_text_list.Top.Pixels = 10;
            ui_text_list.SetScrollbar(ui_text_sb);

            ui_text_sp = new UIStackPanel();
            ui_text_sp.Width.Precent = 1;
            ui_text_sp.ItemMargin = 4;

            ui_button_sp = new UIStackPanel();
            ui_button_sp.Width.Precent = 1;
            ui_button_sp.Height.Pixels = 50;
            ui_button_sp.Top.Set(-100, 1);
            ui_button_sp.HAlign = 0.5f;
            ui_button_sp.Horizontal = true;
            ui_button_sp.ItemMargin = 10;

            UIButton1 ui_btn1 = new UIButton1("确认");
            ui_btn1.OnLeftClick += (e, s) => onClose?.Invoke();

            UIButton1 ui_btn2 = new UIButton1("打开模组文件夹");
            ui_btn2.OnLeftClick += (e, s) => ModLoadException.OpenModDirectory();

            Append(ui_panel);
            Append(ui_button_sp);
            ui_panel.Append(ui_panel_title);
            ui_panel.Append(ui_text_list);
            ui_panel.Append(ui_text_sb);
            ui_text_list.Add(ui_text_sp);
            ui_button_sp.Append(ui_btn1);
            ui_button_sp.Append(ui_btn2);
        }

        public void InitializeException(Exception ex)
        {
            ui_text_sp.RemoveAllChildren();

            string s = $"{ex.Message}\n{ex}";

            while (s.Length > 0)
            {
                int start = s.IndexOf('\n');
                int textLen = start == -1 ? s.Length : start;
                int rLen = start == -1 ? s.Length : start + 1;

                string text = s.Substring(0, textLen);

                s = s.Remove(0, rLen);

                ui_text_sp.Append(new UIText(text));
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            ui_text_sp.UpdateContainer_Height();

            ui_button_sp.UpdateContainer_Width();
            ui_button_sp.UpdateContainer_Height();
        }
    }
}
