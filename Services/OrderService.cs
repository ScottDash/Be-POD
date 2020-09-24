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
            new Order { OrderId = 1, CustomerCode = "abc001", PackageTotal = 6, Ranking = 1 }
        };

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            List<Order> orderList = new List<Order>();

            string sql = $"SELECT OrderId, CustomerCode, PackageTotal, Ranking FROM [{TableName}]";

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
                                orders.CustomerCode = reader.GetString(1);
                                orders.PackageTotal = reader.GetInt32(2);
                                orders.Ranking = reader.GetInt32(3);
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
                    command.CommandText = $"INSERT INTO [{TableName}] (PackageTotal, CustomerCode, Ranking) Values (@getpackage, @getcustomer, @getrank)";
                    command.Parameters.Add("@getpackage", SqlDbType.VarChar).Value = order.PackageTotal;
                    command.Parameters.Add("@getcustomer", SqlDbType.VarChar).Value = order.CustomerCode;
                    command.Parameters.Add("@getrank", SqlDbType.VarChar).Value = order.Ranking;               
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

