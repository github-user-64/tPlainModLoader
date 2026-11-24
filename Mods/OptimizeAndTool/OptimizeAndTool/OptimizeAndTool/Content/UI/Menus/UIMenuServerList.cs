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

        public Action<int> OnSwitchItemStyle = null;
        public Action<UIServerListItem> ItemStyle = UIServerListItem.Style1;

        public UIServerListItem CheckedItem = null;
        public readonly UIStackPanelMove UISP = null;

        public UIMenuServerList(ListData.IControl control = null, UIState back = null) : base("选择服务器", back: back)
        {
            UIPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;

            UIButton1 btn1 = new UIButton1("返回");
            btn1.OnLeftClick += (e, s) => Back(BackUI);
            AddBtn(btn1, 2);

            UIButton1 btn2 = new UIButton1("添加服务器");
            btn2.MouseText = "在当前选中服务器处插入\n或在游戏内将服务器添加到列表(优化和工具)";
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

            UIImageButton switchStyle = new UIImageButton(btns.Width.Pixels, "Images/UI/ChestCraft_2");
            switchStyle.MouseText = "切换样式";
            switchStyle.OnLeftClick += (e, s) => SwitchItemStyle(ItemStyle == UIServerListItem.Style1 ? 1 : 0);
            btns.Append(switchStyle);
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
            UIServerListItem ui = new UIServerListItem(data);
            ui.Style = ItemStyle;
            ui.GetIsChecked = () => CheckedItem == ui;
            ui.OnLeftClick += (e, s) => CheckedItem = ui;
            ui.OnDel += () => SetControlDel(ui);
            ui.OnUp += () => SetControlUp(ui);
            ui.OnDown += () => SetControlDown(ui);

            UISP.AddItem(ui, index);
        }

        public virtual void SwitchItemStyle(int style)
        {
            switch (style)
            {
                case 1: ItemStyle = UIServerListItem.Style2; break;
                default: ItemStyle = UIServerListItem.Style1; break;
            }

            OnSwitchItemStyle?.Invoke(style);

            UpdateUIData();
        }

        public virtual void UpdateUIData()
        {
            CheckedItem = null;
            UISP.RemoveAllChildren();

            List<ServerInfo> data = Data;
            if (data == null) return;

            for (int i = 0; i < data.Count; ++i) AddUIItem(data[i], i);
        }
    }
}
