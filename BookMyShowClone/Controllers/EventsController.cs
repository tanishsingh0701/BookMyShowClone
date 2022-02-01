using BookMyShowClone.Data;
using BookMyShowClone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookMyShowClone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventDbContext _dbContext;

        public EventsController(EventDbContext dbContext)
        {
            this._dbContext = dbContext;
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

    }
}
