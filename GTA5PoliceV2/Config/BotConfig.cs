using Newtonsoft.Json;
using System;
using System.IO;

namespace GTA5PoliceV2.Config
{
    class BotConfig
    {
        [JsonIgnore]
        public static readonly string appdir = AppContext.BaseDirectory;

        public string Prefix { get; set; }
        public string Token { get; set; }
        public ulong ServerId { get; set; }
        public ulong LogsId { get; set; }
        public int StatusTimerInterval { get; set; }
        public int MessageTimerInterval { get; set; }

        public BotConfig()
        {
            Prefix = "!";
            Token = "";
            ServerId = 0;
            LogsId = 0;
            StatusTimerInterval = 1;
            MessageTimerInterval = 30;
        }

        public void Save(string dir = "configuration/config.json")
        {
            string file = Path.Combine(appdir, dir);
            File.WriteAllText(file, ToJson());
        }
        public static BotConfig Load(string dir = "configuration/config.json")
        {
            string file = Path.Combine(appdir, dir);
            return JsonConvert.DeserializeObject<BotConfig>(File.ReadAllText(file));
        }
        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}
