using System.Threading.Tasks;
using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using Discord;
using System;
using Microsoft.Extensions.DependencyInjection;
using GTA5PoliceV2.Config;
using GTA5PoliceV2.Util;
using System.Linq;
using System.Threading;
using GTA5PoliceV2.Connection;

namespace GTA5PoliceV2
{
    class CommandHandler
    {
        Errors errors = new Errors();
        private CommandService commands;
        private DiscordSocketClient bot;
        private IServiceProvider map;

        public CommandHandler(IServiceProvider provider)
        {
            map = provider;
            bot = map.GetService<DiscordSocketClient>();
            bot.UserJoined += AnnounceUserJoined;
            bot.UserLeft += AnnounceLeftUser;
            bot.Ready += SetGame;
            bot.Ready += StartTimers;
            bot.MessageReceived += HandleCommand;
            commands = map.GetService<CommandService>();
            bot.MessageReceived += ProfanityCheck;
            bot.MessageReceived += TimerCooldown;
        }

        public async Task AnnounceLeftUser(SocketGuildUser user) {}
        public async Task AnnounceUserJoined(SocketGuildUser user) {}
        public async Task SetGame() { await bot.SetGameAsync("GTA5Police.com"); }
        public async Task ConfigureAsync() { await commands.AddModulesAsync(Assembly.GetEntryAssembly());}

        public async Task HandleCommand(SocketMessage pMsg)
        {
            //Don't handle the command if it is a system message
            var message = pMsg as SocketUserMessage;
            if (message == null)
                return;
            var context = new SocketCommandContext(bot, message);

            //Mark where the prefix ends and the command begins
            int argPos = 0;
            //Determine if the message has a valid prefix, adjust argPos
            if (message.HasStringPrefix(BotConfig.Load().Prefix, ref argPos))
            {
                if (message.Author.IsBot)
                    return;
                //Execute the command, store the result
                var result = await commands.ExecuteAsync(context, argPos, map);

                //If the command failed, notify the user
                if (!result.IsSuccess && result.ErrorReason != "Unknown command.")
                    await errors.sendErrorTemp(pMsg.Channel, result.ErrorReason, Colours.errorCol);
            }
        }


        public int messages = 0;
        public static int statusMessages = BotConfig.Load().MessageTimerCooldown;
        public static int rulesMessages = BotConfig.Load().MessageTimerCooldown;
        public static int linksMessages = BotConfig.Load().MessageTimerCooldown;
        public static int applyMessages = BotConfig.Load().MessageTimerCooldown;

        public async Task TimerCooldown(SocketMessage pMsg)
        {
            var message = pMsg as SocketUserMessage;
            if (message == null)
                return;

            if (pMsg.Channel.Id.Equals(BotConfig.Load().TimerChannelId)) messages++;
            statusMessages++;
            rulesMessages++;
            linksMessages++;
            applyMessages++;
        }

        public async Task ProfanityCheck(SocketMessage pMsg)
        {
            var message = pMsg as SocketUserMessage;
            if (message == null)
                return;

            for (int i = 0; i <= BotConfig.Load().Filters - 1; i++)
            {
                if (message.ToString().ToLower().Contains(BotConfig.Load().FilteredWords[i].ToLower())) await ProfanityMessage(pMsg, BotConfig.Load().FilteredWords[i]);
            }
        }

        public async Task ProfanityMessage(SocketMessage pMsg, string word)
        {
            var message = pMsg as SocketUserMessage;
            await message.DeleteAsync();

            var user = pMsg.Author;
            var userId = pMsg.Author.Id;
            var channel = pMsg.Channel.ToString();
            var fullMsg = pMsg.ToString();
            var logsChannel = BotConfig.Load().LogsId; //find the channel

            var warningEmbed = new EmbedBuilder() { Color = Colours.errorCol };
            warningEmbed.WithAuthor("Profanity Detected!", References.gta5policeLogo);
            warningEmbed.Description = user + " | Do not use that profanity, your message has been deleted.";
            var msg = await pMsg.Channel.SendMessageAsync("", false, warningEmbed);
            await Delete.DelayDeleteEmbed(msg, 10);

            var context = new SocketCommandContext(bot, message);
            //var logChannel = context.Guild.Channels.FirstOrDefault(x => x.Id == BotConfig.Load().LogsId);
            var server = bot.Guilds.FirstOrDefault(x => x.Id == BotConfig.Load().ServerId);
            var logChannel = server.GetTextChannel(BotConfig.Load().LogsId);

            var logEmbed = new EmbedBuilder() { Color = Colours.errorCol };
            logEmbed.WithAuthor("Profanity detected in discord chat");
            logEmbed.WithThumbnailUrl(References.gta5policeLogo);
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
        }

        Timer timerStatus, timerMessage;
        ServerStatus status = new ServerStatus();
        Success success = new Success();
        bool ny, la, nywl, lawl;
        SocketGuild server;
        IMessageChannel channel;

        public async Task StartTimers()
        {
            server = bot.Guilds.FirstOrDefault(x => x.Id == BotConfig.Load().ServerId);
            channel = server.GetTextChannel(BotConfig.Load().TimerChannelId);
       
            
            ny = false; la = false; nywl = false; lawl = false;
            timerStatus = new Timer(SendStatus, null, 0, 1000 * 60 * BotConfig.Load().StatusTimerInterval);
            timerMessage = new Timer(SendMessage, null, 0, 1000 * 60 * BotConfig.Load().MessageTimerInterval);

            await channel.SendMessageAsync("Timers started.");
        }

        public async void SendStatus(object state)
        {
            status.pingServers();
            if (ny != status.getNyStatus())
            {
                if (status.getNyStatus()) await success.sendSuccess(channel, "Server Status Change", "New York has come online!", Colours.generalCol, References.dashboardURL);
                if (!status.getNyStatus()) await success.sendSuccess(channel, "Server Status Change", "New York has gone offline!", Colours.generalCol, References.dashboardURL);
                ny = status.getNyStatus();
            }
            if (la != status.getLaStatus())
            {
                if (status.getLaStatus()) await success.sendSuccess(channel, "Server Status Change", "Los Angeles has come online!", Colours.generalCol, References.dashboardURL);
                if (!status.getLaStatus()) await success.sendSuccess(channel, "Server Status Change", "Los Angeles has gone offline!", Colours.generalCol, References.dashboardURL);
                la = status.getLaStatus();
            }
            if (nywl != status.getNyWlStatus())
            {
                if (status.getNyWlStatus()) await success.sendSuccess(channel, "Server Status Change", "New York Whitelist has come online!", Colours.generalCol, References.dashboardURL);
                if (!status.getNyWlStatus()) await success.sendSuccess(channel, "Server Status Change", "New York Whitelist has gone offline!", Colours.generalCol, References.dashboardURL);
                nywl = status.getNyWlStatus();
            }
            if (lawl != status.getLaWlStatus())
            {
                if (status.getLaWlStatus()) await success.sendSuccess(channel, "Server Status Change", "Los Angeles Whitelist has come online!", Colours.generalCol, References.dashboardURL);
                if (!status.getLaWlStatus()) await success.sendSuccess(channel, "Server Status Change", "Los Angeles Whitelist has gone offline!", Colours.generalCol, References.dashboardURL);
                lawl = status.getLaWlStatus();
            }
        }
        public async void SendMessage(object state)
        {
            if (messages >= BotConfig.Load().MessageTimerCooldown)
            {
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                embed.WithAuthor("GTA5Police Help", References.gta5policeLogo);
                embed.WithUrl(References.dashboardURL);
                embed.Description = "Be sure to check out our rules and policies, as well as other useful links!";
                embed.WithThumbnailUrl(References.gta5policeLogo);
                embed.AddField(new EmbedFieldBuilder() { Name = "!Rules", Value = "Rules and How We Ban." });
                embed.AddField(new EmbedFieldBuilder() { Name = "!Apply", Value = "Police, EMS, Mechanic, and Whitelist Applications" });
                embed.AddField(new EmbedFieldBuilder() { Name = "!Links", Value = "Useful Links." });
                embed.AddField(new EmbedFieldBuilder() { Name = "!Status", Value = "View the current status of the servers." });
                embed.WithFooter("Message Timer with " + BotConfig.Load().MessageTimerInterval + " minute interval");
                embed.WithCurrentTimestamp();

                await channel.SendMessageAsync("", false, embed);
                messages = 0;
                Console.WriteLine("Message timer has delivered the message!");
            }
            Console.WriteLine("Message timer has not delivered the message!");
        }
    }
}
