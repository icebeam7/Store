using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreWebApi.Models;
using AutoMapper;
using StoreWebApi.DTOs;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace StoreWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Customers")]
    public class CustomersController : Controller
    {
        private readonly StoreDBContext _context;

        public CustomersController(StoreDBContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public IEnumerable<CustomerDTO> GetCustomer()
        {
            return Mapper.Map<IEnumerable<CustomerDTO>>(_context.Customer.OrderBy(x => x.LastName).ThenBy(x => x.FirstName));
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Customer.SingleOrDefaultAsync(m => m.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<CustomerDTO>(customer));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] dynamic credentials)
        {
            var username = (string)credentials["username"];
            var password = (string)credentials["password"];

            var customer = await _context.Customer.SingleOrDefaultAsync(m => m.UserName == username && m.Password == password);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<CustomerDTO>(customer));
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer([FromRoute] int id, [FromBody] CustomerDTO customer)
        {
            customer.CustomerOrder = null;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.Id)
            {
                return BadRequest();
            }

            _context.Entry(Mapper.Map<Customer>(customer)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<IActionResult> PostCustomer([FromBody] CustomerDTO customer)
        {
            customer.CustomerOrder = null;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var c = Mapper.Map<Customer>(customer);
            _context.Customer.Add(c);
            await _context.SaveChangesAsync();
            customer.Id = c.Id;

            return CreatedAtAction("GetCustomer", new { id = c.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Customer.SingleOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok(Mapper.Map<CustomerDTO>(customer));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.Id == id);
        }
    }
}