using CommandHelp;
using Microsoft.Xna.Framework;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.UI;

namespace SundryTool.Content.Function2
{
    internal class Function_particleSpawn : ModPlayer
    {
        public static GetSetReset<bool> particleSpawn = new GetSetReset<bool>();
        public static GetSetReset<int> particleSpawn_set = new GetSetReset<int>();
        public static GetSetReset<int> particleSpawn_cd = new GetSetReset<int>();

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get3("particleSpawn", particleSpawn,
                new CommandHRA<int>("type", particleSpawn_set, new CommandInt()),
                new CommandHRA<int>("cd", particleSpawn_cd, new CommandInt())),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get1(particleSpawn, particleSpawn_set, int.Parse, "类型<int>", text: "生成粒子"),
                new UI.UIItemTextBoxBind<int>(particleSpawn_cd, int.Parse, null, "生成粒子cd"){ MouseText = "<int>" },
            };

            return uis;
        }

        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;
            if (particleSpawn.val == false) return;

            if (particleSpawn_set.val < 0) particleSpawn_set.val = 0;
            else if (particleSpawn_set.val > 32) particleSpawn_set.val = 32;
            if (particleSpawn_cd.val < 1) particleSpawn_cd.val = 1;

            if (Main.mouseLeft == false || This.mouseInterface) return;
            if (Main.GameUpdateCount % particleSpawn_cd.val != 0) return;

            Vector2 spawnPos = Main.MouseWorld;
            ParticleOrchestraType type = (ParticleOrchestraType)particleSpawn_set.val;

            ParticleOrchestrator.BroadcastOrRequestParticleSpawn(type, new ParticleOrchestraSettings
            {
                PositionInWorld = spawnPos,
            });
        }
    }
}
