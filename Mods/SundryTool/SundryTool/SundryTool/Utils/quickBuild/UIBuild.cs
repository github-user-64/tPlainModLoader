using Microsoft.Xna.Framework.Graphics;
using SundryTool.Content.UI;
using System;
using System.Collections.Generic;
using tContentPatch.Content.UI;
using tContentPatch.Content.UI.ModSet;
using Terraria;
using Terraria.UI;

namespace SundryTool.Utils.quickBuild
{
    /// <summary>
    /// 创建一些预设的ui
    /// </summary>
    internal static class UIBuild
    {
        /// <summary>
        /// 文本框, 开关, 绑定值
        /// </summary>
        public static UIElement get1<T>(GetSetReset<bool> gsr, GetSetReset<T> setGsr, Func<string, T> parseTry, string mouseText = null, string ico = null, string text = null)
        {
            Texture2D texture = ico == null ? null : Main.Assets.Request<Texture2D>(ico, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            UIItemSwitchATextBox<T> ui = new UIItemSwitchATextBox<T>(texture, text);
            ui.MouseText = mouseText;
            ui.ValueToText = v => $"{v}";
            ui.TextToValue = parseTry;
            ui.OnGetIsChecked = () => gsr.val;
            ui.OnSetIsChecked = v => gsr.val = v;
            ui.OnGetValue = () => setGsr.val;
            ui.OnSetValue = v => setGsr.val = v;

            return ui;
        }

        /// <summary>
        /// 开关, 绑定值
        /// </summary>
        public static UIElement get2(GetSetReset<bool> gsr, string mouseText = null, string ico = null, string text = null)
        {
            Texture2D texture = ico == null ? null : Main.Assets.Request<Texture2D>(ico, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            UIItemSwitch ui = new UIItemSwitch(texture, text);
            ui.MouseText = mouseText;
            ui.OnGetIsChecked = () => gsr.val;
            ui.OnSetIsChecked = v => gsr.val = v;

            return ui;
        }

        public static UIElement get3(List<UIElement> uis)
        {
            UIScrollViewer2 sv = new UIScrollViewer2();
            sv.Width.Precent = 1;
            sv.Height.Precent = 1;

            foreach (UIElement ui in uis)
            {
                sv.AddChild(ui);
            }

            return sv;
        }

        public static UIElement get4(string btnTxt, Action click, string mouseText = null, string ico = null, string text = null)
        {
            Texture2D texture = ico == null ? null : Main.Assets.Request<Texture2D>(ico, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            UIItemButton ui = new UIItemButton(btnTxt, texture, text);
            ui.MouseText = mouseText;
            ui.OnClick = click;

            return ui;
        }

        /// <summary>
        /// 文本框
        /// </summary>
        public static UIElement get5<T>(GetSetReset<T> gsr, Func<string, T> parseTry, string mouseText = null, Texture2D ico = null, string text = null)
        {
            UIItemTextBox<T> ui = new UIItemTextBox<T>(ico, text);
            ui.MouseText = mouseText;
            ui.ValueToText = v => $"{v}";
            ui.TextToValue = parseTry;
            ui.OnGetValue = () => gsr.val;
            ui.OnSetValue = v => gsr.val = v;

            return ui;
        }
    }
}
