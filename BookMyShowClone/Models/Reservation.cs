﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMyShowClone.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }
        public double TotalAmount { get; set; }
        public string Phone { get; set; }
        public DateTime ReservationTime { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
    }
}
