
namespace TONBRAINS.QUANTON.Grpc.Models
{
    public class PaymentLogModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Amount { get; set; }
        public string Nonce { get; set; }
        public string ClientData { get; set; }
        public string Status { get; set; }
    }
}
