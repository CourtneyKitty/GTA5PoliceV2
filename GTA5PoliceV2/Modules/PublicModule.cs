using Discord.Commands;
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
            if (difference >= CommandHandler.GetCommandCooldown())
            {
                ServerStatus status = new ServerStatus();
                status.pingServers();

                await status.displayStatus(channel, user);
                CommandHandler.statusLast = current;
                CommandHandler.outgoingMessages++;
            }
            else await errors.sendErrorTemp(channel, user + errorMessage + "\nCooldown " + difference + "/" + CommandHandler.GetCommandCooldown() + " seconds", Colours.errorCol);
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

            if (difference >= CommandHandler.GetCommandCooldown())
            {
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                embed.WithAuthor("GTA5Police Rules", References.gta5policeLogo());
                embed.Title = "Click to view all GTA5Police rules.";
                embed.WithUrl(References.rulesURL());
                embed.WithThumbnailUrl(References.gta5policeLogo());
                embed.WithImageUrl(References.howBanURL());
                embed.WithFooter("Requested by " + Context.User);
                embed.WithCurrentTimestamp();

                var message = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteEmbed(message, (int) CommandHandler.GetCommandCooldown());

                CommandHandler.rulesLast = current;
                CommandHandler.outgoingMessages++;
            }
            else await errors.sendErrorTemp(channel, user + errorMessage + "\nCooldown " + difference + "/" + CommandHandler.GetCommandCooldown() + " seconds", Colours.errorCol);
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

            if (difference >= CommandHandler.GetCommandCooldown())
            {
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                embed.WithAuthor("GTA5Police Links", References.gta5policeLogo());
                embed.Description = "Useful GTA5Police links for you. Teamspeak IP: gta5police.com";
                embed.WithThumbnailUrl(References.gta5policeLogo());
                embed.AddField(new EmbedFieldBuilder() { Name = "Website", Value = References.websiteURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Dashboard", Value = References.dashboardURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Forums", Value = References.forumsURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Support", Value = References.supportURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Donations", Value = References.donateURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Vacbanned - For Steam Hex", Value = References.vacbannedURL() });
                embed.WithFooter("Requested by " + Context.User);
                embed.WithCurrentTimestamp();

                var message = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteEmbed(message, (int)CommandHandler.GetCommandCooldown());

                CommandHandler.linksLast = current;
                CommandHandler.outgoingMessages++;
            }
            else await errors.sendErrorTemp(channel, user + errorMessage + "\nCooldown " + difference + "/" + CommandHandler.GetCommandCooldown() + " seconds", Colours.errorCol);
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

            if (difference >= CommandHandler.GetCommandCooldown())
            {
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                embed.WithAuthor("GTA5Police Applications", References.gta5policeLogo());
                embed.WithUrl(References.applicationsURL());
                embed.Description = "Whitelist jobs and server applications.";
                embed.WithThumbnailUrl(References.gta5policeLogo());
                embed.AddField(new EmbedFieldBuilder() { Name = "Whitelist Servers", Value = References.whitelistURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Police", Value = References.policeURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "EMS", Value = References.emsURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Mechanic", Value = References.mechanicURL() });
                embed.AddField(new EmbedFieldBuilder() { Name = "Taxi", Value = References.taxiURL() });
                embed.WithFooter("Requested by " + Context.User);
                embed.WithCurrentTimestamp();

                var message = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteEmbed(message, (int)CommandHandler.GetCommandCooldown());

                CommandHandler.applyLast = current;
                CommandHandler.outgoingMessages++;
            }
            else await errors.sendErrorTemp(channel, user + errorMessage + "\nCooldown " + difference + "/" + CommandHandler.GetCommandCooldown() + " seconds", Colours.errorCol);
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

            if (difference >= CommandHandler.GetCommandCooldown())
            {
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                embed.WithAuthor("How to clear your cache 101", References.gta5policeLogo());
                embed.WithUrl(References.clearcacheURL());
                embed.Title = "Click here to learn how!";
                embed.Description = "Clearing your FiveM cache will help with many errors. This includes resources not loading, graphical issues and fps issues.";
                embed.WithThumbnailUrl(References.gta5policeLogo());
                embed.WithFooter("Requested by " + Context.User);
                embed.WithCurrentTimestamp();

                var message = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteEmbed(message, (int)CommandHandler.GetCommandCooldown());

                CommandHandler.clearcacheLast = current;
                CommandHandler.outgoingMessages++;
            }
            else await errors.sendErrorTemp(channel, user + errorMessage + "\nCooldown " + difference + "/" + CommandHandler.GetCommandCooldown() + " seconds", Colours.errorCol);
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

            if (difference >= CommandHandler.GetCommandCooldown())
            {
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                var blankField = new EmbedFieldBuilder() { Name = "\u200b", Value = "\u200b" };
                embed.WithAuthor("Bot Uptime and Statistics", References.gta5policeLogo());
                embed.WithDescription("Here are all the statistics since last startup.");
                embed.WithThumbnailUrl(References.gta5policeLogo());
                embed.WithUrl("http://www.blurrdev.com/gta5police.html");
                embed.AddField(new EmbedFieldBuilder() { Name = "Bot Uptime", Value = DateTime.Now.TimeOfDay - CommandHandler.startupTime });
                embed.AddField(new EmbedFieldBuilder() { Name = "Incoming Messages", Value = CommandHandler.incomingMessages, IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Outgoing Messages", Value = CommandHandler.outgoingMessages, IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Command Requests", Value = CommandHandler.commandRequests, IsInline = true });
                embed.AddField(blankField);
                embed.AddField(new EmbedFieldBuilder() { Name = "Profanity Detected", Value = CommandHandler.profanityDetected, IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Errors Detected", Value = CommandHandler.errorsDetected, IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Timer Messages", Value = CommandHandler.timerMessages, IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Status Changes", Value = CommandHandler.statusChanges, IsInline = true });
                embed.WithFooter("Requested by " + Context.User);
                embed.WithCurrentTimestamp();

                var message = await Context.Channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteEmbed(message, (int)CommandHandler.GetCommandCooldown());

                CommandHandler.uptimeLast = current;
                CommandHandler.outgoingMessages++;
            }
            else await errors.sendErrorTemp(channel, user + errorMessage + "\nCooldown " + difference + "/" + CommandHandler.GetCommandCooldown() + " seconds", Colours.errorCol);
        }
    }
}
