using Discord.Commands;
using System.Threading;
using System.Threading.Tasks;
using GTA5PoliceV2.Config;
using GTA5PoliceV2.Util;
using GTA5PoliceV2.Connection;
using Discord;
using Discord.WebSocket;

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
                    var message = await Context.Channel.SendMessageAsync("", false, embed);
                    await Delete.DelayDeleteEmbed(message, 120);

                    await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police", "Settings command was used by " + Context.User + "."));
                    CommandHandler.outgoingMessages++;
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
                    var message = await Context.Channel.SendMessageAsync("", false, embed);
                    await Delete.DelayDeleteEmbed(message, 120);

                    await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police", "Filter add command was used by " + Context.User + "."));
                    CommandHandler.outgoingMessages++;
                }
            }
        }

        [Command("clear")]
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        public async Task Clear(int amount = 100, SocketGuildUser user = null)
        {
            Errors errors = new Errors();
            await Context.Message.DeleteAsync();

            if (user == null)
            {
                if (amount > 100)
                {
                    await errors.sendError(Context.Channel, "You can not clear more than 100! Deleting 100.", Colours.errorCol);
                    var messageHistory = await Context.Channel.GetMessagesAsync(101).Flatten();
                    await Context.Channel.DeleteMessagesAsync(messageHistory);
                    await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police", "Clear command was used by " + Context.User + "."));
                    await Program.Logger(new LogMessage(LogSeverity.Verbose, "GTA5Police", "100 messages were deleted in the channel " + Context.Channel.Name + " by " + Context.User + "."));
                }
                else if (amount < 1) await errors.sendErrorTemp(Context.Channel, "You can not clear a negative amount of messages!", Colours.errorCol);
                else
                {
                    var messageHistory = await Context.Channel.GetMessagesAsync(amount).Flatten();

                    await Context.Channel.DeleteMessagesAsync(messageHistory);

                    await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police", "Clear command was used by " + Context.User + "."));
                    await Program.Logger(new LogMessage(LogSeverity.Verbose, "GTA5Police", amount + " messages were deleted in the channel " + Context.Channel.Name + " by " + Context.User + "."));
                }
            }
        }

        [Command("welcometohell")]
        public async Task Welcome()
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
            await Context.User.SendMessageAsync("", false, embed);
        }

        [Command("dev add")]
        public async Task DevAdd(IUser developer)
        {
            for (int i = 0; i <= BotConfig.Load().Commanders - 1; i++)
            {
                if (Context.User.Id == BotConfig.Load().BotCommanders[i])
                {
                    var dev = developer.Id;

                    DevConfig devConfig = new DevConfig();
                    devConfig = Update.UpdateDevConfig(devConfig);
                    devConfig.Developers[DevConfig.Load().Devs] = dev;
                    devConfig.Devs = DevConfig.Load().Devs + 1;
                    devConfig.Save();

                    var embed = new EmbedBuilder() { Color = Colours.adminCol };
                    embed.WithAuthor("Successfully Added", References.gta5policeLogo());
                    embed.WithThumbnailUrl(References.gta5policeLogo());
                    embed.Description = "The developer " + developer + " was successfully added.";
                    embed.WithFooter(new EmbedFooterBuilder() { Text = "Requested by " + Context.User });
                    embed.WithCurrentTimestamp();

                    await Context.Message.DeleteAsync();
                    var message = await Context.Channel.SendMessageAsync("", false, embed);
                    await Delete.DelayDeleteEmbed(message, 120);

                    await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police", "Dev add command was used by " + Context.User + "."));
                    CommandHandler.outgoingMessages++;
                }
            }
        }
    }
}
