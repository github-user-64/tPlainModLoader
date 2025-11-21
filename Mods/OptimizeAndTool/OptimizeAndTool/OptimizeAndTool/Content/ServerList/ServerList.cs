using CommandHelp;
using Microsoft.Xna.Framework;
using OptimizeAndTool.Utils;
using OptimizeAndTool.Utils.quickBuild;
using System;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.Localization;
using Terraria.UI;

namespace OptimizeAndTool.Content.ServerList
{
    internal class ServerList : PatchMain
    {
        public static GetSetReset<bool> Enable = new GetSetReset<bool>(true, true);
        public static Action OnSave = null;
        private static UIServerList uistate = null;
        private static List<ServerInfo> data = null;

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

        public static void Initialize(List<ServerInfo> data)
        {
            if (uistate == null)
            {
                uistate = new UIServerList();
            }

            ServerList.data = data;
            uistate.Initialize(ServerList.data);
        }

        public override void UpdatePrefix(GameTime gameTime)
        {
            if (uistate == null) return;

            if (Main.menuMode == 13 && Enable.val)
            {
                if (uistate.Parent == null) ModifyInterfaceLayers.ui_menu_state.Append(uistate);
            }
            else
            {
                uistate.Parent?.RemoveChild(uistate);
            }
        }

        public static void AddItem(ServerInfo addTo = null)
        {
            if (Enable.val == false) return;
            if (data == null) return;

            if (addTo == null)
            {
                data.Add(new ServerInfo());
            }
            else
            {
                int index = data.IndexOf(addTo);
                if (index == -1) return;
                data.Insert(index, new ServerInfo());
            }
            
            uistate.Initialize(data);
        }

        public static void DelItem(ServerInfo si)
        {
            if (Enable.val == false) return;
            if (data == null) return;

            data.Remove(si);
            uistate.Initialize(data);
        }

        public static void Save()
        {
            if (Enable.val == false) return;
            if (data == null) return;

            OnSave?.Invoke();
        }

        public static void Join(ServerInfo si)
        {
            Main.getIP = si.ip;
            Netplay.ListenPort = si.port;
            Main.autoPass = false;
            Netplay.SetRemoteIPAsync(Main.getIP, new Action(Main.StartClientGameplay));
            Main.menuMode = 14;
            Main.statusText = Language.GetTextValue("Net.ConnectingTo", Main.getIP);
        }

        public static void SwitchItem(ServerInfo si1, ServerInfo si2)
        {
            if (Enable.val == false) return;
            if (data == null) return;
            if (si1 == si2) return;

            int index1 = data.IndexOf(si1);
            int index2 = data.IndexOf(si2);
            if (index1 == -1 || index2 == -1) return;
            if(index1 == index2) return;

            data[index1] = si2;
            data[index2] = si1;
            uistate.Initialize(data);
        }
    }
}
