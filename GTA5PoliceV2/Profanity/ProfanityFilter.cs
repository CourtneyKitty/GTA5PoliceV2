using Discord;
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
            var user = pMsg.Author;
            if (message == null)
                return;

            if (message.ToString().ToLower().Contains("where rp") || message.ToString().ToLower().Contains("wheres rp") || message.ToString().ToLower().Contains("admen"))
            {
                await Program.Logger(new LogMessage(LogSeverity.Critical, "NEWB", "ADMEN NEEDED!"));
                var embed = new EmbedBuilder() { Color = Colours.adminCol };
                embed.WithAuthor(user.Username.ToString(), user.GetAvatarUrl());
                embed.WithTitle("ADMEN WHERE RP");
                embed.WithDescription("RP is **everywhere!**");
                embed.WithCurrentTimestamp();
                await message.Channel.SendMessageAsync("", false, embed);
                Statistics.AddOutgoingMessages();
            }

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
