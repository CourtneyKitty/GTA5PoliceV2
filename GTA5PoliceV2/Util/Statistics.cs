using System;
using System.Collections.Generic;
using System.Text;

namespace GTA5PoliceV2.Util
{
    class Statistics
    {
        private static int incomingMessages, outgoingMessages, commandRequests, timerMessages, statusChanges, errorsDetected, profanityDetected, admenRequests, oofMessages, metaMessages;

        public static int GetIncomingMessages() { return incomingMessages; }
        public static int GetOutgoingMessages() { return outgoingMessages; }
        public static int GetCommandRequests() { return commandRequests; }
        public static int GetTimerMessages() { return timerMessages; }
        public static int GetStatusChanges() { return statusChanges; }
        public static int GetErrorsDetected() { return errorsDetected; }
        public static int GetProfanityDetected() { return profanityDetected; }
        public static int GetAdmenRequests() { return admenRequests;  }
        public static int GetOofMessages() { return oofMessages; }
        public static int GetMetaMessages() { return metaMessages; }

        public static void AddIncomingMessages() { incomingMessages++; }
        public static void AddOutgoingMessages() { outgoingMessages++; }
        public static void AddCommandRequests() { commandRequests++; }
        public static void AddTimerMessages() { timerMessages++; }
        public static void AddStatusChanges() { statusChanges++; }
        public static void AddErrorsDetected() { errorsDetected++; }
        public static void AddProfanityDetected() { profanityDetected++; }
        public static void AddAdmenRequests() { admenRequests++; }
        public static void AddOofMessages() { oofMessages++; }
        public static void AddMetaMessages() { metaMessages++; }

        public static void ResetIncomingMessages() { incomingMessages = 0; }
        public static void ResetOutgoingMessages() { outgoingMessages = 0; }
        public static void ResetCommandRequests() { commandRequests = 0; }
        public static void ResetTimerMessages() { timerMessages = 0; }
        public static void ResetStatusChanges() { statusChanges = 0; }
        public static void ResetErrorsDetected() { errorsDetected = 0; }
        public static void ResetProfanityDetected() { profanityDetected = 0; }
        public static void ResetAdmenRequests() { admenRequests = 0; }
        public static void ResetOofMessages() { oofMessages = 0;  }
        public static void ResetMetaMessages() { metaMessages = 0; }
    }
}
