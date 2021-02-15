using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoctorClub.Models.DataModels
{
    public class UserSpecializations
    {
        [Key]
        public string id { get; set; }
        [ForeignKey("User")]
        [DisplayName("User ID")]
        public Nullable<int> UserID { get; set; }
        [ForeignKey("Specialization")]
        [DisplayName("Specialization ID")]
        public Nullable<int> SpId { get; set; }
        [DisplayName("Position")]
        [Required(ErrorMessage = "Position field can not be empty.")]
        [StringLength(100, ErrorMessage = "Position can not more than 100 characters.")]
        public string position { get; set; }
        [DisplayName("Place")]
        [Required(ErrorMessage = "Place field can not be empty.")]
        [StringLength(200, ErrorMessage = "Place can not more than 200 characters.")]
        public string Place { get; set; }
        [DisplayName("From")]
        public int From { get; set; }
        [DisplayName("To")]
        public Nullable<int> To { get; set; }

        public virtual Specializations Specialization { get; set; }
        public virtual Users User { get; set; }
    }
}