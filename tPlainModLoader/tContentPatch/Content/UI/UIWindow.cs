using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace tContentPatch.Content.UI
{
    /// <summary>
    /// 窗口, 可以打开关闭拖动位置和大小
    /// </summary>
    public class UIWindow : UIPanel
    {
        public Action OnOpen = null;
        public Action OnClose = null;
        public UIElement Children { get; protected set; } = null;
        public bool IsOpen { get; protected set; } = false;
        private UIElement WindowParent = null;
        private UIElement uie = null;
        private bool dragPos = false;
        private Vector2 dragPosOff = Vector2.Zero;
        private UIImage ui_dragSize_img = null;
        private bool dragSize = false;
        private Vector2 dragSizeOff = Vector2.Zero;

        public UIWindow(string title, int width, int height)
        {
            Width.Pixels = width;
            Height.Pixels = height;
            MinWidth.Pixels = 100;
            MinHeight.Pixels = 100;
            Left.Set(Main.screenWidth / 2 - Width.Pixels / 2, 0);
            Top.Set(Main.screenHeight / 2 - Height.Pixels / 2, 0);
            SetPadding(10);

            Children = new UIElement();
            Children.Width.Precent = 1;

            uie = new UIElement();
            uie.Width.Precent = 1;
            uie.Height.Pixels = 40;
            uie.OnLeftMouseDown += (e, s) =>
            {
                dragPosOff = new Vector2(Main.mouseX - Left.Pixels, Main.mouseY - Top.Pixels);
                dragPos = true;
            };

            UIText ui_title = new UIText(title ?? string.Empty);

            UIText ui_close = new UIText("X");
            ui_close.HAlign = 1;
            ui_close.OnUpdate += _ => ui_close.TextColor = ui_close.IsMouseHovering ? Color.Red : Color.White;
            ui_close.OnLeftClick += (e, s) => Close();

            ui_dragSize_img = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Wires_11", ReLogic.Content.AssetRequestMode.ImmediateLoad));
            ui_dragSize_img.Width.Pixels = ui_dragSize_img.Height.Pixels = 10;
            ui_dragSize_img.HAlign = 1;
            ui_dragSize_img.VAlign = 1;
            ui_dragSize_img.ScaleToFit = true;
            ui_dragSize_img.OnLeftMouseDown += (e, s) =>
            {
                dragSizeOff = new Vector2(Main.mouseX, Main.mouseY);
                dragSize = true;
            };

            uie.Append(ui_title);
            uie.Append(ui_close);
            Append(uie);
            Append(Children);
            Append(ui_dragSize_img);
        }

        public override void Update(GameTime gameTime)
        {
            if (IsOpen == false) return;

            base.Update(gameTime);

            #region 拖动位置
            if (dragPos)
            {
                Left.Pixels = Main.mouseX - dragPosOff.X;
                Top.Pixels = Main.mouseY - dragPosOff.Y;
                if (Main.mouseLeft == false) dragPos = false;
            }
            #endregion

            #region 拖动大小
            if (dragSize)
            {
                Width.Pixels += Main.mouseX - dragSizeOff.X;
                Height.Pixels += Main.mouseY - dragSizeOff.Y;
                dragSizeOff = new Vector2(Main.mouseX, Main.mouseY);
                if (Main.mouseLeft == false) dragSize = false;
            }
            #endregion

            //限制位置
            int margin = 10;
            if (Top.Pixels + Height.Pixels > Main.screenHeight - margin) Top.Pixels = Main.screenHeight - margin - Height.Pixels;
            else if (Top.Pixels < margin) Top.Pixels = margin;

            //限制大小
            if (Width.Pixels < MinWidth.Pixels) Width.Pixels = MinWidth.Pixels;
            if (Height.Pixels < MinHeight.Pixels) Height.Pixels = MinHeight.Pixels;

            if (Left.Pixels + Width.Pixels > Main.screenWidth - margin) Left.Pixels = Main.screenWidth - margin - Width.Pixels;
            else if (Left.Pixels < margin) Left.Pixels = margin;

            //
            uie.UpdateContainer_Height();
            Children.Height.Set(-(uie.Height.Pixels + ui_dragSize_img.Height.Pixels), 1);
            Children.Top.Pixels = uie.Height.Pixels;

            if (IsMouseHovering) Main.LocalPlayer.mouseInterface = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsOpen == false) return;

            base.Draw(spriteBatch);
        }

        public virtual void Open(UIElement windowParent)
        {
            WindowParent?.RemoveChild(this);
            WindowParent = windowParent;
            if (WindowParent == null) return;

            WindowParent.Append(this);

            IsOpen = true;
            OnOpen?.Invoke();
        }

        public virtual void Close()
        {
            WindowParent?.RemoveChild(this);
            WindowParent = null;

            IsOpen = false;
            dragPos = false;
            dragSize = false;
            OnClose?.Invoke();
        }
    }
}
