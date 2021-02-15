using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoctorClub.Models.DataModels
{
    public class Feedback
    {
        [Key]
        [DisplayName("ID")]
        public string id { get; set; }
        [DisplayName("User ID")]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [DisplayName("Title")]
        [Required(ErrorMessage = "Title field can not be empty.")]
        [StringLength(200, ErrorMessage = "Title can not more than 200 characters.")]
        public string Title { get; set; }
        [DisplayName("Content")]
        [DataType(DataType.MultilineText)]
        [MinLength(50, ErrorMessage = "Content can not less than 50 characters.")]
        [Required(ErrorMessage = "Content field can not be empty.")]
        public string Content { get; set; }
        public virtual Users User { get; set; }

    }
}