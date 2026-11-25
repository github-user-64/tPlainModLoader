using OptimizeAndTool.Content.ServerList;
using System;
using tContentPatch.Content.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace OptimizeAndTool.Content.UI.Menus
{
    public class UIEditServerInfo : UIMenuPanel
    {
        public readonly ServerInfo Info = null;

        public readonly UIScrollViewer2 UIStack = null;

        public UIEditServerInfo(ServerInfo info, UIState back = null) : base(title: "编辑服务器信息", back: back)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));

            //

            UIButton1 btn1 = new UIButton1("返回");
            btn1.OnLeftClick += (e, s) => Back(BackUI);
            AddBtn(btn1, 1);
            btn1.Width.Set(0f, 0.5f);
            btn1.HAlign = 0.5f;

            UIStack = new UIScrollViewer2();
            UIStack.Width.Set(0f, 1f);
            UIStack.Height.Set(0f, 1f);
            UIPanel.Append(UIStack);

            AddTitle("显示名称");

            UITextBox displayName = new UITextBox();
            displayName.MarginBottom = 20f;
            displayName.MaxInputLength = 50;
            displayName.Style = UITextBox.Style2;
            displayName.OnGetText = () => Info.DisplayName;
            displayName.OnSetText = v => Info.DisplayName = v;
            UIStack.AddChild(displayName);

            AddTitle("IP");

            UITextBox ip = new UITextBox();
            ip.MarginBottom = 20f;
            ip.MaxInputLength = 50;
            ip.Style = UITextBox.Style2;
            ip.OnGetText = () => Info.IP;
            ip.OnSetText = v => Info.IP = v;
            UIStack.AddChild(ip);

            AddTitle("端口");

            UITextBoxInt port = new UITextBoxInt();
            port.MarginBottom = 20f;
            port.UITextsBox.MaxInputLength = 10;
            port.UITextsBox.Style = UITextBox.Style2;
            port.OnGetValue = () => Info.Port;
            port.OnSetValue = v => Info.Port = v;
            UIStack.AddChild(port);
        }

        protected void AddTitle(string text)
        {
            UIText t1 = new UIText(text);
            t1.MarginBottom = 2f;

            UIStack.AddChild(t1);
        }
    }
}
