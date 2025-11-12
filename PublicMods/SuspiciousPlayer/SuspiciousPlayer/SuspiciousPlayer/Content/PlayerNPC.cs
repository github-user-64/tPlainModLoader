using Microsoft.Xna.Framework;
using SuspiciousPlayer.Content.VirtualPlayer;
using System.Linq;
using tContentPatch;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;

namespace SuspiciousPlayer.Content.Event1
{
    internal class PlayerNPC : ModPlayer
    {
        public override void UpdatePrefix(Player This, int playerI)
        {
            if (VP.vps?.Contains(This) == false) return;

            if (This.buffType.Contains(137) == false) return;

            This.dead = true;
            if (Main.netMode == 2)
            {
                NetMessage.SendData(107, -1, playerI,
                    NetworkText.FromLiteral($"{This.name}被激怒了"),
                    255, 175, 75, 255,
                    460);

                NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(This.position,
                    341, SoundID.NPCDeath59.Style));
                
            }
            else
            {
                Main.NewText($"{This.name}被激怒了", 175, 75, 255);

                //ushort index = SoundID.IndexByName[nameof(SoundID.NPCDeath59)];
                //LegacySoundStyle sound = SoundID.SoundByIndex[index];
                SoundEngine.PlaySound(SoundID.NPCDeath59);
            }

            Event.Run(This.Center);
        }
    }

    public class PlayerNPC_addBuff : ModProjectile
    {
        public override void UpdatePostfix(Projectile This, int i)
        {
            if (This.type != 406) return;
            if (This.active == false) return;
            if (Main.netMode != 2) return;
            if (This.ai[0] > 3f == false) return;

            for (int pi = 0; pi < VP.vps?.Count; pi++)
            {
                Player player = VP.vps[pi];

                if (player.whoAmI == This.owner) continue;

                Rectangle rect1 = new Rectangle((int)(This.position.X + This.velocity.X), (int)(This.position.Y + This.velocity.Y),
                    This.width, This.height);
                Rectangle rect2 = new Rectangle((int)player.position.X - 10, (int)player.position.Y - 10,
                    player.width + 20, player.height + 20);

                if (rect1.Intersects(rect2))
                {
                    This.Kill();
                    player.AddBuff(137, 1500, quiet: false);
                    NetMessage.SendData(50, number: player.whoAmI);//buff
                }
            }
        }
    }
}
