using Discord.Commands;
using System.Threading.Tasks;
using GTA5PoliceV2.Config;
using GTA5PoliceV2.Util;
using GTA5PoliceV2.Connection;
using Discord;

namespace GTA5PoliceV2.Modules
{
    public class AdminModule : ModuleBase
    {
        [Command("g5p settings")]
        public async Task Settings()
        {
            var embed = new EmbedBuilder() { Color = Colours.adminCol};
            embed.WithAuthor("GTA5PoliceV2 Settings", References.gta5policeLogo);
            embed.WithThumbnailUrl(References.gta5policeLogo);

            embed.AddField(new EmbedFieldBuilder() { Name = "Filtered words", Value = "Nigga, Nibba, Nigger, Chink" });
            embed.AddField(new EmbedFieldBuilder() { Name = "Bot commanders", Value = "Blurr#9177, Unit 207 | R. James#7092" });

            embed.WithFooter(new EmbedFooterBuilder() { Text = "Requested by " + Context.User });
            embed.WithCurrentTimestamp();

            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
