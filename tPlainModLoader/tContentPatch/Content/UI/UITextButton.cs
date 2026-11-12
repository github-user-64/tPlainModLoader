using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace tContentPatch.Content.UI
{
    /// <summary>
    /// 文本按钮
    /// </summary>
    public class UITextButton : UIMouseText
    {
        /// <summary/>
        public readonly UIText UIText = null;
        /// <summary/>
        public readonly UIElement UI1 = null;

        /// <summary/>
        public Action OnClick = null;

        /// <summary/>
        public UITextButton(string text = null)
        {
            Width.Set(0f, 0f);
            Height.Set(20f, 0f);

            UIText = new UIText(text ?? "-", 0.8f);
            UIText.VAlign = 0.5f;
            Append(UIText);

            UI1 = new UIElement();
            UI1.Width.Set(0f, 1f);
            UI1.Height.Set(0f, 1f);
            UI1.OnLeftClick += (e, s) =>
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                OnClick?.Invoke();
            };
            UI1.OnMouseOver += (e, s) =>
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                UIText.TextColor = Colors.FancyUIFatButtonMouseOver;
            };
            UI1.OnMouseOut += (e, s) => UIText.TextColor = Color.White;
            Append(UI1);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MinWidth = UIText.MinWidth;
            MinHeight = UIText.MinHeight;
        }
    }
}
