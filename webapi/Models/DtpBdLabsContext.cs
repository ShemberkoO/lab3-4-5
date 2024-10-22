using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace webapi.Models;

public partial class DtpBdLabsContext : DbContext
{
    public DtpBdLabsContext()
    {
    }

    public DtpBdLabsContext(DbContextOptions<DtpBdLabsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Accident> Accidents { get; set; }

    public virtual DbSet<OccupantTransport> OccupantTransports { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Transport> Transports { get; set; }

    public virtual DbSet<Victim> Victims { get; set; }

    public virtual DbSet<Violation> Violations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=dtp_bd_labs;Username=postgres;Password=45685293");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum("transport_type", new[] { "car", "truck", "motorcycle", "scooter", "pedestrian" });

        modelBuilder.Entity<Accident>(entity =>
        {
            entity.HasKey(e => e.AccidentId).HasName("accidents_pkey");

            entity.ToTable("accidents");

            entity.Property(e => e.AccidentId).HasColumnName("accident_id");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("date");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
        });

        modelBuilder.Entity<OccupantTransport>(entity =>
        {
            entity.HasKey(e => e.VictimId).HasName("occupant_transport_pkey");

            entity.ToTable("occupant_transport");

            entity.Property(e => e.VictimId)
                .ValueGeneratedNever()
                .HasColumnName("victim_id");
            entity.Property(e => e.DriverLicenseNumber)
                .HasMaxLength(50)
                .HasColumnName("driver_license_number");
            entity.Property(e => e.TransportId).HasColumnName("transport_id");

            entity.HasOne(d => d.Transport).WithMany(p => p.OccupantTransports)
                .HasForeignKey(d => d.TransportId)
                .HasConstraintName("occupant_transport_transport_id_fkey");

            entity.HasOne(d => d.Victim).WithOne(p => p.OccupantTransport)
                .HasForeignKey<OccupantTransport>(d => d.VictimId)
                .HasConstraintName("occupant_transport_victim_id_fkey");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PasportId).HasName("person_pkey");

            entity.ToTable("person");

            entity.Property(e => e.PasportId)
                .HasMaxLength(50)
                .HasColumnName("pasport_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(100)
                .HasColumnName("patronymic");
            entity.Property(e => e.RegistrationAddress)
                .HasMaxLength(255)
                .HasColumnName("registration_address");
        });

        modelBuilder.Entity<Transport>(entity =>
        {
            entity.HasKey(e => e.TransportId).HasName("transport_pkey");

            entity.ToTable("transport");

            entity.Property(e => e.TransportId).HasColumnName("transport_id");
            entity.Property(e => e.Brand)
                .HasMaxLength(100)
                .HasColumnName("brand");
            entity.Property(e => e.Model)
                .HasMaxLength(100)
                .HasColumnName("model");
            entity.Property(e => e.OwnerId)
                .HasMaxLength(50)
                .HasColumnName("owner_id");
            entity.Property(e => e.RegistrationNumber)
                .HasMaxLength(50)
                .HasColumnName("registration_number");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
            entity.Property(e => e.YearOfManufacture).HasColumnName("year_of_manufacture");

            entity.HasOne(d => d.Owner).WithMany(p => p.Transports)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("transport_owner_id_fkey");
        });

        modelBuilder.Entity<Victim>(entity =>
        {
            entity.HasKey(e => e.VictimId).HasName("victims_pkey");

            entity.ToTable("victims");

            entity.Property(e => e.VictimId).HasColumnName("victim_id");
            entity.Property(e => e.AccidentId).HasColumnName("accident_id");
            entity.Property(e => e.PasportId)
                .HasMaxLength(50)
                .HasColumnName("pasport_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'неушкоджений'::character varying")
                .HasColumnName("status");

            entity.HasOne(d => d.Accident).WithMany(p => p.Victims)
                .HasForeignKey(d => d.AccidentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("victims_accident_id_fkey");

            entity.HasOne(d => d.Pasport).WithMany(p => p.Victims)
                .HasForeignKey(d => d.PasportId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("victims_pasport_id_fkey");
        });

        modelBuilder.Entity<Violation>(entity =>
        {
            entity.HasKey(e => e.ViolationId).HasName("violations_pkey");

            entity.ToTable("violations");

            entity.Property(e => e.ViolationId).HasColumnName("violation_id");
            entity.Property(e => e.Article)
                .HasMaxLength(255)
                .HasColumnName("article");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.VictimId).HasColumnName("victim_id");

            entity.HasOne(d => d.Victim).WithMany(p => p.Violations)
                .HasForeignKey(d => d.VictimId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("violations_victim_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
