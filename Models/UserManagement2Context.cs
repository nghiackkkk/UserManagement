using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.Models;

public partial class UserManagement2Context : DbContext
{
    public UserManagement2Context()
    {
    }

    public UserManagement2Context(DbContextOptions<UserManagement2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Family> Families { get; set; }

    public virtual DbSet<Studyprocess> Studyprocesses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Workingprocess> Workingprocesses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=xian\\dotnet;Initial Catalog=UserManagement2;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_100_CI_AS_SC_UTF8");

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ADDRESS__3213E83F6E3A0FED");

            entity.ToTable("ADDRESS");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address1)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.Commune)
                .HasMaxLength(255)
                .HasColumnName("commune");
            entity.Property(e => e.District)
                .HasMaxLength(255)
                .HasColumnName("district");
        });

        modelBuilder.Entity<Family>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FAMILY__3213E83F841B3697");

            entity.ToTable("FAMILY");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CurrentOcupation)
                .HasMaxLength(255)
                .HasColumnName("current_ocupation");
            entity.Property(e => e.CurrentResident)
                .HasMaxLength(255)
                .HasColumnName("current_resident");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Realtionship)
                .HasMaxLength(255)
                .HasColumnName("realtionship");
            entity.Property(e => e.WorkingAgency)
                .HasMaxLength(255)
                .HasColumnName("working_agency");
            entity.Property(e => e.YearOfBirth).HasColumnName("year_of_birth");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Families)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK__FAMILY__id_user__59FA5E80");
        });

        modelBuilder.Entity<Studyprocess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__STUDYPRO__3213E83F64D0763F");

            entity.ToTable("STUDYPROCESS");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.ModeOfStudy)
                .HasMaxLength(255)
                .HasColumnName("mode_of_study");
            entity.Property(e => e.SchoolUniversity)
                .HasMaxLength(255)
                .HasColumnName("school_university");
            entity.Property(e => e.StartTime).HasColumnName("start_time");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Studyprocesses)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK__STUDYPROC__id_us__5FB337D6");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USERS__3213E83F742639C7");

            entity.ToTable("USERS");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CoverImage)
                .HasMaxLength(255)
                .HasColumnName("cover_image");
            entity.Property(e => e.CulturalStandard)
                .HasMaxLength(255)
                .HasColumnName("cultural_standard");
            entity.Property(e => e.DateCreated)
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateModified)
                .HasColumnType("datetime")
                .HasColumnName("date_modified");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.EthnicGroup)
                .HasMaxLength(255)
                .HasColumnName("ethnic_group");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .HasColumnName("gender");
            entity.Property(e => e.IdCard)
                .HasMaxLength(255)
                .HasColumnName("id_card");
            entity.Property(e => e.IdPernamentResidence).HasColumnName("id_pernament_residence");
            entity.Property(e => e.IdRegularAddress).HasColumnName("id_regular_address");
            entity.Property(e => e.InTrash)
                .HasMaxLength(20)
                .HasColumnName("in_trash");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
            entity.Property(e => e.Priority)
                .HasMaxLength(20)
                .HasColumnName("priority");
            entity.Property(e => e.Regilion)
                .HasMaxLength(255)
                .HasColumnName("regilion");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");

            entity.HasOne(d => d.IdPernamentResidenceNavigation).WithMany(p => p.UserIdPernamentResidenceNavigations)
                .HasForeignKey(d => d.IdPernamentResidence)
                .HasConstraintName("FK__USERS__id_pernam__5629CD9C");

            entity.HasOne(d => d.IdRegularAddressNavigation).WithMany(p => p.UserIdRegularAddressNavigations)
                .HasForeignKey(d => d.IdRegularAddress)
                .HasConstraintName("FK__USERS__id_regula__571DF1D5");
        });

        modelBuilder.Entity<Workingprocess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WORKINGP__3213E83F4FCACB98");

            entity.ToTable("WORKINGPROCESS");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Position)
                .HasMaxLength(255)
                .HasColumnName("position");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.WorkingAgency)
                .HasMaxLength(255)
                .HasColumnName("working_agency");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Workingprocesses)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK__WORKINGPR__id_us__5CD6CB2B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
