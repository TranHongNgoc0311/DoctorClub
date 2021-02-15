using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoctorClub.Models.DataModels
{
    public class Users
    {
        private DateTime? _date = DateTime.Now;
        private int default_level = 1;

        [Key]
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("Name")]
        [Required(ErrorMessage = "Name field can not be empty.")]
        [StringLength(200, ErrorMessage = "Name can not more than 200 characters.")]
        public string Name { get; set; }
        [DisplayName("Username")]
        [RegularExpression(@"^[^<>.,?;:'()!~%\-@#/*""\s]+$", ErrorMessage = "Username can not include special character.")]
        [Required(ErrorMessage = "Username field can not be empty.")]
        [StringLength(50, ErrorMessage = "Username can not more than 50 characters.")]
        public string Username { get; set; }
        [DisplayName("Password")]
        [Required(ErrorMessage = "Password field can not be empty.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; }

        //0 = fisrt time (unverify), 1 = ban, 2= verified
        [DisplayName("Account status")]
        public int AccStatus { get; set; }

        //1= user, 0 = admin
        [DisplayName("Account Level")]
        public int Level
        {
            get { return default_level; }
            set { default_level = value; }
        }
        public string Remember_token { get; set; }
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> token_created_at { get; set; }
        [DisplayName("Birthday")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }
        [DisplayName("Avatar")]
        public string Image { get; set; }
        [DisplayName("Phone number")]
        [Required(ErrorMessage = "Phone field can not be empty.")]
        [StringLength(20, ErrorMessage = "Phone number can not more than 20 characters.")]
        public string Phone { get; set; }
        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Email format is not correct.")]
        [Required(ErrorMessage = "Email field can not be empty.")]
        public string Email { get; set; }
        [DisplayName("Address")]
        [Required(ErrorMessage = "Address field can not be empty.")]
        public string Address { get; set; }
        [DisplayName("Awards")]
        public string Awards { get; set; }
        [DisplayName("Online status")]
        [DefaultValue(false)]
        public bool online { get; set; }
        [DisplayName("Introdution")]
        [DataType(DataType.MultilineText)]
        public string Introdution { get; set; }
        [DisplayName("Active Points")]
        public int ActivePoints { get; set; }
        [DisplayName("Private")]
        [DefaultValue(false)]
        public bool Private { get; set; }
        [DisplayName("Created date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Created
        {
            get { return _date; }
            set { _date = value; }
        }

        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<Posts> Posts { get; set; }
        public virtual ICollection<UserAcademies> UserAcademies { get; set; }
        public virtual ICollection<UserSpecializations> UserSpecializations { get; set; }
        public virtual ICollection<UserNotifications> UserNotifications { get; set; }
    }
}