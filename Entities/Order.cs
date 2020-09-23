using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfDeliveryAPI.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int Packages { get; set; }
        public int CustomerId { get; set; }
        public int Ranking { get; set; }
    }
}
