using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skil.Utils;
using System;

namespace Skil.Content.UI
{
    internal class UIItemTextBoxAButtonBind<T> : UIItemMouseText
    {
        public Action OnClick = null;
        private UITextBoxBind<T> ui_tb = null;
        private UITextButton ui_btn = null;

        public UIItemTextBoxAButtonBind(GetSetReset<T> gsr, Func<string, T> parseTry, string btnText, Texture2D ico = null, string text = null) : base(ico, text)
        {
            ui_tb = new UITextBoxBind<T>(gsr, parseTry);
            ui_tb.Width.Precent = 0.5f;
            ui_tb.Height.Set(-6, 1);
            ui_tb.HAlign = 1;
            ui_tb.VAlign = 0.5f;

            ui_btn = new UITextButton(btnText, 0.8f);
            ui_btn.HAlign = 1;
            ui_btn.VAlign = 0.5f;
            ui_btn.OnLeftClick += (e, s) => OnClick?.Invoke();

            Append(ui_tb);
            Append(ui_btn);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float width = ui_btn.GetOuterDimensions().Width + 4;

            ui_tb.Width.Pixels = -width;
            ui_tb.Left.Pixels = -width;
        }
    }
}
