using Microsoft.EntityFrameworkCore;
using MyPDV.Schemas.Entities;

namespace MyPDV.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<ItemPedido> ItemPedido { get; set; }

    public DbSet<Produto> Produtos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Pedido>()
            .HasMany(pedido => pedido.Itens)
            .WithOne(item => item.Pedido)
            .HasForeignKey(item => item.PedidoId);
    }
}
