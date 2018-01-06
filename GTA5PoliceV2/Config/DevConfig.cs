using Discord;
using Newtonsoft.Json;
using System;
using System.IO;

namespace GTA5PoliceV2.Config
{
    class DevConfig
    {
        [JsonIgnore]
        public static readonly string appdir = AppContext.BaseDirectory;
        
        public int Devs { get; set; }
        public ulong[] Developers { get; set; }

        public DevConfig()
        {
            Devs = 0;
            Developers = new ulong[15];
        }

        public void Save(string dir = "configuration/dev_config.json")
        {
            string file = Path.Combine(appdir, dir);
            File.WriteAllText(file, ToJson());
            Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police", "Dev configuration saved successfully."));
        }
        public static DevConfig Load(string dir = "configuration/dev_config.json")
        {
            string file = Path.Combine(appdir, dir);
            return JsonConvert.DeserializeObject<DevConfig>(File.ReadAllText(file));
            Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police", "Dev configuration loaded successfully."));
        }
        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}
