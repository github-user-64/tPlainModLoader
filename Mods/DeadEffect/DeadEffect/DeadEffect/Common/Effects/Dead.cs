using DeadEffect.Shader;
using Microsoft.Xna.Framework;
using tContentPatch;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;

namespace DeadEffect.Common.Effects
{
    internal class Dead : PatchMain
    {
        protected ValChange vc = new ValChange(0.5f, 0.2f);

        public override void Initialize()
        {
            TriggerEffect.TriggerDead += OnDead;
        }

        protected void OnDead(bool content, bool sound, Player This, PlayerDeathReason damageSource, double dmg, int hitDirection, bool pvp)
        {
            if (This != Main.LocalPlayer) return;

            if (content) vc.Set(10);
            if (sound) SoundEngine.PlayTrackedSound(Resources.Sounds.End, This.Center);
        }

        public override void UpdatePostfix(GameTime gameTime)
        {
            if (Main.dedServ == true) return;

            Filter filter = Filters.Scene[ShaderManager.PassName];
            if (filter == null) return;

            if (Main.LocalPlayer.dead == false ||
                Main.LocalPlayer.active != true ||
                vc.MaxVal <= 0)
            {
                vc.Set(0);

                if (filter.IsActive() != true) return;

                Filters.Scene.Deactivate(ShaderManager.PassName);
                (filter.GetShader() as MyScreenShaderData)?.SetRotationAngle(0);

                return;
            }

            vc.Update();

            (filter.GetShader() as MyScreenShaderData)?.SetRotationAngle(MathHelper.TwoPi / 360f * vc.NowVal);
            Filters.Scene.Activate(ShaderManager.PassName);
        }
    }
}
