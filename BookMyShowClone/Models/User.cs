using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookMyShowClone.Models
{
    public class User
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "Name Field Cannot be null")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email Field Cannot be null")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Field Cannot be null")]
        public string Password { get; set; }

        //[Required(ErrorMessage = "Role Field Cannot be null")]
        public string Role { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Favourite> Favourites { get; set; }
    }
}

