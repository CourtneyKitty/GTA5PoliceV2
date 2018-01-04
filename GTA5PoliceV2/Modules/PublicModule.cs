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
        string errorMessage = ": This command has been used recently.";

        [Command("status")]
        public async Task Status()
        {
            var channel = Context.Channel;
            var user = Context.User;
            await Context.Message.DeleteAsync();
            await Context.Channel.TriggerTypingAsync();

            if (CommandHandler.statusMessages >= BotConfig.Load().MessageTimerCooldown / 2)
            {
                ServerStatus status = new ServerStatus();
                status.pingServers();
                
                await status.displayStatus(channel, user);
                CommandHandler.statusMessages = 0;
                CommandHandler.outgoingMessages++;
            }
            else
            {
                if (CommandHandler.statusMessages > 0) CommandHandler.statusMessages--;
                await errors.sendErrorTemp(channel, user + errorMessage, Colours.errorCol);
            }
        }

        [Command("rules")]
        public async Task Rules()
        {
            var channel = Context.Channel;
            var user = Context.User;
            await Context.Message.DeleteAsync();

            if (CommandHandler.rulesMessages >= BotConfig.Load().MessageTimerCooldown / 2)
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
                await Delete.DelayDeleteEmbed(message, 120);
                CommandHandler.rulesMessages = 0;
                CommandHandler.outgoingMessages++;
            }
            else
            {
                if (CommandHandler.rulesMessages > 0) CommandHandler.rulesMessages--;
                await errors.sendErrorTemp(channel, user + errorMessage, Colours.errorCol);
            }
        }

        [Command("links")]
        public async Task Links()
        {
            var channel = Context.Channel;
            var user = Context.User;
            await Context.Message.DeleteAsync();

            if (CommandHandler.linksMessages >= BotConfig.Load().MessageTimerCooldown / 2)
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
                await Delete.DelayDeleteEmbed(message, 120);
                CommandHandler.linksMessages = 0;
                CommandHandler.outgoingMessages++;
            }
            else
            {
                if (CommandHandler.linksMessages > 0) CommandHandler.linksMessages--;
                await errors.sendErrorTemp(channel, user + errorMessage, Colours.errorCol);
            }
        }

        [Command("apply")]
        public async Task Apply()
        {
            var channel = Context.Channel;
            var user = Context.User;
            await Context.Message.DeleteAsync();

            if (CommandHandler.applyMessages >= BotConfig.Load().MessageTimerCooldown / 2)
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
                await Delete.DelayDeleteEmbed(message, 120);
                CommandHandler.applyMessages = 0;
                CommandHandler.outgoingMessages++;
            }
            else
            {
                if (CommandHandler.applyMessages > 0) CommandHandler.applyMessages--;
                await errors.sendErrorTemp(channel, user + errorMessage, Colours.errorCol);
            }
        }

        [Command("clearcache")]
        public async Task ClearCache()
        {
            var channel = Context.Channel;
            var user = Context.User;
            await Context.Message.DeleteAsync();

            if (CommandHandler.clearcacheMessages >= BotConfig.Load().MessageTimerCooldown / 2)
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
                await Delete.DelayDeleteEmbed(message, 120);
                CommandHandler.clearcacheMessages = 0;
                CommandHandler.outgoingMessages++;
            }
            else
            {
                if (CommandHandler.clearcacheMessages > 0) CommandHandler.clearcacheMessages--;
                await errors.sendErrorTemp(channel, user + errorMessage, Colours.errorCol);
            }
        }

        [Command("uptime")]
        [Alias("stats", "statistics")]
        public async Task Uptime()
        {
            var channel = Context.Channel;
            var user = Context.User;
            await Context.Message.DeleteAsync();

            if (CommandHandler.uptimeMessages >= BotConfig.Load().MessageTimerCooldown / 2)
            {
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                var blankField = new EmbedFieldBuilder() { Name = "\u200b", Value = "\u200b" };
                embed.WithAuthor("Bot Uptime and Statistics", References.gta5policeLogo());
                embed.WithDescription("Here are all the statistics since last startup.");
                embed.WithThumbnailUrl(References.gta5policeLogo());
                embed.WithUrl("http://www.blurrdev.com/gta5police.html");
                embed.AddField(new EmbedFieldBuilder() { Name = "Time of Bot Launch (Currently " + DateTime.Now.ToString("h:mm:ss tt") + ")", Value = CommandHandler.startupTime });
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
                await Delete.DelayDeleteEmbed(message, 120);
                CommandHandler.outgoingMessages++;
                CommandHandler.uptimeMessages = 0;
            }
            else
            {
                if (CommandHandler.uptimeMessages > 0) CommandHandler.uptimeMessages--;
                await errors.sendErrorTemp(channel, user + errorMessage, Colours.errorCol);
            }
        }
    }
}
