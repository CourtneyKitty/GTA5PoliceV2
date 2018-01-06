using Discord.WebSocket;
using GTA5PoliceV2.Config;
using GTA5PoliceV2.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GTA5PoliceV2.DevReports
{
    class Reports
    {

        public static async Task HandleReport(SocketMessage pMsg)
        {
            Errors errors = new Errors();
            var message = pMsg as SocketUserMessage;
            var user = message.Author;

            if (message == null)
                return;
            if (user.IsBot)
                return;
            if (message.Channel.Id != DevConfig.Load().DevReports)
                return;
            if (ReportChecks.IsDev(user) == true)
                return;
            if (ReportChecks.IsCorrectLayout(pMsg.ToString()) == true)
                return;

            await errors.sendErrorTemp(message.Channel, message.Author.Mention + " please use the correct message layout, it is pinned in this channel!", Colours.errorCol);
            var iDMChannel = await user.GetOrCreateDMChannelAsync();
            await iDMChannel.SendMessageAsync("Here is a copy of your bug report that was the wrong layout.\n```\n" + pMsg.ToString() + "\n```\nPlease use the layout that is pinned in the bug reports channel!");
            await pMsg.DeleteAsync();
        }
    }
}
