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
    /// 屏幕偏移
    /// </summary>
    internal class Function_screenOff : PatchMain
    {
        public static GetSetReset<bool> Enable = new GetSetReset<bool>();
        public static GetSetReset<float> MoveSpeed = new GetSetReset<float>(16, 16);
        private static Vector2 off = new Vector2(float.NaN, float.NaN);

        private bool CheckEnable()
        {
            if (Enable.val == false) return false;
            if (Main.mouseRight == false) return true;

            Enable.val = false;
            return Enable.val;
        }

        public override void OnEnterWorld()
        {
            Enable.val = false;
        }

        public override void DoUpdateInWorldPostfix()
        {
            if (CheckEnable() == false)
            {
                off = Main.screenPosition;
                return;
            }
            if (off.HasNaNs()) off = Main.screenPosition;

            Main.LocalPlayer.isOperatingAnotherEntity = true;//防止移动

            //

            TriggersSet control = PlayerInput.Triggers?.Current;
            if (control == null)
            {
                control = new TriggersSet();
                control.CopyInto(Main.LocalPlayer);
            }

            Vector2 p = off;
            if (control.Up) p.Y -= MoveSpeed.val;
            if (control.Down) p.Y += MoveSpeed.val;
            if (control.Left) p.X -= MoveSpeed.val;
            if (control.Right) p.X += MoveSpeed.val;
            if (p.HasNaNs() == false) off = p;
        }

        public override Vector2 PlayerFocusedScreenPosition(Vector2 origin, Vector2 modifi)
        {
            if (CheckEnable() == false) return modifi;
            if (off.HasNaNs() == true) return modifi;
            return off;
        }

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get3("screenOff", Enable,
                new CommandHRA<float>("speed", MoveSpeed, new CommandFloat())),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get1(Enable, MoveSpeed, float.Parse, "速度<flot>//鼠标右键退出", null, "灵魂出窍"),
            };

            return uis;
        }
    }
}
