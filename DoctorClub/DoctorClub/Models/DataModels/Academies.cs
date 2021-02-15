using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoctorClub.Models.DataModels
{
    [Table("Academy")]
    public class Academies
    {
        private DateTime? _date = DateTime.Now;

        [Key]
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("Name")]
        [Required(ErrorMessage = "Name field can not be empty.")]
        [StringLength(200, ErrorMessage = "Name can not more than 200 characters.")]
        public string Name { get; set; }
        [DisplayName("Record status")]
        [DefaultValue(true)]
        public bool Status { get; set; }
        [DisplayName("Created date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Created
        {
            get { return _date; }
            set { _date = value; }
        }

        public virtual ICollection<UserAcademies> UserAcademies { get; set; }
    }
}