using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookMyShowClone.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name Field Cannot be null")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Artist { get; set; }

        [Required]
        public string Language { get; set; }

        public string Duration { get; set; }
        public DateTime PlayingDate { get; set; }
        public DateTime PlayingTime { get; set; }
        public double TicketPrice { get; set; }
        public string City { get; set; }

        public double Rating { get; set; } = 0;
        public int RatingCount { get; set; } = 0;

        [Required]
        public int UnReservedSeats { get; set; }

        public int ReservedSeats { get; set; } = 0;
        public string Genre { get; set; }
        public string TrailorUrl { get; set; }


        [NotMapped]
        public IFormFile Image { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Favourite> Favourites { get; set; }
    }
}
