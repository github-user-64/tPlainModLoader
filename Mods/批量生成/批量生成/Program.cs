using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace BatchSapwn
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "生成配置.txt");

                SpawnConfig config = SpawnConfig.TryLoad(path);
                if (config == null) throw new Exception("配置为null");
                config.Check();

                Console.WriteLine($"从:{config.form}");
                Console.WriteLine($"到:{config.to}");
                for (int i = 0; i < config.mods.Count; ++i)
                {
                    Console.WriteLine($"{i}:{config.mods[i]}");
                }

                SwitchAction(config);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("ok");
            Console.ReadLine();
        }

        private static void SwitchAction(SpawnConfig config)
        {
            while (true)
            {
                Console.WriteLine("选择操作(0:生成,1:修改版本)");
                string v = Console.ReadLine();

                switch (v)
                {
                    case "0": ActionSpawn(config); return;
                    case "1": ActionSetVersion(config); return;
                    default: break;
                }
            }
        }

        private static void ActionSpawn(SpawnConfig config)
        {
            Console.WriteLine("开始生成");
            Spawn(config);
        }

        private static void ActionSetVersion(SpawnConfig config)
        {
            Console.WriteLine("现在版本:");
            string v1 = Console.ReadLine();
            Console.WriteLine("设置版本:");
            string v2 = Console.ReadLine();

            if (v1 == null || v2 == null) throw new Exception("版本为null");
            if (v1 == v2) throw new Exception("版本相同");

            SetVersion(config, v1, v2);
        }

        private static void Spawn(SpawnConfig config)
        {
            config.For((form, to, name) =>
            {
                form = Path.Combine(form, name);
                to = Path.Combine(to, name);

                Directory.CreateDirectory(to);

                CopyFile(form, to, "info.json");
                CopyFile(form, to, "loadConfig.json");
                CopyFile(form, to, "ico.png");
                CopyFile(form, to, $"{name}.dll");
            });
        }

        private static void CopyFile(string form, string to, string file)
        {
            form = Path.Combine(form, file);
            to = Path.Combine(to, file);

            if (File.Exists(form) == false)
            {
                Console.WriteLine($"文件不存在, 跳过[{form}]");
                return;
            }
            File.Copy(form, to);
        }

        private static void SetVersion(SpawnConfig config, string v1, string v2)
        {
            config.mods.ForEach(name =>
            {
                Console.WriteLine($"设置:{name}");

                string file = $"{Path.Combine(config.SetVersionPath, name, name, name, $"{name}.csproj")}";
                if (File.Exists(file) == false) throw new Exception($"文件不存在:{file}");

                SetProjectVersion(file, v1, v2);
            });
        }

        private static void SetProjectVersion(string file, string v1, string v2)
        {
            XDocument doc = XDocument.Load(file);
            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";//MSBuild命名空间

            XElement Reference = doc.Descendants(ns + "Reference")
                .FirstOrDefault(r => r.Attribute("Include")?.Value == v1);

            if (Reference == null) throw new Exception($"未找到引用:{v1}");

            Reference.Attribute("Include").Value = v2;

            XElement HintPath = Reference.Element(ns + "HintPath");
            HintPath.Value = HintPath.Value.Replace(v1, v2);

            doc.Save(file);
        }
    }
}
