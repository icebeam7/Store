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

namespace StoreWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/CustomerOrders")]
    public class CustomerOrdersController : Controller
    {
        private readonly StoreDBContext _context;

        public CustomerOrdersController(StoreDBContext context)
        {
            _context = context;
        }

        // GET: api/CustomerOrders
        [HttpGet]
        public IEnumerable<CustomerOrderDTO> GetCustomerOrder()
        {
            return Mapper.Map<IEnumerable<CustomerOrderDTO>>(_context.CustomerOrder.OrderByDescending(x => x.Date));
        }

        // GET: api/CustomerOrders/Customer/5
        [HttpGet("Customer/{customerId}")]
        public IEnumerable<CustomerOrderDTO> GetCustomer_CustomerOrder([FromRoute] int customerId)
        {
            return Mapper.Map<IEnumerable<CustomerOrderDTO>>(_context.CustomerOrder
                .Include(x => x.OrderStatus)
                .Where(x => x.CustomerId == customerId)
                .OrderByDescending(x => x.Date));
        }

        // GET: api/CustomerOrders/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerOrder([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerOrder = await _context.CustomerOrder.SingleOrDefaultAsync(m => m.Id == id);

            if (customerOrder == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<CustomerOrderDTO>(customerOrder));
        }

        // PUT: api/CustomerOrders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerOrder([FromRoute] int id, [FromBody] CustomerOrderDTO customerOrder)
        {
            customerOrder.OrderDetail = null;
            customerOrder.OrderStatus = null;
            customerOrder.Customer = null;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customerOrder.Id)
            {
                return BadRequest();
            }

            _context.Entry(Mapper.Map<CustomerOrder>(customerOrder)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerOrderExists(id))
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

        // POST: api/CustomerOrders
        [HttpPost]
        public async Task<IActionResult> PostCustomerOrder([FromBody] CustomerOrderDTO customerOrder)
        {
            customerOrder.OrderDetail = null;
            customerOrder.OrderStatus = null;
            customerOrder.Customer = null;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var co = Mapper.Map<CustomerOrder>(customerOrder);
            _context.CustomerOrder.Add(co);
            await _context.SaveChangesAsync();
            customerOrder.Id = co.Id;

            return CreatedAtAction("GetCustomerOrder", new { id = co.Id }, customerOrder);
        }

        // DELETE: api/CustomerOrders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerOrder([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerOrder = await _context.CustomerOrder.SingleOrDefaultAsync(m => m.Id == id);
            if (customerOrder == null)
            {
                return NotFound();
            }

            _context.CustomerOrder.Remove(customerOrder);
            await _context.SaveChangesAsync();

            return Ok(Mapper.Map<CustomerOrderDTO>(customerOrder));
        }

        private bool CustomerOrderExists(int id)
        {
            return _context.CustomerOrder.Any(e => e.Id == id);
        }
    }
}