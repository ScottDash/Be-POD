using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProofOfDeliveryAPI.Entities;
using ProofOfDeliveryAPI.Services;

namespace ProofOfDeliveryAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }
        
        // POST api/vehicle
        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] Vehicle vehicle)
        {
            if (vehicle == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Vehicle createdVehicle = _vehicleService.AddVehicle(vehicle);
            return Created("vehicle", createdVehicle);
        }

        // GET api/vehicle
        [HttpGet]
        public async Task<IActionResult> GetAllVehicles()
        {
            var orders = await _vehicleService.GetAllVehicles();
            return Ok(orders);
        }
    }
}