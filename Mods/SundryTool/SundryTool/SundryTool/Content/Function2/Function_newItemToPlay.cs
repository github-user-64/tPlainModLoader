using CommandHelp;
using Microsoft.Xna.Framework;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using System.Diagnostics;
using tContentPatch;
using Terraria;
using Terraria.ID;
using Terraria.UI;

namespace SundryTool.Content.Function2
{
    /// <summary>
    /// //NoPublic
    /// </summary>
    internal class Function_newItemToPlay : PatchMain
    {
        public static GetSetReset<bool> nitp = new GetSetReset<bool>();
        public static GetSetReset<int> nitp_play = new GetSetReset<int>(-1, -1);
        public static GetSetReset<int> nitp_id = new GetSetReset<int>(3453, 3453, GetSetReset.GetIntFunc(0, ItemID.Count - 1));
        public static GetSetReset<int> nitp_cd = new GetSetReset<int>(60, 60, GetSetReset.GetIntFunc(1));
        public static GetSetReset<int> nitp_count = new GetSetReset<int>(1, 1, GetSetReset.GetIntFunc(0));

        public override void Initialize()
        {
            nitp_play.OnValUpdate += v => Utils.OutputPlayerName(nitp_play.val);
        }

        public override void DoUpdateInWorldPrefix(Stopwatch sw)
        {
            if (nitp.val == false) return;

            if (Main.GameUpdateCount % nitp_cd.val != 0) return;

            if (nitp_play.val < 0) return;
            if (nitp_play.val >= Main.player.Length) return;

            Player player = Main.player[nitp_play.val];
            if (player?.active == false) return;

            int id = Item.NewItem(null, Main.player[nitp_play.val].Center, Vector2.Zero,
                nitp_id.val, nitp_count.val, noGrabDelay: true);

            NetMessage.SendData(21, -1, -1, null, id, 1);
        }

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get3("newItemToPlay", nitp,
                new CommandHRA<int>("play", nitp_play, new CommandInt()),
                new CommandHRA<int>("id", nitp_id, new CommandInt()),
                new CommandHRA<int>("cd", nitp_cd, new CommandInt()),
                new CommandHRA<int>("count", nitp_count, new CommandInt())),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get1(nitp, nitp_play, int.Parse, "玩家id<int>", null, "生成物品到玩家"),
            };

            return uis;
        }
    }
}
