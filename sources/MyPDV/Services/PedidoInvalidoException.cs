namespace MyPDV.Services;

public sealed class PedidoInvalidoException(int posicao, string motivo)
    : Exception($"Pedido na posição {posicao} é inválido: {motivo}");
