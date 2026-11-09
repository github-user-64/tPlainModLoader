using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace tContentPatch.Content.UI
{
    /// <summary>
    /// 文本框
    /// </summary>
    public partial class UITextBox : UIMouseText
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

            UV = new Utils.UIVal<string>(UpdateDisplayTextWhenText);
            UV.SetUIVal(null, true);
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
            UpdateInput();
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateFocus();
            if (Focus != true || NoUpdateDisplayTextWhenHasFocus != true) UV.UpdateUI();
            UpdateUIText(DisplayText);
        }

        /// <summary/>
        protected virtual void UpdateUIText(string Text = null)
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

            ++time;
            if (time * 16 > 2000) time = 0;

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
            string s = Main.GetInputText(DisplayText);

            CalculatedStyle size = GetDimensions();
            DrawIME.NeedIME = true;
            DrawIME.IME_P = new Vector2(size.X, size.Y + size.Height + 36f);

            s = CheckInputText(s);
            if (s == DisplayText) return;

            DisplayText = s;
        }

        /// <summary/>
        protected virtual string CheckInputText(string s)
        {
            if (s == null) return null;
            if (MaxInputLength < 0) return s;
            if (MaxInputLength >= s.Length) return s;

            return s.Substring(0, Math.Min(s.Length, MaxInputLength));
        }

        /// <summary>
        /// 在<see cref="Text"/>更新时更新<see cref="DisplayText"/>
        /// </summary>
        protected virtual void UpdateDisplayTextWhenText()
        {
            DisplayText = Text;
        }

        /// <summary>
        /// 在<see cref="DisplayText"/>更新时更新<see cref="Text"/>
        /// </summary>
        protected virtual void UpdateTextWhenDisplayText()
        {
            if (IsUpdateTextWhenDisplayText) Text = DisplayText;
        }

        /// <summary>
        /// 在<see cref="Focus"/>为<see langword="false"/>时更新<see cref="Text"/>
        /// </summary>
        protected virtual void UpdateTextWhenLostFocus()
        {
            if (IsUpdateTextWhenLostFocus) Text = DisplayText;
        }

        /// <summary/>
        protected virtual void GotFocus()
        {
            Main.GetInputText(null);//在获得焦点前清空输入

            OnGotFocus?.Invoke();
        }

        /// <summary/>
        protected virtual void LostFocus()
        {
            UpdateTextWhenLostFocus();

            OnLostFocus?.Invoke();
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
