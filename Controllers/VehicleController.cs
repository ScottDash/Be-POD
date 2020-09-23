using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProofOfDeliveryAPI.Services;

namespace ProofOfDeliveryAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {

        private IVehicleService _vehicleService;
        private readonly ConnectionStrings _connectionStrings;

        public VehicleController(IOptions<ConnectionStrings> connectionStrings, IVehicleService vehicleService)
        {
            _connectionStrings = connectionStrings.Value;
            _vehicleService = vehicleService;
        }

        // POST api/vehicle/checklist
        [HttpPost("checklist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken)
        {
            bool isSaveSuccess = false;

            if (CheckIfPDF(file))
            {
                isSaveSuccess = await _vehicleService.WriteFile(file);
                if (!isSaveSuccess) return BadRequest(new { message = "Error saving file" });
            }
            else
            {
                return BadRequest(new { message = "Invalid file extension" });
            }

            return Ok($"File saved = {isSaveSuccess}");
        }

        //GET api/vehicle/checklist/{fileName}
        //[HttpGet("checklist/{fileName}")]
        //public async Task<IActionResult> ReadFile(string fileName)
        //{
        //    FileStream stream = await _vehicleService.ReadFile(fileName);
        //    if (stream == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        var result = File(stream, "application/pdf", fileName + ".pdf");
        //        return Ok(result.FileStream);
        //    }
        //}

        [Authorize]
        // GET api/vehicle/checklist{fileName}
        [HttpGet("checklist/{fileName}")]
        public async Task<IActionResult> GetByFileName(string fileName)
        {
            var vehicles = await _vehicleService.GetByFileName(fileName);
            return Ok(vehicles);
        }

        [Authorize]
        // GET api/vehicle/checklists
        [HttpGet("checklists")]
        public async Task<IActionResult> GetAll()
        {
            var vehicles = await _vehicleService.GetAllVehicleChecklists();
            return Ok(vehicles);
        }



        private bool CheckIfPDF(IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return (extension == ".pdf");
        }     
    }
}