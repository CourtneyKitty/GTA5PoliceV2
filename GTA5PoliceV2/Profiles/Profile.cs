using Discord;
using Newtonsoft.Json;
using System;
using System.IO;

namespace GTA5PoliceV2.Profiles
{
    class Profile
    {
        [JsonIgnore]
        public static readonly string appdir = AppContext.BaseDirectory;

        public string PlayerNAME { get; set; }
        public ulong PlayerID { get; set; }
        public string PlayerHEX { get; set; }
        public int SuccessfulWins { get; set; }
        public int SuccessfulDefends { get; set; }
        public int Attempts { get; set; }
        public int Revives { get; set; }
        public int Money { get; set; }

        public Profile()
        {
            PlayerNAME = "";
            PlayerID = 0l;
            PlayerHEX = "steam:";
            SuccessfulWins = 0;
            SuccessfulDefends = 0;
            Attempts = 0;
            Revives = 0;
            Money = 5000;
        }

        public void Save(ulong id)
        {
            string dir = "profiles/" + id + ".json";
            string file = Path.Combine(appdir, dir);
            File.WriteAllText(file, ToJson());
            Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Profiles", "Profile saved successfully."));
        }
        public static Profile Load(ulong id)
        {
            string dir = "profiles/" + id + ".json";
            string file = Path.Combine(appdir, dir);
            return JsonConvert.DeserializeObject<Profile>(File.ReadAllText(file));
            Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Profiles", "Profile loaded successfully."));
        }
        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}
