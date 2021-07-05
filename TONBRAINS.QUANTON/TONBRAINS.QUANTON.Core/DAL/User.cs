using System;

namespace TONBRAINS.QUANTON.Core.DAL
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ExternalId { get; set; }
        public string ExternalData { get; set; }
        public string SmartAccountId { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
