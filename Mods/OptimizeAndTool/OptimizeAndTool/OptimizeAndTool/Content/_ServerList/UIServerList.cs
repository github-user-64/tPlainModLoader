using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using tContentPatch.Content.UI;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace OptimizeAndTool.Content.ServerList
{
    internal class UIServerList : UIPanel
    {
        private UIScrollViewer2 ui_sv = null;

        private List<UIServerItem> drag_uis = null;
        private UIServerItem drag_ui = null;
        private UIServerItem drag_ui2 = null;

        public UIServerList()
        {
            Width.Pixels = 300;
            Height.Set(-200, 1);
            Left.Pixels = 100;
            VAlign = 0.5f;
            BackgroundColor *= 0.5f;

            drag_uis = new List<UIServerItem>();

            UIStackPanel ui_sp = new UIStackPanel();
            ui_sp.Width.Precent = 1;
            ui_sp.Height.Pixels = 20;
            ui_sp.Horizontal = true;
            ui_sp.IsAutoUpdateSize = true;
            ui_sp.ItemMargin = 10;

            UIButton ui_save = new UIButton(ui_sp.Height.Pixels, "保存",
                Main.Assets.Request<Texture2D>("Images/UI/Cursor_7", ReLogic.Content.AssetRequestMode.ImmediateLoad));
            ui_save.OnLeftClick += (e, s) => ServerList.Save();

            UIButton ui_add = new UIButton(ui_sp.Height.Pixels, "添加, 双击项可以插入, 右键项移动位置",
                Main.Assets.Request<Texture2D>("Images/UI/Cursor_9", ReLogic.Content.AssetRequestMode.ImmediateLoad));
            ui_add.OnLeftClick += (e, s) => ServerList.AddItem();

            ui_sv = new UIScrollViewer2(true);
            ui_sv.Width.Precent = 1;
            ui_sv.Height.Set(-(ui_sp.Height.Pixels + 8), 1);
            ui_sv.VAlign = 1;
            ui_sv.ItemMargin = 4;

            Append(ui_sp);
            Append(ui_sv);
            ui_sp.Append(ui_save);
            ui_sp.Append(ui_add);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsMouseHovering)
            {
                BackgroundColor = new Color(63, 82, 151) * 0.5f;
                BorderColor = Color.Black * 0.8f;
            }
            else
            {
                BackgroundColor = new Color(63, 82, 151) * 0.2f;
                BorderColor = Color.Black * 0.5f;
            }

            Update_Drag();
        }

        public void Initialize(List<ServerInfo> data)
        {
            ui_sv.ClearChild();
            drag_uis.Clear();

            drag_ui = null;

            if (ServerList.Enable.val == false) return;

            for (int i = 0; i < data?.Count; ++i)
            {
                UIServerItem ui = new UIServerItem(data[i]);
                ui.OnRightMouseDown += (e, s) => Drag(ui);

                ui_sv.AddChild(ui);
                drag_uis.Add(ui);
            }
        }

        private void Drag(UIServerItem ui)
        {
            drag_ui = ui;
            drag_ui2 = null;
        }

        private void Update_Drag()
        {
            if (drag_ui == null) return;

            drag_ui2 = null;

            foreach (UIServerItem ui in drag_uis)
            {
                if (ui == drag_ui) continue;

                Terraria.UI.CalculatedStyle dim = ui.GetDimensions();

                if (Main.mouseY < dim.Y) continue;
                if (Main.mouseY > dim.Y + dim.Height) continue;

                drag_ui2 = ui;
                break;
            }

            if (drag_ui2 != null) drag_ui2.isLight = true;

            if (Main.mouseRight == false)
            {
                if (drag_ui == null || drag_ui2 == null)
                {
                    drag_ui = null;
                    return;
                }

                ServerList.SwitchItem(drag_ui.serverInfo, drag_ui2.serverInfo);
                drag_ui = null;
            }
        }
    }
}
