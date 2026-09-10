using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Shaders;

namespace DeadEffect.Shader
{
    internal class MyScreenShaderData : ScreenShaderData
    {
        protected EffectParameter<float> RotationAngle = null;

        public MyScreenShaderData(Asset<Effect> shader, string passName) : base(shader, passName)
        {
            RotationAngle = shader.Value.GetParameter<float>("RotationAngle");
        }

        public void SetRotationAngle(float val) => RotationAngle.SetValue(val);
    }
}
