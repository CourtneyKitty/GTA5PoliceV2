using Discord;
using GTA5PoliceV2.Config;
using GTA5PoliceV2.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GTA5PoliceV2.Connection
{
    class AutoBan
    {
        public static async Task AutoBanAsync(IGuild guild, IUser user)
        {
            await guild.AddBanAsync(user, 7, "Auto ban system detected them join and banned them.");

            int banDay = DateTime.Now.Day;
            int banMonth = DateTime.Now.Month;
            int banYear = DateTime.Now.Year;
            TimeSpan banTime = DateTime.Now.TimeOfDay;

            var embed = new EmbedBuilder() { Color = Colours.errorCol };
            embed.WithAuthor("User was banned from Discord");
            embed.WithThumbnailUrl(References.GetGta5policeLogo());
            embed.AddField(new EmbedFieldBuilder() { Name = "Discord User", Value = user.Username.ToString(), IsInline = true });
            embed.AddField(new EmbedFieldBuilder() { Name = "Discord Id", Value = user.Id, IsInline = true });
            embed.AddField(new EmbedFieldBuilder() { Name = "Reason", Value = "Auto-Ban Detected User Join Discord.", IsInline = false });
            embed.AddField(new EmbedFieldBuilder() { Name = "Banned By", Value = "Auto-Ban", IsInline = true });
            embed.AddField(new EmbedFieldBuilder() { Name = "Time", Value = banMonth + "/" + banDay + "/" + banYear + " - " + banTime, IsInline = true });

            var chan = await guild.GetTextChannelAsync(BotConfig.Load().LogsId);
            await chan.SendMessageAsync("", false, embed);
        }
    }
}
