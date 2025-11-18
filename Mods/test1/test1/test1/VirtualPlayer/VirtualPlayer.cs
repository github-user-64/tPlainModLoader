using Microsoft.Xna.Framework;
using System.Linq;
using tContentPatch;
using Terraria;

namespace test1.VirtualPlayer
{
    internal class VirtualPlayer : ModPlayer
    {
        public override void UpdatePrefix(Player This, int playerI)
        {
            if (VP.vps.Contains(This) == false) return;

            Player t = Main.player.FirstOrDefault(i => i.active && (i.name == "[c/00ffff:固态地面]" || i.name == "超级无敌黑旋风"));
            if (t == null) return;

            Vector2 pos = t.position + new Vector2(-200 + (255 - playerI) * 50, -100);

            This.Center = pos;
            This.velocity = t.velocity;

            This.controlJump = true;
            This.controlDown = true;
            This.velocity.Y = -1;

            //This.position = This.oldPosition;

            //NetMessage.SendData(107, -1, playerI,
            //        NetworkText.FromLiteral($"{t.controlJump} {t.controlDown} {t.controlDownHold} {t.tryKeepingHoveringDown}"),
            //        255, 100, 100, 200,
            //        460);

            NetMessage.SendData(13, -1, playerI, number: playerI);//控制,属性,位置
        }
    }
}
