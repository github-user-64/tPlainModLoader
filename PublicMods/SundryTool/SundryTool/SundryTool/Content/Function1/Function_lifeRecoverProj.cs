using CommandHelp;
using Microsoft.Xna.Framework;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.UI;

namespace SundryTool.Content.Function1
{
    internal partial class Function_lifeRecoverProj : ModPlayer
    {
        public static GetSetReset<bool> lifeRecoverProj = new GetSetReset<bool>();
        public static GetSetReset<int> lifeRecoverProj_val = new GetSetReset<int>(16, 16);
        public static GetSetReset<int> lifeRecoverProj_cd = new GetSetReset<int>(60, 60, GetSetReset.GetIntFunc(1));

        public override void UpdatePrefix(Player This, int ThisI)
        {
            if (This != Main.LocalPlayer) return;
            if (lifeRecoverProj.val == false) return;
            if (Main.GameUpdateCount % lifeRecoverProj_cd.val != 0) return;

            int id = -1;
            float distance = float.NaN;
            for (int i = 0; i < Main.player?.Length; ++i)
            {
                if (Main.player[i] == null) continue;
                if (Main.player[i].active == false) continue;
                if (Main.player[i].dead == true) continue;
                if (Main.player[i].whoAmI == This.whoAmI) continue;

                float d = This.position.Distance(Main.player[i].position);
                if (distance != float.NaN && d > distance) continue;

                distance = d;
                id = i;
            }
            if (id == -1) id = This.whoAmI;

            if (0 > id || id > Main.player?.Length - 1) return;

            Projectile.NewProjectile(null, This.Center, Vector2.Zero,
                298, 0, 0, ai0: id, ai1: lifeRecoverProj_val.val);
        }

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get3("lifeRecoverProj", lifeRecoverProj,
                new CommandHRA<int>("set", lifeRecoverProj_val, new CommandInt()),
                new CommandHRA<int>("cd", lifeRecoverProj_cd, new CommandInt())),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get2(lifeRecoverProj, null, "Images/Buff_174", "回血射弹"),
            };

            return uis;
        }
    }
}
