using CommandHelp;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
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
            if (WorldGen.isGeneratingOrLoadingWorld) return;

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
            string tip = "默认启用" +
                "\n单人和服务端可用" +
                "\n在服务端使用该功能需要服务端加载该模组" +
                "\n可以在开服并开始游戏后使用tPlainModLoaderInjector注入服务端(一般叫TerrariaServer)" +
                "\n关闭注入的窗口不影响使用";

            List<UIElement> uis = new List<UIElement>
            {
                //UIBuild.get2(Enable, "阻止方块被转化为腐化圣神猩红\n魔粉改造枪等也无法使用\n默认启用,在服务端使用该功能需要服务端加载该模组", null, "阻止方块转化"),
                UIBuild.get2(Enable, tip, "Images/Buff_320", "阻止传播感染"),
            };

            return uis;
        }
    }
}
