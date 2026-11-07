using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace tContentPatch.Content.UI
{
    /// <summary/>
    public partial class UITextBox : UIElement
    {
        /// <summary>
        /// 显示文本的ui
        /// </summary>
        public readonly UIText UIText = null;
        /// <summary>
        /// ui的背景
        /// </summary>
        public readonly UIElement UIBackStyle = null;

        private int time = 0;
        private bool mouseLeftOld = false;

        /// <summary/>
        public UITextBox(string defaultDisplayText = null)
        {
            DefaultDisplayText = defaultDisplayText;

            Width.Set(0f, 1f);
            Height.Set(25f, 0f);
            OverflowHidden = true;//溢出隐藏

            UIBackStyle = new UIState();
            Append(UIBackStyle);

            UIText = new UIText(string.Empty);
            UIText.Width.Set(0f, 0f);
            UIText.Height.Set(0f, 0f);
            UIText.VAlign = 0.5f;
            UIText.SetPadding(4f);
            Append(UIText);

            OnSetBackStyle();

            UV = new Utils.UIVal<string>(UpdateUIText);
            UV.SetUIVal(null, true);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            ++time;
            if (time * 16 > 2000) time = 0;

            UpdateFocus();
            UpdateInput();
            UV.UpdateUI(true);
        }

        /// <summary/>
        protected virtual void UpdateUIText(string Text)
        {
            if (Text == null) Text = string.Empty;

            if (Text.Length > 0 || Focus)
            {
                UIText.TextColor = UITextColor;
                UIText.ShadowColor = UITextShadowColor;

                if (Focus && time * 16 > 1000) Text += "|";
            }
            else
            {
                Text = DefaultDisplayText;
                UIText.TextColor = UITextColorDeft;
                UIText.ShadowColor = UITextShadowColorDeft;
            }

            if (Text == null) Text = string.Empty;
            if (Text != UIText.Text) UIText.SetText(Text);
        }

        /// <summary/>
        protected virtual void UpdateFocus()
        {
            bool mouseLeftOld = this.mouseLeftOld;
            this.mouseLeftOld = Main.mouseLeft;

            if (
                //Main.gamePaused ||
                Main.drawingPlayerChat ||
                Main.ingameOptionsWindow ||
                Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                Focus = false;
                return;
            }

            if (Main.mouseLeft && mouseLeftOld == false)
            {
                Focus = IsMouseHovering;
            }
        }

        /// <summary/>
        protected virtual void UpdateInput()
        {
            if (Focus == false) return;

            Terraria.GameInput.PlayerInput.WritingText = true;
            Main.instance.HandleIME();
            string s = Main.GetInputText(Text);

            CalculatedStyle size = GetDimensions();
            DrawIME.NeedIME = true;
            DrawIME.IME_P = new Vector2(size.X, size.Y + size.Height + 36);

            Text = s;
        }

        /// <inheritdoc/>
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            OnDrawUpdateInput();
        }

        /// <summary/>
        protected virtual void OnDrawUpdateInput()
        {

        }

        /// <summary/>
        protected string CheckSetText(string s)
        {
            if (s == null) return null;
            if (TextMaxLength < 0) return s;
            if (TextMaxLength < s.Length) return s.Substring(0, Math.Min(s.Length, TextMaxLength));

            return s;
        }

        /// <summary>
        /// 设置页面背景
        /// </summary>
        protected virtual void OnSetBackStyle()
        {
            Style1(this);
        }
    }
}
