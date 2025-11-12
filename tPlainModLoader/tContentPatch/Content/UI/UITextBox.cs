using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace tContentPatch.Content.UI
{
    public class UITextBox : UIPanel
    {
        public Action<string> OnTextChanged = null;
        public Action OnLostFocus = null;
        public int Text_MaxLength = -1;

        private bool focus = false;
        public bool Focus
        {
            get => focus;
            set
            {
                if (focus && !value) OnLostFocus?.Invoke();
                focus = value;
            }
        }
        private string text = null;
        public string Text
        {
            get => text;
            set => SetText(value);
        }
        private string textDefault = null;
        public string TextDefault
        {
            get => textDefault;
            set => textDefault = value ?? string.Empty;
        }

        private string text_old = null;
        private UIText ui_text = null;
        private int time1 = 0;
        private bool mouseLeftOld = false;


        public UITextBox(string text_default = "")
        {
            Text = string.Empty;
            text_old = Text;
            TextDefault = text_default;

            OverflowHidden = true;
            ui_text = new UIText(TextDefault);

            SetPadding(2);
            BackgroundColor = Color.White;
            BorderColor = Color.White;
            ui_text.ShadowColor = BackgroundColor;
            ui_text.VAlign = 0.5f;

            Append(ui_text);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            ++time1;
            if (time1 * 16 > 2000) time1 = 0;

            if (Main.mouseLeft && mouseLeftOld == false)
            {
                Focus = IsMouseHovering;
            }
            mouseLeftOld = Main.mouseLeft;
            if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape)) Focus = false;

            if (Focus)
            {
                Update_Input();
            }

            string ui_text_text = null;
            if (Focus)
            {
                ui_text_text = Text;
                if (time1 * 16 > 1000) ui_text_text += "|";

                ui_text.TextColor = Color.Black;
            }
            else
            {
                if (Text == "")
                {
                    ui_text_text = TextDefault;
                    ui_text.TextColor = Color.Gray;
                }
                else
                {
                    ui_text_text = Text;
                    ui_text.TextColor = Color.Black;
                }
            }
            ui_text.SetText(ui_text_text);
        }

        private void Update_Input()
        {
            Terraria.GameInput.PlayerInput.WritingText = true;
            Main.instance.HandleIME();
            string s = Main.GetInputText(Text);

            SetText(s);
        }

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

        public void SetTextScale(float textScale)
        {
            ui_text.SetText(text, textScale, false);
        }
    }
}
