using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoctorClub.Models.DataModels
{
    public class Tags
    {
        [Key]
        [DisplayName("Tag ID")]
        public string id { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("Slug")]
        public string Slug { get; set; }
        public virtual ICollection<PostTags> PostTags { get; set; }
    }
}