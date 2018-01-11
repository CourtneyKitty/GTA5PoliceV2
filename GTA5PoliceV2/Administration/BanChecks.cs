using System;
using System.Collections.Generic;
using System.Text;

namespace GTA5PoliceV2.Administration
{
    class BanChecks
    {
        private static bool isCommandBan = false;

        public static bool GetIsCommandBan()
        {
            return isCommandBan;
        }

        public static void SetIsCommandBan()
        {
            isCommandBan = true;
        }

        public static void ResetIsCommandBan()
        {
            isCommandBan = false;
        }
    }
}
