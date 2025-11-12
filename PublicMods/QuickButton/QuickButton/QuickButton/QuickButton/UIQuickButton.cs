using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using tContentPatch.Content.UI;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace QuickButton.QuickButton
{
    internal class UIQuickButton : UIStackPanel
    {
        private UIImageButton btn = null;
        private UIStackPanel ui_sp = null;
        private List<(string, UIElement)> keyUi = null;
        private bool isOpen = false;
        private bool isMove = false;
        private Vector2 moveOff = Vector2.Zero;

        public UIQuickButton()
        {
            keyUi = new List<(string, UIElement)>();

            Height.Pixels = 32;
            ItemMargin = 6;
            Horizontal = true;
            IsAutoUpdateSize = true;

            btn = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay", ReLogic.Content.AssetRequestMode.ImmediateLoad));
            btn.VAlign = 0.5f;
            btn.OnLeftClick += (e, s) =>
            {
                if (isOpen) RemoveChild(ui_sp);
                else Append(ui_sp);
                isOpen = !isOpen;
            };
            btn.OnRightMouseDown += (e, s) =>
            {
                isMove = true;
                moveOff = e.MousePosition - new Vector2(Left.Pixels, Top.Pixels);
            };
            btn.OnRightMouseUp += (e, s) => isMove = false;

            ui_sp = new UIStackPanel();
            ui_sp.Height.Precent = 1;
            ui_sp.ItemMargin = 6;
            ui_sp.Horizontal = true;
            ui_sp.IsAutoUpdateSize = true;

            Append(btn);
        }

        public void Add(string key, UIElement uie)
        {
            if (key == null || uie == null) return;

            ui_sp.Append(uie);
            keyUi.Add((key, uie));
        }

        public void KeyOrder(List<string> keyOrder)
        {
            int index = 0;

            foreach (string key in keyOrder)
            {
                for (int i = index; i < keyUi.Count; ++i)
                {
                    if (keyUi[i].Item1 != key) continue;

                    (keyUi[i], keyUi[index]) = (keyUi[index], keyUi[i]);
                    ++index;
                }
            }

            ui_sp.RemoveAllChildren();
            foreach ((string, UIElement) ku in keyUi) ui_sp.Append(ku.Item2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (isMove)
            {
                Left.Pixels = Main.mouseX - moveOff.X;
                Top.Pixels = Main.mouseY - moveOff.Y;
            }

            //

            CalculatedStyle btnDim = btn.GetDimensions();
            CalculatedStyle dim = GetDimensions();
            float xOff = btnDim.X - dim.X;
            float yOff = btnDim.Y - dim.Y;
            int margin = 10;

            if (btnDim.X < margin) Left.Pixels = margin;
            else
            if (btnDim.X + btnDim.Width > Main.screenWidth - margin) Left.Pixels = Main.screenWidth - margin - btnDim.Width - xOff;

            if (btnDim.Y < margin) Top.Pixels = margin;
            else
            if (btnDim.Y + btnDim.Height > Main.screenHeight - margin) Top.Pixels = Main.screenHeight - margin - btnDim.Height - yOff;

            //

            if (btn.IsMouseHovering) Main.instance.MouseText("右键拖动位置. 在设置界面排序快捷内容");

            if (IsMouseHovering) Main.LocalPlayer.mouseInterface = true;
        }

        public void SetPos(int pos)
        {
            switch (pos)
            {
                case 0:
                    Left.Pixels = 20;
                    Top.Pixels = Main.screenHeight / 2 - Height.Pixels / 2;
                    break;

                case 1:
                    float v = 20f + (10 * 56) * 0.85f;
                    Left.Pixels = v;
                    Top.Pixels = 20;
                    break;

                default: return;
            }
        }

        public List<string> GetKeys()
        {
            List<string> keys = keyUi.ConvertAll(v => v.Item1);

            return keys;
        }
    }
}
