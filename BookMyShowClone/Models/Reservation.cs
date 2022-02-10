using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookMyShowClone.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Qty Field Cannot be null")]
        public int Qty { get; set; }
        public double Price { get; set; }


        public double TotalAmount { get; set; }

        [Required(ErrorMessage = "Phone Field Cannot be null")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number.")]
        public string Phone { get; set; }
        public DateTime ReservationTime { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
    }
}
