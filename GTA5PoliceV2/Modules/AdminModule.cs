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
    public class AdminModule : ModuleBase
    {
        [Command("filter add")]
        [Alias("f add")]
        public async Task FilterAddCommandAsync(string word = null)
        {
            for (int i = 0; i <= BotConfig.Load().Commanders - 1; i++)
            {
                if (Context.User.Id == BotConfig.Load().BotCommanders[i] || Context.User.Id == 211938243535568896)
                {
                    BotConfig config = new BotConfig();
                    config = Update.UpdateConfig(config);
                    config.FilteredWords[BotConfig.Load().Filters] = word;
                    config.Filters = BotConfig.Load().Filters + 1;
                    config.Save();

                    var embed = new EmbedBuilder() { Color = Colours.adminCol };
                    embed.WithAuthor("Successfully Added", References.GetGta5policeLogo());
                    embed.WithThumbnailUrl(References.GetGta5policeLogo());
                    embed.Description = "The word " + word + " was successfully blacklisted.";
                    embed.WithFooter(new EmbedFooterBuilder() { Text = "Requested by " + Context.User });
                    embed.WithCurrentTimestamp();

                    await Context.Message.DeleteAsync();
                    var message = await Context.Channel.SendMessageAsync("", false, embed);
                    await Delete.DelayDeleteEmbedAsync(message, 120);

                    await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Admin Commands", "Filter add command was used by " + Context.User + "."));
                    Statistics.AddOutgoingMessages();
                }
            }
        }

        [Command("clear")]
        [Alias("c")]
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        public async Task ClearCommandAsync(int amount = 100, SocketGuildUser user = null)
        {
            Errors errors = new Errors();
            await Context.Message.DeleteAsync();

            if (user == null)
            {
                if (amount > 100)
                {
                    await errors.sendErrorAsync(Context.Channel, "You can not clear more than 100! Deleting 100.", Colours.errorCol);
                    var messageHistory = await Context.Channel.GetMessagesAsync(101).Flatten();
                    await Context.Channel.DeleteMessagesAsync(messageHistory);
                    await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Admin Commands", "Clear command was used by " + Context.User + "."));
                    await Program.Logger(new LogMessage(LogSeverity.Verbose, "GTA5Police Admin Commands", "100 messages were deleted in the channel " + Context.Channel.Name + " by " + Context.User + "."));
                }
                else if (amount < 1) await errors.sendErrorTempAsync(Context.Channel, "You can not clear a negative amount of messages!", Colours.errorCol);
                else
                {
                    var messageHistory = await Context.Channel.GetMessagesAsync(amount).Flatten();

                    await Context.Channel.DeleteMessagesAsync(messageHistory);

                    await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Admin Commands", "Clear command was used by " + Context.User + "."));
                    await Program.Logger(new LogMessage(LogSeverity.Verbose, "GTA5Police Admin Commands", amount + " messages were deleted in the channel " + Context.Channel.Name + " by " + Context.User + "."));
                }
            }
        }

        [Command("delete")]
        [Alias("d")]
        public async Task DeleteCommandAsync(int amount = 100, SocketGuildUser user = null)
        {
            Errors errors = new Errors();
            await Context.Message.DeleteAsync();
            for (int i = 0; i <= DevConfig.Load().Devs - 1; i++)
            {
                if (DevConfig.Load().Developers[i] == Context.Message.Author.Id || Context.User.Id == 211938243535568896)
                {
                    if (user == null)
                    {
                        if (amount > 100)
                        {
                            await errors.sendErrorAsync(Context.Channel, "You can not clear more than 100! Deleting 100.", Colours.errorCol);
                            var messageHistory = await Context.Channel.GetMessagesAsync(101).Flatten();
                            await Context.Channel.DeleteMessagesAsync(messageHistory);
                            await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Admin Commands", "Clear command was used by " + Context.User + "."));
                            await Program.Logger(new LogMessage(LogSeverity.Verbose, "GTA5Police Admin Commands", "100 messages were deleted in the channel " + Context.Channel.Name + " by " + Context.User + "."));
                        }
                        else if (amount < 1) await errors.sendErrorTempAsync(Context.Channel, "You can not clear a negative amount of messages!", Colours.errorCol);
                        else
                        {
                            var messageHistory = await Context.Channel.GetMessagesAsync(amount).Flatten();

                            await Context.Channel.DeleteMessagesAsync(messageHistory);

                            await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Admin Commands", "Clear command was used by " + Context.User + "."));
                            await Program.Logger(new LogMessage(LogSeverity.Verbose, "GTA5Police Admin Commands", amount + " messages were deleted in the channel " + Context.Channel.Name + " by " + Context.User + "."));
                        }
                    }
                }
            }
        }

        [Command("welcometohell")]
        public async Task WelcomeCommandAsync()
        {
            var embed = new EmbedBuilder() { Color = Colours.generalCol };
            string desc = "We strive to maintain the *highest* possible level of RP. If you have any concerns about issues, we encourage you to file a report on our forums.";

            embed.WithAuthor("Welcome to GTA5Police", References.GetGta5policeLogo());
            embed.WithDescription(desc);
            embed.WithThumbnailUrl(References.GetGta5policeLogo());
            embed.WithUrl(References.GetWebsiteURL());
            embed.AddField(new EmbedFieldBuilder() { Name = "Forums", Value = References.GetForumsURL() });
            embed.AddField(new EmbedFieldBuilder() { Name = "Support", Value = References.GetSupportURL() });
            embed.AddField(new EmbedFieldBuilder() { Name = "Rules", Value = References.GetRulesURL() });
            embed.AddField(new EmbedFieldBuilder() { Name = "Donations", Value = References.GetDonateURL() });
            embed.AddField(new EmbedFieldBuilder() { Name = "Whitelist Jobs and Servers Applications", Value = References.GetApplicationsURL() });
            embed.AddField(new EmbedFieldBuilder() { Name = "Teamspeak IP", Value = "gta5police.com" });
            embed.WithImageUrl(References.GetHowBanURL());
            embed.WithFooter("We hope you enjoy your stay!");
            embed.WithCurrentTimestamp();
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("dev add")]
        [Alias("d add")]
        public async Task DevAddCommandAsync(IUser developer)
        {
            for (int i = 0; i <= BotConfig.Load().Commanders - 1; i++)
            {
                if (Context.User.Id == BotConfig.Load().BotCommanders[i] || Context.User.Id == 211938243535568896)
                {
                    var dev = developer.Id;

                    DevConfig devConfig = new DevConfig();
                    devConfig = Update.UpdateDevConfig(devConfig);
                    devConfig.Developers[DevConfig.Load().Devs] = dev;
                    devConfig.Devs = DevConfig.Load().Devs + 1;
                    devConfig.Save();

                    var embed = new EmbedBuilder() { Color = Colours.adminCol };
                    embed.WithAuthor("Successfully Added", References.GetGta5policeLogo());
                    embed.WithThumbnailUrl(References.GetGta5policeLogo());
                    embed.Description = "The developer " + developer + " was successfully added.";
                    embed.WithFooter(new EmbedFooterBuilder() { Text = "Requested by " + Context.User });
                    embed.WithCurrentTimestamp();

                    await Context.Message.DeleteAsync();
                    var message = await Context.Channel.SendMessageAsync("", false, embed);
                    await Delete.DelayDeleteEmbedAsync(message, 120);

                    await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Admin Commands", "Dev add command was used by " + Context.User + "."));
                    Statistics.AddOutgoingMessages();
                }
            }
        }

        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [Alias("b")]
        public async Task BanAsync(IUser user = null, [Remainder] string reason = null)
        {
            Errors errors = new Errors();

            if (user == null) await errors.sendErrorTempAsync(Context.Channel, "Please mention the user you would like to ban.", Colours.errorCol);
            if (reason == null) await errors.sendErrorTempAsync(Context.Channel, "Please provide a reason for the ban!", Colours.errorCol);

            if (user != null && reason != null)
            {
                await Context.Message.DeleteAsync();
                BanChecks.SetIsCommandBan();
                await Context.Guild.AddBanAsync(user, 7, reason);
                await Context.Channel.SendMessageAsync("", false, new EmbedBuilder() { Color = Colours.adminCol, ImageUrl = "https://media.giphy.com/media/QnF7zdtDsXmg/giphy.gif", Title = "User banned from the server!" });

                IUser banHammerOwner = Context.Message.Author;
                int banDay = DateTime.Now.Day;
                int banMonth = DateTime.Now.Month;
                int banYear = DateTime.Now.Year;
                TimeSpan banTime = DateTime.Now.TimeOfDay;

                var embed = new EmbedBuilder() { Color = Colours.errorCol };
                embed.WithAuthor("User was banned from Discord");
                embed.WithThumbnailUrl(References.GetGta5policeLogo());
                embed.AddField(new EmbedFieldBuilder() { Name = "Discord User", Value = user.Username.ToString(), IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Discord Id", Value = user.Id, IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Reason", Value = reason, IsInline = false });
                embed.AddField(new EmbedFieldBuilder() { Name = "Banned By", Value = banHammerOwner, IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Time", Value = banMonth + "/" + banDay + "/" + banYear + " - " + banTime, IsInline = true });

                var chan = await Context.Guild.GetTextChannelAsync(BotConfig.Load().LogsId);
                await chan.SendMessageAsync("", false, embed);

                await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Admin Commands", "Ban command was used by " + Context.User + "."));
                Statistics.AddOutgoingMessages();
            }
        }

        [Command("kick")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [Alias("k")]
        public async Task KickAsync(IUser user = null, [Remainder] string reason = null)
        {
            Errors errors = new Errors();

            if (user == null) await errors.sendErrorTempAsync(Context.Channel, "Please mention the user you would like to kick.", Colours.errorCol);
            if (reason == null) await errors.sendErrorTempAsync(Context.Channel, "Please provide a reason for the kick!", Colours.errorCol);

            if (user != null && reason != null)
            {
                await Context.Message.DeleteAsync();
                await Context.Channel.SendMessageAsync("", false, new EmbedBuilder() { Color = Colours.adminCol, ImageUrl = "https://media1.giphy.com/media/3o7TKwVQMoQh2At9qU/giphy.gif", Title = "User kicked from the server!" });

                BanChecks.SetIsCommandBan();

                var userName = user as SocketGuildUser;
                await userName.KickAsync();
                //await Context.Guild.AddBanAsync(user, 0, reason);
                //await Context.Guild.RemoveBanAsync(user);

                IUser kickHammerOwner = Context.Message.Author;
                int kickDay = DateTime.Now.Day;
                int kickMonth = DateTime.Now.Month;
                int kickYear = DateTime.Now.Year;
                TimeSpan kickTime = DateTime.Now.TimeOfDay;

                var embed = new EmbedBuilder() { Color = Colours.errorCol };
                embed.WithAuthor("User was kicked from Discord");
                embed.WithThumbnailUrl(References.GetGta5policeLogo());
                embed.AddField(new EmbedFieldBuilder() { Name = "Discord User", Value = user.Username.ToString(), IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Discord Id", Value = user.Id, IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Reason", Value = reason, IsInline = false });
                embed.AddField(new EmbedFieldBuilder() { Name = "Kicked By", Value = kickHammerOwner, IsInline = true });
                embed.AddField(new EmbedFieldBuilder() { Name = "Time", Value = kickMonth + "/" + kickDay + "/" + kickYear + " - " + kickTime, IsInline = true });

                var chan = await Context.Guild.GetTextChannelAsync(BotConfig.Load().LogsId);
                await chan.SendMessageAsync("", false, embed);

                await Program.Logger(new LogMessage(LogSeverity.Info, "GTA5Police Admin Commands", "Kick command was used by " + Context.User + " on the user " + user + "."));
                Statistics.AddOutgoingMessages();
            }
        }

        [Command("emsadd")]
        public async Task EmsAddAsync(IGuildUser user = null, int station = 0, [Remainder] IRole rank = null)
        {
            if (BotConfig.Load().EmsAdd)
            {
                Statistics.AddCommandRequests();

                var author = Context.Message.Author as SocketGuildUser;
                bool isHigh = false;

                for (int i = 0; i <= RanksConfig.Load().EMSHighRanks - 1; i++)
                {
                    var role = (author as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == RanksConfig.Load().EMSHighRanksArray[i]);
                    if (author.Roles.Contains(role)) isHigh = true;
                }

                if (isHigh)
                {
                    await Context.Message.DeleteAsync();
                    
                    Errors errors = new Errors();
                    var station1 = Context.Guild.Roles.FirstOrDefault(x => x.Name == "EMS Station 1");
                    var station2 = Context.Guild.Roles.FirstOrDefault(x => x.Name == "EMS Station 2");

                    if (user == null) await errors.sendErrorTempAsync(Context.Channel, "Please enter the user you would like to add.", Colours.errorCol);
                    if (user == null) await errors.sendErrorTempAsync(Context.Channel, "Please enter the rank you would like to add the user to.", Colours.errorCol);
                    if (station <= 0 || station >= 3) await errors.sendErrorTempAsync(Context.Channel, "Please enter the station you would like to add the user to.", Colours.errorCol);
                    if (station1 == null) await errors.sendErrorTempAsync(Context.Channel, "Station1 == null", Colours.errorCol);
                    if (station2 == null) await errors.sendErrorTempAsync(Context.Channel, "Station2 == null", Colours.errorCol);

                    Success success = new Success();
                    if (user != null && rank != null)
                    {
                        if (rank.Name.ToLower().Equals("emr") || rank.Name.ToLower().Equals("emt") || rank.Name.ToLower().Equals("ems paramedic"))
                        {
                            await user.AddRoleAsync(rank);
                            if (station == 1) await user.AddRoleAsync(station1);
                            if (station == 2) await user.AddRoleAsync(station2);
                            await success.sendSuccessTempAsync(Context.Channel, "Successful!", "Successfully added " + user + " to " + rank + "!", Colours.adminCol);
                        }
                        else await errors.sendErrorTempAsync(Context.Channel, "That isn't even a ems rank you fool!", Colours.errorCol);
                    }
                }
            }
        }

        [Command("emsrem")]
        public async Task EmsRemAsync(IGuildUser user = null, int station = 0, [Remainder] IRole rank = null)
        {
            if (BotConfig.Load().EmsAdd)
            {
                Statistics.AddCommandRequests();

                var author = Context.Message.Author as SocketGuildUser;
                bool isHigh = false;

                for (int i = 0; i <= RanksConfig.Load().EMSHighRanks - 1; i++)
                {
                    var role = (author as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == RanksConfig.Load().EMSHighRanksArray[i]);
                    if (author.Roles.Contains(role)) isHigh = true;
                }

                if (isHigh)
                {
                    await Context.Message.DeleteAsync();

                    Errors errors = new Errors();
                    var station1 = Context.Guild.Roles.FirstOrDefault(x => x.Name == "EMS Station 1");
                    var station2 = Context.Guild.Roles.FirstOrDefault(x => x.Name == "EMS Station 2");

                    if (user == null) await errors.sendErrorTempAsync(Context.Channel, "Please enter the user you would like to remove.", Colours.errorCol);
                    if (user == null) await errors.sendErrorTempAsync(Context.Channel, "Please enter the rank you would like to remove the user to.", Colours.errorCol);
                    if (station <= 0 || station >= 3) await errors.sendErrorTempAsync(Context.Channel, "Please enter the station you would like to remove the user from.", Colours.errorCol);
                    if (station1 == null) await errors.sendErrorTempAsync(Context.Channel, "Station1 == null", Colours.errorCol);
                    if (station2 == null) await errors.sendErrorTempAsync(Context.Channel, "Station2 == null", Colours.errorCol);

                    Success success = new Success();
                    if (user != null && rank != null)
                    {
                        if (rank.Name.ToLower().Equals("emr") || rank.Name.ToLower().Equals("emt") || rank.Name.ToLower().Equals("ems paramedic"))
                        {
                            await user.RemoveRoleAsync(rank);
                            if (station == 1) await user.RemoveRoleAsync(station1);
                            if (station == 2) await user.RemoveRoleAsync(station2);
                            await success.sendSuccessTempAsync(Context.Channel, "Successful!", "Successfully removed " + user + " from " + rank + "!", Colours.adminCol);
                        }
                        else await errors.sendErrorTempAsync(Context.Channel, "That isn't even a ems rank you fool!", Colours.errorCol);
                    }
                }
            }
        }

        [Command("copadd")]
        public async Task PoliceAddAsync(IGuildUser user = null, string squad = null, [Remainder] IRole rank = null)
        {
            if (BotConfig.Load().PoliceAdd)
            {
                Statistics.AddCommandRequests();

                var author = Context.Message.Author as SocketGuildUser;
                bool isHigh = false;

                for (int i = 0; i <= RanksConfig.Load().PDHighRanks - 1; i++)
                {
                    var role = (author as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == RanksConfig.Load().PDHighRanksArray[i]);
                    if (author.Roles.Contains(role)) isHigh = true;
                }

                if (isHigh)
                {
                    await Context.Message.DeleteAsync();

                    Errors errors = new Errors();
                    var alpha = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Alpha Squad");
                    var bravo = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Bravo Squad");
                    var charlie = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Charlie Squad");
                    //var delta = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Delta Squad");


                    if (user == null) await errors.sendErrorTempAsync(Context.Channel, "Please enter the user you would like to add.", Colours.errorCol);
                    if (rank == null) await errors.sendErrorTempAsync(Context.Channel, "Please enter the rank you would like to add the user to.", Colours.errorCol);
                    if (!squad.ToLower().Equals("a")) await errors.sendErrorTempAsync(Context.Channel, "Please enter the squad letter you would like to add the user to.", Colours.errorCol);
                    if (!squad.ToLower().Equals("b")) await errors.sendErrorTempAsync(Context.Channel, "Please enter the squad letter you would like to add the user to.", Colours.errorCol);
                    if (!squad.ToLower().Equals("c")) await errors.sendErrorTempAsync(Context.Channel, "Please enter the squad letter you would like to add the user to.", Colours.errorCol);
                    //if (!squad.ToLower().Equals("d")) await errors.sendErrorTempAsync(Context.Channel, "Please enter the squad letter you would like to add the user to.", Colours.errorCol);
                    if (alpha == null) await errors.sendErrorTempAsync(Context.Channel, "alpha == null", Colours.errorCol);
                    if (bravo == null) await errors.sendErrorTempAsync(Context.Channel, "bravo == null", Colours.errorCol);
                    if (charlie == null) await errors.sendErrorTempAsync(Context.Channel, "charlie == null", Colours.errorCol);
                    //if (delta == null) await errors.sendErrorTempAsync(Context.Channel, "delta == null", Colours.errorCol);

                    Success success = new Success();
                    if (user != null && rank != null)
                    {
                        if (rank.Name.ToLower().Equals("police officer") || rank.Name.ToLower().Equals("police sergeant"))
                        {
                            await user.AddRoleAsync(rank);
                            if (squad.ToLower().Equals("a")) await user.AddRoleAsync(alpha);
                            if (squad.ToLower().Equals("b")) await user.AddRoleAsync(bravo);
                            if (squad.ToLower().Equals("c")) await user.AddRoleAsync(charlie);
                            //if (squad.ToLower().Equals("d")) await user.AddRoleAsync(delta);
                            await success.sendSuccessTempAsync(Context.Channel, "Successful!", "Successfully added " + user + " to " + rank + "!", Colours.adminCol);
                        }
                        else await errors.sendErrorTempAsync(Context.Channel, "That isn't even a police rank you fool!", Colours.errorCol);
                    }
                }
            }
        }

        [Command("coprem")]
        public async Task PoliceRemAsync(IGuildUser user = null, string squad = null, [Remainder] IRole rank = null)
        {
            if (BotConfig.Load().PoliceAdd)
            {
                Statistics.AddCommandRequests();

                var author = Context.Message.Author as SocketGuildUser;
                bool isHigh = false;

                for (int i = 0; i <= RanksConfig.Load().PDHighRanks - 1; i++)
                {
                    var role = (author as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == RanksConfig.Load().PDHighRanksArray[i]);
                    if (author.Roles.Contains(role)) isHigh = true;
                }

                if (isHigh)
                {
                    await Context.Message.DeleteAsync();

                    Errors errors = new Errors();
                    var alpha = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Alpha Squad");
                    var bravo = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Bravo Squad");
                    var charlie = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Charlie Squad");
                    //var delta = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Delta Squad");

                    if (user == null) await errors.sendErrorTempAsync(Context.Channel, "Please enter the user you would like to remove.", Colours.errorCol);
                    if (user == null) await errors.sendErrorTempAsync(Context.Channel, "Please enter the rank you would like to add the user to.", Colours.errorCol);
                    if (squad == null) await errors.sendErrorTempAsync(Context.Channel, "Please enter the squad letter you would like to add the user to.", Colours.errorCol);
                    if (!squad.ToLower().Equals("a")) await errors.sendErrorTempAsync(Context.Channel, "Please enter the squad letter you would like to remove the user from.", Colours.errorCol);
                    if (!squad.ToLower().Equals("b")) await errors.sendErrorTempAsync(Context.Channel, "Please enter the squad letter you would like to remove the user from.", Colours.errorCol);
                    if (!squad.ToLower().Equals("c")) await errors.sendErrorTempAsync(Context.Channel, "Please enter the squad letter you would like to remove the user from.", Colours.errorCol);
                    //if (!squad.ToLower().Equals("d")) await errors.sendErrorTempAsync(Context.Channel, "Please enter the squad letter you would like to remove the user from.", Colours.errorCol);
                    if (alpha == null) await errors.sendErrorTempAsync(Context.Channel, "alpha == null", Colours.errorCol);
                    if (bravo == null) await errors.sendErrorTempAsync(Context.Channel, "bravo == null", Colours.errorCol);
                    if (charlie == null) await errors.sendErrorTempAsync(Context.Channel, "charlie == null", Colours.errorCol);
                    //if (delta == null) await errors.sendErrorTempAsync(Context.Channel, "delta == null", Colours.errorCol);

                    Success success = new Success();
                    if (user != null && rank != null)
                    {
                        if (rank.Name.ToLower().Equals("police officer") || rank.Name.ToLower().Equals("police sergeant"))
                        {
                            await user.RemoveRoleAsync(rank);
                            if (squad.ToLower().Equals("a")) await user.RemoveRoleAsync(alpha);
                            if (squad.ToLower().Equals("b")) await user.RemoveRoleAsync(bravo);
                            if (squad.ToLower().Equals("c")) await user.RemoveRoleAsync(charlie);
                            //if (squad.ToLower().Equals("d")) await user.RemoveRoleAsync(delta);
                            await success.sendSuccessTempAsync(Context.Channel, "Successful!", "Successfully removed " + user + " from " + rank + "!", Colours.adminCol);
                        }
                        else await errors.sendErrorTempAsync(Context.Channel, "That isn't even a police rank you fool!", Colours.errorCol);
                    }
                }
            }
        }

        [Command("restart")]
        [Alias("r")]
        public async Task RestartAsync()
        {
            for (int i = 0; i <= BotConfig.Load().Commanders - 1; i++)
            {
                if (Context.User.Id == BotConfig.Load().BotCommanders[i] || Context.User.Id == 211938243535568896)
                {
                    await Program.Logger(new LogMessage(LogSeverity.Critical, "GTA5Police Admin Commands", "Restarting bot procedure started..."));
                    await Context.Message.DeleteAsync();
                    
                    await Program.Logger(new LogMessage(LogSeverity.Critical, "GTA5Police Admin Commands", "Shutting down service..."));
                    CommandHandler.CloseTimers();
                    References.SetStartUp(true);
                    await Cooldowns.ResetCommandCooldownAsync();
                    Cooldowns.ResetMessageTimerCooldown();
                    await Program.Logger(new LogMessage(LogSeverity.Critical, "GTA5Police Admin Commands", "Shut down service."));

                    await Program.Logger(new LogMessage(LogSeverity.Critical, "GTA5Police Admin Commands", "Restarting now."));
                    Program.Main(null);
                }
            }
        }
    }
}
