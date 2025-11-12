using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.Social;

namespace tContentPatch.Content.Menus.ModSetSwitch
{
    //屎
    internal class Patch
    {
        [HarmonyPatch(typeof(IngameOptions), nameof(IngameOptions.DrawLeftSide))]
        private class IngameOptions_DrawLeftSide
        {
            private static int iOff = 0;
            private static float[] scale = new float[1];

            internal static void Prefix(SpriteBatch sb, string txt, int i, ref Vector2 anchor, ref Vector2 offset, float[] scales, float minscale = 0.7f, float maxscale = 0.8f, float scalespeed = 0.01f)
            {
                if (i == 0) iOff = 0;

                //

                bool flag5 = SocialAPI.Network != null && SocialAPI.Network.CanInvite();
                int num7 = (flag5 ? 1 : 0);
                int num8 = 5 + num7 + 1 + 2;
                Vector2 vector2 = new Vector2(670f, 480f);
                offset = new Vector2(0f, vector2.Y - 20 * 5) / (num8 + 1);
                anchor += offset * iOff;

                //

                if (i != 5 || txt == "模组设置") return;

                float num4 = 0.7f;
                float num5 = 0.8f;
                float num6 = 0.01f;

                bool flag = GameCulture.FromCultureName(GameCulture.CultureName.Russian).IsActive || GameCulture.FromCultureName(GameCulture.CultureName.Portuguese).IsActive || GameCulture.FromCultureName(GameCulture.CultureName.Polish).IsActive || GameCulture.FromCultureName(GameCulture.CultureName.French).IsActive;
                if (flag)
                {
                    num4 = 0.4f;
                    num5 = 0.44f;
                }

                bool isActive2 = GameCulture.FromCultureName(GameCulture.CultureName.German).IsActive;
                if (isActive2)
                {
                    num4 = 0.55f;
                    num5 = 0.6f;
                }

                Vector2 start = anchor + offset * (i + iOff);
                if (DrawLeftSide(sb, "模组设置", 0, start, offset, scale, minscale, maxscale, scalespeed))
                {
                    scale[0] += num6;

                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        IngameOptions.Close();
                        ModSetSwitch.OpenModSetSwitchMenu(null, ContentPatch.GetModObjects());
                    }
                }
                else
                {
                    scale[0] -= num6;
                }

                if (scale[0] < num4) scale[0] = num4;
                else if (scale[0] > num5) scale[0] = num5;

                ++iOff;
                anchor += offset * iOff;
            }

            public static bool DrawLeftSide(SpriteBatch sb, string txt, int i, Vector2 anchor, Vector2 offset, float[] scales, float minscale = 0.7f, float maxscale = 0.8f, float scalespeed = 0.01f)
            {
                Color color = Color.Lerp(Color.Gray, Color.White, (scales[i] - minscale) / (maxscale - minscale));

                Vector2 vector = Terraria.Utils.DrawBorderStringBig(sb, txt, anchor + offset * (1 + i), color, scales[i], 0.5f, 0.5f);
                bool flag2 = new Rectangle((int)anchor.X - (int)vector.X / 2, (int)anchor.Y + (int)(offset.Y * (float)(1 + i)) - (int)vector.Y / 2, (int)vector.X, (int)vector.Y).Contains(new Point(Main.mouseX, Main.mouseY));

                return flag2;
            }
        }
    }
}
