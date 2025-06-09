using System;
using System.Collections.Generic;
using ExamPortal.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamPortal.DataAccess.DataContext;

public partial class ExamPortalContext : DbContext
{
    public ExamPortalContext()
    {
    }

    public ExamPortalContext(DbContextOptions<ExamPortalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<ExamSchedule> ExamSchedules { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionOption> QuestionOptions { get; set; }
    public DbSet<ExamAttempt> ExamAttempts { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:my_connection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.RoleId).HasDefaultValueSql("2");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_role");
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
        });

        modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            }
        );
        modelBuilder.Entity<Answer>()
           .HasOne(a => a.SelectedOption)
           .WithMany()
           .HasForeignKey(a => a.SelectedOptionId)
           .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<QuestionOption>()
            .HasOne(qo => qo.Question)
            .WithMany(q => q.Options)
            .HasForeignKey(qo => qo.QuestionId);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
