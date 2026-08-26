using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BatchSapwn
{
    internal class SpawnConfig
    {
        public delegate void ForMod(string form, string to, string name);

        public string SetVersionPath = "C:\\";
        public string form = "C:\\";
        public string to = "C:\\";
        public List<string> mods = new List<string>();

        public void Check()
        {
            if (SetVersionPath == null) throw new Exception($"{nameof(SetVersionPath)}为null");
            if (form == null) throw new Exception($"{nameof(form)}为null");
            if (to == null) throw new Exception($"{nameof(to)}为null");
            if (mods == null) throw new Exception($"{nameof(mods)}为null");
        }

        public void For(ForMod action)
        {
            if (action == null) return;
            if (mods == null) return;

            mods.ForEach(mod => action(form, to, mod));
        }

        public static SpawnConfig TryLoad(string FilePath1)
        {
            try
            {
                return Get2(FilePath1);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

            try
            {
                Save(new SpawnConfig(), FilePath1, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }

        public static SpawnConfig Get2(string FilePath1)
        {
            try
            {
                if (!File.Exists(FilePath1)) throw new Exception($"文件不存在:{FilePath1}");

                return JsonConvert.DeserializeObject<SpawnConfig>(File.ReadAllText(FilePath1, Encoding.UTF8));
            }
            catch (Exception ex)
            {
                MethodBase mb = MethodBase.GetCurrentMethod();
                throw new Exception($"{mb?.DeclaringType.Name}.{mb?.Name}:{ex.Message}");
            }
        }

        public static void Save(object val, string FilePath1, bool indented = false)
        {
            try
            {
                string directory = Path.GetDirectoryName(FilePath1);

                if (!Directory.Exists(directory)) throw new Exception($"目录不存在:{directory}");

                string text = null;
                if (indented) text = JsonConvert.SerializeObject(val, Formatting.Indented);
                else text = JsonConvert.SerializeObject(val);

                File.WriteAllText(FilePath1, text, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MethodBase mb = MethodBase.GetCurrentMethod();
                throw new Exception($"{mb?.DeclaringType.Name}.{mb?.Name}:{ex.Message}");
            }
        }
    }
}
