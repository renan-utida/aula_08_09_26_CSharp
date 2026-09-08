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

    Task<PedidoResponse?> AtualizarAsync(
        int id,
        AtualizarPedidoRequest request,
        CancellationToken cancellationToken);

    Task<bool> DesativarAsync(
        int id,
        CancellationToken cancellationToken);

    Task<PedidoResponse?> FecharAsync(
        int id,
        CancellationToken cancellationToken);
}
