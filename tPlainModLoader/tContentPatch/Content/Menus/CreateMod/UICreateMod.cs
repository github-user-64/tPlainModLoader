using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tContentPatch.Content.UI;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace tContentPatch.Content.Menus.CreateMod
{
    internal partial class UICreateMod : UIMenuPanel
    {
        protected readonly UITextBoxItem UIDisplayName = null;
        protected readonly UITextBoxItem UIModName = null;
        protected readonly UITextBoxItem UIPath = null;
        protected readonly UIButton1 UIButton_Create = null;
        protected readonly UIText UITip = null;

        public UICreateMod(UIState back = null) : base("创建模组", back: back)
        {
            MinWidth.Set(700f, 0f);

            UIButton1 btn_back = new UIButton1("返回");
            btn_back.OnLeftClick += (e, s) => Back(BackUI);
            AddBtn(btn_back, 2);

            UIButton_Create = new UIButton1("创建");
            UIButton_Create.isEnable = false;
            UIButton_Create.OnLeftClick += (e, s) => TryCreate();
            AddBtn(UIButton_Create, 2);

            //

            UIStackPanel sp = new UIStackPanel();
            sp.Width.Set(0f, 1f);
            sp.IsAutoUpdateSize = true;
            sp.ItemMargin = 20f;
            UIPanel.Append(sp);

            UIDisplayName = new UITextBoxItem("模组显示名称（可不填）");
            UIDisplayName.UITextBox.MaxInputLength = 32;
            UIDisplayName.UITextBox.Style = null;
            UITextBox.Style2(UIDisplayName.UITextBox);
            UIDisplayName.UITextBox.UITextShadowColor = Color.DeepSkyBlue;
            sp.Append(UIDisplayName);

            UIModName = new UITextBoxItem("模组内部名称");
            UIModName.UITextBox.MaxInputLength = 32;
            UIModName.GetTip = GetTipCantNullOrEmpty;
            UIModName.UITip.TextColor = Color.Red;
            sp.Append(UIModName);

            UIElement uie = new UIElement();
            uie.Width.Set(0f, 1f);
            uie.OnUpdate += _ => uie.UpdateContainer_Height();
            sp.Append(uie);

            UIImageButton switchPath = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Camera_6"));
            switchPath.HAlign = 1f;
            switchPath.VAlign = 1f;
            switchPath.OnUpdate += _ =>
            {
                if (switchPath.IsMouseHovering) Main.instance.MouseText("左键选择, 右键打开当前路径");
            };
            switchPath.OnLeftClick += (e, s) => TrySwitchPath();
            switchPath.OnRightClick += (e, s) => TryOpenDirectory(UIPath.UITextBox.Text);
            uie.Append(switchPath);

            UIPath = new UITextBoxItem("创建路径");
            UIPath.Width.Set(-switchPath.Width.Pixels - 4f, 1f);
            UIPath.GetTip = GetTipCantNullOrEmpty;
            UIPath.UITip.TextColor = Color.Red;
            uie.Append(UIPath);

            UITip = new UIText(string.Empty);
            sp.Append(UITip);

            //

            UIStackPanel tips = new UIStackPanel();
            tips.Width.Set(0f, 1f);
            tips.VAlign = 1f;
            tips.ItemMargin = 8f;
            tips.IsAutoUpdateSize = true;
            UIPanel.Append(tips);

            tips.Append(new UIText("原版的ReLogic.dll文件无法直接引用，可以用tModLoader那的dll替代"));
            tips.Append(new UIText("在创建出的模组文件ConfigPublic.targets中修改ReLogic的引用位置"));

            UITextLink tip = new UITextLink();
            tip.MarginTop = 6f;
            tip.AddText("下载开发工具：");
            tip.AddText("Visual Studio Community", "https://visualstudio.microsoft.com/");
            tips.Append(tip);

            //

            SetPathDefault();
        }

        private string GetTipCantNullOrEmpty(string v)
        {
            return string.IsNullOrEmpty(v?.Trim()) ? "不可为空" : null;
        }

        protected void SetTip(string tip, Color color)
        {
            try
            {
                UITip.SetText(GetTextLine(tip, UIPanel.GetInnerDimensions().Width));
                UITip.TextColor = color;
            }
            catch
            {
                if (tip != null) ContentPatch.PrintTry(tip);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            try
            {
                if (UIPath.UITextBox.IsMouseHovering)
                {
                    DrawTip.SetDraw(GetTextLine(UIPath.UITextBox.Text, UIPanel.GetInnerDimensions().Width));
                }
            }
            catch { }

            try
            {
                bool enable = UIModName.GetTip?.Invoke(UIModName.UITextBox.Text) == null
                && UIPath.GetTip?.Invoke(UIPath.UITextBox.Text) == null;

                UIButton_Create.isEnable = enable;
            }
            catch { }
        }
    }
}
