using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using tContentPatch.ModLoad;

namespace tContentPatch.Utils
{
    public static class ModFile
    {
        /// <summary>
        /// 在该模组的文件夹下保存文件, 目录不存在则创建
        /// </summary>
        /// <param name="file">文件相对位置</param>
        /// <param name="save"></param>
        /// <param name="mo">为<see langword="null"/>时从已加载的模组中匹配调用该方法的模组的路径</param>
        public static bool SaveFileTry(string file, Func<string, bool> save, ModObject mo = null)
        {
            try
            {
                if (file == null) return false;
                if (save == null) return false;

                if (mo == null)
                {
                    MethodBase method = new StackTrace().GetFrame(1).GetMethod();
                    Assembly assembly = method.ReflectedType.Assembly;
                    mo = LoaderControl.GetModObjects()?.FirstOrDefault(i => i.assembly == assembly);
                    if (mo == null) return false;
                }

                //

                if (Directory.Exists(mo.modPath) == false) return false;

                file = Path.Combine(mo.modPath, file);
                string fileName = Path.GetFileName(file);
                string path = Path.GetDirectoryName(file);

                if (fileName == null) return false;
                fileName = fileName.Trim();
                if (fileName == string.Empty) return false;

                if (Directory.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);
                    if (Directory.Exists(path) == false) return false;
                }

                return save(file);
            }
            catch { return false; }
        }

        /// <summary>
        /// 在该模组的文件夹下读取文件
        /// </summary>
        /// <param name="file">文件相对位置</param>
        /// <param name="read"></param>
        /// <param name="mo">为<see langword="null"/>时从已加载的模组中匹配调用该方法的模组的路径</param>
        public static bool ReadFileTry(string file, Func<string, bool> read, ModObject mo = null)
        {
            try
            {
                if (file == null) return false;
                if (read == null) return false;

                if (mo == null)
                {
                    MethodBase method = new StackTrace().GetFrame(1).GetMethod();
                    Assembly assembly = method.ReflectedType.Assembly;
                    mo = LoaderControl.GetModObjects()?.FirstOrDefault(i => i.assembly == assembly);
                    if (mo == null) return false;
                }

                //

                file = Path.Combine(mo.modPath, file);

                if (File.Exists(file) == false) return false;

                return read(file);
            }
            catch { return false; }
        }
    }
}
