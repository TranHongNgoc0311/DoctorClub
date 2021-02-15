using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoctorClub.Models
{
    public class ForgotenPasswordModel
    {
        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Email format is not correct")]
        [Required(ErrorMessage = "Email field can not be empty.")]
        public string Email { get; set; }
        [DisplayName("Username")]
        [Required(ErrorMessage = "Username field can not be empty.")]
        [StringLength(50, ErrorMessage = "Username can not more than 50 characters.")]
        public string Username { get; set; }
    }
}