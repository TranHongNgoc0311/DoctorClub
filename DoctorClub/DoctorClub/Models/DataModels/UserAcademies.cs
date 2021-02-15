using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoctorClub.Models.DataModels
{
    public class UserAcademies
    {
        [Key]
        public string id { get; set; }
        [ForeignKey("User")]
        [DisplayName("User ID")]
        public Nullable<int> UserID { get; set; }
        [ForeignKey("Academy")]
        [DisplayName("Academy ID")]
        public Nullable<int> AId { get; set; }
        [DisplayName("Diploma photo")]
        public string Image { get; set; }
        [DisplayName("From year")]
        [Required(ErrorMessage = "We need when you start at here.")]
        public int From { get; set; }
        [DisplayName("To year")]
        public int To { get; set; }

        public virtual Academies Academy { get; set; }
        public virtual Users User { get; set; }
    }
}