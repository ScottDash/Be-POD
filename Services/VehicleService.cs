using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using ProofOfDeliveryAPI.Helpers;
using System.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using ProofOfDeliveryAPI.Entities;
using System.Linq;

namespace ProofOfDeliveryAPI.Services
{
    public interface IVehicleService
    {
        Task<bool> WriteFile(IFormFile file);
        Task<FileStream> ReadFile(string fileName);
        Task<IEnumerable<VehicleChecklist>> GetAllVehicleChecklists();
        Task<VehicleChecklist> GetByFileName(string fileName);
    }

    public class VehicleService : IVehicleService
    {
        public readonly IOptions<ConnectionStrings> _connectionStrings;

        public VehicleService(IOptions<ConnectionStrings> ConnectionStrings)  
        {
            _connectionStrings = ConnectionStrings;       
        }

        // vehicle data hardcoded for initial testing
        private List<VehicleChecklist> _vehicles = new List<VehicleChecklist>
        {
            new VehicleChecklist { Id = 1, Date = new DateTime(2020, 02, 01), Registration = "AA70PABC", FileName = "test1" },
            new VehicleChecklist { Id = 2, Date = new DateTime(2020, 02, 01), Registration = "BB70PABC", FileName = "test2" },
            new VehicleChecklist { Id = 3, Date = new DateTime(2020, 02, 01), Registration = "CC70PABC", FileName = "test3" },
            new VehicleChecklist { Id = 4, Date = new DateTime(2020, 02, 01), Registration = "DD70PABC", FileName = "test4" }
        };

        public async Task<bool> WriteFile(IFormFile file)
        {
            bool isSaveSuccess = false;
            string fileName = ExtensionMethods.RemoveWhitespace(file.FileName);
            
            try
            {              
                var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), _connectionStrings.Value.VehicleChecklistFilePath);

                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }

                var path = Path.Combine(Directory.GetCurrentDirectory(), _connectionStrings.Value.VehicleChecklistFilePath,
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
                var path = await Task.Run(() => Path.GetFullPath(_connectionStrings.Value.VehicleChecklistFilePath + fileName + ".pdf"));
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
            return _vehicles;
        }

        public async Task<VehicleChecklist> GetByFileName(string checkList)
        {
            return _vehicles.FirstOrDefault(fileName => fileName.Equals(checkList)); 
        }
    }
}
