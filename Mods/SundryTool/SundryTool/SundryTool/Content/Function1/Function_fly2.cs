using CommandHelp;
using Microsoft.Xna.Framework;
using SundryTool.Utils;
using SundryTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;

namespace SundryTool.Content.Function1
{
    internal class Function_fly2 : PatchPlayer
    {
        public static GetSetReset<bool> fly2 = new GetSetReset<bool>();
        public static GetSetReset<float> fly2_val = new GetSetReset<float>(20, 20);
        public static GetSetReset<bool> fly2_inertia = new GetSetReset<bool>(true, true);

        public static bool fly2_resume = false;
        private static Vector2 fly2_p = Vector2.Zero;
        private static Vector2 fly2_v = Vector2.Zero;
        public override void UpdatePrefix(Player This, int ThisI)
        {
            if (This != Main.LocalPlayer) return;
            if (fly2.val == false)
            {
                fly2_false(This);
                return;
            }

            if (fly2_resume)
            {
                fly2_p = This.position;
                fly2_v = Vector2.Zero;

                fly2_resume = false;
            }

            TriggersSet control = PlayerInput.Triggers?.Current;
            if (control == null)
            {
                control = new TriggersSet();
                control.CopyInto(This);
            }

            if (control.Up)
            {
                fly2_v.Y = -fly2_val.val;
            }
            if (control.Down)
            {
                fly2_v.Y = fly2_val.val;
            }
            if (control.Left)
            {
                fly2_v.X = -fly2_val.val;
            }
            if (control.Right)
            {
                fly2_v.X = fly2_val.val;
            }

            fly2_p += fly2_v;

            if (fly2_inertia.val)
            {
                if (fly2_v.Length() < 1)
                {
                    fly2_v = Vector2.Zero;
                }
                else
                {
                    fly2_v -= fly2_v / 10;
                }
            }
            else
            {
                fly2_v = Vector2.Zero;
            }

            This.position = fly2_p;
            This.velocity = Vector2.Zero;
            This.fallStart = (int)(This.position.Y / 16f);//重置下落高度

            NetMessage.SendData(13, number: This.whoAmI);
        }

        public static void fly2_false(Player This)
        {
            fly2_resume = true;
        }

        private class Function_fly2_ModMain : PatchMain
        {
            public override void OnEnterWorld()
            {
                fly2_resume = true;
            }
        }

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get3("fly2", fly2,
                new CommandHRA<float>("set", fly2_val, new CommandFloat()),
                CommandBuild.get2("inertia", fly2_inertia)),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get1(fly2, fly2_val, float.Parse, "<float>", "Images/Buff_353", "飞行2"),
            };

            return uis;
        }
    }
}
