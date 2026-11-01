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
        protected UIState BackUI = null;
        /// <summary>
        /// 页面
        /// </summary>
        protected UIPanel Panel = null;
        /// <summary>
        /// 下面的按钮容器
        /// </summary>
        protected UIWrapPanel Btns = null;

        /// <summary/>
        public UIMenuPanel(string title, float textScale = 0.8f, bool textLarge = true, float titleYOff = 10f,
            UIState back = null)
        {
            BackUI = back;

            UITextPanel<string> uitext = new UITextPanel<string>(title, textScale, textLarge);
            uitext.Recalculate();
            uitext.Top.Pixels = -uitext.GetOuterDimensions().Height + titleYOff;
            uitext.HAlign = 0.5f;
            uitext.BackgroundColor = new Color(63, 82, 151);

            Width.Set(-800f, 1f);
            Height.Set(-100f + uitext.Top.Pixels, 1f);
            HAlign = 0.5f;
            VAlign = 0.5f;

            Btns = new UIWrapPanel();
            Btns.Width.Set(0f, 1f);
            Btns.Height.Set(0f, 0f);
            Btns.VAlign = 1f;
            Btns.ItemMargin = 4f;
            Btns.OnUpdate += _ => Btns.UpdateContainer_Height();
            Append(Btns);

            Panel = new UIPanel();
            Panel.Width.Set(0f, 1f);
            Panel.Height.Set(0f, 1f);
            Panel.OnUpdate += _ => Panel.Height.Pixels = -Btns.GetOuterDimensions().Height - 4f;
            Append(Panel);

            Append(uitext);
        }

        /// <summary>
        /// 添加按钮(ui, 一行的数量)
        /// </summary>
        public void AddBtn(UIElement btn, int column = 1)
        {
            float off = -Btns.ItemMargin;
            off += (float)Math.Floor(Math.Max(0f, Btns.ItemMargin - 1f) / column);

            btn.Width.Set(off, 1f / column);
            btn.Height.Set(40f, 0f);
            Btns.Append(btn);
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
        public void Back(UIState back = null)
        {
            GameUitl.BackMenu(back);

            SoundEngine.PlaySound(SoundID.MenuClose);
        }
    }
}
