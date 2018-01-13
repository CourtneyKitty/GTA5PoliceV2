using Discord;
using GTA5PoliceV2.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GTA5PoliceV2.Util
{
    class StatusChange
    {
        private static int i;
        private static Random rnd = new Random();

        public static async Task CycleGameAsync()
        {
            i = rnd.Next(0, 5);

            if (i == 0) { await CommandHandler.GetBot().SetGameAsync("GTA5Police.com"); }
            else if (i == 1) { await CommandHandler.GetBot().SetGameAsync(BotConfig.Load().Prefix + "apply"); }
            else if (i == 2) { await CommandHandler.GetBot().SetGameAsync(BotConfig.Load().Prefix + "rules"); }
            else if (i == 3) { await CommandHandler.GetBot().SetGameAsync("Teamspeak: GTA5Police.com"); }
            else if (i == 4) { await CommandHandler.GetBot().SetGameAsync("discord.me/gta5police"); }
            else
            {
                await Program.Logger(new LogMessage(LogSeverity.Info, "Status Changer", "Random int not 0 - 4."));
                i = 1;
                await CommandHandler.GetBot().SetGameAsync("GTA5Police.com");
            }
        }
    }
}
