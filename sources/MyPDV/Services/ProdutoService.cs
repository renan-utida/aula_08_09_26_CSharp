using Microsoft.EntityFrameworkCore;
using MyPDV.Database;
using MyPDV.Dtos;
using MyPDV.Schemas.Entities;

namespace MyPDV.Services;

public sealed class ProdutoService(AppDbContext db) : IProdutoService
{
    public async Task<IReadOnlyCollection<ProdutoResponse>> ListarAsync(
        CancellationToken cancellationToken)
    {
        return await db.Produtos
            .AsNoTracking()
            .Where(produto => produto.Ativo)
            .OrderBy(produto => produto.Nome)
            .Select(produto => Mapear(produto))
            .ToListAsync(cancellationToken);
    }

    public async Task<ProdutoResponse?> ObterAsync(
        int id,
        CancellationToken cancellationToken)
    {
        Produto? produto = await db.Produtos
            .AsNoTracking()
            .FirstOrDefaultAsync(
                item => item.Id == id && item.Ativo,
                cancellationToken);

        return produto is null ? null : Mapear(produto);
    }

    public async Task<ProdutoResponse> CriarAsync(
        CriarProdutoRequest request,
        CancellationToken cancellationToken)
    {
        Produto produto = new()
        {
            Nome = request.Nome.Trim(),
            Preco = request.Preco,
            Estoque = request.Estoque
        };

        db.Produtos.Add(produto);
        await db.SaveChangesAsync(cancellationToken);

        return Mapear(produto);
    }

    private static ProdutoResponse Mapear(Produto produto)
    {
        return new ProdutoResponse
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Preco = produto.Preco,
            Estoque = produto.Estoque
        };
    }
}
