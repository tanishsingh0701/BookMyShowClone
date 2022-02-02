using BookMyShowClone.Data;
using BookMyShowClone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookMyShowClone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EventsController(EventDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this._dbContext = dbContext;
            this._httpContextAccessor = httpContextAccessor;
        }


        [HttpPost]
        [Authorize(Roles="Admin")]
        public IActionResult Post([FromForm] Event eventObj)
        {
            var guid = Guid.NewGuid();

            var filePath = Path.Combine("wwwroot", guid + ".jpg");

            if (eventObj.Image != null)
            {
                var fileStream = new FileStream(filePath, FileMode.Create);
                eventObj.Image.CopyTo(fileStream);
            }

            eventObj.ImageUrl = filePath.Remove(0, 7);
            _dbContext.Events.Add(eventObj);
            _dbContext.SaveChanges();
            return Ok();

        }


        [HttpPut("{id}")]
        [Authorize(Roles ="Admin")]
        public IActionResult Put(int id,[FromForm] Event eventObj) 
        {
            var events = _dbContext.Events.Find(id);

            if (events == null)
            {
                return NotFound("No record found with this id");
            }

            else 
            {
                var guid = Guid.NewGuid();
                var filepath = Path.Combine("wwwroot", guid + ".jpg");

                if(events.Image != null) 
                {
                    var fileStream = new FileStream(filepath, FileMode.Create);
                    events.Image.CopyTo(fileStream);
                    events.ImageUrl = filepath.Remove(0,7);

                }

                events.Name = eventObj.Name;
                events.Language = eventObj.Language;
                events.PlayingDate = eventObj.PlayingDate;
                events.PlayingTime = eventObj.PlayingTime;
                events.Rating = eventObj.Rating;
                events.TicketPrice = eventObj.TicketPrice;
                events.TrailorUrl = eventObj.TrailorUrl;
                events.Genre = eventObj.Genre;
                events.Duration = eventObj.Duration;
                events.Description = eventObj.Description;
                events.Artist = eventObj.Artist;

                _dbContext.SaveChanges();
                return Ok("Record updated successfully");


            }


        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public IActionResult Delete(int id)
        {
            var events = _dbContext.Events.Find(id);

            if (events == null) 
            {
                return NotFound("No record found with this id");
            }

            else 
            {
                _dbContext.Events.Remove(events);
                _dbContext.SaveChanges();
                return Ok("Record deleted successfully");
            }

        }

    }
}
