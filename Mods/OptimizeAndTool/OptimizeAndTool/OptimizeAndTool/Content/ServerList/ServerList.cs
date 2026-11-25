using CommandHelp;
using OptimizeAndTool.Content.Patch;
using OptimizeAndTool.Content.UI.Menus;
using OptimizeAndTool.Utils;
using OptimizeAndTool.Utils.quickBuild;
using System;
using System.Collections.Generic;
using System.Reflection;
using tContentPatch;
using tContentPatch.Content;
using tContentPatch.Content.UI.Item1;
using Terraria;
using Terraria.ID;
using Terraria.UI;

namespace OptimizeAndTool.Content.ServerList
{
    internal class ServerList : PatchMain
    {
        private static readonly MethodInfo OnSubmitServerPort = typeof(Main).GetMethod("OnSubmitServerPort", BindingFlags.Static | BindingFlags.NonPublic);

        public static GetSetReset<bool> Enable = new GetSetReset<bool>(true, true);
        public static GetSetReset<int> ItemStyle = new GetSetReset<int>(0, 0);
        protected static ListData.IControl control = null;

        public override void Initialize()
        {
            if (Main.dedServ) return;

            Patch_Main.OnSelectPlayerPostfix += () =>
            {
                if (Main.menuMode != MenuID.ServerIP) return;
                if (Main.gameMenu != true) return;

                if (Enable.val == true) OpenMenu();
            };

            Patch_WorldGen.OnSaveAndQuitPrefix += () =>
            {
                if (Main.netMode != 1) return;

                if (Enable.val == true) UpdateAndSaveInfo();
            };
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
            List<UIElement> uis = new List<UIElement>();

            uis.Add(UIBuild.get2(Enable, text: "服务器列表"));

            UIItemTextButton btn = new UIItemTextButton(title: "将服务器添加到列表", buttonText: "添加");
            btn.OnClick += AddNowServer;
            uis.Add(btn);

            return uis;
        }

        public static void SetControl(ListData.IControl control)
        {
            if (Main.dedServ) return;

            ServerList.control = control;
        }

        //

        public static void OpenMenu()
        {
            UIMenuServerList ui = new UIMenuServerList(control, Main.MenuUI?.CurrentState);
            ui.SwitchItemStyle(ItemStyle.val);
            ui.OnSwitchItemStyle += v => ItemStyle.val = v;
            ui.OnGetEnable = () => Enable.val;
            ui.OnSetEnable = v => Enable.val = v;

            GameUitl.OpenMenu(ui);
        }

        public static void Join(ServerInfo si)
        {
            Main.getIP = si.IP;
            //Netplay.ListenPort = si.Port;

            //Main.autoPass = false;
            //Netplay.SetRemoteIPAsync(Main.getIP, Main.StartClientGameplay);
            //Main.menuMode = 14;
            //Main.statusText = Language.GetTextValue("Net.ConnectingTo", Main.getIP);

            OnSubmitServerPort.Invoke(null, new object[] { si.Port.ToString() });
        }

        public static void OpenMenuServerIP()
        {
            //Main.SelectPlayer
            Main.menuMode = MenuID.ServerIP;
            Main.clrInput();
        }

        public static void AddNowServer()
        {
            try
            {
                string ip = Netplay.ServerIPText;
                int port = Netplay.ListenPort;

                if (control.Data.Exists(i => i.IP == ip && i.Port == port)) throw new Exception($"已存在[{ip}:{port}]");

                Exception ex = GameUtils.GetNowServerInfo(out ServerInfo info);
                if (ex != null) throw ex;

                control.Add(info, 0);

                Main.NewText($"[c/00ff00:已添加:][{ip}:{port}]");
            }
            catch (Exception ex)
            {
                Main.NewText($"[c/ff0000:添加失败:]{ex.Message}");
            }
        }

        public static void UpdateAndSaveInfo()
        {
            try
            {
                Exception ex = GameUtils.GetNowServerInfo(out ServerInfo info);
                if (ex != null) return;

                int index = control.Data.FindIndex(i => i.IP == info.IP && i.Port == info.Port);

                info.DisplayName = control.Data[index].DisplayName;

                control.Replace(index, info);
                control.Save();
            }
            catch { }
        }
    }
}
