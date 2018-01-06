using Discord;
using Discord.Commands;
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
    class ProfanityMessage
    {
        public static async Task WarningMessageAsync(SocketMessage pMsg, string word)
        {
            var message = pMsg as SocketUserMessage;
            await message.DeleteAsync();
            var user = pMsg.Author;

            var warningEmbed = new EmbedBuilder() { Color = Colours.errorCol };
            warningEmbed.WithAuthor("Profanity Detected!", References.gta5policeLogo());
            warningEmbed.Description = user + " | Do not use that profanity, your message has been deleted.";
            var msg = await pMsg.Channel.SendMessageAsync("", false, warningEmbed);
            await Delete.DelayDeleteEmbedAsync(msg, 10);
        }

        public static async Task LogMessageAsync(DiscordSocketClient bot, SocketMessage pMsg, string word)
        {
            var message = pMsg as SocketUserMessage;
            var user = pMsg.Author;
            var userId = pMsg.Author.Id;
            var channel = pMsg.Channel.ToString();
            var fullMsg = pMsg.ToString();
            var logsChannel = BotConfig.Load().LogsId;

            var context = new SocketCommandContext(bot, message);
            var server = bot.Guilds.FirstOrDefault(x => x.Id == BotConfig.Load().ServerId);
            var logChannel = server.GetTextChannel(BotConfig.Load().LogsId);

            var logEmbed = new EmbedBuilder() { Color = Colours.errorCol };
            logEmbed.WithAuthor("Profanity detected in discord chat");
            logEmbed.WithThumbnailUrl(References.gta5policeLogo());
            logEmbed.Description = "Full message: " + fullMsg;
            var userField = new EmbedFieldBuilder() { Name = "Discord User", Value = user };
            var userIdField = new EmbedFieldBuilder() { Name = "DiscordId", Value = userId };
            var channelField = new EmbedFieldBuilder() { Name = "Channel", Value = channel };
            var wordField = new EmbedFieldBuilder() { Name = "Word", Value = word };
            logEmbed.AddField(userField);
            logEmbed.AddField(userIdField);
            logEmbed.AddField(channelField);
            logEmbed.AddField(wordField);
            await logChannel.SendMessageAsync("", false, logEmbed);
            CommandHandler.AddOutgoingMessages();
        }

        public static async Task DMMessageAsync(DiscordSocketClient bot, SocketMessage pMsg, string word)
        {
            var message = pMsg as SocketUserMessage;
            var user = pMsg.Author;
            var server = bot.Guilds.FirstOrDefault(x => x.Id == BotConfig.Load().ServerId);
            var dmMessage = new EmbedBuilder() { Color = Colours.errorCol };
            dmMessage.WithAuthor("Profanity Detected!", References.gta5policeLogo());
            dmMessage.Description = user + " | Do not use that profanity, your message has been deleted and you have been banned from the discord.";
            dmMessage.AddField(new EmbedFieldBuilder() { Name = "How to appeal", Value = "Head over to " + References.supportURL() + " and fill out an appeal or head to Teamspeak using IP gta5police.com" });
            var iDMChannel = await user.GetOrCreateDMChannelAsync();
            await iDMChannel.SendMessageAsync("", false, dmMessage);
            CommandHandler.AddOutgoingMessages();
        }
    }
}
