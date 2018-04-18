using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA5PoliceV2.Profiles
{

    /**
     * The role manager will run some checks when a player send a message.
     * If they are police, ems or mechanic, it will make sure they do not have the civ role.
     * If they are not police, ems or mechanic, it will make sure they do have the civ role.
     * This is part of the profiles module I am making.
     **/

    public class RoleManager
    {
        private static String[] civRoles = { "Civilian" };
        private static String[] policeRoles = { "Police Officer", "Police Sergeant", "Police Lieutenant", "Police Captain", "Police Asst. Chief", "Police Bureau Chief", "Police Chief" };
        private static String[] emsRoles = { "EMS Chief", "EMS Assistant Chief", "EMS Captain", "EMS Lieutenant", "EMS Doctor", "EMS Paramedic", "EMT", "EMR" };
        private static String[] mechanicRoles = { "Chief Mechanic", "Asst. Chief Mechanic", "Lead Mechanic", "Technician", "Probationary" };

        public static async Task SetRolesAsync(SocketMessage pMsg, IGuild guild)
        {
            var message = pMsg as SocketUserMessage;
            var user = message.Author as IGuildUser;
            if (user.IsBot) return;
            bool hasCiv = false, hasPolice = false, hasEMS = false, hasMechanic = false;
            for (int i = 0; i < civRoles.Length; i++)
            {
                var civRole = guild.Roles.FirstOrDefault(x => x.Name == civRoles[i]);
                ulong roleExist = 0;
                roleExist = user.RoleIds.FirstOrDefault(x => x.Equals(civRole.Id));

                if (roleExist != 0)
                {
                    // I think this means they have a civ role...
                    hasCiv = true;
                }
            }
            for (int i = 0; i < policeRoles.Length; i++)
            {
                var policeRole = guild.Roles.FirstOrDefault(x => x.Name == policeRoles[i]);
                ulong roleExist = 0;
                roleExist = user.RoleIds.FirstOrDefault(x => x.Equals(policeRole.Id));

                if (roleExist != 0)
                {
                    // I think this means they have a police role...
                    hasPolice = true;
                }
            }
            for (int i = 0; i < emsRoles.Length; i++)
            {
                var emsRole = guild.Roles.FirstOrDefault(x => x.Name == emsRoles[i]);
                ulong roleExist = 0;
                roleExist = user.RoleIds.FirstOrDefault(x => x.Equals(emsRole.Id));

                if (roleExist != 0)
                {
                    // I think this means they have a ems role...
                    hasEMS = true;
                }
            }
            for (int i = 0; i < mechanicRoles.Length; i++)
            {
                var mechanicRole = guild.Roles.FirstOrDefault(x => x.Name == mechanicRoles[i]);
                ulong roleExist = 0;
                roleExist = user.RoleIds.FirstOrDefault(x => x.Equals(mechanicRole.Id));

                if (roleExist != 0)
                {
                    // I think this means they have a mechanic role...
                    hasMechanic = true;
                }
            }

            if (hasPolice || hasEMS || hasMechanic)
            {
                if (hasCiv)
                {
                    var civRole = guild.Roles.FirstOrDefault(x => x.Name == "Civilian");
                    await user.RemoveRoleAsync(civRole);
                }
            }
            else
            {
                if (!hasCiv)
                {
                    var civRole = guild.Roles.FirstOrDefault(x => x.Name == "Civilian");
                    await user.AddRoleAsync(civRole);
                }
            }
        }
    }
}
