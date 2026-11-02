using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using tContentPatch.Content.UI;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace tContentPatch.Content.Menus.CreateMod
{
    internal class UICreateMod : UIMenuPanel
    {
        private readonly object _lock = new object();
        private bool Createing = false;

        protected readonly UITextBoxItem UIDisplayName = null;
        protected readonly UITextBoxItem UIModName = null;
        protected readonly UITextBoxItem UIPath = null;
        protected readonly UIButton1 UIButton_Create = null;
        protected readonly UIText UITip = null;

        public UICreateMod(UIState back = null) : base("创建模组", back: back)
        {
            UIButton1 btn_back = new UIButton1("返回");
            btn_back.OnLeftClick += (e, s) => Back(BackUI);
            AddBtn(btn_back, 2);

            UIButton_Create = new UIButton1("创建");
            UIButton_Create.isEnable = false;
            UIButton_Create.OnLeftClick += (e, s) => Create();
            AddBtn(UIButton_Create, 2);

            UIStackPanel sp = new UIStackPanel();
            sp.Width.Set(0f, 1f);
            sp.IsAutoUpdateSize = true;
            sp.ItemMargin = 20f;
            UIPanel.Append(sp);

            UIDisplayName = new UITextBoxItem("模组显示名称");
            sp.Append(UIDisplayName);

            UIModName = new UITextBoxItem("模组内部名称");
            UIModName.GetTip = GetTipCantNullOrEmpty;
            UIModName.UITip.TextColor = Color.Red;
            sp.Append(UIModName);

            UIPath = new UITextBoxItem("创建路径");
            UIPath.GetTip = GetTipCantNullOrEmpty;
            UIPath.UITip.TextColor = Color.Red;
            sp.Append(UIPath);

            UITip = new UIText(string.Empty);
            sp.Append(UITip);

            //

            UIStackPanel tips = new UIStackPanel();
            tips.Width.Set(0f, 1f);
            tips.VAlign = 1f;
            tips.ItemMargin = 8f;
            tips.IsAutoUpdateSize = true;
            UIPanel.Append(tips);

            UIText tip2 = new UIText("下载开发工具：Visual Studio Community");//https://visualstudio.microsoft.com/
            tip2.TextColor = Color.DeepSkyBlue;
            tips.Append(tip2);

            UIText tip3 = new UIText("原版的ReLogic.dll文件无法直接引用, 可以用tModLoader那的dll替代");
            tips.Append(tip3);

            UIText tip4 = new UIText("在创建出的模组文件ConfigPublic.targets中修改ReLogic的引用位置");
            tips.Append(tip4);

            SetPathDefault();
        }

        protected virtual void SetPathDefault()
        {
            try
            {
                SetPath(typeof(ContentPatch).Assembly.Location);
            }
            catch { }
        }

        private string GetTipCantNullOrEmpty(string v)
        {
            return string.IsNullOrEmpty(v?.Trim()) ? "不可为空" : null;
        }

        protected void SetPath(string path)
        {
            UIPath.UITextBox.SetText(path);
        }

        protected void SetTip(string tip, Color color)
        {
            UITip.SetText(tip ?? string.Empty);
            UITip.TextColor = color;
        }

        protected void Create()
        {
            try
            {
                lock (_lock)
                {
                    if (Createing) return;
                    Createing = true;
                }

                string displayName = UIDisplayName.UITextBox.Text;
                string modName = UIModName.UITextBox.Text;
                string path = UIPath.UITextBox.Text;

                Content.CreateMod.Check(ref displayName, ref modName, ref path);
                SetTip(path, Color.Green);
            }
            catch (Exception ex)
            {
                SetTip(ex.Message, Color.Red);
            }
            finally
            {
                Createing = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Main.keyState.IsKeyDown(Keys.LeftControl) && Main.oldKeyState.IsKeyUp(Keys.LeftControl))
            {
                CreateMod.OpenMenu(BackUI);
                return;
            }

            base.Update(gameTime);

            bool enable = UIModName.GetTip?.Invoke(UIModName.UITextBox.Text) == null
                && UIPath.GetTip?.Invoke(UIPath.UITextBox.Text) == null;

            UIButton_Create.isEnable = enable;
        }
    }
}
