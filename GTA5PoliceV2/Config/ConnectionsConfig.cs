using Discord;
using Newtonsoft.Json;
using System;
using System.IO;

namespace GTA5PoliceV2.Config
{
    class ConnectionsConfig
    {
        [JsonIgnore]
        public static readonly string appdir = AppContext.BaseDirectory;

        public string ServerIp { get; set; }
        public int NyPort { get; set; }
        public int LaPort { get; set; }
        public int NyWlPort { get; set; }
        public int LaWlPort { get; set; }

        public ConnectionsConfig()
        {
            ServerIp = "";
            NyPort = 30150;
            LaPort = 30141;
            NyWlPort = 30151;
            LaWlPort = 30142;
        }

        public void Save(string dir = "configuration/connections_config.json")
        {
            string file = Path.Combine(appdir, dir);
            File.WriteAllText(file, ToJson());
            Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police", "Connections configuration saved successfully."));
        }
        public static ConnectionsConfig Load(string dir = "configuration/connections_config.json")
        {
            string file = Path.Combine(appdir, dir);
            return JsonConvert.DeserializeObject<ConnectionsConfig>(File.ReadAllText(file));
            Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police", "Connections configuration loaded successfully."));
        }
        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
    }  
}
