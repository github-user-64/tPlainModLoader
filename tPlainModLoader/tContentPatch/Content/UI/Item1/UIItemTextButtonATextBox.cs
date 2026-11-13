using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.UI;

namespace tContentPatch.Content.UI.Item1
{
    /// <summary>
    /// 组合ui: <see cref="UITextButton"/>, <see cref="UIItemTextBox{T}"/>
    /// </summary>
    public class UIItemTextButtonATextBox<T> : UIItemTextBox<T>
    {
        /// <summary/>
        public readonly UIElement UIS = null;
        /// <summary/>
        public readonly UITextButton UIButton = null;

        /// <summary/>
        public Action OnClick { get => UIButton.OnClick; set => UIButton.OnClick = value; }

        /// <summary/>
        public UIItemTextButtonATextBox(Texture2D ico = null, string title = null, string defaultDisplayText = null, string buttonText = null) : base(ico, title, defaultDisplayText)
        {
            UIS = new UIElement();
            UIS.Width.Set(0f, 0.5f);
            UIS.Height.Set(0f, 1f);
            UIS.HAlign = 1f;
            Append(UIS);

            UIButton = new UITextButton(buttonText);
            UIButton.Height.Set(0f, 1f);
            UIButton.HAlign = 1f;
            UIButton.VAlign = 0.5f;
            UIS.Append(UIButton);

            UITextBox.Width.Set(0f, 1f);
            UITextBox.Height.Set(-8f, 1f);
            UITextBox.HAlign = 0f;
            UITextBox.VAlign = 0.5f;
            UIS.Append(UITextBox);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UITextBox.Width.Pixels = -UIButton.GetOuterDimensions().Width - 2f;
        }
    }
}
