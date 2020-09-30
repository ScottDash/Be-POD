using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfDeliveryAPI.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public int OrderNo { get; set; }
        public int PackageTotal { get; set; }
        public string CustomerCode { get; set; }
        public int Ranking { get; set; }
        public DateTime ShippingDate { get; set; }
        public int? DeliveryId { get; set; }
    }
}
