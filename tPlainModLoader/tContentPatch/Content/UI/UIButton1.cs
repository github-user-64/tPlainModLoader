using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace tContentPatch.Content.UI
{
    /// <summary>
    /// 有个框里面显示文本, 鼠标在上方影响颜色和播放声音
    /// </summary>
    public class UIButton1 : UITextPanel<string>
    {
        /// <summary>
        /// 为<see langword="false"/>时不触发<see cref="LeftClick(UIMouseEvent)"/>
        /// </summary>
        public bool isEnable = true;
        /// <summary>
        /// 为<see langword="false"/>时不调用<see cref="Draw"/>
        /// </summary>
        public bool isDraw = true;
        /// <summary/>
        public Color EnableColorBack = new Color(63, 82, 151) * 0.8f;
        /// <summary/>
        public Color EnableColorBoredr = Color.Black;
        /// <summary/>
        public Color NoEnableColorBack = Color.Gray * 0.8f;
        /// <summary/>
        public Color NoEnableColorBoredr = Color.Black;
        /// <summary/>
        public Color MouseOverColorBack = new Color(73, 94, 171);
        /// <summary/>
        public Color MouseOverColorBoredr = Colors.FancyUIFatButtonMouseOver;

        /// <summary/>
        public UIButton1(string text, float textScale = 1f, bool large = false) : base(text, textScale, large)
        {
            OnMouseOver += FadedMouseOver;
        }

        /// <inheritdoc/>
        public override void LeftClick(UIMouseEvent evt)
        {
            if (isEnable) base.LeftClick(evt);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (isEnable)
            {
                if (IsMouseHovering)
                {
                    BackgroundColor = MouseOverColorBack;
                    BorderColor = MouseOverColorBoredr;
                }
                else
                {
                    BackgroundColor = EnableColorBack;
                    BorderColor = EnableColorBoredr;
                }
            }
            else
            {
                BackgroundColor = NoEnableColorBack;
                BorderColor = NoEnableColorBoredr;
            }
        }

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isDraw == false) return;
            base.Draw(spriteBatch);
        }

        private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            if (isEnable == false) return;
            SoundEngine.PlaySound(SoundID.MenuTick);
        }
    }
}
