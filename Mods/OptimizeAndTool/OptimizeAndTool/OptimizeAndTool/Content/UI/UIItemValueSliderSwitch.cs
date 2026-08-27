using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OptimizeAndTool.Utils;
using tContentPatch.Content.UI.ModSet;
using Terraria;
using Terraria.Audio;
using Terraria.UI;

namespace OptimizeAndTool.Content.UI
{
    internal class UIItemValueSliderSwitch : UIItemValueSlider
    {
        public string MouseText = "右键开关";
        public GetSetReset<bool> GSRSwitch = null;

        public UIItemValueSliderSwitch(int min, int max, Texture2D ico = null, string text = null) : base(min, max, ico, text)
        {
        }

        public UIItemValueSliderSwitch(float min, float max, Texture2D ico = null, string text = null) : base(min, max, ico, text)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            color = GSRSwitch.val ? new Color(100, 100, 71) : new Color(73, 94, 171);

            if (IsMouseHovering == false) return;
            if (MouseText == null) return;

            Main.instance.MouseText(MouseText);
        }

        public override void RightClick(UIMouseEvent evt)
        {
            base.RightClick(evt);

            SoundEngine.PlaySound(12);
            GSRSwitch.val = !GSRSwitch.val;
        }
    }
}
