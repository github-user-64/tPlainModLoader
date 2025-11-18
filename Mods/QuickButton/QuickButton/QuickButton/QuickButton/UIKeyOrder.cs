using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using tContentPatch.Content.UI;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;

namespace QuickButton.QuickButton
{
    public class UIKeyOrder : UIWrapPanel
    {
        public class UIKetItem : UITextPanel<string>
        {
            public bool isLight = false;
            public string key = null;
            public string name = null;

            public UIKetItem(string key, string name) : base(name)
            {
                this.key = key;
                this.name = name;
                Width.Pixels = Height.Pixels = 40;
            }

            public void Exchange(UIKetItem ki)
            {
                if (ki == null) return;

                string _key = key;
                string _name = name;

                key = ki.key;
                name = ki.name;

                ki.key = _key;
                ki.name = _name;

                SetText(name);
                ki.SetText(ki.name);
            }

            public override void Update(GameTime gameTime)
            {
                if (IsMouseHovering) Main.instance.MouseText(key);
                BorderColor = isLight ? Colors.FancyUIFatButtonMouseOver : Color.Black;
                isLight = false;
            }
        }

        public Action<string, string> OnExchange = null;
        private UIKetItem move1 = null;
        private UIKetItem move2 = null;
        private bool moveing = false;
        private List<UIKetItem> kis = null;

        public UIKeyOrder(List<string> keys)
        {
            kis = new List<UIKetItem>();

            Width.Precent = 1;
            ItemMargin = 4;

            for (int i = 0; i < keys.Count; ++i)
            {
                UIKetItem ki = GetItem(keys[i], i.ToString());
                Append(ki);
                kis.Add(ki);
            }
        }

        public UIKetItem GetItem(string key, string name)
        {
            UIKetItem ki = new UIKetItem(key, name);
            ki.OnLeftMouseDown += (e, s) =>
            {
                if (moveing) return;
                move1 = ki;
                moveing = true;
            };

            return ki;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.UpdateContainer_Height();
            UpdateMove();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (moveing == false) return;

            Rectangle rect = new Rectangle(Main.mouseX - 18, Main.mouseY - 18, 36, 36);

            spriteBatch.Draw(TextureAssets.InventoryBack14.Value, rect, Color.White * 0.5f);
        }

        private void UpdateMove()
        {
            if (moveing == false) return;

            move2 = null;

            for (int i = 0; i < kis.Count; ++i)
            {
                if (kis[i] == move1) continue;

                Terraria.UI.CalculatedStyle dim = kis[i].GetDimensions();

                Vector2 pos2 = new Vector2(dim.X + dim.Width / 2, dim.Y + dim.Height / 2);

                float d = Vector2.Distance(new Vector2(Main.mouseX, Main.mouseY), pos2);
                if (d > 20) continue;

                kis[i].isLight = true;
                move2 = kis[i];
            }

            if (Main.mouseLeftRelease)
            {
                moveing = false;
                if (move2 == null) return;

                move1.Exchange(move2);

                OnExchange?.Invoke(move1.key, move2.key);
            }
        }
    }
}
