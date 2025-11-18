using CommandHelp;
using System;
using System.Collections.Generic;
using tContentPatch;
using Terraria;

namespace SundryTool.Content
{
    internal class ClientUUIDSetting : ModSetting
    {
        public override string FilePath => "clientUUID.txt";
        public override bool HasUI => false;
        public override Type DataType => typeof(string);
        public static ClientUUIDSetting instance = null;

        public override void Load(object v)
        {
            if (Function1.Function.NoPublic) return;

            instance = this;

            if (v is string uuid)
            {
                SetClientUUID(uuid);
            }
            else
            {
                NeedSave = true;
                Save();
            }
        }

        public override object GetSaveData() => Main.clientUUID;

        public static void SetClientUUID(string uuid)
        {
            if (uuid == null)
            {
                Console.WriteLine("uuid is [null]");
                return;
            }

            Main.clientUUID = uuid;
            PrintClientUUID();
        }

        public static void UpdateClientUUID()
        {
            SetClientUUID(instance?.Read() as string);
        }

        public static void PrintClientUUID()
        {
            Console.WriteLine($"[{Main.clientUUID}]");
            Main.NewText($"[{Main.clientUUID}]");
        }

        public static List<CommandObject> GetCO()
        {
            CommandObject uuid = new CommandObject("uuid");
            CommandMethod update = new CommandMethod("update");
            CommandMethod get = new CommandMethod("get");

            uuid.SubCommand.Add(tContentPatch.Command.Utils.GetCO_OutputCOList(uuid.SubCommand));
            uuid.SubCommand.Add(update);
            uuid.SubCommand.Add(get);

            update.Runing += _ => UpdateClientUUID();

            get.Runing += _ => PrintClientUUID();

            return new List<CommandObject>() { uuid };
        }
    }
}
