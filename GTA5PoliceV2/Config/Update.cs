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
            config.CommandCooldown = BotConfig.Load().CommandCooldown;
            config.Commanders = BotConfig.Load().Commanders;
            config.Filters = BotConfig.Load().Filters;
            config.PoliceAdd = BotConfig.Load().PoliceAdd;
            config.EmsAdd = BotConfig.Load().EmsAdd;
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

        public static DevConfig UpdateDevConfig(DevConfig config)
        {
            config.DevReports = DevConfig.Load().DevReports;
            config.Devs = DevConfig.Load().Devs;
            for (int j = 0; j <= DevConfig.Load().Devs - 1; j++)
            {
                config.Developers[j] = DevConfig.Load().Developers[j];
            }
            return config;
        }

        public static RanksConfig UpdateRanksConfig(RanksConfig config)
        {
            config.EMSHighRanks = RanksConfig.Load().EMSHighRanks;
            for (int j = 0; j <= RanksConfig.Load().EMSHighRanks - 1; j++)
            {
                config.EMSHighRanksArray[j] = RanksConfig.Load().EMSHighRanksArray[j];
            }

            config.PDHighRanks = RanksConfig.Load().PDHighRanks;
            for (int j = 0; j <= RanksConfig.Load().PDHighRanks - 1; j++)
            {
                config.PDHighRanksArray[j] = RanksConfig.Load().PDHighRanksArray[j];
            }

            return config;
        }
    }
}
