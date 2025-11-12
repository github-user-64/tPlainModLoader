using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tContentPatch.Utils;

namespace tContentPatch.ModLoad
{
    internal class LoaderControl
    {
        public static Action<IModLoader> OnModLoad_Start = null;
        public static Action OnModLoad_Ok = null;
        public static Action<IModLoader> OnModLoad_Cancel = null;
        public static Action OnModLoad_Canceled = null;
        public static Action<Exception> OnModLoad_Exception = null;
        public static Action<Exception> OnModUnload_Exception = null;

        private static List<ModObject> loadedMod = null;
        private static IModLoader modLoader = null;

        internal static void SetModLoader(IModLoader modLoader, Intercept intercept)
        {
            intercept.OnLoaded += mos => loadedMod = mos;

            Intercept ml = new Intercept(modLoader);
            ml.OnLoadException += ex => loadedMod = null;

            LoaderControl.modLoader = ml;
        }

        /// <summary>
        /// 获取加载的模组, 加载失败时为null, 需要修改内容建议使用返回复制内容的<see cref="ContentPatch.GetModObjects"/>
        /// </summary>
        /// <returns></returns>
        internal static List<ModObject> GetModObjects()
        {
            return loadedMod?.ToList();
        }

        /// <summary>
        /// 已加载过再调用会先卸载
        /// </summary>
        internal static void Load()
        {
            Log.Add($"{nameof(LoaderControl)}:加载模组");
            Console.Clear();
            Console.WriteLine("加载模组");

            OnModLoad_Start?.Invoke(modLoader);

            Task.Run(() =>
            {
                try
                {
                    if (loadedMod != null)
                        if (Unload() == false) return;
                    loadedMod = null;

                    _ = modLoader.Load();

                    Log.Add($"{nameof(LoaderControl)}:加载模组成功");
                    Console.WriteLine("加载完成");
                    OnModLoad_Ok?.Invoke();//成功
                }
                catch (TaskCanceledException)
                {
                    Log.Add($"{nameof(LoaderControl)}:加载模组取消");
                    Console.WriteLine("加载取消");

                    OnModLoad_Cancel?.Invoke(modLoader);//取消

                    if (Unload() == false) return;

                    OnModLoad_Canceled?.Invoke();
                }
                catch (Exception ex)
                {
                    Log.Add($"{nameof(LoaderControl)}:加载模组失败:{ex}");
                    Console.WriteLine("加载失败");

                    OnModLoad_Exception?.Invoke(ex);//失败

                    if (Unload() == false) return;
                }
            });
        }

        internal static void CancelLoad()
        {
            Log.Add($"{nameof(LoaderControl)}:取消加载模组");
            Console.Clear();
            Console.WriteLine("取消加载");

            modLoader.CancelLoad();
        }

        private static bool Unload()
        {
            try
            {
                Log.Add($"{nameof(LoaderControl)}:卸载模组");
                Console.WriteLine("卸载");

                loadedMod = null;
                modLoader.Unload();

                Log.Add($"{nameof(LoaderControl)}:卸载模组完成");
                Console.WriteLine("卸载完成");

                return true;
            }
            catch (Exception ex)
            {
                Log.Add($"{nameof(LoaderControl)}:卸载失败:{ex}");
                Console.WriteLine("卸载失败");

                OnModUnload_Exception?.Invoke(ex);

                return false;
            }
        }
    }
}
