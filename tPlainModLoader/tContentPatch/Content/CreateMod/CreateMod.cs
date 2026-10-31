using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using tContentPatch.ModLoad;

namespace tContentPatch.Content
{
    internal static class CreateMod
    {
        public static readonly HashSet<string> TextExtension = new HashSet<string>()
        {
            ".slnx",
            ".csproj",
            ".user",
            ".cs",
            ".targets",
        };

        public static string TryCreate(string displayName, string modName, string fullPath)
        {
            try
            {
                Check(ref displayName, ref modName, ref fullPath);
                ContentPatch.PrintTry($"{nameof(CreateMod)}:创建路径:{fullPath}");

                if (Directory.Exists(fullPath))
                {
                    if (Directory.GetDirectories(fullPath).Length > 0) throw new Exception("目录不为空");
                    if (Directory.GetFiles(fullPath).Length > 0) throw new Exception("目录不为空");
                }
                else
                {
                    Directory.CreateDirectory(fullPath);
                }

                CreateFile(displayName, modName, fullPath);

                return fullPath;
            }
            catch (Exception ex)
            {
                ContentPatch.PrintTry($"{nameof(CreateMod)}:创建失败:{ex.Message}");
                return null;
            }
        }

        /// <exception cref="Exception"></exception>
        public static void Check(ref string displayName, ref string modName, ref string fullPath)
        {
            if (string.IsNullOrEmpty(displayName)) displayName = null;
            if (string.IsNullOrEmpty(modName)) throw new ArgumentNullException(nameof(modName));
            if (string.IsNullOrEmpty(fullPath)) throw new ArgumentNullException(nameof(fullPath));

            modName = modName.Trim();
            fullPath = Path.Combine(fullPath.Trim(), modName);
            fullPath = Path.GetFullPath(fullPath);

            CodeDomProvider cdp = CodeDomProvider.CreateProvider("C#");

            if (cdp.IsValidIdentifier(modName) != true) throw new Exception($"名称不可用:[{modName}]");
        }

        /// <exception cref="Exception"></exception>
        public static void CreateFile(string displayName, string modName, string fullPath)
        {
            Dictionary<string, string> kvs = GetFormat(modName);

            Assembly assembly = typeof(CreateMod).Assembly;
            string namePr = $"{typeof(CreateMod).FullName}.Files.";
            string[] names = assembly.GetManifestResourceNames();

            for (int i = 0; i < names.Length; ++i)
            {
                string name = names[i];
                if (name.StartsWith(namePr) != true) continue;

                name = names[i].Substring(namePr.Length, name.Length - namePr.Length);
                name = Format(name, kvs, "{{", "}}");

                string file = Path.Combine(fullPath, name);
                ContentPatch.PrintTry($"{nameof(CreateMod)}:复制文件到:{file}");

                using (Stream stream = assembly.GetManifestResourceStream(names[i]))
                {
                    if (TextExtension.Contains(Path.GetExtension(name)))
                    {
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            string content = sr.ReadToEnd();
                            content = Format(content, kvs, "{{", "}}");
                            File.WriteAllText(file, content);
                        }
                    }
                    else
                    {
                        using (FileStream fs = File.Create(file))
                        {
                            stream.CopyTo(fs);
                        }
                    }
                }
            }

            //

            ModConfig config = new ModConfig();
            config.key = modName;
            config.version = "1.0";
            config.isEnable = true;
            config.dllPath = $"{modName}.dll";
            Utils.MyJson1.Save(config, Path.Combine(fullPath, InfoList.Files.ModLoadConfig));

            ModInfo info = new ModInfo();
            info.name = displayName;
            Utils.MyJson1.Save(info, Path.Combine(fullPath, InfoList.Files.ModInfo));
        }

        /// <exception cref="Exception"></exception>
        public static Dictionary<string, string> GetFormat(string modName)
        {
            Dictionary<string, string> kvs = new Dictionary<string, string>();
            kvs.Add("ModName", modName);
            kvs.Add("DllTerraria", typeof(Terraria.Main).Assembly.Location);
            kvs.Add("tPMLDllPath", Path.GetDirectoryName(typeof(ContentPatch).Assembly.Location));

            return kvs;
        }

        public static string Format(string s, Dictionary<string, string> kvs, string left, string right)
        {
            List<string> ss = new List<string>();

            int index = 0;
            while (index < s.Length)
            {
                int index1 = s.IndexOf(left, index);
                if (index1 < 0)
                {
                    ss.Add(GetRange(s, index, s.Length - 1));
                    break;
                }
                ss.Add(GetRange(s, index, index1 - 1));
                index1 += left.Length;

                int index2 = s.IndexOf(right, index1);
                if (index2 < 0) throw new Exception($"未找到right:{right}");
                index2 -= 1;

                string key = GetRange(s, index1, index2);
                if (kvs.TryGetValue(key, out string val) != true) throw new Exception($"找不到{key}的值");

                index = index2 + right.Length + 1;
                ss.Add(val);
            }

            StringBuilder sb = new StringBuilder();
            foreach (string i in ss) sb.Append(i);

            return sb.ToString();
        }

        private static string GetRange(string s, int index1, int index2)
        {
            int len = index2 + 1 - index1;
            if (len < 1) return string.Empty;

            return s.Substring(index1, len);
        }
    }
}