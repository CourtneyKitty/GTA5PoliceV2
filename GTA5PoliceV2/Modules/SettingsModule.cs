using Discord.Commands;
using System.Threading.Tasks;
using GTA5PoliceV2.Config;
using GTA5PoliceV2.Util;
using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using GTA5PoliceV2.Administration;

namespace GTA5PoliceV2.Modules
{
    public class SettingsModule : ModuleBase
    {
        Errors errors = new Errors();
        Success success = new Success();

        [Command("settings help")]
        [Alias("settings ?", "settings?")]
        public async Task SettingsHelpAsync()
        {
            for (int i = 0; i <= BotConfig.Load().Commanders - 1; i++)
            {
                if (BotConfig.Load().BotCommanders[i] == Context.User.Id)
                {
                    await Context.Message.DeleteAsync();
                    await Context.Channel.SendMessageAsync("Not yet implemented the settings help, for now refer to Admin discord in #text-code-etc-dump");
                    await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Configuration Commands", "Settings command was used by " + Context.User + "."));
                }
            }
        }

        [Command("settings prefix")]
        [Alias("settings !")]
        public async Task SettingsPrefixAsync([Remainder] string prefix)
        {
            for (int i = 0; i <= BotConfig.Load().Commanders - 1; i++)
            {
                if (BotConfig.Load().BotCommanders[i] == Context.User.Id)
                {
                    await Context.Message.DeleteAsync();

                    BotConfig config = new BotConfig();
                    config = Update.UpdateConfig(config);
                    config.Prefix = prefix;
                    config.Save();

                    await success.sendSuccessTempAsync(Context.Channel, "Prefix Set", "Prefix set to `" + prefix + "` successfully!", Colours.adminCol);
                    await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Configuration Commands", "Settings command was used by " + Context.User + "."));
                }
            }
        }

        [Command("settings logs")]
        [Alias("settings logschannel")]
        public async Task SettingsLogsAsync([Remainder] IChannel channel)
        {
            for (int i = 0; i <= BotConfig.Load().Commanders - 1; i++)
            {
                if (BotConfig.Load().BotCommanders[i] == Context.User.Id)
                {
                    await Context.Message.DeleteAsync();

                    BotConfig config = new BotConfig();
                    config = Update.UpdateConfig(config);
                    config.LogsId = channel.Id;
                    config.Save();

                    await success.sendSuccessTempAsync(Context.Channel, "Logs Channel Set", "Channel set to `" + channel.Name + "` successfully!", Colours.adminCol);
                    await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Configuration Commands", "Settings command was used by " + Context.User + "."));
                }
            }
        }

        [Command("settings timers")]
        [Alias("settings timerschannel")]
        public async Task SettingsTimersAsync([Remainder] IChannel channel)
        {
            for (int i = 0; i <= BotConfig.Load().Commanders - 1; i++)
            {
                if (BotConfig.Load().BotCommanders[i] == Context.User.Id)
                {
                    await Context.Message.DeleteAsync();

                    BotConfig config = new BotConfig();
                    config = Update.UpdateConfig(config);
                    config.TimerChannelId = channel.Id;
                    config.Save();

                    await success.sendSuccessTempAsync(Context.Channel, "Timers Channel Set", "Channel set to `" + channel.Name + "` successfully!", Colours.adminCol);
                    await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Configuration Commands", "Settings command was used by " + Context.User + "."));
                }
            }
        }

        [Command("settings statustimer")]
        [Alias("settings stimer")]
        public async Task SettingsStatustimersAsync(int minutes)
        {
            for (int i = 0; i <= BotConfig.Load().Commanders - 1; i++)
            {
                if (BotConfig.Load().BotCommanders[i] == Context.User.Id)
                {
                    await Context.Message.DeleteAsync();

                    BotConfig config = new BotConfig();
                    config = Update.UpdateConfig(config);
                    config.StatusTimerInterval = minutes;
                    config.Save();

                    await success.sendSuccessTempAsync(Context.Channel, "Status Timer Interval Set", "Status timer interval set to `" + minutes + " minutes` successfully!", Colours.adminCol);
                    await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Configuration Commands", "Settings command was used by " + Context.User + "."));
                }
            }
        }

        [Command("settings messagetimer")]
        [Alias("settings mtimer")]
        public async Task SettingsMessagetimersAsync(int seconds)
        {
            for (int i = 0; i <= BotConfig.Load().Commanders - 1; i++)
            {
                if (BotConfig.Load().BotCommanders[i] == Context.User.Id)
                {
                    await Context.Message.DeleteAsync();

                    BotConfig config = new BotConfig();
                    config = Update.UpdateConfig(config);
                    config.MessageTimerInterval = seconds;
                    config.Save();

                    await success.sendSuccessTempAsync(Context.Channel, "Message Timer Interval Set", "Message timer interval set to `" + seconds + " seconds` successfully!", Colours.adminCol);
                    await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Configuration Commands", "Settings command was used by " + Context.User + "."));
                }
            }
        }

        [Command("settings messagecooldown")]
        [Alias("settings mcooldown")]
        public async Task SettingsMessagecooldownAsync(int messages)
        {
            for (int i = 0; i <= BotConfig.Load().Commanders - 1; i++)
            {
                if (BotConfig.Load().BotCommanders[i] == Context.User.Id)
                {
                    await Context.Message.DeleteAsync();

                    BotConfig config = new BotConfig();
                    config = Update.UpdateConfig(config);
                    config.MessageTimerCooldown = messages;
                    config.Save();

                    await success.sendSuccessTempAsync(Context.Channel, "Message Timer Cooldown Set", "Message timer cooldown set to `" + messages + " messages` successfully!", Colours.adminCol);
                    await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Configuration Commands", "Settings command was used by " + Context.User + "."));
                }
            }
        }

        [Command("settings commandcooldown")]
        [Alias("settings ccooldown")]
        public async Task SettingsCommandcooldownAsync(int seconds)
        {
            for (int i = 0; i <= BotConfig.Load().Commanders - 1; i++)
            {
                if (BotConfig.Load().BotCommanders[i] == Context.User.Id)
                {
                    await Context.Message.DeleteAsync();

                    BotConfig config = new BotConfig();
                    config = Update.UpdateConfig(config);
                    config.CommandCooldown = seconds;
                    config.Save();

                    await success.sendSuccessTempAsync(Context.Channel, "Command Cooldown Set", "Command cooldown set to `" + seconds + " seconds` successfully!", Colours.adminCol);
                    await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Configuration Commands", "Settings command was used by " + Context.User + "."));
                }
            }
        }

        [Command("settings copadd")]
        [Alias("settings cadd")]
        public async Task SettingsCopaddAsync()
        {
            for (int i = 0; i <= BotConfig.Load().Commanders - 1; i++)
            {
                if (BotConfig.Load().BotCommanders[i] == Context.User.Id)
                {
                    await Context.Message.DeleteAsync();

                    BotConfig config = new BotConfig();
                    config = Update.UpdateConfig(config);
                    if (!BotConfig.Load().PoliceAdd) config.PoliceAdd = true;
                    if (BotConfig.Load().PoliceAdd) config.PoliceAdd = false;
                    config.Save();

                    await success.sendSuccessTempAsync(Context.Channel, "Cop Add", "Cop add has been toggled successfully! Now set to " + BotConfig.Load().PoliceAdd, Colours.adminCol);
                    await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Configuration Commands", "Settings command was used by " + Context.User + "."));
                }
            }
        }

        [Command("settings emsadd")]
        [Alias("settings eadd")]
        public async Task SettingsEmsaddAsync()
        {
            for (int i = 0; i <= BotConfig.Load().Commanders - 1; i++)
            {
                if (BotConfig.Load().BotCommanders[i] == Context.User.Id)
                {
                    await Context.Message.DeleteAsync();

                    BotConfig config = new BotConfig();
                    config = Update.UpdateConfig(config);
                    if (!BotConfig.Load().EmsAdd) config.EmsAdd = true;
                    if (BotConfig.Load().EmsAdd) config.EmsAdd = false;
                    config.Save();

                    await success.sendSuccessTempAsync(Context.Channel, "EMS Add", "EMS add has been toggled successfully! Now set to " + BotConfig.Load().EmsAdd, Colours.adminCol);
                    await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Configuration Commands", "Settings command was used by " + Context.User + "."));
                }
            }
        }

        [Command("settings ip")]
        [Alias("settings serverip")]
        public async Task SettingsIpAsync([Remainder] string ip)
        {
            for (int i = 0; i <= BotConfig.Load().Commanders - 1; i++)
            {
                if (BotConfig.Load().BotCommanders[i] == Context.User.Id)
                {
                    await Context.Message.DeleteAsync();

                    ConnectionsConfig config = new ConnectionsConfig();
                    config = Update.UpdateConnectionsConfig(config);
                    config.ServerIp = ip;
                    config.Save();

                    await success.sendSuccessTempAsync(Context.Channel, "IP Change", "IP Changed for the server box to " + ip, Colours.adminCol);
                    await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Configuration Commands", "Settings command was used by " + Context.User + "."));
                }
            }
        }

        [Command("settings port")]
        [Alias("settings p")]
        public async Task SettingsPortAsync(int id, int port)
        {
            for (int i = 0; i <= BotConfig.Load().Commanders - 1; i++)
            {
                if (BotConfig.Load().BotCommanders[i] == Context.User.Id)
                {
                    await Context.Message.DeleteAsync();

                    if (id == 1)
                    {
                        ConnectionsConfig config = new ConnectionsConfig();
                        config = Update.UpdateConnectionsConfig(config);
                        config.NyPort = port;
                        config.Save();
                        await success.sendSuccessTempAsync(Context.Channel, "Port Change", "Port changed to " + port + "for New York.", Colours.adminCol);
                    }
                    else if (id == 2)
                    {
                        ConnectionsConfig config = new ConnectionsConfig();
                        config = Update.UpdateConnectionsConfig(config);
                        config.LaPort = port;
                        config.Save();
                        await success.sendSuccessTempAsync(Context.Channel, "Port Change", "Port changed to " + port + "for Los Angeles.", Colours.adminCol);
                    }
                    else if (id == 3)
                    {
                        ConnectionsConfig config = new ConnectionsConfig();
                        config = Update.UpdateConnectionsConfig(config);
                        config.NyWlPort = port;
                        config.Save();
                        await success.sendSuccessTempAsync(Context.Channel, "Port Change", "Port changed to " + port + "for Whitelist New York.", Colours.adminCol);
                    }
                    else if (id == 4)
                    {
                        ConnectionsConfig config = new ConnectionsConfig();
                        config = Update.UpdateConnectionsConfig(config);
                        config.LaWlPort = port;
                        config.Save();
                        await success.sendSuccessTempAsync(Context.Channel, "Port Change", "Port changed to " + port + "for Whitelist Los Angeles.", Colours.adminCol);
                    }
                    else
                        await errors.sendErrorTempAsync(Context.Channel, "Enter a valid server ID! ID's: 1=NY, 2=LA, 3=NYWL, 4=LAWL", Colours.errorCol);

                    await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Configuration Commands", "Settings command was used by " + Context.User + "."));
                }
            }
        }




        [Command("settings")]
        public async Task SettingsCommandAsync()
        {
            for (int i = 0; i <= BotConfig.Load().Commanders - 1; i++)
            {
                if (BotConfig.Load().BotCommanders[i] == Context.User.Id)
                {
                    var embed = new EmbedBuilder() { Color = Colours.adminCol };
                    var blankField = new EmbedFieldBuilder() { Name = "\u200b", Value = "\u200b" };

                    embed.WithAuthor("GTA5PoliceV2 Settings", References.GetGta5policeLogo());
                    embed.WithThumbnailUrl(References.GetGta5policeLogo());

                    string filtered = null;
                    for (int j = 0; j <= BotConfig.Load().Filters - 1; j++)
                    {
                        var filter = BotConfig.Load().FilteredWords[j].ToString();
                        if (filtered != null) filtered = filtered + ", " + filter;
                        if (filtered == null) filtered = filter;
                    }
                    if (filtered != null) embed.AddField(new EmbedFieldBuilder() { Name = "Filtered Words", Value = filtered, IsInline = true });
                    if (filtered == null) embed.AddField(new EmbedFieldBuilder() { Name = "Filtered Words", Value = "No filtered words.", IsInline = true });

                    string commanders = null;
                    for (int j = 0; j <= BotConfig.Load().Commanders - 1; j++)
                    {
                        var commander = Context.Client.GetUserAsync(BotConfig.Load().BotCommanders[j]).Result.ToString();
                        if (commanders != null) commanders = commanders + ", " + commander;
                        if (commanders == null) commanders = commander;
                    }
                    if (commanders != null) embed.AddField(new EmbedFieldBuilder() { Name = "Bot commanders", Value = commanders, IsInline = true });
                    if (commanders == null) embed.AddField(new EmbedFieldBuilder() { Name = "Bot commanders", Value = "No commanders.", IsInline = true });


                    embed.AddField(new EmbedFieldBuilder() { Name = "Server Id", Value = BotConfig.Load().ServerId, IsInline = true });
                    embed.AddField(new EmbedFieldBuilder() { Name = "Logs Id", Value = BotConfig.Load().LogsId, IsInline = true });
                    embed.AddField(blankField);
                    embed.AddField(new EmbedFieldBuilder() { Name = "Timer Channel Id", Value = BotConfig.Load().TimerChannelId, IsInline = true });
                    embed.AddField(new EmbedFieldBuilder() { Name = "Status Timer Interval (Minutes)", Value = BotConfig.Load().StatusTimerInterval, IsInline = true });
                    embed.AddField(new EmbedFieldBuilder() { Name = "Message Timer Interval (Minutes)", Value = BotConfig.Load().MessageTimerInterval, IsInline = true });
                    embed.AddField(new EmbedFieldBuilder() { Name = "Message Timer Cooldown (Messages)", Value = BotConfig.Load().MessageTimerCooldown, IsInline = true });
                    embed.AddField(new EmbedFieldBuilder() { Name = "Command Cooldown (Seconds)", Value = BotConfig.Load().CommandCooldown, IsInline = true });
                    embed.AddField(blankField);
                    embed.AddField(new EmbedFieldBuilder() { Name = "Server IP", Value = ConnectionsConfig.Load().ServerIp, IsInline = true });
                    embed.AddField(new EmbedFieldBuilder() { Name = "NY Port", Value = ConnectionsConfig.Load().NyPort, IsInline = true });
                    embed.AddField(new EmbedFieldBuilder() { Name = "LA Port", Value = ConnectionsConfig.Load().LaPort, IsInline = true });
                    embed.AddField(new EmbedFieldBuilder() { Name = "NY WL Port", Value = ConnectionsConfig.Load().NyWlPort, IsInline = true });
                    embed.AddField(new EmbedFieldBuilder() { Name = "LA WL Port", Value = ConnectionsConfig.Load().LaWlPort, IsInline = true });

                    embed.AddField(blankField);
                    embed.AddField(new EmbedFieldBuilder() { Name = "EMS Add/Remove", Value = BotConfig.Load().EmsAdd, IsInline = true });
                    string emsHighUps = null;
                    for (int j = 0; j <= RanksConfig.Load().EMSHighRanks - 1; j++)
                    {
                        var emsHighUp = RanksConfig.Load().EMSHighRanksArray[j];
                        if (emsHighUps != null) emsHighUps = emsHighUps + ", " + emsHighUp;
                        if (emsHighUps == null) emsHighUps = emsHighUp;
                    }
                    if (commanders != null) embed.AddField(new EmbedFieldBuilder() { Name = "EMS Highup Ranks", Value = emsHighUps, IsInline = true });
                    if (commanders == null) embed.AddField(new EmbedFieldBuilder() { Name = "EMS Highup Ranks", Value = "No high up ranks?", IsInline = true });

                    embed.AddField(new EmbedFieldBuilder() { Name = "Cop Add/Remove", Value = BotConfig.Load().PoliceAdd, IsInline = true });
                    string copHighUps = null;
                    for (int j = 0; j <= RanksConfig.Load().PDHighRanks - 1; j++)
                    {
                        var copHighUp = RanksConfig.Load().PDHighRanksArray[j];
                        if (copHighUps != null) copHighUps = copHighUps + ", " + copHighUp;
                        if (copHighUps == null) copHighUps = copHighUp;
                    }
                    if (commanders != null) embed.AddField(new EmbedFieldBuilder() { Name = "Police Highup Ranks", Value = copHighUps, IsInline = true });
                    if (commanders == null) embed.AddField(new EmbedFieldBuilder() { Name = "Police Highup Ranks", Value = "No high up ranks?", IsInline = true });

                    embed.AddField(blankField);
                    string devs = null;
                    for (int j = 0; j <= DevConfig.Load().Devs - 1; j++)
                    {
                        var dev = Context.Client.GetUserAsync(DevConfig.Load().Developers[j]).Result.ToString();
                        if (devs != null) devs = devs + ", " + dev;
                        if (devs == null) devs = dev;
                    }
                    if (devs != null) embed.AddField(new EmbedFieldBuilder() { Name = "Developers", Value = devs, IsInline = true });
                    if (devs == null) embed.AddField(new EmbedFieldBuilder() { Name = "Developers", Value = "No developers added", IsInline = true });

                    embed.WithFooter(new EmbedFooterBuilder() { Text = References.NAME + References.VERSION });
                    embed.WithCurrentTimestamp();

                    await Context.Message.DeleteAsync();
                    var message = await Context.Channel.SendMessageAsync("", false, embed);
                    await Delete.DelayDeleteEmbedAsync(message, 120);

                    await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Configuration Commands", "Settings command was used by " + Context.User + "."));
                    Statistics.AddOutgoingMessages();
                }
            }
        }
    }
}
