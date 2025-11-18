using CommandHelp;
using Microsoft.Xna.Framework;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;

namespace SundryTool.Content.Function2
{
    /// <summary>
    /// //NoPublic
    /// </summary>
    internal class Function_playToPlay : PatchPlayer
    {
        public static GetSetReset<bool> playToPlay = new GetSetReset<bool>();
        public static GetSetReset<int> playToPlay_play = new GetSetReset<int>(-1, -1);
        public static GetSetReset<float> playToPlay_x = new GetSetReset<float>(0, 0);
        public static GetSetReset<float> playToPlay_y = new GetSetReset<float>(-64, -64);

        public override void Initialize()
        {
            playToPlay_play.OnValUpdate += v => Utils.OutputPlayerName(playToPlay_play.val);
        }

        public override void UpdatePrefix(Player This, int playerI)
        {
            if (This != Main.LocalPlayer) return;

            if (playToPlay.val == false) return;

            if (playToPlay_play.val < 0) return;
            if (playToPlay_play.val >= Main.player.Length) return;

            Update_off(This);

            This.position = Main.player[playToPlay_play.val].position;
            This.position.X += playToPlay_x.val;
            This.position.Y += playToPlay_y.val;
            This.velocity = Vector2.Zero;
            This.fallStart = (int)(This.position.Y / 16f);//重置下落高度

            if (This.position.X < 0) This.position.X = 0;
            else if (This.position.X > Main.maxTilesX * 16) This.position.X = Main.maxTilesX * 16;
            if (This.position.Y < 0) This.position.Y = 0;
            else if (This.position.Y > Main.maxTilesY * 16) This.position.Y = Main.maxTilesY * 16;

            NetMessage.SendData(13, number: playerI);
        }

        private void Update_off(Player player)
        {
            if (Function1.Function_fly2.fly2.val == false) return;

            TriggersSet control = PlayerInput.Triggers?.Current;
            if (control == null)
            {
                control = new TriggersSet();
                control.CopyInto(player);
            }

            if (control.Up)
            {
                playToPlay_y.val -= Function1.Function_fly2.fly2_val.val;
            }
            if (control.Down)
            {
                playToPlay_y.val += Function1.Function_fly2.fly2_val.val;
            }
            if (control.Left)
            {
                playToPlay_x.val -= Function1.Function_fly2.fly2_val.val;
            }
            if (control.Right)
            {
                playToPlay_x.val += Function1.Function_fly2.fly2_val.val;
            }

            Function1.Function_fly2.fly2_resume = true;
        }

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get3("playToPlay", playToPlay,
                new CommandHRA<int>("play", playToPlay_play, new CommandInt()),
                new CommandHRA<float>("x", playToPlay_x, new CommandFloat()),
                new CommandHRA<float>("y", playToPlay_y, new CommandFloat())),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get1(playToPlay, playToPlay_play, int.Parse, "<int>", null, "传送到玩家"),
            };

            return uis;
        }
    }
}
