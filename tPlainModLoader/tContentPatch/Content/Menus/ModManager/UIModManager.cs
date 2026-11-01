using Microsoft.Xna.Framework;
using System.Collections.Generic;
using tContentPatch.Content.UI;
using tContentPatch.ModLoad;
using Terraria.UI;

namespace tContentPatch.Content.Menus.ModManager
{
    internal class UIModManager : UIMenuPanel
    {
        public static readonly Color color_isEnable = Color.Green;
        public static readonly Color color_isEnable_false = Color.Red;

        private UIScrollViewer2 sv = null;

        public UIModManager() : base("模组列表", titleYOff: 20f)
        {
            Width.Set(600f, 0f);
            Height.Set(-100, 1f);
            HAlign = 0.5f;
            VAlign = 0.5f;

            Panel.BackgroundColor = new Color(31, 41, 71, 200);

            sv = new UIScrollViewer2();
            sv.Width.Precent = 1;
            sv.Height.Set(-10, 1);
            sv.Top.Pixels = 10;
            sv.ItemMargin = 4;
            Panel.Append(sv);

            UIButton1[] btns = new UIButton1[]
            {
                new UIButton1("启用全部模组"),
                new UIButton1("禁用全部模组"),
                new UIButton1("重新加载模组"),
                new UIButton1("返回"),
                new UIButton1("打开模组文件夹"),
                new UIButton1("模组设置"),
            };

            btns[0].TextColor = color_isEnable;
            btns[0].OnLeftClick += (e, s) => ModManager.ModEnableAll();
            btns[1].TextColor = color_isEnable_false;
            btns[1].OnLeftClick += (e, s) => ModManager.ModNoEnableAll();
            btns[2].OnLeftClick += (e, s) => ModManager.AgainLoadMod();
            btns[2].OnUpdate += (e) => { if (btns[2].IsMouseHovering) DrawTip.SetDraw("建议重启程序"); };
            btns[3].OnLeftClick += (e, s) => Back(BackUI);
            btns[3].OnUpdate += (e) => { if (btns[3].IsMouseHovering && ModManager.CheckModUpdate()) DrawTip.SetDraw("需要重新加载, 建议重启程序"); };
            btns[4].OnLeftClick += (e, s) => ModManager.OpenModDirectory();
            btns[5].OnLeftClick += (e, s) => ModManager.OpenModSetSwitch();

            foreach (UIButton1 i in btns) AddBtn(i, 3);
        }

        public void InitializeModList(List<ModObject> mos)
        {
            sv.ClearChild();
            if (mos != null)
            {
                foreach (ModObject mo in mos) if (mo.config.isEnable) sv.AddChild(new UIModItem(this, mo, mos));
                foreach (ModObject mo in mos) if (!mo.config.isEnable) sv.AddChild(new UIModItem(this, mo, mos));
            }
        }

        public override void Back(UIState back = null)
        {
            if (ModManager.CheckModUpdate())
            {
                ModManager.AgainLoadMod();
                return;
            }

            base.Back(back);
        }
    }
}
