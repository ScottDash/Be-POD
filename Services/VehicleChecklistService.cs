using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using ProofOfDeliveryAPI.Helpers;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using ProofOfDeliveryAPI.Entities;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace ProofOfDeliveryAPI.Services
{
    public interface IVehicleChecklistService
    {
        Task<bool> WriteFile(IFormFile file);
        Task<FileStream> ReadFile(string fileName);
        Task<IEnumerable<VehicleChecklist>> GetAllVehicleChecklists();
        Task<bool> AddVehicleChecklist(string registration, string filename);
    }

    public class VehicleChecklistService : IVehicleChecklistService
    {
        private readonly ConnectionStrings _connectionStrings;
        public readonly string TableName = "VehicleChecklist";

        public VehicleChecklistService(IOptions<ConnectionStrings> ConnectionStrings)  
        {
            _connectionStrings = ConnectionStrings.Value;       
        }

        // vehicle data hardcoded for initial testing
        private List<VehicleChecklist> _vehicles = new List<VehicleChecklist>
        {
            new VehicleChecklist { VehicleChecklistId = 1, Date = new DateTime(2020, 02, 01), Registration = "AA70PABC", FileName = "test1" },
            new VehicleChecklist { VehicleChecklistId = 2, Date = new DateTime(2020, 02, 01), Registration = "BB70PABC", FileName = "test2" },
            new VehicleChecklist { VehicleChecklistId = 3, Date = new DateTime(2020, 02, 01), Registration = "CC70PABC", FileName = "test3" },
            new VehicleChecklist { VehicleChecklistId = 4, Date = new DateTime(2020, 02, 01), Registration = "DD70PABC", FileName = "test4" }
        };

        public async Task<bool> WriteFile(IFormFile file)
        {
            bool isSaveSuccess = false;
            string fileName = ExtensionMethods.RemoveWhitespace(file.FileName);
            
            try
            {              
                var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), _connectionStrings.VehicleChecklistFilePath);

                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }

                var path = Path.Combine(Directory.GetCurrentDirectory(), _connectionStrings.VehicleChecklistFilePath,
                   fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                isSaveSuccess = true;

                
            }
            catch (Exception e)
            {
                throw new Exception($"Error saving file: {e}");
            }
            return isSaveSuccess;
        }

        public async Task<FileStream> ReadFile(string fileName)
        {
            try
            {              
                var path = await Task.Run(() => Path.GetFullPath(_connectionStrings.VehicleChecklistFilePath + fileName + ".pdf"));
                if (!File.Exists(path)) return null;
                return new FileStream(path, FileMode.Open, FileAccess.Read);
            }
            catch (Exception e)
            {
                throw new Exception($"Error saving file: {e}");
            }
        }


        public async Task<IEnumerable<VehicleChecklist>> GetAllVehicleChecklists()
        {

            List<VehicleChecklist> vehicleChecklist = new List<VehicleChecklist>();

            string sql = $"SELECT VehicleChecklistId, Date, Registration, FileName FROM [{TableName}]";

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
                                VehicleChecklist checklist = new VehicleChecklist();
                                checklist.VehicleChecklistId = reader.GetInt32(0);
                                checklist.Date = reader.GetDateTime(1);
                                checklist.Registration = reader.GetString(2);
                                checklist.FileName = reader.GetString(3);
                                vehicleChecklist.Add(checklist);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return vehicleChecklist;
        }


        public async Task<bool> AddVehicleChecklist(string registration, string filename)
        {
            var date = DateTime.Now.ToShortDateString();

            try
            {
                using var connection = new SqlConnection(_connectionStrings.PODTestDb);
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = $"INSERT INTO [VehicleChecklist] (Date, Registration, FileName) Values (@gdate, @registration, @filename)";
                    command.Parameters.Add("@gdate", SqlDbType.VarChar).Value = date;
                    command.Parameters.Add("@registration", SqlDbType.VarChar).Value = registration;
                    command.Parameters.Add("@filename", SqlDbType.VarChar).Value = filename;
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }
    }
}
