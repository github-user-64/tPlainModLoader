using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using tContentPatch.Content.UI;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace OptimizeAndTool.Content.ServerList
{
    public class UIMenuServerList : UIMenuPanel
    {
        public readonly ListData.IControl Control = null;
        public List<ServerInfo> Data => Control?.Data;

        public readonly UIScrollViewer2 UISV = null;

        public UIMenuServerList(ListData.IControl control = null, UIState back = null) : base("选择服务器", back: back)
        {
            UIPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;

            UIButton1 btn1 = new UIButton1("返回");
            btn1.OnLeftClick += (e, s) => Back(BackUI);
            AddBtn(btn1, 2);

            UIButton1 btn2 = new UIButton1("添加服务器");
            btn2.MouseText = "在当前选中服务器下方插入";
            //btn2.OnLeftClick += (e, s) => Back(BackUI);
            AddBtn(btn2, 2);

            //

            InitializeButtons();

            UISV = new UIScrollViewer2();
            UISV.Width.Set(0f, 1f);
            UISV.Height.Set(0f, 1f);
            UIPanel.Append(UISV);

            //

            Control = control;

            UpdateUIData();
        }

        protected virtual void InitializeButtons()
        {
            UIStackPanel btns = new UIStackPanel();
            btns.Width.Set(30f, 0f);
            btns.Left.Set(-btns.Width.Pixels, 0f);
            btns.MarginLeft = -6f;
            btns.MarginTop = 10f;
            btns.IsAutoUpdateSize = true;
            btns.ItemMargin = 10f;
            Append(btns);

            UIImage save = GetImageButton(btns.Width.Pixels, "Images/UI/IconQuickload", "保存");
            save.OnLeftClick += (e, s) => SaveData();
            btns.Append(save);

            UIImage load = GetImageButton(btns.Width.Pixels, "Images/UI/Sort_0", "加载");
            load.OnLeftClick += (e, s) => LoadData();
            btns.Append(load);
        }

        private UIImage GetImageButton(float size, string img, string mouseText)
        {
            UIImage ui = new UIImage(Main.Assets.Request<Texture2D>(img));
            ui.Width.Set(size, 0f);
            ui.Height.Set(size, 0f);
            ui.Color = Color.White * 0.6f;
            ui.ScaleToFit = true;
            ui.OnUpdate += _ =>
            {
                if (ui.IsMouseHovering) Main.instance.MouseText(mouseText);
            };
            ui.OnMouseOver += (e, s) =>
            {
                ui.Color = Color.White;
                SoundEngine.PlaySound(SoundID.MenuTick);
            };
            ui.OnMouseOut += (e, s) => ui.Color = Color.White * 0.6f;
            ui.OnLeftClick += (e, s) => SoundEngine.PlaySound(SoundID.MenuTick);

            return ui;
        }

        public virtual void UpdateUIData()
        {
            UISV.ClearChild();

            List<ServerInfo> data = Data;
            if (data == null) return;


        }

        public virtual void LoadData()
        {
            if (Control == null) return;

            Control.Load();

            UpdateUIData();
        }

        public virtual void SaveData() => Control?.Save();
    }
}
