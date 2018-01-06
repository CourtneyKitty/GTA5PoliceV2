using Discord.WebSocket;
using GTA5PoliceV2.Config;
using GTA5PoliceV2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA5PoliceV2.Profanity
{
    class ProfanityFilter
    {
        public static async Task ProfanityCheckAsync(SocketMessage pMsg)
        {
            var message = pMsg as SocketUserMessage;
            if (message == null)
                return;

            for (int i = 0; i <= BotConfig.Load().Filters - 1; i++)
            {
                if (message.ToString().ToLower().Contains(BotConfig.Load().FilteredWords[i].ToLower()))
                {
                    await ProfanityMessage.WarningMessageAsync(pMsg, BotConfig.Load().FilteredWords[i]);
                    await ProfanityMessage.LogMessageAsync(CommandHandler.GetBot(), pMsg, BotConfig.Load().FilteredWords[i]);
                    await ProfanityMessage.DMMessageAsync(CommandHandler.GetBot(), pMsg, BotConfig.Load().FilteredWords[i]);
                    await ProfanityBanAsync(CommandHandler.GetBot(), pMsg);
                    Statistics.AddProfanityDetected();
                }
            }
        }

        public static async Task ProfanityBanAsync(DiscordSocketClient bot, SocketMessage pMsg)
        {
            var server = bot.Guilds.FirstOrDefault(x => x.Id == BotConfig.Load().ServerId);
            await server.AddBanAsync(pMsg.Author, 7, "Profanity detected in discord chat. Check server logs for more information.");
        }
    }
}
