using Discord.Commands;
using System.Threading.Tasks;
using GTA5PoliceV2.Config;
using GTA5PoliceV2.Util;
using GTA5PoliceV2.Connection;
using Discord;

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

            if (CommandHandler.statusMessages >= BotConfig.Load().MessageTimerCooldown)
            {
                ServerStatus status = new ServerStatus();
                status.pingServers();


                await Context.Message.DeleteAsync();
                await status.displayStatus(channel, user);
                CommandHandler.statusMessages = 0;
            }
            else
            {
                await Context.Message.DeleteAsync();
                CommandHandler.statusMessages--;
                await errors.sendErrorTemp(channel, user + errorMessage, Colours.errorCol);
            }
        }

        [Command("rules")]
        public async Task Rules()
        {
            var channel = Context.Channel;
            var user = Context.User;

            await Context.Message.DeleteAsync();

            if (CommandHandler.rulesMessages >= BotConfig.Load().MessageTimerCooldown)
            {

                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                embed.WithAuthor("GTA5Police Rules", References.gta5policeLogo);
                embed.Title = "Click to view all GTA5Police rules.";
                embed.WithUrl(References.rulesURL);
                embed.WithThumbnailUrl(References.gta5policeLogo);
                embed.WithImageUrl(References.howBanURL);
                embed.WithFooter("Requested by " + Context.User);
                embed.WithCurrentTimestamp();

                await Context.Channel.SendMessageAsync("", false, embed);
                CommandHandler.rulesMessages = 0;
            }
            else
            {
                await Context.Message.DeleteAsync();
                CommandHandler.rulesMessages--;
                await errors.sendErrorTemp(channel, user + errorMessage, Colours.errorCol);
            }
        }

        [Command("links")]
        public async Task Links()
        {
            var channel = Context.Channel;
            var user = Context.User;


            await Context.Message.DeleteAsync();

            if (CommandHandler.linksMessages >= BotConfig.Load().MessageTimerCooldown)
            {
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                embed.WithAuthor("GTA5Police Links", References.gta5policeLogo);
                embed.Description = "Useful GTA5Police links for you. Teamspeak IP: gta5police.com";
                embed.WithThumbnailUrl(References.gta5policeLogo);
                embed.AddField(new EmbedFieldBuilder() { Name = "Website", Value = References.websiteURL });
                embed.AddField(new EmbedFieldBuilder() { Name = "Dashboard", Value = References.dashboardURL });
                embed.AddField(new EmbedFieldBuilder() { Name = "Forums", Value = References.forumsURL });
                embed.AddField(new EmbedFieldBuilder() { Name = "Support", Value = References.supportURL });
                embed.AddField(new EmbedFieldBuilder() { Name = "Donations", Value = References.donateURL });
                embed.AddField(new EmbedFieldBuilder() { Name = "Vacbanned - For Steam Hex", Value = References.vacbannedURL });
                embed.WithFooter("Requested by " + Context.User);
                embed.WithCurrentTimestamp();

                await Context.Channel.SendMessageAsync("", false, embed);
                CommandHandler.linksMessages = 0;
            }
            else
            {
                await Context.Message.DeleteAsync();
                CommandHandler.linksMessages--;
                await errors.sendErrorTemp(channel, user + errorMessage, Colours.errorCol);
            }
        }

        [Command("apply")]
        public async Task Apply()
        {
            var channel = Context.Channel;
            var user = Context.User;

            await Context.Message.DeleteAsync();

            if (CommandHandler.applyMessages >= BotConfig.Load().MessageTimerCooldown)
            {
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                embed.WithAuthor("GTA5Police Applications", References.gta5policeLogo);
                embed.WithUrl(References.applicationsURL);
                embed.Description = "Whitelist jobs and server applications.";
                embed.WithThumbnailUrl(References.gta5policeLogo);
                embed.AddField(new EmbedFieldBuilder() { Name = "Whitelist Servers", Value = References.whitelistURL });
                embed.AddField(new EmbedFieldBuilder() { Name = "Police", Value = References.policeURL });
                embed.AddField(new EmbedFieldBuilder() { Name = "EMS", Value = References.emsURL });
                embed.AddField(new EmbedFieldBuilder() { Name = "Mechanic", Value = References.mechanicURL });
                embed.AddField(new EmbedFieldBuilder() { Name = "Taxi", Value = References.taxiURL });
                embed.WithFooter("Requested by " + Context.User);
                embed.WithCurrentTimestamp();

                await Context.Channel.SendMessageAsync("", false, embed);
                CommandHandler.applyMessages = 0;
            }
            else
            {
                await Context.Message.DeleteAsync();
                CommandHandler.applyMessages--;
                await errors.sendErrorTemp(channel, user + errorMessage, Colours.errorCol);
            }
        }
    }
}
