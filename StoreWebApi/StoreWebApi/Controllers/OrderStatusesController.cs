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
    [Route("api/OrderStatuses")]
    public class OrderStatusesController : Controller
    {
        private readonly StoreDBContext _context;

        public OrderStatusesController(StoreDBContext context)
        {
            _context = context;
        }

        // GET: api/OrderStatuses
        [HttpGet]
        public IEnumerable<OrderStatusDTO> GetOrderStatus()
        {
            return Mapper.Map<IEnumerable<OrderStatusDTO>>(_context.OrderStatus.OrderBy(x => x.Name));
        }

        // GET: api/OrderStatuses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderStatus([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderStatus = await _context.OrderStatus.SingleOrDefaultAsync(m => m.Id == id);

            if (orderStatus == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<OrderStatusDTO>(orderStatus));
        }

        // PUT: api/OrderStatuses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderStatus([FromRoute] int id, [FromBody] OrderStatusDTO orderStatus)
        {
            orderStatus.CustomerOrder = null;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != orderStatus.Id)
            {
                return BadRequest();
            }

            _context.Entry(Mapper.Map<OrderStatus>(orderStatus)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderStatusExists(id))
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

        // POST: api/OrderStatuses
        [HttpPost]
        public async Task<IActionResult> PostOrderStatus([FromBody] OrderStatusDTO orderStatus)
        {
            orderStatus.CustomerOrder = null;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var os = Mapper.Map<OrderStatus>(orderStatus);

            _context.OrderStatus.Add(os);
            await _context.SaveChangesAsync();
            orderStatus.Id = os.Id;

            return CreatedAtAction("GetOrderStatus", new { id = os.Id }, orderStatus);
        }

        // DELETE: api/OrderStatuses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderStatus([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderStatus = await _context.OrderStatus.SingleOrDefaultAsync(m => m.Id == id);
            if (orderStatus == null)
            {
                return NotFound();
            }

            _context.OrderStatus.Remove(orderStatus);
            await _context.SaveChangesAsync();

            return Ok(Mapper.Map<OrderStatusDTO>(orderStatus));
        }

        private bool OrderStatusExists(int id)
        {
            return _context.OrderStatus.Any(e => e.Id == id);
        }
    }
}