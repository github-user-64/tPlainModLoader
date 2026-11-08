using System;
using System.Collections.Generic;
using tContentPatch.Content.UI;
using tContentPatch.ModLoad;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;

namespace tContentPatch.Content.Menus.ModInfo
{
    internal class UIModInfo : UIMenuPanel
    {
        public Action<ModObject> ActionDelMod = null;
        private ModObject mo = null;
        private UIScrollViewer2 ui_sv = null;
        private UIButton1 btn3 = null;
        private UIButton1 btn4 = null;

        public UIModInfo() : base(string.Empty)
        {
            ui_sv = new UIScrollViewer2();
            ui_sv.Width.Precent = 1;
            ui_sv.Height.Precent = 1;
            ui_sv.ItemMargin = 4;
            UIPanel.Append(ui_sv);

            UIButton1 btn1 = new UIButton1("返回");
            btn1.OnLeftClick += (e, s) => Back(BackUI);

            UIButton1 btn2 = new UIButton1("打开文件夹");
            btn2.OnLeftClick += (e, s) => ModInfo.OpenModDirectory(mo);

            btn3 = new UIButton1("删除");
            btn3.OnLeftClick += (e, s) =>
            {
                ActionDelMod?.Invoke(mo);
                Back(BackUI);
            };

            btn4 = new UIButton1("跳转到相关链接");
            btn4.OnLeftClick += (e, s) => ModInfo.JumpTo(mo.info);

            AddBtn(btn1, 3);
            AddBtn(btn2, 3);
            AddBtn(btn3, 3);
            AddBtn(btn4, 3);
        }

        public void InitializeMod(ModObject mo = null, Action<ModObject> ActionDelMod = null)
        {
            this.mo = mo;

            this.ActionDelMod = null;
            btn3.isEnable = false;
            btn4.isEnable = false;
            ui_sv.ClearChild();

            if (this.mo == null)
            {
                UITitle.SetText("模组信息");
                return;
            }

            string path = this.mo.info?.jumpPath;

            UITitle.SetText($"模组信息: {this.mo.info?.name ?? this.mo.config.key}");
            btn3.isEnable = ModInfo.GetModIsLoaded(this.mo) == false;
            btn3.MouseText = btn3.isEnable ? null : "卸载模组以删除";
            btn4.isEnable = path != null;
            btn4.MouseText = btn4.isEnable ? $"不要相信可疑链接.跳转到:{path}" : null;

            InitializeInfoText();

            this.ActionDelMod = ActionDelMod;
        }

        private void InitializeInfoText()
        {
            if (mo.info?.description == null) return;

            string text = mo.info.description;

            List<string> texts = new List<string>();

            while (text.Length > 0)
            {
                int start = text.IndexOf('\n');
                int textLen = start == -1 ? text.Length : start;
                int rLen = start == -1 ? text.Length : start + 1;

                string t = text.Substring(0, textLen);

                text = text.Remove(0, rLen);

                texts.Add(t);
            }

            int widthMax = (int)ui_sv.GetInnerDimensions().Width - 40;

            foreach (string s in texts)
            {
                string[] ss = Terraria.Utils.WordwrapString(s, FontAssets.MouseText.Value,
                    widthMax, 16, out int lineAmount);

                for (int i = 0; i < lineAmount + 1; ++i)
                {
                    if (ss[i] == null) continue;
                    ui_sv.AddChild(new UIText(ss[i]));
                }
            }
        }
    }
}
