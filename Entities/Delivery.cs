using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfDeliveryAPI.Entities
{
    public class Delivery
    {
        public int DeliveryId { get; set; }
        public DateTime Date { get; set; }
        public int [] OrderId { get; set; }
        public int VehicleId { get; set; }
    }
}
