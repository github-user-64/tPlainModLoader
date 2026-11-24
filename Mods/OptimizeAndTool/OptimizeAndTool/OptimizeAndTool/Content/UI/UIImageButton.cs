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

        public Color TextureColor = Color.White;

        public UIImageButton(float size, Texture2D nonReloadingTexture) : base(nonReloadingTexture) => Init(size, ref UIMouseText);

        public UIImageButton(float size, Asset<Texture2D> texture) : base(texture) => Init(size, ref UIMouseText);

        public UIImageButton(float size, string texture) : this(size, Main.Assets.Request<Texture2D>(texture)) { }

        private void Init(float size, ref UIMouseText mt)
        {
            Width.Set(size, 0f);
            Height.Set(size, 0f);
            ScaleToFit = true;
            OnLeftClick += (e, s) => SoundEngine.PlaySound(SoundID.MenuTick);

            mt = new UIMouseText();
            UIMouseText.Width.Set(0f, 1f);
            UIMouseText.Height.Set(0f, 1f);
            UIMouseText.OnMouseOver += (e, s) => SoundEngine.PlaySound(SoundID.MenuTick);
            Append(UIMouseText);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Color = TextureColor;
            if (IsMouseHovering != true) Color *= 0.6f;
        }
    }
}
