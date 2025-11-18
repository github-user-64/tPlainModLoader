using CommandHelp;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;

namespace SundryTool.Content.Function1
{
    internal class Function_fly : PatchPlayer
    {
        public static GetSetReset<bool> fly = new GetSetReset<bool>();
        public static GetSetReset<float> fly_val = new GetSetReset<float>(16, 16);

        public override void UpdatePrefix(Player This, int ThisI)
        {
            if (This != Main.LocalPlayer) return;
            if (fly.val == false) return;

            TriggersSet control = PlayerInput.Triggers?.Current;
            if (control == null)
            {
                control = new TriggersSet();
                control.CopyInto(This);
            }

            if (control.Up && This.velocity.Y > -fly_val.val)
            {
                This.velocity.Y -= 1f;
            }
            if (control.Down && This.velocity.Y < fly_val.val && This.position.Y != This.oldPosition.Y)
            {
                This.velocity.Y += 1f;
            }
            if (control.Left && This.velocity.X > -fly_val.val)
            {
                This.velocity.X -= 1f;
            }
            if (control.Right && This.velocity.X < fly_val.val)
            {
                This.velocity.X += 1f;
            }
            if (control.Jump && This.velocity.Y > -fly_val.val)
            {
                This.velocity.Y -= 1f;
            }
            if (control.Down == false && This.velocity.Y > 0f)
            {
                This.velocity.Y -= 1f;
                if (This.velocity.Y < 0f)
                {
                    This.velocity.Y = 0f;
                }
            }
        }

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get1("fly", fly, fly_val, new CommandFloat()),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get1(fly, fly_val, float.Parse, "<float>", "Images/Buff_8", "飞行"),
            };

            return uis;
        }
    }
}
