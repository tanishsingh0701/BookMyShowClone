using BookMyShowClone.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMyShowClone.Data
{
    public class EventDbContext:DbContext
    {

        public EventDbContext(DbContextOptions<EventDbContext> options):base(options)
        {

        }
        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
