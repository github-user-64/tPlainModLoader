using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using tContentPatch.Content.UI;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;

namespace QuickSetting.QuickSetting
{
    internal class UIQuickSetting : UIWindow
    {
        public Action<string> OnAddItem = null;
        public Action<string, string> OnSwitchItem = null;
        private UIWrapPanel ui_wp = null;
        private UIScrollViewer2 ui_children = null;
        private List<(string, UIRadioButton, List<UIElement>)> asd = null;
        private bool switchRb = false;
        private UIRadioButton switchRb_rb = null;
        private UIRadioButton switchRb_rb2 = null;

        public UIQuickSetting(string title, int width, int height) : base(title, width, height)
        {
            asd = new List<(string, UIRadioButton, List<UIElement>)>();

            ui_wp = new UIWrapPanel();
            ui_wp.Width.Precent = 1;
            ui_wp.ItemMargin = 2;

            ui_children = new UIScrollViewer2();
            ui_children.Width.Precent = 1;
            ui_children.VAlign = 1;
            ui_children.ItemMargin = 2;

            Children.PaddingTop = 4;

            Children.Append(ui_wp);
            Children.Append(ui_children);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            ui_wp.UpdateContainer_Height();
            ui_children.Height.Set(-(ui_wp.Height.Pixels + 2), 1);

            if (switchRb)
            {
                if (Main.mouseRight == false)
                {
                    switchRb = false;
                    SwitchItem();
                }
                else
                {
                    UpdateDrag();
                }
            }
        }

        private void UpdateDrag()
        {
            switchRb_rb2 = null;

            foreach (var item in asd)
            {
                UIRadioButton rb = item.Item2;
                if (rb == switchRb_rb) continue;

                CalculatedStyle dim = rb.GetDimensions();
                if (Vector2.Distance(new Vector2(Main.mouseX, Main.mouseY), new Vector2(dim.X + dim.Width / 2, dim.Y + dim.Height / 2)) < 10)
                {
                    switchRb_rb2 = rb;
                    return;
                }
            };
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (switchRb == false) return;

            Rectangle rect = new Rectangle(0, 0, 20, 20);
            if (switchRb_rb2 == null)
            {
                rect.X = Main.mouseX - 10;
                rect.Y = Main.mouseY - 10;
            }
            else
            {
                CalculatedStyle dim = switchRb_rb2.GetDimensions();
                rect.X = (int)dim.X;
                rect.Y = (int)dim.Y;
            }

            spriteBatch.Draw(switchRb_rb2 == null ? TextureAssets.InventoryBack14.Value : TextureAssets.InventoryBack17.Value,
                rect, Color.White * 0.5f);
        }

        public void KeyOrder(List<string> keyOrder)
        {
            int index = 0;

            foreach (string key in keyOrder)
            {
                for (int i = index; i < asd.Count; ++i)
                {
                    if (asd[i].Item1 != key) continue;

                    (asd[i], asd[index]) = (asd[index], asd[i]);
                    ++index;
                }
            }

            ui_wp.RemoveAllChildren();
            foreach (var i in asd) ui_wp.Append(i.Item2);
        }

        private void SwitchItem()
        {
            if (switchRb_rb2 == null || switchRb_rb == null) return;

            int index = asd.FindIndex(i => i.Item2 == switchRb_rb);
            int index2 = asd.FindIndex(i => i.Item2 == switchRb_rb2);

            if (index == index2 || index == -1 || index2 == -1) return;

            OnSwitchItem?.Invoke(asd[index].Item1, asd[index2].Item1);
        }

        public void AddItem(Texture2D ico, string name, UIElement uie)
        {
            if (name == null) return;

            int index = asd.FindIndex(i => i.Item1 == name);
            bool isnew = false;
            if (index == -1)
            {
                isnew = true;

                List<UIElement> uis = new List<UIElement>();
                UIRadioButton rb = new UIRadioButton(TextureAssets.Reforge[0].Value, 20, 20);
                rb.MouseHoveringText = name;
                rb.OnChecked += () =>
                {
                    ui_children.ClearChild();
                    foreach (UIElement i in uis) ui_children.AddChild(i);
                };
                rb.OnRightMouseDown += (e, s) =>
                {
                    switchRb_rb = rb;
                    switchRb = true;
                };
                asd.Add((name, rb, uis));

                index = asd.Count - 1;

                ui_wp.Append(rb);
            }

            (string, UIRadioButton, List<UIElement>) item = asd[index];

            item.Item2.SetIco(ico);
            item.Item3.Add(uie);

            if (isnew) OnAddItem?.Invoke(item.Item1);
        }

        public override void Close()
        {
            base.Close();
            switchRb = false;
        }
    }
}
