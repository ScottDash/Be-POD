using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProofOfDeliveryAPI.Services;

namespace ProofOfDeliveryAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleChecklistController : ControllerBase
    {
        private IVehicleChecklistService _vehicleChecklistService;      

        public VehicleChecklistController(IVehicleChecklistService vehicleChecklistService)
        {
            _vehicleChecklistService = vehicleChecklistService;
        }

        // POST api/vehiclechecklist/{registration}
        [HttpPost("{registration}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFile(IFormFile file, string registration)
        {
            bool isSaveSuccess = false;
            bool isCreated = false;

            if (Helpers.ExtensionMethods.CheckIfPDF(file))
            {
                isSaveSuccess = await _vehicleChecklistService.WriteFile(file);
                if (!isSaveSuccess) return BadRequest(new { message = "Error saving file" });
                isCreated = await _vehicleChecklistService.AddVehicleChecklist(registration, file.FileName);
            }
            else
            {
                return BadRequest(new { message = "Invalid file extension" });
            }
            return Ok($"File saved = {isSaveSuccess}");
        }

        // GET api/IVehicleChecklistService
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var checklists = await _vehicleChecklistService.GetAllVehicleChecklists();
            return Ok(checklists);
        }

        // This is the old method, it has now been changed to read the pdf from local storage.
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

        // Front end now handles this
        //[Authorize]
        // GET api/vehicle/checklist/{fileName}
        //[HttpGet("checklist/{fileName}")]
        //public async Task<IActionResult> GetByFileName(string fileName)
        //{
        //    var vehicles = await _vehicleService.GetByFileName(fileName);
        //    return Ok(vehicles);
        //}
    }
}