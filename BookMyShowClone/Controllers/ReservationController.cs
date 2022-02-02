using BookMyShowClone.Data;
using BookMyShowClone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMyShowClone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly EventDbContext _dbContext;

        public ReservationController(EventDbContext dbContext)
        {
            this._dbContext = dbContext;
        }


        [HttpPost]
        [Authorize(Roles ="Users")]
        public IActionResult Post([FromBody] Reservation reservationObj) 
        {
            reservationObj.ReservationTime = DateTime.Now;
            _dbContext.Reservations.Add(reservationObj);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
