﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfDeliveryAPI.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string PostCode { get; set; }
        public string MobileNo { get; set; }
    }
}
