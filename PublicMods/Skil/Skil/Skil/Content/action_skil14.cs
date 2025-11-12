using CommandHelp;
using Microsoft.Xna.Framework;
using Skil.Utils;
using Skil.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.UI;

namespace Skil.Content
{
    public class skil14 : ModPlayer
    {
        //技能14, 传送
        public static GetSetReset<bool> Enable = new GetSetReset<bool>();
        public static GetSetReset<int> Len = new GetSetReset<int>(10, 10);

        public static List<CommandObject> GetCO()
        {
            return new List<CommandObject>()
            {
                CommandBuild.get1("skil14", Enable, Len, new CommandInt()),
            };
        }

        public static List<UIElement> GetUI()
        {
            return new List<UIElement>()
            {
                UIBuild.get1(Enable, Len, int.Parse, "长度<int>", "Images/Buff_353", "技能14")
            };
        }

        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;

            if (Enable.val) a1_particleSkil1(This);
            else a1_particleSkil1_false(This);
        }

        private static int particleSkil1_lenCount = 0;
        private static bool particleSkil1_up = true;
        public static void a1_particleSkil1(Player player)
        {
            if (player == null) return;

            bool one = false;

            if (player.mouseInterface == false)
            {
                if (Main.mouseLeft && Main.mouseLeftRelease)
                {
                    particleSkil1_lenCount = Len.val;
                    particleSkil1_up = true;
                    one = true;
                }
                else
                if (Main.mouseRight && Main.mouseRightRelease)
                {
                    particleSkil1_lenCount = Len.val;
                    particleSkil1_up = false;
                    one = true;
                }
            }

            if (particleSkil1_lenCount < 1) return;
            --particleSkil1_lenCount;

            if (one) player.AddBuff(10, Len.val);

            Vector2 spawnPos = player.Center;
            spawnPos.Y += particleSkil1_up ? -40 : 40;

            int mapWidth = Main.maxTilesX * 16;
            int mapHeight = Main.maxTilesY * 16;
            float plyWidthHalf = player.width / 2;
            float plyHeightHalf = player.height / 2;

            if (spawnPos.X < 0) spawnPos.X = 0;
            else if (spawnPos.X + plyWidthHalf > mapWidth) spawnPos.X = mapWidth - plyWidthHalf;
            if (spawnPos.Y < 0) spawnPos.Y = 0;
            else if (spawnPos.Y + plyHeightHalf > mapHeight) spawnPos.Y = mapHeight - plyHeightHalf;

            player.Center = spawnPos;

            NetMessage.SendData(13, number: player.whoAmI);
            ParticleOrchestrator.BroadcastOrRequestParticleSpawn((ParticleOrchestraType)(one ? 31 : 32), new ParticleOrchestraSettings
            {
                PositionInWorld = spawnPos,
            });
        }
        public static void a1_particleSkil1_false(Player player)
        {
            if (player == null) return;

            if (particleSkil1_lenCount > 0) player.ClearBuff(10);
            particleSkil1_lenCount = 0;
        }
    }
}
