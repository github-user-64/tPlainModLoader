using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;

namespace Skil.Content.UI
{
    internal class UITextButton : UIText
    {
        public UITextButton(string text, float textScale = 1, bool large = false) : base(text, textScale, large)
        {
            OnMouseOver += (e, s) =>
            {
                SoundEngine.PlaySound(12);
                TextColor = Colors.FancyUIFatButtonMouseOver;
            };
            OnMouseOut += (e, s) => TextColor = Color.White;
            OnLeftClick += (e, s) => SoundEngine.PlaySound(12);
        }
    }
}
