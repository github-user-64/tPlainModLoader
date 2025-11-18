using SundryTool.Utils.quickBuild;
using tContentPatch;
using Terraria.UI;

namespace SundryTool
{
    internal class SettingUI_player : ModSetting
    {
        public override string Name => "玩家属性";
        public override string Title => "杂项功能: 玩家属性";

        public override UIElement GetUI()
        {
            return UIBuild.get3(Content.PlayerModify.ValSet.GetUI());
        }
    }

    internal class SettingUI_item : ModSetting
    {
        public override string Name => "手持物品属性";
        public override string Title => "杂项功能: 手持物品属性";

        public override UIElement GetUI()
        {
            return UIBuild.get3(Content.HeldItemModify.ValSet.GetUI());
        }
    }

    internal class SettingUI_function1 : ModSetting
    {
        public override string Name => "其它功能1";
        public override string Title => "杂项功能: 其它功能1";

        public override UIElement GetUI()
        {
            return UIBuild.get3(Content.Function1.Function.GetUI());
        }
    }

    internal class SettingUI_function2 : ModSetting
    {
        public override string Name => "其它功能2";
        public override string Title => "杂项功能: 其它功能2";

        public override UIElement GetUI()
        {
            return UIBuild.get3(Content.Function2.Function.GetUI());
        }
    }
}
