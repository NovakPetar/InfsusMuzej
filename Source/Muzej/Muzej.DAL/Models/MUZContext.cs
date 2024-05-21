using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Muzej.DAL.Models
{
    public partial class MUZContext : DbContext
    {
        public MUZContext()
        {
        }

        public MUZContext(DbContextOptions<MUZContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Job> Jobs { get; set; } = null!;
        public virtual DbSet<MuseumItem> MuseumItems { get; set; } = null!;
        public virtual DbSet<Notice> Notices { get; set; } = null!;
        public virtual DbSet<Reservation> Reservations { get; set; } = null!;
        public virtual DbSet<ReservationContent> ReservationContents { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;
        public virtual DbSet<Task> Tasks { get; set; } = null!;
        public virtual DbSet<TicketCategory> TicketCategories { get; set; } = null!;
        public virtual DbSet<TicketLimit> TicketLimits { get; set; } = null!;
        public virtual DbSet<TicketPriceChange> TicketPriceChanges { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<WorkingHour> WorkingHours { get; set; } = null!;
        public virtual DbSet<WorkingHoursChange> WorkingHoursChanges { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Database=MUZ;User Id=sa;Password=stipe;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("FK_JobID");
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.ToTable("Job");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<MuseumItem>(entity =>
            {
                entity.ToTable("MuseumItem");

                entity.Property(e => e.MuseumItemId).HasColumnName("MuseumItemID");

                entity.Property(e => e.MultimediaContentType).HasMaxLength(300);

                entity.Property(e => e.Name).HasMaxLength(300);

                entity.Property(e => e.Period).HasMaxLength(300);

                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.MuseumItems)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_RoomID");
            });

            modelBuilder.Entity<Notice>(entity =>
            {
                entity.ToTable("Notice");

                entity.Property(e => e.NoticeId).HasColumnName("NoticeID");

                entity.Property(e => e.EndDateTime).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.ToTable("Reservation");

                entity.HasIndex(e => e.ReservationNumber, "UQ__Reservat__FAA69AEBDFD322A4")
                    .IsUnique();

                entity.Property(e => e.ReservationId).HasColumnName("ReservationID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.ReservationNumber).HasMaxLength(300);

                entity.Property(e => e.VisitorEmail).HasMaxLength(300);

                entity.Property(e => e.VisitorFirstName).HasMaxLength(300);

                entity.Property(e => e.VisitorLastName).HasMaxLength(300);
            });

            modelBuilder.Entity<ReservationContent>(entity =>
            {
                entity.HasKey(e => new { e.ReservationId, e.CategoryId })
                    .HasName("PK__Reservat__167ECCA6E72D883C");

                entity.ToTable("ReservationContent");

                entity.Property(e => e.ReservationId).HasColumnName("ReservationID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.PriceOfSingleTicket).HasColumnType("decimal(5, 2)");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.ReservationContents)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reservati__Categ__4222D4EF");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.ReservationContents)
                    .HasForeignKey(d => d.ReservationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reservati__Reser__412EB0B6");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");

                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("Task");

                entity.Property(e => e.TaskId).HasColumnName("TaskID");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.EndDateTime).HasColumnType("datetime");

                entity.Property(e => e.StartDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_EmployeeID");
            });

            modelBuilder.Entity<TicketCategory>(entity =>
            {
                entity.ToTable("TicketCategory");

                entity.Property(e => e.TicketCategoryId).HasColumnName("TicketCategoryID");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.RegularPrice).HasColumnType("decimal(5, 2)");
            });

            modelBuilder.Entity<TicketLimit>(entity =>
            {
                entity.HasKey(e => e.Date)
                    .HasName("PK__TicketLi__77387D06401E609A");

                entity.ToTable("TicketLimit");

                entity.Property(e => e.Date).HasColumnType("date");
            });

            modelBuilder.Entity<TicketPriceChange>(entity =>
            {
                entity.ToTable("TicketPriceChange");

                entity.Property(e => e.TicketPriceChangeId).HasColumnName("TicketPriceChangeID");

                entity.Property(e => e.EndDateTime).HasColumnType("date");

                entity.Property(e => e.NewPrice).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.TicketCategoryId).HasColumnName("TicketCategoryID");

                entity.HasOne(d => d.TicketCategory)
                    .WithMany(p => p.TicketPriceChanges)
                    .HasForeignKey(d => d.TicketCategoryId)
                    .HasConstraintName("FK_TicketPriceChange_Category");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Email).HasMaxLength(300);

                entity.Property(e => e.Password).HasMaxLength(500);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_RoleID");
            });

            modelBuilder.Entity<WorkingHour>(entity =>
            {
                entity.HasKey(e => e.DayOfWeek)
                    .HasName("PK__WorkingH__00D400DC7190C980");
            });

            modelBuilder.Entity<WorkingHoursChange>(entity =>
            {
                entity.HasKey(e => e.Date)
                    .HasName("PK__WorkingH__77387D06F151B1F2");

                entity.ToTable("WorkingHoursChange");

                entity.Property(e => e.Date).HasColumnType("date");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
