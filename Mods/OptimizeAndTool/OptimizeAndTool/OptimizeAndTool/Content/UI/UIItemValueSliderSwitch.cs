using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OptimizeAndTool.Utils;
using tContentPatch.Content.UI.ModSet;
using Terraria.Audio;
using Terraria.ID;
using Terraria.UI;

namespace OptimizeAndTool.Content.UI
{
    internal class UIItemValueSliderSwitch : UIItemValueSliderFloat
    {
        public GetSetReset<bool> GSRSwitch = null;
        protected Color oldColor;

        public UIItemValueSliderSwitch(float min, float max, Texture2D ico = null, string text = null) : base(min, max, ico, text)
        {
            MouseText = "右键开关";
            oldColor = Color;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Color = GSRSwitch.val ? new Color(100, 100, 71) : oldColor;
        }

        public override void RightClick(UIMouseEvent evt)
        {
            base.RightClick(evt);

            SoundEngine.PlaySound(SoundID.MenuTick);
            GSRSwitch.val = !GSRSwitch.val;
        }
    }
}
