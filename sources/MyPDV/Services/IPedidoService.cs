using MyPDV.Dtos;

namespace MyPDV.Services;

public interface IPedidoService
{
    Task<IReadOnlyCollection<PedidoResponse>> ListarAsync(
        CancellationToken cancellationToken);

    Task<PedidoResponse?> ObterAsync(
        int id,
        CancellationToken cancellationToken);

    Task<PedidoResponse> CriarAsync(
        CriarPedidoRequest request,
        CancellationToken cancellationToken);
}
