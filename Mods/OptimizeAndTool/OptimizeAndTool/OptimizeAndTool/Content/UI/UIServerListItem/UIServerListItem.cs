using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OptimizeAndTool.Content.ServerList;
using ReLogic.Content;
using ReLogic.OS;
using System;
using tContentPatch.Content.UI;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
using Terraria.UI;

namespace OptimizeAndTool.Content.UI
{
    public partial class UIServerListItem : AWorldListItem
    {
        private Asset<Texture2D> _dividerTexture;
        private Asset<Texture2D> _innerPanelTexture;
        private Asset<Texture2D> _buttonCloudActiveTexture;
        private Asset<Texture2D> _buttonCloudInactiveTexture;
        private Asset<Texture2D> _buttonFavoriteActiveTexture;
        private Asset<Texture2D> _buttonFavoriteInactiveTexture;
        private Asset<Texture2D> _buttonPlayTexture;
        private Asset<Texture2D> _buttonSeedTexture;
        private Asset<Texture2D> _buttonRenameTexture;
        private Asset<Texture2D> _buttonDeleteTexture;
        private Asset<Texture2D> _hasBeenPlayedByActivePlayerTexture;
        private Asset<Texture2D> _isNewlyGeneratedTexture;

        public readonly ServerInfo info = null;

        private UIElement _worldIcon = null;
        private UIText _buttonLabel = null;

        public readonly UIStackPanel UILeftButtons = null;

        public UIServerListItem(ServerInfo info)
        {
            _data = this.info = info ?? throw new ArgumentNullException(nameof(info));

            //

            LoadTextures();
            InitializeAppearance();

            //

            _worldIcon = GetIcoUI(this.info);
            Append(_worldIcon);

            InitializeInfos();

            //

            UILeftButtons = new UIStackPanel();
            UILeftButtons.VAlign = 1f;
            UILeftButtons.Horizontal = true;
            UILeftButtons.IsAutoUpdateSize = true;
            UILeftButtons.ItemMargin = 4f;
            UILeftButtons.OnUpdate += _ => UILeftButtons.UpdateContainer_Height();
            Append(UILeftButtons);

            InitializeButtons();

            _buttonLabel = new UIText(string.Empty);
            _buttonLabel.VAlign = 1f;
            _buttonLabel.MarginLeft = 4f;
            _buttonLabel.MarginBottom = 3f;
            UILeftButtons.Append(_buttonLabel);
        }

        protected UIElement GetIcoUI(WorldFileData data)
        {
            UIElement ico = GetIconElement();

            if (data.DefeatedMoonlord)
            {
                UIImage uie = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/IconCompletion"));
                uie.HAlign = 0.5f;
                uie.VAlign = 0.5f;
                uie.Top = new StyleDimension(-10f, 0f);
                uie.Left = new StyleDimension(-3f, 0f);
                uie.IgnoresMouseInteraction = true;
                ico.Append(uie);
            }

            if (GetIcons().Count > 1 && !data.ZenithWorld)
            {
                UIImage uie = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/IconMixedSeed"));
                uie.HAlign = 0f;
                uie.VAlign = 1f;
                uie.Top = StyleDimension.FromPixels(0f);
                uie.Left = StyleDimension.FromPixels(0f);
                ico.Append(uie);
            }

            return ico;
        }

        protected void SetLabel(string txt = null)
        {
            _buttonLabel.SetText(txt ?? string.Empty);
        }

        private void InitializeAppearance()
        {
            Height.Set(96f, 0f);
            Width.Set(0f, 1f);
            SetPadding(6f);
            SetColorsToNotHovered();
        }

        private void SetColorsToNotHovered()
        {
            BackgroundColor = new Color(63, 82, 151) * 0.7f;
            BorderColor = new Color(89, 116, 213) * 0.7f;
            //if (!_canBePlayed)
            //{
            //    BorderColor = new Color(127, 127, 127) * 0.7f;
            //    BackgroundColor = Color.Lerp(new Color(63, 82, 151), new Color(80, 80, 80), 0.5f) * 0.7f;
            //}
        }

        private void LoadTextures()
        {
            _dividerTexture = Main.Assets.Request<Texture2D>("Images/UI/Divider");
            _innerPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground");
            _buttonCloudActiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonCloudActive");
            _buttonCloudInactiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonCloudInactive");
            _buttonFavoriteActiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonFavoriteActive");
            _buttonFavoriteInactiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonFavoriteInactive");
            _buttonPlayTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay");
            _buttonSeedTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonSeed");
            _buttonRenameTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonRename");
            _buttonDeleteTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete");
            _hasBeenPlayedByActivePlayerTexture = Main.Assets.Request<Texture2D>("Images/UI/IconPlayedBefore");
            _isNewlyGeneratedTexture = Main.Assets.Request<Texture2D>("Images/UI/IconNewlyGenerated");
        }

        protected void TryCopyText(string text = null)
        {
            try
            {
                if (text != null) Platform.Get<IClipboard>().Value = text;
            }
            catch { }
        }
    }
}
