using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Effects;

namespace DeadEffect.Shader
{
    internal static class ShaderManager
    {
        public const string PassName = "FilterScreenRotation";

        public static void Load(IAssetRepository Asset)
        {
            Asset<Effect> ScreenShaderRef = Asset.Request<Effect>("MyScreenShader1");

            MyScreenShaderData shader = new MyScreenShaderData(ScreenShaderRef, PassName);

            Filter filter = new Filter(shader, EffectPriority.VeryHigh);
            filter.Load();
            Filters.Scene[PassName] = filter;
        }

        public static void Unload()
        {
            Filter filter = Filters.Scene[PassName];
            if (filter == null) return;

            Filters.Scene.Deactivate(PassName);
        }
    }
}
