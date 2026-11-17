using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.UI;

namespace tContentPatch.Content.UI
{
    public partial class UISwitch
    {
        /// <summary>
        /// <see cref="Style2(UISwitch)"/>的ui
        /// </summary>
        public class UISwitchStyle2 : UIElement
        {
            /// <summary/>
            protected readonly Asset<Texture2D> texture = null;
            /// <summary/>
            protected readonly Asset<Texture2D> textureInner = null;

            /// <summary/>
            protected float anim = 0f;//0=关, 1=开

            /// <summary/>
            public readonly UISwitch ParentUI = null;

            /// <summary/>
            public UISwitchStyle2(UISwitch parent)
            {
                ParentUI = parent;
                if (ParentUI == null) throw new ArgumentNullException(nameof(ParentUI));

                texture = Main.Assets.Request<Texture2D>("Images/UI/Scrollbar2");
                textureInner = Main.Assets.Request<Texture2D>("Images/UI/ScrollbarInner");

                Width.Set(40f, 0f);
                Height.Set(texture.Width(), 0f);
            }

            /// <inheritdoc/>
            protected override void DrawSelf(SpriteBatch spriteBatch)
            {
                if (ParentUI.IsChecked) anim += (anim + 0.05f) / 2f;
                else anim -= (1f - anim + 0.05f) / 2f;

                anim = MathHelper.Clamp(anim, 0f, 1f);

                //

                int paddint = 4;

                CalculatedStyle dim = GetDimensions();
                CalculatedStyle dimBar = dim;
                dimBar.X += paddint;
                dimBar.Width = dimBar.Height - 8f;

                float max = dim.Width - (paddint * 2f) - dimBar.Width;
                dimBar.X += max * anim;

                Color color1 = Color.White;
                Color color2 = Color.White;
                if (ParentUI.IsChecked != true)
                {
                    color1.R = (byte)MathHelper.Clamp(color1.R * 0.5f, byte.MinValue, byte.MaxValue);
                    color1.G = (byte)MathHelper.Clamp(color1.G * 0.5f, byte.MinValue, byte.MaxValue);
                    color1.A = (byte)MathHelper.Clamp(color1.A * 0.5f, byte.MinValue, byte.MaxValue);

                    color2 *= 0.6f;
                }

                if (ParentUI.IsMouseHovering) color2 = Color.Wheat;

                DrawRect(spriteBatch, texture.Value, dim.ToRectangle(), color1);
                DrawRect(spriteBatch, textureInner.Value, dimBar.ToRectangle(), color2);
            }

            private void DrawRect(SpriteBatch spriteBatch, Texture2D texture, Rectangle dim, Color color)
            {
                int v = 6;
                float r = MathHelper.TwoPi / -4f;

                spriteBatch.Draw(texture, new Rectangle(dim.X, dim.Y + dim.Height, dim.Height, v), new Rectangle(0, 0, texture.Width, v),
                    color,
                    r, default,
                    SpriteEffects.None, 0);
                spriteBatch.Draw(texture, new Rectangle(dim.X + dim.Width - v, dim.Y + dim.Height, dim.Height, v), new Rectangle(0, texture.Height - v, texture.Width, v),
                    color,
                    r, default,
                    SpriteEffects.None, 0);
                spriteBatch.Draw(texture, new Rectangle(dim.X + v, dim.Y + dim.Height, dim.Height, dim.Width - (2 * v)), new Rectangle(0, v, texture.Width, texture.Height - (v * 2)),
                    color,
                    r, default,
                    SpriteEffects.None, 0);
            }
        }
    }
}
