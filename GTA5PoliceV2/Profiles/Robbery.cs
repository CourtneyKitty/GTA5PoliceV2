using Discord;
using GTA5PoliceV2.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GTA5PoliceV2.Profiles
{
    class Robbery
    {
        public static int invested;

        public static async Task addRobberAsync(ulong robber, int investment)
        {
            Robberies robberies = new Robberies();
            robberies.LeaderCriminal = Robberies.Load().LeaderCriminal;
            robberies.LeaderCriminalPoints = Robberies.Load().LeaderCriminalPoints;
            robberies.LeaderPolice = Robberies.Load().LeaderPolice;
            robberies.LeaderPolicePoints = Robberies.Load().LeaderPolicePoints;
            robberies.LeaderEMS = Robberies.Load().LeaderEMS;
            robberies.LeaderEMSPoints = Robberies.Load().LeaderEMSPoints;
            robberies.RobberyChannel = Robberies.Load().RobberyChannel;
            robberies.CurrentCriminals = Robberies.Load().CurrentCriminals;
            robberies.CurrentCriminals = Util.Array.Insert(robberies.CurrentCriminals, robber);
            robberies.CurrentPolice = Robberies.Load().CurrentPolice;
            robberies.CurrentEMS = Robberies.Load().CurrentEMS;
            invested = invested + investment;
            robberies.Save();
        }

        public static async Task addPoliceAsync(ulong cop)
        {
            Robberies robberies = new Robberies();
            robberies.LeaderCriminal = Robberies.Load().LeaderCriminal;
            robberies.LeaderCriminalPoints = Robberies.Load().LeaderCriminalPoints;
            robberies.LeaderPolice = Robberies.Load().LeaderPolice;
            robberies.LeaderPolicePoints = Robberies.Load().LeaderPolicePoints;
            robberies.LeaderEMS = Robberies.Load().LeaderEMS;
            robberies.LeaderEMSPoints = Robberies.Load().LeaderEMSPoints;
            robberies.RobberyChannel = Robberies.Load().RobberyChannel;
            robberies.CurrentCriminals = Robberies.Load().CurrentCriminals;
            robberies.CurrentPolice = Robberies.Load().CurrentPolice;
            robberies.CurrentPolice = Util.Array.Insert(robberies.CurrentPolice, cop);
            robberies.CurrentEMS = Robberies.Load().CurrentEMS;
            robberies.Save();
        }

        public static async Task addEmsAsync(ulong medic)
        {
            Robberies robberies = new Robberies();
            robberies.LeaderCriminal = Robberies.Load().LeaderCriminal;
            robberies.LeaderCriminalPoints = Robberies.Load().LeaderCriminalPoints;
            robberies.LeaderPolice = Robberies.Load().LeaderPolice;
            robberies.LeaderPolicePoints = Robberies.Load().LeaderPolicePoints;
            robberies.LeaderEMS = Robberies.Load().LeaderEMS;
            robberies.LeaderEMSPoints = Robberies.Load().LeaderEMSPoints;
            robberies.RobberyChannel = Robberies.Load().RobberyChannel;
            robberies.CurrentCriminals = Robberies.Load().CurrentCriminals;
            robberies.CurrentPolice = Robberies.Load().CurrentPolice;
            robberies.CurrentEMS = Robberies.Load().CurrentEMS;
            robberies.CurrentEMS = Util.Array.Insert(robberies.CurrentEMS, medic);
            robberies.Save();
        }

        public static async Task drawWinner(IGuild guild, IGuildChannel channel)
        {
            Random rnd = new Random();
            int i = invested;
            int c = Robberies.Load().CurrentCriminals.Length;
            int cp = c + rnd.Next(1, 20);
            int p = Robberies.Load().CurrentPolice.Length;
            int pp = p / (invested / 200000);
            int e = Robberies.Load().CurrentEMS.Length;
            int ep = e;
            int pep = pp + ep + rnd.Next(1, 15);

            bool winner;
            if (cp > pep)
            {
                winner = true;
            }
            else if (cp > pep)
            {
                winner = false;
            }
            else
            {
                winner = false;
            }

            if (winner)
            {
                string winners = "";
                for (int a = 0; a < Robberies.Load().CurrentCriminals.Length; a++)
                {
                    winners = winners + ",\n" + Profile.Load(Robberies.Load().CurrentCriminals[a]).PlayerNAME;
                    // ADD WINNER POINTS YASSS
                    // ADD MONEY SHARES TO PROFILE
                }
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                embed.WithAuthor("Robbers Won!", References.GetGta5policeLogo());
                embed.WithUrl(References.GetDashboardURL());
                embed.Description = "The better men won!";
                embed.WithThumbnailUrl(References.GetGta5policeLogo());
                embed.AddField(new EmbedFieldBuilder() { Name = "Winners", Value = winners });
                embed.AddField(new EmbedFieldBuilder() { Name = "Money Invested", Value = invested });
                embed.AddField(new EmbedFieldBuilder() { Name = "Money Won (Per Criminal)", Value = (invested * 2) / c });
                embed.WithFooter("Robbery Over...");
                embed.WithCurrentTimestamp();

                await (channel as IMessageChannel).SendMessageAsync("", false, embed);
            }
            else if (!winner)
            {
                string winners = "";
                for (int a = 0; a < Robberies.Load().CurrentPolice.Length; a++)
                {
                    winners = winners + ",\n" + Profile.Load(Robberies.Load().CurrentPolice[a]).PlayerNAME;
                    // ADD WINNER POINTS YASSS
                    // ADD MONEY SHARES TO PROFILE
                }
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                embed.WithAuthor("Police Won!", References.GetGta5policeLogo());
                embed.WithUrl(References.GetDashboardURL());
                embed.Description = "The OP defences won!";
                embed.WithThumbnailUrl(References.GetGta5policeLogo());
                embed.AddField(new EmbedFieldBuilder() { Name = "Winners", Value = winners });
                embed.AddField(new EmbedFieldBuilder() { Name = "Money Invested", Value = invested });
                embed.AddField(new EmbedFieldBuilder() { Name = "Money Won (Per Cop)", Value = invested / p });
                embed.WithFooter("Robbery Over...");
                embed.WithCurrentTimestamp();

                await (channel as IMessageChannel).SendMessageAsync("", false, embed);
            }
        }

        public static async Task resetRobberyAsync()
        {
            Robberies robberies = new Robberies();
            robberies.LeaderCriminal = Robberies.Load().LeaderCriminal;
            robberies.LeaderCriminalPoints = Robberies.Load().LeaderCriminalPoints;
            robberies.LeaderPolice = Robberies.Load().LeaderPolice;
            robberies.LeaderPolicePoints = Robberies.Load().LeaderPolicePoints;
            robberies.LeaderEMS = Robberies.Load().LeaderEMS;
            robberies.LeaderEMSPoints = Robberies.Load().LeaderEMSPoints;
            robberies.RobberyChannel = Robberies.Load().RobberyChannel;
            robberies.CurrentCriminals = null;
            robberies.CurrentPolice = null;
            robberies.CurrentEMS = null;
            robberies.Save();
        }
    }
}
