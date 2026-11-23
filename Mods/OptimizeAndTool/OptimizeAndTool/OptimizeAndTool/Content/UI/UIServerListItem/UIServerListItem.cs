using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OptimizeAndTool.Content.ServerList;
using ReLogic.Content;
using ReLogic.OS;
using System;
using tContentPatch.Content;
using tContentPatch.Content.UI;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
using Terraria.UI;

namespace OptimizeAndTool.Content.UI
{
    public partial class UIServerListItem : AWorldListItem
    {
        private Asset<Texture2D> _buttonPlayTexture;
        private Asset<Texture2D> _buttonSeedTexture;
        private Asset<Texture2D> _buttonDeleteTexture;

        private UIElement _worldIcon = null;
        private UIText _buttonLabel = null;

        public Func<bool> GetIsChecked = null;
        public Action OnUp = null;
        public Action OnDown = null;
        public Action OnDel = null;
        public readonly ServerInfo Info = null;
        public readonly UIStackPanel UILeftButtons = null;
        public readonly UIStackPanel UIRightButtons = null;

        public UIServerListItem(ServerInfo info)
        {
            _data = Info = info ?? throw new ArgumentNullException(nameof(info));

            //

            LoadTextures();

            Width.Set(0f, 1f);
            Height.Set(100f, 0f);
            BackgroundColor = new Color(63, 82, 151) * 0.7f;
            BorderColor = new Color(89, 116, 213) * 0.7f;
            SetPadding(8f);

            //

            _worldIcon = GetIcoUI(Info);
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

            UIRightButtons = new UIStackPanel();
            UIRightButtons.HAlign = 1f;
            UIRightButtons.VAlign = 1f;
            UIRightButtons.Horizontal = true;
            UIRightButtons.IsAutoUpdateSize = true;
            UIRightButtons.ItemMargin = 4f;
            UIRightButtons.OnUpdate += _ => UIRightButtons.UpdateContainer_Height();
            Append(UIRightButtons);

            InitializeButtons();

            _buttonLabel = new UIText(string.Empty);
            _buttonLabel.VAlign = 1f;
            _buttonLabel.MarginLeft = 4f;
            _buttonLabel.MarginBottom = 3f;
            UILeftButtons.Append(_buttonLabel);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            bool IsChecked = GetIsChecked?.Invoke() == true;

            BorderColor = IsChecked ? Color.YellowGreen : new Color(89, 116, 213) * 0.7f;

            if (IsChecked != true) return;

            if (InputUtils.DownCount(Keys.Up) == 1 || InputUtils.DownCount(Keys.Up) > 30) Up();
            else
            if (InputUtils.DownCount(Keys.Down) == 1 || InputUtils.DownCount(Keys.Down) > 30) Down();
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

        protected void SetUILabel(UIElement ui, Func<string> OnGetText)
        {
            ui.OnMouseOver += (e, s) => SetLabel(OnGetText());
            ui.OnMouseOut += (e, s) => SetLabel(null);
        }

        protected void TryCopyText(string text = null)
        {
            try
            {
                if (text != null) Platform.Get<IClipboard>().Value = text;
            }
            catch { }
        }

        private void LoadTextures()
        {
            _buttonPlayTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay");
            _buttonSeedTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonSeed");
            _buttonDeleteTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete");
        }
    }
}
