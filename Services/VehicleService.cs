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
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicle>> GetAllVehicles();
        public Vehicle AddVehicle(Vehicle vehicle);
    }

    public class VehicleService : IVehicleService
    {
        private readonly ConnectionStrings _connectionStrings;
        public readonly string TableName = "Vehicle";

        public VehicleService(IOptions<ConnectionStrings> ConnectionStrings)
        {
            _connectionStrings = ConnectionStrings.Value;
        }

        // vehicle data hardcoded for initial testing
        private List<Vehicle> _users = new List<Vehicle>
        {
            new Vehicle { VehicleId = 1, Registration = "AA70PABC"}
        };

        public async Task<IEnumerable<Vehicle>> GetAllVehicles()
        {
            List<Vehicle> vehicleList = new List<Vehicle>();

            string sql = $"SELECT VehicleId, Registration FROM [{TableName}]";

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
                                Vehicle vehicles = new Vehicle();
                                vehicles.VehicleId = reader.GetInt32(0);
                                vehicles.Registration = reader.GetString(1);
                                vehicleList.Add(vehicles);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return vehicleList;
        }

        public Vehicle AddVehicle(Vehicle vehicle)
        {
            try
            {
                using var connection = new SqlConnection(_connectionStrings.PODTestDb);
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = $"INSERT INTO [{TableName}] (Registration) Values (@getreg)";
                    command.Parameters.Add("@getreg", SqlDbType.VarChar).Value = vehicle.Registration;                 
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return vehicle;
        }
    }
}
