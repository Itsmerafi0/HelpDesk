using API.Models;
using API.Utility;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace API.Contexts
{
    public class HelpDeskManagementDBContext : DbContext
    {
        public HelpDeskManagementDBContext(DbContextOptions<HelpDeskManagementDBContext> options) : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Resolution> Resolutions { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder Builder)
        {
            base.OnModelCreating(Builder);

            Builder.Entity<Category>().HasData(
                new Category
                {
                    Guid = Guid.Parse("3fa82f64-2322-4562-b3fc-2e963f44afb6"),
                    CategoryName = nameof(CategoryLevel.Access),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                },
                new Category
                {
                    Guid = Guid.Parse("3fa82f64-8333-2332-b3fc-2e963f44afb6"),
                    CategoryName = nameof(CategoryLevel.Reimbursement),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                });

            Builder.Entity<SubCategory>().HasData(
                new SubCategory
                {
                    Guid = Guid.Parse("3fa85f64-2212-3322-b3fc-2c963f66afa6"),
                    Name = "Login Issue",
                    CategoryGuid = Guid.Parse("3fa82f64-2322-4562-b3fc-2e963f44afb6"),
                    RiskLevel = Risk.High,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                },
                new SubCategory
                {
                    Guid = Guid.Parse("3fa85f64-8823-4313-b3fc-2c963f66afa6"),
                    Name = "Forgot Password Issue",
                    CategoryGuid = Guid.Parse("3fa82f64-2322-4562-b3fc-2e963f44afb6"),
                    RiskLevel = Risk.High,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                },
                new SubCategory
                {
                    Guid = Guid.Parse("3fa85f64-2212-4444-b3fc-2c963f66afa6"),
                    Name = "Parking Reimbursement",
                    RiskLevel = Risk.Low,
                    CategoryGuid = Guid.Parse("3fa82f64-8333-2332-b3fc-2e963f44afb6"),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                },
                new SubCategory
                {
                    Guid = Guid.Parse("3fa85f64-9213-8444-b3fc-2c963f66afa6"),
                    Name = "Transportation Reimbursement Issue",
                    RiskLevel = Risk.High,
                    CategoryGuid = Guid.Parse("3fa82f64-8333-2332-b3fc-2e963f44afb6"),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                },
                new SubCategory
                {
                    Guid = Guid.Parse("3fa85f64-7231-2933-b3fc-2c963f66afa6"),
                    Name = "Overtime Reimbursement Issue",
                    RiskLevel = Risk.Medium,
                    CategoryGuid = Guid.Parse("3fa82f64-8333-2332-b3fc-2e963f44afb6"),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                });
            
            Builder.Entity<Role>().HasData(
            new Role
            {
                Guid = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                Name = nameof(RoleLevel.User),
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            },
            new Role
            {
                Guid = Guid.Parse("158f7caf-f4d2-45ad-4c30-08db58db1641"),
                Name = nameof(RoleLevel.Developer),
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            },
            new Role
            {
                Guid = Guid.Parse("158f7caf-D2AD-45ad-4c30-08db58db1641"),
                Name = nameof(RoleLevel.Finance),
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            },
            new Role
            {
                Guid = Guid.Parse("6f0cab9c-77ee-4720-fbe7-08db584bbbc0"),
                Name = nameof(RoleLevel.Admin),
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            });

            Builder.Entity<Employee>().HasIndex(e => new
            {
                e.Nik,
                e.Email,
                e.PhoneNumber
            }).IsUnique();

            Builder.Entity<Ticket>().HasIndex(e => new
            {
                e.TicketId
            }).IsUnique();

            Builder.Entity<AccountRole>().HasOne(a => a.Account)
                .WithMany(ar => ar.AccountRoles)
                .HasForeignKey(ar => ar.AccountGuid);

            Builder.Entity<AccountRole>().HasOne(r => r.Role)
                .WithMany(ar => ar.AccountRoles)
                .HasForeignKey(ar => ar.RoleGuid);

            Builder.Entity<Account>().HasOne(e => e.Employee)
                .WithOne(a => a.Account)
                .HasForeignKey<Account>(a => a.Guid);

            Builder.Entity<Ticket>().HasOne(e => e.Employee)
                 .WithMany(c => c.Complains)
                 .HasForeignKey(c => c.EmployeeGuid);

            Builder.Entity<Ticket>().HasOne(e => e.Resolution)
                .WithOne(c => c.Complains)
                .HasForeignKey<Resolution>(c => c.Guid);

            Builder.Entity<Ticket>().HasOne(e => e.SubCategory)
                .WithMany(c => c.Complains)
                .HasForeignKey(e => e.SubCategoryGuid);

            Builder.Entity<Category>().HasMany(e => e.SubCategories)
                .WithOne(c => c.Category)
                .HasForeignKey(e => e.CategoryGuid);

            Builder.Entity<Resolution>().HasOne(e => e.Employee)
                .WithMany(r => r.Resolutions)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(r => r.ResolvedBy);

        }
    }
}

