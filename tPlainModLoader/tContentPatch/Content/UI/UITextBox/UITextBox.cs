using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        /// <summary/>
        protected int InputCD = 0;

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

            UV = new Utils.UIVal<string>(UpdateDisplayTextWhenText);
            UV.SetUIVal(null, true);
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            OnDrawUpdateInput();//在绘制里更新, 在Update里输入法会鬼畜

            DrawSelect(spriteBatch);//绘制光标
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            if (StyleLoaded == false)
            {
                StyleLoaded = true;
                SetBackStyle();
            }

            base.Update(gameTime);

            UpdateFocus();//更新焦点
            UV.UpdateUI();//更新Text

            UpdateSelect();//更新光标

            UpdateUIText(DisplayText);//更新ui
        }

        /// <summary/>
        protected virtual void UpdateUIText(string Text = null)
        {
            if (Text == null) Text = string.Empty;

            if (Text.Length > 0 || Focus)
            {
                UIText.TextColor = UITextColor;
                UIText.ShadowColor = UITextShadowColor;
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
            if (
                //Main.gamePaused ||
                Main.drawingPlayerChat ||
                Main.ingameOptionsWindow ||
                Main.keyState.IsKeyDown(Keys.Escape))
            {
                Focus = false;
                return;
            }

            if (InputUtils.MouseLeftClick) Focus = IsMouseHovering;
        }

        /// <summary/>
        protected virtual void OnDrawUpdateInput()
        {
            UpdateInput();
        }

        /// <summary/>
        protected virtual void GotFocus()
        {
            Main.GetInputText(null);//在获得焦点前清空输入

            ResetSelectTime();//重置光标时间

            OnGotFocus?.Invoke();
        }

        /// <summary/>
        protected virtual void LostFocus()
        {
            UpdateTextWhenLostFocus();//将DisplayText更新到Text

            OnLostFocus?.Invoke();
        }

        /// <summary>
        /// 设置页面背景
        /// </summary>
        protected virtual void SetBackStyle()
        {
            Style?.Invoke(this);
        }
    }
}
