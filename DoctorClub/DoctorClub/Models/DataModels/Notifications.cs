using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoctorClub.Models.DataModels
{
    public class Notifications
    {
        private DateTime? _date = DateTime.Now;

        [Key]
        [DisplayName("ID")]
        public string Id { get; set; }
        [DisplayName("Title")]
        [Required(ErrorMessage = "Title field can not be empty.")]
        [StringLength(200, ErrorMessage = "Title can not more than 200 characters.")]
        public string Title { get; set; }
        [DisplayName("Url")]
        public string url { get; set; }
        [DisplayName("Created date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Created
        {
            get { return _date; }
            set { _date = value; }
        }

        public virtual ICollection<UserNotifications> UserNotifications { get; set; }
    }
}