using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;

namespace OptimizeAndTool.Content.ServerList
{
    internal class UIButton : UIImage
    {
        private string mouseText = null;

        public UIButton(float size, string mouseText, Asset<Texture2D> texture) : base(texture)
        {
            Width.Pixels = Height.Pixels = size;
            ScaleToFit = true;
            this.mouseText = mouseText;

            OnMouseOver += (e, s) =>
            {
                SoundEngine.PlaySound(12);
                Color = Color.White;
            };

            Color = Color.White * 0.5f;
            OnMouseOut += (e, s) => Color = Color.White * 0.5f;

            OnLeftClick += (e, s) =>
            {
                SoundEngine.PlaySound(12);
            };
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsMouseHovering && mouseText != null) Main.instance.MouseText(mouseText);
        }
    }
}
