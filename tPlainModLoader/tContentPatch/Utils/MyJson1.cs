using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace tContentPatch.Utils
{
    /// <summary>
    /// Json的简易操作
    /// </summary>
    public static class MyJson1
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T">将数据转为该类型的对象</typeparam>
        /// <param name="FilePath1">文件路径</param>
        /// <returns>指定类型的对象</returns>
        /// <exception cref="Exception"></exception>
        public static T Get2<T>(string FilePath1)
        {
            try
            {
                if (!File.Exists(FilePath1)) throw new Exception($"文件不存在:{FilePath1}");

                return JsonConvert.DeserializeObject<T>(File.ReadAllText(FilePath1, Encoding.UTF8));
            }
            catch (Exception ex)
            {
                MethodBase mb = MethodBase.GetCurrentMethod();
                throw new Exception($"{mb?.DeclaringType.Name}.{mb?.Name}:{ex.Message}");
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="FilePath1">文件路径</param>
        /// <param name="type">数据类型</param>
        /// <returns>指定类型的对象</returns>
        /// <exception cref="Exception"></exception>
        public static object Get2(string FilePath1, Type type)
        {
            try
            {
                if (!File.Exists(FilePath1)) throw new Exception($"文件不存在:{FilePath1}");

                return JsonConvert.DeserializeObject(File.ReadAllText(FilePath1, Encoding.UTF8), type);
            }
            catch (Exception ex)
            {
                MethodBase mb = MethodBase.GetCurrentMethod();
                throw new Exception($"{mb?.DeclaringType.Name}.{mb?.Name}:{ex.Message}");
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="val">要保存的对象</param>
        /// <param name="FilePath1">文件路径</param>
        /// <exception cref="Exception"></exception>
        public static void Save(object val, string FilePath1)
        {
            try
            {
                string directory = Path.GetDirectoryName(FilePath1);

                if (!Directory.Exists(directory)) throw new Exception($"目录不存在:{directory}");

                File.WriteAllText(FilePath1, JsonConvert.SerializeObject(val), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MethodBase mb = MethodBase.GetCurrentMethod();
                throw new Exception($"{mb?.DeclaringType.Name}.{mb?.Name}:{ex.Message}");
            }
        }

        /// <summary>
        /// 字符串转对象
        /// </summary>
        /// <typeparam name="T">转换到指定类型</typeparam>
        /// <param name="string1">被转化的字符串</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T StringToObject<T>(string string1)
        {
            try
            {
                JObject jObject1 = JObject.Parse(string1.ToString());

                return JsonConvert.DeserializeObject<T>(jObject1.ToString());
            }
            catch (Exception ex)
            {
                MethodBase mb = MethodBase.GetCurrentMethod();
                throw new Exception($"{mb?.DeclaringType.Name}.{mb?.Name}:{ex.Message}");
            }
        }
    }
}
