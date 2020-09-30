using Microsoft.Extensions.Options;
using ProofOfDeliveryAPI.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfDeliveryAPI.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrders();
        public Order AddOrder(Order order);
    }

    public class OrderService : IOrderService
    {
        private readonly ConnectionStrings _connectionStrings;
        public readonly string TableName = "Order";
        public OrderService(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
        }

        // order data hardcoded for initial testing
        private List<Order> _orders = new List<Order>
        {
            new Order { OrderId = 6, OrderNo = 12345, CustomerCode = "ABC001", PackageTotal = 6 },
            new Order { OrderId = 7, OrderNo = 67890, CustomerCode = "XYZ001", PackageTotal = 4 },
            new Order { OrderId = 8, OrderNo = 54321, CustomerCode = "LMN001", PackageTotal = 1 }
        };

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            List<Order> orderList = new List<Order>();

            string sql = $"SELECT OrderId, OrderNo, CustomerCode, PackageTotal, Ranking FROM [{TableName}]";

            try
            {
                using (var connection = new SqlConnection(_connectionStrings.PODTestDb))
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = sql;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (await reader.ReadAsync())
                            {
                                Order orders = new Order();
                                orders.OrderId = reader.GetInt32(0);
                                orders.OrderNo = reader.GetInt32(1);
                                orders.CustomerCode = reader.GetString(2);
                                orders.PackageTotal = reader.GetInt32(3);
                                orders.Ranking = reader.GetInt32(4);
                                orderList.Add(orders);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return orderList;
        }

        public Order AddOrder(Order order)
        {
            try
            {
                using var connection = new SqlConnection(_connectionStrings.PODTestDb);
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = $"INSERT INTO [{TableName}] (OrderNo, CustomerCode, PackageTotal, Ranking, ShippingDate, DeliveryId) Values (@getorder, @getcustomer, @getpackage, @getrank, @getdate, @getdelivery)";
                    command.Parameters.Add("@getorder", SqlDbType.Int).Value = order.OrderNo;
                    command.Parameters.Add("@getcustomer", SqlDbType.VarChar).Value = order.CustomerCode;
                    command.Parameters.Add("@getpackage", SqlDbType.Int).Value = order.PackageTotal;               
                    command.Parameters.Add("@getrank", SqlDbType.Int).Value = order.Ranking;
                    command.Parameters.Add("@getdate", SqlDbType.Date).Value = order.ShippingDate;              
                    command.Parameters.Add("@getdelivery", SqlDbType.Int).Value = order.DeliveryId;                                                     
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return order;
        }
    }
}

