using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;

namespace OptimizeAndTool.Content.ServerList
{
    public class ServerInfo : WorldFileData
    {
        public string DisplayText = null;
        public string IP = null;
        public int Port = 0;

        public static ServerInfo GetEmpty()
        {
            WorldFileData data = FromInvalidWorld(string.Empty, true, StatusID.EmptyFile, null);

            return WorldFileDataToServerInfo(data);
        }

        public static ServerInfo WorldFileDataToServerInfo(WorldFileData data)
        {
            string text = DataToString(data);

            return StringToData<ServerInfo>(text);
        }

        //

        public static void Save<T>(string path, T data)
        {
            File.WriteAllText(path, DataToString(data), Encoding.UTF8);
        }

        public static T Read<T>(string path)
        {
            return StringToData<T>(File.ReadAllText(path, Encoding.UTF8));
        }

        public static string DataToString<T>(T data)
        {
            return JsonConvert.SerializeObject(data, Formatting.Indented, GetJsonSettings());
        }

        public static T StringToData<T>(string text)
        {
            return JsonConvert.DeserializeObject<T>(text, GetJsonSettings());
        }

        protected static JsonSerializerSettings GetJsonSettings()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ContractResolver = new GetInstanceAll(),
            };
            settings.Converters.Add(new ConverterLocalizedText());

            return settings;
        }

        protected class GetInstanceAll : DefaultContractResolver
        {
            protected override List<MemberInfo> GetSerializableMembers(Type objectType)
            {
                BindingFlags f = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

                IEnumerable<MemberInfo> ms = objectType.GetFields(f);

                ms = ms.Concat(objectType.GetProperties(f).Where(i => i.CanWrite));

                return ms.ToList();
            }

            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                return base.CreateProperties(type, MemberSerialization.Fields);
            }
        }

        protected class ConverterLocalizedText : JsonConverter<LocalizedText>
        {
            public override LocalizedText ReadJson(JsonReader reader, Type objectType, LocalizedText existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                string key = serializer.Deserialize<string>(reader);

                if (key == null) return LocalizedText.Empty;
                if (key == LocalizedText.Empty.Key) return LocalizedText.Empty;
                if (Language.Exists(key) != true) return LocalizedText.Empty;

                return Language.GetText(key);
            }

            public override void WriteJson(JsonWriter writer, LocalizedText value, JsonSerializer serializer)
            {
                writer.WriteValue(value?.Key);
            }
        }
    }
}
