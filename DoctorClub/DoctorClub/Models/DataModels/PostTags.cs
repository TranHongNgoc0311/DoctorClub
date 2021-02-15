using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoctorClub.Models.DataModels
{
    public class PostTags
    {
        [Key]
        public string id { get; set; }
        [DisplayName("Tag ID")]
        [ForeignKey("Tag")]
        public string TagId { get; set; }
        [DisplayName("Post ID")]
        [ForeignKey("Post")]
        public string PostId { get; set; }

        public virtual Posts Post { get; set; }
        public virtual Tags Tag { get; set; }
    }
}