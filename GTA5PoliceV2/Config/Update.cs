using System;
using System.Collections.Generic;
using System.Text;

namespace GTA5PoliceV2.Config
{
    class Update
    {
        public static BotConfig UpdateConfig(BotConfig config)
        {
            config.Prefix = BotConfig.Load().Prefix;
            config.Token = BotConfig.Load().Token;
            config.ServerId = BotConfig.Load().ServerId;
            config.LogsId = BotConfig.Load().LogsId;
            config.TimerChannelId = BotConfig.Load().TimerChannelId;
            config.MessageTimerInterval = BotConfig.Load().MessageTimerInterval;
            config.StatusTimerInterval = BotConfig.Load().StatusTimerInterval;
            config.MessageTimerCooldown = BotConfig.Load().MessageTimerCooldown;
            config.Commanders = BotConfig.Load().Commanders;
            config.Filters = BotConfig.Load().Filters;
            for (int j = 0; j <= BotConfig.Load().Commanders - 1; j++)
            {
                config.BotCommanders[j] = BotConfig.Load().BotCommanders[j];
            }
            for (int j = 0; j <= BotConfig.Load().Filters - 1; j++)
            {
                config.FilteredWords[j] = BotConfig.Load().FilteredWords[j];
            }
            return config;
        }
    }
}
