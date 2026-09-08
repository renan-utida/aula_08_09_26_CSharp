using Microsoft.EntityFrameworkCore;
using MyPDV.Database;
using MyPDV.Dtos;
using MyPDV.Schemas.Entities;

namespace MyPDV.Services;

public sealed class PedidoService(AppDbContext db) : IPedidoService
{
    public async Task<IReadOnlyCollection<PedidoResponse>> ListarAsync(
        CancellationToken cancellationToken)
    {
        List<Pedido> pedidos = await db.Pedidos
            .AsNoTracking()
            .Include(pedido => pedido.Itens)
            .Where(pedido => pedido.Ativo)
            .OrderBy(pedido => pedido.Id)
            .ToListAsync(cancellationToken);

        return pedidos.Select(Mapear).ToList();
    }

    public async Task<PedidoResponse?> ObterAsync(
        int id,
        CancellationToken cancellationToken)
    {
        Pedido? pedido = await db.Pedidos
            .AsNoTracking()
            .Include(item => item.Itens)
            .FirstOrDefaultAsync(
                item => item.Id == id && item.Ativo,
                cancellationToken);

        return pedido is null ? null : Mapear(pedido);
    }

    public async Task<PedidoResponse> CriarAsync(
        CriarPedidoRequest request,
        CancellationToken cancellationToken)
    {
        Pedido pedido = new()
        {
            Cliente = request.Cliente.Trim(),
            Itens = request.Itens.Select(item => new ItemPedido
            {
                Produto = item.Produto.Trim(),
                Quantidade = item.Quantidade,
                ValorUnitario = item.ValorUnitario
            }).ToList()
        };

        db.Pedidos.Add(pedido);
        await db.SaveChangesAsync(cancellationToken);

        return Mapear(pedido);
    }

    private static PedidoResponse Mapear(Pedido pedido)
    {
        return new PedidoResponse
        {
            Id = pedido.Id,
            Cliente = pedido.Cliente,
            Status = pedido.Status,
            Total = pedido.Total,
            CriadoEm = pedido.CriadoEm,
            Itens = pedido.Itens.Select(item => new ItemPedidoResponse
            {
                Id = item.Id,
                Produto = item.Produto,
                Quantidade = item.Quantidade,
                ValorUnitario = item.ValorUnitario
            }).ToList()
        };
    }
}
