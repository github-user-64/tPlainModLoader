using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using tContentPatch.Patch;
using tContentPatch.Utils;
using Terraria;
using Terraria.UI;

namespace tContentPatch.ModPatch
{
    internal class Patch_Main : ClassPatch<PatchMain>
    {
        private static List<PatchMain> mod = new List<PatchMain>();

        public Patch_Main() : base(mod) { }

        public override void Initialize(IAddPatch addPatch)
        {
            Type oType = typeof(Main);
            Type tType = typeof(Patch_Main);
            Action<string, string, BindingFlags> addPre = (m, m2, bf) =>
            {
                addPatch.AddPrefix(oType.GetMethod(m, bf), tType.GetMethod(m2));
            };
            Action<string, string, BindingFlags> addPo = (m, m2, bf) =>
            {
                addPatch.AddPostfix(oType.GetMethod(m, bf), tType.GetMethod(m2));
            };

            addPre("Update", nameof(UpdatePrefix), BindingFlags.NonPublic | BindingFlags.Instance);
            addPo("Update", nameof(UpdatePostfix), BindingFlags.NonPublic | BindingFlags.Instance);

            addPo("SetupDrawInterfaceLayers", nameof(SetupDrawInterfaceLayersPostfix), BindingFlags.NonPublic | BindingFlags.Instance);

            addPre("UpdateUIStates", nameof(UpdateUIStatesPrefix), BindingFlags.NonPublic | BindingFlags.Static);
            addPo("UpdateUIStates", nameof(UpdateUIStatesPostfix), BindingFlags.NonPublic | BindingFlags.Static);

            addPre("DoUpdateInWorld", nameof(DoUpdateInWorldPrefix), BindingFlags.NonPublic | BindingFlags.Instance);
            addPo("DoUpdateInWorld", nameof(DoUpdateInWorldPostfix), BindingFlags.NonPublic | BindingFlags.Instance);

            addPo("DrawMap", nameof(DrawMapPostfix), BindingFlags.NonPublic | BindingFlags.Instance);

            addPre("DrawMenu", nameof(DrawMenuPrefix), BindingFlags.NonPublic | BindingFlags.Instance);

            addPo("MouseText_DrawItemTooltip_GetLinesInfo", nameof(MouseText_DrawItemTooltip_GetLinesInfoPostfix), BindingFlags.Public | BindingFlags.Static);
        }

        #region
        private static FieldInfo _gameInterfaceLayers_fi = null;
        #endregion

        private static bool _UpdatePrefix_CanUpdateGameplay_old = false;

        public static void UpdatePrefix(GameTime gameTime)
        {
            try
            {
                bool ___CanUpdateGameplay_old = _UpdatePrefix_CanUpdateGameplay_old;
                _UpdatePrefix_CanUpdateGameplay_old = Main.CanUpdateGameplay;

                if (___CanUpdateGameplay_old == false && _UpdatePrefix_CanUpdateGameplay_old == true)
                {
                    foreach (PatchMain item in mod) item.OnEnterWorld();
                }

                foreach (PatchMain item in mod) item.UpdatePrefix(gameTime);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        public static void UpdatePostfix(GameTime gameTime)
        {
            try
            {
                foreach (PatchMain item in mod) item.UpdatePostfix(gameTime);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        public static void SetupDrawInterfaceLayersPostfix()
        {
            try
            {
                if (_gameInterfaceLayers_fi == null)
                {
                    _gameInterfaceLayers_fi = typeof(Main).GetField("_gameInterfaceLayers", BindingFlags.NonPublic | BindingFlags.Instance);
                }

                List<GameInterfaceLayer> gameInterfaceLayers = (List<GameInterfaceLayer>)_gameInterfaceLayers_fi.GetValue(Main.instance);

                foreach (PatchMain item in mod) item.SetupDrawInterfaceLayersPostfix(gameInterfaceLayers);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        public static void UpdateUIStatesPrefix(GameTime gameTime)
        {
            try
            {
                foreach (PatchMain item in mod) item.UpdateUIStatesPrefix(gameTime);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        public static void UpdateUIStatesPostfix(GameTime gameTime)
        {
            try
            {
                foreach (PatchMain item in mod) item.UpdateUIStatesPostfix(gameTime);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        public static void DoUpdateInWorldPrefix(Stopwatch sw)
        {
            try
            {
                foreach (PatchMain item in mod) item.DoUpdateInWorldPrefix(sw);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        public static void DoUpdateInWorldPostfix(Stopwatch sw)
        {
            try
            {
                foreach (PatchMain item in mod) item.DoUpdateInWorldPostfix(sw);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        public static void DrawMapPostfix(GameTime gameTime)
        {
            try
            {
                foreach (PatchMain item in mod) item.DrawMapPostfix(gameTime);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

        public static void DrawMenuPrefix(GameTime gameTime)
        {
            try
            {
                foreach (PatchMain item in mod) item.DrawMenuPrefix(gameTime);
            }
            catch (Exception ex)
            {
                OutputDebug.OutputException(ex);
            }
        }

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
