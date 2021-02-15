using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoctorClub.Models.DataModels
{
    public class Categories
    {
        [Key]
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("Title")]
        [Required(ErrorMessage = "Title field can not be empty.")]
        [StringLength(100, ErrorMessage = "Name can not more than 200 characters.")]
        public string Name { get; set; }
        [DisplayName("Slug")]
        public string Slug { get; set; }
        [DisplayName("Description")]
        public string description { get; set; }
        [DisplayName("Status")]
        [DefaultValue(true)]
        public bool Status { get; set; }

        public virtual ICollection<SubCategories> SubCategories { get; set; }
    }
}