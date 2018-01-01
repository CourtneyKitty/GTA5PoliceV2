using Discord.Commands;
using System.Threading;
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
            for (int i = 0; i <= BotConfig.Load().Commanders - 1; i++)
            {
                if (BotConfig.Load().BotCommanders[i] == Context.User.Id)
                {
                    var embed = new EmbedBuilder() { Color = Colours.adminCol };
                    embed.WithAuthor("GTA5PoliceV2 Settings", References.gta5policeLogo());
                    embed.WithThumbnailUrl(References.gta5policeLogo());

                    string filtered = null;
                    for (int j = 0; j <= BotConfig.Load().Filters - 1; j++)
                    {
                        var filter = BotConfig.Load().FilteredWords[j].ToString();
                        if (filtered != null) filtered = filtered + ", " + filter;
                        if (filtered == null) filtered = filter;
                    }
                    if (filtered != null) embed.AddField(new EmbedFieldBuilder() { Name = "Filtered Words", Value = filtered });
                    if (filtered == null) embed.AddField(new EmbedFieldBuilder() { Name = "Filtered Words", Value = "No filtered words." });

                    string commanders = null;
                    for (int j = 0; j <= BotConfig.Load().Commanders - 1; j++)
                    {
                        var commander = Context.Client.GetUserAsync(BotConfig.Load().BotCommanders[j]).Result.ToString();
                        if (commanders != null) commanders = commanders + ", " + commander;
                        if (commanders == null) commanders = commander;
                    }
                    if (commanders != null) embed.AddField(new EmbedFieldBuilder() { Name = "Bot commanders", Value = commanders });
                    if (commanders == null) embed.AddField(new EmbedFieldBuilder() { Name = "Bot commanders", Value = "No commanders." });


                    embed.AddField(new EmbedFieldBuilder() { Name = "Server Id", Value = BotConfig.Load().ServerId, IsInline = true });
                    embed.AddField(new EmbedFieldBuilder() { Name = "Logs Id", Value = BotConfig.Load().LogsId, IsInline = true });
                    embed.AddField(new EmbedFieldBuilder() { Name = "Timer Channel Id", Value = BotConfig.Load().TimerChannelId, IsInline = false });
                    embed.AddField(new EmbedFieldBuilder() { Name = "Timer Interval (Minutes)", Value = BotConfig.Load().MessageTimerInterval, IsInline = true });
                    embed.AddField(new EmbedFieldBuilder() { Name = "Timer Cooldown (Messages)", Value = BotConfig.Load().MessageTimerCooldown, IsInline = true });
                    embed.AddField(new EmbedFieldBuilder() { Name = "Server IP", Value = ConnectionsConfig.Load().ServerIp });
                    embed.AddField(new EmbedFieldBuilder() { Name = "NY Port", Value = ConnectionsConfig.Load().NyPort, IsInline = true });
                    embed.AddField(new EmbedFieldBuilder() { Name = "LA Port", Value = ConnectionsConfig.Load().LaPort, IsInline = true });
                    embed.AddField(new EmbedFieldBuilder() { Name = "NY WL Port", Value = ConnectionsConfig.Load().NyWlPort, IsInline = true });
                    embed.AddField(new EmbedFieldBuilder() { Name = "LA WL Port", Value = ConnectionsConfig.Load().LaWlPort, IsInline = true });

                    embed.WithFooter(new EmbedFooterBuilder() { Text = "Requested by " + Context.User });
                    embed.WithCurrentTimestamp();

                    await Context.Message.DeleteAsync();
                    await Context.Channel.SendMessageAsync("", false, embed);
                }
            }
        }

        [Command("filter add")]
        public async Task FilterAdd(string word = null)
        {
            for (int i = 0; i <= BotConfig.Load().Commanders - 1; i++)
            {
                if (Context.User.Id == BotConfig.Load().BotCommanders[i])
                {
                    BotConfig config = new BotConfig();
                    config = Update.UpdateConfig(config);
                    config.FilteredWords[BotConfig.Load().Filters] = word;
                    config.Filters = BotConfig.Load().Filters + 1;
                    config.Save();

                    var embed = new EmbedBuilder() { Color = Colours.adminCol };
                    embed.WithAuthor("Successfully Added", References.gta5policeLogo());
                    embed.WithThumbnailUrl(References.gta5policeLogo());
                    embed.Description = "The word " + word + " was successfully blacklisted.";
                    embed.WithFooter(new EmbedFooterBuilder() { Text = "Requested by " + Context.User });
                    embed.WithCurrentTimestamp();

                    await Context.Message.DeleteAsync();
                    await Context.Channel.SendMessageAsync("", false, embed);
                }
            }
        }
    }
}
