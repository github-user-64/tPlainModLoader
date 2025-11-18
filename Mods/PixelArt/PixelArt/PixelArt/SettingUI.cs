using PixelArt.Utils.quickBuild;
using tContentPatch;
using Terraria.UI;

namespace PixelArt
{
    internal class SettingUI_player : ModSetting
    {
        public override string Name => "设置";
        public override string Title => "像素画: 设置";

        public override UIElement GetUI()
        {
            return UIBuild.get3(Content.PixelArt.GetUI());
        }
    }
}
