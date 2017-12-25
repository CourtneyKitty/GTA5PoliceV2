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

namespace GTA5PoliceV2.Connection
{
    class ServerStatus
    {
        //servers
        private Byte[] ip = new byte[] { 66, 150, 121, 131 };
        private int nyPort = 30150;
        private int laPort = 30141;
        private int nyWlPort = 30151;
        private int laWlPort = 30142;

        private bool mainStatus;
        private bool isNyLive;
        private bool isLaLive;
        private bool isNyWlLive;
        private bool isLaWlLive;

        public ServerStatus() { }

        /** Ping main server. If connection is good, try to connect to game servers. **/
        public void pingServers()
        {
            var g5pIP = "66.150.121.131";
            var ping = new Ping();
            var timeout = 60 * 1000; // 1 Minute timeout

            var reply = ping.Send(g5pIP, timeout);
            if (reply.Status.ToString().Equals("Success"))
            {
                mainStatus = true;
                pingServer(nyPort, 1);
                pingServer(laPort, 2);
                pingServer(nyWlPort, 3);
                pingServer(laWlPort, 4);
            }
            else
            {
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
                    Console.WriteLine("Successfully connected to server ID: " + id);
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
                    Console.WriteLine("Null exception when trying to connect to server id: " + id);
                    if (id == 1) isNyLive = false;
                    if (id == 2) isLaLive = false;
                    if (id == 3) isNyWlLive = false;
                    if (id == 4) isLaWlLive = false;
                }
                /** Socket exception **/
                catch (SocketException se)
                {
                    Console.WriteLine("Socket exception when trying to connect to server id: " + id);
                    if (id == 1) isNyLive = false;
                    if (id == 2) isLaLive = false;
                    if (id == 3) isNyWlLive = false;
                    if (id == 4) isLaWlLive = false;
                }
                /** Unexpected exception **/
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception when trying to connect to server id: " + id);
                    if (id == 1) isNyLive = false;
                    if (id == 2) isLaLive = false;
                    if (id == 3) isNyWlLive = false;
                    if (id == 4) isLaWlLive = false;
                }
            }
            /** Exception **/
            catch (Exception e)
            {
                Console.WriteLine("Exception when trying to connect to server id: " + id);
                if (id == 1) isNyLive = false;
                if (id == 2) isLaLive = false;
                if (id == 3) isNyWlLive = false;
                if (id == 4) isLaWlLive = false;
            }
        }

        /** Display the status of the servers in the text channel. **/
        public async Task displayStatus(IMessageChannel channel)
        {
            if (mainStatus)
            {
                String newyork, losangeles, newyorkWhitelist, losangelesWhitelist;
                if (isNyLive) newyork = "Online"; else newyork = "Offline";
                if (isLaLive) losangeles = "Online"; else losangeles = "Offline";
                if (isNyWlLive) newyorkWhitelist = "Online"; else newyorkWhitelist = "Offline";
                if (isLaWlLive) losangelesWhitelist = "Online"; else losangelesWhitelist = "Offline";



                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                embed.Title = "Current Status of GTA5Police Servers";
                var nyField = new EmbedFieldBuilder() { Name = "New York", Value = newyork };
                var laField = new EmbedFieldBuilder() { Name = "Los Angeles", Value = losangeles };
                var nyWlField = new EmbedFieldBuilder() { Name = "New York Whitelist", Value = newyorkWhitelist };
                var laWlField = new EmbedFieldBuilder() { Name = "Los Angeles Whitelist", Value = losangelesWhitelist };
                embed.AddField(nyField);
                embed.AddField(laField);
                embed.AddField(nyWlField);
                embed.AddField(laWlField);
                await channel.SendMessageAsync("", false, embed);
            }
            else
            {
                var embed = new EmbedBuilder() { Color = Colours.generalCol };
                embed.Title = "Current Status of GTA5Police Servers";
                embed.Description = "GTA5Police is currently down, check back later for an update!";
                await channel.SendMessageAsync("", false, embed);
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
