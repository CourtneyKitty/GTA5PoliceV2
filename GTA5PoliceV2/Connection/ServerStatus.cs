using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using Discord;
using Discord.Commands;
using GTA5PoliceV2.Util;
using GTA5PoliceV2.Config;

namespace GTA5PoliceV2.Connection
{
    class ServerStatus
    {
        //servers
        private Byte[] ip = new byte[] { 66, 150, 121, 131 };
        private int nyPort = ConnectionsConfig.Load().NyPort;
        private int laPort = ConnectionsConfig.Load().LaPort;
        private int nyWlPort = ConnectionsConfig.Load().NyWlPort;
        private int laWlPort = ConnectionsConfig.Load().LaWlPort;

        private bool mainStatus;
        private bool isNyLive;
        private bool isLaLive;
        private bool isNyWlLive;
        private bool isLaWlLive;

        /** Ping main server. If connection is good, try to connect to game servers. **/
        string g5pIP = ConnectionsConfig.Load().ServerIp;

        public void PingServers()
        {
            var ping = new Ping();
            
            var reply = ping.Send(g5pIP);
            if (reply != null)
            {
                mainStatus = true;
                pingServer(nyPort, 1);
                pingServer(laPort, 2);
                pingServer(nyWlPort, 3);
                pingServer(laWlPort, 4);
            }
            else
            {
                Program.Logger(new LogMessage(LogSeverity.Critical, "GTA5Police Connection", "Houston we have a problem!"));
                mainStatus = false;
            }
        }

        /** Connect to the server **/
        private void pingServer(int port, int id)
        {
            byte[] bytes = new byte[1024];

            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = new IPAddress(ip);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    sender.Connect(remoteEP);
                    Program.Logger(new LogMessage(LogSeverity.Debug, "GTA5Police Connection", "Successfully connected to server ID: " + id));
                    if (id == 1) isNyLive = true;
                    if (id == 2) isLaLive = true;
                    if (id == 3) isNyWlLive = true;
                    if (id == 4) isLaWlLive = true;


                    byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

                    int bytesSent = sender.Send(msg);

                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                /** Null exception **/
                catch (ArgumentNullException ane)
                {
                    Program.Logger(new LogMessage(LogSeverity.Error, "GTA5Police Connection", "Null exception when trying to connect to server id: " + id));
                    if (id == 1) isNyLive = false;
                    if (id == 2) isLaLive = false;
                    if (id == 3) isNyWlLive = false;
                    if (id == 4) isLaWlLive = false;
                }
                /** Socket exception **/
                catch (SocketException se)
                {
                    Program.Logger(new LogMessage(LogSeverity.Warning, "GTA5Police Connection", "Socket exception when trying to connect to server id: " + id));
                    if (id == 1) isNyLive = false;
                    if (id == 2) isLaLive = false;
                    if (id == 3) isNyWlLive = false;
                    if (id == 4) isLaWlLive = false;
                }
                /** Unexpected exception **/
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception when trying to connect to server id: " + id);
                    Program.Logger(new LogMessage(LogSeverity.Error, "GTA5Police Connection", "Unexpected exception when trying to connect to server id: " + id));
                    if (id == 1) isNyLive = false;
                    if (id == 2) isLaLive = false;
                    if (id == 3) isNyWlLive = false;
                    if (id == 4) isLaWlLive = false;
                }
            }
            /** Exception **/
            catch (Exception e)
            {
                Program.Logger(new LogMessage(LogSeverity.Error, "GTA5Police Connection", "Exception when trying to connect to server id: " + id));
                if (id == 1) isNyLive = false;
                if (id == 2) isLaLive = false;
                if (id == 3) isNyWlLive = false;
                if (id == 4) isLaWlLive = false;
            }
        }

        /** Display the status of the servers in the text channel. **/
        public async Task DisplayStatusAsync(IMessageChannel channel, IUser user)
        {
            if (mainStatus)
            {
                String newyork, losangeles, newyorkWhitelist, losangelesWhitelist;
                if (isNyLive) newyork = "Online using Port: " + nyPort; else newyork = "Offline";
                if (isLaLive) losangeles = "Online using Port: " + laPort; else losangeles = "Offline";
                if (isNyWlLive) newyorkWhitelist = "Online using Port: " + nyWlPort; else newyorkWhitelist = "Offline";
                if (isLaWlLive) losangelesWhitelist = "Online using Port: " + laWlPort; else losangelesWhitelist = "Offline";



                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                embed.WithAuthor("Current Server Status", References.GetGta5policeLogo());
                embed.WithDescription("IP: " + g5pIP + ":PORT");
                embed.WithThumbnailUrl(References.GetGta5policeLogo());
                embed.WithUrl("https://gta5police.com/panel/index.php");
                var nyField = new EmbedFieldBuilder() { Name = "New York", Value = newyork };
                var laField = new EmbedFieldBuilder() { Name = "Los Angeles", Value = losangeles };
                var nyWlField = new EmbedFieldBuilder() { Name = "New York Whitelist", Value = newyorkWhitelist };
                var laWlField = new EmbedFieldBuilder() { Name = "Los Angeles Whitelist", Value = losangelesWhitelist };
                embed.AddField(nyField);
                embed.AddField(laField);
                embed.AddField(nyWlField);
                embed.AddField(laWlField);
                embed.WithFooter(new EmbedFooterBuilder() { Text = "Requested by " + user } );
                embed.WithCurrentTimestamp();
                
                var message = await channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteEmbedAsync(message, (int)Cooldowns.GetCommandCooldown());
            }
            else
            {
                var embed = new EmbedBuilder() { Color = Colours.errorCol };
                embed.WithAuthor("Current Server Status", References.GetGta5policeLogo());
                embed.WithThumbnailUrl(References.GetGta5policeLogo());
                embed.WithUrl("https://gta5police.com/panel/index.php");
                embed.AddField(new EmbedFieldBuilder() { Name = "OFFLINE", Value = "GTA5Police servers are currently down, check back later for an update!" });
                embed.WithFooter(new EmbedFooterBuilder() { Text = "Requested by " + user });
                embed.WithCurrentTimestamp();
                var message = await channel.SendMessageAsync("", false, embed);
                await Delete.DelayDeleteEmbedAsync(message, (int)Cooldowns.GetCommandCooldown());

            }
        }

        /** Getters & Setters **/
        public bool getNyStatus()
        {
            return isNyLive;
        }
        public bool getLaStatus()
        {
            return isLaLive;
        }
        public bool getNyWlStatus()
        {
            return isNyWlLive;
        }
        public bool getLaWlStatus()
        {
            return isLaWlLive;
        }
    }
}
