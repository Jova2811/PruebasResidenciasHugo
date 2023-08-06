using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PruebasResidenciasHugo.Models;

public partial class AspitantesApiContext : DbContext
{
    public AspitantesApiContext()
    {
    }

    public AspitantesApiContext(DbContextOptions<AspitantesApiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<ProductosHugo> ProductosHugos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("aspiranteApi");

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<ProductosHugo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producto__3214EC0701B2CE67");

            entity.ToTable("ProductosHugo");

            entity.Property(e => e.Precio).HasColumnType("decimal(18, 0)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
