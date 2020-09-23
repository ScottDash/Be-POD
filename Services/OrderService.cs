using ProofOfDeliveryAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfDeliveryAPI.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrders();
    }

    public class OrderService : IOrderService
    {
        // order data hardcoded for initial testing
        private List<Order> _orders = new List<Order>
        {
            new Order { Id = 1, CustomerId = 3, Packages = 6, Ranking = 1 }
        };


        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return _orders;
        }
    }
}
