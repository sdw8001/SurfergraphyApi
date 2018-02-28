using System.ComponentModel.DataAnnotations;

namespace SurfergraphyApi.Models
{
    public class Purchase
    {
        [Key]
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public string ItemType { get; set; }
        public string PackageName { get; set; }
        public string Sku { get; set; }
        public long PurchaseTime { get; set; }
        public int PurchaseState { get; set; }
        public string DeveloperPayload { get; set; }
        public string PurchaseToken { get; set; }
        public string OriginalJson { get; set; }
        public string Signature { get; set; }
    }
}