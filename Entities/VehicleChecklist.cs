using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfDeliveryAPI.Entities
{
    public class VehicleChecklist
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Registration { get; set; }
        public string FileName { get; set; }
    }
}
