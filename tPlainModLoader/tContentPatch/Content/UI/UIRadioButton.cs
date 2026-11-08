using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
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
    public class UIRadioButton : UIMouseText
    {
        /// <summary/>
        public readonly Utils.UIVal<bool> UV = null;
        /// <summary/>
        public bool IsChecked { get => UV.Val; set => UV.SetUIVal(value); }
        /// <summary/>
        public Action<bool> OnSetIsChecked { get => UV.OnSetVal; set => UV.OnSetVal = value; }
        /// <summary/>
        public Func<bool> OnGetIsChecked { get => UV.OnGetVal; set => UV.OnGetVal = value; }

        /// <summary/>
        public readonly UIImage UIIco = null;

        /// <summary/>
        public UIRadioButton(Texture2D ico, int width = 20, int height = 20)
        {
            Width.Set(width, 0f);
            Height.Set(height, 0f);

            UIIco = new UIImage(ico);
            UIIco.HAlign = 0.5f;
            UIIco.VAlign = 0.5f;
            UIIco.ScaleToFit = true;
            UIIco.AllowResizingDimensions = false;
            SetIco(ico);
            Append(UIIco);

            UIElement uie = new UIElement();
            uie.Width.Set(0f, 1f);
            uie.Height.Set(0f, 1f);
            uie.OnLeftClick += (e, s) =>
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                IsChecked = true;
            };
            uie.OnMouseOver += (e, s) =>
            {
                if (IsChecked != true) SoundEngine.PlaySound(SoundID.MenuTick);
            };
            Append(uie);

            UV = new Utils.UIVal<bool>(OnValUpdate);
            UV.SetUIVal(false, true);
        }

        /// <summary/>
        public void SetIco(Texture2D ico)
        {
            UIIco.SetImage(ico);
            UIIco.Width.Set(0f, 0.72f);
            UIIco.Height.Set(0f, 0.72f);

            if (ico.Width < ico.Height)
            {
                UIIco.Width.Precent *= (float)ico.Width / ico.Height;
            }
            else
            {
                UIIco.Height.Precent *= (float)ico.Height / ico.Width;
            }
        }

        private void OnValUpdate(bool v)
        {
            if (v != true) return;

            Parent?.Children?.AsParallel()?.ForAll((ui) =>
            {
                UIRadioButton rb = ui as UIRadioButton;
                if (rb == null || rb == this) return;

                rb.IsChecked = false;
            });
        }

        /// <inheritdoc/>
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            ReLogic.Content.Asset<Texture2D> img = TextureAssets.InventoryBack14;

            if (IsChecked != true)
            {
                if (IsMouseHovering != true) return;
                img = TextureAssets.InventoryBack24;
            }

            spriteBatch.Draw(img.Value, GetDimensions().ToRectangle(), Color.White);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UV.UpdateUI();
        }
    }
}
