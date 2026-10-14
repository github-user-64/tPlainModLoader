using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.UI;

namespace CreativeInventory.CreativeInventory
{
    internal class UISwitch : UIElement
    {
        public Action<bool> OnValUpdate = null;

        protected bool val = false;
        protected float anim = 0;//0=关, 1=开

        private readonly Asset<Texture2D> _texture = null;
        private readonly Asset<Texture2D> _textureInner = null;

        public UISwitch()
        {
            _texture = Main.Assets.Request<Texture2D>("Images/UI/Scrollbar2");
            _textureInner = Main.Assets.Request<Texture2D>("Images/UI/ScrollbarInner");

            Width.Set(40, 0);
            Height.Set(_texture.Width(), 0);

            OnLeftClick += (e, s) =>
            {
                SetVal(!val);
                SoundEngine.PlaySound(12);
            };
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);

            SoundEngine.PlaySound(12);
        }

        public void SetVal(bool v)
        {
            if (v == val) return;

            val = v;
            OnValUpdate?.Invoke(val);
        }

        public bool GetVal() => val;

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (val) anim += (anim + 0.05f) / 2f;
            else anim -= (1f - anim + 0.05f) / 2f;

            anim = MathHelper.Clamp(anim, 0, 1);

            //

            int paddint = 4;

            CalculatedStyle dim = GetDimensions();
            CalculatedStyle dimBar = dim;
            dimBar.X += paddint;
            dimBar.Width = dimBar.Height - 8;

            float max = dim.Width - (paddint * 2) - dimBar.Width;
            dimBar.X += max * anim;

            Color color1 = Color.White;
            Color color2 = Color.White;
            if (val != true)
            {
                color1.R = (byte)MathHelper.Clamp(color1.R * 0.5f, byte.MinValue, byte.MaxValue);
                color1.G = (byte)MathHelper.Clamp(color1.G * 0.5f, byte.MinValue, byte.MaxValue);
                color1.A = (byte)MathHelper.Clamp(color1.A * 0.5f, byte.MinValue, byte.MaxValue);

                color2 *= 0.6f;
            }

            if (IsMouseHovering) color2 = Color.Wheat;

            DrawRect(spriteBatch, _texture.Value, dim.ToRectangle(), color1);
            DrawRect(spriteBatch, _textureInner.Value, dimBar.ToRectangle(), color2);
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
