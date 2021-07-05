using System;
namespace TONBRAINS.QUANTON.Core.DAL
{
    public class TransferLogView
    {
        public string Id { get; set; }
        public string SmartAccountId { get; set; }
        public DateTime Date { get; set; }
        public long TransferBalance { get; set; }
        public string Status { get; set; }
        public string ToValue { get; set; }
        public bool IsPaused { get; set; }
        public bool IsCanceled { get; set; }
    }
}
