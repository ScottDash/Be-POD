using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfDeliveryAPI.Entities
{
    public class Delivery
    {
        public int DeliveryId { get; set; }
        public int DeliveryNo { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int? VehicleId { get; set; }
        public int? UserId { get; set; }
    }
}