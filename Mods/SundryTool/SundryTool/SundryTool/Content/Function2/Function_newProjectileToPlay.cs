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
    internal class Function_newProjectileToPlay : PatchMain
    {
        public static GetSetReset<bool> nptp = new GetSetReset<bool>();
        public static GetSetReset<int> nptp_play = new GetSetReset<int>(-1, -1);
        public static GetSetReset<int> nptp_id = new GetSetReset<int>(104, 104, GetSetReset.GetIntFunc(0, ProjectileID.Count - 1));
        public static GetSetReset<int> nptp_cd = new GetSetReset<int>(1, 1, GetSetReset.GetIntFunc(1));

        public override void Initialize()
        {
            nptp_play.OnValUpdate += v => Utils.OutputPlayerName(nptp_play.val);
        }

        public override void DoUpdateInWorldPrefix(Stopwatch sw)
        {
            if (nptp.val == false) return;

            if (Main.GameUpdateCount % nptp_cd.val != 0) return;

            if (nptp_play.val < 0) return;
            if (nptp_play.val >= Main.player.Length) return;

            Player player = Main.player[nptp_play.val];
            if (player?.active == false) return;

            int i = Projectile.NewProjectile(null, player.Center, Vector2.Zero,
                nptp_id.val, Function.functionDamage.val, 1);
            Main.projectile[i].timeLeft = 50;
        }

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get3("newProjectileToPlay", nptp,
                new CommandHRA<int>("play", nptp_play, new CommandInt()),
                new CommandHRA<int>("id", nptp_id, new CommandInt()),
                new CommandHRA<int>("cd", nptp_cd, new CommandInt())),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get1(nptp, nptp_play, int.Parse, "玩家id<int>", null, "生成射弹到玩家"),
            };

            return uis;
        }
    }
}
