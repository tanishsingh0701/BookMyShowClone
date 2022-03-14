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
        public IActionResult Post([FromBody] Event eventObj)
        {
            var guid = Guid.NewGuid();

            var filePath = Path.Combine("wwwroot", guid + ".jpg");

            if (eventObj.Image != null)
            {
                var fileStream = new FileStream(filePath, FileMode.Create);
                eventObj.Image.CopyTo(fileStream);
            }

            var eventsObj = new Event
            {
                Name = eventObj.Name,
                Description = eventObj.Description,
                Artist= eventObj.Artist,
                Language=  eventObj.Language,
                Duration= eventObj.Duration,
                Rating= eventObj.Rating,
                ImageUrl= eventObj.ImageUrl,
                
                TicketPrice= eventObj.TicketPrice,
                Genre= eventObj.Genre,
                TrailorUrl= eventObj.TrailorUrl,
                City= eventObj.City,
                UnReservedSeats = eventObj.UnReservedSeats

            };

            //eventObj.ImageUrl = filePath.Remove(0, 7);
            _dbContext.Events.Add(eventObj);
            _dbContext.SaveChanges();
            return Ok();

        }


        [HttpPut("{id}")]
        [Authorize(Roles ="Admin")]
        public IActionResult Put(int id,[FromBody] Event eventObj) 
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
                events.ImageUrl = eventObj.ImageUrl;
                events.UnReservedSeats = eventObj.UnReservedSeats;

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

        [Authorize]
        [HttpGet("search/{info}")]
        public IActionResult FindMovies(string info)
        {
            if (info == "1")
            {
                var movies2 = from movie in _dbContext.Events
                                    
                                    select new
                                    {
                                        id = movie.Id,
                                        name = movie.Name,
                                        duration = movie.Duration,
                                        description = movie.Description,
                                        language = movie.Language,
                                        rating = movie.Rating,
                                        genre = movie.Genre,
                                        image_url = movie.ImageUrl,
                                        unReservedSeats = movie.UnReservedSeats,
                                        artist = movie.Artist,
                                        ticket_price = movie.TicketPrice,
                                        city = movie.City,
                                        reservedSeats=movie.ReservedSeats
                                    };
                return Ok(movies2);

            }
            var movies = from movie in _dbContext.Events
                         where
                         //movie.City.StartsWith(info) ||
                         movie.Name.StartsWith(info)
                         // ||movie.Artist.StartsWith(info) ||
                         //movie.Genre.StartsWith(info)
                         select new
                         {
                             id = movie.Id,
                             name = movie.Name,
                             duration = movie.Duration,
                             description = movie.Description,
                             language = movie.Language,
                             rating = movie.Rating,
                             genre = movie.Genre,
                             image_url = movie.ImageUrl,
                             unReservedSeats = movie.UnReservedSeats,
                             artist = movie.Artist,
                             ticket_price = movie.TicketPrice,
                             city = movie.City,
                             reservedSeats = movie.ReservedSeats
                         };
            return Ok(movies);
        }

        
        [Authorize]
        [HttpGet("getAllEvents")]
        
        public IActionResult AllMovies()
        {
            
            var events_obj = from events in _dbContext.Events
                         select new
                         {
                             id = events.Id,
                             name = events.Name,
                             duration = events.Duration,
                             description=events.Description,
                             language = events.Language,
                             rating = events.Rating,
                             genre = events.Genre,
                             image_url = events.ImageUrl,
                             unReservedSeats=events.UnReservedSeats,
                             artist=events.Artist,
                             ticket_price=events.TicketPrice,
                             city = events.City,
                             reservedSeats = events.ReservedSeats

                         };

            return Ok(events_obj);
            //return _dbContext.Movies;
            // here ok method will return movie and status both

            

        }


        //[Authorize]
        [HttpGet("getEvents/{id}")]

        public IActionResult AllMovies(int id)
        {

            var events_obj = from events in _dbContext.Events
                             where (events.Id == id)
                             select new
                             {
                                 id = events.Id,
                                 name = events.Name,
                                 duration = events.Duration,
                                 description = events.Description,
                                 language = events.Language,
                                 rating = events.Rating,
                                 genre = events.Genre,
                                 image_url = events.ImageUrl,
                                 unReservedSeats = events.UnReservedSeats,
                                 artist = events.Artist,
                                 ticket_price = events.TicketPrice,
                                 city = events.City,
                                 reservedSeats = events.ReservedSeats

                             };

            return Ok(events_obj);
            //return _dbContext.Movies;
            // here ok method will return movie and status both



        }

    }
}
