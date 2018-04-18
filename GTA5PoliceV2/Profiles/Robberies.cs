using Discord;
using Newtonsoft.Json;
using System;
using System.IO;

namespace GTA5PoliceV2.Profiles
{
    class Robberies
    {
        [JsonIgnore]
        public static readonly string appdir = AppContext.BaseDirectory;

        public ulong RobberyChannel { get; set; }
        public string LeaderCriminal { get; set; }
        public int LeaderCriminalPoints { get; set; }
        public string LeaderPolice { get; set; }
        public int LeaderPolicePoints { get; set; }
        public string LeaderEMS { get; set; }
        public int LeaderEMSPoints { get; set; }

        public Robberies()
        {
            RobberyChannel = 0;
            LeaderCriminal = "Blurr";
            LeaderCriminalPoints = 10;
            LeaderPolice = "Blurr";
            LeaderPolicePoints = 10;
            LeaderEMS = "Blurr";
            LeaderEMSPoints = 10;
        }

        public void Save()
        {
            string dir = "profiles/statistics.json";
            string file = Path.Combine(appdir, dir);
            File.WriteAllText(file, ToJson());
            Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Profiles", "Profile saved successfully."));
        }
        public static Profile Load()
        {
            string dir = "profiles/statistics.json";
            string file = Path.Combine(appdir, dir);
            return JsonConvert.DeserializeObject<Profile>(File.ReadAllText(file));
            Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Profiles", "Profile loaded successfully."));
        }
        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}
