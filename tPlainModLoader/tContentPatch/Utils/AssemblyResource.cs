using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace tContentPatch.Utils
{
    /// <summary>
    /// 从程序集加载资源
    /// </summary>
    public static class AssemblyResource
    {
        /// <param name="path">加载路径</param>
        /// <param name="assembly">从程序集加载</param>
        /// <returns>资源</returns>
        public delegate object LoadAsset(string path, Assembly assembly);

        private readonly static object _lock = new object();

        private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
        private static readonly Dictionary<string, object> Assets = new Dictionary<string, object>();
        private static readonly Dictionary<Type, LoadAsset> LoadType = new Dictionary<Type, LoadAsset>();

        static AssemblyResource()
        {
            Register(typeof(Texture2D), GetTexture2D);
            Register(typeof(SoundEffect), GetSoundEffect);
        }

        internal static void Reset()
        {
            lock (_lock)
            {
                Assets.Clear();
                LoadType.Clear();

                Register(typeof(Texture2D), GetTexture2D);
                Register(typeof(SoundEffect), GetSoundEffect);
            }
        }

        /// <summary>
        /// 注册<paramref name="type"/>类型的资源用<paramref name="load"/>的方式加载
        /// </summary>
        /// <param name="type">加载类型</param>
        /// <param name="load">加载方式</param>
        public static void Register(Type type, LoadAsset load)
        {
            lock (_lock)
            {
                if (LoadType.ContainsKey(type)) return;

                LoadType.Add(type, load);
            }
        }

        /// <summary>
        /// 从指定程序集加载资源&lt;资源类型&gt;("程序集名.图片.png", 程序集)//重复加载得到的是同一个资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">资源路径</param>
        /// <param name="assembly">程序集</param>
        /// <returns>资源</returns>
        public static T Load<T>(string path, Assembly assembly = null)
        {
            lock (_lock)
            {
                if (Assets.ContainsKey(path)) return (T)Assets[path];
                if (assembly == null) assembly = Assembly;

                object asset = LoadType[typeof(T)](path, assembly);
                Assets.Add(path, asset);
            }

            return (T)Assets[path];
        }

        /// <summary>
        /// 加载新的资源&lt;资源类型&gt;("程序集名.图片.png", 程序集)
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="path">资源路径</param>
        /// <param name="assembly">程序集</param>
        /// <returns>资源</returns>
        public static T LoadNew<T>(string path, Assembly assembly = null)
        {
            if (assembly == null) assembly = Assembly;

            object asset = LoadType[typeof(T)](path, assembly);

            return (T)asset;
        }

        private static Texture2D GetTexture2D(string path, Assembly assembly)
        {
            Stream stream = assembly.GetManifestResourceStream(path);
            return Texture2D.FromStream(Terraria.Main.graphics.GraphicsDevice, stream);
        }

        private static SoundEffect GetSoundEffect(string path, Assembly assembly)
        {
            Stream stream = assembly.GetManifestResourceStream(path);
            return SoundEffect.FromStream(stream);
        }
    }
}
