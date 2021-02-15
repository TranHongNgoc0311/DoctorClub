using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoctorClub.Models
{
    public class UserLoginModel
    {
        [DisplayName("Username")]
        [Required(ErrorMessage = "Username field can not be empty.")]
        [StringLength(50, ErrorMessage = "Username can not more than 50 characters.")]
        public string Username { get; set; }
        [DisplayName("Password")]
        [Required(ErrorMessage = "Password field can not be empty.")]
        public string Password { get; set; }
    }
}