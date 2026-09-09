using HarmonyLib;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using tContentPatch.Utils;
using Terraria;
using Terraria.GameInput;
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

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        public static void UpdatePrefix(GameTime gameTime)
        {
            try
            {
                UpdatePrefix_CanUpdateGameplay();

                mod.ForTry(item => item.UpdatePrefix(gameTime));
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        private static bool _UpdatePrefix_CanUpdateGameplay_old = false;

        private static void UpdatePrefix_CanUpdateGameplay()
        {
            if (Main.netMode != 0 && Main.netMode != 1) return;

            if (_UpdatePrefix_CanUpdateGameplay_old == false && Main.CanUpdateGameplay == true)
            {
                mod.ForTry(item => item.OnEnterWorld());
            }

            _UpdatePrefix_CanUpdateGameplay_old = Main.CanUpdateGameplay;
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void UpdatePostfix(GameTime gameTime)
        {
            mod.ForTry(item => item.UpdatePostfix(gameTime));
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

                mod.ForTry(item => item.SetupDrawInterfaceLayersPostfix(gameInterfaceLayers));
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
            int scrollWheel = PlayerInput.ScrollWheelDeltaForUI;

            mod.ForTry(item =>
            {
                PlayerInput.ScrollWheelDeltaForUI = scrollWheel;
                item.UpdateUIStatesPrefix(gameTime);
            });

            PlayerInput.ScrollWheelDeltaForUI = scrollWheel;
        }

        [HarmonyPatch("UpdateUIStates")]
        [HarmonyPostfix]
        public static void UpdateUIStatesPostfix(GameTime gameTime)
        {
            int scrollWheel = PlayerInput.ScrollWheelDeltaForUI;

            mod.ForTry(item =>
            {
                PlayerInput.ScrollWheelDeltaForUI = scrollWheel;
                item.UpdateUIStatesPostfix(gameTime);
            });

            PlayerInput.ScrollWheelDeltaForUI = scrollWheel;
        }

        [HarmonyPatch("DoUpdateInWorld")]
        [HarmonyPrefix]
        public static void DoUpdateInWorldPrefix()
        {
            mod.ForTry(item => item.DoUpdateInWorldPrefix());
        }

        [HarmonyPatch("DoUpdateInWorld")]
        [HarmonyPostfix]
        public static void DoUpdateInWorldPostfix()
        {
            mod.ForTry(item => item.DoUpdateInWorldPostfix());
        }

        [HarmonyPatch("DrawMap")]
        [HarmonyPostfix]
        public static void DrawMapPostfix(GameTime gameTime)
        {
            mod.ForTry(item => item.DrawMapPostfix(gameTime));
        }

        [HarmonyPatch("DrawMenu")]
        [HarmonyPrefix]
        public static void DrawMenuPrefix(GameTime gameTime)
        {
            mod.ForTry(item => item.DrawMenuPrefix(gameTime));
        }

        [HarmonyPatch("DrawMenu")]
        [HarmonyPostfix]
        public static void DrawMenuPostfix(GameTime gameTime)
        {
            mod.ForTry(item => item.DrawMenuPostfix(gameTime));
        }

        [HarmonyPatch("MouseText_DrawItemTooltip_GetLinesInfo")]
        [HarmonyPostfix]
        public static void MouseText_DrawItemTooltip_GetLinesInfoPostfix(Item item, ref int yoyoLogo,
            ref float oldKB, ref int numLines, ref string[] toolTipLine, ref Color[] lineColors)
        {
            foreach (PatchMain i in mod)
            {
                try
                {
                    i.MouseText_DrawItemTooltip_GetLinesInfoPostfix(item, ref yoyoLogo,
                       ref oldKB, ref numLines, ref toolTipLine, ref lineColors);
                }
                catch (Exception ex)
                {
                    OutputDebug.OutputException(ex);
                }
            }
        }

        [HarmonyPatch("DoDraw")]
        [HarmonyPrefix]
        public static void DoDrawPrefix(GameTime gameTime)
        {
            mod.ForTry(item => item.DoDrawPrefix(gameTime));
        }

        [HarmonyPatch("DoDraw")]
        [HarmonyPostfix]
        public static void DoDrawPostfix(GameTime gameTime)
        {
            mod.ForTry(item => item.DoDrawPostfix(gameTime));
        }

        [HarmonyPatch("PlayerFocusedScreenPosition")]
        [HarmonyPostfix]
        public static void PlayerFocusedScreenPosition(ref Vector2 __result)
        {
            Vector2 origin = __result;
            Vector2 modifi = __result;

            mod.ForTry(item => modifi = item.PlayerFocusedScreenPosition(origin, modifi));

            __result = modifi;
        }
    }
}
