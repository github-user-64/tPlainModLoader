using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using tContentPatch.Content.UI;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;

namespace CreativeInventory.CreativeInventory
{
    /// <summary>
    /// 失手了, 是屎啊是陈年老屎啊
    /// </summary>
    internal class UICreativeInventory : UIWindow
    {
        protected UIStackPanel panel_rows = null;
        protected UIPanel panel_items = null;//物品列表
        protected UIScrollViewer panel_items_sv = null;
        protected UIWrapPanel panel_items_wp = null;//物品列表项的容器
        //
        protected Terraria.UI.UIState panel_row1 = null;//搜索框
        protected UITextBox panel_row1_tb = null;
        //
        protected UIPanel panel_row2 = null;//筛选
        protected UIStackPanel panel_row2_sp = null;
        protected UIStackPanel panel_row2_sp_row1 = null;//分类1级
        protected UIStackPanel panel_row2_sp_row2 = null;//分类2级
        protected UIRadioButton[][] panel_row2_sp_row2_rbs = null;//每个分类2级的单选框
        //
        protected ItemSort<int> itemsID = null;
        public string Search_Text = null;
        protected string Search_Text_new = null;
        protected int Search_Text_cd = 0;


        public UICreativeInventory(string title, int width, int height) : base(title, width, height)
        {
            panel_rows = new UIStackPanel();
            panel_row1 = new Terraria.UI.UIState();
            panel_row1_tb = new UITextBox("搜索物品");
            panel_row2 = new UIPanel();
            panel_row2_sp = new UIStackPanel();
            panel_row2_sp_row1 = new UIStackPanel();
            panel_row2_sp_row2 = new UIStackPanel();
            panel_items = new UIPanel();
            panel_items_sv = new UIScrollViewer();
            panel_items_wp = new UIWrapPanel();

            //

            panel_row1.Height.Set(30, 0);

            panel_row1_tb.Width.Set(150, 0);
            panel_row1_tb.Height.Set(30, 0);
            panel_row1_tb.Left.Set(-panel_row1_tb.Width.Pixels, 1);
            panel_row1_tb.Text_MaxLength = 8;
            panel_row1_tb.OnTextChanged += (e) =>
            {
                Search_Text_cd = (int)((1 / 16f) * 250);
                Search_Text_new = e;
            };

            panel_row2.Height.Set(40 + 5 + 5, 0);
            panel_row2.MarginTop = 2;
            panel_row2.SetPadding(5);
            panel_row2.OverflowHidden = true;

            panel_row2_sp.Height.Set(40, 0);

            panel_row2_sp_row1.Height.Set(20, 0);
            panel_row2_sp_row1.ItemMargin = 2;
            panel_row2_sp_row1.Horizontal = true;

            panel_row2_sp_row2.Height.Set(20, 0);
            panel_row2_sp_row2.ItemMargin = 2;
            panel_row2_sp_row2.Horizontal = true;

            panel_items.SetPadding(0);
            panel_items.BorderColor = panel_items.BackgroundColor;
            panel_items.OverflowHidden = true;

            //

            Children.Append(panel_rows);
            Children.Append(panel_items);
            panel_items.Append(panel_items_sv);
            panel_items_sv.SetChild(panel_items_wp);
            //
            panel_rows.Append(panel_row1);
            panel_rows.Append(panel_row2);
            panel_row1.Append(panel_row1_tb);
            panel_row2.Append(panel_row2_sp);
            panel_row2_sp.Append(panel_row2_sp_row1);
            panel_row2_sp.Append(panel_row2_sp_row2);
            #region 分类1级的单选框
            Action<string, string, int, int, int> action_rb = (string path, string text, int id1, int id2, int index) =>
            {
                UIRadioButton rb = new UIRadioButton(
                    Main.Assets.Request<Texture2D>($"Images/{path}", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
                    (int)panel_row2_sp_row1.Height.Pixels - 2, (int)panel_row2_sp_row1.Height.Pixels - 2);
                rb.VAlign = 0.5f;
                rb.MouseHoveringText = text;
                rb.OnChecked += () =>
                {
                    update_item(id1, id2);
                    switchItemSort1(index);
                };

                panel_row2_sp_row1.Append(rb);
            };

            action_rb.Invoke("Item_2712", "全部", -1, -1, 0);
            action_rb.Invoke("Item_4", "武器", itemsSort.ID_Weapon, -1, 1);
            action_rb.Invoke("Item_1", "工具", itemsSort.ID_Tool, -1, 2);
            action_rb.Invoke("Item_324", "装备", itemsSort.ID_ToolKit, -1, 3);
            action_rb.Invoke("Item_82", "盔甲", itemsSort.ID_Armor, -1, 4);
            action_rb.Invoke("Item_1302", "弹药", itemsSort.ID_Ammo, -1, 5);
            action_rb.Invoke("Item_54", "饰品", itemsSort.ID_Accessorie, -1, 6);
            action_rb.Invoke("Item_2", "方块", itemsSort.ID_Tile, -1, 7);
            action_rb.Invoke("Item_296", "药水", itemsSort.ID_Buff, -1, 8);
            action_rb.Invoke("Item_557", "boss召唤物", itemsSort.ID_BossSpawn, -1, 9);
            action_rb.Invoke("Item_5", "消耗品", itemsSort.ID_Consumable, -1, 10);
            action_rb.Invoke("Item_9", "其他", itemsSort.ID_Other, -1, 11);
            #endregion
            #region 每个分类2级的单选框
            panel_row2_sp_row2_rbs = new UIRadioButton[12][];
            int rbs_size = (int)panel_row2_sp_row2.Height.Pixels - 2;
            Func<string, string, int, int, UIRadioButton> action_rb2 = (path, text, id1, id2) =>
            {
                UIRadioButton rb =
                new UIRadioButton(Main.Assets.Request<Texture2D>($"Images/{path}", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
                rbs_size, rbs_size)
                {
                    MouseHoveringText = text
                };
                rb.OnChecked += () => update_item(id1, id2);

                return rb;
            };
            panel_row2_sp_row2_rbs[1] = new UIRadioButton[]
            {
                action_rb2.Invoke("Item_3507", "近战武器", itemsSort.ID_Weapon, itemsSort.ID_Weapon_Melee),
                action_rb2.Invoke("Item_39", "远程武器", itemsSort.ID_Weapon, itemsSort.ID_Weapon_Ranged),
                action_rb2.Invoke("Item_165", "魔法武器", itemsSort.ID_Weapon, itemsSort.ID_Weapon_Magic),
                action_rb2.Invoke("Item_1309", "召唤武器", itemsSort.ID_Weapon, itemsSort.ID_Weapon_Summon),
            };
            panel_row2_sp_row2_rbs[2] = new UIRadioButton[]
            {
                action_rb2.Invoke("Item_3509", "稿子", itemsSort.ID_Tool, itemsSort.ID_Tool_pick),
                action_rb2.Invoke("Item_3506", "斧头", itemsSort.ID_Tool, itemsSort.ID_Tool_axe),
                action_rb2.Invoke("Item_3505", "锤子", itemsSort.ID_Tool, itemsSort.ID_Tool_hammer),
            };
            panel_row2_sp_row2_rbs[3] = new UIRadioButton[]
            {
                action_rb2.Invoke("Item_2289", "钓竿", itemsSort.ID_ToolKit, itemsSort.ID_ToolKit_fishingPole),
                action_rb2.Invoke("Item_437", "钩爪", itemsSort.ID_ToolKit, itemsSort.ID_ToolKit_hook),
                action_rb2.Invoke("Item_2430", "坐骑", itemsSort.ID_ToolKit, itemsSort.ID_ToolKit_mount),
                action_rb2.Invoke("Item_5332", "宠物", itemsSort.ID_ToolKit, itemsSort.ID_ToolKit_pet),
            };
            panel_row2_sp_row2_rbs[4] = new UIRadioButton[]
            {
                action_rb2.Invoke("Item_894", "头盔", itemsSort.ID_Armor, itemsSort.ID_Armor_head),
                action_rb2.Invoke("Item_895", "胸甲", itemsSort.ID_Armor, itemsSort.ID_Armor_body),
                action_rb2.Invoke("Item_896", "护腿", itemsSort.ID_Armor, itemsSort.ID_Armor_leg),
                action_rb2.Invoke("Item_1746", "时装", itemsSort.ID_Armor, itemsSort.ID_Armor_vanity),
            };
            panel_row2_sp_row2_rbs[7] = new UIRadioButton[]
            {
                action_rb2.Invoke("Item_2", "方块", itemsSort.ID_Tile, itemsSort.ID_Tile_tile),
                action_rb2.Invoke("Item_130", "墙", itemsSort.ID_Tile, itemsSort.ID_Tile_wall),
            };
            #endregion
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            panel_rows.Width_Stretch();
            panel_row2.Width_Stretch();
            panel_row2_sp.Width_Stretch();
            panel_row2_sp_row1.Width_Stretch();
            panel_row2_sp_row2.Width_Stretch();
            panel_items.Width_Stretch();
            panel_items_sv.Width.Precent = 1;
            panel_items_sv.Height.Precent = 1;
            panel_items_sv.VAlign = 0.5f;
            panel_items_wp.Width.Precent = 1;
            //
            panel_rows.UpdateSize_Height();
            panel_items_wp.UpdateSize_Height();
            panel_items.Top.Pixels = panel_rows.GetDimensions().Height + 10;//在rows下面
            panel_items.Height.Pixels = Children.GetInnerDimensions().Height - panel_rows.Height.Pixels - 10;//填满剩余空间

            //
            if (Search_Text_cd > 0)
            {
                --Search_Text_cd;

                if (Search_Text_cd == 0)
                {
                    Search_Text = Search_Text_new;
                    update_itemUI();
                }
            }
        }

        public void update_itemUI()
        {
            panel_items_wp.RemoveAllChildren();

            if (itemsID == null) return;

            itemsID.for_ItemsAll((i) =>
            {
                if (i >= ItemID.Count) return;
                Item item = new Item();
                item.SetDefaults(i);
                if (item.type < 1 || item.type >= ItemID.Count) return;

                //根据搜索文本筛选
                if (Search_Text != null && Search_Text.Length > 0)
                {
                    if (!searchItem(item.Name, Search_Text)) return;
                }

                UIItemGrid it = new UIItemGrid(item, Terraria.UI.ItemSlot.Context.CreativeInfinite);
                panel_items_wp.Append(it);
            });
        }

        public void update_item(int id1, int id2)
        {
            if (!itemsSort.Loaded) return;

            ItemSort<int> old = itemsID;

            if (id1 == -1)//第1级全部
            {
                itemsID = itemsSort.ID;
            }
            else if (id2 == -1)//第2级全部
            {
                itemsID = itemsSort.ItemsSort_Gets(id1);
            }
            else
            {
                itemsID = itemsSort.ItemsSort_Gets(id1, id2);
            }

            if (itemsID != old) update_itemUI();
        }

        public void switchItemSort1(int index)
        {
            foreach (Terraria.UI.UIElement ui in panel_row2_sp_row2.Children)
            {
                UIRadioButton rb = ui as UIRadioButton;
                if (rb == null) continue;

                rb.IsChecked = false;
            }

            panel_row2_sp_row2.RemoveAllChildren();

            if (index >= panel_row2_sp_row2_rbs.Length) return;

            UIRadioButton[] rbs = panel_row2_sp_row2_rbs[index];

            for (int i = 0; i < rbs?.Length; ++i) panel_row2_sp_row2.Append(rbs[i]);
        }

        public bool searchItem(string name, string s)//name中有包含s的文本
        {
            if (name == null || name.Length < 1) return false;
            if (s == null || s.Length < 1) return false;

            for (int i = 0; i < name.Length; ++i)
            {
                bool isEquals = true;
                if (name.Length - i < s.Length) return false;

                for (int i2 = 0; i2 < s.Length; ++i2)
                {
                    if (name[i + i2] != s[i2])
                    {
                        isEquals = false;
                        break;
                    }
                }

                if (isEquals) return true;
            }

            return false;
        }
    }
}
