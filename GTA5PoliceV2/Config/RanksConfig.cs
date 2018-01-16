using Discord;
using Newtonsoft.Json;
using System;
using System.IO;

namespace GTA5PoliceV2.Config
{
    class RanksConfig
    {
        [JsonIgnore]
        public static readonly string appdir = AppContext.BaseDirectory;

        public int EMSHighRanks { get; set; }
        public string[] EMSHighRanksArray { get; set; }
        public int PDHighRanks { get; set; }
        public string[] PDHighRanksArray { get; set; }

        public RanksConfig()
        {
            EMSHighRanks = 2;
            EMSHighRanksArray = new string[2];
            PDHighRanks = 5;
            PDHighRanksArray = new string[5];
        }

        public void Save(string dir = "configuration/ranks_config.json")
        {
            string file = Path.Combine(appdir, dir);
            File.WriteAllText(file, ToJson());
            Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Configuration", "Ranks configuration saved successfully."));
        }
        public static RanksConfig Load(string dir = "configuration/ranks_config.json")
        {
            string file = Path.Combine(appdir, dir);
            return JsonConvert.DeserializeObject<RanksConfig>(File.ReadAllText(file));
            Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Configuration", "Ranks configuration loaded successfully."));
        }
        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}
