using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using ProofOfDeliveryAPI.Helpers;
using System.Configuration;
using Microsoft.Extensions.Options;

namespace ProofOfDeliveryAPI.Services
{
    public interface IVehicleService
    {
        Task<bool> WriteFile(IFormFile file);
        Task<FileStream> ReadFile(string fileName);
    }

    public class VehicleService : IVehicleService
    {
        public readonly IOptions<ConnectionStrings> _connectionStrings;

        public VehicleService(IOptions<ConnectionStrings> ConnectionStrings)  
        {
            _connectionStrings = ConnectionStrings;       
        }

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
                var path = await Task.Run(() => Path.GetFullPath(@"Data\uploads\checklists\" + fileName + ".pdf"));
                if (!File.Exists(path)) return null;
                return new FileStream(path, FileMode.Open, FileAccess.Read);
            }
            catch (Exception e)
            {
                throw new Exception($"Error saving file: {e}");
            }
        }
    }
}
