using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoctorClub.Models.DataModels
{
    public class Posts
    {
        private DateTime? _date = DateTime.Now;

        [Key]
        [DisplayName("ID")]
        public string Id { get; set; }
        [DisplayName("Poster ID")]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [DisplayName("Title")]
        [Required(ErrorMessage = "Title field can not be empty.")]
        [StringLength(200, ErrorMessage = "Title can not more than 200 characters.")]
        public string Title { get; set; }
        [DisplayName("Slug")]
        public string Slug { get; set; }
        [DisplayName("Subcategory")]
        [ForeignKey("SubCategories")]
        public int SubCatId { get; set; }
        [DisplayName("Content")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        [DisplayName("Views")]
        [DefaultValue(0)]
        public int Views { get; set; }
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
        [DisplayName("Post status")]
        [DefaultValue(false)]
        public bool Status { get; set; }
        [DefaultValue(false)]
        public bool deleted { get; set; }

        public virtual SubCategories SubCategories { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<PostTags> PostTags { get; set; }
        public virtual Users User { get; set; }
    }
}