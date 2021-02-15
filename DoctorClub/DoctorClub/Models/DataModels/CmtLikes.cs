using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoctorClub.Models.DataModels
{
    public class CmtLikes
    {
        [Key]
        public string id { get; set; }
        [DisplayName("Comment ID")]
        [ForeignKey("Comment")]
        public string commentId { get; set; }
        [DisplayName("User ID")]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [DisplayName("status")]
        [DefaultValue(true)]
        public bool Status { get; set; }

        public virtual Comments Comment { get; set; }
        public virtual Users User { get; set; }
    }
}