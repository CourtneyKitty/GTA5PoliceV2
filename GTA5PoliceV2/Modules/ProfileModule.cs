using Discord.Commands;
using System.Threading.Tasks;
using GTA5PoliceV2.Config;
using GTA5PoliceV2.Util;
using GTA5PoliceV2.Connection;
using Discord;
using System;
using Discord.WebSocket;
using GTA5PoliceV2.Profiles;
using System.IO;

namespace GTA5PoliceV2.Modules
{
    public class ProfileModule : ModuleBase
    {
        Errors errors = new Errors();
        Success success = new Success();

        [Command("profile")]
        public async Task ProfileAsync(ulong id = 0)
        {
            await Program.Logger(new LogMessage(LogSeverity.Debug, "GTA5Police Profiles", "Profile command called"));
            if (!Directory.Exists(Path.Combine(AppContext.BaseDirectory, "profiles"))) Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "profiles"));
            if (id == 0)
            {
                await Program.Logger(new LogMessage(LogSeverity.Debug, "GTA5Police Profiles", "Id = 0"));
                Profile profile = new Profile();                
                string dir = "profiles/" + Context.User.Id + ".json";
                if (File.Exists(Path.Combine(AppContext.BaseDirectory, dir)))
                {
                    await Program.Logger(new LogMessage(LogSeverity.Debug, "GTA5Police Profiles", "File exists"));
                    await DisplayProfile(Context.User, Context.Channel);
                }
                else
                {
                    await Program.Logger(new LogMessage(LogSeverity.Debug, "GTA5Police Profiles", "File doesnt exist"));
                    profile.PlayerNAME = Context.User.Username;
                    profile.PlayerID = Context.User.Id;
                    profile.PlayerHEX = "steam:NotSetUp";
                    profile.SuccessfulWins = 0;
                    profile.SuccessfulDefends = 0;
                    profile.Revives = 0;
                    profile.Attempts = 0;
                    profile.Money = 5000;
                    profile.Save(Context.User.Id);

                    await DisplayProfile(Context.User, Context.Channel);
                }
            }
            else
            {
                await Program.Logger(new LogMessage(LogSeverity.Debug, "GTA5Police Profiles", "Id != 0"));
                Profile profile = new Profile();
                string dir = "profiles/" + Context.User.Id + ".json";
                if (File.Exists(Path.Combine(AppContext.BaseDirectory, dir)))
                {
                    await DisplayProfile(Context.Guild.GetUserAsync(id) as IUser, Context.Channel);
                }
                else
                {
                    await errors.sendErrorTempAsync(Context.Channel, "User doesn't exist.", Colours.errorCol);
                }
            }
        }

        [Command("sethex")]
        public async Task SetHexAsync(string hex = null)
        {
            if (hex != null)
            {
                if (!hex.Contains("steam:")) hex = "steam:" + hex;
                if (hex.Length < 21) { await errors.sendErrorTempAsync(Context.Channel, "Please enter a valid hex.", Colours.errorCol); return; }
                if (hex.Length > 21) { await errors.sendErrorTempAsync(Context.Channel, "Please enter a valid hex.", Colours.errorCol); return; }

                Profile profile = new Profile();
                profile = UpdateProfile(Context.User.Id, profile);
                profile.PlayerHEX = hex;
                profile.Save(Context.User.Id);

                await success.sendSuccessTempAsync(Context.Channel, "Profile", "Updated your hex.", Colours.generalCol);
            }
            else
            {
                await errors.sendErrorTempAsync(Context.Channel, "Please enter a valid hex.", Colours.errorCol);
            }
        }

        [Command("joinheist")]
        public async Task JoinHeistAsync(int dolla = 0)
        {
            if (RoleManager.IsCiv(Context.Guild as IGuild, Context.User))
            {
                if (dolla > 0)
                {
                    if (dolla <= Profile.Load(Context.User.Id).Money)
                    {
                        await success.sendSuccessTempAsync(Context.Channel, "Robberies", "Successfully invested $" + dolla + " into a bank robbery.", Colours.generalCol);
                    }
                    else
                    {
                        await errors.sendErrorTempAsync(Context.Channel, "You can not afford that.", Colours.errorCol);
                    }
                }
                else
                {
                    await errors.sendErrorTempAsync(Context.Channel, "Please enter a valid amount of money.", Colours.errorCol);
                }
            }
            else if (RoleManager.IsPolice(Context.Guild as IGuild, Context.User))
            {
                await success.sendSuccessTempAsync(Context.Channel, "Robberies", "Successfully added to bank robbery defense as Police.", Colours.generalCol);
            }
            else if (RoleManager.IsEMS(Context.Guild as IGuild, Context.User))
            {
                await success.sendSuccessTempAsync(Context.Channel, "Robberies", "Successfully added to bank robbery defense as EMS.", Colours.generalCol);
            }
            else if (RoleManager.IsMechanic(Context.Guild as IGuild, Context.User))
            {
                await success.sendSuccessTempAsync(Context.Channel, "Robberies", "You're a mechanic, I guess you can sit and watch?", Colours.generalCol);
            }
        }

        public async Task DisplayProfile(IUser user, IMessageChannel channel)
        {
            var avatar = user.GetAvatarUrl();
            var embed = new EmbedBuilder() { Color = Colours.generalCol };
            embed.WithAuthor(user.Username, avatar);
            embed.WithUrl(avatar);
            embed.Description = "Displaying users profile!";
            embed.WithThumbnailUrl(avatar);
            embed.AddField(new EmbedFieldBuilder() { Name = "Display Name", Value = Profile.Load(user.Id).PlayerNAME });
            embed.AddField(new EmbedFieldBuilder() { Name = "ID", Value = Profile.Load(user.Id).PlayerID, IsInline = true });
            embed.AddField(new EmbedFieldBuilder() { Name = "HEX", Value = Profile.Load(user.Id).PlayerHEX, IsInline = true });
            embed.AddField(new EmbedFieldBuilder() { Name = "Money", Value = "$"+Profile.Load(user.Id).Money });
            embed.AddField(new EmbedFieldBuilder() { Name = "Bank Robberies", Value = Profile.Load(user.Id).Attempts, IsInline = true });
            embed.AddField(new EmbedFieldBuilder() { Name = "Successful (Criminal)", Value = Profile.Load(user.Id).SuccessfulWins, IsInline = true });
            embed.AddField(new EmbedFieldBuilder() { Name = "Successful (Police)", Value = Profile.Load(user.Id).SuccessfulDefends, IsInline = true });
            embed.AddField(new EmbedFieldBuilder() { Name = "Revives (EMS)", Value = Profile.Load(user.Id).Revives, IsInline = true });
            embed.WithFooter("Displaying profile for " + user.Username);
            embed.WithCurrentTimestamp();

            await channel.SendMessageAsync("", false, embed);
        }

        private Profile UpdateProfile(ulong id, Profile profile)
        {
            profile.PlayerNAME = Profile.Load(id).PlayerNAME;
            profile.PlayerHEX = Profile.Load(id).PlayerHEX;
            profile.PlayerID = Profile.Load(id).PlayerID;
            profile.SuccessfulWins = Profile.Load(id).SuccessfulWins;
            profile.SuccessfulDefends = Profile.Load(id).SuccessfulDefends;
            profile.Revives = Profile.Load(id).Revives;
            profile.Attempts = Profile.Load(id).Attempts;
            profile.Money = Profile.Load(id).Money;

            return profile;
        }


    }
}
