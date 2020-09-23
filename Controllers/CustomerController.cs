using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CustomerController : ControllerBase
    {
        private ICustomerService _customerService;
        private readonly ConnectionStrings _connectionStrings;

        public CustomerController(IOptions<ConnectionStrings> connectionStrings, ICustomerService customerService)
        {
            _connectionStrings = connectionStrings.Value;
            _customerService = customerService;
        }

        // GET api/customer
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _customerService.GetAllCustomers();  
            return Ok(customers);           
        }
    }
}
    