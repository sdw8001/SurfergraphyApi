using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SurfergraphyApi.Models
{
    public class Purchase
    {
        [Key]
        public string OrderId { get; set; }
        public string PackageName { get; set; }
        public string ProductId { get; set; }
        public long PurchaseTime { get; set; }
        public int PurchaseState { get; set; }
        public string DeveloperPayload { get; set; }
        public string PurchaseToken { get; set; }
    }
}