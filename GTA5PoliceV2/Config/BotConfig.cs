using Discord;
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
        public ulong TimerChannelId { get; set; }
        public int StatusTimerInterval { get; set; }
        public int MessageTimerInterval { get; set; }
        public int MessageTimerCooldown { get; set; }
        public double CommandCooldown { get; set; }
        public int Commanders { get; set; }
        public ulong[] BotCommanders { get; set; }
        public int Filters { get; set; }
        public string[] FilteredWords { get; set; }
        public bool PoliceAdd { get; set; }
        public bool EmsAdd { get; set; }

        public BotConfig()
        {
            Prefix = "!";
            Token = "";
            ServerId = 0;
            LogsId = 0;
            TimerChannelId = 0;
            StatusTimerInterval = 1;
            MessageTimerInterval = 30;
            MessageTimerCooldown = 5;
            CommandCooldown = 120.0d;
            Commanders = 1;
            BotCommanders = new ulong[10];
            Filters = 4;
            FilteredWords = new string[20];
            PoliceAdd = false;
            EmsAdd = false;
        }

        private const string save = "configuration/" + name + "config.json";
        public void Save(string dir = save)
        {
            string file = Path.Combine(appdir, dir);
            File.WriteAllText(file, ToJson());
            Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Configuration", "Main configuration saved successfully."));
        }
        public static BotConfig Load(string dir = "configuration/config.json")
        {
            string file = Path.Combine(appdir, dir);
            return JsonConvert.DeserializeObject<BotConfig>(File.ReadAllText(file));
            Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Configuration", "Main configuration loaded successfully."));
        }
        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}
