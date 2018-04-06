using Discord.Commands;
using System.Threading.Tasks;
using GTA5PoliceV2.Config;
using GTA5PoliceV2.Util;
using GTA5PoliceV2.Connection;
using Discord;
using System;
using Discord.WebSocket;

namespace GTA5PoliceV2.Modules
{
    public class PublicModule : ModuleBase
    {
        Errors errors = new Errors();
        string errorMessage = ": This command has been used recently...";

        [Command("help")]
        [Alias("?", "!")]
        public async Task HelpAsync()
        {
            await Context.Message.DeleteAsync();

            if (Cooldowns.GetMessageTimerCooldown() >= BotConfig.Load().MessageTimerCooldown / 2)
            {
                if (CommandHandler.GetLastTimerMessage() != null) await CommandHandler.GetLastTimerMessage().DeleteAsync();

                Statistics.AddTimerMessages();
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                embed.WithAuthor("GTA5Police Help", References.GetGta5policeLogo());
                embed.WithUrl(References.GetDashboardURL());
                embed.Description = "Be sure to check out our rules and policies, as well as other useful links!";
                embed.WithThumbnailUrl(References.GetGta5policeLogo());
                embed.AddField(new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "Rules", Value = "Rules and How We Ban." });
                embed.AddField(new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "Apply", Value = "Police, EMS, Mechanic, and Whitelist Applications" });
                embed.AddField(new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "Links", Value = "Useful Links." });
                embed.AddField(new EmbedFieldBuilder() { Name = BotConfig.Load().Prefix + "Status", Value = "View the current status of the servers." });
                embed.WithFooter("Message Timer with " + BotConfig.Load().MessageTimerInterval + " minute interval");
                embed.WithCurrentTimestamp();

                var msg = await CommandHandler.GetTimerChannel().SendMessageAsync("", false, embed);
                CommandHandler.SetLastTimerMessage(msg);
                Cooldowns.ResetMessageTimerCooldown();
                await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Commands", "Timer message delivered successfully."));
            }
            else
            {
                await errors.sendErrorTempAsync(Context.Channel, "Help wasn't delivered as it was recently posted by the message timer.", Colours.errorCol);
                await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Commands", "Timer message was not delivered due to the cooldown."));
            }
        }

        [Command("status")]
        [Alias("server")]
        public async Task StatusAsync()
        {
            var channel = Context.Channel;
            var user = Context.User;
            await Context.Message.DeleteAsync();
            await Context.Channel.TriggerTypingAsync();

            TimeSpan last = Cooldowns.GetStatusLast();
            TimeSpan current = DateTime.Now.TimeOfDay;
            double difference = current.TotalSeconds - last.TotalSeconds;
            if (difference >= Cooldowns.GetCommandCooldown() || difference < 0)
            {
                ServerStatus status = new ServerStatus();
                status.PingServers();

                await status.DisplayStatusAsync(channel, user);
                await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Commands", "Status command was used by " + user + "."));
                Cooldowns.SetStatusLast(current);
                Statistics.AddOutgoingMessages();
            }
            else await errors.sendErrorTempAsync(channel, user + errorMessage + "\nCooldown " + difference + "/" + Cooldowns.GetCommandCooldown() + " seconds", Colours.errorCol);
        }

        [Command("rules")]
        public async Task RulesAsync()
        {
            var channel = Context.Channel;
            var user = Context.User;
            await Context.Message.DeleteAsync();

            TimeSpan last = Cooldowns.GetRulesLast();
            TimeSpan current = DateTime.Now.TimeOfDay;
            double difference = current.TotalSeconds - last.TotalSeconds;

            if (difference >= Cooldowns.GetCommandCooldown() || difference < 0)
            {
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                embed.WithAuthor("GTA5Police Rules", References.GetGta5policeLogo());
                embed.Title = "Click to view all GTA5Police rules.";
                embed.Description = "**Teamspeak IP: gta5police.com**";
                embed.WithUrl(References.GetRulesURL());
                embed.WithThumbnailUrl(References.GetGta5policeLogo());
                embed.WithImageUrl(References.GetHowBanURL());
                embed.WithFooter("Requested by " + Context.User);
                embed.WithCurrentTimestamp();

                var message = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteEmbedAsync(message, (int)Cooldowns.GetCommandCooldown());

                await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Commands", "Rules command was used by " + user + "."));
                Cooldowns.SetRulesLast(current);
                Statistics.AddOutgoingMessages();
            }
            else await errors.sendErrorTempAsync(channel, user + errorMessage + "\nCooldown " + difference + "/" + Cooldowns.GetCommandCooldown() + " seconds", Colours.errorCol);
        }

        [Command("links")]
        [Alias("teamspeak", "ts")]
        public async Task LinksAsync()
        {
            var channel = Context.Channel;
            var user = Context.User;
            await Context.Message.DeleteAsync();

            TimeSpan last = Cooldowns.GetLinksLast();
            TimeSpan current = DateTime.Now.TimeOfDay;
            double difference = current.TotalSeconds - last.TotalSeconds;

            if (difference >= Cooldowns.GetCommandCooldown() || difference < 0)
            {
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                embed.WithAuthor("GTA5Police Links", References.GetGta5policeLogo());
                embed.Description = "Useful GTA5Police links for you.\n**Teamspeak IP: gta5police.com**";
                embed.WithThumbnailUrl(References.GetGta5policeLogo());
                embed.AddField(new EmbedFieldBuilder() { Name = "Website", Value = References.GetWebsiteURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Dashboard", Value = References.GetDashboardURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Forums", Value = References.GetForumsURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Support", Value = References.GetSupportURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Suggestions", Value = References.GetSuggestionsURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Donations", Value = References.GetDonateURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Vacbanned - For Steam Hex", Value = References.GetVacbannedURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Greggs", Value = "https://www.greggs.co.uk/" });
                embed.WithFooter("Requested by " + Context.User);
                embed.WithCurrentTimestamp();

                var message = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteEmbedAsync(message, (int)Cooldowns.GetCommandCooldown());

                await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Commands", "Links command was used by " + user + "."));
                Cooldowns.SetLinksLast(current);
                Statistics.AddOutgoingMessages();
            }
            else await errors.sendErrorTempAsync(channel, user + errorMessage + "\nCooldown " + difference + "/" + Cooldowns.GetCommandCooldown() + " seconds", Colours.errorCol);
        }

        [Command("apply")]
        public async Task ApplyAsync()
        {
            var channel = Context.Channel;
            var user = Context.User;
            await Context.Message.DeleteAsync();

            TimeSpan last = Cooldowns.GetApplyLast();
            TimeSpan current = DateTime.Now.TimeOfDay;
            double difference = current.TotalSeconds - last.TotalSeconds;

            if (difference >= Cooldowns.GetCommandCooldown() || difference < 0)
            {
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                embed.WithAuthor("GTA5Police Applications", References.GetGta5policeLogo());
                embed.WithUrl(References.GetApplicationsURL());
                embed.Description = "Whitelist jobs and server applications.\n**Teamspeak IP: gta5police.com**";
                embed.WithThumbnailUrl(References.GetGta5policeLogo());
                embed.AddField(new EmbedFieldBuilder() { Name = "Whitelist Servers", Value = References.GetWhitelistURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Police", Value = References.GetPoliceURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "EMS", Value = References.GetEmsURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Mechanic", Value = References.GetMechanicURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Taxi", Value = References.GetTaxiURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Stream Verification", Value = References.GetStreamURL() });
                embed.WithFooter("Requested by " + Context.User);
                embed.WithCurrentTimestamp();

                var message = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteEmbedAsync(message, (int)Cooldowns.GetCommandCooldown());

                await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Commands", "Apply command was used by " + user + "."));
                Cooldowns.SetApplyLast(current);
                Statistics.AddOutgoingMessages();
            }
            else await errors.sendErrorTempAsync(channel, user + errorMessage + "\nCooldown " + difference + "/" + Cooldowns.GetCommandCooldown() + " seconds", Colours.errorCol);
        }

        [Command("clearcache")]
        [Alias("cache", "cc")]
        public async Task ClearCachAsync()
        {
            var channel = Context.Channel;
            var user = Context.User;
            await Context.Message.DeleteAsync();

            TimeSpan last = Cooldowns.GetClearcacheLast();
            TimeSpan current = DateTime.Now.TimeOfDay;
            double difference = current.TotalSeconds - last.TotalSeconds;

            if (difference >= Cooldowns.GetCommandCooldown() || difference < 0)
            {
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                embed.WithAuthor("How to clear your cache 101", References.GetGta5policeLogo());
                embed.WithUrl(References.GetClearcacheURL());
                embed.Title = "Click here to learn how!";
                embed.Description = "Clearing your FiveM cache will help with many errors. This includes resources not loading, graphical issues and fps issues.";
                embed.WithThumbnailUrl(References.GetGta5policeLogo());
                embed.WithFooter("Requested by " + Context.User);
                embed.WithCurrentTimestamp();

                var message = await Context.Channel.SendMessageAsync("", false, embed);
                //await Delete.DelayDeleteEmbedAsync(message, (int)Cooldowns.GetCommandCooldown());

                await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Commands", "Clearcache command was used by " + user + "."));
                Cooldowns.SetClearcacheLast(current);
                Statistics.AddOutgoingMessages();
            }
            else await errors.sendErrorTempAsync(channel, user + errorMessage + "\nCooldown " + difference + "/" + Cooldowns.GetCommandCooldown() + " seconds", Colours.errorCol);
        }

        [Command("uptime")]
        [Alias("stats", "statistics")]
        public async Task UptimeAsync()
        {
            var channel = Context.Channel;
            var user = Context.User;
            await Context.Message.DeleteAsync();

            TimeSpan last = Cooldowns.GetUptimeLast();
            TimeSpan current = DateTime.Now.TimeOfDay;
            double difference = current.TotalSeconds - last.TotalSeconds;

            if (difference >= Cooldowns.GetCommandCooldown() || difference < 0)
            {
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                var blankField = new EmbedFieldBuilder() { Name = "\u200b", Value = "\u200b" };
                embed.WithAuthor("Bot Uptime and Statistics", References.GetGta5policeLogo());
                embed.WithDescription("Here are all the statistics since last startup.\n**Teamspeak IP: gta5police.com**");
                embed.WithThumbnailUrl(References.GetGta5policeLogo());
                embed.WithUrl("http://www.blurrdev.com/gta5police.html");
                embed.AddField(new EmbedFieldBuilder() { Name = "Bot Uptime", Value = DateTime.Now.TimeOfDay - Cooldowns.GetStartupTime() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Incoming Messages", Value = Statistics.GetIncomingMessages(), IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Outgoing Messages", Value = Statistics.GetOutgoingMessages(), IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Command Requests", Value = Statistics.GetCommandRequests(), IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Admen Requests", Value = Statistics.GetAdmenRequests(), IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Oofers", Value = Statistics.GetOofMessages(), IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Meta Messages", Value = Statistics.GetMetaMessages(), IsInline = true });
                embed.AddField(blankField);
                embed.AddField(new EmbedFieldBuilder() { Name = "Profanity Detected", Value = Statistics.GetProfanityDetected(), IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Errors Detected", Value = Statistics.GetErrorsDetected(), IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Timer Messages", Value = Statistics.GetTimerMessages(), IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Status Changes", Value = Statistics.GetStatusChanges(), IsInline = true });
                embed.WithFooter("Requested by " + Context.User);
                embed.WithCurrentTimestamp();

                var message = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteEmbedAsync(message, (int)Cooldowns.GetCommandCooldown());

                await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Commands", "Uptime command was used by " + user + "."));
                Cooldowns.SetUptimeLast(current);
                Statistics.AddOutgoingMessages();
            }
            else await errors.sendErrorTempAsync(channel, user + errorMessage + "\nCooldown " + difference + "/" + Cooldowns.GetCommandCooldown() + " seconds", Colours.errorCol);
        }

        [Command("meta")]
        public async Task MetaAsync(IUser metarerar)
        {
            await Context.Message.DeleteAsync();

            var message = await Context.Channel.SendMessageAsync(metarerar.Mention + " **#META**\n_'Metagaming is any strategy, action or method used in a game which transcends a prescribed ruleset, uses external factors to affect the game, or goes beyond the supposed limits or environment set by the game. Another definition refers to the game universe outside of the game itself.'_ Walkingking, 2k18");
            Statistics.AddOutgoingMessages();
            Statistics.AddMetaMessages();
        }
    }
}
