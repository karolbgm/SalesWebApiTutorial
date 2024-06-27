using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebApiTutorial.Data;
using SalesWebApiTutorial.Models;

namespace SalesWebApiTutorial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        //GET: api/Orders/new 
        //[HttpGet("new")]
        //public async Task<ActionResult<IEnumerable<Order>>> GetNewOrders()
        //{
        //    List<Order> myOrders = new List<Order>();
        //    var orders = await GetOrder();

        //    foreach (var order in orders.Value!)
        //    {
        //        if (order.Status == "NEW")
        //        {
        //            myOrders.Add(order);
        //        }

        //    }

        //    return myOrders;

        //}

        //GET: api/Orders/status/type
        [HttpGet("status/{type}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetNewOrders(string type)
        {
            return await _context.Orders.Where(x => x.Status == type.ToUpper()).ToListAsync(); //USE LINQ

            //List<Order> myOrders = new List<Order>();
            //var orders = await GetOrder();

            //foreach (var order in orders.Value!)
            //{
            //    if (order.Status == "NEW" && type == "new")
            //    {
            //        myOrders.Add(order);
            //    }
            //    if (order.Status == "SHIPPED" && type == "shipped")
            //    {
            //        myOrders.Add(order);
            //    }

            //}

            //return myOrders;

        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
        {
            return await _context.Orders.ToListAsync();
        }

        
        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        //PUT api/Orders/Shipped/5
        [HttpPut("shipped/{id}")]
        public async Task<IActionResult> ShippedOrder(int id, Order order)
        {
            order.Status = "SHIPPED";

            return await PutOrder(id, order);
            }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) //it will tell me I have to re-read data before updating it because it's been updated before
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            order.Status = "NEW"; 
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order); //re-reading it
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
