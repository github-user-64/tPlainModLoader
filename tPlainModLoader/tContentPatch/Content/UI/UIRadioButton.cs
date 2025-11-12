using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace tContentPatch.Content.UI
{
    /// <summary>
    /// 单选框
    /// </summary>
    public class UIRadioButton : UIElement
    {
        private bool _isChecked = false;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked == value) return;
                _isChecked = value;
                if (_isChecked) Checked();
            }
        }
        public Action OnChecked = null;
        public string MouseHoveringText = null;
        protected UIImage ico1 = null;

        public UIRadioButton()
        {
            OnLeftClick += (e, s) =>
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                IsChecked = true;
            };
        }

        public UIRadioButton(Texture2D ico, int width, int height) : this()
        {
            Width.Set(width, 0);
            Height.Set(height, 0);

            SetIco(ico);
        }

        public void SetIco(Texture2D ico)
        {
            if (ico1 == null)
            {
                ico1 = new UIImage(ico);
                ico1.Width.Pixels = ico1.Height.Pixels = 0;
                ico1.HAlign = 0.5f;
                ico1.VAlign = 0.5f;
                ico1.ScaleToFit = true;
                ico1.AllowResizingDimensions = false;
                Append(ico1);
            }
            else ico1.SetImage(ico);

            ico1.Width.Pixels = 0;
            ico1.Height.Pixels = 0;
            ico1.Width.Precent = 0.72f;
            ico1.Height.Precent = 0.72f;

            if (ico.Width < ico.Height)
            {
                ico1.Width.Precent *= (float)ico.Width / ico.Height;
            }
            else
            {
                ico1.Height.Precent *= (float)ico.Height / ico.Width;
            }

            //ico1.Width.Pixels = Width.Pixels - (Width.Pixels * 0.4f);
            //ico1.Height.Pixels = Height.Pixels - (Height.Pixels * 0.4f);

            //if (ico.Width < ico.Height)
            //{
            //    ico1.Width.Pixels *= (float)ico.Width / ico.Height;
            //}
            //else
            //{
            //    ico1.Height.Pixels *= (float)ico.Height / ico.Width;
            //}
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsMouseHovering && MouseHoveringText != null)
            {
                Main.instance.MouseText(MouseHoveringText);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (IsChecked == false) return;

            CalculatedStyle dim = GetDimensions();

            spriteBatch.Draw(TextureAssets.InventoryBack14.Value,
                new Rectangle((int)dim.X, (int)dim.Y, (int)Width.Pixels, (int)Height.Pixels), Color.White);
        }

        private void Checked()
        {
            Parent?.Children?.AsParallel()?.ForAll((ui) =>
            {
                UIRadioButton rb = ui as UIRadioButton;
                if (rb == null || rb == this) return;

                rb.IsChecked = false;
            });

            OnChecked?.Invoke();
        }
    }
}
