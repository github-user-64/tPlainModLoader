using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.Localization;
using Terraria.UI;

namespace tContentPatch.Content.Menus.Patch_UIWorkshopHub
{
    internal class Patch
    {
        [HarmonyPatch(typeof(Main), "OpenResourcePacksMenu")]
        private class Main_OpenResourcePacksMenu
        {
            internal static bool Prefix(UIState uiStateToGoBackTo)
            {
                if (uiStateToGoBackTo != null) return true;

                Main.menuMode = 888;
                UIWorkshopHub uiworkshopHub = new UIWorkshopHub(null);
                uiworkshopHub.EnterHub();
                Main.MenuUI.SetState(uiworkshopHub);

                return false;
            }
        }

        [HarmonyPatch(typeof(Main), "DrawMenu")]
        private class Main_DrawMenu
        {
            internal static void Prefix(GameTime gameTime)
            {
                if (Main.menuMode != 0) return;

                LocalizedText lt = Language.GetText("UI.ResourcePacks");
                if (lt.Value == Language.GetText("UI.Workshop").Value) return;

                System.Reflection.PropertyInfo pi = typeof(LocalizedText).GetProperty("Value");
                pi.SetValue(lt, Language.GetText("UI.Workshop").Value);
            }
        }

        [HarmonyPatch(typeof(UIWorkshopHub), "OnInitialize")]
        private class GameContent_UI_States_UIWorkshopHub_OnInitialize
        {
            internal static void Postfix(UIWorkshopHub __instance)
            {
                System.Reflection.FieldInfo fi = typeof(UIWorkshopHub).GetField("_descriptionText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                UIText _descriptionText = (UIText)fi.GetValue(__instance);

                Action ShowOptionDescription = () => _descriptionText.SetText("管理模组的启用, 设置和删除");

                if (__instance.Children?.Count() < 0) return;

                UIElement uIElement = __instance.Children.First();
                if (uIElement?.Children?.Count() > 0 == false) return;

                UIElement uIPanel = uIElement.Children.FirstOrDefault(i => i.GetType() == typeof(UIPanel));
                if (uIPanel?.Children?.Count() > 0 == false) return;

                UIElement uIElement2 = uIPanel.Children.Last();
                if (uIElement2?.Children?.Count() > 0 == false) return;

                UIElement[] uies = uIElement2.Children.ToArray();

                uies[0] = MakeFancyButton(__instance,
                    Utils.Resource.GetTexture2D($"{nameof(tContentPatch)}.Resources.UI.Workshop.HudModManager.png"),
                    "模组管理器",
                    ShowOptionDescription);
                uies[0].OnLeftClick += (e, s) => ModManager.ModManager.OpenModManagerMenu(__instance);

                uIElement2.RemoveAllChildren();
                foreach (UIElement uie in uies) uIElement2.Append(uie);
            }
        }

        private static UIElement MakeFancyButton(UIWorkshopHub This, Texture2D iconImage, string text, Action ShowOptionDescription)
        {
            UIPanel uIPanel = new UIPanel();
            int num = -3;
            int num2 = -3;
            uIPanel.Width = StyleDimension.FromPixelsAndPercent(num, 0.5f);
            uIPanel.Height = StyleDimension.FromPixelsAndPercent(num2, 0.5f);
            uIPanel.OnMouseOver += SetColorsToHovered;
            uIPanel.OnMouseOut += SetColorsToNotHovered;
            uIPanel.BackgroundColor = new Color(63, 82, 151) * 0.7f;
            uIPanel.BorderColor = new Color(89, 116, 213) * 0.7f;
            uIPanel.SetPadding(6f);
            UIImage uIImage = new UIImage(iconImage)
            {
                IgnoresMouseInteraction = true,
                VAlign = 0.5f
            };
            uIImage.Left.Set(2f, 0f);
            uIPanel.Append(uIImage);
            uIPanel.OnMouseOver += (e, s) => ShowOptionDescription();
            uIPanel.OnMouseOut += This.ClearOptionDescription;
            UIText uIText = new UIText(text, 0.45f, large: true)
            {
                HAlign = 0f,
                VAlign = 0.5f,
                Width = StyleDimension.FromPixelsAndPercent(-80f, 1f),
                Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
                Top = StyleDimension.FromPixelsAndPercent(5f, 0f),
                Left = StyleDimension.FromPixels(80f),
                IgnoresMouseInteraction = true,
                TextOriginX = 0f,
                TextOriginY = 0f
            };
            uIText.PaddingLeft = 0f;
            uIText.PaddingRight = 20f;
            uIText.PaddingTop = 10f;
            uIText.IsWrapped = true;
            uIPanel.Append(uIText);
            uIPanel.SetSnapPoint("Button", 0);
            return uIPanel;
        }

        private static void SetColorsToHovered(UIMouseEvent evt, UIElement listeningElement)
        {
            UIPanel obj = (UIPanel)evt.Target;
            obj.BackgroundColor = new Color(73, 94, 171);
            obj.BorderColor = new Color(89, 116, 213);
        }

        private static void SetColorsToNotHovered(UIMouseEvent evt, UIElement listeningElement)
        {
            UIPanel obj = (UIPanel)evt.Target;
            obj.BackgroundColor = new Color(63, 82, 151) * 0.7f;
            obj.BorderColor = new Color(89, 116, 213) * 0.7f;
        }
    }
}
