using Microsoft.Xna.Framework.Input;
using tContentPatch;
using Terraria;
using Terraria.DataStructures;

namespace DeadEffect.Common.Effects
{
    internal class DeBug : PatchMain
    {
        public override void DoUpdateInWorldPrefix()
        {
            if (Config.Data.DeBug != true) return;
            if (Main.dedServ == true) return;

            Player player = Main.LocalPlayer;
            if (player == null) return;
            if (player.active != true) return;

            if (Utils.IsKeyClick(Keys.NumPad1)) player.Hurt(new PlayerDeathReason(), 80, 0);
            if (Utils.IsKeyClick(Keys.NumPad2)) player.Hurt(new PlayerDeathReason(), 100, 0);
            if (Utils.IsKeyClick(Keys.NumPad3)) player.KillMe(new PlayerDeathReason(), 0, 0);
        }
    }
}
