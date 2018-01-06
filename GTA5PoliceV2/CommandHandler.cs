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
using GTA5PoliceV2.DevReports;
using GTA5PoliceV2.Profanity;

namespace GTA5PoliceV2
{
    class CommandHandler
    {
        Errors errors = new Errors();
        private CommandService commands;
        private static DiscordSocketClient bot;
        private IServiceProvider map;

        private static int incomingMessages, outgoingMessages, commandRequests, timerMessages, statusChanges, errorsDetected, profanityDetected;
        private static TimeSpan startupTime;
        private static IUserMessage lastTimerMessage = null;

        public CommandHandler(IServiceProvider provider)
        {
            map = provider;
            bot = map.GetService<DiscordSocketClient>();
            bot.UserJoined += AnnounceUserJoinedAsync;
            bot.UserJoined += WelcomeUserJoinedAsync;
            bot.UserLeft += AnnounceLeftUserAsync;
            bot.UserBanned += AnnounceBannedUserAsync;
            bot.UserUnbanned += AnnounceUnbannedUserAsync;
            bot.Ready += SetGameAsync;
            bot.Ready += StartTimersAsync;
            bot.Ready += ResetUptimeAsync;
            bot.Ready += CommandCooldownAsync;
            bot.MessageReceived += HandleCommandAsync;
            commands = map.GetService<CommandService>();
            bot.MessageReceived += Reports.HandleReportAsync;
            bot.MessageReceived += ProfanityFilter.ProfanityCheckAsync;
            bot.MessageReceived += TimerCooldownAsync;
        }

        public async Task AnnounceLeftUserAsync(SocketGuildUser user) {}
        public async Task AnnounceUserJoinedAsync(SocketGuildUser user) {}
        public async Task WelcomeUserJoinedAsync(SocketGuildUser user)
        {
            var embed = new EmbedBuilder() { Color = Colours.generalCol };
            string desc = "We strive to maintain the *highest* possible level of RP. If you have any concerns about issues, we encourage you to file a report on our forums.";

            embed.WithAuthor("Welcome to GTA5Police", References.gta5policeLogo());
            embed.WithDescription(desc);
            embed.WithThumbnailUrl(References.gta5policeLogo());
            embed.WithUrl(References.websiteURL());
            embed.AddField(new EmbedFieldBuilder() { Name = "Forums", Value = References.forumsURL() });
            embed.AddField(new EmbedFieldBuilder() { Name = "Support", Value = References.supportURL() });
            embed.AddField(new EmbedFieldBuilder() { Name = "Rules", Value = References.rulesURL() });
            embed.AddField(new EmbedFieldBuilder() { Name = "Donations", Value = References.donateURL() });
            embed.AddField(new EmbedFieldBuilder() { Name = "Whitelist Jobs and Servers Applications", Value = References.applicationsURL() });
            embed.AddField(new EmbedFieldBuilder() { Name = "Teamspeak IP", Value = "gta5police.com" });
            embed.WithImageUrl(References.howBanURL());
            embed.WithFooter("We hope you enjoy your stay!");
            embed.WithCurrentTimestamp();
            await user.SendMessageAsync("", false, embed);
            outgoingMessages++;
        }
        public async Task AnnounceBannedUserAsync(SocketUser user, SocketGuild guild)
        {
            var server = bot.Guilds.FirstOrDefault(x => x.Id == BotConfig.Load().ServerId);
            var logChannel = server.GetTextChannel(BotConfig.Load().LogsId);

            var logEmbed = new EmbedBuilder() { Color = Colours.errorCol };
            logEmbed.WithAuthor("User was banned from Discord");
            logEmbed.WithThumbnailUrl(References.gta5policeLogo());
            logEmbed.AddField(new EmbedFieldBuilder() { Name = "Discord User", Value = user.Username.ToString(), IsInline = true });
            logEmbed.AddField(new EmbedFieldBuilder() { Name = "DiscordId", Value = user.Id, IsInline = true });

            await logChannel.SendMessageAsync("", false, logEmbed);
            outgoingMessages++;
        }
        public async Task AnnounceUnbannedUserAsync(SocketUser user, SocketGuild guild)
        {
            var server = bot.Guilds.FirstOrDefault(x => x.Id == BotConfig.Load().ServerId);
            var logChannel = server.GetTextChannel(BotConfig.Load().LogsId);

            var logEmbed = new EmbedBuilder() { Color = Colours.adminCol };
            logEmbed.WithAuthor("User was unbanned from Discord");
            logEmbed.WithThumbnailUrl(References.gta5policeLogo());
            logEmbed.AddField(new EmbedFieldBuilder() { Name = "Discord User", Value = user.Username.ToString(), IsInline = true });
            logEmbed.AddField(new EmbedFieldBuilder() { Name = "DiscordId", Value = user.Id, IsInline = true });

            await logChannel.SendMessageAsync("", false, logEmbed);
            outgoingMessages++;
        }

        public async Task SetGameAsync() { await bot.SetGameAsync("GTA5Police.com"); }
        public async Task ConfigureAsync() { await commands.AddModulesAsync(Assembly.GetEntryAssembly());}

        public async Task ResetUptimeAsync()
        {
            startupTime = DateTime.Now.TimeOfDay;
            incomingMessages = 0;
            outgoingMessages = 0;
            commandRequests = 0;
            timerMessages = 0;
            statusChanges = 0;
            errorsDetected = 0;
            profanityDetected = 0;
        }

        public async Task HandleCommandAsync(SocketMessage pMsg)
        {
            var message = pMsg as SocketUserMessage;
            if (message == null)
                return;
            incomingMessages++;
            var context = new SocketCommandContext(bot, message);
            
            int argPos = 0;
            if (message.HasStringPrefix(BotConfig.Load().Prefix, ref argPos))
            {
                commandRequests++;

                if (message.Author.IsBot)
                    return;
                var result = await commands.ExecuteAsync(context, argPos, map);
                
                if (!result.IsSuccess && result.ErrorReason != "Unknown command.")
                    await errors.sendErrorTempAsync(pMsg.Channel, result.ErrorReason, Colours.errorCol);
            }
        }

        public int messages = 0;
        public static double GetCommandCooldown() { return BotConfig.Load().CommandCooldown; }
        public static TimeSpan statusLast, rulesLast, linksLast, applyLast, clearcacheLast, uptimeLast;
        public async Task TimerCooldownAsync(SocketMessage pMsg)
        {
            var message = pMsg as SocketUserMessage;
            if (message == null)
                return;

            if (pMsg.Channel.Id.Equals(BotConfig.Load().TimerChannelId)) messages++;
        }
        public async Task CommandCooldownAsync()
        {
            statusLast = DateTime.Now.TimeOfDay.Subtract(new TimeSpan(0, 0, (int) BotConfig.Load().CommandCooldown));
            rulesLast = DateTime.Now.TimeOfDay.Subtract(new TimeSpan(0, 0, (int)BotConfig.Load().CommandCooldown));
            linksLast = DateTime.Now.TimeOfDay.Subtract(new TimeSpan(0, 0, (int)BotConfig.Load().CommandCooldown));
            applyLast = DateTime.Now.TimeOfDay.Subtract(new TimeSpan(0, 0, (int)BotConfig.Load().CommandCooldown));
            clearcacheLast = DateTime.Now.TimeOfDay.Subtract(new TimeSpan(0, 0, (int)BotConfig.Load().CommandCooldown));
            uptimeLast = DateTime.Now.TimeOfDay.Subtract(new TimeSpan(0, 0, (int)BotConfig.Load().CommandCooldown));
        }

        Timer timerStatus, timerMessage;
        ServerStatus status = new ServerStatus();
        Success success = new Success();
        bool ny, la, nywl, lawl;
        SocketGuild server;
        IMessageChannel channel;

        public async Task StartTimersAsync()
        {
            server = bot.Guilds.FirstOrDefault(x => x.Id == BotConfig.Load().ServerId);
            channel = server.GetTextChannel(BotConfig.Load().TimerChannelId);
       
            
            ny = false; la = false; nywl = false; lawl = false;
            timerStatus = new Timer(SendStatusAsync, null, 0, 1000 * 60 * BotConfig.Load().StatusTimerInterval);
            timerMessage = new Timer(SendMessageAsync, null, 0, 1000 * 60 * BotConfig.Load().MessageTimerInterval);

            var message = await channel.SendMessageAsync("Timers started.");
            await Delete.DelayDeleteMessageAsync(message, 5);
        }

        public async void SendStatusAsync(object state)
        {
            status.pingServers();
            if (ny != status.getNyStatus())
            {
                statusChanges++;
                if (References.isStartUp == true)
                {
                    if (status.getNyStatus()) await success.sendSuccessTempAsync(channel, "Server Status Change", "New York has come online!", Colours.generalCol, References.dashboardURL(), 5);
                    if (!status.getNyStatus()) await success.sendSuccessTempAsync(channel, "Server Status Change", "New York has gone offline!", Colours.generalCol, References.dashboardURL(), 5);
                }
                else
                {
                    if (status.getNyStatus()) await success.sendSuccessAsync(channel, "Server Status Change", "New York has come online!", Colours.generalCol, References.dashboardURL());
                    if (!status.getNyStatus()) await success.sendSuccessAsync(channel, "Server Status Change", "New York has gone offline!", Colours.generalCol, References.dashboardURL());
                }
                ny = status.getNyStatus();
            }
            if (la != status.getLaStatus())
            {
                statusChanges++;
                if (References.isStartUp == true)
                {
                    if (status.getLaStatus()) await success.sendSuccessTempAsync(channel, "Server Status Change", "Los Angeles has come online!", Colours.generalCol, References.dashboardURL(), 5);
                    if (!status.getLaStatus()) await success.sendSuccessTempAsync(channel, "Server Status Change", "Los Angeles has gone offline!", Colours.generalCol, References.dashboardURL(), 5);
                }
                else
                {
                    if (status.getLaStatus()) await success.sendSuccessAsync(channel, "Server Status Change", "Los Angeles has come online!", Colours.generalCol, References.dashboardURL());
                    if (!status.getLaStatus()) await success.sendSuccessAsync(channel, "Server Status Change", "Los Angeles has gone offline!", Colours.generalCol, References.dashboardURL());
                }
                la = status.getLaStatus();
            }
            if (nywl != status.getNyWlStatus())
            {
                statusChanges++;
                if (References.isStartUp == true)
                {
                    if (status.getNyWlStatus()) await success.sendSuccessTempAsync(channel, "Server Status Change", "New York Whitelist has come online!", Colours.generalCol, References.dashboardURL(), 5);
                    if (!status.getNyWlStatus()) await success.sendSuccessTempAsync(channel, "Server Status Change", "New York Whitelist has gone offline!", Colours.generalCol, References.dashboardURL(), 5);
                }
                else
                {
                    if (status.getNyWlStatus()) await success.sendSuccessAsync(channel, "Server Status Change", "New York Whitelist has come online!", Colours.generalCol, References.dashboardURL());
                    if (!status.getNyWlStatus()) await success.sendSuccessAsync(channel, "Server Status Change", "New York Whitelist has gone offline!", Colours.generalCol, References.dashboardURL());
                }
                nywl = status.getNyWlStatus();
            }
            if (lawl != status.getLaWlStatus())
            {
                statusChanges++;
                if (References.isStartUp == true)
                {
                    if (status.getLaWlStatus()) await success.sendSuccessTempAsync(channel, "Server Status Change", "Los Angeles Whitelist has come online!", Colours.generalCol, References.dashboardURL(), 5);
                    if (!status.getLaWlStatus()) await success.sendSuccessTempAsync(channel, "Server Status Change", "Los Angeles Whitelist has gone offline!", Colours.generalCol, References.dashboardURL(), 5);
                }
                else
                {
                    if (status.getLaWlStatus()) await success.sendSuccessAsync(channel, "Server Status Change", "Los Angeles Whitelist has come online!", Colours.generalCol, References.dashboardURL());
                    if (!status.getLaWlStatus()) await success.sendSuccessAsync(channel, "Server Status Change", "Los Angeles Whitelist has gone offline!", Colours.generalCol, References.dashboardURL());
                }
                lawl = status.getLaWlStatus();
            }
            if (References.isStartUp == true) References.isStartUp = false;
        }

        public async void SendMessageAsync(object state)
        {
            if (messages >= BotConfig.Load().MessageTimerCooldown)
            {
                if (lastTimerMessage != null) await lastTimerMessage.DeleteAsync();

                timerMessages++;
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                embed.WithAuthor("GTA5Police Help", References.gta5policeLogo());
                embed.WithUrl(References.dashboardURL());
                embed.Description = "Be sure to check out our rules and policies, as well as other useful links!";
                embed.WithThumbnailUrl(References.gta5policeLogo());
                embed.AddField(new EmbedFieldBuilder() { Name = "!Rules", Value = "Rules and How We Ban." });
                embed.AddField(new EmbedFieldBuilder() { Name = "!Apply", Value = "Police, EMS, Mechanic, and Whitelist Applications" });
                embed.AddField(new EmbedFieldBuilder() { Name = "!Links", Value = "Useful Links." });
                embed.AddField(new EmbedFieldBuilder() { Name = "!Status", Value = "View the current status of the servers." });
                embed.WithFooter("Message Timer with " + BotConfig.Load().MessageTimerInterval + " minute interval");
                embed.WithCurrentTimestamp();

                lastTimerMessage = await channel.SendMessageAsync("", false, embed);
                messages = 0;
                await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police", "Timer message delivered successfully."));
            }
            await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police", "Timer message was not delivered due to the cooldown."));
        }

        /** Getters and Setters **/
        public static DiscordSocketClient GetBot() { return bot; }

        public static int GetIncomingMessages() { return incomingMessages; }
        public static int GetOutgoingMessages() { return outgoingMessages; }
        public static int GetCommandRequests() { return commandRequests; }
        public static int GetTimerMessages() { return timerMessages; }
        public static int GetStatusChanges() { return statusChanges; }
        public static int GetErrorsDetected() { return errorsDetected; }
        public static int GetProfanityDetected() { return profanityDetected; }

        public static void AddIncomingMessages() { incomingMessages++; }
        public static void AddOutgoingMessages() { outgoingMessages++; }
        public static void AddCommandRequests() { commandRequests++; }
        public static void AddTimerMessages() { timerMessages++; }
        public static void AddStatusChanges() { statusChanges++; }
        public static void AddErrorsDetected() { errorsDetected++; }
        public static void AddProfanityDetected() { profanityDetected++; }

        public static TimeSpan GetStartupTime() { return startupTime; }
    }
}
