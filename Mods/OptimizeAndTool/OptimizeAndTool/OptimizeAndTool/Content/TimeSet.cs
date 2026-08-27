using CommandHelp;
using OptimizeAndTool.Content.UI;
using OptimizeAndTool.Utils;
using OptimizeAndTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.UI;

namespace OptimizeAndTool.Content
{
    internal class TimeSet : PatchMain
    {
        public static GetSetReset<bool> EnableTime = new GetSetReset<bool>(false, false);
        public static GetSetReset<float> Time = new GetSetReset<float>(0, 0, GetSetReset.GetFloatFunc(timeMin, timeMax));
        public static GetSetReset<bool> EnableTimeRate = new GetSetReset<bool>(false, false);
        public static GetSetReset<int> TimeRate = new GetSetReset<int>(1, 1, GetSetReset.GetIntFunc(timeRateMin, timeRateMax));
        private const float timeMin = 0f;
        private const float timeMax = (float)(Main.dayLength + Main.nightLength);
        private const int timeRateMin = 0;
        private const int timeRateMax = 555;

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get1("timeSet", EnableTime, Time, new CommandFloat()),
                CommandBuild.get1("timeRate", EnableTimeRate, TimeRate, new CommandInt()),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            UIItemValueSliderSwitch timeSet = new UIItemValueSliderSwitch(timeMin, timeMax, null, "时间");
            timeSet.OnUpdate += _ => timeSet.SetVal(Time.val);
            timeSet.OnValUpdate += v => Time.val = v;
            timeSet.GSRSwitch = EnableTime;

            UIItemValueSliderSwitch timeRate = new UIItemValueSliderSwitch(timeRateMin, timeRateMax, null, "时间速率");
            timeRate.OnUpdate += _ => timeRate.SetVal(TimeRate.val);
            timeRate.OnValUpdate += v => TimeRate.val = (int)v;
            timeRate.GSRSwitch = EnableTimeRate;

            List<UIElement> uis = new List<UIElement>
            {
                timeSet,
                timeRate,
            };

            return uis;
        }

        public override void DoUpdateInWorldPrefix()
        {
            if (EnableTime.val == false) return;

            double time = Time.val;
            bool isDay = time < Main.dayLength;

            if (isDay == false) time -= Main.dayLength;

            Main.time = time;
            Main.dayTime = isDay;
        }

        public static void UpdateTimeRate()
        {
            if (EnableTimeRate.val == false) return;
            if (Main.gameMenu) return;

            int rate = TimeRate.val;

            Main.dayRate = rate;
            Main.desiredWorldTilesUpdateRate = rate;
        }
    }
}
