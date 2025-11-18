using Skil.Content;
using Skil.Utils.quickBuild;
using tContentPatch;
using Terraria.UI;

namespace Skil
{
    internal class SettingUI_SkilList1 : ModSetting
    {
        public override string Name => "技能列表1";
        public override string Title => "魔法和技能: 技能列表1";

        public override UIElement GetUI()
        {
            return UIBuild.get3(SkilListControl1.GetUI());
        }
    }

    internal class SettingUI_SkilList2 : ModSetting
    {
        public override string Name => "技能列表2";
        public override string Title => "魔法和技能: 技能列表2";

        public override UIElement GetUI()
        {
            return UIBuild.get3(SkilListControl2.GetUI());
        }
    }

    internal class SettingUI_SkilList3 : ModSetting
    {
        public override string Name => "技能列表3";
        public override string Title => "魔法和技能: 技能列表3";

        public override UIElement GetUI()
        {
            return UIBuild.get3(SkilListControl3.GetUI());
        }
    }

    internal class SettingUI_SkilList4 : ModSetting
    {
        public override string Name => "技能列表4";
        public override string Title => "魔法和技能: 技能列表4";

        public override UIElement GetUI()
        {
            return UIBuild.get3(SkilListControl4.GetUI());
        }
    }
}
