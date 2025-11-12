using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace tContentPatch.Utils
{
    public static class Resource
    {
        /// <summary>
        /// 在指定程序集中加载资源
        /// </summary>
        /// <param name="path"></param>
        /// <param name="assembly">为<see langword="null"/>时从调用该方法的程序集中加载</param>
        /// <returns></returns>
        public static Texture2D GetTexture2D(string path, Assembly assembly = null)
        {
            if (assembly == null)
            {
                MethodBase method = new StackTrace().GetFrame(1).GetMethod();
                assembly = method.ReflectedType.Assembly;
            }

            Stream stream = assembly.GetManifestResourceStream(path);

            if (stream == null) return null;

            return Texture2D.FromStream(Terraria.Main.graphics.GraphicsDevice, stream);
        }
    }
}
