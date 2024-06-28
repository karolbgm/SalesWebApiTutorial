using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebApiTutorial.Data;
using SalesWebApiTutorial.Models;

namespace SalesWebApiTutorial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderlinesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderlinesController(AppDbContext context)
        {
            _context = context;
        }

        //GET api/Orderlines/Total/{id}
        //[HttpGet("Total/{orderId}")]
        //public async Task<ActionResult<Order>> GetOrderTotal(int orderId)
        //{
        //    var myOrder = await _context.Orders.FindAsync(orderId); //we read it, so it's in the cache

        //    if (myOrder == null)
        //    {
        //        return NotFound();
        //    }

        //    var total = await (from o in _context.Orders
        //                       join ol in _context.Orderlines on o.Id equals ol.OrderId
        //                       join i in _context.Items on ol.ItemId equals i.Id
        //                       where o.Id == orderId //INSTEAD OF DOING A LOOP, USE WHERE CLAUSE AND SELECT!!
        //                       select ol.Quantity * i.Price).SumAsync();

        //    myOrder.Total = total; //and the cache understands this was changed

        //    /*_context.Entry(myOrder).State = EntityState.Modified;*/ //we don't need to do this because we read order and it's in the cache
        //    await _context.SaveChangesAsync();

        //    return myOrder;
        //}

        //GREG'S METHOD
        //No Http Method - user don't call it 

        //this method will be inside ADD, UPDATE AND DELETE
        private async Task<IActionResult> RecalculateOrderTotal(int orderId) //it doesn't return data
            //use IACTIONRESULT when you don't want to return data
            //IActionResult is an interface, doesn't need a type and that's what we want because we don't want to return data!
            //ActionResult is a class, it needs a Type
            //method was private so the user can't access it directly
        {


            // read the order to be recalculated

            var order = await _context.Orders.FindAsync(orderId); //we have to read it so we can update it


            // if the order is not found, return NOT FOUND

            if (order is null)
            {

                return NotFound();

            }

            // if we get here, we did find the order

            // now calculate the total and store it in order.Total

            order.Total = (from ol in _context.Orderlines

                           join i in _context.Items

                             on ol.ItemId equals i.Id

                           where ol.OrderId == orderId

                           select new //the select helps us to select what piece of data we want, hence the TYPE we want
                           {

                               LineTotal = ol.Quantity * i.Price //it will store in this column called LineTotal -- we'll have a collection of LineTotal

                           }).Sum(x => x.LineTotal); //sum will return the line total of all of them together


            // now update the order with the new Total

            await _context.SaveChangesAsync();

            return Ok();

        }
        // GET: api/Orderlines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orderline>>> GetOrderlines()
        {
            return await _context.Orderlines.ToListAsync();
        }

        // GET: api/Orderlines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Orderline>> GetOrderline(int id)
        {
            var orderline = await _context.Orderlines.FindAsync(id);

            if (orderline == null)
            {
                return NotFound();
            }

            return orderline;
        }

        // PUT: api/Orderlines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderline(int id, Orderline orderline)
        {
            if (id != orderline.Id)
            {
                return BadRequest();
            }

            _context.Entry(orderline).State = EntityState.Modified;

            try //try catch is used here to make sure two people don't try to update it at the same time
            {
                await _context.SaveChangesAsync();
                await RecalculateOrderTotal(orderline.OrderId); //we're gonna recalculate the order after the user makes changes
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderlineExists(id))
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

        // POST: api/Orderlines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Orderline>> PostOrderline(Orderline orderline)
        {
            _context.Orderlines.Add(orderline);
            await _context.SaveChangesAsync();
            await RecalculateOrderTotal(orderline.OrderId);

            return CreatedAtAction("GetOrderline", new { id = orderline.Id }, orderline);
        }

        // DELETE: api/Orderlines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderline(int id)
        {
            var orderline = await _context.Orderlines.FindAsync(id);
            if (orderline == null)
            {
                return NotFound();
            }

            _context.Orderlines.Remove(orderline);
            await _context.SaveChangesAsync();
            await RecalculateOrderTotal(orderline.OrderId);

            return NoContent();
        }

        private bool OrderlineExists(int id)
        {
            return _context.Orderlines.Any(e => e.Id == id);
        }
    }
}
