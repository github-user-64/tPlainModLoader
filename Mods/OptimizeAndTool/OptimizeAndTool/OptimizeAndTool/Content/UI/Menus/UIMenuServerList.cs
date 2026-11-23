using Microsoft.Xna.Framework;
using OptimizeAndTool.Content.ServerList;
using System;
using System.Collections.Generic;
using tContentPatch.Content.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace OptimizeAndTool.Content.UI.Menus
{
    public partial class UIMenuServerList : UIMenuPanel
    {
        public readonly ListData.IControl Control = null;
        public List<ServerInfo> Data => Control?.Data;

        /// <summary>
        /// <see cref="UIElement.Update(GameTime)"/>前的操作
        /// </summary>
        protected Action UpdatePrefixControl = null;
        public UIServerListItem CheckedItem = null;
        public readonly UIStackPanelMove UISP = null;

        public UIMenuServerList(ListData.IControl control = null, UIState back = null) : base("选择服务器", back: back)
        {
            UIPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;

            UIButton1 btn1 = new UIButton1("返回");
            btn1.OnLeftClick += (e, s) => Back(BackUI);
            AddBtn(btn1, 2);

            UIButton1 btn2 = new UIButton1("添加服务器");
            btn2.MouseText = "在当前选中服务器下方插入";
            btn2.OnLeftClick += (e, s) => SetControlAdd();
            AddBtn(btn2, 2);

            //

            InitializeButtons();

            UIScrollViewer sv = new UIScrollViewer();
            sv.Width.Set(0f, 1f);
            sv.Height.Set(0f, 1f);
            UIPanel.Append(sv);

            UISP = new UIStackPanelMove();
            UISP.Width.Set(0f, 1f);
            UISP.ItemMargin = 6f;
            UISP.IsAutoUpdateSize = true;
            sv.SetChild(UISP);

            //

            Control = control;

            UpdateUIData();
        }

        protected virtual void InitializeButtons()
        {
            UIStackPanel btns = new UIStackPanel();
            btns.Width.Set(30f, 0f);
            btns.Left.Set(-btns.Width.Pixels, 0f);
            btns.MarginLeft = -6f;
            btns.MarginTop = 10f;
            btns.IsAutoUpdateSize = true;
            btns.ItemMargin = 10f;
            Append(btns);

            UIImageButton save = new UIImageButton(btns.Width.Pixels, "Images/UI/IconQuickload");
            save.MouseText = "保存";
            save.OnLeftClick += (e, s) => SetControlSave();
            btns.Append(save);

            UIImageButton load = new UIImageButton(btns.Width.Pixels, "Images/UI/Sort_0");
            load.MouseText = "加载";
            load.OnLeftClick += (e, s) => SetControlLoad();
            btns.Append(load);
        }

        public override void Update(GameTime gameTime)
        {
            if (UpdatePrefixControl != null)
            {
                UpdatePrefixControl();
                UpdatePrefixControl = null;
            }

            base.Update(gameTime);
        }

        protected virtual void AddUIItem(ServerInfo data, int index)
        {
            UIServerListItem item = new UIServerListItem(data);
            item.GetIsChecked = () => CheckedItem == item;
            item.OnLeftClick += (e, s) => CheckedItem = item;
            item.OnDel += () => SetControlDel(item);
            item.OnUp += () => SetControlUp(item);
            item.OnDown += () => SetControlDown(item);

            UISP.AddItem(item, index);
        }

        protected virtual void UpdateUIData()
        {
            CheckedItem = null;
            UISP.RemoveAllChildren();

            List<ServerInfo> data = Data;
            if (data == null) return;

            for (int i = 0; i < data.Count; ++i) AddUIItem(data[i], i);
        }
    }
}
