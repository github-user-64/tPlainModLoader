using CommandHelp;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using tContentPatch.Content.UI;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.UI;

namespace SundryTool.Content.Function2
{
    /// <summary>
    /// 禁用传染
    /// </summary>
    internal class Function_stopTileConvert : PatchWorldGen
    {
        public static GetSetReset<bool> Enable = new GetSetReset<bool>(true, true);

        //public override bool CanConvert(int i2, int j2, int conversionType, bool tiles, bool walls)
        //{
        //    if (Enable.val == false) return true;

        //    if (conversionType != BiomeConversionID.Corruption &&
        //        conversionType != BiomeConversionID.Hallow &&
        //        conversionType != BiomeConversionID.Crimson) return true;

        //    return false;
        //}

        public override void UpdateWorldPrefix()
        {
            if (WorldGen.isGeneratingOrLoadingWorld) return;//正在生成或加载世界

            CreativePowers.StopBiomeSpreadPower cp = CreativePowerManager.Instance.GetPower<CreativePowers.StopBiomeSpreadPower>();
            cp.SetPowerInfo(Enable.val);
        }

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get2("stopTileConvert", Enable),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            string tip = "单人和服务端可用" +
                "\n在服务端使用该功能需要服务端加载该模组" +
                "\n可以在开服并开始游戏后使用tPlainModLoaderInjector注入服务端(一般叫TerrariaServer)" +
                "\n关闭注入的窗口不影响使用";

            tContentPatch.Content.UI.Item1.UIItemSwitch ui = UIBuild.get2(Enable, tip, "Images/Buff_320", "阻止传播感染");
            ui.MouseTextStyle = UIMouseText.MouseTextStyle2;

            return new List<UIElement> { ui };
        }
    }
}
