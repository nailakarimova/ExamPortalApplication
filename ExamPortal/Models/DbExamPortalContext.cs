using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ExamPortal.Models;

public partial class DbExamPortalContext : DbContext
{
    public DbExamPortalContext()
    {
    }

    public DbExamPortalContext(DbContextOptions<DbExamPortalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TExam> TExams { get; set; }

    public virtual DbSet<TStudent> TStudents { get; set; }

    public virtual DbSet<TSubject> TSubjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TExam>(entity =>
        {
            entity.HasKey(e => e.EId);

            entity.ToTable("T_EXAM");

            entity.Property(e => e.EId).HasColumnName("E_ID");
            entity.Property(e => e.EDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date")
                .HasColumnName("E_DATE");
            entity.Property(e => e.EGrade)
                .HasColumnType("numeric(1, 0)")
                .HasColumnName("E_GRADE");
            entity.Property(e => e.ESCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("subject code from t_subject")
                .HasColumnName("E_S_CODE");
            entity.Property(e => e.ESNumber)
                .HasComment("student number from t_student")
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("E_S_NUMBER");
            entity.Property(e => e.EStatus)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasComment("is exam active (1) or deleted (0)")
                .HasColumnName("E_STATUS");

            entity.HasOne(d => d.ESCodeNavigation).WithMany(p => p.TExams)
                .HasPrincipalKey(p => p.SCode)
                .HasForeignKey(d => d.ESCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T_EXAM_T_SUBJECT");

            entity.HasOne(d => d.ESNumberNavigation).WithMany(p => p.TExams)
                .HasPrincipalKey(p => p.SNumber)
                .HasForeignKey(d => d.ESNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T_EXAM_T_STUDENT");
        });

        modelBuilder.Entity<TStudent>(entity =>
        {
            entity.HasKey(e => e.SId);

            entity.ToTable("T_STUDENT");

            entity.HasIndex(e => e.SNumber, "UQ_S_NUMBER").IsUnique();

            entity.Property(e => e.SId).HasColumnName("S_ID");
            entity.Property(e => e.SClass)
                .HasColumnType("numeric(2, 0)")
                .HasColumnName("S_CLASS");
            entity.Property(e => e.SName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('')")
                .HasColumnName("S_NAME");
            entity.Property(e => e.SNumber)
                .HasColumnType("numeric(5, 0)")
                .HasColumnName("S_NUMBER");
            entity.Property(e => e.SStatus)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasComment("is student active (1) or deleted (0)")
                .HasColumnName("S_STATUS");
            entity.Property(e => e.SSurname)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('')")
                .HasColumnName("S_SURNAME");
        });

        modelBuilder.Entity<TSubject>(entity =>
        {
            entity.HasKey(e => e.SId);

            entity.ToTable("T_SUBJECT");

            entity.HasIndex(e => e.SCode, "UQ_S_CODE").IsUnique();

            entity.Property(e => e.SId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(4, 0)")
                .HasColumnName("S_ID");
            entity.Property(e => e.SClass)
                .HasColumnType("numeric(2, 0)")
                .HasColumnName("S_CLASS");
            entity.Property(e => e.SCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValueSql("('')")
                .IsFixedLength()
                .HasComment("unique constraint added for distinction of the same subject for different classes")
                .HasColumnName("S_CODE");
            entity.Property(e => e.SStatus)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasComment("is subject active (1) or deleted (0) ")
                .HasColumnName("S_STATUS");
            entity.Property(e => e.STName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("('')")
                .HasComment("teacher name")
                .HasColumnName("S_T_NAME");
            entity.Property(e => e.STSurname)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("('')")
                .HasComment("teacher surname")
                .HasColumnName("S_T_SURNAME");
            entity.Property(e => e.STitle)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('')")
                .HasColumnName("S_TITLE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
