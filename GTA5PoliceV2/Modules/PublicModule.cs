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
        [Command("status")]
        public async Task Status()
        {
            ServerStatus status = new ServerStatus();
            status.pingServers();

            var channel = Context.Channel;
            var user = Context.User;
            await Context.Message.DeleteAsync();
            await status.displayStatus(channel, user);
        }

        [Command("rules")]
        public async Task Rules()
        {
            await Context.Message.DeleteAsync();

            var embed = new EmbedBuilder() { Color = Colours.generalCol };
            embed.WithAuthor("GTA5Police Rules", References.gta5policeLogo);
            embed.Title = "Click to view all GTA5Police rules.";
            embed.WithUrl(References.rulesURL);
            embed.WithThumbnailUrl(References.gta5policeLogo);
            embed.WithImageUrl(References.howBanURL);
            embed.WithFooter("Requested by " + Context.User);
            embed.WithCurrentTimestamp();

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("links")]
        public async Task Links()
        {
            await Context.Message.DeleteAsync();

            var embed = new EmbedBuilder() { Color = Colours.generalCol };
            embed.WithAuthor("GTA5Police Links", References.gta5policeLogo);
            embed.Description = "Useful GTA5Police links for you.";
            embed.WithThumbnailUrl(References.gta5policeLogo);
            embed.AddField(new EmbedFieldBuilder() { Name = "Website", Value = "https://www.gta5police.com" });
            embed.AddField(new EmbedFieldBuilder() { Name = "Forums", Value = "https://gta5police.com/forums/" });
            embed.AddField(new EmbedFieldBuilder() { Name = "Support", Value = "http://gta5police.com/forums/index.php?/support/" });
            embed.AddField(new EmbedFieldBuilder() { Name = "Donations", Value = "http://gta5police.com/forums/index.php?/donate/" });
            embed.WithFooter("Requested by " + Context.User);
            embed.WithCurrentTimestamp();

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("apply")]
        public async Task Apply()
        {
            await Context.Message.DeleteAsync();

            var embed = new EmbedBuilder() { Color = Colours.generalCol };
            embed.WithAuthor("GTA5Police Applications", References.gta5policeLogo);
            embed.Description = "Whitelist jobs and server applications.";
            embed.WithThumbnailUrl(References.gta5policeLogo);
            embed.AddField(new EmbedFieldBuilder() { Name = "Whitelist Servers", Value = "https://goo.gl/TLSGdf" });
            embed.AddField(new EmbedFieldBuilder() { Name = "Police", Value = "https://goo.gl/RYNDBA" });
            embed.AddField(new EmbedFieldBuilder() { Name = "EMS", Value = "https://goo.gl/vNzGvr" });
            embed.AddField(new EmbedFieldBuilder() { Name = "Mechanic", Value = "https://goo.gl/rChgek" });
            embed.WithFooter("Requested by " + Context.User);
            embed.WithCurrentTimestamp();

            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
