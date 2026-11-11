using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace tContentPatch.Content.UI
{
    /// <summary>
    /// 开关
    /// </summary>
    public class UISwitch : UIMouseText
    {
        /// <summary/>
        public readonly Utils.UIVal<bool> UV = null;
        /// <summary/>
        public bool IsChecked { get => UV.Val; set => UV.SetUIVal(value); }
        /// <summary/>
        public Func<bool> OnGetIsChecked { set => UV.OnGetVal = value; }
        /// <summary/>
        public Action<bool> OnSetIsChecked { set => UV.OnSetVal = value; }
        /// <summary/>
        public Action<bool> OnIsCheckChanged { get => UV.OnValChanged; set => UV.OnValChanged = value; }

        /// <summary/>
        public readonly UIText UIText = null;
        /// <summary/>
        public readonly UIImageFramed UIIco = null;
        private Rectangle Frame;

        /// <summary/>
        public UISwitch()
        {
            Width.Set(50f, 0f);
            Height.Set(20f, 0f);

            UIText = new UIText("-", 0.8f);
            UIText.VAlign = 0.5f;
            Append(UIText);

            Asset<Texture2D> img = Main.Assets.Request<Texture2D>("Images/UI/Settings_Toggle", AssetRequestMode.ImmediateLoad);
            Frame = img.Frame(horizontalFrames: 2);

            UIIco = new UIImageFramed(img, Frame);
            UIIco.HAlign = 1;
            UIIco.VAlign = 0.5f;
            Append(UIIco);

            UIElement uie = new UIElement();
            uie.Width.Set(0f, 1f);
            uie.Height.Set(0f, 1f);
            uie.OnLeftClick += (e, s) =>
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                UV.SetUIVal(!IsChecked);
            };
            uie.OnMouseOver += (e, s) =>
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                UIText.TextColor = UIIco.Color = Colors.FancyUIFatButtonMouseOver;
            };
            uie.OnMouseOut += (e, s) => UIText.TextColor = UIIco.Color = Color.White;
            Append(uie);

            UV = new Utils.UIVal<bool>(UpdateUI);
            UV.SetUIVal(false, true);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UV.UpdateUI();
        }

        private void UpdateUI()
        {
            bool v = IsChecked;
            UIText.SetText(v ? "开" : "关");

            //

            Rectangle f = Frame;
            if (v) f.X += f.Height;

            UIIco.SetFrame(f);
        }
    }
}
