using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using NZParadiseAPI.Library;
using NZParadiseAPI.Model.Entities;

namespace NZParadiseAPI.Model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Properties> Properties { get; set; }
        public DbSet<PropertyTypes> PropertyTypes { get; set; }
        public DbSet<Attachments> Attachments { get; set; }
        public DbSet<AuditLogs> AuditLog { get; set; }
        public DbSet<PropertyFeatures> PropertyFeatures { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Messages> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Default Data for PropertyType
            modelBuilder.Entity<PropertyTypes>().HasData(
                new PropertyTypes { PropertyTypeID = 1, Description = "Apartment" },
                new PropertyTypes { PropertyTypeID = 2, Description = "Condominium" },
                new PropertyTypes { PropertyTypeID = 3, Description = "Mansion" },
                new PropertyTypes { PropertyTypeID = 4, Description = "Room" },
                new PropertyTypes { PropertyTypeID = 5, Description = "Landed House" },
                new PropertyTypes { PropertyTypeID = 6, Description = "Townhouse" },
                // ....
                new PropertyTypes { PropertyTypeID = 20, Description = "Other" }
            );

            // Seed Default Data for PropertyFeatures
            modelBuilder.Entity<PropertyFeatures>().HasData(
                new PropertyFeatures { PropertyFeatureID = 1, Description = "Gas" },
                new PropertyFeatures { PropertyFeatureID = 2, Description = "Washing Machine" },
                new PropertyFeatures { PropertyFeatureID = 3, Description = "Dryer" },
                new PropertyFeatures { PropertyFeatureID = 4, Description = "Electricity" },
                new PropertyFeatures { PropertyFeatureID = 5, Description = "Cold Water" },
                new PropertyFeatures { PropertyFeatureID = 6, Description = "Hot Water" },
                new PropertyFeatures { PropertyFeatureID = 7, Description = "Air Conditioner" },
                new PropertyFeatures { PropertyFeatureID = 8, Description = "Heater" },
                new PropertyFeatures { PropertyFeatureID = 9, Description = "Swimming Pool" },
                new PropertyFeatures { PropertyFeatureID = 10, Description = "GYM" },
                // ....
                new PropertyFeatures { PropertyFeatureID = 20, Description = "Other" }
            );

            // Seed Defautl Admin Users
        //     modelBuilder.Entity<Users>().HasData(
        //        new Users
        //        {
        //            UserID = 1,
        //            UserTypeID = (int)CommonEnum.UserType.Admin,
        //            FirstName = "NZParadise",
        //            LastName = "Admin 01",
        //            AgencyName = "NZ Paradise Group",
        //            PhoneNo = "290450430001",
        //            Password = "$2a$11$PsKZ5eohauebADO9QkhjPec2typfNOq47GUsCVN/pYrCP0y8d.T2C", // P@ssword123
        //            Gender = 0,
        //            Email = "admin1@nzparadise.com",
        //            CreatedDate = new DateTime(2025, 05, 12, 0, 0, 0, DateTimeKind.Utc)
        //        },
        //        new Users
        //        {
        //            UserID = 2,
        //            UserTypeID = (int)CommonEnum.UserType.Admin,
        //            FirstName = "NZParadise",
        //            LastName = "Admin 02",
        //            AgencyName = "NZ Paradise Group",
        //            PhoneNo = "290450430001",
        //            Password = "$2a$11$PsKZ5eohauebADO9QkhjPec2typfNOq47GUsCVN/pYrCP0y8d.T2C", // P@ssword123
        //            Gender = 0,
        //            Email = "admin2@nzparadise.com",
        //            CreatedDate = new DateTime(2025, 05, 12, 0, 0, 0, DateTimeKind.Utc)
        //        },
        //        new Users
        //        {
        //            UserID = 3,
        //            UserTypeID = (int)CommonEnum.UserType.Admin,
        //            FirstName = "NZParadise",
        //            LastName = "Admin 03",
        //            AgencyName = "NZ Paradise Group",
        //            PhoneNo = "290450430001",
        //            Password = "$2a$11$PsKZ5eohauebADO9QkhjPec2typfNOq47GUsCVN/pYrCP0y8d.T2C", // P@ssword123
        //            Gender = 0,
        //            Email = "admin3@nzparadise.com",
        //            CreatedDate = new DateTime(2025, 05, 12, 0, 0, 0, DateTimeKind.Utc)
        //        }
        //    );
           

            // Configure Messages.FromUserID to disable cascading delete
            modelBuilder.Entity<Messages>()
                .HasOne(m => m.FromUser)
                .WithMany() // No navigation property in Users for Messages
                .HasForeignKey(m => m.FromUserID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Messages.PropertyID to enable cascading delete
            modelBuilder.Entity<Messages>()
                .HasOne(m => m.Property)
                .WithMany(p => p.Messages)
                .HasForeignKey(m => m.PropertyID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Properties.UserID to disable cascading delete
            modelBuilder.Entity<Properties>()
                .HasOne(p => p.User)
                .WithMany() // No navigation property in Users for Properties
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
