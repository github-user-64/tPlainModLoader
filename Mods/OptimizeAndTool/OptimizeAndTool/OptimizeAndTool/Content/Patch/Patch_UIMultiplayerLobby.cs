using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Reflection;
using tContentPatch;
using tContentPatch.ModLoad;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.UI;

namespace OptimizeAndTool.Content.Patch
{
    [HarmonyPatch(typeof(UIMultiplayerLobby))]
    internal class Patch_UIMultiplayerLobby : Mod
    {
        private static UIMultiplayerLobby _multiplayerLobby = null;

        private static UIElement MyUI = null;

        public override void Load(ModObject mo) => Init();

        public override void Unload() => MyUI?.Remove();

        [HarmonyPatch(nameof(UIMultiplayerLobby.OnInitialize))]
        [HarmonyPostfix]
        private static void OnInitialize(UIMultiplayerLobby __instance) => Init();

        private static void Init()
        {
            if (Main.dedServ) return;
            if (MyUI != null) return;
            if (ServerList.ServerList.Enable.val != true) return;

            if (_multiplayerLobby == null)//Main._multiplayerLobby只会创建一次
            {
                FieldInfo __multiplayerLobby = typeof(Main).GetField("_multiplayerLobby", BindingFlags.Static | BindingFlags.NonPublic);
                if (__multiplayerLobby == null) return;

                _multiplayerLobby = __multiplayerLobby.GetValue(null) as UIMultiplayerLobby;
                if (_multiplayerLobby == null) return;
            }

            UIElement panel = null;

            foreach (UIElement i in _multiplayerLobby.Children)
            {
                panel = i.Children.FirstOrDefault(v => v.GetType() == typeof(UIPanel));
                if (panel != null) break;
            }

            if (panel == null) return;

            UI.UIImageButton btn = new UI.UIImageButton(30f, Main.Assets.Request<Texture2D>("Images/UI/Banner_Toggle_2"));
            btn.MarginLeft = -(6f + btn.Width.Pixels);
            btn.MarginTop = 10f;
            btn.MouseText = "打开服务器列表(优化和工具)";
            btn.OnUpdate += _ =>
            {
                CalculatedStyle pos = panel.GetOuterDimensions();
                btn.Left.Pixels = pos.X;
                btn.Top.Pixels = pos.Y;
            };
            btn.OnLeftClick += (e, s) =>
            {
                SoundEngine.PlaySound(SoundID.MenuOpen);

                //切到别的菜单后会有异常, 但游戏本身点击返回按钮就有异常
                //ThreadAbortException, 应该是搜索服务器的线程退出用的
                ServerList.ServerList.OpenMenu();
            };
            MyUI = btn;

            _multiplayerLobby.Append(MyUI);
        }
    }
}
