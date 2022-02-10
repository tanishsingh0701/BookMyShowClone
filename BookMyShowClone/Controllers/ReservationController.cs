using BookMyShowClone.Data;
using BookMyShowClone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookMyShowClone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly EventDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReservationController(EventDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this._dbContext = dbContext;
            this._httpContextAccessor = httpContextAccessor;
        }
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));


        [HttpPost]
        [Authorize(Roles ="Users")]
        public IActionResult Post([FromBody] Reservation reservationObj) 
        {
            var resev = _dbContext.Events.Where(m => m.Id == reservationObj.EventId)
                .Select(m => m.TicketPrice)
                .SingleOrDefault();

            var eventId = reservationObj.EventId;
            var unResesrvedSeats = _dbContext.Events.Where(m => m.Id == reservationObj.EventId)
                                    .Select(m => m.UnReservedSeats).SingleOrDefault();

            if (unResesrvedSeats-reservationObj.Qty <0 ) 
            {
                
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var eventsObj = _dbContext.Events.Find(eventId);

            eventsObj.ReservedSeats+=reservationObj.Qty;
            eventsObj.UnReservedSeats-= reservationObj.Qty;

            reservationObj.UserId = GetUserId();
            reservationObj.Price = resev; 
            reservationObj.ReservationTime = DateTime.Now;
            reservationObj.TotalAmount = reservationObj.Price * reservationObj.Qty;
            _dbContext.Reservations.Add(reservationObj);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [Authorize(Roles = "Users")]
        [HttpGet]
        public IActionResult GetReservationsUser()
        {
            var reservations = from reservation in _dbContext.Reservations
                               join customer in _dbContext.Users on reservation.UserId equals customer.Id
                               join movie in _dbContext.Events on reservation.EventId equals movie.Id
                               where (customer.Id == GetUserId())
                               //join event in _dbContext.Events on reservation.EventId equals event.Id
                               select new
                               {
                                   Id = reservation.Id,
                                   ReservationTime = reservation.ReservationTime,
                                   CustomerName = customer.Name,
                                   EventName = movie.Name
                               };

            return Ok(reservations);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("totalEarningSingle/{id}")]
        public double GetReservationDetail(int id)
        {
            var reservations = from reservation in _dbContext.Reservations
                               join customer in _dbContext.Users on reservation.UserId equals customer.Id
                               join movie in _dbContext.Events on reservation.EventId equals movie.Id
                               where (reservation.EventId == id)
                               select new
                               {
                                   Id = reservation.Id,
                                   ReservationTime = reservation.ReservationTime,
                                   CustomerName = customer.Name,
                                   MovieName = movie.Name,
                                   Email = customer.Email,
                                   Qty = reservation.Qty,
                                   Price = reservation.Price,
                                   TotalAmount = reservation.TotalAmount,
                                   Phone = reservation.Phone,
                                   playingDate = movie.PlayingDate,
                                   playingTime = movie.PlayingTime

                               };





            //return Ok(reservations.Count);
            double result = 0;
            foreach(var reservation in reservations) 
            {
                result +=reservation.TotalAmount;
                System.Console.WriteLine(result);
            }
            
            return result;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("totalEarningAll")]
        public double GetReservationDetailAll()
        {
            var reservations = from reservation in _dbContext.Reservations
                               join customer in _dbContext.Users on reservation.UserId equals customer.Id
                               join movie in _dbContext.Events on reservation.EventId equals movie.Id
                              
                               select new
                               {
                                   Id = reservation.Id,
                                   ReservationTime = reservation.ReservationTime,
                                   CustomerName = customer.Name,
                                   MovieName = movie.Name,
                                   Email = customer.Email,
                                   Qty = reservation.Qty,
                                   Price = reservation.Price,
                                   TotalAmount = reservation.TotalAmount,
                                   Phone = reservation.Phone,
                                   playingDate = movie.PlayingDate,
                                   playingTime = movie.PlayingTime

                               };


            //return Ok(reservations.Count);
            double result = 0;
            foreach (var reservation in reservations)
            {
                result += reservation.TotalAmount;
                System.Console.WriteLine(result);
            }

            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("totalBookingEvent/{id}")]
        public int GetEventDetail(int id)
        {
            var reservations = from reservation in _dbContext.Reservations
                               join customer in _dbContext.Users on reservation.UserId equals customer.Id
                               join movie in _dbContext.Events on reservation.EventId equals movie.Id
                               where (reservation.EventId == id)
                               select new
                               {
                                   Id = reservation.Id,
                                   ReservationTime = reservation.ReservationTime,
                                   CustomerName = customer.Name,
                                   MovieName = movie.Name,
                                   Email = customer.Email,
                                   Qty = reservation.Qty,
                                   Price = reservation.Price,
                                   TotalAmount = reservation.TotalAmount,
                                   Phone = reservation.Phone,
                                   playingDate = movie.PlayingDate,
                                   playingTime = movie.PlayingTime

                               };



            int book_count = 0;
            foreach (var reservation in reservations)
            {
                book_count += reservation.Qty;
            }

            return book_count;


            return reservations.Count();
        }

    }
}
