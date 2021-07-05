using System;

namespace TONBRAINS.QUANTON.Core.DAL
{
    public class TransferLog
    {
        public string Id { get; set; }
        public string SmartAccountId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Token { get; set; }
        public long TransferBalance { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? SendingDate { get; set; }
        public string RecipientSmartAccountId { get; set; }
        public bool IsPaused { get; set; }
        public bool IsCanceled { get; set; }
        public string PaymentLogId { get; set; }
    }
}
