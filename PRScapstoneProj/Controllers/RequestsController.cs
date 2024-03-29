﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRScapstoneProj;
using PRScapstoneProj.Models;

namespace PRScapstoneProj.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase {

        private readonly CapDBContext _context;

        public RequestsController(CapDBContext context) {
            _context = context;
        }
       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Requests>>> GetRequests() 
         {
            return await _context.Request.ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Requests>> GetRequest(int id) {

            var requests = await _context.Request.FindAsync(id);

            if (requests == null)
            {
                return NotFound();
            }

            return requests;
        }
        [HttpPut("review/{id}")]
        public async Task<IActionResult> PutStatusReview(int id,Requests request) {

            request.Status = "REVIEW";
            if (request.Total <= 50)
            {
                request.Status = RequestApproved;
            }
            return await PutRequests(id, request);
        }

        [HttpPut("approve/{id}")]
        public async Task<IActionResult> PutStatusApprove(int id, Requests request) {

            request.Status = "APPROVED";
            return await PutRequests(id, request);
        }

        [HttpPut("reject/{id}")]
        public async Task<IActionResult> PutStatusReject(int id, Requests request) {

            request.Status = "REJECTED";
            return await PutRequests(id, request);
        }

        // PUT: api/Requests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequests(int id, Requests requests) {
            if (id != requests.Id)
            {
                return BadRequest();
            }

            _context.Entry(requests).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestsExists(id))
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

        // POST: api/Requests
        [HttpPost]
        public async Task<ActionResult<Requests>> PostRequests(Requests requests) {
           _context.Request.Add(requests);
            
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequests", new { id = requests.Id }, requests);

        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Requests>> DeleteRequests(int id)     {
            var requests = await _context.Request.FindAsync(id);
            if (requests == null)
            {
                return NotFound();
            }

            _context.Request.Remove(requests);
            await _context.SaveChangesAsync();

            return requests;
        }

        private bool RequestsExists(int id) {
            return _context.Request.Any(e => e.Id == id);
        }
        private static CapDBContext context = new CapDBContext();

        public static string RequestNew = "NEW";
        public static string RequestReview = "REVIEW";
        public static string RequestEdit = "EDIT";
        public static string RequestApproved = "APPROVED";
        public static string RequestRejected = "REJECTED";

        //Get: api/Requests/ReviewOnly - not a request that you own
        [HttpGet("reviewOnly/{userid}")]
        public async Task<ActionResult<IEnumerable<Requests>>> ReviewStatusOnly(int userId) {
            var request = await _context.Request.Where(r => r.UsersId!= userId && r.Status == RequestReview).ToListAsync();
            if (Request == null)
            {
                return NotFound();
            }

            return request;

            




        }
         

        //Put: api/Requests/Review
        [HttpPut("reviewstatus/{id}")]
        public async Task<IActionResult> ReviewStatus(int id) {
            var request = await _context.Request.FindAsync(id);



            if (Request == null)
            {
                return NotFound();
            }
            if (request.Total <= 50)
            {
                request.Status = RequestApproved;
            }
            request.Status = RequestReview;
            _context.SaveChanges();

            return NoContent();



        }
        //Put: api/Requests/Approve
        [HttpPut("approvestatus/{id}")]
        public async Task<IActionResult> EditStatus(int id) {
            var request = await _context.Request.FindAsync(id);



            if (Request == null)
            {
                return NotFound();
            }
            if (request.Total <= 50)
            {
                request.Status = RequestApproved;
            }
            request.Status = RequestApproved;
            _context.SaveChanges();

            return NoContent();

        }

        //Put: api/Requests/Reject

        [HttpPut("rejectstatus/{id}")]
        public async Task<IActionResult> RejectStatus(int id) {
            var request = await _context.Request.FindAsync(id);



            if (Request == null)
            {
                return NotFound();
            }
            if (request.Total <= 50)
            {
                request.Status = RequestApproved;
            }
            request.Status = RequestRejected;
            _context.SaveChanges();

            return NoContent();

        }
        // static getTotal() {
        //    //var request =  _context.Request(id);
            
        //    Requests.Total = _context.RequestLines;
        //    where(l => l.RequestId == requestId)
        //    .Sum(l => l.Product.Price * l.Quantity)

        //    - context.SaveChanges();

        //    return Requests.Total;

        //};

    }
}
