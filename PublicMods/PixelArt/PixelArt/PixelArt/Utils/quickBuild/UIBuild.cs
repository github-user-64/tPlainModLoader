using Microsoft.Xna.Framework.Graphics;
using PixelArt.Content.UI;
using System;
using System.Collections.Generic;
using tContentPatch.Content.UI;
using Terraria;
using Terraria.UI;

namespace PixelArt.Utils.quickBuild
{
    /// <summary>
    /// 创建一些预设的ui
    /// </summary>
    internal static class UIBuild
    {
        /// <summary>
        /// 文本框, 开关, 绑定值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gsr"></param>
        /// <param name="setGsr"></param>
        /// <param name="parseTry"></param>
        /// <param name="mouseText"></param>
        /// <param name="ico"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static UIElement get1<T>(GetSetReset<bool> gsr, GetSetReset<T> setGsr, Func<string, T> parseTry, string mouseText = null, string ico = null, string text = null)
        {
            Texture2D texture = ico == null ? null : Main.Assets.Request<Texture2D>(ico, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            UIItemTextBoxASwitchBind<T> ui = new UIItemTextBoxASwitchBind<T>(gsr, setGsr, parseTry, texture, text);
            ui.MouseText = mouseText;

            return ui;
        }

        /// <summary>
        /// 开关, 绑定值
        /// </summary>
        /// <param name="gsr"></param>
        /// <param name="mouseText"></param>
        /// <param name="ico"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static UIElement get2(GetSetReset<bool> gsr, string mouseText = null, string ico = null, string text = null)
        {
            Texture2D texture = ico == null ? null : Main.Assets.Request<Texture2D>(ico, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            UIItemSwitchBind ui = new UIItemSwitchBind(gsr, texture, text);
            ui.MouseText = mouseText;

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
    }
}
