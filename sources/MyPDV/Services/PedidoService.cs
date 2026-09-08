using Microsoft.EntityFrameworkCore;
using MyPDV.Database;
using MyPDV.Dtos;
using MyPDV.Schemas.Entities;
using MyPDV.Schemas.Enums;

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

    public async Task<PedidoResponse?> AtualizarAsync(
        int id,
        AtualizarPedidoRequest request,
        CancellationToken cancellationToken)
    {
        Pedido? pedido = await db.Pedidos
            .Include(pedido => pedido.Itens)
            .FirstOrDefaultAsync(
                pedido => pedido.Id == id && pedido.Ativo,
                cancellationToken);

        if (pedido is null)
        {
            return null;
        }

        if (pedido.Status == Status.Fechado)
        {
            throw new PedidoFechadoException(id);
        }

        if (request.Cliente is not null)
        {
            pedido.Cliente = request.Cliente.Trim();
        }

        await db.SaveChangesAsync(cancellationToken);

        return Mapear(pedido);
    }

    public async Task<bool> DesativarAsync(
        int id,
        CancellationToken cancellationToken)
    {
        Pedido? pedido = await db.Pedidos
            .FirstOrDefaultAsync(
                pedido => pedido.Id == id && pedido.Ativo,
                cancellationToken);

        if (pedido is null)
        {
            return false;
        }

        pedido.Ativo = false;
        await db.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<PedidoResponse?> FecharAsync(
        int id,
        CancellationToken cancellationToken)
    {
        Pedido? pedido = await db.Pedidos
            .Include(pedido => pedido.Itens)
            .FirstOrDefaultAsync(
                pedido => pedido.Id == id && pedido.Ativo,
                cancellationToken);

        if (pedido is null)
        {
            return null;
        }

        if (pedido.Itens.Count == 0)
        {
            throw new PedidoSemItensException(id);
        }

        if (pedido.Status == Status.Fechado)
        {
            throw new PedidoFechadoException(id);
        }

        pedido.Total = pedido.Itens.Sum(
            item => item.Quantidade * item.ValorUnitario);

        pedido.Status = Status.Fechado;

        await db.SaveChangesAsync(cancellationToken);

        return Mapear(pedido);
    }

    public async Task<ImportarPedidosResponse> ImportarAsync(
        IReadOnlyCollection<CriarPedidoRequest> requests,
        CancellationToken cancellationToken)
    {
        if (requests.Count == 0)
        {
            throw new ImportacaoInvalidaException("A lista de pedidos não pode ser vazia.");
        }

        List<CriarPedidoRequest> lista = requests.ToList();

        for (int posicao = 0; posicao < lista.Count; posicao++)
        {
            CriarPedidoRequest request = lista[posicao];

            if (string.IsNullOrWhiteSpace(request.Cliente))
            {
                throw new PedidoInvalidoException(posicao, "cliente não informado.");
            }

            if (request.Itens.Count == 0)
            {
                throw new PedidoInvalidoException(posicao, "pedido sem itens.");
            }

            foreach (CriarItemPedidoRequest item in request.Itens)
            {
                if (string.IsNullOrWhiteSpace(item.Produto))
                {
                    throw new PedidoInvalidoException(posicao, "item sem produto.");
                }

                if (item.Quantidade <= 0)
                {
                    throw new PedidoInvalidoException(posicao, "quantidade inválida.");
                }

                if (item.ValorUnitario <= 0)
                {
                    throw new PedidoInvalidoException(posicao, "valor unitário inválido.");
                }
            }
        }

        List<Pedido> pedidos = lista.Select(request => new Pedido
        {
            Cliente = request.Cliente.Trim(),
            Itens = request.Itens.Select(item => new ItemPedido
            {
                Produto = item.Produto.Trim(),
                Quantidade = item.Quantidade,
                ValorUnitario = item.ValorUnitario
            }).ToList()
        }).ToList();

        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

        await db.Pedidos.AddRangeAsync(pedidos, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return new ImportarPedidosResponse
        {
            Quantidade = pedidos.Count,
            Ids = pedidos.Select(pedido => pedido.Id).ToList()
        };
    }

    public async Task<IReadOnlyCollection<ResumoPedidoResponse>> ObterResumoAsync(
        CancellationToken cancellationToken)
    {
        List<ResumoPedidoResponse> resumo = await db.Pedidos
            .Where(pedido => pedido.Ativo)
            .GroupBy(pedido => pedido.Status)
            .Select(grupo => new ResumoPedidoResponse
            {
                Status = grupo.Key,
                Quantidade = grupo.Count(),
                ValorTotal = grupo.Sum(pedido => pedido.Total)
            })
            .OrderBy(item => item.Status)
            .ToListAsync(cancellationToken);

        return resumo;
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
