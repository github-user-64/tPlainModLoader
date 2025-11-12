using CommandHelp;
using Skil.Utils;
using Skil.Utils.quickBuild;
using System.Collections.Generic;
using Terraria.UI;

namespace Skil.Content
{
    public class SkilListControl1
    {
        public static GetSetReset<int> damage = new GetSetReset<int>();
        public static GetSetReset<bool> aimAdvance = new GetSetReset<bool>();
        public static GetSetReset<float> aimAdvance_val = new GetSetReset<float>(38, 38);

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>();
            cos.Add(new CommandHRA<int>("damage", damage, new CommandInt()));
            cos.Add(CommandBuild.get1("aimAdvance", aimAdvance, aimAdvance_val, new CommandFloat()));
            cos.AddRange(skil1.GetCO());
            cos.AddRange(skil2.GetCO());
            cos.AddRange(skil3.GetCO());
            cos.AddRange(skil4.GetCO());
            cos.AddRange(skil6.GetCO());
            cos.AddRange(skil7.GetCO());
            cos.AddRange(skil8.GetCO());
            cos.AddRange(skil10.GetCO());
            cos.AddRange(skil11.GetCO());
            cos.AddRange(skil13.GetCO());
            cos.AddRange(skil14.GetCO());

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>();
            uis.Add(new UI.UIItemTextBoxBind<int>(damage, int.Parse, null, "技能伤害") { MouseText = "<int>" });
            uis.Add(UIBuild.get1(aimAdvance, aimAdvance_val, float.Parse, "应用于[技能2(38), 技能8(40), 技能9(60)]<float>", null, "预瞄"));
            uis.AddRange(skil1.GetUI());
            uis.AddRange(skil2.GetUI());
            uis.AddRange(skil3.GetUI());
            uis.AddRange(skil4.GetUI());
            uis.AddRange(skil6.GetUI());
            uis.AddRange(skil7.GetUI());
            uis.AddRange(skil8.GetUI());
            uis.AddRange(skil10.GetUI());
            uis.AddRange(skil11.GetUI());
            uis.AddRange(skil13.GetUI());
            uis.AddRange(skil14.GetUI());

            return uis;
        }
    }

    public class SkilListControl2
    {
        public static List<CommandObject> GetCO()
        {
            return skil5.GetCO();
        }

        public static List<UIElement> GetUI()
        {
            return skil5.GetUI();
        }
    }

    public class SkilListControl3
    {
        public static List<CommandObject> GetCO()
        {
            return skil9.GetCO();
        }

        public static List<UIElement> GetUI()
        {
            return skil9.GetUI();
        }
    }

    public class SkilListControl4
    {
        public static List<CommandObject> GetCO()
        {
            return skil12.GetCO();
        }

        public static List<UIElement> GetUI()
        {
            return skil12.GetUI();
        }
    }
}
