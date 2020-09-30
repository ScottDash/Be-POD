using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProofOfDeliveryAPI.Entities;
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

        // POST api/customer
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
        {
            if (customer == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Customer createdCustomer = _customerService.AddCustomer(customer);
            return Created("order", createdCustomer);
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
    