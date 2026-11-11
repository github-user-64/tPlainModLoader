using CommandHelp;
using Microsoft.Xna.Framework;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using tContentPatch.Content.UI;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.UI;

namespace SundryTool.Content.Function2
{
    internal class Function_particleSpawn : PatchPlayer
    {
        public static GetSetReset<bool> particleSpawn = new GetSetReset<bool>();
        public static GetSetReset<int> particleSpawn_set = new GetSetReset<int>();
        public static GetSetReset<int> particleSpawn_info = new GetSetReset<int>();
        public static GetSetReset<float> particleSpawn_speed = new GetSetReset<float>();
        public static GetSetReset<int> particleSpawn_cd = new GetSetReset<int>();

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get3("particleSpawn", particleSpawn,
                new CommandHRA<int>("type", particleSpawn_set, new CommandInt()),
                new CommandHRA<int>("info", particleSpawn_info, new CommandInt()),
                new CommandHRA<float>("speed", particleSpawn_speed, new CommandFloat()),
                new CommandHRA<int>("cd", particleSpawn_cd, new CommandInt())),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get1(particleSpawn, particleSpawn_set, UITextBox.StringToInt, "类型<int>有的可能有问题:39,56-59", text: "生成粒子"),
                UIBuild.get5(particleSpawn_info, UITextBox.StringToInt, "<int>", null, "生成粒子信息"),
                UIBuild.get5(particleSpawn_speed, UITextBox.StringToFloat, "<float>", null, "生成粒子速度"),
                UIBuild.get5(particleSpawn_cd, UITextBox.StringToInt, "<int>", null, "生成粒子cd"),
            };

            return uis;
        }

        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;
            if (particleSpawn.val == false) return;

            if (particleSpawn_set.val < 0) particleSpawn_set.val = 0;
            else if (particleSpawn_set.val > (int)ParticleOrchestraType.Count) particleSpawn_set.val = (int)ParticleOrchestraType.Count;
            if (particleSpawn_cd.val < 1) particleSpawn_cd.val = 1;

            if (Main.mouseLeft == false || This.mouseInterface) return;
            if (Main.GameUpdateCount % particleSpawn_cd.val != 0) return;

            Vector2 spawnPos = Main.MouseWorld;
            ParticleOrchestraType type = (ParticleOrchestraType)particleSpawn_set.val;

            Vector2 vector = Vector2.Normalize(Main.MouseWorld - This.Center) * particleSpawn_speed.val;

            ParticleOrchestrator.BroadcastOrRequestParticleSpawn(type, new ParticleOrchestraSettings
            {
                PositionInWorld = spawnPos,
                MovementVector = vector,
                UniqueInfoPiece = particleSpawn_info.val,
            });
        }
    }
}
