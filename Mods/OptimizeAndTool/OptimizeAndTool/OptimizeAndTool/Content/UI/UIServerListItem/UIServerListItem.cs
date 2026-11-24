using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OptimizeAndTool.Content.ServerList;
using ReLogic.Content;
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
        private Asset<Texture2D> _buttonMoveTexture;
        private Asset<Texture2D> _buttonEditInfoTexture;

        /// <summary>
        /// ui的背景
        /// </summary>
        public readonly UIElement UIBackStyle = null;
        /// <summary>
        /// 设置样式
        /// </summary>
        public Action<UIServerListItem> Style = Style1;
        private bool StyleLoaded = false;

        public Func<bool> GetIsChecked = null;
        public Action OnUp = null;
        public Action OnDown = null;
        public Action OnDel = null;
        public readonly ServerInfo Info = null;

        public readonly UIStackPanel UILeftButtons = null;
        public readonly UIStackPanel UIRightButtons = null;
        public readonly UIText UILabel = null;

        public UIServerListItem(ServerInfo info)
        {
            _data = Info = info ?? throw new ArgumentNullException(nameof(info));

            //

            LoadTextures();

            Width.Set(0f, 1f);
            OnLeftDoubleClick += (e, s) => PlayGame();

            UIBackStyle = new UIState();
            Append(UIBackStyle);

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
            UIRightButtons.ItemMargin = 8f;
            UIRightButtons.OnUpdate += _ => UIRightButtons.UpdateContainer_Height();
            Append(UIRightButtons);

            InitializeButtons();

            UILabel = new UIText(string.Empty);
            UILabel.VAlign = 1f;
            UILabel.MarginLeft = 4f;
            UILabel.MarginBottom = 3f;
            UILeftButtons.Append(UILabel);
        }

        public override void Update(GameTime gameTime)
        {
            if (StyleLoaded == false)
            {
                StyleLoaded = true;
                ApplyBackStyle();
            }

            base.Update(gameTime);

            bool IsChecked = GetIsChecked?.Invoke() == true;

            BorderColor = IsChecked ? Color.YellowGreen : new Color(89, 116, 213) * 0.7f;

            if (IsChecked != true) return;

            if (InputUtils.DownCount(Keys.Up) == 1 || InputUtils.DownCount(Keys.Up) > 30) Up();
            else
            if (InputUtils.DownCount(Keys.Down) == 1 || InputUtils.DownCount(Keys.Down) > 30) Down();
        }

        /// <summary>
        /// 设置页面背景
        /// </summary>
        protected virtual void ApplyBackStyle()
        {
            Style?.Invoke(this);
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

        protected void SetUILabel(UIElement ui, Func<string> OnGetText)
        {
            ui.OnMouseOver += (e, s) => SetLabel(OnGetText());
            ui.OnMouseOut += (e, s) => SetLabel(null);
        }

        private void LoadTextures()
        {
            _buttonPlayTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay");
            _buttonSeedTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonSeed");
            _buttonDeleteTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete");
            _buttonMoveTexture = Main.Assets.Request<Texture2D>("Images/UI/TexturePackButtons");
            _buttonEditInfoTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonRename");
        }
    }
}
