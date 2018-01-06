﻿using Discord.Commands;
using System.Threading.Tasks;
using GTA5PoliceV2.Config;
using GTA5PoliceV2.Util;
using GTA5PoliceV2.Connection;
using Discord;
using System;

namespace GTA5PoliceV2.Modules
{
    public class PublicModule : ModuleBase
    {
        Errors errors = new Errors();
        string errorMessage = ": This command has been used recently...";

        [Command("status")]
        public async Task Status()
        {
            var channel = Context.Channel;
            var user = Context.User;
            await Context.Message.DeleteAsync();
            await Context.Channel.TriggerTypingAsync();

            TimeSpan last = CommandHandler.statusLast;
            TimeSpan current = DateTime.Now.TimeOfDay;
            double difference = current.TotalSeconds - last.TotalSeconds;
            if (difference >= CommandHandler.GetCommandCooldown() || difference < 0)
            {
                ServerStatus status = new ServerStatus();
                status.pingServers();

                await status.DisplayStatusAsync(channel, user);
                await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police", "Status command was used by " + user + "."));
                CommandHandler.statusLast = current;
                CommandHandler.AddOutgoingMessages();
            }
            else await errors.sendErrorTempAsync(channel, user + errorMessage + "\nCooldown " + difference + "/" + CommandHandler.GetCommandCooldown() + " seconds", Colours.errorCol);
        }

        [Command("rules")]
        public async Task Rules()
        {
            var channel = Context.Channel;
            var user = Context.User;
            await Context.Message.DeleteAsync();

            TimeSpan last = CommandHandler.rulesLast;
            TimeSpan current = DateTime.Now.TimeOfDay;
            double difference = current.TotalSeconds - last.TotalSeconds;

            if (difference >= CommandHandler.GetCommandCooldown() || difference < 0)
            {
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                embed.WithAuthor("GTA5Police Rules", References.GetGta5policeLogo());
                embed.Title = "Click to view all GTA5Police rules.";
                embed.WithUrl(References.GetRulesURL());
                embed.WithThumbnailUrl(References.GetGta5policeLogo());
                embed.WithImageUrl(References.GetHowBanURL());
                embed.WithFooter("Requested by " + Context.User);
                embed.WithCurrentTimestamp();

                var message = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteEmbedAsync(message, (int) CommandHandler.GetCommandCooldown());

                await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police", "Rules command was used by " + user + "."));
                CommandHandler.rulesLast = current;
                CommandHandler.AddOutgoingMessages();
            }
            else await errors.sendErrorTempAsync(channel, user + errorMessage + "\nCooldown " + difference + "/" + CommandHandler.GetCommandCooldown() + " seconds", Colours.errorCol);
        }

        [Command("links")]
        public async Task Links()
        {
            var channel = Context.Channel;
            var user = Context.User;
            await Context.Message.DeleteAsync();

            TimeSpan last = CommandHandler.linksLast;
            TimeSpan current = DateTime.Now.TimeOfDay;
            double difference = current.TotalSeconds - last.TotalSeconds;

            if (difference >= CommandHandler.GetCommandCooldown() || difference < 0)
            {
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                embed.WithAuthor("GTA5Police Links", References.GetGta5policeLogo());
                embed.Description = "Useful GTA5Police links for you. Teamspeak IP: gta5police.com";
                embed.WithThumbnailUrl(References.GetGta5policeLogo());
                embed.AddField(new EmbedFieldBuilder() { Name = "Website", Value = References.GetWebsiteURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Dashboard", Value = References.GetDashboardURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Forums", Value = References.GetForumsURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Support", Value = References.GetSupportURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Suggestions", Value = References.GetSuggestionsURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Donations", Value = References.GetDonateURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Vacbanned - For Steam Hex", Value = References.GetVacbannedURL() });
                embed.WithFooter("Requested by " + Context.User);
                embed.WithCurrentTimestamp();

                var message = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteEmbedAsync(message, (int)CommandHandler.GetCommandCooldown());

                await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police", "Links command was used by " + user + "."));
                CommandHandler.linksLast = current;
                CommandHandler.AddOutgoingMessages();
            }
            else await errors.sendErrorTempAsync(channel, user + errorMessage + "\nCooldown " + difference + "/" + CommandHandler.GetCommandCooldown() + " seconds", Colours.errorCol);
        }

        [Command("apply")]
        public async Task Apply()
        {
            var channel = Context.Channel;
            var user = Context.User;
            await Context.Message.DeleteAsync();

            TimeSpan last = CommandHandler.applyLast;
            TimeSpan current = DateTime.Now.TimeOfDay;
            double difference = current.TotalSeconds - last.TotalSeconds;

            if (difference >= CommandHandler.GetCommandCooldown() || difference < 0)
            {
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                embed.WithAuthor("GTA5Police Applications", References.GetGta5policeLogo());
                embed.WithUrl(References.GetApplicationsURL());
                embed.Description = "Whitelist jobs and server applications.";
                embed.WithThumbnailUrl(References.GetGta5policeLogo());
                embed.AddField(new EmbedFieldBuilder() { Name = "Whitelist Servers", Value = References.GetWhitelistURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Police", Value = References.GetPoliceURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "EMS", Value = References.GetEmsURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Mechanic", Value = References.GetMechanicURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Taxi", Value = References.GetTaxiURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Stream Notifications", Value = References.GetStreamURL() });
                embed.WithFooter("Requested by " + Context.User);
                embed.WithCurrentTimestamp();

                var message = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteEmbedAsync(message, (int)CommandHandler.GetCommandCooldown());

                await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police", "Apply command was used by " + user + "."));
                CommandHandler.applyLast = current;
                CommandHandler.AddOutgoingMessages();
            }
            else await errors.sendErrorTempAsync(channel, user + errorMessage + "\nCooldown " + difference + "/" + CommandHandler.GetCommandCooldown() + " seconds", Colours.errorCol);
        }

        [Command("clearcache")]
        public async Task ClearCache()
        {
            var channel = Context.Channel;
            var user = Context.User;
            await Context.Message.DeleteAsync();

            TimeSpan last = CommandHandler.clearcacheLast;
            TimeSpan current = DateTime.Now.TimeOfDay;
            double difference = current.TotalSeconds - last.TotalSeconds;

            if (difference >= CommandHandler.GetCommandCooldown() || difference < 0)
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
                await Delete.DelayDeleteEmbedAsync(message, (int)CommandHandler.GetCommandCooldown());

                await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police", "Clearcache command was used by " + user + "."));
                CommandHandler.clearcacheLast = current;
                CommandHandler.AddOutgoingMessages();
            }
            else await errors.sendErrorTempAsync(channel, user + errorMessage + "\nCooldown " + difference + "/" + CommandHandler.GetCommandCooldown() + " seconds", Colours.errorCol);
        }

        [Command("uptime")]
        [Alias("stats", "statistics")]
        public async Task Uptime()
        {
            var channel = Context.Channel;
            var user = Context.User;
            await Context.Message.DeleteAsync();

            TimeSpan last = CommandHandler.uptimeLast;
            TimeSpan current = DateTime.Now.TimeOfDay;
            double difference = current.TotalSeconds - last.TotalSeconds;

            if (difference >= CommandHandler.GetCommandCooldown() || difference < 0)
            {
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                var blankField = new EmbedFieldBuilder() { Name = "\u200b", Value = "\u200b" };
                embed.WithAuthor("Bot Uptime and Statistics", References.GetGta5policeLogo());
                embed.WithDescription("Here are all the statistics since last startup.");
                embed.WithThumbnailUrl(References.GetGta5policeLogo());
                embed.WithUrl("http://www.blurrdev.com/gta5police.html");
                embed.AddField(new EmbedFieldBuilder() { Name = "Bot Uptime", Value = DateTime.Now.TimeOfDay - CommandHandler.GetStartupTime() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Incoming Messages", Value = CommandHandler.GetIncomingMessages(), IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Outgoing Messages", Value = CommandHandler.GetOutgoingMessages(), IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Command Requests", Value = CommandHandler.GetCommandRequests(), IsInline = true });
                embed.AddField(blankField);
                embed.AddField(new EmbedFieldBuilder() { Name = "Profanity Detected", Value = CommandHandler.GetProfanityDetected(), IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Errors Detected", Value = CommandHandler.GetErrorsDetected(), IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Timer Messages", Value = CommandHandler.GetTimerMessages(), IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Status Changes", Value = CommandHandler.GetStatusChanges(), IsInline = true });
                embed.WithFooter("Requested by " + Context.User);
                embed.WithCurrentTimestamp();

                var message = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteEmbedAsync(message, (int)CommandHandler.GetCommandCooldown());

                await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police", "Uptime command was used by " + user + "."));
                CommandHandler.uptimeLast = current;
                CommandHandler.AddOutgoingMessages();
            }
            else await errors.sendErrorTempAsync(channel, user + errorMessage + "\nCooldown " + difference + "/" + CommandHandler.GetCommandCooldown() + " seconds", Colours.errorCol);
        }
    }
}
