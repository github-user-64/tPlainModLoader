using System;
using Terraria;

namespace SuspiciousPlayer.Content
{
    public partial class Utils
    {
        public static int getRand(int v, int v2)//v=1,v2=2,return 1
        {
            return Main.rand.Next(Math.Min(v, v2), Math.Max(v, v2));
        }

        public static float getRandFloat()//0.9-0.1
        {
            return Main.rand.NextFloat();
        }
    }
}
