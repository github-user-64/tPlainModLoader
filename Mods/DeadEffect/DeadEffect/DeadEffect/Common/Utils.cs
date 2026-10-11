using Microsoft.Xna.Framework.Input;
using Terraria;

namespace DeadEffect.Common
{
    public static class Utils
    {
        public static bool IsKeyClick(Keys key)
        {
            return Main.keyState.IsKeyDown(key) == true && Main.oldKeyState.IsKeyDown(key) == false;
        }
    }
}
