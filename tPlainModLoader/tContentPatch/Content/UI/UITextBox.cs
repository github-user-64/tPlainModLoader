using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace tContentPatch.Content.UI
{
    /// <summary/>
    public class UITextBox : UIElement
    {
        /// <summary/>
        public Color UITextColor;
        /// <summary/>
        public Color UITextColorDeft;
        /// <summary/>
        public Color UITextShadowColor;
        /// <summary/>
        public Color UITextShadowColorDeft;
        /// <summary/>
        public Action<string> OnTextChanged = null;
        /// <summary/>
        public Action OnGotFocus = null;
        /// <summary/>
        public Action OnLostFocus = null;
        /// <summary/>
        public int Text_MaxLength = -1;

        private bool focus = false;
        /// <summary/>
        public bool Focus
        {
            get => focus;
            set
            {
                if (focus == value) return;
                focus = value;
                (focus ? OnGotFocus : OnLostFocus)?.Invoke();
            }
        }
        private string text = null;
        /// <summary/>
        public string Text
        {
            get => text;
            set => SetText(value);
        }
        private string textDefault = null;
        /// <summary/>
        public string TextDefault
        {
            get => textDefault;
            set => textDefault = value ?? string.Empty;
        }

        /// <summary>
        /// 显示文本的ui
        /// </summary>
        public readonly UIText UIText = null;
        /// <summary>
        /// ui的背景
        /// </summary>
        public readonly UIElement UIBackStyle = null;

        private string text_old = null;
        private int time = 0;
        private bool mouseLeftOld = false;

        /// <summary/>
        public UITextBox(string text_default = "")
        {
            Text = string.Empty;
            text_old = Text;
            TextDefault = text_default;

            Width.Set(0f, 1f);
            Height.Set(25f, 0f);
            OverflowHidden = true;//溢出隐藏

            UIBackStyle = new UIState();
            Append(UIBackStyle);

            UIText = new UIText(TextDefault);
            UIText.Width.Set(0f, 0f);
            UIText.Height.Set(0f, 0f);
            UIText.VAlign = 0.5f;
            UIText.SetPadding(4f);
            Append(UIText);

            SetBackStyle();
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            ++time;
            if (time * 16 > 2000) time = 0;

            UpdateFocus();
            UpdateUIText();
        }

        /// <summary/>
        protected virtual void UpdateUIText()
        {
            string uitext_text = string.Empty;

            if (Focus)
            {
                uitext_text = Text;
                if (time * 16 > 1000) uitext_text += "|";

                UIText.TextColor = UITextColor;
                UIText.ShadowColor = UITextShadowColor;
            }
            else
            {
                if (Text.Length < 1)
                {
                    uitext_text = TextDefault;
                    UIText.TextColor = UITextColorDeft;
                    UIText.ShadowColor = UITextShadowColorDeft;
                }
                else
                {
                    uitext_text = Text;
                    UIText.TextColor = UITextColor;
                    UIText.ShadowColor = UITextShadowColor;
                }
            }

            if (uitext_text != UIText.Text) UIText.SetText(uitext_text);
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

            SetText(s);
        }

        /// <summary/>
        protected virtual void OnDrawUpdateInput()
        {
            UpdateInput();
        }

        /// <inheritdoc/>
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            OnDrawUpdateInput();
        }

        /// <summary/>
        public void SetText(string s)
        {
            if (s == null) s = string.Empty;
            if (text_old == s) return;

            if (Text_MaxLength > -1 && s.Length > Text_MaxLength)
            {
                s = s.Substring(0, Math.Min(s.Length, Text_MaxLength));
            }

            if (text_old == s) return;

            text_old = text = s;
            OnTextChanged?.Invoke(Text);
        }

        /// <summary>
        /// 设置页面背景
        /// </summary>
        protected virtual void SetBackStyle()
        {
            Style1(this);
        }

        //

        /// <summary/>
        public static void Style1(UITextBox ui)
        {
            UIPanel back = new UIPanel();
            back.Width.Set(0f, 1f);
            back.Height.Set(0f, 1f);
            back.BackgroundColor = Color.White;
            back.BorderColor = Color.White;

            ui.UIBackStyle.RemoveAllChildren();
            ui.UIBackStyle.Append(back);

            ui.UITextColor = Color.Black;
            ui.UITextColorDeft = Color.Gray;
            ui.UITextShadowColor = ui.UITextShadowColorDeft = back.BackgroundColor;
        }

        /// <summary/>
        public static void Style2(UITextBox ui)
        {
            UIPanel2 back = new UIPanel2();
            back.Width.Set(0f, 1f);
            back.Height.Set(0f, 1f);
            back.BackgroundColor = default;
            back.BorderColor = Color.MidnightBlue * 0.6f;
            back.OnMouseOver += (e, s) => SoundEngine.PlaySound(SoundID.MenuTick);
            back.OnUpdate += _ =>
            {
                back.BorderColor = Color.MidnightBlue;
                if (ui.Focus != true && back.IsMouseHovering != true) back.BorderColor *= 0.6f;

                back.BackgroundColor = ui.Focus ? Color.MidnightBlue * 0.6f : default;
            };

            ui.UIBackStyle.RemoveAllChildren();
            ui.UIBackStyle.Append(back);

            ui.UITextColor = Color.White;
            ui.UITextColorDeft = Color.MidnightBlue * 0.6f;
            ui.UITextShadowColor = ui.UITextShadowColorDeft = default;

            ui.UIText.SetPadding(6f);
            ui.OnGotFocus += () => SoundEngine.PlaySound(SoundID.MenuTick);
            ui.OnLostFocus += () => SoundEngine.PlaySound(SoundID.MenuClose);
        }
    }
}
