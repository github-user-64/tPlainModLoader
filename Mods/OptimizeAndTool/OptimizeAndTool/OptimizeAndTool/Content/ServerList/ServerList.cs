using CommandHelp;
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
        protected static ListData.IControl control = null;

        public static void OpenMenu()
        {
            GameUitl.OpenMenu(new UIMenuServerList(control, Main.MenuUI?.CurrentState));
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
