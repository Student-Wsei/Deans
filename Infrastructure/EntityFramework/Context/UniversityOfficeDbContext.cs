using AppCore.Services;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Context;

public class UniversityOfficeDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Lecturer> Lecturers { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<DegreeProgram> DegreePrograms { get; set; }
    public DbSet<AcademicYear> AcademicYears { get; set; }

    public UniversityOfficeDbContext() { }

    public UniversityOfficeDbContext(DbContextOptions<UniversityOfficeDbContext> options) : base(options) { }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<Guid>().UseCollation("NOCASE");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("data source=university.db");
        }
        optionsBuilder.ConfigureWarnings(w =>
            w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>(entity =>
        {
            entity.Property(u => u.FirstName).HasMaxLength(100);
            entity.Property(u => u.LastName).HasMaxLength(100);
            entity.Property(u => u.Department).HasMaxLength(100);
            entity.HasIndex(u => u.Email).IsUnique();
        });

        builder.Entity<AppRole>(entity =>
        {
            entity.Property(r => r.Name).HasMaxLength(20);
        });

        builder.Entity<Person>()
            .HasDiscriminator<string>("PersonType")
            .HasValue<Student>("Student")
            .HasValue<Lecturer>("Lecturer");

        builder.Entity<Person>(entity =>
        {
            entity.Property(p => p.FirstName).HasMaxLength(100);
            entity.Property(p => p.LastName).HasMaxLength(200);
            entity.Property(p => p.NationalId).HasMaxLength(20);
            entity.Property(p => p.Email).HasMaxLength(200).HasConversion(
                v => (string)v,
                v => new Domain.ValueObjects.EmailAddress(v));
        });

        builder.Entity<Student>(entity =>
        {
            entity.Property(p => p.StudentId).HasMaxLength(20);
            entity.Property(p => p.Status).HasConversion<string>();
        });

        builder.Entity<Lecturer>(entity =>
        {
            entity.Property(p => p.Title).HasMaxLength(50);
            entity.Property(p => p.Faculty).HasMaxLength(100);
        });

        builder.Entity<Course>(entity =>
        {
            entity.Property(p => p.Code).HasMaxLength(20);
            entity.Property(p => p.Name).HasMaxLength(200);
            entity.Property(p => p.CompletionType).HasConversion<string>();
            entity.Property(p => p.Semester).HasConversion<string>();
        });

        builder.Entity<Grade>(entity =>
        {
            entity.Property(p => p.GradeType).HasConversion<string>();
            entity.Property(p => p.GradeValue).HasConversion<string>();
        });

        builder.Entity<DegreeProgram>(entity =>
        {
            entity.Property(p => p.Code).HasMaxLength(20);
            entity.Property(p => p.Name).HasMaxLength(200);
            entity.Property(p => p.Faculty).HasMaxLength(100);
            entity.Property(p => p.DegreeType).HasConversion<string>();
        });

        builder.Entity<AcademicYear>(entity =>
        {
            entity.Property(p => p.Name).HasMaxLength(20);
        });

        SeedIdentity(builder);
    }

    private static void SeedIdentity(ModelBuilder builder)
    {
        var adminRoleId = "11111111-1111-1111-1111-111111111111";
        var deanRoleId = "22222222-2222-2222-2222-222222222222";
        var chancellorRoleId = "33333333-3333-3333-3333-333333333333";
        var lecturerRoleId = "44444444-4444-4444-4444-444444444444";
        var studentRoleId = "55555555-5555-5555-5555-555555555555";

        builder.Entity<AppRole>().HasData(
            new AppRole(UserRole.Administrator.ToString(), "Administrator systemu")
            {
                Id = adminRoleId,
                NormalizedName = UserRole.Administrator.ToString().ToUpper()
            },
            new AppRole(UserRole.DeanOfficeWorker.ToString(), "Pracownik dziekanatu")
            {
                Id = deanRoleId,
                NormalizedName = UserRole.DeanOfficeWorker.ToString().ToUpper()
            },
            new AppRole(UserRole.Chancellor.ToString(), "Kanclerz")
            {
                Id = chancellorRoleId,
                NormalizedName = UserRole.Chancellor.ToString().ToUpper()
            },
            new AppRole(UserRole.Lecturer.ToString(), "Prowadzacy zajecia")
            {
                Id = lecturerRoleId,
                NormalizedName = UserRole.Lecturer.ToString().ToUpper()
            },
            new AppRole(UserRole.Student.ToString(), "Student")
            {
                Id = studentRoleId,
                NormalizedName = UserRole.Student.ToString().ToUpper()
            }
        );

        var adminUserId = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa";
        var deanUserId = "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb";

        builder.Entity<AppUser>().HasData(
            new AppUser
            {
                Id = adminUserId,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@wsei.edu.pl",
                NormalizedEmail = "ADMIN@WSEI.EDU.PL",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEAFZiZ9z10equ3GrF6JDBFxKvLlnWn+ja0ag5nhR+hL1tNjzHLteUq4qLn7zB6Z3OA==",
                SecurityStamp = string.Empty,
                ConcurrencyStamp = "admin-stamp",
                FirstName = "Adam",
                LastName = "Adminowski",
                FullName = "Adam Adminowski",
                Department = "IT",
                Status = SystemUserStatus.Active,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
            },
            new AppUser
            {
                Id = deanUserId,
                UserName = "dean",
                NormalizedUserName = "DEAN",
                Email = "dean@wsei.edu.pl",
                NormalizedEmail = "DEAN@WSEI.EDU.PL",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEBC3Jitl4fMGYKBrADSeN1O7jJ364dvbih2DQ847rYPimzmE5ZtcX1PYMpCiapeABA==",
                SecurityStamp = string.Empty,
                ConcurrencyStamp = "dean-stamp",
                FirstName = "Ewa",
                LastName = "Dziekanska",
                FullName = "Ewa Dziekanska",
                Department = "Dziekanat",
                Status = SystemUserStatus.Active,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
            }
        );
    }
}
