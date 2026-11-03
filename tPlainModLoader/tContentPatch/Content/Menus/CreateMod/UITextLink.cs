using Microsoft.Xna.Framework;
using ReLogic.OS;
using System;
using tContentPatch.Content.UI;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;

namespace tContentPatch.Content.Menus.CreateMod
{
    internal class UITextLink : UIStackPanel
    {
        public UITextLink()
        {
            Width.Set(0f, 1f);
            Horizontal = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.UpdateContainer_Height();
        }

        public void AddText(string text, string link = null)
        {
            UIText ui = new UIText(text);
            Append(ui);

            if (link == null) return;

            ui.OnLeftClick += (e, s) =>
            {
                SoundEngine.PlaySound(SoundID.MenuTick);

                try
                {
                    Platform.Get<IPathService>().OpenURL(link);
                }
                catch (Exception ex)
                {
                    ContentPatch.PrintTry($"无法打开链接:{ex}");
                }
            };
            ui.OnUpdate += _ =>
            {
                ui.TextColor = Color.DeepSkyBlue;
                if (ui.IsMouseHovering != true) ui.TextColor *= 0.8f;
            };
            ui.OnMouseOver += (e, s) => SoundEngine.PlaySound(SoundID.MenuTick);
        }
    }
}
