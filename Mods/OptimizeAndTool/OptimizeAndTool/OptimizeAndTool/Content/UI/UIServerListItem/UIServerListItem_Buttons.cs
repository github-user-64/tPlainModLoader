using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Localization;

namespace OptimizeAndTool.Content.UI
{
    public partial class UIServerListItem
    {
        protected virtual void InitializeButtons()
        {
            UILeftButtons.Append(GetButton(PlayGame, () => Language.GetTextValue("UI.Play"), _buttonPlayTexture));

            if (Info.WorldGeneratorVersion != 0L)
            {
                UILeftButtons.Append(GetButton(
                    CopySeed,
                    () => Language.GetTextValue("UI.CopySeed", Info.GetFullSeedText(allowCropping: true)),
                    _buttonSeedTexture));
            }

            UILeftButtons.Append(GetButton(EditInfo, () => "编辑信息", _buttonEditInfoTexture));

            //

            UIImageButton up = GetButton(Up, () => $"选中时按{Keys.Up}也可移动", _buttonMoveTexture);
            up.Frame = _buttonMoveTexture.Frame(horizontalFrames: 2, verticalFrames: 2);
            up.TextureColor = Color.GreenYellow;
            UIRightButtons.Append(up);

            UIImageButton down = GetButton(Down, () => $"选中时按{Keys.Down}也可移动", _buttonMoveTexture);
            down.Frame = _buttonMoveTexture.Frame(horizontalFrames: 2, verticalFrames: 2, frameX: 1);
            down.TextureColor = Color.GreenYellow;
            UIRightButtons.Append(down);

            UIImageButton del = GetButton(Delete, () => Language.GetTextValue("UI.Delete"), _buttonDeleteTexture);
            del.MarginLeft = 10f;
            UIRightButtons.Append(del);
        }

        protected UIImageButton GetButton(Action OnClick, Func<string> OnGetText, Asset<Texture2D> texture)
        {
            UIImageButton ui = new UIImageButton(22f, texture);
            ui.OnLeftClick += (e, s) => OnClick();
            SetUILabel(ui, OnGetText);

            return ui;
        }
    }
}
