using Discord;
using Newtonsoft.Json;
using System;
using System.IO;

namespace GTA5PoliceV2.Config
{
    class UrlConfig
    {
        [JsonIgnore]
        public static readonly string appdir = AppContext.BaseDirectory;

        public string Website { get; set; }
        public string Dashboard { get; set; }
        public string Forums { get; set; }
        public string Support { get; set; }
        public string Suggestions { get; set; }
        public string Donate { get; set; }
        public string Vacbanned { get; set; }

        public string Applications { get; set; }
        public string Whitelist { get; set; }
        public string Police { get; set; }
        public string EMS { get; set; }
        public string Mechanic { get; set; }
        public string Taxi { get; set; }
        public string Stream { get; set; }

        public string Logo { get; set; }
        public string Rules { get; set; }
        public string HowWeBan { get; set; }
        public string ClearCache { get; set; }

        public UrlConfig()
        {
            Website = "";
            Dashboard = "";
            Forums = "";
            Support = "";
            Suggestions = "";
            Donate = "";
            Vacbanned = "";

            Applications = "";
            Whitelist = "";
            Police = "";
            EMS = "";
            Mechanic = "";
            Taxi = "";
            Stream = "";

            Logo = "";
            Rules = "";
            HowWeBan = "";
            ClearCache = "";
        }

        public void Save(string dir = "configuration/url_config.json")
        {
            string file = Path.Combine(appdir, dir);
            File.WriteAllText(file, ToJson());
            Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police", "URL configuration saved successfully."));
        }
        public static UrlConfig Load(string dir = "configuration/url_config.json")
        {
            string file = Path.Combine(appdir, dir);
            return JsonConvert.DeserializeObject<UrlConfig>(File.ReadAllText(file));
            Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police", "URL configuration loaded successfully."));
        }
        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
    }  
}
