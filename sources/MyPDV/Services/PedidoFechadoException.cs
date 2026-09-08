namespace MyPDV.Services;

public sealed class PedidoFechadoException(int id)
    : Exception($"O pedido {id} está fechado e não pode ser atualizado.");
