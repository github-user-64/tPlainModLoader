using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using tContentPatch.Content.UI;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace tContentPatch.Content.Menus.CreateMod
{
    internal class UICreateMod : UIMenuPanel
    {
        protected readonly object _lock = new object();
        private bool Createing = false;

        public UICreateMod(UIState back = null) : base("创建模组", back: back)
        {
            UIButton1 btn_back = new UIButton1("返回");
            btn_back.OnLeftClick += (e, s) => Back(BackUI);
            AddBtn(btn_back, 2);

            UIButton1 btn_create = new UIButton1("创建");
            btn_create.OnLeftClick += (e, s) => Create();
            AddBtn(btn_create, 2);

            UIStackPanel sp = new UIStackPanel();
            sp.Width.Set(0f, 1f);
            sp.IsAutoUpdateSize = true;
            sp.ItemMargin = 20f;
            UIPanel.Append(sp);

            sp.Append(new UI1());

            sp.Append(new UI1());

            sp.Append(new UI1());
        }

        protected void Create()
        {
            lock (_lock)
            {
                if (Createing) return;
                Createing = true;
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
        }

        private class UI1 : UIStackPanel
        {
            public UI1()
            {
                Width.Set(0f, 1f);
                Height.Set(0f, 0f);
                IsAutoUpdateSize = true;

                UIStackPanel sp_txt = new UIStackPanel();
                sp_txt.Width.Set(0f, 1f);
                sp_txt.Height.Set(0f, 0f);
                sp_txt.IsAutoUpdateSize = true;
                sp_txt.Horizontal = true;
                sp_txt.ItemMargin = 4f;
                sp_txt.OnUpdate += UI.Utils.UpdateContainer_Height;
                Append(sp_txt);

                //

                UIText ut1 = new UIText("模组显示名称");
                sp_txt.Append(ut1);

                UIText ut2 = new UIText("不可为空");
                ut2.TextColor = Color.Red;
                sp_txt.Append(ut2);

                //

                UITextBox textbox = new UITextBox("asd");
                textbox.MarginTop = 2f;
                UITextBox.Style2(textbox);
                Append(textbox);
            }
        }
    }
}
