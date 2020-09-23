//using ProofOfDeliveryAPI.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ProofOfDeliveryAPI.Services
//{
//    public interface IDeliveryService
//    {
//        Task<IEnumerable<Delivery>> GetAllDeliveries();
//    }

//    public class DeliveryService : IDeliveryService
//    {
//        // delivery data hardcoded for initial testing
//        private List<Delivery> _deliveries = new List<Delivery>
//        {
//            //new Delivery { Id = 1, Date = new DateTime(2020, 02, 01), OrderId = [1, 2], VehicleId = 1 },
            
//            //new Delivery { Id = 2, Date = new DateTime(2020, 02, 01), OrderId = 4, VehicleId = 2 },
//            //new Delivery { Id = 3, Date = new DateTime(2020, 02, 01), OrderId = 3 , VehicleId = 3 },
//            //new Delivery { Id = 4, Date = new DateTime(2020, 02, 01), OrderId = 2 , VehicleId = 4 }
//        };

//        public async Task<IEnumerable<Delivery>> GetAllDeliveries()
//        {
//            return _deliveries;
//        }
//    }
//}
