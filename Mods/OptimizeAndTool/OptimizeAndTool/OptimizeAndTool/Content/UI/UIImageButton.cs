using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using tContentPatch.Content.UI;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;

namespace OptimizeAndTool.Content.UI
{
    public class UIImageButton : UIImage
    {
        public readonly UIMouseText UIMouseText = null;

        public string MouseText { get => UIMouseText.MouseText; set => UIMouseText.MouseText = value; }
        public Action<string> MouseTextStyle { get => UIMouseText.MouseTextStyle; set => UIMouseText.MouseTextStyle = value; }

        public UIImageButton(float size, Texture2D nonReloadingTexture) : base(nonReloadingTexture)
        {
            Width.Set(size, 0f);
            Height.Set(size, 0f);
            Color = Color.White * 0.6f;
            ScaleToFit = true;
            OnLeftClick += (e, s) => SoundEngine.PlaySound(SoundID.MenuTick);

            UIMouseText = new UIMouseText();
            UIMouseText.Width.Set(0f, 1f);
            UIMouseText.Height.Set(0f, 1f);
            UIMouseText.OnMouseOver += (e, s) =>
            {
                Color = Color.White;
                SoundEngine.PlaySound(SoundID.MenuTick);
            };
            UIMouseText.OnMouseOut += (e, s) => Color = Color.White * 0.6f;
            Append(UIMouseText);
        }

        public UIImageButton(float size, Asset<Texture2D> texture) : this(size, texture.Value) { }

        public UIImageButton(float size, string texture) : this(size, Main.Assets.Request<Texture2D>(texture)) { }
    }
}
