using DeadEffect.Audio;
using Microsoft.Xna.Framework.Audio;
using tContentPatch.Utils;

namespace DeadEffect
{
    internal static class Resources
    {
        internal static class Sounds
        {
            public static readonly MySoundStyle Long = new MySoundStyle(Load<SoundEffect>("Sounds.sound_long.wav"), 3);
            public static readonly MySoundStyle Short = new MySoundStyle(Load<SoundEffect>("Sounds.sound_short.wav"), 3);
            public static readonly MySoundStyle End = new MySoundStyle(Load<SoundEffect>("Sounds.sound_end.wav"), 3);
        }

        private static T Load<T>(string path)
        {
            return AssemblyResource.Load<T>($"DeadEffect.Resources.{path}", ThisMod.mo.assembly);
        }
    }
}
