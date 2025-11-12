using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using System.Threading.Tasks;
using tContentPatch.Content.UI;
using tContentPatch.ModLoad;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace tContentPatch.Content.Menus.ModManager
{
    internal class UIElementEmpty : UIElement
    {
        public UIElementEmpty()
        {
            Width.Set(-10, 1);
            Height.Pixels = 4;
            MarginTop = 10;
            MarginBottom = 10;
            HAlign = 0.5f;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dim = GetDimensions();
            Rectangle rect = new Rectangle(0, 0, (int)dim.Width, 2);
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Vector2(dim.X, dim.Y), rect, Color.White * 0.1f);
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Vector2(dim.X, dim.Y + 2), rect, Color.Black * 0.1f);
        }
    }

    internal class UIModItem : UIPanel
    {
        private readonly static Texture2D defaultIco = null;

        private ModObject mo = null;

        private bool modIcoOneLoad = true;
        private string modIcoPath = null;

        private UIStackPanel ui_sp = null;
        private UIElement ui_sp_uie = null;
        private UIStackPanel ui_sp_uie_sp = null;
        private UIImage ui_ico = null;
        private UIText ui_mod_name = null;
        private UITextPanel<string> ui_mod_isEnable = null;
        private UIImageButton ui_mod_del = null;

        static UIModItem()
        {
            defaultIco = Utils.Resource.GetTexture2D($"{nameof(tContentPatch)}.Resources.UI.ModIcon.png");
        }

        public UIModItem(UIState backUI, ModObject mo_)
        {
            mo = mo_;

            Width.Precent = 1;
            Height.Pixels = 100;
            SetPadding(8);
            BorderColor = new Color(73, 92, 171);

            modIcoPath = Path.Combine(mo.modPath, InfoList.Files.ModIco);

            ui_sp = new UIStackPanel();
            ui_sp.HAlign = 1;
            ui_sp.VAlign = 0.5f;

            UIElementEmpty ui_sp_Empty = new UIElementEmpty();

            ui_sp_uie = new UIElement();
            ui_sp_uie.Width.Precent = 1;

            ui_sp_uie_sp = new UIStackPanel();
            ui_sp_uie_sp.HAlign = 1;
            ui_sp_uie_sp.VAlign = 0.5f;
            ui_sp_uie_sp.Horizontal = true;
            ui_sp_uie_sp.ItemMargin = 10;

            ui_ico = new UIImage(defaultIco);
            ui_ico.Height.Precent = 1;
            ui_ico.VAlign = 0.5f;
            ui_ico.ScaleToFit = true;
            ui_ico.AllowResizingDimensions = false;//不让图片影响大小

            #region
            string name = $"{mo.info?.name ?? mo.config.key}";
            if (name.Length > 20) name = $"{name.Substring(0, 20)}...";

            ui_mod_name = new UIText($"{name}{(mo.config.version == null ? "" : $" v{mo.config.version}")}");
            #endregion

            ui_mod_isEnable = new UITextPanel<string>(string.Empty);
            ui_mod_isEnable.VAlign = 0.5f;
            ui_mod_isEnable.SetPadding(8);
            ui_mod_isEnable.OnLeftMouseDown += (e, s) =>
            {
                SoundEngine.PlaySound(12);
                ModManager.ModEnableReversal(mo);
            };

            #region 右下按钮
            if (ModManager.GetModIsLoaded(mo))
            {
                UIElement ui_mod_set = GetBtn(Main.Assets.Request<Texture2D>("Images/UI/Craft_Toggle_2", AssetRequestMode.ImmediateLoad),
                    () => ModManager.OpenModSetSwitch(mo), "设置");
                ui_sp_uie_sp.Append(ui_mod_set);

                UIElement ui_mod_openD = GetBtn(Main.Assets.Request<Texture2D>("Images/UI/Camera_6", AssetRequestMode.ImmediateLoad),
                    () => ModManager.OpenDirectory(mo.modPath), "文件夹");
                ui_sp_uie_sp.Append(ui_mod_openD);
            }
            else
            {
                ui_mod_del = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete", AssetRequestMode.ImmediateLoad));
                ui_mod_del.VAlign = 0.5f;
                ui_mod_del.OnLeftClick += (e, s) => ModManager.DelMod(mo);
                ui_sp_uie_sp.Append(ui_mod_del);
            }

            UIElement ui_mod_info = GetBtn(Main.Assets.Request<Texture2D>("Images/UI/Workshop/Tags", AssetRequestMode.ImmediateLoad),
                    () => ModManager.OpenModInfo(mo), "信息");
            ui_sp_uie_sp.Append(ui_mod_info);
            #endregion

            Append(ui_ico);
            Append(ui_sp);
            ui_sp.Append(ui_mod_name);
            ui_sp.Append(ui_sp_Empty);
            ui_sp.Append(ui_sp_uie);
            ui_sp_uie.Append(ui_sp_uie_sp);
            ui_sp_uie.Append(ui_mod_isEnable);
        }

        private UIElement GetBtn(Asset<Texture2D> texture, Action onclick, string mouseText = null)
        {
            UIImage ui = new UIImage(texture);
            ui.Width.Pixels = 32;
            ui.Height.Pixels = 32;
            ui.ScaleToFit = true;
            ui.Color = Color.Gray;
            ui.OnMouseOver += (e, s) => { ui.Color = Color.White; SoundEngine.PlaySound(12); };
            ui.OnMouseOut += (e, s) => ui.Color = Color.Gray;
            ui.OnLeftClick += (e, s) => onclick?.Invoke();
            if (mouseText != null) ui.OnUpdate += _ =>
            {
                if (ui.IsMouseHovering) DrawTip.SetDraw(mouseText);
            };

            return ui;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsMouseHovering)
            {
                BackgroundColor = new Color(93, 92, 191);
            }
            else
            {
                BackgroundColor = BorderColor;
            }

            if (mo.config.isEnable)
            {
                ui_mod_isEnable.TextColor = UIModManager.color_isEnable;
                ui_mod_isEnable.BackgroundColor = new Color(36, 46, 85);
                ui_mod_isEnable.BorderColor = BackgroundColor;
                ui_mod_isEnable.SetText("已启用");
            }
            else
            {
                ui_mod_isEnable.TextColor = UIModManager.color_isEnable_false;
                ui_mod_isEnable.BackgroundColor = Color.Transparent;
                ui_mod_isEnable.BorderColor = BackgroundColor;
                ui_mod_isEnable.SetText("已禁用");
            }

            if (ui_mod_isEnable.IsMouseHovering)
            {
                ui_mod_isEnable.BackgroundColor = mo.config.isEnable ? Color.Transparent : new Color(49, 61, 114);
            }

            ui_ico.Width.Pixels = ui_ico.GetDimensions().Height;
            ui_sp.Width.Set(-ui_ico.Width.Pixels - 4, 1);

            ui_sp_uie_sp.UpdateContainer_Width();

            ui_sp_uie_sp.UpdateContainer_Height();
            ui_sp_uie.UpdateContainer_Height();
            ui_sp.UpdateContainer_Height();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (ui_mod_isEnable.IsMouseHovering)
            {
                string[] s = null;

                if (mo.config.frontModKeys?.Length > 0 == true)
                {
                    s = new string[2 + mo.config.frontModKeys.Length];
                    s[1] = "必须的前置模组:";
                    for (int i = 2; i < s.Length; i++)
                    {
                        s[i] = mo.config.frontModKeys[i - 2];
                    }
                }
                else
                {
                    s = new string[1];
                }

                s[0] = mo.config.isEnable ? "[c/ff0000:禁用]" : "[c/00ff00:启用]";

                DrawTip.SetDraw(s);
            }
            else if (ui_mod_del?.IsMouseHovering == true)
            {
                DrawTip.SetDraw("删除");
            }
            else if (ui_mod_name.IsMouseHovering && mo.info?.author != null)
            {
                DrawTip.SetDraw($"作者:{mo.info.author}");
            }

            if (modIcoOneLoad == false) return;
            modIcoOneLoad = false;

            Task.Run(() =>
            {
                if (File.Exists(modIcoPath) == false) return;
                if (new FileInfo(modIcoPath).Length > 1024 * 1024) return;

                using (FileStream fs = new FileStream(modIcoPath, FileMode.Open, FileAccess.Read))
                {
                    Texture2D ico = Texture2D.FromStream(Main.graphics.GraphicsDevice, fs);
                    if (ico == null) return;
                    ui_ico.SetImage(ico);
                }
            });
        }
    }
}
