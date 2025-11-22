using HarmonyLib;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using tContentPatch.Utils;
using Terraria;
using Terraria.UI;

namespace tContentPatch.ModPatch
{
    [HarmonyPatch(typeof(Main))]
    internal class Patch_Main : ListCopy<PatchMain>
    {
        private static List<PatchMain> mod = new List<PatchMain>();

        public Patch_Main() : base(mod) { }

        #region
        private static FieldInfo _gameInterfaceLayers_fi = null;
        #endregion

        private static bool _UpdatePrefix_CanUpdateGameplay_old = false;

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        public static void UpdatePrefix(GameTime gameTime)
        {
            try
            {
                bool ___CanUpdateGameplay_old = _UpdatePrefix_CanUpdateGameplay_old;
                _UpdatePrefix_CanUpdateGameplay_old = Main.CanUpdateGameplay;

                if (___CanUpdateGameplay_old == false && _UpdatePrefix_CanUpdateGameplay_old == true)
                {
                    mod.For(item => item.OnEnterWorld());
                }

                mod.For(item => item.UpdatePrefix(gameTime));
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void UpdatePostfix(GameTime gameTime)
        {
            try
            {
                mod.For(item => item.UpdatePostfix(gameTime));
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        [HarmonyPatch("SetupDrawInterfaceLayers")]
        [HarmonyPostfix]
        public static void SetupDrawInterfaceLayersPostfix()
        {
            try
            {
                if (_gameInterfaceLayers_fi == null)
                {
                    _gameInterfaceLayers_fi = typeof(Main).GetField("_gameInterfaceLayers", BindingFlags.NonPublic | BindingFlags.Instance);
                }

                List<GameInterfaceLayer> gameInterfaceLayers = (List<GameInterfaceLayer>)_gameInterfaceLayers_fi.GetValue(Main.instance);

                mod.For(item => item.SetupDrawInterfaceLayersPostfix(gameInterfaceLayers));
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        [HarmonyPatch("UpdateUIStates")]
        [HarmonyPrefix]
        public static void UpdateUIStatesPrefix(GameTime gameTime)
        {
            try
            {
                mod.For(item => item.UpdateUIStatesPrefix(gameTime));
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        [HarmonyPatch("UpdateUIStates")]
        [HarmonyPostfix]
        public static void UpdateUIStatesPostfix(GameTime gameTime)
        {
            try
            {
                mod.For(item => item.UpdateUIStatesPostfix(gameTime));
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        [HarmonyPatch("DoUpdateInWorld")]
        [HarmonyPrefix]
        public static void DoUpdateInWorldPrefix(Stopwatch sw)
        {
            try
            {
                mod.For(item => item.DoUpdateInWorldPrefix(sw));
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        [HarmonyPatch("DoUpdateInWorld")]
        [HarmonyPostfix]
        public static void DoUpdateInWorldPostfix(Stopwatch sw)
        {
            try
            {
                mod.For(item => item.DoUpdateInWorldPostfix(sw));
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        [HarmonyPatch("DrawMap")]
        [HarmonyPostfix]
        public static void DrawMapPostfix(GameTime gameTime)
        {
            try
            {
                mod.For(item => item.DrawMapPostfix(gameTime));
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        [HarmonyPatch("DrawMenu")]
        [HarmonyPrefix]
        public static void DrawMenuPrefix(GameTime gameTime)
        {
            try
            {
                mod.For(item => item.DrawMenuPrefix(gameTime));
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        [HarmonyPatch("MouseText_DrawItemTooltip_GetLinesInfo")]
        [HarmonyPostfix]
        public static void MouseText_DrawItemTooltip_GetLinesInfoPostfix(Item item, ref int yoyoLogo, ref int researchLine,
            ref float oldKB, ref int numLines, ref string[] toolTipLine, ref bool[] preFixLine, ref bool[] badPreFixLine)
        {
            try
            {
                foreach (PatchMain i in mod) i.MouseText_DrawItemTooltip_GetLinesInfoPostfix(item, ref yoyoLogo, ref researchLine,
                    ref oldKB, ref numLines, ref toolTipLine, ref preFixLine, ref badPreFixLine);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }
    }
}
