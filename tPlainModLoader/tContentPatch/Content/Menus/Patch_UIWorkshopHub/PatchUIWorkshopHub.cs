using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using tContentPatch.Content.UI;
using tContentPatch.Utils;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.UI;

namespace tContentPatch.Content.Menus.Patch_UIWorkshopHub
{
    [HarmonyPatch(typeof(UIWorkshopHub), nameof(UIWorkshopHub.OnInitialize))]
    internal static class PatchUIWorkshopHub
    {
        private static readonly int index_uIElement2 = 8;
        private static readonly int index_num6 = 6;
        private static readonly FieldInfo __descriptionText = typeof(UIWorkshopHub).GetField("_descriptionText", BindingFlags.NonPublic | BindingFlags.Instance);

        //[HarmonyDebug]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpiler_OnInitialize(IEnumerable<CodeInstruction> instructions)
        {
            CodeMatcher codeMatcher = new CodeMatcher(instructions);

            CodeMatch[] cs = new CodeMatch[]
            {
                new CodeMatch(OpCodes.Call, typeof(UIWorkshopHub).GetMethod("AddDescriptionPanel", BindingFlags.Instance | BindingFlags.NonPublic)),
            };

            codeMatcher.MatchStartForward(cs);
            codeMatcher.Advance(cs.Length);
            if (codeMatcher.IsInvalid)
            {
                string s = $"无法添加创意工坊中心的选项:找不到IL位置";
                Log.Add(s);
                ContentPatch.PrintTry(s);
                return instructions;
            }

            codeMatcher.InsertAndAdvance(
                new CodeMatch(OpCodes.Ldarg_0),//this
                new CodeMatch(OpCodes.Ldloca_S, index_uIElement2),
                new CodeMatch(OpCodes.Ldloc_S, index_num6),
                new CodeMatch(OpCodes.Call, typeof(PatchUIWorkshopHub).GetMethod(nameof(SetUI), BindingFlags.Static | BindingFlags.NonPublic)));

            return codeMatcher.Instructions();
        }

        private static void SetUI(UIWorkshopHub This, ref UIElement uie, int height)
        {
            UIElement[] uis = uie.Children.ToArray();

            UIScrollViewer sv = new UIScrollViewer();
            sv.Width.Set(0f, 1f);
            sv.Height.Set(height, 0f);
            uie = sv;

            UIWrapPanel wp = new UIWrapPanel();
            wp.Width.Set(1f, 1f);
            wp.Height.Set(0f, 0f);
            wp.ItemMargin = 4f;
            wp.OnUpdate += _ => wp.UpdateContainer_Height();
            sv.SetChild(wp);

            UIText _descriptionText = (UIText)__descriptionText.GetValue(This);
            StyleDimension itemWidth = new StyleDimension(-wp.ItemMargin, 0.5f);
            StyleDimension itemHeight = new StyleDimension(height / 2f, 0f);

            UIElement ui1 = MakeFancyButton(This, AssemblyResource.LoadNew<Texture2D>($"{nameof(tContentPatch)}.Resources.UI.Workshop.ModManager.png"),
                "模组管理器", "管理模组的启用, 设置和删除", _descriptionText,
                itemWidth, itemHeight);
            ui1.OnLeftClick += (e, s) => ModManager.ModManager.OpenModManagerMenu(This);
            wp.Append(ui1);

            UIElement ui2 = MakeFancyButton(This, AssemblyResource.LoadNew<Texture2D>($"{nameof(tContentPatch)}.Resources.UI.Workshop.CreateMod.png"),
                "创建模组", "创建一个新的模组项目", _descriptionText,
                itemWidth, itemHeight);
            ui2.OnLeftClick += (e, s) => CreateMod.UICreateMod.OpenMenu(This);
            wp.Append(ui2);

            foreach (UIElement i in uis)
            {
                i.Width = itemWidth;
                i.Height = itemHeight;
                i.HAlign = 0f;
                i.VAlign = 0f;
                wp.Append(i);
            }
        }

        private static UIElement MakeFancyButton(UIWorkshopHub This, Texture2D iconImage, string text, string msg, UIText uitip, StyleDimension width, StyleDimension height)
        {
            UIPanel uIPanel = new UIPanel();
            uIPanel.Width = width;
            uIPanel.Height = height;
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
            uIPanel.OnMouseOver += (e, s) => uitip.SetText(msg);
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
            uIPanel.OnLeftClick += (e, s) => SoundEngine.PlaySound(SoundID.MenuOpen);

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
