using Discord;
using Newtonsoft.Json;
using System;
using System.IO;

namespace GTA5PoliceV2.Config
{
    class AutoBans
    {
        [JsonIgnore]
        public static readonly string appdir = AppContext.BaseDirectory;

        public int Bans { get; set; }
        public ulong[] Offenders { get; set; }

        public AutoBans()
        {
            Bans = 0;
            Offenders = new ulong[15];
        }

        public void Save(string dir = "configuration/auto_bans.json")
        {
            string file = Path.Combine(appdir, dir);
            File.WriteAllText(file, ToJson());
            Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Configuration", "Auto bans saved successfully."));
        }
        public static AutoBans Load(string dir = "configuration/auto_bans.json")
        {
            string file = Path.Combine(appdir, dir);
            return JsonConvert.DeserializeObject<AutoBans>(File.ReadAllText(file));
            Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Configuration", "Auto bans loaded successfully."));
        }
        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}
