using Discord.WebSocket;
using GTA5PoliceV2.Config;
using GTA5PoliceV2.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GTA5PoliceV2.Suggestions
{
    class Suggestion
    {

        public static async Task HandleSuggestionAsync(SocketMessage pMsg)
        {
            Errors errors = new Errors();
            var message = pMsg as SocketUserMessage;
            var user = message.Author;

            if (message == null)
                return;
            if (user.IsBot)
                return;
            if (message.Channel.Id != DevConfig.Load().Suggestions)
                return;
            if (SuggestionCheck.IsDev(user) == true)
                return;
            if (SuggestionCheck.IsCorrectLayout(pMsg.ToString()) == true)
                return;

            await errors.sendErrorTempAsync(message.Channel, message.Author.Mention + " please use the correct message layout, it is pinned in this channel!", Colours.errorCol);
            var iDMChannel = await user.GetOrCreateDMChannelAsync();
            await iDMChannel.SendMessageAsync("Here is a copy of your suggestion that was in the wrong layout.\n```\n" + pMsg.ToString() + "\n```\nPlease use the layout that is pinned in the suggestions channel!");
            await pMsg.DeleteAsync();
        }

    }
}
