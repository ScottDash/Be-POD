using ProofOfDeliveryAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfDeliveryAPI.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomers();
    }

    public class CustomerService : ICustomerService
    {
        // customer data hardcoded for initial testing
        private List<Customer> _customers = new List<Customer>
        {
            new Customer { Id = 1, PostCode = "BB72FL", MobileNo = "078934348136" },
            new Customer { Id = 2, PostCode = "BB99ST", MobileNo = "078934348139" },
            new Customer { Id = 3, PostCode = "BB55SA", MobileNo = "078934348189" },
            new Customer { Id = 4, PostCode = "BB72QN", MobileNo = "078934348190" }
        };

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            return _customers;
        }
    }
}
