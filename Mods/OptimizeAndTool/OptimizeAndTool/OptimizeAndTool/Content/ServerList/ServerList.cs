using CommandHelp;
using OptimizeAndTool.Content.UI.Menus;
using OptimizeAndTool.Utils;
using OptimizeAndTool.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using tContentPatch.Content;
using Terraria;
using Terraria.UI;

namespace OptimizeAndTool.Content.ServerList
{
    internal class ServerList : PatchMain
    {
        public static GetSetReset<bool> Enable = new GetSetReset<bool>(true, true);
        public static GetSetReset<int> ItemStyle = new GetSetReset<int>(0, 0);
        protected static ListData.IControl control = null;

        public static void OpenMenu()
        {
            UIMenuServerList ui = new UIMenuServerList(control, Main.MenuUI?.CurrentState);
            ui.SwitchItemStyle(ItemStyle.val);
            ui.OnSwitchItemStyle += v => ItemStyle.val = v;

            ui.NoGamepadSupport = true;//不支持游戏手柄, 取消按移动键鼠标会定位到ui上的功能

            GameUitl.OpenMenu(ui);
        }

        public static List<CommandObject> GetCO()
        {
            List<CommandObject> cos = new List<CommandObject>
            {
                CommandBuild.get2("serverList", Enable),
            };

            return cos;
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                UIBuild.get2(Enable, text: "服务器列表"),
            };

            return uis;
        }

        public static void SetControl(ListData.IControl control)
        {
            if (Main.dedServ) return;

            ServerList.control = control;
        }

        public static void Join(ServerInfo si)
        {
            //Main.getIP = si.ip;
            //Netplay.ListenPort = si.port;
            //Main.autoPass = false;
            //Netplay.SetRemoteIPAsync(Main.getIP, new Action(Main.StartClientGameplay));
            //Main.menuMode = 14;
            //Main.statusText = Language.GetTextValue("Net.ConnectingTo", Main.getIP);
        }
    }
}
