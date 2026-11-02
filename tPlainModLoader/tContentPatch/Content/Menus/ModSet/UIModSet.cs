using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using tContentPatch.Content.UI;
using Terraria.UI;

namespace tContentPatch.Content.Menus.ModSet
{
    internal class UIModSet : UIMenuPanel
    {
        private List<ModSetting> mss = null;
        private ModSetting ms_this = null;
        private int mssIndex = -1;

        private UIElement uie = null;
        private UIButton1 btn_prev = null;
        private UIButton1 btn_next = null;
        private UIButton1 btn_save = null;

        public UIModSet() : base(string.Empty)
        {
            MaxWidth.Set(700f, 0f);
            UIPanel.BackgroundColor = Color.MidnightBlue * 0.8f;

            uie = new UIElement();
            uie.Width.Set(0f, 1f);
            uie.Height.Set(0f, 1f);
            UIPanel.Append(uie);

            btn_prev = new UIButton1("<");
            btn_prev.OnLeftClick += (e, s) => SetItem(mssIndex - 1);
            AddBtn(btn_prev, 2);

            btn_next = new UIButton1(">");
            btn_next.OnLeftClick += (e, s) => SetItem(mssIndex + 1);
            AddBtn(btn_next, 2);

            UIButton1 btn_back = new UIButton1("返回");
            btn_back.OnLeftClick += (e, s) => Back(BackUI);
            AddBtn(btn_back, 3);

            btn_save = new UIButton1("保存");
            btn_save.OnLeftClick += (e, s) => ModSet.SaveData(mss);
            AddBtn(btn_save, 3);

            UIButton1 btn_setDeft = new UIButton1("恢复默认");
            btn_setDeft.OnLeftClick += (e, s) => ms_this?.SetDefault();
            AddBtn(btn_setDeft, 3);
        }

        public void InitializeSetList(UIState backUI, List<ModSetting> mss, ModSetting open = null)
        {
            BackUI = backUI;
            this.mss = mss;

            if (mss == null)
            {
                UITitle.SetText(string.Empty);
                uie.RemoveAllChildren();
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

            UITitle.SetText(ms_this?.Title ?? string.Empty);
            this.uie.RemoveAllChildren();

            UIElement uie = ms_this?.GetUI();
            if (uie != null) this.uie.Append(uie);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //有需要保存的就显示保存按钮
            bool needSave = mss?.FirstOrDefault(i => i.NeedSave) != null;
            btn_save.isDraw = btn_save.isEnable = needSave;

            //

            btn_prev.isEnable = mss != null && mssIndex > 0;
            btn_next.isEnable = mss != null && mssIndex < mss.Count - 1;
        }
    }
}
