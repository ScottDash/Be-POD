using Microsoft.Extensions.Options;
using ProofOfDeliveryAPI.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfDeliveryAPI.Services
{
    public interface IDeliveryService
    {
        Task<IEnumerable<Delivery>> GetAllDeliveries();
        public Delivery AddDelivery(Delivery delivery);
    }

    public class DeliveryService : IDeliveryService
    {
        private readonly ConnectionStrings _connectionStrings;
        public readonly string TableName = "Delivery";
        public DeliveryService(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
        }

        // delivery data hardcoded for initial testing
        private List<Delivery> _deliveries = new List<Delivery>
        {
            new Delivery { DeliveryId = 1, DeliveryNo = 26, DeliveryDate = new DateTime(2020, 02, 01), VehicleId = 1, UserId = 1 }
        };

        public async Task<IEnumerable<Delivery>> GetAllDeliveries()
        {
            List<Delivery> deliveryList = new List<Delivery>();

            string sql = $"SELECT DeliveryId, DeliveryNo, DeliveryDate, VehicleId, UserId FROM [{TableName}]";

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
                                Delivery deliveries = new Delivery();
                                deliveries.DeliveryId = reader.GetInt32(0);
                                deliveries.DeliveryNo = reader.GetInt32(1);
                                deliveries.DeliveryDate = reader.IsDBNull(2) ? null : (DateTime?)reader.GetDateTime(2);
                                deliveries.VehicleId = reader.IsDBNull(3) ? null : (int?)reader.GetInt32(3);
                                deliveries.UserId = reader.GetInt32(4);
                                deliveryList.Add(deliveries);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return deliveryList;
        }

        public Delivery AddDelivery(Delivery delivery)
        {
            try
            {
                using var connection = new SqlConnection(_connectionStrings.PODTestDb);
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = $"INSERT INTO [{TableName}] (DeliveryNo, DeliveryDate, UserId) Values (@getdeliveryno, @getdeliverydate, @getuser)";
                    command.Parameters.Add("@getdeliveryno", SqlDbType.Int).Value = delivery.DeliveryNo;
                    command.Parameters.Add("@getdeliverydate", SqlDbType.DateTimeOffset).Value = delivery.DeliveryDate;
                    command.Parameters.Add("@getuser", SqlDbType.Int).Value = delivery.UserId;
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return delivery;
        }
    }
}
