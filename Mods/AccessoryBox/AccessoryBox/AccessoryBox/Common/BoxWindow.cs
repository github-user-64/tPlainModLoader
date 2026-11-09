using AccessoryBox.Common.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using tContentPatch.Content.UI;
using tContentPatch.Content.UI.ModSet;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace AccessoryBox.Common
{
    internal class BoxWindow : UIWindow
    {
        protected IBoxConsole console;
        protected UIWrapPanel wp = null;

        public BoxWindow(IBoxConsole console, string title, int width, int height) : base(title, width, height)
        {
            this.console = console;

            UIElement btns = BuildBtns();
            Child.Append(btns);

            UIPanel panel = new UIPanel();
            panel.Width.Set(0, 1);
            panel.Height.Set(-btns.Height.Pixels - 4, 1);
            panel.VAlign = 1;
            panel.SetPadding(6);
            panel.BorderColor = panel.BackgroundColor;
            Child.Append(panel);

            UIScrollViewer sv = new UIScrollViewer();
            sv.Width.Set(0, 1);
            sv.Height.Set(0, 1);
            panel.Append(sv);

            wp = new UIWrapPanel();
            wp.Width.Set(0, 1);
            wp.ItemMargin = 4;
            sv.SetChild(wp);

            console.OnLoaded += UpdateData;
            console.OnClearItemed += UpdateData;
            console.OnAdded += AddItem;
            console.OnDeled += DelItem;
            console.OnSetItemed += SetItem;

            UpdateData();
        }

        private UIElement BuildBtns()
        {
            UIItemSwitch s = new UIItemSwitch();
            s.OnGetIsChecked = console.GetEnable;
            s.OnSetIsChecked = console.SetEnable;

            int height = 20;

            UIStackPanel sp = new UIStackPanel();
            sp.Height.Set(height, 0);
            sp.VAlign = 0.5f;
            sp.IsAutoUpdateSize = true;
            sp.Horizontal = true;
            sp.ItemMargin = 8;
            s.Append(sp);

            UIButtonImage load = new UIButtonImage(height, "加载", "Images/UI/Cursor_8");
            load.OnClick += console.Load;
            sp.Append(load);

            UIButtonImage save = new UIButtonImage(height, "保存", "Images/UI/Cursor_9");
            save.OnClick += console.Save;
            sp.Append(save);

            UIButtonImage add = new UIButtonImage(height, "添加", "Images/UI/Cursor_4");
            add.OnClick += () => console.AddItem(new Item());
            sp.Append(add);

            UIButtonImage clear = new UIButtonImage(height, "清空", "Images/UI/Cursor_6");
            clear.OnClick += console.ClearItem;
            sp.Append(clear);

            return s;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            wp.UpdateContainer_Height();
        }

        protected void UpdateData()
        {
            List<Item> items = console.GetItems();

            wp.RemoveAllChildren();

            items.ForEach(AddItem);
        }

        protected BoxItem ItemToUI(Item item)
        {
            BoxItem ui = new BoxItem(item);
            ui.OnSetItem += console.SetItem;
            ui.OnDel += console.DelItem;

            return ui;
        }

        protected void AddItem(Item item)
        {
            BoxItem ui = ItemToUI(item);

            wp.Append(ui);
        }

        protected void ActionItem(Item item, Action<BoxItem> action)
        {
            BoxItem ui = (BoxItem)wp.Children.FirstOrDefault(i =>
            {
                BoxItem bi = i as BoxItem;
                if (bi == null) return false;

                return bi.item == item;
            });

            action(ui);
        }

        protected void DelItem(Item item)
        {
            ActionItem(item, wp.RemoveChild);
        }

        protected void SetItem(Item item, Item val)
        {
            ActionItem(item, i => i.SetItem(val));
        }
    }
}
