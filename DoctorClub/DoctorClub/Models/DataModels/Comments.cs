using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoctorClub.Models.DataModels
{
    public class Comments
    {
        private DateTime? _date = DateTime.Now;

        [Key]
        [DisplayName("ID")]
        public string Id { get; set; }
        [DisplayName("Content")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        [DisplayName("Created date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Created
        {
            get { return _date; }
            set { _date = value; }
        }
        [DisplayName("Updated date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Updated { get; set; }
        [DisplayName("Poster ID")]
        [ForeignKey("User")]
        public Nullable<int> UserId { get; set; }
        [DisplayName("Post ID")]
        [ForeignKey("Post")]
        public string PostId { get; set; }
        public string ParrentId { get; set; }

        public virtual Posts Post { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<CmtLikes> CmtLikes { get; set; }
    }
}