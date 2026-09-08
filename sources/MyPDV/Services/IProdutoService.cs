using MyPDV.Dtos;

namespace MyPDV.Services;

public interface IProdutoService
{
    Task<IReadOnlyCollection<ProdutoResponse>> ListarAsync(
        CancellationToken cancellationToken);

    Task<ProdutoResponse?> ObterAsync(
        int id,
        CancellationToken cancellationToken);

    Task<ProdutoResponse> CriarAsync(
        CriarProdutoRequest request,
        CancellationToken cancellationToken);
}
