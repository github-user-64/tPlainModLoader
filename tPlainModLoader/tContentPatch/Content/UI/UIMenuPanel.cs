using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.UI;

namespace tContentPatch.Content.UI
{
    /// <summary>
    /// 菜单页面模板
    /// </summary>
    public class UIMenuPanel : UIState
    {
        /// <summary>
        /// 返回菜单
        /// </summary>
        public UIState BackUI = null;
        /// <summary>
        /// 页面
        /// </summary>
        public readonly UIPanel UIPanel = null;
        /// <summary>
        /// 下面的按钮容器
        /// </summary>
        public readonly UIWrapPanel UIBtns = null;
        /// <summary>
        /// 标题
        /// </summary>
        public readonly UITextPanel<string> UITitle = null;

        /// <summary/>
        public UIMenuPanel(string title, float textScale = 0.8f, bool textLarge = true, float titleYOff = 10f,
            UIState back = null)
        {
            BackUI = back;

            UITitle = new UITextPanel<string>(title, textScale, textLarge);
            UITitle.Recalculate();
            UITitle.Top.Pixels = -UITitle.GetOuterDimensions().Height + titleYOff;
            UITitle.HAlign = 0.5f;
            UITitle.BackgroundColor = new Color(73, 94, 171);

            Width.Set(-800f, 1f);
            Height.Set(-100f + UITitle.Top.Pixels, 1f);
            MinWidth.Set(700f, 0f);
            MaxWidth.Set(800f, 0f);
            HAlign = 0.5f;
            VAlign = 0.5f;

            UIBtns = new UIWrapPanel();
            UIBtns.Width.Set(0f, 1f);
            UIBtns.Height.Set(0f, 0f);
            UIBtns.VAlign = 1f;
            UIBtns.ItemMargin = 4f;
            UIBtns.OnUpdate += _ => UIBtns.UpdateContainer_Height();
            Append(UIBtns);

            UIPanel = new UIPanel();
            UIPanel.Width.Set(0f, 1f);
            UIPanel.Height.Set(0f, 1f);
            UIPanel.OnUpdate += _ => UIPanel.Height.Pixels = -UIBtns.GetOuterDimensions().Height - 4f;
            Append(UIPanel);

            Append(UITitle);
        }

        /// <summary>
        /// 添加按钮(ui, 一行的数量)
        /// </summary>
        public void AddBtn(UIElement btn, int column = 1)
        {
            float off = -UIBtns.ItemMargin;
            off += (float)Math.Floor(Math.Max(0f, UIBtns.ItemMargin - 1f) / column);

            btn.Width.Set(off, 1f / column);
            btn.Height.Set(40f, 0f);
            UIBtns.Append(btn);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (PlayerInput.Triggers.JustPressed.Inventory) Back(BackUI);
        }

        /// <summary>
        /// 返回菜单
        /// </summary>
        public virtual void Back(UIState back = null)
        {
            GameUitl.BackMenu(back);

            SoundEngine.PlaySound(SoundID.MenuClose);
        }
    }
}
