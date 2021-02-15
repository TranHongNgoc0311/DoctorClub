using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoctorClub.Models.DataModels
{
    public class UserNotifications
    {
        [Key]
        public string id { get; set; }
        [DisplayName("Notification ID")]
        [ForeignKey("Notification")]
        public string NotificationId { get; set; }
        [DisplayName("User ID")]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [DisplayName("status")]
        [DefaultValue(false)]
        public bool Status { get; set; }

        public virtual Notifications Notification { get; set; }
        public virtual Users User { get; set; }
    }
}