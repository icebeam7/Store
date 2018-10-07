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
using Newtonsoft.Json;

namespace StoreWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Employees")]
    public class EmployeesController : Controller
    {
        private readonly StoreDBContext _context;

        public EmployeesController(StoreDBContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public IEnumerable<EmployeeDTO> GetEmployee()
        {
            return Mapper.Map<IEnumerable<EmployeeDTO>>(_context.Employee.OrderBy(x => x.LastName).ThenBy(x => x.FirstName));
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employee.SingleOrDefaultAsync(m => m.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<EmployeeDTO>(employee));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] dynamic credentials)
        {
            var username = (string)credentials["username"];
            var password = (string)credentials["password"];

            var employee = await _context.Employee.SingleOrDefaultAsync(m => m.UserName == username && m.Password == password);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<EmployeeDTO>(employee));
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee([FromRoute] int id, [FromBody] EmployeeDTO employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.Id)
            {
                return BadRequest();
            }

            _context.Entry(Mapper.Map<Employee>(employee)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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

        // POST: api/Employees
        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromBody] EmployeeDTO employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var e = Mapper.Map<Employee>(employee);
            _context.Employee.Add(e);
            await _context.SaveChangesAsync();
            employee.Id = e.Id;

            return CreatedAtAction("GetEmployee", new { id = e.Id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employee.SingleOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok(Mapper.Map<EmployeeDTO>(employee));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }
}