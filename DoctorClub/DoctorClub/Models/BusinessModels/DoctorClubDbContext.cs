using DoctorClub.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DoctorClub.Models.BusinessModels
{
    public class DoctorClubDbContext : DbContext
    {
        public DoctorClubDbContext() : base("name=DoctorClubDbConstr")
        {

        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Academies> Academies { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<SubCategories> SubCategories { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<Posts> Posts { get; set; }
        public DbSet<Specializations> Specializations { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Tags> Tags { get; set; }
        public DbSet<UserAcademies> UserAcademies { get; set; }
        public DbSet<UserNotifications> UserNotifications { get; set; }
        public DbSet<UserSpecializations> UserSpecializations { get; set; }
        public DbSet<CmtLikes> CmtLikes { get; set; }
        public DbSet<PostTags> PostTags { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
    }
}