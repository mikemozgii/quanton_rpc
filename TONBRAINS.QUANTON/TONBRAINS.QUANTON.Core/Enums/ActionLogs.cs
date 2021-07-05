using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TONBRAINS.QUANTON.Core.Enums
{
    public static class ActionLogs
    {
        public static string signIn { get; set; } = "sigIn";
        public static string registerNewUser { get; set; } = "registerNewUser";
        public static string initTransferDeaction { get; set; } = "initTransferDeaction";
        public static string deleteTransferDeaction { get; set; } = "deleteTransferDeaction";
        public static string pauseTransferDeaction { get; set; } = "pauseTransferDeaction";
        public static string resumeTransferDeaction { get; set; } = "resumeTransferDeaction";
        public static string completeTransferDeaction { get; set; } = "completeTransferDeaction";
        public static string initPayment { get; set; } = "initPayment";
    }
}
