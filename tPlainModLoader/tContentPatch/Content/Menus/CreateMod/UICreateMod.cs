using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using tContentPatch.Content.UI;
using Terraria;
using Terraria.UI;

namespace tContentPatch.Content.Menus.CreateMod
{
    internal class UICreateMod : UIMenuPanel
    {
        public UICreateMod(UIState back = null) : base("创建模组", back: back)
        {
            UIButton1 btn_back = new UIButton1("返回");
            btn_back.OnLeftClick += (e, s) => Back(BackUI);
            AddBtn(btn_back, 2);

            UIButton1 btn_create = new UIButton1("创建");
            btn_create.OnLeftClick += (e, s) => Create();
            AddBtn(btn_create, 2);
        }

        protected void Create()
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (Main.keyState.IsKeyDown(Keys.Space) && Main.oldKeyState.IsKeyUp(Keys.Space))
            {
                CreateMod.OpenMenu(BackUI);
                return;
            }

            base.Update(gameTime);
        }
    }
}
