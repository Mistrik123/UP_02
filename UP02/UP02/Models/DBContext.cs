using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UP02.Models;

public partial class DBContext : DbContext
{
    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Adress> Adresses { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<EdIzm> EdIzms { get; set; }

    public virtual DbSet<Postavshik> Postavshiks { get; set; }

    public virtual DbSet<Proizvoditel> Proizvoditels { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SostavZakaza> SostavZakazas { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Street> Streets { get; set; }

    public virtual DbSet<Tovari> Tovaris { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Zakazi> Zakazis { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=up02,1;Username=postgres;Password=postgres;SSL Mode=Disable");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Adress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Adress_pkey");

            entity.ToTable("Adress");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.HouseNumber).HasColumnName("House_Number");
            entity.Property(e => e.StreetId).HasColumnName("Street_ID");

            entity.HasOne(d => d.Street).WithMany(p => p.Adresses)
                .HasForeignKey(d => d.StreetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Street_ID");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Category_pkey");

            entity.ToTable("Category");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasColumnType("character varying");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("City_pkey");

            entity.ToTable("City");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasColumnType("character varying");
        });

        modelBuilder.Entity<EdIzm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Ed_izm_pkey");

            entity.ToTable("Ed_izm");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasColumnType("character varying");
        });

        modelBuilder.Entity<Postavshik>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Postavshik_pkey");

            entity.ToTable("Postavshik");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasColumnType("character varying");
        });

        modelBuilder.Entity<Proizvoditel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Proizvoditel_pkey");

            entity.ToTable("Proizvoditel");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasColumnType("character varying");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Role_pkey");

            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasColumnType("character varying");
        });

        modelBuilder.Entity<SostavZakaza>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SostavZakaza_pkey");

            entity.ToTable("SostavZakaza");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Article).HasColumnType("character varying");
            entity.Property(e => e.ZakazId).HasColumnName("Zakaz_ID");

            entity.HasOne(d => d.ArticleNavigation).WithMany(p => p.SostavZakazas)
                .HasForeignKey(d => d.Article)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tovari");

            entity.HasOne(d => d.Zakaz).WithMany(p => p.SostavZakazas)
                .HasForeignKey(d => d.ZakazId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_zakazi");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Statuses_pkey");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasColumnType("character varying");
        });

        modelBuilder.Entity<Street>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Streets_pkey");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CityId).HasColumnName("City_ID");
            entity.Property(e => e.Name).HasColumnType("character varying");

            entity.HasOne(d => d.City).WithMany(p => p.Streets)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_City_ID");
        });

        modelBuilder.Entity<Tovari>(entity =>
        {
            entity.HasKey(e => e.Article).HasName("Tovari_pkey");

            entity.ToTable("Tovari");

            entity.Property(e => e.Article).HasColumnType("character varying");
            entity.Property(e => e.CategoryId).HasColumnName("Category_ID");
            entity.Property(e => e.Description).HasColumnType("character varying");
            entity.Property(e => e.EdIzmId).HasColumnName("Ed_Izm_ID");
            entity.Property(e => e.Name).HasColumnType("character varying");
            entity.Property(e => e.Photo).HasColumnType("character varying");
            entity.Property(e => e.PostavshikId).HasColumnName("Postavshik_ID");
            entity.Property(e => e.Price).HasPrecision(12, 2);
            entity.Property(e => e.ProizvoditelId).HasColumnName("Proizvoditel_ID");

            entity.HasOne(d => d.Category).WithMany(p => p.Tovaris)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Category");

            entity.HasOne(d => d.EdIzm).WithMany(p => p.Tovaris)
                .HasForeignKey(d => d.EdIzmId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_edIzmID");

            entity.HasOne(d => d.Postavshik).WithMany(p => p.Tovaris)
                .HasForeignKey(d => d.PostavshikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Post");

            entity.HasOne(d => d.Proizvoditel).WithMany(p => p.Tovaris)
                .HasForeignKey(d => d.ProizvoditelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Proizv");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_pkey");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Famil).HasColumnType("character varying");
            entity.Property(e => e.Login).HasColumnType("character varying");
            entity.Property(e => e.Name).HasColumnType("character varying");
            entity.Property(e => e.Otchestvo).HasColumnType("character varying");
            entity.Property(e => e.Password).HasColumnType("character varying");
            entity.Property(e => e.RoleId).HasColumnName("Role_ID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Role");
        });

        modelBuilder.Entity<Zakazi>(entity =>
        {
            entity.HasKey(e => e.IdZakaz).HasName("Zakazi_pkey");

            entity.ToTable("Zakazi");

            entity.Property(e => e.IdZakaz).HasColumnName("ID_Zakaz");
            entity.Property(e => e.AdressVidahi).HasColumnName("Adress vidahi");
            entity.Property(e => e.ClientId).HasColumnName("Client_ID");
            entity.Property(e => e.CodeReceipt)
                .HasMaxLength(3)
                .HasColumnName("Code receipt ");
            entity.Property(e => e.DateDostavka).HasColumnName("Date_dostavka");
            entity.Property(e => e.DateZakaza).HasColumnName("Date_Zakaza");
            entity.Property(e => e.StatusId).HasColumnName("Status_ID");

            entity.HasOne(d => d.Client).WithMany(p => p.Zakazis)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_client");

            entity.HasOne(d => d.Status).WithMany(p => p.Zakazis)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
