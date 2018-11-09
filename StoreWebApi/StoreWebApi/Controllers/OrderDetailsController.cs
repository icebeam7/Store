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
    [Route("api/OrderDetails")]
    public class OrderDetailsController : Controller
    {
        private readonly StoreDBContext _context;

        public OrderDetailsController(StoreDBContext context)
        {
            _context = context;
        }

        // GET: api/OrderDetails
        [HttpGet]
        public IEnumerable<OrderDetail> GetOrderDetail()
        {
            return _context.OrderDetail;
        }

        // GET: api/OrderDetails/Order/5
        [HttpGet("Order/{orderId}")]
        public async Task<IActionResult> GetOrder_OrderDetail([FromRoute] int orderId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderDetail = _context.OrderDetail
                .Include(x => x.Product)
                .Where(m => m.CustomerOrderId == orderId);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<IEnumerable<OrderDetailDTO>>(orderDetail));
        }

        // POST: api/OrderDetails
        [HttpPost]
        public async Task<IActionResult> PostOrderDetail([FromBody] List<OrderDetailDTO> orderDetail)
        {
            foreach (var item in orderDetail)
            {
                item.CustomerOrder = null;
                item.Product = null;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var od = Mapper.Map<IEnumerable<OrderDetail>>(orderDetail);
            _context.OrderDetail.AddRange(od);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderDetail", new { id = od.First().CustomerOrderId }, orderDetail);
        }

        // DELETE: api/OrderDetails/5
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrderDetail([FromRoute] int orderId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderDetail = _context.OrderDetail.Where(m => m.CustomerOrderId == orderId);
            if (orderDetail == null)
            {
                return NotFound();
            }

            _context.OrderDetail.RemoveRange(orderDetail);
            await _context.SaveChangesAsync();

            return Ok(orderDetail);
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetail.Any(e => e.CustomerOrderId == id);
        }
    }
}